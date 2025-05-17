using System.Security.Claims;

namespace BookReview.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static long GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return long.TryParse(id, out var userId) ? userId : 0;
    }
}
