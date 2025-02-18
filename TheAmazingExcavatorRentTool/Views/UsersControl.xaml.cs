using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TheAmazingExcavatorRentTool.Models;
using TheAmazingExcavatorRentTool.Services;
using TheAmazingExcavatorRentTool.ViewModels;

namespace TheAmazingExcavatorRentTool.Views;

public partial class UsersControl : UserControl
{
    
    private BitmapImage invalidIcon;
    private BitmapImage validIcon;
        
    private BitmapImage minusIcon;
    private BitmapImage plusIcon;

    private bool editValidUsername;
    private bool editvalidPassword;
        
    private bool addValidUsername;
    private bool addValidPassword;

    private UserVM _userVm;

    private PasswordManager passwordManager;
    
    
    public UsersControl(UserVM userVm)
    {
        _userVm = userVm;
        InitializeComponent();
        DataContext = _userVm;

        passwordManager = new PasswordManager();
        
        invalidIcon = new BitmapImage(new Uri(@"../Assets/redcross-icon.png", UriKind.Relative));
        validIcon = new BitmapImage(new Uri(@"../Assets/checkmark-icon2.png", UriKind.Relative));
            
        minusIcon = new BitmapImage(new Uri(@"../Assets/minus-icon.png", UriKind.Relative));
        plusIcon = new BitmapImage(new Uri(@"../Assets/add-icon.png", UriKind.Relative));
    }

    private void on_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
            
        if (EditForm.IsVisible || AddForm.IsVisible)
            return;

