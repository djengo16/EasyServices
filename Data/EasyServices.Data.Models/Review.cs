namespace EasyServices.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using EasyServices.Data.Common.Models;

    public class Review : BaseDeletableModel<int>
    {
        [MaxLength(5)]
        public int Rating { get; set; }

        [MaxLength(100)]
        public string Comment { get; set; }

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        [ForeignKey(nameof(Models.Announcement))]
        public string AnnouncementId { get; set; }

        public Announcement Announcement { get; set; }
    }
}
