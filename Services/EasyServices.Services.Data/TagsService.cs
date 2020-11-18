namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class TagsService : ITagsService
    {
        private readonly ApplicationDbContext dbContext;

        public TagsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool CheckIfExist(string tagName)
        {
            return this.dbContext.Tags.Any(x => x.Name == tagName);
        }

        public async Task<int> Create(string tagName)
        {
            var newTag = new Tag
            {
                Name = tagName,
            };

            await this.dbContext.Tags.AddAsync(newTag);
            await this.dbContext.SaveChangesAsync();

            return newTag.Id;
        }

        public int FindTagId(string tagName)
        {
            return this.dbContext.Tags.FirstOrDefault(x => x.Name == tagName).Id;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var query =
                this.dbContext.Tags;

            return query.To<T>().ToList();
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
        {
            return this.dbContext.Tags
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                }).ToList().Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name));
        }
    }
}
