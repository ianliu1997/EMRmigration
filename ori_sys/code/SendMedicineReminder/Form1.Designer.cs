using System;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Collections.Generic;
//using CrystalDecisions.CrystalReports.Engine;

namespace SendMedicineReminder
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "Form1";
        }

        #endregion

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
        string Rpt = null;
        long ReportID = 0;
        string StrSQL = "";
        SqlConnection cn = null;
        SqlCommand cmd = null;
        SqlDataReader reader = null;
        List<clsMISConfigurationVO> RecordList = null;
        List<clsMISReportVO> UserList = null;
        //ReportDocument rptDoc;

        //public Form1()
        //{
        //    InitializeComponent();
        //}

        private void Form1_Loaded(object sender, EventArgs e)
        {
            RecordList = new List<clsMISConfigurationVO>();
            UserList = new List<clsMISReportVO>();

        }

        private void SendEmail(string ToEmailId, string rpt, string rptNm, long rptFormat)
        {
            try
            {
                MailMessage objMsg = new MailMessage();
              //  rptDoc = new ReportDocument();
              //  WriteLog("Email Send Started  :   " + " " + DateTime.Now);
                //if (rpt != null)
                //{
                //    rptDoc.Load(strReportPath + "\\" + rpt, CrystalDecisions.Shared.OpenReportMethod.OpenReportByDefault);
                //}                
                //rptDoc.SetDatabaseLogon(strDBUserName, strDBUserPassword, strDBServerName, strDBName);

                objMsg.From = new MailAddress(strSystemEmailId);

                if (ToEmailId != "" && ToEmailId != null)
                {
                    objMsg.To.Add(ToEmailId);
                    objMsg.Subject = "Razi Clinic report status ";
                    objMsg.Body = "This is a auto email generated using Palash Razi Application.";

                    //if (rptFormat == 1)
                    //{
                    //    objMsg.Attachments.Add(new Attachment(rptDoc.ExportToStream(ExportFormatType.PortableDocFormat), rptNm + ".pdf"));
                    //}
                    //else if (rptFormat == 2)
                    //{
                    //    objMsg.Attachments.Add(new Attachment(rptDoc.ExportToStream(ExportFormatType.Excel), rptNm + ".xls"));
                    //}

                    SmtpClient objSmtpClient = new SmtpClient(strSmtpServer, intSmtpPort);
                    objSmtpClient.Credentials = new NetworkCredential(strSystemEmailId, strEmailPassword);
                    try
                    {
                        objSmtpClient.Send(objMsg);
                     //   WriteLog("Email Sent Successfully  :   " + " " + DateTime.Now);
                    }
                    catch
                    {
                      //  WriteLog("Email sent Fail" + " " + DateTime.Now);
                    }
                }
            }
            catch (Exception ex)
            {
               // WriteLog("Error occured in SendEmail" + " " + DateTime.Now + " " + ex.Message);
            }


        }
        
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
        public DateTime? ConfigDate { get; set; }
        public string ScheduleName { get; set; }
        public long ScheduleOn { get; set; }
        public string ScheduleDetails { get; set; }
        public DateTime? ScheduleTime { get; set; }
        public DateTime? ScheduleStartDate { get; set; }
        public DateTime? ScheduleEndDate { get; set; }
        public bool Status { get; set; }
    }

}

