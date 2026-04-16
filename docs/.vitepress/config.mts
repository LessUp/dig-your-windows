import { defineConfig } from 'vitepress'

export default defineConfig({
  lang: 'zh-CN',
  title: 'DigYourWindows',
  description: 'Windows 深度诊断工具 — 一键采集硬件信息、事件日志、可靠性记录，生成系统健康评分与优化建议',

  // GitHub Pages 部署
  base: '/dig-your-windows/',

  cleanUrls: true,

  // Sitemap 配置
  sitemap: {
    hostname: 'https://lessup.github.io/dig-your-windows/',
    changefreq: 'weekly',
    priority: 0.7,
  },

  head: [
    ['link', { rel: 'canonical', href: 'https://lessup.github.io/dig-your-windows/' }],
    ['meta', { name: 'theme-color', content: '#0078d4' }],
    ['meta', { property: 'og:type', content: 'website' }],
    ['meta', { property: 'og:title', content: 'DigYourWindows' }],
    ['meta', { property: 'og:description', content: 'Windows 深度诊断工具 — 一键采集硬件信息、事件日志、可靠性记录，生成系统健康评分与优化建议' }],
    ['meta', { property: 'og:url', content: 'https://lessup.github.io/dig-your-windows/' }],
    ['meta', { property: 'og:image', content: 'https://lessup.github.io/dig-your-windows/og-image.png' }],
    ['meta', { name: 'twitter:card', content: 'summary_large_image' }],
    ['meta', { name: 'twitter:title', content: 'DigYourWindows' }],
    ['meta', { name: 'twitter:description', content: 'Windows 深度诊断工具 — 采集硬件信息、事件日志与系统健康评分' }],
    ['meta', { name: 'keywords', content: 'Windows,诊断,硬件信息,事件日志,SMART,WPF,.NET,系统健康,LibreHardwareMonitor' }],
    ['link', { rel: 'icon', href: '/favicon.ico', type: 'image/x-icon' }],
    ['link', { rel: 'apple-touch-icon', href: '/apple-touch-icon.png' }],
  ],

  srcExclude: [
    'diagnostic-data-schema.json',
    'TEST_INFRASTRUCTURE_SETUP.md',
  ],

  markdown: {
    lineNumbers: true,
  },

  lastUpdated: true,

  themeConfig: {
    logo: '/logo.svg',

    nav: [
      { text: '首页', link: '/' },
      {
        text: '指南',
        items: [
          { text: '快速开始', link: '/guide/getting-started' },
          { text: '项目架构', link: '/guide/architecture' },
          { text: '测试指南', link: '/guide/testing' },
          { text: '贡献指南', link: '/guide/contributing' },
          { text: 'FAQ', link: '/guide/faq' },
        ],
      },
      {
        text: '参考',
        items: [
          { text: '数据 Schema', link: '/reference/data-schema' },
        ],
      },
      { text: '变更日志', link: '/changelog' },
    ],

    sidebar: {
      '/guide/': [
        {
          text: '指南',
          items: [
            { text: '快速开始', link: '/guide/getting-started' },
            { text: '项目架构', link: '/guide/architecture' },
            { text: '测试指南', link: '/guide/testing' },
            { text: '贡献指南', link: '/guide/contributing' },
            { text: 'FAQ', link: '/guide/faq' },
          ],
        },
      ],
      '/reference/': [
        {
          text: '参考',
          items: [
            { text: '数据 Schema', link: '/reference/data-schema' },
          ],
        },
      ],
      '/': [
        {
          text: '开始使用',
          items: [
            { text: '快速开始', link: '/guide/getting-started' },
            { text: '项目架构', link: '/guide/architecture' },
          ],
        },
        {
          text: '开发指南',
          items: [
            { text: '测试指南', link: '/guide/testing' },
            { text: '贡献指南', link: '/guide/contributing' },
            { text: 'FAQ', link: '/guide/faq' },
          ],
        },
        {
          text: '参考文档',
          items: [
            { text: '数据 Schema', link: '/reference/data-schema' },
            { text: '变更日志', link: '/changelog' },
          ],
        },
      ],
    },

    editLink: {
      pattern: 'https://github.com/LessUp/dig-your-windows/edit/master/docs/:path',
      text: '在 GitHub 上编辑此页',
    },

    socialLinks: [
      { icon: 'github', link: 'https://github.com/LessUp/dig-your-windows' },
    ],

    footer: {
      message: '基于 MIT 许可发布',
      copyright: 'Copyright © 2025-2026 LessUp',
    },

    search: {
      provider: 'local',
      options: {
        translations: {
          button: {
            buttonText: '搜索文档',
            buttonAriaLabel: '搜索文档',
          },
          modal: {
            noResultsText: '无法找到相关结果',
            resetButtonTitle: '清除查询条件',
            footer: {
              selectText: '选择',
              navigateText: '切换',
              closeText: '关闭',
            },
          },
        },
      },
    },

    outline: {
      level: [2, 3],
      label: '页面导航',
    },

    lastUpdated: {
      text: '最后更新',
      formatOptions: {
        dateStyle: 'short',
        timeStyle: 'medium',
      },
    },

    docFooter: {
      prev: '上一页',
      next: '下一页',
    },

    returnToTopLabel: '返回顶部',
    sidebarMenuLabel: '菜单',
    darkModeSwitchLabel: '主题',
    lightModeSwitchTitle: '切换到浅色模式',
    darkModeSwitchTitle: '切换到深色模式',
    externalLinkIcon: true,
  },
})
