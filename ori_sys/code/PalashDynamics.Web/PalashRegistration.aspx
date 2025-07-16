<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PalashRegistration.aspx.cs" Inherits="PalashDynamics.Web.PalashRegistration" %>

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
         <div  style="background:url(CIMSBgTheme.jpg) no-repeat top; left:0px; right:0px; top:50px; height:700px; width:950px; position:relative;">
            <div style="left:5px; top:5px;  right:1px; position: absolute; text-align:left;">
            
               
                <table class="style1" cellpadding="2" width="100%" >
                <tr>
                    <td colspan="3" > 
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblLicense" runat="server" 
                            Text="This copy is Licensed To " Font-Bold="True" Font-Size="Larger" 
                            Font-Underline="False" ForeColor="Green" Font-Names="Times New Roman"></asp:Label>
                    </td>
                </tr>
               <%-- <tr>
                    <td colspan="3"> 
                     &nbsp;
                    
                    </td>
                </tr>--%>
                <tr>
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
                   

                     <td class="style3" align="right" width="25%">
                       <asp:Label ID="Label1"  runat="server" Text="Clinic Name" Font-Bold="True"  Font-Names="Times New Roman"></asp:Label>
                     </td>
                            <td align="left" width="30%">
                                     <asp:DropDownList ID="ddlUnits" runat="server"   BorderColor="Red" AutoPostBack="true"
                                        onselectedindexchanged="ddlUnits_SelectedIndexChanged"  Width="100%" Tooltip="Select Clinic">
                                    </asp:DropDownList>
                              
                            </td>
                            <td class="style3" align="right" width="15%">
                                <asp:Label ID="lblHOName" runat="server" Text="HO Name" Font-Names="Times New Roman" Visible="false"></asp:Label>
                            </td>
                            <td align="left">
                                
                             <asp:TextBox ID="txtHOName" runat="server" 
                                ToolTip="Enter Head Office Name"  Font-Names="Times New Roman" MaxLength="50" width="95%" Visible="false" BorderColor="Red"></asp:TextBox>
                            </td>
                
                </tr>

                <tr>
                    <td  align="right" width="25%">
                        <asp:Label ID="lblClinicDetails" Visible="False" runat="server" Font-Bold="True" 
                            Font-Names="Times New Roman" Text = "Clinic Details : "></asp:Label>
                    </td>


                </tr>
                <tr>
                  
                    
                         <td align="right"  width="25%">
                              <asp:Label ID="Label2" runat="server" Text="Contact No."  Font-Names="Times New Roman"></asp:Label>
                        </td>
                        <td align="left" width="30%" >
                        
                                <asp:TextBox ID="txtCountryCode" runat="server" 
                                ToolTip="Enter Country Code "  Font-Names="Times New Roman" MaxLength="15" width="15%" ></asp:TextBox>
                                 <asp:TextBox ID="txtSTDCode" runat="server" 
                                ToolTip="Enter STD Code"  Font-Names="Times New Roman" MaxLength="15" width="15%" ></asp:TextBox>
                             <asp:TextBox ID="txtContactNo" runat="server" 
                                ToolTip="Enter Clinic Contact No."  Font-Names="Times New Roman" MaxLength="15" width="40%" ></asp:TextBox>
                        </td>

                        <td align="right" width="5%">
                            <asp:Label ID="lblClinicFax" runat="server" Text="Fax No."  Font-Names="Times New Roman"></asp:Label>
                        </td>
                         <td align="left"  >
                            <asp:TextBox ID="txtClinicFaxNo" runat="server" 
                                ToolTip="Enter Clinic Fax No."  Font-Names="Times New Roman" MaxLength="15" width="90%" ></asp:TextBox>
                        </td>
                     
                </tr>
               <tr >
                <td class="style3" align="right" >
                     <asp:Label ID="lblClinicEmail" runat="server" Text="Email"  Font-Names="Times New Roman"></asp:Label>
                </td>
                <td width="30%">
                                <asp:TextBox ID="txtClinicEmailId" runat="server" width="100%"
                                ToolTip="Enter Clinic Email Id"  Font-Names="Times New Roman"  
                                MaxLength="50" BorderColor="Red"></asp:TextBox>
                </td>
            </tr>

            <tr>
                <td class="style3" align="right">
                    <asp:Label ID="lblAddress1" runat="server" Text="Address Line 1"  Font-Names="Times New Roman"></asp:Label>
                </td>
                <td width="30%">
                                 <asp:TextBox ID="txtAdress1" runat="server" 
                                    ToolTip="Enter Clinic Address"  Font-Names="Times New Roman"  width="100%"></asp:TextBox>
                </td>
            </tr>


            <tr>
                <td class="style3" align="right">
                    <asp:Label ID="lblAddressLine2" runat="server" Text="Line 2"  Font-Names="Times New Roman"></asp:Label>
                </td>
               
                 <td align="left" width="30%">
                                <asp:TextBox ID="txtAddressLine2" runat="server" ToolTip="Enter Clinic Address" Font-Names="Times New Roman"  width="100%"></asp:TextBox>
                               
                </td>
            </tr>
            <tr>
                <td class="style3" align="right">
                    <asp:Label ID="lblAddressLine3" runat="server" Text="Line 3" Font-Names="Times New Roman"></asp:Label>
                </td>
                 <td align="left" width="30%">
                     <asp:TextBox ID="txtAddressLine3" runat="server" ToolTip="Enter Clinic Address" Font-Names="Times New Roman" Width="100%"></asp:TextBox>
                 </td>
            </tr>
          
            <tr>
               
                <td class="style3" align="right" width="25%">
                    <asp:Label ID="lblCountry" runat="server" Text="Country" Font-Names="Times New Roman"></asp:Label>
                </td>
                            <td align="left" width="30%">
                                <asp:TextBox ID="txtCountry" runat="server" 
                                    ToolTip="Enter Country Name" Font-Names="Times New Roman" Width="100%"></asp:TextBox>
                            </td>
                            <td class="style3" align="right" width="15%">
                                <asp:Label ID="lblState" runat="server" Text="State" Font-Names="Times New Roman"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtState" runat="server" 
                                    ToolTip="Enter State Name" Font-Names="Times New Roman" Width="95%"></asp:TextBox>
                            </td>
         </tr>
                        <tr>
                            <td class="style3" align="right" width="25%">
                                <asp:Label ID="lblDistrict" runat="server" Text="District" Font-Names="Times New Roman"></asp:Label>
                            </td>
                            <td align="left" width="30%">
                                <asp:TextBox ID="txtDistrict" runat="server" 
                                    ToolTip="Enter District Name" Font-Names="Times New Roman" Width="100%"></asp:TextBox>
                            </td>
                            <td class="style3" align="right" width="15%">
                                <asp:Label ID="lblTaluka" runat="server" Text="Taluka" Font-Names="Times New Roman"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtTaluka" runat="server" 
                                    ToolTip="Enter Taluka Name" Font-Names="Times New Roman" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3" align="right" width="25%">
                                <asp:Label ID="lblCity" runat="server" Text="City" Font-Names="Times New Roman"></asp:Label>
                            </td>
                            <td align="left" width="30%">
                                <asp:TextBox ID="txtCity" runat="server" 
                                    ToolTip="Enter City Name" Font-Names="Times New Roman" Width="100%"></asp:TextBox>
                            </td>
                            <td class="style3" align="right" width="15%">
                                <asp:Label ID="lblArea" runat="server" Text="Area" Font-Names="Times New Roman"></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtArea" runat="server" BorderColor="Red" 
                                    ToolTip="Enter Area Name" Font-Names="Times New Roman" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                      
                  

            <tr>
                <td class="style3" align="right" width="25%">
                    <asp:Label ID="lblPinCode" runat="server" Text="Pin Code" Font-Names="Times New Roman"></asp:Label>
                </td>
                <td align="left" width="30%">
                    <asp:TextBox ID="txtPinCode" runat="server" BorderColor="Red" 
                        ToolTip="Enter Pin Code" Font-Names="Times New Roman" Width="100%" MaxLength="6"></asp:TextBox>
                </td>

                  <td class="style3" width="15%">
                    &nbsp;</td>
                    <td align="right">
                     <asp:Button ID="btnSave" runat="server"   Text="Save" onclick="btnSave_Click" 
                        CssClass="frmButton"  Font-Bold="False" Font-Size="Small" ForeColor="#0033CC" Height="20px" Width="75%"/>
             
                    </td>
            </tr>
         
         
            <tr>
                <td width="25%" align="right">
                   <%-- &nbsp;--%>
                    <asp:Label ID="lblActivationKey" runat="server" Font-Bold="True" 
                        Text="Your Activation Key :" Visible="False"  
                        Font-Names="Times New Roman" ForeColor="Red"></asp:Label>
                </td>
                <td width="30%">
                      <asp:Label ID="lblKey" runat="server" Text=" "  Forecolor="Blue"
                        Font-Names="Times New Roman"></asp:Label>
                </td>
                <td width="15%" align="right">
                    &nbsp;
                </td>
                <td align="right">
                 <asp:Button ID="btnActivation" runat="server" Text="Activate" Visible="false" 
                        onclick="btnActivation_Click" CssClass="frmButton" Width="75%" Font-Bold="False" Font-Size="Small" ForeColor="#0033CC" Height="20px"/>
              
                </td>
            </tr>
           <tr >
           <td>
           &nbsp;
           </td>
           <td colspan="3" > 
                        <asp:Label ID="lblKeyNote" runat="server" 
                            Text="IMP Note : Copy/Save/Notedown generated key,this key will required while Acivation." Font-Bold="True" Font-Size="Smaller"
                            Font-Underline="False" ForeColor="Blue" Font-Names="Times New Roman" Visible="false"></asp:Label>
                    </td>
           </tr>
          
         
        </table>
         
            </div>
         </div>
         </center>
    </form>
</body>
</html>
