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
    }
}