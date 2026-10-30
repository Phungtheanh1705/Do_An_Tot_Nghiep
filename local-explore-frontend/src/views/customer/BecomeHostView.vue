<template>
  <div class="become-host-page">
    <!-- Hero Banner -->
    <section class="host-hero">
      <div class="container hero-content">
        <a-breadcrumb class="hero-breadcrumb">
          <a-breadcrumb-item><router-link to="/">Trang chủ</router-link></a-breadcrumb-item>
          <a-breadcrumb-item>Trở thành Host</a-breadcrumb-item>
        </a-breadcrumb>

        <span class="hero-badge">CỘNG ĐỒNG NGƯỜI BẢN ĐỊA</span>
        <h1 class="hero-title">Biến niềm tự hào quê hương thành nguồn thu nhập bền vững</h1>
        <p class="hero-subtitle">
          Bạn là nghệ nhân, người am hiểu văn hóa hay người dân bản xứ yêu mến vùng đất của mình? 
          Hãy gia nhập LocalExplore để mở cửa đón du khách và chia sẻ những câu chuyện chân thực nhất.
        </p>

        <div class="hero-cta-row">
          <a-button type="primary" size="large" class="btn-hero-cta" @click="handleHostAction">
            {{ hostActionLabel }}
          </a-button>
          <a href="#how-it-works" class="btn-hero-secondary">Tìm hiểu quy trình ↓</a>
        </div>
      </div>
    </section>

    <!-- Why Host With LocalExplore (3 Value Props) -->
    <section class="container value-section">
      <div class="section-title-center">
        <span class="sub-badge">QUYỀN LỢI CỦA HOST</span>
        <h2>Tại sao nên đồng hành cùng LocalExplore?</h2>
        <p>Chúng tôi cung cấp mọi công cụ công nghệ và giải pháp hỗ trợ để bạn yên tâm tập trung vào chuyên môn.</p>
      </div>

      <div class="values-grid">
        <div class="value-card">
          <div class="val-icon bg-amber">💰</div>
          <h3>Thu nhập xứng đáng & minh bạch</h3>
          <p>Tự định giá vé trải nghiệm và nhận 100% doanh thu sau phí sàn cố định chỉ 10%. Quyết toán nhanh chóng về tài khoản ngân hàng hàng tuần.</p>
        </div>

        <div class="value-card">
          <div class="val-icon bg-green">⏱️</div>
          <h3>Làm chủ thời gian hoàn toàn</h3>
          <p>Tự do mở lịch khởi hành theo thời gian rảnh của bạn. Dễ dàng giới hạn số lượng khách tối đa mỗi chuyến để đảm bảo chất lượng phục vụ tốt nhất.</p>
        </div>

        <div class="value-card">
          <div class="val-icon bg-blue">📱</div>
          <h3>Công nghệ quản lý thông minh</h3>
          <p>Trang bị riêng Host Portal: Theo dõi đơn đặt, danh sách họ tên khách, và ứng dụng quét QR Check-in tức thì tại điểm hẹn bằng camera điện thoại.</p>
        </div>
      </div>
    </section>

    <!-- Earnings Calculator Widget -->
    <section class="calc-section">
      <div class="container">
        <div class="calc-box glass-panel">
          <div class="calc-text">
            <span class="sub-badge">ƯỚC TÍNH THU NHẬP</span>
            <h2>Bạn có thể kiếm được bao nhiêu?</h2>
            <p>Thu nhập thực tế phụ thuộc vào loại hình trải nghiệm, quy mô nhóm và tần suất tổ chức chuyến đi của bạn.</p>

            <div class="calc-inputs">
              <div class="input-item">
                <label>Giá vé mỗi khách: <strong>{{ formatPrice(calcPrice) }}đ</strong></label>
                <a-slider v-model:value="calcPrice" :min="150000" :max="1500000" :step="50000" />
              </div>
              <div class="input-item">
                <label>Số lượng khách mỗi tháng: <strong>{{ calcGuests }} khách</strong></label>
                <a-slider v-model:value="calcGuests" :min="5" :max="100" :step="5" />
              </div>
            </div>
          </div>

          <div class="calc-result">
            <span class="res-label">Ước tính thu nhập / tháng</span>
            <div class="res-num">{{ formatPrice(estimatedMonthlyRevenue) }}đ</div>
            <span class="res-note">Dựa trên {{ calcGuests }} khách với giá {{ formatPrice(calcPrice) }}đ/người</span>
            <a-button type="primary" size="large" block class="btn-calc-cta" @click="handleHostAction">
              Bắt đầu tạo trải nghiệm ngay
            </a-button>
          </div>
        </div>
      </div>
    </section>

    <!-- How It Works (3 Steps) -->
    <section id="how-it-works" class="container steps-section">
      <div class="section-title-center">
        <span class="sub-badge">QUY TRÌNH THAM GIA</span>
        <h2>3 Bước đơn giản để trở thành Local Host</h2>
        <p>Quy trình minh bạch, hỗ trợ tối đa từ đội ngũ LocalExplore để bạn đăng tải tour đầu tiên thành công.</p>
      </div>

      <div class="host-steps-grid">
        <div class="step-card">
          <div class="step-num">01</div>
          <h3>Nộp hồ sơ xác minh KYC</h3>
          <p>Đăng ký tài khoản và gửi hình chụp CCCD/CMND cùng thông tin tài khoản ngân hàng để đảm bảo an toàn, uy tín cho sàn.</p>
        </div>

        <div class="step-card">
          <div class="step-num">02</div>
          <h3>Đăng tải Tour trải nghiệm</h3>
          <p>Điền lịch trình từng chặng, dịch vụ bao gồm, giá vé niêm yết và ảnh chụp thực tế. Ban quản trị sẽ hỗ trợ biên tập và duyệt trong 24 giờ.</p>
        </div>

        <div class="step-card">
          <div class="step-num">03</div>
          <h3>Đón khách & Quét QR check-in</h3>
          <p>Nhận thông báo khi có khách đặt chỗ, đón tiếp tại điểm hẹn, quét mã QR trên vé của khách và nhận thanh toán tự động.</p>
        </div>
      </div>
    </section>

    <!-- Host FAQ Section -->
    <section class="container faq-section">
      <div class="section-title-center">
        <span class="sub-badge">GIẢI ĐÁP THẮC MẮC</span>
        <h2>Câu hỏi thường gặp của Host</h2>
      </div>

      <div class="faq-accordion-box">
        <a-collapse v-model:activeKey="activeFaqKeys" ghost>
          <a-collapse-panel key="1" header="Tôi không biết ngoại ngữ có thể làm Host trên LocalExplore không?">
            <p>Hoàn toàn được! LocalExplore phục vụ lượng lớn khách du lịch trong nước yêu thích khám phá văn hóa. Với khách quốc tế, hệ thống hỗ trợ chỉ định hướng dẫn viên cộng tác hoặc công cụ dịch thuật tiện lợi.</p>
          </a-collapse-panel>
          <a-collapse-panel key="2" header="Chi phí đăng ký tham gia sàn là bao nhiêu?">
            <p>Đăng ký hồ sơ và đăng tải tour trên LocalExplore là <strong>hoàn toàn miễn phí 100%</strong>. Sàn chỉ thu một khoản phí hoa hồng công nghệ nhỏ (10%) trên mỗi đơn đặt tour hoàn thành thành công.</p>
          </a-collapse-panel>
          <a-collapse-panel key="3" header="Làm sao để biết khách đã thanh toán tiền vé?">
            <p>Mọi đơn đặt tour trên hệ thống đều phải được du khách thanh toán trước qua cổng trực tuyến. Khi khách đến điểm hẹn, bạn chỉ cần mở máy ảnh quét mã QR trên vé điện tử của khách để xác thực check-in an toàn tuyệt đối.</p>
          </a-collapse-panel>
          <a-collapse-panel key="4" header="Thời gian phê duyệt hồ sơ và tour mất bao lâu?">
            <p>Đội ngũ kiểm duyệt của LocalExplore làm việc hàng ngày. Hồ sơ KYC của bạn thường được thẩm định trong vòng 4 - 8 giờ làm việc, và nội dung tour mới được phản hồi trong vòng tối đa 24 giờ.</p>
          </a-collapse-panel>
        </a-collapse>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const calcPrice = ref(350000)
