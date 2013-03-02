<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Sample4._Default" %>
<%@ Register TagPrefix ="uc1" TagName ="Copyright"  src="~/Copyrights.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Sample ELinq Provider Application</title>
    <link href="/App_Themes/Blue/Simple.css" type = "text/css" rel="stylesheet" />
     
</head>
<body>
    <form id="form1" runat="server">
    <h2 class="styleM" style="font-weight: bold">欢迎来到 ELinq Membership Provider Demo App</h2>
    <div align ="center">
    
        <asp:Login ID="Login1" runat="server" Height="168px" Width="305px" 
            BackColor="#F7F6F3" BorderColor="#E6E2D8" BorderPadding="4" BorderStyle="Solid" 
            BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" 
            ForeColor="#333333" 
            DisplayRememberMe="false" RememberMeSet="false"
            onauthenticate="Login1_Authenticate">
            
            <TextBoxStyle Font-Size="0.8em" />
            <LoginButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" BorderStyle="Solid" 
                BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284775" />
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" 
                ForeColor="White" />
        </asp:Login>
        <br />
    <asp:HyperLink  ID="lnkForgotPwd"  class="styleL"  NavigateUrl="~/ForgotPassword.aspx" runat="server">忘记密码</asp:HyperLink>
    <asp:HyperLink  ID="HyperLink1" class="styleL"  NavigateUrl="~/CreateUser.aspx" runat="server">注册</asp:HyperLink>
    </div>
    
    <uc1:Copyright id="copyright1" runat ="server"></uc1:Copyright>
    </form>
</body>
</html>
