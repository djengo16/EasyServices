namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;

    using EasyServices.Services.Mapping;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public IEnumerable<T> GetAll<T>()
        {
            IQueryable<Category> query =
                this.categoriesRepository.All().OrderBy(x => x.Name);

            return query.To<T>().ToList();
        }

        public ICollection<Category> GetFromDb()
        {
            return this.categoriesRepository.All().ToList();
        }

        // TODO: fix this method
        public string GetShort<T>(IEnumerable<T> input, int length)
        {
            return string.Join(", ", input).Substring(0, length) + "...";
        }

        public string GetNameById(int id)
        {
            return this.categoriesRepository.All().FirstOrDefault(x => x.Id == id)?.Name;
        }

        public ICollection<T> GetCategoriesAndSubCategories<T>()
        {
            IQueryable<Category> query =
                this.categoriesRepository.All().OrderBy(x => x.Name);

            return query.To<T>()
                .ToList();
        }
    }
}
