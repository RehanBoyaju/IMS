using IMS.Infrastructure.EntityFramework.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using IMS.Application.Extension.Identity;
using System.Runtime.CompilerServices;
using IMS.Application.Interface.Identity;
using IMS.Infrastructure.EntityFramework.Repository;

namespace IMS.Infrastructure.EntityFramework.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services,IConfiguration config)
        {
            services.AddDbContext<AppDbContext>( o=> o.UseSqlServer(config.GetConnectionString("Database")), ServiceLifetime.Scoped);
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddIdentityCookies();
            services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddAuthorizationBuilder()
                .AddPolicy("AdministrationPolicy", adp =>
                {
                    adp.RequireAuthenticatedUser();
                    adp.RequireRole("Admin", "Manager");
                }).
                AddPolicy("UserPolicy", adp =>
                {
                    adp.RequireAuthenticatedUser();
                    adp.RequireRole("User");
                });
            services.AddCascadingAuthenticationState();
            services.AddScoped<IAccount, Account>();
            
            return services;
        }
    }
}
