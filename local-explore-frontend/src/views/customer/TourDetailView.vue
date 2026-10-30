<template>
  <div v-if="loading" class="container loading-box">
    <TourLoadingSpin tip="Đang tải thông tin chi tiết tour & lịch trình bản địa..." min-height="380px" />
  </div>

  <div v-else-if="tour" class="detail-page container">
    <!-- Breadcrumb -->
    <a-breadcrumb class="breadcrumb">
      <a-breadcrumb-item><router-link to="/">Trang chủ</router-link></a-breadcrumb-item>
      <a-breadcrumb-item><router-link to="/tours">Khám phá</router-link></a-breadcrumb-item>
      <a-breadcrumb-item>{{ tour.categoryName }}</a-breadcrumb-item>
      <a-breadcrumb-item>{{ tour.title }}</a-breadcrumb-item>
    </a-breadcrumb>

    <!-- Header Section -->
    <div class="tour-header">
      <div class="tags">
        <span class="badge-cat">{{ tour.categoryName }}</span>
        <span class="badge-loc"><EnvironmentOutlined /> {{ tour.provinceName }}</span>
      </div>

      <h1 class="title">{{ tour.title }}</h1>

      <div class="header-meta">
        <div class="rating">
          <StarFilled class="star-icon" />
          <span class="score">{{ tour.averageRating.toFixed(1) }}</span>
          <span class="count">({{ tour.reviewCount }} đánh giá)</span>
        </div>
        <span class="dot">•</span>
        <div class="address"><EnvironmentOutlined /> {{ tour.address }}</div>
        <span class="dot">•</span>
        <div class="duration"><ClockCircleOutlined /> {{ tour.durationHours }} giờ</div>
      </div>
    </div>

    <!-- Gallery Section -->
    <div class="gallery-grid">
      <div class="main-img">
        <img :src="selectedImage || tour.coverImageUrl" :alt="tour.title" />
      </div>
      <div v-if="tour.images && tour.images.length > 1" class="thumb-list">
        <div
          v-for="img in tour.images"
          :key="img.id"
          class="thumb-item"
          :class="{ active: selectedImage === img.imageUrl }"
          @click="selectedImage = img.imageUrl"
        >
          <img :src="img.imageUrl" :alt="img.caption || tour.title" />
        </div>
      </div>
    </div>

    <!-- Main Content & Sticky Booking Grid -->
    <div class="detail-layout">
      <!-- Left Content -->
      <div class="content-left">
        <!-- Host Bio Box -->
        <div class="host-box glass-panel">
          <a-avatar :src="tour.hostAvatarUrl" :size="56">
            {{ tour.hostName.charAt(0) }}
          </a-avatar>
          <div class="host-details">
            <h3>Trải nghiệm được tổ chức bởi {{ tour.hostName }}</h3>
            <p>Người dân bản địa tâm huyết • Hướng dẫn bằng {{ tour.supportedLanguages }}</p>
          </div>
        </div>

        <!-- Description -->
        <div class="section-block">
          <h2>Giới thiệu trải nghiệm</h2>
          <p class="desc-text">{{ tour.detailedDescription }}</p>
        </div>

        <!-- Highlights & Inclusions -->
        <div class="section-block grid-2">
          <div class="highlight-card">
            <h3><CheckCircleOutlined class="icon-green" /> Dịch vụ bao gồm</h3>
            <p>{{ tour.includedServices || 'Dụng cụ trải nghiệm, hướng dẫn viên bản địa, nước uống thảo mộc.' }}</p>
          </div>
          <div class="highlight-card">
            <h3><CloseCircleOutlined class="icon-red" /> Chưa bao gồm</h3>
            <p>{{ tour.excludedServices || 'Chi phí di chuyển cá nhân ngoài chương trình, chi phí mua sắm thêm.' }}</p>
          </div>
        </div>

        <!-- Itinerary Timeline -->
        <div v-if="tour.itineraries && tour.itineraries.length" class="section-block">
          <h2>Lộ trình chi tiết từng chặng</h2>
          <div class="itinerary-timeline">
            <div v-for="itin in tour.itineraries" :key="itin.stepOrder" class="timeline-step">
              <div class="step-badge">Chặng {{ itin.stepOrder }}</div>
              <div class="step-body">
                <h4>{{ itin.title }} <span class="step-time">({{ itin.durationMinutes }} phút)</span></h4>
                <p>{{ itin.description }}</p>
                <div v-if="itin.locationName" class="step-loc">
                  <EnvironmentOutlined /> {{ itin.locationName }}
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Cancellation Policy -->
        <div class="section-block">
          <h2>Chính sách hoàn hủy vé</h2>
          <div class="policy-box">
            <h4>{{ tour.cancellationPolicyName }}</h4>
            <p>{{ tour.cancellationPolicyDescription }}</p>
          </div>
        </div>

        <!-- Leaflet Map Location -->
        <div class="section-block">
          <h2>Vị trí điểm hẹn trên bản đồ</h2>
          <p class="map-address"><EnvironmentOutlined /> {{ tour.address }}</p>
          <div id="tour-map" class="map-container"></div>
        </div>

        <!-- Customer Reviews -->
        <div class="section-block">
          <div class="reviews-head">
            <h2>Đánh giá từ khách tham gia ({{ tour.reviewCount }})</h2>
            <div class="score-large">
              <StarFilled class="star-icon" /> {{ tour.averageRating.toFixed(1) }} / 5.0
            </div>
          </div>

          <div v-if="tour.recentReviews && tour.recentReviews.length" class="reviews-list">
            <div v-for="rev in tour.recentReviews" :key="rev.id" class="review-card">
              <div class="rev-header">
                <a-avatar :src="rev.customerAvatarUrl" :size="40">
                  {{ rev.customerName.charAt(0) }}
                </a-avatar>
                <div>
                  <div class="rev-name">{{ rev.customerName }}</div>
                  <div class="rev-date">{{ formatDate(rev.createdAt) }}</div>
                </div>
                <div class="rev-stars">
                  <StarFilled v-for="n in rev.rating" :key="n" class="star-icon" />
                </div>
              </div>
              <p class="rev-comment">{{ rev.comment }}</p>

              <!-- Host reply -->
              <div v-if="rev.hostReply" class="host-reply-box">
                <div class="reply-header">Phản hồi từ Host {{ tour.hostName }}:</div>
                <p>{{ rev.hostReply }}</p>
              </div>
            </div>
          </div>
          <p v-else class="text-muted">Chưa có đánh giá nào cho tour này.</p>
        </div>
      </div>

      <!-- Right Sticky Booking Widget -->
      <div class="content-right">
        <div class="booking-widget glass-panel">
          <div class="widget-price">
            <span class="price-num">{{ formatPrice(selectedSchedule?.price || tour.basePrice) }}đ</span>
            <span class="price-unit">/ khách</span>
          </div>

          <!-- Schedule Dropdown -->
          <div class="widget-field">
            <label>1. Chọn lịch khởi hành</label>
            <a-select
              v-model:value="selectedScheduleId"
              placeholder="Chọn ngày & giờ đi"
              style="width: 100%"
              @change="onScheduleChange"
            >
              <a-select-option
                v-for="s in tour.availableSchedules"
                :key="s.id"
                :value="s.id"
              >
                {{ formatDate(s.departureDate) }} ({{ s.startTime.substring(0, 5) }} - {{ s.endTime.substring(0, 5) }}) - Còn {{ s.remainingCapacity }} chỗ
              </a-select-option>
            </a-select>
          </div>

          <!-- Capacity alert -->
          <div v-if="selectedSchedule" class="capacity-alert">
            <span class="dot-green">●</span>
            <span>Còn trống <strong>{{ selectedSchedule.remainingCapacity }}</strong> chỗ cho đợt này</span>
          </div>

          <!-- Number of Guests -->
          <div class="widget-field">
            <label>2. Số lượng khách tham gia</label>
            <a-input-number
              v-model:value="guestCount"
              :min="1"
              :max="selectedSchedule ? selectedSchedule.remainingCapacity : 10"
              style="width: 100%"
            />
          </div>

          <!-- Summary calculation -->
          <div class="widget-summary">
            <div class="sum-row">
              <span>Đơn giá:</span>
              <span>{{ formatPrice(selectedSchedule?.price || tour.basePrice) }}đ</span>
            </div>
            <div class="sum-row">
              <span>Số lượng:</span>
              <span>x {{ guestCount }} khách</span>
            </div>
            <div class="sum-row total-row">
              <span>Tổng cộng:</span>
              <span class="total-price">{{ formatPrice(totalPrice) }}đ</span>
            </div>
          </div>

          <!-- Action Button -->
          <a-button
            type="primary"
            size="large"
            block
            :disabled="!selectedSchedule || selectedSchedule.remainingCapacity < 1"
            class="book-now-btn"
            @click="handleProceedBooking"
          >
            {{ selectedSchedule ? 'Tiến hành đặt chỗ ngay' : 'Vui lòng chọn ngày khởi hành' }}
          </a-button>

          <p class="widget-note">
            <SafetyCertificateOutlined /> Giữ chỗ tức thì 15 phút • Không tính phí đặt chỗ
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, nextTick } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { message } from 'ant-design-vue'
import L from 'leaflet'
import {
  EnvironmentOutlined,
  ClockCircleOutlined,
  StarFilled,
  CheckCircleOutlined,
  CloseCircleOutlined,
  SafetyCertificateOutlined
} from '@ant-design/icons-vue'
import apiClient from '@/services/api'
import type { TourDetail, TourSchedule, ApiResponse } from '@/types'
import { useAuthStore } from '@/stores/auth'
import TourLoadingSpin from '@/components/common/TourLoadingSpin.vue'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

