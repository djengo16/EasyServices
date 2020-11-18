namespace EasyServices.Web.ViewModels.Announcements
{
    using System;
    using System.Linq;

    using EasyServices.Common;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;
    using AutoMapper;

    public class AnnouncementViewModel : IMapFrom<Announcement>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Title { get; set; }

        public string MainImage { get; set; }

        public string PriceAsString { get; set; }

        public string Username { get; set; }

        public string CityName { get; set; }

        public int ReviewsCount { get; set; }

        public string CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Announcement, AnnouncementViewModel>()
                .ForMember(
                x => x.MainImage,
                c => c.MapFrom(e => e.Images.Any() ? e.Images.FirstOrDefault().Url : GlobalConstants.DefaultAnnouncementImg))
                .ForMember(
                x => x.PriceAsString,
                c => c.MapFrom(e => e.Price == 0 || e.Price == null ? "По договаряне." : e.Price.ToString() + "лв."))
                .ForMember(
                x => x.CreatedOn,
                c => c.MapFrom(e => e.CreatedOn.ToString("dd/MM/yyyy")))
                .ForMember(
                x => x.Username,
                c => c.MapFrom(e => e.User.Name == null ? e.User.Email : e.User.Name));
        }
    }
}
