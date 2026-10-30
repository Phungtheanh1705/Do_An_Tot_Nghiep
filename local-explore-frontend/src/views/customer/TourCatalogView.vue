<template>
  <div class="catalog-page container">
    <div class="catalog-header">
      <h1 class="page-title">Khám phá các tour trải nghiệm bản địa</h1>
      <p class="page-subtitle">Tìm kiếm và chọn lựa chuyến đi mang đậm văn hóa theo sở thích của bạn</p>
    </div>

    <div class="catalog-layout">
      <!-- Filter Sidebar -->
      <aside class="filter-sidebar">
        <div class="filter-card">
          <div class="filter-head">
            <h3>Bộ lọc tìm kiếm</h3>
            <button class="reset-btn" @click="resetFilters">Đặt lại</button>
          </div>

          <!-- Keyword -->
          <div class="filter-group">
            <label class="group-label">Từ khóa</label>
            <a-input
              v-model:value="filters.keyword"
              placeholder="Tên tour, làng nghề, món ăn..."
              allow-clear
              @pressEnter="loadTours"
            >
              <template #prefix><SearchOutlined /></template>
            </a-input>
          </div>

          <!-- Categories -->
          <div class="filter-group">
            <label class="group-label">Danh mục trải nghiệm</label>
            <a-select
              v-model:value="filters.categoryId"
              placeholder="Tất cả danh mục"
              :options="categoryOptions"
              allow-clear
              style="width: 100%"
              @change="loadTours"
            />
          </div>

          <!-- Locations -->
          <div class="filter-group">
            <label class="group-label">Điểm đến (Tỉnh / Thành)</label>
            <a-select
              v-model:value="filters.locationId"
              placeholder="Tất cả địa điểm"
              :options="locationOptions"
              allow-clear
              style="width: 100%"
              @change="loadTours"
            />
          </div>

          <!-- Price Range Slider -->
          <div class="filter-group">
            <label class="group-label">Khoảng giá (VNĐ)</label>
            <a-slider
              v-model:value="priceRange"
              range
              :min="0"
              :max="2000000"
              :step="50000"
              @afterChange="onPriceChange"
            />
            <div class="price-range-text">
              <span>{{ formatPrice(priceRange[0]) }}đ</span> - <span>{{ formatPrice(priceRange[1]) }}đ</span>
            </div>
          </div>

          <!-- Min Rating -->
          <div class="filter-group">
            <label class="group-label">Đánh giá tối thiểu</label>
            <a-radio-group v-model:value="filters.minRating" @change="loadTours">
              <a-radio :value="undefined">Tất cả</a-radio>
              <a-radio :value="4.5">★ 4.5 trở lên</a-radio>
              <a-radio :value="4.0">★ 4.0 trở lên</a-radio>
            </a-radio-group>
          </div>

          <a-button type="primary" block @click="loadTours">
            Áp dụng bộ lọc
          </a-button>
        </div>
      </aside>

      <!-- Main Results Area -->
      <section class="results-area">
        <!-- Top Sort & Summary Bar -->
        <div class="sort-bar">
          <div class="total-text">
            Tìm thấy <strong>{{ totalCount }}</strong> tour phù hợp
          </div>

          <div class="sort-controls">
            <div class="view-toggle">
              <a-radio-group v-model:value="viewMode" button-style="solid" size="small" @change="onViewModeChange">
                <a-radio-button value="grid">
                  <AppstoreOutlined /> Dạng Lưới
                </a-radio-button>
                <a-radio-button value="map">
                  <CompassOutlined /> Bản Đồ
                </a-radio-button>
              </a-radio-group>
            </div>

            <div class="sort-select">
              <span>Sắp xếp: </span>
              <a-select
                v-model:value="filters.sortBy"
                style="width: 170px"
                @change="loadTours"
              >
                <a-select-option value="newest">Mới nhất</a-select-option>
                <a-select-option value="rating">Đánh giá cao nhất</a-select-option>
                <a-select-option value="price_asc">Giá tăng dần</a-select-option>
                <a-select-option value="price_desc">Giá giảm dần</a-select-option>
              </a-select>
            </div>
          </div>
        </div>

        <!-- Tour Cards and Map Area with LocalExplore Custom Loading Animation -->
        <TourLoadingSpin
          v-if="loading"
          tip="Đang tìm kiếm & tải các trải nghiệm bản địa"
          min-height="420px"
        />

        <div v-else-if="tours.length > 0" :class="['catalog-content', { 'split-view': viewMode === 'map' }]">
            <!-- Cards Column -->
            <div :class="['cards-pane', { 'two-cols': viewMode === 'map', 'three-cols': viewMode === 'grid' }]">
              <div
                v-for="tour in tours"
                :key="tour.id"
                class="tour-card"
                :class="{ 'active-card': activeTourId === tour.id }"
                @click="onTourCardClick(tour)"
                @mouseenter="highlightMarker(tour.id)"
              >
                <div class="tour-img-wrapper">
                  <img :src="tour.coverImageUrl" :alt="tour.title" class="tour-img" />
                  <span class="category-tag">{{ tour.categoryName }}</span>
                  <span class="location-badge">
                    <EnvironmentOutlined /> {{ tour.provinceName }}
                  </span>
                </div>

                <div class="tour-body">
                  <div class="host-info">
                    <a-avatar :src="tour.hostAvatarUrl" :size="24">
                      {{ tour.hostName.charAt(0) }}
                    </a-avatar>
                    <span class="host-name">Host {{ tour.hostName }}</span>
                  </div>

                  <h3 class="tour-title" :title="tour.title">{{ tour.title }}</h3>
                  <p class="tour-desc">{{ tour.shortDescription }}</p>

                  <div class="tour-meta">
                    <div class="rating">
                      <StarFilled class="star-icon" />
                      <span class="score">{{ tour.averageRating.toFixed(1) }}</span>
                      <span class="count">({{ tour.reviewCount }})</span>
                    </div>
                    <div class="duration">
                      <ClockCircleOutlined /> {{ tour.durationHours }} giờ
                    </div>
                  </div>

                  <div class="tour-footer">
                    <div class="price-box">
                      <span class="price-label">Giá từ</span>
                      <span class="price-value">{{ formatPrice(tour.basePrice) }}đ</span>
                      <span class="price-unit">/ khách</span>
                    </div>
                    <a-button type="primary" size="small" @click.stop="$router.push(`/tours/${tour.slug}`)">Đặt ngay</a-button>
                  </div>
                </div>
              </div>
            </div>

            <!-- Sticky Leaflet Map Column -->
            <div v-show="viewMode === 'map'" class="map-pane">
              <div class="map-sticky-wrapper">
                <div class="map-header-bar">
                  <span><EnvironmentOutlined /> Bản đồ các điểm trải nghiệm bản địa</span>
                  <small>{{ tours.filter(t => t.latitude).length }} điểm ghim</small>
                </div>
                <div id="catalog-map" class="leaflet-map-box"></div>
              </div>
            </div>
          </div>

          <a-empty
            v-else
            description="Không tìm thấy tour phù hợp với bộ lọc hiện tại."
            style="margin: 60px 0;"
          />

        <!-- Pagination -->
        <div v-if="totalCount > filters.pageSize" class="pagination-wrapper">
          <a-pagination
            v-model:current="filters.pageIndex"
            :total="totalCount"
            :page-size="filters.pageSize"
            show-less-items
            @change="loadTours"
          />
        </div>
      </section>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, watch, nextTick } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import L from 'leaflet'
