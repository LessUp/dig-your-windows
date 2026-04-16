import { h, watch } from 'vue'
import { useData, useRoute } from 'vitepress'
import DefaultTheme from 'vitepress/theme'
import type { Theme } from 'vitepress'
import './styles/custom.css'

// Custom components
import HomeFeatures from './components/HomeFeatures.vue'
import DocFooter from './components/DocFooter.vue'

export default {
  extends: DefaultTheme,
  
  Layout() {
    const { Layout } = DefaultTheme
    const route = useRoute()
    
    return h(Layout, null, {
      // Add custom slots
      'home-hero-before': () => h(HomeFeatures),
      'doc-footer-before': () => h(DocFooter),
    })
  },
  
  enhanceApp({ app, router, siteData }) {
    // Register global components
    app.component('HomeFeatures', HomeFeatures)
    
    // Smooth scroll behavior
    if (typeof window !== 'undefined') {
      // Handle anchor links with smooth scroll
      document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
          e.preventDefault()
          const target = document.querySelector(this.getAttribute('href'))
          if (target) {
            target.scrollIntoView({
              behavior: 'smooth',
              block: 'start'
            })
          }
        })
      })
      
      // Add copy button to code blocks
      const addCopyButtons = () => {
        document.querySelectorAll('.vp-code-block').forEach(block => {
          if (block.querySelector('.copy-button')) return
          
          const button = document.createElement('button')
          button.className = 'copy-button'
          button.innerHTML = '<span class="copy-icon">📋</span> Copy'
          button.setAttribute('aria-label', 'Copy code to clipboard')
          
          button.addEventListener('click', async () => {
            const code = block.querySelector('code')?.textContent
            if (code) {
              await navigator.clipboard.writeText(code)
              button.innerHTML = '<span class="copy-icon">✓</span> Copied!'
              button.classList.add('copied')
              setTimeout(() => {
                button.innerHTML = '<span class="copy-icon">📋</span> Copy'
                button.classList.remove('copied')
              }, 2000)
            }
          })
          
          block.appendChild(button)
        })
      }
      
      // Run on initial load and route change
      addCopyButtons()
      
      // Web Vitals tracking (optional)
      if ('web-vitals' in window) {
        // @ts-ignore
        import('web-vitals').then(({ getCLS, getFID, getFCP, getLCP, getTTFB }) => {
          getCLS(console.log)
          getFID(console.log)
          getFCP(console.log)
          getLCP(console.log)
          getTTFB(console.log)
        })
      }
    }
  },
  
  setup() {
    const route = useRoute()
    const { page, site, frontmatter } = useData()
    
    // Watch for route changes to update meta tags
    watch(
      () => route.path,
      () => {
        if (typeof window !== 'undefined') {
          // Update page-specific meta
          updatePageMeta()
        }
      }
    )
    
    // Update meta tags based on current page
    const updatePageMeta = () => {
      const path = route.path
      const lang = path.startsWith('/en-US') ? 'en-US' : 'zh-CN'
      
      // Update HTML lang attribute
      document.documentElement.lang = lang === 'zh-CN' ? 'zh-CN' : 'en'
      
      // Update Open Graph locale
      const ogLocale = document.querySelector('meta[property="og:locale"]')
      if (ogLocale) {
        ogLocale.setAttribute('content', lang === 'zh-CN' ? 'zh_CN' : 'en_US')
      }
      
      // Update canonical URL
      const canonical = document.querySelector('link[rel="canonical"]')
      if (canonical) {
        const baseUrl = 'https://lessup.github.io/dig-your-windows'
        const cleanPath = path.replace(/\.html$/, '').replace(/\/index$/, '/')
        canonical.setAttribute('href', `${baseUrl}${cleanPath}`)
      }
    }
  }
} satisfies Theme
