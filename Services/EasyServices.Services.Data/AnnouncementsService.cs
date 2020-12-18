namespace EasyServices.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.Announcements;
    using Microsoft.EntityFrameworkCore;

    public class AnnouncementsService : IAnnouncementsService
    {
        private readonly IDeletableEntityRepository<Announcement> announcementsRepository;
        private readonly ISubCategoriesService subCategoriesService;
        private readonly ITagsService tagsService;
        private readonly IImagesService imagesService;

        public AnnouncementsService(
            IDeletableEntityRepository<Announcement> announcementsRepository,
            ISubCategoriesService subCategoriesService,
            ITagsService tagsService,
            IImagesService imagesService)
        {
            this.announcementsRepository = announcementsRepository;
            this.subCategoriesService = subCategoriesService;
            this.tagsService = tagsService;
            this.imagesService = imagesService;
        }

        public async Task<string> CreateAsync(AnnouncementInputModel announcementInputModel)
        {
            var announcement = new Announcement
            {
                SubCategoryId = announcementInputModel.SubCategoryId,
                Title = announcementInputModel.Title,
                Description = announcementInputModel.Description,
                CityId = announcementInputModel.CityId,
                Price = announcementInputModel.Price,
                UserId = announcementInputModel.UserId,
                CategoryId = this.subCategoriesService.GetCategoryId(announcementInputModel.SubCategoryId),
            };

            await this.imagesService.AddImagesToAnnouncement(announcementInputModel, announcement);

            await this.tagsService.GetOrUpdateTagsAsync(announcementInputModel, announcement);

            await this.announcementsRepository.AddAsync(announcement);
            await this.announcementsRepository.SaveChangesAsync();

            return announcement.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var announcement = await this.announcementsRepository.GetByIdWithDeletedAsync(id);

            foreach (var image in announcement.Images)
            {
                await this.imagesService.DeleteAnnouncementPhoto(image.Url, id);
            }

            this.announcementsRepository.Delete(announcement);
            await this.announcementsRepository.SaveChangesAsync();
        }

        public async Task<int> GetCountBySubCategoryIdAsync(int subCategoryId)
        {
            return await this.announcementsRepository.All().CountAsync(x => x.SubCategoryId == subCategoryId);
        }

        public async Task<Announcement> GetByIdAsync(string id)
        {
            var announcement = await this.announcementsRepository.All().FirstOrDefaultAsync(x => x.Id == id);

            announcement.Images = this.imagesService.GetAnnouncementImages(id).ToList();

            return announcement;
        }

        public T GetDetails<T>(string id)
        {
            var test = this.announcementsRepository.All().FirstOrDefault(x => x.Id == id);

            var announcement =
                   this.announcementsRepository
                     .All()
                     .Where(x => x.Id == id)
                     .To<T>()
                     .FirstOrDefault();

            return announcement;
        }

        public async Task<string> UpdateAsync(UpdateAnnouncementInputModel announcementInputModel, string id)
        {
            var announcement = await this.GetByIdAsync(id);

            announcement.Title = announcementInputModel.Title;
            announcement.Description = announcementInputModel.Description;
            announcement.Price = announcementInputModel.Price;
            announcement.SubCategoryId = announcementInputModel.SubCategoryId;
            announcement.CategoryId = this.subCategoriesService.GetCategoryId(announcementInputModel.SubCategoryId);
            announcement.CityId = announcementInputModel.CityId;

            // Check if user removed existing images and removing them
            var imagesToDelete = new List<string>();

            foreach (var currentImg in announcement.Images)
            {
                if (!announcementInputModel.ImagesUrl.Contains(currentImg.Url))
                {
                    imagesToDelete.Add(currentImg.Url);
                }
            }

            if (imagesToDelete.Any())
            {
                foreach (var current in imagesToDelete)
                {
                    await this.imagesService.DeleteAnnouncementPhoto(current, announcement.Id);
                }
            }

            // upload the new images if there's any
            if (announcementInputModel.Images != null)
            {
                await this.imagesService.AddImagesToAnnouncement(announcementInputModel, announcement);
            }

            if (announcementInputModel.Tags.Any())
            {
                await this.tagsService.GetOrUpdateTagsAsync(announcementInputModel, announcement);
            }

            await this.announcementsRepository.SaveChangesAsync();

            return announcement.Id;
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

        public IEnumerable<T> GetBySearchParams<T>(int? cityId, int? subCategoryId, string keywords, int? take = null, int skip = 0)
        {
            var test = this.announcementsRepository.All().ToList();

            var queryModel =
                this.announcementsRepository.All()
                .Where(x => cityId != null ? x.CityId == cityId : true);

            queryModel = queryModel
                .Where(x => subCategoryId != null ? x.SubCategoryId == subCategoryId : true);

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

            queryModel = take.HasValue ? queryModel.Skip(skip).Take(take.Value) : queryModel.Skip(skip);

            return queryModel.To<T>().ToList();
        }

        public int GetCount()
        {
            return this.announcementsRepository.All().Count();
        }

        public int GetCountFromSearched(int? cityId, int? subCategoryId, string keywords)
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

            return queryModel.Count();
        }
    }
}
