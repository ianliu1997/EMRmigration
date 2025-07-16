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

namespace com.seedhealthcare.hms.Web.Logging.Formatters
{
    [ConfigurationElementType(typeof(CustomFormatterData))]
    public class XmlFileTraceListenerFormatter : LogFormatter
    {
        private NameValueCollection Attributes = null;
        string UserId = "", MethodeName = "", ClassName = "", EventId = "";

        public XmlFileTraceListenerFormatter(NameValueCollection attributes)
        {
            this.Attributes = attributes;
        }

        public override string Format(LogEntry log)
        {
            string prefix = this.Attributes["prefix"];
            string ns = this.Attributes["namespace"];

            if (log.ExtendedProperties["UserId"] != null)
                UserId = log.ExtendedProperties["UserId"].ToString();
            if (log.ExtendedProperties["MethodeName"] != null)
                MethodeName = log.ExtendedProperties["MethodeName"].ToString();
            if (log.ExtendedProperties["ClassName"] != null)
                ClassName = log.ExtendedProperties["ClassName"].ToString();
            //if (log.ExtendedProperties["EventId"] != null)
            //    ClassName = log.ExtendedProperties["ClassName"].ToString();

            using (StringWriter s = new StringWriter())
            {
                XmlTextWriter w = new XmlTextWriter(s);
                w.Formatting = Formatting.Indented;
                w.Indentation = 2;
                // w.WriteStartDocument(true);
                w.WriteStartElement("logEntry", ns);
                //w.WriteAttributeString("EventId", EventId);
                w.WriteElementString("EventId", ns, log.ActivityId.ToString());
                w.WriteElementString("UserId", ns, UserId.ToString());
                w.WriteElementString("Timestamp", ns, log.TimeStampString);
                w.WriteElementString("ClassName", ns, ClassName);
                w.WriteElementString("MethodeName", ns, MethodeName);
                w.WriteElementString("Message", ns, log.Message);
                w.WriteElementString("Machine", ns, log.MachineName);
                w.WriteEndElement();
                //w.WriteEndDocument();
                return s.ToString();
            }
        }
    }
}