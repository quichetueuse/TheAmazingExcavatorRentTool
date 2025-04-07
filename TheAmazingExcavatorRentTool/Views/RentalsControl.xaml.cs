using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TheAmazingExcavatorRentTool.Models;
using TheAmazingExcavatorRentTool.ViewModels;

namespace TheAmazingExcavatorRentTool.Views;

public partial class RentalsControl : UserControl
{
    private BitmapImage invalidIcon;
    private BitmapImage validIcon;
        
    private BitmapImage minusIcon;
    private BitmapImage plusIcon;

    private bool editValidCustomer;
    private bool editValidExcavator;
    private bool editValidStartDate;
    private bool editValidReturnDate;
        
    private bool addValidCustomer;
    private bool addValidExcavator;
    private bool addValidStartDate; 
    private bool addValidReturnDate;

    private RentalVM _rentalVm;
    
    private bool isInfosViewOpened;

    public RentalsControl()
    {
        
        InitializeComponent();
        
        invalidIcon = new BitmapImage(new Uri(@"../Assets/redcross-icon.png", UriKind.Relative));
        validIcon = new BitmapImage(new Uri(@"../Assets/checkmark-icon2.png", UriKind.Relative));
            
        minusIcon = new BitmapImage(new Uri(@"../Assets/minus-icon.png", UriKind.Relative));
        plusIcon = new BitmapImage(new Uri(@"../Assets/add-icon.png", UriKind.Relative));

        isInfosViewOpened = false;
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
        cbAddCustomer.SetCurrentValue(ComboBox.SelectedItemProperty, null);
        cbAddExcavator.SetCurrentValue(ComboBox.SelectedItemProperty, null);
        dpAddStartDate.SetCurrentValue(DatePicker.SelectedDateProperty, DateTime.Now.Date); // rend le champ invalide alors qu'il est censé toujours l'etre
        dpAddReturnDate.SetCurrentValue(DatePicker.SelectedDateProperty, DateTime.Now.Date);
    }
        
