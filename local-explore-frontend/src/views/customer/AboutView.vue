<template>
  <div class="about-page">
    <!-- Hero Banner -->
    <section class="about-hero">
      <div class="container hero-content">
        <a-breadcrumb class="hero-breadcrumb">
          <a-breadcrumb-item><router-link to="/">Trang chủ</router-link></a-breadcrumb-item>
          <a-breadcrumb-item>Về chúng tôi</a-breadcrumb-item>
        </a-breadcrumb>

        <span class="hero-badge">SỨ MỆNH & TẦM NHÌN</span>
        <h1 class="hero-title">Kết nối du khách với tâm hồn văn hóa Việt</h1>
        <p class="hero-subtitle">
          LocalExplore là nền tảng tiên phong kết nối du khách với những người dân bản địa tâm huyết, 
          nghệ nhân làng nghề cổ truyền nhằm bảo tồn và phát triển du lịch di sản bền vững.
        </p>
      </div>
    </section>

    <!-- Stats Bar -->
    <section class="stats-bar-section">
      <div class="container stats-bar-container">
        <div class="stat-col">
          <span class="stat-num">50+</span>
          <span class="stat-lbl">Làng nghề & Bản làng kết nối</span>
        </div>
        <div class="stat-col">
          <span class="stat-num">100%</span>
          <span class="stat-lbl">Host là người dân bản xứ</span>
        </div>
        <div class="stat-col">
          <span class="stat-num">10.000+</span>
          <span class="stat-lbl">Du khách tham gia trải nghiệm</span>
        </div>
        <div class="stat-col">
          <span class="stat-num">98.6%</span>
          <span class="stat-lbl">Đánh giá 5 sao tích cực</span>
        </div>
      </div>
    </section>

    <!-- Story & Mission Section -->
    <section class="container mission-section">
      <div class="mission-grid">
        <div class="mission-text">
          <span class="sub-badge">CÂU CHUYỆN KHỞI NGUỒN</span>
          <h2>Vì sao LocalExplore ra đời?</h2>
          <p class="lead-text">
            Việt Nam sở hữu hàng ngàn năm bề dày văn hóa với hơn 5.400 làng nghề truyền thống và 54 dân tộc anh em.
            Tuy nhiên, trong làn sóng du lịch đại trà, nhiều giá trị cổ truyền đang dần mai một, và người dân bản địa – những người giữ hồn di sản – thường ít nhận được giá trị kinh tế tương xứng.
          </p>
          <p class="body-text">
            LocalExplore được xây dựng với sứ mệnh: <strong>Trao quyền công nghệ cho người dân địa phương</strong>. 
            Chúng tôi giúp các nghệ nhân làm gốm Bát Tràng, người dệt lanh Sa Pa, nông dân làng rau Trà Quế hay người chèo đò miền Tây trực tiếp giới thiệu nét đẹp quê hương đến du khách trên toàn thế giới mà không cần qua các khâu trung gian phức tạp.
          </p>
        </div>

        <div class="mission-media">
          <div class="img-wrapper main-img">
            <img src="https://images.unsplash.com/photo-1565193566173-7a0ee3dbe261?w=800" alt="Làng nghề Việt Nam" />
          </div>
          <div class="img-wrapper floating-card">
            <div class="float-quote">
              <span class="quote-mark">“</span>
              <p>Mỗi tour không đơn thuần là một chuyến đi, mà là một cuộc gặp gỡ giữa những tâm hồn yêu văn hóa.</p>
              <strong>— Đội ngũ Sáng lập LocalExplore</strong>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- 4 Core Pillars -->
    <section class="pillars-section">
      <div class="container">
        <div class="section-title-center">
          <span class="sub-badge">GIÁ TRỊ CỐT LÕI</span>
          <h2>4 Trụ cột định hình mọi hành trình</h2>
          <p>Mọi trải nghiệm trên LocalExplore đều được thiết kế và kiểm duyệt chặt chẽ theo 4 nguyên tắc vàng.</p>
        </div>

        <div class="pillars-grid">
          <div class="pillar-card">
            <div class="pillar-icon bg-green">🏺</div>
            <h3>Bảo tồn di sản sống</h3>
            <p>Hỗ trợ các nghệ nhân duy trì kỹ nghệ cổ truyền, truyền cảm hứng cho thế hệ trẻ tự hào về bản sắc văn hóa cội nguồn.</p>
          </div>

          <div class="pillar-card">
            <div class="pillar-icon bg-amber">🤝</div>
            <h3>Trao quyền cho người bản địa</h3>
            <p>Tạo sinh kế bền vững, giúp người dân địa phương tăng thu nhập trực tiếp từ văn hóa và tri thức bản địa của mình.</p>
          </div>

          <div class="pillar-card">
            <div class="pillar-icon bg-blue">🌿</div>
            <h3>Trải nghiệm chân thực & ý nghĩa</h3>
            <p>Nói không với các hoạt động mua sắm ép buộc hay dàn dựng. Du khách cùng ăn, cùng làm, cùng sống với người dân địa phương.</p>
          </div>

          <div class="pillar-card">
            <div class="pillar-icon bg-emerald">🌱</div>
            <h3>Du lịch có trách nhiệm</h3>
            <p>Tôn trọng phong tục tập quán, bảo vệ cảnh quan thiên nhiên nguyên sơ và đóng góp một phần doanh thu cho quỹ cộng đồng.</p>
          </div>
        </div>
      </div>
    </section>

    <!-- Featured Artisans / Hosts -->
    <section class="container artisans-section">
      <div class="section-title-center">
        <span class="sub-badge">GƯƠNG MẶT ĐẠI DIỆN</span>
        <h2>Những người giữ hồn di sản trên LocalExplore</h2>
        <p>Họ là những người bạn sẽ đồng hành, trò chuyện và cùng tạo nên những kỷ niệm khó quên.</p>
      </div>

      <div class="artisans-grid">
        <div class="artisan-card">
          <div class="artisan-avatar">
            <img src="https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=300" alt="Cô Mai Bát Tràng" />
          </div>
          <h3>Cô Trần Thị Mai</h3>
          <span class="artisan-role">Nghệ nhân gốm sứ • Bát Tràng, Hà Nội</span>
          <p>Hơn 35 năm gắn bó bên bàn xoay gốm thủ công. Cô Mai mở cửa xưởng đón du khách đến tự tay vuốt đất, vẽ men lam truyền thống.</p>
          <router-link to="/tours/tu-tay-vuot-gom-battrang" class="artisan-link">Xem tour của cô Mai →</router-link>
        </div>

        <div class="artisan-card">
          <div class="artisan-avatar">
            <img src="https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=300" alt="Anh Chinh Sa Pa" />
          </div>
          <h3>Anh Lò Văn Chinh</h3>
          <span class="artisan-role">Người dẫn đường bản H'Mông • Tả Phìn, Sa Pa</span>
          <p>Sinh ra và lớn lên trên dãy Hoàng Liên Sơn, anh Chinh đam mê chia sẻ tri thức về rừng chè Shan Tuyết cổ thụ và nghề dệt thổ cẩm bản địa.</p>
          <router-link to="/tours/hai-che-shan-tuyet-det-lanh-sa-pa" class="artisan-link">Xem tour của anh Chinh →</router-link>
        </div>

        <div class="artisan-card">
          <div class="artisan-avatar">
            <img src="https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=300" alt="Bác Thảo Hội An" />
          </div>
          <h3>Bác Nguyễn Văn Thảo</h3>
          <span class="artisan-role">Nghệ nhân làm lồng đèn • Phố Cổ Hội An</span>
          <p>Là đời thứ 3 trong gia đình làm đèn lồng lụa Hội An. Bác Thảo kiên nhẫn hướng dẫn du khách từng bước vót nan tre, căng lụa tạo nên chiếc lồng đèn may mắn.</p>
          <router-link to="/tours/lam-long-den-dap-xe-lang-rau-tra-que" class="artisan-link">Xem tour của bác Thảo →</router-link>
        </div>
      </div>
    </section>

    <!-- Join Community CTA -->
    <section class="container cta-banner-section">
      <div class="cta-banner">
        <h2>Bạn đã sẵn sàng cho một chuyến đi mang đậm văn hóa?</h2>
        <p>Khám phá ngay hàng chục tour trải nghiệm độc bản hoặc trở thành Host để lan tỏa tình yêu quê hương.</p>
        <div class="cta-actions">
          <router-link to="/tours" class="btn-primary-lg">Khám phá các tour</router-link>
          <router-link to="/become-host" class="btn-outline-lg">Đăng ký làm Host bản địa</router-link>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
