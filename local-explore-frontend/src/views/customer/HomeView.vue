<template>
  <div class="home-page">
    <!-- ====================================================================
         1. HERO SECTION (Using user's uploaded iconic Vietnam panorama)
         ==================================================================== -->
    <section class="hero-section">
      <div class="hero-bg-media"></div>
      <div class="hero-bg-overlay"></div>
      
      <div class="container hero-container">
        <!-- Floating Sketch Badge (Top Right) -->
        <div class="hero-sketch-badge">
          <svg class="sketch-badge-svg" viewBox="0 0 160 160" fill="none" xmlns="http://www.w3.org/2000/svg">
            <circle cx="80" cy="80" r="74" stroke="rgba(255,255,255,0.75)" stroke-width="2" stroke-dasharray="6 6" />
            <circle cx="80" cy="80" r="66" stroke="rgba(255,255,255,0.3)" stroke-width="1" />
          </svg>
          <div class="sketch-badge-content">
            <span class="sketch-sparkle">✦</span>
            <div class="sketch-text-main">Trải nghiệm thực</div>
            <div class="sketch-text-sub">giá trị thực!</div>
          </div>
        </div>

        <!-- Hero Top: Left Title & Subtitle (No arbitrary line breaks) -->
        <div class="hero-top-row">
          <div class="hero-text-block">
            <h1 class="hero-title">
              Khám phá những trải nghiệm địa phương độc đáo
            </h1>
            <p class="hero-subtitle">
              Từ ẩm thực, văn hóa, làng nghề đến thiên nhiên hoang sơ,<br />
              LocalExplore kết nối bạn với những hành trình ý nghĩa.
            </p>
          </div>
        </div>

        <!-- Hero Bottom: Wide Pill Search Bar (Full Width, No text cut-off) -->
        <div class="hero-search-bar">
          <!-- 1. Destination -->
          <div class="search-cell cell-dest">
            <EnvironmentOutlined class="search-icon" />
            <input
              v-model="searchForm.destination"
              type="text"
              placeholder="Bạn muốn đi đâu?"
              class="search-input-field"
              @keyup.enter="handleSearch"
            />
          </div>

          <div class="search-divider"></div>

          <!-- 2. Date -->
          <div class="search-cell cell-date">
            <CalendarOutlined class="search-icon" />
            <a-date-picker
              v-model:value="searchForm.departureDate"
              placeholder="Ngày khởi hành"
              format="DD/MM/YYYY"
              :bordered="false"
              class="search-date-picker"
            />
          </div>

          <div class="search-divider"></div>

          <!-- 3. Guests -->
          <div class="search-cell cell-guests">
            <TeamOutlined class="search-icon" />
            <select v-model="searchForm.guests" class="search-select-field">
              <option value="Số người">Số người</option>
              <option value="1">1 người</option>
              <option value="2">2 người</option>
              <option value="3-5">3 - 5 người</option>
              <option value="5+">Trên 5 người</option>
            </select>
            <DownOutlined class="select-chevron" />
          </div>

          <div class="search-divider"></div>

          <!-- 4. Category -->
          <div class="search-cell cell-cat">
            <AppstoreOutlined class="search-icon" />
            <select v-model="searchForm.categoryId" class="search-select-field">
              <option :value="undefined">Tất cả danh mục</option>
              <option v-for="cat in categories" :key="cat.id" :value="cat.id">
                {{ cat.name }}
              </option>
            </select>
            <DownOutlined class="select-chevron" />
          </div>

          <!-- Submit Button -->
          <button class="search-btn-pill btn-motion-primary" @click="handleSearch">
            <SearchOutlined />
            <span>Tìm kiếm</span>
          </button>
        </div>
      </div>
    </section>

    <!-- ====================================================================
         2. KHÁM PHÁ THEO DANH MỤC (Pastel Circular Floating Icons)
         ==================================================================== -->
    <section id="categories" class="section-categories container reveal-on-scroll">
      <div class="section-header-row">
        <h2 class="section-main-title">Khám phá theo danh mục</h2>
        <router-link to="/categories" class="see-all-link">Xem tất cả danh mục →</router-link>
      </div>

      <div class="categories-icon-row">
        <!-- 1. Food & Cuisine – Ẩm thực -->
        <div class="category-icon-item stagger-1" @click="filterByCategory('Ẩm thực')">
          <div class="cat-circle">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
              <path d="M12 2v3"></path>
              <path d="M8 2v3"></path>
              <path d="M16 2v3"></path>
              <path d="M3 10c0 4.97 4.03 9 9 9s9-4.03 9-9H3z"></path>
              <line x1="8" y1="21" x2="16" y2="21"></line>
            </svg>
          </div>
          <span class="cat-name">Ẩm thực</span>
          <span class="cat-count">(120 tour)</span>
        </div>

        <!-- 2. Local Culture – Văn hóa bản địa -->
        <div class="category-icon-item stagger-2" @click="filterByCategory('Văn hóa')">
          <div class="cat-circle">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
              <line x1="3" y1="22" x2="21" y2="22"></line>
              <line x1="6" y1="18" x2="6" y2="11"></line>
              <line x1="10" y1="18" x2="10" y2="11"></line>
              <line x1="14" y1="18" x2="14" y2="11"></line>
              <line x1="18" y1="18" x2="18" y2="11"></line>
              <polygon points="12 2 20 7 4 7"></polygon>
            </svg>
          </div>
          <span class="cat-name">Văn hóa bản địa</span>
          <span class="cat-count">(98 tour)</span>
        </div>

        <!-- 3. Traditional Crafts – Làng nghề truyền thống -->
        <div class="category-icon-item stagger-3" @click="filterByCategory('Làng nghề')">
          <div class="cat-circle">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
              <path d="M10 2h4"></path>
              <path d="M10 2v3c0 2-4 3-4 7 0 4.4 2.7 8 6 8s6-3.6 6-8c0-4-4-5-4-7V2"></path>
              <path d="M6 9a3 3 0 0 0-3 3c0 2.5 1.5 4 3 4"></path>
              <path d="M18 9a3 3 0 0 1 3 3c0 2.5-1.5 4-3 4"></path>
              <path d="M9 22h6"></path>
            </svg>
          </div>
          <span class="cat-name">Làng nghề truyền thống</span>
          <span class="cat-count">(76 tour)</span>
        </div>

        <!-- 4. Mountains – Núi rừng -->
        <div class="category-icon-item stagger-4" @click="filterByCategory('Núi rừng')">
          <div class="cat-circle">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
              <path d="m8 3 4 8 5-5 5 15H2L8 3z"></path>
              <path d="M4.15 15.1c2.62-1.6 5.25-1.4 7.85.4 2.75 2 5.5 2 8.25.2"></path>
            </svg>
          </div>
          <span class="cat-name">Núi rừng</span>
          <span class="cat-count">(84 tour)</span>
        </div>

        <!-- 5. Agriculture – Nông nghiệp -->
        <div class="category-icon-item stagger-5" @click="filterByCategory('Nông nghiệp')">
          <div class="cat-circle">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
              <path d="m2 22 10-10"></path>
              <path d="M16 8a4 4 0 0 0-4-4c-2 2-2 4 0 6 2 2 4 2 4-2Z"></path>
              <path d="M8 16a4 4 0 0 0 4 4c2-2 2-4 0-6-2-2-4-2-4 2Z"></path>
              <path d="M14 14a4 4 0 0 0 0-6c-2 0-4 2-2 4 2 2 2 2 2 2Z"></path>
              <path d="M10 10a4 4 0 0 0 0 6c2 0 4-2 2-4-2-2-2-2-2-2Z"></path>
            </svg>
          </div>
          <span class="cat-name">Nông nghiệp</span>
          <span class="cat-count">(28 tour)</span>
        </div>

        <!-- 6. Beach & Islands – Biển đảo -->
        <div class="category-icon-item stagger-6" @click="filterByCategory('Biển đảo')">
          <div class="cat-circle">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
              <path d="M12 4V2"></path>
              <path d="m4.93 6.93 1.41 1.41"></path>
              <path d="M2 12h2"></path>
              <path d="M20 12h2"></path>
              <path d="m17.66 8.34 1.41-1.41"></path>
              <path d="M16 12a4 4 0 0 0-8 0"></path>
              <path d="M2 17c1.5-.8 3.5-.8 5 0 1.5.8 3.5.8 5 0 1.5-.8 3.5-.8 5 0 1.5.8 3.5.8 5 0"></path>
              <path d="M2 21c1.5-.8 3.5-.8 5 0 1.5.8 3.5.8 5 0 1.5-.8 3.5-.8 5 0 1.5.8 3.5.8 5 0"></path>
            </svg>
          </div>
          <span class="cat-name">Biển đảo</span>
          <span class="cat-count">(65 tour)</span>
        </div>

        <!-- 7. Adventure – Phiêu lưu -->
        <div class="category-icon-item stagger-1" @click="filterByCategory('Phiêu lưu')">
          <div class="cat-circle">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
              <circle cx="12" cy="12" r="10"></circle>
              <polygon points="16.24 7.76 14.12 14.12 7.76 16.24 9.88 9.88 16.24 7.76"></polygon>
            </svg>
          </div>
          <span class="cat-name">Phiêu lưu</span>
          <span class="cat-count">(34 tour)</span>
        </div>

        <!-- 8. Workshop – Workshop thủ công -->
        <div class="category-icon-item stagger-2" @click="filterByCategory('Workshop')">
          <div class="cat-circle">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
              <circle cx="13.5" cy="6.5" r=".8" fill="#285A45"></circle>
              <circle cx="17.5" cy="10.5" r=".8" fill="#285A45"></circle>
              <circle cx="8.5" cy="7.5" r=".8" fill="#285A45"></circle>
              <circle cx="6.5" cy="12.5" r=".8" fill="#285A45"></circle>
              <path d="M12 2C6.5 2 2 6.5 2 12s4.5 10 10 10c.926 0 1.648-.746 1.648-1.688 0-.437-.18-.835-.437-1.125-.29-.289-.438-.652-.438-1.125a1.64 1.64 0 0 1 1.668-1.668h1.996c3.051 0 5.555-2.503 5.555-5.554C21.965 6.012 17.461 2 12 2z"></path>
            </svg>
          </div>
          <span class="cat-name">Workshop thủ công</span>
          <span class="cat-count">(45 tour)</span>
        </div>

        <!-- 9. Camping – Cắm trại -->
        <div class="category-icon-item stagger-3" @click="filterByCategory('Cắm trại')">
          <div class="cat-circle">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
              <path d="M3.5 21 14 3"></path>
              <path d="m20.5 21-10.5-18"></path>
              <path d="M15.5 21 12 15l-3.5 6"></path>
              <path d="M2 21h20"></path>
            </svg>
          </div>
          <span class="cat-name">Cắm trại</span>
          <span class="cat-count">(38 tour)</span>
        </div>

        <!-- 10. Photography – Nhiếp ảnh -->
        <div class="category-icon-item stagger-4" @click="filterByCategory('Nhiếp ảnh')">
          <div class="cat-circle">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
              <path d="M14.5 4h-5L7 7H4a2 2 0 0 0-2 2v9a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2V9a2 2 0 0 0-2-2h-3l-2.5-3z"></path>
              <circle cx="12" cy="13" r="3"></circle>
            </svg>
          </div>
          <span class="cat-name">Nhiếp ảnh</span>
          <span class="cat-count">(29 tour)</span>
        </div>

        <!-- 11. Nature – Thiên nhiên -->
        <div class="category-icon-item stagger-5" @click="filterByCategory('Thiên nhiên')">
          <div class="cat-circle">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
              <path d="M11 20A7 7 0 0 1 9.8 6.1C15.5 5 17 4.48 19 2c1 2 2 4.18 2 8 0 5.5-4.78 10-10 10Z"></path>
              <path d="M2 21c0-3 1.85-5.36 5.08-6C9.5 14.52 12 13 13 12"></path>
            </svg>
          </div>
          <span class="cat-name">Thiên nhiên</span>
          <span class="cat-count">(102 tour)</span>
        </div>

        <!-- 12. Community – Trải nghiệm cộng đồng -->
        <div class="category-icon-item stagger-6" @click="filterByCategory('Cộng đồng')">
          <div class="cat-circle">
            <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="#285A45" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round">
              <path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"></path>
              <circle cx="9" cy="7" r="4"></circle>
              <path d="M22 21v-2a4 4 0 0 0-3-3.87"></path>
              <path d="M16 3.13a4 4 0 0 1 0 7.75"></path>
            </svg>
          </div>
          <span class="cat-name">Trải nghiệm cộng đồng</span>
          <span class="cat-count">(51 tour)</span>
        </div>
      </div>
    </section>

    <!-- ====================================================================
         3. TOUR NỔI BẬT (5-Column Grid with Badges)
         ==================================================================== -->
    <section class="section-featured-tours container reveal-on-scroll">
      <div class="section-header-row">
        <div>
          <h2 class="section-main-title">Tour nổi bật</h2>
          <p class="section-sub-title">Những trải nghiệm được yêu thích nhất</p>
        </div>
        <router-link to="/tours" class="see-all-link">Xem tất cả →</router-link>
      </div>

      <div class="featured-tours-row">
        <!-- Tour 1 -->
        <div class="tour-card-5 card-hover-lift stagger-1" @click="$router.push('/tours/tu-tay-vuot-gom-battrang')">
          <div class="tour-thumb-box">
            <img src="https://images.unsplash.com/photo-1565193566173-7a0ee3dbe261?w=600&q=80" alt="Bát Tràng" />
            <span class="tour-pill-badge badge-amber">Đặc sắc</span>
            <button
              class="tour-wishlist-heart"
              :class="{ 'heart-pop-active': wishlist['tour-1'], 'is-active': wishlist['tour-1'] }"
              title="Yêu thích"
              @click.stop="toggleWishlist('tour-1')"
            >
              <HeartFilled v-if="wishlist['tour-1']" style="color: #ef4444;" />
              <HeartOutlined v-else />
            </button>
          </div>
          <div class="tour-info-box">
            <h3 class="tour-h3" title="Trải nghiệm làm gốm Bát Tràng">Trải nghiệm làm gốm Bát Tràng</h3>
            <div class="tour-loc-row"><EnvironmentOutlined /> Hà Nội</div>
            <div class="tour-rating-time">
              <span class="t-star">★ 5.0</span>
              <span class="t-reviews">(320)</span>
              <span class="t-sep">•</span>
              <span class="t-duration">4 giờ</span>
            </div>
            <div class="tour-price-line">
              <span class="t-price">350.000đ</span>
              <span class="t-unit">/ khách</span>
            </div>
          </div>
        </div>

        <!-- Tour 2 -->
        <div class="tour-card-5 card-hover-lift stagger-2" @click="$router.push('/tours/lam-long-den-dap-xe-lang-rau-tra-que')">
          <div class="tour-thumb-box">
            <img src="https://images.unsplash.com/photo-1555396273-367ea4eb4db5?w=600&q=80" alt="Hội An Cooking" />
            <span class="tour-pill-badge badge-green">Mới</span>
            <button
              class="tour-wishlist-heart"
              :class="{ 'heart-pop-active': wishlist['tour-2'], 'is-active': wishlist['tour-2'] }"
              title="Yêu thích"
              @click.stop="toggleWishlist('tour-2')"
            >
              <HeartFilled v-if="wishlist['tour-2']" style="color: #ef4444;" />
              <HeartOutlined v-else />
            </button>
          </div>
          <div class="tour-info-box">
            <h3 class="tour-h3" title="Làm lồng đèn lụa Hội An & Làng rau Trà Quế">Làm lồng đèn lụa Hội An & Làng rau Trà Quế</h3>
            <div class="tour-loc-row"><EnvironmentOutlined /> Quảng Nam</div>
            <div class="tour-rating-time">
              <span class="t-star">★ 4.9</span>
              <span class="t-reviews">(189)</span>
              <span class="t-sep">•</span>
              <span class="t-duration">5 giờ</span>
            </div>
            <div class="tour-price-line">
              <span class="t-price">420.000đ</span>
              <span class="t-unit">/ khách</span>
            </div>
          </div>
        </div>

        <!-- Tour 3 -->
        <div class="tour-card-5 card-hover-lift stagger-3" @click="$router.push('/tours/hai-che-shan-tuyet-det-lanh-sa-pa')">
          <div class="tour-thumb-box">
            <img src="https://images.unsplash.com/photo-1544644181-1484b3fdfc62?w=600&q=80" alt="Sapa Trekking" />
            <span class="tour-pill-badge badge-red">Yêu thích</span>
            <button
              class="tour-wishlist-heart"
              :class="{ 'heart-pop-active': wishlist['tour-3'], 'is-active': wishlist['tour-3'] }"
              title="Yêu thích"
              @click.stop="toggleWishlist('tour-3')"
            >
              <HeartFilled v-if="wishlist['tour-3']" style="color: #ef4444;" />
              <HeartOutlined v-else />
            </button>
          </div>
          <div class="tour-info-box">
            <h3 class="tour-h3" title="Hái chè Shan Tuyết cổ thụ & Dệt lanh Sa Pa">Hái chè Shan Tuyết & Dệt lanh Sa Pa</h3>
            <div class="tour-loc-row"><EnvironmentOutlined /> Lào Cai</div>
            <div class="tour-rating-time">
              <span class="t-star">★ 4.95</span>
              <span class="t-reviews">(412)</span>
              <span class="t-sep">•</span>
              <span class="t-duration">6 giờ</span>
            </div>
            <div class="tour-price-line">
              <span class="t-price">550.000đ</span>
              <span class="t-unit">/ khách</span>
            </div>
          </div>
        </div>

        <!-- Tour 4 -->
        <div class="tour-card-5 card-hover-lift stagger-4" @click="$router.push('/tours/tu-tay-vuot-gom-battrang')">
          <div class="tour-thumb-box">
            <img src="https://images.unsplash.com/photo-1528127269322-539801943592?w=600&q=80" alt="Tràng An Thuyền" />
            <button
              class="tour-wishlist-heart"
              :class="{ 'heart-pop-active': wishlist['tour-4'], 'is-active': wishlist['tour-4'] }"
              title="Yêu thích"
              @click.stop="toggleWishlist('tour-4')"
            >
              <HeartFilled v-if="wishlist['tour-4']" style="color: #ef4444;" />
              <HeartOutlined v-else />
            </button>
          </div>
          <div class="tour-info-box">
            <h3 class="tour-h3" title="Khám phá Tràng An bằng thuyền">Khám phá Tràng An bằng thuyền</h3>
            <div class="tour-loc-row"><EnvironmentOutlined /> Ninh Bình</div>
            <div class="tour-rating-time">
              <span class="t-star">★ 4.7</span>
              <span class="t-reviews">(158)</span>
              <span class="t-sep">•</span>
              <span class="t-duration">1 ngày</span>
            </div>
            <div class="tour-price-line">
              <span class="t-price">300.000đ</span>
              <span class="t-unit">/ người</span>
            </div>
          </div>
        </div>

        <!-- Tour 5 -->
        <div class="tour-card-5 card-hover-lift stagger-5" @click="$router.push('/tours/lam-long-den-dap-xe-lang-rau-tra-que')">
          <div class="tour-thumb-box">
            <img src="https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?w=600&q=80" alt="Phố cổ Hội An" />
            <button
              class="tour-wishlist-heart"
              :class="{ 'heart-pop-active': wishlist['tour-5'], 'is-active': wishlist['tour-5'] }"
              title="Yêu thích"
              @click.stop="toggleWishlist('tour-5')"
            >
              <HeartFilled v-if="wishlist['tour-5']" style="color: #ef4444;" />
              <HeartOutlined v-else />
            </button>
          </div>
          <div class="tour-info-box">
            <h3 class="tour-h3" title="Tour phố cổ Hội An về đêm">Tour phố cổ Hội An</h3>
            <div class="tour-loc-row"><EnvironmentOutlined /> Quảng Nam</div>
            <div class="tour-rating-time">
              <span class="t-star">★ 4.8</span>
              <span class="t-reviews">(276)</span>
              <span class="t-sep">•</span>
              <span class="t-duration">1 ngày</span>
            </div>
            <div class="tour-price-line">
              <span class="t-price">420.000đ</span>
              <span class="t-unit">/ người</span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- ====================================================================
         4. NHỮNG VÙNG ĐẤT TRUYỀN CẢM HỨNG (Inspirational Banner)
         ==================================================================== -->
    <section class="section-inspire container reveal-on-scroll">
      <div class="inspire-banner">
        <div class="inspire-left">
          <div class="inspire-tag">✦ Điểm đến phổ biến</div>
          <h2 class="inspire-title">Những vùng đất truyền cảm hứng</h2>
          <p class="inspire-desc">
            Khám phá các địa điểm du lịch nổi tiếng và trải nghiệm văn hóa thú vị nhất Việt Nam.
          </p>
          <router-link to="/tours" class="inspire-btn btn-motion-primary">
            Khám phá ngay →
          </router-link>
        </div>

        <div class="inspire-destinations">
          <div class="insp-card card-hover-lift" @click="filterByDest('Hà Nội')">
            <img src="https://images.unsplash.com/photo-1509042239860-f550ce710b93?w=500&q=80" alt="Hà Nội" />
            <div class="insp-label">
              <h4>Hà Nội</h4>
              <p>25 tour</p>
            </div>
          </div>

          <div class="insp-card card-hover-lift" @click="filterByDest('Hội An')">
            <img src="https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?w=500&q=80" alt="Hội An" />
            <div class="insp-label">
              <h4>Hội An</h4>
              <p>34 tour</p>
            </div>
          </div>

          <div class="insp-card card-hover-lift" @click="filterByDest('Sapa')">
            <img src="https://images.unsplash.com/photo-1544644181-1484b3fdfc62?w=500&q=80" alt="Sapa" />
            <div class="insp-label">
              <h4>Sapa</h4>
              <p>28 tour</p>
            </div>
          </div>

          <div class="insp-card card-hover-lift" @click="filterByDest('Ninh Bình')">
            <img src="https://images.unsplash.com/photo-1528127269322-539801943592?w=500&q=80" alt="Ninh Bình" />
            <div class="insp-label">
              <h4>Ninh Bình</h4>
              <p>36 tour</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- ====================================================================
         5. DUAL ROW 1: TRẢI NGHIỆM DÀNH RIÊNG CHO BẠN + KHÁM PHÁ THEO NGÂN SÁCH
         ==================================================================== -->
    <section class="section-dual container reveal-on-scroll">
      <!-- Cột Trái: Trải nghiệm dành riêng cho bạn -->
      <div class="dual-col-4">
        <div class="section-header-row compact">
          <div>
            <h3 class="dual-title">Trải nghiệm dành riêng cho bạn</h3>
            <p class="section-sub-title">Gợi ý dựa trên sở thích và hành vi của bạn</p>
          </div>
          <router-link to="/tours" class="see-all-link">Xem tất cả →</router-link>
        </div>

        <div class="grid-4-items">
          <div class="mini-tour-card card-hover-lift" @click="$router.push('/tours')">
            <div class="mini-img-wrap">
              <img src="https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=400" alt="Thiên nhiên" />
              <button
                class="mini-heart"
                :class="{ 'heart-pop-active': wishlist['rec-1'], 'is-active': wishlist['rec-1'] }"
                title="Yêu thích"
                @click.stop="toggleWishlist('rec-1')"
              >
                <HeartFilled v-if="wishlist['rec-1']" style="color: #ef4444;" />
                <HeartOutlined v-else />
              </button>
            </div>
            <div class="mini-body">
              <h5>Dành cho người yêu thiên nhiên</h5>
              <p>Khám phá 18 tour</p>
            </div>
          </div>

          <div class="mini-tour-card card-hover-lift" @click="$router.push('/tours')">
            <div class="mini-img-wrap">
              <img src="https://images.unsplash.com/photo-1504674900247-0877df9cc836?w=400" alt="Ẩm thực" />
              <button
                class="mini-heart"
                :class="{ 'heart-pop-active': wishlist['rec-2'], 'is-active': wishlist['rec-2'] }"
                title="Yêu thích"
                @click.stop="toggleWishlist('rec-2')"
              >
                <HeartFilled v-if="wishlist['rec-2']" style="color: #ef4444;" />
                <HeartOutlined v-else />
              </button>
            </div>
            <div class="mini-body">
              <h5>Dành cho tín đồ ẩm thực</h5>
              <p>Thưởng thức 24 tour</p>
            </div>
          </div>

          <div class="mini-tour-card card-hover-lift" @click="$router.push('/tours')">
            <div class="mini-img-wrap">
              <img src="https://images.unsplash.com/photo-1476514525535-07fb3b4ae5f1?w=400" alt="Gia đình" />
              <button
                class="mini-heart"
                :class="{ 'heart-pop-active': wishlist['rec-3'], 'is-active': wishlist['rec-3'] }"
                title="Yêu thích"
                @click.stop="toggleWishlist('rec-3')"
              >
                <HeartFilled v-if="wishlist['rec-3']" style="color: #ef4444;" />
                <HeartOutlined v-else />
              </button>
            </div>
            <div class="mini-body">
              <h5>Dành cho gia đình</h5>
              <p>Trải nghiệm 15 tour</p>
            </div>
          </div>

          <div class="mini-tour-card card-hover-lift" @click="$router.push('/tours')">
            <div class="mini-img-wrap">
              <img src="https://images.unsplash.com/photo-1469854523086-cc02fe5d8800?w=400" alt="Phiêu lưu" />
              <button
                class="mini-heart"
                :class="{ 'heart-pop-active': wishlist['rec-4'], 'is-active': wishlist['rec-4'] }"
                title="Yêu thích"
                @click.stop="toggleWishlist('rec-4')"
              >
                <HeartFilled v-if="wishlist['rec-4']" style="color: #ef4444;" />
                <HeartOutlined v-else />
              </button>
            </div>
            <div class="mini-body">
              <h5>Dành cho người thích phiêu lưu</h5>
              <p>Chinh phục 30 tour</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Cột Phải: Khám phá theo ngân sách -->
      <div class="dual-col-3">
        <div class="section-header-row compact">
          <div>
            <h3 class="dual-title">Khám phá theo ngân sách</h3>
            <p class="section-sub-title">Trải nghiệm tuyệt vời cho mọi mức ngân sách</p>
          </div>
          <router-link to="/tours" class="see-all-link">Xem tất cả →</router-link>
        </div>

        <div class="grid-3-items">
          <div class="budget-card-box card-hover-lift" @click="filterByMaxPrice(300000)">
            <div class="budget-img-box">
              <img src="https://images.unsplash.com/photo-1528127269322-539801943592?w=400" alt="Dưới 300k" />
            </div>
            <div class="budget-desc-box">
              <h5>Dưới 300.000đ</h5>
              <p>Từ 1.000+ tour</p>
            </div>
          </div>

          <div class="budget-card-box card-hover-lift" @click="filterByMaxPrice(1000000)">
            <div class="budget-img-box">
              <img src="https://images.unsplash.com/photo-1544644181-1484b3fdfc62?w=400" alt="300k - 1tr" />
            </div>
            <div class="budget-desc-box">
              <h5>300.000đ - 1.000.000đ</h5>
              <p>Từ 800+ tour</p>
            </div>
          </div>

          <div class="budget-card-box card-hover-lift" @click="filterByMaxPrice(5000000)">
            <div class="budget-img-box">
              <img src="https://images.unsplash.com/photo-1565193566173-7a0ee3dbe261?w=400" alt="Trên 1tr" />
            </div>
            <div class="budget-desc-box">
              <h5>Trên 1.000.000đ</h5>
              <p>Từ 450+ tour</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- ====================================================================
         6. DUAL ROW 2: TRẢI NGHIỆM THEO MÙA + KHÁM PHÁ VIỆT NAM QUA BẢN ĐỒ
         ==================================================================== -->
    <section class="section-dual container reveal-on-scroll">
      <!-- Cột Trái: Trải nghiệm theo mùa -->
      <div class="dual-col-4">
        <div class="section-header-row compact">
          <div>
            <h3 class="dual-title">Trải nghiệm theo mùa</h3>
            <p class="section-sub-title">Những khoảnh khắc đẹp nhất trong năm</p>
          </div>
          <router-link to="/tours" class="see-all-link">Xem tất cả →</router-link>
        </div>

        <div class="grid-4-items">
          <div class="mini-tour-card card-hover-lift" @click="$router.push('/tours')">
            <div class="mini-img-wrap">
              <img src="https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=400" alt="Mùa lúa chín" />
              <button
                class="mini-heart"
                :class="{ 'heart-pop-active': wishlist['season-1'], 'is-active': wishlist['season-1'] }"
                title="Yêu thích"
                @click.stop="toggleWishlist('season-1')"
              >
                <HeartFilled v-if="wishlist['season-1']" style="color: #ef4444;" />
                <HeartOutlined v-else />
              </button>
            </div>
            <div class="mini-body">
              <h5>Mùa lúa chín</h5>
              <p class="text-green">Tây Bắc</p>
              <span class="text-xs">Tháng 9 - 10</span>
            </div>
          </div>

          <div class="mini-tour-card card-hover-lift" @click="$router.push('/tours')">
            <div class="mini-img-wrap">
              <img src="https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?w=400" alt="Lễ hội đèn lồng" />
              <button
                class="mini-heart"
                :class="{ 'heart-pop-active': wishlist['season-2'], 'is-active': wishlist['season-2'] }"
                title="Yêu thích"
                @click.stop="toggleWishlist('season-2')"
              >
                <HeartFilled v-if="wishlist['season-2']" style="color: #ef4444;" />
                <HeartOutlined v-else />
              </button>
            </div>
            <div class="mini-body">
              <h5>Lễ hội đèn lồng</h5>
              <p class="text-green">Hội An</p>
              <span class="text-xs">Hàng tháng</span>
            </div>
          </div>

          <div class="mini-tour-card card-hover-lift" @click="$router.push('/tours')">
            <div class="mini-img-wrap">
              <img src="https://images.unsplash.com/photo-1469854523086-cc02fe5d8800?w=400" alt="Hoa mận trắng" />
              <button
                class="mini-heart"
                :class="{ 'heart-pop-active': wishlist['season-3'], 'is-active': wishlist['season-3'] }"
                title="Yêu thích"
                @click.stop="toggleWishlist('season-3')"
              >
                <HeartFilled v-if="wishlist['season-3']" style="color: #ef4444;" />
                <HeartOutlined v-else />
              </button>
            </div>
            <div class="mini-body">
              <h5>Hoa mận trắng</h5>
              <p class="text-green">Mộc Châu</p>
              <span class="text-xs">Tháng 1 - 2</span>
            </div>
          </div>

          <div class="mini-tour-card card-hover-lift" @click="$router.push('/tours')">
            <div class="mini-img-wrap">
              <img src="https://images.unsplash.com/photo-1528127269322-539801943592?w=400" alt="Hoa tam giác mạch" />
              <button
                class="mini-heart"
                :class="{ 'heart-pop-active': wishlist['season-4'], 'is-active': wishlist['season-4'] }"
                title="Yêu thích"
                @click.stop="toggleWishlist('season-4')"
              >
                <HeartFilled v-if="wishlist['season-4']" style="color: #ef4444;" />
                <HeartOutlined v-else />
              </button>
            </div>
            <div class="mini-body">
              <h5>Hoa tam giác mạch</h5>
              <p class="text-green">Hà Giang</p>
              <span class="text-xs">Tháng 10 - 11</span>
            </div>
          </div>
        </div>
      </div>

      <!-- Cột Phải: Khám phá Việt Nam qua bản đồ -->
      <div class="dual-col-3">
        <VietnamMapBanner />
      </div>
    </section>

    <!-- ====================================================================
         7. TRỞ THÀNH LOCAL HOST (Call to Action Banner)
         ==================================================================== -->
    <section class="section-host-cta container reveal-on-scroll">
      <div class="host-cta-panel">
        <div class="host-photo-wrap">
          <img
            src="https://images.unsplash.com/photo-1528605248644-14dd04022da1?w=600&q=80"
            alt="Trở thành Local Host"
          />
        </div>

        <div class="host-info-col">
          <h2 class="host-h2">Trở thành Local Host</h2>
          <h3 class="host-sub-h3">
            Chia sẻ văn hóa địa phương và tạo nên những trải nghiệm ý nghĩa
          </h3>
          <p class="host-p">
            Bạn là hướng dẫn viên, nghệ nhân, người dân địa phương hay đơn vị tổ chức?
            Hãy tham gia LocalExplore để kết nối với du khách trên khắp thế giới.
          </p>
          <router-link to="/become-host" class="host-btn-register btn-motion-primary">
            Tìm hiểu & Đăng ký ngay →
          </router-link>
        </div>

        <div class="host-benefits-grid">
          <div class="benefit-box">
            <div class="benefit-icon-circle"><TrophyOutlined /></div>
            <div>
              <h6>Tăng thu nhập</h6>
              <p>Tự do định giá & nhận 100% doanh thu minh bạch</p>
            </div>
          </div>

          <div class="benefit-box">
            <div class="benefit-icon-circle"><CompassOutlined /></div>
            <div>
              <h6>Quảng bá địa phương</h6>
              <p>Lan tỏa văn hóa quê hương đến du khách quốc tế</p>
            </div>
          </div>

          <div class="benefit-box">
            <div class="benefit-icon-circle"><SafetyCertificateOutlined /></div>
            <div>
              <h6>Hỗ trợ chuyên nghiệp</h6>
              <p>Đội ngũ LocalExplore hỗ trợ chụp ảnh & tư vấn bài bản</p>
            </div>
          </div>

          <div class="benefit-box">
            <div class="benefit-icon-circle"><TeamOutlined /></div>
            <div>
              <h6>Kết nối cộng đồng</h6>
              <p>Gia nhập mạng lưới hàng nghìn Local Host tài năng</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- ====================================================================
         8. CÂU CHUYỆN TỪ NGƯỜI BẢN ĐỊA (Horizontal Cards)
         ==================================================================== -->
    <section class="section-stories container reveal-on-scroll">
      <div class="section-header-row">
        <div>
          <h2 class="section-main-title">Câu chuyện từ người bản địa</h2>
          <p class="section-sub-title">Những câu chuyện chân thực từ những con người tuyệt vời</p>
        </div>
        <router-link to="/tours" class="see-all-link">Xem tất cả →</router-link>
      </div>

      <div class="stories-row-3">
        <!-- Story 1 -->
        <div class="story-h-card card-hover-lift">
          <div class="story-h-photo">
            <img src="https://images.unsplash.com/photo-1565193566173-7a0ee3dbe261?w=500&q=80" alt="Nghệ nhân gốm" />
          </div>
          <div class="story-h-body">
            <h4 class="story-h-title">Nghệ nhân làng gốm Bát Tràng</h4>
            <span class="story-pill-tag tag-culture">Văn hóa</span>
            <p class="story-h-desc">
              Hành trình gìn giữ nghề truyền thống qua nhiều thế hệ tại thủ đô nghìn năm văn hiến...
            </p>
            <a href="#" class="story-read-link" @click.prevent>Đọc tiếp →</a>
          </div>
        </div>

        <!-- Story 2 -->
        <div class="story-h-card card-hover-lift">
          <div class="story-h-photo">
            <img src="https://images.unsplash.com/photo-1544644181-1484b3fdfc62?w=500&q=80" alt="Người dẫn đường Hà Giang" />
          </div>
          <div class="story-h-body">
            <h4 class="story-h-title">Người dẫn đường tại Hà Giang</h4>
            <span class="story-pill-tag tag-people">Con người</span>
            <p class="story-h-desc">
              Khám phá những cung đường hùng vĩ và văn hóa vùng cao qua góc nhìn người bản địa...
            </p>
            <a href="#" class="story-read-link" @click.prevent>Đọc tiếp →</a>
          </div>
        </div>

        <!-- Story 3 -->
        <div class="story-h-card card-hover-lift">
          <div class="story-h-photo">
            <img src="https://images.unsplash.com/photo-1555396273-367ea4eb4db5?w=500&q=80" alt="Ẩm thực Hội An" />
          </div>
          <div class="story-h-body">
            <h4 class="story-h-title">Người giữ hồn ẩm thực Hội An</h4>
            <span class="story-pill-tag tag-food">Ẩm thực</span>
            <p class="story-h-desc">
              Những món ăn gia truyền và câu chuyện đằng sau mỗi hương vị phố cổ đậm đà...
            </p>
            <a href="#" class="story-read-link" @click.prevent>Đọc tiếp →</a>
          </div>
        </div>
      </div>
    </section>

    <!-- ====================================================================
         9. NHỮNG HOST NỔI BẬT (Horizontal with Arrow Dividers)
         ==================================================================== -->
    <section class="section-featured-hosts container reveal-on-scroll">
      <div class="section-header-row">
        <div>
          <h2 class="section-main-title">Những Host nổi bật</h2>
          <p class="section-sub-title">Gặp gỡ những người bản địa phương đam mê chia sẻ</p>
        </div>
        <router-link to="/tours" class="see-all-link">Xem tất cả →</router-link>
      </div>

      <div class="hosts-connected-row">
        <!-- Host 1 -->
        <div class="host-pill-card card-hover-lift">
          <div class="host-circle-avatar">
            <img src="https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=200" alt="Nguyễn Văn Hùng" />
          </div>
          <div class="host-card-info">
            <h4 class="h-name">Nguyễn Văn Hùng</h4>
            <p class="h-location"><EnvironmentOutlined /> Hà Giang</p>
            <div class="h-rating-tours">
              <span class="star">★ 4.9</span>
              <span class="num">(120)</span>
              <span class="dot">•</span>
              <span class="tour-cnt">12 tour</span>
            </div>
            <a href="#" class="h-profile-link" @click.prevent="$router.push('/tours')">Xem hồ sơ →</a>
          </div>
        </div>

        <div class="host-arrow-divider">→</div>

        <!-- Host 2 -->
        <div class="host-pill-card card-hover-lift">
          <div class="host-circle-avatar">
            <img src="https://images.unsplash.com/photo-1580489944761-15a19d654956?w=200" alt="Trần Thị Mai" />
          </div>
          <div class="host-card-info">
            <h4 class="h-name">Trần Thị Mai</h4>
            <p class="h-location"><EnvironmentOutlined /> Hội An</p>
            <div class="h-rating-tours">
              <span class="star">★ 4.9</span>
              <span class="num">(88)</span>
              <span class="dot">•</span>
              <span class="tour-cnt">8 tour</span>
            </div>
            <a href="#" class="h-profile-link" @click.prevent="$router.push('/tours')">Xem hồ sơ →</a>
          </div>
        </div>

        <div class="host-arrow-divider">→</div>

        <!-- Host 3 -->
        <div class="host-pill-card card-hover-lift">
          <div class="host-circle-avatar">
            <img src="https://images.unsplash.com/photo-1570295999919-56ceb5ecca61?w=200" alt="Lê Minh Anh" />
          </div>
          <div class="host-card-info">
            <h4 class="h-name">Lê Minh Anh</h4>
            <p class="h-location"><EnvironmentOutlined /> Sapa</p>
            <div class="h-rating-tours">
              <span class="star">★ 4.8</span>
              <span class="num">(64)</span>
              <span class="dot">•</span>
              <span class="tour-cnt">6 tour</span>
            </div>
            <a href="#" class="h-profile-link" @click.prevent="$router.push('/tours')">Xem hồ sơ →</a>
          </div>
        </div>

        <div class="host-arrow-divider">→</div>

        <!-- Host 4 -->
        <div class="host-pill-card card-hover-lift">
          <div class="host-circle-avatar">
            <img src="https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=200" alt="Phạm Thị Lan Hương" />
          </div>
          <div class="host-card-info">
            <h4 class="h-name">Phạm Thị Lan Hương</h4>
            <p class="h-location"><EnvironmentOutlined /> Ninh Bình</p>
            <div class="h-rating-tours">
              <span class="star">★ 5.0</span>
              <span class="num">(95)</span>
              <span class="dot">•</span>
              <span class="tour-cnt">10 tour</span>
            </div>
            <a href="#" class="h-profile-link" @click.prevent="$router.push('/tours')">Xem hồ sơ →</a>
          </div>
        </div>
      </div>
    </section>

    <!-- ====================================================================
         10. KHÁCH HÀNG NÓI GÌ VỀ CHÚNG TÔI (Horizontal Photo + Quote)
         ==================================================================== -->
    <section class="section-testimonials container reveal-on-scroll">
      <div class="section-header-row">
        <div>
          <h2 class="section-main-title">Khách hàng nói gì về chúng tôi</h2>
          <p class="section-sub-title">Cùng lắng nghe những trải nghiệm thực tế từ cộng đồng du khách</p>
        </div>
        <router-link to="/tours" class="see-all-link">Xem 150+ đánh giá →</router-link>
      </div>

      <div class="reviews-4-row">
        <!-- Review 1 -->
        <div class="review-h-box card-hover-lift">
          <div class="review-thumb-sq">
            <img src="https://images.unsplash.com/photo-1565193566173-7a0ee3dbe261?w=300" alt="Gốm Bát Tràng" />
          </div>
          <div class="review-quote-side">
            <h5 class="reviewer-name">Nguyễn Thị Mai</h5>
            <div class="reviewer-stars">★★★★★</div>
            <p class="reviewer-quote">"Trải nghiệm làm gốm thật sự tuyệt vời! Nghệ nhân rất nhiệt tình hướng dẫn..."</p>
          </div>
        </div>

        <!-- Review 2 -->
        <div class="review-h-box card-hover-lift">
          <div class="review-thumb-sq">
            <img src="https://images.unsplash.com/photo-1555396273-367ea4eb4db5?w=300" alt="Ẩm thực Hội An" />
          </div>
          <div class="review-quote-side">
            <h5 class="reviewer-name">David Wilson</h5>
            <div class="reviewer-stars">★★★★★</div>
            <p class="reviewer-quote">"Tôi đã học được cách nấu món phở truyền thống và thưởng thức ẩm thực Việt Nam..."</p>
          </div>
        </div>

        <!-- Review 3 -->
        <div class="review-h-box card-hover-lift">
          <div class="review-thumb-sq">
            <img src="https://images.unsplash.com/photo-1528127269322-539801943592?w=300" alt="Tràng An" />
          </div>
          <div class="review-quote-side">
            <h5 class="reviewer-name">Lê Minh Anh</h5>
            <div class="reviewer-stars">★★★★★</div>
            <p class="reviewer-quote">"Khung cảnh tuyệt đẹp và người dân rất mến khách. Cảm ơn LocalExplore rất nhiều!"</p>
          </div>
        </div>

        <!-- Review 4 -->
        <div class="review-h-box card-hover-lift">
          <div class="review-thumb-sq">
            <img src="https://images.unsplash.com/photo-1544644181-1484b3fdfc62?w=300" alt="Trekking Sapa" />
          </div>
          <div class="review-quote-side">
            <h5 class="reviewer-name">Sarah Johnson</h5>
            <div class="reviewer-stars">★★★★★</div>
            <p class="reviewer-quote">"Trekking Sapa cùng anh Hùng là trải nghiệm tuyệt nhất của tôi tại Việt Nam..."</p>
          </div>
        </div>
      </div>
    </section>

    <!-- ====================================================================
         11. HƯỚNG DẪN CÁCH ĐẶT TOUR & VỀ CHÚNG TÔI (4 Connected Steps)
         ==================================================================== -->
    <section id="about" class="section-how container reveal-on-scroll">
      <div class="section-header-row">
        <div>
          <h2 class="section-main-title">Về LocalExplore & Cách đặt tour</h2>
          <p class="section-sub-title">Kết nối văn hóa bản địa chỉ với 4 bước đơn giản</p>
        </div>
        <router-link to="/about" class="see-all-link">Tìm hiểu thêm về chúng tôi →</router-link>
      </div>

      <div class="steps-flow-card">
        <!-- Step 1 -->
        <div class="step-flow-item">
          <div class="step-green-number">1</div>
          <div class="step-flow-text">
            <h5>Khám phá tour</h5>
            <p>Tìm kiếm và chọn trải nghiệm phù hợp</p>
          </div>
        </div>

        <div class="step-arrow-flow">→</div>

        <!-- Step 2 -->
        <div class="step-flow-item">
          <div class="step-green-number">2</div>
          <div class="step-flow-text">
            <h5>Chọn lịch & số lượng</h5>
            <p>Xem lịch, số lượng, thông tin chi tiết</p>
          </div>
        </div>

        <div class="step-arrow-flow">→</div>

        <!-- Step 3 -->
        <div class="step-flow-item">
          <div class="step-green-number">3</div>
          <div class="step-flow-text">
            <h5>Đặt & Thanh toán</h5>
            <p>Nhanh chóng và an toàn</p>
          </div>
        </div>

        <div class="step-arrow-flow">→</div>

        <!-- Step 4 -->
        <div class="step-flow-item">
          <div class="step-green-number">4</div>
          <div class="step-flow-text">
            <h5>Tận hưởng trải nghiệm</h5>
            <p>Khám phá và ghi lại khoảnh khắc</p>
          </div>
        </div>
      </div>
    </section>

    <!-- ====================================================================
         12. BLOG & CẨM NANG DU LỊCH (4-Column Grid)
         ==================================================================== -->
    <section class="section-blog container reveal-on-scroll">
      <div class="section-header-row">
        <div>
          <h2 class="section-main-title">Blog & Cẩm nang du lịch</h2>
          <p class="section-sub-title">Những bài viết hữu ích cho hành trình của bạn</p>
        </div>
        <router-link to="/tours" class="see-all-link">Xem tất cả →</router-link>
      </div>

      <div class="blog-cards-4">
        <div class="blog-post-card card-hover-lift" @click="$router.push('/tours')">
          <div class="blog-thumb-wrap">
            <img src="https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=500" alt="Sapa lúa chín" />
            <button
              class="blog-heart"
              :class="{ 'heart-pop-active': wishlist['blog-1'], 'is-active': wishlist['blog-1'] }"
              title="Yêu thích"
              @click.stop="toggleWishlist('blog-1')"
            >
              <HeartFilled v-if="wishlist['blog-1']" style="color: #ef4444;" />
              <HeartOutlined v-else />
            </button>
          </div>
          <div class="blog-post-body">
            <h4 class="blog-post-title">Kinh nghiệm du lịch SaPa mùa lúa chín</h4>
            <span class="blog-read-time">8 phút đọc</span>
          </div>
        </div>

        <div class="blog-post-card card-hover-lift" @click="$router.push('/tours')">
          <div class="blog-thumb-wrap">
            <img src="https://images.unsplash.com/photo-1555396273-367ea4eb4db5?w=500" alt="Ẩm thực Hội An" />
            <button
              class="blog-heart"
              :class="{ 'heart-pop-active': wishlist['blog-2'], 'is-active': wishlist['blog-2'] }"
              title="Yêu thích"
              @click.stop="toggleWishlist('blog-2')"
            >
              <HeartFilled v-if="wishlist['blog-2']" style="color: #ef4444;" />
              <HeartOutlined v-else />
            </button>
          </div>
          <div class="blog-post-body">
            <h4 class="blog-post-title">Top 10 món ăn đặc sản Hội An</h4>
            <span class="blog-read-time">5 phút đọc</span>
          </div>
        </div>

        <div class="blog-post-card card-hover-lift" @click="$router.push('/tours')">
          <div class="blog-thumb-wrap">
            <img src="https://images.unsplash.com/photo-1528127269322-539801943592?w=500" alt="Ninh Bình" />
            <button
              class="blog-heart"
              :class="{ 'heart-pop-active': wishlist['blog-3'], 'is-active': wishlist['blog-3'] }"
              title="Yêu thích"
              @click.stop="toggleWishlist('blog-3')"
            >
              <HeartFilled v-if="wishlist['blog-3']" style="color: #ef4444;" />
              <HeartOutlined v-else />
            </button>
          </div>
          <div class="blog-post-body">
            <h4 class="blog-post-title">Cẩm nang du lịch Ninh Bình 2 ngày 1 đêm</h4>
            <span class="blog-read-time">6 phút đọc</span>
          </div>
        </div>

        <div class="blog-post-card card-hover-lift" @click="$router.push('/tours')">
          <div class="blog-thumb-wrap">
            <img src="https://images.unsplash.com/photo-1565193566173-7a0ee3dbe261?w=500" alt="Làng nghề" />
            <button
              class="blog-heart"
              :class="{ 'heart-pop-active': wishlist['blog-4'], 'is-active': wishlist['blog-4'] }"
              title="Yêu thích"
              @click.stop="toggleWishlist('blog-4')"
            >
              <HeartFilled v-if="wishlist['blog-4']" style="color: #ef4444;" />
              <HeartOutlined v-else />
            </button>
          </div>
          <div class="blog-post-body">
            <h4 class="blog-post-title">Những làng nghề truyền thống không thể bỏ qua</h4>
            <span class="blog-read-time">7 phút đọc</span>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import {
  EnvironmentOutlined,
  CalendarOutlined,
  TeamOutlined,
  AppstoreOutlined,
  SearchOutlined,
  DownOutlined,
  HeartOutlined,
  HeartFilled,
  TrophyOutlined,
  CompassOutlined,
  SafetyCertificateOutlined
} from '@ant-design/icons-vue'
import { useScrollReveal } from '@/composables/useScrollReveal'
import apiClient from '@/services/api'
import type { Category, LocationItem, ApiResponse } from '@/types'
import VietnamMapBanner from '@/components/home/VietnamMapBanner.vue'

const router = useRouter()
const { initScrollReveal } = useScrollReveal()

const searchForm = ref({
  destination: '',
  departureDate: null as any,
  guests: 'Số người',
  categoryId: undefined as number | undefined
})

const categories = ref<Category[]>([])
const locations = ref<LocationItem[]>([])
const wishlist = ref<Record<string, boolean>>({})
const activePin = ref<string>('')

function toggleWishlist(id: string) {
  wishlist.value[id] = !wishlist.value[id]
}

function handleSearch() {
  const query: Record<string, any> = {}
  if (searchForm.value.destination) query.keyword = searchForm.value.destination
  if (searchForm.value.categoryId) query.categoryId = searchForm.value.categoryId
  if (searchForm.value.departureDate) {
    query.departureDate = searchForm.value.departureDate.format('YYYY-MM-DD')
  }
  router.push({ path: '/tours', query })
}

function filterByCategory(name: string) {
  const cleanName = name.toLowerCase()
  const found = categories.value.find(c => 
    c.name.toLowerCase().includes(cleanName) || cleanName.includes(c.name.toLowerCase().split(' ')[0])
  )
  if (found) {
    router.push({ path: '/tours', query: { categoryId: found.id } })
  } else {
    router.push({ path: '/tours', query: { keyword: name } })
  }
}

function filterByDest(name: string) {
  router.push({ path: '/tours', query: { keyword: name } })
}

function filterByMaxPrice(price: number) {
  router.push({ path: '/tours', query: { maxPrice: price } })
}

async function loadData() {
  try {
    const [catRes, locRes] = await Promise.all([
      apiClient.get<ApiResponse<Category[]>>('/master-data/categories'),
      apiClient.get<ApiResponse<LocationItem[]>>('/master-data/locations')
    ])
    categories.value = catRes.data.data || []
    locations.value = locRes.data.data || []
  } catch (error) {
    console.warn('Backend master-data fallback applied', error)
  }
}

onMounted(() => {
  loadData()
  initScrollReveal()
})
</script>

<style scoped>
.home-page {
  background-color: #fafbfc;
  color: #1e293b;
  font-family: 'Plus Jakarta Sans', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
  padding-bottom: 70px;
}

.container {
  max-width: 1240px;
  margin: 0 auto;
  padding: 0 20px;
}

/* SECTION HEADER ROW */
.section-header-row {
  display: flex;
  align-items: flex-end;
  justify-content: space-between;
  margin-bottom: 22px;
}

.section-header-row.compact {
  margin-bottom: 16px;
}

.section-main-title {
  font-size: 24px;
  font-weight: 800;
  color: #0f172a;
  margin-bottom: 4px;
  letter-spacing: -0.02em;
  white-space: nowrap;
}

.section-sub-title {
  font-size: 13.5px;
  color: #64748b;
  margin: 0;
  white-space: nowrap;
}

.see-all-link {
  font-size: 13px;
  font-weight: 600;
  color: #15803d;
  text-decoration: none;
  transition: all 0.2s;
  white-space: nowrap;
}

.see-all-link:hover {
  color: #166534;
  transform: translateX(3px);
}

/* ====================================================================
   1. HERO SECTION
   ==================================================================== */
.hero-section {
  position: relative;
  min-height: 560px;
  display: flex;
  align-items: center;
  padding: 60px 0 70px;
  overflow: hidden;
}

.hero-bg-media {
  position: absolute;
  inset: -40px;
  background-image: url('/hero-banner.jpg');
  background-size: cover;
  background-position: center;
  animation: kenBurns 24s ease-in-out infinite alternate;
  transform-origin: center center;
  will-change: transform;
  pointer-events: none;
}

.hero-bg-overlay {
  position: absolute;
  inset: 0;
  background: linear-gradient(
    90deg,
    rgba(15, 23, 42, 0.78) 0%,
    rgba(15, 23, 42, 0.52) 48%,
    rgba(15, 23, 42, 0.22) 80%,
    rgba(15, 23, 42, 0.08) 100%
  );
  pointer-events: none;
}

.hero-container {
  position: relative;
  z-index: 2;
  width: 100%;
}

/* Floating Sketch Badge (Top Right) */
.hero-sketch-badge {
  position: absolute;
  top: 10px;
  right: 50px;
  width: 140px;
  height: 140px;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 3;
  transform: rotate(-8deg);
  pointer-events: none;
  animation: floatGentle 5.5s ease-in-out infinite;
}

.sketch-badge-svg {
  position: absolute;
  inset: 0;
  width: 100%;
  height: 100%;
}

.sketch-badge-content {
  text-align: center;
  color: #ffffff;
  position: relative;
  z-index: 2;
  padding: 8px;
}

.sketch-sparkle {
  color: #fef08a;
  font-size: 18px;
  display: block;
  margin-bottom: 2px;
}

.sketch-text-main {
  font-size: 13.5px;
  font-weight: 800;
  line-height: 1.25;
  white-space: nowrap;
  text-shadow: 0 2px 4px rgba(0, 0, 0, 0.6);
}

.sketch-text-sub {
  font-size: 12px;
  font-style: italic;
  color: #f1f5f9;
  white-space: nowrap;
  text-shadow: 0 1px 3px rgba(0, 0, 0, 0.6);
}

.hero-top-row {
  margin-bottom: 40px;
}

.hero-text-block {
  max-width: 820px;
}

.hero-title {
  font-size: 42px;
  line-height: 1.22;
  font-weight: 800;
  color: #ffffff;
  margin-bottom: 16px;
  letter-spacing: -0.02em;
  text-shadow: 0 2px 10px rgba(0, 0, 0, 0.55);
  text-wrap: balance;
  word-break: keep-all;
  animation: fadeInUp 0.65s cubic-bezier(0.16, 1, 0.3, 1) both;
}

.hero-subtitle {
  font-size: 16px;
  line-height: 1.6;
  color: #f1f5f9;
  margin: 0;
  max-width: 720px;
  text-shadow: 0 1px 4px rgba(0, 0, 0, 0.45);
  text-wrap: pretty;
  animation: fadeInUp 0.65s cubic-bezier(0.16, 1, 0.3, 1) 0.12s both;
}

/* Wide Pill Search Bar (Full Container Width) */
.hero-search-bar {
  background: #ffffff;
  border-radius: 999px;
  padding: 8px 10px 8px 22px;
  box-shadow: 0 18px 40px rgba(0, 0, 0, 0.25);
  display: flex;
  align-items: center;
  width: 100%;
  max-width: 1140px;
  border: 1px solid transparent;
  transition: transform var(--dur-card, 0.25s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              box-shadow var(--dur-card, 0.25s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              border-color var(--dur-card, 0.25s) ease;
  animation: fadeInUp 0.65s cubic-bezier(0.16, 1, 0.3, 1) 0.22s both;
}

.hero-search-bar:focus-within {
  transform: translateY(-2px);
  box-shadow: 0 22px 50px rgba(20, 83, 45, 0.24);
  border-color: rgba(21, 128, 61, 0.35);
}

.search-cell {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 6px 14px;
  position: relative;
  white-space: nowrap;
  transition: opacity 0.2s ease;
}

.cell-dest { flex: 1.4; min-width: 190px; }
.cell-date { flex: 1.1; min-width: 160px; }
.cell-guests { flex: 0.9; min-width: 130px; }
.cell-cat { flex: 1.2; min-width: 170px; }

.search-icon {
  font-size: 17px;
  color: #15803d;
  flex-shrink: 0;
  transition: transform var(--dur-micro, 0.18s) ease;
}

.search-cell:focus-within .search-icon {
  transform: scale(1.15);
}

.search-input-field {
  border: none;
  outline: none;
  background: transparent;
  font-size: 14px;
  font-weight: 600;
  color: #0f172a;
  width: 100%;
  padding: 0;
  white-space: nowrap;
}

.search-input-field::placeholder {
  color: #64748b;
  font-weight: 500;
}

.search-date-picker {
  padding: 0 !important;
  font-size: 14px !important;
  font-weight: 600 !important;
  width: 100% !important;
}

.search-select-field {
  border: none;
  outline: none;
  background: transparent;
  font-size: 14px;
  font-weight: 600;
  color: #0f172a;
  width: 100%;
  cursor: pointer;
  appearance: none;
  white-space: nowrap;
}

.select-chevron {
  font-size: 11px;
  color: #94a3b8;
  pointer-events: none;
  margin-left: -12px;
  transition: transform 0.2s ease;
}

.search-cell:focus-within .select-chevron {
  transform: rotate(180deg);
  color: #15803d;
}

.search-divider {
  width: 1px;
  height: 32px;
  background: #e2e8f0;
  margin: 0 4px;
}

.search-btn-pill {
  background: #14532d;
  color: #ffffff;
  border: none;
  border-radius: 999px;
  padding: 13px 28px;
  font-size: 14px;
  font-weight: 700;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: all var(--dur-btn, 0.2s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
  white-space: nowrap;
  flex-shrink: 0;
}

.search-btn-pill:hover {
  background: #166534;
  transform: translateY(-2px);
  box-shadow: 0 8px 20px rgba(20, 83, 45, 0.28);
}

.search-btn-pill:active {
  transform: scale(0.97);
}

/* ====================================================================
   2. KHÁM PHÁ THEO DANH MỤC (Pastel Circular Floating Icons)
   ==================================================================== */
.section-categories {
  margin-top: 44px;
  margin-bottom: 52px;
}

.categories-icon-row {
  display: grid;
  grid-template-columns: repeat(6, 1fr);
  gap: 24px 16px;
}

.category-icon-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  cursor: pointer;
  transition: transform var(--dur-card, 0.25s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.category-icon-item:hover {
  transform: translateY(-5px);
}

.cat-circle {
  width: 64px;
  height: 64px;
  border-radius: 50%;
  background-color: #EAF1EA;
  color: #285A45;
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 10px;
  border: 1px solid rgba(40, 90, 69, 0.12);
  transition: transform var(--dur-card, 0.25s) var(--ease-spring, cubic-bezier(0.34, 1.56, 0.64, 1)),
              background-color var(--dur-card, 0.25s) ease,
              border-color var(--dur-card, 0.25s) ease,
              box-shadow var(--dur-card, 0.25s) ease;
}

.category-icon-item:hover .cat-circle {
  transform: scale(1.1);
  background-color: #dbe7db;
  border-color: #285A45;
  box-shadow: 0 8px 20px rgba(40, 90, 69, 0.18);
}

.category-icon-item svg {
  transition: transform var(--dur-micro, 0.18s) var(--ease-spring, cubic-bezier(0.34, 1.56, 0.64, 1));
}

.category-icon-item:hover svg {
  transform: scale(1.16);
}

.cat-name {
  font-size: 13.5px;
  font-weight: 700;
  color: #0f172a;
  margin-bottom: 2px;
  text-align: center;
  white-space: nowrap;
  transition: color var(--dur-micro, 0.18s) ease;
}

.category-icon-item:hover .cat-name {
  color: #15803d;
}

.cat-count {
  font-size: 11px;
  color: #94a3b8;
  white-space: nowrap;
}

/* ====================================================================
   3. TOUR NỔI BẬT (5-Column Grid with Badges)
   ==================================================================== */
.section-featured-tours {
  margin-bottom: 56px;
}

.featured-tours-row {
  display: grid;
  grid-template-columns: repeat(5, 1fr);
  gap: 16px;
}

.tour-card-5 {
  background: #ffffff;
  border-radius: 14px;
  border: 1px solid #f1f5f9;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.03);
  cursor: pointer;
  transition: transform var(--dur-card, 0.28s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              box-shadow var(--dur-card, 0.28s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              border-color var(--dur-card, 0.28s) ease;
  display: flex;
  flex-direction: column;
}

.tour-card-5:hover {
  transform: translateY(-6px);
  box-shadow: 0 16px 32px -6px rgba(0, 0, 0, 0.12);
  border-color: #cbd5e1;
}

.tour-thumb-box {
  position: relative;
  aspect-ratio: 4 / 3;
  overflow: hidden;
}

.tour-thumb-box::after {
  content: '';
  position: absolute;
  inset: 0;
  background: linear-gradient(to top, rgba(0, 0, 0, 0.35) 0%, transparent 40%);
  opacity: 0;
  transition: opacity var(--dur-card, 0.28s) ease;
  pointer-events: none;
}

.tour-card-5:hover .tour-thumb-box::after {
  opacity: 1;
}

.tour-thumb-box img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform var(--dur-zoom, 0.45s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.tour-card-5:hover .tour-thumb-box img {
  transform: scale(1.08);
}

.tour-pill-badge {
  position: absolute;
  top: 10px;
  left: 10px;
  padding: 3px 10px;
  border-radius: 999px;
  font-size: 11px;
  font-weight: 700;
  color: #ffffff;
  white-space: nowrap;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
  z-index: 2;
  transition: transform var(--dur-micro, 0.18s) ease;
}

.tour-card-5:hover .tour-pill-badge {
  transform: scale(1.05);
}

.badge-amber { background: #f59e0b; }
.badge-green { background: #10b981; }
.badge-red { background: #ef4444; }

.tour-wishlist-heart {
  position: absolute;
  top: 10px;
  right: 10px;
  width: 30px;
  height: 30px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.88);
  backdrop-filter: blur(4px);
  border: none;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #64748b;
  font-size: 14px;
  z-index: 2;
  transition: transform var(--dur-micro, 0.18s) var(--ease-spring, cubic-bezier(0.34, 1.56, 0.64, 1)),
              color var(--dur-micro, 0.18s) ease,
              background-color var(--dur-micro, 0.18s) ease,
              box-shadow var(--dur-micro, 0.18s) ease;
}

.tour-wishlist-heart:hover {
  color: #ef4444;
  background: #ffffff;
  transform: scale(1.15);
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.12);
}

.tour-wishlist-heart.is-active {
  color: #ef4444;
  background: #ffffff;
  box-shadow: 0 4px 10px rgba(239, 68, 68, 0.25);
}

.tour-info-box {
  padding: 12px;
  display: flex;
  flex-direction: column;
  flex: 1;
}

.tour-h3 {
  font-size: 13.5px;
  font-weight: 700;
  color: #0f172a;
  line-height: 1.35;
  margin-bottom: 6px;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  height: 36px;
  text-wrap: balance;
  transition: color var(--dur-micro, 0.18s) ease;
}

.tour-card-5:hover .tour-h3 {
  color: #15803d;
}

.tour-loc-row {
  font-size: 11.5px;
  color: #64748b;
  display: flex;
  align-items: center;
  gap: 4px;
  margin-bottom: 8px;
  white-space: nowrap;
}

.tour-rating-time {
  display: flex;
  align-items: center;
  gap: 4px;
  font-size: 11.5px;
  margin-bottom: 10px;
  white-space: nowrap;
}

.t-star { font-weight: 700; color: #d97706; }
.t-reviews { color: #94a3b8; }
.t-sep { color: #cbd5e1; }
.t-duration { color: #64748b; }

.tour-price-line {
  margin-top: auto;
  display: flex;
  align-items: baseline;
  gap: 3px;
  white-space: nowrap;
}

.t-price {
  font-size: 15px;
  font-weight: 800;
  color: #15803d;
  transition: transform var(--dur-micro, 0.18s) ease;
}

.tour-card-5:hover .t-price {
  transform: scale(1.04);
}

.t-unit {
  font-size: 11px;
  color: #94a3b8;
}

/* ====================================================================
   4. NHỮNG VÙNG ĐẤT TRUYỀN CẢM HỨNG (Inspirational Banner)
   ==================================================================== */
.section-inspire {
  margin-bottom: 56px;
}

.inspire-banner {
  position: relative;
  background-image: url('https://images.unsplash.com/photo-1528127269322-539801943592?w=1600&q=80');
  background-size: cover;
  background-position: center;
  border-radius: 20px;
  overflow: hidden;
  padding: 40px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 28px;
  box-shadow: 0 12px 30px -8px rgba(0, 0, 0, 0.18);
}

.inspire-banner::before {
  content: '';
  position: absolute;
  inset: 0;
  background: linear-gradient(
    90deg,
    rgba(15, 23, 42, 0.85) 0%,
    rgba(15, 23, 42, 0.6) 50%,
    rgba(15, 23, 42, 0.35) 100%
  );
}

.inspire-left {
  position: relative;
  z-index: 2;
  max-width: 440px;
  color: #ffffff;
}

.inspire-tag {
  font-size: 12px;
  font-weight: 700;
  color: #86efac;
  margin-bottom: 6px;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  white-space: nowrap;
}

.inspire-title {
  font-size: 30px;
  font-weight: 800;
  color: #ffffff;
  line-height: 1.25;
  margin-bottom: 10px;
  text-wrap: balance;
}

.inspire-desc {
  font-size: 13.5px;
  color: #e2e8f0;
  line-height: 1.5;
  margin-bottom: 22px;
  text-wrap: pretty;
}

.inspire-btn {
  display: inline-flex;
  align-items: center;
  background: rgba(255, 255, 255, 0.16);
  backdrop-filter: blur(8px);
  border: 1px solid rgba(255, 255, 255, 0.35);
  color: #ffffff;
  padding: 10px 22px;
  border-radius: 999px;
  font-size: 13px;
  font-weight: 700;
  text-decoration: none;
  transition: all var(--dur-btn, 0.2s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
  white-space: nowrap;
}

.inspire-btn:hover {
  background: #ffffff;
  color: #0f172a;
  transform: translateY(-2px);
  box-shadow: 0 6px 16px rgba(255, 255, 255, 0.25);
}

.inspire-destinations {
  position: relative;
  z-index: 2;
  display: flex;
  gap: 12px;
}

.insp-card {
  width: 130px;
  border-radius: 12px;
  overflow: hidden;
  position: relative;
  aspect-ratio: 3 / 4;
  cursor: pointer;
  box-shadow: 0 6px 14px rgba(0, 0, 0, 0.25);
  transition: transform var(--dur-card, 0.28s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              box-shadow var(--dur-card, 0.28s) ease;
}

.insp-card:hover {
  transform: translateY(-6px);
  box-shadow: 0 14px 28px rgba(0, 0, 0, 0.35);
}

.insp-card img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform var(--dur-zoom, 0.48s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.insp-card:hover img {
  transform: scale(1.1);
}

.insp-label {
  position: absolute;
  bottom: 0;
  inset-inline: 0;
  background: linear-gradient(to top, rgba(0, 0, 0, 0.85) 0%, rgba(0, 0, 0, 0.3) 70%, transparent 100%);
  padding: 10px 8px;
  color: #ffffff;
  transition: transform var(--dur-card, 0.25s) ease;
}

.insp-card:hover .insp-label {
  transform: translateY(-3px);
}

.insp-label h4 {
  font-size: 13px;
  font-weight: 700;
  margin: 0;
  color: #ffffff;
  white-space: nowrap;
}

.insp-label p {
  font-size: 10px;
  color: #cbd5e1;
  margin: 0;
  white-space: nowrap;
}

/* ====================================================================
   5 & 6. DUAL ROW SECTIONS
   ==================================================================== */
.section-dual {
  display: grid;
  grid-template-columns: 1.15fr 0.85fr;
  gap: 28px;
  margin-bottom: 56px;
}

.dual-title {
  font-size: 19px;
  font-weight: 800;
  color: #0f172a;
  margin-bottom: 2px;
  white-space: nowrap;
}

/* 4-Item Grid */
.grid-4-items {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 12px;
}

.mini-tour-card {
  background: #ffffff;
  border-radius: 12px;
  border: 1px solid #f1f5f9;
  overflow: hidden;
  cursor: pointer;
  transition: transform var(--dur-card, 0.25s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              box-shadow var(--dur-card, 0.25s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              border-color var(--dur-card, 0.25s) ease;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.03);
}

.mini-tour-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 12px 24px -4px rgba(0, 0, 0, 0.09);
  border-color: #cbd5e1;
}

.mini-img-wrap {
  position: relative;
  aspect-ratio: 4 / 3;
  overflow: hidden;
}

.mini-img-wrap img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform var(--dur-zoom, 0.4s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.mini-tour-card:hover .mini-img-wrap img {
  transform: scale(1.08);
}

.mini-heart {
  position: absolute;
  top: 6px;
  right: 6px;
  width: 24px;
  height: 24px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.88);
  border: none;
  font-size: 12px;
  color: #64748b;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: transform var(--dur-micro, 0.18s) var(--ease-spring, cubic-bezier(0.34, 1.56, 0.64, 1)),
              color var(--dur-micro, 0.18s) ease,
              background-color var(--dur-micro, 0.18s) ease;
}

.mini-heart:hover {
  transform: scale(1.15);
  color: #ef4444;
  background: #ffffff;
}

.mini-heart.is-active {
  color: #ef4444;
  background: #ffffff;
}

.mini-body {
  padding: 10px;
}

.mini-body h5 {
  font-size: 12px;
  font-weight: 700;
  color: #0f172a;
  line-height: 1.3;
  margin-bottom: 2px;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  text-wrap: balance;
  transition: color var(--dur-micro, 0.18s) ease;
}

.mini-tour-card:hover .mini-body h5 {
  color: #15803d;
}

.mini-body p {
  font-size: 11px;
  color: #94a3b8;
  margin: 0;
  white-space: nowrap;
}

.text-green { color: #15803d; font-weight: 600; margin: 0; font-size: 11px; white-space: nowrap; }
.text-xs { color: #94a3b8; font-size: 10px; white-space: nowrap; }

/* 3-Item Budget Grid */
.grid-3-items {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 12px;
}

.budget-card-box {
  background: #ffffff;
  border-radius: 12px;
  border: 1px solid #f1f5f9;
  overflow: hidden;
  cursor: pointer;
  transition: transform var(--dur-card, 0.25s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              box-shadow var(--dur-card, 0.25s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              border-color var(--dur-card, 0.25s) ease;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.03);
}

.budget-card-box:hover {
  transform: translateY(-5px);
  box-shadow: 0 14px 28px -4px rgba(0, 0, 0, 0.09);
  border-color: #86efac;
}

.budget-img-box {
  aspect-ratio: 4 / 3;
  overflow: hidden;
}

.budget-img-box img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform var(--dur-zoom, 0.4s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.budget-card-box:hover .budget-img-box img {
  transform: scale(1.08);
}

.budget-desc-box {
  padding: 10px;
}

.budget-desc-box h5 {
  font-size: 12px;
  font-weight: 700;
  color: #0f172a;
  margin-bottom: 2px;
  white-space: nowrap;
  transition: color var(--dur-micro, 0.18s) ease,
              transform var(--dur-micro, 0.18s) ease;
}

.budget-card-box:hover .budget-desc-box h5 {
  color: #15803d;
  transform: translateX(2px);
}

.budget-desc-box p {
  font-size: 11px;
  color: #94a3b8;
  margin: 0;
  white-space: nowrap;
}



/* ====================================================================
   7. TRỞ THÀNH LOCAL HOST (Call to Action Banner)
   ==================================================================== */
.section-host-cta {
  margin-bottom: 56px;
}

.host-cta-panel {
  background: #f0fdf4;
  border: 1px solid #bbf7d0;
  border-radius: 20px;
  padding: 28px;
  display: grid;
  grid-template-columns: 220px 1.2fr 1fr;
  gap: 28px;
  align-items: center;
}

.host-photo-wrap {
  height: 200px;
  border-radius: 14px;
  overflow: hidden;
  box-shadow: 0 8px 16px -4px rgba(22, 101, 52, 0.15);
  animation: floatSoft 6.5s ease-in-out infinite;
}

.host-photo-wrap img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.host-h2 {
  font-size: 22px;
  font-weight: 800;
  color: #14532d;
  margin-bottom: 4px;
  white-space: nowrap;
}

.host-sub-h3 {
  font-size: 15px;
  font-weight: 700;
  color: #15803d;
  line-height: 1.4;
  margin-bottom: 8px;
  text-wrap: balance;
}

.host-p {
  font-size: 12.5px;
  color: #475569;
  line-height: 1.5;
  margin-bottom: 18px;
  text-wrap: pretty;
}

.host-btn-register {
  display: inline-block;
  background: #14532d;
  color: #ffffff;
  padding: 10px 22px;
  border-radius: 999px;
  font-size: 13px;
  font-weight: 700;
  text-decoration: none;
  transition: all var(--dur-btn, 0.2s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
  white-space: nowrap;
}

.host-btn-register:hover {
  background: #166534;
  transform: translateY(-2px);
  box-shadow: 0 8px 20px rgba(20, 83, 45, 0.28);
}

.host-benefits-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 12px;
}

.benefit-box {
  background: #ffffff;
  border: 1px solid #dcfce7;
  border-radius: 12px;
  padding: 12px;
  display: flex;
  flex-direction: column;
  gap: 6px;
  transition: transform var(--dur-card, 0.25s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              box-shadow var(--dur-card, 0.25s) ease,
              border-color var(--dur-card, 0.25s) ease;
}

.benefit-box:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 18px rgba(22, 101, 52, 0.08);
  border-color: #86efac;
}

.benefit-icon-circle {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  background: #dcfce7;
  color: #15803d;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 15px;
  transition: transform var(--dur-micro, 0.18s) var(--ease-spring, cubic-bezier(0.34, 1.56, 0.64, 1)),
              background-color var(--dur-btn, 0.2s) ease,
              color var(--dur-btn, 0.2s) ease;
}

.benefit-box:hover .benefit-icon-circle {
  transform: scale(1.15) rotate(-6deg);
  background: #166534;
  color: #ffffff;
}

.benefit-box h6 {
  font-size: 12.5px;
  font-weight: 700;
  color: #0f172a;
  margin: 0;
  white-space: nowrap;
}

.benefit-box p {
  font-size: 10.5px;
  color: #64748b;
  margin: 0;
  line-height: 1.35;
}

/* ====================================================================
   8. CÂU CHUYỆN TỪ NGƯỜI BẢN ĐỊA (Horizontal Cards)
   ==================================================================== */
.section-stories {
  margin-bottom: 56px;
}

.stories-row-3 {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 18px;
}

.story-h-card {
  background: #ffffff;
  border: 1px solid #f1f5f9;
  border-radius: 16px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.03);
  display: flex;
  transition: transform var(--dur-card, 0.28s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              box-shadow var(--dur-card, 0.28s) ease,
              border-color var(--dur-card, 0.28s) ease;
}

.story-h-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 14px 28px rgba(0, 0, 0, 0.08);
  border-color: #cbd5e1;
}

.story-h-photo {
  width: 40%;
  flex-shrink: 0;
  overflow: hidden;
}

.story-h-photo img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform var(--dur-zoom, 0.45s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.story-h-card:hover .story-h-photo img {
  transform: scale(1.08);
}

.story-h-body {
  padding: 14px;
  display: flex;
  flex-direction: column;
  flex: 1;
}

.story-h-title {
  font-size: 14px;
  font-weight: 700;
  color: #0f172a;
  line-height: 1.3;
  margin-bottom: 6px;
  transition: color var(--dur-micro, 0.18s) ease;
}

.story-h-card:hover .story-h-title {
  color: #15803d;
}

.story-pill-tag {
  align-self: flex-start;
  font-size: 10.5px;
  font-weight: 700;
  padding: 2px 7px;
  border-radius: 4px;
  margin-bottom: 8px;
  white-space: nowrap;
}

.tag-culture { background: #fee2e2; color: #dc2626; }
.tag-people { background: #dcfce7; color: #15803d; }
.tag-food { background: #fef3c7; color: #d97706; }

.story-h-desc {
  font-size: 12px;
  color: #64748b;
  line-height: 1.45;
  margin-bottom: 10px;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.story-read-link {
  margin-top: auto;
  font-size: 12px;
  font-weight: 700;
  color: #15803d;
  text-decoration: none;
  white-space: nowrap;
  display: inline-block;
  transition: transform var(--dur-btn, 0.2s) ease,
              color var(--dur-btn, 0.2s) ease;
}

.story-h-card:hover .story-read-link {
  transform: translateX(4px);
  color: #166534;
}

/* ====================================================================
   9. NHỮNG HOST NỔI BẬT (Horizontal with Arrow Dividers)
   ==================================================================== */
.section-featured-hosts {
  margin-bottom: 56px;
}

.hosts-connected-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
}

.host-pill-card {
  background: #ffffff;
  border: 1px solid #f1f5f9;
  border-radius: 14px;
  padding: 14px;
  display: flex;
  align-items: center;
  gap: 12px;
  flex: 1;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.03);
  transition: transform var(--dur-card, 0.26s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              box-shadow var(--dur-card, 0.26s) ease,
              border-color var(--dur-card, 0.26s) ease;
}

.host-pill-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 12px 24px rgba(0, 0, 0, 0.07);
  border-color: #86efac;
}

.host-circle-avatar {
  width: 52px;
  height: 52px;
  border-radius: 50%;
  overflow: hidden;
  flex-shrink: 0;
  border: 2px solid #86efac;
  transition: border-color var(--dur-btn, 0.2s) ease,
              box-shadow var(--dur-btn, 0.2s) ease;
}

.host-pill-card:hover .host-circle-avatar {
  border-color: #15803d;
  box-shadow: 0 0 0 3px rgba(21, 128, 61, 0.2);
}

.host-circle-avatar img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform var(--dur-card, 0.3s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.host-pill-card:hover .host-circle-avatar img {
  transform: scale(1.12);
}

.host-card-info {
  flex: 1;
}

.h-name {
  font-size: 13px;
  font-weight: 700;
  color: #0f172a;
  margin-bottom: 2px;
  white-space: nowrap;
  transition: color var(--dur-micro, 0.18s) ease;
}

.host-pill-card:hover .h-name {
  color: #15803d;
}

.h-location {
  font-size: 11px;
  color: #64748b;
  margin-bottom: 3px;
  white-space: nowrap;
}

.h-rating-tours {
  font-size: 11px;
  margin-bottom: 4px;
  white-space: nowrap;
}

.h-rating-tours .star {
  font-weight: 700;
  color: #d97706;
  display: inline-block;
  transition: transform var(--dur-micro, 0.18s) var(--ease-spring, cubic-bezier(0.34, 1.56, 0.64, 1));
}

.host-pill-card:hover .star {
  transform: scale(1.25);
}

.h-rating-tours .num { color: #94a3b8; }
.h-rating-tours .dot { color: #cbd5e1; margin: 0 2px; }
.h-rating-tours .tour-cnt { color: #64748b; }

.h-profile-link {
  font-size: 11.5px;
  font-weight: 700;
  color: #15803d;
  text-decoration: none;
  white-space: nowrap;
  display: inline-block;
  transition: transform var(--dur-btn, 0.2s) ease;
}

.host-pill-card:hover .h-profile-link {
  transform: translateX(3px);
}

.host-arrow-divider {
  font-size: 18px;
  color: #cbd5e1;
  font-weight: 700;
  flex-shrink: 0;
  animation: arrowSlideRight 2.4s infinite ease-in-out;
}

/* ====================================================================
   10. KHÁCH HÀNG NÓI GÌ VỀ CHÚNG TÔI (Horizontal Photo + Quote)
   ==================================================================== */
.section-testimonials {
  margin-bottom: 56px;
}

.reviews-4-row {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 14px;
}

.review-h-box {
  background: #ffffff;
  border: 1px solid #f1f5f9;
  border-radius: 14px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.03);
  display: flex;
  transition: transform var(--dur-card, 0.26s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              box-shadow var(--dur-card, 0.26s) ease,
              border-color var(--dur-card, 0.26s) ease;
}

.review-h-box:hover {
  transform: translateY(-4px);
  box-shadow: 0 12px 24px rgba(0, 0, 0, 0.08);
  border-color: #86efac;
}

.review-thumb-sq {
  width: 80px;
  flex-shrink: 0;
  overflow: hidden;
}

.review-thumb-sq img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform var(--dur-zoom, 0.4s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.review-h-box:hover .review-thumb-sq img {
  transform: scale(1.08);
}

.review-quote-side {
  padding: 10px 12px;
  display: flex;
  flex-direction: column;
  flex: 1;
}

.reviewer-name {
  font-size: 12.5px;
  font-weight: 700;
  color: #0f172a;
  margin-bottom: 2px;
  white-space: nowrap;
}

.reviewer-stars {
  color: #f59e0b;
  font-size: 10.5px;
  margin-bottom: 4px;
  white-space: nowrap;
  display: inline-block;
  transition: transform var(--dur-micro, 0.18s) var(--ease-spring, cubic-bezier(0.34, 1.56, 0.64, 1));
}

.review-h-box:hover .reviewer-stars {
  transform: scale(1.1);
}

.reviewer-quote {
  font-size: 11px;
  color: #475569;
  line-height: 1.4;
  margin: 0;
  font-style: italic;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

/* ====================================================================
   11. HƯỚNG DẪN CÁCH ĐẶT TOUR (4 Connected Steps)
   ==================================================================== */
.section-how {
  margin-bottom: 56px;
}

.steps-flow-card {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 12px;
  background: #ffffff;
  border: 1px solid #f1f5f9;
  border-radius: 18px;
  padding: 20px 24px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.03);
}

.step-flow-item {
  display: flex;
  align-items: center;
  gap: 12px;
  flex: 1;
  transition: transform var(--dur-card, 0.22s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.step-flow-item:hover {
  transform: translateY(-3px);
}

.step-green-number {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: #14532d;
  color: #ffffff;
  font-size: 15px;
  font-weight: 800;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  transition: transform var(--dur-card, 0.22s) var(--ease-spring, cubic-bezier(0.34, 1.56, 0.64, 1)),
              background-color var(--dur-btn, 0.2s) ease,
              box-shadow var(--dur-btn, 0.2s) ease;
}

.step-flow-item:hover .step-green-number {
  transform: scale(1.15);
  background: #166534;
  box-shadow: 0 6px 14px rgba(20, 83, 45, 0.32);
}

.step-flow-text h5 {
  font-size: 13.5px;
  font-weight: 700;
  color: #0f172a;
  margin-bottom: 2px;
  white-space: nowrap;
}

.step-flow-text p {
  font-size: 11.5px;
  color: #64748b;
  margin: 0;
  white-space: nowrap;
}

.step-arrow-flow {
  font-size: 18px;
  color: #cbd5e1;
  font-weight: 700;
  flex-shrink: 0;
  animation: arrowSlideRight 2.2s infinite ease-in-out;
}

/* ====================================================================
   12. BLOG & CẨM NANG DU LỊCH (4-Column Grid)
   ==================================================================== */
.section-blog {
  margin-bottom: 30px;
}

.blog-cards-4 {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 16px;
}

.blog-post-card {
  background: #ffffff;
  border: 1px solid #f1f5f9;
  border-radius: 14px;
  overflow: hidden;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.03);
  cursor: pointer;
  transition: transform var(--dur-card, 0.26s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1)),
              box-shadow var(--dur-card, 0.26s) ease,
              border-color var(--dur-card, 0.26s) ease;
}

.blog-post-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 14px 28px -4px rgba(0, 0, 0, 0.1);
  border-color: #cbd5e1;
}

.blog-thumb-wrap {
  position: relative;
  aspect-ratio: 16 / 10;
  overflow: hidden;
}

.blog-thumb-wrap img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform var(--dur-zoom, 0.45s) var(--ease-natural, cubic-bezier(0.16, 1, 0.3, 1));
}

.blog-post-card:hover .blog-thumb-wrap img {
  transform: scale(1.08);
}

.blog-heart {
  position: absolute;
  top: 8px;
  right: 8px;
  width: 28px;
  height: 28px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.88);
  border: none;
  font-size: 13px;
  color: #64748b;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: transform var(--dur-micro, 0.18s) var(--ease-spring, cubic-bezier(0.34, 1.56, 0.64, 1)),
              color var(--dur-micro, 0.18s) ease,
              background-color var(--dur-micro, 0.18s) ease;
}

.blog-heart:hover {
  transform: scale(1.15);
  color: #ef4444;
  background: #ffffff;
}

.blog-heart.is-active {
  color: #ef4444;
  background: #ffffff;
}

.blog-post-body {
  padding: 12px;
}

.blog-post-title {
  font-size: 13.5px;
  font-weight: 700;
  color: #0f172a;
  line-height: 1.35;
  margin-bottom: 6px;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  height: 36px;
  text-wrap: balance;
  transition: color var(--dur-micro, 0.18s) ease;
}

.blog-post-card:hover .blog-post-title {
  color: #15803d;
}

.blog-read-time {
  font-size: 11.5px;
  color: #94a3b8;
  white-space: nowrap;
}

/* ====================================================================
   RESPONSIVE DESIGN
   ==================================================================== */
@media (max-width: 1100px) {
  .hero-search-bar {
    max-width: 100%;
  }
  .featured-tours-row {
    grid-template-columns: repeat(3, 1fr);
  }
  .inspire-banner {
    flex-direction: column;
    align-items: flex-start;
  }
  .inspire-destinations {
    width: 100%;
    overflow-x: auto;
    padding-bottom: 8px;
  }
  .section-dual {
    grid-template-columns: 1fr;
  }
  .host-cta-panel {
    grid-template-columns: 1fr;
  }
}

@media (max-width: 900px) {
  .hero-title {
    font-size: 34px;
  }
  .hero-sketch-badge {
    right: 20px;
    top: 5px;
  }
  .hero-search-bar {
    flex-wrap: wrap;
    border-radius: 20px;
    padding: 14px;
    gap: 8px;
  }
  .search-divider {
    display: none;
  }
  .search-cell {
    flex: 1 1 45%;
    border-bottom: 1px solid #f1f5f9;
  }
  .search-btn-pill {
    width: 100%;
    justify-content: center;
    margin-top: 6px;
  }
}

@media (max-width: 1024px) {
  .categories-icon-row {
    grid-template-columns: repeat(4, 1fr);
    gap: 20px 12px;
  }
}

@media (max-width: 650px) {
  .hero-title {
    font-size: 28px;
  }
  .search-cell {
    flex: 1 1 100%;
  }
  .categories-icon-row {
    grid-template-columns: repeat(3, 1fr);
    gap: 16px 10px;
  }
  .featured-tours-row {
    grid-template-columns: repeat(2, 1fr);
  }
  .grid-4-items,
  .grid-3-items,
  .stories-row-3,
  .reviews-4-row,
  .blog-cards-4 {
    grid-template-columns: repeat(2, 1fr);
  }
  .hosts-connected-row,
  .steps-flow-card {
    flex-direction: column;
  }
  .host-arrow-divider,
  .step-arrow-flow {
    transform: rotate(90deg);
  }
}

@media (max-width: 440px) {
  .categories-icon-row {
    grid-template-columns: repeat(2, 1fr);
    gap: 14px 8px;
  }
  .featured-tours-row,
  .grid-4-items,
  .grid-3-items,
  .stories-row-3,
  .reviews-4-row,
  .blog-cards-4 {
    grid-template-columns: 1fr;
  }
}
</style>
