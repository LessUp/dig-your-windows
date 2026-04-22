using DigYourWindows.UI.Converters;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace DigYourWindows.Tests.Unit;

/// <summary>
/// Unit tests for UI value converters.
/// </summary>
public class ConvertersTests
{
    #region CountToVisibilityConverter Tests

    [Fact]
    public void CountToVisibilityConverterPositiveCountReturnsVisible()
    {
        // Arrange
        var converter = new CountToVisibilityConverter();

        // Act
        var result = converter.Convert(5, typeof(Visibility), null!, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(Visibility.Visible, result);
    }

    [Fact]
    public void CountToVisibilityConverterZeroCountReturnsCollapsed()
    {
        // Arrange
        var converter = new CountToVisibilityConverter();

        // Act
        var result = converter.Convert(0, typeof(Visibility), null!, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(Visibility.Collapsed, result);
    }

    [Fact]
    public void CountToVisibilityConverterNegativeCountReturnsCollapsed()
    {
        // Arrange
        var converter = new CountToVisibilityConverter();

        // Act
        var result = converter.Convert(-1, typeof(Visibility), null!, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(Visibility.Collapsed, result);
    }

    [Fact]
    public void CountToVisibilityConverterNonIntValueReturnsCollapsed()
    {
        // Arrange
        var converter = new CountToVisibilityConverter();

        // Act
        var result = converter.Convert("not an int", typeof(Visibility), null!, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(Visibility.Collapsed, result);
    }

    [Fact]
    public void CountToVisibilityConverterNullValueReturnsCollapsed()
    {
        // Arrange
        var converter = new CountToVisibilityConverter();

        // Act
        var result = converter.Convert(null!, typeof(Visibility), null!, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(Visibility.Collapsed, result);
    }

    [Fact]
    public void CountToVisibilityConverterConvertBackThrowsNotImplemented()
    {
        // Arrange
        var converter = new CountToVisibilityConverter();

        // Act & Assert
        Assert.Throws<NotImplementedException>(() =>
            converter.ConvertBack(Visibility.Visible, typeof(int), null!, CultureInfo.CurrentCulture));
    }

    #endregion

    #region StringToBrushConverter Tests

    [Fact]
    public void StringToBrushConverterValidHexColorReturnsCorrectBrush()
    {
        // Arrange
        var converter = new StringToBrushConverter();

        // Act
        var result = converter.Convert("#FF0000", typeof(Brush), null!, CultureInfo.CurrentCulture) as SolidColorBrush;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0xFF, result.Color.R);
        Assert.Equal(0x00, result.Color.G);
        Assert.Equal(0x00, result.Color.B);
    }

    [Fact]
    public void StringToBrushConverterValidNamedColorReturnsCorrectBrush()
    {
        // Arrange
        var converter = new StringToBrushConverter();

        // Act
        var result = converter.Convert("Red", typeof(Brush), null!, CultureInfo.CurrentCulture) as SolidColorBrush;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Colors.Red, result.Color);
    }

    [Fact]
    public void StringToBrushConverterInvalidColorReturnsDefaultGrayBrush()
    {
        // Arrange
        var converter = new StringToBrushConverter();

        // Act
        var result = converter.Convert("invalid-color", typeof(Brush), null!, CultureInfo.CurrentCulture) as SolidColorBrush;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Colors.Gray, result.Color);
    }

    [Fact]
    public void StringToBrushConverterNullValueReturnsDefaultGrayBrush()
    {
        // Arrange
        var converter = new StringToBrushConverter();

        // Act
        var result = converter.Convert(null, typeof(Brush), null!, CultureInfo.CurrentCulture) as SolidColorBrush;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Colors.Gray, result.Color);
    }

    [Fact]
    public void StringToBrushConverterEmptyStringReturnsDefaultGrayBrush()
    {
        // Arrange
        var converter = new StringToBrushConverter();

        // Act
        var result = converter.Convert("", typeof(Brush), null!, CultureInfo.CurrentCulture) as SolidColorBrush;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Colors.Gray, result.Color);
    }

    [Fact]
    public void StringToBrushConverterWhitespaceStringReturnsDefaultGrayBrush()
    {
        // Arrange
        var converter = new StringToBrushConverter();

        // Act
        var result = converter.Convert("   ", typeof(Brush), null!, CultureInfo.CurrentCulture) as SolidColorBrush;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Colors.Gray, result.Color);
    }

    [Fact]
    public void StringToBrushConverterConvertBackThrowsNotImplemented()
    {
        // Arrange
        var converter = new StringToBrushConverter();
        var brush = new SolidColorBrush(Colors.Red);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() =>
            converter.ConvertBack(brush, typeof(string), null!, CultureInfo.CurrentCulture));
    }

    #endregion
}
