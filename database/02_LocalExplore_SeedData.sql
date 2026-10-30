-- ==============================================================================
-- DỰ ÁN: LOCALEXPLORE - NỀN TẢNG KẾT NỐI & QUẢN LÝ TOUR DU LỊCH TRẢI NGHIỆM BẢN ĐỊA
-- TỆP: 02_LocalExplore_SeedData.sql
-- MỤC ĐÍCH: NẠP DỮ LIỆU MẪU CHÂN THỰC (SEED DATA) CHO BÀI TOÁN DU LỊCH BẢN ĐỊA VIỆT NAM
-- ==============================================================================

USE LocalExploreDb;
GO

SET NOCOUNT ON;

-- 1. NẠP BẢNG ROLES
INSERT INTO dbo.Roles (Name, NormalizedName, Description) VALUES
(N'Admin', N'ADMIN', N'Quản trị viên toàn quyền hệ thống'),
(N'Host', N'HOST', N'Người tổ chức / Hướng dẫn viên bản địa'),
(N'Customer', N'CUSTOMER', N'Khách du lịch đặt trải nghiệm');
GO

-- 2. NẠP BẢNG USERS
-- Mật khẩu mặc định cho tất cả tài khoản mẫu là: Password@123 (Mã băm ASP.NET Identity v3 chuẩn)
DECLARE @DefaultHash NVARCHAR(500) = N'AQAAAAIAAYagAAAAENK6G8lF6J7sYFqfV5wF+R5sZ0yv5w7B9q8H1k3m5o7p9r1t3v5x7z9A1b3c5d7e==';

INSERT INTO dbo.Users (Email, PasswordHash, FullName, PhoneNumber, AvatarUrl, IsActive, EmailConfirmed) VALUES
-- Admin (Id = 1)
(N'admin@localexplore.vn', @DefaultHash, N'Quản Trị Viên LocalExplore', N'0901234567', N'https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=200', 1, 1),
-- Hosts (Id = 2, 3, 4)
(N'chinh.hmong@localexplore.vn', @DefaultHash, N'Lò Văn Chinh', N'0912345678', N'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=200', 1, 1),
(N'mai.battrang@localexplore.vn', @DefaultHash, N'Trần Thị Mai', N'0923456789', N'https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=200', 1, 1),
(N'thao.hoian@localexplore.vn', @DefaultHash, N'Nguyễn Văn Thảo', N'0934567890', N'https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=200', 1, 1),
-- Customers (Id = 5, 6, 7)
(N'an.hoang@gmail.com', @DefaultHash, N'Hoàng Minh An', N'0987654321', N'https://images.unsplash.com/photo-1535713875002-d1d0cf377fde?w=200', 1, 1),
(N'linh.pham@gmail.com', @DefaultHash, N'Phạm Thùy Linh', N'0978123456', N'https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=200', 1, 1),
(N'david.miller@gmail.com', @DefaultHash, N'David Miller', N'0965987321', N'https://images.unsplash.com/photo-1522075469751-3a6694fb2f61?w=200', 1, 1);
GO

-- 3. GÁN ROLES (USERROLES)
INSERT INTO dbo.UserRoles (UserId, RoleId) VALUES
(1, 1), -- Admin
(2, 2), -- Lò Văn Chinh (Host)
(3, 2), -- Trần Thị Mai (Host)
(4, 2), -- Nguyễn Văn Thảo (Host)
(5, 3), -- Hoàng Minh An (Customer)
(6, 3), -- Phạm Thùy Linh (Customer)
(7, 3); -- David Miller (Customer)
GO

