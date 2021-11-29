using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _repository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cityEntities = _repository.GetCities();
            var cityDtos = _mapper.Map<IEnumerable<CityDto>>(cityEntities);
            return Ok(cityDtos);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCity(int id, bool includePoi = false)
        {
            var city = _repository.GetCity(id, includePoi);
            if (city is null)
                return NotFound();

            if (includePoi)
            {
                var cityDto = _mapper.Map<CityDto>(city);
                return Ok(cityDto);
            }

            var cityNoPoiDto = _mapper.Map<CityNoPoiDto>(city);
            return Ok(cityNoPoiDto);
        }
    }
}