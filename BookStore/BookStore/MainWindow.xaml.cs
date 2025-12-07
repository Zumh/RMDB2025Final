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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

        private void AddCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            bool validated = true;

            string customerName = CustomerNameTextBox.Text.Trim();
            string customerEmail = CustomerEmailTextBox.Text.Trim();
            string customerAddress = CustomerAddressTextBox.Text.Trim();
            string customerPhoneNumber = CustomerPhoneTextBox.Text.Trim();

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
                EmailErr.Content = "Field is Mandatory";
                CustomerAddressTextBox.Text = "";
            }
            if (!(validated = Customer.ValidatePhoneNumber(customerPhoneNumber)))
            {
                EmailErr.Content = "Format should be \"555-555-5555\"";
                CustomerEmailTextBox.Text = "";
            }

            if (validated) 
            { 
                //add customer to DB
            }
        }


    }
}