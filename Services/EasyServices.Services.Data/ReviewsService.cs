namespace EasyServices.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;
    using EasyServices.Web.ViewModels.Reviews;

    public class ReviewsService : IReviewsService
    {
        private readonly IDeletableEntityRepository<Review> reviewsRepository;
        private readonly INotificationsService notificationsService;

        public ReviewsService(IDeletableEntityRepository<Review> reviewsRepository, INotificationsService notificationsService)
        {
            this.reviewsRepository = reviewsRepository;
            this.notificationsService = notificationsService;
        }

        public async Task CreateAsync(ReviewInputModel review)
        {
            var userId = review.User.Id;

            var existingReview = this.reviewsRepository.All()
                .FirstOrDefault(x => x.AnnouncementId == review.AnnouncementId && x.UserId == userId);

            if (existingReview == null)
            {
                var newReview = new Review
                {
                    Announcement = review.Announcement,
                    Comment = review.Comment,
                    Rating = review.Rate,
                    User = review.User,
                };

                await this.notificationsService.AddNotificationFromReviewAsync(newReview);

                await this.reviewsRepository.AddAsync(newReview);
            }
            else
            {
                existingReview.Rating = review.Rate;
                existingReview.Comment = review.Comment;

                this.reviewsRepository.Update(existingReview);
            }

            await this.reviewsRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int reviewId)
        {
            var review = this.reviewsRepository.All().FirstOrDefault(x => x.Id == reviewId);

            this.reviewsRepository.Delete(review);

            await this.reviewsRepository.SaveChangesAsync();
        }
    }
}
