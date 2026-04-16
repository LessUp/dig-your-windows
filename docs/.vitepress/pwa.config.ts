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
    
    icons: [
      {
        src: '/icons/icon-72x72.png',
        sizes: '72x72',
        type: 'image/png',
        purpose: 'maskable any',
      },
      {
        src: '/icons/icon-96x96.png',
        sizes: '96x96',
        type: 'image/png',
        purpose: 'maskable any',
      },
      {
        src: '/icons/icon-128x128.png',
        sizes: '128x128',
        type: 'image/png',
        purpose: 'maskable any',
      },
      {
        src: '/icons/icon-144x144.png',
        sizes: '144x144',
        type: 'image/png',
        purpose: 'maskable any',
      },
      {
        src: '/icons/icon-152x152.png',
        sizes: '152x152',
        type: 'image/png',
        purpose: 'maskable any',
      },
      {
        src: '/icons/icon-192x192.png',
        sizes: '192x192',
        type: 'image/png',
        purpose: 'maskable any',
      },
      {
        src: '/icons/icon-384x384.png',
        sizes: '384x384',
        type: 'image/png',
        purpose: 'maskable any',
      },
      {
        src: '/icons/icon-512x512.png',
        sizes: '512x512',
        type: 'image/png',
        purpose: 'maskable any',
      },
    ],
    
    categories: ['developer', 'developer tools', 'utilities'],
    screenshots: [
      {
        src: '/screenshots/home-wide.png',
        sizes: '1280x720',
        type: 'image/png',
        form_factor: 'wide',
        label: 'DigYourWindows Documentation Home',
      },
      {
        src: '/screenshots/home-narrow.png',
        sizes: '750x1334',
        type: 'image/png',
        form_factor: 'narrow',
        label: 'DigYourWindows Documentation Mobile',
      },
    ],
    
    shortcuts: [
      {
        name: '快速开始',
        short_name: 'Start',
        description: '快速开始指南',
        url: '/zh-CN/guide/getting-started',
        icons: [{ src: '/icons/shortcut-start.png', sizes: '96x96' }],
      },
      {
        name: 'Getting Started',
        short_name: 'Start',
        description: 'Getting Started Guide',
        url: '/en-US/guide/getting-started',
        icons: [{ src: '/icons/shortcut-start.png', sizes: '96x96' }],
      },
      {
        name: 'GitHub',
        short_name: 'GitHub',
        description: 'View on GitHub',
        url: 'https://github.com/LessUp/dig-your-windows',
        icons: [{ src: '/icons/shortcut-github.png', sizes: '96x96' }],
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
    'sitemap.xml',
    'favicon.ico',
    'apple-touch-icon.png',
    'safari-pinned-tab.svg',
    'browserconfig.xml',
    'opensearch.xml',
  ],
}
