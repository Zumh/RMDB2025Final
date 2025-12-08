
using MySql.Data.MySqlClient;
using System.Data;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace BookStore
{
    public partial class MainWindow : Window
    {
        //database manager for sending queries
        DBManager? db = new DBManager();
        List<Category>? categories = null;
        public string? ConnectionString { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            InitializeCategories();
            LoadLogin();


        }



        private void InitializeCategories()
        {
            categories = new List<Category>
            {
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Non-Fiction" },
                new Category { Id = 2, Name = "Other" }
            };

            catComboBox.ItemsSource = categories;
            catComboBox.DisplayMemberPath = "Name";
            catComboBox.SelectedValuePath = "Id";
        }

        ////////////////////////////////////////        LOGIN FUNCTIONS      ///////////////////////////////////////////////////////////

      
     
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
                //add customer to customer list
                if (validated)
                {
                    Customer._customers.Add(new Customer
                    {
                        CustomerName = customerName,
                        Email = customerEmail,
                        Address = customerAddress,
                        Phone = customerPhoneNumber
                    });
                    ClearUIInput();
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
            //query to get all customers
            DataTable dataTable = db.DataBaseQuery("SELECT * FROM customer");
            
            if (dataTable == null) 
            {
                StatusText.Text = "No Customers in Database.";
            } 
            else
            {
                Customer.LoadCustomerData(dataTable);
                //add the list to the dataGrid
                CustomerList.ItemsSource = Customer._customers;
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

            //Successfully loads data ------ this is just for test purposes, need to implement actual search
            DataTable dataTable = db.DataBaseQuery("SELECT * FROM book");
            Book.LoadBookData(dataTable);
            BookList.ItemsSource = Book._books;
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
            string stock = BookStock.Text;
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
                BookStock.Text = "";
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
                if (validated)
                {
                    Book._books.Add(new Book
                    {
                        Title = title,
                        ISBN = numericISBN,
                        Price = numericPrice,
                        Stock = numericStock
                    });
                    StatusText.Text = "";
                    ClearUIInput();
                }
            }        
        }

        //////////////////////////////////          ORDER FUNCTIONS            ////////////////////////////////////////



        //////////////////////////////////          GENERAL UI FUNCTIONS            ////////////////////////////////////////
        
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
            BookStock.Text = "";
            CustomerNameTextBox.Text = "";
            CustomerEmailTextBox.Text = "";
            CustomerAddressTextBox.Text = "";
            CustomerPhoneTextBox.Text = "";
        }

        private void LoadLogin()
        {
            serverName.Text = "localhost";
            portNumber.Text = "3306";
            userId.Text = "root";
            loginPassword.Password = "student1";
            dataBaseName.Text = "BookStore";
        }


        private void LoginClick(object sender, RoutedEventArgs e)
        {

            string server = serverName.Text.Trim();
            string user = userId.Text.Trim();
            string password = loginPassword.Password;
            string database = dataBaseName.Text.Trim();
            string port = portNumber.Text.Trim();

            // Check for empty fields
            if (string.IsNullOrEmpty(server) ||
                string.IsNullOrEmpty(user) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(database) ||
                string.IsNullOrEmpty(port))
            {
                StatusText.Text = "All fields are required!";
                StatusText.Foreground = Brushes.Red; // Show error in red
                return;
            }

            // Temporary connection to server without database
            string connStr = $"server={server};port={port};uid={user};pwd={password};";

            try
            {
                using MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();

                // Check if database exists
                using MySqlCommand checkDbCmd = new MySqlCommand($"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{database}';", conn);
                object result = checkDbCmd.ExecuteScalar();

                if (result == null)
                {
                    // Database does not exist, ask user if they want to create it
                    MessageBoxResult answer = MessageBox.Show($"Database '{database}' does not exist. Do you want to create it?",
                                                 "Create Database", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (answer == MessageBoxResult.Yes)
                    {
                        using MySqlCommand createDbCmd = new MySqlCommand($"CREATE DATABASE `{database}`;", conn);
                        createDbCmd.ExecuteNonQuery();
                        StatusText.Text = $"Database '{database}' created successfully!";
                        StatusText.Foreground = Brushes.Green;
                    }
                    else
                    {
                        StatusText.Text = "Database creation canceled.";
                        StatusText.Foreground = Brushes.Red;
                        return;
                    }
                }
                else
                {
                    StatusText.Text = $"Database '{database}' exists. Connected successfully!";
                    StatusText.Foreground = Brushes.Green;
                }

                // Save connection string with database
                ConnectionString = $"server={server};port={port};uid={user};pwd={password};database={database};";

              
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Connection failed: {ex.Message}";
                StatusText.Foreground = Brushes.Red;
            }


        }

        //NAME: ClearLoginClick
        //DESCRIPTION: Clears the login fields of all text
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void ClearLoginClick(object sender, RoutedEventArgs e)
        {
            userId.Text = "";
            loginPassword.Clear();
            dataBaseName.Text = "";
            serverName.Text = "";
            portNumber.Text = "";
        }
    }
}