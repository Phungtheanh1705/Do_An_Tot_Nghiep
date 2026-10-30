# TÀI LIỆU HƯỚNG DẪN BẢO VỆ ĐỒ ÁN TỐT NGHIỆP & BỘ CÂU HỎI PHẢN BIỆN (Q&A)

**Hệ thống:** Nền tảng Quản lý và Đặt Tour Trải nghiệm Bản địa (**LocalExplore**)  
**Chuyên ngành:** Kỹ thuật Phần mềm / Công nghệ Thông tin  
**Công nghệ:** .NET 9 Clean Architecture, EF Core 9, SQL Server 2022, Vue 3, TypeScript, Vite, Ant Design Vue, Leaflet GIS, Docker.

---

## PHẦN I: ĐỀ CƯƠNG SLIDE THUYẾT TRÌNH BẢO VỆ (15 SLIDES CHUẨN MỰC)

### Slide 1: Trang tiêu đề (Title Slide)
- **Tên đề tài:** XÂY DỰNG HỆ THỐNG HỖ TRỢ QUẢN LÝ VÀ KẾT NỐI TOUR DU LỊCH TRẢI NGHIỆM / BẢN ĐỊA (LOCALEXPLORE).
- **Sinh viên thực hiện:** [Họ và tên sinh viên] – MSSV: [Mã số sinh viên]
- **Giảng viên hướng dẫn:** [Học hàm, học vị, Họ và tên GVHD]
- **Khoa / Trường:** Khoa Công nghệ Thông tin – Trường Đại học [...]
- **Năm thực hiện:** 2026.

---

### Slide 2: Đặt vấn đề & Tính cấp thiết của đề tài
- **Bối cảnh thực tế:**
  - Xu hướng du lịch chuyển dịch mạnh mẽ từ "tham quan đại trà" (mass tourism) sang "du lịch trải nghiệm bản địa" (local living, thủ công truyền thống, văn hóa ẩm thực gia đình).
  - Khách du lịch mong muốn tìm kiếm những trải nghiệm chân thực, do chính người dân địa phương (Local Host / Nghệ nhân) dẫn dắt.
- **Tồn tại của các giải pháp hiện nay:**
  - Các OTA lớn (Agoda, Klook, Traveloka) tập trung vào vé máy bay, khách sạn và các tour trọn gói công nghiệp.
  - Các nghệ nhân, người dân bản địa thiếu một kênh số hóa để tự đăng ký (KYC), quản lý lịch trình linh hoạt, kiểm soát số lượng khách nhỏ (micro-group).
- **Mục tiêu đề tài:** Xây dựng nền tảng Fullstack hoàn chỉnh, giải quyết bài toán đặt tour, quản lý lịch khởi hành, thanh toán giả lập, vé điện tử QR duy nhất và kiểm duyệt chặt chẽ.

---

### Slide 3: Đối tượng người dùng & Yêu cầu nghiệp vụ cốt lõi
Hệ thống phục vụ 3 phân hệ người dùng chính (RBAC):
1. **Khách hàng (Customer):**
   - Tìm kiếm, lọc theo địa điểm, mức giá, danh mục và bản đồ tương tác (Leaflet GIS).
   - Giữ chỗ và thanh toán với cơ chế khóa ghế tức thì (15 phút).
   - Nhận vé điện tử gắn mã QR định danh duy nhất (Single-use QR Ticket).
   - Đánh giá, chấm điểm sau khi hoàn thành chuyến đi.
2. **Local Host (Người bản địa / Nghệ nhân):**
   - Đăng ký KYC (CCCD, kinh nghiệm, tài khoản ngân hàng).
   - Thiết kế lịch trình chi tiết (Itinerary), mở các ngày khởi hành và số chỗ giới hạn.
   - Ứng dụng quét mã QR để Check-in khách tại điểm hẹn trong thời gian thực.
