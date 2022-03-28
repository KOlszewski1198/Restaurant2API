using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant2API.Entities;
using Restaurant2API.Models;
using Restaurant2API.Services;
using System.Security.Claims;

namespace Restaurant2API.Controllers
{
    [Route("api/restaurant")]
    [ApiController] //walidacja poprawności przesyłanego body do mapowania
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var id = _restaurantService.Create(dto);

            return Created($"/api/restaurant/{id}",null);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin,Manager", Policy = "HasNationality")]
        //[Authorize(Policy = "Atleast20")]
       // [Authorize(Policy = "MultiOwnerRequirement")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll([FromQuery] RestaurantSearchQuery? searchCondidion)
        {
            var restaurant =  _restaurantService.GetAll(searchCondidion);

            return Ok(restaurant);
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<RestaurantDto>> Get([FromRoute] int id)
        {
            var restaurant = _restaurantService.GetById(id);

            return Ok(restaurant);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _restaurantService.Delete(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Change([FromRoute] int id, [FromBody] UpdateRestaurantDto dto)
        {
             _restaurantService.Change(id, dto);

            return Ok();
        }
    }
}
