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

        public string? name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }

        public static bool ValidateName(string customerName)
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(customerName))
            {
                isValid = false;
            }
                return isValid;
        }
        public static bool ValidateAddress(string address)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(address))
            {
                isValid = false;
            }
            return isValid;
        }
        public static bool ValidatePhoneNumber(string phone) 
        {
            bool isValid = true;
            if (!string.IsNullOrEmpty(phone))
            {
                if (!Regex.IsMatch(phone, regexPhonePattern))
                {
                    isValid = false;
                }
            }
            return isValid;
        }

        public static bool ValidateEmail(string email) 
        {
            bool isValid = true;
            if (!string.IsNullOrEmpty(email))
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
