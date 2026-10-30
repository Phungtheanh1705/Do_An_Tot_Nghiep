import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

import CustomerLayout from '@/layouts/CustomerLayout.vue'
import HostLayout from '@/layouts/HostLayout.vue'
import AdminLayout from '@/layouts/AdminLayout.vue'

import HomeView from '@/views/customer/HomeView.vue'
import TourCatalogView from '@/views/customer/TourCatalogView.vue'
import TourDetailView from '@/views/customer/TourDetailView.vue'
import BookingCheckoutView from '@/views/customer/BookingCheckoutView.vue'
import BookingSuccessView from '@/views/customer/BookingSuccessView.vue'
import MyBookingsView from '@/views/customer/MyBookingsView.vue'
import CategoriesView from '@/views/customer/CategoriesView.vue'
import AboutView from '@/views/customer/AboutView.vue'
import BecomeHostView from '@/views/customer/BecomeHostView.vue'
import ContactView from '@/views/customer/ContactView.vue'

import LoginView from '@/views/auth/LoginView.vue'
import RegisterView from '@/views/auth/RegisterView.vue'

import HostDashboardView from '@/views/host/HostDashboardView.vue'
import CreateTourView from '@/views/host/CreateTourView.vue'
import QrScannerView from '@/views/host/QrScannerView.vue'

import AdminDashboardView from '@/views/admin/AdminDashboardView.vue'
import TourApprovalView from '@/views/admin/TourApprovalView.vue'
import AdminHostApprovalView from '@/views/admin/AdminHostApprovalView.vue'

const routes: RouteRecordRaw[] = [
  // 1. Customer Portal Routes
  {
    path: '/',
    component: CustomerLayout,
    children: [
      {
        path: '',
        name: 'Home',
        component: HomeView,
        meta: { title: 'LocalExplore - Tour Du Lịch Trải Nghiệm Bản Địa' }
      },
      {
        path: 'tours',
        name: 'Tours',
        component: TourCatalogView,
        meta: { title: 'Khám phá Tour Trải nghiệm' }
      },
      {
        path: 'tours/:slug',
        name: 'TourDetail',
        component: TourDetailView,
        meta: { title: 'Chi tiết Tour Trải nghiệm' }
      },
      {
        path: 'checkout',
        name: 'BookingCheckout',
        component: BookingCheckoutView,
        meta: { title: 'Đặt tour & Giữ chỗ', requiresAuth: true }
      },
      {
        path: 'booking-success/:code',
        name: 'BookingSuccess',
        component: BookingSuccessView,
        meta: { title: 'Đặt tour thành công', requiresAuth: true }
      },
      {
        path: 'my-bookings',
        name: 'MyBookings',
        component: MyBookingsView,
        meta: { title: 'Vé & Chuyến đi của tôi', requiresAuth: true }
      },
      {
        path: 'categories',
        name: 'Categories',
        component: CategoriesView,
        meta: { title: 'Danh mục Trải nghiệm Bản địa' }
      },
      {
        path: 'about',
        name: 'About',
        component: AboutView,
        meta: { title: 'Về chúng tôi - Sứ mệnh LocalExplore' }
      },
      {
        path: 'become-host',
        name: 'BecomeHost',
        component: BecomeHostView,
        meta: { title: 'Trở thành Local Host' }
      },
      {
        path: 'contact',
        name: 'Contact',
        component: ContactView,
        meta: { title: 'Liên hệ & Hỗ trợ' }
      }
    ]
  },

  // 2. Auth Routes
  {
    path: '/login',
    name: 'Login',
    component: LoginView,
    meta: { title: 'Đăng nhập LocalExplore' }
  },
  {
    path: '/register',
    name: 'Register',
    component: RegisterView,
    meta: { title: 'Đăng ký tài khoản' }
  },

  // 3. Host Portal Routes
  {
    path: '/host',
    component: HostLayout,
    meta: { requiresAuth: true, role: 'Host' },
    children: [
      {
        path: '',
        name: 'HostDashboard',
        component: HostDashboardView,
        meta: { title: 'Tổng quan Host Dashboard' }
      },
      {
        path: 'tours',
        name: 'HostTours',
        component: HostDashboardView,
        meta: { title: 'Quản lý Tour của tôi' }
      },
      {
        path: 'tours/create',
        name: 'CreateTour',
        component: CreateTourView,
        meta: { title: 'Tạo Tour Trải nghiệm Mới' }
      },
      {
        path: 'bookings',
        name: 'HostBookings',
        component: HostDashboardView,
        meta: { title: 'Danh sách Đơn đặt tour' }
      },
      {
        path: 'checkin',
        name: 'HostCheckIn',
        component: QrScannerView,
        meta: { title: 'Quét QR Check-in khách' }
      }
    ]
  },

  // 4. Admin Portal Routes
  {
    path: '/admin',
    component: AdminLayout,
    meta: { requiresAuth: true, role: 'Admin' },
    children: [
      {
        path: '',
        name: 'AdminDashboard',
        component: AdminDashboardView,
        meta: { title: 'Dashboard Quản trị sàn' }
      },
      {
        path: 'tours/approval',
        name: 'TourApproval',
        component: TourApprovalView,
        meta: { title: 'Kiểm duyệt Tour đăng tải' }
      },
      {
        path: 'hosts',
        name: 'HostApproval',
        component: AdminHostApprovalView,
        meta: { title: 'Duyệt hồ sơ KYC Host' }
      },
      {
        path: 'users',
        name: 'AdminUsers',
        component: AdminDashboardView,
        meta: { title: 'Quản lý Người dùng' }
      }
    ]
  },

  // 404 Fallback
  {
    path: '/:pathMatch(.*)*',
    redirect: '/'
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior(to, _from, savedPosition) {
    if (savedPosition) return savedPosition
    if (to.hash) {
      return { el: to.hash, behavior: 'smooth' }
    }
    return { top: 0 }
  }
})

// Navigation Guard
router.beforeEach((to, _from, next) => {
  const authStore = useAuthStore()

  if (to.meta.title) {
    document.title = `${to.meta.title} | LocalExplore`
  }

  // Kiểm tra yêu cầu đăng nhập
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next({ path: '/login', query: { redirect: to.fullPath } })
    return
  }

  // Kiểm tra Role Host
  if (to.meta.role === 'Host' && !authStore.isHost && !authStore.isAdmin) {
    next({ path: '/' })
    return
  }

  // Kiểm tra Role Admin
  if (to.meta.role === 'Admin' && !authStore.isAdmin) {
    next({ path: '/' })
    return
  }

  next()
})

export default router
