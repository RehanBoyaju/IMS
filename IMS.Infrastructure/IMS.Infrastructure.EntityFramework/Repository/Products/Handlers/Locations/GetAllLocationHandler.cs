using IMS.Application.DTO.Response.Products;
using IMS.Application.Service.Products.Queries.Categories;
using IMS.Application.Service.Products.Queries.Locations;
using IMS.Infrastructure.EntityFramework.Data;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.EntityFramework.Repository.Products.Handlers.Locations
{
    public class GetAllLocationHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<GetAllLocationsQuery, IEnumerable<GetLocationResponseDTO>>
    {
        public async Task<IEnumerable<GetLocationResponseDTO>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = contextFactory.CreateDbContext();
            var data = await dbContext.Locations.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
            return data.Adapt<List<GetLocationResponseDTO>>();
        }
    }
}
