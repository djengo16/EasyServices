namespace EasyServices.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tag
    {
        public Tag()
        {
            this.Tags = new HashSet<AnnouncementTag>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<AnnouncementTag> Tags { get; set; }
    }
}
