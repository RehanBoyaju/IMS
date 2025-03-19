using System.Security.Claims;

namespace IMS.WebUI.Components.Layout.Identity
{
    public class CustomAuthorizationService : ICustomAuthorizationService
    {
        public bool CustomClaimChecker(ClaimsPrincipal user, string specifiedClaim)
        {
            if (!user.Identity!.IsAuthenticated)
            {
                return false;
            }
            var getClaim = user.HasClaim( u=> u.Type == specifiedClaim );

            if (!getClaim) return false;

            var getState = user.Claims.FirstOrDefault(u => u.Type == specifiedClaim)!.Value;

            return Convert.ToBoolean(getState) is true;
        }
    }
}
