using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sample4
{
    public partial class Members : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string au  = Page.User.Identity.Name;
            if (Page.User.Identity.IsAuthenticated)
                this.UserInfo1.LoggedinName = String.Format("Welcome : {0}", au);
            else

                Server.Transfer("~/Default.aspx");

        }

        protected void LoginStatus1_LoggingOut(object sender, LoginCancelEventArgs e)
        {

        }

        protected void LoginStatus1_LoggedOut(object sender, EventArgs e)
        {

        }


    }
}