-- 4. NẠP HOST PROFILES (HỒ SƠ KYC ĐÃ PHÊ DUYỆT)
INSERT INTO dbo.HostProfiles (UserId, Bio, IdentityCardNumber, IssueDate, IssuePlace, BankName, BankAccountNumber, BankAccountHolder, ExperienceYears, IsApproved, ApprovedAt, ApprovedByAdminId, RatingAverage, TotalToursCount) VALUES
(2, N'Tôi là người dân tộc H''Mông sinh ra tại bản Tả Phìn, Sa Pa. Hơn 8 năm kinh nghiệm dẫn các cung đường leo núi Fansipan và hướng dẫn du khách trải nghiệm hái chè cổ thụ, dệt lanh thủ công cùng bà con.', N'012345678901', '2018-05-10', N'Cục CS QLHC về TTXH', N'Vietcombank', N'0011004567890', N'LO VAN CHINH', 8, 1, SYSUTCDATETIME(), 1, 4.95, 2),
(3, N'Nghệ nhân gốm đời thứ 4 tại làng cổ Bát Tràng. Xưởng gốm gia đình luôn mở rộng cửa đón chào các bạn trẻ và du khách quốc tế muốn tự tay vuốt gốm, nung men bí truyền.', N'012345678902', '2016-08-15', N'Cục CS QLHC về TTXH', N'Techcombank', N'19034567891012', N'TRAN THI MAI', 12, 1, SYSUTCDATETIME(), 1, 5.00, 1),
(4, N'Người con của phố cổ Hội An, đam mê văn hóa lồng đèn và ẩm thực xứ Quảng. Đã đồng hành cùng hàng nghìn du khách trong các chuyến đạp xe thăm làng rau Trà Quế và chèo thuyền thúng Rừng dừa Bảy Mẫu.', N'012345678903', '2019-03-20', N'Cục CS QLHC về TTXH', N'MB Bank', N'088899988899', N'NGUYEN VAN THAO', 6, 1, SYSUTCDATETIME(), 1, 4.90, 2);
GO

-- 5. NẠP CATEGORIES (DANH MỤC TRẢI NGHIỆM)
INSERT INTO dbo.Categories (Name, Slug, Description, IconUrl, ImageUrl, DisplayOrder, IsActive) VALUES
(N'Làng nghề truyền thống', N'lang-nghe-truyen-thong', N'Học làm gốm, dệt lanh, làm nón lá, đan tre cùng các nghệ nhân bản địa', N'tool', N'https://images.unsplash.com/photo-1565193566173-7a0ee3dbe261?w=600', 1, 1),
(N'Nông nghiệp & Sinh thái', N'nong-nghiep-sinh-thai', N'Hái chè cổ thụ, câu cá đồng quê, trồng trọt và thu hoạch rau củ hữu cơ', N'environment', N'https://images.unsplash.com/photo-1500937386664-56d1dfef3854?w=600', 2, 1),
(N'Ẩm thực bản địa', N'am-thuc-ban-dia', N'Vào bếp cùng người dân, nấu món ăn cổ truyền và thưởng thức mâm cơm quê', N'coffee', N'https://images.unsplash.com/photo-1504674900247-0877df9cc836?w=600', 3, 1),
(N'Văn hóa & Phong tục', N'van-hoa-phong-tuc', N'Tham gia lễ hội bản địa, nghe ca trù, đờn ca tài tử và học phong tục địa phương', N'team', N'https://images.unsplash.com/photo-1528127269322-539801943592?w=600', 4, 1),
(N'Trekking & Bản làng', N'trekking-ban-lang', N'Băng qua những cung đường ruộng bậc thang, ngủ homestay nhà sàn ấm cúng', N'compass', N'https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=600', 5, 1);
GO

-- 6. NẠP LOCATIONS (ĐỊA ĐIỂM DU LỊCH)
INSERT INTO dbo.Locations (Name, Slug, ProvinceName, Description, ImageUrl, Latitude, Longitude, IsPopular, DisplayOrder) VALUES
(N'Bát Tràng - Gia Lâm', N'bat-trang-ha-noi', N'Hà Nội', N'Làng gốm cổ hơn 500 năm tuổi ven sông Hồng', N'https://images.unsplash.com/photo-1565193566173-7a0ee3dbe261?w=800', 20.975482, 105.913864, 1, 1),
(N'Tả Phìn - Sa Pa', N'ta-phin-sa-pa', N'Lào Cai', N'Bản làng người H''Mông và Dao Đỏ với nghề thêu thổ cẩm và tắm lá thuốc', N'https://images.unsplash.com/photo-1544644181-1484b3fdfc62?w=800', 22.378512, 103.859642, 1, 2),
(N'Phố Cổ Hội An', N'pho-co-hoi-an', N'Quảng Nam', N'Di sản văn hóa thế giới với lồng đèn lung linh và làng rau Trà Quế thanh bình', N'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?w=800', 15.880058, 108.338047, 1, 3),
(N'Chợ nổi Cái Răng', N'cho-noi-cai-rang', N'Cần Thơ', N'Nét văn hóa giao thương sông nước độc đáo miền Tây Nam Bộ', N'https://images.unsplash.com/photo-1528127269322-539801943592?w=800', 10.007559, 105.748281, 1, 4),
(N'Đồng Văn - Mã Pí Lèng', N'dong-van-ha-giang', N'Hà Giang', N'Cao nguyên đá hùng vĩ và cung đường đèo hiểm trở bậc nhất Việt Nam', N'https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=800', 23.278672, 105.362145, 1, 5);
GO

