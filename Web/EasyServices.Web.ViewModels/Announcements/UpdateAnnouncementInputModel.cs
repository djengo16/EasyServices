namespace EasyServices.Web.ViewModels.Announcements
{
    using System.Collections.Generic;

    public class UpdateAnnouncementInputModel : AnnouncementInputModel
    {
        public string Id { get; set; }

        public IEnumerable<string> ImagesUrl { get; set; }

    }
}
