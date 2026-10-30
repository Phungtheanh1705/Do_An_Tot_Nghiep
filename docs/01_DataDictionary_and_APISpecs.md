# TÀI LIỆU TỪ ĐIỂN DỮ LIỆU & ĐẶC TẢ RESTFUL API CONTRACTS
## DỰ ÁN: LOCALEXPLORE (LOCAL EXPERIENCE & TOUR BOOKING)

---

## PHẦN I: TỪ ĐIỂN DỮ LIỆU CHI TIẾT (DATA DICTIONARY - 18 BẢNG)

Hệ thống sử dụng cơ sở dữ liệu Microsoft SQL Server với chuẩn đặt tên PascalCase, các khóa chính (PK) tự tăng kiểu `INT IDENTITY(1,1)` để tối ưu hiệu năng và dễ dàng tra cứu, kiểm thử.

### 1. Bảng `Users`
Lưu trữ thông tin tài khoản người dùng của toàn bộ hệ thống (Khách hàng, Host, Admin).

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã định danh duy nhất của người dùng |
| `Email` | `NVARCHAR(256)` | Unique, Index | Không | Email đăng nhập hệ thống |
| `PasswordHash` | `NVARCHAR(500)` | | Không | Chuỗi băm mật khẩu bảo mật (ASP.NET Identity/BCrypt) |
| `FullName` | `NVARCHAR(150)` | | Không | Họ và tên đầy đủ |
| `PhoneNumber` | `NVARCHAR(20)` | | Có | Số điện thoại liên hệ |
| `AvatarUrl` | `NVARCHAR(500)` | | Có | Đường dẫn ảnh đại diện |
| `IsActive` | `BIT` | Default (1) | Không | Trạng thái hoạt động (1: Hoạt động, 0: Bị khóa) |
| `EmailConfirmed` | `BIT` | Default (0) | Không | Trạng thái xác thực email |
| `RefreshToken` | `NVARCHAR(500)` | | Có | Token cấp lại phiên đăng nhập JWT |
| `RefreshTokenExpiryTime` | `DATETIME2` | | Có | Thời điểm hết hạn của Refresh Token |
| `CreatedAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm tạo tài khoản |
| `UpdatedAt` | `DATETIME2` | | Có | Thời điểm cập nhật hồ sơ gần nhất |

---

### 2. Bảng `Roles`
Danh mục vai trò trong hệ thống (`Customer`, `Host`, `Admin`).

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã vai trò |
| `Name` | `NVARCHAR(50)` | Unique | Không | Tên hiển thị của vai trò (`Customer`, `Host`, `Admin`) |
| `NormalizedName` | `NVARCHAR(50)` | | Không | Tên viết hoa chuẩn hóa (`CUSTOMER`, `HOST`, `ADMIN`) |
| `Description` | `NVARCHAR(250)` | | Có | Mô tả phạm vi quyền hạn của vai trò |

---

### 3. Bảng `UserRoles`
Bảng trung gian thiết lập quan hệ Many-to-Many giữa `Users` và `Roles`.

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `UserId` | `INT` | PK, FK -> Users(Id) | Không | Mã người dùng |
| `RoleId` | `INT` | PK, FK -> Roles(Id) | Không | Mã vai trò được gán |
| `AssignedAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm gán vai trò |

---

