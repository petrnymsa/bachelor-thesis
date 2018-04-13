using System;
using System.Globalization;
using Xamarin.Forms;

namespace BachelorThesis.Converters
{
    public class BoolToBoldConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isRevelaed = (bool) value;

            return isRevelaed ? FontAttributes.Bold : FontAttributes.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
