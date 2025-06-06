using System.Collections.ObjectModel;
using System.Windows;
using MySqlConnector;
using TheAmazingExcavatorRentTool.Exceptions;
using TheAmazingExcavatorRentTool.Models;
using TheAmazingExcavatorRentTool.Services;

namespace TheAmazingExcavatorRentTool.ViewModels;

public class UserVM: BaseVM
{

    public ObservableCollection<User> Users { get; set; }
    
    private SoundPlayer soundPlayer;

    private PasswordManager passwordManager;

    private readonly string loadQuery;
    private readonly string addQuery;
    private readonly string updateQuery;
    private readonly string updateQueryWithoutPassword;
    private readonly string deleteQuery;

    public UserVM()
    {
        // Creating queries string
        loadQuery = "SELECT user_id, username, password, is_admin FROM _user";
        updateQuery =
            "UPDATE _user SET username=@username, password=@password, is_admin=@is_admin WHERE user_id=@id";
        updateQueryWithoutPassword =
            "UPDATE _user SET username=@username, is_admin=@is_admin WHERE user_id=@id";
        deleteQuery ="DELETE FROM _user WHERE user_id=@id";
        addQuery = "INSERT INTO _user (username, password, is_admin) VALUES (@username, @password, @is_admin)";
        
        // Loading sound player and password manager
        soundPlayer = new SoundPlayer();
        passwordManager = new PasswordManager();
        
        // loading data
        Load();
    }

    private string _Username;

    public string Username
    {
        get { return _Username; }
        set
        {
            _Username = value;
            OnPropertyChanged();
        }
    }
    
    private string _Password;

    public string Password
    {
        get { return _Password; }
        set
        {
            _Password = value;
            OnPropertyChanged();
        }
    }
    
    private bool _IsAdmin;

    public bool IsAdmin
    {
        get { return _IsAdmin; }
        set
        {
            _IsAdmin = value;
            OnPropertyChanged();
        }
    }
    
    
    // Command that get called from view
    private DelegateCommand<User> _deleteCommand;
    public DelegateCommand<User> DeleteCommand =>
        _deleteCommand ?? (_deleteCommand = new DelegateCommand<User>(Delete));
    
    private DelegateCommand _addCommand;
    public DelegateCommand AddCommand =>
        _addCommand ?? (_addCommand = new DelegateCommand(Add));





    private void Load()
    {
        ObservableCollection<User> users = new ObservableCollection<User>();

        var dbCon = getDbCon();

        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new ConnectionFailedException("Connection to database failed");
        }
        
        var cmd = new MySqlCommand(loadQuery, dbCon.Connection);

        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {

            Int32 UserId = reader.GetInt32(0);
            string Username = reader.GetString(1);
            string Password = reader.GetString(2);
            bool is_admin = reader.GetBoolean(3);
            
            
            users.Add(new User(userid: UserId, username: Username, password: Password, isAdmin: is_admin));
        }

        dbCon.Close();

