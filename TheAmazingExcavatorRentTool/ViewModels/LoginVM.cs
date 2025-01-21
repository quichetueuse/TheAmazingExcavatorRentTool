using System;
using System.Net;
using System.Security;
using System.Text;
using System.Windows;
using TheAmazingExcavatorRentTool.Services;
using TheAmazingExcavatorRentTool.Views;
using MySqlConnector;
using Prism.Commands;

namespace TheAmazingExcavatorRentTool.ViewModels
{
    public class LoginVM: BaseVM
    {
        
        private string _username;

        public string Username
        {
            get
            {
                return _username;
            }
            set { _username = value; }
        }

        private SecureString _password;
        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
            }
        }
        
        
        private string _errormsg;

        public string ErrorMsg
        {
            get { return _errormsg; }
            set { _errormsg = value; }
        }

        
        private DelegateCommand<Window> _loginCommand;
        public DelegateCommand<Window> LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand<Window>(Login));


        private void Login(Window current_window)
        {
            // Console.WriteLine("username: " + Username);
            // Console.WriteLine("password: " +Password);
            DB dbCon = getDbCon();
            string query = "SELECT * FROM _user WHERE username=@username AND password=@password";
        
        
            if (!dbCon.IsConnect()) {
                MessageBox.Show("La connexion à la base de données à échouée", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(0);
            }
            

            NetworkCredential credential = new NetworkCredential(Username, Password);
            
            // Console.WriteLine(password);
            var cmd = new MySqlCommand(query, dbCon.Connection);
            cmd.Parameters.AddWithValue("@username", credential.UserName);
            cmd.Parameters.AddWithValue("@password", hashPasswordGenerator(credential.Password));
            // var reader = cmd.ExecuteReader();
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count == 0)
            {
                // ErrorMsg = "Mot de passe ou nom d'utilisateur incorrect";
                MessageBox.Show("Mot de passe ou nom d'utilisateur invalide", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Int32 userId = reader.GetInt32(0);
                Session.Id = userId;
                string username = reader.GetString(1);
                Session.Username = username;
                string password = reader.GetString(2);
                Session.Password = password;
                bool isAdmin = Convert.ToBoolean(reader.GetInt32(3));
                Session.IsAdmin = isAdmin;
            }
            dbCon.Close();
            // Session.ShowSessionInfos();
            OpenMainWindow(current_window);
        }
        
        
        private void OpenMainWindow(Window current_windows)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.MainWindow = mainWindow;
            current_windows.Close();
            mainWindow.WindowState = WindowState.Normal;
            mainWindow.Activate();
        }
        
        private static string hashPasswordGenerator(string password)
        {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            return Convert.ToBase64String(hash);
        }
        
        private DB getDbCon()
        {
            var dbCon = new DB();
            dbCon.Server = "localhost";
            dbCon.DatabaseName = "heavy_app_e5";
            dbCon.UserName = "root";
            dbCon.Password = "";

            return dbCon;
        }
    }
}