using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        IQueryable<City> GetCities();

        Task<City> GetCityAsync(int cityId, bool includePoi = false);

        IQueryable<PointOfInterest> GetPointsOfInterestForCity(int cityId);

        Task<PointOfInterest> GetPointOfInterestForCity(int cityId, int pointOfInterestId);

        Task AddPointsOfInterestForCity(int cityId, params PointOfInterest[] poi);

        Task UpdatePointOfInterestForCity(int cityId, PointOfInterest poi);
        Task DeletePointOfInterest(PointOfInterest poi);
    }
}