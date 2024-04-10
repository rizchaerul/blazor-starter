using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Server.Handlers;
using WebApp.Server.Middlewares;

namespace WebApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = SessionIdDefaults.AuthenticationScheme, Policy = SessionIdDefaults.AdminRolePolicy)]
[CustomMiddleware(true)]
public class AuthenticationController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("check-status")]
    public ActionResult Get()
    {
        var userId = _httpContextAccessor
            .HttpContext
            ?.User
            .Claims
            .FirstOrDefault(Q => Q.Type == ClaimTypes.NameIdentifier);

        return Ok(userId!.Value);
    }
}
