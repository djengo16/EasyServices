﻿namespace EasyServices.Web.Controllers
{
    using EasyServices.Services.Data;
    using EasyServices.Web.ViewModels.SubCategories;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;

    public class SubCategoriesController : BaseController
    {
        private const int AnnouncementsPerPage = 6;

        private readonly IAnnouncementsService announcementsService;
        private readonly ISubCategoriesService subCategoriesService;

        public SubCategoriesController(
            IAnnouncementsService announcementsService,
            ISubCategoriesService subCategoriesService)
        {
            this.announcementsService = announcementsService;
            this.subCategoriesService = subCategoriesService;
        }

        public IActionResult ById(int id, int page = 1)
        {
            var model = this.subCategoriesService.GetById<SubCategoryViewModel>(id);

            model.ServiceAnnouncements = this.announcementsService
                .GetAllBySubCategoryId<AnnouncementViewModel>(id, AnnouncementsPerPage, (page - 1) * AnnouncementsPerPage);

            int count = this.announcementsService.GetCountBySubCategoryId(id);

            model.PagesCount = (int)Math.Ceiling((double)count / AnnouncementsPerPage);

            model.CurrentPage = page;
            ;
            return this.View(model);
        }
    }
}