### 4. Bảng `HostProfiles`
Hồ sơ định danh và thông tin xác thực năng lực (KYC) của Local Host. Có quan hệ 1-1 với bảng `Users`.

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã định danh hồ sơ Host |
| `UserId` | `INT` | FK -> Users(Id), Unique | Không | Mã tài khoản người dùng tương ứng |
| `Bio` | `NVARCHAR(MAX)` | | Có | Tiểu sử, câu chuyện cá nhân và văn hóa bản địa |
| `IdentityCardNumber`| `NVARCHAR(30)` | | Không | Số CCCD/Hộ chiếu của Host |
| `IssueDate` | `DATE` | | Có | Ngày cấp CCCD |
| `IssuePlace` | `NVARCHAR(150)` | | Có | Nơi cấp CCCD |
| `FrontCardUrl` | `NVARCHAR(500)` | | Có | Ảnh mặt trước CCCD |
| `BackCardUrl` | `NVARCHAR(500)` | | Có | Ảnh mặt sau CCCD |
| `BankName` | `NVARCHAR(100)` | | Không | Tên ngân hàng nhận thanh toán doanh thu |
| `BankAccountNumber`| `NVARCHAR(50)` | | Không | Số tài khoản ngân hàng |
| `BankAccountHolder`| `NVARCHAR(150)` | | Không | Tên chủ tài khoản ngân hàng |
| `ExperienceYears` | `INT` | Default (1) | Không | Số năm kinh nghiệm hướng dẫn/làm nghề |
| `IsApproved` | `BIT` | Default (0) | Không | Trạng thái duyệt KYC (0: Chờ duyệt, 1: Đã duyệt) |
| `ApprovedAt` | `DATETIME2` | | Có | Thời điểm Admin phê duyệt |
| `ApprovedByAdminId` | `INT` | FK -> Users(Id) | Có | Mã Admin đã duyệt |
| `RejectionReason` | `NVARCHAR(500)` | | Có | Lý do từ chối nếu không đạt yêu cầu KYC |
| `RatingAverage` | `DECIMAL(3,2)` | Default (5.00) | Không | Điểm đánh giá trung bình từ khách hàng |
| `TotalToursCount` | `INT` | Default (0) | Không | Tổng số tour mà Host đã tạo |
| `CreatedAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm nộp hồ sơ Host |
| `UpdatedAt` | `DATETIME2` | | Có | Thời điểm cập nhật hồ sơ |

---

### 5. Bảng `Categories`
Danh mục phân loại trải nghiệm (Làng nghề, Nông nghiệp, Ẩm thực...).

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã danh mục |
| `Name` | `NVARCHAR(100)` | Unique | Không | Tên danh mục trải nghiệm |
| `Slug` | `NVARCHAR(120)` | Unique | Không | Đường dẫn thân thiện SEO (`lang-nghe-truyen-thong`) |
| `Description` | `NVARCHAR(500)` | | Có | Mô tả ngắn về danh mục |
| `IconUrl` | `NVARCHAR(500)` | | Có | Tên icon hoặc link icon hiển thị |
| `ImageUrl` | `NVARCHAR(500)` | | Có | Ảnh minh họa danh mục |
| `DisplayOrder` | `INT` | Default (0) | Không | Thứ tự hiển thị ưu tiên trên giao diện |
| `IsActive` | `BIT` | Default (1) | Không | Trạng thái hiển thị (1: Hiện, 0: Ẩn) |
| `CreatedAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm tạo |

---

### 6. Bảng `Locations`
Danh sách điểm đến du lịch, tỉnh/thành phố gắn liền với tọa độ địa lý phục vụ bản đồ Leaflet.

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã địa phương |
| `Name` | `NVARCHAR(150)` | | Không | Tên địa danh (ví dụ: Bát Tràng, Tả Phìn, Phố cổ Hội An) |
| `Slug` | `NVARCHAR(160)` | Unique | Không | Slug địa danh |
| `ProvinceName` | `NVARCHAR(100)` | Index | Không | Tên tỉnh/thành phố (Hà Nội, Lào Cai, Quảng Nam...) |
| `Description` | `NVARCHAR(MAX)` | | Có | Giới thiệu nét đặc sắc vùng miền |
| `ImageUrl` | `NVARCHAR(500)` | | Có | Ảnh đại diện điểm đến |
| `Latitude` | `DECIMAL(9,6)` | | Không | Vĩ độ trung tâm |
| `Longitude` | `DECIMAL(9,6)` | | Không | Kinh độ trung tâm |
| `IsPopular` | `BIT` | Default (0) | Không | Đánh dấu địa điểm du lịch phổ biến |
| `DisplayOrder` | `INT` | Default (0) | Không | Thứ tự hiển thị |
| `CreatedAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm tạo |

---

### 7. Bảng `CancellationPolicies`
Mẫu quy định chính sách hủy tour và hoàn tiền minh bạch cho du khách.

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã chính sách |
| `Name` | `NVARCHAR(100)` | | Không | Tên chính sách (Linh hoạt, Tiêu chuẩn, Nghiêm ngặt) |
| `Description` | `NVARCHAR(500)` | | Không | Diễn giải chi tiết điều kiện hoàn hủy |
| `RefundPercentageBefore72h` | `INT` | Default (100) | Không | Tỷ lệ hoàn tiền khi hủy trước 72 giờ (%) |
| `RefundPercentageBetween24And72h` | `INT` | Default (50) | Không | Tỷ lệ hoàn tiền khi hủy từ 24h đến 72h (%) |
| `RefundPercentageUnder24h` | `INT` | Default (0) | Không | Tỷ lệ hoàn tiền khi hủy dưới 24h (%) |
| `IsDefault` | `BIT` | Default (0) | Không | Đánh dấu chính sách áp dụng mặc định |
| `CreatedAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm tạo |

