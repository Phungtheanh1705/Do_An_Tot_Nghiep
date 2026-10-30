using LocalExplore.Application.Common.Models;
using LocalExplore.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocalExplore.API.Controllers;

[Route("api/master-data")]
[Route("api/masterdata")]
public class MasterDataController : BaseApiController
{
    private readonly ApplicationDbContext _context;

    public MasterDataController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Slug,
                c.Description,
                c.IconUrl,
                c.ImageUrl
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.SuccessResult(categories));
    }

    [HttpGet("locations")]
    public async Task<IActionResult> GetLocations()
    {
        var locations = await _context.Locations
            .OrderBy(l => l.DisplayOrder)
            .Select(l => new
            {
                l.Id,
                l.Name,
                l.Slug,
                l.ProvinceName,
                l.Description,
                l.ImageUrl,
                l.Latitude,
                l.Longitude,
                l.IsPopular
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.SuccessResult(locations));
    }

    [HttpGet("cancellation-policies")]
    public async Task<IActionResult> GetCancellationPolicies()
    {
        var policies = await _context.CancellationPolicies
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.RefundPercentageBefore72h,
                p.RefundPercentageBetween24And72h,
                p.RefundPercentageUnder24h,
                p.IsDefault
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.SuccessResult(policies));
    }
}
