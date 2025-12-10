//FILE : BookRepository.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: 
This class handles all database operations for books.
It allows adding, updating, deleting, and searching for books.
*/


using System.Data;


namespace BookStore.DataAccess
{
    /*
* Class: BookRepository
* Purpose: The BookRepository class has been created to manage all database operations
 *  related to books within the bookstore system. It provides methods to retrieve,
 *  search, add, update, and delete book records. The class handles mapping database
 *  rows to Book objects, ensures required columns exist in the database, and
 *  supports complex searches using multiple criteria such as title, author, ISBN,
 *  price, and category. This class serves as a bridge between the application logic
 *  and the underlying database for all book-related functionality.
 */
    internal class BookRepository
    {
        private DBManager db = new DBManager();

        // Flag to check if we've already verified the column
        private static bool _authorColumnChecked = false;
        
        // Cache for column names
        private static string _publisherNameCol = "";
        private static string _categoryNameCol = "";

        //NAME: EnsureAuthorColumnExists
        //DESCRIPTION: Checks if the 'author' column exists in the database and adds it if missing.
        //PARAMETERS: None.
        //RETURN: void
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
        
        private string GetPublisherNameColumn()
        {
            if (!string.IsNullOrEmpty(_publisherNameCol)) return _publisherNameCol;
            
            try 
            {
                DataTable dt = db.DataBaseQuery("SELECT * FROM publisher LIMIT 1");
                if (dt != null)
                {
                    if (dt.Columns.Contains("publisherName")) _publisherNameCol = "publisherName";
                    else if (dt.Columns.Contains("name")) _publisherNameCol = "name";
                    else if (dt.Columns.Contains("Title")) _publisherNameCol = "Title";
                    else if (dt.Columns.Contains("content")) _publisherNameCol = "content";
                    else if (dt.Columns.Count >= 2) _publisherNameCol = dt.Columns[1].ColumnName; // Fallback
                    else _publisherNameCol = "name";
                }
                else
                {
                     _publisherNameCol = "name";
                }
            } 
            catch { _publisherNameCol = "name"; }
            
            return _publisherNameCol;
        }

        private string GetCategoryNameColumn()
        {
            if (!string.IsNullOrEmpty(_categoryNameCol)) return _categoryNameCol;

            try
            {
                DataTable dt = db.DataBaseQuery("SELECT * FROM category LIMIT 1");
                if (dt != null)
                {
                    if (dt.Columns.Contains("name")) _categoryNameCol = "name";
                    else if (dt.Columns.Contains("CategoryName")) _categoryNameCol = "CategoryName";
                    else if (dt.Columns.Contains("categoryName")) _categoryNameCol = "categoryName";
                    else if (dt.Columns.Contains("Title")) _categoryNameCol = "Title";
                    else if (dt.Columns.Contains("Genre")) _categoryNameCol = "Genre";
                    else if (dt.Columns.Count >= 2) _categoryNameCol = dt.Columns[1].ColumnName; // Fallback
                    else _categoryNameCol = "name";
                }
                 else
                {
                     _categoryNameCol = "name";
                }
            }
            catch { _categoryNameCol = "name"; }

            return _categoryNameCol;
        }

        private string GetBaseSelectQuery()
        {
            string pCol = GetPublisherNameColumn();
            string cCol = GetCategoryNameColumn();
            return $"SELECT b.*, p.{pCol} AS PublisherName, c.{cCol} AS CategoryName FROM book b LEFT JOIN publisher p ON b.publisherID = p.id LEFT JOIN category c ON b.categoryID = c.id";
        }

        //NAME: GetAll
        //DESCRIPTION: Gets all books from the database.
        //PARAMETERS: None.
        //RETURN: books
        public List<Book> GetAll()
        {
            EnsureAuthorColumnExists();
            List<Book> books = new List<Book>();
            DataTable data = db.DataBaseQuery(GetBaseSelectQuery());
            if (data != null)
            {
                foreach (DataRow row in data.Rows)
                {
                    books.Add(MapRowToBook(row));
                }
            }
            return books;
        }

        //NAME: GetById
        //DESCRIPTION: Finds a book by its ID.
        //PARAMETERS: int bookId
        //RETURN: MapRowToBook(data.Rows[0]), or null if not found.
        public Book? GetById(int bookId)
        {
            string query = $"{GetBaseSelectQuery()} WHERE b.id = @id";
            DataTable data = db.DataBaseQuery(query, new[] { "@id", bookId.ToString() });
            if (data?.Rows.Count > 0)
            {
                return MapRowToBook(data.Rows[0]);
            }
            return null;
        }

