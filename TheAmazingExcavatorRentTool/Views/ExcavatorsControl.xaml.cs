using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TheAmazingExcavatorRentTool.Models;
using TheAmazingExcavatorRentTool.ViewModels;
using Microsoft.Win32;

namespace TheAmazingExcavatorRentTool.Views
{
    public partial class ExcavatorsControl : UserControl
    {    
        
        // initialising variables
        private BitmapImage invalidIcon;
        private BitmapImage validIcon;
        
        private BitmapImage minusIcon;
        private BitmapImage plusIcon;

        private bool editValidName;
        private bool editValidDesc;
        private bool editValidBrand;
        private bool editValidBucketLiters;
        private bool editValidReleaseYear;
        private bool editValidDailyPrice;
        
        private bool addValidName;
        private bool addValidDesc;
        private bool addValidBrand; 
        private bool addValidBucketLiters;
        private bool addValidReleaseYear;
        private bool addValidDailyPrice;

        private string ImagesDir;
        private string editSafeFileName;
        private string addSafeFileName;
        

        
        private ExcavatorVM _excavatorvm;
        public ExcavatorsControl()
        {
            // InitializeComponent();
            InitializeComponent();
            
            invalidIcon = new BitmapImage(new Uri(@"../Assets/redcross-icon.png", UriKind.Relative));
            validIcon = new BitmapImage(new Uri(@"../Assets/checkmark-icon2.png", UriKind.Relative));
            
            minusIcon = new BitmapImage(new Uri(@"../Assets/minus-icon.png", UriKind.Relative));
            plusIcon = new BitmapImage(new Uri(@"../Assets/add-icon.png", UriKind.Relative));
            ImagesDir =
                "C:\\Users\\Eliot\\Desktop\\WPF next gen\\TheAmazingExcavatorRentTool_v2\\TheAmazingExcavatorRentTool\\ExcavatorImages\\";
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
            txtAddDesc.SetCurrentValue(TextBox.TextProperty, "");
            cbAddBrand.SetCurrentValue(ComboBox.SelectedItemProperty, null);
            // cbAddBrand.SelectedItem = null;
            txtAddBucketLiters.SetCurrentValue(TextBox.TextProperty, "0");
            txtAddReleaseYear.SetCurrentValue(TextBox.TextProperty, "0");
            txtAddDailyPrice.SetCurrentValue(TextBox.TextProperty, "0");

            AddImagePreview.SetCurrentValue(Image.SourceProperty, null);
            txtAddImagePath.SetCurrentValue(TextBox.TextProperty, "Aucun fichier séclectionné");
            addSafeFileName = null;
        }
        
        private void ClearEditForm(object sender, RoutedEventArgs e)
        {
            txtEditName.SetCurrentValue(TextBox.TextProperty, "");
            txtEditDesc.SetCurrentValue(TextBox.TextProperty, "");
            cbEditBrand.SetCurrentValue(ComboBox.SelectedItemProperty, null);
            txtEditBucketLiters.SetCurrentValue(TextBox.TextProperty, "");
            txtEditReleaseYear.SetCurrentValue(TextBox.TextProperty, "");
            // checkBEditIsUsed.IsChecked = false;
            // checkBEditIsUsed.SetCurrentValue(CheckBox.IsCheckedProperty, false);
            txtEditDailyPrice.SetCurrentValue(TextBox.TextProperty, "");
            
            // EditImagePreview.Source = null;
            // txtEditImagePath.Text = "Aucun fichier séclectionné";
            // editSafeFileName = null;

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
                if (ExcavatorGrid.SelectedItems.Count > 0)
                {
                    ManageEditFormBtn.IsEnabled = true;
                    DeleteBtn.IsEnabled = true;
                }
                
                addSafeFileName = null;
                ExcavatorGrid.Focus();
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
                if (ExcavatorGrid.SelectedItems.Count > 0)
                {
                    ManageAddFormBtn.IsEnabled = true;
                    DeleteBtn.IsEnabled = true;
                }
                editSafeFileName = null;
                ExcavatorGrid.Focus();
            }
        }
        private void CanEdit()
        {
            if (editValidName && editValidDesc && editValidBrand && editValidBucketLiters && editValidReleaseYear && editValidDailyPrice)
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
            if (addValidName && addValidDesc && addValidBrand && addValidBucketLiters && addValidReleaseYear && addValidDailyPrice)
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

            if (senderElement.Name == cbEditBrand.Name)
            {
                if (!isValidComboBoxSelection(senderElement))
                {
                    EditValidBrandImg.Source = invalidIcon;
                    editValidBrand = false;
                    _excavatorvm.Brand = cbEditBrand.SelectedItem as Brand;
                    CanEdit();
                    return;
                }
                
                EditValidBrandImg.Source = validIcon;
                editValidBrand = true;
                CanEdit();
            }
            
            if (senderElement.Name == cbAddBrand.Name)
            {
                if (!isValidComboBoxSelection(senderElement))
                {
                    AddValidBrandImg.Source = invalidIcon;
                    addValidBrand = false;
                    CanAdd();
                    return;
                }
                
                AddValidBrandImg.Source = validIcon;
                addValidBrand = true;
                CanAdd();
            }
        }
        
