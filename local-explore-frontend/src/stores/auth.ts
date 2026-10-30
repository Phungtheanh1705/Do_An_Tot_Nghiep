import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import apiClient from '@/services/api'
import type { ApiResponse, AuthResponse, UserProfile } from '@/types'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('localexplore_token'))
  const user = ref<UserProfile | null>(
    localStorage.getItem('localexplore_user')
      ? JSON.parse(localStorage.getItem('localexplore_user')!)
      : null
  )

  const isAuthenticated = computed(() => !!token.value && !!user.value)
  const isHost = computed(() => user.value?.roles.includes('Host') ?? false)
  const isAdmin = computed(() => user.value?.roles.includes('Admin') ?? false)

  function setAuth(data: AuthResponse) {
    token.value = data.accessToken
    user.value = data.user
    localStorage.setItem('localexplore_token', data.accessToken)
    localStorage.setItem('localexplore_refresh_token', data.refreshToken)
    localStorage.setItem('localexplore_user', JSON.stringify(data.user))
  }

  async function login(email: string, password: string):Promise<ApiResponse<AuthResponse>> {
    const response = await apiClient.post<ApiResponse<AuthResponse>>('/auth/login', { email, password })
    if (response.data.success && response.data.data) {
      setAuth(response.data.data)
    }
    return response.data
  }

  async function register(payload: { email: string; password: string; fullName: string; phoneNumber?: string }): Promise<ApiResponse<AuthResponse>> {
    const response = await apiClient.post<ApiResponse<AuthResponse>>('/auth/register', payload)
    if (response.data.success && response.data.data) {
      setAuth(response.data.data)
    }
    return response.data
  }

  function logout() {
    token.value = null
    user.value = null
    localStorage.removeItem('localexplore_token')
    localStorage.removeItem('localexplore_refresh_token')
    localStorage.removeItem('localexplore_user')
  }

  async function fetchCurrentUser() {
    if (!token.value) return
    try {
      const response = await apiClient.get<ApiResponse<UserProfile>>('/auth/me')
      if (response.data.success && response.data.data) {
        user.value = response.data.data
        localStorage.setItem('localexplore_user', JSON.stringify(response.data.data))
      }
    } catch {
      logout()
    }
  }

  return {
    token,
    user,
    isAuthenticated,
    isHost,
    isAdmin,
    login,
    register,
    logout,
    fetchCurrentUser
  }
})
