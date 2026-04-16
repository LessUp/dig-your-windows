import { defineConfig } from 'vitepress'
import { fileURLToPath, URL } from 'node:url'

// https://vitepress.dev/reference/site-config
export default defineConfig({
  // GitHub Pages deployment
  base: '/dig-your-windows/',
  
  // Site metadata
  title: 'DigYourWindows',
  titleTemplate: ':title | Windows Deep Diagnostics Tool',
  description: 'Windows Deep Diagnostics Tool — One-click hardware info collection, event log analysis, and system health scoring',
  lang: 'en-US',
  
  // Clean URLs (no .html extension)
  cleanUrls: true,
  
  // Last updated timestamp
  lastUpdated: true,
  
  // Build optimization
  srcExclude: [
    '**/diagnostic-data-schema.json',
    '**/TEST_INFRASTRUCTURE_SETUP.md',
    '**/node_modules/**',
    '**/.vitepress/cache/**',
    '**/dist/**',
  ],
  
  // Ignore dead links (for external URLs)
  ignoreDeadLinks: [
    /^https?:\/\/localhost/,
    /^https?:\/\/127\.0\.0\.1/,
  ],
  
  // Sitemap configuration
  sitemap: {
    hostname: 'https://lessup.github.io/dig-your-windows/',
    lastmodDateOnly: false,
    changefreq: 'weekly',
    priority: 0.7,
    transformItems: (items) => {
      return items.map((item) => {
        // Set higher priority for main pages
        if (item.url === 'index.md' || item.url === 'zh-CN/index.md') {
          return { ...item, priority: 1.0, changefreq: 'daily' }
        }
        if (item.url.includes('getting-started')) {
          return { ...item, priority: 0.9 }
        }
        if (item.url.includes('changelog')) {
          return { ...item, changefreq: 'daily' }
        }
        return item
      })
    },
  },
  
  // Head configuration
  head: [
    // Primary charset and viewport
    ['meta', { charset: 'utf-8' }],
    ['meta', { name: 'viewport', content: 'width=device-width, initial-scale=1.0, maximum-scale=5.0' }],
    
    // Color scheme
    ['meta', { name: 'color-scheme', content: 'light dark' }],
    ['meta', { name: 'theme-color', content: '#0078d4', media: '(prefers-color-scheme: light)' }],
    ['meta', { name: 'theme-color', content: '#0078d4', media: '(prefers-color-scheme: dark)' }],
    
    // Canonical URL
    ['link', { rel: 'canonical', href: 'https://lessup.github.io/dig-your-windows/' }],
    
    // Favicon and icons
    ['link', { rel: 'icon', href: '/favicon.ico', sizes: 'any' }],
    ['link', { rel: 'icon', type: 'image/svg+xml', href: '/logo.svg' }],
    ['link', { rel: 'apple-touch-icon', sizes: '180x180', href: '/apple-touch-icon.png' }],
    ['link', { rel: 'mask-icon', href: '/safari-pinned-tab.svg', color: '#0078d4' }],
    
    // Preconnect to external resources
    ['link', { rel: 'dns-prefetch', href: 'https://api.github.com' }],
    ['link', { rel: 'preconnect', href: 'https://fonts.googleapis.com' }],
    ['link', { rel: 'preconnect', href: 'https://fonts.gstatic.com', crossorigin: '' }],
    
    // SEO Meta tags
    ['meta', { name: 'author', content: 'LessUp' }],
    ['meta', { name: 'robots', content: 'index, follow, max-image-preview:large, max-snippet:-1, max-video-preview:-1' }],
    ['meta', { name: 'googlebot', content: 'index, follow, max-snippet:-1, max-image-preview:large, max-video-preview:-1' }],
    ['meta', { name: 'bingbot', content: 'index, follow, max-snippet:-1, max-image-preview:large, max-video-preview:-1' }],
    
    // Open Graph / Facebook
    ['meta', { property: 'og:site_name', content: 'DigYourWindows' }],
    ['meta', { property: 'og:type', content: 'website' }],
    ['meta', { property: 'og:locale', content: 'zh_CN' }],
    ['meta', { property: 'og:locale:alternate', content: 'en_US' }],
    ['meta', { property: 'og:title', content: 'DigYourWindows - Windows Deep Diagnostics Tool' }],
    ['meta', { property: 'og:description', content: 'One-click hardware info collection, event log analysis, reliability records, system health scoring, and optimization recommendations.' }],
    ['meta', { property: 'og:url', content: 'https://lessup.github.io/dig-your-windows/' }],
    ['meta', { property: 'og:image', content: 'https://lessup.github.io/dig-your-windows/og-image.png' }],
    ['meta', { property: 'og:image:width', content: '1200' }],
    ['meta', { property: 'og:image:height', content: '630' }],
    ['meta', { property: 'og:image:alt', content: 'DigYourWindows - Windows Deep Diagnostics Tool' }],
    
    // Twitter Card
    ['meta', { name: 'twitter:card', content: 'summary_large_image' }],
    ['meta', { name: 'twitter:site', content: '@LessUp' }],
    ['meta', { name: 'twitter:creator', content: '@LessUp' }],
    ['meta', { name: 'twitter:title', content: 'DigYourWindows - Windows Deep Diagnostics Tool' }],
    ['meta', { name: 'twitter:description', content: 'One-click hardware info collection, event log analysis, reliability records, system health scoring, and optimization recommendations.' }],
    ['meta', { name: 'twitter:image', content: 'https://lessup.github.io/dig-your-windows/og-image.png' }],
    ['meta', { name: 'twitter:image:alt', content: 'DigYourWindows Interface Preview' }],
    
    // Apple Meta Tags
    ['meta', { name: 'apple-mobile-web-app-capable', content: 'yes' }],
    ['meta', { name: 'apple-mobile-web-app-status-bar-style', content: 'black-translucent' }],
    ['meta', { name: 'apple-mobile-web-app-title', content: 'DigYourWindows' }],
    ['meta', { name: 'format-detection', content: 'telephone=no' }],
    
    // Microsoft Meta Tags
    ['meta', { name: 'msapplication-TileColor', content: '#0078d4' }],
    ['meta', { name: 'msapplication-TileImage', content: '/mstile-144x144.png' }],
    ['meta', { name: 'msapplication-config', content: '/browserconfig.xml' }],
    
    // Keywords
    ['meta', { name: 'keywords', content: 'Windows diagnostics, system health, hardware monitoring, event log analysis, SMART data, CPU monitoring, GPU monitoring, system optimization, Windows 10, Windows 11, .NET, WPF, open source' }],
    
    // Referrer Policy
    ['meta', { name: 'referrer', content: 'strict-origin-when-cross-origin' }],
    
    // Mobile Web App
    ['link', { rel: 'manifest', href: '/manifest.json' }],
    
    // Structured Data - JSON-LD (SoftwareApplication)
    ['script', { type: 'application/ld+json' }, JSON.stringify({
      '@context': 'https://schema.org',
      '@type': 'SoftwareApplication',
      name: 'DigYourWindows',
      alternateName: 'Windows Deep Diagnostics Tool',
      description: 'Windows Deep Diagnostics Tool — One-click hardware info collection, event log analysis, and system health scoring',
      applicationCategory: 'UtilitiesApplication',
      operatingSystem: 'Windows 10, Windows 11',
      softwareVersion: '1.1.0',
      license: 'https://github.com/LessUp/dig-your-windows/blob/main/LICENSE',
      url: 'https://lessup.github.io/dig-your-windows/',
      codeRepository: 'https://github.com/LessUp/dig-your-windows',
      programmingLanguage: ['C#', '.NET', 'WPF'],
      offers: {
        '@type': 'Offer',
        price: '0',
        priceCurrency: 'USD'
      },
      author: {
        '@type': 'Organization',
        name: 'LessUp',
        url: 'https://github.com/LessUp'
      },
      publisher: {
        '@type': 'Organization',
        name: 'LessUp',
        logo: {
          '@type': 'ImageObject',
          url: 'https://lessup.github.io/dig-your-windows/logo.png'
        }
      },
      sameAs: [
        'https://github.com/LessUp/dig-your-windows'
      ],
      image: 'https://lessup.github.io/dig-your-windows/og-image.png',
      screenshot: 'https://lessup.github.io/dig-your-windows/screenshot-overview.png',
      featureList: [
        'Hardware information collection',
        'Real-time CPU/GPU monitoring',
        'Event log analysis',
        'Windows reliability records',
        'System health scoring',
        'SMART data reading'
      ]
    })],
    
    // WebSite Structured Data
    ['script', { type: 'application/ld+json' }, JSON.stringify({
      '@context': 'https://schema.org',
      '@type': 'WebSite',
      name: 'DigYourWindows Documentation',
      url: 'https://lessup.github.io/dig-your-windows/',
      potentialAction: {
        '@type': 'SearchAction',
        target: {
          '@type': 'EntryPoint',
          'urlTemplate': 'https://lessup.github.io/dig-your-windows/search?q={search_term_string}'
        },
        'query-input': 'required name=search_term_string'
      },
      inLanguage: ['zh-CN', 'en-US']
    })],
    
    // Organization Structured Data
    ['script', { type: 'application/ld+json' }, JSON.stringify({
      '@context': 'https://schema.org',
      '@type': 'Organization',
      name: 'LessUp',
      url: 'https://github.com/LessUp',
      logo: 'https://lessup.github.io/dig-your-windows/logo.png',
      sameAs: [
        'https://github.com/LessUp'
      ]
    })],
  ],
  
  // Markdown configuration
  markdown: {
    lineNumbers: true,
  },
  
  // Vite configuration
  vite: {
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./theme', import.meta.url)),
        'components': fileURLToPath(new URL('./theme/components', import.meta.url)),
      },
    },
    build: {
      chunkSizeWarningLimit: 1000,
    },
    optimizeDeps: {
      include: ['vue', '@vueuse/core'],
    },
    ssr: {
      external: ['web-vitals'],
    },
  },
  
  // Theme configuration
  themeConfig: {
    logo: {
      src: '/logo.svg',
      alt: 'DigYourWindows Logo',
    },
    
    siteTitle: 'DigYourWindows',
    
    nav: [],
    
    sidebar: {},
    
    search: {
      provider: 'local',
      options: {
        detailedView: true,
        miniSearch: {
          searchOptions: {
            fuzzy: 0.2,
            boost: { title: 4, text: 2, titles: 1 },
          },
        },
        locales: {
          'zh-CN': {
            translations: {
              button: {
                buttonText: '搜索文档',
                buttonAriaLabel: '搜索文档',
              },
              modal: {
                displayDetails: '显示详细列表',
                resetButtonTitle: '清除查询条件',
                backButtonTitle: '关闭搜索',
                noResultsText: '无法找到相关结果',
                footer: {
                  selectText: '选择',
                  selectKeyAriaLabel: '回车键',
                  navigateText: '切换',
                  navigateUpKeyAriaLabel: '向上箭头',
                  navigateDownKeyAriaLabel: '向下箭头',
                  closeText: '关闭',
                  closeKeyAriaLabel: 'Esc键',
                },
              },
            },
          },
          'en-US': {
            translations: {
              button: {
                buttonText: 'Search Documentation',
                buttonAriaLabel: 'Search Documentation',
              },
              modal: {
                displayDetails: 'Display detailed list',
                resetButtonTitle: 'Clear query',
                backButtonTitle: 'Close search',
                noResultsText: 'No results found',
                footer: {
                  selectText: 'to select',
                  selectKeyAriaLabel: 'Enter',
                  navigateText: 'to navigate',
                  navigateUpKeyAriaLabel: 'Up arrow',
                  navigateDownKeyAriaLabel: 'Down arrow',
                  closeText: 'to close',
                  closeKeyAriaLabel: 'Escape',
                },
              },
            },
          },
        },
      },
    },
    
    editLink: {
      pattern: 'https://github.com/LessUp/dig-your-windows/edit/main/docs/:path',
      text: 'Edit this page on GitHub',
    },
    
    socialLinks: [
      { icon: 'github', link: 'https://github.com/LessUp/dig-your-windows' },
    ],
    
    footer: {
      message: 'Released under the <a href="https://github.com/LessUp/dig-your-windows/blob/main/LICENSE" target="_blank" rel="noopener">MIT License</a>.',
      copyright: 'Copyright © 2025-2026 <a href="https://github.com/LessUp" target="_blank" rel="noopener">LessUp</a>',
    },
    
    outline: {
      level: [2, 3],
      label: 'On this page',
    },
    
    lastUpdated: {
      text: 'Last updated',
      formatOptions: {
        dateStyle: 'short',
        timeStyle: 'short',
      },
    },
    
    docFooter: {
      prev: 'Previous page',
      next: 'Next page',
    },
    
    returnToTopLabel: 'Return to top',
    sidebarMenuLabel: 'Menu',
    langMenuLabel: 'Change language',
    darkModeSwitchLabel: 'Appearance',
    lightModeSwitchTitle: 'Switch to light theme',
    darkModeSwitchTitle: 'Switch to dark theme',
    externalLinkIcon: true,
    appearance: true,
  },
  
  locales: {
    root: {
      label: '简体中文',
      lang: 'zh-CN',
      link: '/zh-CN/',
      themeConfig: {
        nav: [
          { text: '首页', link: '/zh-CN/', activeMatch: '^/zh-CN/$' },
          {
            text: '指南',
            items: [
              { text: '快速开始', link: '/zh-CN/guide/getting-started' },
              { text: '项目架构', link: '/zh-CN/guide/architecture' },
              { text: '测试指南', link: '/zh-CN/guide/testing' },
              { text: '贡献指南', link: '/zh-CN/guide/contributing' },
              { text: 'FAQ', link: '/zh-CN/guide/faq' },
            ],
          },
          {
            text: '参考',
            items: [
              { text: '数据 Schema', link: '/zh-CN/reference/data-schema' },
            ],
          },
          { text: '变更日志', link: '/zh-CN/changelog' },
        ],
        
        sidebar: {
          '/zh-CN/guide/': [
            {
              text: '指南',
              collapsed: false,
              items: [
                { text: '快速开始', link: '/zh-CN/guide/getting-started' },
                { text: '项目架构', link: '/zh-CN/guide/architecture' },
                { text: '测试指南', link: '/zh-CN/guide/testing' },
                { text: '贡献指南', link: '/zh-CN/guide/contributing' },
                { text: 'FAQ', link: '/zh-CN/guide/faq' },
              ],
            },
          ],
          '/zh-CN/reference/': [
            {
              text: '参考',
              collapsed: false,
              items: [
                { text: '数据 Schema', link: '/zh-CN/reference/data-schema' },
              ],
            },
          ],
        },
      },
    },
    'en-US': {
      label: 'English',
      lang: 'en-US',
      link: '/en-US/',
      themeConfig: {
        nav: [
          { text: 'Home', link: '/en-US/', activeMatch: '^/en-US/$' },
          {
            text: 'Guide',
            items: [
              { text: 'Getting Started', link: '/en-US/guide/getting-started' },
              { text: 'Architecture', link: '/en-US/guide/architecture' },
              { text: 'Testing Guide', link: '/en-US/guide/testing' },
              { text: 'Contributing', link: '/en-US/guide/contributing' },
              { text: 'FAQ', link: '/en-US/guide/faq' },
            ],
          },
          {
            text: 'Reference',
            items: [
              { text: 'Data Schema', link: '/en-US/reference/data-schema' },
            ],
          },
          { text: 'Changelog', link: '/en-US/changelog' },
        ],
        
        sidebar: {
          '/en-US/guide/': [
            {
              text: 'Guide',
              collapsed: false,
              items: [
                { text: 'Getting Started', link: '/en-US/guide/getting-started' },
                { text: 'Architecture', link: '/en-US/guide/architecture' },
                { text: 'Testing Guide', link: '/en-US/guide/testing' },
                { text: 'Contributing', link: '/en-US/guide/contributing' },
                { text: 'FAQ', link: '/en-US/guide/faq' },
              ],
            },
          ],
          '/en-US/reference/': [
            {
              text: 'Reference',
              collapsed: false,
              items: [
                { text: 'Data Schema', link: '/en-US/reference/data-schema' },
              ],
            },
          ],
        },
      },
    },
  },
})
