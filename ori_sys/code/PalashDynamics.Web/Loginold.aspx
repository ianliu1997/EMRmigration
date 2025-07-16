<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Loginold.aspx.cs" Inherits="PalashDynamics.Web.Loginold" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <!-- Bootstrap core CSS -->
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link rel="shortcut icon" href="http://palashivf.com/wp-content/uploads/bfi_thumb/palash-ivf-favicon-301m2s1cz7yd0mkno813ii.png" />
    <link href="css/ss.css" rel="stylesheet" />
    <link href="http://netdna.bootstrapcdn.com/font-awesome/4.0.3/css/font-awesome.css"
        rel="stylesheet" />
    <!-- Just for debugging purposes. Don't actually copy these 2 lines! -->
    <!--[if lt IE 9]><script src="../../assets/js/ie8-responsive-file-warning.js"></script><![endif]-->
    <script src="../../assets/js/ie-emulation-modes-warning.js"></script>
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <script type="text/javascript">
        function setHourglass() {
            document.body.style.cursor = 'wait';
        }
        function disablebackButton() {

            window.history.forward(-1);

        }
        setTimeout("disablebackButton()", 0);
    </script>
</head>
<body onload="disablebackButton();" onbeforeunload="setHourglass();" onunload="setHourglass();"
    style="background-imag; background-image: url('img/healthcare.png');">
    <div class="container-fluid">
        <div class="row" style="padding: 10px 30px">
            <div class="col-lg-12 ">
                <img src="https://d2zklegsrclt8m.cloudfront.net/wp-content/uploads/2017/01/16105148/rsz_finalised_logo_white1.png"
                    style="width: 190px; height: 59px" class="pull-right img-responsive" alt="Palash" /></div>
        </div>
        <div class="row" >
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
            </div>
            <div class="col-lg-4 col-md-4 col-sm-8 col-xs-12 pull-right" style="padding: 100px 20px 0px 20px">
                <h2 class="text-center">
                    Login
                </h2>
                <form id="form1" runat="server" class="form-horizontal">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <div class="form-group">
                    <label for="inputEmail3" class="col-sm-4 control-label">
                        User Name
                    </label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtLoginName" Text="" MaxLength="100" BorderColor="Red" ToolTip="Please Enter your Login Name."
                            runat="server" CssClass="form-control" AutoPostBack="True" ontextchanged="txtLoginName_TextChanged"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group">
                    <label for="inputPassword3" class="col-sm-4 control-label">
                        Password</label>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" ToolTip="Please Enter your Password."
                            TextMode="Password" CssClass="form-control" placeholder="Password"></asp:TextBox>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-group">
                            <label for="inputPassword3" class="col-sm-4 control-label">
                                Clinic Name</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlUnits" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlUnits_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="CashCout" runat="server">
                            <label for="inputPassword3" class="col-sm-4 control-label">
                                Cash Counter Name</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlCashCounter" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="form-group">
                    <div class="col-sm-offset-4 col-sm-8">
                        <asp:Button ID="btn" Text="Login" runat="server" OnClick="OK_Click" CssClass="btn btn-success"
                            Style="background-color: White; color: Black; width: 150px" />
                        <asp:LinkButton ID="lnkBack" Text="Forgot Password?" runat="server" PostBackUrl="~/ForrgotPassword.aspx"
                            CssClass="btn btn-link" Style="color: Blue; visibility: hidden" />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-sm-offset-4 col-sm-8">
                        <label>
                            <%--<input type="checkbox"> Remember me--%>
                            <asp:Label ID="Error" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                        </label>
                    </div>
                </div>
                </form>
            </div>
        </div>
        <!--end the row -->
        <br>
        <br>
        <br>
        <br>
        <div class="row navbar-fixed-bottom" style="background-color: #094e79; color: #FFF;
            padding: 20px 0">
            <div class="col-lg-12">
                <div class="row">
                    <%-- <div class="col-lg-5 col-md-4 col-sm-12 col-xs-12 pull-left"><p class="small">Copyright © 2014 PALASH<sup>TM</sup> Healthcare . All Rights Reserved. </p></div>--%>
                    <div class="col-lg-5 col-md-4 col-sm-12 col-xs-12 pull-left" style="text-align: center;
                        vertical-align: middle;">
                        <p class="small">
                            Copyright © 2016 PALASH IVF Solutions Pvt Ltd. Pune, India. All Rights Reserved.
                        </p>
                    </div>
                    <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                        <p class="small text-center">
                        </p>
                    </div>
                    <%--<div class="col-lg-3 col-md-4 col-sm-12 col-xs-12"><p  class="text-center small">Design & Developed by PALASH Healthcare Systems Pvt. Ltd. Pune </p></div>--%>
                    <div class="col-lg-3 col-md-4 col-sm-12 col-xs-12">
                        <p class="text-center small">
                            &nbsp;</p>
                    </div>
                    <div class="col-lg-3 col-md-4 col-sm-12 col-xs-12 pull-right">
                        <p class="text-center small">
                            &nbsp;</p>
                    </div>
                </div>
            </div>
        </div>
        <!--end the row -->
    </div>
    <!--container-fluid -->
    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script src="js/bootstrap.min.js"></script>
    <!-- IE10 viewport hack for Surface/desktop Windows 8 bug -->
    <script src="../../assets/js/ie10-viewport-bug-workaround.js"></script>
</body>
</html>
