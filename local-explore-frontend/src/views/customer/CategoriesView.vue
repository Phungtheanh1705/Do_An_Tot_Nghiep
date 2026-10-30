<template>
  <div class="categories-page">
    <!-- Hero Banner -->
    <section class="cat-hero">
      <div class="container hero-content">
        <a-breadcrumb class="hero-breadcrumb">
          <a-breadcrumb-item><router-link to="/">Trang chủ</router-link></a-breadcrumb-item>
          <a-breadcrumb-item>Danh mục trải nghiệm</a-breadcrumb-item>
        </a-breadcrumb>

        <span class="hero-badge">KHÁM PHÁ THEO CHỦ ĐỀ</span>
        <h1 class="hero-title">Đa dạng sắc màu trải nghiệm bản địa</h1>
        <p class="hero-subtitle">
          Từ những xưởng gốm trăm năm, nương chè mờ sương Tây Bắc đến những mâm cơm gia đình ấm cúng miền Tây sông nước.
        </p>
      </div>
    </section>

    <!-- Main Categories Grid -->
    <section class="container categories-body">
      <!-- Quick Filter Chips -->
      <div class="filter-chips-row">
        <button
          class="chip-btn"
          :class="{ active: selectedCatId === undefined }"
          @click="selectedCatId = undefined"
        >
          🌟 Tất cả danh mục ({{ categories.length }})
        </button>
        <button
          v-for="cat in categories"
          :key="cat.id"
          class="chip-btn"
          :class="{ active: selectedCatId === cat.id }"
          @click="selectedCatId = cat.id"
        >
          {{ getCategoryEmoji(cat.name) }} {{ cat.name }}
        </button>
      </div>

      <!-- Category Cards Showcase -->
      <div class="categories-grid">
        <div
          v-for="cat in filteredCategories"
          :key="cat.id"
          class="cat-card-showcase"
        >
          <div class="cat-card-media">
            <img :src="cat.imageUrl" :alt="cat.name" class="cat-img" />
            <div class="cat-overlay"></div>
            <div class="cat-badge-float">
              <span>{{ getCategoryEmoji(cat.name) }}</span>
            </div>
          </div>

          <div class="cat-card-info">
            <div class="cat-header">
              <h2 class="cat-name">{{ cat.name }}</h2>
              <span class="cat-tour-count">{{ getTourCount(cat.id) }} trải nghiệm</span>
            </div>

            <p class="cat-desc">{{ cat.description }}</p>

            <!-- Highlights tags -->
            <div class="cat-tags">
              <span v-for="tag in getCategoryTags(cat.name)" :key="tag" class="tag-pill">
                {{ tag }}
              </span>
            </div>

            <div class="cat-footer">
              <router-link
                :to="{ path: '/tours', query: { categoryId: cat.id } }"
                class="explore-btn"
              >
                <span>Xem các tour {{ cat.name }}</span>
                <ArrowRightOutlined />
              </router-link>
            </div>
          </div>
        </div>
      </div>

      <!-- Inspiration Callout Banner -->
      <div class="inspire-box glass-panel">
        <div class="inspire-text">
          <h3>Bạn chưa tìm thấy chủ đề yêu thích?</h3>
          <p>LocalExplore liên tục kết nối với các nghệ nhân và người bản địa để ra mắt những hành trình mới mỗi tuần.</p>
        </div>
        <div class="inspire-actions">
          <router-link to="/tours" class="btn-solid">Duyệt toàn bộ tour</router-link>
          <router-link to="/become-host" class="btn-outline">Trở thành Host chia sẻ nghề</router-link>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { ArrowRightOutlined } from '@ant-design/icons-vue'
import apiClient from '@/services/api'
import type { Category, ApiResponse, TourSummary, PagedResponse } from '@/types'

const loading = ref(false)
const categories = ref<Category[]>([])
const selectedCatId = ref<number | undefined>(undefined)
const tours = ref<TourSummary[]>([])

const filteredCategories = computed(() => {
  if (!selectedCatId.value) return categories.value
  return categories.value.filter(c => c.id === selectedCatId.value)
})

function getCategoryEmoji(name: string) {
  if (name.includes('Làng nghề')) return '🏺'
  if (name.includes('Nông nghiệp')) return '🌾'
  if (name.includes('Ẩm thực')) return '🍲'
  if (name.includes('Văn hóa')) return '🎭'
  if (name.includes('Trekking')) return '🥾'
  return '✨'
}

function getCategoryTags(name: string) {
  if (name.includes('Làng nghề')) {
    return ['Gốm Bát Tràng', 'Dệt lanh Sa Pa', 'Lồng đèn Hội An', 'Nón lá làng Chuông']
  }
  if (name.includes('Nông nghiệp')) {
    return ['Hái chè cổ thụ', 'Rau hữu cơ Trà Quế', 'Làm vườn miền Tây', 'Mùa mật ong hoa rừng']
  }
  if (name.includes('Ẩm thực')) {
    return ['Bánh xèo miền Tây', 'Nấu cơm niêu đất', 'Làm gia vị thảo mộc', 'Bữa cơm sum họp bản địa']
  }
  if (name.includes('Văn hóa')) {
    return ['Lễ hội truyền thống', 'Đờn ca tài tử', 'Hát then đàn tính', 'Phong tục người H\'Mông']
  }
  if (name.includes('Trekking')) {
    return ['Ruộng bậc thang', 'Đèo Mã Pí Lèng', 'Bản Tả Phìn', 'Ngủ homestay nhà sàn']
  }
  return ['Văn hóa địa phương', 'Trải nghiệm thực tế', 'Nghệ nhân dẫn dắt']
}