---

### 8. Bảng `Tours`
Bảng trung tâm lưu trữ thông tin sản phẩm trải nghiệm du lịch bản địa.

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã định danh tour |
| `HostId` | `INT` | FK -> HostProfiles(Id) | Không | Mã Host sở hữu tour |
| `CategoryId` | `INT` | FK -> Categories(Id) | Không | Mã danh mục trải nghiệm |
| `LocationId` | `INT` | FK -> Locations(Id) | Không | Mã điểm đến địa lý |
| `CancellationPolicyId`| `INT` | FK -> Policies(Id) | Không | Mã chính sách hoàn hủy áp dụng |
| `Title` | `NVARCHAR(250)` | | Không | Tên tiêu đề trải nghiệm |
| `Slug` | `NVARCHAR(280)` | Unique | Không | Slug thân thiện URL |
| `ShortDescription` | `NVARCHAR(500)` | | Không | Tóm tắt ngắn gọn cuốn hút |
| `DetailedDescription`| `NVARCHAR(MAX)` | | Không | Bài viết chi tiết câu chuyện và hoạt động |
| `Address` | `NVARCHAR(300)` | | Không | Địa chỉ cụ thể điểm hẹn/tập trung |
| `Latitude` | `DECIMAL(9,6)` | | Không | Vĩ độ điểm hẹn trên bản đồ |
| `Longitude` | `DECIMAL(9,6)` | | Không | Kinh độ điểm hẹn trên bản đồ |
| `DurationHours` | `DECIMAL(4,1)` | | Không | Thời lượng trải nghiệm (ví dụ: 4.0 giờ) |
| `SupportedLanguages`| `NVARCHAR(150)` | Default ('Tiếng Việt')| Không | Ngôn ngữ giao tiếp của Host |
| `MinAge` | `INT` | Default (5) | Không | Độ tuổi tối thiểu được phép tham gia |
| `BasePrice` | `DECIMAL(18,2)` | Index | Không | Giá tham khảo tiêu chuẩn (VND) |
| `IncludedServices` | `NVARCHAR(MAX)` | | Có | Các dịch vụ bao gồm (dụng cụ, bữa ăn...) |
| `ExcludedServices` | `NVARCHAR(MAX)` | | Có | Các dịch vụ không bao gồm (xe đưa đón...) |
| `ImportantNotes` | `NVARCHAR(MAX)` | | Có | Lưu ý an toàn, trang phục, đồ dùng cá nhân |
| `Status` | `NVARCHAR(30)` | CHECK Status, Index | Không | `Draft`, `PendingApproval`, `Published`, `Rejected`, `Hidden`, `Archived` |
| `RejectionReason` | `NVARCHAR(500)` | | Có | Phản hồi của Admin nếu từ chối duyệt |
| `AverageRating` | `DECIMAL(3,2)` | Default (0.00), Index | Không | Điểm đánh giá trung bình (1.00 - 5.00) |
| `ReviewCount` | `INT` | Default (0) | Không | Tổng số lượt khách đã viết đánh giá |
| `ViewCount` | `INT` | Default (0) | Không | Số lượt người xem trang chi tiết |
| `CreatedAt` | `DATETIME2` | Default (UTC) | Không | Ngày tạo tour |
| `UpdatedAt` | `DATETIME2` | | Có | Ngày cập nhật gần nhất |

---

