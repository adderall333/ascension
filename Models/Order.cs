using System;
using System.Collections.Generic;

namespace Models
{
    public class Order
    {
        public int Id { get; set; }
        
        public DateTime OrderTime { get; set; }
        
        public int UserId { get; set; }

        public double Amount { get; set; }
        
        public string Status { get; set; }
        
        public string PaymentMethod { get; set; } // Cash, Online
        
        public string DeliveryType { get; set; } // Delivery, SelfTake
        
        public string DeliveryAddress { get; set; } // If DeliveryType == "Delivery"

        public List<ProductLine> ProductLines { get; set; }

        public Order()
        {
        }
    }
}