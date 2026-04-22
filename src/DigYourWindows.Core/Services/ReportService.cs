using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using DigYourWindows.Core.Exceptions;
using DigYourWindows.Core.Models;

namespace DigYourWindows.Core.Services;

public interface IReportService
{
    string SerializeToJson(DiagnosticData data, bool indented = true);
    DiagnosticData? DeserializeFromJson(string json);
    string GenerateHtmlReport(DiagnosticData data, int daysBackForEvents, int maxEvents = 100);
}

public class ReportService : IReportService
{
    private const int EventMessageMaxLength = 100;
    private static readonly JsonSerializerOptions IndentedOptions = new() { WriteIndented = true };
    private static readonly JsonSerializerOptions CompactOptions = new() { WriteIndented = false };

    public string SerializeToJson(DiagnosticData data, bool indented = true)
    {
        try
        {
            return JsonSerializer.Serialize(
                data,
                indented ? IndentedOptions : CompactOptions);
        }
        catch (JsonException ex)
        {
            throw ReportException.Serialization(ex.Message);
        }
    }

    public DiagnosticData? DeserializeFromJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw ReportException.InvalidData("JSON 内容为空");
        }

        try
        {
            return JsonSerializer.Deserialize<DiagnosticData>(json);
        }
        catch (JsonException ex)
        {
            throw ReportException.Serialization(ex.Message);
        }
    }

    public string GenerateHtmlReport(DiagnosticData data, int daysBackForEvents, int maxEvents = 100)
    {
        var sb = new StringBuilder();

        AppendDocumentStart(sb, data.CollectedAt);
        AppendOverviewSection(sb, data.Hardware);
        AppendPerformanceSection(sb, data.Performance);
        AppendGpuSection(sb, data.Hardware.Gpus);
        AppendEventsSection(sb, data.Events, daysBackForEvents, maxEvents);
        AppendDocumentEnd(sb);

        return sb.ToString();
    }

    private static void AppendDocumentStart(StringBuilder sb, DateTime collectedAt)
    {
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang='zh-CN'>");
        sb.AppendLine("<head>");
        sb.AppendLine("    <meta charset='UTF-8'>");
        sb.AppendLine("    <meta name='viewport' content='width=device-width, initial-scale=1.0'>");
        sb.AppendLine("    <title>DigYourWindows 诊断报告</title>");
        sb.AppendLine("    <style>");
        sb.AppendLine("        *, *::before, *::after { box-sizing: border-box; }");
        sb.AppendLine("        body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; padding: 20px; background: #f5f5f5; color: #212529; line-height: 1.5; }");
        sb.AppendLine("        h1, h3, h5 { margin-top: 0; }");
        sb.AppendLine("        .row { display: flex; flex-wrap: wrap; margin: 0 -8px; }");
        sb.AppendLine("        .col-md-3 { flex: 0 0 25%; max-width: 25%; padding: 0 8px; }");
        sb.AppendLine("        .card { background: #fff; border: 1px solid #dee2e6; border-radius: 8px; margin-bottom: 20px; }");
        sb.AppendLine("        .card-header { padding: 12px 16px; background: #f8f9fa; border-bottom: 1px solid #dee2e6; border-radius: 8px 8px 0 0; }");
        sb.AppendLine("        .card-body { padding: 16px; }");
        sb.AppendLine("        .text-center { text-align: center; }");
        sb.AppendLine("        .p-3 { padding: 1rem; }");
        sb.AppendLine("        .mb-3 { margin-bottom: 1rem; }");
        sb.AppendLine("        .mb-4 { margin-bottom: 1.5rem; }");
        sb.AppendLine("        .mt-4 { margin-top: 1.5rem; }");
        sb.AppendLine("        .metric { font-size: 1.5rem; font-weight: bold; }");
        sb.AppendLine("        .text-danger { color: #dc3545; }");
        sb.AppendLine("        .text-warning { color: #ffc107; }");
        sb.AppendLine("        .badge { display: inline-block; padding: 4px 8px; font-size: 0.75rem; border-radius: 4px; }");
        sb.AppendLine("        .bg-secondary { background: #6c757d; color: #fff; }");
        sb.AppendLine("        table { width: 100%; border-collapse: collapse; margin-bottom: 1rem; }");
        sb.AppendLine("        th, td { padding: 8px 12px; text-align: left; border-bottom: 1px solid #dee2e6; }");
        sb.AppendLine("        thead th { background: #f8f9fa; font-weight: 600; }");
        sb.AppendLine("        .table-sm th, .table-sm td { padding: 4px 8px; }");
        sb.AppendLine("        .table-striped tbody tr:nth-child(odd) { background: #f8f9fa; }");
        sb.AppendLine("        ul { padding-left: 1.5rem; }");
        sb.AppendLine("        li { margin-bottom: 4px; }");
        sb.AppendLine("        @media (max-width: 768px) { .col-md-3 { flex: 0 0 50%; max-width: 50%; } }");
        sb.AppendLine("    </style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine(CultureInfo.InvariantCulture, $"    <h1 class='mb-4'>Windows 诊断报告 - {collectedAt.ToLocalTime():yyyy-MM-dd HH:mm:ss}</h1>");
    }

    private static void AppendOverviewSection(StringBuilder sb, HardwareData hardware)
    {
        sb.AppendLine("    <div class='card'>");
        sb.AppendLine("        <div class='card-header'><h3>系统概览</h3></div>");
        sb.AppendLine("        <div class='card-body'>");
        sb.AppendLine("            <div class='row'>");
        sb.AppendLine(CultureInfo.InvariantCulture, $"                <div class='col-md-3'><strong>计算机名:</strong> {WebUtility.HtmlEncode(hardware.ComputerName)}</div>");
        sb.AppendLine(CultureInfo.InvariantCulture, $"                <div class='col-md-3'><strong>操作系统:</strong> {WebUtility.HtmlEncode(hardware.OsVersion)}</div>");
        sb.AppendLine(CultureInfo.InvariantCulture, $"                <div class='col-md-3'><strong>CPU:</strong> {WebUtility.HtmlEncode(hardware.CpuName)}</div>");
        sb.AppendLine(CultureInfo.InvariantCulture, $"                <div class='col-md-3'><strong>内存:</strong> {hardware.TotalMemoryMB} MB</div>");
        sb.AppendLine("            </div>");
        sb.AppendLine("        </div>");
        sb.AppendLine("    </div>");
    }

    private static void AppendPerformanceSection(StringBuilder sb, PerformanceAnalysisData? performance)
    {
        if (performance is null)
        {
            return;
        }

        sb.AppendLine("    <div class='card'>");
        sb.AppendLine("        <div class='card-header'><h3>系统性能分析</h3></div>");
        sb.AppendLine("        <div class='card-body'>");
        sb.AppendLine("            <div class='row mb-3'>");
        AppendMetricCard(sb, "系统健康评分", $"{performance.SystemHealthScore:F0}/100", performance.HealthColor, performance.HealthGrade);
        AppendMetricCard(sb, "稳定性评分", $"{performance.StabilityScore:F0}/100");
        AppendMetricCard(sb, "性能评分", $"{performance.PerformanceScore:F0}/100");
        AppendMetricCard(sb, "内存评分", $"{performance.MemoryUsageScore:F0}/100");
        sb.AppendLine("            </div>");
        sb.AppendLine("            <div class='row'>");
        AppendMetricCard(sb, "磁盘健康", $"{performance.DiskHealthScore:F0}/100");
        AppendMetricCard(sb, "关键问题", performance.CriticalIssuesCount.ToString(System.Globalization.CultureInfo.InvariantCulture), valueClass: "text-danger");
        AppendMetricCard(sb, "警告数量", performance.WarningsCount.ToString(System.Globalization.CultureInfo.InvariantCulture), valueClass: "text-warning");
        AppendMetricCard(sb, "系统运行时间", FormatUptime(performance.SystemUptimeDays));
        sb.AppendLine("            </div>");

        if (performance.Recommendations.Count > 0)
        {
            sb.AppendLine("            <div class='mt-4'>");
            sb.AppendLine("                <h5>优化建议</h5>");
            sb.AppendLine("                <ul>");
            foreach (var recommendation in performance.Recommendations)
            {
                sb.AppendLine(System.Globalization.CultureInfo.InvariantCulture, $"                    <li>{WebUtility.HtmlEncode(recommendation)}</li>");
            }
            sb.AppendLine("                </ul>");
            sb.AppendLine("            </div>");
        }

        sb.AppendLine("        </div>");
        sb.AppendLine("    </div>");
    }

    private static void AppendMetricCard(
        StringBuilder sb,
        string title,
        string value,
        string? valueColor = null,
        string? badge = null,
        string? valueClass = null)
    {
        var encodedTitle = WebUtility.HtmlEncode(title);
        var encodedValue = WebUtility.HtmlEncode(value);
        var encodedBadge = badge is null ? null : WebUtility.HtmlEncode(badge);
        var colorAttribute = string.IsNullOrWhiteSpace(valueColor)
            ? string.Empty
            : $" style='color: {WebUtility.HtmlEncode(valueColor)}'";
        var classAttribute = string.IsNullOrWhiteSpace(valueClass)
            ? "metric"
            : $"metric {valueClass}";

        sb.AppendLine("                <div class='col-md-3'>");
        sb.AppendLine("                    <div class='card text-center p-3'>");
        sb.AppendLine(CultureInfo.InvariantCulture, $"                        <h5>{encodedTitle}</h5>");
        sb.AppendLine(CultureInfo.InvariantCulture, $"                        <div class='{classAttribute}'{colorAttribute}>{encodedValue}</div>");
        if (encodedBadge is not null)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"                        <span class='badge bg-secondary'>{encodedBadge}</span>");
        }
        sb.AppendLine("                    </div>");
        sb.AppendLine("                </div>");
    }

    private static void AppendGpuSection(StringBuilder sb, List<GpuInfoData> gpus)
    {
        if (gpus.Count == 0)
        {
            return;
        }

        sb.AppendLine("    <div class='card'>");
        sb.AppendLine("        <div class='card-header'><h3>GPU 信息</h3></div>");
        sb.AppendLine("        <div class='card-body'>");
        sb.AppendLine("            <table class='table'>");
        sb.AppendLine("                <thead><tr><th>名称</th><th>温度</th><th>负载</th><th>显存</th><th>核心频率</th><th>功耗</th></tr></thead>");
        sb.AppendLine("                <tbody>");
        foreach (var gpu in gpus)
        {
            sb.AppendLine(System.Globalization.CultureInfo.InvariantCulture, $"                    <tr><td>{WebUtility.HtmlEncode(gpu.Name)}</td><td>{gpu.Temperature:F1}°C</td><td>{gpu.Load:F1}%</td><td>{gpu.MemoryUsed:F0}/{gpu.MemoryTotal:F0} MB</td><td>{gpu.CoreClock:F0} MHz</td><td>{gpu.Power:F1} W</td></tr>");
        }
        sb.AppendLine("                </tbody>");
        sb.AppendLine("            </table>");
        sb.AppendLine("        </div>");
        sb.AppendLine("    </div>");
    }

    private static void AppendEventsSection(StringBuilder sb, List<LogEventData> events, int daysBackForEvents, int maxEvents)
    {
        sb.AppendLine("    <div class='card'>");
        sb.AppendLine(CultureInfo.InvariantCulture, $"        <div class='card-header'><h3>错误日志 (最近{daysBackForEvents}天) - {events.Count} 条</h3></div>");
        sb.AppendLine("        <div class='card-body'>");
        sb.AppendLine("            <table class='table table-sm table-striped'>");
        sb.AppendLine("                <thead><tr><th>时间</th><th>来源</th><th>类型</th><th>ID</th><th>消息</th></tr></thead>");
        sb.AppendLine("                <tbody>");
        foreach (var evt in events.Take(Math.Max(0, maxEvents)))
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"                    <tr><td>{evt.TimeGenerated:yyyy-MM-dd HH:mm}</td><td>{WebUtility.HtmlEncode(evt.SourceName)}</td><td>{WebUtility.HtmlEncode(evt.EventType)}</td><td>{evt.EventId}</td><td>{WebUtility.HtmlEncode(TruncateMessage(evt.Message))}</td></tr>");
        }
        sb.AppendLine("                </tbody>");
        sb.AppendLine("            </table>");
        sb.AppendLine("        </div>");
        sb.AppendLine("    </div>");
    }

    private static string TruncateMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return string.Empty;
        }

        return message.Length <= EventMessageMaxLength
            ? message
            : message[..EventMessageMaxLength];
    }

    private static string FormatUptime(double? uptimeDays)
    {
        return uptimeDays.HasValue
            ? $"{uptimeDays.Value:F0} 天"
            : "未知";
    }

    private static void AppendDocumentEnd(StringBuilder sb)
    {
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
    }
}
