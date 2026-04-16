<template>
  <div class="home-features">
    <div class="features-grid">
      <div class="feature-item" v-for="feature in features" :key="feature.title">
        <div class="feature-icon">{{ feature.icon }}</div>
        <h3 class="feature-title">{{ feature.title }}</h3>
        <p class="feature-desc">{{ feature.description }}</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useData } from 'vitepress'

const { lang } = useData()

const featuresZh = [
  { icon: '🖥️', title: '硬件信息采集', description: '全面采集 CPU、GPU、内存、磁盘（含 SMART）等硬件信息' },
  { icon: '📊', title: '实时监控', description: 'CPU/ GPU 温度、负载、频率，网络流量实时追踪' },
  { icon: '📋', title: '事件日志分析', description: '自动提取 System/Application 错误和警告事件' },
  { icon: '📈', title: '可靠性记录', description: 'Windows 可靠性监视器数据可视化趋势' },
  { icon: '✅', title: '系统健康评分', description: '综合评估生成 0-100 分的系统健康评分' },
  { icon: '💡', title: '智能优化建议', description: '根据分析结果自动生成针对性优化建议' },
]

const featuresEn = [
  { icon: '🖥️', title: 'Hardware Detection', description: 'Collect CPU, GPU, RAM, Disk (with SMART) information' },
  { icon: '📊', title: 'Real-time Monitoring', description: 'CPU/GPU temperature, load, frequency, network traffic' },
  { icon: '📋', title: 'Event Log Analysis', description: 'Auto-extract System/Application errors and warnings' },
  { icon: '📈', title: 'Reliability Records', description: 'Windows Reliability Monitor data visualization' },
  { icon: '✅', title: 'Health Scoring', description: 'Generate 0-100 system health score' },
  { icon: '💡', title: 'Smart Recommendations', description: 'AI-generated optimization suggestions' },
]

const features = computed(() => lang.value === 'zh-CN' ? featuresZh : featuresEn)
</script>

<style scoped>
.home-features {
  padding: 2rem 0;
  margin: 2rem 0;
}

.features-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 1.5rem;
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1.5rem;
}

.feature-item {
  background: var(--vp-c-bg-soft);
  border: 1px solid var(--vp-c-divider);
  border-radius: 12px;
  padding: 1.5rem;
  text-align: center;
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
}

.feature-item:hover {
  transform: translateY(-4px);
  box-shadow: 0 12px 40px -12px rgba(0, 0, 0, 0.15);
  border-color: var(--vp-c-brand-1);
}

.feature-icon {
  font-size: 2.5rem;
  margin-bottom: 1rem;
}

.feature-title {
  font-size: 1.125rem;
  font-weight: 600;
  margin-bottom: 0.5rem;
  color: var(--vp-c-text-1);
}

.feature-desc {
  font-size: 0.875rem;
  color: var(--vp-c-text-2);
  line-height: 1.6;
  margin: 0;
}

@media (max-width: 768px) {
  .features-grid {
    grid-template-columns: 1fr;
    padding: 0 1rem;
  }
}
</style>
