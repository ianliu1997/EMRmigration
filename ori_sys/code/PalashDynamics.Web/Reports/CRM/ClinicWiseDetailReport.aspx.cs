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
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace PalashDynamics.Web.Reports.CRM
{
    public partial class ClinicWiseDetailReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            string strPath;
            string strConnection;
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            String strReportName = "Clinic Wise Patient Details Report";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";
           
            string FromDate = null;
            string ToDate = null;
            string ToDatePrint = null;
            string LastMonth1 = "";
            string LastMonth2 = "";
            string SecondLastMonth1 = "";
            string SecondLastMonth2 = "";

            DateTime? FrDT = null;
            DateTime? ToDT = null;
            DateTime? ToDP = null;           

            long clinicID = 0;
            long BillNo = 0;
            bool IsExporttoExcel = false;
            string AppPath = Request.ApplicationPath;

            ReportDocument myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("crClinicWisePatientDetailReport.rpt"));

            FromDate = Convert.ToString(Request.QueryString["FromDate"]);
            ToDate = Convert.ToString(Request.QueryString["ToDate"]);
            ToDatePrint = Convert.ToString(Request.QueryString["ToDatePrint"]);
            IsExporttoExcel = Convert.ToBoolean(Request.QueryString["IsExporttoExcel"]);

            if (!string.IsNullOrEmpty(FromDate))
            {
                FrDT = Convert.ToDateTime(FromDate);
            }

            if (!string.IsNullOrEmpty(ToDate))
            {
                ToDT = Convert.ToDateTime(ToDate);
               // ToDP = ToDT;
            }
            if (!string.IsNullOrEmpty(ToDatePrint))
            {
                ToDP = Convert.ToDateTime(ToDatePrint);               
            }          
            if (Request.QueryString["ClinicID"] != null)
            {
                clinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }

            if (!IsExporttoExcel)
            {
                myDoc.SetParameterValue("@FromDate", FrDT);
                myDoc.SetParameterValue("@ToDate", ToDT);
                myDoc.SetParameterValue("@ClinicID", clinicID);
                myDoc.SetParameterValue("@ToDatePrint", ToDP);
                myDoc.SetParameterValue("@UnitID", clinicID, "rptUnitLogo.rpt");
                myDoc.SetParameterValue("@UnitID", clinicID, "rptCommonReportHeader.rpt");
                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);
                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                SqlConnection sqlConnection1 = new SqlConnection(strConnection);
                sqlConnection1.Open();
                cmd.Connection = sqlConnection1;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CIMS_rptClinicWisePatientReport";
                SqlParameter param = new SqlParameter();
                param.DbType = DbType.DateTime;
                param.Value = FrDT;
                param.ParameterName = "FromDate";
                cmd.Parameters.Add(param);
                param = new SqlParameter();
                param.DbType = DbType.DateTime;
                param.Value = ToDT;
                param.ParameterName = "ToDate";
                cmd.Parameters.Add(param);
                param = new SqlParameter();
                param.DbType = DbType.DateTime;
                param.Value = ToDP;
                param.ParameterName = "ToDatePrint";
                cmd.Parameters.Add(param);
                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = clinicID;
                param.ParameterName = "ClinicID";
                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                ExportToExcel.CExcelExport objExcel = new ExportToExcel.CExcelExport();
                DataTable dtReturn = new DataTable();
                objExcel.ConnectionString = strConnection;
                if (dt.Rows.Count > 0)
                {
                    var varlist = from c in dt.AsEnumerable()
                                  select new
                                  {
                                      SrNo = 0,
                                      Unit = c.IsNull("Unit") ? string.Empty : c.Field<string>("Unit"),
                                      PatientName = c.IsNull("PatientName") ? string.Empty : c.Field<string>("PatientName"),
                                      MRNo = c.IsNull("MRNo") ? string.Empty : c.Field<string>("MRNo"),
                                      BillDate = "",
                                      BillNo = c.IsNull("BillNo") ? string.Empty : c.Field<string>("BillNo"),
                                      ClinicalBill = 0,
                                      PharmacyAmt = c.IsNull("PharmacyAmt") ? 0.0 : c.Field<System.Double>("PharmacyAmt"),
                                      BillAmount = c.IsNull("BillAmount") ? 0.0 : c.Field<System.Double>("Billamount")
                                  };
                    PropertyInfo[] oProps = null;
                    foreach (var rec in varlist)
                    {
                        if (oProps == null)
                        {
                            oProps = ((Type)rec.GetType()).GetProperties();
                            foreach (PropertyInfo pi in oProps)
                            {
                                Type colType = pi.PropertyType;
                                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                                == typeof(Nullable<>)))
                                {
                                    colType = colType.GetGenericArguments()[0];
                                }
                                dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                            }
                        }

                        DataRow dr = dtReturn.NewRow();

                        foreach (PropertyInfo pi in oProps)
                        {
                            dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                            (rec, null);
                        }
                        dtReturn.Rows.Add(dr);
                    }
                }

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtReturn.Columns.Count; i++)
                        {
                            if (dtReturn.Columns[i].ColumnName.Equals("SrNo"))
                            {
                                dtReturn.Columns[i].ColumnName = "Sr. No";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("Unit"))
                            {
                                dtReturn.Columns[i].ColumnName = "Clinic Name";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("BillDate"))
                            {
                                dtReturn.Columns[i].ColumnName = "Bill Date";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("BillNo"))
                            {
                                dtReturn.Columns[i].ColumnName = "Bill No";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("PharmacyAmt"))
                            {
                                dtReturn.Columns[i].ColumnName = "Pharmacy Bill";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("BillAmount"))
                            {
                                dtReturn.Columns[i].ColumnName = "Total Bill Amount";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("PatientName"))
                            {
                                dtReturn.Columns[i].ColumnName = "Patient Name";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("Mr No"))
                            {
                                dtReturn.Columns[i].ColumnName = "Mr No";
                            }
                        }
                        //this is use for fields which are calculated by Formula and Sr no. 
                        for (int i = 0; i < dtReturn.Rows.Count; i++)
                        {
                            dtReturn.Rows[i]["Sr. No"] = i + 1;
                            dtReturn.Rows[i]["Bill Date"] = ((DateTime)dt.Rows[i]["BillDate"]).ToString("dd-MM-yyyy");
                            //dtReturn.Rows[i]["Expiry Date"] = ((DateTime)dt.Rows[i]["ExpiryDate"]).ToString("dd-MM-yyyy");
                            double BillAmount = (double)(dt.Rows[i]["BillAmount"]);
                            double PharmacyAmount = (double)(dt.Rows[i]["PharmacyAmt"]);
                            double ClinicaAmount = BillAmount - PharmacyAmount;
                            dtReturn.Rows[i]["Clinical Bill"] = ClinicaAmount;
                        }
                        objExcel.ExportToExcel("", strReportName, strPath, dtReturn);
                    }
                    else
                    {
                        dt = new DataTable();
                        dt.Columns.Add("Sr. No");
                        dt.Columns.Add("Clinic Name");
                        dt.Columns.Add("Patient Name");
                        dt.Columns.Add("Mr. No.");
                        dt.Columns.Add("Bill Date");
                        dt.Columns.Add("Bill No");
                        dt.Columns.Add("Clinical Bill");
                        dt.Columns.Add("Pharmacy Bill");
                        dt.Columns.Add("Total Bill Amount");
                        objExcel.ExportToExcel("", strReportName, strPath, dt);
                    }
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add("Sr. No");
                    dt.Columns.Add("Clinic Name");
                    dt.Columns.Add("Patient Name");
                    dt.Columns.Add("Mr. No.");
                    dt.Columns.Add("Bill Date");
                    dt.Columns.Add("Bill No");
                    dt.Columns.Add("Clinical Bill");
                    dt.Columns.Add("Pharmacy Bill");
                    dt.Columns.Add("Total Bill Amount");
                    objExcel.ExportToExcel("", strReportName, strPath, dt);
                }
                //Response.Redirect(AppPath + "\\Reports\\CRM\\" + strReportName + ".xls");
                //Response.Redirect(strReportName + ".xls");
                string URL = "/" + System.Configuration.ConfigurationSettings.AppSettings["VirtualDir"].ToString() + "/Reports/CRM/" + strReportName + ".xls";
                Response.Write("<script language='javascript'>window.open('" + URL + "','_self');</script>");
                //}
                sqlConnection1.Close();
            }
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