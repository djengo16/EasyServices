namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.Announcements;
    using CloudinaryDotNet;

    public class AnnouncementsService : IAnnouncementsService
    {
        private readonly IDeletableEntityRepository<Announcement> announcementsRepository;
        private readonly IRepository<Image> imagesRepository;
        private readonly Cloudinary cloudinary;
        private readonly ISubCategoriesService subCategoriesService;
        private readonly ITagsService tagsService;
        private readonly ICitiesService citiesService;

        public AnnouncementsService(
            IDeletableEntityRepository<Announcement> announcementsRepository,
            IRepository<Image> imagesRepository,
            Cloudinary cloudinary,
            ISubCategoriesService subCategoriesService,
            ITagsService tagsService,
            ICitiesService citiesService)
        {
            this.announcementsRepository = announcementsRepository;
            this.imagesRepository = imagesRepository;
            this.cloudinary = cloudinary;
            this.subCategoriesService = subCategoriesService;
            this.tagsService = tagsService;
            this.citiesService = citiesService;
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

            await this.GetOrCreateTags(announcementInputModel, announcement);

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

        public IEnumerable<T> GetAllBySubCategoryId<T>(int subCategoryId, int? take = null, int skip = 0)
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

            return query.To<T>().ToList();
        }

        public int GetCountBySubCategoryId(int subCategoryId)
        {
            return this.announcementsRepository.All().Count(x => x.SubCategoryId == subCategoryId);
        }

        public T GetDetails<T>(string id)
        {
            var announcement =
                 this.announcementsRepository
                     .All()
                     .Where(x => x.Id == id)
                     .To<T>()
                     .FirstOrDefault();

            return announcement;
        }

        public async Task<string> UpdateAsync(AnnouncementInputModel announcementInputModel, string id)
        {
            var announcement = this.announcementsRepository.All().FirstOrDefault(x => x.Id == id);

            announcement.Title = announcementInputModel.Title;
            announcement.Description = announcementInputModel.Description;
            announcement.Price = announcementInputModel.Price;
            announcement.SubCategoryId = announcementInputModel.SubCategoryId;
            announcement.CategoryId = this.subCategoriesService.GetCategoryId(announcementInputModel.SubCategoryId);
            announcement.CityId = announcementInputModel.CityId;

            await this.AddImagesToAnnouncement(announcementInputModel, announcement);

            await this.GetOrCreateTags(announcementInputModel, announcement);

            await this.announcementsRepository.SaveChangesAsync();

            return announcement.Id;
        }

        public async Task DeleteAnnouncementPhoto(string imgUrl, string announcementId)
        {
            var announcement = await this.announcementsRepository.GetByIdWithDeletedAsync(announcementId);
            var imageToRemove = this.imagesRepository.All().FirstOrDefault(x => x.Url == imgUrl);

            this.imagesRepository.Delete(imageToRemove);
            await this.imagesRepository.SaveChangesAsync();

            await CloudinaryHelper.RemoveFileAsync(this.cloudinary, imgUrl);
        }

        private async Task GetOrCreateTags(AnnouncementInputModel announcementInputModel, Announcement announcement)
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
