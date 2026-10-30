<template>
  <div class="admin-dashboard">
    <!-- Stat Cards -->
    <div class="stats-grid">
      <div class="stat-card">
        <div class="stat-icon bg-purple">
          <CrownOutlined />
        </div>
        <div class="stat-info">
          <span class="stat-title">Tổng doanh thu toàn sàn</span>
          <span class="stat-value">1.950.000đ</span>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon bg-green">
          <CompassOutlined />
        </div>
        <div class="stat-info">
          <span class="stat-title">Tour đang hoạt động</span>
          <span class="stat-value">3</span>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon bg-blue">
          <TeamOutlined />
        </div>
        <div class="stat-info">
          <span class="stat-title">Người dùng hệ thống</span>
          <span class="stat-value">7</span>
        </div>
      </div>

      <div class="stat-card">
        <div class="stat-icon bg-amber">
          <SafetyCertificateOutlined />
        </div>
        <div class="stat-info">
          <span class="stat-title">Local Host đã duyệt KYC</span>
          <span class="stat-value">3</span>
        </div>
      </div>
    </div>

    <!-- Quick Moderation Links -->
    <div class="quick-cards">
      <div class="action-card" @click="$router.push('/admin/tours/approval')">
        <div class="act-icon"><CheckSquareOutlined /></div>
        <div class="act-info">
          <h4>Kiểm duyệt Tour đăng tải</h4>
          <p>Xem xét nội dung các tour mới do Host gửi lên trước khi công khai</p>
        </div>
        <a-button type="primary">Truy cập ngay →</a-button>
      </div>

      <div class="action-card" @click="$router.push('/')">
        <div class="act-icon" style="background: #E0E7FF; color: #3730A3;"><EyeOutlined /></div>
        <div class="act-info">
          <h4>Xem sàn giao dịch thực tế</h4>
          <p>Truy cập giao diện của khách hàng để trải nghiệm đặt tour trực tiếp</p>
        </div>
        <a-button>Về trang chủ →</a-button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import {
  CrownOutlined,
  CompassOutlined,
  TeamOutlined,
  SafetyCertificateOutlined,
  CheckSquareOutlined,
  EyeOutlined
} from '@ant-design/icons-vue'
</script>

<style scoped>
.stats-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
  margin-bottom: 30px;
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
  width: 50px;
  height: 50px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 24px;
}

.bg-purple { background: #F3E8FF; color: #6B21A8; }
.bg-green { background: #DCFCE7; color: #166534; }
.bg-blue { background: #E0E7FF; color: #3730A3; }
.bg-amber { background: #FEF3C7; color: #92400E; }

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

.quick-cards {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 20px;
}

.action-card {
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-md);
  padding: 24px;
  display: flex;
  align-items: center;
  gap: 16px;
  cursor: pointer;
  transition: all 0.2s;
}

.action-card:hover {
  transform: translateY(-3px);
  box-shadow: var(--shadow-md);
}

.act-icon {
  width: 50px;
  height: 50px;
  background: #DCFCE7;
  color: #166534;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 24px;
}

.act-info {
  flex: 1;
}

.act-info h4 {
  font-size: 16px;
  font-weight: 700;
  margin-bottom: 4px;
}

.act-info p {
  font-size: 13px;
  color: var(--text-secondary);
}

@media (max-width: 900px) {
  .stats-grid {
    grid-template-columns: 1fr 1fr;
  }
  .quick-cards {
    grid-template-columns: 1fr;
  }
}
</style>
