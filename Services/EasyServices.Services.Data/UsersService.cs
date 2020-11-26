namespace EasyServices.Services.Data
{
    using System.Linq;

    using EasyServices.Data.Common.Repositories;
    using EasyServices.Data.Models;
    using EasyServices.Services.Mapping;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public T GetUserById<T>(string userId)
        {
            var userModel = this.usersRepository
                .All().Where(x => x.Id == userId).To<T>().FirstOrDefault();

            return userModel;
        }
    }
}
