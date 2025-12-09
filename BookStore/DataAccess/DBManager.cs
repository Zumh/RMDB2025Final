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

        //NAME: ExecuteNonQuery
        //DESCRIPTION: Executes a SQL command that does not return data
        //PARAMETERS: string query. Dictionary<string, string> parameters = null
        //RETURN: command.ExecuteNonQuery(), or -1 if there was an error.
        public int ExecuteNonQuery(string query, Dictionary<string, string> parameters = null)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }
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

        //NAME: DataBaseQuery
        //DESCRIPTION: Executes a SQL command that returns data
        //PARAMETERS: string query. string[] parameters =null
        //RETURN: dataTable
        public DataTable DataBaseQuery(string query, string[] parameters = null)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            for (int i = 0; i < parameters.Length; i += 2)
                            {
                                command.Parameters.AddWithValue(parameters[i], parameters[i + 1]);
                            }
                        }
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return dataTable;
        }
    }
}
