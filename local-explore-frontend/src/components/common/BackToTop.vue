<template>
  <Transition name="fade-slide">
    <button
      v-if="isVisible"
      class="back-to-top-btn"
      title="Lên đầu trang"
      aria-label="Cuộn lên đầu trang"
      @click="scrollToTop"
    >
      <UpOutlined class="arrow-icon" />
    </button>
  </Transition>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { UpOutlined } from '@ant-design/icons-vue'

const isVisible = ref(false)

function checkScroll() {
  isVisible.value = window.scrollY > 380
}

function scrollToTop() {
  window.scrollTo({
    top: 0,
    behavior: 'smooth'
  })
}

onMounted(() => {
  window.addEventListener('scroll', checkScroll, { passive: true })
  checkScroll()
})

onUnmounted(() => {
  window.removeEventListener('scroll', checkScroll)
})
</script>

<style scoped>
.back-to-top-btn {
  position: fixed;
  bottom: 30px;
  right: 30px;
  z-index: 99;
  width: 44px;
  height: 44px;
  border-radius: 50%;
  background: #14532d;
  color: #ffffff;
  border: 1px solid rgba(255, 255, 255, 0.2);
  box-shadow: 0 8px 24px -4px rgba(20, 83, 45, 0.4);
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: transform var(--dur-btn, 220ms) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              background-color var(--dur-btn, 220ms) ease,
              box-shadow var(--dur-btn, 220ms) ease;
  will-change: transform;
}

.back-to-top-btn:hover {
  background: #166534;
  transform: translateY(-4px) scale(1.08);
  box-shadow: 0 12px 28px -4px rgba(20, 83, 45, 0.5);
}

.back-to-top-btn:active {
  transform: translateY(0) scale(0.95);
}

.arrow-icon {
  font-size: 16px;
  transition: transform 0.2s ease;
}

.back-to-top-btn:hover .arrow-icon {
  transform: translateY(-2px);
}

/* Fade Slide Transition */
.fade-slide-enter-active,
.fade-slide-leave-active {
  transition: opacity 0.3s cubic-bezier(0.16, 1, 0.3, 1),
              transform 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}

.fade-slide-enter-from,
.fade-slide-leave-to {
  opacity: 0;
  transform: translateY(16px) scale(0.85);
}
</style>
