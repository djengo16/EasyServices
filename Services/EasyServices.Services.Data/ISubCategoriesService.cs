namespace EasyServices.Services.Data
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using EasyServices.Web.ViewModels.Administration.SubCategories;

    public interface ISubCategoriesService
    {
        IEnumerable<T> GetAllByCategoryId<T>(int id);

        string GetNameById(int id);

        string GetCategoryName(int subCategoryId);

        int GetCategoryId(int subcategoryId);

        int GetIdByName(string name);

        T GetById<T>(int id);

        Task AddSubCategory(AddSubCategoryInputModel inputModel);

        Task EditSubCategory(EditSubCategoryModel inputModel);

        Task DeleteSubCategory(int categoryId);

        int GetCount();
    }
}
