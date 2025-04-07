using System.Windows;
using System.Windows.Input;
using TheAmazingExcavatorRentTool.Models;
using TheAmazingExcavatorRentTool.ViewModels;

namespace TheAmazingExcavatorRentTool.Views;

public partial class ViewBrandInfos : Window
{
    private Brand _selectedBrand;
    
    public ViewBrandInfos(Brand selectedBrand)
    {
        InitializeComponent();

        _selectedBrand = selectedBrand;
        FillViewForm();
    }


    private void FillViewForm()
    {
        TbName.Text = _selectedBrand.Name;
        TbCreationYear.Text = _selectedBrand.CreationYear.ToString();
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