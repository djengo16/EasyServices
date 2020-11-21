namespace EasyServices.Web.ViewModels.Administration.Dashboard
{
    using System.Collections.Generic;

    using EasyServices.Web.ViewModels.Administration.Categories;
    using EasyServices.Web.ViewModels.Categories;

    public class IndexViewModel
    {
        public AddCategoryInputModel AddCategoryModel { get; set; }

        public EditCategoryModel EditCategoryModel { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
