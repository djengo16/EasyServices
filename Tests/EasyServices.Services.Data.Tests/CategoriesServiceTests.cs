namespace EasyServices.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data;
    using EasyServices.Data.Models;
    using EasyServices.Data.Repositories;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.Administration.Categories;
    using EasyServices.Web.ViewModels.Categories;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class CategoriesServiceTests
    {
        public CategoriesServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(CategoryViewModel).Assembly);
        }

        public CategoriesService GetService()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var db = new ApplicationDbContext(options);
            var categoriesRepository = new EfDeletableEntityRepository<Category>(db);
            var subCategoriesService = new Mock<ISubCategoriesService>();

            return new CategoriesService(categoriesRepository, subCategoriesService.Object);
        }

        [Fact]
        public async Task GetAllShouldWorkCorrectly()
        {
            var service = this.GetService();

            for (int i = 0; i < 5; i++)
            {
                await service.AddCategoryAsync(new AddCategoryInputModel
                {
                    Name = "Name " + i.ToString(),
                    ImgUrl = "testimgurl " + i.ToString(),
                });
            }

            var all = service.GetAll<CategoryViewModel>();

            Assert.Equal(5, all.Count());
        }

        [Fact]
        public async Task GetNameByIdShouldWorkCorrectly()
        {
            var service = this.GetService();

            var categoryInput = new AddCategoryInputModel
            {
                Name = "test",
                ImgUrl = "test",
            };

            var category = await service.AddCategoryAsync(categoryInput);

            var actualName = service.GetNameById(category.Id);

            Assert.Equal("test", actualName);
        }

        [Fact]
        public async Task EditCategoryShouldEditCorrectly()
        {
            var service = this.GetService();

            var categoryInput = new AddCategoryInputModel
            {
                Name = "test",
                ImgUrl = "test",
            };

            var category = await service.AddCategoryAsync(categoryInput);

            await service.EditCategoryAsync(new EditCategoryModel
            {
                Id = category.Id,
                Name = "new name",
                ImgUrl = "new image",
            });

            var afterEdit = service.GetById(category.Id);

            Assert.Equal("new name", afterEdit.Name);
            Assert.Equal("new image", afterEdit.ImgUrl);
        }

        [Fact]
        public async Task DeleteCategoryShouldDelete()
        {
            var service = this.GetService();

            var categoryInput = new AddCategoryInputModel
            {
                Name = "test",
                ImgUrl = "test",
            };

            var category = await service.AddCategoryAsync(categoryInput);

            await service.DeleteCategory(category.Id);

            Assert.True(category.IsDeleted);
        }
    }
}
