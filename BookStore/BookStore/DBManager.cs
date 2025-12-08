
using MySql.Data.MySqlClient;
using System.Data;

namespace BookStore
{
    internal class DBManager
    {
        static string connectionString = "Server=localhost;Port=3306;Uid=root;Pwd=123456;Database=BookStore;";
        MySqlConnection connection = new(connectionString);

        public DataTable GetDataTable(string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Application Connected to DB");

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dataTable); // Fill the DataTable with results
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return dataTable;
        }


    }
}
