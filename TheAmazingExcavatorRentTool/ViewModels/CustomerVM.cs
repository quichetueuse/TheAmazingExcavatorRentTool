using System.Collections.ObjectModel;
using System.Data;
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
        checkRentalQuery = "SELECT COUNT(*) FROM rental WHERE excavator_id=@id";
        deleteQuery ="DELETE FROM customer WHERE customer_id=@id";
        addQuery = "INSERT INTO customer (first_name, last_name, email, birth_date) VALUES (@first_name, @last_name, @email, @birth_date)";
        
        // Loading sound player
        soundPlayer = new SoundPlayer();
        
        // Setting default birth date
        BirthDate = DateTime.Now;
        
        // loading data
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
    private DelegateCommand<Customer> _deleteCommand;
    public DelegateCommand<Customer> DeleteCommand =>
        _deleteCommand ?? (_deleteCommand = new DelegateCommand<Customer>(Delete));
    
    private DelegateCommand _addCommand;
    public DelegateCommand AddCommand =>
        _addCommand ?? (_addCommand = new DelegateCommand(Add));
    

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


    public void Update(Customer customer_to_update)
    {
        // Check if a customer already exists with the same first name, last name and email
        foreach (var customer in Customers)
        {
            // if (customer.CustomerId == customer_to_update.CustomerId)
            //     continue;
            if (customer.FirstName == customer_to_update.FirstName && customer.LastName == customer_to_update.LastName && customer.Email == customer_to_update.Email && customer.CustomerId != customer_to_update.CustomerId)
            {
                soundPlayer.PlayFailSound();
                MessageBox.Show("Un client de même nom avec la même adresse email éxiste déjà!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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
        // string str_birth_date = customer_to_update.BirthDate.ToShortDateString();
        // cmd.Parameters.AddWithValue("@birth_date", str_birth_date);
        cmd.Parameters.Add("@birth_date", MySqlDbType.Date).Value = customer_to_update.BirthDate;
        cmd.Parameters.AddWithValue("@id", customer_to_update.CustomerId);
        cmd.ExecuteReader();
        dbCon.Close();
        
        // Updating customer in app
        for (int i = 0; i < Customers.Count; i++)
        {
            if (Customers[i].CustomerId == customer_to_update.CustomerId)
            {
                Customers[i] = customer_to_update;
                break;
            }
        }
        
        // Notify the user that the update succeeded
        soundPlayer.PlaySuccessSound();
        MessageBox.Show("Modifications appliquées au client", "Modifications Appliquées", MessageBoxButton.OK,
            MessageBoxImage.Information);
        
    }

    private void Delete(Customer customer_to_delete)
    {
        var Result = MessageBox.Show($"Voulez-vous vraiment supprimer le client sélectionné '{customer_to_delete.FirstName} {customer_to_delete.LastName}'?", "Supression ?",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        
        var dbCon = getDbCon();
        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new Exception(); //todo creer exception custom (style FailedConnectionException)
        }
        
        // Checking if customer is not used in rental
        MySqlCommand cmd1 = new MySqlCommand(checkRentalQuery, dbCon.Connection);
        cmd1.Parameters.AddWithValue("@id", customer_to_delete.CustomerId);

        Int32 count = Convert.ToInt32(cmd1.ExecuteScalar());
        if (count > 0)
        {
            soundPlayer.PlayFailSound();
            MessageBox.Show("Une Location utilise ce ce client!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        // Deleting customer from database
        var cmd = new MySqlCommand(deleteQuery, dbCon.Connection);
        cmd.Parameters.AddWithValue("@id", customer_to_delete.CustomerId);
        cmd.ExecuteReader(); //todo vérifier si la requete à fonctionner avant du supprimer de la liste
        
        // Deleting customer from app
        foreach (var varCustomer in Customers.ToList())
        {
            if (varCustomer.CustomerId != customer_to_delete.CustomerId)
                continue;
            
            // Notify the user that the removal succeeded
            Customers.Remove(varCustomer);
            soundPlayer.PlaySuccessSound();
            MessageBox.Show("Suppression du client effectuée", "suppression effectuée", MessageBoxButton.OK,
                MessageBoxImage.Information);
            break;
            
        }
        dbCon.Close();
    }

    private void Add()
    {
        var Result = MessageBox.Show("Voulez-vous vraiment ajouter un client ?", "Ajout ?", MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        
        var dbCon = getDbCon();
        
        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new Exception(); //todo creer exception custom (style FailedConnectionException)
        }
        
        // Check if customer with the same first name / last name / email already exists
        string first_name = _FirstName;
        string last_name = _LastName;
        string email = _Email;
        DateTime birth_date = _BirthDate;

        foreach (var customer in Customers)
        {
            if (customer.FirstName == first_name && customer.LastName == last_name && customer.Email == email)
            {
                soundPlayer.PlayFailSound();
                MessageBox.Show("Un client possède le même nom, prénom et email!", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
        }
        
        // Adding customer to database
        var cmd = new MySqlCommand(addQuery, dbCon.Connection);
        cmd.Parameters.AddWithValue("@first_name", first_name);
        cmd.Parameters.AddWithValue("@last_name", last_name);
        cmd.Parameters.AddWithValue("@email", email);

        // string str_birth_date = BirthDate.ToShortDateString();
        cmd.Parameters.Add("@birth_date", MySqlDbType.Date).Value =birth_date;
        // cmd.Parameters.AddWithValue("@birth_date", str_birth_date);
        
        cmd.ExecuteReader();
        
        // Adding customer to app
        int customer_id = Convert.ToInt32(cmd.LastInsertedId);
        Customer customer_obj = new Customer(customer_id: customer_id, first_name: first_name, last_name: last_name,
            email: email, birth_date: birth_date);
        Customers.Add(customer_obj);
        dbCon.Close();
        
        // Notify the user that adding succeeded
        soundPlayer.PlaySuccessSound();
        MessageBox.Show("Ajout du client effectué", "Ajout effectué", MessageBoxButton.OK,
            MessageBoxImage.Information);

        // Clearing add form
        FirstName = "";
        LastName = "";
        Email = "";
        BirthDate = DateTime.Now;
    }
}