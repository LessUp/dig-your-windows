# 快速开始

本指南帮助你在 Windows 机器上安装和运行 DigYourWindows。

## 环境要求

开始之前，请确保你有：

- **Windows 10/11**（Build 19041 或更高版本）
- **.NET 10.0 SDK**（从源码构建需要）
- **管理员权限**（完整功能访问需要）

## 安装方式

### 方式一：下载安装包（推荐）

从 [GitHub Releases](https://github.com/LessUp/dig-your-windows/releases) 下载最新版本：

| 版本 | 大小 | 环境要求 | 适用场景 |
|------|------|----------|----------|
| `DigYourWindows_Setup.exe` | ~5MB | 自动下载 .NET | 大多数用户 |
| FDD（框架依赖版） | ~60MB | 需要 .NET 10 Runtime | 已安装 .NET 的用户 |
| SCD（独立版） | ~180MB | 无需依赖 | 离线使用 |

#### 安装步骤

1. 下载安装包
2. 运行 `DigYourWindows_Setup.exe`
3. 按照安装向导操作
4. 从开始菜单启动 DigYourWindows

### 方式二：从源码构建

```powershell
# 克隆仓库
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# 还原依赖
dotnet restore

# 构建并运行
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

> ⚠️ **提示**：部分功能（GPU 监控、SMART 数据）需要管理员权限。

## 首次使用

1. **启动应用程序**：
   - 从开始菜单启动，或
   - 运行 `DigYourWindows.UI.exe`，或
   - 使用 `dotnet run`

2. **运行诊断**：
   - 点击"运行诊断"按钮
   - 等待数据采集完成（通常 5-15 秒）

3. **查看结果**：
   - 在仪表板查看硬件信息
   - 检查事件日志分析
   - 查看可靠性记录
   - 查看健康评分和优化建议

4. **导出报告**（可选）：
   - 点击"导出"按钮
   - 选择 JSON 或 HTML 格式
   - 选择保存位置

## 管理员权限

部分功能需要管理员权限：

- ✅ GPU 温度/负载监控
- ✅ 磁盘 SMART 数据读取
- ✅ 部分硬件信息采集

### 如何以管理员身份运行

**方法一**：右键快捷方式
- 右键点击 `DigYourWindows.UI.exe`
- 选择"以管理员身份运行"

**方法二**：在 Visual Studio 中配置
- 在 Visual Studio 中打开项目
- 编辑 `.vscode/launch.json`
- 添加 `"runAsAdministrator": true`

## 功能使用

### 仪表板概览

启动 DigYourWindows 后，主界面显示：

- **系统信息**：计算机名、操作系统版本、处理器、内存
- **实时数据**：当前 CPU/GPU 指标
- **操作按钮**：运行诊断、导出、设置

### 查看硬件信息

#### CPU 信息

CPU 部分显示：
- 型号名称和规格
- 核心和线程数
- 实时温度和负载
- 当前频率和基准频率

#### 内存

内存信息包括：
- 总内存和可用内存
- 使用百分比
- 内存类型和速度

#### 磁盘

每个磁盘显示：
- 型号和接口类型（NVMe/SATA）
- 健康状态
- 温度（如支持）
- SMART 数据（需要管理员权限）

#### GPU

GPU 监控显示：
- 显卡型号
- 温度和负载
- 显存使用
- 时钟频率（如支持）

### 分析事件日志

事件日志分析部分显示：

- **系统错误**：关键系统故障
- **系统警告**：潜在问题
- **应用程序错误**：软件崩溃
- **应用程序警告**：应用程序问题

### 查看可靠性记录

Windows 可靠性监视器数据显示：
- 稳定性指数趋势（0-10 刻度）
- 历史事件时间线
- 关键事件列表

### 理解健康评分

#### 总分

基于以下维度的 0-100 综合评分：
- **稳定性评分**：系统崩溃频率
- **性能评分**：资源利用率
- **内存评分**：RAM 健康状况
- **磁盘评分**：存储状态

#### 优化建议

AI 生成的建议包括：
- 类别（CPU/GPU/内存/磁盘/系统）
- 优先级（低/中/高/紧急）
- 可操作的建议

### 导出报告

#### JSON 格式

适用于：
- 程序化分析
- 数据归档
- 随时间比较

**步骤**：
1. 点击"导出"
2. 选择"JSON"
3. 选择保存位置
4. 文件命名：`DigYourWindows_Report_[日期].json`

#### HTML 格式

适用于：
- 分享给他人
- 离线查看
- 打印

**步骤**：
1. 点击"导出"
2. 选择"HTML"
3. 选择保存位置
4. 文件命名：`DigYourWindows_Report_[日期].html`

> 📝 **注意**：HTML 报告是自包含的，无外部依赖。

### 主题切换

在深色和浅色主题之间切换：
- 点击主题切换按钮（右上角）
- 偏好设置会保存到下次会话

## 故障排除

### "需要管理员权限"消息

**解决方案**：以管理员身份运行应用程序（见上文）。

### 缺少 .NET Runtime

**解决方案**：
- 下载独立版本（SCD），或
- 从 [Microsoft](https://dotnet.microsoft.com/download) 安装 .NET Desktop Runtime 10

### Windows Defender 警告

**解决方案**：
- 这对于未签名的应用程序很常见
- 在 Windows Defender 中添加例外
- 或自行从源码构建

## 下一步

- 🏗️ 阅读 [项目架构](./architecture.md)
- 🧪 了解 [测试指南](./testing.md)
- 🤝 查看 [贡献指南](https://github.com/LessUp/dig-your-windows/blob/main/CONTRIBUTING.md)
- 📋 查看 [规范文档](https://github.com/LessUp/dig-your-windows/tree/main/specs)
