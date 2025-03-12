using IMS.Application.DTO.Request.Identity;
using IMS.Application.DTO.Response;
using IMS.Application.Extension.Identity;
using IMS.Infrastructure.EntityFramework.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using IMS.Application.DTO.Response.Identity;
using Microsoft.EntityFrameworkCore;
using Mapster;
using IMS.Application.Interface.Identity;
using IMS.Application.DTO.Request.ActivityTracker;
using IMS.Application.DTO.Response.ActivityTracker;
using IMS.Domain.ActivityTracker;

namespace IMS.Infrastructure.EntityFramework.Repository
{
    public class Account(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,AppDbContext context) : IAccount
    {
        public async Task<ServiceResponse> RegisterAsync(RegisterUserRequestDTO model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if(user is not null)
            {
                return new ServiceResponse(false, "User already exists");
            }
            var newUser = new ApplicationUser
            {
                UserName = model.Email,
                PasswordHash = model.Password,
                Email = model.Email,
                Name = model.Name,
            };
            var result = CheckResult(await userManager.CreateAsync(newUser, model.Password));
            if (!result.Flag)
            {
                return result;
            }
            return await CreateUserClaims(model);
        }
        public async Task<ServiceResponse> CreateUserClaims(RegisterUserRequestDTO model)
        {
            if (string.IsNullOrEmpty(model.Policy)) return new ServiceResponse(false,"No policy");

            Claim[] userClaims = [];

            if(model.Policy.Equals(Policy.AdminPolicy , StringComparison.OrdinalIgnoreCase))
            {
                userClaims= [
                new Claim(ClaimTypes.Email,model.Email),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim("Name",model.Name),
                new Claim("Create","true"),
                new Claim("Update","true"),
                new Claim("Delete","true"),
                new Claim("Read","true"),
                new Claim("ManageUser","true")

                ];
            }
            else if (model.Policy.Equals(Policy.ManagerPolicy, StringComparison.OrdinalIgnoreCase))
            {
                userClaims = [
                new Claim(ClaimTypes.Email,model.Email),
                new Claim(ClaimTypes.Role,"Manager"),
                new Claim("Name",model.Name),
                new Claim("Create","true"),
                new Claim("Read","true"),
                new Claim("Update","true"),
                new Claim("Delete","false"),
                new Claim("ManageUser","false")

                ];
            }
            else if(model.Policy.Equals(Policy.UserPolicy, StringComparison.OrdinalIgnoreCase)){
                userClaims = [
                new Claim(ClaimTypes.Email,model.Email),
                new Claim(ClaimTypes.Role,"Manager"),
                new Claim("Name",model.Name),
                new Claim("Create","false"),
                new Claim("Read","false"),
                new Claim("Update","false"),
                new Claim("Delete","false"),
                new Claim("ManageUser","false")

                ];
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if(user== null)
            {
                return new ServiceResponse(false, "User doesn't exist");
            }
            var result = CheckResult(await userManager.AddClaimsAsync(user,userClaims));

            if (result.Flag)
            {
                return new ServiceResponse(true, "User has been registered");
            }
            return result;

        }

        public async Task<ServiceResponse> LoginAsync(LoginUserRequestDTO model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if(user == null)
            {
                return new ServiceResponse(false, "User not found");
            }
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password,false);

            if (!result.Succeeded)
            {
                return new ServiceResponse(false, "Incorrect Password");
            }
            var signin = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (!signin.Succeeded){
                return new ServiceResponse(false, "Error while logging in");
            }

            return new ServiceResponse(true,null);

        }
        public async Task<IEnumerable<GetUserWithClaimResponseDTO>> GetUsersWithClaimsAsync()
        {
            var UserList = new List<GetUserWithClaimResponseDTO>();
            var allUsers = await userManager.Users.ToListAsync();
            if(allUsers.Count == 0)
            {
                return UserList;
            }
            foreach(var user in allUsers)
            {
                var currentUser = await userManager.FindByIdAsync(user.Id);
                if(currentUser == null)
                {
                    throw new ArgumentNullException("You are not logged in");
                }
                var getCurrentUserClaims = await userManager.GetClaimsAsync(currentUser);
                if (getCurrentUserClaims.Any())
                {
                    UserList.Add(new GetUserWithClaimResponseDTO
                    {
                        UserId = user.Id,
                        Email = getCurrentUserClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value,
                        RoleName = getCurrentUserClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value,
                        Name = getCurrentUserClaims.FirstOrDefault(c => c.Type == "Name")!.Value,
                        ManageUser = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(c=>c.Type == "ManageUser")!.Value),
                        Create = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault( c=>c.Type == "Create")!.Value),
                        Update = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(c => c.Type == "Update")!.Value),
                        Read = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(c => c.Type == "Read")!.Value),
                        Delete = Convert.ToBoolean(getCurrentUserClaims.FirstOrDefault(c => c.Type == "Delete")!.Value),

                    });
                }
            }
            return UserList;
        }
        public async Task<ServiceResponse> UpdateUserAsync(ChangeUserClaimRequestDTO model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if(user is null)
            {
                return new ServiceResponse(false, "User not found");

            }
            var oldUserClaims = await userManager.GetClaimsAsync(user);
            var result = await userManager.RemoveClaimsAsync(user, oldUserClaims);
            var response = CheckResult(result);
            if (!response.Flag)
            {
                return response;
            }
            Claim[] newUserClaims = [
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, model.RoleName),
                new Claim("Name",model.Name),
                new Claim("ManageUser",model.ManageUser.ToString()),
                new Claim("Create",model.Create.ToString()),
                new Claim("Update",model.Update.ToString()),
                new Claim("Read",model.Read.ToString()),
                new Claim("Delete",model.Delete.ToString()),

                ];

            var addNewClaims = await userManager.AddClaimsAsync(user, newUserClaims);
            var claimsResponse = CheckResult(addNewClaims);
            if (claimsResponse.Flag)
            {
                return new ServiceResponse(true, "User updated");
            }
            return claimsResponse;

        }
        public async Task SetupAsync()
        {
            await RegisterAsync(new RegisterUserRequestDTO
            {
                Name = "Administrator",
                Email = "admin@admin.com",
                Password = "Admin@123",
                Policy = Policy.AdminPolicy
            });
        }

        private static ServiceResponse CheckResult(IdentityResult result)
        {
            if (result.Succeeded)
            {
                return new ServiceResponse(true, null);
            }
            var errors = result.Errors.Select(e => e.Description);
            return new ServiceResponse(false, string.Join(Environment.NewLine,errors));
        }


        public async Task SaveActivityAsync(ActivityTrackerRequestDTO model)
        {
            context.ActivityTracker.Add(model.Adapt(new Tracker()));
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ActivityTrackerResponseDTO>> GetActivityAsync()
        {
            var list = new List<ActivityTrackerResponseDTO>();
            var data = (await context.ActivityTracker.ToListAsync()).Adapt<List<ActivityTrackerResponseDTO>>();
            foreach (var activity in data)
            {
                activity.UserName = (await userManager.FindByIdAsync(activity.UserId))!.Name;
                list.Add(activity);
            }

            return data;

        }

    }
}
