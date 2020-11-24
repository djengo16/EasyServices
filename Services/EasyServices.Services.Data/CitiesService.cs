namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using EasyServices.Services.Data;
    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;

    public class CitiesService : ICitiesService
    {
        private readonly IDeletableEntityRepository<City> citiesRepository;

        public CitiesService(IDeletableEntityRepository<City> citiesRepository)
        {
            this.citiesRepository = citiesRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.citiesRepository.AllAsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                }).ToList().Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public City GetCityById(int? cityId)
        {
            return this.citiesRepository.All()
                .FirstOrDefault(x => x.Id == cityId);
        }
    }
}
