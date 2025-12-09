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