const calcGuests = ref(20)
const activeFaqKeys = ref(['1', '2'])

const estimatedMonthlyRevenue = computed(() => {
  return calcPrice.value * calcGuests.value * 0.9 // trừ 10% phí sàn
})

const hostActionLabel = computed(() => {
  if (authStore.isHost) return 'Vào Host Portal của bạn →'
  if (authStore.isAuthenticated) return 'Nộp hồ sơ Host (KYC) ngay'
  return 'Đăng ký trở thành Host'
})

function handleHostAction() {
  if (authStore.isHost) {
    router.push('/host')
  } else if (authStore.isAuthenticated) {
    router.push('/host') // Sẽ chuyển tiếp hoặc mở modal KYC
  } else {
    router.push({ path: '/register', query: { role: 'Host' } })
  }
}

function formatPrice(val: number) {
  return new Intl.NumberFormat('vi-VN').format(val)
}
</script>

<style scoped>
.become-host-page {
  background-color: #fafbfc;
  min-height: 100vh;
  padding-bottom: 80px;
}

/* Hero Section */
.host-hero {
  background: linear-gradient(135deg, #1B4332 0%, #2D6A4F 60%, #153e2d 100%);
  color: #fff;
  padding: 48px 0 70px;
  position: relative;
  overflow: hidden;
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
  font-size: 40px;
  font-weight: 800;
  color: #fff;
  line-height: 1.25;
  margin-bottom: 14px;
  max-width: 820px;
}

.hero-subtitle {
  font-size: 16px;
  color: rgba(255, 255, 255, 0.85);
  max-width: 700px;
  line-height: 1.6;
  margin-bottom: 32px;
}

.hero-cta-row {
  display: flex;
  align-items: center;
  gap: 16px;
  flex-wrap: wrap;
}

.btn-hero-cta {
  background: #D97706 !important;
  border-color: #D97706 !important;
  font-weight: 700;
  font-size: 15px;
  height: 48px;
  padding: 0 28px;
  border-radius: 10px;
}

.btn-hero-cta:hover {
  background: #b45309 !important;
  border-color: #b45309 !important;
}

.btn-hero-secondary {
  color: #fff;
  font-weight: 600;
  font-size: 14px;
  padding: 12px 20px;
  text-decoration: none;
  transition: opacity 0.2s;
}

.btn-hero-secondary:hover {
  opacity: 0.8;
}

/* Values Section */
.value-section {
  padding: 64px 20px;
}

.section-title-center {
  text-align: center;
  max-width: 640px;
  margin: 0 auto 48px;
}

.sub-badge {
  display: inline-block;
  color: #D97706;
  font-size: 11px;
  font-weight: 800;
  letter-spacing: 1.2px;
  margin-bottom: 8px;
}

.section-title-center h2 {
  font-size: 30px;
  font-weight: 800;
  color: #1e293b;
  margin: 6px 0 10px;
}

.section-title-center p {
  font-size: 15px;
  color: #64748b;
}

.values-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 28px;
}