3. **Quản trị viên (Admin sàn):**
   - Thẩm định hồ sơ KYC của Local Host trước khi cấp quyền tạo tour.
   - Kiểm duyệt nội dung, độ an toàn và giá cả của Tour đăng tải.
   - Theo dõi doanh thu, tỷ lệ hoa hồng và số liệu vận hành.

---

### Slide 4: Kiến trúc hệ thống tổng thể (Clean Architecture)
Hệ thống áp dụng kiến trúc 4 tầng phân tách nghiêm ngặt:
- **Domain Layer (Core Core):** Chứa 18 thực thể nghiệp vụ (Entities), Enums, Value Objects và Base Entity không phụ thuộc vào bất kỳ thư viện bên ngoài nào.
- **Application Layer:** Định nghĩa các DTOs, ViewModel, Interface dịch vụ (`IBookingService`, `ITourService`, `ICheckInService`, `IAuthService`) và hợp đồng API chuẩn mực `ApiResponse<T>`, `PagedResponse<T>`.
- **Infrastructure Layer:** Cấu hình EF Core 9 Fluent API, SQL Server 2022, BCrypt Password Hasher, JWT & Refresh Token Service, và engine giao dịch `IsolationLevel.Serializable`.
- **Presentation Layer (Web API):** ASP.NET Core 9 Controllers, Swagger UI, Middleware bắt lỗi tập trung (Global Exception Handler), CORS và Authorize RBAC.

---

### Slide 5: Thiết kế Cơ sở Dữ liệu (18 Bảng Chuẩn hóa 3NF)
- **Nhóm Tài khoản & KYC:** `Users`, `Roles`, `UserRoles`, `HostProfiles`.
- **Nhóm Danh mục & Địa lý:** `Categories`, `Locations`, `CancellationPolicies`.
- **Nhóm Sản phẩm Tour & Lịch trình:** `Tours`, `TourImages`, `TourItineraries`, `TourSchedules`.
- **Nhóm Giao dịch & Đặt tour:** `Bookings`, `BookingParticipants`, `Payments`, `CheckIns`.
- **Nhóm Tương tác & Phản hồi:** `Reviews`, `Favorites`, `Notifications`.
- **Tính năng tối ưu hóa DB:**
  - Concurrency Token: Sử dụng `RowVersion` (Timestamp) chống Lost Update.
  - Non-Clustered Indexes trên các trường thường xuyên tìm kiếm và join: `Slug`, `DepartureDate`, `HostId`, `CustomerId`, `QrCodeToken`.

---

### Slide 6: Nghiệp vụ trọng tâm 1 – Cơ chế Chống Overbooking (Anti-Overbooking Engine)
- **Thách thức:** Khi một tour bản địa chỉ nhận tối đa 6 khách, nếu 2 khách hàng đồng thời gửi yêu cầu đặt 4 chỗ trong cùng 1 giây:
  - Nếu chỉ dùng `SELECT` kiểm tra rồi `UPDATE`, cả 2 giao dịch đều thấy còn 6 chỗ và đặt thành công -> Tổng đặt 8 chỗ -> **Overbooking (Vi phạm nghiệp vụ nghiêm trọng)**.
- **Giải pháp của LocalExplore:**
  - Áp dụng Database Transaction cấp độ cao nhất: `IsolationLevel.Serializable`.
  - Khóa vùng dữ liệu (Range Lock / Table Lock) tại thời điểm đọc và ghi của bản ghi `TourSchedule`.
  - Kiểm tra điều kiện atomic: `if (schedule.BookedCapacity + requestedGuests > schedule.MaxCapacity) throw OverbookingException;`
  - Tự động chuyển trạng thái lịch trình thành `SoldOut` khi số chỗ còn lại = 0.

---

