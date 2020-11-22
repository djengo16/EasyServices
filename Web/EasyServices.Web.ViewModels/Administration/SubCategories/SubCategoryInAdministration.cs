namespace EasyServices.Web.ViewModels.Administration.SubCategories
{
    using System;

    using AutoMapper;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class SubCategoryInAdministration : IMapFrom<SubCategory>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public int AnnouncementsCount { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<SubCategory, SubCategoryInAdministration>()
                 .ForMember(
                     x => x.AnnouncementsCount,
                     c => c.MapFrom(ac => ac.Announcements.Count));
        }
    }
}