const loading = ref(true)
const tour = ref<TourDetail | null>(null)
const selectedImage = ref<string>('')
const selectedScheduleId = ref<number | undefined>(undefined)
const guestCount = ref<number>(1)

const selectedSchedule = computed<TourSchedule | undefined>(() => {
  if (!tour.value || !selectedScheduleId.value) return undefined
  return tour.value.availableSchedules.find((s: TourSchedule) => s.id === selectedScheduleId.value)
})

const totalPrice = computed(() => {
  const price = selectedSchedule.value?.price || tour.value?.basePrice || 0
  return price * guestCount.value
})

function formatPrice(val: number) {
  return new Intl.NumberFormat('vi-VN').format(val)
}

function formatDate(dateStr: string) {
  if (!dateStr) return ''
  const d = new Date(dateStr)
  return `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')}/${d.getFullYear()}`
}

function onScheduleChange() {
  if (selectedSchedule.value && guestCount.value > selectedSchedule.value.remainingCapacity) {
    guestCount.value = Math.max(1, selectedSchedule.value.remainingCapacity)
  }
}

function handleProceedBooking() {
  if (!selectedScheduleId.value) {
    message.warning('Vui lòng chọn một lịch khởi hành.')
    return
  }

  if (!authStore.isAuthenticated) {
    message.info('Vui lòng đăng nhập để hoàn tất đặt tour.')
    router.push({ path: '/login', query: { redirect: route.fullPath } })
    return
  }

  router.push({
    path: '/checkout',
    query: {
      tourSlug: tour.value?.slug,
      scheduleId: selectedScheduleId.value,
      guests: guestCount.value
    }
  })
}

