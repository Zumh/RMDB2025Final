//FILE : Book.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: This is the Book class that manages the book objects created when the database information is retrieved.
 *             It contains all the functions necessary to validate new books, existing books and load datatables with data.
*/
using System.Data;

namespace BookStore
{
    internal class Book
    {
        public int BookID { get; set; }
        public int PublisherID { get; set; }
        public int CategoryID { get; set; }
        public string? Title { get; set; }
        public double ISBN { get; set; }
        public float Price { get; set; }
        public int Stock { get; set; }

        static public List<Book> _books = new List<Book>();

        //NAME: ValidateBookTitle
        //DESCRIPTION: Validates the title is not blank
        //PARAMETERS: string author - author of the book
        //RETURN: bool isValid - true if author is valid otherwise false
        public static bool ValidateBookTitle (string bookName)
        {
            bool isValid = true;
            if(string.IsNullOrEmpty(bookName.Trim()))
            {
                isValid = false;
            }
            return isValid;
        }

        //NAME: ValidateIBSN
        //DESCRIPTION: Validates the IBSN is not blank, an int type, and not negative
        //PARAMETERS: string ibsn - IBSN number of the book
        //RETURN: bool isValid - true if IBSN is valid otherwise false
        public static bool ValidateIBSN(string ibsn)
        {
            int ibsnLength = 13;
            bool isValid = true;
            double tempISBN = 0;
            if (string.IsNullOrEmpty(ibsn.Trim()))
            {
                isValid = false;
            }
            //check it its a number
            if (!double.TryParse(ibsn, out tempISBN))
            {
                isValid = false;
            }
            //check for the right length
            int numberOfIntegers = ibsn.Count();
            if (numberOfIntegers != ibsnLength)
            {
                isValid = false;
            }
            //check if negative
            if (double.IsNegative(tempISBN))
            {
                isValid = false;
            }

            return isValid;
        }
        //NAME: ValidatePrice
        //DESCRIPTION: Validates the price is not blank, a float type, and not negative
        //PARAMETERS: string price - price value of book
        //RETURN: bool isValid - true if price is valid otherwise false
        public static bool ValidatePrice(string price)
        {
            bool isValid = true;
            float tempPrice = 0;
            if (string.IsNullOrEmpty(price.Trim()))
            {
                isValid = false;
            }
            if (!float.TryParse(price, out tempPrice))
            {
                isValid = false;
            }

            if (float.IsNegative(tempPrice))
            {
                isValid = false;
            }

            return isValid;
        }
        //NAME: ValidateStock
        //DESCRIPTION: Validates the stock is not blank, an int type, and not negative
        //PARAMETERS: string stock - stock value of books
        //RETURN: bool isValid - true if stock is valid otherwise false
        public static bool ValidateStock(string stock)
        {
            bool isValid = true;
            int tempStock = 0;
            //check it its a number
            if (string.IsNullOrEmpty(stock.Trim())) 
            {
                isValid = false;
            }
            if (!int.TryParse(stock, out tempStock))
            {
                isValid = false;
            }
            //check if negative
            if (float.IsNegative(tempStock))
            {
                isValid = false;
            }

            return isValid;
        }

        //NAME: LoadBookData
        //DESCRIPTION: Adds the books from the database to the books List
        //PARAMETERS: DataTable data - customer data from database
        //RETURN: void
        public static void LoadBookData(DataTable data)
        {
            //create a new customer object for each customer in database
            foreach (DataRow row in data.Rows)
            {
                _books.Add(new Book
                {
                    BookID = Convert.ToInt32(row["id"]),
                    PublisherID = Convert.ToInt32(row["publisherID"]),
                    CategoryID = Convert.ToInt32(row["categoryID"]),
                    Title = row["title"].ToString(),
                    ISBN = Convert.ToInt64(row["isbn"]),
                    Price = Convert.ToInt32(row["price"]),
                    Stock = Convert.ToInt32(row["stock"])
                });
            }
        }
    }
}
