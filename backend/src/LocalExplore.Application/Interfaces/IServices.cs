using LocalExplore.Application.Common.Models;
using LocalExplore.Application.DTOs;

namespace LocalExplore.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto request);
    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto request);
    Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task<ApiResponse<UserProfileDto>> GetProfileAsync(int userId);
    Task<ApiResponse<string>> RegisterHostKycAsync(int userId, HostRegisterKycDto request);
    Task<ApiResponse<List<HostProfileSummaryDto>>> GetAllHostsAsync(bool? onlyPending = null);
    Task<ApiResponse<bool>> ApproveHostAsync(int hostProfileId, int adminUserId);
    Task<ApiResponse<bool>> RejectHostAsync(int hostProfileId, string reason);
}

public interface ITourService
{
    Task<PagedResponse<TourSummaryDto>> GetToursAsync(TourFilterRequest filter);
    Task<ApiResponse<TourDetailDto>> GetTourBySlugAsync(string slug);
    Task<ApiResponse<TourDetailDto>> GetTourByIdAsync(int id);
    Task<ApiResponse<int>> CreateTourAsync(int hostUserId, CreateTourRequest request);
    Task<ApiResponse<bool>> UpdateTourAsync(int hostUserId, int tourId, CreateTourRequest request);
    Task<ApiResponse<int>> AddScheduleAsync(int hostUserId, int tourId, CreateScheduleRequest request);
    Task<ApiResponse<List<TourScheduleDto>>> GetTourSchedulesAsync(int tourId);

    // Admin & Host actions
    Task<ApiResponse<bool>> ApproveTourAsync(int tourId);
    Task<ApiResponse<bool>> RejectTourAsync(int tourId, string reason);
    Task<ApiResponse<List<TourSummaryDto>>> GetHostToursAsync(int hostUserId);
}

public interface IBookingService
{
    Task<ApiResponse<BookingDetailDto>> CreateBookingAsync(int customerId, CreateBookingRequest request);
    Task<ApiResponse<List<BookingDetailDto>>> GetMyBookingsAsync(int customerId);
    Task<ApiResponse<BookingDetailDto>> GetBookingByCodeAsync(string bookingCode, int requestingUserId, bool isAdmin);
    Task<ApiResponse<bool>> CancelBookingAsync(int customerId, int bookingId);
    Task<ApiResponse<List<BookingDetailDto>>> GetHostBookingsAsync(int hostUserId);
    Task<ApiResponse<bool>> SimulatePaymentSuccessAsync(int bookingId, string method);
}

public interface ICheckInService
{
    Task<ApiResponse<CheckInResultDto>> VerifyAndCheckInAsync(int hostUserId, CheckInRequest request);
}

public interface IReviewService
{
    Task<ApiResponse<ReviewDto>> CreateReviewAsync(int customerId, CreateReviewRequest request);
    Task<ApiResponse<bool>> ReplyReviewAsync(int hostUserId, int reviewId, string reply);
    Task<ApiResponse<List<ReviewDto>>> GetTourReviewsAsync(int tourId);
}

public interface ITokenService
{
    string GenerateAccessToken(Domain.Entities.User user, IEnumerable<string> roles, int? hostProfileId);
    string GenerateRefreshToken();
    System.Security.Claims.ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
