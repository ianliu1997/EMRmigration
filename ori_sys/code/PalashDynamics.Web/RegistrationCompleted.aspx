<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistrationCompleted.aspx.cs" Inherits="PalashDynamics.Web.RegistrationCompleted" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table class="style1">
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessahe" runat="server" 
                        Text="Your Registration is successfully completed." Font-Bold="True" 
                        ForeColor="#00CCFF"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage1" runat="server" Text="Please Contact in SHS."></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
