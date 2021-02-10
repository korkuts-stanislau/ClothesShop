using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesShop.Models
{
    public class ClothingItem : Entity
    {
        public int TypeId { get; set; }
        public ClothingItemType ClothingItemType { get; set; }

        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Size { get; set; }
        public bool IsMail { get; set; }
        public decimal Price { get; set; }
    }
}
