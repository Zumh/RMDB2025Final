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
        static public string connectionString = "Server=localhost;Port=3306;Uid=root;Pwd=123456;Database=BookStore;";

        static public string BuildConnectionString (string name, string password, string database)
        {
             return $"Server=localhost;Port=3306;Uid={name};Pwd={password};Database={database};";
        }

        //NAME: DataBaseQuery
        //DESCRIPTION: takes the query string and sends it to the database to retreive data
        //PARAMETERS: string query - mySQL statement used to query the database
        //RETURN: DataTable - datatable with relevant search results
        public DataTable DataBaseQuery(string query)
        {
            
            DataTable dataTable = new DataTable();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

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

        //NAME: DataBaseCRUD
        //DESCRIPTION: takes the query string and performs various CRUD operations on the database
        //PARAMETERS: string query - mySQL statement used to perform the operation on the database
        //RETURN: void
        public static int DataBaseCRUD(string query)
        {
            //default set to error value
            int result = -1;
            try
            {
                //create connection
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    //open connection
                    connection.Open();
                    //create command object
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        //get return value
                        result = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return result;
        }
    }
}
