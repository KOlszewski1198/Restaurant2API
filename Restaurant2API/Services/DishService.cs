using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant2API.Entities;
using Restaurant2API.Exeptions;
using Restaurant2API.Models;

namespace Restaurant2API.Services
{
    public interface IDishService
    {
        IEnumerable<DishExtendedDto> GetAll();
        DishExtendedDto GetById(int id);
        IEnumerable<DishDto> GetAllFromRestaurant(int id);
        int Create(int id, CreateDishDto dto);
        void Delete(int DishId);
    }

    public class DishService :IDishService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public DishService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<DishExtendedDto> GetAll()
        {
            var dishes = _dbContext
                .Dishes
                .Include(r=>r.Restaurant)
                .ToList();

            var DishDto = _mapper.Map<List<DishExtendedDto>>(dishes);

            return DishDto;
        }

        public IEnumerable<DishDto> GetAllFromRestaurant(int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(r=>r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            if (!restaurant.Dishes.Any())
                throw new NotFoundException("Restaurant has no dishes");

            var DishDto = _mapper.Map<List<DishDto>>(restaurant.Dishes);

            return DishDto;
        }

        public DishExtendedDto GetById(int id)
        {
            var dishes = _dbContext
                .Dishes
                .Include(r => r.Restaurant)
                .FirstOrDefault(r => r.Id == id);

            if (dishes is null)
                throw new NotFoundException("Dish not found");

            var DishDto = _mapper.Map<DishExtendedDto>(dishes);
            return DishDto;
        }

        public int Create(int id, CreateDishDto dto)
        {
            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var dish = _mapper.Map<Dish>(dto);
            dish.RestaurantId = id;

            _dbContext.Dishes.Add(dish);
            _dbContext.SaveChanges();

            return dish.Id;
        }

        public void Delete(int DishId)
        {
            _logger.LogWarning($"Dish with id: {DishId} DELETE action invoked");

            var dishes = _dbContext
                .Dishes
                .FirstOrDefault(r => r.Id == DishId);

            if (dishes is null)
                throw new NotFoundException("Dish not found");

            _dbContext.Dishes.Remove(dishes);
            _dbContext.SaveChanges();

            return;
        }
    }
}
