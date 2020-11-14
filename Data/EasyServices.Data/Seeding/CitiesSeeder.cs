namespace EasyServices.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using EasyServices.Data.Models;
    using EasyServices.Data.Seeding.DTO;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;

    public class CitiesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Cities.Any())
            {
                return;
            }

            ImportCitiesDTO[] importedCities = JsonConvert
                .DeserializeObject<ImportCitiesDTO[]>(File.ReadAllText(@"../../Data/EasyServices.Data/Seeding/Data/Cities.json"));

            List<City> cities = new List<City>();

            foreach (var importedCity in importedCities)
            {
                cities.Add(new City
                {
                    Name = importedCity.Name,
                });
            }

            await dbContext.Cities.AddRangeAsync(cities);

            await dbContext.SaveChangesAsync();
        }
    }
}
