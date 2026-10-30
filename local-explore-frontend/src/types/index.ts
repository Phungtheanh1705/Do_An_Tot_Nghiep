export interface ApiResponse<T> {
  success: boolean
  message: string
  data: T
  errors: string[] | null
  timestamp: string
}

export interface PagedResponse<T> {
  items: T[]
  pageIndex: number
  pageSize: number
  totalCount: number
  totalPages: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}

export interface UserProfile {
  id: number
  email: string
  fullName: string
  phoneNumber?: string
  avatarUrl?: string
  roles: string[]
  hostProfileId?: number | null
  isHostApproved: boolean
}

export interface AuthResponse {
  accessToken: string
  refreshToken: string
  expiresAt: string
  user: UserProfile
}

export interface Category {
  id: number
  name: string
  slug: string
  description?: string
  iconUrl?: string
  imageUrl?: string
}

export interface LocationItem {
  id: number
  name: string
  slug: string
  provinceName: string
  description?: string
  imageUrl?: string
  latitude: number
  longitude: number
  isPopular: boolean
}

export interface CancellationPolicy {
  id: number
  name: string
  description: string
  refundPercentageBefore72h: number
  refundPercentageBetween24And72h: number
  refundPercentageUnder24h: number
  isDefault: boolean
}

export interface TourSummary {
  id: number
  title: string
  slug: string
  shortDescription: string
  coverImageUrl: string
  categoryName: string
  locationName: string
  provinceName: string
  basePrice: number
  durationHours: number
  averageRating: number
  reviewCount: number
  hostName: string
  hostAvatarUrl?: string
  latitude: number
  longitude: number
  status: string | number
}

export interface TourImage {
  id: number
  imageUrl: string
  caption?: string
  isCover: boolean
  displayOrder: number
}

export interface TourItinerary {
  stepOrder: number
  title: string
  description: string
  durationMinutes: number
  locationName?: string
}

export interface TourSchedule {
  id: number
  tourId: number
  departureDate: string
  startTime: string
  endTime: string
  price: number
  maxCapacity: number
  bookedCapacity: number
  remainingCapacity: number
  status: string
}

export interface ReviewItem {
  id: number
  customerId: number
  customerName: string
  customerAvatarUrl?: string
  rating: number
  comment: string
  imageUrls?: string[]
  hostReply?: string
  hostRepliedAt?: string
  createdAt: string
}

export interface TourDetail extends TourSummary {
  detailedDescription: string
  address: string
  latitude: number
  longitude: number
  supportedLanguages: string
  minAge: number
  includedServices?: string
  excludedServices?: string
  importantNotes?: string
  cancellationPolicyName: string
  cancellationPolicyDescription: string
  images: TourImage[]
  itineraries: TourItinerary[]
  availableSchedules: TourSchedule[]
  recentReviews: ReviewItem[]
}

export interface BookingParticipant {
  fullName: string
  gender: string
  participantType: 'Adult' | 'Child'
  birthDate?: string
  specialRequest?: string
}

export interface BookingDetail {
  id: number
  bookingCode: string
  customerId: number
  customerName: string
  tourId: number
  tourTitle: string
  tourSlug: string
  tourCoverImage: string
  tourAddress: string
  tourLatitude: number
  tourLongitude: number
  scheduleId: number
  departureDate: string
  startTime: string
  endTime: string
  contactName: string
  contactEmail: string
  contactPhone: string
  totalParticipants: number
  totalAmount: number
  discountAmount: number
  finalAmount: number
  status: string
  paymentStatus: string
  notes?: string
  qrCodeToken: string
  expirePaymentAt: string
  createdAt: string
  isCheckedIn: boolean
  checkedInAt?: string
  hasReviewed: boolean
  participants: BookingParticipant[]
}

export interface CheckInResult {
  success: boolean
  message: string
  bookingCode?: string
  tourTitle?: string
  customerName?: string
  totalParticipants: number
  checkedInAt?: string
}

export interface HostProfileSummary {
  id: number
  userId: number
  fullName: string
  email: string
  phoneNumber?: string
  avatarUrl?: string
  bio?: string
  identityCardNumber: string
  bankName: string
  bankAccountNumber: string
  bankAccountHolder: string
  experienceYears: number
  isApproved: boolean
  approvedAt?: string
  rejectionReason?: string
  createdAt: string
}
