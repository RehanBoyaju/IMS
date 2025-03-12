using IMS.Application.PluginInterfaces;
using IMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.InMemory
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly List<Inventory> _inventories;
        public InventoryRepository()
        {
            _inventories =
            [
                new() { InventoryId = 1, InventoryName = "Bike" , Quantity = 10, Price = 2},
                new() { InventoryId = 2, InventoryName = "Car" , Quantity = 15, Price = 15},
                new() { InventoryId = 3, InventoryName = "Bicycle" , Quantity = 20, Price = 8},

            ];

        }
        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return await Task.FromResult(_inventories);
            }
            return await Task.FromResult(_inventories.Where(i => i.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
