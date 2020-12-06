namespace EasyServices.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using EasyServices.Services.Data;
    using EasyServices.Web.ViewModels.Administration.SubCategories;
    using Microsoft.AspNetCore.Mvc;

    public class SubCategoriesController : AdministrationController
    {
        private readonly ISubCategoriesService subCategoriesService;

        public SubCategoriesController(ISubCategoriesService subCategoriesService)
        {
            this.subCategoriesService = subCategoriesService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddSubCategoryInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect($"/Administration/Categories/ById/{inputModel.CategoryId}");
            }

            await this.subCategoriesService.AddSubCategory(inputModel);

            return this.Redirect($"/Administration/Categories/ById/{inputModel.CategoryId}");
        }

        public async Task<IActionResult> Edit(EditSubCategoryModel editModel)
        {
            int categoryId = this.subCategoriesService.GetCategoryId(editModel.Id);

            if (!this.ModelState.IsValid)
            {
                return this.Redirect($"/Administration/Categories/ById/{categoryId}");
            }

            await this.subCategoriesService.EditSubCategory(editModel);

            return this.Redirect($"/Administration/Categories/ById/{categoryId}");
        }

        public async Task<IActionResult> Delete(int id)
        {
            int categoryId = this.subCategoriesService.GetCategoryId(id);
            await this.subCategoriesService.DeleteSubCategory(id);

            return this.Redirect($"/Administration/Categories/ById/{categoryId}");
        }

    }
}
