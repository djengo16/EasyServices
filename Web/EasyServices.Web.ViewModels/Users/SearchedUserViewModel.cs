namespace EasyServices.Web.ViewModels.Users
{
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class SearchedUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ProfilePicture { get; set; }

        public string Email { get; set; }
    }
}
