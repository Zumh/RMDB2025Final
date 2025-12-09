 using MySql.Data.MySqlClient;
 using System.Collections.Generic;
using System.Data;

namespace BookStore
{
    
    public class CustomerRepository
    {
        private string connectionString;


        public CustomerRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

       
    
    }
    

}
