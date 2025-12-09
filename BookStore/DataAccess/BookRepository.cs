using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Entities;
using System.Data;

namespace BookStore.DataAccess
{
    internal class BookRepository
    {
        private DBManager db = new DBManager();

        public List<Book> GetAll()
        {
            List<Book> books = new List<Book>();
            DataTable data = db.DataBaseQuery("SELECT * FROM book");
            if (data != null)
            {
                foreach (DataRow row in data.Rows)
                {
                    books.Add(new Book
                    {
                        BookID = Convert.ToInt32(row["id"]),
                        PublisherID = Convert.ToInt32(row["publisherID"]),
                        CategoryID = Convert.ToInt32(row["categoryID"]),
                        Title = row["title"].ToString(),
                        ISBN = row["isbn"].ToString(),
                        Price = Convert.ToDecimal(row["price"]),
                        Stock = Convert.ToInt32(row["stock"])
                    });
                }
            }
            return books;
        }

        public void Add(Book book)
        {
            // Note: Assuming PublisherID and CategoryID are valid or handled. 
            // For simplicity/prototype, defaulting to 1 or allowing inputs if UI supported it.
            // Using 1 for now if not set, or ensuring UI sets it.
            // Based on current UI, Publisher is a text box, but schema has ID. 
            // We need to resolve Publisher Name -> ID or just insert.
            // Requirement says "Publishers as direct suppliers". 
            // I will assume for now we might need a method to get/add publisher by name.
            
            // For now, simple insert.
            string query = $"INSERT INTO book (title, isbn, price, stock, publisherID, categoryID) VALUES ('{book.Title}', '{book.ISBN}', {book.Price}, {book.Stock}, {book.PublisherID}, {book.CategoryID})";
            db.ExecuteNonQuery(query);
        }
    }
}
