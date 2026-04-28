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
                // Initialize history on startup
                await viewModel.InitializeHistoryAsync();

                // Load current diagnostic data
                await viewModel.LoadDataCommand.ExecuteAsync(null);

                // Load history list
                if (viewModel.HistoryListViewModel != null)
                {
                    await viewModel.HistoryListViewModel.LoadHistoryCommand.ExecuteAsync(null);
                }
            }
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"加载数据失败: {ex.Message}", "错误", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
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