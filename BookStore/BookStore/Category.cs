
using System.Collections.ObjectModel;


namespace BookStore
{
    public class Category
    {
        private int id;
        private string? categoryName;

        // Default constructor
        public Category()
        {
            Id = 0;               
            CategoryName = "Other"; 
        }

       
        public Category(int id, string categoryName)
        {
            Id = id;
            CategoryName = categoryName; 
        }

  
        public int Id
        {
            get { return id; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Id cannot be negative.");
                }
                id = value;
            }
        }

   
        public string? CategoryName
        {
            get { return categoryName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Category name cannot be empty.");
                }
                categoryName = value;
            }
        }

       
    }

}
