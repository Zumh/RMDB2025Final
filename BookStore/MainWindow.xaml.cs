using MySqlX.XDevAPI.Relational;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using BookStore.DataAccess;
using BookStore.Entities;

namespace BookStore
{
    public partial class MainWindow : Window
    {
        // Repositories
        CustomerRepository _customerRepo = new CustomerRepository();
        BookRepository _bookRepo = new BookRepository();
        OrderRepository _orderRepo = new OrderRepository();
    
        public MainWindow()
        {
            InitializeComponent();
            // LoadData(); // Don't load until login
        }


        ////////////////////////////////////////        LOGIN FUNCTIONS      ///////////////////////////////////////////////////////////

        //NAME: ClearLogin_Click
        //DESCRIPTION: Clears the login fields of all text
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        //NAME: LoginBtn_Click
        //DESCRIPTION: Updates connection string and loads data
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string user = userId.Text;
            string pass = loginPassword.Text; // Note: In real app use PasswordBox
            string dbName = dataBaseName.Text;

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(dbName))
            {
                MessageBox.Show("Please enter User ID and Database Name.");
                return;
            }

            // Construct connection string
            // Assuming localhost/port 3306 for now based on previous hardcode
            string newConnString = $"Server=localhost;Port=3306;Uid={user};Pwd={pass};Database={dbName};";
            
            DBManager.connectionString = newConnString; // Update static connection string

            try
            {
                // Test connection by trying to load data
                LoadData();
                MessageBox.Show("Connected and Data Loaded!");

                // DEBUG: Probe Schema
                try {
                    var db = new BookStore.DBManager();
                    string debugInfo = "Schema Debug:\n";
                    
                    try {
                        DataTable t1 = db.DataBaseQuery("DESCRIBE `order`");
                        debugInfo += "ORDER TABLE:\n";
                        foreach(DataRow r in t1.Rows) debugInfo += r["Field"] + "\n";
                    } catch(Exception ex1) { debugInfo += "Order Table Error: " + ex1.Message + "\n"; }

                    try {
                        DataTable t2 = db.DataBaseQuery("DESCRIBE `orderdetail`");
                        debugInfo += "\nORDERDETAIL TABLE:\n";
                        foreach(DataRow r in t2.Rows) debugInfo += r["Field"] + "\n";
                    } catch(Exception ex2) { debugInfo += "OrderDetail Table Error: " + ex2.Message + "\n"; }

                    // Write to a known location
                    string path = @"d:\cone\Second grade 1st semester\Relational Databases2111(queries)\assginment\TeamP\RMDB2025Final\BookStore\schema_info.txt";
                    System.IO.File.WriteAllText(path, debugInfo);
                    MessageBox.Show("Schema details saved to schema_info.txt");
                } catch (Exception exProbe) { MessageBox.Show("Probe failed: " + exProbe.Message); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login Failed: " + ex.Message);
            }
        }

        private void ClearLogin_Click(object sender, RoutedEventArgs e)
        {
            userId.Text = "";
            loginPassword.Text = "";
            dataBaseName.Text = "";
        }
        //NAME: AddCustomerBtn_Click
        //DESCRIPTION: Validates the customer input
        //             Adds the customer to the dataTable
        //             Handles input errors and UI clean up
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void

        //////////////////////////////             CUSTOMER FUNCTIONS             //////////////////////////////////////////////////////

        private void AddCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            bool validated = true;

            string customerName = CustomerNameTextBox.Text;
            string customerEmail = CustomerEmailTextBox.Text;
            string customerAddress = CustomerAddressTextBox.Text;
            string customerPhoneNumber = CustomerPhoneTextBox.Text;
            //clear error messages
            StatusText.Text = "";

