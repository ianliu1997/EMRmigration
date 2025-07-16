<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link rel="shortcut icon" href="http://palashivf.com/wp-content/uploads/bfi_thumb/palash-ivf-favicon-301m2s1cz7yd0mkno813ii.png" />

<script runat="server">

    
    //protected override void OnPreRender(EventArgs e)
    //{
    //    base.OnPreRender(e);
    //    string strDisAbleBackButton;
    //    strDisAbleBackButton = "";
    //    ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "clientScript", strDisAbleBackButton);
    //} 
    
    
    public long SampleVal = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        SampleVal = Convert.ToInt64(System.Configuration.ConfigurationManager.AppSettings["SampleVal"]);
        if (Session["USER"] == null)
        {
            
         Response.Redirect("Login.aspx");
        }
        else
        {            
        }
    }

    protected void Page_Unload(object sender, EventArgs e)
    {       
    }

    private void RegisterCallBackReference()
    {
        String callBack = Page.ClientScript.GetCallbackEventReference(this, "arg",
             "CallbackOnSucceeded", "context", "CallbackOnFailed", true);

        String clientFunction = "function CallServer(arg, context){ "
                       + callBack + "; }";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                "Call To Server", clientFunction, true);
    }

    [System.Web.Services.WebMethod]
    public static String UpdateAuditTrail()
    {
        long AuditTrailId = Convert.ToInt64(HttpContext.Current.Session["AuditId"]);
        PalashDynamics.ValueObjects.clsUserVO User = null;
        User = PalashDynamics.BusinessLayer.User.UpdateUserOnClose.GetInstance().UpdateUser(AuditTrailId);
        return Convert.ToString("You are successfully Logged Out!");
    }
    
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    
    <link rel="icon" href="img/favicon.png">
    <title>Palash - Fertility Clinic Information Management System</title>
    <style type="text/css">
        html, body {
	        height: 100%;
	        overflow: auto;
        }
        body {
	        padding: 0;
	        margin: 0;
        }
        #silverlightControlHost {
	        height: 100%;
	        text-align:center;
        }
    </style>
   <%--  <script type="text/javascript" language="javascript">
         javascript: window.history.forward(1);
    </script>--%>
   <%--     <script type = "text/javascript" language="javascript">
        function disableBackButton()
        {
             window.history.forward(1);
        }
        setTimeout("disableBackButton()", 0);
        </script>--%>

    <script type="text/javascript" src="Silverlight.js"></script>
    <script type="text/javascript">
        function GoBack() {
            history.go(-1);
        }

        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
              appSource = sender.getHost().Source;
            }
            
            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
              return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " +  appSource + "\n" ;

            errMsg += "Code: "+ iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {           
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " +  args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }

        function openAttachment(text) {
            window.open("EmailTemplateAttachment/" + text, 'OpenTemplate', 'resizable=yes,scrollbars=yes,toolbar=no,directories=no,status=no,menubar=no,copyhistory=no');
        }
        function alertText(text) {

            window.open("EMR/Images/" + text, 'OpenTemplate', 'resizable=yes,scrollbars=yes,toolbar=no,directories=no,status=no,menubar=no,copyhistory=no');
        }

        function OpenReport(text) {

            window.open("PatientPathTestReportDocuments/" + text, 'OpenTemplate', 'resizable=yes,scrollbars=yes,toolbar=no,directories=no,status=no,menubar=no,copyhistory=no');
        }

        function GlobalOpenFile(text) {

            window.open(text, 'OpenTemplate', 'resizable=yes,scrollbars=yes,toolbar=no,directories=no,status=no,menubar=no,copyhistory=no');
        }

        function OpenEXCELReport(text) {

            window.open(text, 'OpenTemplate', 'resizable=yes,scrollbars=yes,toolbar=no,directories=no,status=no,menubar=no,copyhistory=no');
        }
        </script>
        <script language="javascript">
        function UpdateA() {
                   
            PageMethods.UpdateAuditTrail(OnSucceeded, OnFailed);
            //alert("1");
           
          }

        // Callback function invoked on successful 
        // completion of the page method.
        function OnSucceeded(result, userContext, methodName) {
            //alert("2");
            if (methodName == 'UpdateAuditTrail'){
            //DisplayDate.innerHTML = result;
                //alert(result);
             }
        }

        // Callback function invoked on failure 
        // of the page method.
        function OnFailed(error, userContext, methodName) {
            if (error !== null) {
                alert("An error occurred: " + error.get_message());
            }
        }

    </script>
</head>
<body onbeforeunload="UpdateA();">
    <form id="form1" runat="server" style="height:100%">
        <div id="silverlightControlHost">
            <object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
		      <param name="source" value="ClientBin/PalashDynamics.xap"/>
		      <param name="onError" value="onSilverlightError" />
		      <param name="background" value="white" />
		      <param name="minRuntimeVersion" value="3.0.40624.0" />
		      <param name="autoUpgrade" value="true" />
              <param name="enableHtmlAccess" value="true" />
		      <%--<param name="initParams" value="User=<%=Session["User"]%>,cc=true,m=/relative,TimeForSession=<%=SampleVal%>,LiveTimeSession=<%=LiveTimeVal%>" />--%>
              <param name="initParams" value="User=<%=Session["User"]%>,cc=true,m=/relative,TimeForSession=<%=SampleVal%>" />
          <%-- <param name="sessionvalue" value="a,<%=SampleVal%>"/> --%>
            <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0" style="text-decoration:none">
 			      <img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight" style="border-style:none"/>
		      </a>
	        </object><iframe id="_sl_historyFrame" style="visibility:hidden;height:0px;width:0px;border:0px"></iframe>
            <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true"  runat="server"/>
	     </div>
    </form>
</body>

</html>
