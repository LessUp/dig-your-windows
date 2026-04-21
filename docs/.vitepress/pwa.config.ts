import { VitePWAOptions } from 'vite-plugin-pwa'

export const pwaConfig: VitePWAOptions = {
  // PWA Configuration
  strategies: 'generateSW',

  // Service Worker configuration
  workbox: {
    // Cache strategies
    runtimeCaching: [
      {
        urlPattern: /^https:\/\/fonts\.googleapis\.com\/.*/i,
        handler: 'CacheFirst',
        options: {
          cacheName: 'google-fonts-cache',
          expiration: {
            maxEntries: 10,
            maxAgeSeconds: 60 * 60 * 24 * 365, // 1 year
          },
          cacheableResponse: {
            statuses: [0, 200],
          },
        },
      },
      {
        urlPattern: /^https:\/\/fonts\.gstatic\.com\/.*/i,
        handler: 'CacheFirst',
        options: {
          cacheName: 'gstatic-fonts-cache',
          expiration: {
            maxEntries: 10,
            maxAgeSeconds: 60 * 60 * 24 * 365, // 1 year
          },
          cacheableResponse: {
            statuses: [0, 200],
          },
        },
      },
      {
        urlPattern: /\.(?:png|jpg|jpeg|svg|gif|webp|avif)$/i,
        handler: 'CacheFirst',
        options: {
          cacheName: 'images-cache',
          expiration: {
            maxEntries: 100,
            maxAgeSeconds: 60 * 60 * 24 * 30, // 30 days
          },
        },
      },
      {
        urlPattern: /\.(?:js|css)$/i,
        handler: 'StaleWhileRevalidate',
        options: {
          cacheName: 'assets-cache',
          expiration: {
            maxEntries: 100,
            maxAgeSeconds: 60 * 60 * 24 * 7, // 7 days
          },
        },
      },
    ],

    // Skip waiting for service worker activation
    skipWaiting: true,
    clientsClaim: true,

    // Cleanup outdated caches
    cleanupOutdatedCaches: true,

    // Navigation fallback
    navigateFallback: '/index.html',
    navigateFallbackDenylist: [/^\/api/, /\/admin/],
  },

  // Web App Manifest configuration
  manifest: {
    name: 'DigYourWindows Documentation',
    short_name: 'DYW Docs',
    description: 'Windows Deep Diagnostics Tool Documentation - One-click hardware info collection, event log analysis, and system health scoring',
    theme_color: '#0078d4',
    background_color: '#ffffff',
    display: 'standalone',
    orientation: 'portrait',
    scope: '/dig-your-windows/',
    start_url: '/dig-your-windows/',
    lang: 'zh-CN',
    dir: 'ltr',

    categories: ['developer', 'developer tools', 'utilities'],

    shortcuts: [
      {
        name: '快速开始',
        short_name: 'Start',
        description: '快速开始指南',
        url: '/zh-CN/guide/getting-started',
      },
      {
        name: 'Getting Started',
        short_name: 'Start',
        description: 'Getting Started Guide',
        url: '/en-US/guide/getting-started',
      },
      {
        name: 'GitHub',
        short_name: 'GitHub',
        description: 'View on GitHub',
        url: 'https://github.com/LessUp/dig-your-windows',
      },
    ],

    related_applications: [
      {
        platform: 'windows',
        url: 'https://github.com/LessUp/dig-your-windows/releases',
      },
    ],

    prefer_related_applications: false,
  },

  // Dev options
  devOptions: {
    enabled: false, // Disable in development for faster builds
    type: 'module',
    navigateFallback: '/',
  },

  // Inject register script
  injectRegister: 'auto',

  // Register type
  registerType: 'autoUpdate',

  // Include assets
  includeAssets: [
    'robots.txt',
    'favicon.ico',
    'browserconfig.xml',
    'opensearch.xml',
  ],
}
