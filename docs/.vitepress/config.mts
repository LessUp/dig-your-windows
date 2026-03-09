import { defineConfig } from 'vitepress'

export default defineConfig({
  lang: 'zh-CN',
  title: 'DigYourWindows',
  description: 'Windows 深度诊断工具 — 一键采集硬件信息、事件日志、可靠性记录，生成系统健康评分与优化建议',

  // GitHub Pages 部署：base 需要与仓库名一致
  base: '/dig-your-windows/',

  // 排除非文档文件
  srcExclude: [
    'diagnostic-data-schema.json',
    'TEST_INFRASTRUCTURE_SETUP.md',
  ],

  markdown: {
    lineNumbers: true,
  },

  lastUpdated: true,

  themeConfig: {
    nav: [
      { text: '指南', link: '/guide/getting-started' },
      { text: '架构', link: '/guide/architecture' },
      {
        text: '参考',
        items: [
          { text: '测试指南', link: '/guide/testing' },
          { text: '数据 Schema', link: '/reference/data-schema' },
        ],
      },
    ],

    sidebar: [
      {
        text: '指南',
        items: [
          { text: '快速开始', link: '/guide/getting-started' },
          { text: '项目架构', link: '/guide/architecture' },
          { text: '测试指南', link: '/guide/testing' },
        ],
      },
      {
        text: '参考',
        items: [
          { text: '数据 Schema', link: '/reference/data-schema' },
        ],
      },
    ],

    socialLinks: [
      { icon: 'github', link: 'https://github.com/LessUp/dig-your-windows' },
    ],

    search: {
      provider: 'local',
    },

    outline: {
      level: [2, 3],
      label: '目录',
    },

    lastUpdated: {
      text: '最后更新',
    },

    docFooter: {
      prev: '上一页',
      next: '下一页',
    },

    returnToTopLabel: '返回顶部',
    sidebarMenuLabel: '菜单',
    darkModeSwitchLabel: '主题',
  },
})
