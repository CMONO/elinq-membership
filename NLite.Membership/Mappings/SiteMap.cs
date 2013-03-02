using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLite.Data.Mapping.Fluent;
using NLite.Membership.Entities;

namespace NLite.Membership.Mappings
{
    public class SiteMap : ClassMap<Site>
    {
        public SiteMap()
        {
            TableName("_C_Site");
            Id(x => x.Id);
            Column(x => x.Name).Required().Length(50);
            Column(x => x.Url).Required().Length(200);
            Column(x => x.Icon).Length(200);
            Column(x => x.AdminId);
            Column(x => x.Status).Required();

            OneToMany<User>(t => t.Users).ThisKey(t => t.Id).OtherKey(o => o.ApplicationName);
            OneToMany<Role>(t => t.Roles).ThisKey(t => t.Id).OtherKey(o => o.ApplicationName);
        }
    }
}
