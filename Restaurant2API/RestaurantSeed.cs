using Restaurant2API.Entities;

namespace Restaurant2API
{
    public class RestaurantSeed
    {
        private readonly RestaurantDbContext _dbContext;

        public RestaurantSeed(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description = 2,
                    ContactEmail = "contact@kfc.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Strips",
                            Price = 10.30M,
                        },
                        new Dish()
                        {
                            Name = "Zinger",
                            Price = 12.99M,
                        },
                    },
                    Address = new Address()
                    {
                        City = "Kraków",
                        Street = "Długa 5",
                        PostalCode = "30-221",
                    },

                },

                new Restaurant()
                {
                    Name = "Alibaba Kabab",
                    Category = "Fast Food",
                    Description = 2,
                    ContactEmail = "abibaba@kebab.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Baranian na grubym",
                            Price = 13.30M,
                        },
                        new Dish()
                        {
                            Name = "Falafel",
                            Price = 15.00M,
                        },
                    },
                    Address = new Address()
                    {
                        City = "Warszawa",
                        Street = "Świętokrzyska 24",
                        PostalCode = "04-384",
                    },

                },
            };

            return restaurants;
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                 new Role()
                {
                    Name = "Admin"
                },
            };

            return roles;
        }
    }
}
