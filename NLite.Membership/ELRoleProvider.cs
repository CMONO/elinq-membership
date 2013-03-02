using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Collections.Specialized;
using NLite.Membership.Entities;
using System.Configuration.Provider;

namespace NLite.Membership
{
    sealed class ELRoleProvider : RoleProvider
    {
        public override string ApplicationName { get; set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "ELRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "ELinq Role provider");
            }

            base.Initialize(name, config);

            var connectionStringName = config["connectionStringName"];

            UnitOfWork.Configure(connectionStringName);

            ApplicationName = config["applicationName"];
            if (ApplicationName.IsNullOrEmpty())
                ApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            else if (ApplicationName != "/")
            {
                var site = UnitOfWork.Current.CreateRepository<Site>().FirstOrDefault(p => p.Id == ApplicationName && p.Status == Enums.SiteStatus.Enable);
                if (site == null)
                    throw new ProviderException("ApplicationName not exists.");
            }
        }

        public override void CreateRole(string roleName)
        {
            Guard.NotNullOrEmpty(roleName, "roleName");
            if (roleName.Contains(","))
                throw new ArgumentException("Role names cannot contain commas.");

            var roles = UnitOfWork.Current.CreateRepository<Role>();
            var role = roles.FirstOrDefault(p => p.Name == roleName && ApplicationName == ApplicationName);
            if(role != null)
                throw new ProviderException("Role name already exists.");

            role = new Role { ApplicationName = ApplicationName, Name = roleName };
            roles.Insert(role);
        }

        public override string[] GetAllRoles()
        {
            return UnitOfWork.Current.CreateRepository<Role>().Select(p => p.Name).ToArray();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            Guard.NotNullOrEmpty(roleName, "roleName");
            if (roleName.Contains(","))
                throw new ArgumentException("Role names cannot contain commas.");

            var roles = UnitOfWork.Current.CreateRepository<Role>() as NLite.Data.IDbSet<Role>;
            if (throwOnPopulatedRole)
                roles.Include(p => p.UsersInRole);

            var role = roles.FirstOrDefault(p => p.Name == roleName && ApplicationName == ApplicationName);
            if (role == null)
                throw new ProviderException("Role does not exist.");

            if (throwOnPopulatedRole && role.UsersInRole.Count > 0)
                throw new ProviderException("Cannot delete a populated role.");

            return roles.Delete(p => p.Id == role.Id) > 0;
        }

        public override bool RoleExists(string roleName)
        {
            Guard.NotNullOrEmpty(roleName, "roleName");
            return UnitOfWork.Current.CreateRepository<Role>()
                .FirstOrDefault(p => p.Name == roleName)
                != null;
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            var length = usernames.Length;
            var userIds = UnitOfWork.Current.CreateRepository<User>()
                .Where(p => usernames.Contains(p.UserName))
                .Select(p=>p.Id)
                .ToArray();
            var roles = UnitOfWork.Current.CreateRepository<Role>()
                .Where(p => roleNames.Contains(p.Name))
                .Select(p=>p.Id)
                .ToArray();

            if (userIds.Length != usernames.Length)
                throw new ProviderException("some User name not found in db.");
            if (roles.Length != roleNames.Length)
                throw new ProviderException("some Role name not found in db.");

            var userRoles = new List<UserRole>();
            foreach (var userId in userIds)
                foreach (var roleId in roles)
                    userRoles.Add(new UserRole { UserId = userId, RoleId = roleId });

            UnitOfWork.Current.CreateRepository<UserRole>().Batch(userRoles, (s, e) => s.Insert(e));
        }


        public override string[] FindUsersInRole(string roleName, string userNameToMatch)
        {
            Guard.NotNullOrEmpty(roleName, "roleName");
            Guard.NotNullOrEmpty(userNameToMatch, "userNameToMatch");
            var q = from user in UnitOfWork.Current.CreateRepository<User>()
                    from userRole in user.UserRoles
                    where user.UserName.Contains(userNameToMatch)
                    where userRole.Role.Name == roleName
                    select user.UserName;
            return q.ToArray();
        }

        public override string[] GetRolesForUser(string userName)
        {
            Guard.NotNullOrEmpty(userName, "userName");
            var q = from user in UnitOfWork.Current.CreateRepository<User>()
                    from userRole in user.UserRoles
                    where user.UserName == userName
                    select userRole.Role.Name;
            return q.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            Guard.NotNullOrEmpty(roleName, "roleName");
            var q = from user in UnitOfWork.Current.CreateRepository<User>()
                    from userRole in user.UserRoles
                    where userRole.Role.Name == roleName
                    select user.UserName;
            return q.ToArray();
        }

        public override bool IsUserInRole(string userName, string roleName)
        {
            Guard.NotNullOrEmpty(userName, "userName");
            Guard.NotNullOrEmpty(roleName, "roleName");
            var q = from user in UnitOfWork.Current.CreateRepository<User>()
                    from userRole in user.UserRoles
                    where userRole.Role.Name == roleName
                    where user.UserName == userName
                    select userRole;

            return q.Count() == 1;
        }

        public override void RemoveUsersFromRoles(string[] userNames, string[] roleNames)
        {
            Guard.NotNull(userNames, "userNames");
            Guard.NotNull(roleNames, "roleNames");

            var length = userNames.Length;
            var userIds = UnitOfWork.Current.CreateRepository<User>()
                .Where(p => userNames.Contains(p.UserName))
                .Select(p => p.Id)
                .ToArray();
            var roles = UnitOfWork.Current.CreateRepository<Role>()
                .Where(p => roleNames.Contains(p.Name))
                .Select(p => p.Id)
                .ToArray();

            if (userIds.Length != userNames.Length)
                throw new ProviderException("some User name not found in db.");
            if (roles.Length != roleNames.Length)
                throw new ProviderException("some Role name not found in db.");

            var userRoles = new List<UserRole>();
            foreach (var userId in userIds)
                foreach (var roleId in roles)
                    userRoles.Add(new UserRole { UserId = userId, RoleId = roleId });

            UnitOfWork.Current.CreateRepository<UserRole>().Batch(userRoles, (s, e) => s.Delete(e));
        }
    }
}
