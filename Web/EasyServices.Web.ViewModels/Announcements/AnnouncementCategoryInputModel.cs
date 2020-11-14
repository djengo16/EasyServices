namespace EasyServices.Web.ViewModels.Announcements
{
    using System.Collections.Generic;

    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class AnnouncementCategoryInputModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<AnnouncementSubCategoryInputModel> SubCategories { get; set; }
    }
}