using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;
using NLite.Membership.Entities;
using NLite.Data;

namespace NLite.Membership
{
    public class ELMembershipProvider : MembershipProvider
    {
        private MachineKeySection _machineKey;

        public override string ApplicationName { get; set; }
        private bool enablePasswordReset;
        public override bool EnablePasswordReset { get { return enablePasswordReset; } }

        private bool enablePasswordRetrieval;
        public override bool EnablePasswordRetrieval { get { return enablePasswordRetrieval; } }

        private bool requiresQuestionAndAnswer;
        public override bool RequiresQuestionAndAnswer { get { return requiresQuestionAndAnswer; } }

        private bool requiresUniqueEmail;
        public override bool RequiresUniqueEmail { get { return requiresUniqueEmail; } }

        private int maxInvalidPasswordAttempts;
        public override int MaxInvalidPasswordAttempts { get { return maxInvalidPasswordAttempts; } }

        private int passwordAttemptWindow;
        public override int PasswordAttemptWindow { get { return passwordAttemptWindow; } }

        private MembershipPasswordFormat passwordFormat;
        public override MembershipPasswordFormat PasswordFormat { get { return passwordFormat; } }

        int minRequiredNonAlphanumericCharacters;
        public override int MinRequiredNonAlphanumericCharacters { get { return minRequiredNonAlphanumericCharacters; } }

        int minRequiredPasswordLength;
        public override int MinRequiredPasswordLength { get { return minRequiredPasswordLength; } }

        string passwordStrengthRegularExpression;
        public override string PasswordStrengthRegularExpression { get { return passwordStrengthRegularExpression; } }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "ELMemebershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "ELinq Membership provider");
            }

            base.Initialize(name, config);

           
          
            maxInvalidPasswordAttempts = config.Get<int>( "maxInvalidPasswordAttempts",5);
            passwordAttemptWindow = config.Get<int>("passwordAttemptWindow",10);
            minRequiredNonAlphanumericCharacters = config.Get<int>("minRequiredNonAlphanumericCharacters",1);
            minRequiredPasswordLength = config.Get<int>("minRequiredPasswordLength",7);
            passwordStrengthRegularExpression =config.Get<string>("passwordStrengthRegularExpression", "");
            enablePasswordReset =config.Get<bool>("enablePasswordReset",true);
            enablePasswordRetrieval = config.Get<bool>("enablePasswordRetrieval",true);
            requiresQuestionAndAnswer = config.Get<bool>("requiresQuestionAndAnswer",false);
            requiresUniqueEmail =config.Get<bool>("requiresUniqueEmail",true);

            switch (config.Get<string>("passwordFormat", "Clear"))
            {
                case "Hashed":
                    passwordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    passwordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    passwordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            //Encryption skipped
            var cfg =
                            WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            _machineKey = (MachineKeySection)cfg.GetSection("system.web/machineKey");

            if (_machineKey.ValidationKey.Contains("AutoGenerate"))
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                    throw new ProviderException("Hashed or Encrypted passwords are not supported with auto-generated keys.");

            var connectionStringName = config["connectionStringName"];

            UnitOfWork.Configure(connectionStringName);

            ApplicationName = config["applicationName"];
            if (ApplicationName.IsNullOrEmpty())
                ApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            else if(ApplicationName != "/")
            {
                var site = UnitOfWork.Current.CreateRepository<Site>().FirstOrDefault(p => p.Id == ApplicationName && p.Status == Enums.SiteStatus.Enable);
                if(site == null)
                    throw new ProviderException("ApplicationName not exists.");
            }
        }

        public override bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            Guard.NotNullOrEmpty(userName, "userName");
            Guard.NotNullOrEmpty(oldPassword, "oldPassword");
            Guard.NotNullOrEmpty(newPassword, "newPassword");

            var q = UnitOfWork.Current.CreateRepository<User>();
            
            var usr = q
                .FirstOrDefault(p => p.ApplicationName == ApplicationName && p.UserName == userName);

