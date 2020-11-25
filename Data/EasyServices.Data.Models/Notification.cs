namespace EasyServices.Data.Models
{
    using EasyServices.Data.Common.Models;

    public class Notification : BaseDeletableModel<int>
    {
        public string Content { get; set; }

        public bool IsSeen { get; set; }

        public string Destination { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
