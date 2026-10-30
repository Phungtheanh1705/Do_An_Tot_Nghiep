<template>
  <div :class="['tour-loading-container', { 'is-overlay': overlay, 'is-fullscreen': fullscreen }]">
    <div class="loading-animation-box" :style="{ minHeight: minHeight }">
      <!-- Glow Ripple Effect -->
      <div class="ripple-ring ripple-1"></div>
      <div class="ripple-ring ripple-2"></div>

      <!-- Main Spinner with LocalExplore Emblem -->
      <div class="spinner-core" :class="`size-${size}`">
        <!-- SVG Orbiting Gradient Ring -->
        <svg class="orbit-svg" viewBox="0 0 120 120" fill="none" xmlns="http://www.w3.org/2000/svg">
          <!-- Background Track -->
          <circle
            cx="60"
            cy="60"
            r="50"
            stroke="#E2E8F0"
            stroke-width="3.5"
            stroke-dasharray="4 4"
            opacity="0.6"
          />

          <!-- Active Spinning Gradient Ring -->
          <circle
            class="active-orbit-ring"
            cx="60"
            cy="60"
            r="50"
            stroke="url(#localExploreGrad)"
            stroke-width="4.5"
            stroke-linecap="round"
            stroke-dasharray="160 160"
          />

          <!-- Defs with LocalExplore Brand Gradients -->
          <defs>
            <linearGradient id="localExploreGrad" x1="0%" y1="0%" x2="100%" y2="100%">
              <stop offset="0%" stop-color="#2D6A4F" stop-opacity="0.1" />
              <stop offset="45%" stop-color="#52B788" stop-opacity="0.8" />
              <stop offset="85%" stop-color="#1B4332" stop-opacity="1" />
              <stop offset="100%" stop-color="#D97706" stop-opacity="1" />
            </linearGradient>
          </defs>
        </svg>

        <!-- Orbiting Satellite Glow Dot -->
        <div class="satellite-orbit">
          <span class="satellite-dot"></span>
        </div>

        <!-- Center LocalExplore Brand Emblem -->
        <div class="center-emblem">
          <img src="/favicon.png" alt="LocalExplore" class="emblem-image" />
        </div>
      </div>

      <!-- Loading Text & Subtitle -->
      <div class="loading-meta">
        <p class="loading-title">
          <span>{{ tip }}</span>
          <span class="dots-wave">
            <span class="dot d1">.</span>
            <span class="dot d2">.</span>
            <span class="dot d3">.</span>
          </span>
        </p>
        <p v-if="subtip" class="loading-subtitle">{{ subtip }}</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
withDefaults(
  defineProps<{
    tip?: string
    subtip?: string
    size?: 'small' | 'default' | 'large'
    overlay?: boolean
    fullscreen?: boolean
    minHeight?: string
  }>(),
  {
    tip: 'Đang tải trải nghiệm bản địa',
    subtip: 'LocalExplore • Trải nghiệm chân thực cùng người địa phương',
    size: 'default',
    overlay: false,
    fullscreen: false,
    minHeight: '260px'
  }
)
</script>

<style scoped>
.tour-loading-container {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 100%;
  padding: 30px 20px;
}

.tour-loading-container.is-overlay {
  position: absolute;
  inset: 0;
  background: rgba(255, 255, 255, 0.88);
  backdrop-filter: blur(4px);
  z-index: 50;
  border-radius: inherit;
}

.tour-loading-container.is-fullscreen {
  position: fixed;
  inset: 0;
  background: rgba(255, 255, 255, 0.94);
  backdrop-filter: blur(8px);
  z-index: 9999;
}

.loading-animation-box {
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  text-align: center;
}

/* RIPPLE HALO EFFECTS */
.ripple-ring {
  position: absolute;
  top: 40%;
  left: 50%;
  transform: translate(-50%, -50%);
  border-radius: 50%;
  border: 1px solid rgba(45, 106, 79, 0.18);
  pointer-events: none;
}

.ripple-1 {
  width: 120px;
  height: 120px;
  animation: ripplePulse 2.8s ease-out infinite;
}

