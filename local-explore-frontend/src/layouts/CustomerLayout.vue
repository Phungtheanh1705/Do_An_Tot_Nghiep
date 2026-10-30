<template>
  <div class="layout-wrapper">
    <!-- Navbar with Glassmorphism on Scroll -->
    <header class="navbar" :class="{ 'is-scrolled': isScrolled }">
      <div class="container nav-container">
        <!-- Logo -->
        <router-link to="/" class="logo">
          <img src="/logo.png" alt="LocalExplore" class="brand-logo-img" />
        </router-link>

        <!-- Navigation Links -->
        <nav class="nav-links">
          <router-link to="/" class="nav-item" exact-active-class="active">Trang chủ</router-link>
          <router-link to="/tours" class="nav-item" active-class="active">Khám phá</router-link>
          <router-link to="/categories" class="nav-item" active-class="active">Danh mục</router-link>
          <router-link to="/about" class="nav-item" active-class="active">Về chúng tôi</router-link>
          <router-link v-if="!authStore.isHost" to="/become-host" class="nav-item" active-class="active">Trở thành Host</router-link>
          <router-link v-else to="/host" class="nav-item">Host Portal</router-link>
        </nav>

        <!-- Action Icons & Auth Buttons -->
        <div class="nav-actions">
          <a-tooltip title="Tìm kiếm tour" placement="bottom">
            <button class="icon-btn" @click="$router.push('/tours')">
              <SearchOutlined />
            </button>
          </a-tooltip>
          
          <a-tooltip title="Tour yêu thích" placement="bottom">
            <button class="icon-btn" @click="$router.push('/tours')">
              <HeartOutlined />
            </button>
          </a-tooltip>

          <a-tooltip title="Vé & Chuyến đi của tôi" placement="bottom">
            <button class="icon-btn cart-btn" @click="$router.push('/my-bookings')">
              <CalendarOutlined />
              <span class="cart-badge">{{ authStore.isAuthenticated ? '1' : '0' }}</span>
            </button>
          </a-tooltip>

          <!-- User Logged In -->
          <template v-if="authStore.isAuthenticated && authStore.user">
            <router-link v-if="authStore.isHost" to="/host" class="portal-badge host-badge">
              Host Portal
            </router-link>
            <router-link v-if="authStore.isAdmin" to="/admin" class="portal-badge admin-badge">
              Admin Portal
            </router-link>

            <a-dropdown :trigger="['click']">
              <div class="user-pill">
                <a-avatar :src="authStore.user.avatarUrl" :size="34">
                  {{ authStore.user.fullName?.charAt(0) }}
                </a-avatar>
                <span class="user-name">{{ authStore.user.fullName }}</span>
                <DownOutlined class="dropdown-icon" />
              </div>
              <template #overlay>
                <a-menu>
                  <a-menu-item key="bookings" @click="$router.push('/my-bookings')">
                    <CalendarOutlined /> Vé & Chuyến đi của tôi
                  </a-menu-item>
                  <a-menu-item v-if="authStore.isHost" key="host-dash" @click="$router.push('/host')">
                    <DashboardOutlined /> Quản lý Tour & Quét QR
                  </a-menu-item>
                  <a-menu-item v-if="authStore.isAdmin" key="admin-dash" @click="$router.push('/admin')">
                    <CrownOutlined /> Trang quản trị Admin
                  </a-menu-item>
                  <a-menu-divider />
                  <a-menu-item key="logout" danger @click="handleLogout">
                    <LogoutOutlined /> Đăng xuất
                  </a-menu-item>
                </a-menu>
              </template>
            </a-dropdown>
          </template>

          <!-- Guest State -->
          <template v-else>
            <router-link to="/login" class="login-btn">Đăng nhập</router-link>
            <router-link to="/register" class="register-pill-btn">Đăng ký</router-link>
          </template>
        </div>
      </div>
    </header>

    <!-- Main Content -->
    <main class="main-content">
      <router-view />
    </main>

    <!-- Footer Matching Mockup -->
    <footer class="footer">
      <div class="container footer-container">
        <!-- Col 1: Brand -->
        <div class="footer-col brand-col">
          <div class="logo-footer">
            <img src="/logo-white.png" alt="LocalExplore" class="footer-logo-img" />
          </div>
          <p class="brand-slogan">Trải nghiệm bản địa • Kết nối tình yêu</p>
          <p class="footer-desc">
            Nền tảng du lịch kết nối du khách với những trải nghiệm chân thực, mang đậm bản sắc văn hóa do chính người dân địa phương dẫn dắt khắp Việt Nam.
          </p>
        </div>

        <!-- Col 2: Liên kết nhanh -->
        <div class="footer-col">
          <h4>Liên kết nhanh</h4>
          <ul>
            <li><router-link to="/">Trang chủ</router-link></li>
            <li><router-link to="/tours">Khám phá tour</router-link></li>
            <li><router-link to="/categories">Danh mục trải nghiệm</router-link></li>
            <li><router-link to="/about">Về chúng tôi</router-link></li>
            <li><router-link to="/become-host">Trở thành Host bản địa</router-link></li>
          </ul>
        </div>

        <!-- Col 3: Hỗ trợ -->
        <div class="footer-col">
          <h4>Hỗ trợ & Pháp lý</h4>
          <ul>
            <li><router-link to="/contact">Trung tâm trợ giúp (FAQ)</router-link></li>
            <li><router-link to="/about">Chính sách bảo tồn văn hóa</router-link></li>
            <li><router-link to="/contact">Quy định & Điều khoản</router-link></li>
            <li><router-link to="/contact">Liên hệ hỗ trợ 24/7</router-link></li>
          </ul>
        </div>

        <!-- Col 4: Theo dõi & Nhận tin -->
        <div class="footer-col newsletter-col">
          <h4>Theo dõi chúng tôi</h4>
          <div class="social-icons">
            <a href="#" class="social-circle" title="Facebook"><FacebookFilled /></a>
            <a href="#" class="social-circle" title="Instagram"><InstagramFilled /></a>
            <a href="#" class="social-circle" title="Youtube"><YoutubeFilled /></a>
            <a href="#" class="social-circle" title="TikTok"><TwitterOutlined /></a>
          </div>

          <h4 style="margin-top: 20px;">Đăng ký nhận bản tin</h4>
          <div class="newsletter-input-box">
            <input v-model="newsletterEmail" type="email" placeholder="Nhập email của bạn" />
            <button class="send-btn" @click="handleSubscribe"><ArrowRightOutlined /></button>
          </div>
        </div>
      </div>

      <!-- Footer Bottom Bar -->
      <div class="footer-bottom">
        <div class="container bottom-content">
          <p>© 2026 LocalExplore. Tất cả quyền được bảo lưu.</p>
          <div class="lang-selector">
            <span>Tiếng Việt</span>
            <span class="divider">|</span>
            <span>English</span>
            <span class="globe-icon">🌐</span>
          </div>
        </div>
      </div>
    </footer>

    <!-- Back To Top Button -->
    <BackToTop />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'
