using System.Text;

namespace TheAmazingExcavatorRentTool.Services;

public class PasswordManager
{
    public PasswordManager()
    {
        int password_length = 14;
        int min_num = 2;
        int min_lower = 2;
        int min_upper = 2;
        int min_special = 2;
    }


    internal string HashPassword(string password)
    {
        System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
        byte[] hash = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
        return Convert.ToBase64String(hash);
    }
    
    public string GeneratePassword()
    {
        return "";
    }
}