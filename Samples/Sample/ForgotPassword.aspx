<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Sample4.ForgotPassword" %>
<%@ Register TagPrefix ="uc1" TagName ="UserInfo"  src="~/StatusControl.ascx" %>
<%@ Register TagPrefix ="uc1" TagName ="Copyright"  src="~/Copyrights.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Forgot password</title>
    <link href="/App_Themes/Blue/Simple.css" type = "text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
     <div align ="right"  >
        <uc1:UserInfo id="UserInfo1" runat ="server"></uc1:UserInfo>
    </div>
    <div align = "center">
    <asp:passwordrecovery ID="Passwordrecovery1" runat="server" BackColor="#F7F6F3" 
            BorderColor="#E6E2D8" BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" 
            Font-Names="Verdana" Font-Size="0.8em" Height="125px" Width="402px" 
            onsendingmail="Passwordrecovery1_SendingMail"  
            SuccessText="Your password has been sent to you. Click Home to login.">
        <MailDefinition From="netcasewqs@gmail.com" Subject="Reset Password">
        </MailDefinition>
        <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
        <SuccessTextStyle Font-Bold="True" ForeColor="#5D7B9D" />
        <TextBoxStyle Font-Size="0.8em" />
        <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" 
            ForeColor="White" />
        <SubmitButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" 
            BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" 
            ForeColor="#284775" />
        </asp:passwordrecovery>
    </div>
    <uc1:Copyright id="copyright1" runat ="server"></uc1:Copyright>
    </form>
</body>

</html>
