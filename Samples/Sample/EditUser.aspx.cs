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
    public partial class EditUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string au = Page.User.Identity.Name;
            if (Page.User.Identity.IsAuthenticated)
                this.UserInfo1.LoggedinName = String.Format("Welcome : {0}", au);
            else
                Server.Transfer("~/Default.aspx");

            if (!IsPostBack)
            {
                MembershipUser userInfo = Membership.GetUser(au);
                if (userInfo == null)
                {
                    Server.Transfer("~/Default.aspx");
                }
                else
                {
                    this.txtUsernm.Text = userInfo.UserName;
                    this.txtEmail.Text = userInfo.Email;

                }
            }
        }

        protected void update_Click(object sender, EventArgs e)
        {

            MembershipUser userInfo = Membership.GetUser(this.txtUsernm.Text);
            if (userInfo == null)
            {
                Server.Transfer("~/Default.aspx");
            }
            else
            {
                userInfo.Email= this.txtEmail.Text;
                Membership.UpdateUser(userInfo);
                this.lblEmail.Text = "The email was updated";

            }

        }
    }
}
