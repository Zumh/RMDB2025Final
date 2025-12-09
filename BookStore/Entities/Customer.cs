//FILE : Customer.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: This is the Customer class that manages the customer objects created when the database information is retrieved.
 *             It contains all the functions necessary to validate new customers, existing customers and load datatables with data.
*/

using System.Text.RegularExpressions;
using System.Data;

namespace BookStore
{
    internal class Customer
    {
        //regex patterns for email and phone number
        static readonly string regexEmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        static readonly string  regexPhonePattern = @"^\d{3}-\d{3}-\d{4}$";

        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        static public List<Customer> _customers = new List<Customer>(); 

        //NAME: ValidateName
        //DESCRIPTION: Validates the customer name is not blank
        //PARAMETERS: string customerName - name of customer
        //RETURN: bool isValid - true if name is valid otherwise false
        public static bool ValidateName(string customerName)
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(customerName.Trim()))
            {
                isValid = false;
            }
                return isValid;
        }
        //NAME: ValidateAddress
        //DESCRIPTION: Validates the customer address is not blank
        //PARAMETERS: string address - address of customer
        //RETURN: bool isValid - true if address is valid otherwise false
        public static bool ValidateAddress(string address)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(address.Trim()))
            {
                isValid = false;
            }
            return isValid;
        }
        //NAME: ValidatePhoneNumber
        //DESCRIPTION: Validates the customer phone number is not blank and in proper format
        //PARAMETERS: string phone - phone number of customer
        //RETURN: bool isValid - true if phone number is valid otherwise false
        public static bool ValidatePhoneNumber(string phone) 
        {
            bool isValid = true;
            if (!string.IsNullOrEmpty(phone.Trim()))
            {
                if (!Regex.IsMatch(phone, regexPhonePattern))
                {
                    isValid = false;
                }
            }
            return isValid;
        }
        //NAME: ValidateEmail
        //DESCRIPTION: Validates the customer email is not blank and in proper format
        //PARAMETERS: string email - email of customer
        //RETURN: bool isValid - true if email is valid otherwise false
        public static bool ValidateEmail(string email) 
        {
            bool isValid = true;
            if (!string.IsNullOrEmpty(email.Trim()))
            {
                if (!Regex.IsMatch(email, regexEmailPattern))
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        //NAME: LoadCustomerData
        //DESCRIPTION: Adds the dataTable to a Customer List
        //PARAMETERS: DataTable data - customer data from database
        //RETURN: void
        public static void LoadCustomerData (DataTable data)
        {
                //create a new customer object for each customer in database
                foreach (DataRow row in data.Rows)
                {
                    _customers.Add(new Customer
                    {
                        CustomerId = Convert.ToInt32(row["id"]),
                        CustomerName = row["name"].ToString(),
                        Address = row["address"].ToString(),
                        Email = row["email"].ToString(),
                        Phone = row["phonenumber"].ToString()
                    });
                }
        }
        //NAME: SearchByName
        //DESCRIPTION: Searches through customer list to filter by name
        //PARAMETERS: none
        //RETURN: void
        public static List<Customer> SearchByName(string name)
        {
            List<Customer> list = new List<Customer>();
            foreach (Customer customer in Customer._customers)
            {
                if (customer.CustomerName != null &&
                    customer.CustomerName.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(customer);
                }
            }
            return list;
        }
    }

}