import { message } from 'ant-design-vue'
import {
  SearchOutlined,
  HeartOutlined,
  DownOutlined,
  CalendarOutlined,
  DashboardOutlined,
  CrownOutlined,
  LogoutOutlined,
  FacebookFilled,
  InstagramFilled,
  YoutubeFilled,
  TwitterOutlined,
  ArrowRightOutlined
} from '@ant-design/icons-vue'
import { useAuthStore } from '@/stores/auth'
import BackToTop from '@/components/common/BackToTop.vue'

const router = useRouter()
const authStore = useAuthStore()
const newsletterEmail = ref('')
const isScrolled = ref(false)

function onScroll() {
  isScrolled.value = window.scrollY > 20
}

onMounted(() => {
  window.addEventListener('scroll', onScroll, { passive: true })
  onScroll()
})

onUnmounted(() => {
  window.removeEventListener('scroll', onScroll)
})

function handleLogout() {
  authStore.logout()
  message.success('Đã đăng xuất thành công')
  router.push('/')
}

function scrollToCategories() {
  const el = document.getElementById('categories')
  if (el) {
    el.scrollIntoView({ behavior: 'smooth' })
  } else {
    router.push('/#categories')
  }
}

function scrollToAbout() {
  const el = document.getElementById('about')
  if (el) {
    el.scrollIntoView({ behavior: 'smooth' })
  } else {
    router.push('/#about')
  }
}

function handleSubscribe() {
  if (!newsletterEmail.value.includes('@')) {
    message.warning('Vui lòng nhập địa chỉ email hợp lệ.')
    return
  }
  message.success('Đăng ký nhận bản tin thành công!')
  newsletterEmail.value = ''
}
</script>

<style scoped>
.layout-wrapper {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  background-color: #fcfcfc;
}

