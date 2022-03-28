using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Restaurant2API.Authorization;
using Restaurant2API.Entities;
using Restaurant2API.Exeptions;
using Restaurant2API.Models;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Restaurant2API.Services
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDto dto);
        PagedResult<RestaurantDto> GetAll(RestaurantSearchQuery query);
        RestaurantDto GetById(int id);
        void Delete(int id);
        void Change(int id, UpdateRestaurantDto dto);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger,
            IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public void Delete(int id)
        {
            _logger.LogWarning($"Restaurant with id: {id} DELETE action invoked");

            var restaurants = _dbContext
                .Restaurants
                .FirstOrDefault(r=>r.Id == id);

            if (restaurants is null)
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurants,
                 new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _dbContext.Restaurants.Remove(restaurants);
            _dbContext.SaveChanges();

            return;
        }

        public RestaurantDto GetById(int id)
        {
            var restaurants = _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if (restaurants is null)
                throw new NotFoundException("Restaurant not found");

            var RestaurantDto = _mapper.Map<RestaurantDto>(restaurants);
            return RestaurantDto;
        }

        public PagedResult<RestaurantDto> GetAll(RestaurantSearchQuery query)
        {
            var baseQuery = _dbContext
               .Restaurants
               .Include(r => r.Address)
               .Include(r => r.Dishes)
               .Where(r => query.SearchPhrase == null ||
                            (r.Name.ToLower().Contains(query.SearchPhrase.ToLower()) || r.Category.ToLower().Contains(query.SearchPhrase.ToLower())));

            if (query.PageSize == null)
            {
                query.PageSize = 15;
            }
            if (query.PageNumber == null || query.PageNumber == 0)
            {
                query.PageNumber = 1;
            }

            if(!string.IsNullOrEmpty(query.SortBy))
            {
                var collumnsSelectors = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    {nameof(Restaurant.Name), r=>r.Name },
                    {nameof(Restaurant.Description),r=>r.Description },
                    {nameof(Restaurant.Category),r=>r.Category }
                };

                var selectedColumn = collumnsSelectors[query.SortBy];

                baseQuery = query.SortDirection ==SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var restaurants = baseQuery
                .Skip((int) (query.PageSize * (query.PageNumber - 1)))
                .Take((int) query.PageSize)
                .ToList();
               

            

            var totalItemsCount = baseQuery.Count();

            var RestaurantDto = _mapper.Map<List<RestaurantDto>>(restaurants);

            var result = new PagedResult<RestaurantDto>(RestaurantDto, totalItemsCount,(int) query.PageSize, (int)query.PageNumber);

            return result;
        }

        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContextService.GetUserId;
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }

        public void Change(int id, UpdateRestaurantDto dto)
        {

            var restaurants = _dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == id);

            if (restaurants is null) 
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurants,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if(!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            restaurants.Name = dto.Name;
            restaurants.Category = dto.Category;
            restaurants.HasDelivery = dto.HasDelivery;

            _dbContext.SaveChanges();

            return;
        }
    }
}
