//FILE : OrderRepository.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: 
This class manages orders in the database.
It handles creating orders, saving order details, and searching order history.
*/


using BookStore.Entities;
using System.Data;

namespace BookStore.DataAccess
{
    /*
    * Class: OrderRepository
    Purpose:  The OrderRepository class has been created to manage all database operations
    *  related to customer orders. This class is responsible for creating new orders
    *  and their associated order details, as well as retrieving, searching, and
    *  deleting order records. It acts as a bridge between the application’s business
    *  logic and the database by handling SQL queries, data loading, and object
    *  mapping. The OrderRepository class ensures that order information is stored,
    *  retrieved, and maintained in a consistent and reliable manner.
    */
    internal class OrderRepository
    {
        private DBManager db = new DBManager();

        //NAME: CreateOrder
        //DESCRIPTION: Creates a new order in the database, including the order details.
        //PARAMETERS: Order order
        //RETURN: void
        public void CreateOrder(Order order)
        {
            // 1. Insert Order
            // Schema: id, customerID, orderDate, totalAmount
            string orderQuery = $"INSERT INTO `order` (customerID, orderDate, totalAmount) VALUES ({order.CustomerID}, '{order.OrderDate}', {order.OrderAmount})";
            db.ExecuteNonQuery(orderQuery);

            // 2. Get the new Order ID
            string idQuery = "SELECT MAX(id) FROM `order`";
            DataTable idTable = db.DataBaseQuery(idQuery);
            int newOrderId = 0;
            if (idTable.Rows.Count > 0 && idTable.Rows[0][0] != DBNull.Value)
            {
                newOrderId = Convert.ToInt32(idTable.Rows[0][0]);
            }

            // 3. Insert Order Details
            foreach (var detail in order.OrderDetails)
            {
                string detailQuery = $"INSERT INTO orderdetail (orderID, bookID, quantity, unitPrice) VALUES ({newOrderId}, {detail.BookId}, {detail.Quantity}, {detail.Price})";
                db.ExecuteNonQuery(detailQuery);
            }
        }

        //NAME: SearchOrders
        //DESCRIPTION: Searches for orders based on ID, customer name, or book title.
        //PARAMETERS: string method, string value
        //RETURN: orders
        public List<Order> SearchOrders(string method, string value)
        {
            List<Order> orders = new List<Order>();
            string query = "";

            if (method == "Order ID")
            {
                // Simple search by ID
                if (int.TryParse(value, out int id))
                {
                    query = $"SELECT * FROM `order` WHERE id = {id}";
                }
            }
            else if (method == "Customer Name")
            {
                // Join with customer table to find orders by customer name
                query = $"SELECT o.* FROM `order` o JOIN customer c ON o.customerID = c.id WHERE c.name LIKE '%{value}%'";
            }
            else if (method == "Book Title")
            {
                // Join with orderdetail and book to find orders containing a book title
                query = $"SELECT DISTINCT o.* FROM `order` o JOIN orderdetail od ON o.id = od.orderID JOIN book b ON od.bookID = b.id WHERE b.title LIKE '%{value}%'";
            }
            else 
            {
                // Default fetch all if no search 
                query = "SELECT * FROM `order`";
            }

            if (!string.IsNullOrEmpty(query))
            {
                DataTable table = db.DataBaseQuery(query);
                foreach (DataRow row in table.Rows)
                {
                    orders.Add(new Order
                    {
                        Id = Convert.ToInt32(row["id"]),
                        CustomerID = Convert.ToInt32(row["customerID"]),
                        OrderDate = row["orderDate"].ToString(),
                        OrderAmount = Convert.ToSingle(row["totalAmount"])
                    });
                }
            }
            return orders;
        }

        //NAME: GetOrderDetails
        //DESCRIPTION: Gets the details of a specific order.
        //PARAMETERS: int orderId
        //RETURN: details
        public List<OrderDetail> GetOrderDetails(int orderId)
        {
            List<OrderDetail> details = new List<OrderDetail>();
            
            string query = $"SELECT * FROM orderdetail WHERE orderID = {orderId}";
            DataTable table = db.DataBaseQuery(query);
            foreach (DataRow row in table.Rows)
            {
                details.Add(new OrderDetail
                {
                   OrderId = Convert.ToInt32(row["orderID"]),
                   BookId = Convert.ToInt32(row["bookID"]),
                   Quantity = Convert.ToInt32(row["quantity"]),
                   Price = Convert.ToDecimal(row["unitPrice"])
                });
            }
            return details;
        }

        //NAME: GetOrderHistory
        //DESCRIPTION: Gets the full history of orders with customer and book names.
        //PARAMETERS: string method, string value
        //RETURN: history
        public List<OrderDetail> GetOrderHistory(string method, string value)
        {
            List<OrderDetail> history = new List<OrderDetail>();
            string query = "SELECT o.id, c.name as CustomerName, b.title as BookTitle, od.bookID, od.quantity, od.unitPrice, o.totalAmount, o.orderDate " +
                           "FROM `order` o " +
                           "JOIN customer c ON o.customerID = c.id " +
                           "JOIN orderdetail od ON o.id = od.orderID " +
                           "JOIN book b ON od.bookID = b.id";

            if (!string.IsNullOrEmpty(value))
            {
                if (method == "Order ID")
                {
                     if (int.TryParse(value, out int id))
                     {
                         query += $" WHERE o.id = {id}";
                     }
                }
                else if (method == "Customer Name")
                {
                     query += $" WHERE c.name LIKE '%{value}%'";
                }
                else if (method == "Book Title")
                {
                     query += $" WHERE b.title LIKE '%{value}%'";
                }
            }
            
            // Order by ID descending
            query += " ORDER BY o.id DESC";

            DataTable table = db.DataBaseQuery(query);
            foreach (DataRow row in table.Rows)
            {
                history.Add(new OrderDetail
                {
                    OrderId = Convert.ToInt32(row["id"]),
                    BookId = Convert.ToInt32(row["bookID"]),
                    CustomerName = row["CustomerName"].ToString(),
                    BookTitle = row["BookTitle"].ToString(),
                    Quantity = Convert.ToInt32(row["quantity"]),
                    Price = Convert.ToDecimal(row["unitPrice"]),
                    TotalAmount = Convert.ToSingle(row["totalAmount"]),
                    OrderDate = row["orderDate"].ToString()
                });
            }
            return history;
        }

        //NAME: DeleteOrder
        //DESCRIPTION: Deletes an order and its details from the database.
        //PARAMETERS: int orderId
        //RETURN: void
        public void DeleteOrder(int orderId)
        {
            // Delete details first due to foreign key constraints
            string detailQuery = $"DELETE FROM orderdetail WHERE orderID = {orderId}";
            db.ExecuteNonQuery(detailQuery);

            // Delete order
            string orderQuery = $"DELETE FROM `order` WHERE id = {orderId}";
            db.ExecuteNonQuery(orderQuery);
        }
    }
}
