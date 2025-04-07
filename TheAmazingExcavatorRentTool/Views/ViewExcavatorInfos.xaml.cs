using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TheAmazingExcavatorRentTool.Models;

namespace TheAmazingExcavatorRentTool.Views;

public partial class ViewExcavatorInfos : Window
{
    private Excavator _selectedExcavator;
    
    public ViewExcavatorInfos(Excavator selectedExcavator)
    {
        InitializeComponent();

        _selectedExcavator = selectedExcavator;
        FillViewForm();

    }
    
    
    private void FillViewForm()
    {
        txtName.Text = _selectedExcavator.Name;
        txtDesc.Text = _selectedExcavator.Description;
        txtMarque.Text = _selectedExcavator.Brand.Name;
        txtBucketLiters.Text = _selectedExcavator.BucketLiters.ToString();
        txtReleaseYear.Text = _selectedExcavator.ReleaseYear.ToString();
        txtPrice.Text = _selectedExcavator.DailyPrice.ToString();
        txtImagePath.Text = _selectedExcavator.PicturePath;
        imagePreview.Source = new BitmapImage(new Uri(_selectedExcavator.PicturePath, UriKind.Absolute));
    }
        
    private void DragWindow(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }
}