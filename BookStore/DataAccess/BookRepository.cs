

using MySql.Data.MySqlClient;
using System.Data;

namespace BookStore
{
    public class BookRepository
    {
        private readonly string connectionString;
        private DataSet dataset;
        private MySqlDataAdapter bookAdapter;
        private MySqlDataAdapter publisherAdapter;
        private MySqlDataAdapter categoryAdapter;

        public DataTable? Table;

        public BookRepository(string connectionString, DataSet ds)
        {
            this.connectionString = connectionString;
            dataset = ds;

            InitializeAdapters();
            LoadAll();
        }

        private void InitializeAdapters()
        {
            bookAdapter = new MySqlDataAdapter("SELECT * FROM book", connectionString);
            publisherAdapter = new MySqlDataAdapter("SELECT * FROM publisher", connectionString);
            categoryAdapter = new MySqlDataAdapter("SELECT * FROM category", connectionString);

            new MySqlCommandBuilder(bookAdapter);
        }

        private void LoadAll()
        {
            // Clear previous data
            ClearTable("Book");
            ClearTable("Publisher");
            ClearTable("Category");

            // Load schema and data
            publisherAdapter.FillSchema(dataset, SchemaType.Source, "Publisher");
            publisherAdapter.Fill(dataset, "Publisher");

            categoryAdapter.FillSchema(dataset, SchemaType.Source, "Category");
            categoryAdapter.Fill(dataset, "Category");

            bookAdapter.FillSchema(dataset, SchemaType.Source, "Book");
            bookAdapter.Fill(dataset, "Book");

            // Assign main table
            Table = dataset.Tables["Book"];

            // Let MySQL handle identity
            Table.Columns["id"].AutoIncrement = true;
            Table.PrimaryKey = new[] { Table.Columns["id"] };

            CreateRelations();
        }

        private void ClearTable(string name)
        {
            if (dataset.Tables.Contains(name))
            {
                dataset.Tables[name].Clear();
            }
                
        }

        private void CreateRelations()
        {
            if (!dataset.Relations.Contains("FK_Book_Publisher"))
            {
                dataset.Relations.Add(
                    "FK_Book_Publisher",
                    dataset.Tables["Publisher"].Columns["id"],
                    dataset.Tables["Book"].Columns["publisherID"],
                    true
                );
            }

            if (!dataset.Relations.Contains("FK_Book_Category"))
            {
                dataset.Relations.Add(
                    "FK_Book_Category",
                    dataset.Tables["Category"].Columns["id"],
                    dataset.Tables["Book"].Columns["categoryID"],
                    true
                );
            }
        }
        public void Add(Book currentBook)
        {

            DataRow row = Table.NewRow();


            row["title"] = currentBook.Title;
            row["price"] = currentBook.Price;
            row["stock"] = currentBook.Stock;
            row["publisherID"] = currentBook.PublisherID;
            row["categoryID"] = currentBook.CategoryID;
            row["isbn"] = currentBook.ISBN;


            Table.Rows.Add(row);
        }
        public void SaveChanges() => bookAdapter.Update(Table);

        // Get all books

        //NAME: GetAllBooks
        //DESCRIPTION: Adds the books from the database to the books List
        //PARAMETERS: None
        //RETURN: void
        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();
            LoadAll();
            foreach (DataRow row in Table.Rows)
            {
         
          
                books.Add(new Book
                {
                    BookID = Convert.ToInt32(row["id"]),
                    Title = row["title"].ToString(),
                    ISBN = Convert.ToDouble(row["isbn"]),
                    Price = Convert.ToSingle(row["price"]),
                    Stock = Convert.ToInt32(row["stock"]),
                    PublisherID = Convert.ToInt32(row["publisherID"]),
                    CategoryID = Convert.ToInt32(row["categoryID"])
                });
            }
            return books;
        }

        public void Delete(Book currentBook)
        {
            DataRow? row = Table.Rows.Find(currentBook.BookID);
            if (row != null)
            {
                row.Delete();
            }
   

        }


    }


}
