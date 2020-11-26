namespace EasyServices.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using EasyServices.Services.Data;
    using EasyServices.Web.ViewModels.Announcements;
    using EasyServices.Web.ViewModels.SubCategories;
    using Microsoft.AspNetCore.Mvc;

    public class SubCategoriesController : BaseController
    {
        private const int AnnouncementsPerPage = 6;

        private readonly IAnnouncementsService announcementsService;
        private readonly ISubCategoriesService subCategoriesService;

        public SubCategoriesController(
            IAnnouncementsService announcementsService,
            ISubCategoriesService subCategoriesService)
        {
            this.announcementsService = announcementsService;
            this.subCategoriesService = subCategoriesService;
        }

        public async Task<IActionResult> ById(int id, int page = 1)
        {
            var model = this.subCategoriesService.GetById<SubCategoryViewModel>(id);

            model.Announcements = await this.announcementsService
                .GetAllBySubCategoryIdAsync<AnnouncementViewModel>(id, AnnouncementsPerPage, (page - 1) * AnnouncementsPerPage);

            int count = await this.announcementsService.GetCountBySubCategoryIdAsync(id);

            model.PagesCount = (int)Math.Ceiling((double)count / AnnouncementsPerPage);

            model.CurrentPage = page;

            return this.View(model);
        }
    }
}
