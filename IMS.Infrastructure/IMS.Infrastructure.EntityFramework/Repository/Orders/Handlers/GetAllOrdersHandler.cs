using IMS.Application.DTO.Request.Orders;
using IMS.Application.DTO.Response.Orders;
using IMS.Application.Extension.Identity;
using IMS.Application.Service.Orders.Queries;
using IMS.Domain.Orders;
using IMS.Infrastructure.EntityFramework.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.EntityFramework.Repository.Orders.Handlers
{
    public class GetAllOrdersHandler(Data.IDbContextFactory<AppDbContext> contextFactory , UserManager<ApplicationUser> userManager) : IRequestHandler<GetAllOrderQuery, IEnumerable<GetOrderResponseDTO>>
    {
        public async  Task<IEnumerable<GetOrderResponseDTO>> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = contextFactory.CreateDbContext();
            var orders = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken:cancellationToken);
            var products = await dbContext.Products.AsNoTracking().ToListAsync(cancellationToken : cancellationToken);
            var users = await userManager.Users.ToListAsync(cancellationToken:cancellationToken);
            return orders.Select(order => new GetOrderResponseDTO
            {
                Id = order.Id,
                ProductName = products.FirstOrDefault(p => p.Id == order.ProductId).Name,
                Price = order.Price,
                DeliveringDate = order.DeliveringDate,
                OrderedDate = order.DateOrdered,
                ProductId = order.ProductId,
                ProductImage = products.FirstOrDefault( p => p.Id == order.ProductId).Base64Image,
                Quantity = order.Quantity,
                SerialNumber = products.FirstOrDefault(p => p.Id == order.ProductId).SerialNumber,
                State = order.OrderState,
                ClientId = order.ClientId,
                ClientName = users.FirstOrDefault(u => u.Id.Equals(order.ClientId)).Name
            }).ToList();
        }
    }
}
