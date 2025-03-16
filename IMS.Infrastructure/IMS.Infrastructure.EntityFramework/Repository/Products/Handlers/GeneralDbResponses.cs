using IMS.Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.EntityFramework.Repository.Products.Handlers
{
    public static class GeneralDbResponses
    {
        public static ServiceResponse ItemAlreadyExist (string itemName) => new ServiceResponse(false, $"{itemName} already exists");
        public static ServiceResponse ItemNotFound(string itemName) => new ServiceResponse(false, $"{itemName} not found");
        public static ServiceResponse ItemCreated(string itemName) => new ServiceResponse(true, $"{itemName} created successfully");
        public static ServiceResponse ItemUpdate(string itemName) => new ServiceResponse(true, $"{itemName} updated successfully");
        public static ServiceResponse ItemDelete(string itemName) => new ServiceResponse(true, $"{itemName} deleted successfully");

    }
}
