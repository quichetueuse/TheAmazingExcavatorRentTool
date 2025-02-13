﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TheAmazingExcavatorRentTool.Services;
using TheAmazingExcavatorRentTool.ViewModels;
using TheAmazingExcavatorRentTool.Views;

namespace TheAmazingExcavatorRentTool;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    // initialising variables
    private BitmapImage invalidIcon;
    private BitmapImage validIcon;

    private MainWindowVM _mainwindowvm;
    
    
    public MainWindow()
    {
        
        InitializeComponent();
        
        _mainwindowvm = new MainWindowVM();
        DataContext = _mainwindowvm.ExcavatorVm;
        
        invalidIcon = new BitmapImage(new Uri(@"../Assets/redcross-icon.png", UriKind.Relative));
        validIcon = new BitmapImage(new Uri(@"../Assets/checkmark-icon2.png", UriKind.Relative));
        
        
        // Check if user is admin
        
        
        
        
        // Excavator view
        TabItem excavatorItem = modules_tabcontrol.Items[0] as TabItem;
        excavatorItem.Content = new ExcavatorsControl(_mainwindowvm.ExcavatorVm);
        
        // Brand view
        TabItem brandItem = modules_tabcontrol.Items[1] as TabItem;
        brandItem.Content = new BrandsControl(_mainwindowvm.BrandVm);
        
        // Customer view
        TabItem customerItem = modules_tabcontrol.Items[2] as TabItem;
        customerItem.Content = new CustomersControl(_mainwindowvm.CustomerVm);
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

    private void UIElement_OnGotFocus(object sender, RoutedEventArgs e)
    {
        var Result = MessageBox.Show("Voulez-vous vraiment vous déconnecter ?", "Déconnexion ?", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;

            
        Disconnect();
    }
}