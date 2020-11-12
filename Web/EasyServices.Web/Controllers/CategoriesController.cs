namespace EasyServices.Web.Controllers
{
    using EasyServices.Services.Data;
    using EasyServices.Web.ViewModels.Categories;
    using EasyServices.Web.ViewModels.SubCategories;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;
        private readonly ISubCategoriesService subCategoriesService;

        public CategoriesController(ICategoriesService categoriesService, ISubCategoriesService subCategoriesService)
        {
            this.categoriesService = categoriesService;
            this.subCategoriesService = subCategoriesService;
        }

        public IActionResult All()
        {
            var model = new GetAllCategoriesViewModel
            {
                Categories = this.categoriesService.GetAll<CategoryViewModel>(),
            };

            return this.View(model);
        }

        public IActionResult ById(int id)
        {
            var model = new GetAllSubCategoriesViewModel
            {
                CategoryName = this.categoriesService.GetNameById(id),
                SubCategories = this.subCategoriesService.GetAllByCategoryId<SubCategoryViewModel>(id),
            };

            return this.View(model);
        }
    }
}
