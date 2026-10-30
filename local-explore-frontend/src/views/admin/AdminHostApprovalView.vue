<template>
  <div class="host-approval-page">
    <div class="card-box">
      <div class="header-line">
        <div>
          <h3>Quản lý & Thẩm định Hồ sơ KYC Local Host</h3>
          <p class="subtitle">Kiểm tra thông tin định danh CCCD, năng lực trải nghiệm và tài khoản ngân hàng của Host trước khi cấp quyền tạo tour</p>
        </div>
        <div class="header-actions">
          <a-radio-group v-model:value="filterStatus" button-style="solid" size="middle" @change="loadHosts">
            <a-radio-button value="all">Tất cả ({{ hosts.length }})</a-radio-button>
            <a-radio-button value="pending">Chờ phê duyệt ({{ pendingCount }})</a-radio-button>
            <a-radio-button value="approved">Đã kích hoạt ({{ approvedCount }})</a-radio-button>
          </a-radio-group>
          <a-button type="default" @click="loadHosts">
            <ReloadOutlined /> Làm mới
          </a-button>
        </div>
      </div>

      <a-table
        :data-source="filteredHosts"
        :columns="columns"
        :loading="loading"
        row-key="id"
        :pagination="{ pageSize: 10 }"
      >
        <template #bodyCell="{ column, record }">
          <!-- Host info -->
          <template v-if="column.key === 'hostInfo'">
            <div class="host-cell">
              <a-avatar :src="record.avatarUrl" :size="40" style="background-color: #008080;">
                {{ record.fullName.charAt(0) }}
              </a-avatar>
              <div class="host-detail">
                <strong class="host-name">{{ record.fullName }}</strong>
                <span class="host-email">{{ record.email }}</span>
                <span v-if="record.phoneNumber" class="host-phone">📞 {{ record.phoneNumber }}</span>
              </div>
            </div>
          </template>

          <!-- Citizen ID -->
          <template v-else-if="column.key === 'identityCardNumber'">
            <div class="id-cell">
              <IdcardOutlined />
              <span class="id-number">{{ record.identityCardNumber }}</span>
            </div>
          </template>

          <!-- Experience -->
          <template v-else-if="column.key === 'experienceYears'">
            <a-tag color="cyan">{{ record.experienceYears }} năm kinh nghiệm</a-tag>
          </template>

          <!-- Bank Account -->
          <template v-else-if="column.key === 'bank'">
            <div class="bank-cell">
              <div class="bank-name">{{ record.bankName }}</div>
              <div class="bank-acc">{{ record.bankAccountNumber }}</div>
              <small class="bank-holder">{{ record.bankAccountHolder }}</small>
            </div>
          </template>

          <!-- Status -->
          <template v-else-if="column.key === 'status'">
            <a-tag v-if="record.isApproved" color="success">
              <CheckCircleOutlined /> Đã phê duyệt KYC
            </a-tag>
            <a-tag v-else-if="record.rejectionReason" color="error">
              <CloseCircleOutlined /> Bị từ chối
            </a-tag>
            <a-tag v-else color="warning">
              <ClockCircleOutlined /> Chờ kiểm duyệt
            </a-tag>
          </template>

          <!-- Actions -->
          <template v-else-if="column.key === 'actions'">
            <div class="action-btns">
              <a-button size="small" @click="viewHostDetail(record)">
                <EyeOutlined /> Chi tiết
              </a-button>

              <a-popconfirm
                v-if="!record.isApproved"
                title="Xác nhận phê duyệt cấp quyền Local Host cho người này?"
                ok-text="Duyệt hồ sơ"
                cancel-text="Hủy"
                @confirm="handleApprove(record.id)"
              >
                <a-button type="primary" size="small">
                  <CheckOutlined /> Duyệt
                </a-button>
              </a-popconfirm>

              <a-button
                v-if="!record.isApproved && !record.rejectionReason"
                danger
                size="small"
                @click="openRejectModal(record)"
              >
                <CloseOutlined /> Từ chối
              </a-button>
            </div>
          </template>
        </template>
      </a-table>
    </div>

    <!-- Modal Xem chi tiết hồ sơ KYC -->
    <a-modal
      v-model:open="detailModalVisible"
      title="Hồ sơ Định danh Local Host (KYC)"
      :footer="null"
      width="640px"
    >
      <div v-if="selectedHost" class="kyc-modal-content">
        <div class="kyc-header">
          <a-avatar :src="selectedHost.avatarUrl" :size="64" style="background-color: #008080;">
            {{ selectedHost.fullName.charAt(0) }}
          </a-avatar>
          <div class="kyc-header-info">
            <h3>{{ selectedHost.fullName }}</h3>
            <p>{{ selectedHost.email }} • {{ selectedHost.phoneNumber || 'Chưa cập nhật SĐT' }}</p>
            <a-tag :color="selectedHost.isApproved ? 'success' : 'warning'">
              {{ selectedHost.isApproved ? 'Đã kích hoạt tài khoản Host' : 'Đang chờ thẩm định' }}
            </a-tag>
          </div>
        </div>

        <a-descriptions bordered :column="1" size="small" style="margin-top: 20px;">
          <a-descriptions-item label="Số CCCD / Hộ chiếu">
            <strong>{{ selectedHost.identityCardNumber }}</strong>
          </a-descriptions-item>
          <a-descriptions-item label="Kinh nghiệm bản địa">
            {{ selectedHost.experienceYears }} năm dẫn tour / trải nghiệm làng nghề
          </a-descriptions-item>
          <a-descriptions-item label="Ngân hàng thụ hưởng">
            {{ selectedHost.bankName }}
          </a-descriptions-item>
          <a-descriptions-item label="Số tài khoản">
            {{ selectedHost.bankAccountNumber }}
          </a-descriptions-item>
          <a-descriptions-item label="Chủ tài khoản">
            {{ selectedHost.bankAccountHolder }}
          </a-descriptions-item>
          <a-descriptions-item label="Giới thiệu bản thân (Bio)">
            {{ selectedHost.bio || 'Chưa có thông tin mô tả chi tiết.' }}
          </a-descriptions-item>
          <a-descriptions-item v-if="selectedHost.rejectionReason" label="Lý do từ chối trước đó">
            <span style="color: #ef4444;">{{ selectedHost.rejectionReason }}</span>
          </a-descriptions-item>
        </a-descriptions>

        <div v-if="!selectedHost.isApproved" class="modal-footer-actions">
          <a-button danger @click="openRejectModal(selectedHost)">Từ chối hồ sơ</a-button>
          <a-button type="primary" @click="handleApprove(selectedHost.id)">Phê duyệt ngay</a-button>
        </div>
      </div>
    </a-modal>

    <!-- Modal Từ chối hồ sơ -->
    <a-modal
      v-model:open="rejectModalVisible"
      title="Từ chối hồ sơ KYC Host"
      ok-text="Xác nhận từ chối"
      cancel-text="Hủy"
      ok-type="danger"
      @ok="handleRejectSubmit"
    >
      <p>Vui lòng cung cấp lý do từ chối để thông báo cho ứng viên hoàn thiện lại hồ sơ:</p>
      <a-textarea
        v-model:value="rejectReason"
        :rows="4"
        placeholder="Ví dụ: Ảnh CCCD bị mờ, thông tin số tài khoản ngân hàng không khớp với họ tên..."
      />
    </a-modal>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { message } from 'ant-design-vue'
