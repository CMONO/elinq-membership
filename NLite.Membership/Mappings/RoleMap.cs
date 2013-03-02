using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLite.Membership.Entities;
using NLite.Data.Mapping.Fluent;

namespace NLite.Membership.Mappings
{
    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            TableName("_C_Role");

            Id(x => x.Id).DbGenerated();
            Column(x => x.Name);
            Column(x => x.ApplicationName);

            ManyToOne<Site>(t => t.Site).ThisKey(t => t.ApplicationName).OtherKey(o => o.Id);
            OneToMany<UserRole>(t => t.UsersInRole).ThisKey(t=>t.Id).OtherKey(o => o.RoleId);
        }
    }
}
