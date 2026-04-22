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
    public void CountToVisibilityConverter_PositiveCount_ReturnsVisible()
    {
        // Arrange
        var converter = new CountToVisibilityConverter();

        // Act
        var result = converter.Convert(5, typeof(Visibility), null!, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(Visibility.Visible, result);
    }

    [Fact]
    public void CountToVisibilityConverter_ZeroCount_ReturnsCollapsed()
    {
        // Arrange
        var converter = new CountToVisibilityConverter();

        // Act
        var result = converter.Convert(0, typeof(Visibility), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(Visibility.Collapsed, result);
    }

    [Fact]
    public void CountToVisibilityConverter_NegativeCount_ReturnsCollapsed()
    {
        // Arrange
        var converter = new CountToVisibilityConverter();

        // Act
        var result = converter.Convert(-1, typeof(Visibility), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(Visibility.Collapsed, result);
    }

    [Fact]
    public void CountToVisibilityConverter_NonIntValue_ReturnsCollapsed()
    {
        // Arrange
        var converter = new CountToVisibilityConverter();

        // Act
        var result = converter.Convert("not an int", typeof(Visibility), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(Visibility.Collapsed, result);
    }

    [Fact]
    public void CountToVisibilityConverter_NullValue_ReturnsCollapsed()
    {
        // Arrange
        var converter = new CountToVisibilityConverter();

        // Act
        var result = converter.Convert(null, typeof(Visibility), null, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(Visibility.Collapsed, result);
    }

    [Fact]
    public void CountToVisibilityConverter_ConvertBack_ThrowsNotImplemented()
    {
        // Arrange
        var converter = new CountToVisibilityConverter();

        // Act & Assert
        Assert.Throws<NotImplementedException>(() =>
            converter.ConvertBack(Visibility.Visible, typeof(int), null, CultureInfo.CurrentCulture));
    }

    #endregion

    #region StringToBrushConverter Tests

    [Fact]
    public void StringToBrushConverter_ValidHexColor_ReturnsCorrectBrush()
    {
        // Arrange
        var converter = new StringToBrushConverter();

        // Act
        var result = converter.Convert("#FF0000", typeof(Brush), null, CultureInfo.CurrentCulture) as SolidColorBrush;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0xFF, result.Color.R);
        Assert.Equal(0x00, result.Color.G);
        Assert.Equal(0x00, result.Color.B);
    }

    [Fact]
    public void StringToBrushConverter_ValidNamedColor_ReturnsCorrectBrush()
    {
        // Arrange
        var converter = new StringToBrushConverter();

        // Act
        var result = converter.Convert("Red", typeof(Brush), null, CultureInfo.CurrentCulture) as SolidColorBrush;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Colors.Red, result.Color);
    }

    [Fact]
    public void StringToBrushConverter_InvalidColor_ReturnsDefaultGrayBrush()
    {
        // Arrange
        var converter = new StringToBrushConverter();

        // Act
        var result = converter.Convert("invalid-color", typeof(Brush), null, CultureInfo.CurrentCulture) as SolidColorBrush;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Colors.Gray, result.Color);
    }

    [Fact]
    public void StringToBrushConverter_NullValue_ReturnsDefaultGrayBrush()
    {
        // Arrange
        var converter = new StringToBrushConverter();

        // Act
        var result = converter.Convert(null, typeof(Brush), null, CultureInfo.CurrentCulture) as SolidColorBrush;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Colors.Gray, result.Color);
    }

    [Fact]
    public void StringToBrushConverter_EmptyString_ReturnsDefaultGrayBrush()
    {
        // Arrange
        var converter = new StringToBrushConverter();

        // Act
        var result = converter.Convert("", typeof(Brush), null, CultureInfo.CurrentCulture) as SolidColorBrush;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Colors.Gray, result.Color);
    }

    [Fact]
    public void StringToBrushConverter_WhitespaceString_ReturnsDefaultGrayBrush()
    {
        // Arrange
        var converter = new StringToBrushConverter();

        // Act
        var result = converter.Convert("   ", typeof(Brush), null, CultureInfo.CurrentCulture) as SolidColorBrush;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Colors.Gray, result.Color);
    }

    [Fact]
    public void StringToBrushConverter_ConvertBack_ThrowsNotImplemented()
    {
        // Arrange
        var converter = new StringToBrushConverter();
        var brush = new SolidColorBrush(Colors.Red);

        // Act & Assert
        Assert.Throws<NotImplementedException>(() =>
            converter.ConvertBack(brush, typeof(string), null, CultureInfo.CurrentCulture));
    }

    #endregion
}
