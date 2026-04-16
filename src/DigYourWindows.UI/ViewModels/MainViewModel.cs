using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigYourWindows.Core.Models;
using DigYourWindows.Core.Services;
using Microsoft.Win32;
using ScottPlot.WPF;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Threading;
using Wpf.Ui.Appearance;

namespace DigYourWindows.UI.ViewModels;

public partial class MainViewModel : ObservableObject, IDisposable
{
    private readonly IDiagnosticCollectorService _collectorService;
    private readonly IReportService _reportService;
    private readonly ICpuMonitorService _cpuMonitorService;
    private readonly INetworkMonitorService _networkMonitorService;
    private readonly ILogService _log;
    private readonly DispatcherTimer _cpuMonitorTimer;
    private CancellationTokenSource? _loadCts;
    private DiagnosticData? _currentData;
    private bool _reloadRequested;

    private const int NetworkHistoryCapacity = 60;
    private long? _lastNetworkBytesReceived;
    private long? _lastNetworkBytesSent;
    private DateTimeOffset? _lastNetworkSampleTime;
    private readonly Queue<DateTime> _networkHistoryTimes = new();
    private readonly Queue<double> _networkHistoryDownload = new();
    private readonly Queue<double> _networkHistoryUpload = new();

    [ObservableProperty]
    private HardwareData? _hardwareInfo;

    [ObservableProperty]
    private CpuInfoData _cpuInfo = new();

    [ObservableProperty]
    private double _networkDownloadMBps;

    [ObservableProperty]
    private double _networkUploadMBps;

    [ObservableProperty]
    private ObservableCollection<ReliabilityRecordData> _reliabilityRecords = new();

    [ObservableProperty]
    private ObservableCollection<LogEventData> _eventLogEntries = new();

    [ObservableProperty]
    private PerformanceAnalysisData? _performanceAnalysis;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string _statusMessage = "就绪";

    [ObservableProperty]
    private int _selectedDaysBack = 3;

    [ObservableProperty]
    private ApplicationTheme _currentTheme = ApplicationTheme.Dark;

    public List<int> AvailableDays { get; } = new() { 1, 3, 7, 30 };

    public WpfPlot ReliabilityTrendPlot { get; } = new();

    public WpfPlot NetworkTrafficPlot { get; } = new();

    public MainViewModel(
        IDiagnosticCollectorService collectorService,
        IReportService reportService,
        ICpuMonitorService cpuMonitorService,
        INetworkMonitorService networkMonitorService,
        ILogService log)
    {
        _collectorService = collectorService;
        _reportService = reportService;
        _cpuMonitorService = cpuMonitorService;
        _networkMonitorService = networkMonitorService;
        _log = log;

        _cpuMonitorTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _cpuMonitorTimer.Tick += CpuMonitorTimer_Tick;
        _cpuMonitorTimer.Start();

        UpdateCpuInfo();
        UpdateNetworkTraffic();
        UpdateReliabilityTrendPlot();
        UpdateNetworkTrafficPlot();
    }

    private void CpuMonitorTimer_Tick(object? sender, EventArgs e)
    {
        UpdateCpuInfo();
        UpdateNetworkTraffic();
    }

    private void UpdateCpuInfo()
    {
        CpuInfo = _cpuMonitorService.GetCpuInfo();
    }

    private void UpdateNetworkTraffic()
    {
        try
        {
            var now = DateTimeOffset.Now;
            var (bytesReceived, bytesSent) = _networkMonitorService.GetTotalBytes();

            if (TryInitializeNetworkSample(now, bytesReceived, bytesSent))
            {
                return;
            }

            if (!TryCalculateNetworkRates(now, bytesReceived, bytesSent, out var downloadMBps, out var uploadMBps))
            {
                return;
            }

            NetworkDownloadMBps = downloadMBps;
            NetworkUploadMBps = uploadMBps;
            UpdateNetworkSampleState(now, bytesReceived, bytesSent);
            AppendNetworkHistory(now.LocalDateTime, downloadMBps, uploadMBps);
            UpdateNetworkTrafficPlot();
        }
        catch (Exception ex)
        {
            _log.Warn($"更新网络流量失败: {ex.Message}");
        }
    }