import {
  SearchOutlined,
  EnvironmentOutlined,
  StarFilled,
  ClockCircleOutlined,
  AppstoreOutlined,
  CompassOutlined
} from '@ant-design/icons-vue'
import apiClient from '@/services/api'
import type { TourSummary, Category, LocationItem, PagedResponse, ApiResponse } from '@/types'
import TourLoadingSpin from '@/components/common/TourLoadingSpin.vue'

const route = useRoute()
const router = useRouter()
const loading = ref(false)
const tours = ref<TourSummary[]>([])
const totalCount = ref(0)
const initialMaxPrice = route.query.maxPrice ? Number(route.query.maxPrice) : undefined
const priceRange = ref<[number, number]>([0, initialMaxPrice || 2000000])
const viewMode = ref<'grid' | 'map'>((route.query.view as string) === 'map' ? 'map' : 'grid')
const activeTourId = ref<number | null>(null)

let mapInstance: L.Map | null = null
let markersGroup: L.FeatureGroup | null = null
const markersMap = new Map<number, L.Marker>()

const categoryOptions = ref<{ label: string; value: number }[]>([])
const locationOptions = ref<{ label: string; value: number }[]>([])

const filters = reactive({
  keyword: (route.query.keyword as string) || '',
  categoryId: route.query.categoryId ? Number(route.query.categoryId) : undefined,
  locationId: route.query.locationId ? Number(route.query.locationId) : undefined,
  minPrice: undefined as number | undefined,
  maxPrice: initialMaxPrice,
  minRating: undefined as number | undefined,
  sortBy: 'newest',
  pageIndex: 1,
  pageSize: 9
})

