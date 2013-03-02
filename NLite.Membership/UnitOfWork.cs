using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLite.Data;
using NLite.Membership.Entities;
using NLite.Membership.Mappings;

namespace NLite.Membership
{
    static class UnitOfWork
    {
        internal static string DbConfigurationName;

        internal static void Configure(string connectionStringName)
        {
            if (connectionStringName.IsNullOrEmpty())
                throw new ArgumentNullException("connectionStringName");

            DbConfiguration DbConfiguration = null;
            if (!DbConfiguration.Items.ContainsKey(connectionStringName))
                DbConfiguration = DbConfiguration.Configure(connectionStringName);
            else
                DbConfiguration = DbConfiguration.Items[connectionStringName];

            DbConfigurationName = connectionStringName;
            if (!DbConfiguration.HasClass<Site>())
                DbConfiguration.AddClass(new SiteMap());
            if (!DbConfiguration.HasClass<User>())
                DbConfiguration.AddClass(new UserMap());
            if (!DbConfiguration.HasClass<Role>())
                DbConfiguration.AddClass(new RoleMap());
            if (!DbConfiguration.HasClass<UserRole>())
                DbConfiguration.AddClass(new UserRoleMap());
            if (!DbConfiguration.HasClass<Profile>())
                DbConfiguration.AddClass(new ProfileMap());
        }

        public static NLite.Data.IUnitOfWork Current
        {
            get { return NLite.Data.UnitOfWork.Get(DbConfigurationName); }
        }
    }
}