        //NAME: SearchByTitle
        //DESCRIPTION: Searches for books by title.
        //PARAMETERS: string title
        //RETURN: books
        public List<Book> SearchByTitle(string title)
        {
            List<Book> books = new List<Book>();
            string query = $"{GetBaseSelectQuery()} WHERE b.title LIKE @title";
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

        //NAME: SearchByAuthor
        //DESCRIPTION: Searches for books by author.
        //PARAMETERS: string author
        //RETURN: books
        public List<Book> SearchByAuthor(string author)
        {
            List<Book> books = new List<Book>();
            string query = $"{GetBaseSelectQuery()} WHERE b.author LIKE @author";
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

        //NAME: SearchByCategory
        //DESCRIPTION: Searches for books in a specific category.
        //PARAMETERS: int categoryId
        //RETURN: books
        public List<Book> SearchByCategory(int categoryId)
        {
            List<Book> books = new List<Book>();
            string query = $"{GetBaseSelectQuery()} WHERE b.categoryID = @categoryId";
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

        //NAME: Search
        //DESCRIPTION: Searches for books using multiple criteria (Title, Author, ISBN, Price, Category, Publisher, Stock).
        //PARAMETERS: string title, string author, string isbn, string price, int categoryId, string publisher, string stock
        //RETURN: books
        public List<Book> Search(string title, string author, string isbn, string price, int categoryId, string publisher, string stock)
        {
            List<Book> books = new List<Book>();
            string query = $"{GetBaseSelectQuery()} WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(title))
                query += $" AND b.title LIKE '%{title}%'";
            if (!string.IsNullOrWhiteSpace(author))
                query += $" AND b.author LIKE '%{author}%'";
            if (!string.IsNullOrWhiteSpace(isbn))
                query += $" AND b.isbn LIKE '%{isbn}%'";
            
            if (decimal.TryParse(price, out decimal p))
            {
               query += $" AND b.price = {p}";
            }

            if (categoryId > 0)
            {
                query += $" AND b.categoryID = {categoryId}";
            }

            if (!string.IsNullOrWhiteSpace(publisher))
            {
                // Use the dynamic publisher column name
                string pCol = GetPublisherNameColumn();
                query += $" AND p.{pCol} LIKE '%{publisher}%'";
            }

            if (int.TryParse(stock, out int s))
            {
                query += $" AND b.stock = {s}";
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

        //NAME: GetOrCreatePublisherId
        //DESCRIPTION: Finds a publisher by name or creates a new one if it doesn't exist.
        //PARAMETERS: string publisherName
        //RETURN: int - Publisher ID
        public int GetOrCreatePublisherId(string publisherName)
        {
            if (string.IsNullOrWhiteSpace(publisherName))
            {
                // Return default if empty - or could throw exception
                return 1; // Default to first publisher
            }

            try
            {
                string pCol = GetPublisherNameColumn();
                
                // Try to find existing publisher
                string query = $"SELECT id FROM publisher WHERE {pCol} = @name";
                DataTable data = db.DataBaseQuery(query, new[] { "@name", publisherName });
                
                if (data != null && data.Rows.Count > 0)
                {
                    return Convert.ToInt32(data.Rows[0]["id"]);
                }
                
                // Publisher doesn't exist, create new one
                string insertQuery = $"INSERT INTO publisher ({pCol}) VALUES (@name)";
                db.ExecuteNonQuery(insertQuery, new Dictionary<string, string> { { "@name", publisherName } });
                
                // Get the newly created ID
                string getIdQuery = "SELECT LAST_INSERT_ID() as id";
                DataTable idData = db.DataBaseQuery(getIdQuery);
                
                if (idData != null && idData.Rows.Count > 0)
                {
                    return Convert.ToInt32(idData.Rows[0]["id"]);
                }
                
                // Fallback
                return 1;
            }
            catch
            {
                // On error, return default
                return 1;
            }
        }

        // CREATE - Add book
        //NAME: Add
        //DESCRIPTION: Adds a new book to the database.
        //PARAMETERS: Book book
        //RETURN: bool - True if adding was successful, False otherwise.
        public bool Add(Book book)
        {
            EnsureAuthorColumnExists();
            string query = "INSERT INTO book (title, isbn, price, stock, publisherID, categoryID, author) " +
                          "VALUES (@title, @isbn, @price, @stock, @publisherId, @categoryId, @author)";

            var parameters = new Dictionary<string, string>
            {
                { "@title", book.Title ?? "" },
                { "@isbn", book.ISBN ?? "" },
                { "@price", book.Price.ToString() },
                { "@stock", book.Stock.ToString() },
                { "@publisherId", book.PublisherID.ToString() },
                { "@categoryId", book.CategoryID.ToString() },
                { "@author", book.Author ?? "" }
            };

            int result = db.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        //NAME: Update
        //DESCRIPTION: Updates an existing book in the database.
        //PARAMETERS: Book book
        //RETURN: bool - True if update was successful, False otherwise.
        public bool Update(Book book)
        {
             EnsureAuthorColumnExists();
            string query = "UPDATE book SET title=@title, isbn=@isbn, price=@price, " +
                          "stock=@stock, publisherID=@publisherId, categoryID=@categoryId, author=@author " +
                          "WHERE id=@id";

            var parameters = new Dictionary<string, string>
            {
                { "@id", book.BookID.ToString() },
                { "@title", book.Title ?? "" },
                { "@isbn", book.ISBN ?? "" },
                { "@price", book.Price.ToString() },
                { "@stock", book.Stock.ToString() },
                { "@publisherId", book.PublisherID.ToString() },
                { "@categoryId", book.CategoryID.ToString() },
                { "@author", book.Author ?? "" }
            };

            int result = db.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        //NAME: Delete
        //DESCRIPTION: Deletes a book from the database.
        //PARAMETERS: int bookId
        //RETURN: bool - True if delete was successful, False otherwise.
        public bool Delete(int bookId)
        {
            string query = "DELETE FROM book WHERE id = @id";
            int result = db.ExecuteNonQuery(query, new Dictionary<string, string> { { "@id", bookId.ToString() } });
            return result > 0;
        }

        //NAME: MapRowToBook
        //DESCRIPTION: Converts a database row into a Book object.
        //PARAMETERS: DataRow row
        //RETURN: new Book
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
                Author = row.Table.Columns.Contains("author") && row["author"] != DBNull.Value ? row["author"].ToString() : "",
                PublisherName = row.Table.Columns.Contains("PublisherName") && row["PublisherName"] != DBNull.Value ? row["PublisherName"].ToString() : "",
                CategoryName = row.Table.Columns.Contains("CategoryName") && row["CategoryName"] != DBNull.Value ? row["CategoryName"].ToString() : ""
            };
        }
    }
}

