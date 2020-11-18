namespace EasyServices.Web.ViewModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    public class ReviewInputModel
    {
        public int Rate { get; set; }

        public string AnnouncementId { get; set; }

        [MaxLength(150)]
        public string Comment { get; set; }
    }
}
