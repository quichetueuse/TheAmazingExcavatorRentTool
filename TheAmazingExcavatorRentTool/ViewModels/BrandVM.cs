using System.Collections.ObjectModel;
using System.Windows;
using MySqlConnector;
using TheAmazingExcavatorRentTool.Models;
using TheAmazingExcavatorRentTool.Services;

namespace TheAmazingExcavatorRentTool.ViewModels;

public class BrandVM: BaseVM
{
    
    public ObservableCollection<Brand> Brands { get; set; }
    
    private SoundPlayer soundPlayer;

    private readonly string loadQuery;
    private readonly string addQuery;
    private readonly string updateQuery;
    private readonly string deleteQuery;
    private readonly string checkExcavQuery;
    
    public BrandVM()
    {
        // Creating queries string
        loadQuery = "SELECT brand_id, name, creation_year FROM brand";
        updateQuery =
            "UPDATE brand SET name=@name, creation_year=@creation_year WHERE brand_id=@id";
        checkExcavQuery = "SELECT COUNT(*) FROM excavator WHERE brand_id=@id";
        deleteQuery ="DELETE FROM brand WHERE brand_id=@id";
        addQuery = "INSERT INTO brand (name, creation_year) VALUES (@name, @creation_year)";

        soundPlayer = new SoundPlayer();
        
        Load();
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
    
    private int _CreationYear;

    public int CreationYear
    {
        get { return _CreationYear; }
        set
        {
            _CreationYear = value; 
            OnPropertyChanged();
        }
    }
    
    private DelegateCommand<Brand> _deleteCommand;
    public DelegateCommand<Brand> DeleteCommand =>
        _deleteCommand ?? (_deleteCommand = new DelegateCommand<Brand>(Delete));
    
    
    private DelegateCommand _addCommand;
    public DelegateCommand AddCommand =>
        _addCommand ?? (_addCommand = new DelegateCommand(Add));
    
    private void Load()
    {
        ObservableCollection<Brand> brands = new ObservableCollection<Brand>();

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

            Int32 BrandId = reader.GetInt32(0);
            string Name = reader.GetString(1);
            int CreationYear = reader.GetInt32(2);
            
            
            brands.Add(new Brand(brandId: BrandId, name: Name, creationYear: CreationYear));
        }

        dbCon.Close();

        Brands = brands;
    }

    public void Update(Brand brand_to_update)
    {
        // Check if a customer already exists with the same first name, last name and email
        foreach (Brand brand in Brands.ToList())
        {
            if (brand.Name == brand_to_update.Name && brand_to_update.BrandId != brand_to_update.BrandId)
            {
                soundPlayer.PlayFailSound();
                MessageBox.Show("Une marque de même nom éxiste déjà!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        // Updating brand in database
        var dbCon = getDbCon();
        
        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new Exception(); //todo creer exception custom (style FailedConnectionException)
        }
        
        var cmd = new MySqlCommand(updateQuery, dbCon.Connection);
        cmd.Parameters.AddWithValue("@name", brand_to_update.Name);
        cmd.Parameters.AddWithValue("@creation_year", brand_to_update.CreationYear);
        cmd.Parameters.AddWithValue("@id", brand_to_update.BrandId);
        cmd.ExecuteReader();
        dbCon.Close();
        
        // Updating brand in app
        for (int i = 0; i < Brands.ToList().Count; i++)
        {
            if (Brands[i].BrandId == brand_to_update.BrandId)
            {
                Brands[i] = brand_to_update;
                break;
            }
        }
        
        // Notify the user that the update succeeded
        soundPlayer.PlaySuccessSound();
        MessageBox.Show("Modifications appliquées à la marque", "Modifications Appliquées", MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void Delete(Brand brand_to_delete)
    {
        var Result = MessageBox.Show($"Voulez-vous vraiment supprimer la marque sélectionnée '{brand_to_delete.Name} ({brand_to_delete.CreationYear})'?", "Suppression ?",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        
        var dbCon = getDbCon();
        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new Exception(); //todo creer exception custom (style FailedConnectionException)
        }
        
        // Checking if brand is not used by any excavator
        MySqlCommand cmd1 = new MySqlCommand(checkExcavQuery, dbCon.Connection);
        cmd1.Parameters.AddWithValue("@id", brand_to_delete.BrandId);

        Int32 count = Convert.ToInt32(cmd1.ExecuteScalar());
        if (count > 0)
        {
            soundPlayer.PlayFailSound();
            MessageBox.Show("Une pelleteuse appartient à cette marque!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        // Deleting brand from database
        var cmd = new MySqlCommand(deleteQuery, dbCon.Connection);
        cmd.Parameters.AddWithValue("@id", brand_to_delete.BrandId);
        cmd.ExecuteReader(); //todo vérifier si la requete à fonctionner avant du supprimer de la liste
        dbCon.Close();
        
        // Deleting brand from app
        foreach (Brand varBrand in Brands.ToList())
        {
            if (varBrand.BrandId != brand_to_delete.BrandId)
                continue;
            
            // Notify the user that the removal succeeded
            Brands.Remove(varBrand);
            soundPlayer.PlaySuccessSound();
            MessageBox.Show("Suppression de la marque effectuée", "suppression effectuée", MessageBoxButton.OK,
                MessageBoxImage.Information);
            break;
            
        }
    }

    private void Add()
    {
        var Result = MessageBox.Show("Voulez-vous vraiment ajouter une marque ?", "Ajout ?", MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        
        var dbCon = getDbCon();
        
        if (!dbCon.IsConnect())
        {
            Console.WriteLine("Cannot connect to database (maybe MySql server isn't running!)");
            throw new Exception(); //todo creer exception custom (style FailedConnectionException)
        }
        
        // Check if brand with the same name already exists
        string name = _Name;
        int creation_year = _CreationYear;

        foreach (Brand brand in Brands.ToList())
        {
            if (brand.Name == name)
            {
                soundPlayer.PlayFailSound();
                MessageBox.Show("Une marque possède le même nom!", "Erreur", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
        }
        
        // Adding brand to database
        var cmd = new MySqlCommand(addQuery, dbCon.Connection);
        cmd.Parameters.AddWithValue("@name", name);
        cmd.Parameters.AddWithValue("@creation_year", creation_year);

        cmd.ExecuteReader();
        
        // Adding brand to app
        int brand_id = Convert.ToInt32(cmd.LastInsertedId);
        Brand brand_obj = new Brand(brandId: brand_id, name: name, creationYear: creation_year);
        Brands.Add(brand_obj);
        dbCon.Close();
        
        // Notify the user that adding succeeded
        soundPlayer.PlaySuccessSound();
        MessageBox.Show("Ajout de la marque effectué", "Ajout effectué", MessageBoxButton.OK,
            MessageBoxImage.Information);

        // Clearing add form
        Name = "";
        CreationYear = DateTime.Now.Year;
    }
}