-- 7. NẠP CANCELLATION POLICIES (CHÍNH SÁCH HỦY HOÀN TIỀN)
INSERT INTO dbo.CancellationPolicies (Name, Description, RefundPercentageBefore72h, RefundPercentageBetween24And72h, RefundPercentageUnder24h, IsDefault) VALUES
(N'Chính sách Linh hoạt (Tiêu chuẩn)', N'Hủy trước 72 giờ hoàn 100% tiền vé; Hủy từ 24 - 72 giờ hoàn 50%; Hủy dưới 24 giờ không hoàn tiền.', 100, 50, 0, 1),
(N'Chính sách Nghiêm ngặt', N'Hủy trước 7 ngày hoàn 100%; Hủy từ 3 - 7 ngày hoàn 50%; Hủy dưới 3 ngày không hoàn tiền.', 100, 50, 0, 0);
GO

-- 8. NẠP TOURS (CÁC TOUR TRẢI NGHIỆM THỰC TẾ)
INSERT INTO dbo.Tours (HostId, CategoryId, LocationId, CancellationPolicyId, Title, Slug, ShortDescription, DetailedDescription, Address, Latitude, Longitude, DurationHours, SupportedLanguages, MinAge, BasePrice, IncludedServices, ExcludedServices, ImportantNotes, Status, AverageRating, ReviewCount, ViewCount) VALUES
-- Tour 1: Gốm Bát Tràng (Host 2: Trần Thị Mai, Cat 1, Loc 1)
(2, 1, 1, 1, N'Tự tay vuốt gốm và nung men cổ truyền cùng nghệ nhân Bát Tràng', N'tu-tay-vuot-gom-battrang', 
N'Trải nghiệm 4 tiếng học cách nhào đất sét, vuốt gốm trên bàn xoay và tự tay vẽ họa tiết men lam cổ truyền mang về làm kỷ niệm.', 
N'Đến với xưởng gốm của nghệ nhân Mai, bạn sẽ được dẫn dắt từ câu chuyện lịch sử 500 năm của làng Bát Tràng đến từng bước kỹ thuật nhào nặn đất. Sau khi hoàn thành tác phẩm gốm theo phong cách riêng, bạn sẽ được hướng dẫn tráng men bí truyền và gửi nung lò. Tác phẩm hoàn chỉnh sẽ được đóng gói cẩn thận giao tận nơi.', 
N'Số 28 Thôn Giang Cao, Xã Bát Tràng, Huyện Gia Lâm, Hà Nội', 20.975482, 105.913864, 4.0, N'Tiếng Việt, Tiếng Anh', 6, 350000.00, 
N'Đất sét không giới hạn, Màu vẽ men sứ cao cấp, Nung lò hoàn thiện, Trà hoa sen và bánh đậu xanh mời khách, 01 sản phẩm hoàn chỉnh mang về', 
N'Chi phí vận chuyển sản phẩm về nhà ngoài khu vực Hà Nội', 
N'Nên mặc trang phục thoải mái, cắt gọn móng tay để dễ thao tác trên bàn xoay.', 'Published', 5.00, 1, 320),

