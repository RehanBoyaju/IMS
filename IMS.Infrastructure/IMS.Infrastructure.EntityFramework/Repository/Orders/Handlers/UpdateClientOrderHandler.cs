using IMS.Application.DTO.Response;
using IMS.Application.Service.Orders.Commands;
using IMS.Infrastructure.EntityFramework.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.EntityFramework.Repository.Orders.Handlers
{
    public class UpdateClientOrderHandler(Data.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<UpdateClientOrderCommand, ServiceResponse>
    {
        public async Task<ServiceResponse> Handle(UpdateClientOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var data = await dbContext.Orders.Where(o => o.Id == request.OrderModel.OrderId).FirstOrDefaultAsync(cancellationToken);
                if (data == null)
                {
                    return new ServiceResponse(false, "Order not found");
                }
                data.OrderState = request.OrderModel.OrderState;
                data.DeliveringDate = request.OrderModel.DeliveringDate;

                await dbContext.SaveChangesAsync(cancellationToken);

                return new ServiceResponse(true, "Order updated successfully");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