### Slide 7: Nghiệp vụ trọng tâm 2 – Chính sách Hủy tour & Hoàn tiền tự động (BR-05)
- **Mô hình tính toán hoàn tiền linh hoạt:**
  - **Hủy trước giờ khởi hành > 72 giờ:** Hoàn **100%** số tiền, tự động giải phóng số chỗ trống (`BookedCapacity -= guests`) để khách khác có thể đặt.
  - **Hủy trong khoảng 24 – 72 giờ:** Hoàn **50%** số tiền, tự động mở lại số chỗ trống.
  - **Hủy dưới 24 giờ:** Hoàn **0%**, **không giải phóng số chỗ** vì Local Host đã mua nguyên vật liệu và chuẩn bị cho chuyến trải nghiệm.

---

### Slide 8: Nghiệp vụ trọng tâm 3 – Vé điện tử QR & Check-in Chống tái sử dụng (Single-Use Idempotency)
- **Quy trình sinh mã vé:**
  - Sau khi khách hoàn tất thanh toán, hệ thống sinh mã vé ngẫu nhiên mã hóa an toàn (`LE-QR-{GUID}-{Timestamp}`).
  - Frontend hiển thị mã QR động qua API chuyên dụng.
- **Quy trình quét và xác thực tại hiện trường:**
  - Local Host đăng nhập, mở camera hoặc trình quét QR trên giao diện Host.
  - Backend kiểm tra 3 điều kiện:
    1. Người quét có phải là Host sở hữu tour hoặc Admin hay không?
    2. Trạng thái đơn đặt có phải là `Paid` / `Confirmed` hay không?
    3. Bảng `CheckIns` đã có bản ghi của `BookingId` này chưa?
  - Nếu đã quét: Báo động vi phạm với thời gian và thiết bị đã quét trước đó.
  - Nếu chưa quét: Tạo bản ghi `CheckIn`, cập nhật trạng thái đơn thành `Completed` (Kích hoạt quyền viết đánh giá cho khách).

---

### Slide 9: Giao diện Người dùng Khách hàng (Customer Experience)
- **Trải nghiệm hình ảnh cao cấp:**
  - Thiết kế tông màu Teal & Emerald thiên nhiên, phong cách hiện đại.
  - Bản đồ tương tác Split View: Bên trái danh sách tour, bên phải bản đồ Leaflet OSM trực quan với các điểm ghim (markers) và thẻ pop-up thông tin nhanh.
  - Trang chi tiết tour với bộ đếm chỗ trống thời gian thực, bảng giá động theo ngày khởi hành.
  - Quản lý chuyến đi cá nhân: Tải vé điện tử, xem QR code, hủy tour có tính tiền hoàn dự kiến.

---

### Slide 10: Phân hệ Quản trị & Local Host Portal
- **Host Dashboard:**
  - Quản lý danh sách tour, mở lịch khởi hành, theo dõi doanh thu.
  - Form tạo tour chuẩn UX 5 bước: Thông tin chung, Giá & Thời lượng, Danh mục & Địa điểm, Lịch trình chi tiết, Hình ảnh minh họa.
  - Công cụ quét mã QR Check-in tức thì.
- **Admin Dashboard:**
  - Thẩm định hồ sơ KYC định danh Local Host (CCCD, kinh nghiệm, tài khoản ngân hàng).
  - Phê duyệt / Từ chối xuất bản tour mới lên sàn giao dịch.
  - Báo cáo thống kê toàn sàn.

---

### Slide 11: An ninh & Bảo mật Hệ thống (Security & Authentication)
- **Mã hóa mật khẩu:** Sử dụng thuật toán BCrypt với Salt ngẫu nhiên, kháng tấn công Rainbow Table.
- **Cơ chế xác thực:** JWT (JSON Web Token) dạng Stateless với Access Token (thời hạn 60 phút) và Refresh Token (7 ngày, lưu trữ có hạn dùng trong DB).
- **Phân quyền dựa trên vai trò (Role-Based Access Control - RBAC):** Sử dụng `[Authorize(Roles = "Admin,Host")]` trên từng Controller Endpoint.
- **Bảo mật giao dịch:** Chống giả mạo thanh toán bằng mã giao dịch duy nhất và kiểm tra trạng thái máy chủ.

