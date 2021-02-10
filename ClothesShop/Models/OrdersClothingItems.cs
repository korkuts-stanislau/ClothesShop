using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesShop.Models
{
    public class OrdersClothingItems : Entity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ClothingItemId { get; set; }
        public ClothingItem ClothingItem { get; set; }

        public int Count { get; set; }
    }
}
