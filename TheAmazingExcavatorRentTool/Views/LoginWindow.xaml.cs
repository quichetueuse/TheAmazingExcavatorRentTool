using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using TheAmazingExcavatorRentTool.ViewModels;

namespace TheAmazingExcavatorRentTool.Views
{
    public partial class LoginWindow : Window
    {
        public LoginVM _loginvm;
        public LoginWindow()
        {
            InitializeComponent();
            _loginvm = new LoginVM();
            Console.WriteLine("user: Admin | password: Admin");
            Console.WriteLine("user: ELIOTB | password: azef");
        }
        
    }
}