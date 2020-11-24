﻿namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EasyServices.Data.Models;

    using EasyServices.Web.ViewModels.Announcements;

    public interface IAnnouncementsService
    {
        IEnumerable<T> GetAllBySubCategoryId<T>(int subCategoryId, int? take = null, int skip = 0);

        Task DeleteAsync(string id);

        T GetDetails<T>(string id);

        Task<string> CreateAsync(AnnouncementInputModel announcementInputModel);

        int GetCountBySubCategoryId(int subCategoryId);

        Task DeleteAnnouncementPhoto(string imgUrl, string announcementId);

        Task<string> UpdateAsync(UpdateAnnouncementViewModel announcementInputModel, string id);

        IEnumerable<T> GetLast<T>(int count);

        IEnumerable<T> GetBySearchParams<T>(int? cityId, int? subCategoryId, string keywords);

        Announcement GetById(string id);

    }
}