@media (max-width: 800px) {
  .values-grid {
    grid-template-columns: 1fr;
  }
}

.value-card {
  background: #fff;
  border: 1px solid #e2e8f0;
  border-radius: 16px;
  padding: 36px 28px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.04);
  transition: transform 0.2s;
}

.value-card:hover {
  transform: translateY(-4px);
}

.val-icon {
  width: 56px;
  height: 56px;
  border-radius: 14px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 26px;
  margin-bottom: 20px;
}

.val-icon.bg-amber { background: #FEF3C7; }
.val-icon.bg-green { background: #DCFCE7; }
.val-icon.bg-blue { background: #E0E7FF; }

.value-card h3 {
  font-size: 18px;
  font-weight: 800;
  color: #1e293b;
  margin-bottom: 10px;
}

.value-card p {
  font-size: 14px;
  line-height: 1.6;
  color: #64748b;
  margin: 0;
}

/* Calculator Section */
.calc-section {
  padding: 20px 0 60px;
}

.calc-box {
  background: #fff;
  border: 1px solid #e2e8f0;
  border-radius: 20px;
  padding: 48px;
  display: grid;
  grid-template-columns: 1.2fr 0.8fr;
  gap: 48px;
  align-items: center;
  box-shadow: 0 12px 32px rgba(0, 0, 0, 0.06);
}

@media (max-width: 900px) {
  .calc-box {
    grid-template-columns: 1fr;
    padding: 32px;
  }
}

.calc-text h2 {
  font-size: 28px;
  font-weight: 800;
  color: #1e293b;
  margin-bottom: 8px;
}

.calc-text p {
  font-size: 14px;
  color: #64748b;
  margin-bottom: 28px;
}

.calc-inputs {
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.input-item label {
  display: flex;
  justify-content: space-between;
  font-size: 14px;
  font-weight: 600;
  color: #334155;
  margin-bottom: 8px;
}

.input-item label strong {
  color: #1B4332;
}

.calc-result {
  background: linear-gradient(135deg, #1B4332 0%, #2D6A4F 100%);
  color: #fff;
  border-radius: 16px;
  padding: 36px 32px;
  text-align: center;
  box-shadow: 0 8px 24px rgba(27, 67, 50, 0.25);
}

.res-label {
  font-size: 13px;
  font-weight: 600;
  color: #a7f3d0;
  text-transform: uppercase;
  letter-spacing: 1px;
}

.res-num {
  font-size: 36px;
  font-weight: 900;
  color: #fff;
  margin: 10px 0;
}

.res-note {
  display: block;
  font-size: 12px;
  color: rgba(255, 255, 255, 0.8);
  margin-bottom: 24px;
}

.btn-calc-cta {
  background: #D97706 !important;
  border-color: #D97706 !important;
  font-weight: 700;
  height: 44px;
  border-radius: 8px;
}

/* Steps Section */
.steps-section {
  padding: 40px 20px 64px;
}

.host-steps-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 28px;
}

@media (max-width: 800px) {
  .host-steps-grid {
    grid-template-columns: 1fr;
  }
}

.step-card {
  background: #fff;
  border: 1px solid #e2e8f0;
  border-radius: 16px;
  padding: 36px 28px;
  position: relative;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.04);
}

.step-num {
  font-size: 32px;
  font-weight: 900;
  color: #1B4332;
  margin-bottom: 14px;
  opacity: 0.85;
}

.step-card h3 {
  font-size: 18px;
  font-weight: 800;
  color: #1e293b;
  margin-bottom: 10px;
}

.step-card p {
  font-size: 14px;
  line-height: 1.6;
  color: #64748b;
  margin: 0;
}

/* FAQ Section */
.faq-section {
  padding: 20px 20px 40px;
}

.faq-accordion-box {
  background: #fff;
  border-radius: 16px;
  border: 1px solid #e2e8f0;
  padding: 24px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.04);
}

.faq-accordion-box :deep(.ant-collapse-header-text) {
  font-size: 15px;
  font-weight: 700;
  color: #1e293b;
}

.faq-accordion-box :deep(.ant-collapse-content-box p) {
  font-size: 14px;
  line-height: 1.7;
  color: #64748b;
  margin: 0;
}
</style>
