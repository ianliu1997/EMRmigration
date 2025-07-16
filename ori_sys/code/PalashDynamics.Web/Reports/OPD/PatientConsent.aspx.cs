using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.Web.Reports.OPD
{
    public partial class PatientConsent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            //String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            //String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            //String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            long ID = 0;
            long UnitID = 0;

            //ReportDocument myDoc = new ReportDocument();
            //myDoc.Load(Server.MapPath("PatientConsentReport.rpt"));

            if (Request.QueryString["ID"] != null)
            {
                ID = Convert.ToInt64(Request.QueryString["ID"]);
            }

            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }


            //myDoc.SetParameterValue("@ID", ID);
            //myDoc.SetParameterValue("@UnitID", UnitID);
          

            //myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            //CReportAuthentication.Impersonate(myDoc);

           // CrystalReportViewer1.ReportSource = myDoc;

           // CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

            //CrystalReportViewer1.DataBind();
            clsPatientConsentVO objVO = new  clsPatientConsentVO();

            clsGetPrintedPatientConscentBizActionVO objConscent = new clsGetPrintedPatientConscentBizActionVO();
           // objAddLicense.UnitDetails = objUnit;
            objConscent.ConsentDetails = new clsPatientConsentVO();
            objConscent.ConsentDetails.ID = ID;
            objConscent.ConsentDetails.UnitID = UnitID;

            PalashDynamicsWeb service = new PalashDynamicsWeb();
            objConscent = (clsGetPrintedPatientConscentBizActionVO)service.Process(objConscent, new clsUserVO());

            if (objConscent != null && objConscent.ConsentDetails != null && objConscent.ConsentDetails.Template != null)
                ConscentHtml.InnerHtml = objConscent.ConsentDetails.Template; // @"<br /><p><span class=""Normal"">Patient Name :&nbsp;&nbsp;{Patient Name}</span></p> <p><span class=""Normal"">MR NO :&nbsp;&nbsp;{MR NO} </span></p> <p style=""text-align:Center;""><span style=""color:#FF6600;font-weight:bold;text-align:Center;"">HTML Center Alligned Part</span></p> <br /><p><span class=""Normal"">First Left Aligned Line</span></p> <br /><br /><p style=""text-align:Right;margin:0px;""><span style=""text-align:Right;"">Right Aligned Text</span></p>";
        }
    }
}