namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EasyServices.Data.Models;
    using EasyServices.Web.ViewModels.Administration.Categories;

    public interface ICategoriesService
    {
        IEnumerable<T> GetAll<T>();

        Task AddCategory(AddCategoryInputModel inputModel);

        Task EditCategory(EditCategoryModel inputModel);

        Task DeleteCategory(int categoryId);

        string GetShort<T>(IEnumerable<T> input);

        string GetNameById(int id);

        Category GetById(int id);

        ICollection<T> GetCategoriesAndSubCategories<T>();
    }
}
