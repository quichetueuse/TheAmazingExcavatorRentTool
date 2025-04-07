using System.Windows;
using System.Windows.Input;
using TheAmazingExcavatorRentTool.Models;

namespace TheAmazingExcavatorRentTool.Views;

public partial class ViewCustomerInfos : Window
{
    private Customer _selectedCustomer;
    
    public ViewCustomerInfos(Customer selectedCustomer)
    {
        InitializeComponent();

        _selectedCustomer = selectedCustomer;
        FillViewForm();
    }


    private void FillViewForm()
    {
        txtLastName.Text = _selectedCustomer.LastName;
        txtFirstName.Text = _selectedCustomer.FirstName;
        txtEmail.Text = _selectedCustomer.Email;
        txtBirthDate.Text = _selectedCustomer.BirthDate.ToString("dd/MM/yyyy");
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