        Users = users;
    }

    public void Update(User user_to_update, bool update_password)
    {
        // Check if you're trying to update current connected user
        if (user_to_update.UserId == Session.Id)
        {
            soundPlayer.PlayFailSound();
            MessageBox.Show("Vous ne pouvez pas modifer l'utilisateur avec lequel vous êtes connecté!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        foreach (User user in Users.ToList())
        {
            // Check if a user already exists with the same username
            if (user.Username == user_to_update.Username && user.UserId != user_to_update.UserId)
            {
                soundPlayer.PlayFailSound();
                MessageBox.Show("Un utilisateur de même nom avec le même nom éxiste déjà!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string hashedOldPassword = passwordManager.HashPassword(user_to_update.Password);
            if (hashedOldPassword == user.Password)
            {
                soundPlayer.PlayFailSound();
                MessageBox.Show("Le mot de passe est le même que le précédent!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        // Updating user in database
        var dbCon = getDbCon();
        
        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new ConnectionFailedException("Connection to database failed");
        }
        
        // Updating user in database
        MySqlCommand cmd;
        if (update_password)
            cmd = new MySqlCommand(updateQuery, dbCon.Connection);
        else
            cmd = new MySqlCommand(updateQueryWithoutPassword, dbCon.Connection);
        cmd.Parameters.AddWithValue("@username", user_to_update.Username);
        if (update_password)
            cmd.Parameters.AddWithValue("@password", passwordManager.HashPassword(user_to_update.Password));
        cmd.Parameters.AddWithValue("@is_admin", user_to_update.IsAdmin);
        cmd.Parameters.AddWithValue("@id", user_to_update.UserId);
        MySqlDataReader result = cmd.ExecuteReader();
        dbCon.Close();
        if (result.RecordsAffected != 1)
        {
            soundPlayer.PlayFailSound();
            MessageBox.Show("Erreur durant l'éxécution de la requête!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        // Updating user in app
        for (int i = 0; i < Users.ToList().Count; i++)
        {
            if (Users[i].UserId == user_to_update.UserId)
            {
                Users[i] = user_to_update;
                break;
            }
        }
        
        // Notify the user that the update succeeded
        soundPlayer.PlaySuccessSound();
        MessageBox.Show("Modifications appliquées à l'utilisaeur", "Modifications Appliquées", MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void Delete(User user_to_delete)
    {
        // Check if user to delete is the one connected
        if (Session.Id == user_to_delete.UserId)
        {
            soundPlayer.PlayFailSound();
            MessageBox.Show("Vous ne pouvez pas supprimer l'utilisateur avec lequel vous êtes connecté!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        var Result = MessageBox.Show($"Voulez-vous vraiment supprimer l'utilisateur sélectionné '{user_to_delete.Username}'?", "Suppression ?",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        
        var dbCon = getDbCon();
        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new ConnectionFailedException("Connection to database failed");
        }
        
        // Deleting user from database
        var cmd = new MySqlCommand(deleteQuery, dbCon.Connection);
        cmd.Parameters.AddWithValue("@id", user_to_delete.UserId);
        MySqlDataReader result = cmd.ExecuteReader();
        dbCon.Close();
        if (result.RecordsAffected != 1)
        {
            soundPlayer.PlayFailSound();
            MessageBox.Show("Erreur durant l'éxécution de la requête!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        // Deleting user from app
        foreach (User varUser in Users.ToList())
        {
            if (varUser.UserId != user_to_delete.UserId)
                continue;
            
            // Notify the user that the removal succeeded
            Users.Remove(varUser);
            soundPlayer.PlaySuccessSound();
            MessageBox.Show("Suppression de l'utilisateur effectuée", "suppression effectuée", MessageBoxButton.OK,
                MessageBoxImage.Information);
            break;
            
        }
    }

    private void Add()
    {
        var Result = MessageBox.Show("Voulez-vous vraiment ajouter un utilisateur ?", "Ajout ?", MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        
        var dbCon = getDbCon();
        
        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new ConnectionFailedException("Connection to database failed");
        }
        
        // Check if user with the same name exists
        string username = _Username;
        string password = passwordManager.HashPassword(_Password);
        bool is_admin = _IsAdmin;

        foreach (User user in Users.ToList())
        {
            if (user.Username == username)
            {
                soundPlayer.PlayFailSound();
                MessageBox.Show("Un utilisateur possède le même nom!", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
        }
        
        // Adding user to database
        var cmd = new MySqlCommand(addQuery, dbCon.Connection);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);
        cmd.Parameters.AddWithValue("@is_admin", is_admin);
        
        MySqlDataReader result = cmd.ExecuteReader();
        if (result.RecordsAffected != 1)
        {
            soundPlayer.PlayFailSound();
            MessageBox.Show("Erreur durant l'éxécution de la requête!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            dbCon.Close();
            return;
        }
        
        // Adding user to app
        int user_id = Convert.ToInt32(cmd.LastInsertedId);
        User user_obj = new User(userid: user_id, username: username, password: password, isAdmin: is_admin);
        Users.Add(user_obj);
        dbCon.Close();
        
        // Notify the user that adding succeeded
        soundPlayer.PlaySuccessSound();
        MessageBox.Show("Ajout de l'utilisateur effectué", "Ajout effectué", MessageBoxButton.OK,
            MessageBoxImage.Information);

        // Clearing add form
        Username = "";
        Password = "";
        IsAdmin = false;
    }
}