<template>
  <div class="create-tour-page">
    <div class="form-container glass-panel">
      <div class="head-line">
        <h2>Tạo Tour Trải nghiệm Bản địa mới</h2>
        <p>Tour sau khi tạo sẽ được gửi tới Admin để phê duyệt trước khi mở bán công khai.</p>
      </div>

      <a-form layout="vertical" @submit.prevent="handleSubmit">
        <!-- 1. General Info -->
        <div class="form-section">
          <h3 class="sec-title">1. Thông tin chung</h3>
          <a-form-item label="Tên trải nghiệm (Tiêu đề cuốn hút)" required>
            <a-input v-model:value="form.title" placeholder="Ví dụ: Học dệt lanh và thưởng thức rượu ngô cùng người H'Mông" />
          </a-form-item>

          <div class="grid-3">
            <a-form-item label="Danh mục trải nghiệm" required>
              <a-select v-model:value="form.categoryId" placeholder="Chọn danh mục" :options="categories" />
            </a-form-item>

            <a-form-item label="Điểm đến du lịch" required>
              <a-select v-model:value="form.locationId" placeholder="Chọn địa điểm" :options="locations" />
            </a-form-item>

            <a-form-item label="Chính sách hoàn hủy" required>
              <a-select v-model:value="form.cancellationPolicyId" placeholder="Chọn chính sách" :options="policies" />
            </a-form-item>
          </div>

          <div class="grid-3">
            <a-form-item label="Giá vé tiêu chuẩn (VNĐ / khách)" required>
              <a-input-number v-model:value="form.basePrice" :min="50000" :step="50000" style="width: 100%" />
            </a-form-item>

            <a-form-item label="Thời lượng (Giờ)" required>
              <a-input-number v-model:value="form.durationHours" :min="1" :max="24" :step="0.5" style="width: 100%" />
            </a-form-item>

            <a-form-item label="Độ tuổi tối thiểu">
              <a-input-number v-model:value="form.minAge" :min="1" :max="99" style="width: 100%" />
            </a-form-item>
          </div>

          <a-form-item label="Tóm tắt ngắn (Dưới 300 ký tự)" required>
            <a-textarea v-model:value="form.shortDescription" :rows="2" placeholder="Tóm tắt những điểm đặc sắc nhất của trải nghiệm..." />
          </a-form-item>

          <a-form-item label="Mô tả chi tiết câu chuyện & hoạt động" required>
            <a-textarea v-model:value="form.detailedDescription" :rows="5" placeholder="Kể về nét văn hóa, lịch sử và cảm xúc mà du khách sẽ nhận được..." />
          </a-form-item>
        </div>

        <!-- 2. Location & Address -->
        <div class="form-section">
          <h3 class="sec-title">2. Địa điểm tập trung đón khách</h3>
          <a-form-item label="Địa chỉ cụ thể" required>
            <a-input v-model:value="form.address" placeholder="Ví dụ: Bản Tả Phìn, Sa Pa, Lào Cai" />
          </a-form-item>

          <div class="grid-2">
            <a-form-item label="Vĩ độ (Latitude)" required>
              <a-input-number v-model:value="form.latitude" :step="0.0001" style="width: 100%" />
            </a-form-item>

            <a-form-item label="Kinh độ (Longitude)" required>
              <a-input-number v-model:value="form.longitude" :step="0.0001" style="width: 100%" />
            </a-form-item>
          </div>
        </div>

        <!-- 3. Inclusions & Photos -->
        <div class="form-section">
          <h3 class="sec-title">3. Hình ảnh & Dịch vụ đi kèm</h3>
          <a-form-item label="Đường dẫn ảnh đại diện (Cover Image URL)" required>
            <a-input v-model:value="coverImageUrl" placeholder="https://images.unsplash.com/..." />
          </a-form-item>

          <div class="grid-2">
            <a-form-item label="Dịch vụ bao gồm">
              <a-textarea v-model:value="form.includedServices" :rows="2" placeholder="Dụng cụ, nước uống, bữa trưa bản địa..." />
            </a-form-item>

            <a-form-item label="Chưa bao gồm">
              <a-textarea v-model:value="form.excludedServices" :rows="2" placeholder="Xe đưa đón từ khách sạn, chi phí cá nhân..." />
            </a-form-item>
          </div>

          <a-form-item label="Lưu ý quan trọng cho du khách">
            <a-textarea v-model:value="form.importantNotes" :rows="2" placeholder="Nên mang giày thể thao, trang phục lịch sự khi vào đền chùa..." />
          </a-form-item>
        </div>

        <div class="form-actions">
          <a-button size="large" @click="$router.push('/host')">Hủy bỏ</a-button>
          <a-button type="primary" size="large" html-type="submit" :loading="submitting">
            Tạo tour & Gửi kiểm duyệt
          </a-button>
        </div>
      </a-form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { message } from 'ant-design-vue'
