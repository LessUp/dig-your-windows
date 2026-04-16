# 快速开始

本指南帮助你快速搭建开发环境并运行 DigYourWindows。

## 环境要求

### 必需依赖

| 依赖项 | 版本要求 | 用途 | 下载链接 |
|--------|----------|------|----------|
| Windows | 10/11 (Build 19041+) | 运行平台 | - |
| .NET SDK | 10.0.x | 构建和运行 | [下载](https://dotnet.microsoft.com/download) |
| IDE（可选）| VS 2022 / Rider / VS Code | 开发调试 | - |

### 验证安装

::: code-group

```powershell [PowerShell]
# 检查 .NET 版本（应显示 10.0.x）
dotnet --version

# 检查 Windows 版本
[Environment]::OSVersion.Version
```

```cmd [CMD]
# 检查 .NET 版本
dotnet --version

# 检查 Windows 版本
ver
```

:::

## 获取源码

### 方式一：克隆仓库（推荐）

```powershell
# 使用 HTTPS
git clone https://github.com/LessUp/dig-your-windows.git

# 或使用 SSH
git clone git@github.com:LessUp/dig-your-windows.git

# 进入项目目录
cd dig-your-windows
```

### 方式二：下载 ZIP

```powershell
# 下载最新源码
Invoke-WebRequest -Uri "https://github.com/LessUp/dig-your-windows/archive/refs/heads/master.zip" -OutFile "dig-your-windows.zip"

# 解压
Expand-Archive -Path "dig-your-windows.zip" -DestinationPath "."
```

## 构建项目

### 1. 还原依赖

```powershell
dotnet restore DigYourWindows.slnx
```

::: tip NuGet 配置
项目使用 NuGet 官方源。如需配置代理或私有源，请编辑 `NuGet.Config`。
:::

### 2. 编译项目

::: code-group

```powershell [Debug 版本]
dotnet build DigYourWindows.slnx
```

```powershell [Release 版本]
dotnet build DigYourWindows.slnx -c Release
```

:::

构建输出位置：
- Debug: `src/DigYourWindows.UI/bin/Debug/net10.0-windows/`
- Release: `src/DigYourWindows.UI/bin/Release/net10.0-windows/`

## 运行应用

### 基本运行

```powershell
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

### 管理员权限运行

::: warning 重要提示
以下功能需要管理员权限才能正常工作：
- GPU 温度/负载监控
- 磁盘 SMART 数据读取
- 部分硬件信息采集
:::

#### 方式一：右键以管理员身份运行

1. 打开资源管理器，导航到 `src/DigYourWindows.UI/bin/Debug/net10.0-windows/`
2. 右键点击 `DigYourWindows.UI.exe`
3. 选择"以管理员身份运行"

#### 方式二：VS Code 配置

在 `.vscode/launch.json` 中添加管理员配置：

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Run as Admin",
      "type": "coreclr",
      "request": "launch",
      "program": "${workspaceFolder}/src/DigYourWindows.UI/bin/Debug/net10.0-windows/DigYourWindows.UI.dll",
      "args": [],
      "cwd": "${workspaceFolder}",
      "stopAtEntry": false,
      "console": "internalConsole",
      "windows": {
        "runAsAdministrator": true
      }
    }
  ]
}
```

#### 方式三：Visual Studio

1. 在 Visual Studio 中打开项目
2. 右键点击 `DigYourWindows.UI` 项目 → "属性"
3. 转到"调试"选项卡
4. 勾选"启用本机代码调试"（可选）
5. 启动 Visual Studio 时选择"以管理员身份运行"

## 运行测试

### 运行所有测试

```powershell
dotnet test DigYourWindows.slnx
```

### 详细输出

```powershell
dotnet test DigYourWindows.slnx --logger "console;verbosity=detailed"
```

### 过滤测试

```powershell
# 按类名过滤
dotnet test --filter "FullyQualifiedName~ReportServiceTests"

# 按方法名过滤
dotnet test --filter "FullyQualifiedName~SerializeToJson"
```

### 代码覆盖率

```powershell
# 收集覆盖率数据
dotnet test --collect:"XPlat Code Coverage"

# 使用 ReportGenerator 生成 HTML 报告
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coverage
```

覆盖率报告位置：`tests/DigYourWindows.Tests/TestResults/{guid}/coverage.cobertura.xml`

## 发布应用

### 使用发布脚本（推荐）

```powershell
# 生成框架依赖 (FDD) 和自包含 (SCD) 两个版本
.\scripts\publish.ps1
```

输出目录：
- `artifacts/publish/FDD/` — 框架依赖版本（64MB，需要 .NET Runtime）
- `artifacts/publish/SCD/` — 自包含版本（185MB，无需依赖）

### 构建安装程序

::: info 前置要求
需要安装 [Inno Setup 6](https://jrsoftware.org/isinfo.php)
:::

```powershell
.\scripts\build-installer.ps1
```

输出：`artifacts/installer/DigYourWindows_Setup.exe`

### 手动发布

#### 框架依赖版本

```powershell
dotnet publish src/DigYourWindows.UI/DigYourWindows.UI.csproj `
  -c Release `
  -o artifacts/publish/FDD
```

#### 自包含版本

```powershell
dotnet publish src/DigYourWindows.UI/DigYourWindows.UI.csproj `
  -c Release `
  -r win-x64 `
  --self-contained true `
  -p:PublishSingleFile=true `
  -o artifacts/publish/SCD
```

## 发布新版本

### 自动发布流程

推送 `v*` 格式的 Git tag 会自动触发 GitHub Actions 构建并发布：

```powershell
# 1. 更新版本号（Directory.Build.props）
# 2. 更新 CHANGELOG.md

# 3. 创建并推送 tag
git add .
git commit -m "chore(release): prepare for v1.1.0"
git tag v1.1.0
git push origin master
git push origin v1.1.0
```

GitHub Actions 会自动：
1. 🏗️ 构建 FDD 和 SCD 版本
2. 📦 创建安装程序
3. 📝 创建 GitHub Release
4. ⬆️ 上传构建产物

### 手动发布

参见 [GitHub Releases](https://github.com/LessUp/dig-your-windows/releases) 页面，手动上传构建产物。

## 故障排除

### 常见问题

#### Q: 提示"无法启动程序，因为计算机中丢失 xxx.dll"

**解决方案:**
```powershell
# 重新还原依赖
dotnet restore --force

# 清理并重新构建
dotnet clean
dotnet build
```

#### Q: GPU 温度显示 N/A

**解决方案:**
1. 以管理员身份运行应用
2. 更新显卡驱动
3. 检查 LibreHardwareMonitor 是否支持你的 GPU

#### Q: 构建时出现警告 MSB3270

这是混合平台警告，不影响功能。如需消除，确保所有项目使用相同的目标平台。

#### Q: 测试失败：找不到测试宿主

**解决方案:**
```powershell
# 重新安装测试 SDK
dotnet new install Microsoft.NET.Test.Sdk

# 或者清理后重新构建
dotnet clean
dotnet build
```

### 日志位置

应用日志保存在以下位置，可用于故障诊断：

```
%LOCALAPPDATA%\DigYourWindows\Logs\
```

日志文件命名格式：`DigYourWindows_YYYYMMDD.log`

## 下一步

- [项目架构](/zh-CN/guide/architecture) — 了解技术架构和设计决策
- [测试指南](/zh-CN/guide/testing) — 学习如何编写和运行测试
- [贡献指南](/zh-CN/guide/contributing) — 参与项目开发
- [数据 Schema](/zh-CN/reference/data-schema) — 查看诊断数据格式
