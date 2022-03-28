using Microsoft.AspNetCore.Mvc;
using Restaurant2API.Models;
using Restaurant2API.Services;

namespace Restaurant2API.Controllers
{
    [Route("api/dish")]
    [ApiController] //walidacja poprawności przesyłanego body do mapowania
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishservice;
        public DishController(IDishService dishservice)
        {
            _dishservice = dishservice;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DishDto>> GetAll()
        {
            var dishes = _dishservice.GetAll();

            return Ok(dishes);
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<RestaurantDto>> Get([FromRoute] int id)
        {
            var dishes = _dishservice.GetById(id);

            return Ok(dishes);
        }

        [HttpGet("restaurant/{id}")]
        public ActionResult<IEnumerable<RestaurantDto>> GetAllFromRestaurant([FromRoute] int id)
        {
            var dishes = _dishservice.GetAllFromRestaurant(id);

            return Ok(dishes);
        }

        [HttpPost("{RestaurantId}")]
        public ActionResult CreateDish([FromRoute] int RestaurantId, [FromBody] CreateDishDto dto)
        {
            var id = _dishservice.Create(RestaurantId, dto);

            return Created($"/api/dish/{id}", null);
        }

        [HttpDelete("{DishId}")]
        public ActionResult DeleteDish([FromRoute] int DishId)
        {
            _dishservice.Delete(DishId);

            return NoContent();
        }
    }
}
