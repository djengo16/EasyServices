namespace EasyServices.Services.Data
{

    using System.Collections.Generic;

    public interface ISubCategoriesService
    {
        IEnumerable<T> GetAllByCategoryId<T>(int id);

        string GetNameById(int id);

        string GetCategoryName(int subCategoryId);

        int GetCategoryId(int subcategoryId);

        int GetIdByName(string name);

        T GetById<T>(int id);


    }
}
