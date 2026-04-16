# 目录结构重组 & GitHub Release 配置

**日期**: 2025-02-27
**版本**: 0.1.0
**类型**: Infrastructure

---

## 概述

消除无意义目录嵌套，采用标准开源项目结构，配置自动化发布流程。

---

## 目录重组

### 优化前

```
dig-your-windows/
├── DigYourWindows/
│   ├── DigYourWindows_WPF/
│   │   ├── DigYourWindows.Core/
│   │   └── DigYourWindows.UI/
│   └── DigYourWindows_Rust/
└── ...（配置文件分散）
```

### 优化后

```
dig-your-windows/
├── src/
│   ├── DigYourWindows.Core/     # 核心业务逻辑
│   └── DigYourWindows.UI/       # WPF 用户界面
├── tests/
│   └── DigYourWindows.Tests/    # 测试项目
├── docs/                        # 文档和 Schema
├── installer/                   # Inno Setup 脚本
├── scripts/                     # 构建和发布脚本
├── Directory.Build.props        # 共享 MSBuild 属性
├── DigYourWindows.slnx          # Solution 文件
├── NuGet.Config                 # NuGet 配置
├── README.md                    # 项目介绍
└── LICENSE                      # MIT 许可证
```

---

## 路径引用更新

| 文件 | 变更 |
|------|------|
| `DigYourWindows.slnx` | 项目路径更新为 `src/` 和 `tests/` 前缀 |
| `DigYourWindows.Tests.csproj` | ProjectReference 更新为 `../../src/...` |
| `scripts/publish.ps1` | 项目路径更新 |
| `scripts/build-installer.ps1` | 项目路径更新 |
| `.github/workflows/ci.yml` | 移除 `working-directory`，使用 slnx 全量构建 |

---

## GitHub Actions

### CI Workflow

```yaml
# 优化前：指定工作目录
- run: dotnet build
  working-directory: DigYourWindows/DigYourWindows_WPF

# 优化后：使用根目录 slnx
- run: dotnet build DigYourWindows.slnx
```

### Release Workflow

新增 `release.yml`，推送 `v*` tag 自动发布：

```yaml
on:
  push:
    tags:
      - 'v*'

jobs:
  release:
    runs-on: windows-latest
    steps:
      - name: Build
        run: |
          dotnet publish -c Release -r win-x64 --self-contained false
          dotnet publish -c Release -r win-x64 --self-contained true

      - name: Create Release
        uses: softprops/action-gh-release@v1
        with:
          files: |
            publish/FDD/*.zip
            publish/SCD/*.zip
```

**发布产物**:
- Framework-dependent 便携版 (`.zip`)
- Self-contained 独立版 (`.zip`)
- 自动生成 Release Notes

---

## 新增文件

| 文件 | 说明 |
|------|------|
| `README.md` | 项目介绍、功能说明、快速开始 |
| `LICENSE` | MIT 许可证 |
| `Directory.Build.props` | 共享 MSBuild 属性 |

---

## 效果

| 指标 | 优化前 | 优化后 |
|------|--------|--------|
| 目录层级 | 3 层嵌套 | 2 层标准结构 |
| CI 配置 | 复杂路径 | 根目录 slnx |
| 发布流程 | 手动 | 自动化 |
