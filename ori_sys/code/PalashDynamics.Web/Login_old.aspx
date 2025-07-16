<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login_old.aspx.cs" Inherits="PalashDynamics.frmLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <style>
 body
 {
     font-family:Arial;
     font-size:64%;
     left : 10px;
 }
.input
{
    border:1px solid #617584;
    background-color:#FFFFFF;
}
.frmButton
{
    width:80px;
     font-size:100%;
	 font-weight:bold;	
     vertical-align:middle;
	 
	 background-color:#FEFEFE;
	 border:1px solid #A2ADB8;
	 height:22px;
	 text-decoration: none;
	 cursor:pointer;
	 letter-spacing:1px;
	 width:60px;
     }
    .style1
    {
        width: 80px;
    }
</style> 
    <script type="text/javascript">
     function setHourglass() {
         document.body.style.cursor = 'wait';
     }

 </script>

 <%--<script language="javascript">
     function fullscreen()
     { window.open('PalashDynamicsTestPage.aspx', 'kyscorp', 'width=' + screen.width + ',height=' + screen.height + ',top=0,left=0'); }  
 </script> --%>
 
</head>

<body  onbeforeunload="setHourglass();" onunload="setHourglass();" >

 <form id="form1" runat="server" >
 <center style="height:100%; width:100%" >
  <div  style="background:url(CIMS.jpg) no-repeat center top; left:0px; right:0px; top:50px; height:400px; width:700px; position:relative;">
	<div style="left:455px; top:125px; right:3px; position: absolute; text-align:left;">
		<fieldset style="border:0px solid #C0C0C0; padding:5px; font-weight:900; color: White; font-style: normal; width: 230px;">
			                 
              <legend style="font-size:small ; color:White; font-weight: bold ; vertical-align:bottom ;" >Login Information</legend>
       
            <table cellspacing="1" cellpadding="1" style="width: 106%; height: 106px;">
                <tr>
                  <td colspan="2">
                  <%--<asp:Label ID="Label1" runat="server" ForeColor="Red" Font-Bold="true" Text="">
                 </asp:Label>--%>
               <%--  <br />--%>
           
                 </td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Label ID="Error" runat="server" ForeColor="Blue" 
                            Font-Bold="True"></asp:Label></td>
                </tr>
                <tr>
                    <td style="font-weight:bold; color:White;" class="style1" >Login Name </td>
                    <td><asp:TextBox ID="txtLoginName" Text="" MaxLength="100" BorderColor="Red" ToolTip="Please Enter your Login Name." runat="server" CssClass="input" 
                            Width="125px" ></asp:TextBox></td>
                </tr>
    
                <tr>
                    <td style="font-weight:bold; color: White;" class="style1">Password</td>
                    <td><asp:TextBox ID="txtPassword" runat="server" MaxLength="50" BorderColor="Red" ToolTip="Please Enter your Password." CssClass="input" Width="125px" 
                            TextMode="Password" ></asp:TextBox></td>
                </tr>
    
                <tr>
                    <td style="font-weight:bold; color: White;" class="style1">Select Clinic</td>
                    <td>
                          <asp:DropDownList ID="ddlUnits" runat="server" Width="125px" >
                          </asp:DropDownList>
      
                    </td>
                </tr>

                <tr>
                    <td class="style1">&nbsp;</td>
                    <td><asp:Button ID="btn" Text="Login" runat="server" OnClick="OK_Click" 
                            CssClass="frmButton" Font-Bold="False" Font-Size="Small" Height="20px" 
                            Width="125px" BackColor="White" ForeColor="Black" /></td>
                </tr>

                <tr>
                    <td class="style1">&nbsp;</td>
                     <td align="center"> 
                    <asp:LinkButton ID="lnkForgotPassword" Font-Bold="true" runat="server" 
                             ForeColor="Blue" PostBackUrl="~/ForrgotPassword.aspx" >Forgot Password? </asp:LinkButton>  
                    <%--<asp:HyperLink ID="lnkForgotPassword1" Font-Bold="false" runat="server" ForeColor="Red" NavigateUrl="~/ForrgotPassword.aspx" Target="_blank" >Forgot Password?</asp:HyperLink>--%>
                    </td>
                </tr>

            </table>
	    </fieldset>
	</div>
  </div>
  <div  style="position:relative; bottom:1px; text-align:center; width:90%; left: 37px;" >
         <%--<center style="font-size:10px; font-weight:bold">
                Designed & Developed by SEED Healthcare Solutions Pvt. Ltd. For details contact <a href="mailto:info@seedhealthcare.com">info@seedhealthcare.com</a>
         </center>--%>
  </div>
 </center>
 </form>
 <%--<script language=javascript>
     window.open('PalashDynamicsTestPage.aspx', '', 'resizable=no, toolbar=no, menubar=no, left=0, top=0, height=710, width=1010');
     window.opener = null; window.close();
</script>--%>
</body>
    
</html>