            if (usr == null || usr.IsLockedOut || !ValidateUser(oldPassword,q,usr))
                return false;

            var args = new ValidatePasswordEventArgs(userName, newPassword, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                var ex = args.FailureInformation ?? new MembershipPasswordException("Change password canceled due to new password validation failure.");
                throw ex;
            }

            return q.Update(new { Password = EncodePassword(newPassword), LastPasswordChangedDate = DateTime.Now }, p => p.Id == usr.Id) > 0;
        }

        public override bool ChangePasswordQuestionAndAnswer(string userName, string password, string question, string answer)
        {
            Guard.NotNullOrEmpty(userName, "userName");
            Guard.NotNullOrEmpty(password, "password");
            Guard.NotNullOrEmpty(question, "question");
            Guard.NotNullOrEmpty(answer, "answer");

            var q = UnitOfWork.Current.CreateRepository<User>();
            var usr = q
                .FirstOrDefault(p => p.ApplicationName == ApplicationName && p.UserName == userName);

            if (usr == null || usr.IsLockedOut || !ValidateUser(password, q, usr))
                return false;
            return q.Update(new { Question = question, Answer = answer }, p => p.Id == usr.Id) > 0;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string question, string answer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);
            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            var q = UnitOfWork.Current.CreateRepository<User>();
            var usr = q
                .FirstOrDefault(p => p.ApplicationName == ApplicationName && p.UserName == username);
            if (usr != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            usr = NewUser(username, password, email, question, answer, isApproved);

            try
            {
                var effectRows = q.Insert(usr);
                if (effectRows < 1)
                {
                    status = MembershipCreateStatus.UserRejected;
                    return null;
                }

                status = MembershipCreateStatus.Success;
                return MapMembershipUser(usr);
            }
            catch(Exception ex)
            {
                status = MembershipCreateStatus.ProviderError;
                LogError(ex, "CreateUser", "ELMembershipProvider.CreateUser");
                return null;
            }
        }

        private MembershipUser MapMembershipUser(User usr)
        {
            var u = new MembershipUser(this.Name,
                                        usr.UserName,
                                        usr.Id,
                                        usr.Email,
                                        usr.Question,
                                        usr.Comment,
                                        usr.IsApproved,
                                        usr.IsLockedOut,
                                        usr.CreationDate,
                                        usr.LastLoginDate,
                                        usr.LastActivityDate,
                                        usr.LastPasswordChangedDate,
                                        usr.LastLockedOutDate);

            return u;
        }

        private User NewUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved)
        {
            var createDate = DateTime.Now;
            var user = new User();
            user.UserName = username;
            user.Password = EncodePassword(password);
            user.Email = email;
            user.Question = passwordQuestion;
            user.Answer = EncodePassword(passwordAnswer);
            user.IsApproved = isApproved;
            user.Comment = "";
            user.CreationDate = createDate;
            user.LastPasswordChangedDate = createDate;
            user.LastActivityDate = createDate;
            user.ApplicationName = ApplicationName;
            user.IsLockedOut = false;
            user.LastLockedOutDate = createDate;
            user.FailedPasswordAttemptCount = 0;
            user.FailedPasswordAttemptWindowStart = createDate;
            user.FailedAnswerAttemptCount = 0;
            user.FailedAnswerAttemptWindowStart = createDate;
            return user;
        }

        private void LogError(Exception e, string action,string operation)
        {
            string message = "An exception occurred communicating with the data source.\n\n";
            message += "Action: " + action + "\n\n";
            message += "Exception: " + e.ToString();

            Trace.WriteLine(message, operation);
        }

        public override bool DeleteUser(string userName, bool deleteAllRelatedData)
        {
            Guard.NotNullOrEmpty(userName, "userName");
            var unitOfWork = UnitOfWork.Current;
            var repository = unitOfWork.CreateRepository<User>();
            var userId = repository
                .Where(p => p.UserName == userName && p.ApplicationName == ApplicationName)
                .Select(p => p.Id)
                .FirstOrDefault();

            if (userId < 1)
                return false;

            try
            {
                bool flag = false;
                unitOfWork.UsingTransaction(() =>
                    {
                        flag = repository.Delete(p => p.Id == userId) > 0;
                        if (flag && deleteAllRelatedData)
                        {
                            flag = unitOfWork.CreateRepository<Profile>().Delete(p => p.UserId == userId) > 0;
                            if (flag)
                                flag = unitOfWork.CreateRepository<UserRole>().Delete(p => p.UserId == userId) > 0;
                        }
                    });
                return flag;
            }
            catch (Exception ex)
            {
                LogError(ex, "DeleteUser", "ELMembershipProvider.DeleteUser");
                return false;
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            Guard.NotNullOrEmpty(emailToMatch, "emailToMatch");
            if (pageIndex < 0)
                throw new ArgumentException("pageIndex < " + pageIndex);
            if (pageSize < 0)
                throw new ArgumentException("pageSize < " + pageSize);
            if ((pageIndex * pageSize + pageSize - 1) > int.MaxValue)
                throw new ArgumentException("(pageIndex * pageSize+pageSize-1) > int.MaxValue ");

            var q =  UnitOfWork.Current.CreateRepository<User>()
                .Where(p=>p.Email.Contains(emailToMatch))
                .Where(p=>p.ApplicationName == ApplicationName);

            totalRecords = q.Count();
            var users = q.Take(pageSize).Skip(pageSize * pageIndex).ToArray();

            var items = new MembershipUserCollection();
            foreach(var user in users)
                items.Add(MapMembershipUser(user));

            return items;
        }

        public override MembershipUserCollection FindUsersByName(string userNameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            Guard.NotNullOrEmpty(userNameToMatch, "userNameToMatch");
            if (pageIndex < 0)
                throw new ArgumentException("pageIndex < " + pageIndex);
            if (pageSize < 0)
                throw new ArgumentException("pageSize < " + pageSize);
            if ((pageIndex * pageSize+pageSize-1) > int.MaxValue)
                throw new ArgumentException("(pageIndex * pageSize+pageSize-1) > int.MaxValue ");

            var q =  UnitOfWork.Current.CreateRepository<User>()
               .Where(p => p.UserName.Contains(userNameToMatch))
               .Where(p => p.ApplicationName == ApplicationName);

            totalRecords = q.Count();
            var users = q.Take(pageSize).Skip(pageSize * pageIndex).ToArray();

            var items = new MembershipUserCollection();
            foreach (var user in users)
                items.Add(MapMembershipUser(user));

            return items;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            if (pageIndex < 0)
                throw new ArgumentException("pageIndex < " + pageIndex);
            if (pageSize < 0)
                throw new ArgumentException("pageSize < " + pageSize);
            if ((pageIndex * pageSize + pageSize - 1) > int.MaxValue)
                throw new ArgumentException("(pageIndex * pageSize+pageSize-1) > int.MaxValue ");

            var q =  UnitOfWork.Current.CreateRepository<User>()
               .Where(p => p.ApplicationName == ApplicationName);

            totalRecords = q.Count();
            var users = q.Take(pageSize).Skip(pageSize * pageIndex).ToArray();

            var items = new MembershipUserCollection();
            foreach (var user in users)
                items.Add(MapMembershipUser(user));

            return items;
        }

        public override int GetNumberOfUsersOnline()
        {
            TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);
            int numOnline = 0;

            var q =  UnitOfWork.Current.CreateRepository<User>()
             .Where(p => p.ApplicationName == ApplicationName)
             .Where(p=>p.LastActivityDate > compareTime)
             .Select(p=>p.Id)
             .Distinct();

            numOnline = q.Count();
            return numOnline;
        }

        public override string GetPassword(string userName, string answer)
        {
            Guard.NotNullOrEmpty(userName, "userName");
            Guard.NotNullOrEmpty(answer, "answer");

            if (!EnablePasswordRetrieval)
                throw new ProviderException("Password Retrieval Not Enabled.");

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
                throw new ProviderException("Cannot retrieve Hashed passwords.");

            var usr =  UnitOfWork.Current.CreateRepository<User>()
                .FirstOrDefault(p => p.ApplicationName == ApplicationName && p.UserName == userName);

            if(usr == null)
                throw new MembershipPasswordException("The supplied user name is not found.");
            if(usr.IsLockedOut)
                throw new MembershipPasswordException("The supplied user is locked out.");

            if (RequiresQuestionAndAnswer && !CheckPassword(answer, usr.Answer))
            {
                UpdatePasswordAnswerFailureCount(usr);
                throw new MembershipPasswordException("Incorrect password answer.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
                return UnEncodePassword(usr.Password);

            return usr.Password;
        }

        public override MembershipUser GetUser(string userName, bool userIsOnline)
        {
            Guard.NotNullOrEmpty(userName, "userName");
            var q = UnitOfWork.Current.CreateRepository<User>();
            var usr = q.FirstOrDefault(p => p.ApplicationName == ApplicationName && p.UserName == userName);
            if (usr == null) return null;

            if (userIsOnline)
                q.Update(new { LastActivityDate = DateTime.Now }, p => p.Id == usr.Id);
            return MapMembershipUser(usr);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            var usrId = (int)providerUserKey;
            var q = UnitOfWork.Current.CreateRepository<User>();
            var usr = q.FirstOrDefault(p =>p.Id == usrId);
            if (usr == null) return null;

            if (userIsOnline)
                q.Update(new { LastActivityDate = DateTime.Now }, p => p.Id == usrId);
            return MapMembershipUser(usr);
        }

        public override string GetUserNameByEmail(string email)
        {
            Guard.NotNullOrEmpty(email, "email");
            return  UnitOfWork.Current.CreateRepository<User>()
                .Where(p => p.Email == email)
                .Distinct()
                .Select(p=>p.UserName)
                .FirstOrDefault();
        }

        public override string ResetPassword(string userName, string answer)
        {
            Guard.NotNullOrEmpty(userName, "userName");
            Guard.NotNullOrEmpty(answer, "answer");

            if (!EnablePasswordReset)
                throw new NotSupportedException("Password reset is not enabled.");

            var q = UnitOfWork.Current.CreateRepository<User>();
            var usr =q.FirstOrDefault(p => p.ApplicationName == ApplicationName && p.UserName == userName);

            if (usr == null)
                throw new MembershipPasswordException("The supplied user name is not found.");
            if (usr.IsLockedOut)
                throw new MembershipPasswordException("The supplied user is locked out.");

            if (answer == null && RequiresQuestionAndAnswer)
            {
                UpdatePasswordAnswerFailureCount(usr);
                throw new ProviderException("Password answer required for password reset.");
            }

            if (answer != null && RequiresQuestionAndAnswer && CheckPassword(answer,usr.Answer))
            {
                UpdatePasswordAnswerFailureCount(usr);
                throw new ProviderException("Incorrect password answer.");
            }
            const int newPasswordLength = 8;
            string newPassword = System.Web.Security.Membership.GeneratePassword(newPasswordLength, MinRequiredNonAlphanumericCharacters);

            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(userName, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                throw args.FailureInformation ?? new MembershipPasswordException("Reset password canceled due to password validation failure.");

            usr.Password = EncodePassword(newPassword);
            usr.LastPasswordChangedDate = System.DateTime.Now;

            return q.Update(new { Password = EncodePassword(newPassword), LastPasswordChangedDate = DateTime.Now }, p => p.Id == usr.Id) > 0 ? newPassword : "";
        }

        public override bool UnlockUser(string userName)
        {
            Guard.NotNullOrEmpty(userName, "userName");
            return  UnitOfWork.Current.CreateRepository<User>()
                .Update(new { LastLockedOutDate = DateTime.Now, IsLockedOut = false }, p => p.ApplicationName == ApplicationName && p.UserName == userName)
                > 0;
        }

        public override void UpdateUser(MembershipUser user)
        {
            Guard.NotNull(user, "user");
            UnitOfWork.Current.CreateRepository<User>()
                .Update(new { Email = user.Email, Comment = user.Comment, IsApproved = user.IsApproved }, p => p.ApplicationName == ApplicationName && p.UserName == user.UserName);
        }

        public override bool ValidateUser(string userName, string password)
        {
            Guard.NotNullOrEmpty(userName, "userName");
            Guard.NotNullOrEmpty(password, "password");

            var q = UnitOfWork.Current.CreateRepository<User>();
            var usr = q
                .FirstOrDefault(p => p.ApplicationName == ApplicationName && p.UserName == userName);
            if (usr == null || usr.IsLockedOut)
                return false;

            return ValidateUser(password, q, usr);
        }

        private bool ValidateUser(string password, Data.IRepository<User> q, User usr)
        {
            if (CheckPassword(password, usr.Password))
            {
                if (usr.IsApproved)
                {
                    q.Update(new { LastLoginDate = DateTime.Now }, p => p.Id == usr.Id);
                    return true;
                }
                return false;
            }

            UpdatePasswordFailureCount(usr);
            return false;
        }

        private void UpdatePasswordFailureCount(User usr)
        {
            var failureCount = usr.FailedPasswordAttemptCount;
            var windowStart = usr.FailedPasswordAttemptWindowStart;

            DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

            var q = UnitOfWork.Current.CreateRepository<User>();

            if (failureCount == 0 || DateTime.Now > windowEnd)
            {
                q.Update(new { FailedPasswordAttemptCount = 1, FailedPasswordAttemptWindowStart = DateTime.Now }, p => p.Id == usr.Id);
                return;
            }

            if (failureCount++ >= MaxInvalidPasswordAttempts)
                q.Update(new { IsLockedOut = true, LastLockedOutDate = DateTime.Now }, p => p.Id == usr.Id);
            else
                q.Update(new { FailedPasswordAttemptCount = failureCount }, p => p.Id == usr.Id);
        }

        private void UpdatePasswordAnswerFailureCount(User usr)
        {
            var failureCount = usr.FailedAnswerAttemptCount;
            var windowStart = usr.FailedAnswerAttemptWindowStart;

            DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

            var q = UnitOfWork.Current.CreateRepository<User>();

            if (failureCount == 0 || DateTime.Now > windowEnd)
            {
                usr.FailedPasswordAttemptCount = 1;
                usr.FailedPasswordAttemptWindowStart = DateTime.Now;
                q.Update(new { FailedPasswordAnswerAttemptCount = 1, FailedPasswordAnswerAttemptWindowStart = DateTime.Now }, p => p.Id == usr.Id);
                return;
            }

            if (failureCount++ >= MaxInvalidPasswordAttempts)
                q.Update(new { IsLockedOut = true, LastLockedOutDate = DateTime.Now }, p => p.Id == usr.Id);
            else
                q.Update(new { FailedPasswordAnswerAttemptCount = failureCount }, p => p.Id == usr.Id);
        }


        private bool CheckPassword(string password, string dbpassword)
        {
            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    return password == Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(dbpassword)));
                case MembershipPasswordFormat.Hashed:
                     HMACSHA1 hash = new HMACSHA1();
                    hash.Key = HexToByte(_machineKey.ValidationKey);
                   return 
                      Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)))  == dbpassword;
                default:
                   return password == dbpassword;
            }
        }

        private string EncodePassword(string password)
        {
            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword =
                      Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    HMACSHA1 hash = new HMACSHA1();
                    hash.Key = HexToByte(_machineKey.ValidationKey);
                    encodedPassword =
                      Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }
            return encodedPassword;
        }

        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password =
                      Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }
    }
}
