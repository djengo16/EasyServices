namespace EasyServices.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using EasyServices.Data;
    using EasyServices.Data.Models;
    using EasyServices.Data.Repositories;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.Announcements;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class AnnouncementServiceTests
    {
        public AnnouncementServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(TestAnnouncementModel).Assembly);
        }

        public AnnouncementsService GetService()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var announcementsRepository = new EfDeletableEntityRepository<Announcement>(db);
            var subCategoriesService = new Mock<ISubCategoriesService>();
            var tagsService = new Mock<ITagsService>();
            var citiesService = new Mock<ICitiesService>();
            var imagesService = new Mock<IImagesService>();

            var announcementsService = new AnnouncementsService(
                announcementsRepository,
                subCategoriesService.Object,
                tagsService.Object,
                citiesService.Object,
                imagesService.Object);

            return announcementsService;
        }

        [Fact]
        public async Task CreateAnnouncementShouldWorkCorrectly()
        {
            var service = this.GetService();

            var announcement = new AnnouncementInputModel
            {
                SubCategoryId = 1,
                Title = "title",
                Description = "description",
                UserId = "123",
                Tags = new List<string> { "tag1", "tag2" },
            };
            var id = await service.CreateAsync(announcement);
            var createdAnnouncement = await service.GetByIdAsync(id);
            Assert.Equal(1, service.GetCount());
            Assert.Equal("title", createdAnnouncement.Title);
        }

        [Fact]
        public async Task DeleteAnnouncementShouldDelete()
        {
            var service = this.GetService();
            var announcement = new AnnouncementInputModel
            {
                SubCategoryId = 1,
                Title = "title",
                Description = "description",
                UserId = "123",
                Tags = new List<string> { "tag1", "tag2" },
            };
            var id = await service.CreateAsync(announcement);

            await service.DeleteAsync(id);
            Assert.Equal(0, service.GetCount());
        }

        [Fact]
        public async Task GetCountBySubCategoryIdShouldWorkCorrectly()
        {
            var service = this.GetService();

            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    var announcement = new AnnouncementInputModel
                    {
                        SubCategoryId = 1,
                        Title = "title" + i.ToString(),
                        Description = "description",
                        UserId = "123",
                        Tags = new List<string> { "tag1", "tag2" },
                    };
                    await service.CreateAsync(announcement);
                }
                else
                {
                    var announcement = new AnnouncementInputModel
                    {
                        SubCategoryId = 2,
                        Title = "title" + i.ToString(),
                        Description = "description",
                        UserId = "123",
                        Tags = new List<string> { "tag1", "tag2" },
                    };
                    await service.CreateAsync(announcement);
                }
            }

            var actual = await service.GetCountBySubCategoryIdAsync(1);

            Assert.Equal(5, actual);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnAnnouncementById()
        {
            var service = this.GetService();

            var announcement = new AnnouncementInputModel
            {
                SubCategoryId = 1,
                Title = "title",
                Description = "description",
                UserId = "123",
                Tags = new List<string> { "tag1", "tag2" },
            };
            var id = await service.CreateAsync(announcement);

            var announcementById = await service.GetByIdAsync(id);

            Assert.NotNull(announcementById);
            Assert.Equal("title", announcementById.Title);
        }

        //[Fact]
        // public async Task GetDetailsShouldWorkCorrectly()
        //{
        //    var service = this.GetService();

        //    var announcement = new AnnouncementInputModel
        //    {
        //        SubCategoryId = 1,
        //        Title = "title",
        //        Description = "description",
        //        UserId = "123",
        //    };
        //    var id = await service.CreateAsync(announcement);

        //    var announcementDetails = service.GetDetails<AnnouncementDetailsViewModel>(id);

        //    Assert.Equal(2, announcementDetails.Tags.Count);
        //    Assert.Equal("title", announcementDetails.Title);
        //    Assert.Equal("description", announcementDetails.Description);
        //}

        [Fact]
        public async Task UpdateShouldUpdateCorrectly()
        {
            var service = this.GetService();

            var announcement = new AnnouncementInputModel
            {
                SubCategoryId = 1,
                Title = "title",
                Description = "description",
                UserId = "123",
            };
            var id = await service.CreateAsync(announcement);

            var updatemodel = new UpdateAnnouncementInputModel
            {
                Description = "new description",
                SubCategoryId = 3,
                CityId = 2,
            };
            await service.UpdateAsync(updatemodel, id);

            var announcementAfterUpdate = await service.GetByIdAsync(id);

            Assert.Equal("new description", announcementAfterUpdate.Description);
            Assert.Equal(3, announcementAfterUpdate.SubCategoryId);
            Assert.Equal(2, announcementAfterUpdate.CityId);
        }

        [Fact]
        public async Task GetLastShouldReturnExactCount()
        {
            var service = this.GetService();

            for (int i = 0; i < 5; i++)
            {
                var announcement = new AnnouncementInputModel
                {
                    SubCategoryId = 1,
                    Title = "title" + i.ToString(),
                    Description = "description",
                    UserId = "123",
                };
                await service.CreateAsync(announcement);
            }

            var lastOnes = await service.GetLastAsync<TestAnnouncementModel>(2);

            Assert.Equal(2, lastOnes.Count());
            Assert.Equal("title4", lastOnes.ElementAt(0).Title);
        }

        [Fact]
        public async Task GetAllBySubCategoryIdShouldWorkCorrectly()
        {
            var service = this.GetService();

            for (int i = 0; i < 10; i++)
            {
                var announcement = new AnnouncementInputModel
                {
                    SubCategoryId = 1,
                    Title = "title" + i.ToString(),
                    Description = "description",
                    UserId = "123",
                };
                await service.CreateAsync(announcement);
            }

            for (int i = 0; i < 3; i++)
            {
                var announcement = new AnnouncementInputModel
                {
                    SubCategoryId = 2,
                    Title = "title" + i.ToString(),
                    Description = "description",
                    UserId = "123",
                };
                await service.CreateAsync(announcement);
            }

            var announcements = await service
                .GetAllBySubCategoryIdAsync<TestAnnouncementModel>(1, 5); // taking 5 models from id 1

            Assert.Equal(5, announcements.Count());
        }

        [Fact]
        public async Task SearchShouldWorkCorrectlyWhenWithAllParameters()
        {
            var service = this.GetService();

            for (int i = 0; i < 4; i++)
            {
                var announcement = new AnnouncementInputModel
                {
                    SubCategoryId = 1,
                    Title = "title" + i.ToString(),
                    Description = "description",
                    UserId = "123",
                    CityId = 2,
                };
                await service.CreateAsync(announcement);
            }

            for (int i = 0; i < 3; i++)
            {
                var announcement = new AnnouncementInputModel
                {
                    SubCategoryId = 1,
                    Title = "another" + i.ToString(),
                    Description = "another",
                    UserId = "123",
                    CityId = 4,
                };
                await service.CreateAsync(announcement);
            }

            // cityId , subcategoryId , keywords
            ;
            var result = service
                .GetBySearchParams<TestAnnouncementModel>(2, 1, "title");

            Assert.Equal(4, result.Count());
            Assert.True(result.Any(x => x.Title.Contains("title")));
        }

    }
}
