# Specs Index

This directory contains all specification documents for DigYourWindows. The project follows **Spec-Driven Development (SDD)**, meaning specs are the single source of truth for all implementation decisions.

## Directory Structure

```
specs/
├── product/          # 产品功能定义与验收标准 (PRD)
├── rfc/              # 技术设计与架构方案 (RFCs)
├── api/              # API 接口定义
├── db/               # 数据模型规范
└── testing/          # BDD 测试用例规范
```

## Specification Types

### `/specs/product/` - 产品规范

功能定义，包含用户故事和验收标准。

**示例**：
- `hardware-detection.md` - 硬件检测功能规范
- `event-log-analysis.md` - 事件日志分析功能规范
- `health-scoring.md` - 健康评分功能规范

**模板**：
```markdown
# Product Spec: Feature Name

**Status**: Draft | Accepted | Implemented
**Date**: YYYY-MM-DD
**Version**: X.Y.Z

## User Stories
### US-1: Story Title
**As a** user  
**I want to** action  
**So that** benefit

**Acceptance Criteria**:
- [ ] Criterion 1
- [ ] Criterion 2
```

### `/specs/rfc/` - 技术设计文档

架构决策和技术设计文档。

**命名**：`NNNN-description.md`（例如：`0001-core-architecture.md`）

**模板**：
```markdown
# RFC-NNNN: Title

**Status**: Draft | Accepted | Implemented
**Date**: YYYY-MM-DD
**Author**: Author
**Category**: Architecture | Performance | Security | etc.

## Summary
Brief description of the decision.

## Decision
Detailed technical decision with code examples.

## Rationale
Why this decision was made.

## Related Documents
- Links to related specs
```

### `/specs/api/` - API 规范

API 接口定义和契约。

**示例**：
- `openapi.yaml` - REST API 定义
- `report-export.md` - 报告导出 API

### `/specs/db/` - 数据模型规范

数据结构和模型定义。

**示例**：
- `schema.dbml` - 数据库 schema
- `data-models.md` - 数据模型规范

### `/specs/testing/` - 测试规范

测试策略和 BDD 测试规范。

**示例**：
- `test-strategy.md` - 整体测试方法
- `*.feature` - BDD 测试用例

## Status Flow

规范文档遵循以下状态流：

1. **Draft** - 讨论中
2. **Accepted** - 已批准，可以实施
3. **Implemented** - 已实施，测试通过

## 如何使用 Spec

### 贡献者

1. **实施前**：阅读相关规范
2. **如果没有规范**：在编码前创建一个
3. **如果规范需要变更**：先更新规范，获得审查后再编码
4. **实施期间**：100% 遵守规范
5. **实施后**：验证所有验收标准已满足

### AI 代理

参见 [AGENTS.md](../AGENTS.md) 了解详细的 AI 工作流指令。

## 相关文档

- [CONTRIBUTING.md](../CONTRIBUTING.md) - 贡献指南
- [AGENTS.md](../AGENTS.md) - AI 代理工作流
- [RFC-0001](./rfc/0001-core-architecture.md) - 核心架构
