//FILE : MainWindow.xaml.cs
//PROJECT : PROG2111 Final Project
//PROGRAMMER : Zumhliansang Lung Ler | Sungmin Leem | Nick Turco
//FIRST VERSION : 03/12/2025
/*DESCRIPTION: 
This is the main window of the application where users interact with the system.
It handles navigation between tabs and user events like clicking buttons to add or search items.
*/

using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


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
        CategoryRepository _categoryRepo = new CategoryRepository();
    
        public MainWindow()
        {
            InitializeComponent();
            // LoadData(); // Don't load until login
        }


        ////////////////////////////////////////        LOGIN FUNCTIONS      ///////////////////////////////////////////////////////////

        //NAME: LoginBtn_Click
        //DESCRIPTION: Updates the connection string and tries to load data.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void LoginBtn_Click(object? sender, RoutedEventArgs? e)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login Failed: " + ex.Message);
            }
        }

        //NAME: ClearLogin_Click
        //DESCRIPTION: Clears the login text boxes.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void ClearLogin_Click(object? sender, RoutedEventArgs? e)
        {
            userId.Text = "";
            loginPassword.Text = "";
            dataBaseName.Text = "";
        }

        //////////////////////////////             CUSTOMER FUNCTIONS             //////////////////////////////////////////////////////

        //NAME: AddCustomerBtn_Click
        //DESCRIPTION: Validates input and adds a new customer to the database.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void AddCustomerBtn_Click(object? sender, RoutedEventArgs? e)
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
                         RefreshOrderCustomerGrid();
                    }
                    catch (Exception ex)
                    {
                        StatusText.Text = "Error adding customer: " + ex.Message;
                    }
                }
            }
        }

        //NAME: DisplayAllCustomers_Click
        //DESCRIPTION: Shows all customers in the list.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void DisplayAllCustomers_Click(object? sender, RoutedEventArgs? e)
        {
            RefreshCustomerList();
        }

        //NAME: RefreshCustomerList
        //DESCRIPTION: Reloads the customer list from the database.
        //PARAMETERS: None
        //RETURN: void
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
        //DESCRIPTION: Makes the ID column read-only in the data grid.
        //PARAMETERS: object sender, DataGridAutoGeneratingColumnEventArgs e
        //RETURN: void
        private void CustomerList_Columns(object? sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //check if column name is id and make it read only
            if(e.PropertyName == "CustomerId")
            {
                e.Column.IsReadOnly = true;
            }
        }

        //NAME: RemoveCustomer_Click
        //DESCRIPTION: Removes the selected customer from the database.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void RemoveCustomer_Click(object? sender, RoutedEventArgs? e)
        {
            if (CustomerList.SelectedItem is Customer selectedCustomer)
            {
                if (MessageBox.Show($"Are you sure you want to delete {selectedCustomer.CustomerName}?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _customerRepo.Delete(selectedCustomer.CustomerId);
                        StatusText.Text = "Customer Deleted Successfully.";
                        RefreshCustomerList();
                        RefreshOrderCustomerGrid();
                        ClearUIInput();
                    }
                    catch (Exception ex)
                    {
                        StatusText.Text = "Error deleting customer: " + ex.Message;
                    }
                }
            }
            else
            {
                 MessageBox.Show("Please select a customer to remove.");
            }
        }

        //NAME: SearchForCustomer_Click
        //DESCRIPTION: Searches for customers based on the criteria entered.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void SearchForCustomer_Click(object? sender, RoutedEventArgs? e)
        {
            string customerName = CustomerNameTextBox.Text;
            string customerEmail = CustomerEmailTextBox.Text;
            string customerAddress = CustomerAddressTextBox.Text;
            string customerPhoneNumber = CustomerPhoneTextBox.Text;

            StatusText.Text = "";
            
            try
            {
                // Use new Search method with AND logic
                List<Customer> list = _customerRepo.Search(customerName, customerEmail, customerAddress, customerPhoneNumber);
                
                if (list.Count == 0)
                {
                    StatusText.Text = "No customers found matching the criteria.";
                    CustomerList.ItemsSource = null;
                }
                else
                {
                    CustomerList.ItemsSource = list;
                    StatusText.Text = $"{list.Count} customer(s) found.";
                }
            }
            catch (Exception ex)
            {
                 StatusText.Text = "Error searching customers: " + ex.Message;
            }
        }

        //////////////////////////////             BOOK FUNCTIONS             //////////////////////////////////////////////////////

        //NAME: RemoveBook_Click
        //DESCRIPTION: Removes the selected book from the database.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void RemoveBook_Click(object? sender, RoutedEventArgs? e)
        {
            if (BookList.SelectedItem is Book selectedBook)
            {
                if (MessageBox.Show($"Are you sure you want to delete {selectedBook.Title}?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _bookRepo.Delete(selectedBook.BookID);
                        StatusText.Text = "Book Deleted Successfully.";
                        RefreshBookList();
                        RefreshOrderBookGrid();
                        ClearUIInput();
                    }
                    catch (Exception ex)
                    {
                        StatusText.Text = "Error deleting book: " + ex.Message;
                    }
                }
            }
            else
            {
                 MessageBox.Show("Please select a book to remove.");
            }
        }

        //NAME: SearchBook_Click
        //DESCRIPTION: Searches for books based on the criteria entered.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void SearchBook_Click(object? sender, RoutedEventArgs? e)
        {
            string title = BookTitletextBox.Text;
            string isbn = IsbnTextBox.Text;
            string price = BookPriceTextBox.Text;
            string author = BookAuthorTextBox.Text;
            string publisher = BookPublisherTextBox.Text;
            string stock = BookStockTextBox.Text;
            int catId = -1;
            
            if (BookCategoryComboBox.SelectedValue != null)
            {
                catId = (int)BookCategoryComboBox.SelectedValue;
            }

            try
            {
                // Updated Search call with new parameters
                List<Book> books = _bookRepo.Search(title, author, isbn, price, catId, publisher, stock);
                Book._books = books;
                BookList.ItemsSource = books;
                
                if (books.Count == 0)
                    StatusText.Text = "No books found matching criteria.";
                else
                    StatusText.Text = $"{books.Count} book(s) found.";
            }
            catch (Exception ex)
            {
                StatusText.Text = "Error searching books: " + ex.Message;
            }
        }

        //NAME: DisplayAllBooks_Click
        //DESCRIPTION: Reloads the book list from the database.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void DisplayAllBooks_Click(object? sender, RoutedEventArgs? e)
        {
            RefreshBookList();
        }

        //NAME: RefreshBookList
        //DESCRIPTION: Reloads the book list from the database.
        //PARAMETERS: None
        //RETURN: void
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

        //NAME: RefreshCategoryList
        //DESCRIPTION: Reloads the category list from the database.
        //PARAMETERS: None
        //RETURN: void
        private void RefreshCategoryList()
        {
            try
            {
                List<Category> categories = _categoryRepo.GetAll();
                BookCategoryComboBox.ItemsSource = categories;
                // If we want to default select the first one?
                // if (categories.Count > 0)
                //    BookCategoryComboBox.SelectedIndex = 0;
                 BookCategoryComboBox.SelectedIndex = -1;  // Request: Start with nothing selected
            }
            catch (Exception ex)
            {
                StatusText.Text = "Error loading categories: " + ex.Message;
            }
        }

        //NAME: BookList_Columns
        //DESCRIPTION: Makes ID columns read-only in the book grid.
        //PARAMETERS: object sender, DataGridAutoGeneratingColumnEventArgs e
        //RETURN: void
        private void BookList_Columns(object? sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //check if column name is id and make it read only
            if (e.PropertyName == "BookID")
            {
                e.Column.IsReadOnly = true;
            }
            if(e.PropertyName == "PublisherID")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }
            if (e.PropertyName == "CategoryID")
            {
                 e.Column.Visibility = Visibility.Collapsed;
            }
            if (e.PropertyName == "PublisherName")
            {
                e.Column.Header = "Publisher";
                e.Column.IsReadOnly = true;
            }
            if (e.PropertyName == "CategoryName")
            {
                e.Column.Header = "Category";
                e.Column.IsReadOnly = true;
            }
        }

        //NAME: AddBook_Click
        //DESCRIPTION: Validates input and adds a new book to the database.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void AddBook_Click(object? sender, RoutedEventArgs? e)
        {
            bool validated = true;

            string title = BookTitletextBox.Text;
            string isbn = IsbnTextBox.Text;
            string publisher = BookPublisherTextBox.Text;
            string price = BookPriceTextBox.Text;
            string stock = BookStockTextBox.Text;
            string author = BookAuthorTextBox.Text;
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
                
                // Get or create publisher by name
                string publisherName = BookPublisherTextBox.Text;
                int pubId = _bookRepo.GetOrCreatePublisherId(publisherName);
                int catId = 1;
                
                if (BookCategoryComboBox.SelectedValue != null)
                {
                    catId = (int)BookCategoryComboBox.SelectedValue;
                }
                else
                {
                    // Force selection or default? Let's force selection validation if we want strictness, 
                    // but for now relying on default 1 if not selected is safer to avoid crashes, 
                    // however, let's alert user if strictly needed. 
                    // The user asked to "Category Selection", so they probably want it to work.
                    if (BookCategoryComboBox.Items.Count > 0)
                    {
                        if (int.TryParse(BookCategoryComboBox.SelectedValue?.ToString(), out int selectedCatId))
                        {
                            catId = selectedCatId;
                        }

                      
                    }
                    if (BookCategoryComboBox.SelectedIndex == -1)
                     {
                         StatusText.Text = "Please select a Category.";
                         return;
                     }
                     if (BookCategoryComboBox.SelectedValue != null)
                     {
                         catId = (int)BookCategoryComboBox.SelectedValue;
                     }
                }

                if (validated)
                {
                    var newBook = new Book
                    {
                        Title = title,
                        ISBN = isbn, // Note: Model has String ISBN but here usage tried double? Model says string.
                        Price = (decimal)numericPrice, // Model says decimal
                        Stock = numericStock,
                        PublisherID = pubId,
                        CategoryID = catId,
                        Author = author
                    };

                    try
                    {
                        _bookRepo.Add(newBook);
                        Book._books.Add(newBook);
                        StatusText.Text = "Book Added Successfully.";
                        ClearUIInput();
                        RefreshBookList();
                        RefreshOrderBookGrid();
                    }
                    catch (Exception ex)
                    {
                         StatusText.Text = "Error adding book: " + ex.Message;
                    }
                }
            }        
        }

        //////////////////////////////////          ORDER FUNCTIONS            ////////////////////////////////////////

        // Cart State
        public class CartItem
        {
            public int BookId { get; set; }
            public string BookTitle { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public decimal TotalPrice => Price * Quantity;
        }

        private List<CartItem> _cartItems = new List<CartItem>();
        private Customer? _selectedOrderCustomer = null;

        //NAME: LoadData
        //DESCRIPTION: Loads initial data from the database.
        //PARAMETERS: None.
        //RETURN: void
        private void LoadData()
        {
            try 
            {
                // Seed categories if needed
                _categoryRepo.SeedCategories();

                // Refresh background lists but DO NOT populate GridSources initially
                RefreshCustomerList();
                RefreshBookList();
                RefreshCategoryList();
                
                // Populate Order Tab Grids - LEFT EMPTY as requested
                OrderCustomerGrid.ItemsSource = _customerRepo.GetAll(); 
                OrderBookGrid.ItemsSource = _bookRepo.GetAll();
                
                RefreshHistory_Click(new object(), new RoutedEventArgs());
            }
            catch (Exception)
            {
                // StatusText might be null if called too early
            }
        }

        //NAME: RefreshOrderBookGrid
        //DESCRIPTION: Refreshes the book list in the Order tab.
        //PARAMETERS: None
        //RETURN: void
        private void RefreshOrderBookGrid()
        {
            try
            {
                OrderBookGrid.ItemsSource = _bookRepo.GetAll();
            }
            catch (Exception ex)
            {
                StatusText.Text = "Error refreshing order book grid: " + ex.Message;
            }
        }

        //NAME: RefreshOrderCustomerGrid
        //DESCRIPTION: Refreshes the customer list in the Order tab.
        //PARAMETERS: None
        //RETURN: void
        private void RefreshOrderCustomerGrid()
        {
            try
            {
                OrderCustomerGrid.ItemsSource = _customerRepo.GetAll();
            }
            catch (Exception ex)
            {
                StatusText.Text = "Error refreshing order customer grid: " + ex.Message;
            }
        }

        // --- Customer Section ---

        //NAME: OrderCustomerSearchBox_TextChanged
        //DESCRIPTION: Filters the customer list in the order tab as the user types.
        //PARAMETERS: object sender, TextChangedEventArgs e
        //RETURN: void
        private void OrderCustomerSearchBox_TextChanged(object? sender, TextChangedEventArgs? e)
        {
             string filter = OrderCustomerSearchBox.Text.ToLower();
             if (string.IsNullOrWhiteSpace(filter))
             {
                 OrderCustomerGrid.ItemsSource = null; // Clear if empty
             }
             else
             {
                List<Customer> all = _customerRepo.GetAll();
                IEnumerable<Customer> filtered = all.Where(c =>
                    (c.CustomerName ?? string.Empty).ToLower().Contains(filter) ||
                    (c.Phone ?? string.Empty).Contains(filter)
                    ).ToList();

                OrderCustomerGrid.ItemsSource = filtered;
             }
        }

        //NAME: OrderCustomerGrid_SelectionChanged
        //DESCRIPTION: Selects a customer for the order when clicked in the grid.
        //PARAMETERS: object sender, SelectionChangedEventArgs e
        //RETURN: void
        private void OrderCustomerGrid_SelectionChanged(object? sender, SelectionChangedEventArgs? e)
        {
            if (OrderCustomerGrid.SelectedItem is Customer c)
            {
                _selectedOrderCustomer = c;
            }
        }

        // --- Book Section ---

        //NAME: OrderBookSearchBox_TextChanged
        //DESCRIPTION: Filters the book list in the order tab as the user types.
        //PARAMETERS: object sender, TextChangedEventArgs e
        //RETURN: void
        private void OrderBookSearchBox_TextChanged(object? sender, TextChangedEventArgs? e)
        {
             string filter = OrderBookSearchBox.Text.ToLower();
             if (string.IsNullOrWhiteSpace(filter))
             {
                 OrderBookGrid.ItemsSource = null; // Clear if empty
             }
             else
             {
                 var all = _bookRepo.GetAll();
                 var filtered = all
                      .Where(b => (b.Title != null && b.Title.ToLower().Contains(filter)) || (b.ISBN != null && b.ISBN.Contains(filter)))
                     .Select(b => new BookSelectionViewModel 
                     {
                         BookID = b.BookID,
                         Title = b.Title,
                         ISBN = b.ISBN,
                         Price = b.Price,
                         Author = b.Author,
                         PublisherID = b.PublisherID,
                         Stock = b.Stock,
                         QuantityToBuy = 1 // Default
                     })
                     .ToList();
                 OrderBookGrid.ItemsSource = filtered;
             }
        }

        //NAME: OrderBookGrid_MouseDoubleClick
        //DESCRIPTION: Adds a book to the cart when double clicked.
        //PARAMETERS: object sender, MouseButtonEventArgs e
        //RETURN: void
        private void OrderBookGrid_MouseDoubleClick(object? sender, MouseButtonEventArgs? e)
        {
            BookSelectionViewModel? currentBook = null;
            if (OrderBookGrid.SelectedItem is Book selectedBook)
            {
                currentBook = new BookSelectionViewModel
                {
                    BookID = selectedBook.BookID,
                    Title = selectedBook.Title,
                    ISBN = selectedBook.ISBN,
                    Price = selectedBook.Price,
                    PublisherID = selectedBook.PublisherID,
                    Author = selectedBook.Author,
                    Stock = selectedBook.Stock,
                    QuantityToBuy = 1   // default
                };
            }

            if (currentBook != null)
            {
                int qtyToAdd = currentBook.QuantityToBuy;
                
                if (qtyToAdd <= 0) 
                {
                     MessageBox.Show("Please select a quantity greater than 0.");
                     return;
                }

                var existing = _cartItems.FirstOrDefault(i => i.BookId == currentBook.BookID);
                if (existing != null)
                {
                    if (existing.Quantity + qtyToAdd <= currentBook.Stock)
                    {
                        existing.Quantity += qtyToAdd;
                    }
                    else
                    {
                        MessageBox.Show($"Cannot add more. Stock limit ({currentBook.Stock}) reached.");
                    }
                }
                else
                {
                    if (currentBook.Stock >= qtyToAdd)
                    {
                        _cartItems.Add(new CartItem 
                        { 
                            BookId = currentBook.BookID, 
                            BookTitle = currentBook.Title ?? "Unknown", 
                            Quantity = qtyToAdd, 
                            Price = currentBook.Price 
                        });
                    }
                    else
                    {
                         MessageBox.Show("Not enough Stock!");
                    }
                }
                RefreshCart();
            }
        }

        // --- Cart Section ---

        //NAME: RefreshCart
        //DESCRIPTION: Updates the shopping cart display.
        //PARAMETERS: None
        //RETURN: void
        private void RefreshCart()
        {
            ShoppingCartGrid.ItemsSource = null;
            ShoppingCartGrid.ItemsSource = _cartItems;
            
            CartTotalItemsText.Text = _cartItems.Sum(i => i.Quantity).ToString();
            CartTotalPriceText.Text = _cartItems.Sum(i => i.TotalPrice).ToString("C");
        }

        //NAME: ClearCart_Click
        //DESCRIPTION: Removes all items from the shopping cart.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void ClearCart_Click(object? sender, RoutedEventArgs? e)
        {
            _cartItems.Clear();
            RefreshCart();
        }

        //NAME: RemoveOrder_Click
        //DESCRIPTION: Deletes a selected order from history.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void RemoveOrder_Click(object? sender, RoutedEventArgs? e)
        {
             if (OrderHistoryGrid.SelectedItem is OrderDetail selected)
             {
                 if (MessageBox.Show($"Are you sure you want to delete Order #{selected.OrderId}?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                 {
                     try
                     {
                        _orderRepo.DeleteOrder(selected.OrderId);
                         StatusText.Text = $"Order #{selected.OrderId} deleted.";
                         RefreshHistory_Click(null, null);
                     }
                     catch(Exception ex)
                     {
                         StatusText.Text = "Error deleting order: " + ex.Message;
                     }
                 }
             }
             else
             {
                 MessageBox.Show("Please select an order to remove.");
             }
        }

        //NAME: SubmitOrder_Click
        //DESCRIPTION: Creates a new order with the items in the cart.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void SubmitOrder_Click(object? sender, RoutedEventArgs? e)
        {
            if (_selectedOrderCustomer == null)
            {
                MessageBox.Show("Please select a customer from the list first.");
                return;
            }

            if (_cartItems.Count == 0)
            {
                MessageBox.Show("Cart is empty.");
                return;
            }

            try
            {
                decimal total = _cartItems.Sum(i => i.TotalPrice);

                // Create Order
                Order newOrder = new Order
                {
                    CustomerID = _selectedOrderCustomer?.CustomerId ?? 0,
                    OrderDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 
                    OrderAmount = (float)total,
                    OrderDetails = new List<OrderDetail>()
                };

                // Create Details
                foreach(var item in _cartItems)
                {
                    newOrder.OrderDetails.Add(new OrderDetail
                    {
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    });
                }

                _orderRepo.CreateOrder(newOrder);
                
                MessageBox.Show("Order Placed Successfully!");
                _cartItems.Clear();
                RefreshCart();
                RefreshHistory_Click(new object(), new RoutedEventArgs()); // Refresh history
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error keying order: " + ex.Message);
            }
        }

        // --- Order History Section ---

        //NAME: RefreshHistory_Click
        //DESCRIPTION: Reloads the order history list.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void RefreshHistory_Click(object? sender, RoutedEventArgs? e)
        {
            try
            {
                 // Default show all
                 string method = ""; 
                 string value = "";
                 
                 // If searching
                 if (sender == null || sender is Button) // Called manually or by button
                 {
                      if (HistorySearchMethodCombo.SelectedItem is ComboBoxItem item)
                      {
                            method = item.Content?.ToString() ?? "";
                           value = HistorySearchValueBox.Text;
                      }
                 }

                 OrderHistoryGrid.ItemsSource = _orderRepo.GetOrderHistory(method, value);
            }
            catch(Exception ex)
            {
                MessageBox.Show("History Error: " + ex.Message);
            }
        }

        //NAME: HistorySearchButton_Click
        //DESCRIPTION: Filters the order history based on search criteria.
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void HistorySearchButton_Click(object sender, RoutedEventArgs e)
        {
             RefreshHistory_Click(sender, e);
        }

        //NAME: HistorySearch_KeyDown
        //DESCRIPTION: Triggers search when Enter key is pressed in history search box.
        //PARAMETERS: object sender, KeyEventArgs e
        //RETURN: void
        private void HistorySearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RefreshHistory_Click(sender, e);
            }
        }

        //NAME: ClearUIInput
        //DESCRIPTION: Clears all input fields in the UI.
        //PARAMETERS: None.
        //RETURN: void
        public void ClearUIInput()
        {
            BookTitletextBox.Text = "";
            IsbnTextBox.Text = "";
            BookPublisherTextBox.Text = "";
            BookPriceTextBox.Text = "";
            BookStockTextBox.Text = "";
            BookAuthorTextBox.Text = "";
            BookCategoryComboBox.SelectedIndex = -1;
            CustomerNameTextBox.Text = "";
            CustomerEmailTextBox.Text = "";
            CustomerAddressTextBox.Text = "";
            CustomerPhoneTextBox.Text = "";
        }

        //NAME: OrderBookGrid_SelectionChanged
        //DESCRIPTION: Handles selection change in order book grid.
        //PARAMETERS: object sender, SelectionChangedEventArgs e
        //RETURN: void
        private void OrderBookGrid_SelectionChanged(object? sender, SelectionChangedEventArgs? e)
        {

        }

        //NAME: OrderHistoryGrid_SelectionChanged
        //DESCRIPTION: Handles selection change in order history grid.
        //PARAMETERS: object sender, SelectionChangedEventArgs e
        //RETURN: void
        private void OrderHistoryGrid_SelectionChanged(object? sender, SelectionChangedEventArgs? e)
        {

        }
    }
}