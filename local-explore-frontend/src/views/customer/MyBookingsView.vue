<template>
  <div class="my-bookings-page container">
    <div class="page-head">
      <h1 class="title">Vé & Chuyến đi của tôi</h1>
      <p class="subtitle">Quản lý các tour đã đặt, xuất trình mã QR và đánh giá trải nghiệm</p>
    </div>

    <TourLoadingSpin v-if="loading" tip="Đang tải danh sách vé & chuyến đi của bạn..." min-height="350px" />

    <template v-else>
      <a-tabs v-model:activeKey="activeTab">
        <a-tab-pane key="all" tab="Tất cả đơn đặt" />
        <a-tab-pane key="upcoming" tab="Sắp diễn ra" />
        <a-tab-pane key="completed" tab="Đã hoàn thành" />
        <a-tab-pane key="cancelled" tab="Đã hủy" />
      </a-tabs>

      <div v-if="filteredBookings.length > 0" class="bookings-list">
        <div v-for="b in filteredBookings" :key="b.id" class="booking-card">
          <div class="card-left">
            <img :src="b.tourCoverImage" :alt="b.tourTitle" class="tour-thumb" />
          </div>

          <div class="card-center">
            <div class="header-line">
              <span class="booking-code">Mã đơn: <strong>{{ b.bookingCode }}</strong></span>
              <span class="badge-status" :class="`badge-${b.status.toLowerCase()}`">
                {{ translateStatus(b.status) }}
              </span>
            </div>

            <h3 class="tour-title">{{ b.tourTitle }}</h3>
            <p class="tour-addr"><EnvironmentOutlined /> {{ b.tourAddress }}</p>

            <div class="meta-row">
              <span class="meta-item"><CalendarOutlined /> Ngày: <strong>{{ formatDate(b.departureDate) }}</strong></span>
              <span class="meta-item"><ClockCircleOutlined /> Giờ: <strong>{{ b.startTime.substring(0, 5) }}</strong></span>
              <span class="meta-item"><UserOutlined /> Khách: <strong>{{ b.totalParticipants }} người</strong></span>
            </div>
          </div>

          <div class="card-right">
            <div class="price-tag">{{ formatPrice(b.finalAmount) }}đ</div>

            <div class="actions">
              <!-- Show QR Code Button -->
              <a-button
                v-if="b.status === 'Confirmed' || b.status === 'Paid'"
                type="primary"
                @click="openQrModal(b)"
              >
                <QrcodeOutlined /> Xuất trình vé QR
              </a-button>

              <!-- Write Review Button -->
              <a-button
                v-if="b.status === 'Completed' && !b.hasReviewed"
                type="default"
                style="color: #F59E0B; border-color: #F59E0B;"
                @click="openReviewModal(b)"
              >
                <StarOutlined /> Viết đánh giá
              </a-button>

              <span v-if="b.hasReviewed" class="reviewed-tag">
                <CheckOutlined /> Đã đánh giá
              </span>

              <!-- Cancel Button -->
              <a-popconfirm
                v-if="b.status === 'Confirmed' || b.status === 'PendingPayment'"
                title="Bạn có chắc chắn muốn hủy đơn đặt tour này theo chính sách hoàn tiền?"
                ok-text="Xác nhận hủy"
                cancel-text="Không"
                @confirm="handleCancelBooking(b.id)"
              >
                <a-button danger type="text">
                  Hủy chuyến đi
                </a-button>
              </a-popconfirm>
            </div>
          </div>
        </div>
      </div>

      <a-empty
        v-else
        description="Bạn chưa có đơn đặt tour nào trong mục này."
        style="margin: 60px 0;"
      >
        <a-button type="primary" @click="$router.push('/tours')">
          Khám phá tour ngay
        </a-button>
      </a-empty>
    </template>

    <!-- Modal Xem Mã QR -->
    <a-modal
      v-model:open="qrModalVisible"
      title="Vé điện tử & Mã QR Check-in"
      :footer="null"
      centered
      width="420px"
    >
      <div v-if="selectedBooking" class="qr-modal-content">
        <h3 class="modal-tour-title">{{ selectedBooking.tourTitle }}</h3>
        <p class="modal-time">{{ formatDate(selectedBooking.departureDate) }} lúc {{ selectedBooking.startTime.substring(0, 5) }}</p>

        <div class="modal-qr-box">
          <img
            :src="`https://api.qrserver.com/v1/create-qr-code/?size=220x220&data=${selectedBooking.qrCodeToken}`"
            alt="Mã QR"
            class="modal-qr-img"
          />
        </div>

        <div class="qr-token-str">Mã xác thực: {{ selectedBooking.qrCodeToken }}</div>
        <p class="qr-guide">Đưa màn hình này cho Local Host quét khi đến điểm hẹn để tham gia trải nghiệm.</p>
      </div>
    </a-modal>

    <!-- Modal Viết Đánh Giá -->
    <a-modal
      v-model:open="reviewModalVisible"
      title="Đánh giá trải nghiệm bản địa"
      :confirm-loading="submittingReview"
      ok-text="Gửi đánh giá"
      cancel-text="Hủy"
      @ok="handleSubmitReview"
    >
      <div v-if="selectedBooking" class="review-form">
        <h4>{{ selectedBooking.tourTitle }}</h4>
        <div class="rate-field">
          <label>Chấm điểm số sao:</label>
          <a-rate v-model:value="reviewForm.rating" />
        </div>

        <div class="comment-field">
          <label>Cảm nhận của bạn về chuyến đi & Local Host:</label>
          <a-textarea
            v-model:value="reviewForm.comment"
            placeholder="Chia sẻ kỷ niệm, những điều bạn thích nhất về văn hóa, món ăn hoặc sự nhiệt tình của Host..."
            :rows="4"
          />
        </div>
      </div>
    </a-modal>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted } from 'vue'
