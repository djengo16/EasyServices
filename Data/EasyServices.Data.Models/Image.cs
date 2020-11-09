namespace EasyServices.Data.Models
{
    using System;

    using EasyServices.Data.Common.Models;

    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Url { get; set; }

        public string AnnouncementId { get; set; }

        public Announcement Announcement { get; set; }

    }
}
