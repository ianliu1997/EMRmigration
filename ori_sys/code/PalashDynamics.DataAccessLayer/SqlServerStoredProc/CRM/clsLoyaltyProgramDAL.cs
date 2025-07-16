using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using System.Data;
using com.seedhealthcare.hms.CustomExceptions;
using PalashDynamics.ValueObjects.CRM;
using System.Data.Common;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM;
using PalashDynamics.ValueObjects.CRM.LoyaltyProgram;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
  public class clsLoyaltyProgramDAL:clsBaseLoyaltyProgramDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsLoyaltyProgramDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql Object
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
                #endregion

            }

            catch (Exception ex)
            {
                throw;
            }


        }

        public override IValueObject AddLoyaltyProgram(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con=null;
            DbTransaction trans = null;
            clsAddLoyaltyProgramBizActionVO BizAction = (clsAddLoyaltyProgramBizActionVO)valueObject;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();
            

                clsLoyaltyProgramVO ObjLoyaltyProgram = BizAction.LoyaltyProgramDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgram");

                dbServer.AddInParameter(command, "Description", DbType.String, ObjLoyaltyProgram.LoyaltyProgramName.Trim());
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, ObjLoyaltyProgram.EffectiveDate);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, ObjLoyaltyProgram.ExpiryDate);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, ObjLoyaltyProgram.TariffID);
                dbServer.AddInParameter(command, "IsFamily", DbType.Boolean, ObjLoyaltyProgram.IsFamily);
                if (ObjLoyaltyProgram.Remark !=null)
                    dbServer.AddInParameter(command, "Remark", DbType.String, ObjLoyaltyProgram.Remark.Trim());
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

           
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjLoyaltyProgram.ID);
                int intStatus = dbServer.ExecuteNonQuery(command,trans);
                BizAction.LoyaltyProgramDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                foreach (var ObjClinic in ObjLoyaltyProgram.ClinicList)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramClinicDetails");


                    dbServer.AddInParameter(command1, "LoyaltyProgramID", DbType.Int64, ObjLoyaltyProgram.ID);
                    dbServer.AddInParameter(command1, "LoyaltyProgramUnitID", DbType.Int64, ObjClinic.LoyaltyUnitID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjClinic.Status);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjClinic.ID);
                    
                    int iStatus = dbServer.ExecuteNonQuery(command1,trans);
                    ObjClinic.ID = (long)dbServer.GetParameterValue(command1, "ID");

                }

                foreach (var ObjCategory in ObjLoyaltyProgram.CategoryList)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramPatientCategoryDetails");


                    dbServer.AddInParameter(command1, "LoyaltyProgramID", DbType.Int64, ObjLoyaltyProgram.ID);
                    dbServer.AddInParameter(command1, "PatientCategoryID", DbType.Int64, ObjCategory.PatientCategoryID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjCategory.Status);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjCategory.ID);

                    int iStatus = dbServer.ExecuteNonQuery(command1,trans);
                    ObjCategory.ID = (long)dbServer.GetParameterValue(command1, "ID");

                }
                foreach (var ObjFamily in ObjLoyaltyProgram.FamilyList)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramFamilyDetails");


                    dbServer.AddInParameter(command1, "LoyaltyProgramID", DbType.Int64, ObjLoyaltyProgram.ID);
                    dbServer.AddInParameter(command1, "RelationID", DbType.Int64, ObjFamily.RelationID);
                    dbServer.AddInParameter(command1, "TariffID", DbType.Int64, ObjFamily.TariffID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjFamily.Status);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjFamily.ID);

                    int iStatus = dbServer.ExecuteNonQuery(command1,trans);
                    ObjFamily.ID = (long)dbServer.GetParameterValue(command1, "ID");

                }

                foreach (var ObjAttachment in ObjLoyaltyProgram.AttachmentList)
                {

                    DbCommand command4= dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramAttachmentDetails");


                    dbServer.AddInParameter(command4, "LoyaltyProgramID", DbType.Int64, ObjLoyaltyProgram.ID);
                    dbServer.AddInParameter(command4, "AttachmentFileName", DbType.String, ObjAttachment.AttachmentFileName);
                    dbServer.AddInParameter(command4, "Attachment", DbType.Binary, ObjAttachment.Attachment);
                    dbServer.AddInParameter(command4, "DocumentName", DbType.String, ObjAttachment.DocumentName);
                    dbServer.AddInParameter(command4, "Status", DbType.Boolean, ObjAttachment.Status);
                    
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjAttachment.ID);

                    int AStatus = dbServer.ExecuteNonQuery(command4,trans);
                    ObjAttachment.ID = (long)dbServer.GetParameterValue(command4, "ID");

                }
                trans.Commit();
                BizAction.SuccessStatus = 0;

            }
            catch (Exception ex)
            {
                //throw;
                BizAction.SuccessStatus = -1;
                trans.Rollback();
                BizAction.LoyaltyProgramDetails = null;

            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizAction;
        }

        public override IValueObject GetLoyaltyProgram(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLoyaltyProgramListBizActionVO BizActionObj = (clsGetLoyaltyProgramListBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetLoyaltyProgramList");

       
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");

                if (BizActionObj.LoyaltyProgramName != null && BizActionObj.LoyaltyProgramName.Length != 0)
                    dbServer.AddInParameter(command, "Description", DbType.String, BizActionObj.LoyaltyProgramName +"%");
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.sortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


             

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    if (BizActionObj.LoyaltyProgramList == null)
                        BizActionObj.LoyaltyProgramList = new List<clsLoyaltyProgramVO>();
                    while (reader.Read())
                    {
                        clsLoyaltyProgramVO objLoyaltyProgramVO = new clsLoyaltyProgramVO();
                        objLoyaltyProgramVO.ID = (long)reader["ID"];
                        objLoyaltyProgramVO.LoyaltyProgramName = (string)reader["Description"];
                        objLoyaltyProgramVO.EffectiveDate = (DateTime)reader["EffectiveDate"];
                        objLoyaltyProgramVO.ExpiryDate = (DateTime)reader["ExpiryDate"];
                        objLoyaltyProgramVO.Status = (bool)reader["Status"];
                        //objLoyaltyProgramVO.PatientCategoryID = (long)reader["PatientCategoryID"];
                        //objLoyaltyProgramVO.PatientCategory = (string)reader["PatientCategory"];
                        objLoyaltyProgramVO.TariffID=(long)reader["TariffID"];
                        objLoyaltyProgramVO.Tariff = (string)reader["Tariff"];

                        BizActionObj.LoyaltyProgramList.Add(objLoyaltyProgramVO);
                    }
                   
                }

                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }


            finally
            {

            }
            return BizActionObj;
        }

        public override IValueObject UpdateLoyaltyProgram(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans=null;
            DbConnection con = null;
            clsUpdateLoyaltyProgramBizActionVO BizActionObj = (clsUpdateLoyaltyProgramBizActionVO)valueObject;
            try
            {
                con = dbServer.CreateConnection();
                if (con.State == ConnectionState.Closed) con.Open();
                trans = con.BeginTransaction();

                clsLoyaltyProgramVO objLoyaltyProgramVO = BizActionObj.LoyaltyProgram;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateLoyaltyProgram");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objLoyaltyProgramVO.ID);
                dbServer.AddInParameter(command, "Description", DbType.String, objLoyaltyProgramVO.LoyaltyProgramName.Trim());
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objLoyaltyProgramVO.EffectiveDate);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objLoyaltyProgramVO.ExpiryDate);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, objLoyaltyProgramVO.TariffID);
                dbServer.AddInParameter(command, "IsFamily", DbType.Boolean, objLoyaltyProgramVO.IsFamily);
                if (objLoyaltyProgramVO.Remark !=null)
                    dbServer.AddInParameter(command, "Remark", DbType.String, objLoyaltyProgramVO.Remark.Trim());
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionobj.SuccessStatus);

                int intStatus = dbServer.ExecuteNonQuery(command,trans);
               
               // BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


                if (objLoyaltyProgramVO.ClinicList != null && objLoyaltyProgramVO.ClinicList.Count > 0)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_DeleteLoyaltyProgramClinicDetails");

                    dbServer.AddInParameter(command3, "LoyaltyProgramID", DbType.Int64, objLoyaltyProgramVO.ID);
                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
             
                    int intStatus2 = dbServer.ExecuteNonQuery(command3,trans);
                }

                if (objLoyaltyProgramVO.CategoryList != null && objLoyaltyProgramVO.CategoryList.Count > 0)
                {
                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_DeleteLoyaltyProgramPatientCategoryDetails");

                    dbServer.AddInParameter(command4, "LoyaltyProgramID", DbType.Int64, objLoyaltyProgramVO.ID);
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    int intStatus3 = dbServer.ExecuteNonQuery(command4,trans);
                }


                foreach (var ObjClinic in objLoyaltyProgramVO.ClinicList)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramClinicDetails");


                    dbServer.AddInParameter(command1, "LoyaltyProgramID", DbType.Int64, objLoyaltyProgramVO.ID);
                    dbServer.AddInParameter(command1, "LoyaltyProgramUnitID", DbType.Int64, ObjClinic.LoyaltyUnitID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjClinic.Status);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjClinic.ID);

                    int iStatus = dbServer.ExecuteNonQuery(command1,trans);
                    ObjClinic.ID = (long)dbServer.GetParameterValue(command1, "ID");

                }

                foreach (var ObjCategory in objLoyaltyProgramVO.CategoryList)
                {

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramPatientCategoryDetails");


                    dbServer.AddInParameter(command2, "LoyaltyProgramID", DbType.Int64, objLoyaltyProgramVO.ID);
                    dbServer.AddInParameter(command2, "PatientCategoryID", DbType.Int64, ObjCategory.PatientCategoryID);
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, ObjCategory.Status);
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjCategory.ID);

                    int Status = dbServer.ExecuteNonQuery(command2,trans);
                    ObjCategory.ID = (long)dbServer.GetParameterValue(command2, "ID");

                }


                if (objLoyaltyProgramVO.FamilyList != null && objLoyaltyProgramVO.FamilyList.Count > 0)
                {

                    DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_DeleteLoyaltyProgramFamilyDetails");

                    dbServer.AddInParameter(command5, "LoyaltyProgramID", DbType.Int64, objLoyaltyProgramVO.ID);
                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    int intStatus5 = dbServer.ExecuteNonQuery(command5,trans);
                }

                foreach (var ObjFamily in objLoyaltyProgramVO.FamilyList)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramFamilyDetails");


                    dbServer.AddInParameter(command1, "LoyaltyProgramID", DbType.Int64, objLoyaltyProgramVO.ID);
                    dbServer.AddInParameter(command1, "RelationID", DbType.Int64, ObjFamily.RelationID);
                    dbServer.AddInParameter(command1, "TariffID", DbType.Int64, ObjFamily.TariffID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, ObjFamily.Status);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjFamily.ID);

                    int iStatus = dbServer.ExecuteNonQuery(command1,trans);
                    ObjFamily.ID = (long)dbServer.GetParameterValue(command1, "ID");

                }

                if (objLoyaltyProgramVO.AttachmentList != null && objLoyaltyProgramVO.AttachmentList.Count > 0)
                {

                    DbCommand command6 = dbServer.GetStoredProcCommand("CIMS_DeleteLoyaltyProgramAttachmentDetailsDetails");

                    dbServer.AddInParameter(command6, "LoyaltyProgramID", DbType.Int64, objLoyaltyProgramVO.ID);
                    dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    int intStatus5 = dbServer.ExecuteNonQuery(command6,trans);
                }

                foreach (var ObjAttachment in objLoyaltyProgramVO.AttachmentList)
                {

                    DbCommand command7 = dbServer.GetStoredProcCommand("CIMS_AddLoyaltyProgramAttachmentDetails");


                    dbServer.AddInParameter(command7, "LoyaltyProgramID", DbType.Int64, objLoyaltyProgramVO.ID);
                    dbServer.AddInParameter(command7, "AttachmentFileName", DbType.String, ObjAttachment.AttachmentFileName);
                    dbServer.AddInParameter(command7, "Attachment", DbType.Binary, ObjAttachment.Attachment);
                    dbServer.AddInParameter(command7, "DocumentName", DbType.String, ObjAttachment.DocumentName);
                    dbServer.AddInParameter(command7, "Status", DbType.Boolean, ObjAttachment.Status);

                    dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command7, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjAttachment.ID);

                    int AStatus = dbServer.ExecuteNonQuery(command7,trans);
                    ObjAttachment.ID = (long)dbServer.GetParameterValue(command7, "ID");

                }
                trans.Commit();
                BizActionObj.SuccessStatus = 0;


            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.LoyaltyProgram = null;


            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetLoyaltyProgramByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLoyaltyProgramByIDBizActionVO BizAction = (clsGetLoyaltyProgramByIDBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetLoyaltyProgramByID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "LoyaltyProgramID", DbType.Int64, BizAction.ID);
             
               


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizAction.LoyaltyProgramDetails == null)
                            BizAction.LoyaltyProgramDetails = new clsLoyaltyProgramVO();

                        BizAction.LoyaltyProgramDetails.ID = (long)reader["ID"];
                        BizAction.LoyaltyProgramDetails.LoyaltyProgramName = (string)reader["Description"];
                        BizAction.LoyaltyProgramDetails.EffectiveDate = (DateTime)reader["EffectiveDate"];
                        BizAction.LoyaltyProgramDetails.ExpiryDate = (DateTime)reader["ExpiryDate"];
                        BizAction.LoyaltyProgramDetails.Remark = (string)DALHelper.HandleDBNull(reader["Remark"]);
                        BizAction.LoyaltyProgramDetails.Status = (bool)reader["Status"];
                        BizAction.LoyaltyProgramDetails.PatientCategory = (string)reader["Description"];
                        BizAction.LoyaltyProgramDetails.TariffID = (long)reader["TariffID"];
                        BizAction.LoyaltyProgramDetails.Tariff = (string)reader["Tariff"];
                        BizAction.LoyaltyProgramDetails.IsFamily = (bool)reader["IsFamily"];

                    }
                }

                reader.NextResult();


                if (reader.HasRows)
                {
                    BizAction.LoyaltyProgramDetails.CategoryList = new List<clsLoyaltyProgramPatientCategoryVO>();
                    while (reader.Read())
                    {
                        clsLoyaltyProgramPatientCategoryVO objCategory = new clsLoyaltyProgramPatientCategoryVO();
                        objCategory.LoyaltyProgramID = (long)DALHelper.HandleDBNull(reader["LoyaltyProgramID"]);
                        objCategory.PatientCategoryID = (long)DALHelper.HandleDBNull(reader["PatientCategoryID"]);
                        objCategory.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objCategory.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizAction.LoyaltyProgramDetails.CategoryList.Add(objCategory);
                    }
                }

                reader.NextResult();

                if (reader.HasRows)
                {
                    BizAction.LoyaltyProgramDetails.ClinicList = new List<clsLoyaltyClinicVO>();
                    while (reader.Read())
                    {
                        clsLoyaltyClinicVO objClinic = new clsLoyaltyClinicVO();
                        objClinic.LoyaltyProgramID = (long)DALHelper.HandleDBNull(reader["LoyaltyProgramID"]);
                        objClinic.LoyaltyUnitID = (long)DALHelper.HandleDBNull(reader["LoyaltyProgramUnitID"]);
                        objClinic.LoyaltyUnitDescription = (string)DALHelper.HandleDBNull(reader["Description"]);
                        objClinic.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizAction.LoyaltyProgramDetails.ClinicList.Add(objClinic);
                    }
                }

                reader.NextResult();

                if (reader.HasRows)
                {
                    BizAction.LoyaltyProgramDetails.FamilyList = new List<clsLoyaltyProgramFamilyDetails>();
                    while (reader.Read())
                    {
                        clsLoyaltyProgramFamilyDetails objFamily = new clsLoyaltyProgramFamilyDetails();
                        objFamily.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objFamily.LoyaltyProgramID = (long)DALHelper.HandleDBNull(reader["LoyaltyProgramID"]);
                        objFamily.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objFamily.RelationID = (long)DALHelper.HandleDBNull(reader["RelationID"]);
                        objFamily.Relation = (string)DALHelper.HandleDBNull(reader["Relation"]);
                        objFamily.TariffID = (long)DALHelper.HandleDBNull(reader["TariffID"]);
                        objFamily.Tariff = (string)DALHelper.HandleDBNull(reader["Tariff"]);
                        objFamily.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        BizAction.LoyaltyProgramDetails.FamilyList.Add(objFamily);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    BizAction.LoyaltyProgramDetails.AttachmentList = new List<clsLoyaltyAttachmentVO>();
                    while (reader.Read())
                    {
                        clsLoyaltyAttachmentVO objAttachment = new clsLoyaltyAttachmentVO();
                        objAttachment.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objAttachment.LoyaltyProgramID = (long)DALHelper.HandleDBNull(reader["LoyaltyProgramID"]);
                        objAttachment.DocumentName = (string)DALHelper.HandleDBNull(reader["DocumentName"]);
                        objAttachment.Attachment = (byte[])DALHelper.HandleDBNull(reader["Attachment"]);
                        objAttachment.AttachmentFileName = (string)DALHelper.HandleDBNull(reader["AttachmentFileName"]);
                        objAttachment.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);


                        BizAction.LoyaltyProgramDetails.AttachmentList.Add(objAttachment);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            return BizAction;
        }

        public override IValueObject GetLoyaltyProgramTariffByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLoyaltyProgramTariffByIDBizActionVO BizAction = (clsGetLoyaltyProgramTariffByIDBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillTariff");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
           


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizAction.Details == null)
                            BizAction.Details = new clsLoyaltyProgramVO();

                        BizAction.Details.ID = (long)reader["ID"];                                       
                        BizAction.Details.TariffID = (long)reader["TariffID"];
                        BizAction.Details.Tariff = (string)reader["Description"];
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            return BizAction;
  
        }

        public override IValueObject GetRelationMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRelationMasterListBizActionVO BizActionObj = (clsGetRelationMasterListBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRelationList");
             

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.FamilyList == null)
                        BizActionObj.FamilyList = new List<clsLoyaltyProgramFamilyDetails>();
                    while (reader.Read())
                    {
                        clsLoyaltyProgramFamilyDetails ObjLoyalty = new clsLoyaltyProgramFamilyDetails();
                        ObjLoyalty.RelationID = (long)reader["ID"];
                        ObjLoyalty.RelationCode = reader["Code"].ToString();
                        ObjLoyalty.RelationDescription = reader["Description"].ToString();
                        ObjLoyalty.Status =(bool)reader["Status"];

                        BizActionObj.FamilyList.Add(ObjLoyalty);
                    }
                }

                reader.Close();

            }

            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }

        public override IValueObject GetCategoryList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLoyaltyPatientCategoryBizActionVO BizActionObj = (clsGetLoyaltyPatientCategoryBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetLoyaltyPatientCategory");
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.CategoryList == null)
                        BizActionObj.CategoryList = new List<clsLoyaltyProgramPatientCategoryVO>();
                    while (reader.Read())
                    {
                        clsLoyaltyProgramPatientCategoryVO ObjLoyalty = new clsLoyaltyProgramPatientCategoryVO();
                        ObjLoyalty.PatientCategoryID = (long)reader["ID"];
                        ObjLoyalty.Description = reader["Description"].ToString();
                        ObjLoyalty.Status = (bool)reader["Status"];

                        BizActionObj.CategoryList.Add(ObjLoyalty);
                    }
                }

                reader.Close();

            }

            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }
       
        public override IValueObject GetClinicList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLoyaltyClinicBizActionVO BizActionObj = (clsGetLoyaltyClinicBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetLoyaltyClinics");
          

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ClinicList == null)
                        BizActionObj.ClinicList = new List<clsLoyaltyClinicVO>();
                    while (reader.Read())
                    {
                        clsLoyaltyClinicVO ObjLoyalty = new clsLoyaltyClinicVO();
                        ObjLoyalty.LoyaltyUnitID = (long)reader["ID"];
                        ObjLoyalty.LoyaltyUnitDescription = reader["Description"].ToString();
                        ObjLoyalty.Status = (bool)reader["Status"];

                        BizActionObj.ClinicList.Add(ObjLoyalty);
                    }
                }

                reader.Close();

            }

            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
            return BizActionObj;
        }

        public override IValueObject GetFamilyDetailsByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLoyaltyProgramFamilyByIdBizActionVO BizAction = (clsGetLoyaltyProgramFamilyByIdBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetLoyaltyFamilyDetailsbyID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
       


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizAction.FamilyDetails == null)
                            BizAction.FamilyDetails = new clsLoyaltyProgramFamilyDetails();

                        BizAction.FamilyDetails.ID = (long)reader["ID"];
                        BizAction.FamilyDetails.Tariff = (string)reader["Tariff"];
                        BizAction.FamilyDetails.Relation = (string)reader["Relation"];
                        BizAction.FamilyDetails.TariffID = (long)reader["TariffID"];
                        BizAction.FamilyDetails.RelationID = (long)reader["RelationID"];


                    }
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            return BizAction;

        }

        public override IValueObject GetAttachmentDetailsByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLoyaltyProgramAttachmentByIdBizActionVO BizAction = (clsGetLoyaltyProgramAttachmentByIdBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetLoyaltyAttachmentbyID");
                DbDataReader reader;
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
              


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizAction.AttachmentDetails == null)
                            BizAction.AttachmentDetails = new clsLoyaltyAttachmentVO();

                        BizAction.AttachmentDetails.ID = (long)reader["ID"];
                        BizAction.AttachmentDetails.LoyaltyProgramID = (long)reader["LoyaltyProgramID"];
                        BizAction.AttachmentDetails.DocumentName = (string)reader["DocumentName"];
                        BizAction.AttachmentDetails.AttachmentFileName = (string)reader["AttachmentFileName"];
                        BizAction.AttachmentDetails.Attachment = (byte[])DALHelper.HandleDBNull(reader["Attachment"]);
                        BizAction.AttachmentDetails.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                      


                    }
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            return BizAction;
            
        }
        public override IValueObject FillFamilyTariffUsingRelationID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsFillFamilyTariffUsingRelationIDBizActionVO BizAction = (clsFillFamilyTariffUsingRelationIDBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillTariffUsingRelation");
                DbDataReader reader;
                dbServer.AddInParameter(command, "RelationID", DbType.Int64, BizAction.RelationID);
                dbServer.AddInParameter(command, "LoyaltyProgramID", DbType.Int64, BizAction.LoyaltyProgramID);
              


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (BizAction.Details == null)
                            BizAction.Details = new clsLoyaltyProgramFamilyDetails();
                         
                        BizAction.Details.LoyaltyProgramID = (long)reader["LoyaltyProgramID"];
                        BizAction.Details.RelationID = (long)reader["RelationID"];
                        BizAction.Details.TariffID = (long)reader["TariffID"];
                        BizAction.Details.Tariff = (string)reader["Tariff"];

                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }

            return BizAction;
        }

        public override IValueObject FillCardTypeCombo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsFillCardTypeComboBizActionVO BizActionOBj = (clsFillCardTypeComboBizActionVO)valueObject;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_FillCardTypeCombo");
            
               
                DbDataReader reader;
                           
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizActionOBj.MasterList == null)
                    {
                        BizActionOBj.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        BizActionOBj.MasterList.Add(new MasterListItem
                            (
                            (long)reader["Id"], 
                            reader["Description"].ToString(),
                            (bool)reader["Status"]
                            )
                            );
                    }
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                throw;
            }
            return BizActionOBj;
        }
    }
}
