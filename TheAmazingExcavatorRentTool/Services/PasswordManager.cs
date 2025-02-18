using System.Text;
using System.Security.Cryptography;
namespace TheAmazingExcavatorRentTool.Services;
using System.Linq;

public class PasswordManager
{
    private const string AlphanumericChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const string SpecialChars = "!@#$%^&*()-_=+[]{};:,.<>?";

    internal string HashPassword(string password)
    {
        System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
        byte[] hash = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
        return Convert.ToBase64String(hash);
    }
    
    public string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
    {
        
        if (length < numberOfNonAlphanumericCharacters)
            throw new ArgumentException("Password length is lower than minimum characters given");

        var rng = new RNGCryptoServiceProvider();
        char[] password = new char[length];

        // Générer les caractères alphanumériques
        for (int i = 0; i < length - numberOfNonAlphanumericCharacters; i++)
            password[i] = AlphanumericChars[GetRandomNumber(rng, AlphanumericChars.Length)];

        // Générer les caractères spéciaux
        for (int i = length - numberOfNonAlphanumericCharacters; i < length; i++)
            password[i] = SpecialChars[GetRandomNumber(rng, SpecialChars.Length)];

        // Mélanger le mot de passe
        return new string(password.OrderBy(_ => GetRandomNumber(rng, length)).ToArray());
    }
    
    private static int GetRandomNumber(RNGCryptoServiceProvider rng, int max)
    {
        byte[] randomNumber = new byte[4];
        rng.GetBytes(randomNumber);
        return (int)(BitConverter.ToUInt32(randomNumber, 0) % max);
    }
}