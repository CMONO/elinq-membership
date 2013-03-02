using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLite.Membership.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ApplicationName { get; set; }
        public string Email { get; set; }

        public string Comment { get; set; }
        public string Password { get; set; }

        public string Question { get; set; }
        public string Answer { get; set; }

        public bool  IsApproved { get; set; }
        public DateTime LastActivityDate { get; set; }

        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime CreationDate { get; set; }

        public bool IsOnLine { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime LastLockedOutDate { get; set; }

        public int FailedPasswordAttemptCount { get; set; }
        public int FailedAnswerAttemptCount { get; set; }

        public DateTime FailedPasswordAttemptWindowStart { get; set; }
        public DateTime FailedAnswerAttemptWindowStart { get; set; }

        public Site Site { get; set; }
        public IList<UserRole> UserRoles { get; set; }


        public User()
        {
            this.CreationDate = Utils.MinDate;
            this.LastPasswordChangedDate = Utils.MinDate;
            this.LastActivityDate = Utils.MinDate;
            this.LastLockedOutDate = Utils.MinDate;
            this.FailedAnswerAttemptWindowStart = Utils.MinDate;
            this.FailedPasswordAttemptWindowStart = Utils.MinDate;
            this.LastLoginDate = Utils.MinDate; 
        }
       
    }
}
