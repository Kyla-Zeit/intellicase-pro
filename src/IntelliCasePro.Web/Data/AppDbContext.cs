using IntelliCasePro.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace IntelliCasePro.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<InvestigatorUser> Investigators => Set<InvestigatorUser>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<CaseFile> Cases => Set<CaseFile>();
    public DbSet<CaseTask> Tasks => Set<CaseTask>();
    public DbSet<EvidenceItem> EvidenceItems => Set<EvidenceItem>();
    public DbSet<ChainOfCustodyEntry> ChainOfCustodyEntries => Set<ChainOfCustodyEntry>();
    public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();
    public DbSet<ExpenseEntry> ExpenseEntries => Set<ExpenseEntry>();
    public DbSet<CaseNote> Notes => Set<CaseNote>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<CalendarEvent> CalendarEvents => Set<CalendarEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>()
            .Property(x => x.Name)
            .HasMaxLength(120);

        modelBuilder.Entity<CaseFile>()
            .Property(x => x.CaseNumber)
            .HasMaxLength(50);

        modelBuilder.Entity<EvidenceItem>()
            .Property(x => x.ReferenceNumber)
            .HasMaxLength(50);

        modelBuilder.Entity<Invoice>()
            .Property(x => x.InvoiceNumber)
            .HasMaxLength(50);

        modelBuilder.Entity<CaseFile>()
            .HasOne(x => x.Client)
            .WithMany(x => x.Cases)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CaseFile>()
            .HasOne(x => x.LeadInvestigator)
            .WithMany(x => x.Cases)
            .HasForeignKey(x => x.LeadInvestigatorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
