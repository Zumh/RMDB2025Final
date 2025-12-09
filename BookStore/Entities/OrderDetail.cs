using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Entities
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Display Properties (Not mapped to DB OrderDetail table directly, but used for UI)
        public string CustomerName { get; set; }
        public string BookTitle { get; set; }
        public string OrderDate { get; set; }
        public float TotalAmount { get; set; } // Order Total
    }
}
