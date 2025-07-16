using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;

namespace PalashDynamics.Web.Reports.InventoryPharmacy
{
    public partial class MaterialConsumptionReport : System.Web.UI.Page
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

            //Added by AJ Date 20/1/2017
          long PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
          long PatientUnitID = Convert.ToInt64(Request.QueryString["PatientUnitID"]);
          long AdmID = Convert.ToInt64(Request.QueryString["AdmID"]);         
          long AdmissionUnitID = Convert.ToInt64(Request.QueryString["AdmissionUnitID"]);
          long ISPatientAgainst = Convert.ToInt64(Request.QueryString["ISPatientAgainst"]);
            //***//--------

          if (ISPatientAgainst == 1)
          {
              myDoc.Load(Server.MapPath("PatientAgainstMaterialConsumption.rpt"));
              myDoc.SetParameterValue("@PatientID", PatientID);
              myDoc.SetParameterValue("@PatientUnitID", PatientUnitID);
              myDoc.SetParameterValue("@AdmID", AdmID);
              myDoc.SetParameterValue("@AdmissionUnitID", AdmissionUnitID);
             // myDoc.SetParameterValue("@UnitId", AdmissionUnitID, "sbrptMaterialConsumption");
              myDoc.SetParameterValue("@UnitID", AdmissionUnitID, "rptCommonReportHeader.rpt");
              myDoc.SetParameterValue("@UnitId", AdmissionUnitID, "rptUnitLogo.rpt"); 
          }
          else
          {
              myDoc.Load(Server.MapPath("MaterialConsumption.rpt"));
              myDoc.SetParameterValue("@ConsumptionId", str);
              myDoc.SetParameterValue("@UnitId", str1);
              myDoc.SetParameterValue("@UnitId", str1, "sbrptMaterialConsumption");
              myDoc.SetParameterValue("@UnitID", str1, "rptUnitLogo.rpt"); //Added by ajit jadhav date 28/11/2016 Get Unit LOGO
              //myDoc.SetParameterValue("@PatientId", PatientId, "SubRptPatientDetails");
          }
         
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            //MemoryStream oStream = new MemoryStream();
            //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);

            Stream oStream = null;
            byte[] byteArray = null;

            oStream = myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            byteArray = new byte[oStream.Length];
            oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
            //Response.BinaryWrite(oStream.ToArray());
            Response.BinaryWrite(byteArray);
            Response.End();

        }
    }
}