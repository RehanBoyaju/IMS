using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.DTO.Request.Identity
{
    public static class Policy
    {
        public const string AdminPolicy = "AdminPolicy";
        public const string ManagerPolicy = "AdminPolicy";
        public const string UserPolicy = "AdminPolicy";

        public static class RoleClaim
        {
            public const string Admin = "Admin";
            public const string Manager = "Manager";
            public const string User = "User";
        }

    }
}
