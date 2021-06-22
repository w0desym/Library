using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Library.Converters
{
    public class IntegerDaysToStringPeriods : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<int> daysCollection)
            {
                return new List<string>(daysCollection.Select(x => ConvertToPeriod(x)));
            }
            else if (value is int days)
            {
                return ConvertToPeriod(days);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<string> daysCollection)
            {
                return new List<int>(daysCollection.Select(x => ConvertFromPeriod(x)));
            } 
            else if (value is string days)
            {
                return ConvertFromPeriod(days);
            }

            return null;
        }

        private string ConvertToPeriod(int days)
        {
            string result;

            if (days == -7)
            {
                result = "Last Week";
            }
            else if (days == -30)
            {
                result = "Last Month";
            }
            else
            {
                result = "Last 3 Months";
            }

            return result;
        }

        private int ConvertFromPeriod(string days)
        {
            int result;

            if (days == "Last Week")
            {
                result = -7;
            }
            else if (days == "Last Month")
            {
                result = -30;
            }
            else
            {
                result = -90;
            }

            return result;
        }
    }
}
