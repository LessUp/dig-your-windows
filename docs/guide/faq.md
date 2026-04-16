# FAQ 常见问题

## 安装与运行

### Q: 运行时提示"需要管理员权限"？

**A**: 以下功能需要管理员权限：
- GPU 温度/负载监控
- 磁盘 SMART 数据读取
- 部分硬件信息采集

**解决方案**: 右键点击应用程序，选择"以管理员身份运行"。

### Q: 启动时提示缺少 .NET Runtime？

**A**: 框架依赖版本（FDD）需要安装 .NET 10 Runtime。

**解决方案**:
1. 下载自包含版本（SCD），无需安装 .NET
2. 或从 [Microsoft 官网](https://dotnet.microsoft.com/download) 安装 .NET Desktop Runtime

### Q: Windows Defender 报告应用为病毒？

**A**: 这是误报，因为应用使用了 WMI 和硬件监控 API。

**解决方案**:
1. 在 Windows Defender 中添加排除项
2. 或仅在使用时临时禁用实时保护

---

## 功能相关

### Q: 为什么 GPU 温度显示为 N/A？

**A**: 可能的原因：
1. 未以管理员身份运行
2. GPU 驱动不支持监控 API
3. 虚拟机中的 GPU 可能不支持

**解决方案**:
1. 以管理员身份运行应用
2. 更新 GPU 驱动程序

### Q: SMART 数据显示不完整？

**A**: 部分磁盘控制器或外接硬盘可能不支持 SMART。

**支持的磁盘类型**:
- SATA SSD/HDD
- NVMe SSD
- 部分外接 USB 硬盘（取决于控制器）

### Q: 可靠性记录为什么是空的？

**A**: 可能的原因：
1. Windows 可靠性监视器数据已被清理
2. 系统刚重装不久，没有历史记录
3. 查询时间范围设置过大

**解决方案**:
- 调整"回溯天数"参数
- 检查事件日志服务是否正常运行

### Q: 导出的 HTML 报告打开很慢？

**A**: 大量事件日志会导致 HTML 文件较大。

**优化方案**:
1. 减少"最大事件数"设置
2. 使用 JSON 格式导出
3. 使用现代浏览器打开

---

## 故障排除

### Q: 应用启动后闪退？

**A**: 检查以下几点：
1. 查看 `%LOCALAPPDATA%\DigYourWindows\Logs\` 中的日志文件
2. 确认 .NET 版本正确
3. 尝试删除配置文件后重新启动

### Q: CPU/GPU 监控数值不更新？

**A**: 监控服务可能未正确初始化。

**解决方案**:
1. 以管理员身份重启应用
2. 检查日志中的错误信息
3. 确认 LibreHardwareMonitor 依赖正常加载

### Q: 导入 JSON 文件失败？

**A**: JSON 文件格式可能不正确或版本不兼容。

**解决方案**:
1. 确认 JSON 文件来自相同版本
2. 使用文本编辑器检查 JSON 格式
3. 查看日志中的具体错误信息

---

## 开发相关

### Q: 如何调试硬件监控功能？

**A**:
1. 使用 Visual Studio 以管理员身份启动调试
2. 在 `.vscode/launch.json` 中配置 `"runAsAdministrator": true`
3. 设置断点在 `CpuMonitorService` 或 `GpuMonitorService`

### Q: 测试覆盖率报告在哪里？

**A**:
```powershell
# 收集覆盖率
dotnet test --collect:"XPlat Code Coverage"

# 报告位置
# tests/DigYourWindows.Tests/TestResults/<guid>/coverage.cobertura.xml
```

### Q: 如何添加新的硬件监控指标？

**A**:
1. 在 `HardwareMonitorProvider` 中启用相应的硬件类型
2. 在对应的服务中添加指标采集逻辑
3. 更新 `HardwareData` 或相关数据模型
4. 添加单元测试

---

## 其他问题

### Q: 支持哪些操作系统？

**A**:
- Windows 10 (Build 19041+)
- Windows 11

不支持：
- Windows 7/8
- Linux/macOS（本项目为 Windows 专用工具）

### Q: 数据会上传到云端吗？

**A**: 不会。所有数据均在本地采集、存储和导出。应用不会将任何数据发送到外部服务器。

### Q: 如何获取最新版本？

**A**:
1. 访问 [Releases 页面](https://github.com/LessUp/dig-your-windows/releases)
2. 下载最新版本的安装包或便携版
3. 或克隆源码自行构建

### Q: 如何反馈问题或建议？

**A**:
- [GitHub Issues](https://github.com/LessUp/dig-your-windows/issues) - Bug 报告和功能请求
- [GitHub Discussions](https://github.com/LessUp/dig-your-windows/discussions) - 一般讨论和问题
