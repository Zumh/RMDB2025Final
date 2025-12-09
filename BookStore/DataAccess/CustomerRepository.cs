 using MySql.Data.MySqlClient;
 using System.Collections.Generic;
using System.Data;

namespace BookStore
{

    public class CustomerRepository
    {


        private readonly MySqlDataAdapter adapter;
        public DataTable? Table = null;
        public CustomerRepository(string connectionString, DataSet ds)
        {
            adapter = new MySqlDataAdapter("SELECT * FROM customer", connectionString);
            new MySqlCommandBuilder(adapter);
            adapter.Fill(ds, "Customer");
            Table = ds.Tables["Customer"];
            Table.PrimaryKey = new DataColumn[] { Table.Columns["CustomerId"] };
        }

        public void Add(Customer currentCustomer)
        {

            DataRow row = Table.NewRow();
            row["name"] = currentCustomer.CustomerName;
            row["email"] = currentCustomer.Email;
            row["address"] = currentCustomer.Address;
            row["phoneNumber"] = currentCustomer.Phone;
            Table.Rows.Add(row);
        }

        public void SaveChanges() => adapter.Update(Table);

        public void Delete(Customer customer)
        {
            DataRow? row = Table.Rows.Find(customer.CustomerId);
            if (row != null)
            {
                row.Delete();
            }
        }

        // Get all customers
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            foreach (DataRow row in Table.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    Customer customer = new Customer
                    {
                        CustomerId = Convert.ToInt32(row["id"]),
                        CustomerName = row["name"].ToString(),
                        Email = row["email"].ToString(),
                        Address = row["address"].ToString(),
                        Phone = row["phoneNumber"].ToString()
                    };
                    customers.Add(customer);
                }
            }
            return customers;
        }

        // find customer by id 
        public Customer? FindById(int customerId)
        {
            DataRow? row = Table.Rows.Find(customerId);
            if (row != null)
            {
                return new Customer
                {
                    CustomerId = Convert.ToInt32(row["id"]),
                    CustomerName = row["name"].ToString(),
                    Email = row["email"].ToString(),
                    Address = row["address"].ToString(),
                    Phone = row["phoneNumber"].ToString()
                };
            }
            return null;

        }

        // find customer by name
        public Customer? FindByName(string customerName)
        {
            foreach (DataRow row in Table.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row["name"].ToString() == customerName)
                {
                    return new Customer
                    {
                        CustomerId = Convert.ToInt32(row["id"]),
                        CustomerName = row["name"].ToString(),
                        Email = row["email"].ToString(),
                        Address = row["address"].ToString(),
                        Phone = row["phoneNumber"].ToString()
                    };
                }
            }
            return null;
        }

        // find customer by email
        public Customer? FindByEmail(string email)
        {
            foreach (DataRow row in Table.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row["email"].ToString() == email)
                {
                    return new Customer
                    {
                        CustomerId = Convert.ToInt32(row["id"]),
                        CustomerName = row["name"].ToString(),
                        Email = row["email"].ToString(),
                        Address = row["address"].ToString(),
                        Phone = row["phoneNumber"].ToString()
                    };
                }
            }
            return null;
        }

        // find customer by phone number
        public Customer? FindByPhone(string phoneNumber)
        {
            foreach (DataRow row in Table.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row["phoneNumber"].ToString() == phoneNumber)
                {
                    return new Customer
                    {
                        CustomerId = Convert.ToInt32(row["id"]),
                        CustomerName = row["name"].ToString(),
                        Email = row["email"].ToString(),
                        Address = row["address"].ToString(),
                        Phone = row["phoneNumber"].ToString()
                    };
                }
            }
            return null;
        }

        // find customer by address
        public Customer? FindByAddress(string address)
        {
            foreach (DataRow row in Table.Rows)
            {
                if (row.RowState != DataRowState.Deleted && row["address"].ToString() == address)
                {
                    return new Customer
                    {
                        CustomerId = Convert.ToInt32(row["id"]),
                        CustomerName = row["name"].ToString(),
                        Email = row["email"].ToString(),
                        Address = row["address"].ToString(),
                        Phone = row["phoneNumber"].ToString()
                    };
                }
            }
            return null;
        }
    }
}
