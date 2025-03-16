using IMS.Application.DTO.Response.Orders;
using IMS.Application.Service.Orders.Queries;
using IMS.Domain.Orders;
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
    public class GetOrderedProductsWithQuantityHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<GetOrderedProductsWithQuantityQuery, IEnumerable<GetOrderedProductWithQuantityResponseDTO>>
    {
        public async Task<IEnumerable<GetOrderedProductWithQuantityResponseDTO>> Handle(GetOrderedProductsWithQuantityQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = contextFactory.CreateDbContext();
            var orders = new List<Order>();
            var data = new List<GetOrderedProductWithQuantityResponseDTO>();
            if (!string.IsNullOrEmpty(request.UserId))
            {
                orders = await dbContext.Orders.AsNoTracking().Where(o => o.ClientId == request.UserId).ToListAsync(cancellationToken);
            }
            else
            {
                orders = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken);
            }

            var selectOrdersWithin12Months = orders.Where(order => order.DateOrdered >= DateTime.Today.AddMonths(-12))
                                                                               .GroupBy(order => new { Name = order.ProductId}).ToList();

            foreach(var order in selectOrdersWithin12Months)
            {
                data.Add(new GetOrderedProductWithQuantityResponseDTO()
                {
                    ProductName = (await dbContext.Products.FirstOrDefaultAsync( p => p.Id == order.Key.Name ,cancellationToken : cancellationToken))!.Name,
                    QuanitityOrdered = order.Sum(p => p.Quantity)

                });
            }
            return data;
        }
    }
}
