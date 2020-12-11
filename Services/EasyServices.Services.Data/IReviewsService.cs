namespace EasyServices.Services.Data
{
    using System.Threading.Tasks;
    using EasyServices.Data.Models;
    using EasyServices.Web.ViewModels.Reviews;

    public interface IReviewsService
    {
        Task<Review> CreateAsync(ReviewInputModel review);

        Task DeleteAsync(int reviewId);
    }
}
