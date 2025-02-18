using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Data;
using Prism.Commands;
using MySqlConnector;
using TheAmazingExcavatorRentTool.Models;
using TheAmazingExcavatorRentTool.Services;

namespace TheAmazingExcavatorRentTool.ViewModels;

public class ExcavatorVM : BaseVM
{
    public ObservableCollection<Excavator> Excavators { get; set; } 
    public CollectionView  NonUsedExcavatorsView { get;}
    public CollectionView  AllExcavatorsView { get;}

    private SoundPlayer soundPlayer;

    private readonly string loadQuery;
    private readonly string addQuery;
    private readonly string updateQuery;
    private readonly string updateAvailabilityQuery;
    private readonly string deleteQuery;
    private readonly string checkRentalQuery;
    

    public ExcavatorVM(BrandVM brandvm)
    {
        soundPlayer = new SoundPlayer();
        
        // Creating queries string
        loadQuery =
            "SELECT excavator_id, name, description, brand_id, bucket_liters, release_year, is_used, daily_price, picture FROM excavator";
        deleteQuery = "DELETE FROM excavator WHERE excavator_id=@id";
        checkRentalQuery = "SELECT COUNT(*) FROM rental WHERE excavator_id=@id";
        updateQuery =
            "UPDATE excavator SET name=@name, description=@description, brand_id=@brand_id, bucket_liters=@bucket_liters, release_year=@release_year, picture=@picture WHERE excavator_id=@id";
        updateAvailabilityQuery = "UPDATE excavator SET is_used=@is_used WHERE excavator_id=@id";
        addQuery =
            "INSERT INTO excavator (name, description, brand_id, bucket_liters, release_year, is_used, daily_price, picture) VALUES (@name, @description, @brand_id, @bucket_liters, @release_year, @is_used, @daily_price, @picture_path)";
            
        
        //todo ajouter les commentaires comme dans le customerVM
        
        _brandvm = brandvm;
        Load();

        // Creating CollectionViewSource to both datagrid (non filtered) and combo box (only non used)
        AllExcavatorsView = (CollectionView)new CollectionViewSource() { Source = Excavators}.View;
        NonUsedExcavatorsView = (CollectionView)new CollectionViewSource() { Source = Excavators}.View;
        NonUsedExcavatorsView.Filter = NonUsedExcavFilter;
    }
    
    private BrandVM _brandvm;

    public BrandVM BrandVm
    {
        get
        {
            return _brandvm;
        }
    }


    private string _Name;

    public string Name
    {
        get { return _Name; }
        set
        {
            _Name = value; 
            OnPropertyChanged();
        }
    }

    private string _Description;

    public string Description
    {
        get { return _Description; }
        set
        {
            _Description = value; 
            OnPropertyChanged();
        }
    }

    private Brand _Brand;

    public Brand Brand
    {
        get { return _Brand; }
        set
        {
            _Brand = value; 
            OnPropertyChanged();
        }
    }

    private int _BucketLiters;

    public int BucketLiters
    {
        get { return _BucketLiters; }
        set
        {
            _BucketLiters = value;
            OnPropertyChanged();
        }
    }

    private int _ReleaseYear;

    public int ReleaseYear
    {
        get { return _ReleaseYear; }
        set
        {
            _ReleaseYear = value;
            OnPropertyChanged();
        }
    }


    private bool _IsUsed;

    public bool IsUsed
    {
        get { return _IsUsed; }
        set
        {
            _IsUsed = value; 
            OnPropertyChanged();
        }
    }
    
    
    private int _DailyPrice;

    public int DailyPrice
    {
        get { return _DailyPrice; }
        set
        {
            _DailyPrice = value; 
            OnPropertyChanged();
        }
    }

    private string _PicturePath;

    public string PicturePath
    {
        get { return _PicturePath; }
        set
        {
            _PicturePath = value;
            OnPropertyChanged();
        }
    }

    // Command that get called from view
    private DelegateCommand<Excavator> _deleteCommand;

    public DelegateCommand<Excavator> DeleteCommand =>
        _deleteCommand ?? (_deleteCommand = new DelegateCommand<Excavator>(Delete));

    private DelegateCommand _addCommand;

    public DelegateCommand AddCommand =>
        _addCommand ?? (_addCommand = new DelegateCommand(Add));

    private void Load()
    {
        ObservableCollection<Excavator> excavators = new ObservableCollection<Excavator>();

        var dbCon = getDbCon();

        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new Exception(); //todo creer exception custom (style FailedConnectionException)
        }
        
        var cmd = new MySqlCommand(loadQuery, dbCon.Connection);

