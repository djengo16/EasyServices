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

        T GetById<T>(int id);

        SubCategory GetById(int id);

        Task<SubCategory> AddSubCategory(AddSubCategoryInputModel inputModel);

        Task EditSubCategory(EditSubCategoryModel inputModel);

        Task DeleteSubCategory(int categoryId);

        int GetCount();
    }
}
