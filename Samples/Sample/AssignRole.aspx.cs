using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Configuration.Provider; 

namespace Sample4
{
    public partial class AssignRole : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             
            string au = Page.User.Identity.Name;
            if (Page.User.Identity.IsAuthenticated)
                this.UserInfo1.LoggedinName = String.Format("Welcome : {0}", au);
            else

                Server.Transfer("~/Default.aspx");

            if (!IsPostBack)
                RefreshControls();

        }

        protected void btnAddrole_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (String.IsNullOrEmpty(this.txtrolename.Text))
                    this.lblMessage.Text = "Role name is cannot be empty.";
                if (this.txtrolename.Text.Length > 0)
                {
                    string r = this.txtrolename.Text.Trim();
                    Roles.CreateRole(r);
                    RefreshRoles();
                    this.lblMessage.Text = String.Format("Role {0} created.", r);
                }
            }
            catch (ProviderException ex1)
            {
                this.lblMessage.Text = ex1.Message;
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = ex.Message;
            }


        }

        protected void btnRemoveRole_Click(object sender, EventArgs e)
        {
            try
            {

                if (String.IsNullOrEmpty(this.ddlRoles.SelectedItem.Value))
                {
                    this.lblMessage.Text = "Role name is cannot be empty.";
                    return;
                }

                if (this.ddlRoles.SelectedItem.Value.Length > 0)
                {
                    string r = this.ddlRoles.SelectedItem.Value;
                    Roles.DeleteRole(r);
                    RefreshRoles();
                    this.lblMessage.Text = String.Format("Role {0} deleted.",r);
                }
            }
            catch (ProviderException ex1)
            {
                this.lblMessage.Text = ex1.Message;
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = ex.Message;
            }

        }

        protected void btnAddRoleToUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(this.ddlUser.SelectedItem.Value))
                {
                    this.lblMessage.Text = "User name is cannot be empty.";
                    return;
                }


                if (String.IsNullOrEmpty(this.ddlRoles2.SelectedItem.Value))
                {
                    this.lblMessage.Text = "Role name is cannot be empty.";
                    return;
                }

                if (this.ddlRoles2.SelectedItem.Value.Length > 0)
                {
                    string u = this.ddlUser.SelectedItem.Value;
                    string r = this.ddlRoles2.SelectedItem.Value;
                    Roles.AddUsersToRole(new string[] {u},r );
                    RefreshRoles();
                    this.lblMessage.Text = String.Format("Role {0} added to user {1}.",r,u);
                }
            }
            catch (ProviderException ex1)
            {
                this.lblMessage.Text = ex1.Message;
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = ex.Message;
            }

        }

        protected void btnRemoveRoleFromUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(this.ddlUser3.SelectedItem.Value))
                {
                    this.lblMessage.Text = "User name is cannot be empty.";
                    return;
                }


                if (String.IsNullOrEmpty(this.ddlRoles3.SelectedItem.Value))
                {
                    this.lblMessage.Text = "Role name is cannot be empty.";
                    return;
                }

                if (this.ddlRoles3.SelectedItem.Value.Length > 0)
                {
                    string u = this.ddlUser3.SelectedItem.Value;
                    string r = this.ddlRoles3.SelectedItem.Value;
                    Roles.RemoveUserFromRole(u, r);
                    RefreshRoles();
                    this.lblMessage.Text = String.Format("Role {0} removed from user {1}.", r, u);
                }
            }
            catch (ProviderException ex1)
            {
                this.lblMessage.Text = ex1.Message;
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = ex.Message;
            }

        }
        private void RefreshControls()
        {
            this.lblMessage.Text = string.Empty;
            this.txtrolename.Text = string.Empty; 
            RefreshUsers();
            RefreshRoles();
        }

        private void RefreshRoles()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();  
            //show the roles
            string[] roles = Roles.GetAllRoles();
            
            SortedList<string, string> rolesUsers = new SortedList<string, string>();
            foreach (string s in roles)
            {

                string[] users = Roles.GetUsersInRole(s);
                rolesUsers.Add(s,ConvertStringArrayToString(users)); 
            }

               
            this.gridviewRoles.DataSource = rolesUsers;
            this.gridviewRoles.DataBind();

            
           
            //add roles to drop down
            this.ddlRoles.Items.Clear();
            this.ddlRoles.DataSource = roles;
            this.ddlRoles.DataBind();

            this.ddlRoles2.Items.Clear();
            this.ddlRoles2.DataSource = roles;
            this.ddlRoles2.DataBind();

            this.ddlRoles3.Items.Clear();
            this.ddlRoles3.DataSource = roles;
            this.ddlRoles3.DataBind();

        }
        private void RefreshUsers()
        {
            //list all users
            MembershipUserCollection users = Membership.GetAllUsers();
            this.gridviewUsers.DataSource = users;
            this.gridviewUsers.DataBind();

            this.ddlUser.DataSource = users;
            this.ddlUser.DataBind();

            this.ddlUser3.DataSource = users;
            this.ddlUser3.DataBind();  

        }

        static string ConvertStringArrayToString(string[] array)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append(',');
            }
            if (builder.Length > 0)
                builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        protected void ddlRoles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
