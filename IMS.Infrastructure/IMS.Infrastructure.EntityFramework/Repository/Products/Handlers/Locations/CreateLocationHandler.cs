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
    public class CreateLocationHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<CreateLocationCommand, ServiceResponse>

    {
        public async Task<ServiceResponse> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var location = await dbContext.Locations.FirstOrDefaultAsync(c => c.Name.ToLower().Equals(request.LocationModel.Name.ToLower()), cancellationToken: cancellationToken);
                if (location != null)
                {
                    return GeneralDbResponses.ItemAlreadyExist(request.LocationModel.Name);
                }
                var data = request.LocationModel.Adapt(new Location());
                dbContext.Locations.Add(data);
                await dbContext.SaveChangesAsync(cancellationToken);

                return GeneralDbResponses.ItemCreated(request.LocationModel.Name);
            }
            catch (Exception ex)
            {
                return new ServiceResponse(true, ex.Message);
            }
        }
    }
}
