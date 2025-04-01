using System.Globalization;
using System.Windows.Data;

namespace TheAmazingExcavatorRentTool.Services;

public class BooleanValueConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((bool)value == true)
            return "Oui";
        return "Non";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((string)value == "Oui")
            return true;
        return false;
    }
}