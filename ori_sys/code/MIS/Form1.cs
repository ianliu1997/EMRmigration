using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Configuration;
using CrystalDecisions.Shared;
using MIS.RaziClinicServiceRef;
using CIMS;
//using PalashDynamics.ValueObjects;
//using PalashDynamics.ValueObjects.Administration.MISConfiguration;
using CrystalDecisions.CrystalReports.Engine;
using System.Timers;
using System.Data.SqlClient;

namespace MIS
{
    public partial class Form1 : Form
    {
        String strSystemEmailId = ConfigurationManager.AppSettings["systemEmailId"].ToString();
        String strEmailPassword = ConfigurationManager.AppSettings["emailPassword"].ToString();
        String strSmtpServer = ConfigurationManager.AppSettings["smtpServer"].ToString();
        int intSmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"].ToString());
        String strReportPath = ConfigurationManager.AppSettings["ReportPath"].ToString();
        String strDBServerName = ConfigurationManager.AppSettings["ServerName"].ToString();
        String strDBName = ConfigurationManager.AppSettings["DBName"].ToString();
        String strDBUserName = ConfigurationManager.AppSettings["UserName"].ToString();
        String strDBUserPassword = ConfigurationManager.AppSettings["UserPwd"].ToString();
        System.Timers.Timer Tm;



        string ToEmailID = null;
        string ReportName = null;
        DateTime? StartDate;
        DateTime? EndDate;
        DateTime? StartTime;
        long ReportFormat = 0;
        string Rpt=null;
        long ReportID = 0;

        string StrSQL = "";
        SqlConnection cn = null;
        SqlCommand cmd = null;
        SqlDataReader reader = null;

        List<clsMISConfigurationVO> RecordList = null;
        List<clsMISReportVO> UserList = null;
        ReportDocument rptDoc;
       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Tm = new System.Timers.Timer();
            //Tm.Elapsed += new System.Timers.ElapsedEventHandler(Tm_Elapsed);
            //Tm.Interval = 5000;
            //Tm.Enabled = true;
            //Tm.Start();
            RecordList = new List<clsMISConfigurationVO>();
            UserList = new List<clsMISReportVO>();

            MessageBox.Show("Do u want to start");
            GetEmailDetails();
            
        }

        void Tm_Elapsed(object sender, ElapsedEventArgs e)
        {
            GetEmailDetails();

        }

        private void SendEmail(string ToEmailId,string rpt,string rptNm,long rptFormat)
        {
            try
            {
                MailMessage objMsg = new MailMessage();
                rptDoc = new ReportDocument();

                WriteLog("Email Send Started  :   " + " " + DateTime.Now);

                if (rpt != null)
                {
                    rptDoc.Load(strReportPath + "\\" + rpt, CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault);
                }
                //rptDoc.Load("D:\\Razi\\RAZIClinic.Web\\Reports\\WeeklyServicewiseCollection.rpt", CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault);



                //if (ReportID == 17)
                //{

                //    rptDoc.SetParameterValue("@ClinicID", 0);
                //    rptDoc.SetParameterValue("@FromDate", DateTime.Now.Date.Date);
                //    WriteLog("Daily Sales report : " + " " + DateTime.Now);
                //}
                rptDoc.SetDatabaseLogon(strDBUserName, strDBUserPassword, strDBServerName, strDBName);

                //for (int count = 0; count < rptDoc.ParameterFields.Count; count++)
                //{
                //    rptDoc.SetParameterValue(rptDoc.ParameterFields[count].Name, null);
                //}

                objMsg.From = new MailAddress(strSystemEmailId);

                if (ToEmailId != "" && ToEmailId != null)
                {
                    objMsg.To.Add(ToEmailId);
                    objMsg.Subject = "Razi Clinic report status ";
                    objMsg.Body = "This is a auto email generated using Palash Razi Application.";

                    if (rptFormat == 1)
                    {
                        objMsg.Attachments.Add(new Attachment(rptDoc.ExportToStream(ExportFormatType.PortableDocFormat), rptNm + ".pdf" ));
                    }
                    else if (rptFormat == 2)
                    {
                        objMsg.Attachments.Add(new Attachment(rptDoc.ExportToStream(ExportFormatType.Excel), rptNm + ".xls"));
                    }

                    SmtpClient objSmtpClient = new SmtpClient(strSmtpServer, intSmtpPort);
                    objSmtpClient.Credentials = new NetworkCredential(strSystemEmailId, strEmailPassword);

                    try
                    {
                        objSmtpClient.Send(objMsg);
                        WriteLog("Email Request Sent To The Server Successfully  :   " + " " + DateTime.Now);
                    }
                    catch
                    {
                        WriteLog("Email sent Fail" + " " + DateTime.Now);

                    }
                }
            }
            catch(Exception ex)
            {
                WriteLog("Error occured in SendEmail" + " " + DateTime.Now + " " + ex.Message);
            }


        }
        
