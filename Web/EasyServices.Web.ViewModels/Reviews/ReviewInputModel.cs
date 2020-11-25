namespace EasyServices.Web.ViewModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    using EasyServices.Data.Models;

    public class ReviewInputModel
    {
        public int Rate { get; set; }

        public string AnnouncementId { get; set; }

        public Announcement Announcement { get; set; }

        [MaxLength(150)]
        public string Comment { get; set; }

        public ApplicationUser User { get; set; }
    }
}
