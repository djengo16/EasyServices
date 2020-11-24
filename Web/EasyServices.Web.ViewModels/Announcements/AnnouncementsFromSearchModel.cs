namespace EasyServices.Web.ViewModels.Announcements
{
    using AspNetCoreTemplate.Web.ViewModels;
    using System.Collections.Generic;

    public class AnnouncementsFromSearchModel : PaggingViewModel
    {
        public string CityName { get; set; }

        public string SubCategoryName { get; set; }

        public string Keywords { get; set; }

        public IEnumerable<AnnouncementViewModel> Announcements { get; set; }
    }
}
