using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.EMR;
using System.Data.Common;
using System.Data;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.EMR.NewEMR;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.EMR.GrowthChart;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsEMRDAL : clsBaseEMRDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;

        //added by neena
        string ImgIP = string.Empty;
        string ImgVirtualDir = string.Empty;
        string ImgSaveLocation = string.Empty;
        //

        #endregion

        private clsEMRDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }

                //Create Instance of the LogManager object 

                if (logManager == null)
                {
                    logManager = LogManager.GetInstance();
                }

                //added by neena
                ImgIP = System.Configuration.ConfigurationManager.AppSettings["EMRImgIP"];
                ImgVirtualDir = System.Configuration.ConfigurationManager.AppSettings["EMRImgVirtualDir"];
                ImgSaveLocation = System.Configuration.ConfigurationManager.AppSettings["EMRImgSavingLocation"];
                //

                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override IValueObject AddEMRTemplate(IValueObject valueObject, clsUserVO UserVo)
        {

            clsAddEMRTemplateBizActionVO BizActionObj = valueObject as clsAddEMRTemplateBizActionVO;
            try
            {
                clsEMRTemplateVO objEMRTemplateVO = BizActionObj.EMRTemplateDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddEMRTemplate");


                dbServer.AddInParameter(command, "LinkServer", DbType.String, objEMRTemplateVO.LinkServer);
                if (objEMRTemplateVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objEMRTemplateVO.LinkServer.Replace(@"\", "_"));


                dbServer.AddInParameter(command, "Title", DbType.String, objEMRTemplateVO.Title);
                dbServer.AddInParameter(command, "Description", DbType.String, objEMRTemplateVO.Description);
                dbServer.AddInParameter(command, "Template", DbType.String, objEMRTemplateVO.Template);

                dbServer.AddInParameter(command, "ApplicableCriteria", DbType.Int16, objEMRTemplateVO.ApplicableCriteria);
                dbServer.AddInParameter(command, "ApplicableToPhysicalExam", DbType.Boolean, objEMRTemplateVO.IsPhysicalExam);
                dbServer.AddInParameter(command, "ApplicableToOT", DbType.Boolean, objEMRTemplateVO.IsForOT);

                //added by rohini dated 15.1.2016
                dbServer.AddInParameter(command, "TemplateTypeID", DbType.String, objEMRTemplateVO.TemplateTypeID);
                dbServer.AddInParameter(command, "TemplateType", DbType.String, objEMRTemplateVO.TemplateType);
                dbServer.AddInParameter(command, "TemplateSubtypeID", DbType.String, objEMRTemplateVO.TemplateSubtypeID);
                dbServer.AddInParameter(command, "TemplateSubtype", DbType.String, objEMRTemplateVO.TemplateSubtype);
                //
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objEMRTemplateVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objEMRTemplateVO.TemplateID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.EMRTemplateDetails.TemplateID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
        }

        public override IValueObject UpdateEMRTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateEMRTemplateBizActionVO BizActionObj = valueObject as clsUpdateEMRTemplateBizActionVO;
            try
            {
                clsEMRTemplateVO objEMRTemplateVO = BizActionObj.EMRTemplateDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateEMRTemplate");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objEMRTemplateVO.LinkServer);
                if (objEMRTemplateVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objEMRTemplateVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Title", DbType.String, objEMRTemplateVO.Title);
                dbServer.AddInParameter(command, "Description", DbType.String, objEMRTemplateVO.Description);
                dbServer.AddInParameter(command, "Template", DbType.String, objEMRTemplateVO.Template);
                dbServer.AddInParameter(command, "IsPhysicalExam", DbType.Boolean, objEMRTemplateVO.IsPhysicalExam);
                dbServer.AddInParameter(command, "IsForOT", DbType.Boolean, objEMRTemplateVO.IsForOT);
                //added by rohini
                dbServer.AddInParameter(command, "TemplateTypeID", DbType.Int64, objEMRTemplateVO.TemplateTypeID);
                dbServer.AddInParameter(command, "TemplateType", DbType.String, objEMRTemplateVO.TemplateType);
                dbServer.AddInParameter(command, "TemplateSubtypeID", DbType.Int64, objEMRTemplateVO.TemplateSubtypeID);
                dbServer.AddInParameter(command, "TemplateSubtype", DbType.String, objEMRTemplateVO.TemplateSubtype);
                //
                dbServer.AddInParameter(command, "ID", DbType.Int64, objEMRTemplateVO.TemplateID);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objEMRTemplateVO.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objEMRTemplateVO.Status);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
        }

        public override IValueObject GetEMRTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetEMRTemplateBizActionVO BizActionObj = valueObject as clsGetEMRTemplateBizActionVO;
            try
            {
                clsEMRTemplateVO objDetailsVO = BizActionObj.objEMRTemplate;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEMRTemplate");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.TemplateID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId); //objDetailsVO.UnitId);


                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.objEMRTemplate.TemplateID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.objEMRTemplate.Title = (string)DALHelper.HandleDBNull(reader["Title"]);
                        BizActionObj.objEMRTemplate.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        BizActionObj.objEMRTemplate.Template = (string)DALHelper.HandleDBNull(reader["Template"]);

                        BizActionObj.objEMRTemplate.ApplicableCriteria = (int)DALHelper.HandleDBNull(reader["AppTo"]);
                        BizActionObj.objEMRTemplate.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        //BizActionObj.objEMRTemplate.ApplicableTo = (Int16)DALHelper.HandleDBNull(reader["AppTo"]);
                        BizActionObj.objEMRTemplate.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        BizActionObj.objEMRTemplate.CreatedUnitID = (Int64)DALHelper.HandleDBNull(reader["CreatedUnitID"]);

                        BizActionObj.objEMRTemplate.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                        BizActionObj.objEMRTemplate.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        BizActionObj.objEMRTemplate.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        BizActionObj.objEMRTemplate.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);
                        BizActionObj.objEMRTemplate.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        BizActionObj.objEMRTemplate.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        BizActionObj.objEMRTemplate.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        BizActionObj.objEMRTemplate.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);

                        //added by rohini dated 20.4.16
                        BizActionObj.objEMRTemplate.IsPhysicalExam = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPhysicalExam"]));
                        BizActionObj.objEMRTemplate.IsForOT = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsProcedure"]));

                        BizActionObj.objEMRTemplate.TemplateTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateTypeID"]));
                        BizActionObj.objEMRTemplate.TemplateSubtypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateSubtypeID"]));


                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {

                CurrentMethodExecutionStatus = false;
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return valueObject;

        }

        public override IValueObject GetEMRTemplateList(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetEMRTemplateListBizActionVO BizActionObj = valueObject as clsGetEMRTemplateListBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEMRTemplate");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "IsPhysicalExam", DbType.Boolean, BizActionObj.IsphysicalExam);

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.objEMRTemplateList == null)
                        BizActionObj.objEMRTemplateList = new List<clsEMRTemplateVO>();

                    while (reader.Read())
                    {
                        clsEMRTemplateVO objEMRTemplateVO = new clsEMRTemplateVO();

                        objEMRTemplateVO.TemplateID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objEMRTemplateVO.Title = (string)DALHelper.HandleDBNull(reader["Title"]);
                        objEMRTemplateVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objEMRTemplateVO.Template = (string)DALHelper.HandleDBNull(reader["Template"]);

                        objEMRTemplateVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        objEMRTemplateVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        objEMRTemplateVO.CreatedUnitID = (Int64)DALHelper.HandleDBNull(reader["CreatedUnitID"]);

                        objEMRTemplateVO.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                        objEMRTemplateVO.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        objEMRTemplateVO.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        objEMRTemplateVO.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);

                        objEMRTemplateVO.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        objEMRTemplateVO.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        objEMRTemplateVO.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        objEMRTemplateVO.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);

                        BizActionObj.objEMRTemplateList.Add(objEMRTemplateVO);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return valueObject;
        }

        

        public override IValueObject GetEMR_PCR_FieldList(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetEMR_PCR_FieldListBizActionVO BizActionObj = (clsGetEMR_PCR_FieldListBizActionVO)valueObject;

            try
            {
                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEMR_PCR_FieldList");

                dbServer.AddInParameter(command, "SectionID", DbType.Int64, BizActionObj.SectionID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.Status);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.PCR_FieldMasterList == null)
                    {
                        BizActionObj.PCR_FieldMasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        BizActionObj.PCR_FieldMasterList.Add(new MasterListItem((long)reader["ID"], reader["Description"].ToString()));
                    }
                }
                //if (!reader.IsClosed)
                //{
                //    reader.Close();
                //}
                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }

            return BizActionObj;
        }

        public override IValueObject GetEMR_CaseReferral_FieldList(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetEMR_CaseReferral_FieldListBizActionVO BizActionObj = (clsGetEMR_CaseReferral_FieldListBizActionVO)valueObject;

            try
            {
                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEMR_CaseReferral_FieldList");

                dbServer.AddInParameter(command, "SectionID", DbType.Int64, BizActionObj.SectionID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.Status);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.CaseReferral_FieldMasterList == null)
                    {
                        BizActionObj.CaseReferral_FieldMasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        BizActionObj.CaseReferral_FieldMasterList.Add(new MasterListItem((long)reader["ID"], reader["Description"].ToString()));
                    }
                }
                //if (!reader.IsClosed)
                //{
                //    reader.Close();
                //}
                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }

            return BizActionObj;
        }

        public override IValueObject AddUpdateReferralDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateReferralDetailsBizActionVO BizActionObj = valueObject as clsAddUpdateReferralDetailsBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            long ReferalID = 0;
            long PrescID = 0;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();
                //DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_DeleteReferralDetails");
                //dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                //dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                //dbServer.AddInParameter(command1, "DoctorCode", DbType.String, BizActionObj.DoctorCode);
                //int iStatus = dbServer.ExecuteNonQuery(command1,trans);
                foreach (var item in BizActionObj.DoctorSuggestedServiceDetail)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientEMRReferralDeatails");
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "IsOPDIPD", DbType.Int64, BizActionObj.IsOPDIPD);
                    dbServer.AddInParameter(command, "Doctorid", DbType.String, BizActionObj.DoctorID);
                    dbServer.AddInParameter(command, "SpecialiizationCode", DbType.String, item.ReferalSpecializationCode);
                    dbServer.AddInParameter(command, "SpecialiizationName", DbType.String, item.ReferalSpecialization);
                    dbServer.AddInParameter(command, "RefDocCode", DbType.String, item.ReferalDoctorCode);
                    dbServer.AddInParameter(command, "RefDocName", DbType.String, item.ReferalDoctor);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, item.Status);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                    dbServer.AddParameter(command, "PrescID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, PrescID);
                    //@IsOPDIPD
                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                    ReferalID = (long)dbServer.GetParameterValue(command, "ID");
                    PrescID = (long)dbServer.GetParameterValue(command, "PrescID");

                    if (BizActionObj.SuccessStatus != 1)
                        throw new Exception();

                    if (item.ReferralLetter != null)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddEMRReferralLetter");
                        dbServer.AddInParameter(command2, "PrescriptionID", DbType.Int64, PrescID);
                        dbServer.AddInParameter(command2, "ReferralID", DbType.Int64, ReferalID);
                        dbServer.AddInParameter(command2, "IsReferral", DbType.Boolean, item.IsRefferal);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        if (item.IsRefferal)
                        {
                            dbServer.AddInParameter(command2, "VisitDetails", DbType.String, item.ReferralLetter.VisitDetails);
                            dbServer.AddInParameter(command2, "Diagnosis", DbType.String, item.ReferralLetter.Diagnosis);
                            dbServer.AddInParameter(command2, "Treatment", DbType.String, item.ReferralLetter.Treatment);
                            dbServer.AddInParameter(command2, "ReferralType", DbType.Int16, item.ReferralLetter.ReferalType);
                        }
                        else
                        {
                            dbServer.AddInParameter(command2, "ReferralRemark", DbType.String, item.ReferralLetter.ReferralRemark);
                            dbServer.AddInParameter(command2, "ReferralTreatment", DbType.String, item.ReferralLetter.ReferralTreatment);
                            dbServer.AddInParameter(command2, "Conclusion", DbType.String, item.ReferralLetter.Conclusion);
                            dbServer.AddInParameter(command2, "ConsultEndDate", DbType.DateTime, item.ReferralLetter.ConsultEndDate);
                            dbServer.AddInParameter(command2, "JointCareDate", DbType.DateTime, item.ReferralLetter.JointCareDate);
                            dbServer.AddInParameter(command2, "NextConsultDate", DbType.DateTime, item.ReferralLetter.NextConsultDate);
                            dbServer.AddInParameter(command2, "TakeOverDate", DbType.DateTime, item.ReferralLetter.TakeOverDate);
                        }
                        dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, item.Status);
                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        dbServer.AddParameter(command2, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                        int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                    }
                }
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return BizActionObj;
        }

        public override IValueObject getDoctorlistonreferralasperService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorlistforReferralAsperServiceBizActionVO BizActionObj = valueObject as clsGetDoctorlistforReferralAsperServiceBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRreferralDoctorAsperServiceDetails");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, BizActionObj.DepartmentId);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceId);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }

                    while (reader.Read())
                    {
                        //       BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), true));
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), Convert.ToDouble(DALHelper.HandleDBNull((reader["Rate"])))));

                    }

                }

                reader.Close();

            }
            catch (Exception ex)
            {

                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }

            return valueObject;
        }

        public override IValueObject GetPatientReferraldetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetReferralDetailsBizActionVO BizActionObj = valueObject as clsGetReferralDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRreferralDetails");
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "doctorID", DbType.Int64, BizActionObj.DoctorID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                DbDataReader reader1 = null;
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorSuggestedServiceDetail == null)
                        BizActionObj.DoctorSuggestedServiceDetail = new List<clsDoctorSuggestedServiceDetailVO>();
                    while (reader.Read())
                    {
                        clsDoctorSuggestedServiceDetailVO Obj = new clsDoctorSuggestedServiceDetailVO();
                        Obj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferralID"]));
                        Obj.PrescriptionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrescriptionID"]));
                        Obj.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        Obj.DoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["Doctorid"]));
                        Obj.Specialization = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                        Obj.SpecializationCode = Convert.ToString(DALHelper.HandleDBNull(reader["Specializationcode"]));
                        Obj.IsRefferal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReferral"]));
                        Obj.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctorName"]));
                        Obj.PrintFlag = "Visible";
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRreferralLetter");
                        dbServer.AddInParameter(command2, "ID", DbType.Int64, Obj.ID);
                        reader1 = (DbDataReader)dbServer.ExecuteReader(command2);
                        if (reader1.HasRows)
                        {
                            while (reader1.Read())
                            {
                                clsEMRReferralLetterVO ObjRef = new clsEMRReferralLetterVO();
                                ObjRef.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader1["Date"]));
                                ObjRef.VisitDetails = Convert.ToString(DALHelper.HandleDBNull(reader1["VisitDetails"]));
                                ObjRef.Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader1["Diagnosis"]));
                                ObjRef.Treatment = Convert.ToString(DALHelper.HandleDBNull(reader1["Treatment"]));
                                ObjRef.ReferalType = Convert.ToInt16(DALHelper.HandleDBNull(reader1["ReferralType"]));
                                Obj.Specialization = Convert.ToString(DALHelper.HandleDBNull(reader1["ReferredSpeciality"]));
                                Obj.SpecializationCode = Convert.ToString(DALHelper.HandleDBNull(reader1["ReferredSpecialityCode"]));
                                Obj.ReferalSpecialization = Convert.ToString(DALHelper.HandleDBNull(reader1["ReferalSpeciality"]));
                                Obj.ReferalSpecializationCode = Convert.ToString(DALHelper.HandleDBNull(reader1["ReferalSpecialityCode"]));
                                Obj.ReferalDoctor = Convert.ToString(DALHelper.HandleDBNull(reader1["ReferalDoctor"]));
                                Obj.ReferalDoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader1["ReferalDoctorCode"]));
                                Obj.DoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader1["ReferredDoctorCode"]));
                                Obj.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader1["ReferredDoctor"]));
                                Obj.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader1["Date"]));
                                ObjRef.AckDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader1["AcknowledgedDate"]));
                                ObjRef.Conclusion = Convert.ToString(reader1["Conclusion"]);
                                ObjRef.ReferralRemark = Convert.ToString(reader1["Remark"]);
                                ObjRef.ReferralTreatment = Convert.ToString(reader1["ReferralTreatment"]);
                                ObjRef.ConsultEndDate = (DateTime?)(DALHelper.HandleDBNull(reader1["ConsultEndDate"]));
                                ObjRef.TakeOverDate = (DateTime?)(DALHelper.HandleDBNull(reader1["TakeOverDate"]));
                                ObjRef.NextConsultDate = (DateTime?)(DALHelper.HandleDBNull(reader1["NextConsultDate"]));
                                ObjRef.JointCareDate = (DateTime?)(DALHelper.HandleDBNull(reader1["JointCareDate"]));
                                ObjRef.ReferredDoctor = Obj.DoctorName;
                                ObjRef.ReferredDoctorCode = Obj.DoctorCode;
                                ObjRef.ReferalDoctorCode = Obj.ReferalDoctorCode;
                                ObjRef.ReferalDoctor = Obj.ReferalDoctor;
                                ObjRef.ReferalSpeciality = Obj.ReferalSpecialization;
                                ObjRef.ReferredSpeciality = Obj.Specialization;
                                Obj.ReferralLetter = ObjRef;
                            }
                        }
                        BizActionObj.DoctorSuggestedServiceDetail.Add(Obj);
                        reader1.Close();
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetPatientReferraldetailsListHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetReferralDetailsBizActionVO BizActionObj = valueObject as clsGetReferralDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRreferralDetailsHistory");
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "ISOPDIPD", DbType.Int64, BizActionObj.IsOPDIPD);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "Doctorid", DbType.Int64, BizActionObj.DoctorID);
                dbServer.AddInParameter(command, "@startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "@maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorSuggestedServiceDetail == null)
                        BizActionObj.DoctorSuggestedServiceDetail = new List<clsDoctorSuggestedServiceDetailVO>();
                    while (reader.Read())
                    {
                        clsDoctorSuggestedServiceDetailVO Obj = new clsDoctorSuggestedServiceDetailVO();
                        Obj.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                        //Obj.DoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctorCode"]));
                        Obj.ReferalDoctor = Convert.ToString(DALHelper.HandleDBNull(reader["ReferalDoctor"]));
                        //Obj.ReferalDoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ReferalDoctorCode"]));
                        Obj.Specialization = Convert.ToString(DALHelper.HandleDBNull(reader["Specialization"]));
                        //Obj.SpecializationCode = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationCode"]));
                        Obj.ReferalSpecialization = Convert.ToString(DALHelper.HandleDBNull(reader["ReferalSpecialization"]));
                        //Obj.ReferalSpecializationCode = Convert.ToString(DALHelper.HandleDBNull(reader["ReferalSpecializationCode"]));
                        Obj.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        BizActionObj.DoctorSuggestedServiceDetail.Add(Obj);
                    }

                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }

            return valueObject;
        }

        public override IValueObject AddUpdateDeleteDiagnosisDetailsBizAction(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateDeleteDiagnosisDetailsBizActionVO BizActionObj = valueObject as clsAddUpdateDeleteDiagnosisDetailsBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();
                if (BizActionObj.IsICD10)
                {
                    DbCommand comand = dbServer.GetStoredProcCommand("CIMS_DeleteICD10DiagnosisDeatails");
                    dbServer.AddInParameter(comand, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(comand, "VisitID", DbType.Int64, BizActionObj.VisitID);
                    dbServer.AddInParameter(comand, "Doctorid", DbType.Int32, BizActionObj.DoctorID);
                    dbServer.AddInParameter(comand, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(comand, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(comand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(comand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(comand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(comand, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                    int iStatus = dbServer.ExecuteNonQuery(comand, trans);
                    foreach (var item in BizActionObj.DiagnosisDetails)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateDeleteICD10DiagnosisDeatails");
                        dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                        dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                        dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                        dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                        dbServer.AddInParameter(command, "DoctorId", DbType.Int32, BizActionObj.DoctorID);
                        dbServer.AddInParameter(command, "Categori", DbType.String, item.Code);
                        dbServer.AddInParameter(command, "DTD", DbType.String, item.DTD);
                        dbServer.AddInParameter(command, "Class", DbType.String, item.Class);
                        dbServer.AddInParameter(command, "DiagnosisName", DbType.String, item.Diagnosis);
                        dbServer.AddInParameter(command, "DiagnosisTypeID", DbType.Int64, item.SelectedDiagnosisType.ID);
                        dbServer.AddInParameter(command, "TemplateID", DbType.Int64, item.TemplateID);
                        dbServer.AddInParameter(command, "TemplateName", DbType.String, item.TemplateName);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                        int intStatus = dbServer.ExecuteNonQuery(command, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        if (BizActionObj.SuccessStatus != 1)
                            throw new Exception();
                        BizActionObj.PatientDiagnosisID = (long)dbServer.GetParameterValue(command, "ID");

                        #region  added by sagar to add patient diagnosis emr detail
                        if (item.listPatientEMRDetails != null)
                        {
                            DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdatePatientDiagnosisDetails");
                            dbServer.AddInParameter(command2, "VisitID", DbType.Int64, BizActionObj.VisitID);
                            dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.PatientID);
                            //dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                            dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, item.TemplateID);
                            dbServer.AddInParameter(command2, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                            //dbServer.AddInParameter(command2, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                            int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");

                            foreach (var item1 in item.listPatientEMRDetails)
                            {
                                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientDiagnosisEMRDetails");

                                dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                                dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                                //dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                                dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, item1.TemplateID);
                                //dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                                dbServer.AddInParameter(command1, "ControlCaption", DbType.String, item1.ControlCaption);
                                dbServer.AddInParameter(command1, "ControlName", DbType.String, item1.ControlName);
                                dbServer.AddInParameter(command1, "ControlType", DbType.String, item1.ControlType);
                                dbServer.AddInParameter(command1, "Value", DbType.String, item1.Value);
                                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);
                                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                                intStatus = dbServer.ExecuteNonQuery(command1, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                                if (BizActionObj.SuccessStatus != 1 && BizActionObj.SuccessStatus != 2)
                                    throw new Exception();
                            }
                        }

                        if (!item.Status)
                        {
                            DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdatePatientDiagnosisDetails");
                            dbServer.AddInParameter(command2, "VisitID", DbType.Int64, BizActionObj.VisitID);
                            dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.PatientID);
                            dbServer.AddInParameter(command2, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                            //dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                            dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, item.TemplateID);
                            //dbServer.AddInParameter(command2, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                            int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);
                        }

                        #endregion
                    }
                }
                else
                {
                    DbCommand comand = dbServer.GetStoredProcCommand("CIMS_DeleteDiagnosisDeatails");
                    dbServer.AddInParameter(comand, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(comand, "VisitID", DbType.Int64, BizActionObj.VisitID);
                    dbServer.AddInParameter(comand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(comand, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(comand, "Doctorid", DbType.String, BizActionObj.DoctorID);
                    dbServer.AddInParameter(comand, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(comand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(comand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(comand, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                    int iStatus = dbServer.ExecuteNonQuery(comand, trans);

                    foreach (var item in BizActionObj.DiagnosisDetails)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateDeletePatientEMRDiagnosisDeatails");
                        dbServer.AddOutParameter(command, "ID", DbType.Int64, int.MaxValue);
                        dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                        dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                        dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                        dbServer.AddInParameter(command, "Doctorid", DbType.String, BizActionObj.DoctorID);
                        dbServer.AddInParameter(command, "Code", DbType.String, item.Code);
                        dbServer.AddInParameter(command, "Class", DbType.String, item.Class);
                        dbServer.AddInParameter(command, "ServiceCode", DbType.String, item.ServiceCode);
                        //dbServer.AddInParameter(command, "IsICOPIMHead", DbType.Boolean, item.IsICOPIMHead);
                        dbServer.AddInParameter(command, "IsICD9", DbType.Boolean, item.IsICD9);
                        dbServer.AddInParameter(command, "DiagnosisName", DbType.String, item.Diagnosis);
                        dbServer.AddInParameter(command, "DiagnosisTypeID", DbType.Int64, item.SelectedDiagnosisType.ID);
                        dbServer.AddInParameter(command, "TemplateName", DbType.String, item.TemplateName);
                        dbServer.AddInParameter(command, "TemplateID", DbType.Int64, item.TemplateID);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command, "Status", DbType.Boolean, item.Status);
                        dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                        //added by neena
                        dbServer.AddInParameter(command, "IsArt", DbType.Boolean, item.ArtEnabled);
                        dbServer.AddInParameter(command, "PlannedTreatmentId", DbType.Int64, item.PlanTreatmentId);
                        dbServer.AddInParameter(command, "IsBilled", DbType.Boolean, item.BilledEnabled);
                        dbServer.AddInParameter(command, "IsClosed", DbType.Boolean, item.IsClosedEnabled);
                        dbServer.AddInParameter(command, "PriorityId", DbType.Int64, item.PriorityId);
                        dbServer.AddInParameter(command, "IsArtStatus", DbType.Boolean, item.IsArtStatus);
                        dbServer.AddInParameter(command, "IsPAC", DbType.Boolean, item.PACEnabled);
                        dbServer.AddInParameter(command, "PlanTherapyId", DbType.Int64, item.PlanTherapyId);
                        dbServer.AddInParameter(command, "PlanTherapyUnitId", DbType.Int64, item.PlanTherapyUnitId);
                        dbServer.AddInParameter(command, "IsDonorCycle", DbType.Boolean, item.IsDonorCycle);
                        dbServer.AddInParameter(command, "DonorID", DbType.Int64, item.DonorID);
                        dbServer.AddInParameter(command, "DonarUnitID", DbType.Int64, item.DonarUnitID);
                        dbServer.AddInParameter(command, "CoupleMRNO", DbType.String, item.CoupleMRNO);
                        dbServer.AddInParameter(command, "IsSurrogacy", DbType.Boolean, item.IsSurrogate);
                        dbServer.AddInParameter(command, "SurrogateMrNo", DbType.String, item.SurrogateMRNO);
                        //
                        int intStatus = dbServer.ExecuteNonQuery(command, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        if (BizActionObj.SuccessStatus != 1)
                            throw new Exception();
                        BizActionObj.PatientDiagnosisID = (long)dbServer.GetParameterValue(command, "ID");

                        if (item.IsSurrogate && BizActionObj.objSurrogatedPatient!=null && BizActionObj.objSurrogatedPatient.Count > 0)
                        {
                            foreach (var item1 in BizActionObj.objSurrogatedPatient)
                            {
                                DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateSurrogateLinking");
                                dbServer.AddOutParameter(command1, "ID", DbType.Int64, int.MaxValue);
                                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "SurrogateID", DbType.Int64, item1.PatientID);
                                dbServer.AddInParameter(command1, "SurrogateUnitID", DbType.Int64, item1.UnitId);
                                dbServer.AddInParameter(command1, "MrNo", DbType.String, item1.MRNo);
                                dbServer.AddInParameter(command1, "PatientEMRDiagnosisID", DbType.Int64, BizActionObj.PatientDiagnosisID);
                                dbServer.AddInParameter(command1, "PatientEMRDiagnosisUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddParameter(command1, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

                                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                                //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                                //BizActionObj.PatientDiagnosisID = (long)dbServer.GetParameterValue(command1, "ID");
                            }
                        }

                        #region  added by sagar to add patient diagnosis emr detail
                        if (item.listPatientEMRDetails != null)
                        {
                            DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdatePatientDiagnosisDetails");
                            dbServer.AddInParameter(command2, "VisitID", DbType.Int64, BizActionObj.VisitID);
                            dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.PatientID);
                            dbServer.AddInParameter(comand, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                            //dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                            dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, item.TemplateID);
                            //dbServer.AddInParameter(command2, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                            int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");

                            foreach (var item1 in item.listPatientEMRDetails)
                            {
                                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientDiagnosisEMRDetails");

                                dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                                dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                                //dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                                dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, item1.TemplateID);
                                //dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                                dbServer.AddInParameter(command1, "ControlCaption", DbType.String, item1.ControlCaption);
                                dbServer.AddInParameter(command1, "ControlName", DbType.String, item1.ControlName);
                                dbServer.AddInParameter(command1, "ControlType", DbType.String, item1.ControlType);
                                dbServer.AddInParameter(command1, "Value", DbType.String, item1.Value);
                                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);
                                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                                intStatus = dbServer.ExecuteNonQuery(command1, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                                if (BizActionObj.SuccessStatus != 1 && BizActionObj.SuccessStatus != 2)
                                    throw new Exception();
                            }
                        }
                        if (!item.Status)
                        {
                            DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdatePatientDiagnosisDetails");
                            dbServer.AddInParameter(command2, "VisitID", DbType.Int64, BizActionObj.VisitID);
                            dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.PatientID);
                            dbServer.AddInParameter(comand, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                            //dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                            dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, item.TemplateID);
                            //dbServer.AddInParameter(command2, "UnitID", DbType.Int64, BizActionObj.UnitID);
                            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                            int intStatus1 = dbServer.ExecuteNonQuery(command2, trans);
                        }
                        #endregion
                    }
                }
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return BizActionObj;
        }

        public override IValueObject GetPatientEMRICDXDiagnosisList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDiagnosisDataBizActionVO BizActionObj = valueObject as clsGetPatientDiagnosisDataBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRICDXDiagnosisDetailsHistory");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                dbServer.AddInParameter(command, "Doctorid", DbType.String, BizActionObj.DoctorID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDiagnosisDetails == null)
                        BizActionObj.PatientDiagnosisDetails = new List<clsEMRAddDiagnosisVO>();
                    while (reader.Read())
                    {
                        clsEMRAddDiagnosisVO Obj = new clsEMRAddDiagnosisVO();
                        Obj.Categori = Convert.ToString(reader["Categori"]);
                        Obj.Class = Convert.ToString(reader["Class"]);
                        Obj.Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["DiagnosisName"]));
                        Obj.IsSelected = (bool)DALHelper.HandleBoolDBNull(reader["PrimaryDiagnosis"]);
                        Obj.SelectedDiagnosisType.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DiagnosisTypeID"]));
                        Obj.SelectedDiagnosisType.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Obj.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        if (BizActionObj.IsOPDIPD)
                        {
                            Obj.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                            Obj.DocSpec = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpeclization"]));
                        }
                        BizActionObj.PatientDiagnosisDetails.Add(Obj);
                    }

                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetPatientNewEMRDiagnosisList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDiagnosisDataBizActionVO BizActionObj = valueObject as clsGetPatientDiagnosisDataBizActionVO;
            try
            {
                DbCommand command = null;
                if (BizActionObj.IsICDX == false)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetPatientNewEMRDiagnosisDetails");
                    //dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRICDXDiagnosisDetails");
                }
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "Doctorid", DbType.Int64, BizActionObj.DoctorID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDiagnosisDetails == null)
                        BizActionObj.PatientDiagnosisDetails = new List<clsEMRAddDiagnosisVO>();

                    while (reader.Read())
                    {
                        clsEMRAddDiagnosisVO Obj = new clsEMRAddDiagnosisVO();
                        Obj.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        Obj.Class = Convert.ToString(DALHelper.HandleDBNull(reader["Class"]));
                        Obj.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                        Obj.IsICOPIMHead = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsICOPIMHead"]));
                        Obj.Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["DiagnosisName"]));
                        Obj.IsSelected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PrimaryDiagnosis"]));
                        Obj.SelectedDiagnosisType.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DiagnosisTypeID"]));
                        Obj.SelectedDiagnosisType.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Obj.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        Obj.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                        Obj.TemplateName = Convert.ToString(DALHelper.HandleDBNull(reader["TemplateName"]));
                        //added by neena
                        Obj.ArtEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsArt"]));
                        Obj.PlanTreatmentId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentId"]));
                        Obj.BilledEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBilled"]));
                        Obj.IsClosedEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                        Obj.PriorityId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PriorityId"]));
                        Obj.IsArtStatus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsArtStatus"]));
                        Obj.PACEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPAC"]));
                        Obj.IsDonorCycle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonorCycle"]));
                        Obj.DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"]));
                        Obj.DonarUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonarUnitID"]));
                        Obj.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        Obj.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        Obj.CoupleMRNO = Convert.ToString(DALHelper.HandleDBNull(reader["CoupleMRNO"]));
                        Obj.IsSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSurrogacy"]));
                        Obj.SurrogateMRNO = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogateMrNo"]));
                        //
                        BizActionObj.PatientDiagnosisDetails.Add(Obj);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
            }
            return valueObject;
        }

        public override IValueObject GetPatientNewEMRDiagnosisListHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDiagnosisDataBizActionVO BizActionObj = valueObject as clsGetPatientDiagnosisDataBizActionVO;
            try
            {
                DbCommand command = null;
                if (BizActionObj.ISDashBoard == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetPatientNewEMRDiagnosisDetailsHistoryForDashBoard");
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetPatientNewEMRDiagnosisDetailsHistory");

                }
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "@startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "@maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                if (BizActionObj.ISDashBoard == false)
                {
                    dbServer.AddInParameter(command, "DoctorID", DbType.String, BizActionObj.DoctorID);
                }
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDiagnosisDetails == null)
                        BizActionObj.PatientDiagnosisDetails = new List<clsEMRAddDiagnosisVO>();

                    while (reader.Read())
                    {
                        clsEMRAddDiagnosisVO Obj = new clsEMRAddDiagnosisVO();
                        Obj.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        Obj.Class = Convert.ToString(DALHelper.HandleDBNull(reader["Class"]));
                        Obj.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceCode"]));
                        Obj.Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["DiagnosisName"]));
                        Obj.IsSelected = (bool)DALHelper.HandleBoolDBNull(reader["PrimaryDiagnosis"]);
                        Obj.SelectedDiagnosisType.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DiagnosisTypeID"]));
                        Obj.SelectedDiagnosisType.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Obj.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        if (BizActionObj.IsOPDIPD)
                        {
                            Obj.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                            Obj.DocSpec = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpeciliztion"]));
                        }
                        //added by neena
                        Obj.ArtEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsArt"]));
                        Obj.PlanTreatmentId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlannedTreatmentId"]));
                        Obj.BilledEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBilled"]));
                        Obj.IsClosedEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                        Obj.PriorityId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PriorityId"]));
                        Obj.IsArtStatus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsArtStatus"]));
                        Obj.PACEnabled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPAC"]));
                        Obj.IsDonorCycle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonorCycle"]));
                        Obj.DonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorID"]));
                        Obj.DonarUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonarUnitID"]));
                        Obj.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        Obj.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        Obj.CoupleMRNO = Convert.ToString(DALHelper.HandleDBNull(reader["CoupleMRNO"]));
                        Obj.IsSurrogate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSurrogacy"]));
                        Obj.SurrogateMRNO = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogateMrNo"]));
                        //
                        BizActionObj.PatientDiagnosisDetails.Add(Obj);
                    }

                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetPatientProcedureDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientProcedureDataBizActionVO BizActionObj = valueObject as clsGetPatientProcedureDataBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientProcedureDetails");//CIMS_GetPatientEMRICDXDiagnosisDetailsHistory  CIMS_GetPatientNewEMRDiagnosisDetails
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.PatientDiagnosisDetails == null)
                        BizActionObj.PatientDiagnosisDetails = new List<clsEMRAddDiagnosisVO>();
                    while (reader.Read())
                    {
                        clsEMRAddDiagnosisVO Obj = new clsEMRAddDiagnosisVO();
                        Obj.Categori = Convert.ToString(reader["Class"]);
                        Obj.Diagnosis = Convert.ToString(DALHelper.HandleDBNull(reader["DiagnosisName"]));
                        Obj.SelectedDiagnosisType.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DiagnosisTypeID"]));
                        Obj.SelectedDiagnosisType.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Obj.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        Obj.TemplateID = Convert.ToInt64(reader["TemplateID"]);
                        Obj.TemplateName = Convert.ToString(reader["TemplateName"]);
                        if (BizActionObj.IsOPDIPD)
                        {
                            Obj.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                            Obj.DocSpec = Convert.ToString(DALHelper.HandleDBNull(reader["DocSpeclization"]));
                        }
                        BizActionObj.PatientDiagnosisDetails.Add(Obj);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetPatientDiagnosisEMRDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDiagnosisEMRDetailsBizActionVO BizActionObj = valueObject as clsGetPatientDiagnosisEMRDetailsBizActionVO;
            try
            {
                bool isBPControl = false;
                bool isVisionControl = false;
                bool isGPControl = false;
                DbDataReader reader;


                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetPatientDiagnosisEMRDetails");
                dbServer.AddInParameter(command1, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command1, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                dbServer.AddInParameter(command1, "Tab", DbType.String, BizActionObj.Tab);
                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command1);
                if (reader.HasRows)
                {
                    if (BizActionObj.EMRDetailsList == null)
                        BizActionObj.EMRDetailsList = new List<clsPatientEMRDetailsVO>();
                    while (reader.Read())
                    {
                        clsPatientEMRDetailsVO EmrDetails = new clsPatientEMRDetailsVO();
                        EmrDetails.ControlCaption = Convert.ToString(DALHelper.HandleDBNull(reader["ControlCaption"]));
                        //Added by Saily P on 05.12.13 Purpose - New control.
                        if (EmrDetails.ControlCaption == "BPControl")
                            isBPControl = true;
                        if (EmrDetails.ControlCaption == "VisionControl")
                            isVisionControl = true;
                        if (EmrDetails.ControlCaption == "GlassPower")
                            isGPControl = true;
                        //
                        EmrDetails.ControlName = Convert.ToString(DALHelper.HandleDBNull(reader["ControlName"]));
                        EmrDetails.Value = Convert.ToString(DALHelper.HandleDBNull(reader["Value"]));
                        EmrDetails.ControlType = Convert.ToString(DALHelper.HandleDBNull(reader["ControlType"]));

                        BizActionObj.EMRDetailsList.Add(EmrDetails);
                    }
                }
                reader.Close();
                command1.Dispose();
                //Added by Saily P on 05.12.13 Purpose - New controls
                //if (isBPControl == true)
                //{
                //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetBPControlDetails");
                //    dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                //    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                //    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                //    dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                //    //dbServer.AddInParameter(command, "Tab", DbType.String, BizActionObj.Tab);
                //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                //    reader1 = (DbDataReader)dbServer.ExecuteReader(command);
                //    if (reader1.HasRows)
                //    {
                //        if (BizActionObj.objBPControl == null)
                //            BizActionObj.objBPControl = new clsBPControlVO();
                //        while (reader1.Read())
                //        {
                //            clsBPControlVO objBP = new clsBPControlVO();
                //            objBP.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["ID"]));
                //            objBP.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["UnitID"]));
                //            objBP.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["VisitID"]));
                //            objBP.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["PatientID"]));
                //            objBP.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["PatientUnitID"]));
                //            objBP.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader1["TemplateID"]));
                //            objBP.BP1 = Convert.ToInt32(DALHelper.HandleDBNull(reader1["BP1"]));
                //            objBP.BP2 = Convert.ToInt32(DALHelper.HandleDBNull(reader1["BP2"]));

                //            BizActionObj.objBPControl = objBP;
                //            BizActionObj.isBPControl = true;
                //        }
                //    }
                //}

                //if (isVisionControl == true)
                //{
                //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetVisionControlDetails");
                //    dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                //    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                //    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                //    dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                //    //dbServer.AddInParameter(command, "Tab", DbType.String, BizActionObj.Tab);
                //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                //    reader2 = (DbDataReader)dbServer.ExecuteReader(command);
                //    if (reader2.HasRows)
                //    {
                //        if (BizActionObj.objVisionControl == null)
                //            BizActionObj.objVisionControl = new clsVisionVO();
                //        while (reader2.Read())
                //        {
                //            clsVisionVO objVision = new clsVisionVO();
                //            objVision.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ID"]));
                //            objVision.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["UnitID"]));
                //            objVision.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["VisitID"]));
                //            objVision.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["PatientID"]));
                //            objVision.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["PatientUnitID"]));
                //            objVision.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["TemplateID"]));
                //            objVision.SPH = Convert.ToDecimal(DALHelper.HandleDBNull(reader2["SPH"]));
                //            objVision.CYL = Convert.ToDecimal(DALHelper.HandleDBNull(reader2["CYL"]));
                //            objVision.Axis = Convert.ToDecimal(DALHelper.HandleDBNull(reader2["Axis"]));

                //            BizActionObj.objVisionControl = objVision;
                //            BizActionObj.isVisionControl = true;
                //        }
                //    }
                //}

                //if (isGPControl == true)
                //{
                //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGlassPowerDetails");
                //    dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                //    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                //    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                //    dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                //    //dbServer.AddInParameter(command, "Tab", DbType.String, BizActionObj.Tab);
                //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                //    reader3 = (DbDataReader)dbServer.ExecuteReader(command);
                //    if (reader3.HasRows)
                //    {
                //        if (BizActionObj.objGPControl == null)
                //            BizActionObj.objGPControl = new clsGlassPowerVO();
                //        while (reader3.Read())
                //        {
                //            clsGlassPowerVO objGP = new clsGlassPowerVO();
                //            objGP.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["ID"]));
                //            objGP.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["UnitID"]));
                //            objGP.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["VisitID"]));
                //            objGP.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["PatientID"]));
                //            objGP.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["PatientUnitID"]));
                //            objGP.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["TemplateID"]));
                //            objGP.SPH1 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["SPH1"]));
                //            objGP.CYL1 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["CYL1"]));
                //            objGP.Axis1 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["Axis1"]));
                //            objGP.VA1 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["VA1"]));

                //            objGP.SPH2 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["SPH2"]));
                //            objGP.CYL2 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["CYL2"]));
                //            objGP.Axis2 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["Axis2"]));
                //            objGP.VA2 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["VA2"]));

                //            BizActionObj.objGPControl = objGP;
                //            BizActionObj.isGPControl = true;
                //        }
                //    }
                //}


                //if (isGPControl == true)
                //{
                //    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGlassPowerDetails");
                //    dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                //    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                //    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                //    dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                //    //dbServer.AddInParameter(command, "Tab", DbType.String, BizActionObj.Tab);
                //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                //    reader3 = (DbDataReader)dbServer.ExecuteReader(command);
                //    if (reader3.HasRows)
                //    {
                //        if (BizActionObj.objGPControl == null)
                //            BizActionObj.objGPControl = new clsGlassPowerVO();
                //        while (reader3.Read())
                //        {
                //            clsGlassPowerVO objGP = new clsGlassPowerVO();
                //            objGP.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["ID"]));
                //            objGP.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["UnitID"]));
                //            objGP.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["VisitID"]));
                //            objGP.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["PatientID"]));
                //            objGP.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["PatientUnitID"]));
                //            objGP.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["TemplateID"]));
                //            objGP.SPH1 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["SPH1"]));
                //            objGP.CYL1 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["CYL1"]));
                //            objGP.Axis1 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["Axis1"]));
                //            objGP.VA1 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["VA1"]));

                //            objGP.SPH2 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["SPH2"]));
                //            objGP.CYL2 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["CYL2"]));
                //            objGP.Axis2 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["Axis2"]));
                //            objGP.VA2 = Convert.ToDecimal(DALHelper.HandleDBNull(reader3["VA2"]));

                //            BizActionObj.objGPControl = objGP;
                //            BizActionObj.isGPControl = true;
                //        }
                //    }
                //}


                //#region Eye Control
                //DbDataReader reader4;
                //DbCommand dbcommand = dbServer.GetStoredProcCommand("CIMS_GetEyeControlDetails");
                //dbServer.AddInParameter(dbcommand, "VisitID", DbType.Int64, BizActionObj.VisitID);
                //dbServer.AddInParameter(dbcommand, "PatientID", DbType.Int64, BizActionObj.PatientID);
                //dbServer.AddInParameter(dbcommand, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                //dbServer.AddInParameter(dbcommand, "TemplateID", DbType.Int64, BizActionObj.TemplateID);
                //dbServer.AddInParameter(dbcommand, "UnitID", DbType.Int64, BizActionObj.UnitID);

                //reader4 = (DbDataReader)dbServer.ExecuteReader(dbcommand);
                //if (reader4.HasRows)
                //{
                //    if (BizActionObj.EyeList == null)
                //        BizActionObj.EyeList = new List<clsEyeControlVO>();
                //    while (reader4.Read())
                //    {
                //        clsEyeControlVO obj = new clsEyeControlVO();
                //        obj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["ID"]));
                //        obj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["UnitID"]));
                //        obj.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["VisitID"]));
                //        obj.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["PatientID"]));
                //        obj.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["PatientUnitID"]));
                //        obj.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["TemplateID"]));
                //        obj.Eye1 = DALHelper.HandleDBNull(reader4["Type"]).ToString();
                //        obj.LeftCYL = DALHelper.HandleDBNull(reader4["LeftCYL"]).ToString();
                //        obj.LeftAXIS = DALHelper.HandleDBNull(reader4["LeftAXIS"]).ToString();
                //        obj.LeftSPH = DALHelper.HandleDBNull(reader4["LeftSPH"]).ToString();
                //        obj.RightAXIS = DALHelper.HandleDBNull(reader4["RightAXIS"]).ToString();
                //        obj.RightCYL = DALHelper.HandleDBNull(reader4["RightCYL"]).ToString();
                //        obj.RightSPH = DALHelper.HandleDBNull(reader4["RightSPH"]).ToString();
                //        BizActionObj.EyeList.Add(obj);
                //    }
                //}
                //#endregion

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return BizActionObj;
        }

        public override IValueObject GetPatientPastPhysicalExamDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPastPhysicalexamDetailsBizActionVO BizAction = valueObject as clsGetPatientPastPhysicalexamDetailsBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command = null;
                if (BizAction.IsOpdIpd)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_IPDEMRPatientConsultationSummary");
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_OPDEMRPatientConsultationSummary");
                }
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizAction.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "DoctorCode", DbType.String, BizAction.DoctorCode);
                dbServer.AddInParameter(command, "DoctorId", DbType.Int64, BizAction.DoctorID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizAction.IsOpdIpd);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizAction.MaximumRows);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizAction.PatientPastPhysicalexam == null)
                        BizAction.PatientPastPhysicalexam = new List<GetPastPhysicalexam>();
                    while (reader.Read())
                    {
                        GetPastPhysicalexam objPrescriptionVO = new GetPastPhysicalexam();
                        objPrescriptionVO.Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
                        objPrescriptionVO.TemplateId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        objPrescriptionVO.TemplateValue = Convert.ToString(DALHelper.HandleDBNull(reader["Value"]));
                        objPrescriptionVO.TemplateHeader = Convert.ToString(reader["Header"]);
                        objPrescriptionVO.DoctorName = Convert.ToString(reader["DoctorName"]);
                        objPrescriptionVO.DoctorSpeclization = Convert.ToString(reader["DrSpeclizition"]);
                        objPrescriptionVO.VisitDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["AddedDateTime"]));
                        objPrescriptionVO.DateTime = Convert.ToString(DALHelper.HandleDBNull(reader["AddedDateTime"]));
                        BizAction.PatientPastPhysicalexam.Add(objPrescriptionVO);
                    }
                    reader.NextResult();
                    BizAction.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
            }
            catch (Exception ee)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetPatientAllergies(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientAllergiesBizActionVO BizAction = valueObject as clsGetPatientAllergiesBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientAllergies");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizAction.AllergiesList == null)
                        BizAction.AllergiesList = new List<clsEMRAllergiesVO>();

                    clsEMRAllergiesVO Obj = new clsEMRAllergiesVO();
                    if (reader.Read())
                    {
                        Obj.FoodAllergy = String.Format(Obj.FoodAllergy + "," + Convert.ToString(DALHelper.HandleDBNull(reader["FoodAllergy"]))).Trim(',');
                        Obj.DrugAllergy = String.Format(Obj.DrugAllergy + "," + Convert.ToString(DALHelper.HandleDBNull(reader["DrugAllergy"]))).Trim(',');
                        Obj.OtherAllergy = String.Format(Obj.OtherAllergy + "," + Convert.ToString(DALHelper.HandleDBNull(reader["OtherAllergy"]))).Trim(',');
                        Obj.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        BizAction.CurrentAllergy = Obj;
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return BizAction;
        }

        public override IValueObject AddUpdatePatientAllergies(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientAllergiesBizActionVO BizAction = valueObject as clsAddUpdatePatientAllergiesBizActionVO;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientEMRAllergies");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizAction.VisitID);
                dbServer.AddInParameter(command, "DoctorCode", DbType.String, BizAction.DoctorCode);
                dbServer.AddInParameter(command, "OPDIPD", DbType.Boolean, BizAction.OPDIPD);
                dbServer.AddInParameter(command, "VisitUnitID", DbType.Int64, BizAction.VisitUnitID);
                //dbServer.AddInParameter(command, "AllergyTypeID", DbType.Int16, item.AllergyTypeID);
                dbServer.AddInParameter(command, "FoodAllergy", DbType.String, BizAction.CurrentAllergies.FoodAllergy.Replace("'", ""));
                dbServer.AddInParameter(command, "DrugAllergy", DbType.String, BizAction.CurrentAllergies.DrugAllergy.Replace("'", ""));
                dbServer.AddInParameter(command, "OtherAllergy", DbType.String, BizAction.CurrentAllergies.OtherAllergy.Replace("'", ""));
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.AllergyID);
                //dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizAction.AllergyID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeletePatientDrugAllergies");
                dbServer.AddInParameter(command2, "PatientId", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                int intStatus2 = dbServer.ExecuteNonQuery(command2);

                List<clsGetDrugForAllergies> objGetAllergiesList = BizAction.DrugAllergies;
                for (int i = 0; i < objGetAllergiesList.Count; i++)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientDrugAllergies");
                    dbServer.AddInParameter(command1, "PatientId", DbType.Int64, BizAction.PatientID);
                    dbServer.AddInParameter(command1, "DrugID", DbType.Int64, objGetAllergiesList[i].DrugId);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
            }
            return BizAction;
        }

        public override IValueObject AddUpdatePatientChiefComplaints(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientChiefComplaintsBizActionVO BizAction = valueObject as clsAddUpdatePatientChiefComplaintsBizActionVO;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientEMRChiefComplaints");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizAction.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Doctorid", DbType.Int32, BizAction.DoctorID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                dbServer.AddInParameter(command, "VisitUnitID", DbType.Int64, BizAction.VisitUnitID);
                dbServer.AddInParameter(command, "ChiefComplaints", DbType.String, BizAction.CurrentChiefComplaints.ChiefComplaints);
                dbServer.AddInParameter(command, "AssChiefComplaints", DbType.String, BizAction.CurrentChiefComplaints.AssChiefComplaints);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.CurrentChiefComplaints.ID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizAction.CurrentChiefComplaints.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
            }
            return BizAction;
        }

        public override IValueObject GetPatientChiefComplaints(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientChiefComplaintsBizActionVO BizAction = valueObject as clsGetPatientChiefComplaintsBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientCurrentChiefComplaints");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizAction.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
                dbServer.AddInParameter(command, "Doctorid", DbType.Int64, BizAction.DoctorID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizAction.ChiefComplaintList == null)
                        BizAction.ChiefComplaintList = new List<clsEMRChiefComplaintsVO>();

                    while (reader.Read())
                    {
                        clsEMRChiefComplaintsVO Obj = new clsEMRChiefComplaintsVO();
                        Obj.AssChiefComplaints = Convert.ToString(DALHelper.HandleDBNull(reader["AssChiefComplaints"]));
                        Obj.ChiefComplaints = Convert.ToString(DALHelper.HandleDBNull(reader["ChiefComplaints"]));
                        Obj.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        Obj.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        Obj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.CurrentChiefComplaints = Obj;
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }

            return BizAction;
        }

        public override IValueObject UploadPatientImage(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsUploadPatientImageBizActionVO ObjFile = valueObject as clsUploadPatientImageBizActionVO;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUploadPatientFile");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, ObjFile.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, ObjFile.UploadMatserDetails.TemplateID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, ObjFile.VisitID);
                dbServer.AddInParameter(command, "SourceURL", DbType.String, ObjFile.UploadMatserDetails.SourceURL);
                dbServer.AddInParameter(command, "OriginalReport", DbType.Binary, ObjFile.OriginalImage);
                dbServer.AddInParameter(command, "EditReport", DbType.Binary, ObjFile.EditImage);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, ObjFile.IsOPDIPD);
                dbServer.AddInParameter(command, "DocumentName", DbType.String, ObjFile.UploadMatserDetails.DocumentName);
                dbServer.AddInParameter(command, "Remark", DbType.String, ObjFile.Remark);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjFile.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                ObjFile.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception ex)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }

            return valueObject;
        }

        public override IValueObject GetUploadPatientImage(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsGetUploadPatientImageBizActionVO ObjFile = valueObject as clsGetUploadPatientImageBizActionVO;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetUploadPatientFile");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, ObjFile.PatientID);
                dbServer.AddInParameter(command, "VISITID", DbType.Int64, ObjFile.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ISOPDIPD", DbType.Boolean, ObjFile.ISOPDIPD);

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPatientFollowUpImageVO ImageDetail = new clsPatientFollowUpImageVO();
                        ImageDetail.EditImage = (byte[])DALHelper.HandleDBNull(reader["EditReport"]);
                        ImageDetail.Remark = (string)DALHelper.HandleDBNull(reader["Remark"]);
                        ImageDetail.SourceURL = (string)DALHelper.HandleDBNull(reader["SourceURL"]);
                        ImageDetail.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        ObjFile.ImageDetails.Add(ImageDetail);
                    }
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject UpdateUploadPatientImage(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            try
            {
                clsUpdateUploadPatientImageBizActionVO ObjFile = valueObject as clsUpdateUploadPatientImageBizActionVO;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateUploadPatientFile");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, ObjFile.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, ObjFile.PatientUnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, ObjFile.VisitID);
                dbServer.AddInParameter(command, "EditReport", DbType.Binary, ObjFile.EditImage);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, ObjFile.TemplateID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "DocumentName", DbType.String, ObjFile.DocumentName);
                dbServer.AddInParameter(command, "SourceURL", DbType.String, ObjFile.SourceURL);
                dbServer.AddInParameter(command, "Remark", DbType.String, ObjFile.Remark);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject AddUpdatePatientInvestigations(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientInvestigationsBizActionVO BizAction = valueObject as clsAddUpdatePatientInvestigationsBizActionVO;
            DbConnection con = null;
            DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDoctorSuggestedServiceDetails");

            dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizAction.VisitID);
            dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
            dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
            //dbServer.AddInParameter(command, "IsOther", DbType.Boolean, BizAction.IsOtherServices);
            dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
            dbServer.AddInParameter(command, "Doctorid", DbType.Int32, BizAction.DoctorID);
            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
            dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
            dbServer.AddParameter(command, "PrescriptionID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
            int intStatus = dbServer.ExecuteNonQuery(command);
            long lPrescriptionID = (long)dbServer.GetParameterValue(command, "PrescriptionID");
            BizAction.SuccessStatus = (int)lPrescriptionID;
            List<clsDoctorSuggestedServiceDetailVO> objDoctorSuggestedServiceVO = BizAction.InvestigationList;

            for (int i = 0; i < objDoctorSuggestedServiceVO.Count; i++)
            {
                try
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDoctorSuggestedServiceDetails");
                    dbServer.AddInParameter(command1, "PrescriptionID", DbType.Int64, lPrescriptionID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "ServiceID", DbType.Int64, objDoctorSuggestedServiceVO[i].ServiceID);
                    dbServer.AddInParameter(command1, "ServiceName", DbType.String, objDoctorSuggestedServiceVO[i].ServiceName);
                    dbServer.AddInParameter(command1, "GroupName", DbType.String, objDoctorSuggestedServiceVO[i].GroupName);
                    dbServer.AddInParameter(command1, "Rate", DbType.Double, objDoctorSuggestedServiceVO[i].ServiceRate);
                    //dbServer.AddInParameter(command1, "ServiceType", DbType.String, objDoctorSuggestedServiceVO[i].ServiceType);
                    //dbServer.AddInParameter(command1, "IsOther", DbType.Double, objDoctorSuggestedServiceVO[i].IsOther);
                    dbServer.AddInParameter(command1, "ServiceCode", DbType.String, objDoctorSuggestedServiceVO[i].ServiceCode);
                    dbServer.AddInParameter(command1, "SpecializationID", DbType.String, objDoctorSuggestedServiceVO[i].SpecializationId);
                    dbServer.AddInParameter(command1, "Doctorid", DbType.String, BizAction.DoctorID);
                    dbServer.AddInParameter(command1, "PriorityID", DbType.Int64, objDoctorSuggestedServiceVO[i].SelectedPriority.ID);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDoctorSuggestedServiceVO[i].ID);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1);

                    BizAction.InvestigationList[i].ID = (long)dbServer.GetParameterValue(command1, "ID");
                }

                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    if (con != null && con.State == ConnectionState.Open) con.Close();
                }
            }

            return BizAction;
        }

        public override IValueObject GetPatientPreviousVisitServices(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetServicesBizActionVO BizActionObj = valueObject as clsGetServicesBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPastInvestigations");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                dbServer.AddInParameter(command, "Doctorid", DbType.Int32, BizActionObj.DoctorID);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.VisitServicesList == null)
                        BizActionObj.VisitServicesList = new List<clsServiceMasterVO>();
                    while (reader.Read())
                    {
                        clsServiceMasterVO ObjService = new clsServiceMasterVO();
                        ObjService.PrescriptionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrescriptionID"]));
                        ObjService.DoctorID = Convert.ToInt32(reader["Doctorid"]);
                        // ObjService.DoctorCode = Convert.ToString(reader["DoctorCode"]);
                        ObjService.ServiceCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        ObjService.ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"]));
                        if (BizActionObj.IsOPDIPD)
                        {
                            ObjService.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                            ObjService.SpecializationString = Convert.ToString(DALHelper.HandleDBNull(reader["DrSpec"]));
                        }
                        ObjService.Group = Convert.ToString(reader["GroupName"]);
                        ObjService.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        ObjService.VisitDate = Convert.ToDateTime(reader["VisitDate"]);
                        ObjService.SelectedPriority = new MasterListItem();
                        ObjService.SelectedPriority.Description = Convert.ToString(DALHelper.HandleDBNull(reader["PriorityDescription"]));
                        BizActionObj.VisitServicesList.Add(ObjService);
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return BizActionObj;
        }

        public override IValueObject AddUpdatePatientMedicationFromCPOE(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientCurrentMedicationsBizActionVO BizAction = valueObject as clsAddUpdatePatientCurrentMedicationsBizActionVO;
            DbConnection con = null;
            DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeletePatientPrescriptionDetailsFromCPOE");
            dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizAction.VisitID);
            dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
            dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
            //dbServer.AddInParameter(command, "DoctorCode", DbType.String, BizAction.DoctorCode);
            dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
            dbServer.AddInParameter(command, "DoctorID", DbType.Int64, BizAction.DoctorID);
            dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
            dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
            dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
            dbServer.AddParameter(command, "PrescriptionID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);

            int intStatus2 = dbServer.ExecuteNonQuery(command);
            long lPrescriptionID = (long)dbServer.GetParameterValue(command, "PrescriptionID");
            command.Dispose();

            List<clsPatientPrescriptionDetailVO> lstMedication = BizAction.PatientCurrentMedicationDetailList;

            foreach (clsPatientPrescriptionDetailVO objCurMedVo in BizAction.PatientCurrentMedicationDetailList)
            {
                try
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPatientPrescriptionDetailFromCPOE");//CIMS_AddPatientPrescriptionDetailCurrentMedication
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "PrescriptionID", DbType.Int64, lPrescriptionID);
                    dbServer.AddInParameter(command1, "DrugID", DbType.String, objCurMedVo.DrugID);

                    dbServer.AddInParameter(command1, "Dose", DbType.String, objCurMedVo.Dose);
                    if (objCurMedVo.SelectedRoute != null && objCurMedVo.SelectedRoute.Description != "--Select--")
                        dbServer.AddInParameter(command1, "Route", DbType.String, objCurMedVo.SelectedRoute.Description);
                    else
                        dbServer.AddInParameter(command1, "Route", DbType.String, null);

                    if (objCurMedVo.SelectedInstruction != null && objCurMedVo.SelectedInstruction.Description != "--Select--")
                        dbServer.AddInParameter(command1, "Instruction", DbType.String, objCurMedVo.SelectedInstruction.Description);
                    else
                        dbServer.AddInParameter(command1, "Instruction", DbType.String, null);

                    if (objCurMedVo.SelectedFrequency != null && objCurMedVo.SelectedFrequency.Description != "--Select--")
                        dbServer.AddInParameter(command1, "Frequency", DbType.String, objCurMedVo.SelectedFrequency.Description);
                    else
                        dbServer.AddInParameter(command1, "Frequency", DbType.String, null);

                    dbServer.AddInParameter(command1, "NewInstruction", DbType.String, objCurMedVo.NewInstruction);
                    dbServer.AddInParameter(command1, "ItemName", DbType.String, objCurMedVo.DrugName);

                    dbServer.AddInParameter(command1, "UOM", DbType.String, objCurMedVo.UOM);
                    dbServer.AddInParameter(command1, "UOMID", DbType.Int64, objCurMedVo.UOMID);

                    dbServer.AddInParameter(command1, "Comment", DbType.String, objCurMedVo.Comment);

                    dbServer.AddInParameter(command1, "Days", DbType.Int64, objCurMedVo.Days);
                    dbServer.AddInParameter(command1, "Quantity", DbType.Double, objCurMedVo.Quantity);
                    dbServer.AddInParameter(command1, "IsOther", DbType.Boolean, objCurMedVo.IsOther);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                    //added by neena
                    dbServer.AddInParameter(command1, "ARTEnables", DbType.Boolean, objCurMedVo.ArtEnabled);
                    dbServer.AddInParameter(command1, "DrugSourceId ", DbType.Int64, objCurMedVo.SelectedDrugSource.ID);
                    dbServer.AddInParameter(command1, "PlanTherapyId ", DbType.Int64, objCurMedVo.PlanTherapyId);
                    dbServer.AddInParameter(command1, "PlanTherapyUnitId ", DbType.Int64, objCurMedVo.PlanTherapyUnitId);
                    //


                    int intStatus1 = dbServer.ExecuteNonQuery(command1);
                    BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                }
                catch (Exception)
                {

                    throw;
                }
            }
            if (con != null && con.State == ConnectionState.Open) con.Close();

            return BizAction;
        }

        public override IValueObject AddUpdateDeleteVitalDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateDeleteVitalDetailsBizActionVO BizActionObj = valueObject as clsAddUpdateDeleteVitalDetailsBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                trans = con.BeginTransaction();
                long GetIPD = 0;
                long GetOPD = 0;
                if (BizActionObj.IsOPDIPD)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("[CIMS_GetIPDFlag]");
                    GetIPD = Convert.ToInt64(dbServer.ExecuteScalar(command1, trans));
                }
                else
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_GetOPDFlag");
                    GetOPD = Convert.ToInt64(dbServer.ExecuteScalar(command1, trans));
                }
                foreach (var item in BizActionObj.PatientVitalDetails)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateDeletePatientEMRVitalDeatails");
                    dbServer.AddInParameter(command, "ID", DbType.Int64, item.PatientVitalID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                    dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                    dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                    dbServer.AddInParameter(command, "Doctorid", DbType.Int32, BizActionObj.DoctorID);
                    if (BizActionObj.IsOPDIPD)
                    {
                        dbServer.AddInParameter(command, "TakenBy", DbType.Int64, BizActionObj.TakenBy);
                    }
                    else
                    {
                        dbServer.AddInParameter(command, "TakenBy", DbType.Int64, null);
                    }
                    dbServer.AddInParameter(command, "TemplateID", DbType.Int64, item.TemplateID);
                    dbServer.AddInParameter(command, "VitalID", DbType.Int64, item.ID);
                    dbServer.AddInParameter(command, "Value", DbType.Double, item.Value);
                    dbServer.AddInParameter(command, "Unit", DbType.String, item.Unit);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "IpdFlag", DbType.String, GetIPD);
                    dbServer.AddInParameter(command, "OPDFlag", DbType.String, GetOPD);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, item.Status);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.Output, null, DataRowVersion.Default, BizActionObj.SuccessStatus);
                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    if (BizActionObj.SuccessStatus != 1)
                        throw new Exception();
                    BizActionObj.PatientDiagnosisID = (long)dbServer.GetParameterValue(command, "ID");
                }

                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
                trans.Dispose();
            }
            return BizActionObj;
        }

        public override IValueObject GetVitalListDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetVitalListDetailsBizActionVO BizActionObj = valueObject as clsGetVitalListDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRVitalDetailsList");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                //dbServer.AddInParameter(command, "DoctorCode", DbType.String, BizActionObj.DoctorCode);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.vitalListDetail == null)
                        BizActionObj.vitalListDetail = new List<clsEMRVitalsVO>();

                    while (reader.Read())
                    {
                        clsEMRVitalsVO Obj = new clsEMRVitalsVO();
                        Obj.ID = (long)DALHelper.HandleDBNull(reader["VitalID"]);
                        Obj.PatientVitalID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Obj.Description = (string)DALHelper.HandleDBNull(reader["Vital"]);
                        Obj.Date = (DateTime?)DALHelper.HandleDBNull(reader["Date"]);
                        Obj.Time = (DateTime?)DALHelper.HandleDBNull(reader["Time"]);
                        Obj.Value = (double)DALHelper.HandleDBNull(reader["Value"]);
                        Obj.Unit = Convert.ToString(DALHelper.HandleDBNull(reader["Unit"]));
                        //Obj.IsDecimal=Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsDecimal"]));
                        // Obj.IsEven=Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsEven"]));
                        if (Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsDecimal"])) == false)
                            Obj.ValueDescription = String.Format("{0:0}", Convert.ToDouble(DALHelper.HandleDBNull(reader["Value"])));
                        else
                            Obj.ValueDescription = String.Format("{0:0.00}", Convert.ToDouble(DALHelper.HandleDBNull(reader["Value"])));
                        BizActionObj.vitalListDetail.Add(Obj);
                    }

                }

                reader.Close();

            }
            catch (Exception ex)
            {

                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }

            return valueObject;
        }

        public override IValueObject GetPatientPatientVitalChartList(IValueObject valueObject, clsUserVO UserVo)
        {
            int flag = 0;
            clsGetPatientVitalChartBizActionVO BizActionObj = valueObject as clsGetPatientVitalChartBizActionVO;
            try
            {
                DbCommand command = null;
                if (BizActionObj.IsFromDashBoard && !BizActionObj.IsOPDIPD)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetPatientVitalChartDetailsForDashBoard");

                    dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                    flag = 1;
                }
                else
                {
                    if (BizActionObj.IsOPDIPD)
                    {
                        command = dbServer.GetStoredProcCommand("CIMS_GetGetIPDPatientVitalChartDetails");
                    }
                    else
                    {
                        command = dbServer.GetStoredProcCommand("CIMS_GetGetPatientVitalChartDetails");
                    }
                }
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientVitalChartlst == null)
                        BizActionObj.PatientVitalChartlst = new List<clsVitalChartVO>();

                    while (reader.Read())
                    {
                        clsVitalChartVO Obj = new clsVitalChartVO();
                        Obj.Height = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["Height"]));
                        Obj.Weight = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["Weight"]));
                        if (Obj.Height > 0)
                            Obj.BMI = (Obj.Weight / (Obj.Height * Obj.Height / 10000));
                        Obj.SystolicBP = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["SBP"]));
                        Obj.DiastolicBP = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["DBP"]));
                        Obj.HC = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["HC"]));
                        Obj.Pulse = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["Pulse"]));
                        Obj.RR = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["RR"]));
                        Obj.O2 = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["O2"]));
                        Obj.Temperature = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["Temprature"]));
                        Obj.Waistgirth = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["Waistgirth"]));
                        Obj.Hipgirth = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["Hipgirth"]));
                        Obj.RBS = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["RBS"]));
                        Obj.TotalCholesterol = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["TotalCholesterol"]));
                        Obj.RandomBloodSugar = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["RandomBloodSugar"]));
                        Obj.FastingSugar = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["FastingSugar"]));
                        Obj.HeadCircumference = Convert.ToDouble(DALHelper.HandleDoubleNull(reader["HeadCircumference"]));
                        Obj.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"]));
                        if (BizActionObj.IsOPDIPD)
                        {
                            Obj.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["Doctorname"]));
                            Obj.DoctorSpeclization = Convert.ToString(DALHelper.HandleDBNull(reader["DrSpec"]));
                        }
                        BizActionObj.PatientVitalChartlst.Add(Obj);
                    }
                }
                if (flag == 1)
                {
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    flag = 0;
                }
                reader.Close();
            }
            catch (Exception)
            { // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return BizActionObj;
        }

        public override IValueObject DoctorlistonReferal(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDoctorlistReferralServiceBizActionVO BizActionObj = valueObject as clsGetDoctorlistReferralServiceBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorListOnReferralLoad");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "ServiceID", DbType.Int64, BizActionObj.ServiceId);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }

                    while (reader.Read())
                    {
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Description"].ToString(), true));

                    }

                }

                reader.Close();

            }
            catch (Exception ex)
            {

                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }

            return valueObject;
        }

        #region Growth Chart
        public override IValueObject GetPatientGrowthChartVisitList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientGrowthChartBizActionVO BizActionObj = valueObject as clsGetPatientGrowthChartBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGrowthChartVisitDetails");
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, BizActionObj.PatientID);
                if (BizActionObj.DrID != null)
                {
                    if (BizActionObj.DrID > 0)
                        dbServer.AddInParameter(command, "DrID", DbType.Int64, BizActionObj.DrID);
                    else
                        dbServer.AddInParameter(command, "DrID", DbType.Int64, null);
                }
                else
                    dbServer.AddInParameter(command, "DrID", DbType.Int64, null);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.GrowthChartDetailList == null)
                        BizActionObj.GrowthChartDetailList = new List<clsGrowthChartVO>();
                    while (reader.Read())
                    {
                        clsGrowthChartVO objGrowthChartDetail = new clsGrowthChartVO();
                        BizActionObj.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objGrowthChartDetail.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));

                        objGrowthChartDetail.VisitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["VisitID"]));
                        //  objGrowthChartDetail.PatientID=Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PatientID"]));
                        objGrowthChartDetail.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitId"]));
                        objGrowthChartDetail.ChiefComplaint = Convert.ToString(DALHelper.HandleDBNull(reader["ComplentChief"]));

                        objGrowthChartDetail.Height = Convert.ToDouble(DALHelper.HandleDBNull(reader["Height"]));
                        objGrowthChartDetail.Weight = Convert.ToDouble(DALHelper.HandleDBNull(reader["Weight"]));
                        objGrowthChartDetail.BMI = Convert.ToDouble(DALHelper.HandleDBNull(reader["BMI"]));
                        objGrowthChartDetail.SBP = Convert.ToDouble(DALHelper.HandleDBNull(reader["SBP"]));
                        objGrowthChartDetail.DBP = Convert.ToDouble(DALHelper.HandleDBNull(reader["DBP"]));
                        objGrowthChartDetail.HC = Convert.ToDouble(DALHelper.HandleDBNull(reader["HC"]));
                        objGrowthChartDetail.Pulse = Convert.ToDouble(DALHelper.HandleDBNull(reader["Pulse"]));
                        objGrowthChartDetail.RR = Convert.ToDouble(DALHelper.HandleDBNull(reader["RR"]));
                        objGrowthChartDetail.Spo2 = Convert.ToDouble(DALHelper.HandleDBNull(reader["Spo2"]));
                        objGrowthChartDetail.Temprature = Convert.ToDouble(DALHelper.HandleDBNull(reader["Temprature"]));

                        objGrowthChartDetail.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        objGrowthChartDetail.MobileCountryCode = Convert.ToInt64(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        objGrowthChartDetail.MOB = Convert.ToString(DALHelper.HandleDBNull(reader["MOB"]));
                        objGrowthChartDetail.VisitDate = Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"]));
                        objGrowthChartDetail.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));

                        objGrowthChartDetail.Age = Convert.ToInt64(DALHelper.HandleDBNull(reader["Age"]));
                        objGrowthChartDetail.AgeInMonth = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInMonth"]));
                        objGrowthChartDetail.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        objGrowthChartDetail.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        objGrowthChartDetail.AgeInYearMonthDays = Convert.ToString(DALHelper.HandleDBNull(reader["AgeInYearMonthDays"]));
                        objGrowthChartDetail.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        objGrowthChartDetail.DrName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        objGrowthChartDetail.OPD = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNo"]));

                        if ((objGrowthChartDetail.Height > 0 && objGrowthChartDetail.Weight > 0) || objGrowthChartDetail.HC > 0)
                            if (objGrowthChartDetail.AgeInMonth <= 240)
                                objGrowthChartDetail.ViewDetails = true;
                            else
                                objGrowthChartDetail.ViewDetails = false;
                        BizActionObj.GrowthChartDetailList.Add(objGrowthChartDetail);
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetPatientGrowthChartMonthlyVisitList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientGrowthChartMonthlyBizActionVO BizActionObj = valueObject as clsGetPatientGrowthChartMonthlyBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGrowthChartVisitMonthlyDetails");
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "ISOPDIPD", DbType.Boolean, BizActionObj.IsOPDIPD);
                if (BizActionObj.DrID != null)
                {
                    if (BizActionObj.DrID > 0)
                        dbServer.AddInParameter(command, "DrID", DbType.Int64, BizActionObj.DrID);
                    else
                        dbServer.AddInParameter(command, "DrID", DbType.Int64, null);
                }
                else
                    dbServer.AddInParameter(command, "DrID", DbType.Int64, null);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.GrowthChartDetailList == null)
                        BizActionObj.GrowthChartDetailList = new List<clsGrowthChartVO>();
                    while (reader.Read())
                    {
                        clsGrowthChartVO objGrowthChartDetail = new clsGrowthChartVO();
                        BizActionObj.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objGrowthChartDetail.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));

                        objGrowthChartDetail.VisitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["VisitID"]));
                        //  objGrowthChartDetail.PatientID=Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PatientID"]));
                        objGrowthChartDetail.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitId"]));
                        objGrowthChartDetail.ChiefComplaint = Convert.ToString(DALHelper.HandleDBNull(reader["ComplentChief"]));

                        objGrowthChartDetail.Height = Convert.ToDouble(DALHelper.HandleDBNull(reader["Height"]));
                        objGrowthChartDetail.Weight = Convert.ToDouble(DALHelper.HandleDBNull(reader["Weight"]));
                        if (objGrowthChartDetail.Height > 0)
                            objGrowthChartDetail.BMI = (objGrowthChartDetail.Weight / (objGrowthChartDetail.Height * objGrowthChartDetail.Height / 10000));
                        objGrowthChartDetail.SBP = Convert.ToDouble(DALHelper.HandleDBNull(reader["SBP"]));
                        objGrowthChartDetail.DBP = Convert.ToDouble(DALHelper.HandleDBNull(reader["DBP"]));
                        objGrowthChartDetail.HC = Convert.ToDouble(DALHelper.HandleDBNull(reader["HC"]));
                        objGrowthChartDetail.Pulse = Convert.ToDouble(DALHelper.HandleDBNull(reader["Pulse"]));
                        objGrowthChartDetail.RR = Convert.ToDouble(DALHelper.HandleDBNull(reader["RR"]));
                        objGrowthChartDetail.Spo2 = Convert.ToDouble(DALHelper.HandleDBNull(reader["Spo2"]));
                        objGrowthChartDetail.Temprature = Convert.ToDouble(DALHelper.HandleDBNull(reader["Temprature"]));

                        objGrowthChartDetail.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        //   objGrowthChartDetail.MobileCountryCode = Convert.ToInt64(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        objGrowthChartDetail.MOB = Convert.ToString(DALHelper.HandleDBNull(reader["MOB"]));
                        objGrowthChartDetail.VisitDate = Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"]));
                        objGrowthChartDetail.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));

                        objGrowthChartDetail.Age = Convert.ToInt64(DALHelper.HandleDBNull(reader["Age"]));
                        objGrowthChartDetail.AgeInMonth = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInMonth"]));
                        //objGrowthChartDetail.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        objGrowthChartDetail.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        objGrowthChartDetail.AgeInYearMonthDays = Convert.ToString(DALHelper.HandleDBNull(reader["AgeInYearMonthDays"]));
                        objGrowthChartDetail.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        objGrowthChartDetail.DrName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        objGrowthChartDetail.OPD = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNo"]));

                        if ((objGrowthChartDetail.Height > 0 && objGrowthChartDetail.Weight > 0) || objGrowthChartDetail.HC > 0)
                            if (objGrowthChartDetail.AgeInMonth <= 240)
                                objGrowthChartDetail.ViewDetails = true;
                            else
                                objGrowthChartDetail.ViewDetails = false;
                        BizActionObj.GrowthChartDetailList.Add(objGrowthChartDetail);
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject GetPatientGrowthChartYearlyVisitList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientGrowthChartYearlyBizActionVO BizActionObj = valueObject as clsGetPatientGrowthChartYearlyBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGrowthChartVisitYearlyDetails");
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Int64, BizActionObj.IsopdIPd);
                if (BizActionObj.DrID != null)
                {
                    if (BizActionObj.DrID > 0)
                        dbServer.AddInParameter(command, "DrID", DbType.Int64, BizActionObj.DrID);
                    else
                        dbServer.AddInParameter(command, "DrID", DbType.Int64, null);
                }
                else
                    dbServer.AddInParameter(command, "DrID", DbType.Int64, null);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.GrowthChartDetailList == null)
                        BizActionObj.GrowthChartDetailList = new List<clsGrowthChartVO>();
                    while (reader.Read())
                    {
                        clsGrowthChartVO objGrowthChartDetail = new clsGrowthChartVO();
                        BizActionObj.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                        objGrowthChartDetail.Id = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));

                        objGrowthChartDetail.VisitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["VisitID"]));
                        //  objGrowthChartDetail.PatientID=Convert.ToInt64(DALHelper.HandleIntegerNull(reader["PatientID"]));
                        objGrowthChartDetail.UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitId"]));
                        objGrowthChartDetail.ChiefComplaint = Convert.ToString(DALHelper.HandleDBNull(reader["ComplentChief"]));

                        objGrowthChartDetail.Height = Convert.ToDouble(DALHelper.HandleDBNull(reader["Height"]));
                        objGrowthChartDetail.Weight = Convert.ToDouble(DALHelper.HandleDBNull(reader["Weight"]));
                        if (objGrowthChartDetail.Height > 0)
                            objGrowthChartDetail.BMI = Math.Round((objGrowthChartDetail.Weight / (objGrowthChartDetail.Height * objGrowthChartDetail.Height / 10000)), 2);

                        objGrowthChartDetail.SBP = Convert.ToDouble(DALHelper.HandleDBNull(reader["SBP"]));
                        objGrowthChartDetail.DBP = Convert.ToDouble(DALHelper.HandleDBNull(reader["DBP"]));
                        objGrowthChartDetail.HC = Convert.ToDouble(DALHelper.HandleDBNull(reader["HC"]));
                        objGrowthChartDetail.Pulse = Convert.ToDouble(DALHelper.HandleDBNull(reader["Pulse"]));
                        objGrowthChartDetail.RR = Convert.ToDouble(DALHelper.HandleDBNull(reader["RR"]));
                        objGrowthChartDetail.Spo2 = Convert.ToDouble(DALHelper.HandleDBNull(reader["Spo2"]));
                        objGrowthChartDetail.Temprature = Convert.ToDouble(DALHelper.HandleDBNull(reader["Temprature"]));

                        objGrowthChartDetail.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        objGrowthChartDetail.MobileCountryCode = Convert.ToInt64(DALHelper.HandleDBNull(reader["MobileCountryCode"]));
                        objGrowthChartDetail.MOB = Convert.ToString(DALHelper.HandleDBNull(reader["MOB"]));
                        objGrowthChartDetail.VisitDate = Convert.ToDateTime(DALHelper.HandleDate(reader["VisitDate"]));
                        objGrowthChartDetail.DOB = Convert.ToDateTime(DALHelper.HandleDate(reader["DOB"]));

                        objGrowthChartDetail.Age = Convert.ToInt64(DALHelper.HandleDBNull(reader["Age"]));
                        objGrowthChartDetail.AgeInMonth = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInMonth"]));
                        objGrowthChartDetail.GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]));
                        objGrowthChartDetail.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        objGrowthChartDetail.AgeInYearMonthDays = Convert.ToString(DALHelper.HandleDBNull(reader["AgeInYearMonthDays"]));
                        objGrowthChartDetail.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        objGrowthChartDetail.DrName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));
                        objGrowthChartDetail.OPD = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNo"]));

                        if ((objGrowthChartDetail.Height > 0 && objGrowthChartDetail.Weight > 0) || objGrowthChartDetail.HC > 0)
                            if (objGrowthChartDetail.AgeInMonth <= 240)
                                objGrowthChartDetail.ViewDetails = true;
                            else
                                objGrowthChartDetail.ViewDetails = false;
                        BizActionObj.GrowthChartDetailList.Add(objGrowthChartDetail);
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
        #endregion

        public override IValueObject GetPatientDrugAllergies(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientDrugAllergiesBizActionVO BizAction = valueObject as clsGetPatientDrugAllergiesBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientDrugAllergies");
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizAction.DrugAllergiesList == null)
                        BizAction.DrugAllergiesList = new List<clsGetDrugForAllergies>();
                    while (reader.Read())
                    {
                        clsGetDrugForAllergies Obj = new clsGetDrugForAllergies();
                        Obj.DrugId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["DrugId"]));
                        Obj.DrugName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        BizAction.DrugAllergiesList.Add(Obj);
                    }
                }
            }
            catch (Exception ee)
            {
            }
            return valueObject;
        }
        public override IValueObject GetPatientDrugAllergiesList(IValueObject valueObject, clsUserVO UserVo)
        {
            ClsGetPatientdrugAllergiesListBizActionVO BizActionObj = valueObject as ClsGetPatientdrugAllergiesListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDrugAllergiesList");
                dbServer.AddInParameter(command, "PatientId", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "unitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "DrugId", DbType.Int64, BizActionObj.DrugID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DrugAllergiesList == null)
                        BizActionObj.DrugAllergiesList = new List<clsGetDrugForAllergies>();
                    while (reader.Read())
                    {
                        clsGetDrugForAllergies Obj = new clsGetDrugForAllergies();
                        Obj.DrugId = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["DrugId"]));
                        //Obj.DrugName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        BizActionObj.DrugAllergiesList.Add(Obj);
                    }
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
            return valueObject;
        }
        public override IValueObject AddUpdateFollowupNote(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientFollowupNotesBizActionVO BizAction = valueObject as clsAddUpdatePatientFollowupNotesBizActionVO;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientEMRFollowUPNote");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizAction.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Doctorid", DbType.Int32, BizAction.DoctorID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                dbServer.AddInParameter(command, "FollowUpNote", DbType.String, BizAction.CurrentFollowUpNotes.Notes);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.CurrentFollowUpNotes.ID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
                int intStatus = dbServer.ExecuteNonQuery(command);
                // BizAction.CurrentChiefComplaints.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
            }
            return BizAction;
        }
        public override IValueObject AddUpdateCostNote(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePatientcostNoteBizActionVO BizAction = valueObject as clsAddUpdatePatientcostNoteBizActionVO;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientcostNote");   //CIMS_AddUpdatePatientEMRFollowUPNote
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizAction.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Doctorid", DbType.Int32, BizAction.DoctorID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                dbServer.AddInParameter(command, "FollowUpNote", DbType.String, BizAction.CurrentFollowUpNotes.Notes);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.CurrentFollowUpNotes.ID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
                int intStatus = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
            }
            return BizAction;
        }
        public override IValueObject GetPatientFollowUpNotes(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientFollowUpNoteBizActionVO BizAction = valueObject as clsGetPatientFollowUpNoteBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientFollowUPNotes");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizAction.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
                dbServer.AddInParameter(command, "Doctorid", DbType.Int64, BizAction.DoctorID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizAction.FollowUpNotesList == null)
                        BizAction.FollowUpNotesList = new List<clsEMRFollowNoteVO>();
                    while (reader.Read())
                    {
                        clsEMRFollowNoteVO Obj = new clsEMRFollowNoteVO();
                        Obj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Obj.Notes = Convert.ToString(DALHelper.HandleDBNull(reader["FollowUpNote"]));
                        Obj.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        BizAction.CurrentFollowUPNotes = Obj;
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }

            return BizAction;

        }

        public override IValueObject GetPatientCostNotes(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientCostBizActionVO BizAction = valueObject as clsGetPatientCostBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientCostNotes");  //CIMS_GetPatientFollowUPNotes
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizAction.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
                dbServer.AddInParameter(command, "Doctorid", DbType.Int64, BizAction.DoctorID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizAction.FollowUpNotesList == null)
                        BizAction.FollowUpNotesList = new List<clsEMRFollowNoteVO>();
                    while (reader.Read())
                    {
                        clsEMRFollowNoteVO Obj = new clsEMRFollowNoteVO();
                        Obj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Obj.Notes = Convert.ToString(DALHelper.HandleDBNull(reader["FollowUpNote"]));
                        Obj.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        BizAction.CurrentFollowUPNotes = Obj;
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }

            return BizAction;

        }
        public override IValueObject GetPatientReferreddetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetReferredDetailsBizActionVO BizActionObj = valueObject as clsGetReferredDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientEMRreferredDoctorDetails");
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizActionObj.VisitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "ISOPDIPD", DbType.Int64, BizActionObj.IsOPDIPD);
                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "@startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                //dbServer.AddInParameter(command, "@maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.DoctorSuggestedServiceDetailforReferred == null)
                        BizActionObj.DoctorSuggestedServiceDetailforReferred = new List<clsDoctorSuggestedServiceDetailVO>();
                    while (reader.Read())
                    {
                        clsDoctorSuggestedServiceDetailVO Obj = new clsDoctorSuggestedServiceDetailVO();
                        //Obj.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctor"]));
                        //Obj.DoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredDoctorCode"]));
                        Obj.ReferalDoctor = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]));
                        Obj.ReferalDoctorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ID"]));
                        Obj.Specialization = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        //Obj.SpecializationCode = Convert.ToString(DALHelper.HandleDBNull(reader["SpecializationCode"]));
                        //Obj.ReferalSpecialization = Convert.ToString(DALHelper.HandleDBNull(reader["ReferalSpecialization"]));
                        //Obj.ReferalSpecializationCode = Convert.ToString(DALHelper.HandleDBNull(reader["ReferalSpecializationCode"]));
                        //Obj.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        BizActionObj.DoctorSuggestedServiceDetailforReferred.Add(Obj);
                    }
                    //reader.NextResult();
                    //BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }

            return valueObject;
        }

        public override IValueObject GetPatientPastHistroScopyAndLaproscopy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientPastHistroScopyandLaproscopyHistoryBizActionVO BizAction = valueObject as clsGetPatientPastHistroScopyandLaproscopyHistoryBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_GetEMRPatientPastHistory");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizAction.TemplateID);
                dbServer.AddInParameter(command, "visitID", DbType.Int64, BizAction.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ISOPDIPD", DbType.Boolean, BizAction.IsOpdIpd);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizAction.MaximumRows);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizAction.PatientPasthistory == null)
                        BizAction.PatientPasthistory = new List<GetPastHistroandlapro>();
                    while (reader.Read())
                    {
                        GetPastHistroandlapro objPrescriptionVO = new GetPastHistroandlapro();
                        objPrescriptionVO.DoctorName = Convert.ToString(reader["DoctorName"]);
                        objPrescriptionVO.DateTime = Convert.ToString(DALHelper.HandleDBNull(reader["Date"]));
                        objPrescriptionVO.EmrId = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmrID"]));
                        BizAction.PatientPasthistory.Add(objPrescriptionVO);
                    }
                    reader.NextResult();
                    BizAction.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
            }
            catch (Exception ee)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;

        }

        public override IValueObject UploadPatientImageFromHystroScopy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUploadPatientHystoLapBizActionVO BizAction = valueObject as clsUploadPatientHystoLapBizActionVO;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                string ImgName = GetImageName(BizAction.TemplateID, BizAction.PatientID, BizAction.VisitID, UserVo.UserLoginInfo.UnitId, BizAction.IsOPDIPD);
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddImageForHistroScopyAndLaproScopy");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizAction.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int32, BizAction.TemplateID);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "ImgPath", DbType.String, ImgName);
                dbServer.AddInParameter(command, "IsOPDIPD", DbType.Boolean, BizAction.IsOPDIPD);
                int intStatus = dbServer.ExecuteNonQuery(command);
                MemoryStream ms = new MemoryStream(BizAction.Image);
                Image image = Image.FromStream(ms);
                Image img = (Image)(new Bitmap(image, new Size(150, 120)));
                ImageConverter converter = new ImageConverter();
                MemoryStream mem = new MemoryStream();
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                img.Save(ImgSaveLocation + ImgName, jpgEncoder, myEncoderParameters);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
            }
            return valueObject;
        }
        public ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        public string GetImageName(long TemplateID, long PatientID, long VisitID, long UnitID, Boolean OPDIPD)
        {
            string ImgName = null;
            Random random = new Random();
            long RandomNumber = random.Next(111111, 666666);
            if (TemplateID == 23)
            {
                ImgName = "HystroScopy-" + Convert.ToString(PatientID) + "-" + Convert.ToString(VisitID) + "-" + Convert.ToString(UnitID) + "-" + Convert.ToString(RandomNumber) + "-" + Convert.ToString(OPDIPD) + ".png";
            }
            else
            {
                ImgName = "LaproScopy-" + Convert.ToString(PatientID) + "-" + Convert.ToString(VisitID) + "-" + Convert.ToString(UnitID) + "-" + Convert.ToString(RandomNumber) + "-" + Convert.ToString(OPDIPD) + ".png";
            }
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State != ConnectionState.Open) con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CheckImageDupliaction");
                dbServer.AddInParameter(command, "ImgName", DbType.String, ImgName);
                int Status = Convert.ToInt32(dbServer.ExecuteScalar(command));
                if (Status == 0)
                {
                    GetImageName(TemplateID, PatientID, VisitID, UnitID, OPDIPD);
                }
                else
                {
                    return ImgName;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
                con.Dispose();
            }
            return "";
        }

        public override IValueObject GetUploadPatientImageFromHystroScopy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientUploadedImagetHystoLapBizActionVO BizAction = valueObject as clsGetPatientUploadedImagetHystoLapBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_GetEMRPatientHistoryScopyandLaproImage");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "TemplateID", DbType.Int64, BizAction.TemplateID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, BizAction.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ISOPDIPD", DbType.Boolean,BizAction.IsOPDIPD);
                dbServer.AddInParameter(command, "IsFromOtImg", DbType.Boolean, BizAction.IsFromOtImg);
                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizAction.StartRowIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizAction.MaximumRows);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizAction.Img1 == null)
                        BizAction.Img1 = new List<ClsImages>();
                    while (reader.Read())
                    {
                        ClsImages objPrescriptionVO = new ClsImages();
                        objPrescriptionVO.ImageID = Convert.ToInt64(reader["id"]);

                        //objPrescriptionVO.ImageName = Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"]));
                        string ImgPath = Convert.ToString(DALHelper.HandleDBNull(reader["ImgPath"]));
                        objPrescriptionVO.ImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ImgPath;
                        if (BizAction.IsFromOtImg)
                        {
                            var webClient = new WebClient();
                            objPrescriptionVO.UserImage =webClient.DownloadData(objPrescriptionVO.ImageName);
                        }
                        BizAction.Img1.Add(objPrescriptionVO);
                    }
                    //reader.NextResult();
                    //BizAction.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                }
            }
            catch (Exception ee)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject deleteUploadPatientImageFromHystroScopy(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteUploadPatientHystoLapBizActionVO BizAction = valueObject as clsDeleteUploadPatientHystoLapBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand command = null;
                command = dbServer.GetStoredProcCommand("CIMS_DeleteImageHistory");
                dbServer.AddInParameter(command, "ImgID", DbType.Int64, BizAction.ImageID);
                int status = dbServer.ExecuteNonQuery(command);
                if (status == -1)
                {
                    BizAction.SuccessStatus = 1;
                }
                else
                {
                    BizAction.SuccessStatus = 0;
                }
            }
            catch
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return valueObject;
        }

        public override IValueObject GetEMRTemplateListForOT(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEMRTemplateListForOTBizActionVO BizActionObj = valueObject as clsGetEMRTemplateListForOTBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEMRTemplateForOT");
                dbServer.AddInParameter(command, "ProcedureID", DbType.String, BizActionObj.ProcedureTemplateID);

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.objEMRTemplateList == null)
                        BizActionObj.objEMRTemplateList = new List<clsEMRTemplateVO>();

                    while (reader.Read())
                    {
                        clsEMRTemplateVO objEMRTemplateVO = new clsEMRTemplateVO();
                        objEMRTemplateVO.TemplateID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objEMRTemplateVO.Title = (string)DALHelper.HandleDBNull(reader["Title"]);
                        objEMRTemplateVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        BizActionObj.objEMRTemplateList.Add(objEMRTemplateVO);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return valueObject;
        }


        public override IValueObject GetEMRTemplateListForOTProcedure(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetEMRTemplateListBizActionVO BizActionObj = valueObject as clsGetEMRTemplateListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetEMRTemplate");

                dbServer.AddInParameter(command, "IsProcedureTemplate", DbType.Boolean, BizActionObj.IsProcedureTemplate);

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.objEMRTemplateList == null)
                        BizActionObj.objEMRTemplateList = new List<clsEMRTemplateVO>();

                    while (reader.Read())
                    {
                        //clsEMRTemplateVO objEMRTemplateVO = new clsEMRTemplateVO();
                        MasterListItem objMasterList = new MasterListItem();

                        //objEMRTemplateVO.TemplateID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        //objEMRTemplateVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        //objEMRTemplateVO.Title = (string)DALHelper.HandleDBNull(reader["Title"]);
                        //objEMRTemplateVO.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        //objEMRTemplateVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);

                        objMasterList.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objMasterList.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objMasterList.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));


                        //BizActionObj.objEMRTemplateList.Add(objEMRTemplateVO);
                        BizActionObj.objMasterList.Add(objMasterList);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                // logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                // logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }
            return valueObject;
        }
    }
}
