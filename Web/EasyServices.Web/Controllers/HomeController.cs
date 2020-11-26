namespace EasyServices.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using EasyServices.Services.Data;
    using EasyServices.Web.ViewModels;
    using EasyServices.Web.ViewModels.Announcements;
    using EasyServices.Web.ViewModels.Home;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IAnnouncementsService announcementsService;
        private readonly ICitiesService citiesService;
        private readonly ICategoriesService categoriesService;

        public HomeController(IAnnouncementsService announcementsService, ICitiesService citiesService, ICategoriesService categoriesService)
        {
            this.announcementsService = announcementsService;
            this.citiesService = citiesService;
            this.categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel viewModel = new IndexViewModel
            {
                Announcements = await this.announcementsService.GetLastAsync<AnnouncementViewModel>(8),
                CitiesItems = this.citiesService.GetAllAsKeyValuePairs(),
                Categories = await this.categoriesService.GetCategoriesAndSubCategoriesAsync<AnnouncementCategoryInputModel>(),
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