function getTourCount(catId: number) {
  const count = tours.value.filter(t => (t as any).categoryId === catId || (t as any).categoryName === categories.value.find(c => c.id === catId)?.name).length
  return count > 0 ? `${count}+` : 'Đang mở'
}

async function loadData() {
  loading.value = true
  try {
    const [catRes, tourRes] = await Promise.all([
      apiClient.get<ApiResponse<Category[]>>('/master-data/categories'),
      apiClient.get<PagedResponse<TourSummary>>('/tours?page=1&pageSize=50')
    ])
    categories.value = catRes.data.data || []
    tours.value = tourRes.data.items || []
  } catch (error) {
    console.error('Failed to load categories', error)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  loadData()
})
</script>

<style scoped>
.categories-page {
  background-color: #fafbfc;
  min-height: 100vh;
  padding-bottom: 80px;
}

/* Hero Section */
.cat-hero {
  background: linear-gradient(135deg, #1B4332 0%, #2D6A4F 60%, #153e2d 100%);
  color: #fff;
  padding: 48px 0 60px;
  position: relative;
  overflow: hidden;
}

.cat-hero::after {
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

/* Body */
.categories-body {
  padding-top: 40px;
}

/* Filter Chips */
.filter-chips-row {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
  margin-bottom: 36px;
}

.chip-btn {
  background: #fff;
  border: 1px solid #e2e8f0;
  color: #475569;
  font-size: 14px;
  font-weight: 600;
  padding: 8px 18px;
  border-radius: 24px;
  cursor: pointer;
  transition: all 0.2s ease;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.04);
}

.chip-btn:hover {
  border-color: #1B4332;
  color: #1B4332;
  transform: translateY(-1px);
}

.chip-btn.active {
  background: #1B4332;
  border-color: #1B4332;
  color: #fff;
  box-shadow: 0 4px 12px rgba(27, 67, 50, 0.25);
}

/* Categories Grid */
.categories-grid {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 28px;
  margin-bottom: 48px;
}

@media (max-width: 900px) {
  .categories-grid {
    grid-template-columns: 1fr;
  }
}

.cat-card-showcase {
  background: #fff;
  border-radius: 16px;
  overflow: hidden;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.06);
  border: 1px solid #f1f5f9;
  display: flex;
  flex-direction: column;
  transition: transform 0.25s ease, box-shadow 0.25s ease;
}

.cat-card-showcase:hover {
  transform: translateY(-4px);
  box-shadow: 0 12px 30px rgba(0, 0, 0, 0.1);
}

.cat-card-media {
  position: relative;
  height: 220px;
  overflow: hidden;
}

.cat-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.4s ease;
}

.cat-card-showcase:hover .cat-img {
  transform: scale(1.05);
}

.cat-overlay {
  position: absolute;
  top: 0; right: 0; bottom: 0; left: 0;
  background: linear-gradient(to top, rgba(0, 0, 0, 0.4) 0%, transparent 60%);
}

.cat-badge-float {
  position: absolute;
  bottom: 16px;
  left: 20px;
  width: 44px;
  height: 44px;
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(8px);
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 22px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.12);
}

.cat-card-info {
  padding: 24px;
  display: flex;
  flex-direction: column;
  flex: 1;
}

.cat-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 8px;
}

.cat-name {
  font-size: 20px;
  font-weight: 800;
  color: #1e293b;
  margin: 0;
}

.cat-tour-count {
  font-size: 12px;
  font-weight: 700;
  color: #1B4332;
  background: #DCFCE7;
  padding: 3px 10px;
  border-radius: 12px;
}

.cat-desc {
  font-size: 14px;
  color: #64748b;
  line-height: 1.55;
  margin-bottom: 16px;
  flex: 1;
}

.cat-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 6px;
  margin-bottom: 20px;
}

.tag-pill {
  font-size: 12px;
  background: #f8fafc;
  border: 1px solid #e2e8f0;
  color: #475569;
  padding: 3px 10px;
  border-radius: 6px;
}

.cat-footer {
  border-top: 1px solid #f1f5f9;
  padding-top: 16px;
}

.explore-btn {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  color: #1B4332;
  font-size: 14px;
  font-weight: 700;
  text-decoration: none;
  transition: gap 0.2s ease, color 0.2s ease;
}

.explore-btn:hover {
  color: #2D6A4F;
  gap: 12px;
}

/* Inspire Callout Box */
.inspire-box {
  background: linear-gradient(135deg, #FAF9F6 0%, #edf7f2 100%);
  border: 1px solid #d1fae5;
  border-radius: 16px;
  padding: 36px 40px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 30px;
}

@media (max-width: 800px) {
  .inspire-box {
    flex-direction: column;
    text-align: center;
  }
}

.inspire-text h3 {
  font-size: 20px;
  font-weight: 800;
  color: #1B4332;
  margin-bottom: 6px;
}

.inspire-text p {
  font-size: 14px;
  color: #475569;
  margin: 0;
  max-width: 540px;
}

.inspire-actions {
  display: flex;
  gap: 12px;
  flex-shrink: 0;
}

.btn-solid {
  background: #1B4332;
  color: #fff;
  font-size: 14px;
  font-weight: 700;
  padding: 10px 22px;
  border-radius: 8px;
  text-decoration: none;
  transition: background 0.2s;
}

.btn-solid:hover {
  background: #2D6A4F;
  color: #fff;
}

.btn-outline {
  background: transparent;
  color: #1B4332;
  border: 1.5px solid #1B4332;
  font-size: 14px;
  font-weight: 700;
  padding: 10px 22px;
  border-radius: 8px;
  text-decoration: none;
  transition: all 0.2s;
}

.btn-outline:hover {
  background: #1B4332;
  color: #fff;
}
</style>
