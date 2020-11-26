namespace EasyServices.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using EasyServices.Data;
    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.Announcements;
    using Microsoft.EntityFrameworkCore;

    public class AnnouncementsService : IAnnouncementsService
    {
        private readonly IDeletableEntityRepository<Announcement> announcementsRepository;
        private readonly IRepository<Image> imagesRepository;
        private readonly Cloudinary cloudinary;
        private readonly ISubCategoriesService subCategoriesService;
        private readonly ITagsService tagsService;
        private readonly ICitiesService citiesService;
        private readonly ApplicationDbContext context;

        public AnnouncementsService(
            IDeletableEntityRepository<Announcement> announcementsRepository,
            IRepository<Image> imagesRepository,
            Cloudinary cloudinary,
            ISubCategoriesService subCategoriesService,
            ITagsService tagsService,
            ICitiesService citiesService,
            ApplicationDbContext context)
        {
            this.announcementsRepository = announcementsRepository;
            this.imagesRepository = imagesRepository;
            this.cloudinary = cloudinary;
            this.subCategoriesService = subCategoriesService;
            this.tagsService = tagsService;
            this.citiesService = citiesService;
            this.context = context;
        }

        public async Task<string> CreateAsync(AnnouncementInputModel announcementInputModel)
        {
            var announcement = new Announcement
            {
                SubCategoryId = announcementInputModel.SubCategoryId,
                Title = announcementInputModel.Title,
                Description = announcementInputModel.Description,
                City = this.citiesService.GetCityById(announcementInputModel.CityId),
                Price = announcementInputModel.Price,
                UserId = announcementInputModel.UserId,
                CategoryId = this.subCategoriesService.GetCategoryId(announcementInputModel.SubCategoryId),
            };

            await this.AddImagesToAnnouncement(announcementInputModel, announcement);

            await this.GetOrCreateTagsAsync(announcementInputModel, announcement);

            await this.announcementsRepository.AddAsync(announcement);
            await this.announcementsRepository.SaveChangesAsync();

            return announcement.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var announcement = await this.announcementsRepository.GetByIdWithDeletedAsync(id);

            foreach (var image in announcement.Images)
            {
                await CloudinaryHelper.RemoveFileAsync(this.cloudinary, image.Url);
            }

            this.announcementsRepository.Delete(announcement);
            await this.announcementsRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllBySubCategoryIdAsync<T>(int subCategoryId, int? take = null, int skip = 0)
        {
            var query = this.announcementsRepository
                .All()
                .OrderBy(x => x.Reviews.Count)
                .ThenByDescending(x => x.CreatedOn)
                .Where(x => x.SubCategoryId == subCategoryId).Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return await query.To<T>().ToListAsync();
        }

        public async Task<int> GetCountBySubCategoryIdAsync(int subCategoryId)
        {
            return await this.announcementsRepository.All().CountAsync(x => x.SubCategoryId == subCategoryId);
        }

        public async Task<Announcement> GetByIdAsync(string id)
        {
            return await this.announcementsRepository.All().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> GetDetailsAsync<T>(string id)
        {
            var announcement =
                 await this.announcementsRepository
                     .All()
                     .Where(x => x.Id == id)
                     .To<T>()
                     .FirstOrDefaultAsync();

            return announcement;
        }

        public async Task<string> UpdateAsync(UpdateAnnouncementViewModel announcementInputModel, string id)
        {
            var announcement = this.announcementsRepository.All().FirstOrDefault(x => x.Id == id);

            announcement.Title = announcementInputModel.Title;
            announcement.Description = announcementInputModel.Description;
            announcement.Price = announcementInputModel.Price;
            announcement.SubCategoryId = announcementInputModel.SubCategoryId;
            announcement.CategoryId = this.subCategoriesService.GetCategoryId(announcementInputModel.SubCategoryId);
            announcement.CityId = announcementInputModel.CityId;

            await this.AddImagesToAnnouncement(announcementInputModel, announcement);

            this.context.AnnouncementTags
                .RemoveRange(this.context.AnnouncementTags.Where(x => x.AnnouncementId == announcement.Id).ToList());

            await this.GetOrCreateTagsAsync(announcementInputModel, announcement);

            await this.announcementsRepository.SaveChangesAsync();

            return announcement.Id;
        }

        public async Task DeleteAnnouncementPhoto(string imgUrl, string announcementId)
        {
            var announcement = await this.announcementsRepository.GetByIdWithDeletedAsync(announcementId);
            var imageToRemove = await this.imagesRepository.All().FirstOrDefaultAsync(x => x.Url == imgUrl);

            this.imagesRepository.Delete(imageToRemove);
            await this.imagesRepository.SaveChangesAsync();

            await CloudinaryHelper.RemoveFileAsync(this.cloudinary, imgUrl);
        }

        public async Task<IEnumerable<T>> GetLastAsync<T>(int count)
        {
            var announcements =
                 this.announcementsRepository
                     .All()
                     .OrderByDescending(x => x.CreatedOn)
                     .Take(count)
                     .To<T>();

            return await announcements.ToListAsync();
        }

        public IEnumerable<T> GetBySearchParams<T>(int? cityId, int? subCategoryId, string keywords)
        {
            var queryModel =
                this.announcementsRepository.All()
                .Where(x => cityId != null ? x.CityId == cityId : true &&
                subCategoryId != null ? x.SubCategoryId == subCategoryId : true);

            if (keywords != null)
            {
                string[] keywordsArr = keywords.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var keyword in keywordsArr)
                {
                    queryModel = queryModel.Where(x =>
                    x.Description.ToLower().Contains(keyword.ToLower())
                    || x.Title.ToLower().Contains(keyword.ToLower())
                    || x.Tags.Select(x => x.Tag.Name).Contains(keyword));
                }
            }

            return queryModel.To<T>().ToList();
        }

        private async Task GetOrCreateTagsAsync(AnnouncementInputModel announcementInputModel, Announcement announcement)
        {
            foreach (var tag in announcementInputModel.Tags)
            {
                int tagId;
                if (tag.Length > 50)
                {
                    continue;
                }

                if (!this.tagsService.CheckIfExist(tag))
                {
                    tagId = await this.tagsService.Create(tag);
                }
                else
                {
                    tagId = this.tagsService.FindTagId(tag);
                }

                announcement.Tags.Add(new AnnouncementTag
                {
                    TagId = tagId,
                    Announcement = announcement,
                });
            }
        }

        private async Task AddImagesToAnnouncement(AnnouncementInputModel announcementInputModel, Announcement announcement)
        {
            if (announcementInputModel.Images != null)
            {
                foreach (var image in announcementInputModel.Images)
                {
                    announcement.Images.Add(new Image
                    {
                        AnnouncementId = announcement.Id,
                        Url = await CloudinaryHelper.UploadFileAsync(this.cloudinary, image, false),
                    });
                }
            }
        }
    }
}
