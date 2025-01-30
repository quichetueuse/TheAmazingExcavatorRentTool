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
        public ExcavatorsControl(ExcavatorVM excavatorvm)
        {
            // InitializeComponent();
            _excavatorvm = excavatorvm;
            InitializeComponent();
            DataContext = _excavatorvm;
            
            invalidIcon = new BitmapImage(new Uri(@"../Assets/redcross-icon.png", UriKind.Relative));
            validIcon = new BitmapImage(new Uri(@"../Assets/checkmark-icon2.png", UriKind.Relative));
            
            minusIcon = new BitmapImage(new Uri(@"../Assets/minus-icon.png", UriKind.Relative));
            plusIcon = new BitmapImage(new Uri(@"../Assets/add-icon.png", UriKind.Relative));
            ImagesDir =
                "C:\\Users\\Eliot\\RiderProjects\\TheAmazingExcavatorRentTool\\TheAmazingExcavatorRentTool\\ExcavatorImages\\";
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
            txtAddName.Clear();
            txtAddDesc.Clear();
            cbAddBrand.SelectedItem = null;
            txtAddBucketLiters.Clear();
            txtAddReleaseYear.Clear();
            txtAddDailyPrice.Clear();
            
            // AddImagePreview.Source = null;
            // txtAddImagePath.Text = "Aucun fichier séclectionné";
            // addSafeFileName = null;
        }
        
        private void ClearEditForm(object sender, RoutedEventArgs e)
        {
            txtEditName.Clear();
            txtEditDesc.Clear();
            cbEditBrand.SelectedItem = null;
            txtEditBucketLiters.Clear();
            txtEditReleaseYear.Clear();
            checkBEditIsUsed.IsChecked = false;
            txtEditDailyPrice.Clear();
            
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
                if (!isValidName(sender_element.Text))
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
                if (!isValidName(sender_element.Text))
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
                if (!isValidDesc(sender_element.Text))
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
                if (!isValidDesc(sender_element.Text))
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
                if (!isValidNumber(sender_element.Text))
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
                if (!isValidNumber(sender_element.Text))
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
                if (!isValidNumber(sender_element.Text))
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
                if (!isValidNumber(sender_element.Text))
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
            var Result = MessageBox.Show($"Voulez-vous vraiment apporter des modifications à la pelleteuse '{txtEditName.Text}'?", "Modifications ?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.No)
                return;

            editSafeFileName = Path.GetFileName(txtEditImagePath.Text);
            
            if (!File.Exists(ImagesDir + editSafeFileName))
            {
                File.Copy(txtEditImagePath.Text, ImagesDir + editSafeFileName);
            }

            int id = (ExcavatorGrid.SelectedItem as Excavator).ExcavatorId;
            String name = txtEditName.Text;
            String desc = txtEditDesc.Text;
            Brand brand = cbEditBrand.SelectedItem as Brand;
            int bucket_liters = Convert.ToInt32(txtEditBucketLiters.Text);
            int release_year = Convert.ToInt32(txtEditReleaseYear.Text);
            bool is_used = Convert.ToBoolean(checkBEditIsUsed.IsChecked);
            int daily_price = Convert.ToInt32(txtEditDailyPrice.Text);
            Excavator excav_obj = new Excavator(excavatorid: id, name: name, description: desc, brand: brand, 
                bucket_liters: bucket_liters, releaseyear: release_year, isused: is_used, dailyprice: daily_price, picturepath: ImagesDir + editSafeFileName);
            _excavatorvm.UpdateExcavator(excav_obj);
        }
        private bool isValidYear(string str)
        {
            Regex regex = new Regex("^(?=.*?(19[56789]|20\\d{2}).*)\\d{4}$");
            return regex.IsMatch(str);
        }
        private bool isValidName(string str)
        {
            Regex regex = new Regex("^[a-zA-Z0-9éàèç. ,]{0,32}$");
            return regex.IsMatch(str);
        }

        private bool isValidComboBoxSelection(ComboBox element)
        {
            return element.SelectedItem != null;
        }
        
        private bool isValidDesc(string str)
        {
            Regex regex = new Regex("^[a-zA-Z0-9éàèç. ,]{0,128}$");
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
                txtEditImagePath.Text = "Aucun fichier séclectionné";
                editSafeFileName = null;
                return;
            }
            txtEditImagePath.Text = dialog.FileName;
            EditImagePreview.Source = new BitmapImage(new Uri(dialog.FileName));
            editSafeFileName = ImagesDir + dialog.SafeFileName;
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
                return;
            }
            txtAddImagePath.Text = dialog.FileName;
            AddImagePreview.Source = new BitmapImage(new Uri(dialog.FileName));
            addSafeFileName = ImagesDir + dialog.SafeFileName;
            _excavatorvm.PicturePath = addSafeFileName;
        }
        
    }
    
}