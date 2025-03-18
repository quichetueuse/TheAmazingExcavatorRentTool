using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TheAmazingExcavatorRentTool.ViewModels;

namespace TheAmazingExcavatorRentTool
{
    public partial class LoginWindowNextGen : Window
    {
        public LoginVM _loginvm;
        public LoginWindowNextGen()
        {
            InitializeComponent();
            _loginvm = new LoginVM();
            Console.WriteLine("user: Admin | password: Admin");
            Console.WriteLine("user: ELIOTB | password: azef");
            login_tb_username.Focus();

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
        
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}