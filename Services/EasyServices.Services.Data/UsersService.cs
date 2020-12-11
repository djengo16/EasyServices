namespace EasyServices.Services.Data
{
    using System.Collections.Generic;
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

        public IEnumerable<T> GetBySearch<T>(string searchKeywords)
        {
            if (string.IsNullOrWhiteSpace(searchKeywords))
            {
                return null;
            }

            var returnModel = this.usersRepository
                .AllAsNoTracking()
                .Where(x => x.Email.Contains(searchKeywords) || x.Name.Contains(searchKeywords));

            return returnModel.To<T>().ToList();

        }

        public int GetCount()
        {
            return this.usersRepository.All().Count();
        }

        public string GetProfilePictureUrl(string userId)
        {
            var profilePicture = userId == null ? null : this.usersRepository.All().FirstOrDefault(x => x.Id == userId).ProfilePicture;

            return profilePicture;
        }

        public T GetUserById<T>(string userId)
        {
            var userModel = this.usersRepository
                .All().Where(x => x.Id == userId).To<T>().FirstOrDefault();

            return userModel;
        }
    }
}
