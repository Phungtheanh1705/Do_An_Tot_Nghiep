using LocalExplore.Application.Common.Models;
using LocalExplore.Application.DTOs;
using LocalExplore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalExplore.API.Controllers;

public class ToursController : BaseApiController
{
    private readonly ITourService _tourService;

    public ToursController(ITourService tourService)
    {
        _tourService = tourService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<TourSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTours([FromQuery] TourFilterRequest filter)
    {
        var result = await _tourService.GetToursAsync(filter);
        return Ok(result);
    }

    [HttpGet("{slug}")]
    [ProducesResponseType(typeof(ApiResponse<TourDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TourDetailDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTourBySlug(string slug)
    {
        var result = await _tourService.GetTourBySlugAsync(slug);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpGet("by-id/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<TourDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TourDetailDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTourById(int id)
    {
        var result = await _tourService.GetTourByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpGet("{id:int}/schedules")]
    [ProducesResponseType(typeof(ApiResponse<List<TourScheduleDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTourSchedules(int id)
    {
        var result = await _tourService.GetTourSchedulesAsync(id);
        return Ok(result);
    }

    // HOST ACTIONS
    [Authorize(Roles = "Host")]
    [HttpPost("host")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateTour([FromBody] CreateTourRequest request)
    {
        var result = await _tourService.CreateTourAsync(CurrentUserId, request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [Authorize(Roles = "Host")]
    [HttpPut("host/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateTour(int id, [FromBody] CreateTourRequest request)
    {
        var result = await _tourService.UpdateTourAsync(CurrentUserId, id, request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [Authorize(Roles = "Host")]
    [HttpPost("host/{id:int}/schedules")]
    [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddSchedule(int id, [FromBody] CreateScheduleRequest request)
    {
        var result = await _tourService.AddScheduleAsync(CurrentUserId, id, request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [Authorize(Roles = "Host")]
    [HttpGet("host/my-tours")]
    [ProducesResponseType(typeof(ApiResponse<List<TourSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyHostTours()
    {
        var result = await _tourService.GetHostToursAsync(CurrentUserId);
        return Ok(result);
    }

    // ADMIN ACTIONS
    [Authorize(Roles = "Admin")]
    [HttpPost("admin/{id:int}/approve")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ApproveTour(int id)
    {
        var result = await _tourService.ApproveTourAsync(id);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("admin/{id:int}/reject")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RejectTour(int id, [FromBody] string reason)
    {
        var result = await _tourService.RejectTourAsync(id, reason);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}
