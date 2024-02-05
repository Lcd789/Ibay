namespace IBay;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class AuthorizationMiddleware(RequestDelegate next, ILogger<AuthorizationMiddleware> logger)
{
    private bool IsAuthenticated(HttpContext context)
    {
        return context.User.Identity!.IsAuthenticated;
    }

    public static bool IsSelfTargetting(HttpContext context)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var routeId = context.Request.RouteValues["id"]?.ToString();

        return userId != null && routeId != null && userId == routeId;
    }
}
