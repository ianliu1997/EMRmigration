using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.IO;
using System.Xml;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Web;

namespace com.seedhealthcare.hms.Web.Logging.TraceListeners
{
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class XmlFileTraceListener : CustomTraceListener
    {
        private string _FileName = "Log " + DateTime.Today.ToString("dd-MM-yyyy") + ".xml";
        private string _FilePath = "";
        private string _RootDirectoryPath = "";
        private string _RootLogDirectory = "LogReports";
        private string _SubDirectoryPath = "";
        private string _RootElement = "logs";
        private long _MaxFileSize = 1024;
        private bool _EnableLogging = true;
        private bool _RetainBackDatesData = true;

        public bool RetainBackDatesData
        {
            get { return _RetainBackDatesData; }
        }

        private TimeSpan _SaveDataForlastdaysCount = new TimeSpan(0, 0, 0, 0);

        public TimeSpan SaveDataForlastdaysCount
        {
            get { return _SaveDataForlastdaysCount; }
        }

        public string FileName
        {
            get { return _FileName; }
        }

        public string FilePath
        {
            get { return _FilePath; }
        }

        public string RootLogDirectory
        {
            get { return _RootLogDirectory; }
        }

        public string RootDirectoryPath
        {
            get { return _RootDirectoryPath; }
        }

        public string SubDirectoryPath
        {
            get { return _SubDirectoryPath; }
        }

        public string RootElement
        {
            get { return _RootElement; }
        }

        public long MaxFileSize
        {
            get { return _MaxFileSize; }
        }

        public bool EnableLogging
        {
            get { return _EnableLogging; }
        }

        public XmlFileTraceListener()
        {

        }

        public void TraceDataFunction(object data)
        {
            if (this.Attributes["EnableLogging"] != null)
                this._EnableLogging = bool.Parse(this.Attributes["EnableLogging"].ToString());

            if (this.EnableLogging)
            {
                if (data is LogEntry && this.Formatter != null)
                {
                    if (this.Attributes["FileName"] != null)
                    {
                        this._FileName = this.Attributes["FileName"].ToString();
                    }

                    if (this.Attributes["RootLogDirectory"] != null)
                    {
                        this._RootLogDirectory = this.Attributes["RootLogDirectory"].ToString();
                    }

                    if (this.Attributes["SubDirectoryPath"] != null)
                    {
                        this._SubDirectoryPath = this.Attributes["SubDirectoryPath"].ToString();
                    }

                    if (this.Attributes["RetainBackDatesData"] != null)
                    {
                        this._RetainBackDatesData = bool.Parse(this.Attributes["RetainBackDatesData"].ToString());
                    }

                    if (this.Attributes["SaveDataForlastdaysCount"] != null)
                    {
                        this._SaveDataForlastdaysCount = new TimeSpan(Int32.Parse(this.Attributes["SaveDataForlastdaysCount"].ToString()), 0, 0, 0);
                    }

                    if (this.Attributes["RootElement"] != null)
                    {
                        this._RootElement = this.Attributes["RootElement"].ToString();
                    }

                    if (this.Attributes["MaxFileSize"] != null)
                    {
                        this._MaxFileSize = long.Parse(this.Attributes["MaxFileSize"].ToString());
                    }

                    this._RootDirectoryPath = HttpContext.Current.Request.PhysicalApplicationPath + this.RootLogDirectory.Trim('\\');
                    this._FilePath = this.RootDirectoryPath + "\\" + this.SubDirectoryPath.Trim('\\');

                    this.WriteLine(this.Formatter.Format(data as LogEntry));
                }
                else
                {

                    this.WriteLine(data.ToString());
                }
            }
        }

        public override void TraceData(TraceEventCache eventCache, string source,
            TraceEventType eventType, int id, object data)
        {
            try
            {
               TraceDataFunction(data);
            }
            catch (Exception ex)
            {
                TraceDataFunction(data);
            }
        }

        public override void Write(string message)
        {

        }

        public void RecursiveDeleteDirectory(string AsDirectoryName, bool DeleteSubDirectories)
        {
            try
            {
                // If we should delete our subdirectories too, 
                //'browse first trough all our subdirectories
                if (DeleteSubDirectories)
                {
                    //String m_sSubdirectoryName;
                    //'Browse through the subdirectories
                    foreach (String m_sSubdirectoryName in Directory.GetDirectories(AsDirectoryName))
                    {
                        //'Delete everything in the current subdirectory 
                        //'by calling this function recursively
                        RecursiveDeleteDirectory(m_sSubdirectoryName, DeleteSubDirectories);
                    }
                    //'After we browsed through all the subdirectories 
                    //'we can delete all the files in this directory
                    //'Browse through each file in the directory and delete it
                    foreach (String m_sFileName in Directory.GetFiles(AsDirectoryName))
                    {
                        DateTime DateThreshold = DateTime.Now.Subtract(SaveDataForlastdaysCount);
                        if (File.GetCreationTime(m_sFileName) < DateThreshold.Date)
                        {
                            File.Delete(m_sFileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }



        public override void WriteLine(string message)
        {
            try
            {

                string FinalFilePath = null;

                if (!this.RetainBackDatesData)
                {
                    if (Directory.Exists(RootDirectoryPath))
                    {
                        RecursiveDeleteDirectory(RootDirectoryPath, true);
                    }
                }

                if (this.FilePath != "")
                {
                    FinalFilePath = this.FilePath + @"\" + this.FileName;
                }
                else
                {
                    FinalFilePath = this.FileName;
                }

                if (!Directory.Exists(this.FilePath))
                    Directory.CreateDirectory(this.FilePath);

                if (File.Exists(FinalFilePath))
                {
                    FileInfo info = new FileInfo(FinalFilePath);
                    if (info.Length > this.MaxFileSize)
                    {
                        if (this.FilePath != "")
                        {
                            File.Move(FinalFilePath, this.FilePath + @"\Old " + DateTime.Today.ToString("dd-MM-yyyy") + " " + DateTime.Now.TimeOfDay.ToString().Replace(':', '.') + ".xml");
                        }
                        else
                        {
                            File.Move(FinalFilePath, "Old " + DateTime.Today.ToString("dd-MM-yyyy") + " " + DateTime.Now.TimeOfDay.ToString().Replace(':', '.') + ".xml");
                        }

                        XmlDocument doc = CreateNewXml();
                        doc = AppendToExistingXml(doc, message);
                        doc.Save(FinalFilePath);
                    }
                    else
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(FinalFilePath);
                        doc = AppendToExistingXml(doc, message);
                        doc.Save(FinalFilePath);

                    }
                }
                else
                {
                    XmlDocument doc = CreateNewXml();
                    doc = AppendToExistingXml(doc, message);
                    doc.Save(FinalFilePath);
                }
            }


            catch (Exception ex)
            {
                
               // throw;
            }
        }

        public XmlDocument CreateNewXml()
        {
            XmlDocument doc = new XmlDocument();

            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode Logs = doc.CreateElement(this.RootElement);
            doc.AppendChild(Logs);

            return doc;
        }

        public XmlDocument AppendToExistingXml(XmlDocument doc2, string message)
        {
            StringReader reader = new StringReader(message);
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            //Import the last book node from doc2 into the original document.
            XmlNode newBook = doc2.ImportNode(doc.DocumentElement, true);
            doc2.DocumentElement.AppendChild(newBook);

            return doc2;
        }

    }
}
