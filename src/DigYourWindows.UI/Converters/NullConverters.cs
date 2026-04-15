using System.Globalization;
using System.Windows.Data;

namespace DigYourWindows.UI.Converters
{
    /// <summary>
    /// Converts null values to empty string and handles null-safe string display.
    /// </summary>
    public class NullToEmptyStringConverter : IValueConverter
    {
        public object Convert(object? value, System.Type targetType, object? parameter, CultureInfo culture)
        {
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object? value, System.Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }

    /// <summary>
    /// Converts null values to a placeholder text (specified as parameter).
    /// </summary>
    public class NullToPlaceholderConverter : IValueConverter
    {
        public object Convert(object? value, System.Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null || (value is string s && string.IsNullOrEmpty(s)))
            {
                return parameter?.ToString() ?? "—";
            }

            return value;
        }

        public object ConvertBack(object? value, System.Type targetType, object? parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }

    /// <summary>
    /// Converts nullable double to formatted string with specified format (parameter).
    /// </summary>
    public class NullableDoubleToStringConverter : IValueConverter
    {
        public object Convert(object? value, System.Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                var format = parameter?.ToString() ?? "F0";
                return d.ToString(format, culture);
            }

            if (value is null)
            {
                return "—";
            }

            return value.ToString() ?? string.Empty;
        }

        public object ConvertBack(object? value, System.Type targetType, object? parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
