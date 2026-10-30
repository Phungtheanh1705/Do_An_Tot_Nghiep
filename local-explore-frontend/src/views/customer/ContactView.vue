<template>
  <div class="contact-page">
    <!-- Hero Banner -->
    <section class="contact-hero">
      <div class="container hero-content">
        <a-breadcrumb class="hero-breadcrumb">
          <a-breadcrumb-item><router-link to="/">Trang chủ</router-link></a-breadcrumb-item>
          <a-breadcrumb-item>Liên hệ & Hỗ trợ</a-breadcrumb-item>
        </a-breadcrumb>

        <span class="hero-badge">TRUNG TÂM TRỢ GIÚP</span>
        <h1 class="hero-title">Chúng tôi luôn sẵn sàng lắng nghe bạn</h1>
        <p class="hero-subtitle">
          Cần hỗ trợ về lịch trình, thanh toán đơn vé, xuất trình mã QR hay đăng ký trở thành Host? 
          Đội ngũ chăm sóc khách hàng LocalExplore luôn túc trực hỗ trợ 24/7.
        </p>
      </div>
    </section>

    <!-- Contact Cards Grid -->
    <section class="container contact-body">
      <div class="channels-grid">
        <div class="channel-card">
          <div class="channel-icon bg-green">📞</div>
          <h3>Tổng đài Hotline</h3>
          <p>Hỗ trợ đặt chỗ & giải đáp thông tin</p>
          <a href="tel:19008866" class="channel-val">1900 8866</a>
          <span class="channel-time">8:00 - 21:00 hàng ngày</span>
        </div>

        <div class="channel-card">
          <div class="channel-icon bg-blue">✉️</div>
          <h3>Email Hỗ trợ</h3>
          <p>Phản hồi trong vòng 2 giờ làm việc</p>
          <a href="mailto:support@localexplore.vn" class="channel-val">support@localexplore.vn</a>
          <span class="channel-time">Tiếp nhận 24/7</span>
        </div>

        <div class="channel-card">
          <div class="channel-icon bg-amber">📍</div>
          <h3>Văn phòng điều hành</h3>
          <p>Tòa nhà Heritage Tower</p>
          <span class="channel-val text-dark">18 Phố Cổ, Hoàn Kiếm, Hà Nội</span>
          <span class="channel-time">Thứ 2 - Thứ 6 (8:30 - 17:30)</span>
        </div>

        <div class="channel-card">
          <div class="channel-icon bg-purple">🆘</div>
          <h3>Hỗ trợ khẩn cấp tại điểm tour</h3>
          <p>Dành riêng cho khách đang đi tour</p>
          <a href="tel:0988123456" class="channel-val">0988 123 456</a>
          <span class="channel-time">Trực hotline 24/7</span>
        </div>
      </div>

      <!-- Contact Form & FAQ Layout -->
      <div class="form-faq-layout">
        <!-- Form Area -->
        <div class="form-card glass-panel">
          <h2>Gửi tin nhắn hoặc yêu cầu hỗ trợ</h2>
          <p class="form-desc">Vui lòng điền thông tin bên dưới, nhân viên phụ trách sẽ liên hệ hỗ trợ bạn sớm nhất.</p>

          <a-form layout="vertical" @submit.prevent="handleSubmit">
            <div class="form-row-2">
              <a-form-item label="Họ và tên" required>
                <a-input v-model:value="form.fullName" placeholder="Nguyễn Văn A" size="large" />
              </a-form-item>
              <a-form-item label="Số điện thoại" required>
                <a-input v-model:value="form.phoneNumber" placeholder="0912 345 678" size="large" />
              </a-form-item>
            </div>

            <div class="form-row-2">
              <a-form-item label="Địa chỉ Email" required>
                <a-input v-model:value="form.email" placeholder="email@example.com" size="large" />
              </a-form-item>
              <a-form-item label="Vấn đề cần hỗ trợ" required>
                <a-select v-model:value="form.topic" size="large" placeholder="Chọn chủ đề">
                  <a-select-option value="booking">Hỏi về đơn đặt tour & Lịch khởi hành</a-select-option>
                  <a-select-option value="payment">Thanh toán & Hóa đơn điện tử</a-select-option>
                  <a-select-option value="refund">Chính sách hủy vé & Hoàn tiền</a-select-option>
                  <a-select-option value="host">Tư vấn trở thành Local Host</a-select-option>
                  <a-select-option value="feedback">Góp ý cải thiện dịch vụ</a-select-option>
                </a-select>
              </a-form-item>
            </div>

            <a-form-item label="Nội dung chi tiết" required>
              <a-textarea
                v-model:value="form.message"
                placeholder="Mô tả cụ thể mã đơn đặt (nếu có) hoặc câu hỏi của bạn..."
                :rows="4"
              />
            </a-form-item>

            <a-button
              type="primary"
              size="large"
              :loading="submitting"
              html-type="submit"
              class="btn-submit-contact"
            >
              Gửi yêu cầu hỗ trợ ngay
            </a-button>
          </a-form>
        </div>

        <!-- FAQ Sidebar Area -->
        <div class="faq-sidebar">
          <h3>Câu hỏi phổ biến</h3>
          <div class="faq-list">
            <div class="faq-mini-item">
              <h4>Tôi có thể hủy vé trước giờ khởi hành bao lâu?</h4>
              <p>Chính sách chuẩn của LocalExplore cho phép hủy trước 72 giờ hoàn 100% tiền vé, từ 24 - 72 giờ hoàn 50%, và dưới 24 giờ không hoàn tiền.</p>
            </div>

            <div class="faq-mini-item">
              <h4>Vé điện tử và mã QR xuất trình ở đâu?</h4>
              <p>Mã QR check-in được gửi qua email ngay sau khi thanh toán thành công và luôn sẵn sàng trong mục <strong>"Vé & Chuyến đi của tôi"</strong> trên website.</p>
            </div>

            <div class="faq-mini-item">
              <h4>Nếu thời tiết xấu tour có diễn ra không?</h4>
              <p>Trong trường hợp bất khả kháng do mưa bão, Host sẽ chủ động liên hệ để dời lịch khởi hành hoặc hệ thống sẽ hoàn 100% tiền vé cho du khách.</p>
            </div>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue'
