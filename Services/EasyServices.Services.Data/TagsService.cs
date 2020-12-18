namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;
    using EasyServices.Web.ViewModels.Announcements;

    public class TagsService : ITagsService
    {
        private const int TagLength = 50;
        private readonly IRepository<Tag> tagsRepository;
        private readonly IRepository<AnnouncementTag> announcementTagsRepository;

        public TagsService(
            IRepository<Tag> tagsRepository,
            IRepository<AnnouncementTag> announcementTagsRepository)
        {
            this.tagsRepository = tagsRepository;
            this.announcementTagsRepository = announcementTagsRepository;
        }

        public bool CheckIfExist(string tagName)
        {
            return this.tagsRepository.All().Any(x => x.Name == tagName);
        }

        public async Task<int> CreateAsync(string tagName)
        {
            var newTag = new Tag
            {
                Name = tagName,
            };

            await this.tagsRepository.AddAsync(newTag);
            await this.tagsRepository.SaveChangesAsync();

            return newTag.Id;
        }

        public int FindTagId(string tagName)
        {
            return this.tagsRepository.All().FirstOrDefault(x => x.Name == tagName).Id;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var query = this.tagsRepository.All();

            return query.To<T>().ToList();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.tagsRepository.All()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                }).ToList().Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }

        public List<string> GetNamesByAnnouncementId(string announcementId)
        {
            var result = this.announcementTagsRepository.All()
                .Where(x => x.AnnouncementId == announcementId)
                .Select(x => x.Tag.Name)
                .ToList();

            return result;
        }

        public async Task GetOrUpdateTagsAsync(
            AnnouncementInputModel announcementInputModel,
            Announcement announcement)
        {
            var actualAnnouncementTags =
                this.announcementTagsRepository
                .All().Where(x => x.AnnouncementId == announcement.Id)
                .Select(x => new
                {
                   TagName = x.Tag.Name,
                   x.TagId,
                })
                .ToList();

            foreach (var tag in announcementInputModel.Tags)
            {
                int tagId;
                if (tag.Length > TagLength)
                {
                    continue;
                }

                if (!this.CheckIfExist(tag))
                {
                    tagId = await this.CreateAsync(tag);
                }
                else
                {
                    tagId = this.FindTagId(tag);
                }

                if (!actualAnnouncementTags.Any(x => x.TagId == tagId))
                {
                    announcement.Tags.Add(new AnnouncementTag
                    {
                        TagId = tagId,
                        Announcement = announcement,
                    });
                }
            }

            // Delete tags if user removed them while editing the announcement
            // Only if type is UpdateAnnouncementInputModel..
            if (announcementInputModel.GetType() == typeof(UpdateAnnouncementInputModel))
            {
                foreach (var currentTag in actualAnnouncementTags)
                {
                    if (!announcementInputModel.Tags.Any(x => x == currentTag.TagName))
                    {
                        var tagToRemove = this.announcementTagsRepository.All()
                            .FirstOrDefault(x => x.TagId == currentTag.TagId && x.AnnouncementId == announcement.Id);
                        this.announcementTagsRepository.Delete(tagToRemove);
                    }
                }

                await this.announcementTagsRepository.SaveChangesAsync();
            }
        }
    }
}
