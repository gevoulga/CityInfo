using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId:int}/[controller]")]
    public class PointsOfInterestBulkController : ControllerBase
    {
        private readonly ICityInfoRepository _repository;
        private readonly IMapper _mapper;

        public PointsOfInterestBulkController(ICityInfoRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<IActionResult> BulkInsert(int cityId,
            IEnumerable<PointOfInterestForCreationDto> pointsOfInterest)
        {
            var pois = _mapper.Map<IEnumerable<PointOfInterest>>(pointsOfInterest);
            await _repository.AddPointsOfInterestForCity(cityId, pois.ToArray());
            
            return Accepted();
        }
    }
}