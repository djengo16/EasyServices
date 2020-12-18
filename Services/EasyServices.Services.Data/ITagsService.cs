namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EasyServices.Data.Models;
    using EasyServices.Web.ViewModels.Announcements;

    public interface ITagsService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

        IEnumerable<T> GetAll<T>();

        bool CheckIfExist(string tagName);

        Task<int> CreateAsync(string tagName);

        int FindTagId(string tagName);

        List<string> GetNamesByAnnouncementId(string announcementId);

        Task GetOrUpdateTagsAsync(AnnouncementInputModel announcementInputModel, Announcement announcement);
    }
}
