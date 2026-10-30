<template>
  <div class="host-dashboard">
    <!-- Stat Cards -->
    <div class="stats-grid">
      <div class="stat-card">
        <div class="stat-icon bg-green">
          <CompassOutlined />
        </div>
        <div class="stat-info">
          <span class="stat-title">Tour đang mở bán</span>
          <span class="stat-value">{{ tours.length }}</span>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon bg-blue">
          <UsergroupAddOutlined />
        </div>
        <div class="stat-info">
          <span class="stat-title">Lượt khách đã đặt</span>
          <span class="stat-value">{{ totalGuests }}</span>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon bg-amber">
          <DollarOutlined />
        </div>
        <div class="stat-info">
          <span class="stat-title">Tổng doanh thu</span>
          <span class="stat-value">{{ formatPrice(totalRevenue) }}đ</span>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon bg-purple">
          <StarFilled />
        </div>
        <div class="stat-info">
          <span class="stat-title">Đánh giá trung bình</span>
          <span class="stat-value">5.0 ★</span>
        </div>
      </div>
    </div>

    <!-- Quick Checkin Demo Action Banner -->
    <div class="checkin-banner">
      <div class="banner-content">
        <h3><QrcodeOutlined /> Quét QR Check-in khách tại điểm đón</h3>
        <p>Sử dụng camera hoặc nhập mã định danh để xác thực vé tham gia ngay khi khách đến.</p>
      </div>
      <a-button type="primary" size="large" @click="$router.push('/host/checkin')">
        Mở máy quét Check-in
      </a-button>
    </div>

    <!-- Tours List Section -->
    <div id="tours-table" class="card-table">
      <div class="table-header">
        <h3>Danh sách tour của tôi</h3>
        <a-button type="primary" @click="$router.push('/host/tours/create')">
          + Thêm tour mới
        </a-button>
      </div>

      <a-table
        :data-source="tours"
        :columns="tourColumns"
        :loading="loadingTours"
        row-key="id"
        :pagination="{ pageSize: 5 }"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'title'">
            <div class="tour-cell">
              <img :src="record.coverImageUrl" class="tour-cell-img" />
              <div>
                <strong>{{ record.title }}</strong>
                <div class="text-muted">{{ record.locationName }}</div>
              </div>
            </div>
          </template>

          <template v-else-if="column.key === 'basePrice'">
            <strong style="color: #1B4332;">{{ formatPrice(record.basePrice) }}đ</strong>
          </template>

          <template v-else-if="column.key === 'status'">
            <a-tag :color="getStatusColor(record.status)">
              {{ translateStatus(record.status) }}
            </a-tag>
          </template>

          <template v-else-if="column.key === 'rating'">
            <span>★ {{ record.averageRating.toFixed(1) }} ({{ record.reviewCount }})</span>
          </template>
        </template>
      </a-table>
    </div>

    <!-- Bookings Table Section -->
    <div id="bookings-table" class="card-table" style="margin-top: 30px;">
      <div class="table-header">
        <h3>Đơn đặt tour gần đây</h3>
      </div>

      <a-table
        :data-source="bookings"
        :columns="bookingColumns"
        :loading="loadingBookings"
        row-key="id"
        :pagination="{ pageSize: 5 }"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'code'">
            <strong>{{ record.bookingCode }}</strong>
          </template>

          <template v-else-if="column.key === 'departureDate'">
            <span>{{ formatDate(record.departureDate) }} ({{ record.startTime.substring(0, 5) }})</span>
          </template>

          <template v-else-if="column.key === 'status'">
            <a-tag :color="getBookingStatusColor(record.status)">
              {{ record.status }}
            </a-tag>
          </template>

          <template v-else-if="column.key === 'totalAmount'">
            <strong>{{ formatPrice(record.finalAmount) }}đ</strong>
          </template>
        </template>
      </a-table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch, nextTick } from 'vue'
import { useRoute } from 'vue-router'
import {
  CompassOutlined,
  UsergroupAddOutlined,
  DollarOutlined,
  StarFilled,
  QrcodeOutlined
} from '@ant-design/icons-vue'
import apiClient from '@/services/api'
import type { TourSummary, BookingDetail, ApiResponse } from '@/types'

const loadingTours = ref(false)
const loadingBookings = ref(false)
const tours = ref<TourSummary[]>([])
const bookings = ref<BookingDetail[]>([])

const tourColumns = [
  { title: 'Tên trải nghiệm', key: 'title' },
  { title: 'Giá niêm yết', key: 'basePrice' },
  { title: 'Thời lượng', dataIndex: 'durationHours', key: 'durationHours' },
  { title: 'Trạng thái', key: 'status' },
  { title: 'Đánh giá', key: 'rating' }
]