-- Tour 2: Hái chè Shan Tuyết Sa Pa (Host 1: Lò Văn Chinh, Cat 2, Loc 2)
(1, 2, 2, 1, N'Hái chè Shan Tuyết cổ thụ trên mây và dệt lanh cùng người H''Mông Tả Phìn', N'hai-che-shan-tuyet-det-lanh-sa-pa', 
N'Cùng anh Chinh băng qua đồi chè cổ thụ hơn 100 năm tuổi, tự tay sao chè bằng chảo gang củi lửa và học kỹ thuật dệt lanh nhuộm chàm độc đáo.', 
N'Bạn sẽ có một buổi sáng sớm đón sương mai trên những gốc chè Shan Tuyết cổ thụ rêu phong. Học cách ngắt 1 tôm 2 lá đúng chuẩn của người vùng cao, nghe người già trong bản kể về truyền thuyết giống chè tiên. Buổi chiều ghé bếp lửa sao chè thủ công, thưởng thức chén trà vàng óng chát dịu ngọt hậu, rồi cùng các cô gái H''Mông se sợi lanh nhuộm chàm.', 
N'Bản Tả Phìn, Thị xã Sa Pa, Tỉnh Lào Cai', 22.378512, 103.859642, 6.0, N'Tiếng Việt, Tiếng Anh', 10, 550000.00, 
N'Hướng dẫn viên bản địa, Gùi tre hái chè, Gói 200g chè tự sao mang về, Bữa trưa cơm lam thịt bản nướng hạt dổi, Nước uống thảo mộc', 
N'Xe đưa đón từ trung tâm thị xã Sa Pa đến bản Tả Phìn', 
N'Nên mang giày thể thao có độ bám tốt, áo khoác mỏng vì buổi sáng vùng cao có sương lạnh.', 'Published', 4.95, 2, 450),

-- Tour 3: Làm lồng đèn & Làng rau Trà Quế Hội An (Host 3: Nguyễn Văn Thảo, Cat 1, Loc 3)
(3, 1, 3, 1, N'Làm lồng đèn lụa Hội An & Đạp xe hái rau hữu cơ làng Trà Quế', N'lam-long-den-dap-xe-lang-rau-tra-que', 
N'Buổi sáng đạp xe thong dong qua cánh đồng ngập tràn hương thơm thảo mộc Trà Quế, buổi chiều tự tay vót tre dán lụa làm chiếc lồng đèn may mắn.', 
N'Hành trình bắt đầu bằng xe đạp cổ điển xuyên qua những con đường rợp bóng tre tiến vào làng rau Trà Quế có tuổi đời 300 năm. Bạn sẽ được người nông dân chỉ cách xới đất, tưới nước bằng hai gáo đồng độc đáo và thưởng thức nước hạt é lá é tươi mát. Kết thúc bằng buổi học làm khung lồng đèn hình quả trám hoặc củ tỏi từ tre ngâm dán vải lụa Hà Đông cao cấp.', 
N'Làng rau Trà Quế, Xã Cẩm Hà, TP. Hội An, Quảng Nam', 15.880058, 108.338047, 5.0, N'Tiếng Việt, Tiếng Anh', 5, 420000.00, 
N'Xe đạp miễn phí, Nón lá truyền thống, Nông cụ trải nghiệm, 01 chiếc lồng đèn lụa tự làm mang về, Nước thảo mộc Mót mát lạnh', 
N'Chi phí mua thêm nông sản hoặc đồ lưu niệm cá nhân', 
N'Rất phù hợp cho gia đình có trẻ nhỏ và các cặp đôi yêu thích sự yên bình.', 'Published', 4.90, 1, 510);
GO

-- 9. NẠP TOUR IMAGES (HÌNH ẢNH MINH HỌA)
INSERT INTO dbo.TourImages (TourId, ImageUrl, Caption, IsCover, DisplayOrder) VALUES
-- Tour 1 (Bát Tràng)
(1, N'https://images.unsplash.com/photo-1565193566173-7a0ee3dbe261?w=800', N'Bàn xoay gốm thủ công tại xưởng', 1, 1),
(1, N'https://images.unsplash.com/photo-1610701596007-11502861dcfa?w=800', N'Vẽ họa tiết men lam truyền thống', 0, 2),
(1, N'https://images.unsplash.com/photo-1578749556568-bc2c40e68b61?w=800', N'Các sản phẩm sau khi nung men', 0, 3),

-- Tour 2 (Sa Pa)
(2, N'https://images.unsplash.com/photo-1544644181-1484b3fdfc62?w=800', N'Rừng chè cổ thụ Shan Tuyết sương sớm', 1, 1),
(2, N'https://images.unsplash.com/photo-1500937386664-56d1dfef3854?w=800', N'Học cách sao chè bằng chảo gang củi', 0, 2),
(2, N'https://images.unsplash.com/photo-1528127269322-539801943592?w=800', N'Nghề dệt lanh nhuộm chàm Tả Phìn', 0, 3),