---

### Slide 12: Kiểm thử Tự động & Đảm bảo Chất lượng (QA & Automated Testing)
- **Dự án kiểm thử:** `LocalExplore.UnitTests` xây dựng trên xUnit, FluentAssertions và EF Core InMemory Provider.
- **Độ bao phủ kiểm thử:** 100% các quy tắc nghiệp vụ cốt lõi (Core Business Rules):
  - Kiểm thử quá tải chỗ đặt (Overbooking Test).
  - Kiểm thử hoàn tiền 3 mốc thời gian (>72h, 24-72h, <24h).
  - Kiểm thử Check-in lần đầu thành công và chặn Check-in lặp lại (Replay attack).
  - Kiểm thử mã hóa BCrypt và tính toàn vẹn của Claim JWT.
- **Kết quả kiểm thử:** Toàn bộ 10/10 test case Passed 100%.

---

### Slide 13: Kịch bản Trình diễn Thực tế (Demo Flow 5 Phút)
- **Bước 1 (1 phút):** Host Sa Pa (Vàng A Thào) đăng nhập -> Tạo tour mới "Tập dệt thổ cẩm cùng người H'Mông".
- **Bước 2 (1 phút):** Admin đăng nhập -> Vào trang `TourApproval` -> Kiểm tra thông tin -> Bấm "Phê duyệt" để công khai tour lên sàn.
- **Bước 3 (1.5 phút):** Khách hàng (Linh) vào trang chủ -> Lọc tour -> Mở bản đồ tương tác Leaflet -> Chọn lịch khởi hành -> Đặt chỗ -> Thanh toán Sandbox giả lập -> Nhận vé mã QR.
- **Bước 4 (1 phút):** Host mở trang quét QR -> Quét vé của Linh -> Hệ thống báo "Xác thực thành công, chào đón khách" -> Cập nhật trạng thái `Completed`.
- **Bước 5 (0.5 phút):** Khách hàng vào trang "Vé của tôi" -> Viết đánh giá 5 sao -> Tour tự động cập nhật điểm trung bình lên 5.0.

---

### Slide 14: Đánh giá & So sánh Giải pháp
| Tiêu chí so sánh | Giải pháp CRUD thông thường | Nền tảng LocalExplore |
| :--- | :--- | :--- |
| **Kiến trúc mã nguồn** | N-Tier nguyên khối gộp Controller + DB | Clean Architecture 4 lớp chuẩn Enterprise |
| **Xử lý tranh chấp đặt chỗ** | Không có hoặc chỉ kiểm tra `Count` | `IsolationLevel.Serializable` ngăn chặn overbooking tuyệt đối |
| **Quy trình vé & Soát vé** | Chỉ có mã đơn hàng text | Vé điện tử QR động, định danh đơn lẻ chống quét lặp |
| **Chính sách hủy tour** | Không hoàn tiền hoặc hoàn thủ công | Động cơ hoàn tiền 3 nấc tự động theo mốc giờ |
| **Kiểm thử tự động** | Không có test | Suite Unit Test xUnit & FluentAssertions chạy tự động |
| **Đóng gói triển khai** | Chạy thủ công trên máy cục bộ | Dockerfile multi-stage & Docker Compose sẵn sàng production |

---

### Slide 15: Kết luận & Hướng phát triển tương lai
- **Kết quả đạt được:**
  - Hoàn thiện 100% mục tiêu đề ra theo chuẩn quy trình phát triển phần mềm chuyên nghiệp (SDLC).
  - Hệ thống Fullstack vận hành trơn tru, giao diện hiện đại, UX mượt mà.
