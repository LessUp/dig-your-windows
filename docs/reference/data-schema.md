# 数据 Schema

DigYourWindows 的诊断数据输出遵循 JSON Schema 规范，完整定义见 [`diagnostic-data-schema.json`](https://github.com/LessUp/dig-your-windows/blob/main/docs/diagnostic-data-schema.json)。

## 顶层结构

诊断报告的 JSON 输出包含以下顶层字段：

| 字段 | 类型 | 说明 |
|------|------|------|
| `SystemInfo` | object | 系统基本信息 |
| `Hardware` | object | 硬件详情（CPU/GPU/内存/磁盘/网络/USB） |
| `EventLogs` | array | 事件日志条目 |
| `ReliabilityRecords` | array | 可靠性监视器记录 |
| `HealthScore` | object | 系统健康评分 |
| `Recommendations` | array | 优化建议 |

## 健康评分

健康评分综合评估以下维度：

- **稳定性** — 基于事件日志错误/警告频率
- **性能** — CPU/GPU 负载与温度
- **内存** — 可用内存比例
- **磁盘** — SMART 指标与剩余寿命

评分范围 0–100，等级划分：

| 等级 | 分数范围 | 颜色 |
|------|----------|------|
| 优秀 | 90–100 | 绿色 |
| 良好 | 70–89 | 蓝色 |
| 一般 | 50–69 | 黄色 |
| 较差 | < 50 | 红色 |