    private bool TryInitializeNetworkSample(DateTimeOffset now, long bytesReceived, long bytesSent)
    {
        if (_lastNetworkSampleTime is not null && _lastNetworkBytesReceived is not null && _lastNetworkBytesSent is not null)
        {
            return false;
        }

        UpdateNetworkSampleState(now, bytesReceived, bytesSent);
        NetworkDownloadMBps = 0d;
        NetworkUploadMBps = 0d;
        AppendNetworkHistory(now.LocalDateTime, 0d, 0d);
        UpdateNetworkTrafficPlot();
        return true;
    }

    private bool TryCalculateNetworkRates(
        DateTimeOffset now,
        long bytesReceived,
        long bytesSent,
        out double downloadMBps,
        out double uploadMBps)
    {
        downloadMBps = 0d;
        uploadMBps = 0d;

        if (_lastNetworkSampleTime is null || _lastNetworkBytesReceived is null || _lastNetworkBytesSent is null)
        {
            return false;
        }

        var dtSeconds = (now - _lastNetworkSampleTime.Value).TotalSeconds;
        if (dtSeconds <= 0)
        {
            return false;
        }

        var deltaReceived = Math.Max(0L, bytesReceived - _lastNetworkBytesReceived.Value);
        var deltaSent = Math.Max(0L, bytesSent - _lastNetworkBytesSent.Value);

        downloadMBps = deltaReceived / dtSeconds / 1024d / 1024d;
        uploadMBps = deltaSent / dtSeconds / 1024d / 1024d;
        return true;
    }

    private void UpdateNetworkSampleState(DateTimeOffset now, long bytesReceived, long bytesSent)
    {
        _lastNetworkSampleTime = now;
        _lastNetworkBytesReceived = bytesReceived;
        _lastNetworkBytesSent = bytesSent;
    }

    private void AppendNetworkHistory(DateTime time, double downloadMBps, double uploadMBps)
    {
        _networkHistoryTimes.Enqueue(time);
        _networkHistoryDownload.Enqueue(downloadMBps);
        _networkHistoryUpload.Enqueue(uploadMBps);

        while (_networkHistoryTimes.Count > NetworkHistoryCapacity)
        {
            _networkHistoryTimes.Dequeue();
            _networkHistoryDownload.Dequeue();
            _networkHistoryUpload.Dequeue();
        }
    }

    private void UpdateNetworkTrafficPlot()
    {
        var plot = NetworkTrafficPlot.Plot;
        plot.Clear();
        plot.Title("网络流量 (最近60秒)");
        plot.XLabel("时间");
        plot.YLabel("MB/s");

        ApplyPlotTheme(plot);

        if (_networkHistoryTimes.Count == 0)
        {
            NetworkTrafficPlot.Refresh();
            return;
        }

        var xs = _networkHistoryTimes.Select(time => time.ToOADate()).ToArray();
        var downYs = _networkHistoryDownload.ToArray();
        var upYs = _networkHistoryUpload.ToArray();

        var downScatter = plot.Add.Scatter(xs, downYs);
        downScatter.LegendText = "下载";
        downScatter.Color = ScottPlot.Color.FromHex("#2196F3");

        var upScatter = plot.Add.Scatter(xs, upYs);
        upScatter.LegendText = "上传";
        upScatter.Color = ScottPlot.Color.FromHex("#4CAF50");

        plot.Legend.IsVisible = true;
        NetworkTrafficPlot.Refresh();
    }

    public void Dispose()
    {
        _cpuMonitorTimer.Stop();
        _cpuMonitorTimer.Tick -= CpuMonitorTimer_Tick;
        _loadCts?.Cancel();
        _loadCts?.Dispose();
        GC.SuppressFinalize(this);
    }

    partial void OnSelectedDaysBackChanged(int value)
    {
        if (IsLoading)
        {
            _reloadRequested = true;
            return;
        }

        _ = LoadDataAsyncSafe();
    }

