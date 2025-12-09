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
        
        // Additional methods like Update, Delete, GetById could be added here
        // For now, focusing on requirements for search and add.
    }
}