        DeleteBtn.IsEnabled = true;
        ManageEditFormBtn.IsEnabled = true;
    }
    
    private void ClearAddForm(object sender, RoutedEventArgs e)
    {
        txtAddUsername.SetCurrentValue(TextBox.TextProperty, "");
        txtAddPassword.SetCurrentValue(TextBox.TextProperty, "");
        checkBAddIsAdmin.SetCurrentValue(CheckBox.IsCheckedProperty, false);
    }
        
    private void ClearEditForm(object sender, RoutedEventArgs e)
    {
        txtEditUsername.SetCurrentValue(TextBox.TextProperty, "");
        txtEditPassword.SetCurrentValue(TextBox.TextProperty, "");
        checkBEditIsAdmin.SetCurrentValue(CheckBox.IsCheckedProperty, false);

    }
    
    private void ManageAddForm(object sender, RoutedEventArgs e)
    {
        if (AddForm.Visibility == Visibility.Hidden)
        {
            AddForm.Visibility = Visibility.Visible;
            manageAddBtnImage.Source = minusIcon;
            ManageEditFormBtn.IsEnabled = false;
            DeleteBtn.IsEnabled = false;
            CanAdd();

        }
        
        else if (AddForm.Visibility == Visibility.Visible)
        {
            AddForm.Visibility = Visibility.Hidden;
            manageAddBtnImage.Source = plusIcon;
            if (UserGrid.SelectedItems.Count > 0)
            {
                ManageEditFormBtn.IsEnabled = true;
                DeleteBtn.IsEnabled = true;
            }
            
            UserGrid.Focus();
        }
    }

    private void ManageEditForm(object sender, RoutedEventArgs e)
    {
        if (EditForm.Visibility == Visibility.Hidden)
        {
            EditForm.Visibility = Visibility.Visible;
            ManageAddFormBtn.IsEnabled = false;
            DeleteBtn.IsEnabled = false;

        }
        else if (EditForm.Visibility == Visibility.Visible)
        {
            EditForm.Visibility = Visibility.Hidden;
            if (UserGrid.SelectedItems.Count > 0)
            {
                ManageAddFormBtn.IsEnabled = true;
                DeleteBtn.IsEnabled = true;
            }
            UserGrid.Focus();
        }
    }
    
    private void CanEdit()
    {
        if (editValidUsername && editvalidPassword)
        {
            editBtn.IsEnabled = true;
        }
        else
        {
            editBtn.IsEnabled = false;
        }
    }

    private void CanAdd()
    {
        if (addValidUsername && addValidPassword)
        {
            addBtn.IsEnabled = true;
        }
        else
        {
            addBtn.IsEnabled = false;
        }
    }
    
    private void ValidateTextFields(object sender, TextChangedEventArgs e)
    {
        TextBox sender_element = (TextBox)sender;
        // username input in add form
        if (sender_element.Name == txtAddUsername.Name)
        {
            if (!isValidName(sender_element.Text) || sender_element.Text.Length == 0)
            {
                AddValidUsernameImg.Source = invalidIcon;
                addValidUsername = false;
                CanAdd();
                return;
            }
            AddValidUsernameImg.Source = validIcon;
            addValidUsername = true;
            CanAdd();
            return;
        }
        // username input in edit form
        if (sender_element.Name == txtEditUsername.Name)
        {
            if (!isValidName(sender_element.Text) || sender_element.Text.Length == 0)
            {
                EditValidUsernameImg.Source = invalidIcon;
                editValidUsername = false;
                CanEdit();
                return;
            }
            EditValidUsernameImg.Source = validIcon;
            editValidUsername = true;
            CanEdit();
            return;
        }
        // password input in add form
        if (sender_element.Name == txtAddPassword.Name)
        {
            if (!isValidPassword(sender_element.Text) || sender_element.Text.Length == 0)
            {
                AddValidPasswordImg.Source = invalidIcon;
                addValidPassword = false;
                CanAdd();
                return;
            }
            AddValidPasswordImg.Source = validIcon;
            addValidPassword = true;
            CanAdd();
            return;
        }
        // password input in edit form
        if (sender_element.Name == txtEditPassword.Name)
        {
            if (!isValidPassword(sender_element.Text) || sender_element.Text.Length == 0)
            {
                EditValidPasswordImg.Source = invalidIcon;
                editvalidPassword = false;
                CanEdit();
                return;
            }
            EditValidPasswordImg.Source = validIcon;
            editvalidPassword = true;
            CanEdit();
            return;
        }
    }

    private bool isValidName(string str)
    {
        Regex regex = new Regex("^[a-zA-Zéàèç-]{0,32}$");
        return regex.IsMatch(str);
    }
    
    private bool isValidPassword(string password)
    {
        Regex regex = new Regex("^(?=.{4,}[a-z])(?=.{4,}[A-Z])(?=.{4,}\\d)(?=.{4,}[@$!%*?&_-])[A-Za-z\\d@$!%*?&_-]{12,}$");
        return regex.IsMatch(password);
    }
    
    
    private void ApplyChanges(object sender, RoutedEventArgs e)
    {
        // Check if data are not changed 
        User selectedUser = (User)UserGrid.SelectedItem as User;
        if (selectedUser.Username == txtEditUsername.Text && selectedUser.Password == passwordManager.HashPassword(txtEditPassword.Text))
        {
            MessageBox.Show("Aucunes modifications n'ont été faites!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        var Result = MessageBox.Show($"Voulez-vous vraiment apporter des modifications à cet utilisateur '{txtEditUsername.Text}'?", "Modifications ?", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        

        int id = selectedUser.UserId;
        string username = txtEditUsername.Text;
        string password = txtEditPassword.Text;
        bool is_admin = Convert.ToBoolean(checkBEditIsAdmin.IsChecked);
        
        User user_obj = new User(userid: id, username: username, password: password, isAdmin: is_admin);
        _userVm.Update(user_obj);

        UserGrid.SelectedItem = user_obj;
    }
    
    private void CreatePasswordBtn_OnClick(object sender, RoutedEventArgs e)
    {
        string password = passwordManager.GeneratePassword(14, 4);
        Button sender_element = (Button)sender;
        if (sender_element.Name == editCreatePasswordBtn.Name)
        {
            txtEditPassword.SetCurrentValue(TextBox.TextProperty, password);
        }
        else
        {
            txtAddPassword.SetCurrentValue(TextBox.TextProperty, password);
        }
    }
    
}