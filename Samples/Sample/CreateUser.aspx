<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs" Inherits="Sample4.CreateUser" %>
<%@ Register TagPrefix ="uc1" TagName ="UserInfo"  src="~/StatusControl.ascx" %>
<%@ Register TagPrefix ="uc1" TagName ="Copyright"  src="~/Copyrights.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Create a new user</title>
      <link href="/App_Themes/Blue/Simple.css" type = "text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div align ="right"  >
        <uc1:UserInfo id="UserInfo1" runat ="server"></uc1:UserInfo>
    </div>
    <div align="center">
    <br />
     <h3 class="styleP" >用户注册</h3>
     <br />
        <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" BackColor="#F7F6F3" 
            BorderColor="#E6E2D8" BorderStyle="Solid" BorderWidth="1px" 
            Font-Names="Verdana" Font-Size="0.8em" Height="311px" 
            oncreateduser="CreateUserWizard1_CreatedUser" 
            Width="362px" 
            CompleteSuccessText="Your account has been successfully created. Click continue to go to memebrs page" 
            DuplicateUserNameErrorMessage="This user name is already taken. Please enter a different user name." 
            oncontinuebuttonclick="CreateUserWizard1_ContinueButtonClick">
            <SideBarStyle BackColor="#5D7B9D" BorderWidth="0px" Font-Size="0.9em" 
                VerticalAlign="Top" />
            <SideBarButtonStyle BorderWidth="0px" Font-Names="Verdana" ForeColor="White" />
            <ContinueButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" 
                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" 
                ForeColor="#284775" />
            <NavigationButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" 
                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" 
                ForeColor="#284775" />
            <HeaderStyle BackColor="#5D7B9D" BorderStyle="Solid" Font-Bold="True" 
                Font-Size="0.9em" ForeColor="White" HorizontalAlign="Center" />
            <CreateUserButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" 
                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" 
                ForeColor="#284775" />
            <TitleTextStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <StepStyle BorderWidth="0px" />
            <WizardSteps>
                <asp:CreateUserWizardStep runat="server" />
                <asp:CompleteWizardStep runat="server" />
            </WizardSteps>
        </asp:CreateUserWizard>
    
    </div>
    <uc1:Copyright id="copyright1" runat ="server"></uc1:Copyright>
    </form>
</body>
</html>
