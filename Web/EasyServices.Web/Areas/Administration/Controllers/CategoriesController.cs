namespace EasyServices.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Services.Data;
    using EasyServices.Web.ViewModels.Administration.Categories;
    using EasyServices.Web.ViewModels.Administration.SubCategories;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;
        private readonly ISubCategoriesService subCategoriesService;

        public CategoriesController(ICategoriesService categoriesService, ISubCategoriesService subCategoriesService)
        {
            this.categoriesService = categoriesService;
            this.subCategoriesService = subCategoriesService;
        }


        [HttpPost]
        public async Task<IActionResult> Create(AddCategoryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Administration/Dashboard");
            }

            await this.categoriesService.AddCategoryAsync(inputModel);

            return this.Redirect("/Administration/Dashboard");
        }

        public async Task<IActionResult> Edit(EditCategoryModel editModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Administration/Dashboard");
            }

            await this.categoriesService.EditCategoryAsync(editModel);

            return this.Redirect("/Administration/Dashboard");
        }

        public async Task<IActionResult> Delete(int id)
        {

            await this.categoriesService.DeleteCategory(id);

            return this.Redirect("/Administration/Dashboard");
        }

        public IActionResult ById(int id)
        {
            var viewModel = new SubCategoryIndexModel
            {
                CategoryName = this.categoriesService.GetNameById(id),
                CategoryId = id,
                SubCategories = this.subCategoriesService
                .GetAllByCategoryId<SubCategoryInAdministration>(id).OrderBy(x => x.CreatedOn),
            };

            return this.View(viewModel);
        }
    }
}
