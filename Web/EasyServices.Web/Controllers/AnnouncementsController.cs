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
        private readonly ISubCategoriesService subCategoriesService;
        private readonly ITagsService tagsService;
        private readonly UserManager<ApplicationUser> userManager;

        public AnnouncementsController(
            IAnnouncementsService announcementsService,
            ICitiesService citiesService,
            ICategoriesService categoriesService,
            ISubCategoriesService subCategoriesService,
            ITagsService tagsService,
            UserManager<ApplicationUser> userManager)
        {
            this.announcementsService = announcementsService;
            this.citiesService = citiesService;
            this.categoriesService = categoriesService;
            this.subCategoriesService = subCategoriesService;
            this.tagsService = tagsService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var viewModel = new AnnouncementInputModel();
            viewModel.CitiesItems = this.citiesService.GetAllAsKeyValuePairs();
            viewModel.Categories = await this.categoriesService.GetCategoriesAndSubCategoriesAsync<AnnouncementCategoryInputModel>();
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
                inputModel.Categories = await this.categoriesService.GetCategoriesAndSubCategoriesAsync<AnnouncementCategoryInputModel>();

                return this.View(inputModel);
            }

            inputModel.UserId = this.userManager.GetUserId(this.HttpContext.User);

            var announcementId = await this.announcementsService.CreateAsync(inputModel);

            return this.Redirect($"/Announcements/Details/{announcementId}");
        }

        public async Task<IActionResult> Details(string id)
        {
            var announcementDatailsModel = await this.announcementsService
                .GetDetailsAsync<AnnouncementDetailsViewModel>(id);

            return this.View(announcementDatailsModel);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var currentUrl = this.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value;

            await this.announcementsService.DeleteAsync(id);

            return this.Redirect(currentUrl);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var currentLoggedUser = this.userManager.GetUserId(this.HttpContext.User);
            var announcement = await this.announcementsService.GetByIdAsync(id);

            var viewModel = new UpdateAnnouncementViewModel
            {
                UserId = announcement.UserId,
                Description = announcement.Description,
                Title = announcement.Title,
                Price = announcement.Price,
                SubCategoryId = announcement.SubCategoryId,
                CityId = announcement.CityId,
                CitiesItems = this.citiesService.GetAllAsKeyValuePairs(),
                Categories = await this.categoriesService.GetCategoriesAndSubCategoriesAsync<AnnouncementCategoryInputModel>(),
                TagsItems = this.tagsService.GetAll<AnnouncementTagsInputModel>(),
            };

            if (currentLoggedUser != announcement.UserId && !this.User.IsInRole("Administrator"))
            {
                return this.Redirect(this.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value;);
            }

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateAnnouncementViewModel inputModel, string id)
        {
            var announcement = await this.announcementsService.GetByIdAsync(id);
            var currentLoggedUser = this.userManager.GetUserId(this.HttpContext.User);

            if (currentLoggedUser != announcement.UserId && !this.User.IsInRole("Administrator"))
            {
                return this.Redirect(this.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value);
            }

            if (!this.ModelState.IsValid)
            {
                var currentInput = new UpdateAnnouncementViewModel
                {
                    UserId = currentLoggedUser,
                    Id = id,
                    Title = inputModel.Title,
                    Description = inputModel.Description,
                    Price = inputModel.Price,
                    CityId = inputModel.CityId,
                    SubCategoryId = inputModel.SubCategoryId,
                };

                currentInput.TagsItems = this.tagsService.GetAll<AnnouncementTagsInputModel>();
                currentInput.CitiesItems = this.citiesService.GetAllAsKeyValuePairs();
                currentInput.Categories = await this.categoriesService.GetCategoriesAndSubCategoriesAsync<AnnouncementCategoryInputModel>();

                return this.View(currentInput);
            }

            inputModel.UserId = this.userManager.GetUserId(this.HttpContext.User);

            var announcementId = await this.announcementsService.UpdateAsync(inputModel, id);

            return this.Redirect($"/Announcements/Details/{announcementId}");
        }

        public IActionResult Search(int? cityId, int? subCategoryId, string keywords)
        {
            var viewModel = new AnnouncementsFromSearchModel
            {
                SubCategoryName = subCategoryId == null ? null : this.subCategoriesService.GetNameById(subCategoryId.GetValueOrDefault()),
                CityName = cityId == null ? null : this.citiesService.GetCityById(cityId).Name,
                Keywords = keywords,
                Announcements = this.announcementsService
                .GetBySearchParams<AnnouncementViewModel>(cityId, subCategoryId, keywords)
                .OrderByDescending(x => x.CreatedOn),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> DeleteAnnouncementPhoto(string announcementId, string imgUrl)
        {
            await this.announcementsService.DeleteAnnouncementPhoto(imgUrl, announcementId);

            return this.RedirectToAction("Details", new { id = announcementId });
        }
    }
}
