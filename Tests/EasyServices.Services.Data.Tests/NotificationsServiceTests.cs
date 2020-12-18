namespace EasyServices.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data;
    using EasyServices.Data.Models;
    using EasyServices.Data.Repositories;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.Notifications;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class NotificationsServiceTests
    {
        public NotificationsServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(NotificationViewModel).Assembly);
        }

        [Fact]
        public async Task AddNotificationFromReviewShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var notificationsRepository = new EfDeletableEntityRepository<Notification>(db);
            var service = new NotificationsService(notificationsRepository);

            var announcementUser = new ApplicationUser
            {
                Id = "123",
                Email = "test@test.test",
                Name = "user1",
            };

            var reviewUser = new ApplicationUser
            {
                Id = "124",
                Email = "test1@test.test",
                Name = "user2",
            };

            db.Users.Add(announcementUser);
            db.Users.Add(reviewUser);
            await db.SaveChangesAsync();

            var announcement = new Announcement
            {
                Id = "123",
                UserId = "123",
                User = announcementUser,
                Title = "title",
                CategoryId = 1,
                SubCategoryId = 1,
            };

            var review = new Review
            {
                UserId = reviewUser.Id,
                User = reviewUser,
                Announcement = announcement,
                Comment = "test",
                Rating = 5,
            };

            await service.AddNotificationFromReviewAsync(review);

            var notifications = await service.GetUnreadUserNotificationsCount(announcementUser.Id);

            Assert.Equal(1, notifications);
        }

        [Fact]
        public async Task DeleteAsyncShouldDeleteNotification()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var notificationsRepository = new EfDeletableEntityRepository<Notification>(db);
            var service = new NotificationsService(notificationsRepository);

            var announcementUser = new ApplicationUser
            {
                Id = "123",
                Email = "test@test.test",
                Name = "user1",
            };

            var reviewUser = new ApplicationUser
            {
                Id = "124",
                Email = "test1@test.test",
                Name = "user2",
            };

            db.Users.Add(announcementUser);
            db.Users.Add(reviewUser);
            await db.SaveChangesAsync();

            var announcement = new Announcement
            {
                Id = "123",
                UserId = "123",
                User = announcementUser,
                Title = "title",
                CategoryId = 1,
                SubCategoryId = 1,
            };

            var review = new Review
            {
                UserId = reviewUser.Id,
                User = reviewUser,
                Announcement = announcement,
                Comment = "test",
                Rating = 5,
            };

            await service.AddNotificationFromReviewAsync(review);
            var id = db.Notifications.FirstOrDefaultAsync().Id;
            await service.DeleteAsync(id);
            var count = db.Notifications.Count();

            Assert.Equal(0, count);
        }

        [Fact]
        public async Task GetAllByUserIdShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var notificationsRepository = new EfDeletableEntityRepository<Notification>(db);
            var service = new NotificationsService(notificationsRepository);

            var announcementUser = new ApplicationUser
            {
                Id = "123",
                Email = "test@test.test",
                Name = "user1",
            };

            var reviewUser = new ApplicationUser
            {
                Id = "124",
                Email = "test1@test.test",
                Name = "user2",
            };

            db.Users.Add(announcementUser);
            db.Users.Add(reviewUser);
            await db.SaveChangesAsync();

            var announcement = new Announcement
            {
                Id = "123",
                UserId = "123",
                User = announcementUser,
                Title = "title",
                CategoryId = 1,
                SubCategoryId = 1,
            };

            for (int i = 1; i < 4; i++)
            {
                var review = new Review
                {
                    UserId = reviewUser.Id,
                    User = reviewUser,
                    Announcement = announcement,
                    Comment = "test",
                    Rating = i,
                };
                await service.AddNotificationFromReviewAsync(review);
            }

            var notifications = await service.GetAllByUserIdAsync<NotificationViewModel>(announcement.Id);

            Assert.Equal(3, notifications.Count());
        }
    }
}
