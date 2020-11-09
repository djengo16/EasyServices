namespace EasyServices.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using EasyServices.Data.Common.Models;

    public class Tag : BaseDeletableModel<int>
    {
        public Tag()
        {
            this.Tags = new HashSet<AnnouncementTag>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<AnnouncementTag> Tags { get; set; }
    }
}
