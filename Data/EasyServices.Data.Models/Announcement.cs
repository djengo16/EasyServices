namespace EasyServices.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    using EasyServices.Data.Common.Models;

    public class Announcement : BaseDeletableModel<string>
    {
        public Announcement()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Tags = new HashSet<AnnouncementTag>();
            this.Reviews = new HashSet<Review>();
            this.Images = new HashSet<Image>();
        }

        // Properties
        public decimal? Price { get; set; } // Service price

        [Required]
        [MaxLength(30)]
        public string Title { get; set; }

        public string Description { get; set; }

        // FK_User
        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        // FK_City
        public int? CityId { get; set; }

        public City City { get; set; }

        // FK_Subcategory
        public int SubCategoryId { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        // FK_Category
        public virtual Category Category { get; set; }

        public int CategoryId { get; set; }

        public double TotalRating => this.Reviews.Sum(x => x.Rating) / this.Reviews.Count;

        public virtual ICollection<AnnouncementTag> Tags { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}
