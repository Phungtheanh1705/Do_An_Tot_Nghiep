<template>
  <div class="approval-page">
    <div class="card-box">
      <div class="header-line">
        <div>
          <h3>Kiểm duyệt Tour đăng tải từ Local Host</h3>
          <p class="subtitle">Thẩm định chất lượng văn hóa, an toàn và mức giá trước khi công khai lên sàn</p>
        </div>
        <a-button @click="loadTours">
          Làm mới danh sách
        </a-button>
      </div>

      <a-table
        :data-source="tours"
        :columns="columns"
        :loading="loading"
        row-key="id"
      >
        <template #bodyCell="{ column, record }">
          <template v-if="column.key === 'title'">
            <div class="tour-cell">
              <img :src="record.coverImageUrl" class="tour-img" />
              <div>
                <strong>{{ record.title }}</strong>
                <div class="tour-meta-sub">{{ record.locationName }} • {{ record.categoryName }}</div>
              </div>
            </div>
          </template>

          <template v-else-if="column.key === 'host'">
            <span>{{ record.hostName }}</span>
          </template>

          <template v-else-if="column.key === 'basePrice'">
            <strong style="color: #1B4332;">{{ formatPrice(record.basePrice) }}đ</strong>
          </template>

          <template v-else-if="column.key === 'status'">
            <a-tag :color="getStatusColor(record.status)">
              {{ translateStatus(record.status) }}
            </a-tag>
          </template>

          <template v-else-if="column.key === 'actions'">
            <div class="action-btns">
              <a-button
                v-if="record.status === 'PendingApproval' || record.status === 1 || record.status === 'Rejected'"
                type="primary"
                size="small"
                @click="handleApprove(record.id)"
              >
                Phê duyệt
              </a-button>

              <a-button
                v-if="record.status === 'Published' || record.status === 2"
                danger
                size="small"
                @click="handleReject(record.id)"
              >
                Tạm dừng / Ẩn
              </a-button>

              <a-button
                v-if="record.status === 'PendingApproval' || record.status === 1"
                danger
                size="small"
                @click="handleReject(record.id)"
              >
                Từ chối
              </a-button>
            </div>
          </template>
        </template>
      </a-table>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { message } from 'ant-design-vue'
import apiClient from '@/services/api'
import type { TourSummary, PagedResponse } from '@/types'

const loading = ref(false)
const tours = ref<TourSummary[]>([])

const columns = [
  { title: 'Trải nghiệm', key: 'title' },
  { title: 'Người đăng (Host)', key: 'host' },
  { title: 'Đơn giá', key: 'basePrice' },
  { title: 'Thời lượng', dataIndex: 'durationHours', key: 'durationHours' },
  { title: 'Trạng thái', key: 'status' },
  { title: 'Hành động', key: 'actions' }
]

function formatPrice(val: number) {
  return new Intl.NumberFormat('vi-VN').format(val)
}

function getStatusColor(status: any) {
  if (status === 'Published' || status === 2) return 'green'
  if (status === 'PendingApproval' || status === 1) return 'gold'
  if (status === 'Rejected' || status === 3) return 'red'
  return 'default'
}

function translateStatus(status: any) {
  if (status === 'Published' || status === 2) return 'Đã duyệt'
  if (status === 'PendingApproval' || status === 1) return 'Chờ duyệt'
  if (status === 'Rejected' || status === 3) return 'Đã từ chối'
  return 'Nháp'
}

async function loadTours() {
  loading.value = true
  try {
    const res = await apiClient.get<PagedResponse<TourSummary>>('/tours?pageSize=50')
    tours.value = res.data.items || []
  } catch (error) {
    console.error('Failed to load tours for admin', error)
  } finally {
    loading.value = false
  }
}

async function handleApprove(tourId: number) {
  try {
    const res = await apiClient.post(`/tours/admin/${tourId}/approve`)
    if (res.data.success) {
      message.success('Đã phê duyệt và xuất bản tour thành công!')
      loadTours()
    } else {
      message.error(res.data.message)
    }
  } catch (error: any) {
    message.error(error.response?.data?.message || 'Lỗi khi duyệt tour')
  }
}

async function handleReject(tourId: number) {
  const reason = prompt('Nhập lý do từ chối hoặc tạm dừng tour:')
  if (reason === null) return

  try {
    const res = await apiClient.post(`/tours/admin/${tourId}/reject`, JSON.stringify(reason || 'Nội dung chưa đạt tiêu chuẩn'), {
      headers: { 'Content-Type': 'application/json' }
    })
    if (res.data.success) {
      message.success('Đã từ chối tour.')
      loadTours()
    } else {
      message.error(res.data.message)
    }
  } catch (error: any) {
    message.error(error.response?.data?.message || 'Lỗi khi từ chối tour')
  }
}

onMounted(() => {
  loadTours()
})
</script>

<style scoped>
.card-box {
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-md);
  padding: 24px;
  box-shadow: var(--shadow-sm);
}

.header-line {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 20px;
}

.header-line h3 {
  font-size: 18px;
  font-weight: 800;
  color: var(--text-primary);
  margin-bottom: 4px;
}

.subtitle {
  font-size: 13px;
  color: var(--text-secondary);
}

.tour-cell {
  display: flex;
  align-items: center;
  gap: 12px;
}

.tour-img {
  width: 50px;
  height: 40px;
  object-fit: cover;
  border-radius: 4px;
}

.tour-meta-sub {
  font-size: 11px;
  color: var(--text-muted);
}

.action-btns {
  display: flex;
  gap: 8px;
}
</style>
