<template>
  <div class="checkout-page container">
    <div class="checkout-header">
      <h1 class="title">Xác nhận thông tin & Đặt chỗ</h1>
      <p class="subtitle">Vui lòng điền thông tin người liên hệ và danh sách người tham gia trải nghiệm</p>
    </div>

    <div v-if="loadingSchedule" class="loading-box">
      <TourLoadingSpin tip="Đang tải dữ liệu chuyến đi & lịch trình..." min-height="350px" />
    </div>

    <div v-else class="checkout-layout">
      <!-- Left Form -->
      <div class="form-area">
        <!-- 1. Contact Info -->
        <div class="card-box">
          <h2 class="card-title">1. Thông tin người đại diện liên hệ</h2>
          <a-form layout="vertical">
            <div class="grid-2">
              <a-form-item label="Họ và tên" required>
                <a-input v-model:value="contactForm.name" placeholder="Nguyễn Văn A" />
              </a-form-item>
              <a-form-item label="Số điện thoại" required>
                <a-input v-model:value="contactForm.phone" placeholder="0912345678" />
              </a-form-item>
            </div>
            <a-form-item label="Email nhận vé điện tử" required>
              <a-input v-model:value="contactForm.email" placeholder="email@example.com" />
            </a-form-item>
            <a-form-item label="Ghi chú cho Host">
              <a-textarea v-model:value="contactForm.notes" placeholder="Yêu cầu ăn kiêng, đón tại điểm hẹn, thuận tay trái..." :rows="2" />
            </a-form-item>
          </a-form>
        </div>

        <!-- 2. Participants List -->
        <div class="card-box">
          <div class="participants-head">
            <h2 class="card-title">2. Danh sách khách tham gia ({{ participants.length }} người)</h2>
            <a-button type="dashed" size="small" @click="addParticipant">
              + Thêm người
            </a-button>
          </div>

          <div v-for="(p, idx) in participants" :key="idx" class="participant-item">
            <div class="p-index">Khách #{{ idx + 1 }}</div>
            <div class="p-fields">
              <a-input v-model:value="p.fullName" placeholder="Họ và tên" style="flex: 2" />
              <a-select v-model:value="p.gender" style="width: 100px">
                <a-select-option value="Nam">Nam</a-select-option>
                <a-select-option value="Nữ">Nữ</a-select-option>
                <a-select-option value="Khác">Khác</a-select-option>
              </a-select>
              <a-select v-model:value="p.participantType" style="width: 130px">
                <a-select-option value="Adult">Người lớn</a-select-option>
                <a-select-option value="Child">Trẻ em</a-select-option>
              </a-select>
              <a-button
                v-if="participants.length > 1"
                type="text"
                danger
                @click="removeParticipant(idx)"
              >
                Xóa
              </a-button>
            </div>
          </div>
        </div>

        <!-- 3. Payment Method -->
        <div class="card-box">
          <h2 class="card-title">3. Phương thức thanh toán</h2>
          <div class="payment-options">
            <label class="pay-option active">
              <input type="radio" checked name="paymentMethod" />
              <div class="pay-info">
                <span class="pay-name">Cổng Sandbox VNPAY (Mô phỏng thanh toán trực tuyến)</span>
                <span class="pay-desc">Thanh toán giả lập an toàn, xác nhận đơn và sinh mã QR vé tức thì.</span>
              </div>
            </label>
          </div>
        </div>
      </div>

      <!-- Right Summary -->
      <div class="summary-area">
        <div class="summary-card glass-panel">
          <h3>Tóm tắt đơn đặt tour</h3>

          <div v-if="scheduleInfo" class="tour-mini-card">
            <img :src="scheduleInfo.tourCoverImage" :alt="scheduleInfo.tourTitle" class="mini-img" />
            <div class="mini-info">
              <h4>{{ scheduleInfo.tourTitle }}</h4>
              <p><EnvironmentOutlined /> {{ scheduleInfo.tourAddress }}</p>
            </div>
          </div>

          <div class="schedule-detail">
            <div class="detail-row">
              <span class="label"><CalendarOutlined /> Ngày đi:</span>
              <span class="val">{{ formatDate(scheduleInfo?.departureDate) }}</span>
            </div>
            <div class="detail-row">
              <span class="label"><ClockCircleOutlined /> Khung giờ:</span>
              <span class="val">{{ scheduleInfo?.startTime?.substring(0, 5) }} - {{ scheduleInfo?.endTime?.substring(0, 5) }}</span>
            </div>
          </div>

          <a-divider style="margin: 16px 0;" />

          <div class="pricing-table">
            <div class="price-row">
              <span>Đơn giá vé:</span>
              <span>{{ formatPrice(scheduleInfo?.price || 0) }}đ / khách</span>
            </div>
            <div class="price-row">
              <span>Số lượng khách:</span>
              <span>x {{ participants.length }} người</span>
            </div>
            <div class="price-row total-row">
              <span>Tổng thanh toán:</span>
              <span class="grand-total">{{ formatPrice(totalAmount) }}đ</span>
            </div>
          </div>

          <div class="holding-alert">
            <ClockCircleOutlined /> Hệ thống sẽ tạm giữ chỗ trong <strong>15 phút</strong> khi bạn bấm xác nhận.
          </div>

          <a-button
            type="primary"
            size="large"
            block
            :loading="submitting"
            class="submit-btn"
            @click="handleSubmitBooking"
          >
            Xác nhận đặt & Thanh toán
          </a-button>

          <p class="terms-note">
            Bằng việc bấm xác nhận, bạn đồng ý với <a href="#">Chính sách hoàn hủy</a> của LocalExplore.
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { message } from 'ant-design-vue'
import {
  EnvironmentOutlined,
  CalendarOutlined,
  ClockCircleOutlined
} from '@ant-design/icons-vue'
import apiClient from '@/services/api'
import type { BookingParticipant, ApiResponse, PagedResponse, TourSummary } from '@/types'
import { useAuthStore } from '@/stores/auth'
import TourLoadingSpin from '@/components/common/TourLoadingSpin.vue'