import {
  ReloadOutlined,
  CheckCircleOutlined,
  CloseCircleOutlined,
  ClockCircleOutlined,
  IdcardOutlined,
  EyeOutlined,
  CheckOutlined,
  CloseOutlined
} from '@ant-design/icons-vue'
import apiClient from '@/services/api'
import type { HostProfileSummary, ApiResponse } from '@/types'

const loading = ref(false)
const hosts = ref<HostProfileSummary[]>([])
const filterStatus = ref<'all' | 'pending' | 'approved'>('all')

const detailModalVisible = ref(false)
const selectedHost = ref<HostProfileSummary | null>(null)

const rejectModalVisible = ref(false)
const rejectingHostId = ref<number | null>(null)
const rejectReason = ref('')

const columns = [
  { title: 'Local Host', key: 'hostInfo' },
  { title: 'CCCD / Định danh', key: 'identityCardNumber', width: 170 },
  { title: 'Kinh nghiệm', key: 'experienceYears', width: 160 },
  { title: 'Tài khoản chi trả', key: 'bank', width: 220 },
  { title: 'Trạng thái KYC', key: 'status', width: 170 },
  { title: 'Thao tác', key: 'actions', width: 180 }
]

const pendingCount = computed(() => hosts.value.filter(h => !h.isApproved && !h.rejectionReason).length)
const approvedCount = computed(() => hosts.value.filter(h => h.isApproved).length)

