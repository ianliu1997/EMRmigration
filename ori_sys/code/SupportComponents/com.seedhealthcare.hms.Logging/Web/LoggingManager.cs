using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using System.Xml;
using System.Web;
using com.seedhealthcare.hms.Logging;

namespace com.seedhealthcare.hms.Web.Logging
{
    public class LogManager : ILogManager
    {
        private FileConfigurationSource source = null;
        private LogWriterFactory factory = null;
        private LogWriter Writer = null;
        private string LogConfigFile = string.Empty;
        private int LoggingMode = 0;
        //Passing Path as a Argunment.Application Configuration, where the log file will be created.
        private LogManager()
        {
            string key = "LogFilePath";
            try
            {
                if (factory == null)
                {

                    if (int.TryParse(System.Configuration.ConfigurationManager.AppSettings["LoggingMode"].ToString(), out LoggingMode) && LoggingMode == 1)
                    {
                        LogConfigFile = System.Configuration.ConfigurationManager.AppSettings[key].ToString();
                        this.source = new FileConfigurationSource(LogConfigFile);
                        this.factory = new LogWriterFactory(source);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static LogManager _instance = null;
        private static object syncRoot = new Object();

        static public LogManager GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        _instance = new LogManager();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        private LogEntry CreateLogEntry(Guid ActivityId, LogCategory Category, long UserId, DateTime TimeStamp, string ClassName, string MethodName, string Message)
        {
            LogEntry log = new LogEntry();
            try
            {
                log.ActivityId = ActivityId;
                log.TimeStamp = TimeStamp;
                log.ExtendedProperties["UserId"] = UserId;
                log.Categories.Add(Category.ToString());
                log.ExtendedProperties["ClassName"] = ClassName;
                log.ExtendedProperties["MethodeName"] = MethodName;
                log.Message = Message;
            }
            catch (Exception ex)
            {
                throw;
            }
            return log;
        }

       
        #region ILogManager Members

        public void WriteLog(Guid ActivityId, LogCategory Category, long UserId, DateTime TimeStamp, string ClassName, string MethodName, string Message)
        {

            try
            {
                if (LoggingMode == 1)
                {
                    LogEntry logEntry = CreateLogEntry(ActivityId, Category, UserId, TimeStamp, ClassName, MethodName, Message);
                    this.Writer = factory.Create();
                    this.Writer.Write(logEntry);
                    this.Writer.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void LogDebug(Guid ActivityId, long UserId, DateTime TimeStamp, string ClassName, string MethodName, string Message)
        {

            try
            {
                if (LoggingMode == 1)
                {
                    LogEntry logEntry = CreateLogEntry(ActivityId, LogCategory.Debug, UserId, TimeStamp, ClassName, MethodName, Message);
                    this.Writer = factory.Create();
                    this.Writer.Write(logEntry);
                    this.Writer.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void LogError( long UserId, DateTime TimeStamp, string ClassName, string MethodName, string Message)
        {

            try
            {
                if (LoggingMode == 1)
                {
                    Guid ActivityId = new Guid();
                    LogEntry logEntry = CreateLogEntry(ActivityId, LogCategory.Error, UserId, TimeStamp, ClassName, MethodName, Message);
                    this.Writer = factory.Create();
                    this.Writer.Write(logEntry);
                    this.Writer.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void LogInfo(Guid ActivityId, long UserId, DateTime TimeStamp, string ClassName, string MethodName, string Message)
        {
            try
            {
                if (LoggingMode == 1)
                {
                    LogEntry logEntry = CreateLogEntry(ActivityId, LogCategory.Info, UserId, TimeStamp, ClassName, MethodName, Message);
                    this.Writer = factory.Create();
                    this.Writer.Write(logEntry);
                    this.Writer.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void LogWarning(Guid ActivityId, long UserId, DateTime TimeStamp, string ClassName, string MethodName, string Message)
        {
            try
            {
                if (LoggingMode == 1)
                {
                    LogEntry logEntry = CreateLogEntry(ActivityId, LogCategory.Warning, UserId, TimeStamp, ClassName, MethodName, Message);
                    this.Writer = factory.Create();
                    this.Writer.Write(logEntry);
                    this.Writer.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