function formatPrice(val: number) {
  return new Intl.NumberFormat('vi-VN').format(val)
}

function onPriceChange(values: number[] | [number, number]) {
  if (Array.isArray(values) && values.length === 2) {
    filters.minPrice = values[0]
    filters.maxPrice = values[1]
    loadTours()
  }
}

function resetFilters() {
  filters.keyword = ''
  filters.categoryId = undefined
  filters.locationId = undefined
  filters.minPrice = undefined
  filters.maxPrice = undefined
  filters.minRating = undefined
  filters.sortBy = 'newest'
  filters.pageIndex = 1
  priceRange.value = [0, 2000000]
  loadTours()
}

function onViewModeChange() {
  if (viewMode.value === 'map') {
    nextTick(() => {
      initOrUpdateMap()
    })
  }
}

function onTourCardClick(tour: TourSummary) {
  if (viewMode.value === 'grid') {
    router.push(`/tours/${tour.slug}`)
  } else {
    activeTourId.value = tour.id
    if (tour.latitude && tour.longitude && mapInstance) {
      mapInstance.flyTo([Number(tour.latitude), Number(tour.longitude)], 13, { duration: 1 })
      const marker = markersMap.get(tour.id)
      if (marker) {
        marker.openPopup()
      }
    } else {
      router.push(`/tours/${tour.slug}`)
    }
  }
}

function highlightMarker(tourId: number) {
  if (viewMode.value !== 'map') return
  activeTourId.value = tourId
}

function initOrUpdateMap() {
  const container = document.getElementById('catalog-map')
  if (!container) return

  if (!mapInstance) {
    mapInstance = L.map('catalog-map').setView([16.0, 106.0], 6)
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors'
    }).addTo(mapInstance)
    markersGroup = L.featureGroup().addTo(mapInstance)
  }

  if (markersGroup) {
    markersGroup.clearLayers()
    markersMap.clear()

    const validTours = tours.value.filter(t => t.latitude && t.longitude && Number(t.latitude) !== 0)
    if (validTours.length > 0) {
      validTours.forEach(tour => {
        const lat = Number(tour.latitude)
        const lng = Number(tour.longitude)
        const marker = L.marker([lat, lng])
        
        const popupHtml = `
          <div style="width: 220px; font-family: sans-serif; font-size: 13px;">
            <img src="${tour.coverImageUrl}" style="width: 100%; height: 110px; object-fit: cover; border-radius: 6px; margin-bottom: 8px;" />
            <div style="font-size: 10px; color: #008080; font-weight: 700; text-transform: uppercase;">${tour.categoryName}</div>
            <div style="font-weight: 700; color: #0f172a; margin: 4px 0; line-height: 1.3;">${tour.title}</div>
            <div style="font-size: 11px; color: #64748b; margin-bottom: 8px;">📍 ${tour.provinceName} • ★ ${tour.averageRating.toFixed(1)}</div>
            <div style="display: flex; justify-content: space-between; align-items: center; border-top: 1px solid #f1f5f9; padding-top: 6px;">
              <span style="color: #008080; font-weight: 800;">${formatPrice(tour.basePrice)}đ</span>
              <a href="/tours/${tour.slug}" style="background: #008080; color: #fff; padding: 3px 8px; border-radius: 4px; font-size: 11px; text-decoration: none; font-weight: 600;">Xem chi tiết</a>
            </div>
          </div>
        `
        marker.bindPopup(popupHtml)
        marker.on('click', () => {
          activeTourId.value = tour.id
        })

        markersGroup!.addLayer(marker)
        markersMap.set(tour.id, marker)
      })

      try {
        mapInstance.fitBounds(markersGroup.getBounds(), { padding: [40, 40], maxZoom: 13 })
      } catch (e) {
        mapInstance.setView([validTours[0].latitude, validTours[0].longitude], 10)
      }
    }
  }

  setTimeout(() => {
    mapInstance?.invalidateSize()
  }, 250)
}

