using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DVDRENT.Models;
using DVDRENT.ViewModels;
using Microsoft.Win32;

namespace DVDRENT.Views
{
    public partial class ExcavatorsControl : UserControl
    {
        private DvdVM _dvdvm;
        public ExcavatorsControl(DvdVM dvdvm)
        {
            InitializeComponent();
            _dvdvm = dvdvm;
            DataContext = _dvdvm;
        }
        //===== DVD variables =====
        private BitmapImage notValidIcon = new BitmapImage(new Uri(@"../Assets/redcross-icon.png", UriKind.Relative));
        private BitmapImage validIcon = new BitmapImage(new Uri(@"../Assets/checkmark-icon2.png", UriKind.Relative));
        private bool isDvdEditing = false;
        private bool isDvdAdding = false;
        
        private bool EditValidTitle = false;
        private bool EditValidDirector = false;
        private bool EditValidGenre = false;
        private bool EditValidReleaseYear = false;
        // private bool EditValidAvailability = false;
        
        private bool AddValidTitle = false;
        private bool AddValidDirector = false;
        private bool AddValidGenre = false;
        private bool AddValidReleaseYear = false;

        private string absoluteSolutionPath = "C:\\Users\\Eliot\\RiderProjects\\DVDRENT\\DVDRENT\\CoverImages\\";
        private string safeFileName;
        
        private void DvdGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isDvdAdding || isDvdEditing)
                return;

