using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace eCRF.Models
{

    public class TableWidthConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            //return ConvertBack(value, targetType, parameter, culture);
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse(value.ToString() as String);
            
        }
    }

    public class DateConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack(value, targetType, parameter, culture);
        }

        object IValueConverter.Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return ""; // DateTime.Today.ToString("dd.MM.yyyy");
            string splitstring = value.ToString().Split(" ")[0];
            return DateTime.ParseExact(splitstring, "M/d/yyyy", null).ToString("dd.MM.yyyy");

        }
    }
}