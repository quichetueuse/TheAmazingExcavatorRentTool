namespace TheAmazingExcavatorRentTool.Models;

public class Rental
{
    internal int RentalId { get; }
    public Customer Customer { get; set; }
    public Excavator Excavator { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public int Price { get; set; }

    public Rental(int rentalId, Customer customer, Excavator excavator, DateTime startDate, DateTime returnDate, int price)
    {
        RentalId = rentalId;
        Customer = customer;
        Excavator = excavator;
        StartDate = startDate;
        ReturnDate = returnDate;
        Price = price;
    }
}