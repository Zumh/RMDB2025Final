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


    /*
     * Class: Book
     * Purpose: The Book class has been created to accurately model a single book record
    *  within the bookstore system. The Book class contains members to track key
    *  information such as the book ID, publisher, category, title, ISBN, author,
    *  price, and stock quantity. The Book class also provides validation methods
    *  to ensure that book data entered by the user is correct before being stored
    *  or processed. In addition, the Book class supports loading book data from
    *  a database into an in-memory collection for easy access by other parts of
    *  the application.
    */

    internal class Book
    {
        public int BookID { get; set; }
        public int PublisherID { get; set; }
        public int CategoryID { get; set; }
        public string? Title { get; set; }
        public string? PublisherName { get; set; }
        public string? CategoryName { get; set; }
        public string? ISBN { get; set; }
        public string? Author { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        static public List<Book> _books = new List<Book>();

        //NAME: ValidateBookTitle
        //DESCRIPTION: Checks if the book title is empty.
        //PARAMETERS: string bookName
        //RETURN: isValid
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
        //DESCRIPTION: Checks if the ISBN is 13 numbers long and not negative.
        //PARAMETERS: string ibsn
        //RETURN: isValid
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
        //DESCRIPTION: Checks if the price is a valid number and not negative.
        //PARAMETERS: string price
        //RETURN: isValid
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
        //DESCRIPTION: Checks if the stock amount is a number and not negative.
        //PARAMETERS: string stock
        //RETURN: isValid
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
        //DESCRIPTION: Gets book data from the database and puts it into a list.
        //PARAMETERS: DataTable data
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
                    ISBN = row["isbn"].ToString(),
                    Price = Convert.ToDecimal(row["price"]),
                    Stock = Convert.ToInt32(row["stock"])
                });
            }
        }
    }
}
