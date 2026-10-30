using LocalExplore.Application.Common.Models;
using LocalExplore.Application.DTOs;
using LocalExplore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalExplore.API.Controllers;

public class BookingsController : BaseApiController
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<BookingDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<BookingDetailDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        var result = await _bookingService.CreateBookingAsync(CurrentUserId, request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("my-bookings")]
    [ProducesResponseType(typeof(ApiResponse<List<BookingDetailDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyBookings()
    {
        var result = await _bookingService.GetMyBookingsAsync(CurrentUserId);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{bookingCode}")]
    [ProducesResponseType(typeof(ApiResponse<BookingDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<BookingDetailDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBookingByCode(string bookingCode)
    {
        var result = await _bookingService.GetBookingByCodeAsync(bookingCode, CurrentUserId, IsAdmin);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("{id:int}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var result = await _bookingService.CancelBookingAsync(CurrentUserId, id);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [Authorize(Roles = "Host")]
    [HttpGet("host/all")]
    [ProducesResponseType(typeof(ApiResponse<List<BookingDetailDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHostBookings()
    {
        var result = await _bookingService.GetHostBookingsAsync(CurrentUserId);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("{id:int}/simulate-payment")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SimulatePayment(int id, [FromQuery] string method = "Sandbox_VNPAY")
    {
        var result = await _bookingService.SimulatePaymentSuccessAsync(id, method);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}
