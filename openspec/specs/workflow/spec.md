# Workflow Specification

> **Domain**: workflow
> **Version**: 1.0.0
> **Status**: accepted
> **Last Updated**: 2026-04-23

## Overview

本文档定义 DigYourWindows 的开发工作流规范，包括分支策略、OpenSpec 驱动开发流程、AI 工具协作模式及发布流程。

---

## Branch Strategy

### 规则

| 规则 | 细节 |
|------|------|
| 主分支 | `master` — 唯一长期分支，永远保持可发布状态 |
| Feature 分支 | 短生命周期（< 1 天）；完成即合并，禁止积累 |
| 禁止 | 长期未合并的分支；本地与远端大量分叉 |
| 保护 | 所有合并前必须 CI 通过（`ci.yml` build + test）|

### 日常流程

```bash
# 直接修复（小改动）
git add -p && git commit -m "fix(core): handle null CPU sensor gracefully"
git push origin master

# Feature 分支（较大改动）
git checkout -b fix/smart-null-handling
# ... 修改 ...
git commit -m "fix(core): return Unknown health status when SMART unavailable"
git push origin fix/smart-null-handling
# merge immediately after CI passes
git checkout master && git merge fix/smart-null-handling && git push
git branch -d fix/smart-null-handling
```

---

## OpenSpec-Driven Development Flow

### 完整工作流

```
┌─────────────────────────────────────────────────────────┐
│  1. CHECK SPEC                                          │
│     cat openspec/specs/<domain>/spec.md                 │
│     → 确认需求已覆盖 or 需要变更                          │
├─────────────────────────────────────────────────────────┤
│  2. PROPOSE (新功能/重大变更)                            │
│     mkdir openspec/changes/<name>/                      │
│     Create proposal.md → design.md → tasks.md           │
│     (Bug 修复可跳过此步，直接进入 IMPLEMENT)              │
├─────────────────────────────────────────────────────────┤
│  3. IMPLEMENT                                           │
│     按 tasks.md 逐条执行                                  │
│     每条 task = 一次 atomic commit                       │
├─────────────────────────────────────────────────────────┤
│  4. REVIEW                                              │
│     /review (GitHub Copilot) 或 Claude Code review      │
│     → 修复所有 High 级别问题                             │
├─────────────────────────────────────────────────────────┤
│  5. VERIFY                                              │
│     dotnet build DigYourWindows.slnx -c Release         │
│     dotnet test DigYourWindows.slnx -c Release          │
│     → 零警告，全测试通过                                  │
├─────────────────────────────────────────────────────────┤
│  6. COMMIT & PUSH                                       │
│     Conventional commit format                          │
│     Push → CI passes → done                             │
├─────────────────────────────────────────────────────────┤
│  7. ARCHIVE (完成 change 后)                            │
│     Move openspec/changes/<name>/ → openspec/archive/   │
│     Update spec status to "implemented"                 │
└─────────────────────────────────────────────────────────┘
```

### OpenSpec Change 目录结构

```
openspec/changes/<change-name>/
├── proposal.md       # 变更描述、动机、影响评估
├── design.md         # 实现方案、架构决策
├── tasks.md          # 可执行任务清单（atomic items）
└── specs/            # Delta specs（相对当前 spec 的变更）
    └── <domain>/
        └── spec.md   # 使用 ADDED/MODIFIED/REMOVED 格式
```

### Delta Spec 格式

```markdown
## ADDED Requirements

### Requirement: New Feature
系统 SHALL [描述新行为]

## MODIFIED Requirements

### Requirement: Existing Feature
~~Old: 旧行为描述~~
New: 新行为描述

## REMOVED Requirements

### Requirement: Deprecated Feature
**原因**: [移除理由]
```

---

## AI Tool Collaboration

### 工具分工

| 工具 | 最佳场景 |
|------|---------|
| **GitHub Copilot CLI** (`gh copilot`) | 项目规划、架构审查、spec 设计、`/review` 质量审查 |
| **Claude Code** (claude CLI) | 复杂多文件重构、测试生成、文档写作、OpenSpec 内容设计 |
| **GLM / Qwen** | 执行 `tasks.md` 中的具体开发任务、常规 bug 修复 |

### 推荐模式

```bash
# 规划阶段（Copilot CLI）
gh copilot suggest "check openspec features spec, propose finalization tasks"

# 复杂实现（Claude Code）
claude "implement PropertyTests for PerformanceService per openspec/specs/testing/spec.md"

# 常规任务（GLM/Qwen）
# 给模型传入 tasks.md 中的单个任务，执行后 commit
```

### /review 使用规范

在以下节点强制执行 `/review`：

1. **每个 Phase 完成后**（如 Phase 1 Critical Fixes 全部提交后）
2. **复杂 bug 修复后**（涉及并发、资源管理、异常处理的修改）
3. **测试代码完善后**（确认测试是否真正覆盖 spec 场景）

```bash
# 在 Claude Code 或 Copilot CLI 中
/review  # 审查当前 staged/unstaged 变更
```

---

## Commit Convention

```
<type>(<scope>): <description>

[optional body — explain WHY, not WHAT]

Co-authored-by: Copilot <223556219+Copilot@users.noreply.github.com>
```

| Type | 使用场景 |
|------|---------|
| `feat` | 新功能（spec 中已定义的） |
| `fix` | Bug 修复 |
| `docs` | 文档变更（含 openspec） |
| `refactor` | 重构（不改变行为） |
| `test` | 测试添加或修改 |
| `chore` | 构建、工具链变更 |
| `specs` | OpenSpec 规范变更 |

Scopes: `core` `ui` `tests` `docs` `openspec` `build`

---

## Release Process

```bash
# 1. 确认所有测试通过
dotnet test DigYourWindows.slnx -c Release

# 2. 更新 CHANGELOG.md（在 [Unreleased] 下补充条目）

# 3. 确认 Directory.Build.props 版本号正确

# 4. 创建并推送版本标签
git tag v1.2.0 -m "DigYourWindows v1.2.0"
git push origin v1.2.0
# → 触发 .github/workflows/release.yml
# → 自动构建 FDD + SCD artifacts，发布到 GitHub Releases
```

### Release 命名规范

| 制品 | 说明 |
|------|------|
| `DigYourWindows_1.2.0_win-x64_portable.zip` | Framework-Dependent (需要 .NET 10 Runtime) |
| `DigYourWindows_1.2.0_win-x64_standalone.zip` | Self-Contained (无需安装 .NET) |

---

## Quality Gates

所有 push 到 master 的代码必须满足：

| 检查项 | 工具 | 要求 |
|--------|------|------|
| Build | `dotnet build` | 零错误、零警告 |
| Unit Tests | `dotnet test --filter Category=Unit` | 100% 通过 |
| Property Tests | `dotnet test --filter Category=Property` | 100% 通过 |
| Integration Tests | CI on Windows | 跳过或通过 |
| Nullable | 编译器检查 | 无 nullable 警告 |

## References

- [Architecture Specification](../architecture/spec.md)
- [Testing Specification](../testing/spec.md)
