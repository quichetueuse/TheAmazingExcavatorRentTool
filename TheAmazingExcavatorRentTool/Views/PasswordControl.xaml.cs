using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace TheAmazingExcavatorRentTool.Views
{
    public partial class PasswordControl : UserControl
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(SecureString), typeof(PasswordControl));

        public SecureString Password
        {
            get { return (SecureString)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        
        public PasswordControl()
        {
            InitializeComponent();
            login_tb_password.PasswordChanged += OnPasswordChanged;
            // login_tb_password.LostFocus += OnLostFocus;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            // Password = ((PasswordBox)sender).SecurePassword;
            Password = login_tb_password .SecurePassword;
            // Console.WriteLine(login_tb_password.Password);
        }

        // private void OnLostFocus(object sender, RoutedEventArgs e)
        // {
        //     Console.WriteLine(login_tb_password.Password);
        // }
    }
}