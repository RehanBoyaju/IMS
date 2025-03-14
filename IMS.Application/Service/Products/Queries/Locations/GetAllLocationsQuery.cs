using IMS.Application.DTO.Response.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Service.Products.Queries.Locations
{
    public class GetAllLocationsQuery : IRequest<IEnumerable<GetLocationResponseDTO>>
    {
    }
}
