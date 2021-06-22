using System;
using System.Globalization;
using Xamarin.Forms;

namespace Library.Converters
{
    public class DateExpirationToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool expired = false;

            if (value is DateTime dateTime)
            {
                expired = DateTime.Now >= dateTime;
            }

            return expired;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