async function loadTourDetail() {
  loading.value = true
  try {
    const slug = route.params.slug as string
    const res = await apiClient.get<ApiResponse<TourDetail>>(`/tours/${slug}`)
    if (res.data.success && res.data.data) {
      tour.value = res.data.data
      selectedImage.value = tour.value.coverImageUrl

      if (tour.value.availableSchedules && tour.value.availableSchedules.length > 0) {
        selectedScheduleId.value = tour.value.availableSchedules[0].id
      }

      await nextTick()
      initMap(tour.value.latitude, tour.value.longitude, tour.value.title)
    }
  } catch (error) {
    console.error('Failed to load tour details', error)
  } finally {
    loading.value = false
  }
}

function initMap(lat: number, lng: number, title: string) {
  const mapContainer = document.getElementById('tour-map')
  if (!mapContainer) return

  const map = L.map('tour-map').setView([lat, lng], 14)

  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    attribution: '© OpenStreetMap contributors'
  }).addTo(map)

  const marker = L.marker([lat, lng]).addTo(map)
  marker.bindPopup(`<b>${title}</b><br/>Điểm hẹn khởi hành`).openPopup()
}

onMounted(() => {
  loadTourDetail()
})
</script>

<style scoped>
.loading-box {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 400px;
}

.detail-page {
  padding: 30px 20px 60px;
}

