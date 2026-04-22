using DigYourWindows.Core.Models;
using DigYourWindows.Core.Services;

namespace DigYourWindows.Tests.Unit;

public class DiagnosticCollectorServiceTests
{
    [Fact]
    public async Task CollectAsyncWhenCanceledShouldThrowOperationCanceledException()
    {
        using var cts = new CancellationTokenSource();
        var service = new DiagnosticCollectorService(
            new StubHardwareService(token => throw new OperationCanceledException(token)),
            new StubReliabilityService((_, _) => Array.Empty<ReliabilityRecordData>()),
            new StubEventLogService((_, _) => new List<LogEventData>()),
            new StubPerformanceService((_, _, _) => new PerformanceAnalysisData()),
            new SpyLogService());

        await Assert.ThrowsAsync<OperationCanceledException>(() => service.CollectAsync(3, cancellationToken: cts.Token));
    }

    [Fact]
    public async Task CollectAsync_WhenHardwareStepFails_ShouldReturnWarningAndContinue()
    {
        var expectedAnalysis = new PerformanceAnalysisData { SystemHealthScore = 88d };
        var service = new DiagnosticCollectorService(
            new StubHardwareService(_ => throw new InvalidOperationException("boom")),
            new StubReliabilityService((_, _) =>
            [
                new ReliabilityRecordData { SourceName = "Reliability" }
            ]),
            new StubEventLogService((_, _) =>
            [
                new LogEventData { SourceName = "System", EventType = "Error", Message = "Event" }
            ]),
            new StubPerformanceService((hardware, events, reliability) =>
            {
                Assert.NotNull(hardware);
                Assert.Single(events);
                Assert.Single(reliability);
                return expectedAnalysis;
            }),
            new SpyLogService());

        var result = await service.CollectAsync(7);

        Assert.Single(result.Warnings);
        Assert.Equal("硬件信息获取失败: boom", result.Warnings[0]);
        Assert.Empty(result.Data.Hardware.ComputerName);
        Assert.Single(result.Data.Reliability);
        Assert.Single(result.Data.Events);
        Assert.Same(expectedAnalysis, result.Data.Performance);
    }

    [Fact]
    public async Task CollectAsync_WhenPerformanceStepFails_ShouldReturnDefaultAnalysisAndWarning()
    {
        var service = new DiagnosticCollectorService(
            new StubHardwareService(_ => new HardwareData { ComputerName = "TEST-PC" }),
            new StubReliabilityService((_, _) => Array.Empty<ReliabilityRecordData>()),
            new StubEventLogService((_, _) => new List<LogEventData>()),
            new StubPerformanceService((_, _, _) => throw new InvalidOperationException("analysis failed")),
            new SpyLogService());

        var result = await service.CollectAsync(3);

        Assert.Single(result.Warnings);
        Assert.Equal("性能分析失败: analysis failed", result.Warnings[0]);
        Assert.NotNull(result.Data.Performance);
        Assert.Equal(0d, result.Data.Performance.SystemHealthScore);
        Assert.Equal("TEST-PC", result.Data.Hardware.ComputerName);
    }

    [Fact]
    public async Task CollectAsync_ShouldReportProgressInExpectedOrderAndAssembleData()
    {
        var hardware = new HardwareData { ComputerName = "TEST-PC" };
        var reliability = new[]
        {
            new ReliabilityRecordData { SourceName = "Reliability", Timestamp = DateTime.UtcNow }
        };
        var events = new List<LogEventData>
        {
            new LogEventData { SourceName = "System", EventType = "Warning", Message = "Test" }
        };
        var analysis = new PerformanceAnalysisData { SystemHealthScore = 95d, HealthGrade = "优秀" };
        var progressItems = new List<DiagnosticCollectionProgress>();
        var progress = new Progress<DiagnosticCollectionProgress>(item => progressItems.Add(item));

        var service = new DiagnosticCollectorService(
            new StubHardwareService(_ => hardware),
            new StubReliabilityService((daysBack, _) =>
            {
                Assert.Equal(3, daysBack);
                return reliability;
            }),
            new StubEventLogService((daysBack, _) =>
            {
                Assert.Equal(3, daysBack);
                return events;
            }),
            new StubPerformanceService((actualHardware, actualEvents, actualReliability) =>
            {
                Assert.Same(hardware, actualHardware);
                Assert.Same(events, actualEvents);
                Assert.Equal(reliability, actualReliability);
                return analysis;
            }),
            new SpyLogService());

        var result = await service.CollectAsync(3, progress);

        Assert.Empty(result.Warnings);
        Assert.Same(hardware, result.Data.Hardware);
        Assert.Equal(reliability, result.Data.Reliability);
        Assert.Same(events, result.Data.Events);
        Assert.Same(analysis, result.Data.Performance);
        Assert.Equal(4, progressItems.Count);
        Assert.Collection(
            progressItems,
            item => Assert.Equal((1, 4, "正在获取硬件信息..."), (item.StepIndex, item.StepCount, item.Message)),
            item => Assert.Equal((2, 4, "正在获取可靠性记录..."), (item.StepIndex, item.StepCount, item.Message)),
            item => Assert.Equal((3, 4, "正在获取事件日志 (最近3天)..."), (item.StepIndex, item.StepCount, item.Message)),
            item => Assert.Equal((4, 4, "正在进行性能分析..."), (item.StepIndex, item.StepCount, item.Message)));
    }

    private sealed class StubHardwareService(Func<CancellationToken, HardwareData> handler) : IHardwareService
    {
        public HardwareData GetHardwareInfo(CancellationToken cancellationToken = default) => handler(cancellationToken);
    }

    private sealed class StubReliabilityService(Func<int, CancellationToken, ReliabilityRecordData[]> handler) : IReliabilityService
    {
        public ReliabilityRecordData[] GetReliabilityRecords(int daysBack = 7, CancellationToken cancellationToken = default) =>
            handler(daysBack, cancellationToken);
    }

    private sealed class StubEventLogService(Func<int, CancellationToken, List<LogEventData>> handler) : IEventLogService
    {
        public List<LogEventData> GetErrorEvents(int daysBack = 3, CancellationToken cancellationToken = default) =>
            handler(daysBack, cancellationToken);
    }

    private sealed class StubPerformanceService(Func<HardwareData, List<LogEventData>, List<ReliabilityRecordData>, PerformanceAnalysisData> handler) : IPerformanceService
    {
        public PerformanceAnalysisData AnalyzeSystemPerformance(
            HardwareData hardware,
            List<LogEventData> events,
            List<ReliabilityRecordData> reliability) => handler(hardware, events, reliability);
    }

    private sealed class SpyLogService : ILogService
    {
        public List<(string Message, Exception? Exception)> Errors { get; } = new();

        public void Info(string message)
        {
        }

        public void Warn(string message)
        {
        }

        public void LogError(string message, Exception? exception = null)
        {
            Errors.Add((message, exception));
        }
    }
}
