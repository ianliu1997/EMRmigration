using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.IO;
using CrystalDecisions.Shared;
//Added by AJ Date 6/3/2017
namespace PalashDynamics.Web.Reports.InventoryPharmacy
{
    public partial class PackageConsumption : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            ReportDocument myDoc = new ReportDocument();
            long str = Convert.ToInt64(Request.QueryString["ConsumptionId"]);
            long str1 = Convert.ToInt64(Request.QueryString["UnitID"]);

           
            long PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
            long PatientUnitID = Convert.ToInt64(Request.QueryString["PatientUnitID"]);
            long AdmID = Convert.ToInt64(Request.QueryString["AdmID"]);
            long AdmissionUnitID = Convert.ToInt64(Request.QueryString["AdmissionUnitID"]);
            long ISPatientAgainst = Convert.ToInt64(Request.QueryString["ISPatientAgainst"]);

            myDoc.Load(Server.MapPath("PatientAgainstPackageConsumption.rpt"));
                myDoc.SetParameterValue("@PatientID", PatientID);
                myDoc.SetParameterValue("@PatientUnitID", PatientUnitID);
                myDoc.SetParameterValue("@AdmID", AdmID);
                myDoc.SetParameterValue("@AdmissionUnitID", AdmissionUnitID);            
                myDoc.SetParameterValue("@UnitID", AdmissionUnitID, "rptCommonReportHeader.rpt");
                myDoc.SetParameterValue("@UnitId", AdmissionUnitID, "rptUnitLogo.rpt");
            

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();
            MemoryStream oStream = new MemoryStream();
            oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(oStream.ToArray());
            Response.End();

        }
    }
}