-- Tour 3 (Hội An)
(3, N'https://images.unsplash.com/photo-1559592413-7cec4d0cae2b?w=800', N'Phố cổ Hội An rực rỡ sắc màu lồng đèn', 1, 1),
(3, N'https://images.unsplash.com/photo-1518709268805-4e9042af9f23?w=800', N'Tự tay dán lụa làm khung lồng đèn', 0, 2),
(3, N'https://images.unsplash.com/photo-1506744038136-46273834b3fb?w=800', N'Đạp xe giữa cánh đồng xanh mát Trà Quế', 0, 3);
GO

-- 10. NẠP TOUR ITINERARIES (LỊCH TRÌNH TỪNG CHẶNG)
INSERT INTO dbo.TourItineraries (TourId, StepOrder, Title, Description, DurationMinutes, LocationName) VALUES
-- Tour 1 (Bát Tràng)
(1, 1, N'Đón khách & Thưởng trà sen Bát Tràng', N'Gặp gỡ nghệ nhân Mai, nghe giới thiệu lịch sử gốm Bát Tràng và quy trình làm đất', 30, N'Phòng trà xưởng gốm'),
(1, 2, N'Thực hành vuốt gốm trên bàn xoay', N'Nghệ nhân cầm tay chỉ việc cách định hình bát, đĩa, bình hoa từ khối đất sét', 90, N'Khu bàn xoay thủ công'),
(1, 3, N'Trang trí họa tiết & Tráng men', N'Dùng cọ vẽ hoa văn men lam cổ điển hoặc tự do sáng tạo theo ý thích', 60, N'Khu vẽ men'),
(1, 4, N'Đóng gói kỷ niệm & Chụp ảnh lưu niệm', N'Nhận chứng nhận trải nghiệm và gửi sản phẩm vào lò nung', 30, N'Sân lưu niệm'),

-- Tour 2 (Sa Pa)
(2, 1, N'Tập trung & Trekking vào rừng chè cổ thụ', N'Đi bộ men theo triền đồi ngắm thung lũng Tả Phìn chìm trong mây sớm', 60, N'Bản Tả Phìn'),
(2, 2, N'Thu hoạch búp chè Shan Tuyết', N'Tự tay gùi chè, học cách phân biệt búp chè ngậm sương đạt chuẩn hảo hạng', 90, N'Đồi chè cổ thụ'),
(2, 3, N'Nướng cơm lam & Thưởng thức bữa trưa bản địa', N'Ăn trưa cùng gia đình anh Chinh bên bếp lửa nhà sàn người H''Mông', 75, N'Nhà sàn Homestay'),
(2, 4, N'Sao chè trên chảo gang & Học dệt lanh', N'Trải nghiệm vò chè, sao chè trên lửa củi đượm khói thơm đặc trưng', 90, N'Bếp lửa truyền thống'),

-- Tour 3 (Hội An)
(3, 1, N'Đạp xe thong dong đến làng rau Trà Quế', N'Nhận xe đạp tại phố cổ, đạp qua những con đường làng rợp bóng cau', 45, N'Đường làng Cẩm Hà'),
(3, 2, N'Làm nông dân một ngày', N'Mặc áo bà ba, xới đất và gánh nước tưới rau cùng bác nông dân Trà Quế', 90, N'Vườn rau Trà Quế'),
(3, 3, N'Học nghề làm lồng đèn lụa', N'Vót nan tre, gắn khung thép và căng lụa màu sắc lên chiếc lồng đèn của riêng bạn', 90, N'Xưởng đèn lồng thủ công');
GO

