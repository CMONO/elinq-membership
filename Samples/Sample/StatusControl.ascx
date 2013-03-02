<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatusControl.ascx.cs" Inherits="Sample4.StatusControl" %>
        <asp:Label  class="styleP" ID="lblUserName" runat="server" Text=""></asp:Label>
          &nbsp;|
        <asp:LoginStatus ID="LoginStatus1" runat="server" 
            LoginText="[登录]" 
            LogoutText="[注销]" 
            LogoutPageUrl="~/Default.aspx" 
            LogoutAction="Redirect" />
          &nbsp;|&nbsp;<asp:HyperLink ID="hypHome" NavigateUrl ="~/Default.aspx" runat="server">首页</asp:HyperLink>