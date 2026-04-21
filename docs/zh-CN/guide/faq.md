# FAQ 常见问题

快速找到解决方案，使用 <kbd>Ctrl</kbd> + <kbd>F</kbd> 搜索关键词。

## 安装与运行 {#installation}

### 运行时提示"需要管理员权限"？

**A**: 以下功能需要管理员权限：
- GPU 温度/负载监控
- 磁盘 SMART 数据读取
- 部分硬件信息采集

**解决方案**:
1. 右键点击应用程序，选择"以管理员身份运行"
2. 或在 `.vscode/launch.json` 中配置 `"runAsAdministrator": true`

### 启动时提示缺少 .NET Runtime？

**A**: 框架依赖版本（FDD）需要 .NET 10 Runtime。

**解决方案**:

| 方法 | 步骤 |
|------|------|
| **方法一** | 下载自包含版本（SCD），无需安装 .NET |
| **方法二** | 从 [Microsoft 官网](https://dotnet.microsoft.com/download) 安装 .NET Desktop Runtime 10 |

### Windows Defender 报告应用为病毒？

**A**: 这是误报，因为应用使用了 WMI 和硬件监控 API。

**解决方案**:
1. 在 Windows Defender 中添加排除项
2. 使用 [VirusTotal](https://www.virustotal.com) 验证安装程序
3. 如需要可临时禁用实时保护

## 功能相关 {#features}

### 为什么 GPU 温度显示为 N/A？

**A**: 可能的原因：

| 原因 | 解决方案 |
|------|----------|
| 未以管理员身份运行 | 以管理员身份运行应用 |
| GPU 驱动不支持监控 API | 更新显卡驱动 |
| 虚拟机环境 | VM 中 GPU 监控可能无法工作 |
| LibreHardwareMonitor 不支持 | 查看[兼容性列表](https://github.com/LibreHardwareMonitor/LibreHardwareMonitor) |

### SMART 数据显示不完整？

**A**: 部分磁盘控制器或外接硬盘可能不支持 SMART。

**支持的磁盘类型**:
- ✅ SATA SSD/HDD
- ✅ NVMe SSD
- ⚠️ 部分外接 USB 硬盘（取决于控制器）

### 可靠性记录为什么是空的？

**A**: 可能的原因：
1. Windows 可靠性监视器数据已被清理
2. 系统刚重装不久，没有历史记录
3. 查询时间范围过小

**解决方案**:
- 调整"回溯天数"参数（默认 7 天，最大 90 天）
- 在控制面板中检查可靠性监视器是否有数据
- 确认事件日志服务正在运行

### 导出的 HTML 报告打开很慢？

**A**: 大量事件日志会导致 HTML 文件较大。

**优化方案**:
1. 减少"最大事件数"设置（默认 1000）
2. 使用 JSON 格式导出（更小、更快）
3. 使用现代浏览器（Chrome/Edge/Firefox）
4. 减少回溯天数范围

## 故障排除 {#troubleshooting}

### 应用启动后闪退？

**A**: 排查步骤：

**第一步**：检查日志
```
%LOCALAPPDATA%\DigYourWindows\Logs\DigYourWindows_YYYYMMDD.log
```

**第二步**：常见问题及解决方案
| 症状 | 原因 | 解决方案 |
|------|------|----------|
| 立即退出 | .NET 版本不匹配 | 重新安装 .NET 10 SDK |
| 加载时 UI 卡死 | 硬件监控初始化失败 | 以管理员身份运行 |
| 内存不足 | 系统内存过低 | 关闭其他应用程序 |

**第三步**：重置应用
```powershell
# 删除配置文件（重置所有设置）
Remove-Item -Path "$env:LOCALAPPDATA\DigYourWindows\settings.json" -Force
```

### CPU/GPU 监控数值不更新？

**A**: 监控服务可能未正确初始化。

**排查方法**:
1. 以管理员身份重启应用
2. 检查日志中的错误信息（搜索 "HardwareMonitor"）
3. 确认 LibreHardwareMonitor 依赖正常加载
4. 尝试禁用并重新启用监控（点击刷新）

### 导入 JSON 文件失败？

**A**: JSON 文件格式可能不正确或版本不兼容。

**解决方案**:
1. 确认 JSON 来自相同或兼容版本
2. 使用 [JSONLint](https://jsonlint.com) 验证 JSON 格式
3. 查看日志中的具体错误信息
4. 确认文件完整（未截断）

## 开发相关 {#development}

### 如何调试硬件监控功能？

**A**: 调试方法：

**方法一：Visual Studio 调试**
1. 以管理员身份启动 Visual Studio
2. 在 `CpuMonitorService.cs` 或 `GpuMonitorService.cs` 设置断点
3. 按 F5 开始调试

**方法二：VS Code 配置**
```json
{
  "name": "Debug Hardware",
  "type": "coreclr",
  "request": "launch",
  "program": "${workspaceFolder}/src/DigYourWindows.UI/bin/Debug/net10.0-windows/DigYourWindows.UI.dll",
  "windows": { "runAsAdministrator": true }
}
```

### 测试覆盖率报告在哪里？

**A**: 覆盖率报告生成步骤：

```powershell
# 1. 收集覆盖率
dotnet test --collect:"XPlat Code Coverage"

# 2. 报告位置
tests/DigYourWindows.Tests/TestResults/{guid}/coverage.cobertura.xml

# 3. 生成 HTML 报告（可选）
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage_report
```

### 如何添加新的硬件监控指标？

**步骤**:
1. 在 `HardwareMonitorProvider` 中启用相应的硬件类型
2. 在对应的服务中添加指标采集逻辑
3. 更新数据模型
4. 添加单元测试

## 其他问题 {#other}

### 支持哪些操作系统？

**A**:

| 操作系统 | 支持状态 | 说明 |
|----------|----------|------|
| Windows 11 | ✅ 完全支持 | 推荐 |
| Windows 10 (Build 19041+) | ✅ 完全支持 | 需要 20H1 或更高版本 |
| Windows 10 (Build < 19041) | ⚠️ 有限支持 | 部分功能不可用 |
| Windows 7/8 | ❌ 不支持 | .NET 10 不支持 |
| Linux/macOS | ❌ 不支持 | Windows 专用工具 |

### 数据会上传到云端吗？

**A**: **不会**。所有数据均在本地采集、存储和导出：
- ✅ 数据保存到本地文件系统
- ✅ 导出文件由用户控制
- ✅ 无远程服务器连接
- ✅ 开源可审计

### 如何获取最新版本？

**A**:

**方法一**：GitHub Releases（推荐）
```
https://github.com/LessUp/dig-your-windows/releases/latest
```

**方法二**：从源码构建
```powershell
git pull
dotnet build
dotnet run
```

### 如何反馈问题或建议？

| 渠道 | 用途 | 链接 |
|------|------|------|
| GitHub Issues | Bug 报告、功能请求 | [创建 Issue](https://github.com/LessUp/dig-your-windows/issues/new/choose) |
| GitHub Discussions | 一般讨论 | [Discussions](https://github.com/LessUp/dig-your-windows/discussions) |

**Bug 报告应包含**:
- 操作系统版本
- .NET 版本（`dotnet --version`）
- 应用版本（查看关于页面）
- 复现步骤
- 相关日志片段

## 快速参考 {#reference}

### 快捷键

| 快捷键 | 功能 |
|--------|------|
| <kbd>F5</kbd> | 刷新诊断数据 |
| <kbd>Ctrl</kbd> + <kbd>S</kbd> | 保存报告 |
| <kbd>Ctrl</kbd> + <kbd>T</kbd> | 切换主题 |
| <kbd>Esc</kbd> | 取消正在进行的采集 |

### 健康评分等级

| 分数 | 等级 | 颜色 | 状态 | 建议 |
|------|------|------|------|------|
| 90-100 | 优秀 | `#28a745` 🟢 | 最佳 | 保持现状 |
| 75-89 | 良好 | `#17a2b8` 🔵 | 正常 | 定期维护 |
| 60-74 | 一般 | `#ffc107` 🟡 | 有问题 | 密切关注 |
| 40-59 | 较差 | `#fd7e14` 🟠 | 需注意 | 尽快优化 |
| < 40 | 需优化 | `#dc3545` 🔴 | 严重 | 立即处理 |

---

**没找到答案？** [创建 Issue](https://github.com/LessUp/dig-your-windows/issues/new) 或查看[完整文档](/zh-CN/guide/)。
