

namespace BookStore
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public enum CategoryType
        {
            Fiction,
            NonFiction,
            Other
        }

        // Instance method instead of static
        public bool ValidateCategoryName(string currentCatName)
        {
            return !string.IsNullOrWhiteSpace(currentCatName);
        }

        // Instance-based category type resolver
        public int GetCategoryType(string currentCatName)
        {
            return Name?.Trim().ToLower() switch
            {
                "fiction" => (int) CategoryType.Fiction,
                "nonfiction" => (int) CategoryType.NonFiction,
                _ => (int) CategoryType.Other
            };
        }
    }


}
