<template>
  <a-config-provider :theme="themeConfig">
    <router-view />
  </a-config-provider>
</template>

<script setup lang="ts">
import { reactive, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'

const authStore = useAuthStore()

const themeConfig = reactive({
  token: {
    colorPrimary: '#1B4332',
    colorLink: '#1B4332',
    colorSuccess: '#16A34A',
    colorWarning: '#D97706',
    colorError: '#DC2626',
    borderRadius: 8,
    fontFamily: "'Plus Jakarta Sans', -apple-system, BlinkMacSystemFont, sans-serif"
  }
})

onMounted(() => {
  if (authStore.token && !authStore.user) {
    authStore.fetchCurrentUser()
  }
})
</script>
