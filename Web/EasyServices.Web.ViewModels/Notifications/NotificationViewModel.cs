namespace EasyServices.Web.ViewModels.Notifications
{
    using System;

    using AutoMapper;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class NotificationViewModel : IMapFrom<Notification>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string SenderUserProfilePicture { get; set; }

        public bool IsSeen { get; set; }

        public string Destination { get; set; }

        public string UserId { get; set; }

        public string NotificationTime { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}