const route = useRoute()
const router = useRouter()
const authStore = useAuthStore()

const loadingSchedule = ref(false)
const submitting = ref(false)
const scheduleId = Number(route.query.scheduleId)
const initialGuests = Number(route.query.guests) || 1

const scheduleInfo = ref<any>(null)

const contactForm = reactive({
  name: authStore.user?.fullName || '',
  phone: authStore.user?.phoneNumber || '',
  email: authStore.user?.email || '',
  notes: ''
})

const participants = ref<BookingParticipant[]>([])

for (let i = 0; i < initialGuests; i++) {
  participants.value.push({
    fullName: i === 0 ? authStore.user?.fullName || '' : '',
    gender: 'Nam',
    participantType: 'Adult'
  })
}

const totalAmount = computed(() => {
  const price = scheduleInfo.value?.price || 0
  return price * participants.value.length
})

function formatPrice(val: number) {
  return new Intl.NumberFormat('vi-VN').format(val)
}

function formatDate(dateStr: string) {
  if (!dateStr) return ''
  const d = new Date(dateStr)
  return `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')}/${d.getFullYear()}`
}

function addParticipant() {
  participants.value.push({
    fullName: '',
    gender: 'Nam',
    participantType: 'Adult'
  })
}

function removeParticipant(index: number) {
  if (participants.value.length > 1) {
    participants.value.splice(index, 1)
  }
}

async function loadSchedule() {
  loadingSchedule.value = true
  try {
    const tourSlug = route.query.tourSlug as string
    if (tourSlug) {
      const tourRes = await apiClient.get<ApiResponse<any>>(`/tours/${tourSlug}`)
      if (tourRes.data.success && tourRes.data.data) {
        const t = tourRes.data.data
        const matchedSched = t.availableSchedules?.find((s: any) => s.id === scheduleId)
        scheduleInfo.value = {
          price: matchedSched ? matchedSched.price : t.basePrice,
          departureDate: matchedSched ? matchedSched.departureDate : new Date().toISOString(),
          startTime: matchedSched ? matchedSched.startTime : '08:30:00',
          endTime: matchedSched ? matchedSched.endTime : '12:30:00',
          tourTitle: t.title,
          tourCoverImage: t.coverImageUrl,
          tourAddress: t.address || t.locationName
        }
        return
      }
    }

    const res = await apiClient.get<PagedResponse<TourSummary>>(`/tours`)
    if (res.data.items && res.data.items.length) {
      const tour = res.data.items[0]
      scheduleInfo.value = {
        price: tour.basePrice,
        departureDate: new Date().toISOString(),
        startTime: '08:30:00',
        endTime: '12:30:00',
        tourTitle: tour.title,
        tourCoverImage: tour.coverImageUrl,
        tourAddress: tour.locationName
      }
    }
  } catch (error) {
    console.error('Failed to load schedule', error)
  } finally {
    loadingSchedule.value = false
  }
}

