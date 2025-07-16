<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Reports/Dashboard/Dashboard.Master"
    CodeBehind="ReferralReport.aspx.cs" Inherits="PalashDynamics.Web.Reports.Dashboard.ReferralReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    

     <!-- Metis Theme stylesheet -->
   <%-- <link rel="stylesheet" href="assets/css/theme.css">
    <link rel="stylesheet" href="assets/lib/jquery.uniform/themes/default/css/uniform.default.css">
    <link rel="stylesheet" href="assets/lib/inputlimiter/jquery.inputlimiter.css">
    <link rel="stylesheet" href="assets/lib/chosen/chosen.min.css">
    <link rel="stylesheet" href="assets/lib/colorpicker/css/colorpicker.css">
    <link rel="stylesheet" href="assets/css/colorpicker_hack.css">
    <link rel="stylesheet" href="assets/lib/tagsinput/jquery.tagsinput.css">
    <link rel="stylesheet" href="assets/lib/daterangepicker/daterangepicker-bs3.css">
    <link rel="stylesheet" href="assets/lib/datepicker/css/datepicker.css">
    <link rel="stylesheet" href="assets/lib/timepicker/css/bootstrap-timepicker.min.css">
    <link rel="stylesheet" href="assets/lib/switch/css/bootstrap3/bootstrap-switch.min.css">
    <link rel="stylesheet" href="assets/lib/jasny-bootstrap/css/jasny-bootstrap.min.css">--%>

    <%--<script type="text/javascript" src="assets/lib/screenfull/screenfull.js"></script>
    <script type="text/javascript" src="assets/lib/jquery.uniform/jquery.uniform.min.js"></script>
    <script type="text/javascript" src="assets/lib/inputlimiter/jquery.inputlimiter.js"></script>
    <script type="text/javascript" src="assets/lib/chosen/chosen.jquery.min.js"></script>
    <script type="text/javascript"src="assets/lib/colorpicker/js/bootstrap-colorpicker.js"></script>
    <script type="text/javascript" src="assets/lib/tagsinput/jquery.tagsinput.js"></script>
    <script type="text/javascript" src="assets/lib/validVal/js/jquery.validVal.min.js"></script>
    <script type="text/javascript" src="assets/lib/moment/moment.min.js"></script>
    <script type="text/javascript" src="assets/lib/daterangepicker/daterangepicker.js"></script>
    <script type="text/javascript" src="assets/lib/datepicker/js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="assets/lib/timepicker/js/bootstrap-timepicker.min.js"></script>
    <script type="text/javascript" src="assets/lib/switch/js/bootstrap-switch.min.js"></script>
    <script type="text/javascript" src="assets/lib/autosize/jquery.autosize.min.js"></script>
    <script type="text/javascript" src="assets/lib/jasny-bootstrap/js/jasny-bootstrap.min.js"></script>--%>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form class="form-horizontal" id="frmRefReport" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="text-center">
        <div class="col-lg-12">
            <div class="box inverse">
                <header>
                    <div class="icons">
                      <i class="fa fa-th-large"></i>
                    </div>
                    <h5>Selects</h5>

                    <!-- .toolbar -->
                    <div class="toolbar">
                      <nav style="padding: 8px;">
                        <a href="javascript:;" class="btn btn-default btn-xs collapse-box">
                          <i class="fa fa-minus"></i>
                        </a> 
                        <a href="javascript:;" class="btn btn-default btn-xs full-box">
                          <i class="fa fa-expand"></i>
                        </a> 
                        <a href="javascript:;" class="btn btn-danger btn-xs close-box">
                          <i class="fa fa-times"></i>
                        </a> 
                      </nav>
                    </div><!-- /.toolbar -->
                  </header>
                <div id="div-2" class="body">
                   <%-- <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>--%>
                            <div class="form-group">
                                <label class="control-label col-lg-4" for="reportrange">Pre-defined Ranges & Callback</label>
                                <div class="col-lg-4">
                                  <div class="input-group">
                                    <span class="input-group-addon"><i class="fa fa-calendar"></i></span> 
                                        <input type="text" class="form-control" id="reportrange"/>
                                  </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-lg-2">
                                    Clinic Name</label>
                                <div class="col-lg-6">
                                    <asp:DropDownList ID="ddlUnits" runat="server" CssClass="form-control" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlUnits_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <%--<div class="form-group" id="CashCout" runat="server">
                            <label for="inputPassword3" class="col-sm-4 control-label">
                                Cash Counter Name</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlCashCounter" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>--%>
                       <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                   <%-- <div class="form-group">
                        <div class="col-sm-offset-4 col-sm-8">
                            <asp:Button ID="btn" Text="OK" runat="server" CssClass="btn btn-success" Style="background-color: White;
                                color: Black; width: 150px" OnClick="btn_Click" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-offset-4 col-sm-8">
                            <label>
                                <%--<input type="checkbox"> Remember me
                                <asp:Label ID="Error" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                            </label>
                        </div>
                    </div>--%>
                      <asp:GridView ID="grdView" runat="server" AutoGenerateColumns="False" 
                            AllowPaging="True" PageSize="20" CssClass="table table-bordered table-condensed table-hover table-striped">
                            <Columns >
                                <asp:BoundField DataField="SRNO" HeaderText="Sr.no" />
                                <asp:BoundField DataField="UnitName" HeaderText="Unit Name" />
                                <asp:BoundField DataField="Date" HeaderText="Date" />
                                <asp:BoundField DataField="BDName" HeaderText="BD Name" />
                                <asp:BoundField DataField="DocName" HeaderText="Doctor Name" />
                               <%-- <asp:BoundField DataField="InternalDoctorRefCount" HeaderText="Internal Doctor Referral" />
                                <asp:BoundField DataField="ExternalDoctorRefCount" HeaderText="External Doctor Referral" />--%>
                                <asp:BoundField DataField="RefPaitentCount" HeaderText="Referral Paitent Count" />
                            </Columns>
                       </asp:GridView>  
                </div>
            </div>
        </div>
        </div>
    </form>
</asp:Content>
