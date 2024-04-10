using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp.Server.Handlers;
using WebApp.Server.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Add services to the container
services.AddControllers();
services.AddSwaggerGen();
services.AddHttpContextAccessor();

services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, SessionIdAuthenticationHandler>(SessionIdDefaults.AuthenticationScheme, null);

services
    .AddAuthorizationBuilder()
    .AddPolicy(
        SessionIdDefaults.AdminRolePolicy,
        policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim(ClaimTypes.Role, "ADMINISTRATOR");
        }
    );

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
