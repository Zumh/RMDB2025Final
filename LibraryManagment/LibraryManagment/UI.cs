
// This class will handle user interface operations for the Library Management System.
namespace LibraryManagment
{
    public class UI
    {

        public void UserOptions()
        {
            Console.WriteLine("Welcome to Library Management System");
            Console.WriteLine("1. Add Book" +
                "\n2. View Books" +
                "\n3. Add Member" +
                "\n4. Borrow Book" +
                "\n5. Return Book");
            Console.Write("Enter your choice: ");
        }
    }
}
