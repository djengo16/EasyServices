namespace EasyServices.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;
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

            if (delta < 45 * 60)
            {
                return "Преди " + timeSpan.Minutes + " минути.";
            }

            if (delta < 90 * 60)
            {
                return "Преди час.";
            }

            if (delta < 24 * 60 * 60)
            {
                return "Преди " + timeSpan.Hours + " часа.";
            }

            if (delta < 48 * 60 * 60)
            {
                return "От вчера.";
            }

            if (delta < 30 * 24 * 60 * 60)
            {
                return "Преди " + timeSpan.Days + " дена.";
            }

            if (delta < 12 * 30 * 24 * 60 * 60)
            {
                int months = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 30));
                return months <= 1 ? "Преди месец." : "Преди " + months + " месеца.";
            }

            int years = Convert.ToInt32(Math.Floor((double)timeSpan.Days / 365));
            return years <= 1 ? "Преди година" : "Преди " + years + " години.";
        }
    }
}
