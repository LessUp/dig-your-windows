# 贡献指南

感谢你对 DigYourWindows 项目的兴趣！本文档将帮助你参与项目开发。

## 行为准则

- 尊重所有贡献者
- 接受建设性批评
- 关注对社区最有利的事情

## 如何贡献

### 报告 Bug

1. 搜索 [Issues](https://github.com/LessUp/dig-your-windows/issues) 确认问题未被报告
2. 使用 Bug 报告模板创建新 Issue
3. 包含以下信息：
   - 操作系统版本
   - .NET 版本
   - 复现步骤
   - 预期行为与实际行为

### 提交功能请求

1. 创建新 Issue，使用功能请求模板
2. 清晰描述功能需求和用例
3. 说明该功能如何使项目受益

### 提交代码

#### 开发环境设置

```powershell
# 克隆仓库
git clone https://github.com/LessUp/dig-your-windows.git
cd dig-your-windows

# 还原依赖
dotnet restore DigYourWindows.slnx

# 验证构建
dotnet build DigYourWindows.slnx

# 运行测试
dotnet test DigYourWindows.slnx
```

#### 代码规范

- 遵循 [Microsoft C# 编码约定](https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- 使用 EditorConfig 配置（项目已包含 `.editorconfig`）
- 所有公共 API 必须有 XML 文档注释
- 单元测试使用 `[MethodName]_[Scenario]_[ExpectedResult]` 命名格式

#### 提交规范

使用 [Conventional Commits](https://www.conventionalcommits.org/) 格式：

```
<type>(<scope>): <description>

[optional body]

[optional footer]
```

**类型**:
- `feat`: 新功能
- `fix`: Bug 修复
- `docs`: 文档更新
- `style`: 代码格式（不影响功能）
- `refactor`: 重构
- `test`: 测试相关
- `chore`: 构建/工具相关

**示例**:
```
feat(monitor): add GPU memory usage monitoring

fix(reliability): handle empty records in timeline builder

docs(guide): add troubleshooting section
```

#### Pull Request 流程

1. Fork 仓库并创建功能分支
   ```powershell
   git checkout -b feat/my-feature
   ```

2. 进行修改并编写测试

3. 确保所有测试通过
   ```powershell
   dotnet test DigYourWindows.slnx
   ```

4. 提交更改
   ```powershell
   git commit -m "feat: add amazing feature"
   ```

5. 推送到 Fork 并创建 Pull Request

6. 等待 CI 通过和代码审查

### Pull Request 检查清单

- [ ] 代码遵循项目编码规范
- [ ] 已添加必要的测试
- [ ] 所有测试通过
- [ ] 文档已更新（如适用）
- [ ] 提交信息符合 Conventional Commits 规范

## 项目结构

```
dig-your-windows/
├── src/
│   ├── DigYourWindows.Core/   # 核心业务逻辑
│   └── DigYourWindows.UI/     # WPF 用户界面
├── tests/
│   └── DigYourWindows.Tests/  # 测试项目
├── docs/                      # VitePress 文档站
└── scripts/                   # 构建脚本
```

## 测试指南

详细的测试指南请参考 [Contributing Guide](https://github.com/LessUp/dig-your-windows/blob/main/CONTRIBUTING.md)。

### 运行测试

```powershell
# 运行所有测试
dotnet test

# 运行特定测试
dotnet test --filter "FullyQualifiedName~ReportServiceTests"

# 收集覆盖率
dotnet test --collect:"XPlat Code Coverage"
```

## 发布流程

1. 更新 `Directory.Build.props` 中的版本号
2. 更新 `CHANGELOG.md`
3. 创建并推送 Git tag
   ```powershell
   git tag v1.x.x
   git push origin v1.x.x
   ```
4. GitHub Actions 自动构建并发布

## 获取帮助

- 查看 [文档](https://lessup.github.io/dig-your-windows/)
- 在 [Discussions](https://github.com/LessUp/dig-your-windows/discussions) 提问
- 创建 [Issue](https://github.com/LessUp/dig-your-windows/issues)

## 许可证

本项目采用 MIT 许可证。提交代码即表示你同意你的贡献将按照相同许可发布。
