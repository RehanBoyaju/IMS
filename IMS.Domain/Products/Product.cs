using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Domain.Products
{
    public class Product : ProductBase
    {
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }
        public Location Location { get; set; }
        public Guid LocationId { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string  SerialNumber { get; set; }
        public string Description { get; set; }
        public string Base64Image { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