- **Định hướng phát triển tiếp theo:**
  - Tích hợp cổng thanh toán thực tế (VNPAY IPN, MoMo Sandbox).
  - Xây dựng ứng dụng di động (Flutter / React Native) dành riêng cho Host để quét QR offline khi vào sâu trong các bản làng không có sóng 4G.
  - Ứng dụng AI (Recommendation System) gợi ý tour theo sở thích và vị trí GPS của người dùng.

---

## PHẦN II: BỘ 20 CÂU HỎI PHẢN BIỆN CỦA HỘI ĐỒNG VÀ HƯỚNG TRẢ LỜI MẪU

### Nhóm 1: Về Kiến trúc Phần mềm & Công nghệ

#### Câu 1: Tại sao em lại chọn Clean Architecture thay vì kiến trúc 3 lớp (3-Tier) truyền thống?
> **Trả lời:**
> "Kiến trúc 3 lớp truyền thống thường đặt Database làm trung tâm (Database-Driven), dẫn đến việc Business Logic bị phụ thuộc chặt vào schema của DB và Entity Framework. Khi cần thay đổi DB hoặc viết Unit Test, hệ thống rất khó cô lập.
> Ngược lại, Clean Architecture tuân theo **Quy tắc Phụ thuộc (Dependency Inversion Principle)**: Tầng Domain và Business Rules nằm ở trung tâm và hoàn toàn độc lập với công nghệ bên ngoài (DB, UI, Third-party service). Các tầng ngoài phụ thuộc vào Interface của tầng trong. Nhờ đó, em có thể viết Unit Test cho `BookingService` và `CheckInService` cực kỳ dễ dàng bằng In-Memory Database mà không cần chạy SQL Server thật, đồng thời mã nguồn có độ mở và bảo trì rất cao."

#### Câu 2: Ưu điểm của việc sử dụng ASP.NET Core .NET 9 Web API kết hợp Vue 3 SPA là gì?
> **Trả lời:**
> "Mô hình Decoupled (Tách biệt Frontend - Backend qua RESTful API) mang lại 3 ưu điểm vượt trội:
> 1. **Hiệu năng cao:** .NET 9 là một trong những web runtime có tốc độ xử lý I/O và memory footprint tốt nhất hiện nay (vượt trội hơn Node.js và Django trong các bài benchmark TechEmpower).
> 2. **Trải nghiệm mượt mà:** Vue 3 sử dụng Composition API và Vite cho tốc độ bundle cực nhanh, kết hợp cơ chế SPA (Single Page Application) giúp người dùng chuyển trang tức thì, không bị nháy màn hình (FOUC).
> 3. **Khả năng mở rộng (Scalability):** Backend API được thiết kế dạng Stateless, sau này có thể dễ dàng phục vụ thêm Mobile App (Flutter/React Native) mà không phải viết lại mã nguồn xử lý nghiệp vụ."

---

### Nhóm 2: Về Cơ sở Dữ liệu & Xử lý Tranh chấp (Concurrency)

#### Câu 3: Em giải quyết bài toán chống Overbooking (đặt trùng chỗ) như thế nào khi có nhiều người cùng đặt 1 tour tại cùng một thời điểm?
> **Trả lời:**
> "Em giải quyết ở 2 cấp độ:
> 1. **Cấp độ Database Transaction:** Trong `BookingService.CreateBookingAsync`, em bao bọc toàn bộ thao tác đọc `TourSchedule`, tính toán số chỗ và tạo đơn đặt tour vào một giao dịch với mức cô lập cao nhất: `await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable)`. Mức này thiết lập khóa đọc-ghi (Key-Range Lock), khiến bất kỳ transaction nào khác cố đọc hay ghi đè lên cùng bản ghi `TourSchedule` đều phải chờ cho đến khi transaction đầu tiên Commit hoặc Rollback.
> 2. **Kiểm tra sức chứa nguyên tử (Atomic Check):** Trước khi tạo đơn, hệ thống kiểm tra `schedule.BookedCapacity + requestedGuests <= schedule.MaxCapacity`. Nếu vượt quá, giao dịch bị hủy và trả về lỗi rõ ràng cho khách hàng kèm số chỗ thực tế còn lại."

