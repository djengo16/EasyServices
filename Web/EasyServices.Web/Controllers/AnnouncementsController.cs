namespace EasyServices.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data.Models;
    using EasyServices.Services.Data;
    using EasyServices.Web.ViewModels.Announcements;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    public class AnnouncementsController : BaseController
    {
        private const int AnnouncementsPerPage = 9;

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

        public IActionResult Details(string id)
        {
            var announcementDatailsModel = this.announcementsService
                .GetDetails<AnnouncementDetailsViewModel>(id);

            return this.View(announcementDatailsModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await this.announcementsService.DeleteAsync(id);

            return this.Redirect("/Home/Index");
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
                ImagesUrl = announcement.Images.Select(x => x.Url).ToList(),
            };

            if (currentLoggedUser != announcement.UserId && !this.User.IsInRole("Administrator"))
            {
                return this.Redirect(this.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value);
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
                    ImagesUrl = announcement.Images.Select(x => x.Url).ToList(),
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

        public IActionResult Search(int? cityId, int? subCategoryId, string keywords, int page = 1)
        {
            var viewModel = new AnnouncementsFromSearchModel
            {
                SubCategoryName = subCategoryId == null ? null : this.subCategoriesService.GetNameById(subCategoryId.GetValueOrDefault()),
                SubCategoryId = subCategoryId,
                CityId = cityId,
                CityName = cityId == null ? null : this.citiesService.GetCityById(cityId).Name,
                Keywords = keywords,
                Announcements = this.announcementsService
                .GetBySearchParams<AnnouncementViewModel>(
                    cityId, subCategoryId, keywords, AnnouncementsPerPage, (page - 1) * AnnouncementsPerPage)
                .OrderByDescending(x => x.CreatedOn),
            };

            int count = this.announcementsService.GetCountFromSearched(cityId, subCategoryId, keywords);

            viewModel.PagesCount = (int)Math.Ceiling((double)count / AnnouncementsPerPage);

            viewModel.CurrentPage = page;

            return this.View(viewModel);
        }
    }
}