.ripple-2 {
  width: 150px;
  height: 150px;
  animation: ripplePulse 2.8s ease-out 1.4s infinite;
}

@keyframes ripplePulse {
  0% {
    transform: translate(-50%, -50%) scale(0.8);
    opacity: 0.8;
  }
  50% {
    opacity: 0.4;
  }
  100% {
    transform: translate(-50%, -50%) scale(1.4);
    opacity: 0;
  }
}

/* SPINNER CORE */
.spinner-core {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
}

.spinner-core.size-small {
  width: 76px;
  height: 76px;
}
.spinner-core.size-default {
  width: 108px;
  height: 108px;
}
.spinner-core.size-large {
  width: 136px;
  height: 136px;
}

/* SVG ORBIT */
.orbit-svg {
  position: absolute;
  inset: 0;
  width: 100%;
  height: 100%;
  animation: rotateOrbit 1.6s cubic-bezier(0.4, 0, 0.2, 1) infinite;
  transform-origin: center center;
  filter: drop-shadow(0 2px 8px rgba(45, 106, 79, 0.15));
}

.active-orbit-ring {
  stroke-dashoffset: 0;
  animation: dashAnimation 1.6s ease-in-out infinite alternate;
}

@keyframes rotateOrbit {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

@keyframes dashAnimation {
  0% {
    stroke-dashoffset: 40;
  }
  100% {
    stroke-dashoffset: 180;
  }
}

/* ORBITING SATELLITE GLOW DOT */
.satellite-orbit {
  position: absolute;
  inset: 0;
  width: 100%;
  height: 100%;
  animation: rotateOrbit 2.4s linear infinite;
  pointer-events: none;
}

.satellite-dot {
  position: absolute;
  top: 1px;
  left: 50%;
  transform: translateX(-50%);
  width: 8px;
  height: 8px;
  background: #d97706;
  border-radius: 50%;
  box-shadow: 0 0 10px #d97706, 0 0 18px rgba(217, 119, 6, 0.6);
}

/* CENTER LOCALEXPLORE EMBLEM */
.center-emblem {
  position: relative;
  z-index: 2;
  border-radius: 50%;
  background: #ffffff;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0 4px 16px rgba(27, 67, 50, 0.12);
  animation: emblemBreathe 2.4s ease-in-out infinite alternate;
}

.size-small .center-emblem {
  width: 44px;
  height: 44px;
}
.size-default .center-emblem {
  width: 62px;
  height: 62px;
}
.size-large .center-emblem {
  width: 78px;
  height: 78px;
}

.emblem-image {
  width: 72%;
  height: 72%;
  object-fit: contain;
  filter: drop-shadow(0 2px 6px rgba(21, 128, 61, 0.22));
}

@keyframes emblemBreathe {
  0% {
    transform: scale(0.94);
    box-shadow: 0 4px 14px rgba(27, 67, 50, 0.1);
  }
  100% {
    transform: scale(1.05);
    box-shadow: 0 8px 24px rgba(45, 106, 79, 0.22);
  }
}

/* LOADING TEXT & SUBTITLE */
.loading-meta {
  margin-top: 22px;
}

.loading-title {
  font-size: 15px;
  font-weight: 700;
  color: #1e293b;
  margin: 0;
  letter-spacing: -0.01em;
}

.dots-wave .dot {
  display: inline-block;
  font-weight: 800;
  color: #15803d;
  animation: dotBounce 1.4s infinite ease-in-out both;
}

.dots-wave .d1 {
  animation-delay: -0.32s;
}
.dots-wave .d2 {
  animation-delay: -0.16s;
}
.dots-wave .d3 {
  animation-delay: 0s;
}

@keyframes dotBounce {
  0%, 80%, 100% {
    transform: scale(0.6);
    opacity: 0.3;
  }
  40% {
    transform: scale(1.3);
    opacity: 1;
  }
}

.loading-subtitle {
  font-size: 12.5px;
  color: #64748b;
  margin: 4px 0 0;
  font-weight: 500;
  letter-spacing: 0.02em;
}
</style>
