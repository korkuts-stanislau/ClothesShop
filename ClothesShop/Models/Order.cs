using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesShop.Models
{
    public class Order : Entity
    {
        public string UserId { get; set; }
        public int MyProperty { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime Date { get; set; }
        public bool IsPaid { get; set; }
        public bool IsSent { get; set; }
    }
}
