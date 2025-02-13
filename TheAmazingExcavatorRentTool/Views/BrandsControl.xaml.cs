using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TheAmazingExcavatorRentTool.Models;
using TheAmazingExcavatorRentTool.ViewModels;

namespace TheAmazingExcavatorRentTool.Views;

public partial class BrandsControl : UserControl
{
    
    private BitmapImage invalidIcon;
    private BitmapImage validIcon;
        
    private BitmapImage minusIcon;
    private BitmapImage plusIcon;

    private bool editValidName;
    private bool editValidCreationYear;

        
    private bool addValidName;
    private bool addValidCreationYear;
    
    private BrandVM _brandVm;
    
    public BrandsControl(BrandVM brandVm)
    {
        _brandVm = brandVm;
        InitializeComponent();
        DataContext = _brandVm;
        
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
        txtAddName.SetCurrentValue(TextBox.TextProperty, "");
        txtAddCreationYear.SetCurrentValue(TextBox.TextProperty, DateTime.Now.Year.ToString());
    }
    
    private void ClearEditForm(object sender, RoutedEventArgs e)
    {
        txtEditName.SetCurrentValue(TextBox.TextProperty, "");
        txtEditCreationYear.SetCurrentValue(TextBox.TextProperty, DateTime.Now.Year.ToString());
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
            if (BrandGrid.SelectedItems.Count > 0)
            {
                ManageEditFormBtn.IsEnabled = true;
                DeleteBtn.IsEnabled = true;
            }
            
            BrandGrid.Focus();
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
            if (BrandGrid.SelectedItems.Count > 0)
            {
                ManageAddFormBtn.IsEnabled = true;
                DeleteBtn.IsEnabled = true;
            }
            BrandGrid.Focus();
        }
    }
    
    private void CanEdit()
    {
        if (editValidName && editValidCreationYear)
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
        if (addValidName && addValidCreationYear)
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
        // Name input in add form
        if (sender_element.Name == txtAddName.Name)
        {
            if (!isValidName(sender_element.Text) || sender_element.Text.Length == 0)
            {
                AddValidNameImg.Source = invalidIcon;
                addValidName = false;
                CanAdd();
                return;
            }
            AddValidNameImg.Source = validIcon;
            addValidName = true;
            CanAdd();
            return;
        }
        // Name input in edit form
        if (sender_element.Name == txtEditName.Name)
        {
            if (!isValidName(sender_element.Text) || sender_element.Text.Length == 0)
            {
                EditValidNameImg.Source = invalidIcon;
                editValidName = false;
                CanEdit();
                return;
            }
            EditValidNameImg.Source = validIcon;
            editValidName = true;
            CanEdit();
            return;
        }
        // Creation year input in add form
        if (sender_element.Name == txtAddCreationYear.Name)
        {
            if (!isValidYear(sender_element.Text) || sender_element.Text.Length == 0)
            {
                AddValidCreationYearImg.Source = invalidIcon;
                addValidCreationYear = false;
                CanAdd();
                return;
            }
            AddValidCreationYearImg.Source = validIcon;
            addValidCreationYear = true;
            CanAdd();
            return;
        }
        // Creation year input in edit form
        if (sender_element.Name == txtEditCreationYear.Name)
        {
            if (!isValidYear(sender_element.Text) || sender_element.Text.Length == 0)
            {
                EditValidCreationYearImg.Source = invalidIcon;
                editValidCreationYear = false;
                CanEdit();
                return;
            }
            EditValidCreationYearImg.Source = validIcon;
            editValidCreationYear = true;
            CanEdit();
            return;
        }
    }
        
    private bool isValidName(string str)
    {
        Regex regex = new Regex("^[a-zA-Zéàèç-]{1,32}$");
        return regex.IsMatch(str);
    }
    
    private bool isValidYear(string year)
    {
        if (year.Length == 0 || year.Length > 4 || year == "")
            return false;
        int int_year = Convert.ToInt32(year);

        if (int_year < 1950 || int_year > DateTime.Now.Year)
            return false;

        return true;
    }
    
    private void ApplyChanges(object sender, RoutedEventArgs e)
    {
        var Result = MessageBox.Show($"Voulez-vous vraiment apporter des modifications à cette marque '{txtEditName.Text} {txtEditCreationYear.Text}'?", "Modifications ?", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (Result == MessageBoxResult.No)
            return;
        
        int id = (BrandGrid.SelectedItem as Brand).BrandId;
        String name = txtEditName.Text;
        int creation_year = Convert.ToInt32(txtEditCreationYear.Text);
        
        Brand brand_obj = new Brand(brandId: id, name: name, creationYear: creation_year);
        _brandVm.Update(brand_obj);
        BrandGrid.SelectedItem = brand_obj;
    }
    
}