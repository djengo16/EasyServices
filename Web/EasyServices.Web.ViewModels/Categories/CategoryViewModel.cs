namespace EasyServices.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.SubCategories;

    public class CategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImgUrl { get; set; }

        public IEnumerable<SubCategoryViewModel> SubCategories { get; set; }
    }
}
