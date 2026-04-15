# 快速开始

本指南帮助你快速搭建开发环境并运行 DigYourWindows。

## 环境要求

| 依赖 | 版本 | 说明 |
|------|------|------|
| 操作系统 | Windows 10/11 | 目标平台 |
| .NET SDK | 10.0 | [下载地址](https://dotnet.microsoft.com/download) |
| IDE | VS 2022 / Rider / VS Code | 任选其一 |

::: tip 验证安装
```powershell
dotnet --version  # 应显示 10.0.x
```
:::

## 获取源码

```powershell
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows
```

## 构建项目

### 还原依赖

```powershell
dotnet restore DigYourWindows.slnx
```

### 编译

```powershell
# Debug 版本
dotnet build DigYourWindows.slnx

# Release 版本
dotnet build DigYourWindows.slnx -c Release
```

## 运行应用

```powershell
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

::: warning 管理员权限
以下功能需要以管理员身份运行：
- GPU 温度/负载监控
- 磁盘 SMART 数据读取
- 部分硬件信息采集
:::

### 以管理员身份运行（VS Code）

在 `.vscode/launch.json` 中配置：

```json
{
  "name": "Run as Admin",
  "type": "coreclr",
  "request": "launch",
  "program": "${workspaceFolder}/src/DigYourWindows.UI/bin/Debug/net10.0-windows/DigYourWindows.UI.dll",
  "args": [],
  "windows": {
    "runAsAdministrator": true
  }
}
```

## 运行测试

```powershell
# 运行所有测试
dotnet test DigYourWindows.slnx

# 详细输出
dotnet test --logger "console;verbosity=detailed"

# 运行特定测试类
dotnet test --filter "FullyQualifiedName~ReportServiceTests"

# 收集代码覆盖率
dotnet test --collect:"XPlat Code Coverage"
```

## 发布应用

### 使用发布脚本

```powershell
# 生成框架依赖版本 (FDD) 和自包含版本 (SCD)
.\scripts\publish.ps1
```

输出目录：
- `publish/FDD/` — 框架依赖版本（需要安装 .NET 10 Runtime）
- `publish/SCD/` — 自包含版本（无需安装 .NET）

### 构建安装包

```powershell
# 需要 Inno Setup 6
.\scripts\build-installer.ps1
```

输出：`installer/Output/DigYourWindows_Setup.exe`

### 手动发布

```powershell
# 框架依赖版本
dotnet publish src/DigYourWindows.UI/DigYourWindows.UI.csproj \
  -c Release \
  -o publish/FDD

# 自包含版本 (Windows x64)
dotnet publish src/DigYourWindows.UI/DigYourWindows.UI.csproj \
  -c Release \
  -r win-x64 \
  --self-contained true \
  -o publish/SCD
```

## 发布新版本

推送 `v*` 格式的 Git tag 会自动触发 GitHub Actions 构建并发布：

```powershell
# 创建 tag
git tag v1.0.0

# 推送 tag
git push origin v1.0.0
```

GitHub Actions 会自动：
1. 构建框架依赖版本和自包含版本
2. 创建 GitHub Release
3. 上传构建产物

## 下一步

- [项目架构](/guide/architecture) — 了解技术架构和设计决策
- [测试指南](/guide/testing) — 学习如何编写和运行测试
- [数据 Schema](/reference/data-schema) — 查看诊断数据格式
