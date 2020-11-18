namespace EasyServices.Web.ViewModels.Announcements
{
    using System.Collections.Generic;

    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class UpdateAnnouncementViewModel : IMapFrom<Announcement>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public string UserId { get; set; }

        public int CityId { get; set; }

        public string CityName { get; set; }

        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }

        public AnnouncementInputModel AnnouncementInputModel { get; set; }

        public IEnumerable<AnnouncementTag> Tags { get; set; }

        public IEnumerable<AnnouncementCategoryInputModel> Categories { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CitiesItems { get; set; }

        public IEnumerable<AnnouncementTagsInputModel> TagsItems { get; set; }
    }
}
