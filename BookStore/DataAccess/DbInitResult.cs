

namespace BookStore
{
    public class DbInitResult
    {
        public bool Success { get; set; }
        public bool DatabaseExists { get; set; }
        public bool NeedsCreation { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ConnectionString { get; set; }
    }
}
