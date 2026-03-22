# 第二阶段：编译恢复、流程收敛与依赖清理

日期：2026-03-22

## 变更内容

- 对 `MainViewModel` 做第二阶段保守收敛：
  - 继续提炼网络流量采样 helper（首次采样初始化、速率计算、采样状态更新）
  - 统一导出公共流程，收敛 JSON / HTML 导出中的重复状态与文件处理逻辑
  - 提炼加载完成、导入完成、导出成功、主题切换等状态文案 helper
- 对图表代码做保守兼容处理：
  - 将时间序列 X 轴转换为 `ToOADate()` 数值轴
  - 暂时将 `ApplyPlotTheme(...)` 缩减为空实现，以降低 `ScottPlot.WPF 5.1.57` API 不兼容导致的编译风险
- 收敛 `HardwareService` 中重复的 WMI 执行骨架，统一异常记录与 fallback 模式，同时保持现有字段映射语义不变
- 修正 `ReportServiceTests` 中的断言类型不一致问题，统一为与模型属性一致的 `double` / `int?` 断言
- 清理 UI 项目中未使用的 `LiveChartsCore.SkiaSharpView.WPF` 包引用
- 清理 `MainWindow.xaml` 中未使用的 `xmlns:scottplot` 命名空间声明

## 背景

上一轮优化后，静态检查显示新的主要风险集中在两类：
1. `MainViewModel` 中的 ScottPlot 调用可能与当前锁定的 `ScottPlot.WPF 5.1.57` API 不完全一致；
2. 测试项目里少数 `Assert.Equal(...)` 断言存在字面量类型与实际模型类型不一致的问题。

同时，UI 层和硬件采集层仍有一部分低风险、可直接收敛的重复逻辑；而 `LiveChartsCore.SkiaSharpView.WPF` 已静态确认高度疑似遗留依赖，因此本轮一起收口清理。

## 验证说明

- 已按静态分析优先级完成编译风险保守修复、重复流程收敛和遗留依赖清理
- 当前会话环境仍缺少 `dotnet` 命令，无法直接运行 `dotnet restore/build/test/publish` 做最终验证
- 建议在具备 .NET SDK 的环境中继续执行：
  - `dotnet restore DigYourWindows.slnx`
  - `dotnet build DigYourWindows.slnx -c Release --no-restore`
  - `dotnet test DigYourWindows.slnx -c Release --no-build`
  - `dotnet publish src/DigYourWindows.UI/DigYourWindows.UI.csproj -c Release`

## 已知取舍

- 为优先恢复可编译，本轮对 `ScottPlot` 主题细节采取保守策略，先缩减 `ApplyPlotTheme(...)`，以保证图表主流程更稳定
- 依赖清理仅删除了静态确认未使用的 `LiveChartsCore.SkiaSharpView.WPF`，保留 `ScottPlot.WPF` 作为当前实际图表实现
