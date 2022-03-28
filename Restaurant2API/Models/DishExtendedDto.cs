namespace Restaurant2API.Models
{
    public class DishExtendedDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
    }
}
