
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web.Profile;
using System.Web.Configuration;
using NLite.Data;
using NLite.Membership.Entities;
using NLite.Membership.Mappings;

namespace NLite.Membership
{
    public sealed class ELProfileProvider : System.Web.Profile.ProfileProvider 
    {
       
        public override string ApplicationName { get; set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "ELProfileProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "ELinq Profile provider");
            }

            base.Initialize(name, config);

            var connectionStringName = config["connectionStringName"];
            UnitOfWork.Configure(connectionStringName);

            ApplicationName = config["applicationName"];
            if (ApplicationName.IsNullOrEmpty())
                ApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            else if (ApplicationName != "/")
            {
                var site = UnitOfWork.Current.CreateRepository<Site>().FirstOrDefault(p => p.Id == ApplicationName && p.Status ==  Enums.SiteStatus.Enable);
                if (site == null)
                    throw new ProviderException("ApplicationName not exists.");
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context,SettingsPropertyCollection ppc)
        {
            string username = (string)context["UserName"];
            bool isAnonymous = !(bool)context["IsAuthenticated"];

            var unitOfWork = UnitOfWork.Current;
            var profiles = unitOfWork.CreateRepository<Profile>();

            SettingsPropertyValueCollection svc = null;
            unitOfWork.UsingTransaction(() =>
                {

                    var profile = GetProfile(username, isAnonymous, profiles);

                    svc = GetPropertyValues(ppc, profile);

                    DateTime activityDate = DateTime.Now;
                    profile.LastActivityDate = activityDate;
                    profile.IsAnonymous = isAnonymous;
                    profiles.Update(profile);
                });

            return svc;
        }

        private Profile GetProfile(string username, bool isAnonymous, IRepository<Profile> profiles)
        {
            var profile = profiles
                .FirstOrDefault(p =>
                          p.User.UserName == username
                          && p.ApplicationName == ApplicationName
                          && p.IsAnonymous == isAnonymous);

            if (profile == null)
            {
                var membershipUser = UnitOfWork.Current.CreateRepository<User>()
                    .FirstOrDefault(p => p.UserName == username
                    && p.ApplicationName == ApplicationName);

                if (membershipUser == null)
                    throw new ProviderException("Profile cannot be created. There is no membership user");

                profile = new Profile();
                profile.IsAnonymous = isAnonymous;
                profile.LastUpdatedDate = System.DateTime.Now;
                profile.LastActivityDate = System.DateTime.Now;
                profile.ApplicationName = this.ApplicationName;
                profile.UserId = membershipUser.Id;

                profiles.Insert(profile);
            }
            return profile;
        }

        private static SettingsPropertyValueCollection GetPropertyValues(SettingsPropertyCollection ppc, Profile profile)
        {
            var svc = new SettingsPropertyValueCollection();
            foreach (SettingsProperty prop in ppc)
            {
                SettingsPropertyValue pv = new SettingsPropertyValue(prop);
                switch (prop.Name)
                {
                    case "IsAnonymous":
                        pv.PropertyValue = profile.IsAnonymous;
                        break;
                    case "LastActivityDate":
                        pv.PropertyValue = profile.LastActivityDate;
                        break;
                    case "LastUpdatedDate":
                        pv.PropertyValue = profile.LastUpdatedDate;
                        break;
                    case "Subscription":
                        pv.PropertyValue = profile.Subscription;
                        break;
                    case "Language":
                        pv.PropertyValue = profile.Language;
                        break;
                    case "FirstName":
                        pv.PropertyValue = profile.FirstName;
                        break;
                    case "LastName":
                        pv.PropertyValue = profile.LastName;
                        break;
                    case "Gender":
                        pv.PropertyValue = profile.Gender;
                        break;
                    case "BirthDate":
                        pv.PropertyValue = profile.BirthDate;
                        break;
                    case "Occupation":
                        pv.PropertyValue = profile.Occupation;
                        break;
                    case "Website":
                        pv.PropertyValue = profile.Website;
                        break;
                    case "Street":
                        pv.PropertyValue = profile.Street;
                        break;
                    case "City":
                        pv.PropertyValue = profile.City;
                        break;
                    case "State":
                        pv.PropertyValue = profile.Province;
                        break;
                    case "Zip":
                        pv.PropertyValue = profile.Zip;
                        break;
                    case "Country":
                        pv.PropertyValue = profile.Country;
                        break;

                    default:
                        throw new ProviderException("Unsupported property.");
                }

                svc.Add(pv);
            }
            return svc;
        }

