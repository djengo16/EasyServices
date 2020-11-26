namespace EasyServices.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Services.Data;
    using EasyServices.Web.ViewModels.Notifications;
    using Microsoft.AspNetCore.Mvc;

    public class NotificationsController : BaseController
    {
        private readonly INotificationsService notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            this.notificationsService = notificationsService;
        }

        public async Task<IActionResult> All(string userId)
        {
            var viewModel = new AllNotificationsViewModel
            {
                Notifications = await this.notificationsService.GetAllByUserIdAsync<NotificationViewModel>(userId),
            };

            foreach (var notification in viewModel.Notifications)
            {
                notification.NotificationTime = this.notificationsService.GetNotificationTime(notification.CreatedOn);
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await this.notificationsService.DeleteAsync(id);

            var currentUrl = this.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value;

            return this.Redirect(currentUrl);
        }

        public async Task<IActionResult> See(int id, string url)
        {
            await this.notificationsService.SeeNotificationAsync(id);
            return this.Redirect(url);
        }
    }
}
