using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesShop.Models
{
    public class BasketItem : Entity
    {
        public string UserId { get; set; }

        public int ClothingItemId { get; set; }
        public ClothingItem ClothingItem { get; set; }

        public int Count { get; set; }
    }
}
