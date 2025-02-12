using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TheAmazingExcavatorRentTool.Models;
using TheAmazingExcavatorRentTool.ViewModels;

namespace TheAmazingExcavatorRentTool.Views;

public partial class CustomersControl : UserControl
{
    
    private BitmapImage invalidIcon;
    private BitmapImage validIcon;
        
    private BitmapImage minusIcon;
    private BitmapImage plusIcon;

    private bool editValidFirstName;
    private bool editValidLastName;
    private bool editValidEmail;
    private bool editValidBirthDate;
        
    private bool addValidFirstName;
    private bool addValidLastName;
    private bool addValidEmail; 
    private bool addValidBirthDate;

    private CustomerVM _customerVm;
    public CustomersControl(CustomerVM customerVm)
    {
        _customerVm = customerVm;
        InitializeComponent();
        DataContext = _customerVm;
        
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
        txtAddFirstName.Clear();
        txtAddLastName.Clear();
        txtAddEmail.Clear();
        dpAddBirthDate.SetCurrentValue(DatePicker.SelectedDateProperty, DateTime.Now);
    }
        
    private void ClearEditForm(object sender, RoutedEventArgs e)
    {
        txtEditFirstName.Clear();
        txtEditLastName.Clear();
        txtEditEmail.Clear();
        dpEditBirthDate.SetCurrentValue(DatePicker.SelectedDateProperty, DateTime.Now);

    }
    
    private void ManageAddForm(object sender, RoutedEventArgs e)
    {
        if (AddForm.Visibility == Visibility.Hidden)
        {
            AddForm.Visibility = Visibility.Visible;
            manageAddBtnImage.Source = minusIcon;
            ManageEditFormBtn.IsEnabled = false;
            DeleteBtn.IsEnabled = false;

        }
        
        else if (AddForm.Visibility == Visibility.Visible)
        {
            AddForm.Visibility = Visibility.Hidden;
            manageAddBtnImage.Source = plusIcon;
            if (CustomerGrid.SelectedItems.Count > 0)
            {
                ManageEditFormBtn.IsEnabled = true;
                DeleteBtn.IsEnabled = true;
            }
            
            CustomerGrid.Focus();
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
            if (CustomerGrid.SelectedItems.Count > 0)
            {
                ManageAddFormBtn.IsEnabled = true;
                DeleteBtn.IsEnabled = true;
            }
            CustomerGrid.Focus();
        }
    }
    
    private void CanEdit()
    {
        if (editValidFirstName && editValidLastName && editValidEmail && editValidBirthDate)
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
        if (addValidFirstName && addValidLastName && addValidEmail && addValidBirthDate)
        {
            addBtn.IsEnabled = true;
        }
        else
        {
            addBtn.IsEnabled = false;
        }
    }
    
    private void ValidateDatePicker(object sender, SelectionChangedEventArgs e)
    {
        DatePicker sender_element = (DatePicker)sender;
        // datepicker in add form
        if (sender_element.Name == dpAddBirthDate.Name)
        {
            if (!isValidBirthDate(sender_element.SelectedDate))
            {
                AddValidBirthDateImg.Source = invalidIcon;
                addValidBirthDate = false;
                CanAdd();
            }
            AddValidBirthDateImg.Source = validIcon;
            addValidBirthDate = true;
            CanAdd();
        }
        // datepicker in edit form
        if (sender_element.Name == dpEditBirthDate.Name)
        {
            if (!isValidBirthDate(sender_element.SelectedDate))
            {
                EditValidBirthDateImg.Source = invalidIcon;
                editValidBirthDate = false;
                CanEdit();
            }
            EditValidBirthDateImg.Source = validIcon;
            editValidBirthDate = true;
            CanEdit();
        }
    }
    
