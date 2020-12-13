namespace EasyServices.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using EasyServices.Data.Models;
    using EasyServices.Data.Seeding.DTO;
    using Microsoft.EntityFrameworkCore.Internal;
    using Newtonsoft.Json;

    public class CategoriesSeederAndSubCategories : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any() || dbContext.SubCategories.Any())
            {
                return;
            }

            ImportCategoriesDTO[] importCategories = JsonConvert
                .DeserializeObject<ImportCategoriesDTO[]>(File.ReadAllText(@"../../Data/EasyServices.Data/Seeding/Data/Categories.json"));

            List<Category> categories = new List<Category>();
            List<SubCategory> subCategories = new List<SubCategory>();

            foreach (var importedCategory in importCategories)
            {
                var category = new Category
                {
                    Name = importedCategory.Name,
                    ImgUrl = importedCategory.ImgUrl,
                };

                foreach (var importedSubCategory in importedCategory.SubCategories)
                {
                    var subCategory = new SubCategory
                    {
                        Name = importedSubCategory.Name,
                        Category = category,
                    };
                    subCategories.Add(subCategory);
                }

                categories.Add(category);
            }

            await dbContext.Categories.AddRangeAsync(categories);
            await dbContext.SubCategories.AddRangeAsync(subCategories);

            await dbContext.SaveChangesAsync();
        }
    }
}
