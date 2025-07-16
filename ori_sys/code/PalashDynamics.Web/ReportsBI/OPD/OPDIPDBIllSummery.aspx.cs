using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;

namespace PalashDynamics.Web.ReportsBI.OPD
{
    public partial class OPDIPDBIllSummery : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();

            long PatientlID = 0;
            long UnitID = 0;
            long PUnitID = 0;
            long PackageID = 0;
            long AdmID = 0;
            long BillID = 0;
            long BillUnitID = 0;

            myDoc = new ReportDocument();

            //isInline = Convert.ToInt64(Request.QueryString["isInline"]);

            //if (Request.QueryString["IsBilled"] != null)
            //{
            //    IsBilled = Convert.ToBoolean(Request.QueryString["IsBilled"]);
            //}
            //if (Request.QueryString["PrintFomatID"] == "1")
            //{
            //    myDoc.Load(Server.MapPath("rptClinicalBillReport.rpt"));
            //}
            //else if (Request.QueryString["PrintFomatID"] == "2")
            //{
            //    myDoc.Load(Server.MapPath("rptClinicalBillReportWithoutHeader.rpt"));
            //}
            //else if (Request.QueryString["PrintFomatID"] == "3")
            //{
            //    myDoc.Load(Server.MapPath("rptClinicalDraftBill.rpt"));
            //   // IsBilled = false;
            //}
            //else if (Request.QueryString["PrintFomatID"] == "4") // added by Bhushanp its for IsBilled = true OR Final Bill
            //{
            //    if (isInline == 1)
            //    {
            //    myDoc.Load(Server.MapPath("rptClinicalInlineBill.rpt"));   //rptClinicalInlineBill.rpt
            //}
            //    else
            //    {
            //        myDoc.Load(Server.MapPath("rptClinicalDraftBill_New.rpt"));
            //    }


            //}
            //else
            //{
            myDoc.Load(Server.MapPath("rptIPD_OPD_Summeryrpt.rpt"));
            //}

            if (Request.QueryString["PaitentID"] != null)
            {
                PatientlID = Convert.ToInt64(Request.QueryString["PaitentID"]);
            }

            if (Request.QueryString["UnitID"] != null)
            {
                UnitID = Convert.ToInt64(Request.QueryString["UnitID"]);
            }
            if (Request.QueryString["PUnitID"] != null)
            {
                PUnitID = Convert.ToInt64(Request.QueryString["PUnitID"]);
            }
            if (Request.QueryString["PackID"] != null)
            {
                PackageID = Convert.ToInt64(Request.QueryString["PackID"]);
            }

            if (Request.QueryString["VisitID"] != null)
            {
                AdmID = Convert.ToInt64(Request.QueryString["AdmID"]);
            }

            if (Request.QueryString["BillID"] != null)
            {
                BillID = Convert.ToInt64(Request.QueryString["BillID"]);
            }

            if (Request.QueryString["BillUnitID"] != null)
            {
                BillUnitID = Convert.ToInt64(Request.QueryString["BillUnitID"]);
            }

            //if (Request.QueryString["PrintFomatID"] == "4") // added by yk to show package details if bill is package for IPD Bill
            //{
            //    if (isInline == 0)
            //    {
            //        myDoc.SetParameterValue("@BillID", BillID, "ServiceDetailsPackage.rpt");//Added by yk
            //        myDoc.SetParameterValue("@UnitID", UnitID, "ServiceDetailsPackage.rpt");//Added by yk
            //    }
            //}

            //                    --@IsBilled = 1,
            //        --@BillID = 2253,
            //        @UnitID =2,
            //@PatientID =3259,
            //@PUnitID =2 ,

            //@PackageId =151

            myDoc.SetParameterValue("@PatientID", PatientlID);
            myDoc.SetParameterValue("@UnitID", BillUnitID);
            myDoc.SetParameterValue("@PUnitID", PUnitID);
            myDoc.SetParameterValue("@PackageId", PackageID);
            myDoc.SetParameterValue("@IsBilled", true);
            myDoc.SetParameterValue("@BillID", BillID);




