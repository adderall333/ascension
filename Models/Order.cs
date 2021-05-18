using System;
using System.Collections.Generic;
using Models.Attributes;

namespace Models
{
    public enum Status
    {
        NotPaid,
        Paid, 
        Packing,
        Delivering,
        Delivered,
        Cancelled
    }
    
    public class Order
    {
        public int Id { get; set; }
        
        public DateTime OrderTime { get; set; }
        
        public User User { get; set; }
        public int UserId { get; set; }
        
        public int Amount { get; set; }
        
        public Status Status { get; set; } // NotPaid ,Paid ,Packing, Delivering, Delivered.
        
        public string DeliveryType { get; set; } // Delivery, SelfTake
        
        public string DeliveryAddress { get; set; } // If DeliveryType == "Delivery"
        
        public List<ProductLine> ProductLines { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, UserId {UserId}";
        }

        public Order()
        {
        }
    }
}