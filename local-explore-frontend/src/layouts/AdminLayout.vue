<template>
  <div class="admin-wrapper">
    <!-- Sidebar -->
    <aside class="sidebar">
      <div class="sidebar-header">
        <router-link to="/" class="sidebar-brand-link">
          <img src="/logo-white.png" alt="LocalExplore" class="sidebar-logo-img" />
        </router-link>
        <div class="header-text">
          <span class="title">Admin Portal</span>
          <span class="subtitle">Quản trị toàn hệ thống</span>
        </div>
      </div>

      <nav class="sidebar-menu">
        <router-link to="/admin" class="menu-item" exact-active-class="active">
          <DashboardOutlined class="icon" />
          <span>Dashboard Thống kê</span>
        </router-link>

        <router-link to="/admin/tours/approval" class="menu-item" active-class="active">
          <CheckSquareOutlined class="icon" />
          <span>Kiểm duyệt Tour</span>
        </router-link>

        <router-link to="/admin/hosts" class="menu-item" active-class="active">
          <SolutionOutlined class="icon" />
          <span>Duyệt hồ sơ Host (KYC)</span>
        </router-link>

        <router-link to="/admin/users" class="menu-item" active-class="active">
          <TeamOutlined class="icon" />
          <span>Quản lý Người dùng</span>
        </router-link>
      </nav>

      <div class="sidebar-footer">
        <router-link to="/" class="back-link">
          <ArrowLeftOutlined /> Về trang khách hàng
        </router-link>
      </div>
    </aside>

    <!-- Main Content Area -->
    <div class="main-area">
      <!-- Top Header -->
      <header class="topbar">
        <div class="topbar-left">
          <h2 class="page-title">{{ $route.meta.title || 'Quản trị Hệ thống' }}</h2>
        </div>

        <div class="topbar-right">
          <div class="user-info">
            <a-avatar :src="authStore.user?.avatarUrl" :size="36" style="background-color: #6D28D9;">
              AD
            </a-avatar>
            <div class="user-meta">
              <span class="name">{{ authStore.user?.fullName }}</span>
              <span class="role">Super Administrator</span>
            </div>
          </div>

          <a-button type="text" danger @click="handleLogout">
            <LogoutOutlined /> Đăng xuất
          </a-button>
        </div>
      </header>

      <!-- View Content -->
      <main class="page-content">
        <router-view />
      </main>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { message } from 'ant-design-vue'
import {
  DashboardOutlined,
  CheckSquareOutlined,
  SolutionOutlined,
  TeamOutlined,
  ArrowLeftOutlined,
  LogoutOutlined
} from '@ant-design/icons-vue'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const authStore = useAuthStore()

function handleLogout() {
  authStore.logout()
  message.success('Đã đăng xuất')
  router.push('/login')
}
</script>

<style scoped>
.admin-wrapper {
  display: flex;
  min-height: 100vh;
  background: #F8FAFC;
}

.sidebar {
  width: 260px;
  background: #0F172A;
  color: #fff;
  display: flex;
  flex-direction: column;
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  z-index: 50;
}

.sidebar-header {
  padding: 24px 20px;
  display: flex;
  align-items: center;
  gap: 12px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
}

.sidebar-logo-img {
  height: 38px;
  background: #ffffff;
  padding: 3px 6px;
  border-radius: 8px;
  object-fit: contain;
}

.sidebar-header .title {
  display: block;
  font-size: 18px;
  font-weight: 800;
  color: #fff;
}

.sidebar-header .subtitle {
  font-size: 11px;
  color: #94A3B8;
}

.sidebar-menu {
  flex: 1;
  padding: 20px 12px;
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.menu-item {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 12px 16px;
  color: #94A3B8;
  font-size: 14px;
  font-weight: 500;
  border-radius: 8px;
  transition: all 0.2s;
}

.menu-item .icon {
  font-size: 18px;
}

.menu-item:hover {
  background: rgba(255, 255, 255, 0.08);
  color: #fff;
}

.menu-item.active {
  background: #3B82F6;
  color: #fff;
  font-weight: 700;
  box-shadow: 0 4px 12px rgba(59, 130, 246, 0.3);
}

.sidebar-footer {
  padding: 20px;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
}

.back-link {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 13px;
  color: #94A3B8;
  font-weight: 600;
}

.back-link:hover {
  color: #fff;
}

.main-area {
  flex: 1;
  margin-left: 260px;
  display: flex;
  flex-direction: column;
}

.topbar {
  height: 70px;
  background: #fff;
  border-bottom: 1px solid var(--border-color);
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 32px;
  position: sticky;
  top: 0;
  z-index: 40;
}

.page-title {
  font-size: 20px;
  font-weight: 700;
  color: var(--text-primary);
}

.topbar-right {
  display: flex;
  align-items: center;
  gap: 20px;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 10px;
}

.user-meta {
  display: flex;
  flex-direction: column;
}

.user-meta .name {
  font-size: 14px;
  font-weight: 600;
  color: var(--text-primary);
}

.user-meta .role {
  font-size: 11px;
  color: var(--text-muted);
}

.page-content {
  padding: 32px;
  flex: 1;
}
</style>
