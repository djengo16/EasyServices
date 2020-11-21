namespace EasyServices.Web.Areas.Administration.Controllers
{
    using System.Linq;

    using EasyServices.Services.Data;
    using EasyServices.Web.ViewModels.Administration.Dashboard;
    using EasyServices.Web.ViewModels.Categories;
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;
        private readonly ICategoriesService categoriesService;

        public DashboardController(ISettingsService settingsService, ICategoriesService categoriesService)
        {
            this.settingsService = settingsService;
            this.categoriesService = categoriesService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                Categories = this.categoriesService.GetAll<CategoryViewModel>()
               .OrderBy(x => x.CreatedOn),
            };

            return this.View(viewModel);
        }
    }
}
