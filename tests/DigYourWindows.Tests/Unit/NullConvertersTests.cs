using System.Globalization;
using DigYourWindows.UI.Converters;

namespace DigYourWindows.Tests.Unit;

/// <summary>
/// Unit tests for NullConverters.
/// </summary>
public class NullConvertersTests
{
    private readonly CultureInfo _culture = CultureInfo.InvariantCulture;

    [Fact]
    public void NullToEmptyStringConverterWithNullValueShouldReturnEmptyString()
    {
        // Arrange
        var converter = new NullToEmptyStringConverter();

        // Act
        var result = converter.Convert(null, typeof(string), null, _culture);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void NullToEmptyStringConverterWithValueShouldReturnStringRepresentation()
    {
        // Arrange
        var converter = new NullToEmptyStringConverter();

        // Act
        var result = converter.Convert("test", typeof(string), null, _culture);

        // Assert
        Assert.Equal("test", result);
    }

    [Fact]
    public void NullToEmptyStringConverterWithNumberShouldReturnStringRepresentation()
    {
        // Arrange
        var converter = new NullToEmptyStringConverter();

        // Act
        var result = converter.Convert(42, typeof(string), null, _culture);

        // Assert
        Assert.Equal("42", result);
    }

    [Fact]
    public void NullToEmptyStringConverterConvertBackShouldReturnValueAsIs()
    {
        // Arrange
        var converter = new NullToEmptyStringConverter();

        // Act
        var result = converter.ConvertBack("test", typeof(string), null, _culture);

        // Assert
        Assert.Equal("test", result);
    }

    [Fact]
    public void NullToPlaceholderConverterWithNullValueShouldReturnDefaultPlaceholder()
    {
        // Arrange
        var converter = new NullToPlaceholderConverter();

        // Act
        var result = converter.Convert(null, typeof(string), null, _culture);

        // Assert
        Assert.Equal("—", result);
    }

    [Fact]
    public void NullToPlaceholderConverterWithNullValueAndParameterShouldReturnParameterAsPlaceholder()
    {
        // Arrange
        var converter = new NullToPlaceholderConverter();

        // Act
        var result = converter.Convert(null, typeof(string), "N/A", _culture);

        // Assert
        Assert.Equal("N/A", result);
    }

    [Fact]
    public void NullToPlaceholderConverterWithEmptyStringShouldReturnDefaultPlaceholder()
    {
        // Arrange
        var converter = new NullToPlaceholderConverter();

        // Act
        var result = converter.Convert(string.Empty, typeof(string), null, _culture);

        // Assert
        Assert.Equal("—", result);
    }

    [Fact]
    public void NullToPlaceholderConverterWithValueShouldReturnValue()
    {
        // Arrange
        var converter = new NullToPlaceholderConverter();

        // Act
        var result = converter.Convert("test value", typeof(string), null, _culture);

        // Assert
        Assert.Equal("test value", result);
    }

    [Fact]
    public void NullToPlaceholderConverterConvertBackShouldReturnDependencyPropertyUnsetValue()
    {
        // Arrange
        var converter = new NullToPlaceholderConverter();

        // Act
        var result = converter.ConvertBack("test", typeof(string), null, _culture);

        // Assert
        Assert.Equal(System.Windows.DependencyProperty.UnsetValue, result);
    }

    [Fact]
    public void NullableDoubleToStringConverterWithDoubleValueShouldReturnFormattedString()
    {
        // Arrange
        var converter = new NullableDoubleToStringConverter();

        // Act
        var result = converter.Convert(42.5, typeof(string), "F1", _culture);

        // Assert
        Assert.Equal("42.5", result);
    }

    [Fact]
    public void NullableDoubleToStringConverterWithDoubleValueAndDefaultFormatShouldReturnIntegerString()
    {
        // Arrange
        var converter = new NullableDoubleToStringConverter();

        // Act
        var result = converter.Convert(42.567, typeof(string), null, _culture);

        // Assert
        Assert.Equal("43", result); // F0 format rounds
    }

    [Fact]
    public void NullableDoubleToStringConverterWithNullValueShouldReturnPlaceholder()
    {
        // Arrange
        var converter = new NullableDoubleToStringConverter();

        // Act
        var result = converter.Convert(null, typeof(string), null, _culture);

        // Assert
        Assert.Equal("—", result);
    }

    [Fact]
    public void NullableDoubleToStringConverterWithNonDoubleValueShouldReturnToString()
    {
        // Arrange
        var converter = new NullableDoubleToStringConverter();

        // Act
        var result = converter.Convert("not a double", typeof(string), null, _culture);

        // Assert
        Assert.Equal("not a double", result);
    }

    [Fact]
    public void NullableDoubleToStringConverterConvertBackShouldReturnDependencyPropertyUnsetValue()
    {
        // Arrange
        var converter = new NullableDoubleToStringConverter();

        // Act
        var result = converter.ConvertBack("42.5", typeof(double), null, _culture);

        // Assert
        Assert.Equal(System.Windows.DependencyProperty.UnsetValue, result);
    }

    [Theory]
    [InlineData(0.0, "F0", "0")]
    [InlineData(100.0, "F0", "100")]
    [InlineData(99.99, "F1", "100.0")]
    [InlineData(3.14159, "F2", "3.14")]
    public void NullableDoubleToStringConverterWithVariousInputsShouldFormatCorrectly(
        double value, string format, string expected)
    {
        // Arrange
        var converter = new NullableDoubleToStringConverter();

        // Act
        var result = converter.Convert(value, typeof(string), format, _culture);

        // Assert
        Assert.Equal(expected, result);
    }
}
