using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
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

        if (Session.IsAdmin)
        {
            // Brand radio buttons
            RadioButton BrandMenuButton = new RadioButton();
            BrandMenuButton.Content = "Marques";
            BrandMenuButton.Foreground = new SolidColorBrush(Colors.White);
            BrandMenuButton.Height = 50;
            BrandMenuButton.FontSize = 14;
            BrandMenuButton.Style = FindResource("MenuButtonTheme") as Style;
            BrandMenuButton.SetBinding(Button.CommandProperty, new Binding("ToBrandCommand"));
            NavBar.Children.Insert(1, BrandMenuButton);
            
            
            // Users radio buttons
            RadioButton UserMenuButton = new RadioButton();
            UserMenuButton.Content = "Utilisateurs";
            UserMenuButton.Foreground = new SolidColorBrush(Colors.White);
            UserMenuButton.Height = 50;
            UserMenuButton.FontSize = 14;
            UserMenuButton.Style = FindResource("MenuButtonTheme") as Style;
            UserMenuButton.SetBinding(Button.CommandProperty, new Binding("ToUserCommand"));
            NavBar.Children.Add(UserMenuButton);
        }
        // RadioButton UserMenuButton = new RadioButton();
        // UserMenuButton.Content = "Utilisateurs";
        // UserMenuButton.Foreground = new SolidColorBrush(Colors.White);
        // UserMenuButton.Height = 50;
        // UserMenuButton.FontSize = 14;
        // UserMenuButton.Style = FindResource("MenuButtonTheme") as Style;
        // Console.WriteLine(NavBar.Children.Add(UserMenuButton));

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