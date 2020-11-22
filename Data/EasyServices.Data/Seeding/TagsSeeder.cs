namespace EasyServices.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EasyServices.Data.Models;
    using Microsoft.EntityFrameworkCore.Internal;

    public class TagsSeeder : ISeeder
    {

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Tags.Any())
            {
                return;
            }

            dbContext.Tags.AddRange(new List<Tag>
            {
                new Tag { Name = "gotvach" },
                new Tag { Name = "pekar" },
                new Tag { Name = "it" },
                new Tag { Name = "c#" },
                new Tag { Name = "maystor" },
                new Tag { Name = "serviz" },
                new Tag { Name = "qa" },
                new Tag { Name = "uchitel" },
                new Tag { Name = "urok" },
                new Tag { Name = "chujd ezik" },
                new Tag { Name = "ruchna izrabotka" },
                new Tag { Name = "boqdjiq" },
                new Tag { Name = "auto" },
                new Tag { Name = "bilkar" },
                new Tag { Name = "izkustvo" },
                new Tag { Name = "zanaqt" },
                new Tag { Name = "programist" },
            });
            await dbContext.SaveChangesAsync();

        }
    }
}
