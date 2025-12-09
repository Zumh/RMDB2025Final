using System;

namespace BookStore.Entities
{
    public class OrderDisplayItem
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public float TotalAmount { get; set; }
        public string OrderDate { get; set; }
    }
}
