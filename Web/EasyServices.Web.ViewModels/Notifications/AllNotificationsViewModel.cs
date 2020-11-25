namespace EasyServices.Web.ViewModels.Notifications
{
    using System.Collections.Generic;

    public class AllNotificationsViewModel
    {
        public IEnumerable<NotificationViewModel> Notifications { get; set; }
    }
}