        public override void SetPropertyValues(SettingsContext context,SettingsPropertyValueCollection ppvc)
        {
            string username = (string)context["UserName"];
            bool isAnonymous = !(bool)context["IsAuthenticated"];

            var unitOfWork = UnitOfWork.Current;
            var profiles = unitOfWork.CreateRepository<Profile>();

            unitOfWork.UsingTransaction(() =>
                {
                    var profile = GetProfile(username, isAnonymous,profiles);

                    SetPropertyValues(ppvc, profile);

                    DateTime activityDate = DateTime.Now;
                    profile.LastActivityDate = activityDate;
                    profile.LastUpdatedDate = activityDate;
                    profile.IsAnonymous = isAnonymous;

                    profiles.Update(profile);
                });
        }

        private static void SetPropertyValues(SettingsPropertyValueCollection ppvc, Profile profile)
        {
            foreach (SettingsPropertyValue pv in ppvc)
            {
                switch (pv.Property.Name)
                {
                    case "IsAnonymous":
                        profile.IsAnonymous = (bool)pv.PropertyValue;
                        break;
                    case "LastActivityDate":
                        profile.LastActivityDate = (DateTime)pv.PropertyValue;
                        break;
                    case "LastUpdatedDate":
                        profile.LastUpdatedDate = (DateTime)pv.PropertyValue;
                        break;
                    case "Subscription":
                        profile.Subscription = pv.PropertyValue.ToString();
                        break;
                    case "Language":
                        profile.Language = pv.PropertyValue.ToString();
                        break;
                    case "FirstName":
                        profile.FirstName = pv.PropertyValue.ToString();
                        break;
                    case "LastName":
                        profile.LastName = pv.PropertyValue.ToString();
                        break;
                    case "Gender":
                        profile.Gender = pv.PropertyValue.ToString();
                        break;
                    case "BirthDate":
                        profile.BirthDate = (DateTime)pv.PropertyValue;
                        break;
                    case "Occupation":
                        profile.Occupation = pv.PropertyValue.ToString();
                        break;
                    case "Website":
                        profile.Website = pv.PropertyValue.ToString();
                        break;
                    case "Street":
                        profile.Street = pv.PropertyValue.ToString();
                        break;
                    case "City":
                        profile.City = pv.PropertyValue.ToString();
                        break;
                    case "State":
                        profile.Province = pv.PropertyValue.ToString();
                        break;
                    case "Zip":
                        profile.Zip = pv.PropertyValue.ToString();
                        break;
                    case "Country":
                        profile.Country = pv.PropertyValue.ToString();
                        break;
                    default:
                        throw new ProviderException("Unsupported property.");
                }
            }
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            return DeleteProfiles(profiles.Cast<ProfileInfo>().Select(p => p.UserName).ToArray());
        }

        public override int DeleteProfiles(string[] usernames)
        {
            return UnitOfWork.Current.CreateRepository<Profile>()
             .Delete(p => usernames.Contains( p.User.UserName) && p.ApplicationName == ApplicationName);
        }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption,DateTime userInactiveSinceDate)
        {
            var isAnonymous = authenticationOption == ProfileAuthenticationOption.Anonymous;
            return UnitOfWork.Current.CreateRepository<Profile>().Delete(p =>
                p.ApplicationName == ApplicationName
                && p.LastActivityDate == userInactiveSinceDate
                && p.IsAnonymous == isAnonymous);
        }

