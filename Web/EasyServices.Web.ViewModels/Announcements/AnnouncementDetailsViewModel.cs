namespace EasyServices.Web.ViewModels.Announcements
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.Reviews;
    using EasyServices.Web.ViewModels.Users;
    using Ganss.XSS;

    public class AnnouncementDetailsViewModel : IMapFrom<Announcement>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string SubCategoryName { get; set; }

        public int SubCategoryId { get; set; }

        public int ReviewsCount { get; set; }

        public string CityName { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        public string TotalRating => this.ReviewsCount == 0 ? "0" : (this.Reviews.Sum(x => x.Rating) / this.ReviewsCount).ToString("0.0");

        public UserAnnouncementViewModel User { get; set; }

        public ICollection<string> Tags { get; set; }

        public ICollection<Image> Images { get; set; }

        public ICollection<ReviewInAnnouncementViewModel> Reviews { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Announcement, AnnouncementDetailsViewModel>()
                .ForMember(
                x => x.Tags,
                c => c.MapFrom(t => t.Tags.Select(tag => tag.Tag.Name)));
        }
    }
}
