namespace EasyServices.Web.Controllers
{
    using System.Threading.Tasks;

    using EasyServices.Data.Models;
    using EasyServices.Services.Data;
    using EasyServices.Web.ViewModels.Reviews;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ReviewsController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IReviewsService reviewsService;
        private readonly IAnnouncementsService announcementsService;

        public ReviewsController(
            UserManager<ApplicationUser> userManager,
            IReviewsService reviewsService, 
            IAnnouncementsService announcementsService)
        {
            this.userManager = userManager;
            this.reviewsService = reviewsService;
            this.announcementsService = announcementsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewInputModel review)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            review.User = user;
            review.Announcement = this.announcementsService.GetById(review.AnnouncementId);

            await this.reviewsService.CreateAsync(review);

            return this.RedirectToAction("Details", "Announcements", new { id = review.AnnouncementId });
        }

        public async Task<IActionResult> Delete(int reviewId, string announcementId)
        {
            await this.reviewsService.DeleteAsync(reviewId);

            return this.RedirectToAction("Details", "Announcements", new { id = announcementId });
        }
    }
}
