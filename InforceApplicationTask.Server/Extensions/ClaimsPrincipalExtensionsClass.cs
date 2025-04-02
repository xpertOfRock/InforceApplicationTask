using System.Security.Claims;

namespace InforceApplicationTask.Server.Extensions
{
    internal static class ClaimsPrincipalExtensions
    {
        internal static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
