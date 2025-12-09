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

namespace BookStore
{
    internal class DBManager
    {
        static public string connectionString = "Server=localhost;Port=3306;Uid=root;Pwd=123456;Database=BookStore;";

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