    private async Task LoadDataAsyncSafe()
    {
        try
        {
            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            _log.LogError("LoadDataAsync failed unexpectedly", ex);
        }
    }

    [RelayCommand]
    private async Task LoadDataAsync()
    {
        if (IsLoading)
        {
            return;
        }

        IsLoading = true;
        StatusMessage = "正在加载数据...";

        _loadCts?.Cancel();
        _loadCts = new CancellationTokenSource();

        try
        {
            var progress = new Progress<DiagnosticCollectionProgress>(p =>
            {
                StatusMessage = p.Message;
            });

            var daysBack = SelectedDaysBack;
            var result = await _collectorService.CollectAsync(daysBack, progress, _loadCts.Token);
            ApplyDiagnosticData(result.Data);

            if (result.Warnings.Count > 0)
            {
                _log.Warn($"数据采集存在 {result.Warnings.Count} 条警告: {string.Join(" | ", result.Warnings)}");
            }

            StatusMessage = BuildLoadCompletedStatus(result);
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "已取消";
        }
        catch (Exception ex)
        {
            _log.LogError("加载失败", ex);
            StatusMessage = $"加载失败: {ex.Message}";
        }
        finally
        {
            IsLoading = false;

            if (_reloadRequested)
            {
                _reloadRequested = false;
                _ = LoadDataAsync();
            }
        }
    }

