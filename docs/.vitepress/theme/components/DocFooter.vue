<template>
  <div class="doc-footer">
    <div class="feedback-section">
      <div class="feedback-content">
        <h4>{{ isZh ? '对本页内容有疑问？' : 'Have questions about this page?' }}</h4>
        <p>{{ isZh ? '如果发现错误或需要改进，欢迎提交反馈。' : 'If you find errors or need improvements, welcome to submit feedback.' }}</p>
        <div class="feedback-actions">
          <a :href="editLink" target="_blank" rel="noopener" class="action-button">
            <span class="icon">✏️</span>
            {{ isZh ? '编辑此页' : 'Edit this page' }}
          </a>
          <a :href="issueLink + '&labels=documentation'" target="_blank" rel="noopener" class="action-button secondary">
            <span class="icon">🐛</span>
            {{ isZh ? '报告问题' : 'Report issue' }}
          </a>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useData } from 'vitepress'

const { page, lang } = useData()

const isZh = computed(() => lang.value === 'zh-CN')

const repoUrl = 'https://github.com/LessUp/dig-your-windows'

const editLink = computed(() => {
  return `${repoUrl}/edit/master/docs/${page.value.filePath}`
})

const issueLink = computed(() => {
  const title = encodeURIComponent(`[Docs] ${page.value.title}`)
  return `${repoUrl}/issues/new?title=${title}`
})
</script>

<style scoped>
.doc-footer {
  margin-top: 3rem;
  padding-top: 2rem;
  border-top: 1px solid var(--vp-c-divider);
}

.feedback-section {
  background: var(--vp-c-bg-soft);
  border-radius: 12px;
  padding: 1.5rem;
}

.feedback-content h4 {
  font-size: 1rem;
  font-weight: 600;
  margin: 0 0 0.5rem;
  color: var(--vp-c-text-1);
}

.feedback-content p {
  font-size: 0.875rem;
  color: var(--vp-c-text-2);
  margin: 0 0 1rem;
}

.feedback-actions {
  display: flex;
  gap: 0.75rem;
  flex-wrap: wrap;
}

.action-button {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.5rem 1rem;
  background: var(--vp-c-brand-1);
  color: white;
  border-radius: 6px;
  text-decoration: none;
  font-size: 0.875rem;
  font-weight: 500;
  transition: all 0.2s ease;
}

.action-button:hover {
  background: var(--vp-c-brand-2);
  transform: translateY(-1px);
}

.action-button.secondary {
  background: var(--vp-c-bg);
  color: var(--vp-c-text-1);
  border: 1px solid var(--vp-c-divider);
}

.action-button.secondary:hover {
  background: var(--vp-c-bg-soft);
  border-color: var(--vp-c-brand-1);
  color: var(--vp-c-brand-1);
}

.icon {
  font-size: 1rem;
}

@media (max-width: 480px) {
  .feedback-actions {
    flex-direction: column;
  }
  
  .action-button {
    justify-content: center;
  }
}
</style>
