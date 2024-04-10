using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApp.Server.Handlers;

public static class SessionIdDefaults
{
    public const string AuthenticationScheme = "SessionId";
    public const string AdminRolePolicy = "AdminRolePolicy";
}

public class SessionIdAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public SessionIdAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder
    )
        : base(options, logger, encoder) { }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        await Task.CompletedTask;

        // Get the token from the Authorization header
        if (!Context.Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValues))
        {
            return AuthenticateResult.Fail("Authorization header not found.");
        }

        var sessionId = authorizationHeaderValues.FirstOrDefault();
        if (string.IsNullOrEmpty(sessionId))
        {
            return AuthenticateResult.Fail("SessionId not found in Authorization header.");
        }

        // Validate SessionId
        // TODO: check from db
        var validationResult = sessionId == "002CF82B-00A4-4EFE-B920-F0654BAE5CC3";

        // Return an authentication failure if the token is not valid
        if (!validationResult)
        {
            return AuthenticateResult.Fail("SessionId is not valid.");
        }

        // Set the authentication result with the claims from the API response
        var claims = new List<Claim>
        {
            // TODO: check from db
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "ADMINISTRATOR"),
        };
        var claimsIdentity = new ClaimsIdentity(claims, "SessionId");
        var principal = new ClaimsPrincipal(claimsIdentity);

        return AuthenticateResult.Success(new AuthenticationTicket(principal, SessionIdDefaults.AuthenticationScheme));
    }
}
