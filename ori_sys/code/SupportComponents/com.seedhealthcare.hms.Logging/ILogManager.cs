using System;
namespace com.seedhealthcare.hms.Logging
{
    public enum LogCategory
    {
        Info,
        Error,
        Warning,
        Debug
    }

    interface ILogManager
    {
        //this interface is of logmanager class.

        //WriteLog is genric method for writing logentry
        void WriteLog(Guid ActivityId, LogCategory Category, long UserId, DateTime TimeStamp, string ClassName, string MethodName, string Message);

        //LogDebug is method for writing log entry with LogCategory as Debug
        void LogDebug(Guid ActivityId, long UserId, DateTime TimeStamp, string ClassName, string MethodName, string Message);

        //LogDebug is method for writing log entry with LogCategory as Debug
        void LogError(long UserId, DateTime TimeStamp, string ClassName, string MethodName, string Message);

        //LogDebug is method for writing log entry with LogCategory as Debug
        void LogInfo(Guid ActivityId, long UserId, DateTime TimeStamp, string ClassName, string MethodName, string Message);

        //LogDebug is method for writing log entry with LogCategory as Debug
        void LogWarning(Guid ActivityId, long UserId, DateTime TimeStamp, string ClassName, string MethodName, string Message);
    }
}
