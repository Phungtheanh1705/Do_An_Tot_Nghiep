-- ==============================================================================
-- DỰ ÁN: LOCALEXPLORE - NỀN TẢNG KẾT NỐI & QUẢN LÝ TOUR DU LỊCH TRẢI NGHIỆM BẢN ĐỊA
-- TỆP: 01_LocalExplore_Schema.sql
-- MỤC ĐÍCH: TẠO CƠ SỞ DỮ LIỆU VÀ TOÀN BỘ 18 BẢNG, RÀNG BUỘC (PK, FK, CHECK), INDEX
-- RDBMS: Microsoft SQL Server 2019 / 2022
-- ==============================================================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'LocalExploreDb')
BEGIN
    CREATE DATABASE LocalExploreDb COLLATE SQL_Latin1_General_CP1_CI_AS;
END
GO

USE LocalExploreDb;
GO

-- XÓA BẢNG CŨ THEO THỨ TỰ PHỤ THUỘC (NẾU CẦN RESET)
IF OBJECT_ID('dbo.Notifications', 'U') IS NOT NULL DROP TABLE dbo.Notifications;
IF OBJECT_ID('dbo.Favorites', 'U') IS NOT NULL DROP TABLE dbo.Favorites;
IF OBJECT_ID('dbo.Reviews', 'U') IS NOT NULL DROP TABLE dbo.Reviews;
IF OBJECT_ID('dbo.CheckIns', 'U') IS NOT NULL DROP TABLE dbo.CheckIns;
IF OBJECT_ID('dbo.Payments', 'U') IS NOT NULL DROP TABLE dbo.Payments;
IF OBJECT_ID('dbo.BookingParticipants', 'U') IS NOT NULL DROP TABLE dbo.BookingParticipants;
IF OBJECT_ID('dbo.Bookings', 'U') IS NOT NULL DROP TABLE dbo.Bookings;
IF OBJECT_ID('dbo.TourSchedules', 'U') IS NOT NULL DROP TABLE dbo.TourSchedules;
IF OBJECT_ID('dbo.TourItineraries', 'U') IS NOT NULL DROP TABLE dbo.TourItineraries;
IF OBJECT_ID('dbo.TourImages', 'U') IS NOT NULL DROP TABLE dbo.TourImages;
IF OBJECT_ID('dbo.Tours', 'U') IS NOT NULL DROP TABLE dbo.Tours;
IF OBJECT_ID('dbo.CancellationPolicies', 'U') IS NOT NULL DROP TABLE dbo.CancellationPolicies;
IF OBJECT_ID('dbo.Locations', 'U') IS NOT NULL DROP TABLE dbo.Locations;
IF OBJECT_ID('dbo.Categories', 'U') IS NOT NULL DROP TABLE dbo.Categories;
IF OBJECT_ID('dbo.HostProfiles', 'U') IS NOT NULL DROP TABLE dbo.HostProfiles;
IF OBJECT_ID('dbo.UserRoles', 'U') IS NOT NULL DROP TABLE dbo.UserRoles;
IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL DROP TABLE dbo.Roles;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
GO

-- ==============================================================================
-- 1. BẢNG USERS (Tài khoản người dùng toàn hệ thống)
-- ==============================================================================
CREATE TABLE dbo.Users (
    Id INT IDENTITY(1,1) CONSTRAINT PK_Users PRIMARY KEY CLUSTERED,
    Email NVARCHAR(256) NOT NULL CONSTRAINT UQ_Users_Email UNIQUE,
    PasswordHash NVARCHAR(500) NOT NULL,
    FullName NVARCHAR(150) NOT NULL,
    PhoneNumber NVARCHAR(20) NULL,
    AvatarUrl NVARCHAR(500) NULL,
    IsActive BIT NOT NULL CONSTRAINT DF_Users_IsActive DEFAULT (1),
    EmailConfirmed BIT NOT NULL CONSTRAINT DF_Users_EmailConfirmed DEFAULT (0),
    RefreshToken NVARCHAR(500) NULL,
    RefreshTokenExpiryTime DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSUTCDATETIME()),
    UpdatedAt DATETIME2 NULL
);
GO

CREATE NONCLUSTERED INDEX IX_Users_Email ON dbo.Users(Email);
CREATE NONCLUSTERED INDEX IX_Users_IsActive ON dbo.Users(IsActive);
GO

