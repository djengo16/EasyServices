namespace EasyServices.Services.Data
{
    public interface IUsersService
    {
        T GetUserById<T>(string userId);
    }
}
