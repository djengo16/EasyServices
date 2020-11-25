namespace EasyServices.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EasyServices.Data.Models;

    public interface INotificationsService
    {
        Task AddNotificationFromReviewAsync(Review review);

        int GetUnreadUserNotificationsCount(string userId);

        Task DeleteAsync(int notificationId);

        Task SeeNotificationAsync(int norificationId);

        IEnumerable<T> GetAllByUserId<T>(string userId);

        string GetNotificationTime(DateTime createdOn);
    }
}
