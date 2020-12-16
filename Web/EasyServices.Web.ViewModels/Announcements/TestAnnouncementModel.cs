namespace EasyServices.Web.ViewModels.Announcements
{
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class TestAnnouncementModel : IMapFrom<Announcement>
    {
        public string Id { get; set; }

        public int SubCategoryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }

        public int CityId { get; set; }
    }
}
