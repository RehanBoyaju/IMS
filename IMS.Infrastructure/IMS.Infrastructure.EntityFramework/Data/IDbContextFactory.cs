using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.EntityFramework.Data
{
    public interface IDbContextFactory<T> where T: DbContext
    {
        T CreateDbContext();
    }
}
