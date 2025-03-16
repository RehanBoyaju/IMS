using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.DTO.Response.Orders
{
    public class GetOrderedProductWithQuantityResponseDTO
    {
        public string ProductName { get; set; }
        public int QuanitityOrdered { get; set; }
    }
}