        private void ValidateTextFields(object sender, TextChangedEventArgs e)
        {
            TextBox sender_element = (TextBox)sender;
            //Name input in add form
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
            }
            // Desc input in add form
            if (sender_element.Name == txtAddDesc.Name)
            {
                if (!isValidDesc(sender_element.Text) || sender_element.Text.Length == 0)
                {
                    AddValidDescImg.Source = invalidIcon;
                    addValidDesc = false;
                    CanAdd();
                    return;
                }
                AddValidDescImg.Source = validIcon;
                addValidDesc = true;
                CanAdd();
            }
            // Desc input in edit form
            if (sender_element.Name == txtEditDesc.Name)
            {
                if (!isValidDesc(sender_element.Text) || sender_element.Text.Length == 0)
                {
                    EditValidDescImg.Source = invalidIcon;
                    editValidDesc = false;
                    CanEdit();
                    return;
                }
                EditValidDescImg.Source = validIcon;
                editValidDesc = true;
                CanEdit();
            }
            // Bucket liters input in add form
            if (sender_element.Name == txtAddBucketLiters.Name)
            {
                if (!isValidNumber(sender_element.Text) || sender_element.Text == "0")
                {
                    AddValidBucketLitersImg.Source = invalidIcon;
                    addValidBucketLiters = false;
                    CanAdd();
                    return;
                }
                AddValidBucketLitersImg.Source = validIcon;
                addValidBucketLiters = true;
                CanAdd();
            }
            // Bucket liters input in edit form
            if (sender_element.Name == txtEditBucketLiters.Name)
            {
                if (!isValidNumber(sender_element.Text) || sender_element.Text == "0")
                {
                    EditValidBucketLitersImg.Source = invalidIcon;
                    editValidBucketLiters = false;
                    CanEdit();
                    return;
                }
                EditValidBucketLitersImg.Source = validIcon;
                editValidBucketLiters = true;
                CanEdit();
            }
            // Release year input in add form
            if (sender_element.Name == txtAddReleaseYear.Name)
            {
                
                if (!isValidYear(sender_element.Text) || Convert.ToInt32(sender_element.Text) > DateTime.Now.Year)
                {
                    AddValidReleaseYearImg.Source = invalidIcon;
                    addValidReleaseYear = false;
                    CanAdd();
                    return;
                }
                
                
                AddValidReleaseYearImg.Source = validIcon;
                addValidReleaseYear = true;
                CanAdd();
            }
            // Release year input in edit form
            if (sender_element.Name == txtEditReleaseYear.Name)
            {
                if (!isValidYear(sender_element.Text) || Convert.ToInt32(sender_element.Text) > DateTime.Now.Year)
                {
                    EditValidReleaseYearImg.Source = invalidIcon;
                    editValidReleaseYear = false;
                    CanEdit();
                    return;
                }
                EditValidReleaseYearImg.Source = validIcon;
                editValidReleaseYear = true;
                CanEdit();
            }
            
