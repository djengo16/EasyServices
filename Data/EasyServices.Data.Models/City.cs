namespace EasyServices.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using EasyServices.Data.Common.Models;

    public class City : BaseDeletableModel<int>
    {
        public City()
        {
            this.Announcements = new HashSet<Announcement>();
        }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public ICollection<Announcement> Announcements { get; set; }
    }
}
