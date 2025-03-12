using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Application;
using IMS.Application.Inventories.Interfaces;
using IMS.Domain;

namespace IMS.Application.Inventories
{
    public class ViewInventoriesByNameUseCase(IInventoryRepository inventoryRepository) : IViewInventoriesByNameUseCase
    {
        private readonly IInventoryRepository inventoryRepository = inventoryRepository;

        public async Task<IEnumerable<Inventory>> ExecuteAsync(string name = "")
        {
            return await inventoryRepository.GetInventoriesByNameAsync(name);
        }

    }
}
