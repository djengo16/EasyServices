namespace EasyServices.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data;
    using EasyServices.Data.Models;
    using EasyServices.Data.Repositories;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.Administration.SubCategories;
    using EasyServices.Web.ViewModels.SubCategories;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class SubCategoriesServiceTests
    {

        public SubCategoriesService GetService()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var db = new ApplicationDbContext(options);
            var subCategoriesRepository = new EfDeletableEntityRepository<SubCategory>(db);
            var announcementRepository = new EfDeletableEntityRepository<Announcement>(db);

            return new SubCategoriesService(subCategoriesRepository, announcementRepository);
        }

        [Fact]
        public async Task GetAllByCategoryIdShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SubCategoryInAdministration).Assembly);
            var service = this.GetService();

            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    await service.AddSubCategory(new AddSubCategoryInputModel
                    {
                        CategoryId = 1,
                        Name = "subcategory " + "i",
                    });
                }
                else
                {
                    await service.AddSubCategory(new AddSubCategoryInputModel
                    {
                        CategoryId = 2,
                        Name = "subcategory " + "i",
                    });
                }
            }

            var subCategories = service.GetAllByCategoryId(1);

            var subCategoriesGeneric = service.GetAllByCategoryId<SubCategoryInAdministration>(1);

            Assert.Equal(5, subCategories.Count());
            Assert.Equal(5, subCategoriesGeneric.Count());
            Assert.Equal(typeof(SubCategoryInAdministration), subCategoriesGeneric.First().GetType());
        }

        [Fact]
        public async Task GetCategoryIdShoudWorkCorrectly()
        {
            var service = this.GetService();

            var subcategory = await service.AddSubCategory(new AddSubCategoryInputModel
            {
                CategoryId = 1,
                Name = "test",
            });

            var result = service.GetCategoryId(subcategory.Id);

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetNameByIdShouldWorkCorrectly()
        {
            var service = this.GetService();

            var subcategory = await service.AddSubCategory(new AddSubCategoryInputModel
            {
                CategoryId = 1,
                Name = "test",
            });

            var result = service.GetNameById(subcategory.Id);

            Assert.Equal("test", result);
        }

        [Fact]
        public async Task GetByIdShouldWorkCorrectly()
        {
            AutoMapperConfig.RegisterMappings(typeof(SubCategoryInAdministration).Assembly);
            var service = this.GetService();

            var subcategory = await service.AddSubCategory(new AddSubCategoryInputModel
            {
                CategoryId = 1,
                Name = "test",
            });

            var result = service.GetById<SubCategoryInAdministration>(subcategory.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task EditSubCategoryShouldEditCorrectly()
        {
            var service = this.GetService();

            var subcategoryInput = new AddSubCategoryInputModel
            {
                CategoryId = 1,
                Name = "test",
            };

            var subcategory = await service.AddSubCategory(subcategoryInput);

            await service.EditSubCategory(new EditSubCategoryModel
            {
                Id = subcategory.Id,
                Name = "new name",
            });

            var afterEdit = service.GetById(subcategory.Id);

            Assert.Equal("new name", afterEdit.Name);
        }

        [Fact]
        public async Task DeleteSubCategoryShouldDelete()
        {
            var service = this.GetService();

            var categoryInput = new AddSubCategoryInputModel
            {
                Name = "test",
                CategoryId = 1,
            };

            var subcategory = await service.AddSubCategory(categoryInput);

            await service.DeleteSubCategory(subcategory.Id);

            Assert.True(subcategory.IsDeleted);
        }

        [Fact]
        public async Task GetCoutShouldReturnCorrectCount()
        {
            var service = this.GetService();

            for (int i = 0; i < 10; i++)
            {
                await service.AddSubCategory(new AddSubCategoryInputModel
                {
                    CategoryId = 1,
                    Name = "subcategory " + "i",
                });
            }

            var actual = service.GetCount();

            Assert.Equal(10, actual);
        }
    }
}