import { message } from 'ant-design-vue'

const submitting = ref(false)
const form = reactive({
  fullName: '',
  phoneNumber: '',
  email: '',
  topic: 'booking',
  message: ''
})

function handleSubmit() {
  if (!form.fullName || !form.phoneNumber || !form.email || !form.message) {
    message.warning('Vui lòng điền đầy đủ các trường thông tin bắt buộc.')
    return
  }

  submitting.value = true
  setTimeout(() => {
    submitting.value = false
    message.success('Cảm ơn bạn! Yêu cầu hỗ trợ đã được gửi thành công. Chúng tôi sẽ phản hồi trong thời gian sớm nhất.')
    form.fullName = ''
    form.phoneNumber = ''
    form.email = ''
    form.message = ''
  }, 1000)
}
</script>

<style scoped>
.contact-page {
  background-color: #fafbfc;
  min-height: 100vh;
  padding-bottom: 80px;
}

/* Hero */
.contact-hero {
  background: linear-gradient(135deg, #1B4332 0%, #2D6A4F 60%, #153e2d 100%);
  color: #fff;
  padding: 48px 0 60px;
}

.hero-breadcrumb {
  margin-bottom: 20px;
}

.hero-breadcrumb :deep(.ant-breadcrumb-link),
.hero-breadcrumb :deep(.ant-breadcrumb-separator) {
  color: rgba(255, 255, 255, 0.7);
  font-size: 13px;
}

.hero-breadcrumb :deep(a) {
  color: rgba(255, 255, 255, 0.85);
}

.hero-badge {
  display: inline-block;
  background: rgba(255, 255, 255, 0.15);
  backdrop-filter: blur(8px);
  color: #a7f3d0;
  font-size: 11px;
  font-weight: 700;
  letter-spacing: 1.5px;
  padding: 4px 14px;
  border-radius: 20px;
  margin-bottom: 14px;
}

.hero-title {
  font-size: 38px;
  font-weight: 800;
  color: #fff;
  line-height: 1.25;
  margin-bottom: 12px;
}

.hero-subtitle {
  font-size: 16px;
  color: rgba(255, 255, 255, 0.85);
  max-width: 680px;
  line-height: 1.6;
}

/* Body */
.contact-body {
  padding-top: 48px;
}

/* Channels Grid */
.channels-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
  margin-bottom: 48px;
}

