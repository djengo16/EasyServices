namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.Administration.Categories;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly ISubCategoriesService subCategoriesService;

        public CategoriesService(
            IDeletableEntityRepository<Category> categoriesRepository,
            ISubCategoriesService subCategoriesService)
        {
            this.categoriesRepository = categoriesRepository;
            this.subCategoriesService = subCategoriesService;
        }

        public IEnumerable<T> GetAll<T>()
        {

            IQueryable<Category> query =
                this.categoriesRepository.All().OrderBy(x => x.Name);

            return query.To<T>().ToList();
        }

        public string GetShort<T>(IEnumerable<T> input)
        {
            return string.Join(", ", input.Take(8)) + " ...";
        }

        public string GetNameById(int id)
        {
            return this.categoriesRepository.All().FirstOrDefault(x => x.Id == id)?.Name;
        }

        public async Task<ICollection<T>> GetCategoriesAndSubCategoriesAsync<T>()
        {

            IQueryable<Category> query =
                this.categoriesRepository.AllAsNoTracking().OrderBy(x => x.Name);

            return await query.To<T>()
                .ToListAsync();
        }

        public async Task AddCategoryAsync(AddCategoryInputModel inputModel)
        {
            await this.categoriesRepository.AddAsync(new Category
            {
                Name = inputModel.Name,
                ImgUrl = inputModel.ImgUrl,
            });

            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task EditCategoryAsync(EditCategoryModel inputModel)
        {
            var category = this.GetById(inputModel.Id);

            category.Name = inputModel.Name;
            category.ImgUrl = inputModel.ImgUrl;

            this.categoriesRepository.Update(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public Category GetById(int id)
        {
            return this.categoriesRepository.All().FirstOrDefault(x => x.Id == id);
        }

        public async Task DeleteCategory(int categoryId)
        {
            var category = this.GetById(categoryId);

            var subCategories = this.subCategoriesService.GetAllByCategoryId(categoryId);

            foreach (var subCategory in subCategories)
            {
                await this.subCategoriesService.DeleteSubCategory(subCategory.Id);
            }

            this.categoriesRepository.Delete(category);

            await this.categoriesRepository.SaveChangesAsync();
        }
    }
}
