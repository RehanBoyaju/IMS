﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.EntityFramework.Data
{
    public class DbContextFactory<T> (IServiceProvider serviceProvider) : IDbContextFactory<T> where T : DbContext
    {
        public T CreateDbContext()
        {
            return serviceProvider.GetRequiredService<T>();
        }
    }
}
