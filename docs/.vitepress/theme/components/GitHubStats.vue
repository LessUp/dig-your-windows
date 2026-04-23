<template>
  <div class="github-stats">
    <div class="stats-grid">
      <a :href="repoUrl" target="_blank" rel="noopener" class="stat-item">
        <span class="stat-icon">⭐</span>
        <span class="stat-value">{{ stars || '...' }}</span>
        <span class="stat-label">{{ isZh ? 'Stars' : 'Stars' }}</span>
      </a>
      <a :href="`${repoUrl}/forks`" target="_blank" rel="noopener" class="stat-item">
        <span class="stat-icon">🍴</span>
        <span class="stat-value">{{ forks || '...' }}</span>
        <span class="stat-label">{{ isZh ? 'Forks' : 'Forks' }}</span>
      </a>
      <a :href="`${repoUrl}/releases`" target="_blank" rel="noopener" class="stat-item">
        <span class="stat-icon">📦</span>
        <span class="stat-value">{{ downloads || '...' }}</span>
        <span class="stat-label">{{ isZh ? 'Downloads' : 'Downloads' }}</span>
      </a>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useData } from 'vitepress'

const { lang } = useData()

const isZh = computed(() => lang.value === 'zh-CN')
const repoUrl = 'https://github.com/LessUp/dig-your-windows'

const stars = ref('')
const forks = ref('')
const downloads = ref('')

async function fetchStats() {
  try {
    // Fetch repo stats
    const repoResponse = await fetch('https://api.github.com/repos/LessUp/dig-your-windows')
    if (repoResponse.ok) {
      const repoData = await repoResponse.json()
      stars.value = formatNumber(repoData.stargazers_count)
      forks.value = formatNumber(repoData.forks_count)
    }

    // Fetch releases for download count
    const releasesResponse = await fetch('https://api.github.com/repos/LessUp/dig-your-windows/releases')
    if (releasesResponse.ok) {
      const releases = await releasesResponse.json()
      let totalDownloads = 0
      releases.forEach((release: any) => {
        release.assets.forEach((asset: any) => {
          totalDownloads += asset.download_count
        })
      })
      downloads.value = formatNumber(totalDownloads)
    }
  } catch (error) {
    console.error('Failed to fetch GitHub stats:', error)
  }
}

function formatNumber(num: number): string {
  if (num >= 1000000) {
    return (num / 1000000).toFixed(1) + 'M'
  }
  if (num >= 1000) {
    return (num / 1000).toFixed(1) + 'k'
  }
  return num.toString()
}

onMounted(() => {
  fetchStats()
})
</script>

<style scoped>
.github-stats {
  margin: 1.5rem 0;
}

.stats-grid {
  display: flex;
  gap: 1rem;
  justify-content: center;
  flex-wrap: wrap;
}

.stat-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.5rem 1rem;
  background: var(--vp-c-bg-soft);
  border: 1px solid var(--vp-c-divider);
  border-radius: 9999px;
  text-decoration: none;
  transition: all 0.2s ease;
}

.stat-item:hover {
  background: var(--vp-c-bg);
  border-color: var(--vp-c-brand-1);
  transform: translateY(-2px);
}

.stat-icon {
  font-size: 1rem;
}

.stat-value {
  font-weight: 700;
  color: var(--vp-c-text-1);
  font-size: 0.95rem;
}

.stat-label {
  font-size: 0.8rem;
  color: var(--vp-c-text-2);
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

@media (max-width: 480px) {
  .stats-grid {
    gap: 0.5rem;
  }
  
  .stat-item {
    padding: 0.4rem 0.8rem;
  }
  
  .stat-label {
    display: none;
  }
}
</style>
