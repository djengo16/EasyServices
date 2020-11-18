namespace EasyServices.Web.ViewModels.SubCategories
{
    using System.Collections.Generic;

    public class GetAllSubCategoriesViewModel
    {
        public string CategoryName { get; set; }

        public IEnumerable<SubCategoryViewModel> SubCategories { get; set; }
    }
}
