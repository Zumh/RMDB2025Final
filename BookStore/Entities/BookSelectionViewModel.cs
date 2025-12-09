//FILE : BookSelectionViewModel.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: 
This class helps to show book details in the UI. 
It holds information like title, specific codes, and prices for display.
*/

using System;

namespace BookStore.Entities
{
    public class BookSelectionViewModel
    {
        public int BookID { get; set; }
        public string? Title { get; set; }
        public string? ISBN { get; set; }
        public decimal Price { get; set; }
        public int PublisherID { get; set; }
        public string? Author { get; set; }
        public int Stock { get; set; }
        public int QuantityToBuy { get; set; }
    }
}
