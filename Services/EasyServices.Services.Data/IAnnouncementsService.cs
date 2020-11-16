namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    // using EasyServices.Web.ViewModels.Announcements;

    public interface IAnnouncementsService
    {
        IEnumerable<T> GetAllBySubCategoryId<T>(int subCategoryId);

        Task DeleteAsync(string id);

        T GetDetails<T>(string id);

        // Task<string> CreateAsync(CreateAnnouncementInputModel announcementInputModel);
    }
}
