namespace EasyServices.Services.Data
{
    using System.Threading.Tasks;

    using EasyServices.Web.ViewModels.Reviews;

    public interface IReviewsService
    {
        Task CreateAsync(ReviewInputModel review, string userId);

        Task DeleteAsync(int reviewId);
    }
}
