namespace EasyServices.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EasyServices.Data;
    using EasyServices.Data.Models;
    using EasyServices.Data.Repositories;
    using EasyServices.Web.ViewModels.Users;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class UsersServiceTests
    {
        [Fact]

        public async Task GetBySearchShouldWorkCorrect()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var db = new ApplicationDbContext(options);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);

            var service = new UsersService(usersRepository);

            for (int i = 0; i < 8; i++)
            {
                if (i % 2 == 0)
                {
                    db.Users.Add(new ApplicationUser
                    {
                        Name = "Name" + i.ToString(),
                        Email = $"mail{i}@mail.com",
                        PasswordHash = "1234",
                        ProfilePicture = "testurl",
                    });
                }

                if (i % 3 == 0)
                {
                    db.Users.Add(new ApplicationUser
                    {
                        Name = "Another" + i.ToString(),
                        Email = $"test{i}@test.com",
                        PasswordHash = "1234",
                        ProfilePicture = "testurl",

                    });
                }
            }

            await db.SaveChangesAsync();

            var result = service.GetBySearch<SearchedUserViewModel>("name");
            var result2 = service.GetBySearch<SearchedUserViewModel>("test");

            Assert.Equal(4, result.Count());
            Assert.Equal(3, result2.Count());
        }

        [Fact]
        public async Task GetCountShouldGetCorrectResult()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var db = new ApplicationDbContext(options);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);

            var service = new UsersService(usersRepository);

            for (int i = 0; i < 2; i++)
            {
                db.Users.Add(new ApplicationUser
                {
                    Name = "Name" + i.ToString(),
                    Email = $"mail{i}@mail.com",
                    PasswordHash = "1234",
                    ProfilePicture = "testurl",
                });
            }

            await db.SaveChangesAsync();
            int actualCount = service.GetCount();

            Assert.Equal(2, actualCount);
        }

        [Fact]
        public async Task GetProfilePictureShouldWorkCorrect()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var db = new ApplicationDbContext(options);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);

            var service = new UsersService(usersRepository);

            db.Users.Add(new ApplicationUser
            {
                Name = "Name",
                Email = $"mail@mail.com",
                PasswordHash = "1234",
                ProfilePicture = "testurl",
            });
            await db.SaveChangesAsync();
            var userId = db.Users.FirstOrDefault().Id;

            var profilePictureUrl = service.GetProfilePictureUrl(userId);

            Assert.Equal("testurl", profilePictureUrl);
        }

        [Fact]
        public async Task GetUserByIdShouldReturnUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var db = new ApplicationDbContext(options);
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(db);

            var service = new UsersService(usersRepository);

            db.Users.Add(new ApplicationUser
            {
                Name = "Name",
                Email = $"mail@mail.com",
                PasswordHash = "1234",
                ProfilePicture = "testurl",
            });
            await db.SaveChangesAsync();
            var userId = db.Users.FirstOrDefault().Id;

            var user = service.GetUserById<UserProfileViewModel>(userId);

            Assert.Equal("Name", user.Name);
        }
    }
}
