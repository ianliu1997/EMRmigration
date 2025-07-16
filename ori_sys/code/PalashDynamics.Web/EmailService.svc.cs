using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using PalashDynamics.BusinessLayer;
using PalashDynamics.ValueObjects;
using WcfExceptionExample.Web.Behavior;
using WcfExceptionExample.Web.DataContracts;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Web.Configuration;
using System.Web;
using System.Net.Configuration;
using System.IO;
using PalashDynamics.ValueObjects.Administration;
using System.Collections.ObjectModel;


namespace PalashDynamics.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [WcfErrorBehavior]
    [WcfSilverlightFaultBehavior]
    [DataContract]
    public class EmailService
    {
        //[OperationContract]
        //public void DoWork()
        //{
        //    // Add your operation implementation here
        //    return;
        //}
        // Add more operations here and mark them with [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        [OperationContract]
        public bool SendEmail(string fromEmail, string toEmail, string subject, string body)
        {
            String Username = null;
            String Password = null;
            try
            {
                //get details from web.config files
                Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
                String Host = settings.Smtp.Network.Host;
                int Port = settings.Smtp.Network.Port;
                Username = settings.Smtp.Network.UserName;
                Password = settings.Smtp.Network.Password;
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromEmail);
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = body;
                SmtpClient smtpserver = new SmtpClient(Host);
                smtpserver.Credentials = new NetworkCredential(Username, Password);
                smtpserver.Port = Port;
                smtpserver.EnableSsl = false;
                smtpserver.Send(message);
                WriteLog("Email Send Succesfully" + " " + DateTime.Now + " User: " + Username + " Pwd: " + Password + " From: " + fromEmail + " To " + toEmail);
                return true;
            }
            catch (Exception ex)
            {
                WriteLog("Email Send Fail" + " " + ex.Message + "  " + DateTime.Now + " User: " + Username + " Pwd: " + Password + " From: " + fromEmail + " To " + toEmail);
                return false;
            }
        }


        [FaultContract(typeof(ConcurrencyException))]
        [OperationContract]
        public bool SendEmailwithAttachment(string fromEmail, string toEmail, string subject, string body, long NoofAttachments, List<string> AttachedFile)
        {
            try
            {
                //get details from web.config files
                Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
                String Host = settings.Smtp.Network.Host;
                int Port = settings.Smtp.Network.Port;
                String Username = settings.Smtp.Network.UserName;
                String Password = settings.Smtp.Network.Password;
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromEmail);
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                //Dim att As New Attachment("C:\Users\Public\warsame\crack\keylogs.txt")
                string filePath =  "/EmailTemplateAttachment/";
                for (int i = 0; i < NoofAttachments; i++)
                {
                    //string RD = HttpContext.Current.Server.MapPath("EmailTemplateAttachment") + @"\" + AttachedFile;
                    filePath = HttpContext.Current.Server.MapPath("EmailTemplateAttachment") + @"\" + AttachedFile[i];
                    Attachment att = new Attachment(filePath);
                    message.Attachments.Add(att);
                }
                SmtpClient smtpserver = new SmtpClient(Host);
                smtpserver.Credentials = new NetworkCredential(Username, Password);
                smtpserver.Port = Port;

                smtpserver.EnableSsl = false;

                smtpserver.Send(message);
                message.Attachments.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        void WriteLog(string strMessage)
        {
            try
            {
                string filePath = "/EmailTemplateAttachment/";

                filePath = HttpContext.Current.Server.MapPath("EmailTemplateAttachment") + @"\" + "Palash Email Send from app.txt";
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
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

        [FaultContract(typeof(ConcurrencyException))]
        [OperationContract]
        public bool SendEmailwithAttachmentForPathology(string fromEmail, string toEmail, string subject, string body, long NoofAttachments, List<string> AttachedFile, long UnitId)
        {
            String Username = null;
            String Password = null;
            String Host = null;
            int Port = 0;
            bool EnableSsl = false;
            try
            {
                //get details from web.config files
                Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
                //String Host = settings.Smtp.Network.Host;
                //int Port = settings.Smtp.Network.Port;
                //String Username = settings.Smtp.Network.UserName;
                //String Password = settings.Smtp.Network.Password;


                clsGetAppConfigBizActionVO BizAction = new clsGetAppConfigBizActionVO();
                BizAction.AppConfig = new clsAppConfigVO();
                BizAction.UnitID = UnitId;
                PalashDynamicsWeb service = new PalashDynamicsWeb();
                BizAction = (clsGetAppConfigBizActionVO)service.Process(BizAction, new clsUserVO());
                if (BizAction.AppConfig != null)
                {
                    Host = BizAction.AppConfig.Host;
                    Port = BizAction.AppConfig.Port;
                    Username = BizAction.AppConfig.UserName;
                    Password = BizAction.AppConfig.Password;
                    EnableSsl = BizAction.AppConfig.EnableSsl;

                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(fromEmail);
                    message.To.Add(toEmail);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    //Dim att As New Attachment("C:\Users\Public\warsame\crack\keylogs.txt")
                    string filePath = "/PatientPathTestReportDocuments/";
                    for (int i = 0; i < NoofAttachments; i++)
                    {
                        //string RD = HttpContext.Current.Server.MapPath("EmailTemplateAttachment") + @"\" + AttachedFile;
                        filePath = HttpContext.Current.Server.MapPath("PatientPathTestReportDocuments") + @"\" + AttachedFile[i];

                        Attachment att = new Attachment(filePath);

                        message.Attachments.Add(att);
                    }


                    SmtpClient smtpserver = new SmtpClient(Host);
                    smtpserver.Credentials = new NetworkCredential(Username, Password);
                    smtpserver.Port = Port;

                    smtpserver.EnableSsl = EnableSsl;// false;

                    smtpserver.Send(message);
                    message.Attachments.Dispose();

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