### 9. Bảng `TourImages`
Lưu trữ danh sách hình ảnh thực tế chất lượng cao của tour.

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã ảnh |
| `TourId` | `INT` | FK -> Tours(Id) Cascade | Không | Mã tour sở hữu ảnh |
| `ImageUrl` | `NVARCHAR(500)` | | Không | Đường dẫn ảnh (Cloudinary/Storage) |
| `Caption` | `NVARCHAR(250)` | | Có | Chú thích bức ảnh |
| `IsCover` | `BIT` | Default (0) | Không | Đánh dấu ảnh đại diện chính (Cover Image) |
| `DisplayOrder` | `INT` | Default (0) | Không | Thứ tự hiển thị trong bộ sưu tập ảnh |
| `CreatedAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm tải lên |

---

### 10. Bảng `TourItineraries`
Lịch trình chi tiết từng chặng / mốc thời gian của tour.

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã chặng |
| `TourId` | `INT` | FK -> Tours(Id) Cascade | Không | Mã tour |
| `StepOrder` | `INT` | | Không | Thứ tự chặng (1, 2, 3...) |
| `Title` | `NVARCHAR(200)` | | Không | Tên hoạt động của chặng |
| `Description` | `NVARCHAR(MAX)` | | Không | Mô tả chi tiết hoạt động trải nghiệm |
| `DurationMinutes`| `INT` | | Không | Thời lượng dự kiến của chặng (phút) |
| `LocationName` | `NVARCHAR(200)` | | Có | Tên địa điểm diễn ra chặng này |
| `CreatedAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm tạo |

---

### 11. Bảng `TourSchedules`
Quản lý các đợt / lịch khởi hành cụ thể của tour cùng giới hạn số khách.

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã lịch khởi hành |
| `TourId` | `INT` | FK -> Tours(Id) Cascade | Không | Mã tour |
| `DepartureDate` | `DATE` | Index | Không | Ngày diễn ra chuyến đi |
| `StartTime` | `TIME(0)` | | Không | Giờ bắt đầu trải nghiệm |
| `EndTime` | `TIME(0)` | | Không | Giờ kết thúc trải nghiệm |
| `Price` | `DECIMAL(18,2)` | | Không | Giá áp dụng riêng cho lịch này (VND) |
| `MaxCapacity` | `INT` | | Không | Sức chứa tối đa (số khách nhiều nhất) |
| `BookedCapacity`| `INT` | Default (0), CHECK <= Max | Không | Số lượng chỗ đã có người đặt |
| `Status` | `NVARCHAR(30)` | CHECK, Index | Không | `Available`, `FullyBooked`, `Cancelled`, `Completed` |
| `RowVersion` | `ROWVERSION` | Concurrency Token | Không | Khóa kiểm soát đồng thời (Optimistic Lock) |
| `CreatedAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm mở lịch |
| `UpdatedAt` | `DATETIME2` | | Có | Thời điểm cập nhật |

---

### 12. Bảng `Bookings`
Đơn đặt tour của khách hàng (nghiệp vụ quan trọng nhất hệ thống).

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã định danh booking |
| `BookingCode` | `NVARCHAR(25)` | Unique, Index | Không | Mã đơn đặt hiển thị (ví dụ: `LE-202610-001`) |
| `CustomerId` | `INT` | FK -> Users(Id) | Không | Mã khách hàng đặt tour |
| `ScheduleId` | `INT` | FK -> TourSchedules(Id) | Không | Mã lịch khởi hành được chọn |
| `ContactName` | `NVARCHAR(150)` | | Không | Họ tên người đại diện liên hệ |
| `ContactEmail` | `NVARCHAR(256)` | | Không | Email nhận vé và thông báo xác nhận |
| `ContactPhone` | `NVARCHAR(20)` | | Không | Số điện thoại liên lạc khi đón khách |
| `TotalParticipants`| `INT` | | Không | Tổng số khách tham gia trong đơn |
| `TotalAmount` | `DECIMAL(18,2)` | | Không | Tổng giá trị vé gốc (VND) |
| `DiscountAmount`| `DECIMAL(18,2)` | Default (0.00) | Không | Số tiền được giảm giá |
| `FinalAmount` | `DECIMAL(18,2)` | | Không | Số tiền thực tế phải thanh toán |
| `Status` | `NVARCHAR(30)` | CHECK, Index | Không | `PendingPayment`, `Paid`, `Confirmed`, `Cancelled`, `Completed`, `Refunded`, `Rejected` |
| `PaymentStatus` | `NVARCHAR(30)` | CHECK | Không | `Unpaid`, `Pending`, `Paid`, `Refunded`, `Failed` |
| `Notes` | `NVARCHAR(500)` | | Có | Ghi chú từ khách hàng gửi Host |
| `QrCodeToken` | `NVARCHAR(256)` | Unique, Index | Không | Chuỗi token bảo mật dùng để sinh mã QR vé |
| `ExpirePaymentAt`| `DATETIME2` | | Không | Mốc hết hạn giữ chỗ (15 phút sau khi tạo đơn) |
| `CreatedAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm đặt tour |
| `UpdatedAt` | `DATETIME2` | | Có | Thời điểm cập nhật trạng thái |

