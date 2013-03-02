<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignRole.aspx.cs" Inherits="Sample4.AssignRole" %>
<%@ Register TagPrefix ="uc1" TagName ="UserInfo"  src="~/StatusControl.ascx" %>
<%@ Register TagPrefix ="uc1" TagName ="Copyright"  src="~/Copyrights.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Assign roles to users</title>
      <link href="/App_Themes/Blue/Simple.css" type = "text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div align ="right"  >
        <uc1:UserInfo id="UserInfo1" runat ="server"></uc1:UserInfo>
    </div>
    <div align="left" style="padding:25px;">
        <br />
        <br />
        <asp:GridView ID="gridviewUsers" runat="server" CellPadding="7" ForeColor="#333333" 
           GridLines="None" AutoGenerateColumns="False" CellSpacing="1">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:BoundField DataField="UserName" HeaderText="姓名" />
                <asp:BoundField DataField="Email" HeaderText="邮件" />
                <asp:BoundField DataField="PasswordQuestion" HeaderText="问题" />
                <asp:BoundField DataField="IsLockedOut" HeaderText="锁定" />
                <asp:BoundField DataField="CreationDate" HeaderText="创建日期" />
                <asp:BoundField DataField="LastLoginDate" HeaderText="上次登录时间" />
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <br />
        <asp:HyperLink ID="lnkAddUser"   class="styleL" NavigateUrl="~/CreateUser.aspx" runat="server">创建用户</asp:HyperLink>
        <br />
        <br />
        <br />
        <hr size=".5"  />
        <br />
        <br />
        <br />
        <br />
        <asp:GridView ID="gridviewRoles" runat="server" CellPadding="7" 
            ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" 
            CellSpacing="1">
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:BoundField DataField="Key" HeaderText="角色" />
                <asp:BoundField DataField="Value" HeaderText="用户" />
            </Columns>
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <br />
           
           <!--Add/remove role-->
           <br />
            &nbsp;&nbsp;<asp:Label  class="styleN" ID="Label3" runat="server" Text="角色名称: "></asp:Label>
            &nbsp;<asp:TextBox ID="txtrolename" runat="server" EnableViewState="False"></asp:TextBox>
            &nbsp;&nbsp;<asp:Button ID="btnAddrole" runat="server" Text="创建角色" 
            onclick="btnAddrole_Click" EnableViewState="False" />
        <br />
        <br />
        &nbsp;&nbsp; 选择角色&nbsp;<asp:DropDownList ID="ddlRoles" runat="server" 
            onselectedindexchanged="ddlRoles_SelectedIndexChanged">
        </asp:DropDownList>
            &nbsp;&nbsp;<asp:Button ID="btnRemoveRole" runat="server" 
            Text="删除角色" onclick="btnRemoveRole_Click" EnableViewState="False" />
            
            <!--Add/remove role from a user-->
            <hr size=".5"  />
        <br />
            &nbsp;&nbsp;<asp:Label  
            class="styleN" ID="Label7" runat="server" Text="选择用户: "></asp:Label>
            &nbsp;<asp:DropDownList ID="ddlUser" runat="server">
        </asp:DropDownList>
            &nbsp;&nbsp;<asp:Label  class="styleN" ID="Label8" runat="server" 
            Text="选择角色: "></asp:Label>
            &nbsp;<asp:DropDownList ID="ddlRoles2" runat="server">
        </asp:DropDownList>
            &nbsp;&nbsp;<asp:Button ID="btnAddRoleToUser" runat="server" 
            Text="设置用户角色" onclick="btnAddRoleToUser_Click" 
            EnableViewState="False" />
        <br />
        <br />
            &nbsp;&nbsp;<asp:Label  
            class="styleN" ID="Label10" runat="server" Text="选择用户: "></asp:Label>
            &nbsp;<asp:DropDownList ID="ddlUser3" runat="server">
        </asp:DropDownList>
            &nbsp;&nbsp;<asp:Label  class="styleN" ID="Label11" runat="server" 
            Text="选择角色: "></asp:Label>
            &nbsp;<asp:DropDownList ID="ddlRoles3" runat="server">
        </asp:DropDownList>
            &nbsp;&nbsp;<asp:Button ID="btnRemoveRoledFromUser" runat="server" 
            Text="移除用户角色" 
            EnableViewState="False" onclick="btnRemoveRoleFromUser_Click" />
        <br />
        <br />
        <br />
        <asp:Label   ID="lblMessage" runat="server" ForeColor="Red" style="font-weight: 700"></asp:Label>
           
    </div>
    
    <uc1:Copyright id="copyright1" runat ="server"></uc1:Copyright>
    </form>
</body>
</html>
