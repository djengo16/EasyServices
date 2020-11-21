namespace EasyServices.Web.ViewModels.Announcements
{
    using System.Collections.Generic;

    public class AnnouncementsListViewModel : PaggingViewModel
    {
        public IEnumerable<AnnouncementViewModel> Announcements { get; set; }
    }
}
