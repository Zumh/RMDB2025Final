using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace BookStore.DataAccess
{
    internal class BookRepository
    {
        private DBManager db = new DBManager();

        // Flag to check if we've already verified the column
        private static bool _authorColumnChecked = false;

        private void EnsureAuthorColumnExists()
        {
            if (_authorColumnChecked) return;

            try
            {
                // Check if column exists
                DataTable dt = db.DataBaseQuery("SELECT * FROM book LIMIT 1");
                if (dt != null && !dt.Columns.Contains("author"))
                {
                    // Add column
                    db.ExecuteNonQuery("ALTER TABLE book ADD COLUMN author VARCHAR(255) DEFAULT ''");
                }
                _authorColumnChecked = true;
            }
            catch (Exception)
            {
                // Usage of message box or logging? 
                // silently fail or maybe DB user doesn't have permissions
            }
        }

        //READ
        public List<Book> GetAll()
        {
            EnsureAuthorColumnExists();
            List<Book> books = new List<Book>();
            DataTable data = db.DataBaseQuery("SELECT * FROM book");
            if (data != null)
            {
                foreach (DataRow row in data.Rows)
                {
                    books.Add(MapRowToBook(row));
                }
            }
            return books;
        }

        // READ - ID
        public Book GetById(int bookId)
        {
            string query = "SELECT * FROM book WHERE id = @id";
            DataTable data = db.DataBaseQuery(query, new[] { "@id", bookId.ToString() });
            if (data?.Rows.Count > 0)
            {
                return MapRowToBook(data.Rows[0]);
            }
            return null;
        }

        // READ - Title
        public List<Book> SearchByTitle(string title)
        {
            List<Book> books = new List<Book>();
            string query = "SELECT * FROM book WHERE title LIKE @title";
            DataTable data = db.DataBaseQuery(query, new[] { "@title", $"%{title}%" });
            if (data != null)
            {
                foreach (DataRow row in data.Rows)
                {
                    books.Add(MapRowToBook(row));
                }
            }
            return books;
        }

        // READ - Author
        public List<Book> SearchByAuthor(string author)
        {
            List<Book> books = new List<Book>();
            string query = "SELECT * FROM book WHERE author LIKE @author";
            DataTable data = db.DataBaseQuery(query, new[] { "@author", $"%{author}%" });
            if (data != null)
            {
                foreach (DataRow row in data.Rows)
                {
                    books.Add(MapRowToBook(row));
                }
            }
            return books;
        }

        // READ - category
        public List<Book> SearchByCategory(int categoryId)
        {
            List<Book> books = new List<Book>();
            string query = "SELECT * FROM book WHERE categoryID = @categoryId";
            DataTable data = db.DataBaseQuery(query, new[] { "@categoryId", categoryId.ToString() });
            if (data != null)
            {
                foreach (DataRow row in data.Rows)
                {
                    books.Add(MapRowToBook(row));
                }
            }
            return books;
        }

        public List<Book> Search(string title, string author, string isbn, string price, int categoryId)
        {
            List<Book> books = new List<Book>();
            string query = "SELECT * FROM book WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(title))
                query += $" AND title LIKE '%{title}%'";
            if (!string.IsNullOrWhiteSpace(author))
                query += $" AND author LIKE '%{author}%'";
            if (!string.IsNullOrWhiteSpace(isbn))
                query += $" AND isbn LIKE '%{isbn}%'";
            
            if (decimal.TryParse(price, out decimal p))
            {
               query += $" AND price = {p}";
            }

            if (categoryId > 0)
            {
                query += $" AND categoryID = {categoryId}";
            }

            DataTable data = db.DataBaseQuery(query);
            if (data != null)
            {
                foreach (DataRow row in data.Rows)
                {
                    books.Add(MapRowToBook(row));
                }
            }
            return books;
        }

        // CREATE - Add book
        public bool Add(Book book)
        {
            EnsureAuthorColumnExists();
            string query = "INSERT INTO book (title, isbn, price, stock, publisherID, categoryID, author) " +
                          "VALUES (@title, @isbn, @price, @stock, @publisherId, @categoryId, @author)";

            var parameters = new Dictionary<string, string>
            {
                { "@title", book.Title },
                { "@isbn", book.ISBN },
                { "@price", book.Price.ToString() },
                { "@stock", book.Stock.ToString() },
                { "@publisherId", book.PublisherID.ToString() },
                { "@categoryId", book.CategoryID.ToString() },
                { "@author", book.Author ?? "" }
            };

            int result = db.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        // UPDATE - BookInfo
        public bool Update(Book book)
        {
             EnsureAuthorColumnExists();
            string query = "UPDATE book SET title=@title, isbn=@isbn, price=@price, " +
                          "stock=@stock, publisherID=@publisherId, categoryID=@categoryId, author=@author " +
                          "WHERE id=@id";

            var parameters = new Dictionary<string, string>
            {
                { "@id", book.BookID.ToString() },
                { "@title", book.Title },
                { "@isbn", book.ISBN },
                { "@price", book.Price.ToString() },
                { "@stock", book.Stock.ToString() },
                { "@publisherId", book.PublisherID.ToString() },
                { "@categoryId", book.CategoryID.ToString() },
                { "@author", book.Author ?? "" }
            };

            int result = db.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        // DELETE - Delete Book
        public bool Delete(int bookId)
        {
            string query = "DELETE FROM book WHERE id = @id";
            int result = db.ExecuteNonQuery(query, new Dictionary<string, string> { { "@id", bookId.ToString() } });
            return result > 0;
        }

        // Helper - Invert DataRows Book object 
        private Book MapRowToBook(DataRow row)
        {
            return new Book
            {
                BookID = Convert.ToInt32(row["id"]),
                PublisherID = Convert.ToInt32(row["publisherID"]),
                CategoryID = Convert.ToInt32(row["categoryID"]),
                Title = row["title"].ToString(),
                ISBN = row["isbn"].ToString(),
                Price = Convert.ToDecimal(row["price"]),
                Stock = Convert.ToInt32(row["stock"]),
                Author = row.Table.Columns.Contains("author") && row["author"] != DBNull.Value ? row["author"].ToString() : ""
            };
        }
    }
}
