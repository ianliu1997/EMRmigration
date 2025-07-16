using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using WcfExceptionExample.Web.Behavior;
using WcfExceptionExample.Web.DataContracts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Data.Common;


using System.Data;
using PalashDynamics.Web.EMR;


namespace PalashDynamics.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [WcfErrorBehavior]
    [WcfSilverlightFaultBehavior]
    public class DataTemplateService
    {
        [OperationContract]
        public void DoWork()
        {
            // Add your operation implementation here
            return;
        }

        // Add more operations here and mark them with [OperationContract]

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public int SaveTemplate(EMRTemplate template)
        {
            DataDrivenModelDataContext c = new DataDrivenModelDataContext();
            c.EMRTemplates.InsertOnSubmit(template);
            c.SubmitChanges();
            return 0;
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public int UpdateTemplate(EMRTemplate template)
        {
            DataDrivenModelDataContext c = new DataDrivenModelDataContext();
            var TempaleQ = c.EMRTemplates.Single(e => e.TemplateId == template.TemplateId);
            TempaleQ.Template = template.Template;
            TempaleQ.Description = template.Description;
            TempaleQ.Title = template.Title;
            c.SubmitChanges();
            return 0;
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public int DeleteTemplate(EMRTemplate template)
        {
            DataDrivenModelDataContext c = new DataDrivenModelDataContext();
            var TempaleQ = c.EMRTemplates.Single(e => e.TemplateId == template.TemplateId);
            c.EMRTemplates.DeleteOnSubmit(TempaleQ);
            c.SubmitChanges();
            return 0;
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public EMRTemplate GetTemplatesDetailsByTemplateTitle(long TemplateId)
        {
            EMRTemplate result = null;

            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString))
            {
                cn.Open();
                const String SQL = "SELECT * FROM EMRTemplate WHERE TemplateId=@p1";

                using (SqlCommand cmd = new SqlCommand(SQL, cn))
                {
                    SqlParameter p1 = cmd.Parameters.Add("@p1", System.Data.SqlDbType.BigInt);
                    p1.Value = TemplateId;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                            result = new EMRTemplate() { TemplateId = (long)dr[0], Title = (string)dr[1], Description = (string)dr[2], Template = (string)dr[3] };
                    }
                }
                cn.Close();
            }
            return result;
        }

        // Add more operations here and mark them with [OperationContract]


        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public List<EMRTemplate> GetTemplatesList()
        {
            DataDrivenModelDataContext c = new DataDrivenModelDataContext();
            return c.EMRTemplates.ToList();
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public List<IntraTemplateRelation> GetRelationList()
        {
            return new List<IntraTemplateRelation>();
        }

        //[OperationContract]
        //[FaultContract(typeof(ConcurrencyException))]
        //public List<Patient> GetPatientList()
        //{
        //    DataDrivenModelDataContext c = new DataDrivenModelDataContext();
        //    return c.Patients.ToList();
        //}


        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public int SavePatientEMR(PatientEMRData PatientEMR)
        {
            DataDrivenModelDataContext c = new DataDrivenModelDataContext();
            c.PatientEMRDatas.InsertOnSubmit(PatientEMR);
            c.SubmitChanges();
            return 0;
        }


       

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public int SavePatientEMR1(PatientEMRData PatientEMRData)
        {
            DataDrivenModelDataContext c = new DataDrivenModelDataContext();
            c.PatientEMRDatas.InsertOnSubmit(PatientEMRData);
            c.SubmitChanges();
            return 0;
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public int UpdatePatientEMR(PatientEMRData PatientEMR)
        {
            DataDrivenModelDataContext c = new DataDrivenModelDataContext();
            var TempaleQ = c.PatientEMRDatas.Single(e => (e.TemplateId == PatientEMR.TemplateId && e.PatientId == PatientEMR.PatientId && e.VisitId == PatientEMR.VisitId));
            TempaleQ.Template = PatientEMR.Template;
            c.SubmitChanges();
            return 0;
            //Database dbServer = HMSConfigurationManager.GetDatabaseReference();

            //DbCommand command = dbServer.GetStoredProcCommand("CIMS_TempPatientEMRUpdate");

            //dbServer.AddInParameter(command, "TemplateId", DbType.Int64, PatientEMR.TemplateId);
            //dbServer.AddInParameter(command, "PatientId", DbType.Int64, PatientEMR.PatientId);
            //dbServer.AddInParameter(command, "VisitId", DbType.Int64, PatientEMR.VisitId);
            //dbServer.AddInParameter(command, "Template", DbType.String, PatientEMR.Template);
            //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

            //int intStatus = dbServer.ExecuteNonQuery(command);
            //int a= (int)dbServer.GetParameterValue(command, "ResultStatus");
            //return a;
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public PatientEMRData GetPatientEMRByPatientIdAndTemplateId(long PatientId, long TemplateId,long VisitId)
        {
            PatientEMRData TempaleQ = null;
            try
            {
                DataDrivenModelDataContext c = new DataDrivenModelDataContext();
                TempaleQ = c.PatientEMRDatas.Single(e => (e.PatientId == PatientId && e.TemplateId == TemplateId && e.VisitId==VisitId));
            }
            catch (Exception ex)
            {
                //throw;
            }
            return TempaleQ;
        }

        //[OperationContract]
        //[FaultContract(typeof(ConcurrencyException))]
        //public PatientEMRData GetPatientEMRByPatientIdAndTemplateId(long PatientId, long TemplateId)
        //{
        //    PatientEMRData TempaleQ = null;
        //    try
        //    {
        //        DataDrivenModelDataContext c = new DataDrivenModelDataContext();
        //        TempaleQ = c.PatientEMRDatas.Single(e => (e.PatientId == PatientId && e.TemplateId == TemplateId));
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw;
        //    }
        //    return TempaleQ;
        //}


        private string filePath;

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public string[] GetFileList()
        {
            filePath = HttpContext.Current.Server.MapPath("Images");
            // Scan the folder for files.
            string[] files = Directory.GetFiles(filePath);
            // Trim out path information.
            for (int i = 0; i < files.Count(); i++)
            {
                files[i] = Path.GetFileName(files[i]);
            }
            // Return the file list.
            return files;
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public string GetFilePath()
        {
            string file = HttpContext.Current.Server.MapPath("Images");

            return file;
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void UploadFile(string fileName, byte[] data)
        {
            filePath = HttpContext.Current.Server.MapPath("Images");
            // Make sure the filename has no path information.
            string file = Path.Combine(filePath, Path.GetFileName(fileName));
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                fs.Write(data, 0, (int)data.Length);
            }
        }

        #region Newly added


        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void DeletePathReportFile(string fileName)
        {
            filePath = HttpContext.Current.Server.MapPath("../PatientPathTestReportDocuments");
            string file = Path.Combine(filePath, Path.GetFileName(fileName));
            File.Delete(file);
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void UploadReportFileForPathology(string fileName, byte[] data)
        {
            filePath = HttpContext.Current.Server.MapPath("../PatientPathTestReportDocuments");
            string file = Path.Combine(filePath, Path.GetFileName(fileName));
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                fs.Write(data, 0, (int)data.Length);
            }
        }

        #endregion



        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void AttachmentFile(string fileName, byte[] data)
        {
            filePath = HttpContext.Current.Server.MapPath("../EmailTemplateAttachment");
            // Make sure the filename has no path information.
            string file = Path.Combine(filePath, Path.GetFileName(fileName));
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                fs.Write(data, 0, (int)data.Length);
            }
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void DeleteAttachmentFile(string fileName)
        {
            filePath = HttpContext.Current.Server.MapPath("../EmailTemplateAttachment");
            // Make sure the filename has no path information.
            //for (int i = 0; i < fileName.Count; i++)
            //{
            //    string file = Path.Combine(filePath, Path.GetFileName(fileName[i]));
            //    File.Delete(file);
            //}
            string file = Path.Combine(filePath, Path.GetFileName(fileName));
            File.Delete(file);            
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public string  ActivationFile(string FileData)
        {
            string SuccessStatus;
            try
            {
                //create and write.

                filePath = HttpContext.Current.Server.MapPath("ActivationKey");
                string file = Path.Combine(filePath, Path.GetFileName("Activation.txt"));
                StreamWriter sw = new StreamWriter(file);

                //Write a line of text
                sw.WriteLine(FileData);

                //Close the file
                sw.Close();
                SuccessStatus = "Successful";
                return SuccessStatus;
            }
            catch (Exception ex)
            {
                SuccessStatus = "Error";
                return SuccessStatus;
            }            
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public string GetApplicationPath()
        {
            try
            {
                // read the file.
                filePath = HttpContext.Current.Server.MapPath("../PatientPathTestReportDocuments");
                //string file = Path.Combine(filePath, Path.GetFileName(FileName));
                //StreamReader sr = new StreamReader(file);
                //string s;
                //s = sr.ReadLine();
                //sr.Close();

                return filePath;
            }
            catch (Exception ex)
            {
               // string error = "Could not map path";
                return ex.Message;
                //throw;
            }
        }              


        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public string  GetActivationFile(string FileName)
        {
            try
            {
                // read the file.
                filePath = HttpContext.Current.Server.MapPath("ActivationKey");
                string file = Path.Combine(filePath, Path.GetFileName(FileName));
                StreamReader sr = new StreamReader(file);
                string s;
                s = sr.ReadLine();
                sr.Close();

                return s;
            }
            catch (Exception ex)
            {
                string error = "File Does Not Exist.";
                return error;
                //throw;
            }
        }              

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void UploadReportFile(string fileName, byte[] data)
        {
            filePath = HttpContext.Current.Server.MapPath("../PatientPathTestReportDocuments");
            // Make sure the filename has no path information.
            string file = Path.Combine(filePath, Path.GetFileName(fileName));
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                fs.Write(data, 0, (int)data.Length);
            }
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void UploadReportFileForRadiology(string fileName, byte[] data)
        {
            filePath = HttpContext.Current.Server.MapPath("../PatientRadTestReportDocuments");
            // Make sure the filename has no path information.
           // string file = Path.Combine(filePath, Path.GetFileName(fileName));Addded By Yogesh K
            string file = Path.Combine(filePath,fileName);
            using (FileStream fs = new FileStream(file, FileMode.CreateNew))
            {
                fs.Write(data, 0, (int)data.Length);
            }
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void UploadReportFileForDischarge(string fileName, byte[] data)
        {
            filePath = HttpContext.Current.Server.MapPath("../PatientDischargeTempReportDocuments");
            // Make sure the filename has no path information.
            string file = Path.Combine(filePath, Path.GetFileName(fileName));
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                fs.Write(data, 0, (int)data.Length);
            }
        }


        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void UploadReportFileForConsent(string fileName, byte[] data)
        {
            filePath = HttpContext.Current.Server.MapPath("../PatientConsentReportDocuments");
            // Make sure the filename has no path information.
            string file = Path.Combine(filePath, Path.GetFileName(fileName));
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                fs.Write(data, 0, (int)data.Length);
            }
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void UploadReportFileForMLCase(string fileName, byte[] data)
        {
            filePath = HttpContext.Current.Server.MapPath("../PatientMlCaseDocuments");
            // Make sure the filename has no path information.
            string file = Path.Combine(filePath, Path.GetFileName(fileName));
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                fs.Write(data, 0, (int)data.Length);
            }
        }


        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void GlobalUploadFile(string RelativePath,string fileName, byte[] data)
        {
            filePath = HttpContext.Current.Server.MapPath(RelativePath);
            // Make sure the filename has no path information.
            string file = Path.Combine(filePath, Path.GetFileName(fileName));
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                fs.Write(data, 0, (int)data.Length);
            }
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void GlobalDeleteFile(string RelativePath, List<string> fileName)
        {
            filePath = HttpContext.Current.Server.MapPath(RelativePath);
            // Make sure the filename has no path information.
            for (int i = 0; i < fileName.Count; i++)
            {
                string file = Path.Combine(filePath, Path.GetFileName(fileName[i]));
                File.Delete(file);
            }
        }


        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void UploadETemplateFile(string fileName, byte[] data)
        {
            filePath = HttpContext.Current.Server.MapPath("../EmailTemplateAttachment");
            // Make sure the filename has no path information.
            string file = Path.Combine(filePath, Path.GetFileName(fileName));
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                fs.Write(data, 0, (int)data.Length);
            }
        }
        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void DeleteReportFile(List<string> fileName)
        {
            filePath = HttpContext.Current.Server.MapPath("../PatientPathTestReportDocuments");
            // Make sure the filename has no path information.
            for (int i = 0; i < fileName.Count; i++)
            {
                string file = Path.Combine(filePath, Path.GetFileName(fileName[i]));
                File.Delete(file);
            }
        }
        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void DeleteMISReportFile(String FolderPath)
        {
            filePath = HttpContext.Current.Server.MapPath("../" + FolderPath);
            // Make sure the filename has no path information.
            foreach (string afile in Directory.GetFiles(filePath,"*.xls")) //if extension required
            {
                 File.Delete(afile);
               //delete all files..
            }
            foreach (string afile in Directory.GetFiles(filePath, "*.xlsx")) //if extension required
            {
                File.Delete(afile);
                //delete all files..
            }
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public int SaveVariance(Variance variance)
        {
            DataDrivenModelDataContext c = new DataDrivenModelDataContext();
            c.Variances.InsertOnSubmit(variance);
            c.SubmitChanges();
            return 0;
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public List<Variance> GetVarinceList()
        {
            DataDrivenModelDataContext c = new DataDrivenModelDataContext();
            return c.Variances.ToList();
        }


        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public List<PatientEMRData> GetPatientEMRList(long PatientId)
        {
            DataDrivenModelDataContext c = new DataDrivenModelDataContext();
            
            var setA = from data in c.PatientEMRDatas
                       where (data.PatientId == PatientId)
                       select data;
            return setA.ToList();
            //return ((List<PatientEMRData>)(c.PatientEMRDatas.ToList().Where(f => (f.PatientId == PatientId))));
        }

        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public byte[] OpenCreatedRadReport(string fileName)
        {
            //filePath = HttpContext.Current.Server.MapPath("../PatientPathTestReportDocuments");
            // Make sure the filename has no path information.
            filePath = HttpContext.Current.Server.MapPath("../PatientRadTestReportDocuments");
            // Make sure the filename has no path information.
            string file = filePath + "\\" + fileName;
            //filePath = ConfigurationManager.AppSettings["EmrateIdImageFolderPath"].ToString();
            //string file = filePath + "\\" + fileName;
            FileInfo imgFile = new FileInfo(file);
            if (!imgFile.Exists)
                return null;
            Stream stream = imgFile.OpenRead();
            BinaryReader binaryReader = new BinaryReader(stream);
            byte[] currentImageInBytes = new byte[0];
            currentImageInBytes = binaryReader.ReadBytes((int)stream.Length);
            return currentImageInBytes;
        }



        [OperationContract]
        [FaultContract(typeof(ConcurrencyException))]
        public void UploadSOPFile(string fileName, byte[] data)
        {
            filePath = HttpContext.Current.Server.MapPath("../SOP");
            // Make sure the filename has no path information.
            string file = Path.Combine(filePath, Path.GetFileName(fileName));
            using (FileStream fs = new FileStream(file, FileMode.Create))
            {
                fs.Write(data, 0, (int)data.Length);
            }
        }

        // Add more operations here and mark them with [OperationContract]

    }
}
