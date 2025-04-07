using System.Windows;
using System.Windows.Input;
using TheAmazingExcavatorRentTool.Models;

namespace TheAmazingExcavatorRentTool.Views;

public partial class ViewUserInfos : Window
{
    private User _selectedUser;
    
    public ViewUserInfos(User selectedUser)
    {
        InitializeComponent();

        _selectedUser = selectedUser;
        FillViewForm();
    }
    
    private void FillViewForm()
    {
        txtUsername.Text = _selectedUser.Username;
        string is_admin = "Non";
        if (_selectedUser.IsAdmin)
        {
            is_admin = "Oui";
        }

        txtIsAdmin.Text = is_admin;
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