---

### 13. Bảng `BookingParticipants`
Danh sách chi tiết từng cá nhân tham gia trong một đơn đặt tour.

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã định danh hành khách |
| `BookingId` | `INT` | FK -> Bookings(Id) Cascade | Không | Mã đơn đặt tour tương ứng |
| `FullName` | `NVARCHAR(150)` | | Không | Họ và tên hành khách |
| `Gender` | `NVARCHAR(10)` | | Không | Giới tính (Nam / Nữ / Khác) |
| `ParticipantType`| `NVARCHAR(20)` | Default ('Adult'), CHECK | Không | `Adult` (Người lớn) hoặc `Child` (Trẻ em) |
| `BirthDate` | `DATE` | | Có | Ngày sinh (để đối chiếu độ tuổi phù hợp) |
| `SpecialRequest`| `NVARCHAR(250)` | | Có | Yêu cầu đặc biệt (dị ứng đồ ăn, ăn chay...) |

---

### 14. Bảng `Payments`
Lịch sử các giao dịch thanh toán trực tuyến gắn với từng booking.

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã giao dịch trong hệ thống |
| `BookingId` | `INT` | FK -> Bookings(Id) Cascade | Không | Mã đơn đặt được thanh toán |
| `TransactionCode`| `NVARCHAR(100)` | Unique, Index | Không | Mã giao dịch duy nhất do hệ thống sinh |
| `PaymentMethod` | `NVARCHAR(50)` | | Không | Phương thức (`Sandbox_VNPAY`, `Sandbox_MOMO`...) |
| `Amount` | `DECIMAL(18,2)` | | Không | Số tiền giao dịch thực tế (VND) |
| `Status` | `NVARCHAR(30)` | CHECK, Index | Không | `Pending`, `Success`, `Failed`, `Cancelled` |
| `GatewayTransactionNo` | `NVARCHAR(100)` | | Có | Mã tham chiếu trả về từ cổng thanh toán |
| `GatewayResponse` | `NVARCHAR(MAX)` | | Có | Dữ liệu thô JSON trả về từ cổng Webhook |
| `PaidAt` | `DATETIME2` | | Có | Thời điểm thanh toán thành công |
| `CreatedAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm tạo yêu cầu thanh toán |

---

### 15. Bảng `CheckIns`
Bản ghi kiểm tra và xác thực vé điện tử bằng quét mã QR tại điểm đón.

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã lượt check-in |
| `BookingId` | `INT` | FK -> Bookings(Id), Unique | Không | Mã vé (Mỗi booking chỉ check-in 1 lần duy nhất) |
| `CheckedInByHostId`| `INT` | FK -> Users(Id) | Không | Mã tài khoản Host thực hiện quét vé |
| `CheckedInAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm quét xác nhận thành công |
| `DeviceInfo` | `NVARCHAR(200)` | | Có | Thông tin thiết bị/trình duyệt quét |
| `Notes` | `NVARCHAR(500)` | | Có | Ghi chú tại hiện trường (khách đến muộn...) |

---