        var reader = cmd.ExecuteReader();
        // Create excavator models
        while (reader.Read())
        {

            Int32 ExcavatorId = reader.GetInt32(0);
            string Name = reader.GetString(1);
            string Description = reader.GetString(2);
            int BrandId = reader.GetInt32(3);
            Brand brand = new Brand(0, "eee", 2024); // placeholder values
            int BucketLiters = reader.GetInt32(4);
            int release_year = reader.GetInt32(5);
            bool IsUsed = reader.GetBoolean(6);
            int DailyPrice = reader.GetInt32(7);
            
            string PicturePath;
            if (!reader.IsDBNull(8))
            {
                PicturePath = reader.GetString(8);
            }
            else
            {
                PicturePath = "Aucun fichier séclectionné";
            }

            foreach (var variabBrand in _brandvm.Brands)
            {
                if (variabBrand.BrandId == BrandId)
                {
                    brand = variabBrand;
                }
            }

            excavators.Add(new Excavator(excavatorid: ExcavatorId, name: Name, description: Description, 
                brand: brand, bucket_liters: BucketLiters, releaseyear: release_year, isused: IsUsed, 
                dailyprice: DailyPrice, picturepath: PicturePath));
        }

        dbCon.Close();

        Excavators = excavators;
    }


    private void Delete(Excavator excavator_to_delete)
    {
        var Result = MessageBox.Show($"Voulez-vous vraiment supprimer la pelleteuse '{excavator_to_delete.Name}'?", "Suppression ?",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        var dbCon = getDbCon();
        
        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new Exception(); //todo creer exception custom (style FailedConnectionException)
        }
        
        // Check if excavator is not used in any rentals
        MySqlCommand cmd1 = new MySqlCommand(checkRentalQuery, dbCon.Connection);
        cmd1.Parameters.AddWithValue("@id", excavator_to_delete.ExcavatorId);

        Int32 count = Convert.ToInt32(cmd1.ExecuteScalar());
        if (count > 0)
        {
            soundPlayer.PlayFailSound();
            MessageBox.Show("Une Location utilise ce cette pelleteuse!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        // Deleting excavator from database
        var cmd = new MySqlCommand(deleteQuery, dbCon.Connection);
        cmd.Parameters.AddWithValue("@id", excavator_to_delete.ExcavatorId);
        cmd.ExecuteReader(); //todo vérifier si la requete à fonctionner avant du supprimer de la liste
        
        
        // Deleting excavator from app
        foreach (var varExcavator in Excavators.ToList())
        {
            if (varExcavator.ExcavatorId != excavator_to_delete.ExcavatorId)
                continue;
            Excavators.Remove(varExcavator);
            // Notify the user that the removal succeeded
            soundPlayer.PlaySuccessSound();
            MessageBox.Show("Suppression de la pelleteuse effectuée", "suppression effectuée", MessageBoxButton.OK,
                MessageBoxImage.Information);
            break;
            
        }
        
        // for (int i = 0; i < Excavators.Count; i++)
        // {
        //     if (Excavators[i].ExcavatorId == excavator_to_delete.ExcavatorId)
        //     {
        //         Excavators.Remove(Excavators[i]);
        //         soundPlayer.PlaySuccessSound();
        //         MessageBox.Show("Suppression de la pelleteuse effectuée", "suppression effectuée", MessageBoxButton.OK,
        //             MessageBoxImage.Information);
        //     }
        // 
        
        dbCon.Close();
    }

    public void Update(Excavator excavator_to_update)
    {
        // Check if an excavator with same name exists
        foreach (var excavator in Excavators)
        {
            if (excavator.Name == excavator_to_update.Name && excavator.ExcavatorId != excavator_to_update.ExcavatorId)
            {
                soundPlayer.PlayFailSound();
                MessageBox.Show("Une pelleteuse de même nom éxiste déjà!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        var dbCon = getDbCon();
        
        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new Exception(); //todo creer exception custom (style FailedConnectionException)
        }
        
        // Update excavator in database
        var cmd = new MySqlCommand(updateQuery, dbCon.Connection);
        cmd.Parameters.AddWithValue("@name", excavator_to_update.Name);
        cmd.Parameters.AddWithValue("@description", excavator_to_update.Description);
        cmd.Parameters.AddWithValue("@brand_id", excavator_to_update.Brand.BrandId);
        cmd.Parameters.AddWithValue("@bucket_liters", excavator_to_update.BucketLiters);
        cmd.Parameters.AddWithValue("@release_year", excavator_to_update.ReleaseYear);
        // cmd.Parameters.AddWithValue("@is_used", Convert.ToInt32(excavator_to_update.IsUsed));
        cmd.Parameters.AddWithValue("@daily_price", excavator_to_update.DailyPrice);
        if (excavator_to_update.PicturePath == "Aucun fichier séclectionné")
        {
            cmd.Parameters.AddWithValue("@picture", null);
        }
        else
        {
            cmd.Parameters.AddWithValue("@picture", excavator_to_update.PicturePath);
        }
        cmd.Parameters.AddWithValue("@id", excavator_to_update.ExcavatorId);
        cmd.ExecuteReader();
        dbCon.Close();

        // Update excavator in app
        for (int i = 0; i < Excavators.Count; i++)
        {
            if (Excavators[i].ExcavatorId == excavator_to_update.ExcavatorId)
            {
                Excavators[i] = excavator_to_update;
                break;
            }
        }
        // Notify the user the update succeeded
        soundPlayer.PlaySuccessSound();
        MessageBox.Show("Modifications appliquées à la pelleteuse", "Modifications Appliquées", MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void Add()
    {
        var Result = MessageBox.Show("Voulez-vous vraiment ajouter une pelleteuse ?", "Ajout ?", MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        
        var dbCon = getDbCon();
        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new Exception(); //todo creer exception custom (style FailedConnectionException)
        }

        //vérification des champs
        string name = _Name;
        string description = _Description;
        Brand brand = _Brand;
        int brand_id = brand.BrandId;
        int bucket_liters = _BucketLiters;
        int release_year = _ReleaseYear;
        bool is_used = _IsUsed;
        int daily_price = _DailyPrice;
        string picture_path = _PicturePath;
        
        // Check if an excavator exists with the same name
        foreach (var excavator in Excavators)
        {
            if (excavator.Name == name)
            {
                soundPlayer.PlayFailSound();
                MessageBox.Show("Une pelleteuse possède le même titre!", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
        }
        
        // Adding excavator to database
        var cmd = new MySqlCommand(addQuery, dbCon.Connection);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@description", description);
        cmd.Parameters.AddWithValue("@brand_id", brand_id);
        cmd.Parameters.AddWithValue("@bucket_liters", bucket_liters);
        cmd.Parameters.AddWithValue("@release_year", release_year);
        cmd.Parameters.AddWithValue("@is_used", false);
        cmd.Parameters.AddWithValue("@daily_price", daily_price);
        
        if (String.IsNullOrEmpty(picture_path) || picture_path == "Aucun fichier séclectionné")
        {
            cmd.Parameters.AddWithValue("@picture_path", null);
            picture_path = "Aucun fichier séclectionné";
        }
        else
        {
            cmd.Parameters.AddWithValue("@picture_path", picture_path);
        }
        cmd.ExecuteReader();
        
        // Adding excavator in app
        int id_excavator_to_add = Convert.ToInt32(cmd.LastInsertedId);
        Excavators.Add(new Excavator(excavatorid: id_excavator_to_add, name: name, description: description, brand: brand,
            bucket_liters: bucket_liters, releaseyear: release_year, isused: Convert.ToBoolean(is_used), 
            dailyprice: daily_price, picturepath: picture_path));
        dbCon.Close();
        
        // Notify the user that adding succeeded
        soundPlayer.PlaySuccessSound();
        MessageBox.Show("Ajout de la pelleteuse effectué", "Ajout effectué", MessageBoxButton.OK,
            MessageBoxImage.Information);

        // Clearing add form
        Name = "";
        Description = "";
        Brand = null;
        BucketLiters = 0;
        ReleaseYear = 0;
        DailyPrice = 0;
    }
    
    
    private bool NonUsedExcavFilter(object obj)
    {
        // If given object is not excavator
        if (obj is not Excavator)
            return false;
        
        Excavator excavator = obj as Excavator;
        // Remove any used excavator from view
        if (excavator.IsUsed)
        {
            return false;
        }
        return true;
        
    }


    public void UpdateExcavatorUsability(Excavator excavator, bool is_used)
    {
        // Check if somehow the excavator is already not available
        if (excavator.IsUsed == is_used)
        {
            // Console.WriteLine("Values are the same");
            return;   
        }
        
        // Console.WriteLine($"changing availability for {excavator.Name}");
        // Updating excavator in database
        var dbCon = getDbCon();
        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new Exception(); //todo creer exception custom (style FailedConnectionException)
        }
        // Update availability in database
        var cmd = new MySqlCommand(updateAvailabilityQuery, dbCon.Connection);
        cmd.Parameters.AddWithValue("@is_used", Convert.ToInt32(is_used));
        cmd.Parameters.AddWithValue("@id", excavator.ExcavatorId);
        var reader = cmd.ExecuteReader();
        if (reader.RecordsAffected != 1)
        {
            // Console.WriteLine("Availability change failed");
            throw new Exception();
        }
        dbCon.Close();

        // excavator.IsUsed = is_used;
        // Updating availability in app
        for (int i = 0; i < Excavators.ToList().Count; i++)
        {
            if (Excavators[i].ExcavatorId == excavator.ExcavatorId)
            {
                // Excavators[i] = excavator;
                Excavators[i].IsUsed = is_used;
                break;
            }
        }
        NonUsedExcavatorsView.Refresh();
        AllExcavatorsView.Refresh();
        
    }
    
}
