<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TokanDisplay.aspx.cs" Inherits="PalashDynamics.Web.Reports.Patient.TokanDisplay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="refresh" content="15" />

    <script type="text/javascript">
        function blink() {
            var grid = document.getElementById('grdTokenPatientlist');
            if (grid != null && typeof grid != undefined) {
                var gridLength = grid.rows.length - 1;
                //grid.rows.
                for (var i = 0; i < gridLength; i++) {

                    var str = 'grdTokenPatientlist_Row_' + i;
                    var elm1 = document.getElementById(str);
                    if (elm1.style.visibility == 'visible') {
                        elm1.style.visibility = 'hidden';
                    }
                    else
                        elm1.style.visibility = 'visible';
                }
                setTimeout('blink()', 500);
            }
            blinkGridViewRows();
        }

        function blinkGridViewRows() {
            document.getElementsByTagName("html")[0].onkeyup = function () {
                var keyCode = "";
                if (window.event)
                    keyCode = event.keyCode;
                else
                    keyCode = event.which;
                if (keyCode == 27) {
                    window.close();
                }
            }
        }

    </script>
</head>
<body><%--onload="PageLoad();"--%>
     <form id="form1" runat="server">
     <div>
          <center><h1 style=" color: #0091ea; font-family: 'Helvetica Neue', sans-serif; font-size: 46px; font-weight: 100; line-height: 50px; letter-spacing: 1px; padding: 0 0 40px; border-bottom: double #01579b;"><asp:Label ID="HospitalName" runat="server" Text='<%# Eval("UnitName") %>'></asp:Label></h1>  </center>
          <center><h2 ><asp:Label ID="dateshow" runat="server" Text="Label"></asp:Label></h2></center>
     </div>
    
    <div id="container" class="container">
        <asp:GridView ID="grdTokenPatientlist" runat="server" AutoGenerateColumns="False"
            BorderColor="#0091ea" BorderStyle="Solid" BorderWidth="4px" BackColor="White" CellPadding="4"
            CellSpacing="2" ForeColor="Black" Width="100%"
            OnRowDataBound="grdTokenPatientlist_RowDataBound">
            <Columns>
             
                <asp:TemplateField HeaderText="Token NO" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BorderColor="#FFFFFF"
                    HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="2px">
                    <ItemStyle BorderColor="#0091ea" BorderStyle="Solid" BorderWidth="1px" />
                    <ItemTemplate>
                       <center> <asp:Label ID="blink" runat="server" Class="blink" Style="color: #0091ea;" Text='<%# Eval("TokenNO") %>' /></center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Patient Name" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BorderColor="#FFFFFF"
                    HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="2px">
                    <ItemStyle BorderColor="#0091ea" BorderStyle="Solid" BorderWidth="2px" />
                    <ItemTemplate>
                        <center><asp:Label ID="blink" runat="server" Class="blink" Style="color: #0091ea;" Text='<%# Eval("Patient Name") %>' /></center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Doctor Name" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BorderColor="#FFFFFF"
                    HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="2px">
                    <ItemStyle BorderColor="#0091ea" BorderStyle="Solid" BorderWidth="2px" />
                    <ItemTemplate>
                        <center><asp:Label ID="blink" runat="server" Class="blink" Style="color: #0091ea;" Text='<%# Eval("DoctorName") %>' /></center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Department Name" HeaderStyle-HorizontalAlign="Center"
                    HeaderStyle-BorderColor="Black" HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="2px">
                    <ItemStyle BorderColor="#0091ea" BorderStyle="Solid" BorderWidth="2px" />
                    <ItemTemplate>
                        <center><asp:Label ID="blink" runat="server" Class="blink" Style="color: #0091ea;" Text='<%# Eval("DepartmentName") %>' /></center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cabin Name" HeaderStyle-HorizontalAlign="Center"
                    HeaderStyle-BorderColor="Black" HeaderStyle-BorderStyle="Solid" HeaderStyle-BorderWidth="2px">
                    <ItemStyle BorderColor="#0091ea" BorderStyle="Solid" BorderWidth="2px" />
                    <ItemTemplate>
                        <center><asp:Label ID="blink" runat="server" Class="blink" Style="color: #0091ea;" Text='<%# Eval("Cabin") %>' /></center>
                    </ItemTemplate>
                </asp:TemplateField>
                
            </Columns>
            <FooterStyle BackColor="#CCCCCC" />
            <HeaderStyle BackColor="#0091ea" Font-Bold="True" ForeColor="#FFFFFF" Font-Size="23" />
            <PagerStyle BackColor="#00e5ff" ForeColor="Black" HorizontalAlign="Left" />
            <RowStyle BackColor="White" Font-Size="20" Font-Bold="true" BorderColor="#0091ea" BorderStyle="Solid" />
            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#808080" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#383838" />
        </asp:GridView>

        <asp:Label ID="lblMessage" runat="server" Text="" Font-Bold="true" Font-Size="20"></asp:Label>
    </div>
    </form>
</body>
</html>
