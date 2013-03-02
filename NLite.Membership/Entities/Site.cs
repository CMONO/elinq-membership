using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLite.Membership.Enums;

namespace NLite.Membership.Entities
{
    public class Site
    {
        public Site()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int? AdminId { get; set; }
        public SiteStatus Status { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Role> Roles { get; set; }

    }
}