            //validate each user input
            if (!(validated = Customer.ValidateName(customerName)))
            {
                StatusText.Text = "Field is Mandatory";
                CustomerNameTextBox.Text = "";
            }
            else if (!(validated = Customer.ValidateEmail(customerEmail)))
            {
                StatusText.Text = "Format should be \"Example@email.com\"";
                CustomerEmailTextBox.Text = "";
            }
            else if (!(validated = Customer.ValidateAddress(customerAddress)))
            {
                StatusText.Text = "Field is Mandatory";
                CustomerAddressTextBox.Text = "";
            }
            else if (!(validated = Customer.ValidatePhoneNumber(customerPhoneNumber)))
            {
                StatusText.Text = "Format should be \"555-555-5555\"";
                CustomerEmailTextBox.Text = "";
            }
            else
            {
                //add customer to customer list AND database
                if (validated)
                {
                    var newCustomer = new Customer
                    {
                        CustomerName = customerName,
                        Email = customerEmail,
                        Address = customerAddress,
                        Phone = customerPhoneNumber
                    };
                    
                    try 
                    {
                         _customerRepo.Add(newCustomer);
                         Customer._customers.Add(newCustomer); // Keep local list in sync or reload
                         StatusText.Text = "Customer Added Successfully.";
                         ClearUIInput();
                         RefreshCustomerList();
                    }
                    catch (Exception ex)
                    {
                        StatusText.Text = "Error adding customer: " + ex.Message;
                    }
                }
            }
        }

        //NAME: DisplayAllCustomers_Click
        //DESCRIPTION: Creates a datatable that contains all customers in the database        
        //             Handles no data returns and user feedback
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void DisplayAllCustomers_Click(object sender, RoutedEventArgs e)
        {
            RefreshCustomerList();
        }

        private void RefreshCustomerList()
        {
            try
            {
                List<Customer> customers = _customerRepo.GetAll();
                Customer._customers = customers; // Update local static list
                
                if (customers.Count == 0) 
                {
                    StatusText.Text = "No Customers in Database.";
                    CustomerList.ItemsSource = null;
                } 
                else
                {
                    CustomerList.ItemsSource = customers;
                }
            }
            catch(Exception ex)
            {
                StatusText.Text = "Error loading customers: " + ex.Message;
            }
        }

        //NAME: CustomerList_Columns
        //DESCRIPTION: Sets the first column to read only so users cannot edit primary key        
        //PARAMETERS: object sender, DataGridAutoGeneratingColumnEventArgs e
        //RETURN: void
        private void CustomerList_Columns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //check if column name is id and make it read only
            if(e.PropertyName == "CustomerId")
            {
                e.Column.IsReadOnly = true;
            }
        }
        //NAME: SearchForCustomer_Click
        //DESCRIPTION: Searches for customer based on name        
        //PARAMETERS: object sender, DataGridAutoGeneratingColumnEventArgs e
        //RETURN: void
        private void SearchForCustomer_Click(object sender, RoutedEventArgs e)
        {
            string customerName = CustomerNameTextBox.Text;
            string customerEmail = CustomerEmailTextBox.Text;
            string customerAddress = CustomerAddressTextBox.Text;
            string customerPhoneNumber = CustomerPhoneTextBox.Text;

            StatusText.Text = "";
            //need to add more options for searching
            List<Customer> list = new List<Customer>();

            if (!string.IsNullOrEmpty(customerName)) 
            {
                if (Customer._customers.Count == 0)
                {
                    StatusText.Text = "Customer DataSet not found.";
                } 
                else
                {
                    list = Customer.SearchByName(customerName);
                    StatusText.Text = "";
                }
            }
            if(list.Count == 0)
            {
                StatusText.Text = "Customer Name not found.";
            } else
            {
                CustomerList.ItemsSource = list;
            }
            
        }

        //////////////////////////////             BOOK FUNCTIONS             //////////////////////////////////////////////////////

        private void SearchBook_Click(object sender, RoutedEventArgs e)
        {
            RefreshBookList();
        }

        private void RefreshBookList()
        {
            try
            {
                List<Book> books = _bookRepo.GetAll();
                Book._books = books;
                BookList.ItemsSource = books;
            }
             catch(Exception ex)
            {
                StatusText.Text = "Error loading books: " + ex.Message;
            }
        }

        //NAME: BookList_Columns
        //DESCRIPTION: Sets bookID, publisher ID, and categoryID to readonly        
        //PARAMETERS: object sender, DataGridAutoGeneratingColumnEventArgs e
        //RETURN: void
        private void BookList_Columns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //check if column name is id and make it read only
            if (e.PropertyName == "BookID")
            {
                e.Column.IsReadOnly = true;
            }
            if(e.PropertyName == "PublisherID")
            {
                e.Column.IsReadOnly = true;
            }
            if (e.PropertyName == "CategoryID")
            {
                e.Column.IsReadOnly = true;
            }
        }

        //NAME: AddBook_Click
        //DESCRIPTION: Validates user input for a new book
        //             Adds a new book object to the _book list        
        //PARAMETERS: object sender, DataGridAutoGeneratingColumnEventArgs e
        //RETURN: void
        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            bool validated = true;

            string title = BookTitletextBox.Text;
            string isbn = IsbnTextBox.Text;
            string publisher = BookPublisherTextBox.Text;
            string price = BookPriceTextBox.Text;
            string stock = BookStockTextBox.Text;
            //clear error messages
            StatusText.Text = "";

            //validate each user input
            if (!(validated = Book.ValidateBookTitle(title)))
            {
                StatusText.Text = "Name is Mandatory";
                BookTitletextBox.Text = "";
            }
            else if (!(validated = Book.ValidateIBSN(isbn)))
            {
                StatusText.Text = "ISBN is Mandatory and must be 13 digits long, ex.(1234567890123).";
                IsbnTextBox.Text = "";
            }
            else if (!(validated = Book.ValidatePrice(price)))
            {
                StatusText.Text = "Price is Mandatory and must be numeric";
                BookPriceTextBox.Text = "";
            }
            else if (!(validated = Book.ValidateStock(stock)))
            {
                StatusText.Text = "Stock is Mandatory and must be numeric";
                BookStockTextBox.Text = "";
            } 
            else
            {
                double numericISBN;
                float numericPrice;
                int numericStock;
                //parse strings into numbers
                double.TryParse(isbn, out numericISBN);
                float.TryParse(price, out numericPrice);
                int.TryParse(stock, out numericStock);
                
                // TODO: Handle Publisher properly (find ID by name or insert new publisher)
                // For now we will just use a default ID or try to parse if user entered ID 
                // Since UI is text box, let's assume for prototype we need to fix this later or assume ID 1.
                int pubId = 1; 
                int catId = 1;

                if (validated)
                {
                    var newBook = new Book
                    {
                        Title = title,
                        ISBN = isbn, // Note: Model has String ISBN but here usage tried double? Model says string.
                        Price = (decimal)numericPrice, // Model says decimal
                        Stock = numericStock,
                        PublisherID = pubId,
                        CategoryID = catId
                    };

                    try
                    {
                        _bookRepo.Add(newBook);
                        Book._books.Add(newBook);
                        StatusText.Text = "Book Added Successfully.";
                        ClearUIInput();
                        RefreshBookList();
                    }
                    catch (Exception ex)
                    {
                         StatusText.Text = "Error adding book: " + ex.Message;
                    }
                }
            }        
        }

        //////////////////////////////////          ORDER FUNCTIONS            ////////////////////////////////////////



        // Order State
        private List<OrderDetail> _currentOrderDetails = new List<OrderDetail>();
        private List<Book> _currentOrderBooksDisplay = new List<Book>(); // For display in ListBox

        private void LoadData()
        {
            try 
            {
                RefreshCustomerList();
                RefreshBookList();
                
                // Populate Order Tab Lists
                OrderCustomerComboBox.ItemsSource = _customerRepo.GetAll();
                OrderBookList.ItemsSource = _bookRepo.GetAll();
                
                InitializeSearch();
            }
            catch (Exception ex)
            {
                // StatusText might be null if called too early, but InitializeComponent is done.
            }
        }

        private void OrderBookList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderBookList.SelectedItem is Book selectedBook)
            {
                SelectedBookTitle.Text = selectedBook.Title;
                SelectedBookISBN.Text = selectedBook.ISBN;
                SelectedBookPrice.Text = selectedBook.Price.ToString("C");
                SelectedBookStock.Text = selectedBook.Stock.ToString();
                SelectedBookPublisher.Text = selectedBook.PublisherID.ToString(); // Ideally fetch Publisher Name
            }
        }

        private void AddToOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrderBookList.SelectedItem is Book selectedBook)
            {
                int quantity = 1;
                int.TryParse(OrderQuantityTextBox.Text, out quantity);

                if (quantity <= 0) 
                {
                     MessageBox.Show("Quantity must be greater than 0");
                     return;
                }
                
                if (quantity > selectedBook.Stock)
                {
                     MessageBox.Show("Not enough stock!");
                     return;
                }

                // Add to Current Order
                _currentOrderDetails.Add(new OrderDetail 
                {
                    BookId = selectedBook.BookID,
                    Quantity = quantity,
                    Price = selectedBook.Price
                });

                // Add to Display list (using Book object for now, or a wrapper)
                // For simplicity, just adding the book again to display list or similar
                _currentOrderBooksDisplay.Add(selectedBook);
                
                UpdateOrderSummary();
            }
            else
            {
                MessageBox.Show("Please select a book.");
            }
        }

        private void RemoveFromOrder_Click(object sender, RoutedEventArgs e)
        {
             if (CurrentOrderList.SelectedIndex >= 0)
             {
                 int index = CurrentOrderList.SelectedIndex;
                 _currentOrderDetails.RemoveAt(index);
                 _currentOrderBooksDisplay.RemoveAt(index);
                 UpdateOrderSummary();
             }
        }

        private void UpdateOrderSummary()
        {
            CurrentOrderList.ItemsSource = null;
            CurrentOrderList.ItemsSource = _currentOrderBooksDisplay; // Shows titles

            decimal total = 0;
            int count = 0;
            for(int i=0; i< _currentOrderDetails.Count; i++)
            {
                total += _currentOrderDetails[i].Price * _currentOrderDetails[i].Quantity;
                count += _currentOrderDetails[i].Quantity;
            }
            
            TotalPriceText.Text = total.ToString("C");
            TotalItemsText.Text = count.ToString();
        }

        private void SubmitOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrderCustomerComboBox.SelectedValue == null)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }

            if (_currentOrderDetails.Count == 0)
            {
                 MessageBox.Show("Order is empty.");
                 return;
            }

            try
            {
                int customerId = (int)OrderCustomerComboBox.SelectedValue;
                
                // Calculate total again or use computed
                decimal total = 0;
                foreach(var d in _currentOrderDetails) total += d.Price * d.Quantity;

                Order newOrder = new Order
                {
                    CustomerID = customerId,
                    OrderDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    OrderAmount = (float)total,
                    OrderDetails = _currentOrderDetails
                };

                _orderRepo.CreateOrder(newOrder);
                
                MessageBox.Show("Order Placed Successfully!");
                
                // Reset UI
                _currentOrderDetails.Clear();
                _currentOrderBooksDisplay.Clear();
                UpdateOrderSummary();
                OrderQuantityTextBox.Text = "1";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error submitting order: " + ex.Message);
            }
        }
        
        //NAME: ClearUIInput
        //DESCRIPTION: Clears all UI inputs        
        //PARAMETERS: none
        //RETURN: void
        public void ClearUIInput()
        {
            BookTitletextBox.Text = "";
            IsbnTextBox.Text = "";
            BookPublisherTextBox.Text = "";
            BookPriceTextBox.Text = "";
            BookStockTextBox.Text = "";
            CustomerNameTextBox.Text = "";
            CustomerEmailTextBox.Text = "";
            CustomerAddressTextBox.Text = "";
            CustomerPhoneTextBox.Text = "";
        }

        //////////////////////////////////          SEARCH LOGIC            ////////////////////////////////////////

        private void InitializeSearch()
        {
           SearchMethodComboBox.Items.Add("Order ID");
           SearchMethodComboBox.Items.Add("Customer Name");
           SearchMethodComboBox.Items.Add("Book Title");
           SearchMethodComboBox.SelectedIndex = 0;
        }

        private void SearchValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string method = SearchMethodComboBox.SelectedItem?.ToString();
                string value = SearchValueTextBox.Text;

                if (string.IsNullOrEmpty(method) || string.IsNullOrEmpty(value)) return;

                try
                {
                    List<Order> results = _orderRepo.SearchOrders(method, value);
                    SearchResultsListBox.ItemsSource = results;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Search Error: " + ex.Message);
                }
            }
        }

        private void SearchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchResultsListBox.SelectedItem is Order selectedOrder)
            {
                // Fetch details
                try
                {
                    List<OrderDetail> details = _orderRepo.GetOrderDetails(selectedOrder.Id);
                    
                    // Enhancement needed: OrderDetail only has BookID. We want to see Book Title.
                    // Quick fix: Fetch all books and lookup title (cache it) or extend OrderDetail.
                    // Or since we already have _bookRepo, maybe just map it here.
                    
                    List<string> displayDetails = new List<string>();
                    foreach (var d in details)
                    {
                        // Find book title from local cache if possible, or fetch. 
                        // _bookRepo.GetAll() might be expensive to call every time. 
                        // Simplest: `Book._books` is public static and loaded. Use that!
                        var book = Book._books.FirstOrDefault(b => b.BookID == d.BookId);
                        string title = book != null ? book.Title : "Unknown Book";
                        displayDetails.Add($"{title} (Qty: {d.Quantity}) - {d.Price:C}");
                    }
                    OrderDetailsListBox.ItemsSource = displayDetails;
                }
                catch (Exception ex)
                {
                     MessageBox.Show("Error loading details: " + ex.Message);
                }
            }
        }
    }
}