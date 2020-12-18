namespace EasyServices.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data;
    using EasyServices.Data.Models;
    using EasyServices.Data.Repositories;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.Announcements;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class TagsServiceTests
    {
        public TagsServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(AnnouncementTagInputModel).Assembly);
        }

        [Fact]
        public async Task CreateShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var tagsRepository = new EfRepository<Tag>(db);
            var announcementTagsRepository = new EfRepository<AnnouncementTag>(db);

            var service = new TagsService(tagsRepository, announcementTagsRepository);

            string tagName = "tag";

            await service.CreateAsync(tagName);

            Assert.True(service.CheckIfExist(tagName));
        }

        [Fact]
        public async Task FindTagIdShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var tagsRepository = new EfRepository<Tag>(db);
            var announcementTagsRepository = new EfRepository<AnnouncementTag>(db);

            var service = new TagsService(tagsRepository, announcementTagsRepository);

            string tagName = "tag";

            int tagIdExpected = await service.CreateAsync(tagName);
            int tagIdActual = service.FindTagId(tagName);

            Assert.Equal(tagIdExpected, tagIdActual);
        }

        [Fact]
        public async Task GetAllShouldWorkCorrect()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var tagsRepository = new EfRepository<Tag>(db);
            var announcementTagsRepository = new EfRepository<AnnouncementTag>(db);

            var service = new TagsService(tagsRepository, announcementTagsRepository);

            for (int i = 0; i < 5; i++)
            {
                string tagName = "tag" + i.ToString();

                await service.CreateAsync(tagName);
            }

            var tags = service.GetAll<AnnouncementTagInputModel>();
            Assert.Equal(5, tags.Count());
        }

        [Fact]
        public async Task GetNamesByAnnouncementShouldReturnTheTags()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var tagsRepository = new EfRepository<Tag>(db);
            var announcementTagsRepository = new EfRepository<AnnouncementTag>(db);

            var service = new TagsService(tagsRepository, announcementTagsRepository);

            db.Announcements.Add(new Announcement
            {
                Title = "title",
                CategoryId = 1,
                SubCategoryId = 1,
            });

            await db.SaveChangesAsync();

            var announcementId = db.Announcements.FirstOrDefault().Id;

            for (int i = 0; i < 5; i++)
            {
                string tagName = "tag" + i.ToString();
                int tagId = await service.CreateAsync(tagName);
                db.AnnouncementTags.Add(new AnnouncementTag
                {
                    AnnouncementId = announcementId,
                    TagId = tagId,
                });
            }

            await db.SaveChangesAsync();

            var tags = service.GetNamesByAnnouncementId(announcementId);
            Assert.Equal(5, tags.Count());
            Assert.Equal("tag0", tags.ElementAt(0));
        }
    }
}
