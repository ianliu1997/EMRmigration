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

namespace PalashDynamics.Web.Reports.Inventory_New
{
    public partial class StoreIndentPrint : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();
            string str=Request.QueryString["IndentId"];
            string str1 = Request.QueryString["UnitID"];

            if (Request.QueryString.Count > 2)
            {
                myDoc.Load(Server.MapPath("rptPurchaseRequisition.rpt"));
            }
            else
            {
                myDoc.Load(Server.MapPath("rptPurchaseRequisition.rpt"));
            }
            myDoc.SetParameterValue("@LinkServerDBName", null);
            myDoc.SetParameterValue("@LinkServerAlias", null);
            myDoc.SetParameterValue("@LinkServer", null);
            myDoc.SetParameterValue("@IndentId", str);
            myDoc.SetParameterValue("@UnitId", str1);

            myDoc.SetParameterValue("@UnitId", str1,"rptCommonReportHeader.rpt");

            //myDoc.SetParameterValue("@PatientId", PatientId, "SubRptPatientDetails");


            myDoc.SetParameterValue("@UnitID", str1, "rptUnitLogo.rpt"); //Added by ajit jadhav date 30/9/2016 Get Unit LOGO

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);

            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

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
        protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
        {
            try
            {
                if (myDoc != null)
                    myDoc.Dispose();
                CrystalReportViewer1.Dispose();
                GC.Collect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}