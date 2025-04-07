using System.Windows;
using System.Windows.Input;
using TheAmazingExcavatorRentTool.Models;

namespace TheAmazingExcavatorRentTool.Views;

public partial class ViewRentalInfos : Window
{
    private Rental _selectedRental;
    
    public ViewRentalInfos(Rental selectedRental)
    {
        InitializeComponent();
        _selectedRental = selectedRental;
        FillViewForm();
    }
    
    private void FillViewForm()
    {
        txtCustomer.Text = _selectedRental.Customer.FullName;
        txtExcavator.Text = _selectedRental.Excavator.Name;
        txtStartDate.Text = _selectedRental.StartDate.ToString("dd/MM/yyyy");
        txtReturnDate.Text = _selectedRental.ReturnDate.ToString("dd/MM/yyyy");
    }
        
    private void DragWindow(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }
}