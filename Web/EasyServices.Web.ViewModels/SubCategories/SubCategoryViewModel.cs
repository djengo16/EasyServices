namespace EasyServices.Web.ViewModels.SubCategories
{
    using System.Collections.Generic;

    using AspNetCoreTemplate.Web.ViewModels;
    using AutoMapper;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.Announcements;

    public class SubCategoryViewModel : PaggingViewModel, IMapFrom<SubCategory>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AnnouncementsCount { get; set; }

        public string CategoryName { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<AnnouncementViewModel> Announcements { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<SubCategory, SubCategoryViewModel>()
                .ForMember(x => x.Announcements, opt => opt.Ignore());
        }
    }
}