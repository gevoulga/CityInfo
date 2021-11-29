using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId:int}/[controller]")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _repository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService,
            ICityInfoRepository repository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var pois = _repository.GetPointsOfInterestForCity(cityId);
            if (pois is null)
            {
                _logger.LogInformation(
                    $"PointsOfInterest for cityId {cityId} were not found.");
                return NotFound();
            }

            var pointsOfInterestDto = _mapper.Map<IEnumerable<PointOfInterestDto>>(pois);
            return Ok(pointsOfInterestDto);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var poi = _repository.GetPointOfInterestForCity(cityId, id);
            if (poi is null)
            {
                _logger.LogInformation(
                    $"PointOfInterest with cityId {cityId} and pointOfInterestId {id} was not found.");
                return NotFound();
            }

            var pointOfInterestDto = _mapper.Map<PointOfInterestDto>(poi);
            return Ok(pointOfInterestDto);
        }

        [HttpPost]
        public IActionResult CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
        {
            // if (pointOfInterest.Description == pointOfInterest.Name)
            // {
            //     ModelState.AddModelError(
            //         "Description", 
            //         "The provided description should be different from the name.");
            //     return BadRequest(ModelState);
            // }

            var city = _repository.GetCity(cityId);
            if (city is null)
                return NotFound();

            var poi = _mapper.Map<PointOfInterest>(pointOfInterest);
            _repository.AddPointOfInterestForCity(cityId, poi);
            var savedPoi = _mapper.Map<PointOfInterestDto>(poi);

            //TODO: why the hell is this not working?
            return Created("/this/is/shit", savedPoi);
            // return CreatedAtRoute(
            //     nameof(GetPointOfInterest),
            //     new {cityId, id = poi.Id},
            //     poi);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, PointOfInterestForCreationDto pointOfInterest)
        {
            var city = _repository.GetCity(cityId);
            if (city is null)
                return NotFound();
            var poi = _repository.GetPointOfInterestForCity(cityId, id);
            if (poi is null)
                return NotFound();

            //Automapper will do the update!
            _mapper.Map(pointOfInterest, poi);
            _repository.UpdatePointOfInterestForCity(cityId, poi);

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            JsonPatchDocument<PointOfInterestForCreationDto> patchDoc)
        {
            var city = _repository.GetCity(cityId);
            if (city is null)
                return NotFound();
            var poi = _repository.GetPointOfInterestForCity(cityId, id);
            if (poi is null)
                return NotFound();

            //Apply the partial update
            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForCreationDto>(poi);
            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!TryValidateModel(pointOfInterestToPatch))
                return BadRequest();

            //Automapper will do the partial update update!
            _mapper.Map(pointOfInterestToPatch, poi);
            _repository.UpdatePointOfInterestForCity(cityId, poi);

            return NoContent();
        }

        [HttpDelete("int:id")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = _repository.GetCity(cityId);
            if (city is null)
                return NotFound();
            var poi = _repository.GetPointOfInterestForCity(cityId,id);
            if (poi is null)
                return NotFound();

            city.PointsOfInterest.Remove(poi);
            _repository.DeletePointOfInterest(poi);
            _mailService.Send("Point of Service Deleted",
                $"Point of interest {poi} has been deleted.");
            return NoContent();
        }
    }
}