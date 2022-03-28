using System.ComponentModel.DataAnnotations;

namespace Restaurant2API.Models
{
    public class UpdateRestaurantDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }

    }
}
