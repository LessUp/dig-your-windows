<template>
  <div class="version-badge">
    <a :href="releaseUrl" target="_blank" rel="noopener" class="badge-link">
      <span class="badge-label">{{ isZh ? '最新版本' : 'Latest' }}</span>
      <span class="badge-version">{{ version || 'v1.1.0' }}</span>
    </a>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useData } from 'vitepress'

const { lang } = useData()

const isZh = computed(() => lang.value === 'zh-CN')
const releaseUrl = 'https://github.com/LessUp/dig-your-windows/releases/latest'
const version = ref('')

async function fetchVersion() {
  try {
    const response = await fetch('https://api.github.com/repos/LessUp/dig-your-windows/releases/latest')
    if (response.ok) {
      const data = await response.json()
      version.value = data.tag_name
    }
  } catch (error) {
    console.error('Failed to fetch version:', error)
  }
}

onMounted(() => {
  fetchVersion()
})
</script>

<style scoped>
.version-badge {
  display: inline-block;
}

.badge-link {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.35rem 0.875rem;
  background: var(--vp-c-bg-soft);
  border: 1px solid var(--vp-c-divider);
  border-radius: 9999px;
  text-decoration: none;
  transition: all 0.2s ease;
}

.badge-link:hover {
  background: var(--vp-c-bg);
  border-color: var(--vp-c-brand-1);
}

.badge-label {
  font-size: 0.75rem;
  color: var(--vp-c-text-2);
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.badge-version {
  font-size: 0.875rem;
  font-weight: 700;
  color: var(--vp-c-brand-1);
}
</style>
