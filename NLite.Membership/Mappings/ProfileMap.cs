using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLite.Data.Mapping.Fluent;
using NLite.Membership.Entities;


namespace NLite.Membership.Mappings
{
    public class ProfileMap : ClassMap<Profile>
    {
        public ProfileMap()
        {
            TableName("_C_Profile");

            Id(x => x.Id).DbGenerated();
            Column(x => x.UserId);
            Column(x => x.ApplicationName);
            Column(x => x.BirthDate);
            Column(x => x.City).Length(150);
            Column(x => x.Country).Length(100);
            Column(x => x.FirstName).Length(10);
            Column(x => x.Gender);
            Column(x => x.IsAnonymous);
            Column(x => x.Language);
            Column(x => x.LastActivityDate);
            Column(x => x.LastName).Length(10);
            Column(x => x.LastUpdatedDate);
            Column(x => x.Occupation).Length(100);
            Column(x => x.Province).Length(20);
            Column(x => x.Street).Length(100);
            Column(x => x.Subscription).Length(200);
            Column(x => x.Website).Length(200);
            Column(x => x.Zip).Length(20);

            ManyToOne<User>(t => t.User).OtherKey(o => o.Id);
        }
    }
}
