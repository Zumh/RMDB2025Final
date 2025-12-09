//FILE : DBManager.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: This is the database manager class that manages the connection to the database.
* It handles the queries to the database, retrieving and sending data back and forth.
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

        // ORIGINAL METHOD - Keep for backward compatibility
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
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return dataTable;
        }

        // ✅ OVERLOAD 1: Array of parameters (param1, value1, param2, value2, ...)
        public DataTable DataBaseQuery(string query, string[] parameters)
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
                            // Add parameters: @param1, value1, @param2, value2...
                            for (int i = 0; i < parameters.Length; i += 2)
                            {
                                if (i + 1 < parameters.Length)
                                {
                                    command.Parameters.AddWithValue(parameters[i], parameters[i + 1]);
                                }
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

        // ✅ OVERLOAD 2: Dictionary of parameters
        public DataTable DataBaseQuery(string query, Dictionary<string, string> parameters)
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
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value ?? "")
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

        // ORIGINAL METHOD - Keep for backward compatibility
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

        // ✅ OVERLOAD 1: Array of parameters
        public int ExecuteNonQuery(string query, string[] parameters)
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
                            for (int i = 0; i < parameters.Length; i += 2)
                            {
                                if (i + 1 < parameters.Length)
                                {
                                    command.Parameters.AddWithValue(parameters[i], parameters[i + 1]);
                                }
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

        // ✅ OVERLOAD 2: Dictionary of parameters
        public int ExecuteNonQuery(string query, Dictionary<string, string> parameters)
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
                                command.Parameters.AddWithValue(param.Key, param.Value ?? "");
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
    }
}
