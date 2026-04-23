# Change Proposal: v1.2.0 Finalization

> **Change**: v1.2.0-finalization
> **Status**: approved
> **Created**: 2026-04-23
> **Target Version**: 1.2.0
> **Domains Affected**: features, testing, export, hardware

## Summary

项目收尾阶段。目标是将 DigYourWindows 推进到 v1.2.0 稳定发布状态，修复所有已知 bug，补全测试覆盖，最终打标签发布。**不添加新功能。**

## Motivation

- 当前版本 1.1.0 存在若干已知问题（属性测试缺失、边界情况处理不完善）
- 测试覆盖率需要提升（PropertyTests 目录几乎为空）
- 项目进入低维护模式前，需要一次质量收口

## Scope

### In Scope ✅

1. **测试覆盖**: 补充 FsCheck 属性测试
2. **Bug 修复**: 修复代码审计发现的所有问题
3. **文档同步**: 确保所有 spec 与实现一致
4. **版本收口**: 准备 v1.2.0 发布材料（CHANGELOG、tag）

### Out of Scope ❌

- CLI mode
- 多语言报告导出
- 性能基准比较
- 任何新的 UI 功能

## Impact Assessment

| 组件 | 变更幅度 | 风险 |
|------|---------|------|
| `DigYourWindows.Tests` | Medium（新增测试） | Low |
| `DigYourWindows.Core` | Low（仅 bug 修复） | Low |
| `DigYourWindows.UI` | Minimal | Very Low |
| OpenSpec docs | Low（状态更新） | None |

## Success Criteria

- [ ] `dotnet test` 100% 通过
- [ ] PropertyTests/ 至少有 5 个属性测试覆盖核心计算逻辑
- [ ] `dotnet build -c Release` 零警告
- [ ] CHANGELOG.md 含完整的 v1.2.0 条目
- [ ] v1.2.0 tag 推送后 GitHub Actions release workflow 成功