    private void ValidateTextFields(object sender, TextChangedEventArgs e)
    {
        TextBox sender_element = (TextBox)sender;
        // First name input in add form
        if (sender_element.Name == txtAddFirstName.Name)
        {
            if (!isValidName(sender_element.Text) || sender_element.Text.Length == 0)
            {
                AddValidFirstNameImg.Source = invalidIcon;
                addValidFirstName = false;
                CanAdd();
                return;
            }
            AddValidFirstNameImg.Source = validIcon;
            addValidFirstName = true;
            CanAdd();
        }
        // Last name input in edit form
        if (sender_element.Name == txtEditFirstName.Name)
        {
            if (!isValidName(sender_element.Text) || sender_element.Text.Length == 0)
            {
                EditValidFirstNameImg.Source = invalidIcon;
                editValidFirstName = false;
                CanEdit();
                return;
            }
            EditValidFirstNameImg.Source = validIcon;
            editValidFirstName = true;
            CanEdit();
        }
        // Last name input in add form
        if (sender_element.Name == txtAddLastName.Name)
        {
            if (!isValidName(sender_element.Text) || sender_element.Text.Length == 0)
            {
                AddValidLastNameImg.Source = invalidIcon;
                addValidLastName = false;
                CanAdd();
                return;
            }
            AddValidLastNameImg.Source = validIcon;
            addValidLastName = true;
            CanAdd();
        }
        // Last name input in edit form
        if (sender_element.Name == txtEditLastName.Name)
        {
            if (!isValidName(sender_element.Text) || sender_element.Text.Length == 0)
            {
                EditValidLastNameImg.Source = invalidIcon;
                editValidLastName = false;
                CanEdit();
                return;
            }
            EditValidLastNameImg.Source = validIcon;
            editValidLastName = true;
            CanEdit();
        }
        // Email input in add form
        if (sender_element.Name == txtAddEmail.Name)
        {
            if (!isValidEmail(sender_element.Text) || sender_element.Text.Length == 0)
            {
                AddValidEmailImg.Source = invalidIcon;
                addValidEmail = false;
                CanAdd();
                return;
            }
            AddValidEmailImg.Source = validIcon;
            addValidEmail = true;
            CanAdd();
        }
        // Email input in edit form
        if (sender_element.Name == txtEditEmail.Name)
        {
            if (!isValidEmail(sender_element.Text) || sender_element.Text.Length == 0)
            {
                EditValidEmailImg.Source = invalidIcon;
                editValidEmail = false;
                CanEdit();
                return;
            }
            EditValidEmailImg.Source = validIcon;
            editValidEmail = true;
            CanEdit();
        }
    }
    
    private bool isValidName(string str)
    {
        Regex regex = new Regex("^[a-zA-Zéàèç-]{0,32}$");
        return regex.IsMatch(str);
    }
    
    private bool isValidEmail(string str)
    {
        Regex regex = new Regex("[a-z0-9.-]+\\.[a-z0-9.-]+@[a-zA-Z-]+\\.[a-zA-Z0-9-.]+");
        return regex.IsMatch(str);
    }

    private bool isValidBirthDate(DateTime? date)
    {
        // return (date.Year < DateTime.Now.Year && date.);
        if (date == null)
        {
            return false;
        }
        return date < DateTime.Now;
    }
    
    private void ApplyChanges(object sender, RoutedEventArgs e)
    {
        var Result = MessageBox.Show($"Voulez-vous vraiment apporter des modifications à ce client '{txtEditFirstName.Text} {txtEditLastName}'?", "Modifications ?", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        

        int id = (CustomerGrid.SelectedItem as Customer).CustomerId;
        String first_name = txtEditFirstName.Text;
        String last_name = txtEditLastName.Text;
        String email = txtEditEmail.Text;
        DateTime birth_date = (DateTime)dpEditBirthDate.SelectedDate;
        
        Customer customer_obj = new Customer(customer_id: id, first_name: first_name, last_name: last_name, email: email, birth_date: birth_date);
        _customerVm.Update(customer_obj);

        CustomerGrid.SelectedItem = customer_obj;
    }
    
}