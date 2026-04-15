# Workflow 深度标准化

**日期**: 2026-03-10
**版本**: 0.3.0
**类型**: Infrastructure

---

## 概述

全仓库 GitHub Actions 深度标准化：统一命名、权限、并发、路径过滤与缓存策略。

---

## 变更详情

### Workflow 重命名

| 原名 | 新名 | 说明 |
|------|------|------|
| `docs.yml` | `pages.yml` | 更准确反映用途 |

### CI Workflow 标准化

```yaml
permissions:
  contents: read

concurrency:
  group: ci-${{ github.workflow }}-${{ github.ref }}
  cancel-in-progress: true
```

### Pages Workflow 增强

```yaml
# 新增 configure-pages 步骤
- name: Setup Pages
  uses: actions/configure-pages@v5

# 新增 paths 触发过滤
on:
  push:
    branches: [master, main]
    paths:
      - 'docs/**'
      - 'package.json'
      - 'package-lock.json'
```

---

## 标准化清单

| 配置项 | CI | Pages | Release |
|--------|:--:|:-----:|:-------:|
| permissions | ✅ | ✅ | ✅ |
| concurrency | ✅ | ✅ | ✅ |
| paths 过滤 | - | ✅ | - |
| 缓存策略 | ✅ | - | - |
| configure-pages | - | ✅ | - |

---

## 效果

- 避免并发构建冲突
- 减少无效构建次数
- 统一权限最小化原则
