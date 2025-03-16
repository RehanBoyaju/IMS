using IMS.Application.DTO.Response;
using IMS.Application.Service.Products.Commands.Categories;
using IMS.Application.Service.Products.Commands.Locations;
using IMS.Domain.Products;
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
    class UpdateLocationHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<UpdateLocationCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var location = await dbContext.Locations.FirstOrDefaultAsync(c => c.Id.Equals(request.LocationModel.Id), cancellationToken: cancellationToken);
                if (location == null)
                {
                    return GeneralDbResponses.ItemNotFound(request.LocationModel.Name);
                }

                dbContext.Entry(location).State = EntityState.Detached;
                var adaptData = request.LocationModel.Adapt(new Location());
                dbContext.Locations.Update(adaptData);
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemUpdate(request.LocationModel.Name);

            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
