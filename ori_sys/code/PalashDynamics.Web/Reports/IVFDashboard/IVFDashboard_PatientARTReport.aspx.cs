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
    public partial class IVFDashboard_PatientARTReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            myDoc = new ReportDocument();

            //if (Request.QueryString["PrintFomatID"] == "1")
            //{
            //    myDoc.Load(Server.MapPath("Rpt_IVFDashboard_ARTReport.rpt"));
            //}
            //else if (Request.QueryString["PrintFomatID"] == "2")
            //{
            //    myDoc.Load(Server.MapPath("Rpt_IVFDashboard_ARTReportWithoutHeader.rpt"));
            //}
            //else
            //{
            //    myDoc.Load(Server.MapPath("Rpt_IVFDashboard_ARTReport.rpt"));
            //}
            long PatientUnitID = 0;
            long PatientID = 0;
            long TherapyID = 0;
            long TherapyUnitID = 0;
            long PlannedTreatmentID = 0;

            if (Request.QueryString["PatientUnitID"] != null)
            {
                PatientUnitID = Convert.ToInt64(Request.QueryString["PatientUnitID"]);
            }
            if (Request.QueryString["TherapyId"] != null)
            {
                TherapyID = Convert.ToInt64(Request.QueryString["TherapyId"]);
            }
            if (Request.QueryString["TherapyUnitId"] != null)
            {
                TherapyUnitID = Convert.ToInt64(Request.QueryString["TherapyUnitId"]);
            }
            if (Request.QueryString["PatientID"] != null)
            {
                PatientID = Convert.ToInt64(Request.QueryString["PatientID"]);
            }
            if (Request.QueryString["PlannedTreatmentID"] != null)
            {
                PlannedTreatmentID = Convert.ToInt64(Request.QueryString["PlannedTreatmentID"]);
            }

            if (PlannedTreatmentID == 15 || PlannedTreatmentID == 16)
            {
                myDoc.Load(Server.MapPath("Rpt_IVFDashboard_ARTReportSummary_IUINew.rpt"));
                myDoc.SetParameterValue("@PatientID", PatientID);
                myDoc.SetParameterValue("@PatientUnitID", PatientUnitID);
                myDoc.SetParameterValue("@TherapyID", TherapyID);
                myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID);
                myDoc.SetParameterValue("@IsFromIUI", true);
            }
            else
            {

                if (PlannedTreatmentID == 1 || PlannedTreatmentID == 2 || PlannedTreatmentID == 3)
                {
                    myDoc.Load(Server.MapPath("Rpt_IVFDashboard_ARTReportSummary_New.rpt"));
                }

                if (PlannedTreatmentID == 12)
                {
                    myDoc.Load(Server.MapPath("Rpt_IVFDashboard_ARTReportSummary_NewRecipient.rpt"));
                }

                if (PlannedTreatmentID == 11)
                {
                    myDoc.Load(Server.MapPath("Rpt_IVFDashboard_ARTReportSummary_NewDonor.rpt"));
                }

                if (PlannedTreatmentID == 5)
                {
                    myDoc.Load(Server.MapPath("Rpt_IVFDashboard_ARTReportSummary_FET.rpt"));
                }

                myDoc.SetParameterValue("@PatientID", PatientID);
                myDoc.SetParameterValue("@PatientUnitID", PatientUnitID);
                myDoc.SetParameterValue("@TherapyID", TherapyID);
                myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID);

                //myDoc.SetParameterValue("@PatientID", PatientID,"TherapyDocument");
                //myDoc.SetParameterValue("@PatientUnitID", PatientUnitID,"TherapyDocument");
                //myDoc.SetParameterValue("@TherapyID", TherapyID,"TherapyDocument");
                //myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID,"TherapyDocument");          

                myDoc.SetParameterValue("@PatientID", PatientID, "Prescription");
                myDoc.SetParameterValue("@PatientUnitID", PatientUnitID, "Prescription");
                myDoc.SetParameterValue("@TherapyID", TherapyID, "Prescription");
                myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID, "Prescription");

                myDoc.SetParameterValue("@PatientID", PatientID, "StimulationPresciption");
                myDoc.SetParameterValue("@PatientUnitID", PatientUnitID, "StimulationPresciption");
                myDoc.SetParameterValue("@TherapyID", TherapyID, "StimulationPresciption");
                myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID, "StimulationPresciption");

                myDoc.SetParameterValue("@PatientID", PatientID, "TriggerPrescription");
                myDoc.SetParameterValue("@PatientUnitID", PatientUnitID, "TriggerPrescription");
                myDoc.SetParameterValue("@TherapyID", TherapyID, "TriggerPrescription");
                myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID, "TriggerPrescription");

                myDoc.SetParameterValue("@PatientID", PatientID, "BlastocystTransfered");
                myDoc.SetParameterValue("@PatientUnitID", PatientUnitID, "BlastocystTransfered");
                myDoc.SetParameterValue("@TherapyID", TherapyID, "BlastocystTransfered");
                myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID, "BlastocystTransfered");

                myDoc.SetParameterValue("@PatientID", PatientID, "EmbryoFreezing");
                myDoc.SetParameterValue("@PatientUnitID", PatientUnitID, "EmbryoFreezing");
                myDoc.SetParameterValue("@TherapyID", TherapyID, "EmbryoFreezing");
                myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID, "EmbryoFreezing");

                myDoc.SetParameterValue("@PatientID", PatientID, "BlastocystFreezing");
                myDoc.SetParameterValue("@PatientUnitID", PatientUnitID, "BlastocystFreezing");
                myDoc.SetParameterValue("@TherapyID", TherapyID, "BlastocystFreezing");
                myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID, "BlastocystFreezing");

                myDoc.SetParameterValue("@PatientID", PatientID, "FrozenEmbryo");
                myDoc.SetParameterValue("@PatientUnitID", PatientUnitID, "FrozenEmbryo");
                myDoc.SetParameterValue("@TherapyID", TherapyID, "FrozenEmbryo");
                myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID, "FrozenEmbryo");

                myDoc.SetParameterValue("@PatientID", PatientID, "EmbryosTransfered");
                myDoc.SetParameterValue("@PatientUnitID", PatientUnitID, "EmbryosTransfered");
                myDoc.SetParameterValue("@TherapyID", TherapyID, "EmbryosTransfered");
                myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID, "EmbryosTransfered");

                myDoc.SetParameterValue("@PatientID", PatientID, "EmbryoDiscarded");
                myDoc.SetParameterValue("@PatientUnitID", PatientUnitID, "EmbryoDiscarded");
                myDoc.SetParameterValue("@TherapyID", TherapyID, "EmbryoDiscarded");
                myDoc.SetParameterValue("@TherapyUnitID", TherapyUnitID, "EmbryoDiscarded");

            }


            myDoc.SetParameterValue("@UnitId", TherapyUnitID, "rpt_Header");
            myDoc.SetParameterValue("@UnitID", TherapyUnitID, "rpt_UnitLogo");

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