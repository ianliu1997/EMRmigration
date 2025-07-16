<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForrgotPassword.aspx.cs" Inherits="PalashDynamics.Web.ForrgotPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
<style>
 .body
 {
     font-family:Arial;     
     font-size:medium;     
     color: Red;    
     left : 10px;
     
 }
.input
{
    border:1px solid #617584;
    background-color:#FFFFFF;
}
.frmLabel
{
       font-family:Arial; 
       font-size:small; 
       
       
     color:Blue;    

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
</head>
<body  onbeforeunload="setHourglass();" onunload="setHourglass();" class="body" >
 <form id="form1" runat="server"  >
	<center style="height:100%; width:100%"  >
        <div  style="background:url(CIMSFPwd.jpg) no-repeat; left:0px; right:0px; top:50px; height:600px; width:950px; position:relative;">
	<div style="left:445px; top:95px;  right:1px; position: absolute; text-align:left;">
		<fieldset style="border:0px solid #C0C0C0; padding:5px; font-weight:900; color: #0000CC; font-style: normal; width:325px;">
			<%--<legend style="font-size: small; font-weight: bold">Login Information   </legend>--%>
            <table scellspacing="1" cellpadding="1" style="width: 150%; height: 106px;" >
            
            <tr>
                <td align="right">
                    <asp:Label  ID="Label2" runat="server" Text="Login" Class="frmLabel"> </asp:Label>
                </td>

                <td align ="left">
                    <asp:TextBox ID="txtLogin" MaxLength="100" ToolTip="Enter your Login Name" BorderColor="Red"  runat="server" width="124px" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblSecretQuestion" runat="server" Text="Secret Question" Class="frmLabel"></asp:Label>
                </td>
                <td align ="left">
                  
                     <asp:DropDownList ID="ddllSecretQtn" ToolTip="Select your secret qusetion"  MaxLength="255" runat="server" width="324px">
                     </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblSecretAnswer" runat="server" Text="Secret Answer" Class="frmLabel"></asp:Label>
                </td>
                <td align ="left">
                     <asp:TextBox ID="txtSecreteAnswer" TextMode="Password" 
                         ToolTip="Enter your Secret Answer." BorderColor="Red" MaxLength="255" 
                         runat="server" width="323px"/>
                </td>
            </tr>

            <tr>
                <td align="right">                    
                     <asp:Label ID="lblNewPassword" runat="server" Text="New Password" Class="frmLabel"></asp:Label>
                </td>
                <td align="left">
                     <asp:TextBox ID="txtNewPassword" runat="server" BorderColor="Red" MaxLength="50" TextMode="Password" ToolTip="Enter the New Password." width="124px"></asp:TextBox>
                     &nbsp;
                </td>
            </tr>

             <tr>
                <td align="right" >
                  
                     <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password" Class="frmLabel"></asp:Label>
                </td>
                <td align="left">
                     <asp:TextBox ID="txtConfirmPassword" TextMode="Password" MaxLength="50" BorderColor="Red" ToolTip="Enter The Password to Confirm." runat="server" Width="124px"></asp:TextBox>
                     &nbsp;
                </td>
            </tr>

           
            <tr>
                <td align="right">
                    
                </td>
            
                <td>
                     
                </td>
            </tr>
            <tr >
                <td colspan="2" align="right" style="width:100%">
                    <asp:Button ID="btnOK" runat="server" Text="OK" CssClass="frmButton" Font-Bold="False" Font-Size="Small" Height="20px" 
            Width="123px" BackColor="#FCDBFA" ForeColor="#0033CC" 
                        onclick="btnOK_Click" />
                       
                    &nbsp;
                    <asp:Button  ID="btnCancel" runat="server" Text="Cancel"  CssClass="frmButton" Font-Bold="False" Font-Size="Small" Height="20px" 
            Width="123px" BackColor="#FCDBFA" ForeColor="#0033CC" 
                        onclick="btnCancel_Click"/>
                </td>
            </tr>
           
            <tr>
                <td align="right">
                 <%-- <asp:Label id="lblLoginMsg" style="color:Green" runat="server" Visible="False" 
                        Font-Bold="True"></asp:Label>--%>
                </td>
            
                <td>
                   <%-- <asp:Label id="lblConfirmMsg" style="color:Green" runat="server" 
                        Visible="False" Font-Bold="True"></asp:Label>--%>
                </td>
            </tr>
          <%--   <tr>
                <td  align="right">
                    <asp:Label id="lblSecretQMsg" style="color:Green" runat="server" 
                        Visible="False" Font-Bold="True"></asp:Label>
                </td>
            
                <td>
                    <asp:Label id="lblSecretAMsg" style="color:Green" runat="server" 
                        Visible="False" Font-Bold="True"></asp:Label>
                </td>
            </tr>--%>
            

           
          <%--  <tr>
                <td colspan="2" align="right">

                    <asp:Label id="lblMessage" style="color:Green" runat="server"  
                        Text="Your password has been sent to your email-id" Visible="False" 
                        Font-Bold="True"></asp:Label>
                </td>

            </tr>--%>
           
            <tr>
                <td colspan="2" align="right">
                    <asp:LinkButton ID="lnkBack" Text="Back" runat="server" 
                        PostBackUrl="~/Login.aspx" Font-Bold="True"  Font-Size="Medium" />
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
     </body>

    
</html>
