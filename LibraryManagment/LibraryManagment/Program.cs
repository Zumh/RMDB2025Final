
namespace LibraryManagment
{
    internal class Program
    {


        static void Main(string[] args)
        {
            UI ui = new UI();
            // user option method
            while (true)
            {

                ui.UserOptions();
                Console.ReadLine();
            }
        }



    }
}