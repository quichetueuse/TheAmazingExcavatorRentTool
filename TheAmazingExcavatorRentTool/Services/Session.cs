using System;

namespace TheAmazingExcavatorRentTool.Services
{
    public class Session
    {
        private static Int32? _id;
        private static string? _username;
        private static string? _password;
        private static bool? _isAdmin;
        public static Int32? Id
        {
            get 
            { 
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        public static string? Username { get; set; }
        public static string? Password { get; set; }
        public static bool IsAdmin { get; set; }


        public static void ShowSessionInfos()
        {
            Console.WriteLine("session user id: "+ Id);
            Console.WriteLine("session username: "+ Username);
            Console.WriteLine("session user password: "+ Password);
            Console.WriteLine("session user admin: "+ IsAdmin);
        }

        public static void SessionClear()
        {
            Id = Convert.ToInt32(null);
            Username = null;
            Password = null;
            IsAdmin = Convert.ToBoolean(null);
        }
    }
}