using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
    }

}
