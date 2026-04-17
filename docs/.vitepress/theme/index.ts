import { h, watch } from 'vue'
import { useData, useRoute } from 'vitepress'
import DefaultTheme from 'vitepress/theme'
import type { Theme } from 'vitepress'
import './styles/custom.css'

// Custom components
import HomeFeatures from './components/HomeFeatures.vue'
import DocFooter from './components/DocFooter.vue'
import BackToTop from './components/BackToTop.vue'

export default {
  extends: DefaultTheme,
  
  Layout() {
    const { Layout } = DefaultTheme
    const route = useRoute()
    
    return h(Layout, null, {
      'home-hero-before': () => h(HomeFeatures),
      'doc-footer-before': () => h(DocFooter),
      'layout-bottom': () => h(BackToTop),
    })
  },
  
  enhanceApp({ app, router }) {
    // Register global components
    app.component('HomeFeatures', HomeFeatures)
    app.component('BackToTop', BackToTop)
    
    // Handle route navigation after page load
    router.onAfterRouteChanged = (to) => {
      if (typeof window !== 'undefined') {
        // Handle hash anchor links with smooth scroll
        if (to.hash) {
          setTimeout(() => {
            const target = document.querySelector(to.hash)
            if (target) {
              target.scrollIntoView({ behavior: 'smooth', block: 'start' })
            }
          }, 100)
        }
      }
    }
  },
  
  setup() {
    const route = useRoute()
    const { lang } = useData()
    
    // Watch for route changes to update page metadata
    watch(
      () => route.path,
      () => {
        if (typeof window !== 'undefined') {
          const currentLang = route.path.startsWith('/en-US') ? 'en' : 'zh-CN'
          document.documentElement.lang = currentLang
          
          // Update Open Graph locale
          const ogLocale = document.querySelector('meta[property="og:locale"]')
          if (ogLocale) {
            ogLocale.setAttribute('content', currentLang === 'zh-CN' ? 'zh_CN' : 'en_US')
          }
          
          // Update canonical URL
          const canonical = document.querySelector('link[rel="canonical"]')
          if (canonical) {
            const baseUrl = 'https://lessup.github.io/dig-your-windows'
            const cleanPath = route.path.replace(/\.html$/, '').replace(/\/index$/, '/')
            canonical.setAttribute('href', `${baseUrl}${cleanPath}`)
          }
        }
      },
      { immediate: true }
    )
  }
} satisfies Theme
