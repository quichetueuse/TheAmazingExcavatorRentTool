using System.Collections.ObjectModel;
using System.Windows;
using MySqlConnector;
using TheAmazingExcavatorRentTool.Models;
using TheAmazingExcavatorRentTool.Services;

namespace TheAmazingExcavatorRentTool.ViewModels;

public class CustomerVM: BaseVM
{

    public ObservableCollection<Customer> Customers { get; set; }
    
    private SoundPlayer soundPlayer;

    private readonly string loadQuery;
    private readonly string addQuery;
    private readonly string updateQuery;
    private readonly string deleteQuery;
    private readonly string checkRentalQuery;
    
    public CustomerVM()
    {
        // Creating queries string
        loadQuery = "SELECT customer_id, first_name, last_name, email, birth_date FROM customer";
        updateQuery =
            "UPDATE customer SET first_name=@first_name, last_name=@last_name, email=@email, birth_date=@birth_date WHERE customer_id=@id";
        
        Load();
    }


    private string _FirstName;

    public string FirstName
    {
        get { return _FirstName; }
        set
        {
            _FirstName = value; 
            OnPropertyChanged();
        }
    }
    
    private string _LastName;

    public string LastName
    {
        get { return _LastName; }
        set
        {
            _LastName = value; 
            OnPropertyChanged();
        }
    }
    
    private string _Email;

    public string Email
    {
        get { return _Email; }
        set
        {
            _Email = value; 
            OnPropertyChanged();
        }
    }
    
    private DateTime _BirthDate;

    public DateTime BirthDate
    {
        get { return _BirthDate; }
        set
        {
            _BirthDate = value; 
            OnPropertyChanged();
        }
    }
    
    // Command that get called from view
    // private DelegateCommand<Excavator> _deleteCommand;
    // public DelegateCommand<Excavator> DeleteCommand =>
    //     _deleteCommand ?? (_deleteCommand = new DelegateCommand<Excavator>(DeleteExcavator));
    //
    // private DelegateCommand _addCommand;
    // public DelegateCommand AddCommand =>
    //     _addCommand ?? (_addCommand = new DelegateCommand(AddExcavator));
    

    private void Load()
    {
        ObservableCollection<Customer> customers = new ObservableCollection<Customer>();

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

            Int32 CustomerId = reader.GetInt32(0);
            string FirstName = reader.GetString(1);
            string LastName = reader.GetString(2);
            string Email = reader.GetString(3);
            DateTime BirthDate = reader.GetDateTime(4);
            
            
            customers.Add(new Customer(customer_id: CustomerId, first_name: FirstName, last_name: LastName, email: Email, birth_date: BirthDate));
        }

        dbCon.Close();

        Customers = customers;
    }


    private void Update(Customer customer_to_update)
    {
        // Check if a customer already exists with the same first name, last name and email
        foreach (var customer in Customers)
        {
            // if (customer.CustomerId == customer_to_update.CustomerId)
            //     continue;
            if (customer.FirstName == customer_to_update.FirstName && customer.LastName == customer_to_update.LastName && customer.Email == customer_to_update.Email && customer.CustomerId != customer_to_update.CustomerId)
            {
                soundPlayer.PlayFailSound();
                MessageBox.Show("Une un client de même nom avec la même adresse email éxiste déjà!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        // Updating customer in database
        var dbCon = getDbCon();
        
        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new Exception(); //todo creer exception custom (style FailedConnectionException)
        }
        
        var cmd = new MySqlCommand(updateQuery, dbCon.Connection);
        cmd.Parameters.AddWithValue("@first_name", customer_to_update.FirstName);
        cmd.Parameters.AddWithValue("@last_name", customer_to_update.LastName);
        cmd.Parameters.AddWithValue("@email", customer_to_update.Email);
        cmd.Parameters.AddWithValue("@birth_date", customer_to_update.BirthDate);
        cmd.Parameters.AddWithValue("@id", customer_to_update.CustomerId);
        cmd.ExecuteReader();
        dbCon.Close();
        
        // Updating user in app
        for (int i = 0; i < Customers.Count; i++)
        {
            if (Customers[i].CustomerId == customer_to_update.CustomerId)
            {
                Customers[i] = customer_to_update;
            }
        }
        
        // Notify the user that the update succeeded
        soundPlayer.PlaySuccessSound();
        MessageBox.Show("Modifications appliquées à la pelleteuse", "Modifications Appliquées", MessageBoxButton.OK,
            MessageBoxImage.Information);
        
    }
}