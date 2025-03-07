using System;
using System.Net;
using System.Security;
using System.Text;
using System.Windows;
using TheAmazingExcavatorRentTool.Services;
using TheAmazingExcavatorRentTool.Views;
using MySqlConnector;
using Prism.Commands;
using TheAmazingExcavatorRentTool.Exceptions;

namespace TheAmazingExcavatorRentTool.ViewModels
{
    public class LoginVM: BaseVM
    {

        private PasswordManager passwordManager;
        
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


        public LoginVM()
        {
            passwordManager = new PasswordManager();
        }

        
        private DelegateCommand<Window> _loginCommand;
        public DelegateCommand<Window> LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand<Window>(Login));


        private void Login(Window current_window)
        {
            // Console.WriteLine("username: " + Username);
            // Console.WriteLine("password: " +Password);
            DB dbCon = getDbCon();
            string query = "SELECT user_id, username, password, is_admin FROM _user WHERE username=@username AND password=@password";
        
        
            if (!dbCon.IsConnect()) {
                MessageBox.Show("La connexion à la base de données à échouée", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new ConnectionFailedException("Connection to database failed");
            }
            

            NetworkCredential credential = new NetworkCredential(Username, Password);
            
            // Console.WriteLine(password);
            var cmd = new MySqlCommand(query, dbCon.Connection);
            cmd.Parameters.AddWithValue("@username", credential.UserName);
            cmd.Parameters.AddWithValue("@password", passwordManager.HashPassword(credential.Password));
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
            MainWindowNextGen mainWindow = new MainWindowNextGen();
            mainWindow.Show();
            Application.Current.MainWindow = mainWindow;
            current_windows.Close();
            mainWindow.WindowState = WindowState.Normal;
            mainWindow.Activate();
        }
        
    }
}