async function handleSubmitBooking() {
  if (!contactForm.name.trim() || !contactForm.phone.trim() || !contactForm.email.trim()) {
    message.error('Vui lòng điền đầy đủ họ tên, số điện thoại và email liên hệ.')
    return
  }

  for (let i = 0; i < participants.value.length; i++) {
    if (!participants.value[i].fullName.trim()) {
      message.error(`Vui lòng nhập họ tên cho khách #${i + 1}`)
      return
    }
  }

  submitting.value = true
  try {
    const payload = {
      scheduleId: scheduleId || 2, // fallback to schedule 2 from seed data
      contactName: contactForm.name,
      contactEmail: contactForm.email,
      contactPhone: contactForm.phone,
      notes: contactForm.notes,
      participants: participants.value
    }

    const res = await apiClient.post<ApiResponse<any>>('/bookings', payload)
    if (res.data.success && res.data.data) {
      const booking = res.data.data

      // Tự động thanh toán Sandbox giả lập thành công để phục vụ demo hoàn chỉnh
      message.loading('Đang chuyển sang cổng thanh toán Sandbox VNPAY...', 1.5)
      await apiClient.post(`/bookings/${booking.id}/simulate-payment?method=Sandbox_VNPAY`)

      message.success('Thanh toán thành công! Vé và mã QR đã được kích hoạt.')
      router.push(`/booking-success/${booking.bookingCode}`)
    } else {
      message.error(res.data.message || 'Đặt tour không thành công')
    }
  } catch (error: any) {
    const errMessage = error.response?.data?.message || 'Có lỗi xảy ra khi đặt tour'
    message.error(errMessage)
  } finally {
    submitting.value = false
  }
}

onMounted(() => {
  loadSchedule()
})
</script>

<style scoped>
.checkout-page {
  padding: 40px 20px 80px;
}

.checkout-header {
  margin-bottom: 30px;
}

.title {
  font-size: 28px;
  font-weight: 800;
  margin-bottom: 6px;
}

.subtitle {
  font-size: 14px;
  color: var(--text-secondary);
}

.checkout-layout {
  display: grid;
  grid-template-columns: 2fr 1.2fr;
  gap: 36px;
  align-items: flex-start;
}

.card-box {
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-md);
  padding: 24px;
  margin-bottom: 24px;
}

.card-title {
  font-size: 17px;
  font-weight: 700;
  margin-bottom: 18px;
  color: var(--text-primary);
}

.grid-2 {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

.participants-head {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 16px;
}

.participant-item {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 12px;
  padding: 12px;
  background: var(--bg-main);
  border-radius: var(--radius-sm);
}

.p-index {
  font-size: 12px;
  font-weight: 700;
  color: var(--primary-color);
  width: 70px;
}

.p-fields {
  display: flex;
  align-items: center;
  gap: 8px;
  flex: 1;
}

.payment-options {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.pay-option {
  display: flex;
  align-items: flex-start;
  gap: 12px;
  padding: 16px;
  border: 2px solid var(--border-color);
  border-radius: var(--radius-sm);
  cursor: pointer;
  transition: all 0.2s;
}

.pay-option.active {
  border-color: var(--primary-color);
  background: #F0FDF4;
}

.pay-info {
  display: flex;
  flex-direction: column;
}

.pay-name {
  font-size: 14px;
  font-weight: 700;
  color: var(--text-primary);
}

.pay-desc {
  font-size: 12px;
  color: var(--text-secondary);
}

.summary-card {
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-md);
  padding: 24px;
  box-shadow: var(--shadow-md);
  position: sticky;
  top: 90px;
}

.summary-card h3 {
  font-size: 18px;
  font-weight: 700;
  margin-bottom: 16px;
}

.tour-mini-card {
  display: flex;
  gap: 12px;
  margin-bottom: 16px;
}

.mini-img {
  width: 70px;
  height: 60px;
  object-fit: cover;
  border-radius: 6px;
}

.mini-info h4 {
  font-size: 14px;
  font-weight: 700;
  line-height: 1.3;
  margin-bottom: 4px;
}

.mini-info p {
  font-size: 12px;
  color: var(--text-secondary);
}

.schedule-detail {
  background: var(--bg-main);
  padding: 12px;
  border-radius: 6px;
  font-size: 13px;
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.detail-row {
  display: flex;
  justify-content: space-between;
}

.detail-row .val {
  font-weight: 600;
}

.pricing-table {
  display: flex;
  flex-direction: column;
  gap: 8px;
  font-size: 13px;
  margin-bottom: 16px;
}

.price-row {
  display: flex;
  justify-content: space-between;
}

.total-row {
  font-size: 16px;
  font-weight: 700;
  color: var(--text-primary);
  margin-top: 8px;
  padding-top: 8px;
  border-top: 1px solid var(--border-color);
}

.grand-total {
  font-size: 20px;
  font-weight: 800;
  color: var(--primary-color);
}

.holding-alert {
  background: #FEF3C7;
  color: #92400E;
  padding: 10px 14px;
  border-radius: 6px;
  font-size: 12px;
  margin-bottom: 20px;
  display: flex;
  align-items: center;
  gap: 8px;
}

.submit-btn {
  height: 48px;
  border-radius: 8px;
  font-weight: 700;
  font-size: 15px;
}

.terms-note {
  font-size: 11px;
  color: var(--text-muted);
  text-align: center;
  margin-top: 12px;
}

@media (max-width: 900px) {
  .checkout-layout {
    grid-template-columns: 1fr;
  }
}
</style>