// AboutView
</script>

<style scoped>
.about-page {
  background-color: #fafbfc;
  min-height: 100vh;
  padding-bottom: 80px;
}

/* Hero Section */
.about-hero {
  background: linear-gradient(135deg, #1B4332 0%, #2D6A4F 60%, #153e2d 100%);
  color: #fff;
  padding: 48px 0 60px;
  position: relative;
  overflow: hidden;
}

.about-hero::after {
  content: '';
  position: absolute;
  top: 0; right: 0; bottom: 0; left: 0;
  background: radial-gradient(circle at 80% 30%, rgba(217, 119, 6, 0.15) 0%, transparent 60%);
  pointer-events: none;
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
  transition: color 0.2s;
}

.hero-breadcrumb :deep(a:hover) {
  color: #fff;
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

/* Stats Bar */
.stats-bar-section {
  background: #fff;
  border-bottom: 1px solid #e2e8f0;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.03);
}

.stats-bar-container {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  padding: 32px 20px;
  gap: 20px;
}

@media (max-width: 768px) {
  .stats-bar-container {
    grid-template-columns: repeat(2, 1fr);
  }
}

.stat-col {
  text-align: center;
}

.stat-num {
  display: block;
  font-size: 34px;
  font-weight: 900;
  color: #1B4332;
  margin-bottom: 4px;
}

.stat-lbl {
  font-size: 13px;
  color: #64748b;
  font-weight: 600;
}

/* Mission Section */
.mission-section {
  padding: 64px 20px;
}

.mission-grid {
  display: grid;
  grid-template-columns: 1.1fr 0.9fr;
  gap: 48px;
  align-items: center;
}

@media (max-width: 900px) {
  .mission-grid {
    grid-template-columns: 1fr;
  }
}

.sub-badge {
  display: inline-block;
  color: #D97706;
  font-size: 11px;
  font-weight: 800;
  letter-spacing: 1.2px;
  margin-bottom: 8px;
}

.mission-text h2 {
  font-size: 30px;
  font-weight: 800;
  color: #1e293b;
  margin-bottom: 16px;
}

.lead-text {
  font-size: 16px;
  line-height: 1.7;
  color: #334155;
  font-weight: 500;
  margin-bottom: 16px;
}

.body-text {
  font-size: 14px;
  line-height: 1.7;
  color: #64748b;
}

.mission-media {
  position: relative;
}

.main-img img {
  width: 100%;
  border-radius: 16px;
  box-shadow: 0 12px 32px rgba(0, 0, 0, 0.1);
}

.floating-card {
  position: absolute;
  bottom: -24px;
  left: -24px;
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(12px);
  padding: 20px 24px;
  border-radius: 14px;
  box-shadow: 0 10px 28px rgba(0, 0, 0, 0.12);
  border: 1px solid rgba(226, 232, 240, 0.8);
  max-width: 320px;
}

@media (max-width: 600px) {
  .floating-card {
    position: static;
    margin-top: 16px;
    max-width: 100%;
  }
}

.quote-mark {
  font-size: 32px;
  line-height: 1;
  color: #D97706;
  font-family: Georgia, serif;
}

.float-quote p {
  font-size: 13px;
  font-style: italic;
  color: #334155;
  margin: 4px 0 8px;
}

.float-quote strong {
  font-size: 11px;
  color: #1B4332;
  display: block;
}

/* Pillars Section */
.pillars-section {
  background: #f1f5f9;
  padding: 64px 0;
}

.section-title-center {
  text-align: center;
  max-width: 640px;
  margin: 0 auto 48px;
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

.pillars-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 24px;
}

@media (max-width: 900px) {
  .pillars-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 500px) {
  .pillars-grid {
    grid-template-columns: 1fr;
  }
}

.pillar-card {
  background: #fff;
  padding: 30px 24px;
  border-radius: 14px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.04);
  transition: transform 0.2s;
}

.pillar-card:hover {
  transform: translateY(-4px);
}

.pillar-icon {
  width: 54px;
  height: 54px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 26px;
  margin-bottom: 18px;
}

.pillar-icon.bg-green { background: #DCFCE7; }
.pillar-icon.bg-amber { background: #FEF3C7; }
.pillar-icon.bg-blue { background: #E0E7FF; }
.pillar-icon.bg-emerald { background: #CCFBF1; }

.pillar-card h3 {
  font-size: 17px;
  font-weight: 800;
  color: #1e293b;
  margin-bottom: 8px;
}

.pillar-card p {
  font-size: 13px;
  line-height: 1.6;
  color: #64748b;
  margin: 0;
}

/* Artisans Section */
.artisans-section {
  padding: 64px 20px;
}

.artisans-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 28px;
}

@media (max-width: 800px) {
  .artisans-grid {
    grid-template-columns: 1fr;
  }
}

.artisan-card {
  background: #fff;
  border: 1px solid #e2e8f0;
  border-radius: 16px;
  padding: 30px 24px;
  text-align: center;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.04);
  display: flex;
  flex-direction: column;
  align-items: center;
  transition: transform 0.2s;
}

.artisan-card:hover {
  transform: translateY(-4px);
}

.artisan-avatar {
  width: 90px;
  height: 90px;
  border-radius: 50%;
  overflow: hidden;
  margin-bottom: 16px;
  border: 3px solid #1B4332;
}

.artisan-avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.artisan-card h3 {
  font-size: 18px;
  font-weight: 800;
  color: #1e293b;
  margin-bottom: 4px;
}

.artisan-role {
  font-size: 12px;
  font-weight: 700;
  color: #1B4332;
  margin-bottom: 12px;
}

.artisan-card p {
  font-size: 13px;
  line-height: 1.6;
  color: #64748b;
  margin-bottom: 18px;
  flex: 1;
}

.artisan-link {
  color: #1B4332;
  font-size: 13px;
  font-weight: 700;
  text-decoration: none;
}

.artisan-link:hover {
  text-decoration: underline;
}

/* CTA Banner */
.cta-banner-section {
  padding-top: 20px;
}

.cta-banner {
  background: linear-gradient(135deg, #1B4332 0%, #2D6A4F 100%);
  color: #fff;
  border-radius: 20px;
  padding: 56px 40px;
  text-align: center;
  box-shadow: 0 12px 36px rgba(27, 67, 50, 0.2);
}

.cta-banner h2 {
  font-size: 30px;
  font-weight: 800;
  color: #fff;
  margin-bottom: 10px;
}

.cta-banner p {
  font-size: 16px;
  color: rgba(255, 255, 255, 0.85);
  max-width: 620px;
  margin: 0 auto 30px;
}

.cta-actions {
  display: flex;
  justify-content: center;
  gap: 16px;
  flex-wrap: wrap;
}

.btn-primary-lg {
  background: #D97706;
  color: #fff;
  font-size: 15px;
  font-weight: 700;
  padding: 12px 30px;
  border-radius: 10px;
  text-decoration: none;
  transition: background 0.2s;
}

.btn-primary-lg:hover {
  background: #b45309;
  color: #fff;
}

.btn-outline-lg {
  background: transparent;
  color: #fff;
  border: 1.5px solid rgba(255, 255, 255, 0.8);
  font-size: 15px;
  font-weight: 700;
  padding: 12px 30px;
  border-radius: 10px;
  text-decoration: none;
  transition: all 0.2s;
}

.btn-outline-lg:hover {
  background: #fff;
  color: #1B4332;
}
</style>
