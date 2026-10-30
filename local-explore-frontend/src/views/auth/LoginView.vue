<template>
  <div class="auth-page">
    <div class="auth-card glass-panel">
      <div class="auth-header">
        <router-link to="/" class="logo">
          <img src="/logo.png" alt="LocalExplore" class="auth-logo-img" />
        </router-link>
        <h2>Đăng nhập hệ thống</h2>
        <p>Truy cập tài khoản để khám phá hoặc quản lý trải nghiệm</p>
      </div>

      <!-- Quick Demo Account Switcher (Rất tiện lợi khi bảo vệ đồ án) -->
      <div class="demo-box">
        <div class="demo-label">⚡ Chọn nhanh tài khoản Demo:</div>
        <div class="demo-buttons">
          <button type="button" class="btn-demo" @click="fillDemo('admin@localexplore.vn')">
            👑 Admin
          </button>
          <button type="button" class="btn-demo" @click="fillDemo('chinh.hmong@localexplore.vn')">
            🧑‍🌾 Host Sa Pa
          </button>
          <button type="button" class="btn-demo" @click="fillDemo('mai.battrang@localexplore.vn')">
            🏺 Host Bát Tràng
          </button>
          <button type="button" class="btn-demo" @click="fillDemo('an.hoang@gmail.com')">
            🎒 Khách hàng An
          </button>
        </div>
      </div>

      <a-form layout="vertical" @submit.prevent="handleLogin">
        <a-form-item label="Địa chỉ Email" required>
          <a-input
            v-model:value="form.email"
            placeholder="nhap.email@example.com"
            size="large"
          >
            <template #prefix><MailOutlined /></template>
          </a-input>
        </a-form-item>

        <a-form-item label="Mật khẩu" required>
          <a-input-password
            v-model:value="form.password"
            placeholder="••••••••"
            size="large"
          >
            <template #prefix><LockOutlined /></template>
          </a-input-password>
        </a-form-item>

        <a-button
          type="primary"
          size="large"
          block
          :loading="loading"
          html-type="submit"
          class="login-btn"
        >
          Đăng nhập ngay
        </a-button>

        <div class="auth-footer">
          <span>Chưa có tài khoản? </span>
          <router-link to="/register" class="link-bold">Đăng ký tại đây</router-link>
        </div>
      </a-form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { message } from 'ant-design-vue'
import { MailOutlined, LockOutlined } from '@ant-design/icons-vue'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

const loading = ref(false)
const form = reactive({
  email: '',
  password: 'Password@123'
})

function fillDemo(email: string) {
  form.email = email
  form.password = 'Password@123'
}

async function handleLogin() {
  if (!form.email || !form.password) {
    message.warning('Vui lòng nhập email và mật khẩu')
    return
  }

  loading.value = true
  try {
    const res = await authStore.login(form.email, form.password)
    if (res.success) {
      message.success(res.message || 'Đăng nhập thành công')

      const redirect = (route.query.redirect as string) || (
        authStore.isAdmin ? '/admin' :
        authStore.isHost ? '/host' : '/'
      )
      router.push(redirect)
    } else {
      message.error(res.message || 'Đăng nhập thất bại')
    }
  } catch (error: any) {
    message.error(error.response?.data?.message || 'Lỗi kết nối máy chủ')
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.auth-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #E8F5E9 0%, #C8E6C9 100%);
  padding: 20px;
}

.auth-card {
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-lg);
  padding: 40px;
  max-width: 460px;
  width: 100%;
  box-shadow: var(--shadow-lg);
}

.auth-header {
  text-align: center;
  margin-bottom: 24px;
}

.logo {
  display: inline-block;
  margin-bottom: 12px;
}

.auth-logo-img {
  height: 52px;
  object-fit: contain;
}

.auth-header h2 {
  font-size: 24px;
  font-weight: 800;
  color: var(--text-primary);
  margin-bottom: 4px;
}

.auth-header p {
  font-size: 13px;
  color: var(--text-secondary);
}

.demo-box {
  background: #F8FAFC;
  border: 1px dashed #CBD5E1;
  border-radius: var(--radius-sm);
  padding: 12px;
  margin-bottom: 20px;
}

.demo-label {
  font-size: 11px;
  font-weight: 700;
  color: var(--text-secondary);
  margin-bottom: 8px;
}

.demo-buttons {
  display: flex;
  flex-wrap: wrap;
  gap: 6px;
}

.btn-demo {
  background: #fff;
  border: 1px solid #E2E8F0;
  border-radius: 4px;
  font-size: 11px;
  font-weight: 600;
  padding: 4px 8px;
  cursor: pointer;
  color: var(--text-primary);
  transition: all 0.2s;
}

.btn-demo:hover {
  border-color: var(--primary-color);
  color: var(--primary-color);
  background: var(--primary-light);
}

.login-btn {
  height: 46px;
  border-radius: 8px;
  font-weight: 700;
  font-size: 15px;
  margin-top: 10px;
}

.auth-footer {
  text-align: center;
  font-size: 13px;
  color: var(--text-secondary);
  margin-top: 20px;
}

.link-bold {
  color: var(--primary-color);
  font-weight: 700;
}
</style>
