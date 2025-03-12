using IMS.Application.DTO.Request.Identity;
using IMS.Application.DTO.Response.Identity;
using IMS.Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Application.DTO.Request.ActivityTracker;
using IMS.Application.DTO.Response.ActivityTracker;

namespace IMS.Application.Service
{
    public interface IAccountService
    {
        Task<ServiceResponse> LoginAsync(LoginUserRequestDTO model);
        Task<ServiceResponse> RegisterAsync(RegisterUserRequestDTO model);
        Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync();
        Task SetupAsync();
        Task<ServiceResponse> UpdateUserAsync(ChangeUserClaimRequestDTO model);
        Task SaveActivityAsync(ActivityTrackerRequestDTO model);
        Task<IEnumerable<IGrouping<DateTime,ActivityTrackerResponseDTO>>> GroupActivityAsync();
    }
}
