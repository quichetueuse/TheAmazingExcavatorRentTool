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
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = login_tb_password .SecurePassword;
        }
        
    }
}