using System;
using System.Collections;
using System.Linq;
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

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        }


        [HttpGet("{id:int}")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(dto => dto.Id == cityId);
            if (city is null)
            {
                _logger.LogInformation($"City with id {cityId} was not found.");
                return NotFound();
            }

            var pointOfInterest = city.PointsOfInterest
                .FirstOrDefault(dto => dto.Id == id);
            if (pointOfInterest is null)
                return NotFound();

            return Ok(pointOfInterest);
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

            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(dto => dto.Id == cityId);
            if (city is null)
                return NotFound();

            //TODO: replace with id
            var maxPoiId = CitiesDataStore.Current.Cities
                .SelectMany(c => c.PointsOfInterest)
                .Select(poi => poi.Id)
                .Max();
            //TODO: use mapper
            var poi = new PointOfInterestDto()
            {
                Id = maxPoiId + 1,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };
            city.PointsOfInterest.Add(poi);

            //TODO: why the hell is this not working?
            return Created("/this/is/shit", poi);
            // return CreatedAtRoute(
            //     nameof(GetPointOfInterest),
            //     new {cityId, id = poi.Id},
            //     poi);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, PointOfInterestForCreationDto pointOfInterest)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(dto => dto.Id == cityId);
            if (city is null)
                return NotFound();
            var poi = city.PointsOfInterest
                .FirstOrDefault(dto => dto.Id == id);
            if (poi is null)
                return NotFound();

            //TODO replace it
            poi.Name = pointOfInterest.Name;
            poi.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            JsonPatchDocument<PointOfInterestForCreationDto> patchDoc)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(dto => dto.Id == cityId);
            if (city is null)
                return NotFound();
            var poi = city.PointsOfInterest
                .FirstOrDefault(dto => dto.Id == id);
            if (poi is null)
                return NotFound();

            //TODO Replace
            //Apply the partial update
            var pointOfInterestToPatch = new PointOfInterestForCreationDto()
            {
                Name = poi.Name,
                Description = poi.Description
            };
            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!TryValidateModel(pointOfInterestToPatch))
                return BadRequest();

            //TODO replace it
            poi.Name = pointOfInterestToPatch.Name;
            poi.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("int:id")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(dto => dto.Id == cityId);
            if (city is null)
                return NotFound();
            var poi = city.PointsOfInterest
                .FirstOrDefault(dto => dto.Id == id);
            if (poi is null)
                return NotFound();

            city.PointsOfInterest.Remove(poi);
            _mailService.Send("Point of Service Deleted", 
                $"Point of interest {poi} has been deleted.");
            return NoContent();
        }
    }
}