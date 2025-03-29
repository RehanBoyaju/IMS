using Azure.Core;
using IMS.Application.DTO.Response;
using IMS.Application.Extension;
using IMS.Application.Service.Orders.Commands;
using IMS.Domain.Orders;
using IMS.Domain.Products;
using IMS.Infrastructure.EntityFramework.Data;
using IMS.Infrastructure.EntityFramework.Repository.Products.Handlers;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMS.Infrastructure.EntityFramework.Repository.Orders.Handlers
{
    public class CreateOrderHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<CreateOrderCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var product = await dbContext.Products.FirstOrDefaultAsync(o => o.Name == request.OrderModel.ProductName, cancellationToken: cancellationToken);
                if (product == null)
                {
                    return new ServiceResponse(false, "Product not found");
                }
                product.Quantity = product.Quantity - request.OrderModel.Quantity;
                await dbContext.SaveChangesAsync(cancellationToken);

              
                var data = request.OrderModel.Adapt<Order>();
                data.TotalAmount = product.Price * data.Quantity;
                data.OrderState = OrderState.Processing;
                data.Price = product.Price;
                data.DateOrdered = DateTime.Now;
                data.DeliveringDate = null;
                dbContext.Orders.Add(data);

                await dbContext.SaveChangesAsync(cancellationToken);

                return new ServiceResponse(true, "Order placed successfully");
            }
            catch(Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
        
        
    }
}
