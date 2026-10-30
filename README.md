# LOCALEXPLORE - HỆ THỐNG QUẢN LÝ VÀ KẾT NỐI TOUR DU LỊCH TRẢI NGHIỆM BẢN ĐỊA

> **Đồ án Tốt nghiệp / Học phần Phát triển phần mềm chuyên nghiệp**
> **Kiến trúc:** Clean Architecture (.NET 9 Web API) + Single Page Application (Vue 3, TypeScript, Ant Design Vue, Pinia, Leaflet) + Microsoft SQL Server 2022.

---

## 🌟 1. TỔNG QUAN HỆ THỐNG

**LocalExplore** là nền tảng 2-sided marketplace kết nối du khách với những người dân bản địa (Local Host), nghệ nhân làng nghề truyền thống và các cá nhân tổ chức tour văn hóa, ẩm thực, nông nghiệp sinh thái tại Việt Nam.

Hệ thống được thiết kế theo quy trình SDLC chuẩn mực:
- **Phân hệ Khách hàng (Customer):** Khám phá tour, lọc đa tiêu chí, xem lịch trình chi tiết và vị trí bản đồ Leaflet, đặt tour giữ chỗ tức thì (chống Overbooking bằng Database Transaction `Serializable`), thanh toán Sandbox VNPAY, nhận vé điện tử mã QR, và viết đánh giá sau khi hoàn thành chuyến đi.
- **Phân hệ Người tổ chức (Local Host):** Nộp hồ sơ KYC, tạo tour mới (gửi Admin duyệt), quản lý các đợt khởi hành và số lượng khách, xem danh sách khách tham gia, và sử dụng máy quét camera xác thực mã QR vé tại điểm đón (Single-Use QR Check-in).
- **Phân hệ Quản trị viên (Admin):** Thẩm định hồ sơ Host, kiểm duyệt chất lượng nội dung tour, quản lý người dùng và theo dõi thống kê toàn sàn.

---

## 🏗️ 2. KIẾN TRÚC & CÔNG NGHỆ SỬ DỤNG

### Backend (.NET 9 Web API - Clean Architecture)
- **`LocalExplore.Domain`:** 18 Entities chuẩn hóa, Enums trạng thái (`TourStatus`, `BookingStatus`, `ScheduleStatus`, `PaymentStatus`), BaseEntity.
- **`LocalExplore.Application`:** Chuẩn phản hồi `ApiResponse<T>`, `PagedResponse<T>`, Request/Response DTOs, Service Interfaces.
- **`LocalExplore.Infrastructure`:** `ApplicationDbContext` (EF Core 9 Fluent API), BCrypt Password Hashing, JWT Bearer Token Service, Booking Engine với Transaction khóa chỗ, QR Check-in Service, Review Service.
- **`LocalExplore.API`:** RESTful Controllers, Swagger UI có hỗ trợ JWT Bearer, CORS Policy.

### Frontend (Vue 3 + TypeScript + Vite)
- **Framework & UI:** Vue 3 Composition API, TypeScript, Ant Design Vue 4.x, Leaflet Map.
- **State Management & Routing:** Pinia (`authStore`), Vue Router với Navigation Guards kiểm tra phân quyền Role (`Customer`, `Host`, `Admin`).
- **HTTP Client:** Axios Client với Interceptor tự động gắn JWT Bearer và bắt lỗi 401.

### Cơ sở dữ liệu (Microsoft SQL Server 2022)
- Cơ sở dữ liệu `LocalExploreDb` gồm 18 bảng với đầy đủ Primary Keys, Foreign Keys, Unique Indexes, Check Constraints và `RowVersion` phục vụ Optimistic Concurrency Control.

---

## 🚀 3. HƯỚNG DẪN KHỞI CHẠY HỆ THỐNG

### Cách 1: Khởi chạy từng phân hệ bằng dòng lệnh (Terminal)

#### Bước 1: Chạy Backend API
Mở cửa sổ Terminal thứ nhất:
```powershell
cd e:\DoAnTotNghiep\backend
dotnet run --project src/LocalExplore.API/LocalExplore.API.csproj --urls "http://localhost:5249"
```
- **Swagger Documentation:** Truy cập [http://localhost:5249/](http://localhost:5249/) để xem toàn bộ 25+ API endpoints.

#### Bước 2: Chạy Frontend Client
Mở cửa sổ Terminal thứ hai:
```powershell
cd e:\DoAnTotNghiep\local-explore-frontend
npm run dev
```
- **Website Client:** Mở trình duyệt truy cập: [http://localhost:5173/](http://localhost:5173/)

---

## 🎯 4. KỊCH BẢN DEMO BẢO VỆ ĐỒ ÁN (THESIS PRESENTATION GUIDE)

Hệ thống đã chuẩn bị sẵn các tài khoản demo và dữ liệu mẫu thực tế tại Việt Nam để bạn tự tin trình diễn trước Hội đồng chấm thi:

### Mật khẩu chung cho tất cả tài khoản demo:
👉 `Password@123`

| Vai trò | Email Demo | Chức năng nổi bật cần biểu diễn |
|---|---|---|
| **👑 Admin** | `admin@localexplore.vn` | Vào `Admin Portal` -> `Kiểm duyệt Tour` -> Bấm duyệt tour mới của Host hoặc gỡ bỏ tour vi phạm. |
| **🧑‍🌾 Local Host (Sa Pa)** | `chinh.hmong@localexplore.vn` | Vào `Host Portal` -> Quản lý tour chè Shan Tuyết Tả Phìn -> Vào `Quét QR Check-in` quét thử vé của khách. |
| **🏺 Local Host (Bát Tràng)**| `mai.battrang@localexplore.vn` | Vào `Host Portal` -> Xem danh sách tour gốm Bát Tràng -> Quản lý lịch khởi hành và danh sách khách. |
| **🎒 Khách hàng An** | `an.hoang@gmail.com` | Vào xem vé đã hoàn thành -> Gửi đánh giá 5 sao. |
| **🎒 Khách hàng Linh** | `linh.pham@gmail.com` | Có sẵn đơn vé sắp tới với **Mã QR Check-in** để làm mẫu thử cho Host quét tại chỗ. |

### Quy trình Demo xuyên suốt 5 phút ghi điểm cao:
1. **Tìm kiếm & Khám phá:** Vào trang chủ [http://localhost:5173/](http://localhost:5173/), lọc tour theo danh mục "Làng nghề truyền thống" hoặc địa điểm "Hà Nội".
2. **Xem chi tiết & Vị trí Bản đồ:** Vào tour "Tự tay vuốt gốm Bát Tràng", xem ảnh chụp thực tế, lịch trình chặng và bản đồ Leaflet.
3. **Đặt tour & Chống Overbooking:** Chọn ngày khởi hành, nhập danh sách người tham gia, chọn thanh toán Sandbox -> Nhận ngay **Vé điện tử có mã QR**.
4. **Quét QR Check-in tại chỗ:** Đăng nhập tài khoản Host (`mai.battrang@localexplore.vn`), vào menu `Quét QR Check-in` -> Dán mã vé và bấm xác thực -> Hệ thống ghi nhận check-in thành công. Thử quét lần 2 -> Hệ thống lập tức cảnh báo "Vé đã được sử dụng".
5. **Đánh giá sau chuyến đi:** Quay lại tài khoản khách hàng, vào mục `Vé của tôi` -> Bấm `Viết đánh giá` và chấm sao -> Điểm trung bình của tour tự động cập nhật ngay lập tức.