@media (max-width: 900px) {
  .channels-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 500px) {
  .channels-grid {
    grid-template-columns: 1fr;
  }
}

.channel-card {
  background: #fff;
  border: 1px solid #e2e8f0;
  border-radius: 16px;
  padding: 28px 20px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.04);
  display: flex;
  flex-direction: column;
  transition: transform 0.2s;
}

.channel-card:hover {
  transform: translateY(-4px);
}

.channel-icon {
  width: 48px;
  height: 48px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 22px;
  margin-bottom: 14px;
}

.channel-icon.bg-green { background: #DCFCE7; }
.channel-icon.bg-blue { background: #E0E7FF; }
.channel-icon.bg-amber { background: #FEF3C7; }
.channel-icon.bg-purple { background: #F3E8FF; }

.channel-card h3 {
  font-size: 16px;
  font-weight: 800;
  color: #1e293b;
  margin-bottom: 4px;
}

.channel-card p {
  font-size: 13px;
  color: #64748b;
  margin-bottom: 12px;
}

.channel-val {
  font-size: 15px;
  font-weight: 700;
  color: #1B4332;
  text-decoration: none;
  margin-bottom: 4px;
}

.channel-val.text-dark {
  color: #1e293b;
}

.channel-time {
  font-size: 12px;
  color: #94a3b8;
}

/* Form and FAQ */
.form-faq-layout {
  display: grid;
  grid-template-columns: 1.4fr 0.8fr;
  gap: 36px;
}

@media (max-width: 900px) {
  .form-faq-layout {
    grid-template-columns: 1fr;
  }
}

.form-card {
  background: #fff;
  border: 1px solid #e2e8f0;
  border-radius: 20px;
  padding: 40px;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.05);
}

.form-card h2 {
  font-size: 24px;
  font-weight: 800;
  color: #1e293b;
  margin-bottom: 6px;
}

.form-desc {
  font-size: 14px;
  color: #64748b;
  margin-bottom: 28px;
}

.form-row-2 {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 16px;
}

@media (max-width: 600px) {
  .form-row-2 {
    grid-template-columns: 1fr;
  }
}

.btn-submit-contact {
  background: #1B4332 !important;
  border-color: #1B4332 !important;
  font-weight: 700;
  height: 46px;
  border-radius: 8px;
  width: 100%;
}

.btn-submit-contact:hover {
  background: #2D6A4F !important;
}

.faq-sidebar {
  background: #f8fafc;
  border: 1px solid #e2e8f0;
  border-radius: 20px;
  padding: 32px 28px;
  height: fit-content;
}

.faq-sidebar h3 {
  font-size: 18px;
  font-weight: 800;
  color: #1e293b;
  margin-bottom: 20px;
}

.faq-list {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.faq-mini-item h4 {
  font-size: 14px;
  font-weight: 700;
  color: #1B4332;
  margin-bottom: 6px;
}

.faq-mini-item p {
  font-size: 13px;
  line-height: 1.6;
  color: #64748b;
  margin: 0;
}
</style>
