using LocalExplore.Application.Common.Models;
using LocalExplore.Application.DTOs;
using LocalExplore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalExplore.API.Controllers;

public class CheckInController : BaseApiController
{
    private readonly ICheckInService _checkInService;

    public CheckInController(ICheckInService checkInService)
    {
        _checkInService = checkInService;
    }

    [Authorize(Roles = "Host,Admin")]
    [HttpPost("verify")]
    [ProducesResponseType(typeof(ApiResponse<CheckInResultDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CheckInResultDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyAndCheckIn([FromBody] CheckInRequest request)
    {
        var result = await _checkInService.VerifyAndCheckInAsync(CurrentUserId, request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}

public class ReviewsController : BaseApiController
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("tour/{tourId:int}")]
    [ProducesResponseType(typeof(ApiResponse<List<ReviewDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTourReviews(int tourId)
    {
        var result = await _reviewService.GetTourReviewsAsync(tourId);
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ReviewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ReviewDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request)
    {
        var result = await _reviewService.CreateReviewAsync(CurrentUserId, request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [Authorize(Roles = "Host")]
    [HttpPost("{reviewId:int}/reply")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReplyReview(int reviewId, [FromBody] string reply)
    {
        var result = await _reviewService.ReplyReviewAsync(CurrentUserId, reviewId, reply);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}
