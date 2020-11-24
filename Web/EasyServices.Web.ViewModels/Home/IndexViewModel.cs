namespace EasyServices.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using EasyServices.Web.ViewModels.Announcements;

    public class IndexViewModel
    {
        public IEnumerable<AnnouncementViewModel> Announcements { get; set; }

        public IEnumerable<AnnouncementCategoryInputModel> Categories { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CitiesItems { get; set; }
    }
}
