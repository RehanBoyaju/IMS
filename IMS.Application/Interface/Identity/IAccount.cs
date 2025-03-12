using IMS.Application.DTO.Request.ActivityTracker;
using IMS.Application.DTO.Request.Identity;
using IMS.Application.DTO.Response;
using IMS.Application.DTO.Response.ActivityTracker;
using IMS.Application.DTO.Response.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Interface.Identity
{
    public interface IAccount
    {
        Task<ServiceResponse> LoginAsync(LoginUserRequestDTO model);
        Task<ServiceResponse> RegisterAsync(RegisterUserRequestDTO model);
        Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync();
        Task SetupAsync();
        Task<ServiceResponse> UpdateUserAsync(ChangeUserClaimRequestDTO model);
        Task SaveActivityAsync(ActivityTrackerRequestDTO model);
        Task<IEnumerable<ActivityTrackerResponseDTO>> GetActivityAsync();

    }
}
