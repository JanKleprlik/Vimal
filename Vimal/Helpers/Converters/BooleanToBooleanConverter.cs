using System;
using Windows.UI.Xaml.Data;

namespace Vimal.Helpers.Converters
{
    /// <summary>
    /// NOT operation over boolean
    /// </summary>
    class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is Boolean))
            {
                throw new ArgumentException($"Argument is not of type boolean.");
            }
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
