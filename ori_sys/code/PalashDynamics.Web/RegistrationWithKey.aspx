<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistrationWithKey.aspx.cs" Inherits="PalashDynamics.Web.RegistrationWithKey" %>

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
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblActivation" runat="server" Text="Activation Key"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblKey" runat="server" Text=" "></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnOnline" runat="server" Text="Online Registration" 
                        onclick="btnOnline_Click" />
                </td>
                <td>
                    <asp:Button ID="btnSHS" runat="server" Text="Register via SHS" 
                        onclick="btnSHS_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
