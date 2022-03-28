namespace Restaurant2API.Models
{
    public class RestaurantSearchQuery
    {
        public  string? SearchPhrase { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? SortBy { get; set; }
        public SortDirection? SortDirection { get; set; }

    }
}
