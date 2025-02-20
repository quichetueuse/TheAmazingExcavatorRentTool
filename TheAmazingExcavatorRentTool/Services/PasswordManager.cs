using System.Text;
using System.Security.Cryptography;
namespace TheAmazingExcavatorRentTool.Services;
using System.Linq;

public class PasswordManager
{
    
    private const string UpperChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string LowerChars = "abcdefghijklmnopqrstuvwxyz";
    private const string Numbers = "0123456789";
    private const string SpecialChars = "!@#$%^&*()-_=+[]{};:,.<>?";
    // private const string AlphanumericChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    // private const string SpecialChars = "!@#$%^&*()-_=+[]{};:,.<>?";

    internal string HashPassword(string password)
    {
        System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
        byte[] hash = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
        return Convert.ToBase64String(hash);
    }

    // internal string GeneratePassword(int length, int min_lower, int min_upper, int min_num, int min_special)
    // {
    //     if (min_lower + min_upper + min_num + min_special > length)
    //         throw new ArgumentException("Password length is lower than minimum characters given");
    //
    //     int count_upper = 0;
    //     int count_lower = 0;
    //     int count_number = 0;
    //     int count_special = 0;
    //
    //     string password = "";
    //
    //     while (password.Length < length)
    //     {
    //         if (count_lower < min_lower)
    //         {
    //             
    //             password += "";
    //         } else if (count_upper < min_upper) {
    //             
    //         } else if (count_number < min_num) {
    //             
    //         }else if (count_special < min_special) {
    //             
    //         }
    //     }
    //     
    //     return "";
    // }
    
    public string GeneratePassword(int length, int num_lower, int num_upper, int num_number, int num_special)
    {
        
        if (num_lower + num_upper + num_number + num_special > length)
            throw new ArgumentException("Password length is lower than minimum characters given");
    
        var rng = new RNGCryptoServiceProvider();
        char[] password = new char[length];

        int count_password = 0;
        
        // Generate lower chars
        for (int i = 0; i < num_lower; i++)
        {
            password[count_password] = LowerChars[GetRandomNumber(rng, LowerChars.Length)];
            count_password++;
        }

        // Generate upper chars
        for (int i = 0; i < num_upper; i++)
        {
            password[count_password] = UpperChars[GetRandomNumber(rng, UpperChars.Length)];
            count_password++;
        }
        // Generate numbers
        for (int i = 0; i < num_number; i++)
        {
            password[count_password] = Numbers[GetRandomNumber(rng, Numbers.Length)];
            count_password++;
        }
        // Generate numbers
        for (int i = 0; i < num_special; i++)
        {
            password[count_password] = SpecialChars[GetRandomNumber(rng, SpecialChars.Length)];
            count_password++;
        }
        // Shuffle the password
        return new string(password.OrderBy(_ => GetRandomNumber(rng, length)).ToArray());
    }
    
    // public string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
    // {
    //     
    //     if (length < numberOfNonAlphanumericCharacters)
    //         throw new ArgumentException("Password length is lower than minimum characters given");
    //
    //     var rng = new RNGCryptoServiceProvider();
    //     char[] password = new char[length];
    //
    //     // Generate alpha chars
    //     for (int i = 0; i < length - numberOfNonAlphanumericCharacters; i++)
    //         password[i] = AlphanumericChars[GetRandomNumber(rng, AlphanumericChars.Length)];
    //
    //     // Generate special chars
    //     for (int i = length - numberOfNonAlphanumericCharacters; i < length; i++)
    //         password[i] = SpecialChars[GetRandomNumber(rng, SpecialChars.Length)];
    //
    //     // Mélanger le mot de passe
    //     return new string(password.OrderBy(_ => GetRandomNumber(rng, length)).ToArray());
    // }
    
    private static int GetRandomNumber(RNGCryptoServiceProvider rng, int max)
    {
        byte[] randomNumber = new byte[4];
        rng.GetBytes(randomNumber);
        return (int)(BitConverter.ToUInt32(randomNumber, 0) % max);
    }
}