.breadcrumb {
  margin-bottom: 20px;
}

.tour-header {
  margin-bottom: 24px;
}

.tags {
  display: flex;
  gap: 8px;
  margin-bottom: 12px;
}

.badge-cat {
  background: var(--primary-light);
  color: var(--primary-color);
  padding: 4px 10px;
  border-radius: 6px;
  font-size: 12px;
  font-weight: 700;
}

.badge-loc {
  background: #F1F5F9;
  color: var(--text-secondary);
  padding: 4px 10px;
  border-radius: 6px;
  font-size: 12px;
  font-weight: 600;
}

.title {
  font-size: 32px;
  font-weight: 800;
  color: var(--text-primary);
  line-height: 1.3;
  margin-bottom: 12px;
}

.header-meta {
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  gap: 12px;
  font-size: 14px;
  color: var(--text-secondary);
}

.rating {
  display: flex;
  align-items: center;
  gap: 4px;
}

.star-icon {
  color: #F59E0B;
}

.score {
  font-weight: 700;
  color: var(--text-primary);
}

.gallery-grid {
  display: grid;
  grid-template-columns: 3fr 1fr;
  gap: 16px;
  height: 440px;
  margin-bottom: 40px;
  border-radius: var(--radius-lg);
  overflow: hidden;
}

.main-img img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.thumb-list {
  display: flex;
  flex-direction: column;
  gap: 12px;
  overflow-y: auto;
}

.thumb-item {
  height: 135px;
  border-radius: var(--radius-sm);
  overflow: hidden;
  cursor: pointer;
  opacity: 0.7;
  transition: opacity 0.2s, transform 0.2s;
}

.thumb-item.active, .thumb-item:hover {
  opacity: 1;
  transform: scale(0.98);
}

.thumb-item img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.detail-layout {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 40px;
  align-items: flex-start;
}

.host-box {
  display: flex;
  align-items: center;
  gap: 16px;
  padding: 20px;
  border-radius: var(--radius-md);
  margin-bottom: 30px;
  border: 1px solid var(--border-color);
}

.host-details h3 {
  font-size: 17px;
  font-weight: 700;
  margin-bottom: 4px;
}

.host-details p {
  font-size: 13px;
  color: var(--text-secondary);
}

.section-block {
  margin-bottom: 40px;
  padding-bottom: 30px;
  border-bottom: 1px solid var(--border-light);
}

.section-block h2 {
  font-size: 22px;
  font-weight: 700;
  margin-bottom: 16px;
}

.desc-text {
  font-size: 15px;
  line-height: 1.8;
  color: var(--text-secondary);
  white-space: pre-line;
}

