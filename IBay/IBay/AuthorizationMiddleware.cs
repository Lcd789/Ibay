namespace IBay;
using System.Security.Claims;
using DAL.Data;
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

    public bool IsProductOwner(HttpContext context, IIbayContext ibayContext)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var productIdRouteValue = context.Request.RouteValues["sellerId"]?.ToString();
        
        if (userId == null || productIdRouteValue == null)
        {
            return false;
        }

        var productId = int.Parse(productIdRouteValue);
        var product = ibayContext.GetProductById(productId);
        if (product == null)
        {
               return false;
        }
        return product.seller.user_id == int.Parse(userId);
    }


}
