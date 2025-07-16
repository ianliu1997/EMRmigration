using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Xml;
// TODO: Add Enterprise Library namespaces
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using System.Text;

namespace com.seedhealthcare.hms.Web.Logging.Formatters
{
    [ConfigurationElementType(typeof(CustomFormatterData))]
    public class TextFileFormatter : LogFormatter
    {
        private NameValueCollection Attributes = null;
        string UserId = "", MethodeName = "", ClassName = "";

        public TextFileFormatter(NameValueCollection attributes)
        {
            this.Attributes = attributes;
        }

        public override string Format(LogEntry log)
        {
            StringBuilder LogEntire = new StringBuilder();

            try
            {
                if (log.ExtendedProperties["UserId"] != null)
                    UserId = log.ExtendedProperties["UserId"].ToString();
                if (log.ExtendedProperties["MethodeName"] != null)
                    MethodeName = log.ExtendedProperties["MethodeName"].ToString();
                if (log.ExtendedProperties["ClassName"] != null)
                    ClassName = log.ExtendedProperties["ClassName"].ToString();


                LogEntire.AppendLine("\r\n--------------------------------------------\r");
                LogEntire.AppendLine("EventId       : " + log.ActivityId.ToString() + "\r");
                LogEntire.AppendLine("UserId       : " + UserId + "\r");
                LogEntire.AppendLine("Timestamp    : " + DateTime.Now + "\r");
                LogEntire.AppendLine("ClassName    : " + ClassName + "\r");
                LogEntire.AppendLine("MethodeName  : " + MethodeName + "\r");
                LogEntire.AppendLine("Message      : " + log.Message + "\r");
                LogEntire.AppendLine("Machine      : " + log.MachineName.ToString() + "\r");
                LogEntire.AppendLine("--------------------------------------------\r");
            }
            catch (Exception ex)
            {
                
                throw;
            }
            return LogEntire.ToString();
        }
    }
}