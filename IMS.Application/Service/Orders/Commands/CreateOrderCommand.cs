using IMS.Application.DTO.Request.Orders;
using IMS.Application.DTO.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Service.Orders.Commands
{
    public record CreateOrderCommand(CreateOrderRequestDTO OrderModel) : IRequest<ServiceResponse>;

}