### 16. Bảng `Reviews`
Đánh giá và chấm điểm trải nghiệm từ khách hàng sau khi tour kết thúc.

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã đánh giá |
| `BookingId` | `INT` | FK -> Bookings(Id), Unique | Không | Mã vé (Mỗi chuyến đi chỉ được đánh giá 1 lần) |
| `CustomerId` | `INT` | FK -> Users(Id) | Không | Mã khách hàng gửi đánh giá |
| `TourId` | `INT` | FK -> Tours(Id) Cascade | Không | Mã tour được đánh giá |
| `Rating` | `INT` | CHECK (1 <= Rating <= 5) | Không | Điểm đánh giá (1 đến 5 sao) |
| `Comment` | `NVARCHAR(MAX)` | | Không | Nội dung cảm nhận của du khách |
| `ImageUrls` | `NVARCHAR(MAX)` | | Có | Mảng JSON chứa link ảnh thực tế do khách chụp |
| `HostReply` | `NVARCHAR(MAX)` | | Có | Phản hồi cảm ơn/giải thích từ phía Host |
| `HostRepliedAt` | `DATETIME2` | | Có | Thời điểm Host gửi phản hồi |
| `IsHidden` | `BIT` | Default (0) | Không | Ẩn review vi phạm bởi Admin |
| `CreatedAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm viết đánh giá |
| `UpdatedAt` | `DATETIME2` | | Có | Thời điểm chỉnh sửa |

---

### 17. Bảng `Favorites`
Danh sách các tour được khách hàng bấm "Thả tim" để lưu vào mục yêu thích.

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã bản ghi |
| `UserId` | `INT` | FK -> Users(Id) Cascade | Không | Mã khách hàng |
| `TourId` | `INT` | FK -> Tours(Id) Cascade | Không | Mã tour được yêu thích |
| `CreatedAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm lưu yêu thích |
| Ràng buộc khác | Unique `(UserId, TourId)` | | | Không cho phép lưu trùng lặp 1 tour nhiều lần |

---

### 18. Bảng `Notifications`
Lưu trữ thông báo trong ứng dụng cho người dùng về các sự kiện nghiệp vụ quan trọng.

| Tên Cột | Kiểu Dữ Liệu | Ràng buộc | Cho phép NULL | Mô tả nghiệp vụ |
|---|---|---|:---:|---|
| `Id` | `INT` | PK, Identity(1,1) | Không | Mã thông báo |
| `UserId` | `INT` | FK -> Users(Id) Cascade | Không | Người dùng nhận thông báo |
| `Title` | `NVARCHAR(200)` | | Không | Tiêu đề thông báo |
| `Message` | `NVARCHAR(MAX)` | | Không | Nội dung thông báo chi tiết |
| `NotificationType`| `NVARCHAR(50)` | | Không | Loại sự kiện (`TourApproved`, `BookingConfirmed`...) |
| `TargetUrl` | `NVARCHAR(300)` | | Có | Đường dẫn điều hướng khi bấm vào thông báo |
| `IsRead` | `BIT` | Default (0) | Không | Trạng thái đã xem (0: Chưa đọc, 1: Đã đọc) |
| `CreatedAt` | `DATETIME2` | Default (UTC) | Không | Thời điểm gửi thông báo |

---

## PHẦN II: ĐẶC TẢ RESTFUL API CONTRACTS CHUẨN MỰC

### 1. Chuẩn hóa Cấu trúc Phản hồi (Standard API Response Wrapper)
Mọi API trong hệ thống đều trả về một định dạng JSON thống nhất `ApiResponse<T>`:

```json
{
  "success": true,
  "message": "Thao tác thành công",
  "data": { ... },
  "errors": null,
  "timestamp": "2026-10-30T16:30:00Z"
}
```

Đối với các API trả về danh sách có phân trang, định dạng `PagedResponse<T>`:
```json
{
  "success": true,
  "message": "Lấy dữ liệu thành công",
  "data": {
    "items": [ ... ],
    "pageIndex": 1,
    "pageSize": 10,
    "totalCount": 45,
    "totalPages": 5,
    "hasPreviousPage": false,
    "hasNextPage": true
  },
  "errors": null,
  "timestamp": "2026-10-30T16:30:00Z"
}
```

---

### 2. Danh mục Endpoints theo Phân hệ

#### 🔐 Module 1: Authentication & Authorization (`/api/auth`)
1. **`POST /api/auth/register`**
   - **Mục đích:** Đăng ký tài khoản Customer mới.
   - **Request Body:** `{ "email": "an@gmail.com", "password": "Password@123", "fullName": "Hoàng Minh An", "phoneNumber": "0987654321" }`
   - **Response (201 Created):** `{ "success": true, "message": "Đăng ký tài khoản thành công" }`
2. **`POST /api/auth/login`**
   - **Mục đích:** Đăng nhập lấy JWT Access Token và Refresh Token.
   - **Request Body:** `{ "email": "an@gmail.com", "password": "Password@123" }`
   - **Response (200 OK):** Trả về `accessToken`, `refreshToken`, và `userProfile` (id, email, fullName, roles, avatarUrl).
