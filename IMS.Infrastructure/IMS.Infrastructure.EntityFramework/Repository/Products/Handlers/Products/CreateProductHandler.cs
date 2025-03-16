using IMS.Application.DTO.Response;
using IMS.Application.Service.Products.Commands.Categories;
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
    public class CreateProductHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<CreateProductCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var product = await dbContext.Products.FirstOrDefaultAsync(c => c.Name.ToLower().Equals(request.ProductModel.Name.ToLower()), cancellationToken: cancellationToken);
                if (product != null)
                {
                    return GeneralDbResponses.ItemAlreadyExist(request.ProductModel.Name);
                }
                var data = request.ProductModel.Adapt(new Product());
                dbContext.Products.Add(data);
                await dbContext.SaveChangesAsync(cancellationToken);

                return GeneralDbResponses.ItemCreated(request.ProductModel.Name);
            }
            catch (Exception ex)
            {
                return new ServiceResponse(true, ex.Message);
            }
        }
    }
}
