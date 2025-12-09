

namespace BookStore
{
    internal class Category
    {
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }

        //set category options
        enum CategoryType
        {
            Fiction,
            NonFiction,
            Other
        }

        //NAME: ValidateCategoryName
        //DESCRIPTION: Validates the category name is not blank
        //PARAMETERS: string categoryName - category of book
        //RETURN: bool isValid - true if category is valid otherwise false
        public static bool ValidateCategoryName (string categoryName)
        {
            bool isValid = true;
            if(string.IsNullOrEmpty(categoryName.Trim())) 
            { 
                isValid = false; 
            }
            return isValid;
        }
    }
}
