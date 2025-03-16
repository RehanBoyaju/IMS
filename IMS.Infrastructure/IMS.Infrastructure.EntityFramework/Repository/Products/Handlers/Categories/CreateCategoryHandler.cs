using IMS.Application.DTO.Response;
using IMS.Application.Service.Products.Commands.Categories;
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

namespace IMS.Infrastructure.EntityFramework.Repository.Products.Handlers.Categories
{
    public class CreateCategoryHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<CreateCategoryCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle (CreateCategoryCommand request,CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var category = await dbContext.Categories.FirstOrDefaultAsync( c=> c.Name.ToLower().Equals(request.CategoryModel.Name.ToLower()),cancellationToken : cancellationToken);
                if (category != null)
                {
                    return GeneralDbResponses.ItemAlreadyExist(request.CategoryModel.Name);
                }
                var data = request.CategoryModel.Adapt(new Category());
                dbContext.Categories.Add(data);
                await dbContext.SaveChangesAsync(cancellationToken);

                return GeneralDbResponses.ItemCreated(request.CategoryModel.Name);
            }
            catch(Exception ex)
            {
                return new ServiceResponse(true,ex.Message);    
            }
        }
    }
}
