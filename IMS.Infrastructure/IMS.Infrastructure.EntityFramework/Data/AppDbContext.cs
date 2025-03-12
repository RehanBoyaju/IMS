using IMS.Application.Extension.Identity;
using IMS.Domain.ActivityTracker;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.EntityFramework.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Tracker> ActivityTracker { get; set; }

        
    }
}
