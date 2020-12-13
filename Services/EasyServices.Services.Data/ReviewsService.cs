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
        private readonly IAnnouncementsService announcementsService;

        public ReviewsService(IDeletableEntityRepository<Review> reviewsRepository,
            INotificationsService notificationsService,
            IAnnouncementsService announcementsService)
        {
            this.reviewsRepository = reviewsRepository;
            this.notificationsService = notificationsService;
            this.announcementsService = announcementsService;
        }

        public async Task<Review> CreateAsync(ReviewInputModel reviewInput)
        {
            var userId = reviewInput.UserId;

            var review = this.reviewsRepository.All()
                .FirstOrDefault(x => x.AnnouncementId == reviewInput.AnnouncementId && x.UserId == userId);

            if (review == null)
            {
                review = new Review
                {
                    AnnouncementId = reviewInput.AnnouncementId,
                    Announcement = await this.announcementsService.GetByIdAsync(reviewInput.AnnouncementId),
                    Comment = reviewInput.Comment,
                    Rating = reviewInput.Rate,
                    UserId = reviewInput.UserId, // the user that add the review
                };

                await this.notificationsService.AddNotificationFromReviewAsync(review);

                await this.reviewsRepository.AddAsync(review);
            }
            else
            {
                review.Rating = reviewInput.Rate;
                review.Comment = reviewInput.Comment;

                this.reviewsRepository.Update(review);
            }

            await this.reviewsRepository.SaveChangesAsync();

            return review;
        }

        public async Task DeleteAsync(int reviewId)
        {
            var review = this.reviewsRepository.All().FirstOrDefault(x => x.Id == reviewId);

            this.reviewsRepository.Delete(review);

            await this.reviewsRepository.SaveChangesAsync();
        }
    }
}
