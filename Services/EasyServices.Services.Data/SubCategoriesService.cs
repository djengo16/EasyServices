﻿namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class SubCategoriesService : ISubCategoriesService
    {
        private readonly IDeletableEntityRepository<SubCategory> subCategoriesRepository;
        private readonly ICategoriesService categoriesService;

        public SubCategoriesService(
            IDeletableEntityRepository<SubCategory> subCategoriesRepository
            , ICategoriesService categoriesService)
        {
            this.subCategoriesRepository = subCategoriesRepository;
            this.categoriesService = categoriesService;
        }

        public IEnumerable<T> GetAllByCategoryId<T>(int id)
        {
            IQueryable<SubCategory> query =
               this.subCategoriesRepository.All()
               .Where(x => x.CategoryId == id)
               .OrderBy(x => x.Name);

            return query.To<T>().ToList();
        }

        public int GetCategoryId(int subcategoryId)
        {
            return this.subCategoriesRepository.All()
                .FirstOrDefault(x => x.Id == subcategoryId).CategoryId;
        }

        public string GetCategoryName(int subCategoryId)
        {
            int categoryId = this.GetCategoryId(subCategoryId);

            return this.categoriesService.GetNameById(categoryId);
        }

        public string GetNameById(int id)
        {
            return this.subCategoriesRepository.All().FirstOrDefault(x => x.Id == id).Name;
        }

        public int GetIdByName(string name)
        {
            return this.subCategoriesRepository.All().FirstOrDefault(x => x.Name.ToLower() == name.ToLower()).Id;
        }

        public T GetById<T>(int id)
        {
            var category = this.subCategoriesRepository.All()
               .Where(x => x.Id == id)
               .To<T>().FirstOrDefault();

            return category;
        }
    }
}
