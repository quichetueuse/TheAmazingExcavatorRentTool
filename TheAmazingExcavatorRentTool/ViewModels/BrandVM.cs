using System.Collections.ObjectModel;
using MySqlConnector;
using TheAmazingExcavatorRentTool.Models;
using TheAmazingExcavatorRentTool.Services;

namespace TheAmazingExcavatorRentTool.ViewModels;

public class BrandVM: BaseVM
{
    
    public ObservableCollection<Brand> Brands { get; set; }
    
    public BrandVM()
    {
        // Load_Excavators();
    }
    
    
    
    private string _Name;

    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }
    
    private string _CreationYear;

    public string CreationYear
    {
        get { return _CreationYear; }
        set { _CreationYear = value; }
    }
    
    public Brand getBrand(int brand_id)
    {
        string name = "";
        int creation_year = 0;

        
        DB dbCon = getDbCon();
        if (!dbCon.IsConnect())
        {
            throw new Exception("Failed to open database");
        }
        
        string query = "SELECT name, creation_year FROM brand Where brand_id=@id";
        var cmd = new MySqlCommand(query, dbCon.Connection);
        cmd.Parameters.AddWithValue("@id", brand_id);

        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            name = reader.GetString(1);
            brand_id =reader.GetInt32(3);

        }

        dbCon.Close();

        return new Brand(brandId: brand_id, name: name, creationYear: creation_year);
    }
    
    
}