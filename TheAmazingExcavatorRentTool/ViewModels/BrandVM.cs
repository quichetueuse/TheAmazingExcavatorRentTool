using System.Collections.ObjectModel;
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
        
        LoadB();
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
    
    private string _CreationYear;

    public string CreationYear
    {
        get { return _CreationYear; }
        set
        {
            _CreationYear = value; 
            OnPropertyChanged();
        }
    }
    
    // private DelegateCommand<Brand> _deleteCommand;
    // public DelegateCommand<Brand> DeleteCommand =>
    //     _deleteCommand ?? (_deleteCommand = new DelegateCommand<Brand>(Delete));
    //
    //
    // private DelegateCommand _addCommand;
    // public DelegateCommand AddCommand =>
    //     _addCommand ?? (_addCommand = new DelegateCommand(Add));
    
    private void LoadB()
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
    
}