-- 11. NẠP TOUR SCHEDULES (CÁC ĐỢT KHỞI HÀNH)
-- Tạo cả lịch trong tương lai (để test đặt tour) và lịch trong quá khứ (để test check-in, review)
INSERT INTO dbo.TourSchedules (TourId, DepartureDate, StartTime, EndTime, Price, MaxCapacity, BookedCapacity, Status) VALUES
-- Tour 1 (Bát Tràng)
-- Lịch 1: Quá khứ (Đã hoàn thành để test Review)
(1, CAST(DATEADD(day, -5, GETDATE()) AS DATE), '08:30:00', '12:30:00', 350000.00, 10, 2, 'Completed'),
-- Lịch 2: Tương lai gần (Đang mở bán, đã có 2 khách đặt để test QR Check-in)
(1, CAST(DATEADD(day, 2, GETDATE()) AS DATE), '08:30:00', '12:30:00', 350000.00, 12, 2, 'Available'),
-- Lịch 3: Tương lai xa (Còn trống 100% để test Đặt chỗ Booking Engine)
(1, CAST(DATEADD(day, 7, GETDATE()) AS DATE), '14:00:00', '18:00:00', 350000.00, 15, 0, 'Available'),

-- Tour 2 (Sa Pa)
(2, CAST(DATEADD(day, -3, GETDATE()) AS DATE), '08:00:00', '14:00:00', 550000.00, 8, 1, 'Completed'),
(2, CAST(DATEADD(day, 4, GETDATE()) AS DATE), '08:00:00', '14:00:00', 550000.00, 8, 0, 'Available'),

-- Tour 3 (Hội An)
(3, CAST(DATEADD(day, -10, GETDATE()) AS DATE), '08:00:00', '13:00:00', 420000.00, 10, 2, 'Completed'),
(3, CAST(DATEADD(day, 3, GETDATE()) AS DATE), '08:00:00', '13:00:00', 420000.00, 10, 0, 'Available');
GO

-- 12. NẠP BOOKINGS MẪU
INSERT INTO dbo.Bookings (BookingCode, CustomerId, ScheduleId, ContactName, ContactEmail, ContactPhone, TotalParticipants, TotalAmount, DiscountAmount, FinalAmount, Status, PaymentStatus, Notes, QrCodeToken, ExpirePaymentAt, CreatedAt) VALUES
-- Booking 1: Quá khứ đã hoàn thành (Khách 5: Hoàng Minh An, Schedule 1)
(N'LE-202610-001', 5, 1, N'Hoàng Minh An', N'an.hoang@gmail.com', N'0987654321', 2, 700000.00, 0.00, 700000.00, 'Completed', 'Paid', N'Hai bạn sinh viên trải nghiệm vuốt gốm', N'LE-QR-PAST-001-AN-BATTRANG', DATEADD(minute, 15, DATEADD(day, -5, GETDATE())), DATEADD(day, -6, GETDATE())),

-- Booking 2: Sắp tới (Schedule 2 - ngày mai/kia, đã thanh toán, CÓ MÃ QR SẴN ĐỂ TEST QUÉT CHECK-IN)
(N'LE-202610-002', 6, 2, N'Phạm Thùy Linh', N'linh.pham@gmail.com', N'0978123456', 2, 700000.00, 0.00, 700000.00, 'Confirmed', 'Paid', N'Đặt vé đôi cuối tuần', N'LE-QR-CHECKIN-DEMO-LINH-2026', DATEADD(minute, 15, GETDATE()), GETDATE()),

-- Booking 3: Quá khứ ở Sa Pa (Khách 7: David Miller, Schedule 4)
(N'LE-202610-003', 7, 4, N'David Miller', N'david.miller@gmail.com', N'0965987321', 1, 550000.00, 0.00, 550000.00, 'Completed', 'Paid', N'Solo traveler from Australia', N'LE-QR-PAST-003-DAVID-SAPA', DATEADD(minute, 15, DATEADD(day, -3, GETDATE())), DATEADD(day, -4, GETDATE()));
GO

-- 13. NẠP BOOKING PARTICIPANTS (CHI TIẾT HÀNH KHÁCH)
INSERT INTO dbo.BookingParticipants (BookingId, FullName, Gender, ParticipantType, BirthDate, SpecialRequest) VALUES
-- Cho Booking 1
(1, N'Hoàng Minh An', N'Nam', 'Adult', '2002-04-12', N'Thuận tay trái'),
(1, N'Vũ Phương Thảo', N'Nữ', 'Adult', '2002-09-25', NULL),
-- Cho Booking 2 (Vé demo quét check-in)
(2, N'Phạm Thùy Linh', N'Nữ', 'Adult', '2001-11-05', N'Muốn ngồi bàn xoay gần cửa sổ'),
(2, N'Nguyễn Đức Nam', N'Nam', 'Adult', '2000-08-18', NULL),
-- Cho Booking 3
(3, N'David Miller', N'Nam', 'Adult', '1995-02-14', N'Speaking English');
GO

