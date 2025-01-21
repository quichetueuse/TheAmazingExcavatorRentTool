namespace TheAmazingExcavatorRentTool.Models
{
    public class User
    {
        internal int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public User(int userid, string username, string password, bool isAdmin)
        {
            UserId = userid;
            Username = username;
            Password = password;
            IsAdmin = isAdmin;
        }
        
    }
}