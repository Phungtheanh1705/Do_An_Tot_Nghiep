import { onMounted, onUnmounted, nextTick } from 'vue'

export function useScrollReveal(selector = '.reveal-on-scroll', threshold = 0.12) {
  let observer: IntersectionObserver | null = null

  function initReveal() {
    if (typeof window === 'undefined' || !('IntersectionObserver' in window)) {
      // Fallback: Reveal all immediately if IntersectionObserver not supported
      document.querySelectorAll(selector).forEach((el) => el.classList.add('is-revealed'))
      return
    }

    if (observer) {
      observer.disconnect()
    }

    observer = new IntersectionObserver(
      (entries) => {
        entries.forEach((entry) => {
          if (entry.isIntersecting) {
            entry.target.classList.add('is-revealed')
            observer?.unobserve(entry.target)
          }
        })
      },
      {
        root: null,
        rootMargin: '0px 0px -40px 0px',
        threshold
      }
    )

    nextTick(() => {
      const elements = document.querySelectorAll(selector)
      elements.forEach((el) => observer?.observe(el))
    })
  }

  onMounted(() => {
    initReveal()
  })

  onUnmounted(() => {
    if (observer) {
      observer.disconnect()
      observer = null
    }
  })

  return {
    initReveal,
    initScrollReveal: initReveal
  }
}
