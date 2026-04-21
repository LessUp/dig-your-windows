using DigYourWindows.Core.Models;

namespace DigYourWindows.Core.Services;

public interface IDiagnosticCollectorService
{
    Task<DiagnosticCollectionResult> CollectAsync(
        int daysBack,
        IProgress<DiagnosticCollectionProgress>? progress = null,
        CancellationToken cancellationToken = default);
}

public class DiagnosticCollectorService : IDiagnosticCollectorService
{
    private readonly IHardwareService _hardwareService;
    private readonly IReliabilityService _reliabilityService;
    private readonly IEventLogService _eventLogService;
    private readonly IPerformanceService _performanceService;
    private readonly ILogService _log;

    public DiagnosticCollectorService(
        IHardwareService hardwareService,
        IReliabilityService reliabilityService,
        IEventLogService eventLogService,
        IPerformanceService performanceService,
        ILogService log)
    {
        _hardwareService = hardwareService;
        _reliabilityService = reliabilityService;
        _eventLogService = eventLogService;
        _performanceService = performanceService;
        _log = log;
    }

    public async Task<DiagnosticCollectionResult> CollectAsync(
        int daysBack,
        IProgress<DiagnosticCollectionProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        if (daysBack <= 0)
            daysBack = 1;

        var warnings = new List<string>();
        const int stepCount = 4;

        var hardware = await ExecuteStepAsync(
            stepIndex: 1,
            stepCount,
            progressMessage: "正在获取硬件信息...",
            logMessage: "获取硬件信息失败",
            warningPrefix: "硬件信息获取失败",
            operation: token => _hardwareService.GetHardwareInfo(token),
            fallbackFactory: static () => new HardwareData(),
            warnings,
            progress,
            cancellationToken);

        var reliability = await ExecuteStepAsync(
            stepIndex: 2,
            stepCount,
            progressMessage: "正在获取可靠性记录...",
            logMessage: "获取可靠性记录失败",
            warningPrefix: "可靠性记录获取失败",
            operation: token => _reliabilityService.GetReliabilityRecords(daysBack, token).ToList(),
            fallbackFactory: static () => new List<ReliabilityRecordData>(),
            warnings,
            progress,
            cancellationToken);

        var events = await ExecuteStepAsync(
            stepIndex: 3,
            stepCount,
            progressMessage: $"正在获取事件日志 (最近{daysBack}天)...",
            logMessage: "获取事件日志失败",
            warningPrefix: "事件日志获取失败",
            operation: token => _eventLogService.GetErrorEvents(daysBack, token),
            fallbackFactory: static () => new List<LogEventData>(),
            warnings,
            progress,
            cancellationToken);

        var analysis = await ExecuteStepAsync(
            stepIndex: 4,
            stepCount,
            progressMessage: "正在进行性能分析...",
            logMessage: "性能分析失败",
            warningPrefix: "性能分析失败",
            operation: _ => _performanceService.AnalyzeSystemPerformance(hardware, events, reliability),
            fallbackFactory: static () => new PerformanceAnalysisData(),
            warnings,
            progress,
            cancellationToken);

        var data = new DiagnosticData
        {
            Hardware = hardware,
            Reliability = reliability,
            Events = events,
            Performance = analysis,
            CollectedAt = DateTime.UtcNow
        };

        return new DiagnosticCollectionResult(data, warnings);
    }

    private async Task<T> ExecuteStepAsync<T>(
        int stepIndex,
        int stepCount,
        string progressMessage,
        string logMessage,
        string warningPrefix,
        Func<CancellationToken, T> operation,
        Func<T> fallbackFactory,
        List<string> warnings,
        IProgress<DiagnosticCollectionProgress>? progress,
        CancellationToken cancellationToken)
    {
        progress?.Report(new DiagnosticCollectionProgress(stepIndex, stepCount, progressMessage));

        try
        {
            return await Task.Run(() => operation(cancellationToken), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _log.LogError(logMessage, ex);
            warnings.Add($"{warningPrefix}: {ex.Message}");
            return fallbackFactory();
        }
    }
}
