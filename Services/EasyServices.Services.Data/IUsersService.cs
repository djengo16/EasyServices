namespace EasyServices.Services.Data
{
    using System.Collections;
    using System.Collections.Generic;

    public interface IUsersService
    {
        T GetUserById<T>(string userId);

        IEnumerable<T> GetBySearch<T>(string username);

        int GetCount();
    }
}
