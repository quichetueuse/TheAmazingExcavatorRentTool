using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
    
    private bool isInfosViewOpened;

    private CustomerVM _customerVm;
    public CustomersControl()
    {
        InitializeComponent();
        
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
        
        isInfosViewOpened = false;
    }
    
    private void ClearAddForm(object sender, RoutedEventArgs e)
    {
        txtAddFirstName.SetCurrentValue(TextBox.TextProperty, "");
        txtAddLastName.SetCurrentValue(TextBox.TextProperty, "");
        txtAddEmail.SetCurrentValue(TextBox.TextProperty, "");
        dpAddBirthDate.SetCurrentValue(DatePicker.SelectedDateProperty, DateTime.Now);
    }
        
    private void ClearEditForm(object sender, RoutedEventArgs e)
    {
        txtEditFirstName.SetCurrentValue(TextBox.TextProperty, "");
        txtEditLastName.SetCurrentValue(TextBox.TextProperty, "");
        txtEditEmail.SetCurrentValue(TextBox.TextProperty, "");
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
            CanAdd();

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
                return;
            }
            
            AddValidBirthDateImg.Source = validIcon;
            addValidBirthDate = true;
            CanAdd();
            return;
        }
        // datepicker in edit form
        if (sender_element.Name == dpEditBirthDate.Name)
        {
            if (!isValidBirthDate(sender_element.SelectedDate))
            {
                EditValidBirthDateImg.Source = invalidIcon;
                editValidBirthDate = false;
                CanEdit();
                return;
            }
            EditValidBirthDateImg.Source = validIcon;
            editValidBirthDate = true;
            CanEdit();
            return;
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
            return;
        }
        // First name input in edit form
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
            return;
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
            return;
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
            return;
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
            return;
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
            return;
        }
    }
    
    private bool isValidName(string str)
    {
        Regex regex = new Regex("^[a-zA-Zéàèç-îôêâùïöä]{0,32}$");
        return regex.IsMatch(str);
    }
    
    private bool isValidEmail(string str)
    {
        Regex regex = new Regex("[a-z0-9.-]+\\.[a-z0-9.-]+@[a-zA-Z-]+\\.[a-zA-Z0-9-.]+");
        return regex.IsMatch(str);
    }

    private bool isValidBirthDate(DateTime? date)
    {
        if (date == null)
        {
            return false;
        }

        if (DateTime.Now.Year - date.Value.Year < 18)
        {
            return false;
        }
        return date < DateTime.Now;
    }
    
    private void ApplyChanges(object sender, RoutedEventArgs e)
    {
        // Check if data are not changed 
        Customer selectedCustomer = (Customer)CustomerGrid.SelectedItem as Customer;
        if (selectedCustomer.FirstName == txtEditFirstName.Text && 
            selectedCustomer.LastName == txtEditLastName.Text &&
            selectedCustomer.Email == txtEditEmail.Text &&
            selectedCustomer.BirthDate == dpEditBirthDate.SelectedDate)
        {
            MessageBox.Show("Aucunes modifications n'ont été faites!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        var Result = MessageBox.Show($"Voulez-vous vraiment apporter des modifications à ce client '{txtEditFirstName.Text} {txtEditLastName.Text}'?", "Modifications ?", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        

        int id = (CustomerGrid.SelectedItem as Customer).CustomerId;
        string first_name = txtEditFirstName.Text;
        string last_name = txtEditLastName.Text;
        string email = txtEditEmail.Text;
        DateTime birth_date = (DateTime)dpEditBirthDate.SelectedDate;
        
        Customer customer_obj = new Customer(customer_id: id, first_name: first_name, last_name: last_name, email: email, birth_date: birth_date);
        _customerVm.Update(customer_obj);

        CustomerGrid.SelectedItem = customer_obj;
    }

    private void setContextVM()
    {
        _customerVm = (CustomerVM)DataContext as CustomerVM;
    }
    
    private void CustomersControl_OnLoaded(object sender, RoutedEventArgs e)
    {
        setContextVM();
    }

    private void CustomerGrid_OnLoaded(object sender, RoutedEventArgs e)
    {
        resizeGrid();
    }

    private void resizeGrid()
    {
        foreach (DataGridColumn column in CustomerGrid.Columns)
        {
            double current_width = column.ActualWidth;
            column.MinWidth = current_width + 50;

        }
    }
    
    private void CustomerGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (isInfosViewOpened == false)
        {
            isInfosViewOpened = true;
            Customer selectedCustomer = (Customer)CustomerGrid.SelectedItem as Customer;
            ViewCustomerInfos ViewInfoWindow = new ViewCustomerInfos(selectedCustomer);
            ViewInfoWindow.Closed += ViewInfoWindowOnClosed;
            ViewInfoWindow.ShowDialog();
        }
    }

    private void ViewInfoWindowOnClosed(object? sender, EventArgs e)
    {
        isInfosViewOpened = false;
    }
}