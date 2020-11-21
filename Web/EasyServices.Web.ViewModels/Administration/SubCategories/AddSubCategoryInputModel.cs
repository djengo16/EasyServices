namespace EasyServices.Web.ViewModels.Administration.SubCategories
{
    using System.ComponentModel.DataAnnotations;

    public class AddSubCategoryInputModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int CategoryId { get; set; }
    }
}