const filteredHosts = computed(() => {
  if (filterStatus.value === 'pending') {
    return hosts.value.filter(h => !h.isApproved && !h.rejectionReason)
  }
  if (filterStatus.value === 'approved') {
    return hosts.value.filter(h => h.isApproved)
  }
  return hosts.value
})

async function loadHosts() {
  loading.value = true
  try {
    const res = await apiClient.get<ApiResponse<HostProfileSummary[]>>('/admin/hosts')
    if (res.data.success) {
      hosts.value = res.data.data || []
    }
  } catch (error) {
    message.error('Không thể tải danh sách hồ sơ Host.')
    console.error(error)
  } finally {
    loading.value = false
  }
}

function viewHostDetail(host: HostProfileSummary) {
  selectedHost.value = host
  detailModalVisible.value = true
}

async function handleApprove(hostId: number) {
  try {
    const res = await apiClient.post<ApiResponse<boolean>>(`/admin/hosts/${hostId}/approve`)
    if (res.data.success) {
      message.success('Đã phê duyệt hồ sơ KYC Host thành công!')
      detailModalVisible.value = false
      await loadHosts()
    } else {
      message.error(res.data.message)
    }
  } catch (error: any) {
    message.error(error.response?.data?.message || 'Lỗi khi phê duyệt hồ sơ Host.')
  }
}

function openRejectModal(host: HostProfileSummary) {
  rejectingHostId.value = host.id
  rejectReason.value = ''
  rejectModalVisible.value = true
}

async function handleRejectSubmit() {
  if (!rejectReason.value.trim()) {
    message.warning('Vui lòng nhập lý do từ chối.')
    return
  }

  try {
    const res = await apiClient.post<ApiResponse<boolean>>(`/admin/hosts/${rejectingHostId.value}/reject`, {
      reason: rejectReason.value.trim()
    })
    if (res.data.success) {
      message.success('Đã gửi thông báo từ chối hồ sơ Host.')
      rejectModalVisible.value = false
      detailModalVisible.value = false
      await loadHosts()
    } else {
      message.error(res.data.message)
    }
  } catch (error: any) {
    message.error(error.response?.data?.message || 'Lỗi khi từ chối hồ sơ.')
  }
}

onMounted(() => {
  loadHosts()
})
</script>

<style scoped>
.host-approval-page {
  padding: 10px 0;
}

.card-box {
  background: #fff;
  border-radius: var(--radius-md);
  padding: 24px;
  border: 1px solid var(--border-color);
  box-shadow: var(--shadow-sm);
}

.header-line {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
  padding-bottom: 16px;
  border-bottom: 1px solid var(--border-color);
}

.header-line h3 {
  font-size: 18px;
  font-weight: 700;
  margin-bottom: 4px;
  color: var(--text-primary);
}

.subtitle {
  color: var(--text-secondary);
  font-size: 13px;
  margin: 0;
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 12px;
}

.host-cell {
  display: flex;
  align-items: center;
  gap: 12px;
}

.host-detail {
  display: flex;
  flex-direction: column;
}

.host-name {
  font-size: 14px;
  color: var(--text-primary);
}

.host-email {
  font-size: 12px;
  color: var(--text-muted);
}

.host-phone {
  font-size: 11px;
  color: var(--text-secondary);
}

.id-cell {
  display: flex;
  align-items: center;
  gap: 6px;
  font-family: monospace;
  font-size: 13px;
  font-weight: 600;
  color: #334155;
}

.bank-cell {
  display: flex;
  flex-direction: column;
  font-size: 12px;
}

.bank-name {
  font-weight: 700;
  color: #008080;
}

.bank-acc {
  font-family: monospace;
  font-weight: 600;
}

.bank-holder {
  color: var(--text-muted);
  text-transform: uppercase;
}

.action-btns {
  display: flex;
  gap: 6px;
  flex-wrap: wrap;
}

.kyc-header {
  display: flex;
  align-items: center;
  gap: 16px;
  padding-bottom: 16px;
  border-bottom: 1px solid var(--border-color);
}

.kyc-header-info h3 {
  margin: 0 0 4px;
  font-size: 18px;
  font-weight: 700;
}

.kyc-header-info p {
  margin: 0 0 6px;
  color: var(--text-secondary);
  font-size: 13px;
}

.modal-footer-actions {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  margin-top: 24px;
  padding-top: 16px;
  border-top: 1px solid var(--border-color);
}
</style>
