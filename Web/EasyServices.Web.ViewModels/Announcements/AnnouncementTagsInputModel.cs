namespace EasyServices.Web.ViewModels.Announcements
{
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class AnnouncementTagsInputModel : IMapFrom<Tag>
    {
        public string Name { get; set; }
    }
}