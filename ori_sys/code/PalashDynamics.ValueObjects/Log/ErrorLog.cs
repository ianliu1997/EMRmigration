using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using com.seedhealthcare.hms.Web.ConfigurationManager;


namespace PalashDynamics.ErrorLog
{
    public class PalashLogManager
    {

        public PalashLogManager()
     {
         errorDt = DateTime.Now;
     }

     private static string File_Path()
     {

        // string filePath =   System.Configuration.ConfigurationManager.AppSettings["ERRORLOG"];
         string filePath = "C:\\PalashError.log";

         return filePath;
     }
        
     private DateTime errorDt;   
       
     private Exception errorInfo;   
   
     public DateTime ErrorDate 
        {     
            get {return errorDt; }    
            set { errorDt = value;}    
        }   

     public Exception ErrorInformation 
        {        
            get { return errorInfo; }   
            set { errorInfo = value; }  
        }     
        
     public static void LogError(Exception Ex) 
        {
            PalashLogManager errInfo = new PalashLogManager();    
            errInfo.ErrorDate = System.DateTime.Now;        
            errInfo.ErrorInformation = Ex;
            PalashLogManager.LogError(errInfo); 
        }

     public static void LogError(PalashLogManager errorDTO)   
        {      
            try   
            {
                string path = File_Path();

               // FileInfo i = new FileInfo(path);

               /// string directoryPath = i.DirectoryName;  // HttpContext.Current.Server.MapPath("Configuration") + "\\Logging" ;
                //string filePath = HttpContext.Current.Server.MapPath("Configuration") + HMSConfigurationManager.GetErrorLogFilePath();

                if (!string.IsNullOrEmpty(path))  
                {               
                    //string path = directoryPath + "\\" + "PalashError.log";       
                    StreamWriter swErrorLog = null;      
                   // DirectoryInfo dtDirectory = null;
                   //FileStream fStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                    //if (!Directory.Exists(directoryPath))           
                    //{               
                    //    dtDirectory = Directory.CreateDirectory(directoryPath);      
                    //    dtDirectory = null;     
                    //}
                    if (!File.Exists(path))
                    {
                        FileStream fStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        swErrorLog = new StreamWriter(fStream);                                    
                        swErrorLog.Close();
                        fStream.Close();

                    }              
                  
                    //append the error message    
                    FileStream fsumpStream = new FileStream(path, FileMode.Append, FileAccess.Write);
                    swErrorLog = new StreamWriter(fsumpStream);

                    swErrorLog.WriteLine("Date and Time: " + errorDTO.ErrorDate);
                    //swErrorLog.WriteLine("Source : " + errorDTO.ErrorInformation.TargetSite);
                    swErrorLog.WriteLine("Message: " + errorDTO.ErrorInformation.Message);
                    swErrorLog.WriteLine("Source : " + errorDTO.ErrorInformation.StackTrace);
                    swErrorLog.WriteLine("Inner Exception: " + errorDTO.ErrorInformation.InnerException);
                    swErrorLog.WriteLine("=========================================================================");
                    swErrorLog.WriteLine(" ");
                   // swErrorLog.WriteLine(System.Security.Principal.WindowsIdentity.GetCurrent().Name);        
                    swErrorLog.Close();
                    fsumpStream.Close();
                    swErrorLog = null;      
                }      
            }       
            catch (NullReferenceException)   
            {        
                throw;   
            }   
        }

    }
}
