using System;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Contexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository, IDisposable
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

        public async Task<City> GetCityAsync(int cityId, bool includePoi = false)
        {
            if (includePoi)
            {
                return await _context.Cities
                    .Include(city => city.PointsOfInterest)
                    .FirstOrDefaultAsync(city => city.Id == cityId);
            }

            return await _context.Cities
                .FirstOrDefaultAsync(city => city.Id == cityId);
        }

        public IQueryable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        {
            return _context.PointOfInterests
                .Where(poi => poi.CityId == cityId);
        }

        public async Task<PointOfInterest> GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return await _context.PointOfInterests
                .FirstOrDefaultAsync(poi => poi.CityId == cityId && poi.Id == pointOfInterestId);
        }

        public async Task AddPointsOfInterestForCity(int cityId, params PointOfInterest[] poi)
        {
            var city = await GetCityAsync(cityId);
            foreach (var pointOfInterest in poi)
            {
                city.PointsOfInterest.Add(pointOfInterest);
            }

            await Save();
        }

        public async Task UpdatePointOfInterestForCity(int cityId, PointOfInterest poi)
        {
            //PointOfInterest is a tracked object by entity framework, so we only need to Save the changes in the repo!
            await Save();
        }

        public async Task DeletePointOfInterest(PointOfInterest poi)
        {
            _context.PointOfInterests.Remove(poi);
            await Save();
        }

        private async Task Save()
        {
            var savedChanges = await _context.SaveChangesAsync();
            if (savedChanges > 0)
            {
                _logger.LogDebug("Saved {} changed in DB {}", savedChanges, _context);
            }
            else
            {
                _logger.LogWarning("No changes saved in DB {}", _context);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}