namespace EasyServices.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using EasyServices.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.SubCategories = new HashSet<SubCategory>();
            this.Announcements = new HashSet<Announcement>();
        }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string ImgUrl { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }

        public virtual ICollection<Announcement> Announcements { get; set; }
    }
}
