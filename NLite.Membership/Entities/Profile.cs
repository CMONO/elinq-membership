using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLite.Membership.Entities
{
    public class Profile
    {
        public int Id { get;  set; }
        public int UserId { get; set; }
        public string ApplicationName { get; set; }
        public bool IsAnonymous { get;  set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string Subscription { get; set; }
        public string Language { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Occupation { get; set; }
        public string Website { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }

        public User User { get; set; }

        public Profile()
        {
            LastActivityDate = Utils.MinDate;
            LastUpdatedDate = Utils.MinDate;
            BirthDate = Utils.MinDate;  
        }
    }


}
