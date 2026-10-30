<template>
  <div class="scanner-page">
    <div class="scanner-card glass-panel">
      <div class="scanner-head">
        <div class="icon-circle">
          <QrcodeOutlined class="icon-qr" />
        </div>
        <h2>Quét mã QR Check-in khách tại điểm đón</h2>
        <p>Hệ thống tự động kiểm tra tính hợp lệ của vé, số lượng khách và ngăn chặn việc quét lặp lại.</p>
      </div>

      <!-- Quick Demo Buttons for Thesis Presentation -->
      <div class="demo-box">
        <span class="demo-tag">⚡ Dành cho Thuyết trình Demo:</span>
        <div class="demo-row">
          <button
            type="button"
            class="demo-qr-btn"
            @click="qrInput = 'LE-QR-CHECKIN-DEMO-LINH-2026'"
          >
            🎫 Dán mã vé hợp lệ (Phạm Thùy Linh)
          </button>
          <button
            type="button"
            class="demo-qr-btn"
            @click="qrInput = 'LE-QR-PAST-001-AN-BATTRANG'"
          >
            ⚠️ Dán mã vé đã check-in trước đó
          </button>
        </div>
      </div>

      <!-- Input Scanner Area -->
      <div class="input-area">
        <label>Nhập chuỗi Token mã QR từ vé của khách:</label>
        <a-input-search
          v-model:value="qrInput"
          placeholder="Ví dụ: LE-QR-CHECKIN-DEMO-LINH-2026"
          size="large"
          enter-button="Xác thực & Check-in"
          :loading="loading"
          @search="handleCheckIn"
        />
      </div>

      <!-- Result Card -->
      <div v-if="result" class="result-box" :class="result.success ? 'result-success' : 'result-error'">
        <div class="result-icon">
          <CheckCircleFilled v-if="result.success" />
          <CloseCircleFilled v-else />
        </div>
        <div class="result-content">
          <h4>{{ result.message }}</h4>
          <div v-if="result.data?.customerName" class="guest-details">
            <div>Khách hàng: <strong>{{ result.data.customerName }}</strong></div>
            <div>Mã đơn: <strong>{{ result.data.bookingCode }}</strong></div>
            <div>Trải nghiệm: <strong>{{ result.data.tourTitle }}</strong></div>
            <div>Số khách: <strong>{{ result.data.totalParticipants }} người</strong></div>
            <div>Thời gian quét: <strong>{{ formatDateTime(result.data.checkedInAt) }}</strong></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { message } from 'ant-design-vue'
import {
  QrcodeOutlined,
  CheckCircleFilled,
  CloseCircleFilled
} from '@ant-design/icons-vue'
import apiClient from '@/services/api'
import type { CheckInResult, ApiResponse } from '@/types'

const qrInput = ref('')
const loading = ref(false)
const result = ref<ApiResponse<CheckInResult> | null>(null)

function formatDateTime(dtStr?: string) {
  if (!dtStr) return ''
  const d = new Date(dtStr)
  return `${d.getHours().toString().padStart(2, '0')}:${d.getMinutes().toString().padStart(2, '0')} ngày ${d.getDate()}/${d.getMonth() + 1}/${d.getFullYear()}`
}

async function handleCheckIn() {
  if (!qrInput.value.trim()) {
    message.warning('Vui lòng nhập hoặc quét mã QR.')
    return
  }

  loading.value = true
  result.value = null

  try {
    const res = await apiClient.post<ApiResponse<CheckInResult>>('/checkin/verify', {
      qrCodeToken: qrInput.value.trim(),
      deviceInfo: 'Host Web Camera Device'
    })

    result.value = res.data
    if (res.data.success) {
      message.success('Check-in thành công!')
    } else {
      message.error(res.data.message)
    }
  } catch (error: any) {
    result.value = {
      success: false,
      message: error.response?.data?.message || 'Không thể xác thực mã QR',
      data: {} as any,
      errors: null,
      timestamp: new Date().toISOString()
    }
    message.error(result.value.message)
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.scanner-page {
  display: flex;
  justify-content: center;
  padding: 20px 0;
}

.scanner-card {
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-lg);
  padding: 36px;
  max-width: 680px;
  width: 100%;
  box-shadow: var(--shadow-md);
}

.scanner-head {
  text-align: center;
  margin-bottom: 24px;
}

.icon-circle {
  width: 64px;
  height: 64px;
  background: #DCFCE7;
  color: #166534;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 16px;
}

.icon-qr {
  font-size: 32px;
}

.scanner-head h2 {
  font-size: 22px;
  font-weight: 800;
  margin-bottom: 6px;
}

.scanner-head p {
  font-size: 13px;
  color: var(--text-secondary);
}

.demo-box {
  background: #FFFBEB;
  border: 1px dashed #F59E0B;
  border-radius: var(--radius-sm);
  padding: 14px;
  margin-bottom: 24px;
}

.demo-tag {
  display: block;
  font-size: 11px;
  font-weight: 700;
  color: #B45309;
  margin-bottom: 8px;
}

.demo-row {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

.demo-qr-btn {
  background: #fff;
  border: 1px solid #FCD34D;
  padding: 6px 12px;
  border-radius: 6px;
  font-size: 12px;
  font-weight: 600;
  color: #92400E;
  cursor: pointer;
  transition: all 0.2s;
}

.demo-qr-btn:hover {
  background: #FEF3C7;
}

.input-area {
  margin-bottom: 24px;
}

.input-area label {
  display: block;
  font-size: 13px;
  font-weight: 700;
  color: var(--text-primary);
  margin-bottom: 8px;
}

.result-box {
  display: flex;
  gap: 16px;
  padding: 20px;
  border-radius: var(--radius-md);
  margin-top: 20px;
}

.result-success {
  background: #F0FDF4;
  border: 1px solid #86EFAC;
  color: #166534;
}

.result-error {
  background: #FEF2F2;
  border: 1px solid #FECACA;
  color: #991B1B;
}

.result-icon {
  font-size: 28px;
}

.result-content h4 {
  font-size: 16px;
  font-weight: 800;
  margin-bottom: 8px;
  color: inherit;
}

.guest-details {
  display: flex;
  flex-direction: column;
  gap: 4px;
  font-size: 13px;
}
</style>
