using System;
using System.Linq;
using CityInfo.API.Contexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly ILogger<CityInfoRepository> _logger;
        private readonly CityInfoContext _context;

        public CityInfoRepository(ILogger<CityInfoRepository> logger, CityInfoContext context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public IQueryable<City> GetCities()
        {
            return _context.Cities.OrderBy(city => city.Name);
        }

        public City GetCity(int cityId, bool includePoi = false)
        {
            if (includePoi)
            {
                return _context.Cities.Include(city => city.PointsOfInterest)
                    .FirstOrDefault(city => city.Id == cityId);
            }

            return _context.Cities
                .FirstOrDefault(city => city.Id == cityId);
        }

        public IQueryable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        {
            return _context.PointOfInterests
                .Where(poi => poi.CityId == cityId);
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _context.PointOfInterests
                .FirstOrDefault(poi => poi.CityId == cityId && poi.Id == pointOfInterestId);
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterest poi)
        {
            var city = GetCity(cityId);
            city.PointsOfInterest.Add(poi);
            Save();
        }

        public void UpdatePointOfInterestForCity(int cityId, PointOfInterest poi)
        {
            //PointOfInterest is a tracked object by entity framework, so we only need to Save the changes in the repo!
            Save();
        }

        public void DeletePointOfInterest(PointOfInterest poi)
        {
            _context.PointOfInterests.Remove(poi);
            Save();
        }

        private void Save()
        {
            var savedChanges = _context.SaveChanges();
            if (savedChanges > 0)
            {
                _logger.LogDebug("Saved {} changed in DB {}", savedChanges, _context);
            }
            else
            {
                _logger.LogWarning("No changes saved in DB {}", _context);
            }
        }
    }
}