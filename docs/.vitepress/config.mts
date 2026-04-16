import { defineConfig } from 'vitepress'

// https://vitepress.dev/reference/site-config
export default defineConfig({
  // GitHub Pages deployment
  base: '/dig-your-windows/',
  
  cleanUrls: true,
  
  // Sitemap configuration
  sitemap: {
    hostname: 'https://lessup.github.io/dig-your-windows/',
    changefreq: 'weekly',
    priority: 0.7,
  },
  
  srcExclude: [
    'diagnostic-data-schema.json',
    'TEST_INFRASTRUCTURE_SETUP.md',
  ],
  
  markdown: {
    lineNumbers: true,
  },
  
  lastUpdated: true,
  
  // Internationalization configuration
  locales: {
    root: {
      label: '简体中文',
      lang: 'zh-CN',
      themeConfig: {
        nav: [
          { text: '首页', link: '/zh-CN/' },
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
      themeConfig: {
        nav: [
          { text: 'Home', link: '/en-US/' },
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
              items: [
                { text: 'Data Schema', link: '/en-US/reference/data-schema' },
              ],
            },
          ],
        },
      },
    },
  },
  
  themeConfig: {
    logo: '/logo.svg',
    
    editLink: {
      pattern: 'https://github.com/LessUp/dig-your-windows/edit/main/docs/:path',
      text: 'Edit this page on GitHub',
    },
    
    socialLinks: [
      { icon: 'github', link: 'https://github.com/LessUp/dig-your-windows' },
    ],
    
    footer: {
      message: 'Released under the MIT License',
      copyright: 'Copyright © 2025-2026 LessUp',
    },
    
    search: {
      provider: 'local',
      options: {
        locales: {
          'zh-CN': {
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
          'en-US': {
            translations: {
              button: {
                buttonText: 'Search Documentation',
                buttonAriaLabel: 'Search Documentation',
              },
              modal: {
                noResultsText: 'No results found',
                resetButtonTitle: 'Clear query',
                footer: {
                  selectText: 'Select',
                  navigateText: 'Navigate',
                  closeText: 'Close',
                },
              },
            },
          },
        },
      },
    },
    
    outline: {
      level: [2, 3],
      label: 'On this page',
    },
    
    lastUpdated: {
      text: 'Last updated',
      formatOptions: {
        dateStyle: 'short',
        timeStyle: 'medium',
      },
    },
    
    docFooter: {
      prev: 'Previous page',
      next: 'Next page',
    },
    
    returnToTopLabel: 'Return to top',
    sidebarMenuLabel: 'Menu',
    darkModeSwitchLabel: 'Theme',
    lightModeSwitchTitle: 'Switch to light mode',
    darkModeSwitchTitle: 'Switch to dark mode',
    externalLinkIcon: true,
  },
})
