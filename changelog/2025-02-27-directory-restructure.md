# 目录结构重组 & GitHub Release 配置 (2025-02-27)

## 目录重组

消除两层无意义嵌套 (`DigYourWindows/DigYourWindows_WPF/`)，采用标准开源项目结构：

- **`src/`** — 源码项目 (`DigYourWindows.Core`, `DigYourWindows.UI`)
- **`tests/`** — 测试项目 (`DigYourWindows.Tests`)
- **`docs/`** — 文档和数据 Schema
- **`installer/`** — Inno Setup 安装包脚本（提升到根目录）
- **`scripts/`** — 构建和发布脚本（提升到根目录）
- **`DigYourWindows.slnx`** — Solution 文件提升到仓库根
- **`NuGet.Config`** — 提升到仓库根
- **`.gitignore`** — 提升到仓库根

## 路径引用更新

- `DigYourWindows.slnx` 项目路径更新为 `src/` 和 `tests/` 前缀
- `DigYourWindows.Tests.csproj` ProjectReference 更新为 `../../src/...`
- `scripts/publish.ps1` 和 `scripts/build-installer.ps1` 项目路径更新
- `.github/workflows/ci.yml` 移除 `working-directory`，改用 slnx 全量构建

## GitHub Actions

- **ci.yml** — 更新为使用仓库根的 slnx 文件
- **release.yml** — 新增 Release 发布 Action，推送 `v*` tag 自动构建并发布：
  - Framework-dependent 便携版 (`.zip`)
  - Self-contained 独立版 (`.zip`)
  - 自动生成 Release Notes

## 新增文件

- **README.md** — 项目介绍、功能说明、快速开始指南
- **LICENSE** — MIT 许可证
