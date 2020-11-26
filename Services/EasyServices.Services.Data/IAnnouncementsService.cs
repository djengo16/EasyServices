namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EasyServices.Data.Models;

    using EasyServices.Web.ViewModels.Announcements;

    public interface IAnnouncementsService
    {
        Task<IEnumerable<T>> GetAllBySubCategoryIdAsync<T>(int subCategoryId, int? take = null, int skip = 0);

        Task DeleteAsync(string id);

        Task<T> GetDetailsAsync<T>(string id);

        Task<string> CreateAsync(AnnouncementInputModel announcementInputModel);

        Task<int> GetCountBySubCategoryIdAsync(int subCategoryId);

        Task DeleteAnnouncementPhoto(string imgUrl, string announcementId);

        Task<string> UpdateAsync(UpdateAnnouncementViewModel announcementInputModel, string id);

        Task<IEnumerable<T>> GetLastAsync<T>(int count);

        IEnumerable<T> GetBySearchParams<T>(int? cityId, int? subCategoryId, string keywords);

        Task<Announcement> GetByIdAsync(string id);

    }
}
