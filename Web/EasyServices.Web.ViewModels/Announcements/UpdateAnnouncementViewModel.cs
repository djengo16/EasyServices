namespace EasyServices.Web.ViewModels.Announcements
{
    using System.Collections.Generic;

    public class UpdateAnnouncementViewModel : AnnouncementInputModel
    {
        public string Id { get; set; }

        public IEnumerable<string> ImagesUrl { get; set; }

    }
}
