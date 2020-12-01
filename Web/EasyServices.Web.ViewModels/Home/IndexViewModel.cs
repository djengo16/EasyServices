namespace EasyServices.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using EasyServices.Web.ViewModels.Announcements;

    public class IndexViewModel
    {
        public int UsersCount { get; set; }

        public int SubCategoriesCount { get; set; }

        public int AnnouncementsCount { get; set; }

        public IEnumerable<AnnouncementViewModel> Announcements { get; set; }

        public IEnumerable<AnnouncementCategoryInputModel> Categories { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CitiesItems { get; set; }
    }
}
