using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLite.Data.Mapping.Fluent;
using NLite.Membership.Entities;

namespace NLite.Membership.Mappings
{
    public class UserRoleMap:ClassMap<UserRole>
    {
        public UserRoleMap()
        {
            TableName("_C_User_Role");

            Id(e => e.UserId);
            Id(e => e.RoleId);

            ManyToOne<User>(t => t.User).OtherKey(o => o.Id);
            ManyToOne<Role>(t => t.Role).OtherKey(o => o.Id);
        }
    }
}