3. **`POST /api/auth/refresh-token`**
   - **Mục đích:** Cấp lại Access Token mới khi token cũ hết hạn 30 phút.
   - **Request Body:** `{ "refreshToken": "..." }`
4. **`GET /api/auth/me`**
   - **Mục đích:** Lấy thông tin tài khoản hiện tại từ JWT Claims (`Authorization: Bearer <token>`).

---

#### 👤 Module 2: Host KYC & Profile (`/api/host-profile`)
1. **`POST /api/host-profile/register`** (Role: Customer)
   - **Mục đích:** Khách hàng nộp hồ sơ xin nâng cấp lên Local Host.
   - **Request Body:** `{ "bio": "...", "identityCardNumber": "0123...", "issueDate": "2020-01-01", "bankName": "Vietcombank", "bankAccountNumber": "...", "bankAccountHolder": "...", "experienceYears": 5 }`
   - **Response (200 OK):** Hồ sơ chuyển trạng thái `PendingApproval`.
2. **`GET /api/admin/hosts/pending`** (Role: Admin)
   - **Mục đích:** Admin xem danh sách hồ sơ Host đang chờ duyệt.
3. **`POST /api/admin/hosts/{id}/approve`** (Role: Admin)
   - **Mục đích:** Phê duyệt hồ sơ Host, gán vai trò `Host` cho tài khoản.
4. **`POST /api/admin/hosts/{id}/reject`** (Role: Admin)
   - **Mục đích:** Từ chối kèm lý do (`rejectionReason`).

---

#### 🗺️ Module 3: Tours & Schedules (`/api/tours`)
1. **`GET /api/tours`** (Public)
   - **Mục đích:** Tìm kiếm và lọc tour công khai.
   - **Query Params:** `keyword`, `categoryId`, `locationId`, `minPrice`, `maxPrice`, `departureDate`, `minRating`, `pageIndex`, `pageSize`, `sortBy`.
2. **`GET /api/tours/{slug}`** (Public)
   - **Mục đích:** Xem chi tiết tour theo slug (kèm ảnh, lịch trình, chính sách, thông tin Host).
3. **`GET /api/tours/{id}/schedules`** (Public)
   - **Mục đích:** Lấy danh sách các ngày khởi hành còn chỗ (`Status == 'Available'`).
4. **`POST /api/host/tours`** (Role: Host)
   - **Mục đích:** Host tạo tour mới ở trạng thái `Draft` hoặc gửi duyệt `PendingApproval`.
5. **`PUT /api/host/tours/{id}`** (Role: Host)
   - **Mục đích:** Host cập nhật nội dung tour của chính mình.
6. **`POST /api/host/tours/{id}/schedules`** (Role: Host)
   - **Mục đích:** Host thêm các ngày khởi hành và số chỗ cho tour.
7. **`POST /api/admin/tours/{id}/approve`** (Role: Admin)
   - **Mục đích:** Admin duyệt tour chuyển sang `Published` để hiển thị trên website.
8. **`POST /api/admin/tours/{id}/reject`** (Role: Admin)
   - **Mục đích:** Admin từ chối tour kèm lý do cụ thể.

---

#### 🎟️ Module 4: Booking Engine (`/api/bookings`)
1. **`POST /api/bookings`** (Role: Customer)
   - **Mục đích:** Khách hàng khởi tạo booking và tạm giữ chỗ trong 15 phút.
   - **Request Body:**
     ```json
     {
       "scheduleId": 2,
       "contactName": "Phạm Thùy Linh",
       "contactEmail": "linh.pham@gmail.com",
       "contactPhone": "0978123456",
       "notes": "Yêu cầu bàn xoay gốm cạnh cửa sổ",
       "participants": [
         { "fullName": "Phạm Thùy Linh", "gender": "Nữ", "participantType": "Adult" },
         { "fullName": "Nguyễn Đức Nam", "gender": "Nam", "participantType": "Adult" }
       ]
     }
     ```
   - **Xử lý Backend:** Bọc trong Database Transaction -> Kiểm tra `BookedCapacity + 2 <= MaxCapacity` -> Tăng `BookedCapacity` -> Tạo đơn với trạng thái `PendingPayment`, sinh `BookingCode`, tạo mã `QrCodeToken` và thiết lập `ExpirePaymentAt = NOW + 15 phút`.
   - **Response (201 Created):** Trả về chi tiết booking và mã giao dịch.
