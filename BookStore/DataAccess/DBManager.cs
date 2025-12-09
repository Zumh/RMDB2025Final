//FILE : DBManager.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: This is the database manager class that manages the connection to the database.
 *             It handles the queries to the database, retrieving and sending data back and forth. 
 *             
*/

using MySql.Data.MySqlClient;
using System.Data;
using static System.Reflection.Metadata.BlobBuilder;


namespace BookStore
{
    public class DBManager
    {
        public string ConnectionString { get; private set; }

        // Lazy-loaded repositories
        private CustomerRepository? customerRepo = null;
        private BookRepository? bookRepo = null;
        private OrderRepository? orderRepo = null;
        public DataSet? Data = null;
        public DBManager(){
            Data = new DataSet();
          //Data.EnforceConstraints = true;

        }
      
        public CustomerRepository Customers => customerRepo ??= new CustomerRepository(ConnectionString, Data);
        public BookRepository Books => bookRepo ??= new BookRepository(ConnectionString, Data);
        //public OrderRepository Orders => orderRepo ??= new OrderRepository(ConnectionString);

        public DbInitResult CheckDatabase(
           string server,
           string user,
           string password,
           string database,
           string port)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(server) ||
                string.IsNullOrWhiteSpace(user) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(database) ||
                string.IsNullOrWhiteSpace(port))
            {
                return new DbInitResult
                {
                    Success = false,
                    Message = "All fields are required."
                };
            }

            string connStr = $"server={server};port={port};uid={user};pwd={password};";

            try
            {
                using MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(
                    "SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @db;",
                    conn);
                cmd.Parameters.AddWithValue("@db", database);

                object? result = cmd.ExecuteScalar();

                if (result == null)
                {
                    return new DbInitResult
                    {
                        Success = false,
                        DatabaseExists = false,
                        NeedsCreation = true,
                        Message = $"Database '{database}' does not exist."
                    };
                }

                ConnectionString = $"server={server};port={port};uid={user};pwd={password};database={database};";

                return new DbInitResult
                {
                    Success = true,
                    DatabaseExists = true,
                    Message = "Database exists.",
                    ConnectionString = ConnectionString
                };
            }
            catch (Exception ex)
            {
                return new DbInitResult
                {
                    Success = false,
                    Message = $"Connection failed: {ex.Message}"
                };
            }
        }



        public DbInitResult CreateDatabase(
            string server,
            string user,
            string password,
            string database,
            string port)
        {
            string connStr = $"server={server};port={port};uid={user};pwd={password};";

            try
            {
                using MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();

                using MySqlCommand createCmd =
                    new MySqlCommand($"CREATE DATABASE `{database}`;", conn);
                createCmd.ExecuteNonQuery();

                ConnectionString =
                    $"server={server};port={port};uid={user};pwd={password};database={database};";

                return new DbInitResult
                {
                    Success = true,
                    DatabaseExists = true,
                    Message = $"Database '{database}' created.",
                    ConnectionString = ConnectionString
                };
            }
            catch (Exception ex)
            {
                return new DbInitResult
                {
                    Success = false,
                    Message = $"Database creation failed: {ex.Message}"
                };
            }
        }

    }
}
