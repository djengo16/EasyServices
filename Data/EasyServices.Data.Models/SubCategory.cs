namespace EasyServices.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using EasyServices.Data.Common.Models;

    public class SubCategory : BaseDeletableModel<int>
    {
        public SubCategory()
        {
            this.Announcements = new HashSet<Announcement>();
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string ImgUrl { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public virtual ICollection<Announcement> Announcements { get; set; }
    }
}
