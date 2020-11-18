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

        public ReviewsService(IDeletableEntityRepository<Review> reviewsRepository)
        {
            this.reviewsRepository = reviewsRepository;
        }

        public async Task CreateAsync(ReviewInputModel review, string userId)
        {
            var existingReview = this.reviewsRepository.All()
                .FirstOrDefault(x => x.AnnouncementId == review.AnnouncementId && x.UserId == userId);

            if (existingReview == null)
            {
                var newReview = new Review
                {
                    AnnouncementId = review.AnnouncementId,
                    Comment = review.Comment,
                    Rating = review.Rate,
                    UserId = userId,
                };

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
