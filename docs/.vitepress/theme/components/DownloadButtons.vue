<template>
  <div class="download-section">
    <div v-if="loading" class="loading-state">
      <span class="loading-spinner"></span>
      <span>{{ isZh ? '加载中...' : 'Loading...' }}</span>
    </div>
    
    <div v-else-if="error" class="error-state">
      <a :href="releasesUrl" target="_blank" rel="noopener" class="download-btn primary">
        <span class="icon">📥</span>
        <span>{{ isZh ? '前往下载' : 'Go to Downloads' }}</span>
      </a>
    </div>
    
    <div v-else class="download-options">
      <div class="primary-download">
        <a :href="primaryAsset?.browser_download_url || releasesUrl" 
           target="_blank" 
           rel="noopener" 
           class="download-btn primary">
          <span class="icon">💻</span>
          <div class="btn-content">
            <span class="btn-title">{{ isZh ? '下载安装程序' : 'Download Installer' }}</span>
            <span class="btn-meta">{{ primaryAsset?.name || 'DigYourWindows_Setup.exe' }} · {{ formatSize(primaryAsset?.size) }}</span>
          </div>
        </a>
        <span class="version-tag">{{ version }}</span>
      </div>
      
      <div class="secondary-downloads">
        <a :href="portableUrl" 
           target="_blank" 
           rel="noopener" 
           class="download-btn secondary">
          <span class="icon">📦</span>
          <span>{{ isZh ? '便携版' : 'Portable' }}</span>
        </a>
        <a :href="releasesUrl" 
           target="_blank" 
           rel="noopener" 
           class="download-btn secondary">
          <span class="icon">📋</span>
          <span>{{ isZh ? '所有版本' : 'All Releases' }}</span>
        </a>
      </div>
    </div>
    
    <p class="download-note">
      {{ isZh ? '适用于 Windows 10/11 · 开源免费' : 'For Windows 10/11 · Open Source & Free' }}
    </p>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useData } from 'vitepress'

const { lang } = useData()

const isZh = computed(() => lang.value === 'zh-CN')
const repoUrl = 'https://github.com/LessUp/dig-your-windows'
const releasesUrl = `${repoUrl}/releases`

const loading = ref(true)
const error = ref(false)
const version = ref('')
const primaryAsset = ref<any>(null)
const portableUrl = ref('')

async function fetchLatestRelease() {
  try {
    const response = await fetch('https://api.github.com/repos/LessUp/dig-your-windows/releases/latest')
    if (!response.ok) throw new Error('Failed to fetch')
    
    const data = await response.json()
    version.value = data.tag_name
    
    // Find primary installer
    const setupAsset = data.assets.find((a: any) => 
      a.name.includes('Setup') && a.name.endsWith('.exe')
    )
    const scdAsset = data.assets.find((a: any) => 
      a.name.includes('SCD') && a.name.endsWith('.zip')
    )
    const fddAsset = data.assets.find((a: any) => 
      a.name.includes('FDD') && a.name.endsWith('.zip')
    )
    
    primaryAsset.value = setupAsset || scdAsset || fddAsset || data.assets[0]
    portableUrl.value = scdAsset?.browser_download_url || releasesUrl
    
    loading.value = false
  } catch (err) {
    console.error('Failed to fetch release:', err)
    error.value = true
    loading.value = false
  }
}

function formatSize(bytes: number | undefined): string {
  if (!bytes) return ''
  const mb = bytes / (1024 * 1024)
  if (mb >= 100) {
    return (mb / 1024).toFixed(1) + ' GB'
  }
  return mb.toFixed(0) + ' MB'
}

onMounted(() => {
  fetchLatestRelease()
})
</script>

<style scoped>
.download-section {
  margin: 1.5rem 0;
}

.loading-state {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  padding: 1rem;
  color: var(--vp-c-text-2);
}

.loading-spinner {
  display: inline-block;
  width: 1rem;
  height: 1rem;
  border: 2px solid var(--vp-c-divider);
  border-top-color: var(--vp-c-brand-1);
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.download-options {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.primary-download {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.75rem;
  flex-wrap: wrap;
}

.download-btn {
  display: inline-flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.875rem 1.5rem;
  border-radius: 10px;
  text-decoration: none;
  font-weight: 600;
  transition: all 0.2s ease;
}

.download-btn.primary {
  background: linear-gradient(135deg, var(--vp-c-brand-1) 0%, var(--vp-c-brand-2) 100%);
  color: white;
  box-shadow: 0 4px 15px rgba(0, 120, 212, 0.3);
}

.download-btn.primary:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(0, 120, 212, 0.4);
}

.download-btn .icon {
  font-size: 1.25rem;
}

.btn-content {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  text-align: left;
}

.btn-title {
  font-size: 1rem;
  font-weight: 600;
}

.btn-meta {
  font-size: 0.75rem;
  opacity: 0.9;
  font-weight: 400;
}

.version-tag {
  padding: 0.35rem 0.75rem;
  background: var(--vp-c-bg-soft);
  border: 1px solid var(--vp-c-divider);
  border-radius: 9999px;
  font-size: 0.8rem;
  font-weight: 600;
  color: var(--vp-c-brand-1);
}

.secondary-downloads {
  display: flex;
  gap: 0.5rem;
  justify-content: center;
  flex-wrap: wrap;
}

.download-btn.secondary {
  padding: 0.6rem 1rem;
  background: var(--vp-c-bg-soft);
  border: 1px solid var(--vp-c-divider);
  color: var(--vp-c-text-1);
  font-size: 0.875rem;
}

.download-btn.secondary:hover {
  background: var(--vp-c-bg);
  border-color: var(--vp-c-brand-1);
  color: var(--vp-c-brand-1);
}

.download-note {
  margin: 0.75rem 0 0;
  font-size: 0.8rem;
  color: var(--vp-c-text-2);
}

@media (max-width: 480px) {
  .primary-download {
    flex-direction: column;
  }
  
  .download-btn.primary {
    width: 100%;
    justify-content: center;
  }
  
  .secondary-downloads {
    width: 100%;
  }
  
  .download-btn.secondary {
    flex: 1;
    justify-content: center;
  }
}
</style>
