using IMS.Application.DTO.Request.Identity;
using IMS.Application.DTO.Response;
using IMS.Application.Extension.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

using IMS.Application.DTO.Response.Identity;

using IMS.Application.Interface.Identity;
using IMS.Application.Service;
using IMS.Application.DTO.Request.ActivityTracker;
using IMS.Application.DTO.Response.ActivityTracker;

namespace IMS.Application.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccount account;

        public AccountService(IAccount account)
        {
            this.account = account;
        }
        public async Task<ServiceResponse> RegisterAsync(RegisterUserRequestDTO model)
        {
            return await account.RegisterAsync(model);
        }
     
        public async Task<ServiceResponse> LoginAsync(LoginUserRequestDTO model)
        {
            return await account.LoginAsync(model);

        }
        public async Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync()
        {
           return await account.GetUsersWithClaimsAsync();
        }
        public async Task<ServiceResponse> UpdateUserAsync(ChangeUserClaimRequestDTO model)
        {
            return await account.UpdateUserAsync(model);

        }
        public async Task SetupAsync()
        {
           await account.SetupAsync();
        }



        public async Task SaveActivityAsync(ActivityTrackerRequestDTO model)
        {
            await account.SaveActivityAsync(model);
        }

        public async Task<IEnumerable<ActivityTrackerResponseDTO>> GetActivityAsync()
        {
           return await account.GetActivityAsync();
        }

        public async Task<IEnumerable<IGrouping<DateTime,ActivityTrackerResponseDTO>>> GroupActivityAsync()
        {
            var data = (await GetActivityAsync()).GroupBy(e => e.Date).AsEnumerable();
            return data;
        }
    }
}
