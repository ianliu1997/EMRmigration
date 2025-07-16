<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PalashDynamics.Web.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="icon" href="img/favicon.png">
    <title>Palash - Fertility Clinic Information Management System</title>
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="css/ie10-viewport-bug-workaround.css" rel="stylesheet">
    <link href="css/signin.css" rel="stylesheet">
    <!--[if lt IE 9]><script src="js/ie8-responsive-file-warning.js"></script><![endif]-->
    <script src="js/ie-emulation-modes-warning.js"></script>
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
      <![endif]-->
</head>
<body>
    <div class="main_container">
        <div class="container">
            <form class="form-signin" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div class="logoClient">
                <img src="img/logo_manipal.png" alt="Manipal Fertility" /></div>
            <div class="contentForm">
                <h2 class="form-heading">
                    User Login</h2>
                <div class="formElement">
                    <label for="inputUser" class="">
                        Username</label>
                    <asp:TextBox ID="txtLoginName" Text="" MaxLength="100" BorderColor="Red" ToolTip="Please Enter your Login Name."
                        runat="server" CssClass="form-control" AutoPostBack="True" OnTextChanged="txtLoginName_TextChanged"
                        placeholder="user@userexample.com"></asp:TextBox>
                    <%--<input type="text" id="inputUser" class="form-control" placeholder="user@userexample.com" required>--%>
                    <div class="ico logUser">
                    </div>
                </div>
                <div class="formElement">
                    <label for="inputPassword" class="">
                        Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" ToolTip="Please Enter your Password."
                        TextMode="Password" CssClass="form-control" placeholder="Password"></asp:TextBox>
                    <%-- <input type="password" id="inputPassword" class="form-control" placeholder="*********" required>--%>
                    <div class="ico logPass">
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="formElement">
                            <label for="selectClinic" class="">
                                Clinic Name</label>
                            <asp:DropDownList ID="ddlUnits" runat="server" CssClass="form-control" AutoPostBack="true">
                            </asp:DropDownList>
                            <%--  <select class="form-control" id="selectClinic">
                        <option>Delhi - Milann Fertility Center</option>
                        <option>DKR Healthcare Pvt. Ltd.</option>
                        <option>Milann - Chandigarh</option>
                        <option>Milann - Jayanagar</option>
                        <option>Milann - Kumarapark</option>
                        <option>Milann - M S R</option>
                     </select>--%>
                        </div>
                        <div class="formElement" id="CashCout" runat="server">
                            <label for="inputPassword3" class="col-sm-4 control-label">
                                Cash Counter Name</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlCashCounter" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlUnits_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
           <%--     <button class="btn btn-lg btn-primary btn-block">
                    Login</button>--%>
                <asp:Button ID="btn" Text="Login" onclick="OK_Click" runat="server" class="btn btn-lg btn-primary btn-block"/>
                <div class="formElement">
                    <div class="col-sm-offset-4 col-sm-8"  >                       
                        <asp:LinkButton ID="lnkBack" Text="Forgot Password?" runat="server" PostBackUrl="~/ForrgotPassword.aspx"
                            CssClass="btn btn-link" Style="color: Blue; visibility: hidden" />
                    </div>
                </div>
            </div>
            <div class="formElement">
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
    <div class="copyrights">
        <img src="img/logo_PIVF.png" class="logo_PIVF" alt="PALASH IVF Solutions Pvt. Ltd.">
        Copyright &copy 2016, Design & Developed by PALASH IVF Solutions Pvt. Ltd.
    </div>
    <script src="js/ie10-viewport-bug-workaround.js"></script>
</body>
</html>