#### Câu 4: Việc dùng `IsolationLevel.Serializable` có làm chậm hệ thống không? Khi quy mô hàng triệu người dùng thì tối ưu thế nào?
> **Trả lời:**
> "Mức `Serializable` đúng là có chi phí khóa cao hơn so với `ReadCommitted`. Tuy nhiên, trong nghiệp vụ tour trải nghiệm bản địa, số lượng chỗ trên mỗi suất rất giới hạn (thường chỉ 6-12 chỗ), rủi ro overbooking gây tổn thất uy tín rất lớn nên tính toàn vẹn (Consistency) được ưu tiên hàng đầu.
> Đối với bài toán quy mô lớn hàng triệu người dùng (High Concurrency):
> - Ta có thể chuyển sang kỹ thuật **Optimistic Concurrency Control** kết hợp trường `RowVersion` (Timestamp): Bằng cách kiểm tra `UPDATE TourSchedules SET BookedCapacity = BookedCapacity + @N WHERE Id = @Id AND BookedCapacity + @N <= MaxCapacity`.
> - Hoặc đưa cơ chế giữ chỗ vào **Distributed Cache với Redis và Lua Script** (như cơ chế hàng đợi vé của Ticketmaster) để giảm tải hoàn toàn cho Database chính."

#### Câu 5: Thiết kế Database của hệ thống đã đạt chuẩn mấy? Em đánh chỉ mục (Index) trên những bảng nào?
> **Trả lời:**
> "Database của hệ sinh thái gồm 18 bảng và đạt **Chuẩn 3NF (Third Normal Form)**:
> - Mỗi bảng có Khóa chính (PK), các thuộc tính nguyên tử (1NF).
> - Tất cả các cột không khóa phụ thuộc hoàn toàn vào Khóa chính (2NF).
> - Không có thuộc tính nào phụ thuộc bắc cầu (Transitive Dependency) vào khóa chính (3NF).
> Về chiến lược Indexing: Em đánh **Non-Clustered Indexes** tại:
> - Cột `Slug` của bảng `Tours`, `Locations`, `Categories` (phục vụ tìm kiếm URL thân thiện SEO).
> - Cột `DepartureDate` và `Status` của bảng `TourSchedules` (phục vụ lọc lịch khởi hành).
> - Cột `QrCodeToken` của bảng `Bookings` (đảm bảo tốc độ quét QR dưới 50ms khi Host check-in)."

---

### Nhóm 3: Về Nghiệp vụ Vé QR & Check-in

#### Câu 6: Làm thế nào để đảm bảo mã QR vé không thể bị làm giả hoặc bị khách chụp màn hình chia sẻ cho người khác đi cùng lúc?
> **Trả lời:**
> "Hệ thống bảo vệ vé bằng 3 lớp:
> 1. **Chuỗi token định danh ngẫu nhiên bảo mật:** Mã `QrCodeToken` được sinh bằng hàm mã hóa ngẫu nhiên an toàn (`Guid` kết hợp Cryptographic Random và Timestamp), không thể đoán mò được `Id` đơn đặt.
> 2. **Nguyên tắc Check-in 1 lần duy nhất (Single-Use Idempotency):** Khi Host quét vé, hệ thống kiểm tra bảng `CheckIns`. Nếu đã có bản ghi tồn tại cho vé này, hệ thống từ chối ngay lập tức và in rõ cảnh báo đỏ trên màn hình của Host: *'Mã vé đã được check-in lúc [Thời gian]!'*.
> 3. **Xác thực quyền của Host:** Chỉ Host sở hữu tour đó (hoặc Admin hệ thống) mới có quyền xác thực vé. Host khác quét vào tour của người khác sẽ bị từ chối."