async function loadTours() {
  loading.value = true
  try {
    const params: Record<string, any> = {
      pageIndex: filters.pageIndex,
      pageSize: filters.pageSize,
      sortBy: filters.sortBy
    }
    if (filters.keyword) params.keyword = filters.keyword
    if (filters.categoryId) params.categoryId = filters.categoryId
    if (filters.locationId) params.locationId = filters.locationId
    if (filters.minPrice) params.minPrice = filters.minPrice
    if (filters.maxPrice) params.maxPrice = filters.maxPrice
    if (filters.minRating) params.minRating = filters.minRating

    const res = await apiClient.get<PagedResponse<TourSummary>>('/tours', { params })
    tours.value = res.data.items || []
    totalCount.value = res.data.totalCount || 0

    if (viewMode.value === 'map') {
      nextTick(() => {
        initOrUpdateMap()
      })
    }
  } catch (error) {
    console.error('Failed to load tours', error)
  } finally {
    loading.value = false
  }
}

async function loadFilterOptions() {
  try {
    const [catRes, locRes] = await Promise.all([
      apiClient.get<ApiResponse<Category[]>>('/master-data/categories'),
      apiClient.get<ApiResponse<LocationItem[]>>('/master-data/locations')
    ])
    categoryOptions.value = (catRes.data.data || []).map((c: Category) => ({ label: c.name, value: c.id }))
    locationOptions.value = (locRes.data.data || []).map((l: LocationItem) => ({ label: `${l.name} (${l.provinceName})`, value: l.id }))
  } catch (error) {
    console.error('Failed to load options', error)
  }
}

watch(
  () => route.query,
  (newQ) => {
    if (newQ.view === 'map' || newQ.view === 'grid') {
      viewMode.value = newQ.view as 'grid' | 'map'
      if (viewMode.value === 'map') {
        nextTick(() => {
          initOrUpdateMap()
        })
      }
    }
    filters.keyword = (newQ.keyword as string) || ''
    filters.categoryId = newQ.categoryId ? Number(newQ.categoryId) : undefined
    filters.locationId = newQ.locationId ? Number(newQ.locationId) : undefined
    if (newQ.maxPrice) {
      filters.maxPrice = Number(newQ.maxPrice)
      priceRange.value = [0, Number(newQ.maxPrice)]
    } else {
      filters.maxPrice = undefined
    }
    filters.pageIndex = 1
    loadTours()
  },
  { deep: true }
)

onMounted(() => {
  loadFilterOptions()
  loadTours()
})
</script>

<style scoped>
.catalog-page {
  padding: 40px 20px;
}

.catalog-header {
  margin-bottom: 30px;
}

.page-title {
  font-size: 32px;
  font-weight: 800;
  color: var(--text-primary);
  margin-bottom: 6px;
}

.page-subtitle {
  font-size: 15px;
  color: var(--text-secondary);
}

.catalog-layout {
  display: flex;
  gap: 30px;
  align-items: flex-start;
}

.filter-sidebar {
  width: 280px;
  flex-shrink: 0;
}

.filter-card {
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-md);
  padding: 24px;
  box-shadow: var(--shadow-sm);
}

.filter-head {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 20px;
  padding-bottom: 12px;
  border-bottom: 1px solid var(--border-light);
}

.filter-head h3 {
  font-size: 16px;
  font-weight: 700;
  color: var(--text-primary);
}

.reset-btn {
  background: none;
  border: none;
  font-size: 13px;
  color: var(--primary-color);
  font-weight: 600;
  cursor: pointer;
}

.filter-group {
  margin-bottom: 20px;
}

.group-label {
  display: block;
  font-size: 13px;
  font-weight: 600;
  color: var(--text-primary);
  margin-bottom: 8px;
}

