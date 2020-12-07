namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EasyServices.Data.Models;
    using EasyServices.Web.ViewModels.Announcements;

    public interface IImagesService
    {
        IEnumerable<Image> GetAnnouncementImages(string announcementId);

        Task AddImagesToAnnouncement(AnnouncementInputModel announcementInputModel, Announcement announcement);

        Task DeleteAnnouncementPhoto(string imgUrl, string announcementId);
    }
}
