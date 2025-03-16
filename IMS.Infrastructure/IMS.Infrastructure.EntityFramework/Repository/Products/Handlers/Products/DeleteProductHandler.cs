using IMS.Application.DTO.Response;
using IMS.Application.Service.Products.Commands.Locations;
using IMS.Application.Service.Products.Commands.Products;
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
    public class DeleteProductHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<DeleteProductCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();

                var product = await dbContext.Products.FirstOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken: cancellationToken);
                if (product == null)
                {
                    return GeneralDbResponses.ItemNotFound("Product");
                }
                dbContext.Products.Remove(product);
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemDelete(product.Name);
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
