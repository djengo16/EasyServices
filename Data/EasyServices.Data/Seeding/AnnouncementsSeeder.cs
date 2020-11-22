namespace EasyServices.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using EasyServices.Data.Models;
    using Microsoft.EntityFrameworkCore.Internal;

    public class AnnouncementsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            ;
            if (dbContext.Announcements.Count() >= 50)
            {
                return;
            }

            Random random = new Random();

            var announcements = new List<Announcement>();
            var tags = dbContext.Tags.ToList();
            var users = dbContext.Users.ToList();
            var categories = dbContext.Categories.ToList();
            var subCategories = dbContext.SubCategories.ToList();
            var cities = dbContext.Cities.ToList();

            int firstCategoryId = categories.FirstOrDefault().Id;
            int firstSubCategoryId = categories.FirstOrDefault().Id;

            for (int i = 0; i < 50; i++)
            {
                var randCateogry = random.Next();

                var announcement = new Announcement
                {
                    Title = "Заглавие..",
                    Description = "Примерно описание 1231353245",
                    CategoryId = random.Next(firstCategoryId, firstCategoryId + categories.Count - 1),
                    SubCategoryId = random.Next(firstSubCategoryId, firstSubCategoryId + subCategories.Count - 1),
                    UserId = users[random.Next(users.Count)].Id,
                    Price = 16.50M,
                    CityId = random.Next(cities.Count),
                    Images = new List<Image>
                    {
                        new Image { Url = "https://blog.hubspot.com/hubfs/use-fade-in-animation-on-your-website.jpg" },
                        new Image { Url = "https://i.pinimg.com/originals/99/fc/0e/99fc0ecfffa7761ec7ffbdeea151d105.jpg" },
                        new Image { Url = "https://www.jotform.com/blog/wp-content/uploads/2019/10/How-to-write-a-business-plan-featured-605x366.png" },
                    },
                    Tags = new List<AnnouncementTag>
                    {
                        new AnnouncementTag { TagId = tags.ElementAt(1).Id },
                        new AnnouncementTag { TagId = tags.ElementAt(2).Id },
                        new AnnouncementTag { TagId = tags.ElementAt(3).Id },
                        new AnnouncementTag { TagId = tags.ElementAt(4).Id },
                        new AnnouncementTag { TagId = tags.ElementAt(5).Id },
                    },
                };
                announcements.Add(announcement);
            }

            dbContext.AddRange(announcements);
            await dbContext.SaveChangesAsync();
        }

    }
}
