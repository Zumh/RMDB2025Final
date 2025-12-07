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
            NameErr.Content = "";
            EmailErr.Content = "";
            AddressErr.Content = "";
            PhoneErr.Content = "";

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

            //clear customer info
            CustomerNameTextBox.Text = "";
            CustomerEmailTextBox.Text = "";
            CustomerAddressTextBox.Text = "";
            CustomerPhoneTextBox.Text = "";

        }

        private void DisplayAllCustomers_Click(object sender, RoutedEventArgs e)
        {


            DataTable dataTable = db.GetDataTable("SELECT * FROM customer");

            List<Customer> customers = new List<Customer>();

            foreach (DataRow row in dataTable.Rows)
            {
                customers.Add(new Customer
                {
                    CustomerId = Convert.ToInt32(row["id"]),
                    CustomerName = row["name"].ToString(),
                    Address = row["address"].ToString(),
                    Email = row["email"].ToString(),
                    Phone = row["phonenumber"].ToString()
                });
            }

            CustomerList.ItemsSource = customers;
        }

    }
}