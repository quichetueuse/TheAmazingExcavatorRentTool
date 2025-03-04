using System.Windows;
using System.Windows.Input;
using TheAmazingExcavatorRentTool.Services;
using TheAmazingExcavatorRentTool.ViewModels;
using TheAmazingExcavatorRentTool.Views;

namespace TheAmazingExcavatorRentTool;

public partial class MainWindowNextGen : Window
{
    public MainWindowNextGen()
    {
        InitializeComponent();
        MainWindowVM mainWindowVm = new MainWindowVM();

    }
    
    private void DragWindow(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void LogoutButton_OnClick(object sender, RoutedEventArgs e)
    {
        var Result = MessageBox.Show("Voulez-vous vraiment vous déconnecter ?", "Déconnexion ?", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        Disconnect();
    }
    
    private void Disconnect()
    {
        Session.SessionClear();
        Window loginWindow = new LoginWindow();
        loginWindow.Show();
        Application.Current.MainWindow = loginWindow;
        this.Close();
        loginWindow.WindowState = WindowState.Normal;
        loginWindow.Activate();
    }


    private void MinimizeWindow(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
    
    private void MaximizeWindow(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Normal)
        {
            WindowState = WindowState.Maximized;
        }
        else
        {
            WindowState = WindowState.Normal;
        }
    }
}