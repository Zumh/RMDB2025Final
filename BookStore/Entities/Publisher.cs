//FILE : Publisher.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: This is the Customer class that manages the customer objects created when the database information is retrieved.
 *             It contains all the functions necessary to validate new customers, existing customers and load datatables with data.
*/

namespace BookStore
{
    public class Publisher
    {
        public int PublisherID { get; set; }
        public string? Name { get; set; }

        //NAME: ValidateName
        //DESCRIPTION: Validates the publisher name is not blank
        //PARAMETERS: string publisherName - name of publisher
        //RETURN: bool isValid - true if name is valid otherwise false
        public static bool ValidateName(string publisherName)
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(publisherName.Trim()))
            {
                isValid = false;
            }
            return isValid;
        }

    }
}
