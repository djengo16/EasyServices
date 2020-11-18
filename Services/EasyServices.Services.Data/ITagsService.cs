namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITagsService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();

        IEnumerable<T> GetAll<T>();

        bool CheckIfExist(string tagName);

        Task<int> Create(string tagName);

        int FindTagId(string tagName);
    }
}
