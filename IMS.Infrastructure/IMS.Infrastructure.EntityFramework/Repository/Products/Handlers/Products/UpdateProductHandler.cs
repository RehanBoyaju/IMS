using IMS.Application.DTO.Response;
using IMS.Application.Service.Products.Commands.Locations;
using IMS.Application.Service.Products.Commands.Products;
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

namespace IMS.Infrastructure.EntityFramework.Repository.Products.Handlers.Products
{
    public class UpdateProductHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<UpdateProductCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var product = await dbContext.Products.FirstOrDefaultAsync(c => c.Id.Equals(request.ProductModel.Id), cancellationToken: cancellationToken);
                if (product == null)
                {
                    return GeneralDbResponses.ItemNotFound(request.ProductModel.Name);
                }

                dbContext.Entry(product).State = EntityState.Detached;
                var adaptData = request.ProductModel.Adapt(new Product());
                dbContext.Products.Update(adaptData);
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemUpdate(request.ProductModel.Name);

            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
