using System;
using MySqlConnector;

namespace TheAmazingExcavatorRentTool.Services
{
    public class DB
    {
        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public MySqlConnection Connection { get; set;}

        private static DB _instance = null;
        public static DB Instance()
        {
            if (_instance == null)
                _instance = new DB();
            return _instance;
        }
    
        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty("client"))
                    return false;
                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
                Connection = new MySqlConnection(connstring);
                Connection.Open();
            }
    
            return true;
        }
    
        public void Close()
        {
            Connection.Close();
        }
        
        // private static DB getDbCon()
        // {
        //     var dbCon = new DB();
        //     dbCon.Server = "localhost";
        //     dbCon.DatabaseName = "bd_c#";
        //     dbCon.UserName = "root";
        //     dbCon.Password = "";
        //
        //     return dbCon;
        // }
    }
}