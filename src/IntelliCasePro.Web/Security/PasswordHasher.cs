using System.Security.Cryptography;

namespace IntelliCasePro.Web.Security;

public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;

    public static (string Hash, string Salt) HashPassword(string password)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(SaltSize);
        var hashBytes = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize);

        return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
    }

    public static bool VerifyPassword(string password, string? hash, string? salt)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash) || string.IsNullOrWhiteSpace(salt))
        {
            return false;
        }

        byte[] saltBytes;
        byte[] expectedHashBytes;

        try
        {
            saltBytes = Convert.FromBase64String(salt);
            expectedHashBytes = Convert.FromBase64String(hash);
        }
        catch (FormatException)
        {
            return false;
        }

        var actualHashBytes = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            Iterations,
            HashAlgorithmName.SHA256,
            expectedHashBytes.Length);

        return CryptographicOperations.FixedTimeEquals(actualHashBytes, expectedHashBytes);
    }
}
