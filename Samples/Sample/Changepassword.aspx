<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Changepassword.aspx.cs" Inherits="Sample4.Changepassword" %>
<%@ Register TagPrefix ="uc1" TagName ="UserInfo"  src="~/StatusControl.ascx" %>
<%@ Register TagPrefix ="uc1" TagName ="Copyright"  src="~/Copyrights.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Chnage password</title>
        <link href="/App_Themes/Blue/Simple.css" type = "text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div align ="right"  >
      <uc1:UserInfo id="UserInfo1" runat ="server"></uc1:UserInfo>    
    </div>
    <div align ="center" >
            <asp:ChangePassword ID="ChangePassword1" runat="server" BackColor="#F7F6F3" 
                    BorderColor="#E6E2D8" BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" 
                    Font-Names="Verdana" Font-Size="0.8em" Height="202px" Width="643px" 
                ContinueDestinationPageUrl="~/Default.aspx">
                    <CancelButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" 
                        BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" 
                        ForeColor="#284775" />
                    <PasswordHintStyle Font-Italic="True" ForeColor="#888888" />
                    <ContinueButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" 
                        BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" 
                        ForeColor="#284775" />
                    <ChangePasswordButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" 
                        BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" 
                        ForeColor="#284775" />
                    <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="0.9em" 
                        ForeColor="White" />
                    <TextBoxStyle Font-Size="0.8em" />
                    <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            </asp:ChangePassword>
    </div>

    <uc1:Copyright id="copyright1" runat ="server"></uc1:Copyright>
    </form>
</body>
</html>
