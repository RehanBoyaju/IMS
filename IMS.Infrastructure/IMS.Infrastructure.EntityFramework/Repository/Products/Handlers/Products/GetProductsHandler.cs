using IMS.Application.DTO.Response.Products;
using IMS.Application.Service.Products.Queries.Products;
using IMS.Infrastructure.EntityFramework.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.EntityFramework.Repository.Products.Handlers.Products
{
    public class GetProductsHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<GetProductQuery, IEnumerable<GetProductResponseDTO>>
    {
        public async Task<IEnumerable<GetProductResponseDTO>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = contextFactory.CreateDbContext();
            var data = await dbContext.Products.AsNoTracking().Include(c => c.Category).Include( l=> l.Location).ToListAsync( cancellationToken : cancellationToken);
            return data.Select(product => new GetProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Base64Image = product.Base64Image,
                CategoryId = product.CategoryId,
                LocationId = product.LocationId,
                Price = product.Price,
                DateAdded = product.DateAdded,
                Quantity = product.Quantity,
                SerialNumber = product.SerialNumber,
                Location = new GetLocationResponseDTO
                {
                    Id = product.LocationId,
                    Name = product.Location.Name
                },
                Category = new GetCategoryResponseDTO
                {
                    Id = product.CategoryId,
                    Name = product.Category.Name
                }
            }).ToList();
        }
    }
}
