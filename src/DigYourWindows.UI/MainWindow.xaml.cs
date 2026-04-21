using System.Windows;
using DigYourWindows.UI.ViewModels;
using Wpf.Ui.Controls;

namespace DigYourWindows.UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : FluentWindow
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += MainWindow_Loaded;
        Closed += MainWindow_Closed;
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is MainViewModel viewModel)
            {
                await viewModel.LoadDataCommand.ExecuteAsync(null);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"加载数据失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void MainWindow_Closed(object? sender, EventArgs e)
    {
        if (DataContext is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}