2. **`GET /api/bookings/my-bookings`** (Role: Customer)
   - **Mục đích:** Lấy danh sách booking của cá nhân người dùng (có phân tab trạng thái).
3. **`GET /api/bookings/{bookingCode}`** (Role: Customer / Host / Admin)
   - **Mục đích:** Xem chi tiết vé, mã QR và trạng thái tham gia.
4. **`POST /api/bookings/{id}/cancel`** (Role: Customer)
   - **Mục đích:** Hủy booking theo chính sách hoàn tiền (`CancellationPolicies`). Hoàn lại số chỗ trống cho lịch.
5. **`GET /api/host/bookings`** (Role: Host)
   - **Mục đích:** Host xem toàn bộ danh sách khách đã đặt các tour của mình.

---

#### 💳 Module 5: Payment Sandbox (`/api/payments`)
1. **`POST /api/payments/create-url`** (Role: Customer)
   - **Mục đích:** Tạo URL chuyển hướng sang cổng thanh toán Sandbox (VNPAY/Momo).
   - **Request Body:** `{ "bookingId": 2, "paymentMethod": "Sandbox_VNPAY" }`
   - **Response:** Trả về `{ "paymentUrl": "https://sandbox.vnpayment.vn/..." }`
2. **`POST /api/payments/vnpay-callback`** (Public - IPN Webhook)
   - **Mục đích:** Cổng thanh toán gọi webhook báo kết quả.
   - **Xử lý Backend:** Xác thực chữ ký HMAC-SHA512 -> Cập nhật trạng thái `Payments` sang `Success` -> Chuyển `Bookings` sang `Paid` và `Confirmed`.

---

#### 📷 Module 6: QR Check-in (`/api/checkin`)
1. **`POST /api/host/checkin/verify`** (Role: Host / Admin)
   - **Mục đích:** Host quét mã QR vé tại hiện trường để xác thực khách.
   - **Request Body:** `{ "qrCodeToken": "LE-QR-CHECKIN-DEMO-LINH-2026", "deviceInfo": "Host Camera Mobile" }`
   - **Kiểm tra Backend:**
     1. Token tồn tại trong bảng `Bookings` không?
     2. Booking thuộc đúng tour do Host này sở hữu không?
     3. Booking đã được thanh toán (`Status == 'Confirmed'` hoặc `'Paid'`) không?
     4. Đúng ngày diễn ra tour (`DepartureDate == Hôm nay`) không?
     5. Đã check-in trước đó chưa (tra cứu bảng `CheckIns`)?
   - **Response (200 OK):** Trả về thông tin khách hợp lệ và ghi nhận vào bảng `CheckIns`.
   - **Response (400 Bad Request):** Báo lỗi cụ thể (Vé không tồn tại, vé sai ngày, hoặc vé đã được quét lúc hh:mm).

---

#### ⭐ Module 7: Review & Rating (`/api/reviews`)
1. **`POST /api/reviews`** (Role: Customer)
   - **Mục đích:** Gửi đánh giá cho chuyến đi đã hoàn thành.
   - **Điều kiện:** `Booking.Status == 'Completed'` và chưa từng đánh giá.
   - **Request Body:** `{ "bookingId": 1, "rating": 5, "comment": "Trải nghiệm rất tuyệt vời...", "imageUrls": [...] }`
2. **`POST /api/host/reviews/{id}/reply`** (Role: Host)
   - **Mục đích:** Host gửi phản hồi cho đánh giá của khách hàng.

---

#### 📊 Module 8: Analytics & Dashboards (`/api/dashboards`)
1. **`GET /api/host/dashboard/statistics`** (Role: Host)
   - **Mục đích:** Trả về tổng doanh thu tháng, số khách đã đón, tỷ lệ lấp đầy lịch khởi hành.
2. **`GET /api/admin/dashboard/overview`** (Role: Admin)
   - **Mục đích:** Báo cáo toàn sàn: Doanh thu 12 tháng, tổng số tour, tổng số khách, top 5 tour bán chạy nhất.
