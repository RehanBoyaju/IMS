using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.DTO.Request.Orders
{
    public class CreateOrderRequestDTO : OrderBaseDTO
    {
        public string ClientId { get; set; }

    }
}
