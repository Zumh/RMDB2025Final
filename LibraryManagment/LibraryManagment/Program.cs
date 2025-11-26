
using MySql.Data.MySqlClient;
using System;
namespace LibraryManagment
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionstring = "Server=localhost;Port=3306;Uid=root;Pwd=;Database=filmclub";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            string query = "SELECT * FROM memberdetails";
            try
            {


                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                reader.Read();
                Console.WriteLine(reader["FirstName"]);
                Console.WriteLine(reader["LastName"]);
                // update 
                Console.WriteLine("Connected to Database");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connection to MySQL: ", ex.Message);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    Console.WriteLine("Disconnected from DB");
                }

            }
        }

    }
}