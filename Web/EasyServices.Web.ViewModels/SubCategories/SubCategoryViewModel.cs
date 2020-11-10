namespace EasyServices.Web.ViewModels.SubCategories
{
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class SubCategoryViewModel : IMapFrom<SubCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AnnouncementsCount { get; set; }

        public string CategoryName { get; set; }
    }
}