using IMS.Application.DTO.Response;
using IMS.Application.Service.Orders.Commands;
using IMS.Infrastructure.EntityFramework.Data;
using IMS.Infrastructure.EntityFramework.Repository.Products.Handlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.EntityFramework.Repository.Orders.Handlers
{
    public class DeleteOrderHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<DeleteOrderCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();

                var order = await dbContext.Orders.FirstOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken: cancellationToken);
                if (order == null)
                {
                    return GeneralDbResponses.ItemNotFound("Order");
                }

                var product = await dbContext.Products.FirstOrDefaultAsync(c => c.Id.Equals(order.ProductId), cancellationToken: cancellationToken);
                if (product == null)
                {
                    return new ServiceResponse(false, "Product not found in order");
                }

                product.Quantity += order.Quantity;

                dbContext.Orders.Remove(order);
                await dbContext.SaveChangesAsync(cancellationToken);
                return new ServiceResponse(true, "Order deleted successfully");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
