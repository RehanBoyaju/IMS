using IMS.Application.DTO.Response.Orders;
using IMS.Application.Service.Orders.Queries;
using IMS.Domain.Orders;
using IMS.Infrastructure.EntityFramework.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.EntityFramework.Repository.Orders.Handlers
{
    public class GetProductOrderedByMonthsHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<GetProductOrderByMonthsQuery, IEnumerable<GetProductOrderByMonthsResponseDTO>>
    {
        public async Task<IEnumerable<GetProductOrderByMonthsResponseDTO>> Handle(GetProductOrderByMonthsQuery request, CancellationToken cancellationToken)
        {

            using var dbContext = contextFactory.CreateDbContext();
            var orders = new List<Order>();
            var data = new List<GetProductOrderByMonthsResponseDTO>();
            if (!string.IsNullOrEmpty(request.UserId))
            {
                orders = await dbContext.Orders.AsNoTracking().Where(o => o.ClientId == request.UserId).ToListAsync(cancellationToken);
            }
            else
            {
                orders = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken);

            }

            var selectOrdersWithin12Months = orders.Where(order => order.DateOrdered >= DateTime.Today.AddMonths(-12))
                                                                               .GroupBy(order => new { Month = order.DateOrdered.Month }).ToList();

            foreach (var order in selectOrdersWithin12Months.OrderBy(o => o.Key.Month))
            {
                data.Add(new GetProductOrderByMonthsResponseDTO()
                {
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(order.Key.Month),
                    TotalAmount = order.Sum( x => x.TotalAmount),
                });
            }
            return data;
        }
    }
}