            btnDvdDelete.IsEnabled = true;
            btnDvdAppearEdit.IsEnabled = true;
        }
        
        private void DvdGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            var lastColumn = DvdGrid.Columns.Last();
            if (lastColumn == null) return;

            lastColumn.Width = new DataGridLength(1.0d, DataGridLengthUnitType.Star);
        }
        
        private void ClearDvdAdd(object sender, RoutedEventArgs e)
        {
            txtAddTitle.Clear();
            txtAddDirector.Clear();
            txtAddGenre.Clear();
            txtAddReleaseYear.Clear();
            
            DvdAddImagePreview.Source = null;
            txtAddImagePath.Text = "Aucun fichier séclectionné";
            safeFileName = null;
        }
        
        private void ClearDvdUpdate(object sender, RoutedEventArgs e)
        {
            txtUpdateTitle.Clear();
            txtUpdateDirector.Clear();
            txtUpdateGenre.Clear();
            txtUpdateReleaseYear.Clear();
            txtUpdateImagePath.Clear();
            DvdUpdateImagePreview.Source = null;
            txtUpdateImagePath.Text = "Aucun fichier séclectionné";
            safeFileName = null;

        }

        private void BtnDvdAppearAdd_OnClick(object sender, RoutedEventArgs e)
        {
            if (gbDvdAdd.Visibility == Visibility.Hidden)
            {
                isDvdAdding = true;
                imgDvdAppearAdd.Source = new BitmapImage(new Uri(@"../Assets/minus-icon.png", UriKind.Relative));
                btnDvdAppearEdit.IsEnabled = false;
                btnDvdDelete.IsEnabled = false;
                gbDvdAdd.Visibility = Visibility.Visible;

            }
            else if (gbDvdAdd.Visibility == Visibility.Visible)
            {
                isDvdAdding = false;
                imgDvdAppearAdd.Source = new BitmapImage(new Uri(@"../Assets/add-icon.png", UriKind.Relative));
                if (DvdGrid.SelectedItems.Count > 0)
                {
                    btnDvdAppearEdit.IsEnabled = true;
                    btnDvdDelete.IsEnabled = true;
                }
                
                gbDvdAdd.Visibility = Visibility.Hidden;
                safeFileName = null;
                DvdGrid.Focus();
            }
        }

        private void BtnDvdAppearEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (gbDvdUpdate.Visibility == Visibility.Hidden)
            {
                isDvdEditing = true;
                btnDvdAppearAdd.IsEnabled = false;
                btnDvdDelete.IsEnabled = false;
                gbDvdUpdate.Visibility = Visibility.Visible;

            }
            else if (gbDvdUpdate.Visibility == Visibility.Visible)
            {
                isDvdEditing = false;
                if (DvdGrid.SelectedItems.Count > 0)
                {
                    btnDvdAppearAdd.IsEnabled = true;
                    btnDvdDelete.IsEnabled = true;
                }
                gbDvdUpdate.Visibility = Visibility.Hidden;
                safeFileName = null;
                DvdGrid.Focus();
            }
        }
        private void CanEditDvd()
        {
            if (EditValidTitle && EditValidDirector && EditValidGenre && EditValidReleaseYear)
            {
                btnDvdApplyChanges.IsEnabled = true;
            }
            else
            {
                btnDvdApplyChanges.IsEnabled = false;
            }
        }

        private void CanAddDvd()
        {
            if (AddValidTitle && AddValidDirector && AddValidGenre && AddValidReleaseYear)
            {
                btnDvdAdd.IsEnabled = true;
            }
            else
            {
                btnDvdAdd.IsEnabled = false;
            }
        }
        
        
        
         private void TxtAddTitle_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (isValidTitle(txtAddTitle.Text))
            {
                DvdAddValidTitle.Source = validIcon;
                AddValidTitle = true;
            }
            else
            {
                DvdAddValidTitle.Source = notValidIcon;
                AddValidTitle = false;
            }
            CanAddDvd();
        }

        private void TxtAddDirector_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (isValidDirector(txtAddDirector.Text))
            {
                DvdAddValidDirector.Source = validIcon;
                AddValidDirector = true;
            }
            else
            {
                DvdAddValidDirector.Source = notValidIcon;
                AddValidDirector = false;
            }
            CanAddDvd();
        }

        private void TxtAddGenre_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (isValidGenre(txtAddGenre.Text))
            {
                DvdAddValidGenre.Source = validIcon;
                AddValidGenre = true;
            }
            else
            {
                DvdAddValidGenre.Source = notValidIcon;
                AddValidGenre = false;
            }
            CanAddDvd();
        }

        private void TxtAddReleaseYear_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (isValidYear(txtAddReleaseYear.Text))
            {
                if(Convert.ToInt32(txtAddReleaseYear.Text) > DateTime.Now.Year)
                    return;
                DvdAddValidReleaseYear.Source = validIcon;
                AddValidReleaseYear = true;
            }
            else
            {
                DvdAddValidReleaseYear.Source = notValidIcon;
                AddValidReleaseYear = false;
            }
            CanAddDvd();
        }

        private void TxtUpdateTitle_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (isValidTitle(txtUpdateTitle.Text))
            {
                DvdUpdateValidTitle.Source = validIcon;
                EditValidTitle = true;
            }
            else
            {
                DvdUpdateValidTitle.Source = notValidIcon;
                EditValidTitle = false;
            }
            CanEditDvd();
        }

        private void TxtUpdateDirector_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (isValidDirector(txtUpdateDirector.Text))
            {
                DvdUpdateValidDirector.Source = validIcon;
                EditValidDirector = true;
            }
            else
            {
                DvdUpdateValidDirector.Source = notValidIcon;
                EditValidDirector = false;
            }
            CanEditDvd();
        }

        private void TxtUpdateGenre_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (isValidGenre(txtUpdateGenre.Text))
            {
                DvdUpdateValidGenre.Source = validIcon;
                EditValidGenre = true;
            }
            else
            {
                DvdUpdateValidGenre.Source = notValidIcon;
                EditValidGenre = false;
            }
            CanEditDvd();
        }

        private void TxtUpdateReleaseYear_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (isValidYear(txtUpdateReleaseYear.Text))
            {
                if(Convert.ToInt32(txtUpdateReleaseYear.Text) > DateTime.Now.Year)
                    return;
                DvdUpdateValidReleaseYear.Source = validIcon;
                EditValidReleaseYear = true;
            }
            else
            {
                DvdUpdateValidReleaseYear.Source = notValidIcon;
                EditValidReleaseYear = false;
            }
            CanEditDvd();
        }
        
        private void BtnDvdApplyChanges_OnClick(object sender, RoutedEventArgs e)
        {
            var Result = MessageBox.Show($"Voulez-vous vraiment apporter des modifications au Dvd '{txtUpdateTitle.Text}'?", "Modifications ?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Result == MessageBoxResult.No)
                return;
            if (!File.Exists(safeFileName))
                File.Copy(txtUpdateImagePath.Text, safeFileName);
            Dvd dvd = DvdGrid.SelectedItem as Dvd;
            String titre = txtUpdateTitle.Text;
            String director = txtUpdateDirector.Text;
            String genre = txtUpdateGenre.Text;
            int release_year = Convert.ToInt32(txtUpdateReleaseYear.Text);
            bool availability = Convert.ToBoolean(txtUpdateIsAvailable.IsChecked);
            _dvdvm.UpdateDvd(new Dvd(dvdid: dvd.DVDId, title: titre, director: director, genre: genre, releaseyear: release_year, isAvailable: availability, coverImagePath: safeFileName));
        }
        private bool isValidYear(string str)
        {
            Regex regex = new Regex("^(?=.*?(19[56789]|20\\d{2}).*)\\d{4}$");
            return regex.IsMatch(str);
        }
        private bool isValidDirector(string str)
        {
            Regex regex = new Regex("[a-zA-Z]+");
            return regex.IsMatch(str);
        }
        
        private bool isValidTitle(string str)
        {
            Regex regex = new Regex("[a-zA-Z-.]+");
            return regex.IsMatch(str);
        }
        private bool isValidGenre(string str)
        {
            Regex regex = new Regex("[a-zA-Z]+");
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
                txtUpdateImagePath.Text = "Aucun fichier séclectionné";
                safeFileName = null;
                return;
            }
            txtUpdateImagePath.Text = dialog.FileName;
            DvdUpdateImagePreview.Source = new BitmapImage(new Uri(dialog.FileName));
            safeFileName = absoluteSolutionPath + dialog.SafeFileName;
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
                safeFileName = null;
                return;
            }
            txtAddImagePath.Text = dialog.FileName;
            DvdAddImagePreview.Source = new BitmapImage(new Uri(dialog.FileName));
            safeFileName = absoluteSolutionPath + dialog.SafeFileName;
            _dvdvm.CoverImagePath = safeFileName;
        }
        
    }
    
}