.price-range-text {
  display: flex;
  justify-content: space-between;
  font-size: 12px;
  font-weight: 600;
  color: var(--primary-color);
}

.results-area {
  flex: 1;
}

.sort-bar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-sm);
  padding: 12px 20px;
  margin-bottom: 24px;
}

.total-text {
  font-size: 14px;
  color: var(--text-secondary);
}

.sort-controls {
  display: flex;
  align-items: center;
  gap: 16px;
}

.sort-select {
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 13px;
  font-weight: 600;
}

.catalog-content {
  width: 100%;
}

.catalog-content.split-view {
  display: flex;
  gap: 20px;
  align-items: flex-start;
}

.cards-pane.three-cols {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(290px, 1fr));
  gap: 24px;
}

.cards-pane.two-cols {
  flex: 1;
  min-width: 0;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(260px, 1fr));
  gap: 16px;
  max-height: 700px;
  overflow-y: auto;
  padding-right: 6px;
}

.map-pane {
  width: 440px;
  flex-shrink: 0;
}

.map-sticky-wrapper {
  position: sticky;
  top: 90px;
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-md);
  overflow: hidden;
  box-shadow: var(--shadow-sm);
}

.map-header-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 14px;
  background: #f8fafc;
  border-bottom: 1px solid var(--border-color);
  font-size: 13px;
  font-weight: 700;
  color: var(--text-primary);
}

.leaflet-map-box {
  height: 600px;
  width: 100%;
  z-index: 1;
}

.tour-card {
  cursor: pointer;
  display: flex;
  flex-direction: column;
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-md);
  overflow: hidden;
  transition: all 0.3s ease;
}

.tour-card.active-card {
  border-color: var(--primary-color) !important;
  box-shadow: 0 4px 16px rgba(0, 128, 128, 0.25) !important;
  transform: translateY(-2px);
}

.tour-img-wrapper {
  position: relative;
  height: 190px;
  overflow: hidden;
}

.tour-img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.4s ease;
}

.tour-card:hover .tour-img {
  transform: scale(1.05);
}

.category-tag {
  position: absolute;
  top: 12px;
  left: 12px;
  background: rgba(255, 255, 255, 0.95);
  padding: 3px 8px;
  border-radius: 4px;
  font-size: 11px;
  font-weight: 700;
  color: var(--primary-color);
}

.location-badge {
  position: absolute;
  bottom: 12px;
  left: 12px;
  background: rgba(15, 23, 42, 0.75);
  backdrop-filter: blur(4px);
  color: #fff;
  padding: 3px 8px;
  border-radius: 4px;
  font-size: 11px;
  font-weight: 600;
}

.tour-body {
  padding: 18px;
  display: flex;
  flex-direction: column;
  flex: 1;
}

.host-info {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-bottom: 8px;
}

.host-name {
  font-size: 12px;
  font-weight: 600;
  color: var(--text-secondary);
}

.tour-title {
  font-size: 16px;
  font-weight: 700;
  color: var(--text-primary);
  line-height: 1.4;
  margin-bottom: 6px;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.tour-desc {
  font-size: 13px;
  color: var(--text-secondary);
  line-height: 1.5;
  margin-bottom: 12px;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}

.tour-meta {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-top: auto;
  padding-bottom: 12px;
  border-bottom: 1px solid var(--border-light);
  font-size: 12px;
}

.rating {
  display: flex;
  align-items: center;
  gap: 4px;
}

.star-icon {
  color: #F59E0B;
}

.score {
  font-weight: 700;
}

.count {
  color: var(--text-muted);
}

.duration {
  color: var(--text-secondary);
}

.tour-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding-top: 12px;
}

.price-box {
  display: flex;
  align-items: baseline;
  gap: 3px;
}

.price-label {
  font-size: 11px;
  color: var(--text-muted);
}

.price-value {
  font-size: 16px;
  font-weight: 800;
  color: var(--primary-color);
}

.price-unit {
  font-size: 11px;
  color: var(--text-muted);
}

.pagination-wrapper {
  display: flex;
  justify-content: center;
  margin-top: 40px;
}

@media (max-width: 900px) {
  .catalog-layout {
    flex-direction: column;
  }
  .filter-sidebar {
    width: 100%;
  }
}
</style>
