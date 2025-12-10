//FILE : CustomerRepository.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: 
This class handles customer data operations.
It allows saving new customers and finding existing ones.
*/


using System.Data;

namespace BookStore.DataAccess
{
    /*
* Class: CustomerRepository
* Purpose: The CustomerRepository class has been created to manage all database
 *  operations related to customers within the bookstore system. It provides
 *  methods to retrieve, add, delete, and search customer records. The class
 *  handles mapping database rows to Customer objects and supports searching
 *  using multiple criteria such as name, email, address, and phone number,
 *  ensuring consistent and convenient access to customer data throughout
 *  the application.
 */

    internal class CustomerRepository
    {
        private DBManager db = new DBManager();

        //NAME: GetAll
        //DESCRIPTION: Gets all customers from the database.
        //PARAMETERS: None
        //RETURN: customers
        public List<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();
            DataTable data = db.DataBaseQuery("SELECT * FROM customer");
            if (data != null)
            {
                foreach (DataRow row in data.Rows)
                {
                    customers.Add(new Customer
                    {
                        CustomerId = Convert.ToInt32(row["id"]),
                        CustomerName = row["name"].ToString(),
                        Address = row["address"].ToString(),
                        Email = row["email"].ToString(),
                        Phone = row["phonenumber"].ToString()
                    });
                }
            }
            return customers;
        }

        //NAME: Add
        //DESCRIPTION: Adds a new customer to the database.
        //PARAMETERS: Customer customer
        //RETURN: void
        public void Add(Customer customer)
        {
            string query = $"INSERT INTO customer (name, address, email, phonenumber) VALUES ('{customer.CustomerName}', '{customer.Address}', '{customer.Email}', '{customer.Phone}')";
            db.ExecuteNonQuery(query);
        }
        
        //NAME: Delete
        //DESCRIPTION: Deletes a customer from the database.
        //PARAMETERS: int id
        //RETURN: void
        public void Delete(int id)
        {
            string query = $"DELETE FROM customer WHERE id = {id}";
            db.ExecuteNonQuery(query);
        }

        //NAME: Search
        //DESCRIPTION: Searches for customers using multiple criteria (Name, Email, Address, Phone).
        //PARAMETERS: string name, string email, string address, string phone
        //RETURN: customers
        public List<Customer> Search(string name, string email, string address, string phone)
        {
            List<Customer> customers = new List<Customer>();
            // Start with a base query
            string query = "SELECT * FROM customer WHERE 1=1";
            
            // Append conditions
            if (!string.IsNullOrWhiteSpace(name))
                query += $" AND name LIKE '%{name}%'";
            if (!string.IsNullOrWhiteSpace(email))
                query += $" AND email LIKE '%{email}%'";
            if (!string.IsNullOrWhiteSpace(address))
                query += $" AND address LIKE '%{address}%'";
            if (!string.IsNullOrWhiteSpace(phone))
                query += $" AND phonenumber LIKE '%{phone}%'";

            DataTable data = db.DataBaseQuery(query);
            if (data != null)
            {
                foreach (DataRow row in data.Rows)
                {
                    customers.Add(new Customer
                    {
                        CustomerId = Convert.ToInt32(row["id"]),
                        CustomerName = row["name"].ToString(),
                        Address = row["address"].ToString(),
                        Email = row["email"].ToString(),
                        Phone = row["phonenumber"].ToString()
                    });
                }
            }
            return customers;
        }
    }
}
