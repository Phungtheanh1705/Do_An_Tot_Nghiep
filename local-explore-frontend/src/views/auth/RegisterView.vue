<template>
  <div class="auth-page">
    <div class="auth-card glass-panel">
      <div class="auth-header">
        <router-link to="/" class="logo">
          <img src="/logo.png" alt="LocalExplore" class="auth-logo-img" />
        </router-link>
        <h2>Tạo tài khoản mới</h2>
        <p>Gia nhập cộng đồng du lịch trải nghiệm bản địa</p>
      </div>

      <a-form layout="vertical" @submit.prevent="handleRegister">
        <a-form-item label="Họ và tên của bạn" required>
          <a-input
            v-model:value="form.fullName"
            placeholder="Nguyễn Văn A"
            size="large"
          >
            <template #prefix><UserOutlined /></template>
          </a-input>
        </a-form-item>

        <a-form-item label="Địa chỉ Email" required>
          <a-input
            v-model:value="form.email"
            placeholder="email@example.com"
            size="large"
          >
            <template #prefix><MailOutlined /></template>
          </a-input>
        </a-form-item>

        <a-form-item label="Số điện thoại" required>
          <a-input
            v-model:value="form.phoneNumber"
            placeholder="0912345678"
            size="large"
          >
            <template #prefix><PhoneOutlined /></template>
          </a-input>
        </a-form-item>

        <a-form-item label="Mật khẩu" required>
          <a-input-password
            v-model:value="form.password"
            placeholder="Tối thiểu 6 ký tự"
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
          class="register-btn"
        >
          Đăng ký tài khoản
        </a-button>

        <div class="auth-footer">
          <span>Đã có tài khoản? </span>
          <router-link to="/login" class="link-bold">Đăng nhập</router-link>
        </div>
      </a-form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { message } from 'ant-design-vue'
import { UserOutlined, MailOutlined, PhoneOutlined, LockOutlined } from '@ant-design/icons-vue'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const loading = ref(false)
const form = reactive({
  fullName: '',
  email: '',
  phoneNumber: '',
  password: ''
})

async function handleRegister() {
  if (!form.fullName || !form.email || !form.password) {
    message.warning('Vui lòng điền đầy đủ các thông tin bắt buộc.')
    return
  }

  loading.value = true
  try {
    const res = await authStore.register({
      fullName: form.fullName,
      email: form.email,
      phoneNumber: form.phoneNumber,
      password: form.password
    })

    if (res.success) {
      message.success('Đăng ký tài khoản thành công!')
      router.push('/')
    } else {
      message.error(res.message || 'Đăng ký thất bại')
    }
  } catch (error: any) {
    message.error(error.response?.data?.message || 'Có lỗi xảy ra khi đăng ký')
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

.register-btn {
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
