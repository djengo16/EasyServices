namespace EasyServices.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data;
    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;
    using EasyServices.Data.Repositories;
    using EasyServices.Web.ViewModels.Notifications;
    using EasyServices.Web.ViewModels.Reviews;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class ReviewsServiceTests
    {
        [Fact]
        public async Task CreateReviewShouldWorkCorrectly()
        {
            var list = new List<Review>();
            var mockRepo = new Mock<IDeletableEntityRepository<Review>>();
            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Review>())).Callback(
                (Review review) => list.Add(review));
            var notificationsService = new Mock<INotificationsService>();
            var announcementsService = new Mock<IAnnouncementsService>();
            var service = new ReviewsService(
                mockRepo.Object,
                notificationsService.Object,
                announcementsService.Object);

            var reviewInput = new ReviewInputModel
            {
                AnnouncementId = "123-asdf-123",
                Rate = 3,
                Comment = "test",
                UserId = "123",
            };
            ;
            await service.CreateAsync(reviewInput);

            reviewInput.Rate = 4;
            reviewInput.Comment = "new comment";

            await service.CreateAsync(reviewInput);

            var notifications = await notificationsService.Object.GetAllByUserIdAsync<NotificationViewModel>("123");

            Assert.Single(list);
            Assert.Equal(4, list.First().Rating);
            Assert.NotNull(notifications);
            Assert.Equal("new comment", list.First().Comment);
        }

        [Fact]
        public async Task DeleteReviewShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var reviewsRepository = new EfDeletableEntityRepository<Review>(db);
            var notificationsService = new Mock<INotificationsService>();
            var announcementsService = new Mock<IAnnouncementsService>();
            var service = new ReviewsService(
                reviewsRepository,
                notificationsService.Object,
                announcementsService.Object);

            var reviewInput = new ReviewInputModel
            {
                AnnouncementId = "123-asdf-123",
                Rate = 3,
                Comment = "test",
                UserId = "123",
            };

            var review = await service.CreateAsync(reviewInput);

            await service.DeleteAsync(review.Id);

            Assert.True(review.IsDeleted);
        }
    }
}
