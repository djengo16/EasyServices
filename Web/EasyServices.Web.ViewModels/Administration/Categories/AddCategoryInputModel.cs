﻿namespace EasyServices.Web.ViewModels.Administration.Categories
{
    using System.ComponentModel.DataAnnotations;

    public class AddCategoryInputModel
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(300)]
        [MinLength(5)]
        public string ImgUrl { get; set; }
    }
}
