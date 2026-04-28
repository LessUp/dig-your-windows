using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigYourWindows.Core.Models;
using DigYourWindows.Core.Services;

namespace DigYourWindows.UI.ViewModels;

/// <summary>
/// ViewModel for displaying the list of historical diagnostics.
/// Manages loading, filtering, and selection of history entries.
/// </summary>
public sealed partial class HistoryListViewModel : ObservableObject
{
    private readonly IHistoryStoreService _historyStoreService;
    private readonly ILogService _logService;

    [ObservableProperty]
    private ObservableCollection<DiagnosticHistorySummary> historyEntries = new();

    [ObservableProperty]
    private DiagnosticHistorySummary? selectedEntry;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private bool isHistoryEmpty;

    public HistoryListViewModel(IHistoryStoreService historyStoreService, ILogService logService)
    {
        _historyStoreService = historyStoreService ?? throw new ArgumentNullException(nameof(historyStoreService));
        _logService = logService ?? throw new ArgumentNullException(nameof(logService));
    }

    [RelayCommand]
    public async Task LoadHistoryAsync()
    {
        IsLoading = true;
        try
        {
            var summaries = await _historyStoreService.ListSummariesAsync();
            HistoryEntries = new ObservableCollection<DiagnosticHistorySummary>(summaries);
            IsHistoryEmpty = !summaries.Any();
        }
        catch (Exception ex)
        {
            _logService.LogError($"Failed to load history: {ex.Message}", ex);
            IsHistoryEmpty = true;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task SelectEntryAsync(DiagnosticHistorySummary? entry)
    {
        SelectedEntry = entry;
    }
}
