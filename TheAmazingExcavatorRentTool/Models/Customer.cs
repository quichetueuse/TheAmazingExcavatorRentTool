namespace TheAmazingExcavatorRentTool.Models;

public class Customer
{
    internal int CustomerId { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }


    public Customer(int customer_id, string first_name, string last_name, string email, DateTime birth_date)
    {
        CustomerId = customer_id;
        FirstName = first_name;
        LastName = last_name;
        Email = email;
        BirthDate = birth_date;
    }
}