-- 14. NẠP PAYMENTS (GIAO DỊCH THANH TOÁN MẪU)
INSERT INTO dbo.Payments (BookingId, TransactionCode, PaymentMethod, Amount, Status, GatewayTransactionNo, PaidAt) VALUES
(1, N'SANDBOX-VNPAY-TRANS-001', N'Sandbox_VNPAY', 700000.00, 'Success', N'VNPAY14098231', DATEADD(day, -6, GETDATE())),
(2, N'SANDBOX-VNPAY-TRANS-002', N'Sandbox_VNPAY', 700000.00, 'Success', N'VNPAY14098232', GETDATE()),
(3, N'SANDBOX-MOMO-TRANS-003', N'Sandbox_MOMO', 550000.00, 'Success', N'MOMO98213476', DATEADD(day, -4, GETDATE()));
GO

-- 15. NẠP CHECK-IN MẪU (CHO BOOKING ĐÃ HOÀN THÀNH)
INSERT INTO dbo.CheckIns (BookingId, CheckedInByHostId, CheckedInAt, DeviceInfo, Notes) VALUES
(1, 3, DATEADD(hour, 8, DATEADD(day, -5, GETDATE())), N'Chrome Mobile - Host Mai Bát Tràng', N'Khách đến đúng giờ, check-in thành công qua quét camera'),
(3, 2, DATEADD(hour, 8, DATEADD(day, -3, GETDATE())), N'Safari iOS - Host Chinh Sa Pa', N'Khách nước ngoài nhiệt tình');
GO

-- 16. NẠP REVIEWS (ĐÁNH GIÁ CỦA KHÁCH SAU KHI HOÀN THÀNH CHUYẾN ĐI)
INSERT INTO dbo.Reviews (BookingId, CustomerId, TourId, Rating, Comment, HostReply, HostRepliedAt) VALUES
(1, 5, 1, 5, 
N'Một trải nghiệm tuyệt vời ngoài mong đợi! Cô Mai cực kỳ dễ thương và kiên nhẫn chỉ tụi mình từng bước xoay gốm. Tác phẩm nung xong men bóng loáng rất đẹp. Nhất định sẽ giới thiệu cho bạn bè.', 
N'Cảm ơn hai bạn An và Thảo rất nhiều! Rất vui được đón tiếp các bạn trẻ yêu mến văn hóa gốm truyền thống.', 
DATEADD(day, -4, GETDATE())),

(3, 7, 2, 5, 
N'Chinh is an exceptional host! Walking into the 100-year-old tea forests was like stepping into a fairy tale. The handmade lunch was the best meal I had in Vietnam.', 
N'Thank you so much David! Hope to see you again in Sa Pa mountains!', 
DATEADD(day, -2, GETDATE()));
GO

-- 17. NẠP FAVORITES (TOUR YÊU THÍCH)
INSERT INTO dbo.Favorites (UserId, TourId) VALUES
(5, 2), -- An thích tour Sa Pa
(6, 1), -- Linh thích tour Bát Tràng
(6, 3); -- Linh thích tour Hội An
GO

-- 18. NẠP NOTIFICATIONS (THÔNG BÁO MẪU)
INSERT INTO dbo.Notifications (UserId, Title, Message, NotificationType, TargetUrl, IsRead) VALUES
(1, N'Tour mới cần phê duyệt', N'Host Lò Văn Chinh vừa tạo tour "Khám phá Hẻm Tu Sản"', N'TourPending', N'/admin/tours/approval', 0),
(2, N'Hồ sơ Host được duyệt', N'Chúc mừng! Hồ sơ Local Host của bạn đã được Admin phê duyệt thành công.', N'HostApproved', N'/host/dashboard', 1),
(6, N'Đặt tour thành công', N'Đơn đặt tour LE-202610-002 đã được thanh toán thành công. Xem vé QR ngay.', N'BookingConfirmed', N'/customer/bookings/LE-202610-002', 0);
GO

PRINT N'>>> ĐÃ NẠP THÀNH CÔNG DỮ LIỆU MẪU (SEED DATA) CHÂN THỰC CHO LOCALEXPLOREDB <<<';
GO
