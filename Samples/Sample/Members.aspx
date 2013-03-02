<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Members.aspx.cs" Inherits="Sample4.Members" %>
<%@ Register TagPrefix ="uc1" TagName ="UserInfo"  src="~/StatusControl.ascx" %>
<%@ Register TagPrefix ="uc1" TagName ="Copyright"  src="~/Copyrights.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Members Page</title>
    <link href="/App_Themes/Blue/Simple.css" type = "text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div align ="right"  >
        <uc1:UserInfo id="UserInfo1" runat ="server"></uc1:UserInfo>
    </div>
   <div  align ="center"  id= "AllUser">
       <asp:HyperLink ID="lnkChgPwd" class="styleL" NavigateUrl ="~/Changepassword.aspx" runat="server">修改密码</asp:HyperLink><br />
       <asp:HyperLink ID="lnkEditUser" class="styleL" NavigateUrl ="~/EditUser.aspx" runat="server">修改邮箱</asp:HyperLink><br /><br />
       <asp:Label ID="Label1" runat="server" Text=" (For admins) "></asp:Label><br />
       <asp:HyperLink ID="HyperLink1" class="styleL" NavigateUrl ="~/AssignRole.aspx" runat="server">管理用户和角色</asp:HyperLink>
          &nbsp; 
       <br />
       <br />
       <br /><br />
       

   </div>
    <uc1:Copyright id="copyright1" runat ="server"></uc1:Copyright>
    </form>
</body>
</html>
