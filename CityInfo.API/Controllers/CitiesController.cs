using System.Collections.Generic;
using System.Linq;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCity(int id)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(cityDto => id == cityDto.Id);
            if (city is not null)
                return Ok(city);
            return NotFound();
        }
    }
}