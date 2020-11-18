namespace EasyServices.Web.ViewModels.Reviews
{
    using AutoMapper;
    using EasyServices.Common;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class ReviewInAnnouncementViewModel : IMapFrom<Review>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public string UserProfilePicture { get; set; }

        public string UserName { get; set; }

        public string UserId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Review, ReviewInAnnouncementViewModel>()
                 .ForMember(
                 x => x.UserProfilePicture,
                 c => c.MapFrom(t => t.User.ProfilePicture == null ? GlobalConstants.DefaultProfilePicture : t.User.ProfilePicture));
        }
    }
}