using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLite.Membership.Entities;
using NLite.Data.Mapping.Fluent;


namespace NLite.Membership.Mappings
{
    public class UserMap: ClassMap<User>
    {
        public UserMap()
        {
            TableName("_C_User");

            Id(x => x.Id).DbGenerated();
            Column(x => x.UserName).Required().Length(30);
            Column(x => x.ApplicationName);
            Column(x => x.Email).Required().Length(100);
            Column(x => x.Password).Required().Length(20);

            Column(x => x.Question);
            Column(x => x.Answer);

            Column(x => x.IsApproved);
            Column(x => x.LastActivityDate);
            Column(x => x.LastLoginDate);
            Column(x => x.LastPasswordChangedDate);
            Column(x => x.CreationDate);
            Column(x => x.IsOnLine);
            Column(x => x.IsLockedOut);
            Column(x => x.LastLockedOutDate);
            Column(x => x.FailedPasswordAttemptCount);
            Column(x => x.FailedAnswerAttemptCount);
            Column(x => x.FailedPasswordAttemptWindowStart);
            Column(x => x.FailedAnswerAttemptWindowStart);
            Column(x => x.Comment).Length(200);

            ManyToOne<Site>(t => t.Site).ThisKey(t => t.ApplicationName).OtherKey(o => o.Id);
            OneToMany<UserRole>(t => t.UserRoles).ThisKey(t=>t.Id).OtherKey(o => o.UserId);
        }

    }
}
