using System.Collections.ObjectModel;
using System.Windows;
using MySqlConnector;
using TheAmazingExcavatorRentTool.Models;
using TheAmazingExcavatorRentTool.Services;

namespace TheAmazingExcavatorRentTool.ViewModels;

public class RentalVM: BaseVM
{
    
    
    public ObservableCollection<Rental> Rentals { get; set; }
    
    private SoundPlayer soundPlayer;

    private readonly string loadQuery;
    private readonly string addQuery;
    private readonly string updateQuery;
    private readonly string deleteQuery;

    public RentalVM(CustomerVM customervm, ExcavatorVM excavatorvm)
    {
        _CustomerVm = customervm;
        _ExcavatorVm = excavatorvm;
        
        // Creating queries string
        loadQuery = "SELECT rental_id, customer_id, excavator_id, start_date, return_date, price FROM rental";
        updateQuery =
            "UPDATE rental SET customer_id=@customer_id, excavator_id=@excavator_id, start_date=@start_date, return_date=@return_date, price=@price WHERE rental_id=@id";
        deleteQuery ="DELETE FROM rental WHERE rental_id=@id";
        addQuery = "INSERT INTO rental (customer_id, excavator_id, start_date, return_date, price) VALUES (@customer_id, @excavator_id, @start_date, @return_date, @price)";

        soundPlayer = new SoundPlayer();
        
        Load();
    }
    
    
    private CustomerVM _CustomerVm;

    public CustomerVM CustomerVm
    {
        get
        {
            return _CustomerVm;
        }
    }
    
    private ExcavatorVM _ExcavatorVm;

    public ExcavatorVM ExcavatorVm
    {
        get
        {
            return _ExcavatorVm;
        }
    }
    
    
    private Customer _Customer;

    public Customer Customer
    {
        get { return _Customer; }
        set
        {
            _Customer = value; 
            OnPropertyChanged();
        }
    }
    
    private Excavator _Excavator;

    public Excavator Excavator
    {
        get { return _Excavator; }
        set
        {
            _Excavator = value; 
            OnPropertyChanged();
        }
    }
    
    private DateTime _StartDate;

    public DateTime StartDate
    {
        get { return _StartDate; }
        set
        {
            _StartDate = value; 
            OnPropertyChanged();
        }
    }
    
    private DateTime _ReturnDate;

    public DateTime ReturnDate
    {
        get { return _ReturnDate; }
        set
        {
            _ReturnDate = value; 
            OnPropertyChanged();
        }
    }
    
    // Command that get called from view
    // private DelegateCommand<Rental> _deleteCommand;
    // public DelegateCommand<Rental> DeleteCommand =>
    //     _deleteCommand ?? (_deleteCommand = new DelegateCommand<Rental>(Delete));
    //
    // private DelegateCommand _addCommand;
    // public DelegateCommand AddCommand =>
    //     _addCommand ?? (_addCommand = new DelegateCommand(Add));

    private void Load()
    {
        ObservableCollection<Rental> rentals = new ObservableCollection<Rental>();

        var dbCon = getDbCon();

        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new Exception(); //todo creer exception custom (style FailedConnectionException)
        }
        
        var cmd = new MySqlCommand(loadQuery, dbCon.Connection);
        
        var reader = cmd.ExecuteReader();
        // Create rental models
        while (reader.Read())
        {
        
            Int32 rental_id = reader.GetInt32(0);
            Int32 customer_id = reader.GetInt32(1);
            Customer customer = null;
            Int32 excavator_id = reader.GetInt32(2);
            Excavator excavator = null;
            DateTime start_date = reader.GetDateTime(3);
            DateTime return_date = reader.GetDateTime(4);
            Int32 price = reader.GetInt32(5);
            
            // Get customer model
            foreach (Customer varCustomer in _CustomerVm.Customers)
            {
                if (varCustomer.CustomerId == customer_id)
                {
                    customer = varCustomer;
                    break;
                }
            }
            
            // Get Excavator model
            foreach (Excavator varExcavator in _ExcavatorVm.Excavators)
            {
                if (varExcavator.ExcavatorId == excavator_id)
                {
                    excavator = varExcavator;
                    break;
                }
            }

            Rental rental_obj = new Rental(rentalId: rental_id, customer: customer, excavator: excavator,
                startDate: start_date, returnDate: return_date, price: price);
            
            rentals.Add(rental_obj);
        }

        dbCon.Close();

        Rentals = rentals;
    }

    public void Update(Rental rental_to_update)
    {
        // Check if excavator is used by another rental
        if (rental_to_update.Excavator.IsUsed)
        {
            soundPlayer.PlayFailSound();
            MessageBox.Show("La pelleteuse choisie est utilisée!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        // Check if a rental with the same customer, excavator on the same time period already exists
        foreach (var rental in Rentals)
        {
            if (rental.RentalId == rental_to_update.RentalId)
                continue;
            if (rental.Excavator.ExcavatorId == rental_to_update.Excavator.ExcavatorId &&
                rental.Customer.CustomerId == rental_to_update.Customer.CustomerId &&
                rental.StartDate == rental_to_update.StartDate &&
                rental.ReturnDate == rental_to_update.ReturnDate)
            {
                soundPlayer.PlayFailSound();
                MessageBox.Show("Une location de ce client avec cette pelleteuse sur cette période éxiste déjà!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        // Updating rental in database
        var dbCon = getDbCon();
        
        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new Exception(); //todo creer exception custom (style FailedConnectionException)
        }
        
        var cmd = new MySqlCommand(updateQuery, dbCon.Connection);
        cmd.Parameters.AddWithValue("@customer_id", rental_to_update.Customer.CustomerId);
        cmd.Parameters.AddWithValue("@excavator_id", rental_to_update.Excavator.ExcavatorId);
        cmd.Parameters.Add("@start_date", MySqlDbType.Date).Value = rental_to_update.StartDate;
        cmd.Parameters.Add("@return_date", MySqlDbType.Date).Value = rental_to_update.ReturnDate;
        cmd.Parameters.AddWithValue("@id", rental_to_update.RentalId);
        cmd.ExecuteReader();
        dbCon.Close();
        
        // Updating rental in app
        for (int i = 0; i < Rentals.Count; i++)
        {
            if (Rentals[i].RentalId == rental_to_update.RentalId)
            {
                Rentals[i] = rental_to_update;
                break;
            }
        }
        
        // Notify the user that the update succeeded
        soundPlayer.PlaySuccessSound();
        MessageBox.Show("Modifications appliquées à la location", "Modifications Appliquées", MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void Delete(Rental rental_to_delete)
    {
        
    }

    private void Add()
    {
        
    }
    
}