#### Câu 7: Tại sao sau khi Check-in xong, trạng thái của đơn đặt lại chuyển thành `Completed`?
> **Trả lời:**
> "Trạng thái `Completed` là chốt chặn quan trọng trong vòng đời dịch vụ:
> - Về mặt nghiệp vụ: Khách hàng chỉ được phép gửi Đánh giá (Review) và Chấm sao sau khi chuyến đi đã hoàn tất (trạng thái `Completed` và `IsCheckedIn == true`). Điều này ngăn chặn triệt để tình trạng 'đánh giá ảo', 'review dạo' khi chưa từng tham gia trải nghiệm."

---

### Nhóm 4: Về An toàn & Bảo mật (Security)

#### Câu 8: Cơ chế JWT Refresh Token trong hệ thống hoạt động như thế nào?
> **Trả lời:**
> "Access Token trong hệ thống có thời hạn ngắn (60 phút) để giảm thiểu rủi ro khi bị lộ qua mạng. Khi Access Token hết hạn:
> - Axios Interceptor ở Frontend bắt mã lỗi `401 Unauthorized`.
> - Tự động gọi endpoint `/api/auth/refresh-token` gửi kèm `RefreshToken` bí mật được lưu trong máy khách.
> - Backend kiểm tra `RefreshToken` trong bảng `Users` còn hạn sử dụng (7 ngày) hay không. Nếu hợp lệ, cấp phát một cặp Access Token và Refresh Token mới (Refresh Token Rotation) rồi tiếp tục thực hiện yêu cầu mà người dùng không bị gián đoạn hay phải đăng nhập lại."

#### Câu 9: Mật khẩu người dùng được lưu trữ như thế nào? Em có kiểm tra độ an toàn không?
> **Trả lời:**
> "Mật khẩu người dùng tuyệt đối không lưu dạng văn bản thô (Plain Text). Em sử dụng thư viện **BCrypt.Net** để băm mật khẩu với Work Factor là 11 và Salt ngẫu nhiên được nhúng tự động trong chuỗi hash. Thuật toán này có tính năng làm chậm có chủ đích (Slow Hashing Algorithm), khiến hacker không thể dùng GPU hoặc Rainbow Table để bẻ khóa hàng loạt."

---

### Nhóm 5: Về Kiểm thử & Đóng gói Triển khai

#### Câu 10: Em đã kiểm thử hệ thống như thế nào? Tỷ lệ Pass ra sao?
> **Trả lời:**
> "Em xây dựng bộ kiểm thử tự động **Unit Testing** riêng biệt tại project `LocalExplore.UnitTests` với xUnit và FluentAssertions:
> - Sử dụng `Microsoft.EntityFrameworkCore.InMemory` để giả lập Database mà không làm ô nhiễm dữ liệu thật.
> - Tập trung kiểm thử 100% các logic nghiệp vụ rủi ro cao: Thuật toán chống overbooking, kiểm thử 3 mốc hoàn tiền hủy tour (>72h, 24-72h, <24h), kiểm thử bảo mật một lần quét mã QR, và kiểm thử JWT Claims.
> - Kết quả kiểm thử hiện tại: **10/10 test cases Passed 100%**."

#### Câu 11: File `docker-compose.yml` trong đồ án giải quyết bài toán gì?
> **Trả lời:**
> "Docker Compose đóng gói toàn bộ giải pháp thành một thể thống nhất gồm 3 dịch vụ:
> 1. `db`: SQL Server 2022 container.
> 2. `backend`: ASP.NET Core 9 Web API container (Multi-stage build tối ưu kích thước image).
> 3. `frontend`: Vue 3 build production phục vụ bởi Nginx làm Reverse Proxy.
> Nhờ đó, bất kỳ ai (Giảng viên, Nhà tuyển dụng) chỉ cần gõ lệnh `docker-compose up -d` là toàn bộ hệ sinh thái từ Database, Backend đến Frontend đều khởi chạy hoàn chỉnh mà không cần cài đặt thủ công .NET SDK, Node.js hay SQL Server Management Studio."
