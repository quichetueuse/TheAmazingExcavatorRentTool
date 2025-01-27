using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using MySqlConnector;
using TheAmazingExcavatorRentTool.Models;
using TheAmazingExcavatorRentTool.Services;

namespace TheAmazingExcavatorRentTool.ViewModels;

public class ExcavatorVM : BaseVM
{
    public ObservableCollection<Excavator> Excavators { get; set; }
    
    private BrandVM _brandvm;

    public ExcavatorVM(BrandVM brandvm)
    {
        _brandvm = brandvm;
        Load_Excavators();
    }


    private string _Name;

    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }

    private string _Description;

    public string Description
    {
        get { return _Description; }
        set { _Description = value; }
    }

    private Brand _Brand;

    public Brand Brand
    {
        get { return _Brand; }
        set { _Brand = value; }
    }

    private int _BucketLiters;

    public int BucketLiters
    {
        get { return _BucketLiters; }
        set { _BucketLiters = value; }
    }

    private int _ReleaseYear;

    public int ReleaseYear
    {
        get { return _ReleaseYear; }
        set { _ReleaseYear = value; }
    }


    private bool _IsUsed;

    public bool IsUsed
    {
        get { return _IsUsed; }
        set { _IsUsed = value; }
    }
    
    
    private int _DailyPrice;

    public int DailyPrice
    {
        get { return _DailyPrice; }
        set { _DailyPrice = value; }
    }

    private string _PicturePath;

    public string PicturePath
    {
        get { return _PicturePath; }
        set { _PicturePath = value; }
    }


    private DelegateCommand<Excavator> _deleteCommand;

    public DelegateCommand<Excavator> DeleteCommand =>
        _deleteCommand ?? (_deleteCommand = new DelegateCommand<Excavator>(DeleteExcavator));

    private DelegateCommand<Excavator> _updateCommand;

    public DelegateCommand<Excavator> UpdateCommand =>
        _updateCommand ?? (_updateCommand = new DelegateCommand<Excavator>(UpdateExcavator));

    private DelegateCommand _addCommand;

    public DelegateCommand AddCommand =>
        _addCommand ?? (_addCommand = new DelegateCommand(AddExcavator));

    public void Load_Excavators()
    {
        ObservableCollection<Excavator> excavators = new ObservableCollection<Excavator>();

        var dbCon = getDbCon();

        if (dbCon.IsConnect())
        {
            string query = "SELECT excavator_id, name, description, brand_id, bucket_liters, release_year, is_used, daily_price, picture FROM excavator";
            var cmd = new MySqlCommand(query, dbCon.Connection);

            var reader = cmd.ExecuteReader();
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
                    PicturePath = null;
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
        }

        Excavators = excavators;
    }


    private void DeleteExcavator(Excavator excavator_to_delete)
    {
        var Result = MessageBox.Show($"Voulez-vous vraiment supprimer le Dvd '{excavator_to_delete.Name}'?", "Supression ?",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        var dbCon = getDbCon();

        if (dbCon.IsConnect())
        {
            string checkquery = "SELECT COUNT(*) FROM rental WHERE excavator_id=@id";
            MySqlCommand cmd1 = new MySqlCommand(checkquery, dbCon.Connection);
            cmd1.Parameters.AddWithValue("@id", excavator_to_delete.ExcavatorId);

            Int32 count = Convert.ToInt32(cmd1.ExecuteScalar());
            if (count > 0)
            {
                MessageBox.Show("Une Location utilise ce cette pelleteuse!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string query = "DELETE FROM excavator WHERE excavator_id =@id";
            var cmd = new MySqlCommand(query, dbCon.Connection);
            cmd.Parameters.AddWithValue("@id", excavator_to_delete.ExcavatorId);
            var reader = cmd.ExecuteReader();
            
            for (int i = 0; i < Excavators.Count; i++)
            {
                if (Excavators[i].ExcavatorId == excavator_to_delete.ExcavatorId)
                {
                    Excavators.Remove(Excavators[i]);
                    MessageBox.Show("Suppression de la pelleteuse effectuée", "suppression effectuée", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }

            dbCon.Close();
        }
    }

    public void UpdateExcavator(Excavator excavator_to_update)
    {
        foreach (var excavator in Excavators)
        {
            if (excavator.Name == excavator_to_update.Name && excavator.ExcavatorId != excavator_to_update.ExcavatorId)
            {
                MessageBox.Show("Une pelleteuse de même nom éxiste déjà!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        var dbCon = getDbCon();
        if (dbCon.IsConnect())
        {
            string query =
                "UPDATE excavator SET name=@name, description=@description, brand_id=@brand_id, bucket_liters=@bucket_liters, release_year=@release_year, is_used=@is_used, picture=@picture WHERE excavator_id=@id";
            var cmd = new MySqlCommand(query, dbCon.Connection);
            cmd.Parameters.AddWithValue("@name", excavator_to_update.Name);
            cmd.Parameters.AddWithValue("@description", excavator_to_update.Description);
            cmd.Parameters.AddWithValue("@brand_id", excavator_to_update.Brand.BrandId);
            cmd.Parameters.AddWithValue("@bucket_liters", excavator_to_update.BucketLiters);
            cmd.Parameters.AddWithValue("@release_year", excavator_to_update.ReleaseYear);
            cmd.Parameters.AddWithValue("@is_used", Convert.ToInt32(excavator_to_update.IsUsed));
            cmd.Parameters.AddWithValue("@daily_price", excavator_to_update.DailyPrice);
            if (excavator_to_update.PicturePath == null)
            {
                cmd.Parameters.AddWithValue("@coverimage", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@coverimage", excavator_to_update.PicturePath);
            }
            cmd.Parameters.AddWithValue("@id", excavator_to_update.ExcavatorId);

            var reader = cmd.ExecuteReader();
            dbCon.Close();
        }

        for (int i = 0; i < Excavators.Count; i++)
        {
            if (Excavators[i].ExcavatorId == excavator_to_update.ExcavatorId)
            {
                Excavators[i] = excavator_to_update;
            }
        }

        MessageBox.Show("Modifications appliquées à la pelleteuse", "Modifications Appliquées", MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    public void AddExcavator()
    {
        var Result = MessageBox.Show("Voulez-vous vraiment ajouter un Dvd ?", "Ajout ?", MessageBoxButton.YesNo,
            MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        var dbCon = getDbCon();

        if (dbCon.IsConnect())
        {

            //vérification des champs
            string name = Name;
            string description = Description;
            Brand brand = Brand;
            int bucket_liters = BucketLiters;
            int release_year = ReleaseYear;
            bool is_used = IsUsed;
            int daily_price = DailyPrice;
            string picture_path = PicturePath;

            foreach (var excavator in Excavators)
            {
                if (excavator.Name == Name)
                {
                    MessageBox.Show("Une pelleteuse possède le même titre!", "Erreur", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }
            }

            string query =
                "INSERT INTO excavator (excavator_id, name, description, brand_id, bucket_liters, release_year, is_used, daily_price, picture) VALUES (@name, @description, @brand_id, @bucket_liters, @release_year, @daily_price, @picture_path)";
            var cmd = new MySqlCommand(query, dbCon.Connection);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@brand_id", brand.BrandId);
            cmd.Parameters.AddWithValue("@bucket_liters", bucket_liters);
            cmd.Parameters.AddWithValue("@release_year", release_year);
            cmd.Parameters.AddWithValue("@is_used", Convert.ToInt32(is_used));
            cmd.Parameters.AddWithValue("@daily_price", daily_price);
            
            if (String.IsNullOrEmpty(picture_path) || picture_path == "Aucun fichier séclectionné")
            {
                cmd.Parameters.AddWithValue("@picture_path", null);
            }
            else
            {
                cmd.Parameters.AddWithValue("@picture_path", picture_path);
            }

            var reader = cmd.ExecuteReader();
            int id_excavator_to_add = Convert.ToInt32(cmd.LastInsertedId);
            Excavators.Add(new Excavator(excavatorid: id_excavator_to_add, name: name, description: description, brand: brand,
                bucket_liters: bucket_liters, releaseyear: release_year, isused: Convert.ToBoolean(is_used), 
                dailyprice: daily_price, picturepath: picture_path));
            dbCon.Close();
            MessageBox.Show("Ajout de la pelleteuse effectué", "Ajout effectué", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }

    public Excavator getExcavator(int excavator_id) //getExcavator
    {
        // creer et retourne un dvd en fonction de l'id passée en parametre 
        //select DvdId, title from DVD
        string name = "";
        string description = "";
        int brand_id = 0;
        Brand brand = new Brand(0, "eee", 2024); // placeholder values
        int bucket_liters = 0;
        int release_year = 0;
        bool is_used = false;
        int daily_price = 0;
        string picture_path = "";
        
        DB dbCon = getDbCon();
        if (!dbCon.IsConnect())
        {
            throw new Exception("Failed to open database");
        }
        
        string query = "SELECT excavator_id, name, description, brand_id, bucket_liters, release_year, is_used, daily_price, picture FROM excavator" +
                       " Where excavator_id=@id";
        var cmd = new MySqlCommand(query, dbCon.Connection);
        cmd.Parameters.AddWithValue("@id", excavator_id);

        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            name = reader.GetString(1);
            description = reader.GetString(2);
            brand_id =reader.GetInt32(3);
            bucket_liters = reader.GetInt32(4);
            release_year = reader.GetInt32(5);
            is_used = reader.GetBoolean(6);
            daily_price = reader.GetInt32(7);
            if (!reader.IsDBNull(8))
            {
                picture_path = reader.GetString(8);
            }
            else
            {
                picture_path = null;
            }
        }

        dbCon.Close();
        
        foreach (var variabBrand in _brandvm.Brands)
        {
            if (variabBrand.BrandId == brand_id)
            {
                brand = variabBrand;
            }
        }

        return new Excavator(excavatorid: excavator_id, name: name, description: description, brand: brand, 
            bucket_liters: bucket_liters, releaseyear: release_year, isused: is_used, dailyprice: daily_price, picturepath: picture_path);
    }

    private string Sanitize(string str)
    {
        string blacklist = ")(*'\"";
        StringBuilder result = new StringBuilder(str.Length);
        foreach (char chr in str)
        {
            if (!blacklist.Contains(chr))
            {
                result.Append(chr);
            }
        }

        return result.ToString();
    }
}
