---
layout: home

hero:
  name: DigYourWindows
  text: Windows Deep Diagnostics Tool
  tagline: One-click hardware information collection, event log analysis, and system health scoring
  actions:
    - theme: brand
      text: ⬇️ Download
      link: https://github.com/LessUp/dig-your-windows/releases/latest
    - theme: alt
      text: 🇨🇳 中文
      link: /zh-CN/
    - theme: alt
      text: 🇺🇸 English
      link: /en-US/

features:
  - icon: 🖥️
    title: Hardware Detection
    details: CPU, GPU, RAM, Disk (SMART), Network, USB - complete hardware inventory
  - icon: 📊
    title: Real-time Monitoring
    details: Temperature, load, frequency tracking with beautiful charts
  - icon: 📋
    title: Event Log Analysis
    details: Automatic System/Application error extraction
  - icon: ✅
    title: Health Scoring
    details: Comprehensive 0-100 system health assessment
---

<!-- Language Selection Section -->
<div class="language-selection vp-doc">
  <h2 style="text-align: center; margin-bottom: 2rem;">Choose Your Language / 选择语言</h2>
  
  <div class="lang-grid">
    <!-- Chinese Entry -->
    <a href="/zh-CN/" class="lang-card">
      <div class="lang-flag">🇨🇳</div>
      <div class="lang-info">
        <h3>简体中文</h3>
        <p>Windows 深度诊断工具</p>
        <span class="lang-link">进入中文站 →</span>
      </div>
    </a>
    
    <!-- English Entry -->
    <a href="/en-US/" class="lang-card">
      <div class="lang-flag">🇺🇸</div>
      <div class="lang-info">
        <h3>English</h3>
        <p>Windows Deep Diagnostics Tool</p>
        <span class="lang-link">Enter English Site →</span>
      </div>
    </a>
  </div>
</div>

<!-- Quick Stats Section -->
<div class="quick-stats">
  <GitHubStats />
</div>

<!-- Quick Links -->
<div class="quick-links vp-doc">
  <div class="links-grid">
    <a href="https://github.com/LessUp/dig-your-windows/releases/latest" target="_blank" rel="noopener" class="quick-link">
      <span class="link-icon">💻</span>
      <div class="link-content">
        <strong>Download</strong>
        <span>Get the latest release</span>
      </div>
    </a>
    
    <a href="https://github.com/LessUp/dig-your-windows" target="_blank" rel="noopener" class="quick-link">
      <span class="link-icon">
        <svg viewBox="0 0 24 24" width="24" height="24" fill="currentColor">
          <path d="M12 0c-6.626 0-12 5.373-12 12 0 5.302 3.438 9.8 8.207 11.387.599.111.793-.261.793-.577v-2.234c-3.338.726-4.033-1.416-4.033-1.416-.546-1.387-1.333-1.756-1.333-1.756-1.089-.745.083-.729.083-.729 1.205.084 1.839 1.237 1.839 1.237 1.07 1.834 2.807 1.304 3.492.997.107-.775.418-1.305.762-1.604-2.665-.305-5.467-1.334-5.467-5.931 0-1.311.469-2.381 1.236-3.221-.124-.303-.535-1.524.117-3.176 0 0 1.008-.322 3.301 1.23.957-.266 1.983-.399 3.003-.404 1.02.005 2.047.138 3.006.404 2.291-1.552 3.297-1.23 3.297-1.23.653 1.653.242 2.874.118 3.176.77.84 1.235 1.911 1.235 3.221 0 4.609-2.807 5.624-5.479 5.921.43.372.823 1.102.823 2.222v3.293c0 .319.192.694.801.576 4.765-1.589 8.199-6.086 8.199-11.386 0-6.627-5.373-12-12-12z"/>
        </svg>
      </span>
      <div class="link-content">
        <strong>GitHub</strong>
        <span>View source code</span>
      </div>
    </a>
    
    <a href="/zh-CN/guide/getting-started" class="quick-link">
      <span class="link-icon">📖</span>
      <div class="link-content">
        <strong>Documentation</strong>
        <span>Getting started guide</span>
      </div>
    </a>
  </div>
</div>

<!-- Info Section -->
<div class="info-section vp-doc">

## About DigYourWindows

**DigYourWindows** is an open-source Windows system diagnostics tool that helps users:

- 🔍 **Collect comprehensive hardware information**
- 📊 **Monitor system performance in real-time**
- 📋 **Analyze Windows event logs automatically**
- ✅ **Get system health scores and recommendations**

### System Requirements

| Component | Requirement |
|-----------|-------------|
| OS | Windows 10/11 (Build 19041+) |
| Runtime | .NET 10.0 Runtime or SDK |
| Permissions | Administrator (for full features) |

### License

[MIT License](https://github.com/LessUp/dig-your-windows/blob/master/LICENSE) - Copyright © 2025-2026 LessUp

</div>

<style scoped>
.language-selection {
  max-width: 800px;
  margin: 0 auto;
  padding: 2rem 1.5rem;
}

.lang-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
  gap: 1.5rem;
}

.lang-card {
  display: flex;
  align-items: center;
  gap: 1.25rem;
  padding: 1.5rem;
  background: var(--vp-c-bg-soft);
  border: 2px solid var(--vp-c-divider);
  border-radius: 16px;
  text-decoration: none;
  transition: all 0.3s ease;
}

.lang-card:hover {
  border-color: var(--vp-c-brand-1);
  transform: translateY(-4px);
  box-shadow: 0 12px 40px -12px rgba(0, 0, 0, 0.15);
}

.lang-flag {
  font-size: 3rem;
  line-height: 1;
}

.lang-info h3 {
  margin: 0 0 0.25rem;
  color: var(--vp-c-text-1);
  font-size: 1.25rem;
}

.lang-info p {
  margin: 0 0 0.75rem;
  color: var(--vp-c-text-2);
  font-size: 0.9rem;
}

.lang-link {
  color: var(--vp-c-brand-1);
  font-weight: 600;
  font-size: 0.9rem;
}

.quick-stats {
  max-width: 600px;
  margin: 0 auto;
  padding: 0 1.5rem;
}

.quick-links {
  max-width: 900px;
  margin: 2rem auto;
  padding: 0 1.5rem;
}

.links-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 1rem;
}

.quick-link {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1rem 1.25rem;
  background: var(--vp-c-bg-soft);
  border: 1px solid var(--vp-c-divider);
  border-radius: 12px;
  text-decoration: none;
  transition: all 0.2s ease;
}

.quick-link:hover {
  background: var(--vp-c-bg);
  border-color: var(--vp-c-brand-1);
}

.link-icon {
  font-size: 1.5rem;
  display: flex;
  align-items: center;
  justify-content: center;
  width: 40px;
  height: 40px;
}

.link-icon svg {
  width: 28px;
  height: 28px;
}

.link-content {
  display: flex;
  flex-direction: column;
}

.link-content strong {
  color: var(--vp-c-text-1);
  font-size: 1rem;
}

.link-content span {
  color: var(--vp-c-text-2);
  font-size: 0.8rem;
}

.info-section {
  max-width: 800px;
  margin: 3rem auto;
  padding: 0 1.5rem;
  text-align: center;
}

@media (max-width: 640px) {
  .lang-card {
    padding: 1.25rem;
  }
  
  .lang-flag {
    font-size: 2.5rem;
  }
}
</style>
