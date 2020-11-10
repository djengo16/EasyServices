namespace EasyServices.Services.Data
{
    using System.Collections.Generic;

    using EasyServices.Data.Models;

    public interface ICategoriesService
    {
        IEnumerable<T> GetAll<T>();

        string GetShort<T>(IEnumerable<T> input, int length);

        string GetNameById(int id);

        ICollection<Category> GetFromDb();

        ICollection<T> GetCategoriesAndSubCategories<T>();
    }
}
