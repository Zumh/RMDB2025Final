 using MySql.Data.MySqlClient;
 using System.Collections.Generic;
using System.Data;

namespace BookStore
{

    public class CustomerRepository
    {

        private readonly string connectionString;
        private DataSet dataset;
        private MySqlDataAdapter adapter;
        public DataTable? Table;

        public CustomerRepository(string connectionString, DataSet ds)
        {
            this.connectionString = connectionString;
            dataset = ds;

            InitializeAdapter();
            LoadCustomers();
        }

        private void InitializeAdapter()
        {
            adapter = new MySqlDataAdapter("SELECT * FROM customer", connectionString);
            new MySqlCommandBuilder(adapter);
        }

        private void LoadCustomers()
        {
            // If the table exists, clear it so we get ONLY the latest data
            if (dataset.Tables.Contains("Customer"))
            {
                dataset.Tables["Customer"].Clear();
            }

            // Reload schema from database (fresh structure)
            adapter.FillSchema(dataset, SchemaType.Source, "Customer");

            // Reload latest data
            adapter.Fill(dataset, "Customer");

            Table = dataset.Tables["Customer"];

            // Let MySQL handle auto-increment, NOT the DataSet
            Table.Columns["id"].AutoIncrement = true;

            // Set primary key
            Table.PrimaryKey = new DataColumn[] { Table.Columns["id"] };
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
            // get all fresh customers from datatable
            LoadCustomers();
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

        // Delete customer by name and check if orders exist for that customer.
        // if orders exist, do not delete customer and return false. 
        public bool DeleteCustomerByName(string customerName, OrderRepository orderRepo)
        {
            Customer? customer = FindByName(customerName);
            if (customer != null)
            {
                Delete(customer);
                return true;
                //List<Order> orders = orderRepo.GetOrdersByCustomerId(customer.CustomerId);
                //if (orders.Count == 0)
                //{
                //    Delete(customer);
                //    return true;
                //}
            }
            return false;
        }

        // delete customer if only orders do not exist for customer
        //public bool DeleteCustomerIfNoOrders(Customer customer, OrderRepository orderRepo)
        //{
        //    List<Order> orders = orderRepo.GetOrdersByCustomerId(customer.CustomerId);
        //    if (orders.Count == 0)
        //    {
        //        Delete(customer);
        //        return true;
        //    }
        //    return false;
        //}
    }
}
