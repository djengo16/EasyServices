namespace EasyServices.Web.ViewModels.Administration.SubCategories
{

    using System.Collections.Generic;

    public class SubCategoryIndexModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public EditSubCategoryModel EditSubCategoryModel { get; set; }

        public AddSubCategoryInputModel AddSubCategoryInputModel { get; set; }

        public IEnumerable<SubCategoryInAdministration> SubCategories { get; set; }
    }
}
