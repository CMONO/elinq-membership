using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;

namespace Sample4
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            string userName = Login1.UserName;
            string password = Login1.Password;


                MembershipUser userInfo = Membership.GetUser(Login1.UserName);
                if (userInfo == null)
                {
                    this.Login1.FailureText = "Invalid username or password";
                }
                else
                {
                    if (!userInfo.IsApproved)
                        Login1.FailureText = "Your account has not yet been approved by the site's administrators. Please try again later...";
                    else if (userInfo.IsLockedOut)
                        Login1.FailureText = "Your account has been locked out(exceeded incorrect login attempts). Please contact Site administrators";
                    else
                    {
                        //check password
                        if (Membership.ValidateUser(userName, password))
                        {
                            e.Authenticated = true;
                            this.Login1.DestinationPageUrl = "~/Members.aspx";  
                        }
                        else
                            this.Login1.FailureText = "Invalid username or password";
                    }
                }
        }
    }
}