const bookingColumns = [
  { title: 'Mã đơn', key: 'code' },
  { title: 'Tên khách đại diện', dataIndex: 'contactName', key: 'contactName' },
  { title: 'Số điện thoại', dataIndex: 'contactPhone', key: 'contactPhone' },
  { title: 'Ngày khởi hành', key: 'departureDate' },
  { title: 'Số khách', dataIndex: 'totalParticipants', key: 'totalParticipants' },
  { title: 'Tổng tiền', key: 'totalAmount' },
  { title: 'Trạng thái', key: 'status' }
]

const totalGuests = computed(() => {
  return bookings.value.reduce((acc: number, b: BookingDetail) => acc + (b.totalParticipants || 0), 0)
})

const totalRevenue = computed(() => {
  return bookings.value.reduce((acc: number, b: BookingDetail) => acc + (b.finalAmount || 0), 0)
})

function formatPrice(val: number) {
  return new Intl.NumberFormat('vi-VN').format(val)
}

function formatDate(dateStr: string) {
  if (!dateStr) return ''
  const d = new Date(dateStr)
  return `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')}/${d.getFullYear()}`
}

function getStatusColor(status: any) {
  if (status === 'Published' || status === 2) return 'green'
  if (status === 'PendingApproval' || status === 1) return 'gold'
  if (status === 'Rejected' || status === 3) return 'red'
  return 'default'
}

function translateStatus(status: any) {
  if (status === 'Published' || status === 2) return 'Đang bán'
  if (status === 'PendingApproval' || status === 1) return 'Chờ duyệt'
  if (status === 'Rejected' || status === 3) return 'Bị từ chối'
  return 'Bản nháp'
}

function getBookingStatusColor(status: string) {
  if (status === 'Confirmed' || status === 'Paid') return 'green'
  if (status === 'Completed') return 'blue'
  if (status === 'Cancelled') return 'red'
  return 'orange'
}

async function loadHostData() {
  loadingTours.value = true
  loadingBookings.value = true
  try {
    const [toursRes, bookingsRes] = await Promise.all([
      apiClient.get<ApiResponse<TourSummary[]>>('/tours/host/my-tours'),
      apiClient.get<ApiResponse<BookingDetail[]>>('/bookings/host/all')
    ])
    tours.value = toursRes.data.data || []
    bookings.value = bookingsRes.data.data || []
  } catch (error) {
    console.error('Failed to load host data', error)
  } finally {
    loadingTours.value = false
    loadingBookings.value = false
  }
}

const route = useRoute()

function checkScrollTarget() {
  nextTick(() => {
    if (route.path.endsWith('/tours')) {
      document.getElementById('tours-table')?.scrollIntoView({ behavior: 'smooth' })
    } else if (route.path.endsWith('/bookings')) {
      document.getElementById('bookings-table')?.scrollIntoView({ behavior: 'smooth' })
    }
  })
}

watch(() => route.path, () => {
  checkScrollTarget()
})

onMounted(() => {
  loadHostData()
  checkScrollTarget()
})
</script>

<style scoped>
.stats-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
  margin-bottom: 24px;
}

.stat-card {
  background: #fff;
  border-radius: var(--radius-md);
  padding: 20px;
  display: flex;
  align-items: center;
  gap: 16px;
  border: 1px solid var(--border-color);
  box-shadow: var(--shadow-sm);
}

.stat-icon {
  width: 48px;
  height: 48px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 22px;
}

.bg-green { background: #DCFCE7; color: #166534; }
.bg-blue { background: #E0E7FF; color: #3730A3; }
.bg-amber { background: #FEF3C7; color: #92400E; }
.bg-purple { background: #F3E8FF; color: #6B21A8; }

.stat-info {
  display: flex;
  flex-direction: column;
}

.stat-title {
  font-size: 13px;
  color: var(--text-secondary);
}

.stat-value {
  font-size: 20px;
  font-weight: 800;
  color: var(--text-primary);
}

.checkin-banner {
  background: linear-gradient(135deg, #1B4332 0%, #2D6A4F 100%);
  color: #fff;
  border-radius: var(--radius-md);
  padding: 24px 30px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 30px;
  box-shadow: var(--shadow-md);
}

.banner-content h3 {
  font-size: 18px;
  font-weight: 700;
  color: #fff;
  margin-bottom: 4px;
}

.banner-content p {
  font-size: 13px;
  color: #D8F3DC;
}

.card-table {
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-md);
  padding: 24px;
  box-shadow: var(--shadow-sm);
}

.table-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 18px;
}

.table-header h3 {
  font-size: 17px;
  font-weight: 700;
}

.tour-cell {
  display: flex;
  align-items: center;
  gap: 12px;
}

.tour-cell-img {
  width: 50px;
  height: 40px;
  object-fit: cover;
  border-radius: 4px;
}

.text-muted {
  font-size: 11px;
  color: var(--text-muted);
}

@media (max-width: 1024px) {
  .stats-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}
</style>
