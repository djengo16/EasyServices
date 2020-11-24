namespace EasyServices.Services.Data
{
    using System.Collections.Generic;

    using EasyServices.Data.Models;

    public interface ICitiesService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

        City GetCityById(int? cityId);
    }
}
