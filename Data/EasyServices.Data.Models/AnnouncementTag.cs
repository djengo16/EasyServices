namespace EasyServices.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class AnnouncementTag
    {
        [Required]
        public string AnnouncementId { get; set; }

        public virtual Announcement Announcement { get; set; }

        public int TagId { get; set; }

        public virtual Tag Tag { get; set; }
    }
}
