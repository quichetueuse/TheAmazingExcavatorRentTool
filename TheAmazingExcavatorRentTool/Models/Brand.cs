namespace TheAmazingExcavatorRentTool.Models;

public class Brand
{
    internal int BrandId { get; }
    
    public string Name { get; set; }
    
    public int CreationYear { get; set; }



    public Brand(int brandId, string name, int creationYear)
    {
        BrandId = brandId;
        Name = name;
        CreationYear = creationYear;
    }
}