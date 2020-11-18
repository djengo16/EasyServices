namespace EasyServices.Web.ViewModels.Users
{
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class UserAnnouncementViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfilePicture { get; set; }
    }
}
