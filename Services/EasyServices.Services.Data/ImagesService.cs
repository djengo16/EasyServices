namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;
    using EasyServices.Web.ViewModels.Announcements;
    using Microsoft.EntityFrameworkCore;

    public class ImagesService : IImagesService
    {
        private readonly IRepository<Image> imagesRepository;
        private readonly Cloudinary cloudinary;

        public ImagesService(
            IRepository<Image> imagesRepository,
            Cloudinary cloudinary)
        {
            this.imagesRepository = imagesRepository;
            this.cloudinary = cloudinary;
        }

        public async Task DeleteAnnouncementPhoto(string imgUrl, string announcementId)
        {
            var imageToRemove = await this.imagesRepository.All().FirstOrDefaultAsync(x => x.Url == imgUrl);

            this.imagesRepository.Delete(imageToRemove);
            await this.imagesRepository.SaveChangesAsync();

            await CloudinaryHelper.RemoveFileAsync(this.cloudinary, imgUrl);
        }

        public IEnumerable<Image> GetAnnouncementImages(string announcementId)
        {
            return this.imagesRepository.All()
                .Where(x => x.AnnouncementId == announcementId)
                .ToList();
        }

        public async Task AddImagesToAnnouncement(AnnouncementInputModel announcementInputModel, Announcement announcement)
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
