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

namespace PalashDynamics.Web.ReportsBI.OPD
{
    public partial class Patient_AdvanceRefund : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            myDoc = new ReportDocument();
            long ID = 0;
            long UnitID = 0;
            long PatientUnitID = 0;
            long PatientID = 0;
            long AdvanceID = 0;
            long RID = 0;
            long AdvanceUnitID = 0;

            if (Request.QueryString["ReportID"] != null)
            {
                RID = Convert.ToInt64(Request.QueryString["ReportID"]);
            }

            if (RID == Convert.ToInt64(1))
            {

                if (Request.QueryString["PrintFomatID"] == "1")
                {
                    myDoc.Load(Server.MapPath("rpt_PatientAdvanceReceipt.rpt"));
                }
                else if (Request.QueryString["PrintFomatID"] == "2")
                {
                    myDoc.Load(Server.MapPath("rpt_PatientAdvanceReceiptWithoutHeader.rpt"));
                }
                else
                {
                    myDoc.Load(Server.MapPath("rpt_PatientAdvanceReceipt.rpt"));
                }
                if (Request.QueryString["PatientID"] != null)
                {
                    PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
                }
                if (Request.QueryString["PatientUnitID"] != null)
                {
                    PatientUnitID = Convert.ToInt64(Request.QueryString["PatientUnitID"]);
                }
                if (Request.QueryString["AdvanceID"] != null)
                {
                    AdvanceID = Convert.ToInt64(Request.QueryString["AdvanceID"]);
                }
                if (Request.QueryString["AdvanceUnitID"] != null)
                {
                    AdvanceUnitID = Convert.ToInt64(Request.QueryString["AdvanceUnitID"]);
                }
                myDoc.SetParameterValue("@PatientID", PatientID);
                myDoc.SetParameterValue("@PatientUnitID", PatientUnitID);
                myDoc.SetParameterValue("@AdvanceID", AdvanceID);
                myDoc.SetParameterValue("@AdvanceUnitID", AdvanceUnitID);
                myDoc.SetParameterValue("@AdvanceID", AdvanceID, "rptPatientAdvanceTransactionSummary");
                myDoc.SetParameterValue("@AdvanceUnitID", AdvanceUnitID, "rptPatientAdvanceTransactionSummary");
                myDoc.SetParameterValue("@UnitID", AdvanceUnitID, "rptCommonReportHeader.rpt");

                myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 20/9/2016 Get Unit LOGO
                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);
                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();
            }

            if (RID == Convert.ToInt64(2))
            {
                if (Request.QueryString["PrintFomatID"] == "1")
                {
                    myDoc.Load(Server.MapPath("rpt_PatientAdvanceRefund.rpt"));
                }
                else if (Request.QueryString["PrintFomatID"] == "2")
                {
                    myDoc.Load(Server.MapPath("rpt_PatientAdvanceRefundWithoutHeader.rpt"));
                }
                else
                {
                    myDoc.Load(Server.MapPath("rpt_PatientAdvanceRefund.rpt"));
                }

                if (Request.QueryString["ID"] != null)
                {
                    ID = Convert.ToInt64(Request.QueryString["ID"]);
                }
                if (Request.QueryString["UnitID"] != null)
                {
                    UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
                }

                myDoc.SetParameterValue("@ID", ID);
                myDoc.SetParameterValue("@UnitID", UnitID);

                myDoc.SetParameterValue("@ID", ID, "rptPatietRefundTransactionSummary");
                myDoc.SetParameterValue("@UnitID", UnitID, "rptPatietRefundTransactionSummary");

                myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");

                myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt"); //Added by ajit jadhav date 20/9/2016 Get Unit LOGO
                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);
                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();

            }
            //MemoryStream oStream = new MemoryStream();
            //oStream = (MemoryStream)myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.BinaryWrite(oStream.ToArray());
            //Response.End();


            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();
            Stream oStream = null;
            byte[] byteArray = null;
            //   oStream = myDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            oStream = myDoc.ExportToStream(ExportFormatType.PortableDocFormat);
            byteArray = new byte[oStream.Length];
            oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";
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