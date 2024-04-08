using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace WebApp.Server.Middlewares;

public class CustomMiddleware
{
    private readonly RequestDelegate _next;

    public CustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        var attribute = endpoint?.Metadata.GetMetadata<CustomMiddlewareAttribute>();
        if (attribute?.Enable == true)
        {
            context.Request.Headers["X-CustomerHeader"] = "Hello, World!";
        }

        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }
}

public static class CustomMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomMiddleware>();
    }
}

public class CustomMiddlewareAttribute : Attribute
{
    public bool Enable { get; set; }

    public CustomMiddlewareAttribute(bool enable)
    {
        Enable = enable;
    }
}
