<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmGrowthChart.aspx.cs" Inherits="PalashDynamics.Web.GrowthChart.frmGrowthChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
        <tr><td><img src="<%=strImage%>" id="idtest" width= "600"/></td>
           <%-- <td valign="top"><font style="font-size:10px" face="Arial">
                <%=strWeightPerct%><br>
                <%=strHeightPerct%><br>
                <%=strBMIPerct%><br>
                <%=strW_FOR_L%><br><br>
                <%=strWeightPerct2%><br>
                <%=strHeightPerct2%><br>
                <%=strBMIPerct2%><br>
                <%=strHCPerct%><br>
                <%=strW_FOR_L2%><br>
                <br>
                </font>
            </td>--%>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
