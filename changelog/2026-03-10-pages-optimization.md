# GitHub Pages 优化

**日期**: 2026-03-10
**版本**: 0.3.0
**类型**: Documentation + Optimization

---

## 概述

优化 VitePress 文档站，增强 SEO、丰富首页内容、优化构建流程。

---

## 变更详情

### Bug 修复

| 文件 | 问题 | 修复 |
|------|------|------|
| `README.md` | Docs badge 指向错误的 workflow | `docs.yml` → `pages.yml` |
| `README.zh-CN.md` | 同上 | 同上 |

### 文档内容

**docs/index.md**:
- 首页增加"项目架构"快捷入口
- 扩充 feature 描述细节
- 新增技术栈表格
- 新增架构亮点板块

**docs/changelog.md**:
- 新增变更日志汇总页
- 整合 5 次重要变更记录

### VitePress 配置

**SEO 优化**:
```typescript
head: [
  ['meta', { property: 'og:title', content: 'DigYourWindows' }],
  ['meta', { property: 'og:description', content: '...' }],
  ['meta', { property: 'og:url', content: '...' }],
  ['meta', { name: 'theme-color', content: '#0078d4' }],
  ['meta', { name: 'keywords', content: 'Windows,诊断,硬件信息,...' }],
]
```

**导航优化**:
- 启用 `cleanUrls`
- 顶栏和侧栏新增"变更日志"导航

### 构建流程

**pages.yml**:
```yaml
# 优化：使用 sparse-checkout 替代全量 git 历史
- name: Checkout
  uses: actions/checkout@v4
  with:
    sparse-checkout: |
      docs
      package.json
      package-lock.json

# 新增触发器
on:
  push:
    paths:
      - 'docs/**'
      - 'package.json'
      - 'package-lock.json'
```

---

## 效果

| 指标 | 优化前 | 优化后 |
|------|--------|--------|
| SEO | 无 meta 标签 | 完整 og:* 标签 |
| 首页内容 | 基础 | 丰富 |
| 构建触发 | 任意 push | 仅 docs 变更 |
| Checkout 时间 | 全量历史 | 仅必要文件 |
