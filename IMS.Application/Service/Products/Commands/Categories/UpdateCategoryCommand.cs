using IMS.Application.DTO.Request.Products;
using IMS.Application.DTO.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Application.Service.Products.Commands.Categories
{
    public record UpdateCategoryCommand(UpdateCategoryRequestDTO CategoryModel) : IRequest<ServiceResponse>
    {
    }
}
