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
        //NAME: AddCustomerBtn_Click
        //DESCRIPTION: Validates the customer input
        //             Adds the customer to the dataTable
        //             Handles input errors and UI clean up
        //PARAMETERS: object sender, RoutedEventArgs e
        //RETURN: void
        private void AddCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            bool validated = true;

            string customerName = CustomerNameTextBox.Text;
            string customerEmail = CustomerEmailTextBox.Text;
            string customerAddress = CustomerAddressTextBox.Text;
            string customerPhoneNumber = CustomerPhoneTextBox.Text;
            //clear error messages
            ClearErrorCodes();

            //validate each user input
            if (!(validated = Customer.ValidateName(customerName)))
            {
                NameErr.Content = "Field is Mandatory";
                CustomerNameTextBox.Text = "";
            }
            if (!(validated = Customer.ValidateEmail(customerEmail)))
            {
                EmailErr.Content = "Format should be \"Example@email.com\"";
                CustomerEmailTextBox.Text = "";
            }
            if (!(validated = Customer.ValidateAddress(customerAddress)))
            {
                AddressErr.Content = "Field is Mandatory";
                CustomerAddressTextBox.Text = "";
            }
            if (!(validated = Customer.ValidatePhoneNumber(customerPhoneNumber)))
            {
                PhoneErr.Content = "Format should be \"555-555-5555\"";
                CustomerEmailTextBox.Text = "";
            }

            if (validated)
            {
                //add customer to DB, need to the data adapter 
            }
            ClearCustomerInfo();

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

        private void SearchForCustomer_Click(object sender, RoutedEventArgs e)
        {
            string customerName = CustomerNameTextBox.Text;
            string customerEmail = CustomerEmailTextBox.Text;
            string customerAddress = CustomerAddressTextBox.Text;
            string customerPhoneNumber = CustomerPhoneTextBox.Text;
           
            ClearErrorCodes();  

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
        //NAME: ClearErrorCodes
        //DESCRIPTION: Clears all error codes       
        //PARAMETERS: none
        //RETURN: void
        public void ClearErrorCodes()
        {
            NameErr.Content = "";
            EmailErr.Content = "";
            AddressErr.Content = "";
            PhoneErr.Content = "";
            StatusText.Text = "";
        }
        //NAME: ClearCustomerInfo
        //DESCRIPTION: Clears all Customer Info       
        //PARAMETERS: none
        //RETURN: void
        public void ClearCustomerInfo()
        {
            CustomerNameTextBox.Text = "";
            CustomerEmailTextBox.Text = "";
            CustomerAddressTextBox.Text = "";
            CustomerPhoneTextBox.Text = "";
        }
    }
}