namespace EasyServices.Web.ViewModels.Announcements
{

    using System.ComponentModel.DataAnnotations;

    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class AnnouncementTagInputModel : IMapFrom<Tag>
    {
        [MaxLength(50)]
        public string Name { get; set; }
    }
}