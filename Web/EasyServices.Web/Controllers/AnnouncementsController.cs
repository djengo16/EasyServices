namespace EasyServices.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data.Models;
    using EasyServices.Services.Data;
    using EasyServices.Web.ViewModels.Announcements;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class AnnouncementsController : BaseController
    {
        private readonly IAnnouncementsService announcementsService;
        private readonly ICitiesService citiesService;
        private readonly ICategoriesService categoriesService;
        private readonly ITagsService tagsService;
        private readonly UserManager<ApplicationUser> userManager;

        public AnnouncementsController(
            IAnnouncementsService announcementsService,
            ICitiesService citiesService,
            ICategoriesService categoriesService,
            ITagsService tagsService,
            UserManager<ApplicationUser> userManager)
        {
            this.announcementsService = announcementsService;
            this.citiesService = citiesService;
            this.categoriesService = categoriesService;
            this.tagsService = tagsService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new AnnouncementInputModel();
            viewModel.CitiesItems = this.citiesService.GetAllAsKeyValuePairs();
            viewModel.Categories = this.categoriesService.GetCategoriesAndSubCategories<AnnouncementCategoryInputModel>();
            viewModel.TagsItems = this.tagsService.GetAll<AnnouncementTagsInputModel>();
            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(AnnouncementInputModel inputModel)
        {

            if (!this.ModelState.IsValid)
            {
                inputModel.TagsItems = this.tagsService.GetAll<AnnouncementTagsInputModel>();
                inputModel.CitiesItems = this.citiesService.GetAllAsKeyValuePairs();
                inputModel.Categories = this.categoriesService.GetCategoriesAndSubCategories<AnnouncementCategoryInputModel>();

                return this.View(inputModel);
            }

            inputModel.UserId = this.userManager.GetUserId(this.HttpContext.User);

            var announcementId = await this.announcementsService.CreateAsync(inputModel);

            return this.Redirect($"/Announcements/Details/{announcementId}");
        }

        public IActionResult Details(string id)
        {
            var announcementDatailsModel = this.announcementsService
                .GetDetails<AnnouncementDetailsViewModel>(id);

            return this.View(announcementDatailsModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var currentUrl = this.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value;

            await this.announcementsService.DeleteAsync(id);

            return this.Redirect(currentUrl);
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            var currentLoggedUser = this.userManager.GetUserId(this.HttpContext.User);
            var viewModel = this.announcementsService.GetDetails<UpdateAnnouncementViewModel>(id);

            if (currentLoggedUser != viewModel.UserId)
            {
                return this.RedirectToAction("AccesDenied");
            }

            viewModel.CitiesItems = this.citiesService.GetAllAsKeyValuePairs();
            viewModel.Categories = this.categoriesService.GetCategoriesAndSubCategories<AnnouncementCategoryInputModel>();
            viewModel.TagsItems = this.tagsService.GetAll<AnnouncementTagsInputModel>();

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateAnnouncementViewModel inputModel, string id)
        {

            if (!this.ModelState.IsValid)
            {
                var currentInput = new UpdateAnnouncementViewModel
                {
                    Id = id,
                    Title = inputModel.Title,
                    Description = inputModel.Description,
                    Price = inputModel.Price,
                    CityId = inputModel.CityId,
                    SubCategoryId = inputModel.SubCategoryId,
                };

                currentInput.TagsItems = this.tagsService.GetAll<AnnouncementTagsInputModel>();
                currentInput.CitiesItems = this.citiesService.GetAllAsKeyValuePairs();
                currentInput.Categories = this.categoriesService.GetCategoriesAndSubCategories<AnnouncementCategoryInputModel>();

                return this.View(inputModel);
            }

            inputModel.UserId = this.userManager.GetUserId(this.HttpContext.User);

            var announcementId = await this.announcementsService.UpdateAsync(inputModel.AnnouncementInputModel, id);

            return this.Redirect($"/Announcements/Details/{announcementId}");
        }

        public async Task<IActionResult> DeleteAnnouncementPhoto(string announcementId, string imgUrl)
        {
            await this.announcementsService.DeleteAnnouncementPhoto(imgUrl, announcementId);

            return this.RedirectToAction("Details", new { id = announcementId });
        }
    }
}
