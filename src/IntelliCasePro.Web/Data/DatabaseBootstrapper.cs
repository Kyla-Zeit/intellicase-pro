using System.Data;
using Microsoft.EntityFrameworkCore;

namespace IntelliCasePro.Web.Data;

public static class DatabaseBootstrapper
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();
        await EnsureInvestigatorAuthColumnsAsync(db);
        await EnsureInvestigatorIndexesAsync(db);
        await SeedData.InitializeAsync(db);
    }

    private static async Task EnsureInvestigatorAuthColumnsAsync(AppDbContext db)
    {
        var columns = await GetColumnNamesAsync(db, "Investigators");

        if (!columns.Contains("PasswordHash"))
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE \"Investigators\" ADD COLUMN \"PasswordHash\" TEXT NULL;");
        }

        if (!columns.Contains("PasswordSalt"))
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE \"Investigators\" ADD COLUMN \"PasswordSalt\" TEXT NULL;");
        }

        if (!columns.Contains("LastLoginAt"))
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE \"Investigators\" ADD COLUMN \"LastLoginAt\" TEXT NULL;");
        }

        if (!columns.Contains("IsActive"))
        {
            await db.Database.ExecuteSqlRawAsync("ALTER TABLE \"Investigators\" ADD COLUMN \"IsActive\" INTEGER NOT NULL DEFAULT 1;");
        }
    }

    private static async Task EnsureInvestigatorIndexesAsync(AppDbContext db)
    {
        await db.Database.ExecuteSqlRawAsync("CREATE UNIQUE INDEX IF NOT EXISTS \"IX_Investigators_Email\" ON \"Investigators\" (\"Email\");");
    }

    private static async Task<HashSet<string>> GetColumnNamesAsync(AppDbContext db, string tableName)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var connection = db.Database.GetDbConnection();
        var closeAfter = connection.State != ConnectionState.Open;

        if (closeAfter)
        {
            await connection.OpenAsync();
        }

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = $"PRAGMA table_info('{tableName.Replace("'", "''")}');";

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(reader.GetString(1));
            }
        }
        finally
        {
            if (closeAfter)
            {
                await connection.CloseAsync();
            }
        }

        return result;
    }
}
