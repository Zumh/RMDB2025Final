//FILE : OrderDisplayItem.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: 
This class is for showing order items on the screen.
It makes it easier to list what customers have ordered in a readable way.
*/

using System;

namespace BookStore.Entities
{

    /*
    * Class: OrderDisplayItem
    * Purpose: The OrderDisplayItem class has been created to represent a simplified view
     *  of an order for display purposes within the application. It contains key
     *  information such as the order ID, customer name, book title, quantity,
     *  total amount, and order date. This class is used to present order data
     *  in user interfaces without exposing the full underlying database structure.
     */

    public class OrderDisplayItem
    {
        public int OrderId { get; set; }
        public string? CustomerName { get; set; }
        public string? BookTitle { get; set; }
        public int Quantity { get; set; }
        public float TotalAmount { get; set; }
        public string? OrderDate { get; set; }
    }
}
