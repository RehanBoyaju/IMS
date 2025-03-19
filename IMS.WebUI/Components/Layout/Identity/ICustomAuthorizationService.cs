using System.Security.Claims;

namespace IMS.WebUI.Components.Layout.Identity
{
    public interface ICustomAuthorizationService
    {
        bool CustomClaimChecker(ClaimsPrincipal user, string specifiedClaim);
    }
}
