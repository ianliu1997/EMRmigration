using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Reflection;

namespace PalashDynamics.Web.Reports.Sales
{
    public partial class WeeklySalesReport : System.Web.UI.Page
    {
        ReportDocument myDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            String dbUserName = ConfigurationManager.AppSettings["DBUserName"].ToString();
            String dbUserPassword = ConfigurationManager.AppSettings["DBPassword"].ToString();
            String dbServer = ConfigurationManager.AppSettings["DBServer"].ToString();
            String dbName = ConfigurationManager.AppSettings["DBName"].ToString();
            string strPath;
            string strConnection;
            String strReportName = "Weekly Sales Report";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";

            bool IsExporttoExcel = Convert.ToBoolean(Request.QueryString["IsExporttoExcel"]);
            DateTime? DTF = null;

            long ClinicID = 0;
            string AppPath= Request.ApplicationPath;            
            if (Request.QueryString["ClinicID"] != null && Request.QueryString["ClinicID"] != "")
            {
                ClinicID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }
            if (Request.QueryString["Date"] != null && Request.QueryString["Date"] != "")
            {
                DTF = Convert.ToDateTime(Request.QueryString["Date"]);
            }
            if (!IsExporttoExcel)
            {
                ReportDocument myDoc = new ReportDocument();
                myDoc.Load(Server.MapPath("rptWeeklySalesReport.rpt"));
                myDoc.SetParameterValue("@ClinicID", ClinicID);
                myDoc.SetParameterValue("@FromDate", DTF);
                myDoc.SetDatabaseLogon(dbUserName, dbUserPassword, dbServer, dbName);
                CReportAuthentication.Impersonate(myDoc);
                CrystalReportViewer1.ReportSource = myDoc;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                CrystalReportViewer1.DataBind();
            }
            else
            {
                DataTable dt = new DataTable();
                SqlConnection sqlConnection1 = new SqlConnection(strConnection);
                sqlConnection1.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = sqlConnection1;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "CIMS_WeeklySalesReport";
                    SqlParameter param = new SqlParameter();
                    param.DbType = DbType.DateTime;
                    param.Value = DTF;
                    param.ParameterName = "FromDate";
                    cmd.Parameters.Add(param);
                    param = new SqlParameter();
                    param.DbType = DbType.Int64;
                    param.Value = ClinicID;
                    param.ParameterName = "ClinicID";
                    cmd.Parameters.Add(param);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                    ExportToExcel.CExcelExport objExcel = new ExportToExcel.CExcelExport();

                    objExcel.ConnectionString = strConnection;
                    DataTable dtReturn = new DataTable();                    
                    //Change Order ad define in Report
                    var varlist = from c in dt.AsEnumerable()                  
  
                    select new
                    {
                        SrNo = 0,
                        Description = "",
                        patientvisit = 0,
                        CountOfConsultancy = 0,
                        consultationrevenue = 0.0,
                        CountOfOtherServices = 0,
                        otherrevenue = 0.0,
                        CountOfPath =0,
                        diagnosticrevenue = 0.0,
                        CountOfPharmacy =0.0,
                        pharmacyrevenue = 0.0,
                        LoyaltyCardCount = 0,
                        LoyaltyCardRevenue = 0.0,
                        TotalRevenue =0.0,
                        AvgRevenue = 0.0,
                        LastMonthCountTilDate =0,
                        SecondLastMonthCountTilDate = 0,
                        IncreaseDecreasePatCount = "",
                        LastMonthrevenue = 0.0,
                        SecondLastMonthrevenue = 0.0,
                        PercentageIncreaseDecreaseForRevenue = "",
                        AvgPatientCount = 0.0,
                        YTDPatientCountTilDate = 0,
                        YTDrevenue = 0.0
                  
                    };
                    //(((c.IsNull("TotalRevenue") ? 0 : c.Field<System.Double>("TotalRevenue"))/(c.IsNull("patientvisit") ? 0 : c.Field<System.Double>("patientvisit")))) as 
                
                    
                    //Insert Above Query Values in New Table
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

                    //Change Columns Name As Mentioned on Report
                    if (dtReturn != null)
                    {
                        if (dtReturn.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtReturn.Columns.Count; i++)
                            {
                                if (dtReturn.Columns[i].ColumnName.Equals("SrNo"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Sr. No";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("Description"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Description";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("patientvisit"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Patient No";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("CountOfConsultancy"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Consultancy";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("consultationrevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Consultation Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("CountOfOtherServices"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Other Services";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("otherrevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Other Services Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("CountOfPath"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Diagnostics";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("diagnosticrevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Diagnostics Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("CountOfPharmacy"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Pharmarcy";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("pharmacyrevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Pharmarcy Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("LoyaltyCardCount"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Loyalty Card";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("LoyaltyCardRevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Loyalty Card Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("TotalRevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Total Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("LastMonthCountTilDate"))
                                {
                                    dtReturn.Columns[i].ColumnName = "WTD Patients Last Week";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("SecondLastMonthCountTilDate"))
                                {
                                    dtReturn.Columns[i].ColumnName = "WTD Patients Second Last Week";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("IncreaseDecreasePatCount"))
                                {
                                    dtReturn.Columns[i].ColumnName = "% Incease or Desrease";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("LastMonthrevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "WTD Revenue Last Week";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("SecondLastMonthrevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "WTD Revenue Second Last Week";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("PercentageIncreaseDecreaseForRevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Incease or Desrease In Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("AvgPatientCount"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Avg. Patient/day";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("YTDPatientCountTilDate"))
                                {
                                    dtReturn.Columns[i].ColumnName = "YTD Patient";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("YTDrevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "YTD Revenue";
                                }
                                if (dtReturn.Columns[i].ColumnName.Equals("AvgRevenue"))
                                {
                                    dtReturn.Columns[i].ColumnName = "Avg Revenue";
                                }
                            }

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dtReturn.Rows[i]["Sr. No"] = i + 1;

                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["Description"])))
                                {
                                    dtReturn.Rows[i]["Description"] = dt.Rows[i]["Description"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["patientvisit"])))
                                {
                                    dtReturn.Rows[i]["Patient No"] = dt.Rows[i]["patientvisit"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["CountOfConsultancy"])))
                                {
                                    dtReturn.Rows[i]["Consultancy"] = dt.Rows[i]["CountOfConsultancy"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["consultationrevenue"])))
                                {
                                    dtReturn.Rows[i]["Consultation Revenue"] = dt.Rows[i]["consultationrevenue"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["CountOfOtherServices"])))
                                {
                                    dtReturn.Rows[i]["Other Services"] = dt.Rows[i]["CountOfOtherServices"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["otherrevenue"])))
                                {
                                    dtReturn.Rows[i]["Other Services Revenue"] = dt.Rows[i]["otherrevenue"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["CountOfPath"])))
                                {
                                    dtReturn.Rows[i]["Diagnostics"] = dt.Rows[i]["CountOfPath"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["diagnosticrevenue"])))
                                {
                                    dtReturn.Rows[i]["Diagnostics Revenue"] = dt.Rows[i]["diagnosticrevenue"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["CountOfPharmacy"])))
                                {
                                    dtReturn.Rows[i]["Pharmarcy"] = dt.Rows[i]["CountOfPharmacy"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["pharmacyrevenue"])))
                                {
                                    dtReturn.Rows[i]["Pharmarcy Revenue"] = dt.Rows[i]["pharmacyrevenue"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LoyaltyCardCount"])))
                                {
                                    dtReturn.Rows[i]["Loyalty Card"] = dt.Rows[i]["LoyaltyCardCount"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LoyaltyCardRevenue"])))
                                {
                                    dtReturn.Rows[i]["Loyalty Card Revenue"] = dt.Rows[i]["LoyaltyCardRevenue"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LastMonthCountTilDate"])))
                                {
                                    dtReturn.Rows[i]["WTD Patients Last Week"] = dt.Rows[i]["LastMonthCountTilDate"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["SecondLastMonthCountTilDate"])))
                                {
                                    dtReturn.Rows[i]["WTD Patients Second Last Week"] = dt.Rows[i]["SecondLastMonthCountTilDate"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["IncreaseDecreasePatCount"])))
                                {
                                    dtReturn.Rows[i]["% Incease or Desrease"] = dt.Rows[i]["IncreaseDecreasePatCount"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["LastMonthrevenue"])))
                                {
                                    dtReturn.Rows[i]["WTD Revenue Last Week"] = dt.Rows[i]["LastMonthrevenue"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["SecondLastMonthrevenue"])))
                                {
                                    dtReturn.Rows[i]["WTD Revenue Second Last Week"] = dt.Rows[i]["SecondLastMonthrevenue"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["PercentageIncreaseDecreaseForRevenue"])))
                                {
                                    dtReturn.Rows[i]["Incease or Desrease In Revenue"] = dt.Rows[i]["PercentageIncreaseDecreaseForRevenue"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["AvgPatientCount"])))
                                {
                                    dtReturn.Rows[i]["Avg. Patient/day"] = dt.Rows[i]["AvgPatientCount"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["YTDPatientCountTilDate"])))
                                {
                                    dtReturn.Rows[i]["YTD Patient"] = dt.Rows[i]["YTDPatientCountTilDate"];
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["YTDrevenue"])))
                                {
                                    dtReturn.Rows[i]["YTD Revenue"] = dt.Rows[i]["YTDrevenue"];
                                }
                                if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["TotalRevenue"])))
                                {
                                    dtReturn.Rows[i]["Total Revenue"] = Convert.ToDouble(dt.Rows[i]["TotalRevenue"]);
                                    double trv = (double)(dtReturn.Rows[i]["Total Revenue"]);
                                    Int32 pc = (Int32)(dtReturn.Rows[i]["Patient No"]);
                                    if (pc != 0)
                                    {
                                        double newval = trv / pc;
                                        dtReturn.Rows[i]["Avg Revenue"] = newval;
                                    }
                                    else
                                    {
                                        dtReturn.Rows[i]["Avg Revenue"] = 0.0;
                                    }
                                }
                            }
                             objExcel.ExportToExcel("", strReportName, strPath, dtReturn);
                        }
                        else
                        {
                            dt = new DataTable();
                            dt.Columns.Add("Sr. No");
                            dt.Columns.Add("Description");
                            dt.Columns.Add("Patient No");
                            dt.Columns.Add("Consultancy");
                            dt.Columns.Add("Consultation Revenue");
                            dt.Columns.Add("Other Services");
                            dt.Columns.Add("Other Services Revenue");
                            dt.Columns.Add("Diagnostics ");
                            dt.Columns.Add("Diagnostics Revenue");
                            dt.Columns.Add("Pharmarcy ");
                            dt.Columns.Add("Pharmarcy Revenue");
                            dt.Columns.Add("LDT Patient Count");
                            dt.Columns.Add("LLDT Patient Count");
                            dt.Columns.Add("Total Revenue");
                            dt.Columns.Add("Avg Revenue");
                            dt.Columns.Add("WTD Patients Last Week");
                            dt.Columns.Add("WTD Patients Second Last Week");
                            dt.Columns.Add("% Incease or Desrease");
                            dt.Columns.Add("WTD Revenue Last Week");
                            dt.Columns.Add("WTD Revenue Second Last Week");
                            dt.Columns.Add("Incease or Desrease In Revenue");
                            dt.Columns.Add("Avg. Patient/day ");
                            dt.Columns.Add("YTD Patient");
                            dt.Columns.Add("YTD Revenue");
                            objExcel.ExportToExcel("", strReportName, strPath, dt);
                        }
                    }
                    else
                    {
                        dt = new DataTable();
                        dt.Columns.Add("Sr. No");
                        dt.Columns.Add("Clinc");
                        dt.Columns.Add("Patient Count");
                        dt.Columns.Add("Consultancy");
                        dt.Columns.Add("Consultation Revenue");
                        dt.Columns.Add("Other Services");
                        dt.Columns.Add("Other Services Revenue");
                        dt.Columns.Add("Diagnostics ");
                        dt.Columns.Add("Diagnostics Revenue");
                        dt.Columns.Add("Pharmarcy ");
                        dt.Columns.Add("Pharmarcy Revenue");
                        dt.Columns.Add("LDT Patient Count");
                        dt.Columns.Add("LLDT Patient Count");
                        dt.Columns.Add("Total Revenue");
                        dt.Columns.Add("Avg Revenue");
                        dt.Columns.Add("WTD Patients Last Week");
                        dt.Columns.Add("WTD Patients Second Last Week");
                        dt.Columns.Add("% Incease or Desrease");
                        dt.Columns.Add("WTD Revenue Last Week");
                        dt.Columns.Add("WTD Revenue Second Last Week");
                        dt.Columns.Add("Incease or Desrease In Revenue");
                        dt.Columns.Add("Avg. Patient/day ");
                        dt.Columns.Add("YTD Patient");
                        dt.Columns.Add("YTD Revenue");
                        objExcel.ExportToExcel("", strReportName, strPath, dt);
                    }

                    //Response.Redirect(Request.ApplicationPath + "\\Reports\\Sales\\" + strReportName + ".xls");
                    string URL = "/" + System.Configuration.ConfigurationSettings.AppSettings["VirtualDir"].ToString() + "/Reports/Sales/" + strReportName + ".xls";
                    Response.Write("<script language='javascript'>window.open('" + URL + "','_self');</script>");
                }
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