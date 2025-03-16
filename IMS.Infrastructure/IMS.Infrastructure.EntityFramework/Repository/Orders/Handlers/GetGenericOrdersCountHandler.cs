using IMS.Application.DTO.Response.Orders;
using IMS.Application.Extension;
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
    public class GetGenericOrdersCountHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<GetGenericOrdersCountQuery, GetOrdersCountResponseDTO>
    {
        public async Task<GetOrdersCountResponseDTO> Handle(GetGenericOrdersCountQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = contextFactory.CreateDbContext();
            var list = new List<Order>();
            if (!request.IsAdmin)
            {
                list = await dbContext.Orders.AsNoTracking().Where( o=> o.ClientId == request.UserId).ToListAsync(cancellationToken);
            }
            else
            {
                list = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken);
            }
            int ProcessingCount = list.Count(o => o.OrderState == OrderState.Processing);
            int DeliveringCount = list.Count(o => o.OrderState == OrderState.Delivering);
            int DeliveredCount = list.Count(o => o.OrderState == OrderState.Delivered);
            int CanceledCount = list.Count(o => o.OrderState == OrderState.Canceled);

            return new GetOrdersCountResponseDTO
            (
                ProcessingCount, DeliveringCount, DeliveredCount, CanceledCount
            );

        }
    }
}
