//FILE : OrderDetail.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: 
This class holds the specific details of a single item in an order.
It connects the order to the book and keeps track of how many were bought.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Entities
{
    /*
   * Class: OrderDetail
   * Purpose: The OrderDetail class has been created to represent the details of a
     *  specific order within the bookstore system. It contains members to track
     *  the book, quantity, and price for each item in an order. In addition,
     *  it includes display-related properties such as customer name, book title,
     *  order date, and total amount to facilitate presentation in user interfaces
     *  or reports. This class bridges the raw order data with a more readable format
     *  for display and processing purposes.
     */

    public class OrderDetail
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Display Properties
        public string? CustomerName { get; set; }
        public string? BookTitle { get; set; }
        public string? OrderDate { get; set; }
        public float TotalAmount { get; set; } // Order Total
    }
}
