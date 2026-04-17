# Project Philosophy: Spec-Driven Development (SDD)

本项目严格遵循**规范驱动开发（Spec-Driven Development）**范式。所有的代码实现必须以 `/specs` 目录下的规范文档为唯一事实来源（Single Source of Truth）。

## Directory Context (目录说明)

### 规范文档 (`/specs/`)
- `/specs/product/` - 产品功能定义与验收标准 (PRD)
- `/specs/rfc/` - 技术设计与架构方案 (RFCs)
- `/specs/api/` - API 接口定义（如 OpenAPI.yaml）
- `/specs/db/` - 数据库/数据模型定义
- `/specs/testing/` - BDD 测试用例规范

### 用户文档 (`/docs/`)
- `/docs/setup/` - 环境搭建指南
- `/docs/tutorials/` - 用户使用教程
- `/docs/architecture/` - 高层面的架构说明（链接到 specs/rfc）
- `/docs/assets/` - 图片、UML图等静态资源

### 其他关键目录
- `/src/` - 源代码
- `/tests/` - 测试代码
- `/changelog/` - 变更日志条目
- `/scripts/` - 构建和自动化脚本

## AI Agent Workflow Instructions (AI 工作流指令)

当你（AI）被要求开发一个新功能、修改现有功能或修复 Bug 时，**必须严格按照以下工作流执行，不可跳过任何步骤**：

### Step 1: 审查与分析 (Review Specs)
- 在编写任何代码之前，首先阅读 `/specs` 目录下相关的产品文档、RFC 和 API 定义
- 检查以下目录：
  - `/specs/product/` - 了解功能需求和验收标准
  - `/specs/rfc/` - 了解技术设计决策
  - `/specs/api/` - 了解接口定义
  - `/specs/db/` - 了解数据模型
- **如果用户指令与现有 Spec 冲突**：立即停止编码，并指出冲突点，询问用户是否需要先更新 Spec
- **如果不存在相关 Spec**：提议创建相应的 Spec 文档，不要直接开始编码

### Step 2: 规范优先 (Spec-First Update)
- 如果这是一个新功能，或者需要改变现有的接口/数据库结构，**必须首先提议修改或创建相应的 Spec 文档**：
  - 新产品功能 → 创建 `/specs/product/<feature-name>.md`
  - 新 API 端点 → 更新 `/specs/api/openapi.yaml` 或创建新的 API spec
  - 新数据模型 → 更新 `/specs/db/` 规范
  - 架构变更 → 创建 `/specs/rfc/<NNNN-description>.md`
- **等待用户确认 Spec 的修改后**，才能进入代码编写阶段
- **维护 Spec 版本**，跟踪变更

### Step 3: 代码实现 (Implementation)
- 编写代码时，**必须 100% 遵守 Spec 中的定义**，包括：
  - 变量命名约定
  - API 路径和 HTTP 方法
  - 数据类型和验证规则
  - HTTP 状态码
  - 错误响应格式
- **No Gold-Plating**：不要在代码中擅自添加 Spec 中未定义的功能
- **遵循 RFC 决策**：参考 `/specs/rfc/` 文档中的技术决策
- **保持代码质量**：遵循项目约定（C# 命名、MVVM 模式等）

### Step 4: 测试验证 (Test against Spec)
- 根据 `/specs` 中的**验收标准（Acceptance Criteria）**编写单元测试和集成测试
- 确保测试用例**覆盖 Spec 中描述的所有边界情况**
- 对于 BDD 风格的特性，参考 `/specs/testing/` 规范
- 运行 `dotnet test` 验证所有测试通过
- 如果需求变更，同步更新测试规范

## Code Generation Rules

1. **API 变更**：任何对外部暴露的 API 变更，必须同步修改 `/specs/api/` 文档
2. **数据模型变更**：任何数据结构变更，必须更新 `/specs/db/` 规范
3. **架构决策**：记录重大技术决策到 `/specs/rfc/`
4. **禁止违反规范**：绝不编写与现有规范相矛盾的代码
5. **参考规范**：遇到不确定的技术细节，查阅 `/specs/rfc/` 下的架构约定，不要自行捏造设计模式
6. **测试覆盖**：确保测试验证 Spec 验收标准

## 项目约定

### C# 命名约定
| 类型 | 约定 | 示例 |
|------|------|------|
| 类 | PascalCase | `DiagnosticService` |
| 方法 | PascalCase | `GetHardwareInfo()` |
| 属性 | PascalCase | `ComputerName` |
| 字段 (private) | _camelCase | `_logService` |
| 参数 | camelCase | `cancellationToken` |
| 接口 | IPascalCase | `IHardwareService` |

### 提交信息约定 (Conventional Commits)
```
<type>(<scope>): <description>

[optional body]
```
类型：`feat`, `fix`, `docs`, `refactor`, `test`, `chore`

### 构建与测试命令
```powershell
# 构建
dotnet build

# 运行所有测试
dotnet test

# 运行并收集覆盖率
dotnet test --collect:"XPlat Code Coverage"

# 运行特定测试
dotnet test --filter "FullyQualifiedName~ReportServiceTests"
```

## 为什么要这样声明？

- **防范 AI 幻觉**：AI 很容易在没有上下文的情况下"自由发挥"。强制它第一步读取 /specs 可以锚定其思考范围
- **约束修改路径**：声明了"修改代码前先改 Spec"，保证了文档与代码永远同步（Document-Code Synchronization）
- **提高 PR 质量**：当 AI 帮你生成 Pull Request 时，它的实现会与业务逻辑高度一致，因为它是根据你在 Spec 中定义的验收标准来进行开发的