import { message } from 'ant-design-vue'
import {
  EnvironmentOutlined,
  CalendarOutlined,
  ClockCircleOutlined,
  UserOutlined,
  QrcodeOutlined,
  StarOutlined,
  CheckOutlined
} from '@ant-design/icons-vue'
import apiClient from '@/services/api'
import type { BookingDetail, ApiResponse } from '@/types'
import TourLoadingSpin from '@/components/common/TourLoadingSpin.vue'

const loading = ref(false)
const bookings = ref<BookingDetail[]>([])
const activeTab = ref('all')

const qrModalVisible = ref(false)
const reviewModalVisible = ref(false)
const selectedBooking = ref<BookingDetail | null>(null)
const submittingReview = ref(false)

const reviewForm = reactive({
  rating: 5,
  comment: ''
})

const filteredBookings = computed(() => {
  if (activeTab.value === 'upcoming') {
    return bookings.value.filter((b: BookingDetail) => b.status === 'Confirmed' || b.status === 'Paid')
  }
  if (activeTab.value === 'completed') {
    return bookings.value.filter((b: BookingDetail) => b.status === 'Completed')
  }
  if (activeTab.value === 'cancelled') {
    return bookings.value.filter((b: BookingDetail) => b.status === 'Cancelled' || b.status === 'Refunded')
  }
  return bookings.value
})

function formatPrice(val: number) {
  return new Intl.NumberFormat('vi-VN').format(val)
}

function formatDate(dateStr: string) {
  if (!dateStr) return ''
  const d = new Date(dateStr)
  return `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')}/${d.getFullYear()}`
}

function translateStatus(st: string) {
  const map: Record<string, string> = {
    PendingPayment: 'Chờ thanh toán',
    Confirmed: 'Đã xác nhận (Có vé)',
    Paid: 'Đã thanh toán',
    Completed: 'Đã hoàn thành',
    Cancelled: 'Đã hủy',
    Refunded: 'Đã hoàn tiền'
  }
  return map[st] || st
}

function openQrModal(b: BookingDetail) {
  selectedBooking.value = b
  qrModalVisible.value = true
}

function openReviewModal(b: BookingDetail) {
  selectedBooking.value = b
  reviewForm.rating = 5
  reviewForm.comment = ''
  reviewModalVisible.value = true
}

async function loadBookings() {
  loading.value = true
  try {
    const res = await apiClient.get<ApiResponse<BookingDetail[]>>('/bookings/my-bookings')
    bookings.value = res.data.data || []
  } catch (error) {
    console.error('Failed to load my bookings', error)
  } finally {
    loading.value = false
  }
}

