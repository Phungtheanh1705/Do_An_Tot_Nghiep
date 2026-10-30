<template>
  <div class="success-page container">
    <div class="success-card glass-panel">
      <div class="icon-circle">
        <CheckCircleFilled class="success-icon" />
      </div>

      <h1 class="success-title">Đặt chỗ & Thanh toán thành công!</h1>
      <p class="success-desc">
        Cảm ơn bạn đã lựa chọn trải nghiệm bản địa cùng LocalExplore. Thông tin vé điện tử và mã QR check-in đã sẵn sàng.
      </p>

      <div v-if="booking" class="ticket-box">
        <div class="ticket-header">
          <span class="badge-tag">VÉ ĐIỆN TỬ CHÍNH THỨC</span>
          <span class="booking-code">Mã đơn: <strong>{{ booking.bookingCode }}</strong></span>
        </div>

        <div class="ticket-body">
          <div class="ticket-info">
            <h3 class="tour-title">{{ booking.tourTitle }}</h3>
            <p class="tour-address"><EnvironmentOutlined /> {{ booking.tourAddress }}</p>

            <div class="info-grid">
              <div>
                <span class="lbl">Ngày khởi hành</span>
                <span class="val">{{ formatDate(booking.departureDate) }}</span>
              </div>
              <div>
                <span class="lbl">Thời gian</span>
                <span class="val">{{ booking.startTime.substring(0, 5) }} - {{ booking.endTime.substring(0, 5) }}</span>
              </div>
              <div>
                <span class="lbl">Số khách</span>
                <span class="val">{{ booking.totalParticipants }} người</span>
              </div>
              <div>
                <span class="lbl">Đã thanh toán</span>
                <span class="val val-price">{{ formatPrice(booking.finalAmount) }}đ</span>
              </div>
            </div>

            <div class="contact-preview">
              <span>Đại diện: <strong>{{ booking.contactName }}</strong> ({{ booking.contactPhone }})</span>
            </div>
          </div>

          <!-- QR Code Area -->
          <div class="ticket-qr">
            <div class="qr-frame">
              <img
                :src="`https://api.qrserver.com/v1/create-qr-code/?size=180x180&data=${booking.qrCodeToken}`"
                alt="QR Code Check-in"
                class="qr-img"
              />
            </div>
            <span class="qr-token-label">Token: {{ booking.qrCodeToken.substring(0, 16) }}...</span>
            <span class="qr-hint">Xuất trình mã này cho Host tại điểm đón</span>
          </div>
        </div>
      </div>

      <div class="success-actions">
        <a-button type="primary" size="large" @click="$router.push('/my-bookings')">
          Xem chuyến đi trong tài khoản
        </a-button>
        <a-button size="large" @click="$router.push('/')">
          Về trang chủ
        </a-button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { CheckCircleFilled, EnvironmentOutlined } from '@ant-design/icons-vue'
import apiClient from '@/services/api'
import type { BookingDetail, ApiResponse } from '@/types'

const route = useRoute()
const bookingCode = route.params.code as string
const booking = ref<BookingDetail | null>(null)

function formatPrice(val: number) {
  return new Intl.NumberFormat('vi-VN').format(val)
}

function formatDate(dateStr: string) {
  if (!dateStr) return ''
  const d = new Date(dateStr)
  return `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')}/${d.getFullYear()}`
}

async function loadBooking() {
  try {
    const res = await apiClient.get<ApiResponse<BookingDetail>>(`/bookings/${bookingCode}`)
    if (res.data.success && res.data.data) {
      booking.value = res.data.data
    }
  } catch (error) {
    console.error('Failed to load booking', error)
  }
}

onMounted(() => {
  loadBooking()
})
</script>

<style scoped>
.success-page {
  padding: 60px 20px 100px;
  display: flex;
  justify-content: center;
}

.success-card {
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-lg);
  padding: 40px;
  max-width: 760px;
  width: 100%;
  text-align: center;
  box-shadow: var(--shadow-lg);
}

.icon-circle {
  width: 72px;
  height: 72px;
  background: #DCFCE7;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 20px;
}

.success-icon {
  font-size: 40px;
  color: #16A34A;
}

.success-title {
  font-size: 26px;
  font-weight: 800;
  color: var(--text-primary);
  margin-bottom: 8px;
}

.success-desc {
  font-size: 14px;
  color: var(--text-secondary);
  max-width: 500px;
  margin: 0 auto 30px;
}

.ticket-box {
  background: var(--bg-main);
  border: 2px dashed #CBD5E1;
  border-radius: var(--radius-md);
  padding: 24px;
  text-align: left;
  margin-bottom: 30px;
}

.ticket-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-bottom: 16px;
  border-bottom: 1px solid #E2E8F0;
  margin-bottom: 16px;
}

.badge-tag {
  background: #1B4332;
  color: #fff;
  font-size: 11px;
  font-weight: 700;
  padding: 3px 8px;
  border-radius: 4px;
  letter-spacing: 0.5px;
}

.booking-code {
  font-size: 14px;
  color: var(--text-secondary);
}

.ticket-body {
  display: flex;
  gap: 30px;
  align-items: center;
}

.ticket-info {
  flex: 1;
}

.tour-title {
  font-size: 18px;
  font-weight: 700;
  margin-bottom: 4px;
  color: var(--text-primary);
}

.tour-address {
  font-size: 12px;
  color: var(--text-secondary);
  margin-bottom: 16px;
}

.info-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 12px;
  margin-bottom: 16px;
}

.info-grid .lbl {
  display: block;
  font-size: 11px;
  color: var(--text-muted);
}

.info-grid .val {
  font-size: 14px;
  font-weight: 600;
}

.info-grid .val-price {
  color: var(--primary-color);
  font-weight: 700;
}

.contact-preview {
  font-size: 12px;
  color: var(--text-secondary);
  padding-top: 10px;
  border-top: 1px solid #E2E8F0;
}

.ticket-qr {
  display: flex;
  flex-direction: column;
  align-items: center;
  background: #fff;
  padding: 16px;
  border-radius: var(--radius-sm);
  border: 1px solid var(--border-color);
  width: 200px;
}

.qr-frame {
  width: 150px;
  height: 150px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.qr-img {
  width: 100%;
  height: 100%;
  object-fit: contain;
}

.qr-token-label {
  font-size: 10px;
  color: var(--text-muted);
  margin-top: 6px;
  font-family: monospace;
}

.qr-hint {
  font-size: 11px;
  color: #166534;
  font-weight: 600;
  margin-top: 4px;
  text-align: center;
}

.success-actions {
  display: flex;
  justify-content: center;
  gap: 16px;
}

@media (max-width: 650px) {
  .ticket-body {
    flex-direction: column;
  }
  .ticket-qr {
    width: 100%;
  }
}
</style>