.grid-2 {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

.highlight-card {
  background: #fff;
  border: 1px solid var(--border-color);
  padding: 20px;
  border-radius: var(--radius-md);
}

.highlight-card h3 {
  font-size: 15px;
  font-weight: 700;
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 10px;
}

.icon-green {
  color: #16A34A;
}

.icon-red {
  color: #DC2626;
}

.itinerary-timeline {
  display: flex;
  flex-direction: column;
  gap: 20px;
  margin-top: 16px;
}

.timeline-step {
  display: flex;
  gap: 16px;
}

.step-badge {
  background: var(--primary-color);
  color: #fff;
  padding: 6px 12px;
  border-radius: 6px;
  font-size: 12px;
  font-weight: 700;
  height: fit-content;
  white-space: nowrap;
}

.step-body h4 {
  font-size: 16px;
  font-weight: 700;
  margin-bottom: 4px;
}

.step-time {
  font-size: 12px;
  color: var(--text-muted);
  font-weight: 500;
}

.step-body p {
  font-size: 14px;
  color: var(--text-secondary);
  margin-bottom: 6px;
}

.step-loc {
  font-size: 12px;
  color: var(--primary-color);
  font-weight: 600;
}

.policy-box {
  background: #FFFBEB;
  border: 1px solid #FDE68A;
  padding: 16px 20px;
  border-radius: var(--radius-md);
}

.policy-box h4 {
  font-size: 15px;
  font-weight: 700;
  color: #92400E;
  margin-bottom: 6px;
}

.policy-box p {
  font-size: 13px;
  color: #78350F;
}

.map-container {
  height: 320px;
  border-radius: var(--radius-md);
  overflow: hidden;
  border: 1px solid var(--border-color);
  margin-top: 12px;
}

.booking-widget {
  position: sticky;
  top: 90px;
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-lg);
  padding: 28px;
  box-shadow: var(--shadow-lg);
}

.widget-price {
  display: flex;
  align-items: baseline;
  gap: 4px;
  margin-bottom: 20px;
  padding-bottom: 16px;
  border-bottom: 1px solid var(--border-light);
}

.price-num {
  font-size: 26px;
  font-weight: 800;
  color: var(--primary-color);
}

.widget-field {
  margin-bottom: 18px;
}

.widget-field label {
  display: block;
  font-size: 13px;
  font-weight: 700;
  margin-bottom: 6px;
}

.capacity-alert {
  font-size: 12px;
  color: #166534;
  background: #DCFCE7;
  padding: 6px 12px;
  border-radius: 6px;
  margin-bottom: 16px;
  display: flex;
  align-items: center;
  gap: 6px;
}

.widget-summary {
  background: var(--bg-main);
  padding: 14px;
  border-radius: var(--radius-sm);
  margin-bottom: 20px;
}

.sum-row {
  display: flex;
  justify-content: space-between;
  font-size: 13px;
  color: var(--text-secondary);
  margin-bottom: 6px;
}

.total-row {
  margin-top: 10px;
  padding-top: 10px;
  border-top: 1px solid var(--border-color);
  font-weight: 700;
  font-size: 16px;
  color: var(--text-primary);
}

.total-price {
  color: var(--primary-color);
  font-size: 20px;
  font-weight: 800;
}

.book-now-btn {
  height: 48px;
  border-radius: 8px;
  font-weight: 700;
  font-size: 15px;
}

.widget-note {
  margin-top: 14px;
  font-size: 12px;
  color: var(--text-muted);
  text-align: center;
}

.reviews-head {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.score-large {
  font-size: 18px;
  font-weight: 800;
  color: var(--text-primary);
}

.reviews-list {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.review-card {
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-md);
  padding: 18px;
}

.rev-header {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 10px;
}

.rev-name {
  font-size: 14px;
  font-weight: 700;
}

.rev-date {
  font-size: 12px;
  color: var(--text-muted);
}

.rev-stars {
  margin-left: auto;
}

.rev-comment {
  font-size: 14px;
  line-height: 1.6;
  color: var(--text-secondary);
}

.host-reply-box {
  background: #F8FAFC;
  border-left: 3px solid var(--primary-color);
  padding: 10px 14px;
  margin-top: 12px;
  border-radius: 4px;
  font-size: 13px;
}

.reply-header {
  font-weight: 700;
  color: var(--primary-color);
  margin-bottom: 4px;
}

@media (max-width: 900px) {
  .detail-layout {
    grid-template-columns: 1fr;
  }
  .gallery-grid {
    grid-template-columns: 1fr;
    height: auto;
  }
  .thumb-list {
    flex-direction: row;
    height: 100px;
  }
  .thumb-item {
    width: 120px;
  }
}
</style>
