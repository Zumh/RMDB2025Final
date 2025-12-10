//FILE : Order.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: This is the Order class that manages the order objects created when the database information is retrieved.
 *             It contains all the functions necessary to validate new orders, existing orders and load datatables with data.
*/

using System.Data;

using System.Text.RegularExpressions;


namespace BookStore
{
    /*
* Class: Order
* Purpose: The Order class has been created to represent a customer order within
 *  the bookstore system. It contains members to track the order ID, customer
 *  ID, order date, total amount, and a list of associated order details.
 *  The class provides validation methods to ensure that the order date and
 *  amount are correctly formatted before storing or processing. It also
 *  supports loading orders from the database into an in-memory collection,
 *  allowing easy access and management of order records within the application.
 */
    internal class Order
    {
        //regex for mySQL date format
        static readonly string dateRegex = @"^(19|20)\\d{2}-(0[1-9]|1[0-2])-(0[1-9]|[12]\\d|3[01])$";

        public int Id { get; set; }
        public int CustomerID { get; set; }
        public string? OrderDate { get; set; }
        public float OrderAmount { get; set; }
        public List<Entities.OrderDetail> OrderDetails { get; set; } = new List<Entities.OrderDetail>();

        public override string ToString()
        {
            return $"Order #{Id} - {OrderDate} ({OrderAmount:C})";
        }

        static public List<Order> _orders = new List<Order>();

        //NAME: ValidateOrderDate
        //DESCRIPTION: Checks if the order date format is correct.
        //PARAMETERS: string orderDate
        //RETURN: isValid
        public static bool ValidateOrderDate(string orderDate)
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(orderDate.Trim()))
            {
                isValid = false;
            }
            if (!Regex.IsMatch(orderDate, dateRegex)) 
            { 
                isValid = false;
            }
            return isValid;
        }

        //NAME: ValidateOrderAmount
        //DESCRIPTION: Checks if the order amount is a valid number.
        //PARAMETERS: string orderAmount
        //RETURN: isValid
        public static bool ValidateOrderAmount(string orderAmount)
        {
            bool isValid = true;
            float amount = 0;
            if (string.IsNullOrEmpty(orderAmount.Trim()))
            {
                isValid = false;
            }
            if(!float.TryParse(orderAmount, out amount)) {
                isValid = false;
            }
            return isValid;
        }

        //NAME: LoadOrders
        //DESCRIPTION: Loads orders from the database into a list.
        //PARAMETERS: DataTable data
        //RETURN: void
        public static void LoadOrders(DataTable data)
        {
            
            foreach (DataRow row in data.Rows) 
            {
                _orders.Add(new Order
                {
                    Id = Convert.ToInt32(row["ID"]),
                    CustomerID = Convert.ToInt32(row["customerID"]),
                    OrderDate = row["orderdate"].ToString(),
                    OrderAmount = Convert.ToInt32(row["orderamount"])
                });
            }
        }
    }
}
