using IMS.Application.DTO.Response;
using IMS.Application.Service.Products.Commands.Categories;
using IMS.Infrastructure.EntityFramework.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.EntityFramework.Repository.Products.Handlers.Categories
{
    public class DeleteCategoryHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<DeleteCategoryCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(DeleteCategoryCommand request,CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();

                var category = await dbContext.Categories.FirstOrDefaultAsync( c => c.Id.Equals(request.Id),cancellationToken :cancellationToken);
                if(category == null)
                {
                   return GeneralDbResponses.ItemNotFound("Category");
                }
                dbContext.Categories.Remove(category);
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemDelete(category.Name);
            }
            catch(Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