async function handleCancelBooking(bookingId: number) {
  try {
    const res = await apiClient.post<ApiResponse<any>>(`/bookings/${bookingId}/cancel`)
    if (res.data.success) {
      message.success(res.data.message || 'Đã hủy tour thành công')
      loadBookings()
    } else {
      message.error(res.data.message)
    }
  } catch (error: any) {
    message.error(error.response?.data?.message || 'Không thể hủy tour')
  }
}

async function handleSubmitReview() {
  if (!selectedBooking.value) return
  if (!reviewForm.comment.trim()) {
    message.warning('Vui lòng viết vài dòng cảm nhận của bạn.')
    return
  }

  submittingReview.value = true
  try {
    const payload = {
      bookingId: selectedBooking.value.id,
      rating: reviewForm.rating,
      comment: reviewForm.comment
    }
    const res = await apiClient.post<ApiResponse<any>>('/reviews', payload)
    if (res.data.success) {
      message.success('Đánh giá của bạn đã được ghi nhận!')
      reviewModalVisible.value = false
      loadBookings()
    } else {
      message.error(res.data.message)
    }
  } catch (error: any) {
    message.error(error.response?.data?.message || 'Lỗi khi gửi review')
  } finally {
    submittingReview.value = false
  }
}

onMounted(() => {
  loadBookings()
})
</script>

<style scoped>
.my-bookings-page {
  padding: 40px 20px 80px;
}

.page-head {
  margin-bottom: 24px;
}

.title {
  font-size: 28px;
  font-weight: 800;
  margin-bottom: 4px;
}

.subtitle {
  font-size: 14px;
  color: var(--text-secondary);
}

.bookings-list {
  display: flex;
  flex-direction: column;
  gap: 20px;
  margin-top: 24px;
}

.booking-card {
  display: flex;
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-md);
  padding: 20px;
  gap: 20px;
  align-items: center;
  box-shadow: var(--shadow-sm);
}

.tour-thumb {
  width: 140px;
  height: 100px;
  object-fit: cover;
  border-radius: var(--radius-sm);
}

.card-center {
  flex: 1;
}

.header-line {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 6px;
}

.booking-code {
  font-size: 12px;
  color: var(--text-secondary);
}

.tour-title {
  font-size: 17px;
  font-weight: 700;
  margin-bottom: 4px;
  color: var(--text-primary);
}

.tour-addr {
  font-size: 12px;
  color: var(--text-secondary);
  margin-bottom: 10px;
}

.meta-row {
  display: flex;
  gap: 16px;
  font-size: 12px;
  color: var(--text-secondary);
}

.card-right {
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 12px;
}

.price-tag {
  font-size: 18px;
  font-weight: 800;
  color: var(--primary-color);
}

.actions {
  display: flex;
  flex-direction: column;
  gap: 8px;
  align-items: flex-end;
}

.reviewed-tag {
  font-size: 12px;
  color: #16A34A;
  font-weight: 600;
}

.qr-modal-content {
  text-align: center;
  padding: 10px 0;
}

.modal-tour-title {
  font-size: 16px;
  font-weight: 700;
  margin-bottom: 4px;
}

.modal-time {
  font-size: 13px;
  color: var(--text-secondary);
  margin-bottom: 16px;
}

.modal-qr-box {
  background: var(--bg-main);
  padding: 16px;
  border-radius: var(--radius-md);
  display: inline-block;
  margin-bottom: 12px;
}

.modal-qr-img {
  width: 200px;
  height: 200px;
}

.qr-token-str {
  font-size: 11px;
  font-family: monospace;
  color: var(--text-muted);
  margin-bottom: 10px;
  word-break: break-all;
}

.qr-guide {
  font-size: 12px;
  color: var(--primary-color);
  font-weight: 600;
}

.review-form h4 {
  font-size: 15px;
  font-weight: 700;
  margin-bottom: 14px;
}

.rate-field {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 16px;
}

.rate-field label {
  font-weight: 600;
}

.comment-field label {
  display: block;
  font-weight: 600;
  margin-bottom: 6px;
}

@media (max-width: 768px) {
  .booking-card {
    flex-direction: column;
    align-items: flex-start;
  }
  .tour-thumb {
    width: 100%;
    height: 160px;
  }
  .card-right {
    width: 100%;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
  }
  .actions {
    flex-direction: row;
  }
}
</style>
