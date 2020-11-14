namespace EasyServices.Web.ViewModels.Announcements
{
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class AnnouncementSubCategoryInputModel : IMapFrom<SubCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}