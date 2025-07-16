using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.IO;


namespace PalashDynamics.Web.Reports.IVFDashboard
{
    public partial class IVFDashboard_SpermThawing : System.Web.UI.Page
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
            long PatientID = 0;
            long PatientUnitID = 0;
            string CollectionNO = "";
            long PrintFomatID = 0;

            if (Request.QueryString["ID"] != null)
            {
                ID = Convert.ToInt64(Request.QueryString["ID"]);
            }
            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            if (Request.QueryString["MalePatientID"] != null)
            {
                PatientID = Convert.ToInt64(Request.QueryString["MalePatientID"]);
            }
            if (Request.QueryString["CollectionNO"] != null)
            {
                CollectionNO = Convert.ToString(Request.QueryString["CollectionNO"]);
            }
            if (Request.QueryString["MalePatientUnitID"] != null)
            {
                PatientUnitID = Convert.ToInt64(Request.QueryString["MalePatientUnitID"]);
            }
            if (Request.QueryString["PrintFomatID"] != null)
            {
                PrintFomatID = Convert.ToInt64(Request.QueryString["PrintFomatID"]);
            }

            myDoc.Load(Server.MapPath("Rpt_IVFDashboard_SpermThawing_New.rpt"));

            myDoc.SetParameterValue("@ID", 0);
            myDoc.SetParameterValue("@UnitID", UnitID);
            myDoc.SetParameterValue("@MalePatientID", PatientID);
            myDoc.SetParameterValue("@MalePatientUnitID", PatientUnitID);
            myDoc.SetParameterValue("@CollectionNO", CollectionNO);

            myDoc.SetParameterValue("@ID", 0,"rpt_ThawingDetails");
            myDoc.SetParameterValue("@UnitID", UnitID,"rpt_ThawingDetails");
            myDoc.SetParameterValue("@MalePatientID", PatientID,"rpt_ThawingDetails");
            myDoc.SetParameterValue("@MalePatientUnitID", PatientUnitID,"rpt_ThawingDetails");
            myDoc.SetParameterValue("@CollectionNO", CollectionNO, "rpt_ThawingDetails");

            myDoc.SetParameterValue("@UnitId", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt");

            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();

            //Stream oStream = null;
            //byte[] byteArray = null;
            //oStream = myDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            //byteArray = new byte[oStream.Length];
            //oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));

            //Response.Clear();
            //Response.Buffer = true;
            //Response.ContentType = "application/pdf";
            //Response.BinaryWrite(byteArray);
            //Response.End();

            
            
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