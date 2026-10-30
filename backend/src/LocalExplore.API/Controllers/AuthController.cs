using LocalExplore.Application.Common.Models;
using LocalExplore.Application.DTOs;
using LocalExplore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalExplore.API.Controllers;

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        var result = await _authService.RegisterAsync(request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var result = await _authService.LoginAsync(request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var result = await _authService.GetProfileAsync(CurrentUserId);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("host-kyc")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterHostKyc([FromBody] HostRegisterKycDto request)
    {
        var result = await _authService.RegisterHostKycAsync(CurrentUserId, request);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/hosts")]
    [ProducesResponseType(typeof(ApiResponse<List<HostProfileSummaryDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllHosts([FromQuery] bool? onlyPending)
    {
        var result = await _authService.GetAllHostsAsync(onlyPending);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("admin/hosts/{id:int}/approve")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ApproveHost(int id)
    {
        var result = await _authService.ApproveHostAsync(id, CurrentUserId);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("admin/hosts/{id:int}/reject")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RejectHost(int id, [FromBody] RejectRequestDto request)
    {
        var result = await _authService.RejectHostAsync(id, request.Reason);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}
