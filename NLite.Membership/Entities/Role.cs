    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLite.Membership.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ApplicationName { get; set; }

        public Site Site { get; set; }
        public IList<UserRole> UsersInRole { get; set; }

        public Role()
        {
            UsersInRole = new List<UserRole>();
        }
       
    }
}