            // Daily price input in add form
            if (sender_element.Name == txtAddDailyPrice.Name)
            {
                if (!isValidNumber(sender_element.Text) || sender_element.Text == "0")
                {
                    AddValidDailyPriceImg.Source = invalidIcon;
                    addValidDailyPrice = false;
                    CanAdd();
                    return;
                }
                AddValidDailyPriceImg.Source = validIcon;
                addValidDailyPrice = true;
                CanAdd();
            }
            
            // Daily price input in add form
            if (sender_element.Name == txtEditDailyPrice.Name)
            {
                if (!isValidNumber(sender_element.Text) || sender_element.Text == "0")
                {
                    EditValidDailyPriceImg.Source = invalidIcon;
                    editValidDailyPrice = false;
                    CanEdit();
                    return;
                }
                EditValidDailyPriceImg.Source = validIcon;
                editValidDailyPrice = true;
                CanEdit();
            }
        }
        
        
        private void ApplyChanges(object sender, RoutedEventArgs e)
        {
            // Check if data are not changed 
            Excavator selectedExcav = (Excavator)ExcavatorGrid.SelectedItem as Excavator;
            if (selectedExcav.Name == txtEditName.Text && selectedExcav.Description == txtEditDesc.Text &&
                selectedExcav.Brand == cbEditBrand.SelectedItem &&
                selectedExcav.BucketLiters == Convert.ToInt32(txtEditBucketLiters.Text) &&
                selectedExcav.ReleaseYear == Convert.ToInt32(txtEditReleaseYear.Text) &&
                selectedExcav.DailyPrice == Convert.ToInt32(txtEditDailyPrice.Text) &&
                selectedExcav.PicturePath == txtEditImagePath.Text)
            {
                MessageBox.Show("Aucunes modifications n'ont été faites!", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            var Result = MessageBox.Show($"Voulez-vous vraiment apporter des modifications à la pelleteuse '{txtEditName.Text}'?", "Modifications ?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.No)
                return;

            editSafeFileName = Path.GetFileName(txtEditImagePath.Text);
            
            // if file is not present in local image dir and file is set
            if (!File.Exists(ImagesDir + editSafeFileName) && editSafeFileName != "Aucun fichier séclectionné")
            {
                File.Copy(txtEditImagePath.Text, ImagesDir + editSafeFileName);
            }

            int id = (ExcavatorGrid.SelectedItem as Excavator).ExcavatorId;
            String name = txtEditName.Text;
            String desc = txtEditDesc.Text;
            Brand brand = cbEditBrand.SelectedItem as Brand;
            int bucket_liters = Convert.ToInt32(txtEditBucketLiters.Text);
            int release_year = Convert.ToInt32(txtEditReleaseYear.Text);
            // bool is_used = Convert.ToBoolean(checkBEditIsUsed.IsChecked);
            int daily_price = Convert.ToInt32(txtEditDailyPrice.Text);
            string? file_path;
            if (editSafeFileName == String.Empty || editSafeFileName == "Aucun fichier séclectionné")
            {
                file_path = "Aucun fichier séclectionné";
            }
            else
            {
                file_path = ImagesDir + editSafeFileName;
            }
            Excavator excav_obj = new Excavator(excavatorid: id, name: name, description: desc, brand: brand, 
                bucket_liters: bucket_liters, releaseyear: release_year, isused: false, dailyprice: daily_price, picturepath: file_path);
            _excavatorvm.Update(excav_obj);

            ExcavatorGrid.SelectedItem = excav_obj;
        }
        private bool isValidYear(string year)
        {
            if (year.Length == 0 || year.Length > 4 || year == "" || !IsDigitsOnly(year))
                return false;
            int int_year = Convert.ToInt32(year);

            if (int_year < 1925 || int_year > DateTime.Now.Year)
                return false;

            return true;
            // Regex regex = new Regex("^(?=.*?(19[56789]|20\\d{2}).*)\\d{4}$");
            // return regex.IsMatch(str);
        }
        
        private bool IsDigitsOnly(string year)
        {
            foreach (char c in year)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
        private bool isValidName(string str)
        {
            Regex regex = new Regex("^[a-zA-Z0-9éàèç. ,îôêâùïöä]{0,32}$");
            return regex.IsMatch(str);
        }

        private bool isValidComboBoxSelection(ComboBox element)
        {
            return element.SelectedItem != null;
        }
        
        private bool isValidDesc(string str)
        {
            Regex regex = new Regex("^[a-zA-Z0-9éàèç. ,îôêâùïöëä'\\%É]{0,512}$");
            return regex.IsMatch(str);
        }
        private bool isValidNumber(string str)
        {
            Regex regex = new Regex("^[+-]?([0-9]+([.][0-9]*)?|[.][0-9]+)$");
            return regex.IsMatch(str);
        }

        private void BtnUpdateBrowseImage_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                Filter = "Images (*.jpg,*.png)|*.jpg;*.png|All Files(*.*)|*.*"
            };

            if (dialog.ShowDialog() != true)
            {
                txtEditImagePath.SetCurrentValue(TextBox.TextProperty, "Aucun fichier séclectionné");
                editSafeFileName = null;
                // EditImagePreview.Source = null;
                EditImagePreview.SetCurrentValue(Image.SourceProperty, null);
                return;
            }
            // txtEditImagePath.Text = dialog.FileName;
            txtEditImagePath.SetCurrentValue(TextBox.TextProperty, dialog.FileName);
            // EditImagePreview.Source = new BitmapImage(new Uri(dialog.FileName));
            EditImagePreview.SetCurrentValue(Image.SourceProperty, new BitmapImage(new Uri(dialog.FileName)));
            editSafeFileName = ImagesDir + dialog.SafeFileName;
            
            
            // Binding myBinding = new Binding();
            // myBinding.Source = _excavatorvm;
            // myBinding.Path = new PropertyPath("PicturePath");
            // myBinding.Mode = BindingMode.TwoWay;
            // myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            // BindingOperations.SetBinding(txtEditImagePath, TextBox.TextProperty, myBinding);
            // Console.WriteLine(safeFileName);
        }

        private void BtnAddBrowseImage_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                Filter = "Images (*.jpg,*.png)|*.jpg;*.png|All Files(*.*)|*.*"
            };

            if (dialog.ShowDialog() != true)
            {
                txtAddImagePath.Text = "Aucun fichier séclectionné";
                addSafeFileName = null;
                _excavatorvm.PicturePath = null;
                AddImagePreview.Source = null;
                return;
            }

            if (!File.Exists(ImagesDir + dialog.SafeFileName))
            {
                File.Copy(dialog.FileName, ImagesDir + dialog.SafeFileName);
            }
            txtAddImagePath.Text = dialog.FileName;
            AddImagePreview.Source = new BitmapImage(new Uri(dialog.FileName));
            addSafeFileName = ImagesDir + dialog.SafeFileName;
            _excavatorvm.PicturePath = addSafeFileName;
        }


        private void setContextVM()
        {
            _excavatorvm = (ExcavatorVM)DataContext as ExcavatorVM;
        }


        private void ExcavatorsControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            setContextVM();
            
            // resizeGrid();
            foreach (DataGridColumn column in ExcavatorGrid.Columns)
            {
                if (column.Header.ToString() == "Description")
                {
                    // Console.Write("Actual width:"+ column.ActualWidth);
                    column.Width = new DataGridLength(200, DataGridLengthUnitType.Pixel);
                    continue;
                }
                column.Width =  100;
            }
            DataGridColumn lastColumn = ExcavatorGrid.Columns.Last();
            if (lastColumn == null) return;
            lastColumn.Width = new DataGridLength(1.0d, DataGridLengthUnitType.Star);
        }

        private void changeDatagridColumnsSize(object sender, SizeChangedEventArgs e)
        {
            
            
            // Console.Write("Changed");
            // foreach (DataGridColumn column in ExcavatorGrid.Columns)
            // {
            //     if (column.Header.ToString() == "Description")
            //     {
            //         column.Width = new DataGridLength(200, DataGridLengthUnitType.Pixel);
            //         continue;
            //     }
            //
            //     if (column.Header.ToString() == "Image de la pelleteuse")
            //     {
            //         column.Width = new DataGridLength(1.0, DataGridLengthUnitType.Star);
            //     }
            //
            //     column.Width = new DataGridLength(1.0, DataGridLengthUnitType.Auto);
            // }
        }
    }
    
    
}