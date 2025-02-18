using System.Collections.ObjectModel;
using MySqlConnector;
using TheAmazingExcavatorRentTool.Models;
using TheAmazingExcavatorRentTool.Services;

namespace TheAmazingExcavatorRentTool.ViewModels;

public class UserVM: BaseVM
{

    public ObservableCollection<User> Users { get; set; }
    
    private SoundPlayer soundPlayer;

    private readonly string loadQuery;
    private readonly string addQuery;
    private readonly string updateQuery;
    private readonly string deleteQuery;

    public UserVM()
    {
        // Creating queries string
        loadQuery = "SELECT user_id, username, password, is_admin FROM _user";
        updateQuery =
            "UPDATE _user SET username=@username, password=@password, is_admin=@is_admin WHERE customer_id=@id";
        deleteQuery ="DELETE FROM _user WHERE user_id=@id";
        addQuery = "INSERT INTO _user (user_id, username, password, is_admin) VALUES (@username, @password, @is_admin)";
        
        // Loading sound player
        soundPlayer = new SoundPlayer();    
        
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
    // private DelegateCommand<Customer> _deleteCommand;
    // public DelegateCommand<Customer> DeleteCommand =>
    //     _deleteCommand ?? (_deleteCommand = new DelegateCommand<Customer>(Delete));
    //
    // private DelegateCommand _addCommand;
    // public DelegateCommand AddCommand =>
    //     _addCommand ?? (_addCommand = new DelegateCommand(Add));





    private void Load()
    {
        ObservableCollection<User> users = new ObservableCollection<User>();

        var dbCon = getDbCon();

        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new Exception(); //todo creer exception custom (style FailedConnectionException)
        }
        
        var cmd = new MySqlCommand(loadQuery, dbCon.Connection);

        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {

            Int32 UserId = reader.GetInt32(0);
            string Username = reader.GetString(1);
            string Password = reader.GetString(2);
            bool Email = reader.GetBoolean(3);
            
            
            users.Add(new User(userid: UserId, username: Username, password: Password, isAdmin: IsAdmin));
        }

        dbCon.Close();

        Users = users;
    }
}