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
using PalashDynamics.DataAccessLayer;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace PalashDynamics.Web.Reports.CRM
{
    public partial class SearchPatientReport : System.Web.UI.Page
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

            String strReportName = "Search Patient Report";
            strReportName = strReportName + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Millisecond.ToString();
            strPath = Server.MapPath(".");
            strConnection = "server=" + dbServer + ";database=" + dbName + ";uid=" + dbUserName + ";pwd=" + dbUserPassword + "";

            DateTime? FromDate = null;
            DateTime? ToDate = null;
            DateTime? ToDatePrint = null;
            string MRNO = null;
            string OPDNo = null;
            string FirstName = null;
            string MiddleName = null;
            string LastName = null;
            string AgeFilter = null;
            string State = null;
            string City = null;
            string Area = null;
            string ContactNo = null;
            long UnitID = 0;
            long DepartmentID = 0;
            long DoctorID = 0;
            long GenderID = 0;
            long MaritalStatusID = 0;
            long LoyaltyCardID = 0;
            long ComplaintID = 0;
            int Age = 0;

            bool IsExporttoExcel = false;
            IsExporttoExcel = Convert.ToBoolean(Request.QueryString["IsExporttoExcel"]);
            string AppPath = Request.ApplicationPath;

            myDoc = new ReportDocument();
            myDoc.Load(Server.MapPath("crSearchPatient.rpt"));


            if (Request.QueryString["FromDate"] != null && Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["ToDate"] != null && Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            if (Request.QueryString["ToDatePrint"] != null && Request.QueryString["ToDatePrint"] != "")
            {
                ToDatePrint = Convert.ToDateTime(Request.QueryString["ToDatePrint"]);
            }
            if (Request.QueryString["MRNO"] != null && Request.QueryString["MRNO"] != "")
            {
                MRNO = Convert.ToString(Request.QueryString["MRNO"]);
            }
            if (Request.QueryString["OPDNO"] != null && Request.QueryString["OPDNO"] != "")
            {
                OPDNo = Convert.ToString(Request.QueryString["OPDNO"]);
            }
            if (Request.QueryString["FirstName"] != null && Request.QueryString["FirstName"] != "")
            {
                FirstName = Convert.ToString(Request.QueryString["FirstName"]);
            }
            if (Request.QueryString["MiddleName"] != null && Request.QueryString["MiddleName"] != "") 
            {
                MiddleName = Convert.ToString(Request.QueryString["MiddleName"]);
            }
            if (Request.QueryString["LastName"] != null && Request.QueryString["LastName"] != "")
            {
                LastName = Convert.ToString(Request.QueryString["LastName"]);
            }
            if (Request.QueryString["ClinicID"] != null)
            {
               UnitID = Convert.ToInt64(Request.QueryString["ClinicID"]);
            }
            if (Request.QueryString["DepartmentID"] != null)
            {
                DepartmentID = Convert.ToInt64(Request.QueryString["DepartmentID"]);
            }
            if (Request.QueryString["DoctorID"] != null)
            {
                DoctorID = Convert.ToInt64(Request.QueryString["DoctorID"]);
            }
            if (Request.QueryString["LoyaltyCardID"] != null)
            {
                LoyaltyCardID = Convert.ToInt64(Request.QueryString["LoyaltyCardID"]);
            }
            if (Request.QueryString["State"] != null && Request.QueryString["State"] != "")
            {
                State = Convert.ToString(Request.QueryString["State"]);
            }
            if (Request.QueryString["City"] != null && Request.QueryString["City"] != "")
            {
                City = Convert.ToString(Request.QueryString["City"]);
            }
            if (Request.QueryString["Area"] != null && Request.QueryString["Area"] != "")
            {
                Area = Convert.ToString(Request.QueryString["Area"]);
            }
            if (Request.QueryString["ContactNo"] != null && Request.QueryString["ContactNo"] != "")
            {
                ContactNo = Convert.ToString(Request.QueryString["ContactNo"]);
            }
            if (Request.QueryString["AgeFilter"] != null && Request.QueryString["AgeFilter"] != "")
            {
                AgeFilter = Convert.ToString(Request.QueryString["AgeFilter"]);
            }
            if (Request.QueryString["Age"] != null)
            {
                Age = Convert.ToInt32(Request.QueryString["Age"]);
            }
            if (Request.QueryString["GenderID"] != null)
            {
                GenderID = Convert.ToInt64(Request.QueryString["GenderID"]);
            }
            if (Request.QueryString["MaritalStatusID"] != null)
            {
                MaritalStatusID = Convert.ToInt64(Request.QueryString["MaritalStatusID"]);
            }
            if (Request.QueryString["ComplaintID"] != null)
            {
                ComplaintID = Convert.ToInt64(Request.QueryString["ComplaintID"]);
            }
            
            if (!IsExporttoExcel)
            {
                myDoc.SetParameterValue("@FromDate", FromDate);
                myDoc.SetParameterValue("@ToDate", ToDate);
                myDoc.SetParameterValue("@FirstName", Security.base64Encode(FirstName));
                myDoc.SetParameterValue("@MiddleName", Security.base64Encode(MiddleName));
                myDoc.SetParameterValue("@LastName", Security.base64Encode(LastName));
                myDoc.SetParameterValue("@MrNo", MRNO);
                myDoc.SetParameterValue("@OPDNo", OPDNo);
                myDoc.SetParameterValue("@UnitID", UnitID);
                myDoc.SetParameterValue("@DepartmentID", DepartmentID);
                myDoc.SetParameterValue("@DoctorID", DoctorID);
                myDoc.SetParameterValue("@State", Security.base64Encode(State));
                myDoc.SetParameterValue("@City", Security.base64Encode(City));
                myDoc.SetParameterValue("@Area",Security.base64Encode(Area));
                myDoc.SetParameterValue("@GenderID", GenderID);
                myDoc.SetParameterValue("@MaritalStatusID", MaritalStatusID);
                myDoc.SetParameterValue("@ContactNo", ContactNo);
                myDoc.SetParameterValue("@LoyaltyCardID", LoyaltyCardID);
                myDoc.SetParameterValue("@ComplaintID", ComplaintID);
                myDoc.SetParameterValue("@Age", Age);
                myDoc.SetParameterValue("@AgeFilter", AgeFilter);
                myDoc.SetParameterValue("@ToDatePrint", ToDatePrint);
                myDoc.SetParameterValue("@UnitID", UnitID, "rptUnitLogo.rpt");
                myDoc.SetParameterValue("@UnitID", UnitID, "rptCommonReportHeader.rpt"); //Added by ajit jadhav date 19/9/2016 Get Unit LOGO
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

                //using (SqlCommand cmd = new SqlCommand())
                //{
                cmd.Connection = sqlConnection1;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "CIMS_rpt_SearchPatient";
                SqlParameter param = new SqlParameter();
                param.DbType = DbType.DateTime;
                param.Value = FromDate;
                param.ParameterName = "FromDate";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.DateTime;
                param.Value = ToDate;
                param.ParameterName = "ToDate";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.String;
                param.Value = Security.base64Encode(FirstName);
                param.ParameterName = "FirstName";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.String;
                param.Value = Security.base64Encode(MiddleName);
                param.ParameterName = "MiddleName";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.String;
                param.Value = Security.base64Encode(LastName);
                param.ParameterName = "LastName";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = MRNO;
                param.ParameterName = "MrNo";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = OPDNo;
                param.ParameterName = "OPDNo";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = UnitID;
                param.ParameterName = "UnitID";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = DepartmentID;
                param.ParameterName = "DepartmentID";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = DoctorID;
                param.ParameterName = "DoctorID";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.String;
                param.Value = State;
                param.ParameterName = "State";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.String;
                param.Value = City;
                param.ParameterName = "City";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.String;
                param.Value = Area;
                param.ParameterName = "Area";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = GenderID;
                param.ParameterName = "GenderID";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = MaritalStatusID;
                param.ParameterName = "MaritalStatusID";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = ContactNo;
                param.ParameterName = "ContactNo";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = LoyaltyCardID;
                param.ParameterName = "LoyaltyCardID";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = ComplaintID;
                param.ParameterName = "ComplaintID";
                cmd.Parameters.Add(param);

                param = new SqlParameter();
                param.DbType = DbType.Int64;
                param.Value = Age;
                param.ParameterName = "Age";
                cmd.Parameters.Add(param);


                param = new SqlParameter();
                param.DbType = DbType.String;
                param.Value = AgeFilter;
                param.ParameterName = "AgeFilter";
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                ExportToExcel.CExcelExport objExcel = new ExportToExcel.CExcelExport();
                DataTable dtReturn = new DataTable();
                objExcel.ConnectionString = strConnection;
                if (dt.Rows.Count > 0)
                {
                    //Change Order ad define in Report
                    var varlist = from c in dt.AsEnumerable()

                                  select new
                                  {
                                      SrNo = 0,
                                      MRNo = "",
                                      PatientName = c.IsNull("PatientName") ? string.Empty : c.Field<string>("PatientName"),
                                      DateOfBirth = "",
                                      Complaint = c.IsNull("Complaint") ? string.Empty : c.Field<string>("Complaint")
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
                            if (dtReturn.Columns[i].ColumnName.Equals("Sr No"))
                            {
                                dtReturn.Columns[i].ColumnName = "Sr. No";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("Mr No"))
                            {
                                dtReturn.Columns[i].ColumnName = "Mr. No";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("PatientName"))
                            {
                                dtReturn.Columns[i].ColumnName = "Patient Name";
                            }
                            if (dtReturn.Columns[i].ColumnName.Equals("DateOfBirth"))
                            {
                                dtReturn.Columns[i].ColumnName = "Date Of Birth";
                            }
                        }
                        //this is use for fields which are calculated by Formula and Sr no. 
                        for (int i = 0; i < dtReturn.Rows.Count; i++)
                        {
                            dtReturn.Rows[i]["Sr. No"] = i + 1;
                            dtReturn.Rows[i]["Mr. No"] = dt.Rows[i]["Mr No"];
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["DateOfBirth"])))
                            {
                                dtReturn.Rows[i]["Date Of Birth"] = ((DateTime)dt.Rows[i]["DateOfBirth"]).ToString("dd-MM-yyyy");
                            }
                        }
                        objExcel.ExportToExcel("", strReportName, strPath, dtReturn);
                    }
                    else
                    {
                        dt = new DataTable();
                        dt.Columns.Add("Sr. No.");
                        dt.Columns.Add("Mr. No.");
                        dt.Columns.Add("Patient Name");
                        dt.Columns.Add("Date OF Birth");
                        dt.Columns.Add("Complaint");

                        objExcel.ExportToExcel("", strReportName, strPath, dt);
                    }
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add("Sr. No.");
                    dt.Columns.Add("Mr. No.");
                    dt.Columns.Add("Patient Name");
                    dt.Columns.Add("Date OF Birth");
                    dt.Columns.Add("Complaint");
                    objExcel.ExportToExcel("", strReportName, strPath, dt);
                }
                //Response.Redirect(AppPath + "\\Reports\\CRM\\" + strReportName + ".xls");
                //Response.Redirect(strReportName + ".xls");
                string URL = "/" + System.Configuration.ConfigurationSettings.AppSettings["VirtualDir"].ToString() + "/Reports/CRM/" + strReportName + ".xls";
                Response.Write("<script language='javascript'>window.open('" + URL + "','_self');</script>");
                //  }
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
