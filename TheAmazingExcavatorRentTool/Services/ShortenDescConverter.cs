using System.Globalization;
using System.Windows.Data;

namespace TheAmazingExcavatorRentTool.Services;

public class ShortenDescConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int int_parameter = System.Convert.ToInt32(parameter);
        if (value.ToString().Length < int_parameter)
            return value;
        
        return value.ToString().Substring(0, int_parameter) + "...";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}