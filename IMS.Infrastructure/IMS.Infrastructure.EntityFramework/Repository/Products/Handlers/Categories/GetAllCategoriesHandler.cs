using IMS.Application.DTO.Response;
using IMS.Application.DTO.Response.Products;
using IMS.Application.Service.Products.Commands.Categories;
using IMS.Application.Service.Products.Queries.Categories;
using IMS.Infrastructure.EntityFramework.Data;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.EntityFramework.Repository.Products.Handlers.Categories
{
    public class GetAllCategoriesHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<GetAllCategoriesQuery,IEnumerable<GetCategoryResponseDTO>>
    {
        public async Task<IEnumerable<GetCategoryResponseDTO>> Handle(GetAllCategoriesQuery request,CancellationToken cancellationToken)
        {
            using var dbContext = contextFactory.CreateDbContext();
            var data  = await dbContext.Categories.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
            return data.Adapt<List<GetCategoryResponseDTO>>();
        }
    }
}
