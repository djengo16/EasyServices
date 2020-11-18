namespace EasyServices.Web.ViewModels.SubCategories
{
    using System.Collections.Generic;

    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class SubCategoryViewModel : IMapFrom<SubCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AnnouncementsCount { get; set; }

        public string CategoryName { get; set; }

        public int CategoryId { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

        public IEnumerable<AnnouncementViewModel> ServiceAnnouncements { get; set; }
    }
}