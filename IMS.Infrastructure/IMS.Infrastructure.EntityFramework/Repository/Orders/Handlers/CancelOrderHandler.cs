using IMS.Application.DTO.Response;
using IMS.Application.Extension;
using IMS.Application.Service.Orders.Commands;
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
    public class CancelOrderHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<CancelOrderCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using(var dbContext = contextFactory.CreateDbContext()){
                    var order = await dbContext.Orders.Where(o => o.Id == request.OrderId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                    if (order == null)
                    {
                        return new ServiceResponse(false, "Order not found");
                    }
                    order.OrderState = OrderState.Canceled;
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                
                return new ServiceResponse(true, "Order canceled succesfully");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false,ex.Message);
            }
        }

    }
}
