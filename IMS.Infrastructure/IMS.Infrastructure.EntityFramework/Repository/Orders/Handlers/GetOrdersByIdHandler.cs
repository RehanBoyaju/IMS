using IMS.Application.DTO.Response.Orders;
using IMS.Application.Service.Orders.Queries;
using IMS.Infrastructure.EntityFramework.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.EntityFramework.Repository.Orders.Handlers
{
    public class GetOrdersByIdHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<GetOrderByIdQuery, IEnumerable<GetOrderResponseDTO>>
    {
        public async Task<IEnumerable<GetOrderResponseDTO>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = contextFactory.CreateDbContext(); 
            var orders = await dbContext.Orders.AsNoTracking().Where(o=> o.ClientId == request.UserId).ToListAsync(cancellationToken);
            var products = await dbContext.Products.AsNoTracking().ToListAsync(cancellationToken);

            return orders.Select(order => new GetOrderResponseDTO
            {
                Id = order.Id,
                ProductName = products.FirstOrDefault(p => p.Id == order.ProductId)!.Name,
                Price = order.Price,
                DeliveringDate = order.DeliveringDate,
                OrderedDate = order.DateOrdered,
                ProductId = order.ProductId,
                ProductImage = products.FirstOrDefault(p => p.Id == order.ProductId).Base64Image,
                Quantity = order.Quantity,
                SerialNumber = products.FirstOrDefault(p => p.Id == order.ProductId).SerialNumber,
                State = order.OrderState
            }).ToList();
        }
    }
}
