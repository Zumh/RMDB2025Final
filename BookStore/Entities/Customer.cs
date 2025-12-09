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
        //DESCRIPTION: Checks if the customer name is empty.
        //PARAMETERS: string customerName
        //RETURN: sValid
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
        //DESCRIPTION: Checks if the customer address is empty.
        //PARAMETERS: string address
        //RETURN: isValid
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
        //DESCRIPTION: Checks if the phone number is valid and not empty.
        //PARAMETERS: string phone
        //RETURN: isValid
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
        //DESCRIPTION: Checks if the email is valid and not empty.
        //PARAMETERS: string email
        //RETURN: isValid
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
        //DESCRIPTION: Gets customer data from the database and adds it to the list.
        //PARAMETERS: DataTable data
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
        //DESCRIPTION: Searches for customers by their name.
        //PARAMETERS: string name
        //RETURN: list
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
