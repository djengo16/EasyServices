namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.Administration.SubCategories;
    using Microsoft.EntityFrameworkCore;

    public class SubCategoriesService : ISubCategoriesService
    {
        private readonly IDeletableEntityRepository<SubCategory> subCategoriesRepository;
        private readonly IDeletableEntityRepository<Announcement> announcementsRepository;

        public SubCategoriesService(
            IDeletableEntityRepository<SubCategory> subCategoriesRepository,
            IDeletableEntityRepository<Announcement> announcementsRepository)
        {
            this.subCategoriesRepository = subCategoriesRepository;
            this.announcementsRepository = announcementsRepository;
        }

        public IEnumerable<T> GetAllByCategoryId<T>(int id)
        {
            IQueryable<SubCategory> query =
               this.subCategoriesRepository.All()
               .Where(x => x.CategoryId == id)
               .OrderBy(x => x.Name);

            return query.To<T>().ToList();
        }

        public IEnumerable<SubCategory> GetAllByCategoryId(int id)
        {
            return this.subCategoriesRepository
                .All()
                .Where(x => x.CategoryId == id)
                .ToList();
        }

        public int GetCategoryId(int subcategoryId)
        {
            return this.subCategoriesRepository.All()
                .FirstOrDefault(x => x.Id == subcategoryId).CategoryId;
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

        public async Task AddSubCategory(AddSubCategoryInputModel inputModel)
        {
            await this.subCategoriesRepository.AddAsync(new SubCategory
            {
                Name = inputModel.Name,
                CategoryId = inputModel.CategoryId,
            });

            await this.subCategoriesRepository.SaveChangesAsync();
        }

        public async Task EditSubCategory(EditSubCategoryModel inputModel)
        {
            var subCategory = this.GetById(inputModel.Id);

            subCategory.Name = inputModel.Name;

            this.subCategoriesRepository.Update(subCategory);
            await this.subCategoriesRepository.SaveChangesAsync();
        }

        public async Task DeleteSubCategory(int categoryId)
        {
            var subCategory = this.GetById(categoryId);

            this.subCategoriesRepository.Delete(subCategory);

            var announcements = await this.announcementsRepository
                .All().Where(x => x.SubCategoryId == subCategory.Id).ToListAsync();

            foreach (var announcement in announcements)
            {
                this.announcementsRepository.Delete(announcement);
            }

            await this.announcementsRepository.SaveChangesAsync();
            await this.subCategoriesRepository.SaveChangesAsync();
        }

        public int GetCount()
        {
            return this.subCategoriesRepository.All().Count();
        }

        private SubCategory GetById(int id)
        {
            return this.subCategoriesRepository.All().FirstOrDefault(x => x.Id == id);
        }
    }
}
