using System.Linq;
using System.Security.Claims;
using DotnetCustomAuth.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = SessionIdDefaults.AuthenticationScheme, Policy = SessionIdDefaults.AdminRolePolicy)]
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
        var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(Q => Q.Type == ClaimTypes.NameIdentifier);
        return Ok(userId!.Value);
    }
}