    [RelayCommand]
    private async Task ImportFromJsonAsync()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "JSON 文件 (*.json)|*.json|所有文件 (*.*)|*.*",
            DefaultExt = ".json"
        };

        if (dialog.ShowDialog() != true)
        {
            return;
        }

        try
        {
            StatusMessage = "正在导入JSON报告...";
            IsLoading = true;

            var json = await Task.Run(() => File.ReadAllText(dialog.FileName, Encoding.UTF8));
            var data = _reportService.DeserializeFromJson(json);

            if (data == null)
            {
                StatusMessage = "导入失败: JSON 解析结果为空";
                return;
            }

            ApplyDiagnosticData(data);
            StatusMessage = BuildImportCompletedStatus(data);
        }
        catch (Exception ex)
        {
            _log.LogError("导入失败", ex);
            StatusMessage = $"导入失败: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ApplyDiagnosticData(DiagnosticData data)
    {
        _currentData = data;
        HardwareInfo = data.Hardware;
        ReplaceCollection(ReliabilityRecords, data.Reliability);
        ReplaceCollection(EventLogEntries, data.Events);
        PerformanceAnalysis = data.Performance;
        UpdateReliabilityTrendPlot();
    }

    private static void ReplaceCollection<T>(ObservableCollection<T> target, IEnumerable<T> items)
    {
        target.Clear();
        foreach (var item in items)
        {
            target.Add(item);
        }
    }

    private DiagnosticData? BuildDiagnosticDataForExport()
    {
        if (_currentData is not null && HasMeaningfulData(_currentData))
        {
            return _currentData;
        }

        var data = new DiagnosticData
        {
            Hardware = HardwareInfo ?? new HardwareData(),
            Reliability = ReliabilityRecords.ToList(),
            Events = EventLogEntries.ToList(),
            Performance = PerformanceAnalysis ?? new PerformanceAnalysisData(),
            CollectedAt = _currentData?.CollectedAt ?? DateTime.UtcNow
        };

        if (!HasMeaningfulData(data))
        {
            return null;
        }

        _currentData = data;
        return data;
    }

    private static bool HasMeaningfulData(DiagnosticData data)
    {
        return !string.IsNullOrWhiteSpace(data.Hardware.ComputerName) ||
               !string.IsNullOrWhiteSpace(data.Hardware.OsVersion) ||
               !string.IsNullOrWhiteSpace(data.Hardware.CpuBrand) ||
               data.Hardware.TotalMemory > 0 ||
               data.Hardware.Disks.Count > 0 ||
               data.Hardware.Gpus.Count > 0 ||
               data.Reliability.Count > 0 ||
               data.Events.Count > 0 ||
               data.Performance.SystemHealthScore > 0 ||
               data.Performance.StabilityScore > 0 ||
               data.Performance.PerformanceScore > 0 ||
               data.Performance.MemoryUsageScore > 0 ||
               data.Performance.DiskHealthScore > 0 ||
               data.Performance.CriticalIssuesCount > 0 ||
               data.Performance.WarningsCount > 0 ||
               data.Performance.Recommendations.Count > 0;
    }

    [RelayCommand]
    private Task ExportToJsonAsync()
    {
        return ExportReportAsync(
            loadingMessage: "正在导出JSON报告...",
            extension: "json",
            successPrefix: "JSON已导出",
            contentFactory: data => _reportService.SerializeToJson(data, indented: true));
    }

    [RelayCommand]
    private Task ExportToHtmlAsync()
    {
        return ExportReportAsync(
            loadingMessage: "正在导出HTML报告...",
            extension: "html",
            successPrefix: "报告已导出",
            contentFactory: data => _reportService.GenerateHtmlReport(data, SelectedDaysBack));
    }

    private async Task ExportReportAsync(
        string loadingMessage,
        string extension,
        string successPrefix,
        Func<DiagnosticData, string> contentFactory)
    {
        try
        {
            StatusMessage = loadingMessage;
            IsLoading = true;

            var data = BuildDiagnosticDataForExport();
            if (data is null)
            {
                StatusMessage = "导出失败: 当前没有可导出的诊断数据";
                return;
            }

            var content = contentFactory(data);
            var (fileName, filePath) = BuildExportPath(extension);
            await WriteExportFileAsync(filePath, content);

            StatusMessage = BuildExportSuccessStatus(successPrefix, fileName);
            OpenExportedFile(filePath);
        }
        catch (Exception ex)
        {
            _log.LogError($"导出{extension.ToUpperInvariant()}失败", ex);
            StatusMessage = $"导出失败: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private static (string FileName, string FilePath) BuildExportPath(string extension)
    {
        var fileName = $"DigYourWindows_Report_{DateTime.Now:yyyyMMdd_HHmmss}.{extension}";
        var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
        return (fileName, filePath);
    }

    private static Task WriteExportFileAsync(string filePath, string content)
    {
        return Task.Run(() => File.WriteAllText(filePath, content, Encoding.UTF8));
    }

    private static void OpenExportedFile(string filePath)
    {
        try
        {
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
        catch (Exception)
        {
            // Ignore if we can't open the file - the user can navigate to it manually
        }
    }

    private static string BuildLoadCompletedStatus(DiagnosticCollectionResult result)
    {
        var performanceScore = result.Data.Performance.SystemHealthScore;
        return $"数据加载完成 | 可靠性记录: {result.Data.Reliability.Count} | 错误事件: {result.Data.Events.Count} | 系统健康评分: {performanceScore:F0}/100" +
               (result.Warnings.Count > 0 ? $" | 警告: {result.Warnings.Count}" : string.Empty);
    }

    private static string BuildImportCompletedStatus(DiagnosticData data)
    {
        return $"JSON已导入 | 采集时间(UTC): {data.CollectedAt:yyyy-MM-dd HH:mm:ss} | 可靠性记录: {data.Reliability.Count} | 错误事件: {data.Events.Count}";
    }

    private static string BuildExportSuccessStatus(string successPrefix, string fileName)
    {
        return $"{successPrefix}: {fileName}";
    }

    [RelayCommand]
    private void ToggleTheme()
    {
        CurrentTheme = CurrentTheme == ApplicationTheme.Dark
            ? ApplicationTheme.Light
            : ApplicationTheme.Dark;

        ApplicationThemeManager.Apply(CurrentTheme);
        UpdateReliabilityTrendPlot();
        UpdateNetworkTrafficPlot();
        StatusMessage = BuildThemeChangedStatus();
    }

    private string BuildThemeChangedStatus()
    {
        return $"主题已切换为: {(CurrentTheme == ApplicationTheme.Dark ? "深色" : "浅色")}";
    }

    private void UpdateReliabilityTrendPlot()
    {
        var plot = ReliabilityTrendPlot.Plot;
        plot.Clear();
        plot.Title("可靠性趋势");
        plot.XLabel("日期");
        plot.YLabel("记录数");

        var records = ReliabilityRecords.ToList();
        if (records.Count == 0)
        {
            ApplyPlotTheme(plot);
            ReliabilityTrendPlot.Refresh();
            return;
        }

        var days = BuildReliabilityTimeline(records);
        var xs = days.Select(day => day.ToOADate()).ToArray();
        var totalYs = BuildReliabilitySeries(days, records, null);

        var totalScatter = plot.Add.Scatter(xs, totalYs);
        totalScatter.LegendText = "总计";
        totalScatter.Color = ScottPlot.Color.FromHex("#9E9E9E");
        totalScatter.LineWidth = 2;

        var categories = new[]
        {
            new { Key = (int?)1, Name = "应用程序故障", Color = "#F44336" },
            new { Key = (int?)2, Name = "Windows 故障", Color = "#FF9800" },
            new { Key = (int?)3, Name = "其他故障", Color = "#FFC107" },
            new { Key = (int?)null, Name = "未知", Color = "#9C27B0" }
        };

        foreach (var category in categories)
        {
            var ys = BuildReliabilitySeries(days, records, category.Key);
            if (ys.All(y => y == 0))
            {
                continue;
            }

            var scatter = plot.Add.Scatter(xs, ys);
            scatter.LegendText = category.Name;
            scatter.Color = ScottPlot.Color.FromHex(category.Color);
        }

        plot.Legend.IsVisible = true;
        ApplyPlotTheme(plot);
        ReliabilityTrendPlot.Refresh();
    }

    private DateTime[] BuildReliabilityTimeline(List<ReliabilityRecordData> records)
    {
        var endDate = DateTime.Today;
        DateTime startDate;

        if (SelectedDaysBack <= 0)
        {
            startDate = records.Count > 0
                ? records.Min(x => x.Timestamp.Date)
                : endDate;
        }
        else
        {
            startDate = endDate.AddDays(-(SelectedDaysBack - 1));
        }

        return Enumerable
            .Range(0, (endDate - startDate).Days + 1)
            .Select(i => startDate.AddDays(i))
            .ToArray();
    }

    private static double[] BuildReliabilitySeries(
        IReadOnlyList<DateTime> days,
        List<ReliabilityRecordData> records,
        int? category)
    {
        return days
            .Select(day => (double)records.Count(r => MatchesReliabilityCategory(r, category) && r.Timestamp.Date == day.Date))
            .ToArray();
    }

    private static bool MatchesReliabilityCategory(ReliabilityRecordData record, int? category)
    {
        if (category is null)
        {
            return !record.RecordType.HasValue || (record.RecordType is not 1 and not 2 and not 3);
        }

        return record.RecordType == category;
    }

    private void ApplyPlotTheme(ScottPlot.Plot plot)
    {
        var isDarkTheme = CurrentTheme == ApplicationTheme.Dark;
        var backgroundColor = isDarkTheme ? ScottPlot.Color.FromHex("#1E1E1E") : ScottPlot.Color.FromHex("#FFFFFF");
        var textColor = isDarkTheme ? ScottPlot.Color.FromHex("#FFFFFF") : ScottPlot.Color.FromHex("#212529");
        var gridColor = isDarkTheme ? ScottPlot.Color.FromHex("#3E3E3E") : ScottPlot.Color.FromHex("#E0E0E0");

        plot.FigureBackground.Color = backgroundColor;
        plot.Axes.Color(textColor);
        plot.Grid.MajorLineColor = gridColor;

        foreach (var axis in plot.Axes.GetAxes())
        {
            axis.Label.ForeColor = textColor;
            axis.TickLabelStyle.ForeColor = textColor;
        }

        plot.TitleLabel.ForeColor = textColor;
    }
}
