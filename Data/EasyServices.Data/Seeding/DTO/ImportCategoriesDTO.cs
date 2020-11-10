namespace EasyServices.Data.Seeding.DTO
{
    public class ImportCategoriesDTO
    {
        public string Name { get; set; }

        public string ImgUrl { get; set; }

        public ImportSubCategoriesDTO[] SubCategories { get; set; }
    }
}
