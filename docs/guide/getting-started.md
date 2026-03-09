# 快速开始

## 环境要求

- Windows 10/11
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)

## 构建

```powershell
dotnet restore DigYourWindows.slnx
dotnet build DigYourWindows.slnx -c Release
```

## 运行

```powershell
dotnet run --project src/DigYourWindows.UI/DigYourWindows.UI.csproj
```

::: tip
部分功能（如 GPU 监控、SMART 数据）需要以管理员权限运行。
:::

## 测试

```powershell
dotnet test DigYourWindows.slnx
```

## 发布

```powershell
# 使用发布脚本（生成 FDD 和 SCD 两个版本）
.\scripts\publish.ps1

# 构建安装包（需要 Inno Setup 6）
.\scripts\build-installer.ps1
```

## Release

推送 `v*` 格式的 tag 会自动触发 GitHub Actions 构建并发布 Release：

```powershell
git tag v1.0.0
git push origin v1.0.0
```
