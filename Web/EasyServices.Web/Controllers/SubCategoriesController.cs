namespace EasyServices.Web.Controllers
{
    using EasyServices.Services.Data;
    using EasyServices.Web.ViewModels.Announcements;
    using Microsoft.AspNetCore.Mvc;

    public class SubCategoriesController : BaseController
    {
        private readonly IAnnouncementsService announcementsService;
        private readonly ISubCategoriesService subCategoriesService;

        public SubCategoriesController(
            IAnnouncementsService announcementsService,
            ISubCategoriesService subCategoriesService)
        {
            this.announcementsService = announcementsService;
            this.subCategoriesService = subCategoriesService;
        }

        public IActionResult ById(int id)
        {
            var model = new GetAllAnnouncementsViewModel
            {
                CategoryName = this.subCategoriesService.GetCategoryName(id),
                SubCategoryName = this.subCategoriesService.GetNameById(id),
                CategoryId = this.subCategoriesService.GetCategoryId(id),
                Announcements = this.announcementsService.GetAllBySubCategoryId<AnnouncementViewModel>(id),
            };

            return this.View(model);
        }
    }
}
