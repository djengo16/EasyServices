namespace EasyServices.Web.ViewModels.Announcements
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using EasyServices.Common;
    using Microsoft.AspNetCore.Http;

    public class AnnouncementInputModel
    {
        public AnnouncementInputModel()
        {
            this.CitiesItems = new List<KeyValuePair<string, string>>();
            this.Tags = new List<string>();
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = ErrorMessages.TitleIsRequired)]
        [MaxLength(40, ErrorMessage = ErrorMessages.TitleIsNotInRange)]
        [MinLength(5, ErrorMessage = ErrorMessages.TitleIsNotInRange)]
        [Display(Name = "Заглавие")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Display(Name = "Цена")]
        public decimal? Price { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Град")]
        public int? CityId { get; set; }

        [Display(Name = "Категория")]
        [Required(AllowEmptyStrings = false, ErrorMessage = ErrorMessages.CategoryRequirement)]
        public int SubCategoryId { get; set; }

        [Display(Name = "(Снимки на ваши проекти)")]
        public IEnumerable<IFormFile> Images { get; set; }

        [Display(Name = "Тагове")]
        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<AnnouncementCategoryInputModel> Categories { get; set; }

        public IEnumerable<KeyValuePair<string, string>> CitiesItems { get; set; }

        public IEnumerable<AnnouncementTagsInputModel> TagsItems { get; set; }

    }
}
