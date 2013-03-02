<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="Sample4.EditUser" %>
<%@ Register TagPrefix ="uc1" TagName ="UserInfo"  src="~/StatusControl.ascx" %>
<%@ Register TagPrefix ="uc1" TagName ="Copyright"  src="~/Copyrights.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Update Email</title>
     <link href="/App_Themes/Blue/Simple.css" type = "text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div align ="right"  >
        <uc1:UserInfo id="UserInfo1" runat ="server"></uc1:UserInfo>
    </div>
    
    <div  id = "Alluser" align ="center" >
    <table>
    <tr><td align ="right" ><asp:Label class="styleL" ID="lblUsernmtxt" runat="server"  
            Text="姓名 :" ></asp:Label></td>
        <td><asp:TextBox ID="txtUsernm" runat="server" ReadOnly="True" Enabled="False" Width = "250px" ></asp:TextBox></td>
    </tr>
    <tr><td align ="right"><asp:Label class="styleL" ID="lblEmail" runat="server"  
            Text="邮件 :"></asp:Label></td>
        <td><asp:TextBox Width = "250px" ID="txtEmail" runat="server"></asp:TextBox></td>

        <tr><td align ="right"></td>
        <td align ="right"><asp:Button ID="update" runat="server" Text="更新" 
                onclick="update_Click" /></td>
    </tr>
    <tr><td colspan =2><asp:Label class="styleER" ID="lblmsg" runat="server"  Text="" ></asp:Label></td></tr>
    
    </table>
   
    
      
    <uc1:Copyright id="copyright1" runat ="server"></uc1:Copyright>
    </div>
    </form>
</body>
</html>