    private void ClearEditForm(object sender, RoutedEventArgs e)
    {
        cbEditCustomer.SetCurrentValue(ComboBox.SelectedItemProperty, null);
        cbEditExcavator.SetCurrentValue(ComboBox.SelectedItemProperty, null);
        dpEditStartDate.SetCurrentValue(DatePicker.SelectedDateProperty, DateTime.Now.Date);
        dpEditReturnDate.SetCurrentValue(DatePicker.SelectedDateProperty, DateTime.Now.Date);
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
            if (RentalGrid.SelectedItems.Count > 0)
            {
                ManageEditFormBtn.IsEnabled = true;
                DeleteBtn.IsEnabled = true;
            }
            
            RentalGrid.Focus();
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
            if (RentalGrid.SelectedItems.Count > 0)
            {
                ManageAddFormBtn.IsEnabled = true;
                DeleteBtn.IsEnabled = true;
            }
            RentalGrid.Focus();
        }
    }
    
    private void CanEdit()
    {
        if (editValidCustomer && editValidExcavator && editValidStartDate && editValidReturnDate)
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
        if (addValidCustomer && addValidExcavator && addValidStartDate && addValidReturnDate)
        {
            addBtn.IsEnabled = true;
        }
        else
        {
            addBtn.IsEnabled = false;
        }
    }
    
    
    private void ValidateComboboxFields(object sender, SelectionChangedEventArgs e)
    {
        ComboBox senderElement = (ComboBox)sender;

        // customer combobox in edit form
        if (senderElement.Name == cbEditCustomer.Name)
        {
            if (!isValidComboBoxSelection(senderElement))
            {
                EditValidCustomerImg.Source = invalidIcon;
                editValidCustomer = false;
                // _excavatorvm.Brand = cbEditBrand.SelectedItem as Brand;
                CanEdit();
                return;
            }
                
            EditValidCustomerImg.Source = validIcon;
            editValidCustomer = true;
            CanEdit();
        }
        
        // customer combobox in add form
        if (senderElement.Name == cbAddCustomer.Name)
        {
            if (!isValidComboBoxSelection(senderElement))
            {
                AddValidCustomerImg.Source = invalidIcon;
                addValidCustomer = false;
                // _excavatorvm.Brand = cbEditBrand.SelectedItem as Brand;
                CanAdd();
                return;
            }
                
            AddValidCustomerImg.Source = validIcon;
            addValidCustomer = true;
            CanAdd();
        }
            
        // excavator combobox in edit form
        if (senderElement.Name == cbEditExcavator.Name)
        {
            if (!isValidComboBoxSelection(senderElement))
            {
                EditValidExcavatorImg.Source = invalidIcon;
                editValidExcavator = false;
                CanEdit();
                return;
            }
                
            EditValidExcavatorImg.Source = validIcon;
            editValidExcavator = true;
            CanEdit();
        }
        
        // excavator combobox in add form
        if (senderElement.Name == cbAddExcavator.Name)
        {
            if (!isValidComboBoxSelection(senderElement))
            {
                AddValidExcavatorImg.Source = invalidIcon;
                addValidExcavator = false;
                CanAdd();
                return;
            }
                
            AddValidExcavatorImg.Source = validIcon;
            addValidExcavator = true;
            CanAdd();
        }
    }

    private void on_DateChanged(object sender, SelectionChangedEventArgs e)
    {
        DatePicker sender_element = (DatePicker)sender;
        ValidateDatePicker(sender_element);
    }
    private void ValidateDatePicker(object sender)
    {
        DatePicker sender_element = (DatePicker)sender;
        
        // start date in add form
        if (sender_element.Name == dpAddStartDate.Name && dpAddReturnDate.SelectedDate != null && sender_element.SelectedDate != null)
        {
            if (dpAddStartDate.SelectedDate.Value.Date > dpAddReturnDate.SelectedDate.Value.Date)
            {
                AddValidStartDateImg.Source = invalidIcon;
                addValidStartDate = false;
                CanAdd();
                return;
            }
            
            AddValidStartDateImg.Source = validIcon;
            addValidStartDate = true;
            CanAdd();
            return;
        }
        // start date in edit form
        if (sender_element.Name == dpEditStartDate.Name && dpEditReturnDate.SelectedDate != null && sender_element.SelectedDate != null)
        {
            if (dpEditStartDate.SelectedDate.Value.Date > dpEditReturnDate.SelectedDate.Value.Date)
            {
                EditValidStartDateImg.Source = invalidIcon;
                editValidStartDate = false;
                CanEdit();
                return;
            }
            EditValidStartDateImg.Source = validIcon;
            editValidStartDate = true;
            CanEdit();
            return;
        }
        
        // return date in add form
        if (sender_element.Name == dpAddReturnDate.Name && dpAddStartDate.SelectedDate != null && sender_element.SelectedDate != null)
        {
            if (dpAddReturnDate.SelectedDate.Value.Date < dpAddStartDate.SelectedDate.Value.Date)
            {
                AddValidReturnDateImg.Source = invalidIcon;
                addValidReturnDate = false;
                CanAdd();
                ValidateDatePicker(dpAddStartDate);
                return;
            }
            AddValidReturnDateImg.Source = validIcon;
            addValidReturnDate = true;
            CanAdd();
            ValidateDatePicker(dpAddStartDate);
            return;
        }
        
        // return date in edit form
        if (sender_element.Name == dpEditReturnDate.Name && dpEditStartDate.SelectedDate != null && sender_element.SelectedDate != null)
        {
            if (dpEditReturnDate.SelectedDate.Value.Date < dpEditStartDate.SelectedDate.Value.Date)
            {
                EditValidReturnDateImg.Source = invalidIcon;
                editValidReturnDate = false;
                CanEdit();
                ValidateDatePicker(dpEditStartDate);
                return;
            }
            EditValidReturnDateImg.Source = validIcon;
            editValidReturnDate = true;
            CanEdit();
            ValidateDatePicker(dpEditStartDate);
            return;
        }
    }
    
    
    private bool isValidComboBoxSelection(ComboBox element)
    {
        return element.SelectedItem != null;
    }
    
    
    private void ApplyChanges(object sender, RoutedEventArgs e)
    {
        // Check if data are not changed 
        Rental selectedRental = (Rental)RentalGrid.SelectedItem as Rental;
        if (selectedRental.Excavator == (Excavator)cbEditExcavator.SelectedItem &&
            selectedRental.Customer == (Customer)cbEditCustomer.SelectedItem &&
            selectedRental.StartDate == dpEditStartDate.SelectedDate &&
            selectedRental.ReturnDate == dpEditReturnDate.SelectedDate)
        {
            MessageBox.Show("Aucunes modifications n'ont été faites!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        

        int id = (RentalGrid.SelectedItem as Rental).RentalId;
        Excavator old_excavator = (RentalGrid.SelectedItem as Rental).Excavator;
        Customer customer = cbEditCustomer.SelectedItem as Customer;
        Excavator excavator = cbEditExcavator.SelectedItem as Excavator;
        DateTime start_date = (DateTime)dpEditStartDate.SelectedDate;
        DateTime return_date = (DateTime)dpEditReturnDate.SelectedDate;
        
        Rental rental_obj = new Rental(rentalId: id, customer: customer, excavator: excavator, startDate: start_date, returnDate: return_date, price: 0);
        _rentalVm.Update(rental_obj, old_excavator);
        RentalGrid.SelectedItem = rental_obj;
    }

    
    private void setContextVM()
    {
        _rentalVm = (RentalVM)DataContext as RentalVM;
    }
    private void RentalsControl_OnLoaded(object sender, RoutedEventArgs e)
    {
        setContextVM();
        resizeGrid();
    }
    
    
    private void resizeGrid()
    {
        foreach (DataGridColumn column in RentalGrid.Columns)
        {
            double current_width = column.ActualWidth;
            // Console.WriteLine($"Current width for column {column.Header} is {current_width} (final size is {current_width + 50})");
            column.Width = current_width + 100;

        }

        RentalGrid.Columns.Last().Width = new DataGridLength(1.0d, DataGridLengthUnitType.Star);
    }
    private void RentalGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (isInfosViewOpened == false)
        {
            isInfosViewOpened = true;
            Rental selectedRental = (Rental)RentalGrid.SelectedItem as Rental;
            ViewRentalInfos ViewInfoWindow = new ViewRentalInfos(selectedRental);
            ViewInfoWindow.Closed += ViewInfoWindowOnClosed;
            ViewInfoWindow.ShowDialog();
        }
    }

    private void ViewInfoWindowOnClosed(object? sender, EventArgs e)
    {
        isInfosViewOpened = false;
    }
    
    
}