        void WriteLog(string strMessage)
        {
            try
            {
                FileStream fs = new FileStream(@"c:\Palash Auto MIS Log.txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter m_streamWriter = new StreamWriter(fs);
                m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                m_streamWriter.WriteLine(strMessage);
                m_streamWriter.Flush();
                m_streamWriter.Close();
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void GetEmailDetails()
        {
           
            try
            {

                cn = new SqlConnection();

                cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ToString();

                cmd = new SqlCommand("CIMS_GetEmailDetailsForMIS", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    clsMISConfigurationVO obj = new clsMISConfigurationVO();
                    obj.ID = Convert.ToInt64((reader["Id"]));
                    obj.ClinicId = Convert.ToInt64((reader["ClinicId"]));
                    obj.ScheduleName = Convert.ToString((reader["ScheduleName"]));
                    obj.MISReportFormatId = Convert.ToInt64((reader["MISReportFormatId"]));
                    obj.ScheduleOn = Convert.ToInt64((reader["ScheduledOn"]));
                    obj.ScheduleDetails = Convert.ToString((reader["ScheduleDetails"]));
                    obj.ScheduleTime = Convert.ToDateTime((reader["ScheduleTime"]));
                    obj.ScheduleStartDate = Convert.ToDateTime((reader["ScheduleStartDate"]));
                    if (reader["ScheduleEndDate"] != DBNull.Value)
                    {
                        obj.ScheduleEndDate = Convert.ToDateTime(reader["ScheduleEndDate"]);
                    }
                    RecordList.Add(obj);
                }

                List<long> ReportIDList = new List<long>();
                foreach (var item in RecordList)
                {
                    StartDate = item.ScheduleStartDate;
                    EndDate = item.ScheduleEndDate;
                    StartTime = item.ScheduleTime;

                    //Daily
                    DateTime ScheduleTime = DateTime.Parse(StartTime.ToString());
                    DateTime DailyTime = DateTime.Parse(DateTime.Now.ToString());
                    string Str1 = null;
                    string Str2 = null;

                    Str1 = DailyTime.ToString("HH:mm");
                    Str2 = ScheduleTime.ToString("HH:mm");
                    DateTime TempDate1 = Convert.ToDateTime(Str1);
                    DateTime TempDate2 = Convert.ToDateTime(Str2);

                    //Weekly
                    DayOfWeek Day = 0;
                    Day = DateTime.Now.Date.DayOfWeek;
                    
                    
                    
                    string ScheduleDetails = item.ScheduleDetails;
                    char[] Splitchar = { ',' };
                    string[] DayID = ScheduleDetails.Split(Splitchar);

                    switch (item.ScheduleOn)
                    {
                        case 1:

                            if (StartDate != null && EndDate == null)
                            {
                                if (StartDate <= DateTime.Now.Date.Date)
                                {
                                    //if (DailyTime.ToString("HH:mm") == ScheduleTime.ToString("HH:mm"))
                                    if (TempDate1 >= TempDate2)
                                    {
                                       // GetReportDetails(item.ID);
                                        ReportIDList.Add(item.ID);
                                    }
                                }
                            }
                            else if (StartDate != null && EndDate != null)
                            {
                                if (StartDate <= DateTime.Now.Date.Date && EndDate >= DateTime.Now.Date.Date)
                                {
                                    //if (DailyTime.ToString("HH:mm") == ScheduleTime.ToString("HH:mm"))
                                    if (TempDate1 >= TempDate2)
                                    {
                                        //GetReportDetails(item.ID);
                                        ReportIDList.Add(item.ID);
                                    }

                                }

                            }
                            break;

                        case 2:
                            if (StartDate != null && EndDate == null)
                            {
                                if (StartDate <= DateTime.Now.Date.Date)
                                {
                                    for (int count = 0; count <= DayID.Length - 1; count++)
                                    {
                                        DayOfWeek mDay;
                                        string strDay;
                                        mDay = (DayOfWeek)(Convert.ToInt32(DayID[count]));
                                       // strDay = Convert.ToString(mDay);
                                        if (Day == mDay)
                                        {
                                            //if (DailyTime.ToString("HH:mm") == ScheduleTime.ToString("HH:mm"))
                                            if (TempDate1 >= TempDate2)
                                            {
                                               // GetReportDetails(item.ID);
                                                ReportIDList.Add(item.ID);
                                            }

                                        }
                                    }
                                }

                            }
                            else if (StartDate != null && EndDate != null)
                            {
                                if (StartDate <= DateTime.Now.Date.Date && EndDate >= DateTime.Now.Date.Date)
                                {
                                    for (int count = 0; count <= DayID.Length - 1; count++)
                                    {
                                        DayOfWeek mDay;
                                        string strDay;
                                        mDay = (DayOfWeek)(Convert.ToInt32(DayID[count]));
                                        //strDay = Convert.ToString(mDay);
                                        if (Day == mDay)
                                        {
                                            //if (DailyTime.ToString("HH:mm") == ScheduleTime.ToString("HH:mm"))
                                            if (TempDate1 >= TempDate2)
                                            {
                                               // GetReportDetails(item.ID);
                                                ReportIDList.Add(item.ID);
                                            }
                                        }
                                    }

                                }

                            }
                            break;

                        case 3:
                            if (StartDate != null && EndDate == null)
                            {
                                if (StartDate <= DateTime.Now.Date.Date)
                                {
                                    if (DateTime.Now.Day == Convert.ToInt32(item.ScheduleDetails))
                                    {
                                        //if (DailyTime.ToString("HH:mm") == ScheduleTime.ToString("HH:mm"))
                                        if (TempDate1 >= TempDate2)
                                        {
                                           // GetReportDetails(item.ID);
                                            ReportIDList.Add(item.ID);
                                        }
                                    }
                                }
                            }
                            else if (StartDate != null && EndDate != null)
                            {
                                if (StartDate <= DateTime.Now.Date.Date && EndDate >= DateTime.Now.Date.Date)
                                {
                                    if (DateTime.Now.Day == Convert.ToInt32(item.ScheduleDetails))
                                    {
                                        //if (DailyTime.ToString("HH:mm") == ScheduleTime.ToString("HH:mm"))
                                        if (TempDate1 >= TempDate2)
                                        {
                                           // GetReportDetails(item.ID);
                                            ReportIDList.Add(item.ID);
                                        }
                                    }
                                }

                            }
                            break;
                    }
                   


                }
                
                if (ReportIDList != null && ReportIDList.Count > 0)
                {
                    foreach (var item1 in ReportIDList)
                    {
                        GetReportDetails(item1);
                    }

                }



            }

            catch (Exception ex)
            {
                WriteLog("Error occured in GetEmailDetails Function :   " + " " + DateTime.Now + " "+ ex.Message);
            }
            finally
            {
                // Close data reader object and database connection
                if (reader != null)
                    reader.Close();

                if (cn.State == ConnectionState.Open)
                    cn.Close();
                WriteLog("GetEmailDetails Function  completed:   " + " " + DateTime.Now);
            }
        }



        private void GetReportDetails(long iID)
        {
            try
            {

                cn = new SqlConnection();

                cn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ToString();

                cmd = new SqlCommand("CIMS_GetMISReportDetails", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                cmd.Parameters.Add("@MISID", SqlDbType.Int).Value = iID;
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    clsMISReportVO ObjDetails = new clsMISReportVO();
                    ObjDetails.MISID = Convert.ToInt64((reader["Id"]));
                    ObjDetails.Sys_MISReportId = Convert.ToInt64((reader["Sys_MISReportId"]));
                    ObjDetails.MISReportFormat = Convert.ToInt64((reader["MISReportFormatId"]));
                    ObjDetails.rptFileName = Convert.ToString((reader["rptFileName"]));
                    ObjDetails.ReportName = Convert.ToString((reader["ReportName"]));
                    ObjDetails.StaffTypeID = Convert.ToInt64((reader["StaffTypeId"]));
                    ObjDetails.StaffID = Convert.ToInt64((reader["StaffId"]));
                    ObjDetails.EmailID = Convert.ToString((reader["EmailID"]));
                    UserList.Add(ObjDetails);
                }

                foreach (var item in UserList)
                {
                    ToEmailID = item.EmailID;
                    ReportName = item.ReportName;
                    ReportFormat = item.MISReportFormat;
                    Rpt = item.rptFileName;
                    ReportID = item.Sys_MISReportId;

                    if (ToEmailID != null)
                    {
                        WriteLog("Send email Started  :   " + " " + DateTime.Now + " " + ReportName);
                        SendEmail(ToEmailID, Rpt, ReportName, ReportFormat);
                    }

                }
                this.Close();



            }
            catch (Exception ex)
            {
                WriteLog("Error occured in GetReportDetails Function :   " + " " + DateTime.Now + " " + ex.Message);
            }
            finally
            {

                if (reader != null)
                    reader.Close();

                if (cn.State == ConnectionState.Open)
                    cn.Close();
                WriteLog("GetReportDetails function completed :   " + " " + DateTime.Now);
            }



        }

        //private void GetReportDetails(long iID)
        //{
        //    clsGetMISReportDetailsBiZActionVO BizAction1 = new clsGetMISReportDetailsBiZActionVO();
        //    BizAction1.MISReportDetails = new List<clsMISReportVO>();

        //    BizAction1.MISID = iID;
        //    PalashServiceClient service = new PalashServiceClient();

        //    BizAction1 = (clsGetMISReportDetailsBiZActionVO)service.Process(BizAction1, new clsUserVO());

        //    if (BizAction1.MISReportDetails != null)
        //    {
        //        foreach (var item in BizAction1.MISReportDetails)
        //        {
        //            ToEmailID = item.EmailID;
        //            ReportName = item.ReportName;
        //            ReportFormat = item.MISReportFormat;
        //            Rpt = item.rptFileName;
        //            ReportID = item.Sys_MISReportId;

        //            if (ToEmailID != null)
        //            {
        //                SendEmail(ToEmailID, Rpt, ReportName,ReportFormat);
        //            }

        //        }
        //        this.Close();
               
        //    }

        //}


        
        #region 
        //private void SendEmail(string ToEmailId)
        //{
        //    MailMessage objMsg = new MailMessage();
        //    CrystalDecisions.CrystalReports.Engine.ReportDocument rptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

        //    WriteLog("Email Send Started  :   " + " " + DateTime.Now);
        //    //rptDoc.Load(strReportPath, CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault);

        //    rptDoc.Load("D:\\Razi\\RAZIClinic.Web\\Reports\\WeeklyServicewiseCollection.rpt", CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault);
        //    rptDoc.SetDatabaseLogon(strDBUserName, strDBUserPassword, strDBServerName, strDBName);

        //    objMsg.From = new MailAddress(strSystemEmailId);
        //    objMsg.To.Add(ToEmailId);

        //    // objMsg.Subject = "Razi Clinic " + " " + ReportName;
        //    objMsg.Subject = "Razi Clinic ";
        //    objMsg.Body = "This is a auto email generated using Palash Razi Application.";



        //    objMsg.Attachments.Add(new Attachment(rptDoc.ExportToStream(ExportFormatType.PortableDocFormat), "Weekly Sales Report.pdf"));
        //    objMsg.Attachments.Add(new Attachment(rptDoc.ExportToStream(ExportFormatType.Excel), "Weekly Sales Report.xls"));

        //    SmtpClient objSmtpClient = new SmtpClient(strSmtpServer, intSmtpPort);
        //    objSmtpClient.Credentials = new NetworkCredential(strSystemEmailId, strEmailPassword);

        //    try
        //    {
        //        objSmtpClient.Send(objMsg);
        //        WriteLog("Email Sent Successfully  :   " + " " + DateTime.Now);

        //    }
        //    catch
        //    {
        //        WriteLog("Fail" + " " + DateTime.Now);

        //    }


        //}
        #endregion

        
    }



    public class clsMISConfigurationVO
    {
        private long _id;
        public long ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public long ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string ClinicCode { get; set; }
        public string MISTypeName { get; set; }
        public DateTime? ConfigDate { get; set; }

        public string ScheduleName { get; set; }

        public long MISTypeId { get; set; }

        public long MISReportFormatId { get; set; }

        public long ScheduleOn { get; set; }

        public string ScheduleDetails { get; set; }

        public DateTime? ScheduleTime { get; set; }

        public DateTime? ScheduleStartDate { get; set; }

        public DateTime? ScheduleEndDate { get; set; }

        public bool Status { get; set; }
    }


    public class clsMISReportVO
    {
        

        private long _id;
        public long MISID
        {
            get { return _id; }
            set { _id = value; }
        }


        private long _Sys_MISReportId;
        public long Sys_MISReportId
        {
            get { return _Sys_MISReportId; }
            set { _Sys_MISReportId = value; }
        }


        private string _rptFileName;
        public string rptFileName
        {
            get { return _rptFileName; }
            set { _rptFileName = value; }
        }


        private string _ReportName;
        public string ReportName
        {
            get { return _ReportName; }
            set { _ReportName = value; }
        }

        public long MISReportFormat { get; set; }

        private long _StaffTypeID;
        public long StaffTypeID
        {
            get { return _StaffTypeID; }
            set { _StaffTypeID = value; }
        }

        private long _StaffID;
        public long StaffID
        {
            get { return _StaffID; }
            set { _StaffID = value; }
        }

        private string _EmailID;
        public string EmailID
        {
            get { return _EmailID; }
            set { _EmailID = value; }
        }



    }
    
}
