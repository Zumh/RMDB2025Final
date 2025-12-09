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
    }
    

}