            myDoc.SetParameterValue("@PatientID", PatientlID, "rpt_PackageOPDCosume.rpt");
            myDoc.SetParameterValue("@IsBilled", 0, "rpt_PackageOPDCosume.rpt");
            myDoc.SetParameterValue("@BillID", BillID, "rpt_PackageOPDCosume.rpt");
            myDoc.SetParameterValue("@UnitID", BillUnitID, "rpt_PackageOPDCosume.rpt");
            myDoc.SetParameterValue("@PackageId", PackageID, "rpt_PackageOPDCosume.rpt");
            myDoc.SetParameterValue("@PUnitID", PUnitID, "rpt_PackageOPDCosume.rpt");




            myDoc.SetParameterValue("@PatientID", PatientlID, "rpt_IPDOPDSUbReport.rpt");
            myDoc.SetParameterValue("@IsBilled", 0, "rpt_IPDOPDSUbReport.rpt");
            myDoc.SetParameterValue("@BillID", BillID, "rpt_IPDOPDSUbReport.rpt");
            myDoc.SetParameterValue("@UnitID", BillUnitID, "rpt_IPDOPDSUbReport.rpt");
            myDoc.SetParameterValue("@PackageId", PackageID, "rpt_IPDOPDSUbReport.rpt");
            myDoc.SetParameterValue("@PUnitID", PUnitID, "rpt_IPDOPDSUbReport.rpt");

            //if (VisitID > 0)
            //{

            myDoc.SetParameterValue("@PatientID", PatientlID, "PatientAgainstMaterialConsumption_New.rpt");
            myDoc.SetParameterValue("@PatientUnitID", PUnitID, "PatientAgainstMaterialConsumption_New.rpt");
            myDoc.SetParameterValue("@AdmID", AdmID, "PatientAgainstMaterialConsumption_New.rpt");
            myDoc.SetParameterValue("@AdmissionUnitID", UnitID, "PatientAgainstMaterialConsumption_New.rpt");
            myDoc.SetParameterValue("@PackageId", PackageID, "PatientAgainstMaterialConsumption_New.rpt");
            myDoc.SetParameterValue("@BillID", BillID, "PatientAgainstMaterialConsumption_New.rpt");

            //}

            //Package New Changes Added on 03052018
            myDoc.SetParameterValue("@PatientID", PatientlID, "rpt_PatientPackageConsumables");
            myDoc.SetParameterValue("@PatientUnitID", PUnitID, "rpt_PatientPackageConsumables");
            myDoc.SetParameterValue("@AdmID", AdmID, "rpt_PatientPackageConsumables");
            myDoc.SetParameterValue("@AdmissionUnitID", UnitID, "rpt_PatientPackageConsumables");
            myDoc.SetParameterValue("@PackageId", PackageID, "rpt_PatientPackageConsumables");
            myDoc.SetParameterValue("@BillID", BillID, "rpt_PatientPackageConsumables");

            //Package New Changes Added on 14052018
            myDoc.SetParameterValue("@PatientID", PatientlID, "rpt_PatientPackageConsmeExcess");
            myDoc.SetParameterValue("@PatientUnitID", PUnitID, "rpt_PatientPackageConsmeExcess");
            myDoc.SetParameterValue("@AdmID", AdmID, "rpt_PatientPackageConsmeExcess");
            myDoc.SetParameterValue("@AdmissionUnitID", UnitID, "rpt_PatientPackageConsmeExcess");
            myDoc.SetParameterValue("@PackageId", PackageID, "rpt_PatientPackageConsmeExcess");
            myDoc.SetParameterValue("@BillID", BillID, "rpt_PatientPackageConsmeExcess");

            // myDoc.SetParameterValue("@IsBilled", IsBilled, "rptClinicalDraftBill_New.rpt");

            //if (isInline == 1)
            //{
            //    myDoc.SetParameterValue("@BillID", PatientlID, "rptClinicalIPDFinalBill.rpt");
            //    myDoc.SetParameterValue("@UnitID", UnitID, "rptClinicalIPDFinalBill.rpt");
            //}




            myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt");
            myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt");
            myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
            CReportAuthentication.Impersonate(myDoc);
            CrystalReportViewer1.ReportSource = myDoc;
            CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            CrystalReportViewer1.DataBind();
            Stream oStream = null;
            byte[] byteArray = null;
            oStream = myDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
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