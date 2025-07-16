<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PalashActivation.aspx.cs" Inherits="PalashDynamics.Web.PalashActivation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style3
        {
          
        }
        .style4
        {
          
        }
        .frmButton
{
    width:80px;
     font-size:100%;
	 font-weight:bold;	
     vertical-align:middle;
	 
	 background-color:#FEFEFE;
	 border:1px solid #A2ADB8;
	 text-decoration: none;
	 cursor:pointer;
	 letter-spacing:1px;
	 width:60px;
     }
     
    </style>
</head>
<body>
    <form id="form1" runat="server">

     <center style="height:100%; width:100%" >
         <div  style="background:url(CIMSBgTheme.jpg) no-repeat; left:0px; right:0px; top:50px; height:250px; width:800px; position:relative;">
            <div style="left:5px; top:5px;  right:1px; position: absolute; text-align:left;">
            
               
                <table class="style1" cellpadding="2" width="100%" >
                <tr  >
                    <%--<td colspan="3"> 
                        <asp:Label ID="lblLicense" runat="server" 
                            Text="This copy of PALASH is Licensed To " Font-Bold="True" Font-Size="Larger" 
                            Font-Underline="False" ForeColor="Green" Font-Names="Times New Roman"></asp:Label>
                    </td>--%>
                     <td colspan="3" > 
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblLicense" runat="server" 
                            Text="This copy is Licensed To " Font-Bold="True" Font-Size="Larger" 
                            Font-Underline="False" ForeColor="Green" Font-Names="Times New Roman"></asp:Label>
                    </td>
                </tr>
             
                <tr >
                    <%--<td colspan="3">
                    <asp:Label ID="lblError" Visible="False" runat="server" Font-Bold="True" 
                        ForeColor="Red" Font-Names="Times New Roman" Text=" "> </asp:Label>
                    </td>--%>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="3">
                    <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                    <asp:Label ID="lblError" Visible="False" runat="server" Font-Bold="True" 
                        ForeColor="Red" Font-Names="Times New Roman"></asp:Label>
                    </td>
                </tr>

                             <tr>
                    <td colspan="3"> 
                     &nbsp;
                    
                    </td>
                </tr>
                <tr >
                    <td width="25%" align="right">
                          <asp:Label ID="Label2" runat="server" Text="Enter Activation Key : "></asp:Label>
                    </td>
                    <td colspan="2" >
                          <%--<asp:TextBox ID="txtDescription" runat="server" BorderColor="Red" 
                                    ToolTip="Enter Clinic Name"  Font-Names="Times New Roman" width="75%" ></asp:TextBox>--%>
                                 <asp:TextBox ID="txtKey1" runat="server" Width="68px" MaxLength="4"></asp:TextBox>
                                    &nbsp;<asp:TextBox ID="txtKey2" runat="server" Width="68px" MaxLength="4"></asp:TextBox>
                                    &nbsp;<asp:TextBox ID="txtKey3" runat="server" Width="68px" MaxLength="4"></asp:TextBox>
                                    &nbsp;<asp:TextBox ID="TxtKey4" runat="server" Width="68px" MaxLength="4"></asp:TextBox>
                    </td>
                   
                
                </tr>

                
                      <tr>
                    <td colspan="3"> 
                     &nbsp;
                    
                    </td>
                </tr>

            <tr>
                <td class="style3" align="right" width="25%">
                   &nbsp;
                </td>
                <td colspan="1" align="right">
                      <asp:Button ID="btnOk" runat="server"   Text="OK" onclick="btnOk_Click" 
                        CssClass="frmButton"  Font-Bold="False" Font-Size="Small" ForeColor="#0033CC" Height="20px" Width="25%"/>
                         &nbsp;
                          <%--<asp:Button ID="btnClose" runat="server"   Text="Close" onclick="btnClose_Click" 
                        CssClass="frmButton"  Font-Bold="False" Font-Size="Small" ForeColor="#0033CC" Height="20px" Width="25%"/>--%>
                       
                </td>

                  
            </tr>
         
                     

                       <tr>
                             <td  colspan="2">
                                <asp:Label ID="lblSucess" runat="server" 
                                    Text="Your activation process is completed sucessfully. Click here to " Font-Bold="True" Font-Size="Medium" 
                                    Font-Underline="False" ForeColor="Green" Font-Names="Times New Roman" Visible="false"></asp:Label>
                                    &nbsp;
                                     <asp:LinkButton ID="lnkLogin" Text="Login ." runat="server" 
                                    PostBackUrl="~/Login.aspx" Font-Bold="True"  Font-Size="Medium" Visible="false"/>
                            </td>
                       

                     
                      </tr>
                      
   
           
          
         
        </table>
         
            </div>
         </div>
         </center>
    <div>
    
      
    
    </div>
    </form>
</body>
</html>
