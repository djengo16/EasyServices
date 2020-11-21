namespace EasyServices.Web.ViewModels.Administration.SubCategories
{
    using System;

    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class SubCategoryInAdministration : IMapFrom<SubCategory>
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public int AnnouncementsCount { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