.navbar {
  position: sticky;
  top: 0;
  z-index: 100;
  background: rgba(255, 255, 255, 0.98);
  border-bottom: 1px solid #edf2f7;
  padding: 10px 0;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.04);
  transition: background var(--dur-card, 0.25s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              backdrop-filter var(--dur-card, 0.25s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              box-shadow var(--dur-card, 0.25s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              border-color var(--dur-card, 0.25s) ease,
              padding var(--dur-card, 0.25s) ease;
}

.navbar.is-scrolled {
  background: rgba(255, 255, 255, 0.88);
  backdrop-filter: blur(14px);
  -webkit-backdrop-filter: blur(14px);
  box-shadow: 0 4px 20px -2px rgba(0, 0, 0, 0.08);
  border-bottom-color: rgba(226, 232, 240, 0.85);
  padding: 8px 0;
}

.nav-container {
  display: flex;
  align-items: center;
  justify-content: space-between;
  max-width: 1240px;
  margin: 0 auto;
  padding: 0 20px;
}

.brand-logo-img {
  height: 42px;
  object-fit: contain;
  display: block;
  transition: transform var(--dur-btn, 0.2s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.logo:hover .brand-logo-img {
  transform: scale(1.03);
}

.nav-links {
  display: flex;
  align-items: center;
  gap: 28px;
}

.nav-item {
  position: relative;
  font-size: 14px;
  font-weight: 500;
  color: #334155;
  text-decoration: none;
  transition: color var(--dur-btn, 0.2s) ease;
  padding: 6px 16px;
  border-radius: 999px;
}

.nav-item::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 50%;
  width: 0;
  height: 2px;
  background: #15803d;
  border-radius: 2px;
  transform: translateX(-50%);
  transition: width var(--dur-btn, 0.2s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.nav-item:hover {
  color: #15803d;
}

.nav-item:hover::after {
  width: calc(100% - 24px);
}

.nav-item.active {
  color: #15803d;
  border: 1px solid #86efac;
  background: #f0fdf4;
  font-weight: 600;
}

.nav-item.active::after {
  display: none;
}

.nav-actions {
  display: flex;
  align-items: center;
  gap: 14px;
}

.icon-btn {
  background: none;
  border: none;
  font-size: 18px;
  color: #475569;
  cursor: pointer;
  padding: 7px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: transform var(--dur-micro, 0.18s) var(--ease-spring, cubic-bezier(0.34, 1.56, 0.64, 1)),
              color var(--dur-micro, 0.18s) ease,
              background-color var(--dur-micro, 0.18s) ease;
}

.icon-btn:hover {
  background: #f0fdf4;
  color: #15803d;
  transform: scale(1.12);
}

.icon-btn:active {
  transform: scale(0.94);
}

.cart-btn {
  position: relative;
}

.cart-badge {
  position: absolute;
  top: 0;
  right: -2px;
  background: #ef4444;
  color: #fff;
  font-size: 10px;
  font-weight: 700;
  width: 16px;
  height: 16px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: transform var(--dur-micro, 0.18s) var(--ease-spring, cubic-bezier(0.34, 1.56, 0.64, 1));
}

.cart-btn:hover .cart-badge {
  transform: scale(1.2);
}

.login-btn {
  font-size: 13px;
  font-weight: 600;
  color: #334155;
  text-decoration: none;
  padding: 6px 18px;
  border: 1px solid #cbd5e1;
  border-radius: 999px;
  transition: all var(--dur-btn, 0.2s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.login-btn:hover {
  border-color: #15803d;
  color: #15803d;
  transform: translateY(-1px);
  box-shadow: 0 2px 8px rgba(21, 128, 61, 0.12);
}

.login-btn:active {
  transform: scale(0.97);
}

.register-pill-btn {
  font-size: 13px;
  font-weight: 700;
  color: #ffffff !important;
  background: #14532d;
  text-decoration: none;
  padding: 7px 20px;
  border-radius: 999px;
  transition: all var(--dur-btn, 0.2s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.register-pill-btn:hover {
  background: #166534;
  transform: translateY(-1px);
  box-shadow: 0 4px 14px rgba(20, 83, 45, 0.28);
}

.register-pill-btn:active {
  transform: scale(0.97);
}

.user-pill {
  display: flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
  padding: 4px 10px 4px 4px;
  border: 1px solid #e2e8f0;
  border-radius: 24px;
  background: #fff;
  transition: all var(--dur-btn, 0.2s) ease;
}

.user-pill:hover {
  border-color: #15803d;
  box-shadow: 0 2px 8px rgba(21, 128, 61, 0.12);
  transform: translateY(-1px);
}

.user-name {
  font-size: 13px;
  font-weight: 600;
  color: #1e293b;
}

.dropdown-icon {
  font-size: 10px;
  color: #94a3b8;
  transition: transform var(--dur-micro, 0.18s) ease;
}

.user-pill:hover .dropdown-icon {
  transform: translateY(1px);
  color: #15803d;
}

.portal-badge {
  font-size: 11px;
  font-weight: 700;
  padding: 4px 10px;
  border-radius: 12px;
  text-decoration: none;
  transition: transform var(--dur-micro, 0.18s) ease;
}

.portal-badge:hover {
  transform: scale(1.05);
}

.host-badge {
  background: #fef3c7;
  color: #b45309;
}

.admin-badge {
  background: #ede9fe;
  color: #6d28d9;
}

.main-content {
  flex: 1;
}

/* Footer Styles */
.footer {
  background: #0d2618;
  color: #e2e8f0;
  margin-top: 60px;
  padding-top: 60px;
}

.footer-container {
  display: grid;
  grid-template-columns: 2.2fr 1fr 1fr 1.6fr;
  gap: 48px;
  max-width: 1240px;
  margin: 0 auto;
  padding: 0 20px 48px;
}

.footer-logo-img {
  height: 46px;
  object-fit: contain;
  margin-bottom: 12px;
  transition: transform var(--dur-card, 0.25s) ease;
}

.footer-logo-img:hover {
  transform: scale(1.03);
}

.brand-slogan {
  font-size: 13px;
  color: #a7f3d0;
  font-weight: 600;
  margin-bottom: 12px;
}

.footer-desc {
  font-size: 13px;
  line-height: 1.6;
  color: #94a3b8;
  max-width: 380px;
}

.footer-col h4 {
  color: #ffffff;
  font-size: 15px;
  font-weight: 700;
  margin-bottom: 18px;
}

.footer-col ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.footer-col li {
  margin-bottom: 10px;
}

.footer-col a {
  color: #94a3b8;
  font-size: 13px;
  text-decoration: none;
  display: inline-block;
  transition: color var(--dur-btn, 0.2s) ease,
              transform var(--dur-btn, 0.2s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.footer-col a:hover {
  color: #34d399;
  transform: translateX(4px);
}

.social-icons {
  display: flex;
  gap: 10px;
}

.social-circle {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.1);
  display: flex;
  align-items: center;
  justify-content: center;
  color: #e2e8f0;
  font-size: 15px;
  transition: transform var(--dur-btn, 0.2s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              background-color var(--dur-btn, 0.2s) ease,
              box-shadow var(--dur-btn, 0.2s) ease,
              color var(--dur-btn, 0.2s) ease;
}

.social-circle:hover {
  background: #10b981;
  color: #ffffff;
  transform: translateY(-3px) rotate(8deg);
  box-shadow: 0 6px 14px rgba(16, 185, 129, 0.35);
}

.social-circle:active {
  transform: translateY(-1px) rotate(4deg) scale(0.96);
}

.newsletter-input-box {
  display: flex;
  align-items: center;
  background: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.15);
  border-radius: 24px;
  padding: 3px 4px 3px 14px;
  transition: border-color var(--dur-card, 0.25s) ease,
              box-shadow var(--dur-card, 0.25s) ease,
              background-color var(--dur-card, 0.25s) ease;
}

.newsletter-input-box:focus-within {
  border-color: #34d399;
  background: rgba(255, 255, 255, 0.16);
  box-shadow: 0 0 0 3px rgba(52, 211, 153, 0.25);
}

.newsletter-input-box input {
  background: none;
  border: none;
  outline: none;
  color: #fff;
  font-size: 13px;
  flex: 1;
}

.newsletter-input-box input::placeholder {
  color: #94a3b8;
}

.send-btn {
  background: #10b981;
  border: none;
  color: #fff;
  width: 32px;
  height: 32px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all var(--dur-btn, 0.2s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.send-btn:hover {
  background: #059669;
  transform: scale(1.08);
  box-shadow: 0 3px 10px rgba(16, 185, 129, 0.4);
}

.send-btn:active {
  transform: scale(0.94);
}

.footer-bottom {
  border-top: 1px solid rgba(255, 255, 255, 0.08);
  padding: 20px 0;
  background: #081a10;
}

.bottom-content {
  display: flex;
  align-items: center;
  justify-content: space-between;
  max-width: 1240px;
  margin: 0 auto;
  padding: 0 20px;
  font-size: 12px;
  color: #64748b;
}

.lang-selector {
  display: flex;
  align-items: center;
  gap: 8px;
}

.divider {
  opacity: 0.4;
}

.globe-icon {
  font-size: 14px;
}

@media (max-width: 900px) {
  .nav-links {
    display: none;
  }
  .footer-container {
    grid-template-columns: 1fr;
    gap: 32px;
  }
}
</style>
