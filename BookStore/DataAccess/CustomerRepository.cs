using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Entities;
using System.Data;

namespace BookStore.DataAccess
{
    internal class CustomerRepository
    {
        private DBManager db = new DBManager();

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

        public void Add(Customer customer)
        {
            string query = $"INSERT INTO customer (name, address, email, phonenumber) VALUES ('{customer.CustomerName}', '{customer.Address}', '{customer.Email}', '{customer.Phone}')";
            db.ExecuteNonQuery(query);
        }
        
        public void Delete(int id)
        {
            string query = $"DELETE FROM customer WHERE id = {id}";
            db.ExecuteNonQuery(query);
        }

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
