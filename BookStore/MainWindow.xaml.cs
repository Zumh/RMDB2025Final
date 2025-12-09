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
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace BookStore
{
    public partial class MainWindow : Window
    {
        //database manager for sending queries
        DBManager db = new DBManager();
    
        public MainWindow()
        {
            InitializeComponent();
           
        }
        ////////////////////////////////////////        LOGIN FUNCTIONS      ///////////////////////////////////////////////////////////

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
           string connectionString = DBManager.BuildConnectionString(userId.Text, loginPassword.Text, dataBaseName.Text);
           DBManager.connectionString = connectionString;
        }

        //NAME: ClearLogin_Click
        //DESCRIPTION: Clears the login fields of all text
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void ClearLogin_Click(object sender, RoutedEventArgs e)
        {
            userId.Text = "";
            loginPassword.Text = "";
            dataBaseName.Text = "";
        }

        //////////////////////////////             CUSTOMER FUNCTIONS             //////////////////////////////////////////////////////
        

        //NAME: AddCustomerBtn_Click
        //DESCRIPTION: Validates the customer input
        //             Adds the customer to the dataTable
        //             Handles input errors and UI clean up
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void AddCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            bool validated = true;
            int failedCRUD = -1;

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
                 int result = DBManager.DataBaseCRUD("INSERT INTO customer (name, email, address, phoneNumber)" +
                        $" VALUES\r\n('{customerName}', '{customerEmail}', '{customerAddress}', '{customerPhoneNumber}')");
                    if (result == failedCRUD) 
                    {
                        StatusText.Text = "Failed to add Customer to Database.";
                    }
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
            BookStockTextBox.Text = "";
            CustomerNameTextBox.Text = "";
            CustomerEmailTextBox.Text = "";
            CustomerAddressTextBox.Text = "";
            CustomerPhoneTextBox.Text = "";
        }

    }
}