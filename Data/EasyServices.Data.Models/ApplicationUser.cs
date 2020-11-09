// ReSharper disable VirtualMemberCallInConstructor
namespace EasyServices.Data.Models
{
    using System;
    using System.Collections.Generic;

    using EasyServices.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Announcements = new HashSet<Announcement>();
            this.Reviews = new HashSet<Review>();
        }

        // Personal Info
        public string Name { get; set; }

        public string WebSite { get; set; }

        public string ProfilePicture { get; set; }

        public string Description { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Announcement> Announcements { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

    }
}
