using Microsoft.AspNetCore.Http;
using System.Linq;

namespace NSE.WebAPI.Core.Identity.Claims
{
    public class CustomAuthorization
    {
        public static bool ValidateClaimsUser(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity.IsAuthenticated &&
                context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
        }
    }
}
