namespace TheAmazingExcavatorRentTool.Models;

public class Excavator
{
    internal int ExcavatorId { get; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int BrandId { get; set; }
    public int BucketLiters { get; set; }
    public int ReleaseYear { get; set; }
    public bool IsUsed { get; set; }
    public int DailyPrice { get; set; }
    public string PicturePath { get; set; }

    public Excavator(int excavatorid, string name, string description, int brandid, int bucket_liters, int releaseyear, bool isused, int dailyprice, string picturepath)
    {
        ExcavatorId = excavatorid;
        Name = name;
        Description = description;
        BrandId = brandid;
        BucketLiters = bucket_liters;
        ReleaseYear = releaseyear;
        IsUsed = isused;
        DailyPrice = dailyprice;
        PicturePath = picturepath;
    }
}