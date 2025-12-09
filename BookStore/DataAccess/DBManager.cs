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
using System.Windows;

namespace BookStore
{
    internal class DBManager
    {
        public static string connectionString = "Server=localhost;Port=3306;Uid=root;Pwd=123456;Database=BookStore;";
        MySqlConnection connection = new(connectionString);

        public DataTable DataBaseQuery(string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    // Console.WriteLine("Application Connected to DB"); // Removed spammy log

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dataTable); // Fill the DataTable with results
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message); // Show error to user
            }
            return dataTable;
        }

        public int ExecuteNonQuery(string query)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return -1;
            }
        }
      

    }
}
