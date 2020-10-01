using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Data.Context;
using Data.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ApiCoreContext context;
        public WeatherController(ApiCoreContext context)
        {
            this.context = context;
        }

        [HttpGet("all")] //localhost/api/weather/all
        public IEnumerable<Weather> Get() => context.Weathers.ToList();

        [HttpGet("info")] //localhost/api/weather/info?name=Погода%20в%20Барнауле&date=16%20сен
        public string Get(string name, string date) => context.Find(name, date);

        [HttpGet("citylist")] //localhost/api/weather/citylist
        
        public string GetCityList() => context.GetCityList();

        [HttpGet("datelist")] //localhost/api/weather/datelist
        public string GetDateList() => context.GetDateList();
    }
}