import apiClient from '@/services/api'
import type { Category, LocationItem, CancellationPolicy, ApiResponse } from '@/types'

const router = useRouter()
const submitting = ref(false)

const categories = ref<{ label: string; value: number }[]>([])
const locations = ref<{ label: string; value: number }[]>([])
const policies = ref<{ label: string; value: number }[]>([])

const coverImageUrl = ref('https://images.unsplash.com/photo-1544644181-1484b3fdfc62?w=800')

const form = ref({
  title: '',
  categoryId: undefined as number | undefined,
  locationId: undefined as number | undefined,
  cancellationPolicyId: 1,
  basePrice: 450000,
  durationHours: 4.0,
  minAge: 6,
  supportedLanguages: 'Tiếng Việt, Tiếng Anh',
  shortDescription: '',
  detailedDescription: '',
  address: '',
  latitude: 21.028511,
  longitude: 105.854444,
  includedServices: 'Dụng cụ trải nghiệm, nước uống thảo mộc, hướng dẫn viên bản địa',
  excludedServices: 'Chi phí mua sắm ngoài chương trình',
  importantNotes: 'Trang phục thoải mái, mang theo áo khoác nhẹ'
})

async function loadOptions() {
  try {
    const [cRes, lRes, pRes] = await Promise.all([
      apiClient.get<ApiResponse<Category[]>>('/master-data/categories'),
      apiClient.get<ApiResponse<LocationItem[]>>('/master-data/locations'),
      apiClient.get<ApiResponse<CancellationPolicy[]>>('/master-data/cancellation-policies')
    ])

    categories.value = (cRes.data.data || []).map((c: Category) => ({ label: c.name, value: c.id }))
    locations.value = (lRes.data.data || []).map((l: LocationItem) => ({ label: `${l.name} (${l.provinceName})`, value: l.id }))
    policies.value = (pRes.data.data || []).map((p: CancellationPolicy) => ({ label: p.name, value: p.id }))

    if (categories.value.length) form.value.categoryId = categories.value[0].value
    if (locations.value.length) form.value.locationId = locations.value[0].value
    if (policies.value.length) form.value.cancellationPolicyId = policies.value[0].value
  } catch (error) {
    console.error('Failed to load options', error)
  }
}

async function handleSubmit() {
  if (!form.value.title.trim() || !form.value.shortDescription.trim() || !form.value.detailedDescription.trim() || !form.value.address.trim()) {
    message.warning('Vui lòng điền đầy đủ các thông tin bắt buộc.')
    return
  }

  submitting.value = true
  try {
    const payload = {
      ...form.value,
      images: [
        { imageUrl: coverImageUrl.value, isCover: true, displayOrder: 1 }
      ],
      itineraries: [
        { stepOrder: 1, title: 'Đón khách & Giới thiệu', description: 'Gặp gỡ Host và thưởng thức trà chào mừng', durationMinutes: 30 },
        { stepOrder: 2, title: 'Trải nghiệm hoạt động chính', description: 'Cùng Host thực hành trải nghiệm bản địa', durationMinutes: 120 },
        { stepOrder: 3, title: 'Tổng kết & Chụp ảnh kỷ niệm', description: 'Hoàn thiện tác phẩm và nhận quà lưu niệm', durationMinutes: 30 }
      ]
    }

    const res = await apiClient.post<ApiResponse<number>>('/tours/host', payload)
    if (res.data.success) {
      message.success('Tạo tour thành công! Tour đã được gửi đến Admin để kiểm duyệt.')
      router.push('/host')
    } else {
      message.error(res.data.message)
    }
  } catch (error: any) {
    message.error(error.response?.data?.message || 'Có lỗi xảy ra khi tạo tour')
  } finally {
    submitting.value = false
  }
}

onMounted(() => {
  loadOptions()
})
</script>

<style scoped>
.create-tour-page {
  display: flex;
  justify-content: center;
}

.form-container {
  background: #fff;
  border: 1px solid var(--border-color);
  border-radius: var(--radius-md);
  padding: 36px;
  max-width: 900px;
  width: 100%;
}

.head-line {
  margin-bottom: 28px;
  padding-bottom: 16px;
  border-bottom: 1px solid var(--border-color);
}

.head-line h2 {
  font-size: 24px;
  font-weight: 800;
  margin-bottom: 4px;
}

.head-line p {
  font-size: 13px;
  color: var(--text-secondary);
}

.form-section {
  margin-bottom: 28px;
  padding-bottom: 20px;
  border-bottom: 1px dashed var(--border-color);
}

.sec-title {
  font-size: 16px;
  font-weight: 700;
  color: var(--primary-color);
  margin-bottom: 16px;
}

.grid-3 {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 16px;
}

.grid-2 {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 12px;
  margin-top: 24px;
}

@media (max-width: 768px) {
  .grid-3, .grid-2 {
    grid-template-columns: 1fr;
  }
}
</style>
