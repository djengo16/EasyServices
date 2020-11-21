namespace EasyServices.Web.ViewModels.Administration.SubCategories
{
    using System.ComponentModel.DataAnnotations;

    public class EditSubCategoryModel
    {
        public int Id { get; set; }


        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}
