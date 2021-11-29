using System.Linq;
using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        IQueryable<City> GetCities();

        City GetCity(int cityId, bool includePoi = false);

        IQueryable<PointOfInterest> GetPointsOfInterestForCity(int cityId);

        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);

        public void AddPointOfInterestForCity(int cityId, PointOfInterest poi);

        public void UpdatePointOfInterestForCity(int cityId, PointOfInterest poi);
        void DeletePointOfInterest(PointOfInterest poi);
    }
}