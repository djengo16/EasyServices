namespace EasyServices.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EasyServices.Data.Models;

    public interface INotificationsService
    {
        Task AddNotificationFromReviewAsync(Review review);

        Task<int> GetUnreadUserNotificationsCount(string userId);

        Task DeleteAsync(int notificationId);

        Task SeeNotificationAsync(int norificationId);

        Task<IEnumerable<T>> GetAllByUserId<T>(string userId);

        string GetNotificationTime(DateTime createdOn);
    }
}
