namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EasyServices.Data.Models;
    using EasyServices.Web.ViewModels.Administration.SubCategories;

    public interface ISubCategoriesService
    {
        IEnumerable<T> GetAllByCategoryId<T>(int id);

        IEnumerable<SubCategory> GetAllByCategoryId(int id);

        string GetNameById(int id);

        int GetCategoryId(int subcategoryId);

        int GetIdByName(string name);

        T GetById<T>(int id);

        Task AddSubCategory(AddSubCategoryInputModel inputModel);

        Task EditSubCategory(EditSubCategoryModel inputModel);

        Task DeleteSubCategory(int categoryId);

        int GetCount();
    }
}