-- ==============================================================================
-- 2. BẢNG ROLES (Danh mục vai trò)
-- ==============================================================================
CREATE TABLE dbo.Roles (
    Id INT IDENTITY(1,1) CONSTRAINT PK_Roles PRIMARY KEY CLUSTERED,
    Name NVARCHAR(50) NOT NULL CONSTRAINT UQ_Roles_Name UNIQUE,
    NormalizedName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(250) NULL
);
GO

-- ==============================================================================
-- 3. BẢNG USERROLES (Quan hệ Many-to-Many Users - Roles)
-- ==============================================================================
CREATE TABLE dbo.UserRoles (
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    AssignedAt DATETIME2 NOT NULL CONSTRAINT DF_UserRoles_AssignedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_UserRoles PRIMARY KEY CLUSTERED (UserId, RoleId),
    CONSTRAINT FK_UserRoles_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleId) REFERENCES dbo.Roles(Id) ON DELETE CASCADE
);
GO

-- ==============================================================================
-- 4. BẢNG HOSTPROFILES (Hồ sơ định danh & KYC của Local Host)
-- Quan hệ 1-1 với Users
-- ==============================================================================
CREATE TABLE dbo.HostProfiles (
    Id INT IDENTITY(1,1) CONSTRAINT PK_HostProfiles PRIMARY KEY CLUSTERED,
    UserId INT NOT NULL CONSTRAINT UQ_HostProfiles_UserId UNIQUE,
    Bio NVARCHAR(MAX) NULL,
    IdentityCardNumber NVARCHAR(30) NOT NULL,
    IssueDate DATE NULL,
    IssuePlace NVARCHAR(150) NULL,
    FrontCardUrl NVARCHAR(500) NULL,
    BackCardUrl NVARCHAR(500) NULL,
    BankName NVARCHAR(100) NOT NULL,
    BankAccountNumber NVARCHAR(50) NOT NULL,
    BankAccountHolder NVARCHAR(150) NOT NULL,
    ExperienceYears INT NOT NULL CONSTRAINT DF_HostProfiles_Exp DEFAULT (1),
    IsApproved BIT NOT NULL CONSTRAINT DF_HostProfiles_IsApproved DEFAULT (0),
    ApprovedAt DATETIME2 NULL,
    ApprovedByAdminId INT NULL,
    RejectionReason NVARCHAR(500) NULL,
    RatingAverage DECIMAL(3,2) NOT NULL CONSTRAINT DF_HostProfiles_Rating DEFAULT (5.00),
    TotalToursCount INT NOT NULL CONSTRAINT DF_HostProfiles_ToursCount DEFAULT (0),
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_HostProfiles_CreatedAt DEFAULT (SYSUTCDATETIME()),
    UpdatedAt DATETIME2 NULL,
    CONSTRAINT FK_HostProfiles_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_HostProfiles_Admin FOREIGN KEY (ApprovedByAdminId) REFERENCES dbo.Users(Id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX IX_HostProfiles_IsApproved ON dbo.HostProfiles(IsApproved);
GO

-- ==============================================================================
-- 5. BẢNG CATEGORIES (Danh mục loại trải nghiệm du lịch)
-- ==============================================================================
CREATE TABLE dbo.Categories (
    Id INT IDENTITY(1,1) CONSTRAINT PK_Categories PRIMARY KEY CLUSTERED,
    Name NVARCHAR(100) NOT NULL CONSTRAINT UQ_Categories_Name UNIQUE,
    Slug NVARCHAR(120) NOT NULL CONSTRAINT UQ_Categories_Slug UNIQUE,
    Description NVARCHAR(500) NULL,
    IconUrl NVARCHAR(500) NULL,
    ImageUrl NVARCHAR(500) NULL,
    DisplayOrder INT NOT NULL CONSTRAINT DF_Categories_DisplayOrder DEFAULT (0),
    IsActive BIT NOT NULL CONSTRAINT DF_Categories_IsActive DEFAULT (1),
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Categories_CreatedAt DEFAULT (SYSUTCDATETIME())
);
GO

-- ==============================================================================
-- 6. BẢNG LOCATIONS (Tỉnh thành / Điểm đến du lịch)
-- ==============================================================================
CREATE TABLE dbo.Locations (
    Id INT IDENTITY(1,1) CONSTRAINT PK_Locations PRIMARY KEY CLUSTERED,
    Name NVARCHAR(150) NOT NULL,
    Slug NVARCHAR(160) NOT NULL CONSTRAINT UQ_Locations_Slug UNIQUE,
    ProvinceName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    ImageUrl NVARCHAR(500) NULL,
    Latitude DECIMAL(9,6) NOT NULL,
    Longitude DECIMAL(9,6) NOT NULL,
    IsPopular BIT NOT NULL CONSTRAINT DF_Locations_IsPopular DEFAULT (0),
    DisplayOrder INT NOT NULL CONSTRAINT DF_Locations_DisplayOrder DEFAULT (0),
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Locations_CreatedAt DEFAULT (SYSUTCDATETIME())
);
GO

CREATE NONCLUSTERED INDEX IX_Locations_Province ON dbo.Locations(ProvinceName);
GO

-- ==============================================================================
-- 7. BẢNG CANCELLATIONPOLICIES (Chính sách hủy tour & hoàn tiền)
-- ==============================================================================
CREATE TABLE dbo.CancellationPolicies (
    Id INT IDENTITY(1,1) CONSTRAINT PK_CancellationPolicies PRIMARY KEY CLUSTERED,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NOT NULL,
    RefundPercentageBefore72h INT NOT NULL CONSTRAINT DF_CP_72h DEFAULT (100),
    RefundPercentageBetween24And72h INT NOT NULL CONSTRAINT DF_CP_24_72h DEFAULT (50),
    RefundPercentageUnder24h INT NOT NULL CONSTRAINT DF_CP_24h DEFAULT (0),
    IsDefault BIT NOT NULL CONSTRAINT DF_CP_IsDefault DEFAULT (0),
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_CP_CreatedAt DEFAULT (SYSUTCDATETIME())
);
GO

-- ==============================================================================
-- 8. BẢNG TOURS (Thông tin trải nghiệm / tour du lịch)
-- ==============================================================================
CREATE TABLE dbo.Tours (
    Id INT IDENTITY(1,1) CONSTRAINT PK_Tours PRIMARY KEY CLUSTERED,
    HostId INT NOT NULL,
    CategoryId INT NOT NULL,
    LocationId INT NOT NULL,
    CancellationPolicyId INT NOT NULL,
    Title NVARCHAR(250) NOT NULL,
    Slug NVARCHAR(280) NOT NULL CONSTRAINT UQ_Tours_Slug UNIQUE,
    ShortDescription NVARCHAR(500) NOT NULL,
    DetailedDescription NVARCHAR(MAX) NOT NULL,
    Address NVARCHAR(300) NOT NULL,
    Latitude DECIMAL(9,6) NOT NULL,
    Longitude DECIMAL(9,6) NOT NULL,
    DurationHours DECIMAL(4,1) NOT NULL,
    SupportedLanguages NVARCHAR(150) NOT NULL CONSTRAINT DF_Tours_Languages DEFAULT (N'Tiếng Việt'),
    MinAge INT NOT NULL CONSTRAINT DF_Tours_MinAge DEFAULT (5),
    BasePrice DECIMAL(18,2) NOT NULL,
    IncludedServices NVARCHAR(MAX) NULL,
    ExcludedServices NVARCHAR(MAX) NULL,
    ImportantNotes NVARCHAR(MAX) NULL,
    Status NVARCHAR(30) NOT NULL CONSTRAINT DF_Tours_Status DEFAULT ('Draft'),
    RejectionReason NVARCHAR(500) NULL,
    AverageRating DECIMAL(3,2) NOT NULL CONSTRAINT DF_Tours_AvgRating DEFAULT (0.00),
    ReviewCount INT NOT NULL CONSTRAINT DF_Tours_ReviewCount DEFAULT (0),
    ViewCount INT NOT NULL CONSTRAINT DF_Tours_ViewCount DEFAULT (0),
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Tours_CreatedAt DEFAULT (SYSUTCDATETIME()),
    UpdatedAt DATETIME2 NULL,
    CONSTRAINT CK_Tours_Status CHECK (Status IN ('Draft', 'PendingApproval', 'Published', 'Rejected', 'Hidden', 'Archived')),
    CONSTRAINT FK_Tours_HostProfiles FOREIGN KEY (HostId) REFERENCES dbo.HostProfiles(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Tours_Categories FOREIGN KEY (CategoryId) REFERENCES dbo.Categories(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Tours_Locations FOREIGN KEY (LocationId) REFERENCES dbo.Locations(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Tours_CancellationPolicies FOREIGN KEY (CancellationPolicyId) REFERENCES dbo.CancellationPolicies(Id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX IX_Tours_HostId ON dbo.Tours(HostId);
CREATE NONCLUSTERED INDEX IX_Tours_CategoryId ON dbo.Tours(CategoryId);
CREATE NONCLUSTERED INDEX IX_Tours_LocationId ON dbo.Tours(LocationId);
CREATE NONCLUSTERED INDEX IX_Tours_Status ON dbo.Tours(Status);
CREATE NONCLUSTERED INDEX IX_Tours_Price ON dbo.Tours(BasePrice);
CREATE NONCLUSTERED INDEX IX_Tours_Rating ON dbo.Tours(AverageRating DESC);
GO

-- ==============================================================================
-- 9. BẢNG TOURIMAGES (Hình ảnh tour)
-- ==============================================================================
CREATE TABLE dbo.TourImages (
    Id INT IDENTITY(1,1) CONSTRAINT PK_TourImages PRIMARY KEY CLUSTERED,
    TourId INT NOT NULL,
    ImageUrl NVARCHAR(500) NOT NULL,
    Caption NVARCHAR(250) NULL,
    IsCover BIT NOT NULL CONSTRAINT DF_TourImages_IsCover DEFAULT (0),
    DisplayOrder INT NOT NULL CONSTRAINT DF_TourImages_DisplayOrder DEFAULT (0),
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_TourImages_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT FK_TourImages_Tours FOREIGN KEY (TourId) REFERENCES dbo.Tours(Id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX IX_TourImages_TourId ON dbo.TourImages(TourId);
GO

-- ==============================================================================
-- 10. BẢNG TOURITINERARIES (Lịch trình từng chặng / mốc thời gian của tour)
-- ==============================================================================
CREATE TABLE dbo.TourItineraries (
    Id INT IDENTITY(1,1) CONSTRAINT PK_TourItineraries PRIMARY KEY CLUSTERED,
    TourId INT NOT NULL,
    StepOrder INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    DurationMinutes INT NOT NULL,
    LocationName NVARCHAR(200) NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_TourItineraries_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT FK_TourItineraries_Tours FOREIGN KEY (TourId) REFERENCES dbo.Tours(Id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX IX_TourItineraries_TourId ON dbo.TourItineraries(TourId);
GO

-- ==============================================================================
-- 11. BẢNG TOURSCHEDULES (Các đợt / lịch khởi hành cụ thể của tour)
-- ==============================================================================
CREATE TABLE dbo.TourSchedules (
    Id INT IDENTITY(1,1) CONSTRAINT PK_TourSchedules PRIMARY KEY CLUSTERED,
    TourId INT NOT NULL,
    DepartureDate DATE NOT NULL,
    StartTime TIME(0) NOT NULL,
    EndTime TIME(0) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    MaxCapacity INT NOT NULL,
    BookedCapacity INT NOT NULL CONSTRAINT DF_TourSchedules_BookedCapacity DEFAULT (0),
    Status NVARCHAR(30) NOT NULL CONSTRAINT DF_TourSchedules_Status DEFAULT ('Available'),
    RowVersion ROWVERSION, -- Phục vụ Optimistic Concurrency Control
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_TourSchedules_CreatedAt DEFAULT (SYSUTCDATETIME()),
    UpdatedAt DATETIME2 NULL,
    CONSTRAINT CK_TourSchedules_Status CHECK (Status IN ('Available', 'FullyBooked', 'Cancelled', 'Completed')),
    CONSTRAINT CK_TourSchedules_Capacity CHECK (BookedCapacity <= MaxCapacity),
    CONSTRAINT FK_TourSchedules_Tours FOREIGN KEY (TourId) REFERENCES dbo.Tours(Id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX IX_TourSchedules_TourDate ON dbo.TourSchedules(TourId, DepartureDate);
CREATE NONCLUSTERED INDEX IX_TourSchedules_Status ON dbo.TourSchedules(Status);
GO

-- ==============================================================================
-- 12. BẢNG BOOKINGS (Đơn đặt tour của khách hàng)
-- ==============================================================================
CREATE TABLE dbo.Bookings (
    Id INT IDENTITY(1,1) CONSTRAINT PK_Bookings PRIMARY KEY CLUSTERED,
    BookingCode NVARCHAR(25) NOT NULL CONSTRAINT UQ_Bookings_Code UNIQUE,
    CustomerId INT NOT NULL,
    ScheduleId INT NOT NULL,
    ContactName NVARCHAR(150) NOT NULL,
    ContactEmail NVARCHAR(256) NOT NULL,
    ContactPhone NVARCHAR(20) NOT NULL,
    TotalParticipants INT NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    DiscountAmount DECIMAL(18,2) NOT NULL CONSTRAINT DF_Bookings_Discount DEFAULT (0.00),
    FinalAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(30) NOT NULL CONSTRAINT DF_Bookings_Status DEFAULT ('PendingPayment'),
    PaymentStatus NVARCHAR(30) NOT NULL CONSTRAINT DF_Bookings_PayStatus DEFAULT ('Unpaid'),
    Notes NVARCHAR(500) NULL,
    QrCodeToken NVARCHAR(256) NOT NULL CONSTRAINT UQ_Bookings_QrCode UNIQUE,
    ExpirePaymentAt DATETIME2 NOT NULL, -- Thời điểm hết hạn giữ chỗ (15 phút)
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Bookings_CreatedAt DEFAULT (SYSUTCDATETIME()),
    UpdatedAt DATETIME2 NULL,
    CONSTRAINT CK_Bookings_Status CHECK (Status IN ('PendingPayment', 'Paid', 'Confirmed', 'Cancelled', 'Completed', 'Refunded', 'Rejected')),
    CONSTRAINT CK_Bookings_PaymentStatus CHECK (PaymentStatus IN ('Unpaid', 'Pending', 'Paid', 'Refunded', 'Failed')),
    CONSTRAINT FK_Bookings_Users FOREIGN KEY (CustomerId) REFERENCES dbo.Users(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Bookings_TourSchedules FOREIGN KEY (ScheduleId) REFERENCES dbo.TourSchedules(Id) ON DELETE NO ACTION
);
GO

CREATE NONCLUSTERED INDEX IX_Bookings_CustomerId ON dbo.Bookings(CustomerId);
CREATE NONCLUSTERED INDEX IX_Bookings_ScheduleId ON dbo.Bookings(ScheduleId);
CREATE NONCLUSTERED INDEX IX_Bookings_Status ON dbo.Bookings(Status);
CREATE NONCLUSTERED INDEX IX_Bookings_QrCodeToken ON dbo.Bookings(QrCodeToken);
GO

-- ==============================================================================
-- 13. BẢNG BOOKINGPARTICIPANTS (Chi tiết từng hành khách trong đơn)
-- ==============================================================================
CREATE TABLE dbo.BookingParticipants (
    Id INT IDENTITY(1,1) CONSTRAINT PK_BookingParticipants PRIMARY KEY CLUSTERED,
    BookingId INT NOT NULL,
    FullName NVARCHAR(150) NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    ParticipantType NVARCHAR(20) NOT NULL CONSTRAINT DF_BP_Type DEFAULT ('Adult'),
    BirthDate DATE NULL,
    SpecialRequest NVARCHAR(250) NULL,
    CONSTRAINT CK_BP_Type CHECK (ParticipantType IN ('Adult', 'Child')),
    CONSTRAINT FK_BookingParticipants_Bookings FOREIGN KEY (BookingId) REFERENCES dbo.Bookings(Id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX IX_BookingParticipants_BookingId ON dbo.BookingParticipants(BookingId);
GO

-- ==============================================================================
-- 14. BẢNG PAYMENTS (Lịch sử giao dịch thanh toán)
-- ==============================================================================
CREATE TABLE dbo.Payments (
    Id INT IDENTITY(1,1) CONSTRAINT PK_Payments PRIMARY KEY CLUSTERED,
    BookingId INT NOT NULL,
    TransactionCode NVARCHAR(100) NOT NULL CONSTRAINT UQ_Payments_TransCode UNIQUE,
    PaymentMethod NVARCHAR(50) NOT NULL, -- 'Sandbox_VNPAY', 'Sandbox_MOMO', 'Direct'
    Amount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(30) NOT NULL CONSTRAINT DF_Payments_Status DEFAULT ('Pending'),
    GatewayTransactionNo NVARCHAR(100) NULL,
    GatewayResponse NVARCHAR(MAX) NULL,
    PaidAt DATETIME2 NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Payments_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT CK_Payments_Status CHECK (Status IN ('Pending', 'Success', 'Failed', 'Cancelled')),
    CONSTRAINT FK_Payments_Bookings FOREIGN KEY (BookingId) REFERENCES dbo.Bookings(Id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX IX_Payments_BookingId ON dbo.Payments(BookingId);
CREATE NONCLUSTERED INDEX IX_Payments_Status ON dbo.Payments(Status);
GO

-- ==============================================================================
-- 15. BẢNG CHECKINS (Ghi nhận quét mã QR vé tại điểm đón)
-- ==============================================================================
CREATE TABLE dbo.CheckIns (
    Id INT IDENTITY(1,1) CONSTRAINT PK_CheckIns PRIMARY KEY CLUSTERED,
    BookingId INT NOT NULL CONSTRAINT UQ_CheckIns_BookingId UNIQUE, -- 1 booking chỉ check-in 1 lần duy nhất
    CheckedInByHostId INT NOT NULL,
    CheckedInAt DATETIME2 NOT NULL CONSTRAINT DF_CheckIns_CheckedInAt DEFAULT (SYSUTCDATETIME()),
    DeviceInfo NVARCHAR(200) NULL,
    Notes NVARCHAR(500) NULL,
    CONSTRAINT FK_CheckIns_Bookings FOREIGN KEY (BookingId) REFERENCES dbo.Bookings(Id) ON DELETE CASCADE,
    CONSTRAINT FK_CheckIns_Users FOREIGN KEY (CheckedInByHostId) REFERENCES dbo.Users(Id) ON DELETE NO ACTION
);
GO

-- ==============================================================================
-- 16. BẢNG REVIEWS (Đánh giá & chấm điểm sau chuyến đi)
-- ==============================================================================
CREATE TABLE dbo.Reviews (
    Id INT IDENTITY(1,1) CONSTRAINT PK_Reviews PRIMARY KEY CLUSTERED,
    BookingId INT NOT NULL CONSTRAINT UQ_Reviews_BookingId UNIQUE, -- Mỗi vé hoàn thành chỉ được review 1 lần
    CustomerId INT NOT NULL,
    TourId INT NOT NULL,
    Rating INT NOT NULL,
    Comment NVARCHAR(MAX) NOT NULL,
    ImageUrls NVARCHAR(MAX) NULL, -- JSON array các url ảnh review
    HostReply NVARCHAR(MAX) NULL,
    HostRepliedAt DATETIME2 NULL,
    IsHidden BIT NOT NULL CONSTRAINT DF_Reviews_IsHidden DEFAULT (0),
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Reviews_CreatedAt DEFAULT (SYSUTCDATETIME()),
    UpdatedAt DATETIME2 NULL,
    CONSTRAINT CK_Reviews_Rating CHECK (Rating >= 1 AND Rating <= 5),
    CONSTRAINT FK_Reviews_Bookings FOREIGN KEY (BookingId) REFERENCES dbo.Bookings(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Reviews_Users FOREIGN KEY (CustomerId) REFERENCES dbo.Users(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Reviews_Tours FOREIGN KEY (TourId) REFERENCES dbo.Tours(Id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX IX_Reviews_TourId ON dbo.Reviews(TourId);
CREATE NONCLUSTERED INDEX IX_Reviews_CustomerId ON dbo.Reviews(CustomerId);
GO

-- ==============================================================================
-- 17. BẢNG FAVORITES (Danh sách tour yêu thích của khách hàng)
-- ==============================================================================
CREATE TABLE dbo.Favorites (
    Id INT IDENTITY(1,1) CONSTRAINT PK_Favorites PRIMARY KEY CLUSTERED,
    UserId INT NOT NULL,
    TourId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Favorites_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT UQ_Favorites_User_Tour UNIQUE (UserId, TourId),
    CONSTRAINT FK_Favorites_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Favorites_Tours FOREIGN KEY (TourId) REFERENCES dbo.Tours(Id) ON DELETE CASCADE
);
GO

-- ==============================================================================
-- 18. BẢNG NOTIFICATIONS (Thông báo hệ thống)
-- ==============================================================================
CREATE TABLE dbo.Notifications (
    Id INT IDENTITY(1,1) CONSTRAINT PK_Notifications PRIMARY KEY CLUSTERED,
    UserId INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    NotificationType NVARCHAR(50) NOT NULL, -- 'TourApproved', 'BookingConfirmed', 'CheckInSuccess', 'ReviewReceived'
    TargetUrl NVARCHAR(300) NULL,
    IsRead BIT NOT NULL CONSTRAINT DF_Notifications_IsRead DEFAULT (0),
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Notifications_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT FK_Notifications_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX IX_Notifications_UserId ON dbo.Notifications(UserId, IsRead);
GO

PRINT N'>>> HOÀN THÀNH TẠO 18 BẢNG, RÀNG BUỘC VÀ INDEX CHO LOCALEXPLOREDB <<<';
GO
