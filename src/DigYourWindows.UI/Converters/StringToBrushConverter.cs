using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DigYourWindows.UI.Converters
{
    /// <summary>
    /// 将颜色字符串（如 "#17a2b8"）转换为 SolidColorBrush
    /// </summary>
    public class StringToBrushConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string colorString && !string.IsNullOrWhiteSpace(colorString))
            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(colorString);
                    return new SolidColorBrush(color);
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