        public override ProfileInfoCollection FindProfilesByUserName(
            ProfileAuthenticationOption authenticationOption,
            string usernameToMatch,
            int pageIndex,
            int pageSize,
            out int totalRecords)
        {
            if (pageIndex < 0)
                throw new ArgumentException("Page index must 0 or greater.");
            if (pageSize < 1)
                throw new ArgumentException("Page size must be greater than 0.");
            return GetProfileInfo(authenticationOption, usernameToMatch,null, pageIndex, pageSize, out totalRecords);
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(
            ProfileAuthenticationOption authenticationOption,
            string usernameToMatch,
            DateTime userInactiveSinceDate,
            int pageIndex,
            int pageSize,
            out int totalRecords)
        {
            if (pageIndex < 0)
                throw new ArgumentException("Page index must 0 or greater.");
            if (pageSize < 1)
                throw new ArgumentException("Page size must be greater than 0.");

            return GetProfileInfo(authenticationOption, usernameToMatch, userInactiveSinceDate,pageIndex, pageSize, out totalRecords);
        }

        public override ProfileInfoCollection GetAllProfiles(
            ProfileAuthenticationOption authenticationOption,
            int pageIndex,
            int pageSize,
            out int totalRecords)
        {
            if (pageIndex < 0)
                throw new ArgumentException("Page index must 0 or greater.");
            if (pageSize < 1)
                throw new ArgumentException("Page size must be greater than 0.");

            return GetProfileInfo(authenticationOption, null, null,pageIndex, pageSize, out totalRecords);
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(
            ProfileAuthenticationOption authenticationOption,
            DateTime userInactiveSinceDate,
            int pageIndex,
            int pageSize,
            out int totalRecords)
        {
            if (pageIndex < 0)
                throw new ArgumentException("Page index must 0 or greater.");
            if (pageSize < 1)
                throw new ArgumentException("Page size must be greater than 0.");

            return GetProfileInfo(authenticationOption, null, userInactiveSinceDate,pageIndex, pageSize, out totalRecords);
        }

        public override int GetNumberOfInactiveProfiles(
            ProfileAuthenticationOption authenticationOption
            ,DateTime userInactiveSinceDate)
        {
             bool isAnonymous = authenticationOption == ProfileAuthenticationOption.Anonymous;
             return UnitOfWork.Current.CreateRepository<Profile>()
                 .Where(p => p.LastActivityDate == userInactiveSinceDate)
                 .Where(p => p.IsAnonymous == isAnonymous)
                 .Count();
        }


        private ProfileInfoCollection GetProfileInfo(
            ProfileAuthenticationOption authenticationOption
            , string usernameToMatch,
            DateTime? userInactiveSinceDate,
            int pageIndex,
            int pageSize,
            out int totalRecords)
        {

            bool isAnonymous = authenticationOption == ProfileAuthenticationOption.Anonymous;

            var q = UnitOfWork.Current.CreateRepository<Profile>()
                .Where(p => p.ApplicationName == ApplicationName)
                .Where(p => p.IsAnonymous == isAnonymous);

            if (usernameToMatch.HasValue())
                q = q.Where(p => p.User.UserName.Contains(usernameToMatch));
            if (userInactiveSinceDate != null)
                q = q.Where(p => p.LastActivityDate == userInactiveSinceDate.Value);
            totalRecords = q.Count();

            var profiles = q
                .Take(pageSize)
                .Skip(pageSize * pageIndex)
                .Select(p => new ProfileInfo(p.User.UserName, p.IsAnonymous, p.LastActivityDate, p.LastUpdatedDate, 0))
                .ToList();

            var items = new ProfileInfoCollection();
            foreach (var item in profiles)
                items.Add(item);
            return items;
        }
    }
}
