namespace EasyServices.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;
    using EasyServices.Services.Data.Common;
    using EasyServices.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class NotificationsService : INotificationsService
    {
        private readonly IDeletableEntityRepository<Notification> notificationsRepository;

        public NotificationsService(
            IDeletableEntityRepository<Notification> notificationsRepository)
        {
            this.notificationsRepository = notificationsRepository;
        }

        public async Task AddNotificationFromReviewAsync(Review review)
        {
            string reviewUserName = review.User.Name != null ? review.User.Name : review.User.Email;

            if (review.User.Id == review.Announcement.UserId)
            {
                return;
            }

            var newNotification = new Notification
            {
                UserId = review.Announcement.UserId,
                Content = string.Format(ServicesConstants.NewReviewNotificationMessage, reviewUserName),
                Destination = $"/Announcements/Details/{review.Announcement.Id}",
            };

            await this.notificationsRepository.AddAsync(newNotification);
            await this.notificationsRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int notificationId)
        {
            var notificationToDelete = await this.notificationsRepository
                .All().FirstAsync(x => x.Id == notificationId);

            this.notificationsRepository.Delete(notificationToDelete);
            await this.notificationsRepository.SaveChangesAsync();
        }

        public async Task<int> GetUnreadUserNotificationsCount(string userId)
        {
            return await this.notificationsRepository.All()
                .Where(x => x.UserId == userId && x.IsSeen == false)
                .CountAsync();
        }

        public async Task SeeNotificationAsync(int norificationId)
        {
            var notification = this.notificationsRepository.All()
                .FirstOrDefault(x => x.Id == norificationId);

            notification.IsSeen = true;

            await this.notificationsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllByUserIdAsync<T>(string userId)
        {
            var query = this.notificationsRepository.All()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedOn);

            return await query.To<T>().ToListAsync();
        }

        public string GetNotificationTime(DateTime createdOn)
        {
            var timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - createdOn.Ticks);
            double delta = Math.Abs(timeSpan.TotalSeconds);

            if (delta < 60)
            {
                return NotificationTime.RightNow;
            }

            if (delta < 60 * 2)
            {
                return NotificationTime.MinuteAgo;
            }

            if (delta < 45 * 60)
            {
                return string.Format(NotificationTime.MinutesAgo, timeSpan.Minutes);
            }

            if (delta < 90 * 60)
            {
                return NotificationTime.HourAgo;
            }

            if (delta < 24 * 60 * 60)
            {
                return string.Format(NotificationTime.HoursAgo, timeSpan.Hours);
            }

            if (delta < 48 * 60 * 60)
            {
                return NotificationTime.Yesterday;
            }

            if (delta < 30 * 24 * 60 * 60)
            {
                return string.Format(NotificationTime.DaysAgo, timeSpan.Days);
            }

            if (delta < 12 * 30 * 24 * 60 * 60)
            {
                int months = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 30));
                return months <= 1
                    ? NotificationTime.MonthAgo
                    : string.Format(NotificationTime.MonthsAgo, months);
            }

            int years = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 365));
            return years <= 1
                ? NotificationTime.YearAgo
                : string.Format(NotificationTime.YearsAgo, years);
        }
    }
}
