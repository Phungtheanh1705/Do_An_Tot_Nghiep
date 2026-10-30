using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace LocalExplore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected int CurrentUserId
    {
        get
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out var id) ? id : 0;
        }
    }

    protected string CurrentUserEmail => User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

    protected int? CurrentHostProfileId
    {
        get
        {
            var claim = User.FindFirst("HostProfileId")?.Value;
            return int.TryParse(claim, out var id) ? id : null;
        }
    }

    protected bool IsAdmin => User.IsInRole("Admin");
}
