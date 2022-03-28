using Microsoft.AspNetCore.Mvc;

namespace Restaurant2API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastService _service;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service)
        {
            _logger = logger;
            _service = new WeatherForecastService();
        }
        public class Range
        {
            public int min { get; set; }
            public int max { get; set; }
        }


        [HttpGet]                //https://localhost:7104/WeatherForecast
        public IEnumerable<WeatherForecast> Get([FromQuery] int num = 4, [FromQuery] int min = -100, [FromQuery] int max=100)
        {
            var result = _service.Get(num,min,max);
            return result;
        }

        [HttpGet]
        [Route("2")]            //https://localhost:7104/WeatherForecast/2
        public IEnumerable<WeatherForecast> Get2()
        {
            var result = _service.Get2();
            return result;
        }

        [HttpGet("number/{max}")] // https://localhost:7104/WeatherForecast/number/2?take=40
        public IEnumerable<WeatherForecast> Get3([FromQuery]int take, [FromRoute]int max)
        {
            var result = _service.Get3();
            return result;

        }

        [HttpPost]
        public ActionResult<string> Welcome([FromBody]string str)
        {
            //HttpContext.Response.StatusCode = 401;
            //return $"Welcome {str}";

            return StatusCode(401, $"Welcome {str}");
            //return Unauthorized($"Welcome {str}");
        }

        [HttpPost("generate")]
        public ActionResult<string> Get4([FromBody] Range range, [FromQuery] int num = 4)
        {
            if (num <= 0 || range.min > range.max)
            {
                return StatusCode(400);
            }
            else
            {
                var result = _service.Get4(num, range.min, range.max);
                return StatusCode(200, result);
            }
        }
    }
}