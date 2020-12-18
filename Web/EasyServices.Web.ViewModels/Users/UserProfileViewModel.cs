namespace EasyServices.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.Announcements;

    public class UserProfileViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string WebSite { get; set; }

        public string PhoneNumber { get; set; }

        public string ProfilePicture { get; set; }

        public string Description { get; set; }

        public ICollection<AnnouncementViewModel> Announcements { get; set; }
    }
}
