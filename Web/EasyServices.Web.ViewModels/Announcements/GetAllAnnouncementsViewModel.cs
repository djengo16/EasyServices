namespace EasyServices.Web.ViewModels.Announcements
{
    using System.Collections.Generic;

    public class GetAllAnnouncementsViewModel
    {
        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<AnnouncementViewModel> Announcements { get; set; }
    }
}
