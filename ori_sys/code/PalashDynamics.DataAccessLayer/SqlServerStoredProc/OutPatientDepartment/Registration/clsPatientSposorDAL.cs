using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using System.Data;
using System.Data.Common;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using com.seedhealthcare.hms.CustomExceptions;
using System.Reflection;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsPatientSposorDAL : clsBasePatientSposorDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsPatientSposorDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override IValueObject AddPatientSponsor(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientSponsorBizActionVO BizActionObj = valueObject as clsAddPatientSponsorBizActionVO;

            if (BizActionObj.PatientSponsorDetails.ID == 0)
                BizActionObj = AddPatientSponsorDetails(BizActionObj, UserVo);
            else
                BizActionObj = UpdatePatientSponsorDetails(BizActionObj);

            return valueObject;

        }

        private clsAddPatientSponsorBizActionVO AddPatientSponsorDetails(clsAddPatientSponsorBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsPatientSponsorVO objDetailsVO = BizActionObj.PatientSponsorDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientSponsor");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitId);
                dbServer.AddInParameter(command, "PatientSourceID", DbType.Int64, objDetailsVO.PatientSourceID);
                dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objDetailsVO.PatientCategoryID);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, objDetailsVO.CompanyID);
                dbServer.AddInParameter(command, "AssociatedCompanyID", DbType.Int64, objDetailsVO.AssociatedCompanyID);
                if (objDetailsVO.ReferenceNo != null) objDetailsVO.ReferenceNo = objDetailsVO.ReferenceNo.Trim();
                dbServer.AddInParameter(command, "ReferenceNo", DbType.String, objDetailsVO.ReferenceNo);
                dbServer.AddInParameter(command, "CreditLimit", DbType.Double, objDetailsVO.CreditLimit);
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objDetailsVO.EffectiveDate);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objDetailsVO.ExpiryDate);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, objDetailsVO.TariffID);
                if (objDetailsVO.EmployeeNo != null) objDetailsVO.EmployeeNo = objDetailsVO.EmployeeNo.Trim();
                dbServer.AddInParameter(command, "EmployeeNo", DbType.String, objDetailsVO.EmployeeNo);
                dbServer.AddInParameter(command, "DesignationID", DbType.Int64, objDetailsVO.DesignationID);
                if (objDetailsVO.Remark != null) objDetailsVO.Remark = objDetailsVO.Remark.Trim();
                dbServer.AddInParameter(command, "Remark", DbType.String, objDetailsVO.Remark);

                //dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId); //objDetailsVO.UnitId);
                //dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, objDetailsVO.CreatedUnitId);

                dbServer.AddInParameter(command, "MemberRelationID", DbType.Int64, objDetailsVO.MemberRelationID);

                //dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objDetailsVO.AddedBy);
                //if (objDetailsVO.AddedOn != null) objDetailsVO.AddedOn = objDetailsVO.AddedOn.Trim();
                //dbServer.AddInParameter(command, "AddedOn", DbType.String, objDetailsVO.AddedOn);
                //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                //if (objDetailsVO.AddedWindowsLoginName != null) objDetailsVO.AddedWindowsLoginName = objDetailsVO.AddedWindowsLoginName.Trim();
                //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objDetailsVO.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "IsDupliSponser", DbType.Boolean, objDetailsVO.IsDupliSponser);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.PatientSponsorDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
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

        private clsAddPatientSponsorBizActionVO UpdatePatientSponsorDetails(clsAddPatientSponsorBizActionVO BizActionObj)
        {
            try
            {
                clsPatientSponsorVO objDetailsVO = BizActionObj.PatientSponsorDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdatePatientSponsor");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitId);
                dbServer.AddInParameter(command, "PatientSourceID", DbType.Int64, objDetailsVO.PatientSourceID);
                dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objDetailsVO.PatientCategoryID);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, objDetailsVO.CompanyID);
                dbServer.AddInParameter(command, "AssociatedCompanyID", DbType.Int64, objDetailsVO.AssociatedCompanyID);
                if (objDetailsVO.ReferenceNo != null) objDetailsVO.ReferenceNo = objDetailsVO.ReferenceNo.Trim();
                dbServer.AddInParameter(command, "ReferenceNo", DbType.String, objDetailsVO.ReferenceNo);
                dbServer.AddInParameter(command, "CreditLimit", DbType.Double, objDetailsVO.CreditLimit);
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objDetailsVO.EffectiveDate);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objDetailsVO.ExpiryDate);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, objDetailsVO.TariffID);
                if (objDetailsVO.EmployeeNo != null) objDetailsVO.EmployeeNo = objDetailsVO.EmployeeNo.Trim();
                dbServer.AddInParameter(command, "EmployeeNo", DbType.String, objDetailsVO.EmployeeNo);
                dbServer.AddInParameter(command, "DesignationID", DbType.Int64, objDetailsVO.DesignationID);
                if (objDetailsVO.Remark != null) objDetailsVO.Remark = objDetailsVO.Remark.Trim();
                dbServer.AddInParameter(command, "Remark", DbType.String, objDetailsVO.Remark);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objDetailsVO.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, objDetailsVO.UpdatedUnitId);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objDetailsVO.UpdatedBy);
                if (objDetailsVO.UpdatedOn != null) objDetailsVO.UpdatedOn = objDetailsVO.UpdatedOn.Trim();
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objDetailsVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objDetailsVO.UpdatedDateTime);
                if (objDetailsVO.UpdatedWindowsLoginName != null) objDetailsVO.UpdatedWindowsLoginName = objDetailsVO.UpdatedWindowsLoginName.Trim();
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objDetailsVO.UpdatedWindowsLoginName);

                dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);

                dbServer.AddParameter(command, "ResultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.SuccessStatus);

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

            return BizActionObj;
        }


        private clsPatientSponsorCardDetailsVO AddPatientSponsorCard(clsPatientSponsorCardDetailsVO valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            bool CurrentMethodExecutionStatus = true;
            //  clsAddPatientSponsorCardBizActionVO BizActionObj = valueObject as clsAddPatientSponsorCardBizActionVO;

            try
            {
                clsPatientSponsorCardDetailsVO objDetailsVO = valueObject;// BizActionObj.PatientSponsorCardDetails;
                DbCommand command;

                command = dbServer.GetStoredProcCommand("CIMS_AddPatientSponsorCardDetails");

                dbServer.AddInParameter(command, "SponsorID", DbType.Int64, objDetailsVO.SponsorID);
                dbServer.AddInParameter(command, "Title", DbType.String, objDetailsVO.Title);
                dbServer.AddInParameter(command, "Image", DbType.Binary, objDetailsVO.Image);
                // dbServer.AddOutParameter(command, "ID", DbType.Int64, objDetailsVO.ID);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                int intStatus = dbServer.ExecuteNonQuery(command);
                //  BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                if (objDetailsVO.SponsorID == 0)
                    objDetailsVO.ID = (long)dbServer.GetParameterValue(command, "ID");
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


        public override IValueObject GetPatientSponsor(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetPatientSponsorBizActionVO BizActionObj = valueObject as clsGetPatientSponsorBizActionVO;
            try
            {
                clsPatientSponsorVO objDetailsVO = BizActionObj.PatientSponsorDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientSponsorDetails");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitId);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                DbDataReader reader;
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizActionObj.PatientSponsorDetails.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        BizActionObj.PatientSponsorDetails.PatientId = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                        BizActionObj.PatientSponsorDetails.PatientUnitId = (long)DALHelper.HandleDBNull(reader["PatientUnitId"]);
                        BizActionObj.PatientSponsorDetails.PatientSourceID = (long)DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        BizActionObj.PatientSponsorDetails.CompanyID = (long)DALHelper.HandleDBNull(reader["CompanyID"]);
                        BizActionObj.PatientSponsorDetails.AssociatedCompanyID = (long)DALHelper.HandleDBNull(reader["AssociatedCompanyID"]);
                        BizActionObj.PatientSponsorDetails.ReferenceNo = (string)DALHelper.HandleDBNull(reader["ReferenceNo"]);
                        BizActionObj.PatientSponsorDetails.CreditLimit = (double)DALHelper.HandleDBNull(reader["CreditLimit"]);
                        BizActionObj.PatientSponsorDetails.EffectiveDate = (DateTime?)DALHelper.HandleDate(reader["EffectiveDate"]);
                        BizActionObj.PatientSponsorDetails.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        BizActionObj.PatientSponsorDetails.TariffID = (long?)DALHelper.HandleDBNull(reader["TariffID"]);
                        BizActionObj.PatientSponsorDetails.EmployeeNo = (string)DALHelper.HandleDBNull(reader["EmployeeNo"]);
                        BizActionObj.PatientSponsorDetails.DesignationID = (long)DALHelper.HandleDBNull(reader["DesignationID"]);

                        BizActionObj.PatientSponsorDetails.Remark = (string)DALHelper.HandleDBNull(reader["Remark"]);

                        BizActionObj.PatientSponsorDetails.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        BizActionObj.PatientSponsorDetails.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);

                        BizActionObj.PatientSponsorDetails.CreatedUnitId = (Int64)DALHelper.HandleDBNull(reader["CreatedUnitId"]);
                        BizActionObj.PatientSponsorDetails.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                        BizActionObj.PatientSponsorDetails.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        BizActionObj.PatientSponsorDetails.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        BizActionObj.PatientSponsorDetails.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);

                        BizActionObj.PatientSponsorDetails.UpdatedUnitId = (Int64?)DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                        BizActionObj.PatientSponsorDetails.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        BizActionObj.PatientSponsorDetails.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        BizActionObj.PatientSponsorDetails.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        BizActionObj.PatientSponsorDetails.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);

                        BizActionObj.PatientSponsorDetails.ComapnyName = (string)DALHelper.HandleDBNull(reader["company"]);
                        BizActionObj.PatientSponsorDetails.AssociateComapnyName = (string)DALHelper.HandleDBNull(reader["AssociatCompany"]);
                        BizActionObj.PatientSponsorDetails.Designation = (string)DALHelper.HandleDBNull(reader["designation"]);
                        BizActionObj.PatientSponsorDetails.PatientSource = (string)DALHelper.HandleDBNull(reader["PatientSource"]);
                        BizActionObj.PatientSponsorDetails.TariffName = (string)DALHelper.HandleDBNull(reader["Tariff"]);
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

        public override IValueObject GetPatientSponsorList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientSponsorListBizActionVO BizActionObj = valueObject as clsGetPatientSponsorListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientSponsorDetails");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.SponsorID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                DbDataReader reader;

                // clsPatientGeneralVO objPatientVO = BizActionObj.PatientDetails;


                //int intStatus = dbServer.ExecuteNonQuery(command);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.PatientSponsorDetails == null)
                        BizActionObj.PatientSponsorDetails = new List<clsPatientSponsorVO>();
                    while (reader.Read())
                    {
                        clsPatientSponsorVO objPatientVO = new clsPatientSponsorVO();

                        objPatientVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objPatientVO.PatientId = (long)DALHelper.HandleDBNull(reader["PatientId"]);
                        objPatientVO.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientCategoryID"]));
                        objPatientVO.PatientCategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientCategoryName"]));
                        objPatientVO.PatientSourceID = (long)DALHelper.HandleDBNull(reader["PatientSourceID"]);
                        objPatientVO.CompanyID = (long)DALHelper.HandleDBNull(reader["CompanyID"]);
                        objPatientVO.AssociatedCompanyID = (long)DALHelper.HandleDBNull(reader["AssociatedCompanyID"]);
                        objPatientVO.ReferenceNo = (string)DALHelper.HandleDBNull(reader["ReferenceNo"]);
                        objPatientVO.CreditLimit = (double)DALHelper.HandleDBNull(reader["CreditLimit"]);
                        objPatientVO.EffectiveDate = (DateTime?)DALHelper.HandleDate(reader["EffectiveDate"]);
                        objPatientVO.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        objPatientVO.TariffID = (long?)DALHelper.HandleDBNull(reader["TariffID"]);
                        objPatientVO.EmployeeNo = (string)DALHelper.HandleDBNull(reader["EmployeeNo"]);
                        objPatientVO.DesignationID = (long)DALHelper.HandleDBNull(reader["DesignationID"]);

                        objPatientVO.Remark = (string)DALHelper.HandleDBNull(reader["Remark"]);

                        objPatientVO.UnitId = (Int64)DALHelper.HandleDBNull(reader["UnitId"]);
                        objPatientVO.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);

                        objPatientVO.CreatedUnitId = (Int64)DALHelper.HandleDBNull(reader["CreatedUnitId"]);
                        objPatientVO.AddedBy = (Int64?)DALHelper.HandleDBNull(reader["AddedBy"]);
                        objPatientVO.AddedOn = (String)DALHelper.HandleDBNull(reader["AddedOn"]);
                        objPatientVO.AddedDateTime = (DateTime?)DALHelper.HandleDate(reader["AddedDateTime"]);
                        objPatientVO.AddedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["AddedWindowsLoginName"]);

                        objPatientVO.UpdatedUnitId = (Int64?)DALHelper.HandleDBNull(reader["UpdatedUnitID"]);
                        objPatientVO.UpdatedBy = (Int64?)DALHelper.HandleDBNull(reader["UpdatedBy"]);
                        objPatientVO.UpdatedOn = (String)DALHelper.HandleDBNull(reader["UpdatedOn"]);
                        objPatientVO.UpdatedDateTime = (DateTime?)DALHelper.HandleDate(reader["UpdatedDateTime"]);
                        objPatientVO.UpdatedWindowsLoginName = (String)DALHelper.HandleDBNull(reader["UpdatedWindowsLoginName"]);

                        objPatientVO.ComapnyName = (string)DALHelper.HandleDBNull(reader["company"]);
                        objPatientVO.AssociateComapnyName = (string)DALHelper.HandleDBNull(reader["AssociatCompany"]);
                        objPatientVO.Designation = (string)DALHelper.HandleDBNull(reader["designation"]);
                        objPatientVO.PatientSource = (string)DALHelper.HandleDBNull(reader["PatientSource"]);
                        objPatientVO.TariffName = (string)DALHelper.HandleDBNull(reader["Tariff"]);

                        objPatientVO.MemberRelationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MemberRelationID"]));

                        objPatientVO.PatientSourceType = Convert.ToInt16(DALHelper.HandleDBNull(reader["PatientSourceType"])); //commented by ashish z.
                        objPatientVO.PatientSourceTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientSourceTypeID"]));

                        BizActionObj.PatientSponsorDetails.Add(objPatientVO);

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
            return valueObject;
        }

        // Added By CDS For Other Service Search Window

        public override IValueObject GetPatientSponsorCompanyList(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPatientSponsorCompanyListBizActionVO BizActionObj = (clsGetPatientSponsorCompanyListBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientSponsorCompanyDetails");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "PatientSourceID", DbType.Int64, BizActionObj.PatientSourceID);
                //dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.CheckDate);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["CompanyID"], reader["CompanyName"].ToString()));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {
            }
            return BizActionObj;
        }

        public override IValueObject GetPatientSponsorTariffList(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPatientSponsorTariffListBizActionVO BizActionObj = (clsGetPatientSponsorTariffListBizActionVO)valueObject;

            try
            {
                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientSponsorTariffDetails");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "PatientCompanyID", DbType.Int64, BizActionObj.PatientCompanyID);

                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.CheckDate);
                //dbServer.AddInParameter(command, "IsDate", DbType.Boolean, BizActionObj.IsDate.Value);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //Check whether the reader contains the records
                if (reader.HasRows)
                {
                    //if masterlist instance is null then creates new instance
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    //Reading the record from reader and stores in list
                    while (reader.Read())
                    {
                        //Add the object value in list
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["TariffID"], reader["Tariff"].ToString()));
                    }
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return BizActionObj;
        }

        public override IValueObject GetPatientPackageInfoList(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPatientPackageInfoListBizActionVO BizActionObj = (clsGetPatientPackageInfoListBizActionVO)valueObject;
            try
            {
                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientPackageDetails");
                //Adding MasterTableName as Input Parameter to filter record

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID1);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID1);
                //By Anjali.....................
                dbServer.AddInParameter(command, "IsfromCounterSale", DbType.Boolean, BizActionObj.IsfromCounterSale);
                //.................................

                //Newly Added
                dbServer.AddInParameter(command, "PatientSourceID", DbType.Int64, BizActionObj.PatientSourceID);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, BizActionObj.PatientCompanyID);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.PatientTariffID);



                //dbServer.AddInParameter(command, "PatientID1", DbType.Int64, BizActionObj.PatientID1);
                //dbServer.AddInParameter(command, "PatientUnitID1", DbType.Int64, BizActionObj.PatientUnitID1);

                //dbServer.AddInParameter(command, "PatientID2", DbType.Int64, BizActionObj.PatientID2);
                //dbServer.AddInParameter(command, "PatientUnitID2", DbType.Int64, BizActionObj.PatientUnitID2);
                //dbServer.AddInParameter(command, "PatientCompanyID", DbType.Int64, BizActionObj.PatientCompanyID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.CheckDate);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                       // BizActionObj.MasterList.Add(new MasterListItem((long)reader["PackageID"], reader["PackageName"].ToString(), (long)reader["PatientInfoID"]));
                         //Added by AJ Date 2/2/2017
                        //BizActionObj.MasterList.Add(new MasterListItem((long)reader["PackageID"], reader["PackageName"].ToString(), (long)reader["PatientInfoID"], (bool)reader["ApplicableToAll"], (double)reader["ApplicableToAllDiscount"], (double)reader["PharmacyFixedRate"], (long)reader["PackageBillID"], (long)reader["PackageBillUnitID"]));

                        //Package New Changes Commented on 26042018
                        //BizActionObj.MasterList.Add(new MasterListItem(Convert.ToInt64(reader["PackageID"]), reader["PackageName"].ToString(), Convert.ToInt64(reader["PatientInfoID"]), Convert.ToBoolean(reader["ApplicableToAll"]), Convert.ToDouble(reader["ApplicableToAllDiscount"]), Convert.ToDouble(reader["PharmacyFixedRate"]), Convert.ToInt64(reader["PackageBillID"]), Convert.ToInt64(reader["PackageBillUnitID"]), Convert.ToInt64(reader["ChargeID"]), Convert.ToDouble((Decimal)reader["PackageConsumptionAmount"]), Convert.ToDouble(reader["OPDConsumption"]), Convert.ToDouble(reader["OPDExcludeServices"]),Convert.ToDouble(reader["TotalPackageAdvance"])));
                         
                        //Package New Changes Added on 26042018
                        BizActionObj.MasterList.Add(new MasterListItem(Convert.ToInt64(reader["PackageID"]), reader["PackageName"].ToString(), Convert.ToInt64(reader["PatientInfoID"]), Convert.ToBoolean(reader["ApplicableToAll"]), Convert.ToDouble(reader["ApplicableToAllDiscount"]), Convert.ToDouble(reader["PharmacyFixedRate"]), Convert.ToInt64(reader["PackageBillID"]), Convert.ToInt64(reader["PackageBillUnitID"]), Convert.ToInt64(reader["ChargeID"]), Convert.ToDouble(Convert.ToDecimal(reader["PackageConsumptionAmount"])), Convert.ToDouble(reader["OPDConsumption"]), Convert.ToDouble(reader["OPDExcludeServices"]), Convert.ToDouble(reader["TotalPackageAdvance"]), Convert.ToDouble(reader["PharmacyConsumeAmount"]), Convert.ToDouble(reader["PackageConsumableLimit"]), Convert.ToDouble(reader["ConsumableServicesBilled"]),0));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return BizActionObj;
        }

        public override IValueObject GetSelectedPackageInfoList(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetPatientPackageInfoListBizActionVO BizActionObj = (clsGetPatientPackageInfoListBizActionVO)valueObject;
            try
            {
                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientPackageDetailsForCS");
                //Adding MasterTableName as Input Parameter to filter record

                dbServer.AddInParameter(command, "PackageBillID", DbType.Int64, BizActionObj.PackageBillID);
                dbServer.AddInParameter(command, "PackageBillUnitID", DbType.Int64, BizActionObj.PackageBillUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.MasterList == null)
                    {
                        BizActionObj.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        //Package New Changes Added on 19062018
                        BizActionObj.MasterList.Add(new MasterListItem(Convert.ToInt64(reader["PackageID"]), reader["PackageName"].ToString(), Convert.ToInt64(reader["PatientInfoID"]), Convert.ToBoolean(reader["ApplicableToAll"]), Convert.ToDouble(reader["ApplicableToAllDiscount"]), Convert.ToDouble(reader["PharmacyFixedRate"]), Convert.ToInt64(reader["PackageBillID"]), Convert.ToInt64(reader["PackageBillUnitID"]), Convert.ToInt64(reader["ChargeID"]), Convert.ToDouble(Convert.ToDecimal(reader["PackageConsumptionAmount"])), Convert.ToDouble(reader["OPDConsumption"]), Convert.ToDouble(reader["OPDExcludeServices"]), Convert.ToDouble(reader["TotalPackageAdvance"]), Convert.ToDouble(reader["PharmacyConsumeAmount"]), Convert.ToDouble(reader["PackageConsumableLimit"]), Convert.ToDouble(reader["ConsumableServicesBilled"]), Convert.ToDouble(reader["PackageClinicalTotal"])));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
            }
            finally
            {

            }
            return BizActionObj;
        }

        // END

        public override IValueObject GetPatientSponsorGroupList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientSponsorGroupListBizActionVO BizActionObj = valueObject as clsGetPatientSponsorGroupListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientSponsorGroupDetails");

                dbServer.AddInParameter(command, "SponsorID", DbType.Int64, BizActionObj.SponsorID);

                DbDataReader reader;

                // clsPatientGeneralVO objPatientVO = BizActionObj.PatientDetails;


                //int intStatus = dbServer.ExecuteNonQuery(command);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DetailsList == null)
                        BizActionObj.DetailsList = new List<clsPatientSponsorGroupDetailsVO>();
                    while (reader.Read())
                    {
                        clsPatientSponsorGroupDetailsVO objectVO = new clsPatientSponsorGroupDetailsVO();

                        objectVO.SponsorID = (long)DALHelper.HandleDBNull(reader["SponsorID"]);
                        objectVO.GroupID = (long)DALHelper.HandleDBNull(reader["GroupID"]);
                        objectVO.GroupName = (string)DALHelper.HandleDBNull(reader["GroupName"]);
                        objectVO.DeductibleAmount = (decimal)DALHelper.HandleDBNull(reader["DeductionAmount"]);
                        objectVO.DeductionPercentage = (double)DALHelper.HandleDBNull(reader["DeductionPercentage"]);
                        BizActionObj.DetailsList.Add(objectVO);

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
            return valueObject;
        }

        public override IValueObject GetPatientSponsorServiceList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientSponsorServiceListBizActionVO BizActionObj = valueObject as clsGetPatientSponsorServiceListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientSponsorServiceDetails");

                dbServer.AddInParameter(command, "SponsorID", DbType.Int64, BizActionObj.SponsorID);

                DbDataReader reader;

                // clsPatientGeneralVO objPatientVO = BizActionObj.PatientDetails;


                //int intStatus = dbServer.ExecuteNonQuery(command);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DetailsList == null)
                        BizActionObj.DetailsList = new List<clsPatientSponsorServiceDetailsVO>();
                    while (reader.Read())
                    {
                        clsPatientSponsorServiceDetailsVO objectVO = new clsPatientSponsorServiceDetailsVO();

                        objectVO.SponsorID = (long)DALHelper.HandleDBNull(reader["SponsorID"]);
                        objectVO.ServiceID = (long)DALHelper.HandleDBNull(reader["ServiceID"]);
                        objectVO.ServiceName = (string)DALHelper.HandleDBNull(reader["ServiceName"]);
                        objectVO.DeductibleAmount = (decimal)DALHelper.HandleDBNull(reader["DeductionAmount"]);
                        objectVO.DeductionPercentage = (double)DALHelper.HandleDBNull(reader["DeductionPercentage"]);
                        BizActionObj.DetailsList.Add(objectVO);

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
            return valueObject;
        }


        public override IValueObject GetPatientSponsorCardList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientSponsorCardListBizActionVO BizActionObj = valueObject as clsGetPatientSponsorCardListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientSponsorCardDetails");

                dbServer.AddInParameter(command, "SponsorID", DbType.Int64, BizActionObj.SponsorID);

                DbDataReader reader;

                // clsPatientGeneralVO objPatientVO = BizActionObj.PatientDetails;


                //int intStatus = dbServer.ExecuteNonQuery(command);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.DetailsList == null)
                        BizActionObj.DetailsList = new List<clsPatientSponsorCardDetailsVO>();
                    while (reader.Read())
                    {
                        clsPatientSponsorCardDetailsVO objectVO = new clsPatientSponsorCardDetailsVO();

                        objectVO.SponsorID = (long)DALHelper.HandleDBNull(reader["SponsorID"]);
                        objectVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objectVO.Title = (string)DALHelper.HandleDBNull(reader["Title"]);
                        objectVO.Image = (Byte[])DALHelper.HandleDBNull(reader["Image"]);

                        BizActionObj.DetailsList.Add(objectVO);

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
            return valueObject;
        }

        public override IValueObject DeletePatientSponsor(IValueObject valueObject, clsUserVO UserVo)
        {


            clsDeletePatientSponsorBizActionVO BizActionObj = valueObject as clsDeletePatientSponsorBizActionVO;
            try
            {
                DbCommand command;

                command = dbServer.GetStoredProcCommand("CIMS_DeletePatientSponsor");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.SponsorID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


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

        public override IValueObject AddPatientSponsorDetailsWithTransaction(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddPatientSponsorBizActionVO BizActionObj = (clsAddPatientSponsorBizActionVO)valueObject;

            try
            {
                if (pConnection == null)
                    pConnection = dbServer.CreateConnection();

                if (pConnection.State != ConnectionState.Open) pConnection.Open();
                if (pTransaction == null)
                    pTransaction = pConnection.BeginTransaction();

                clsPatientSponsorVO objDetailsVO = BizActionObj.PatientSponsorDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientSponsor");
                command.Connection = pConnection;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitId);
                dbServer.AddInParameter(command, "PatientSourceID", DbType.Int64, objDetailsVO.PatientSourceID);
                dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objDetailsVO.PatientCategoryID);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, objDetailsVO.CompanyID);
                dbServer.AddInParameter(command, "AssociatedCompanyID", DbType.Int64, objDetailsVO.AssociatedCompanyID);
                if (objDetailsVO.ReferenceNo != null) objDetailsVO.ReferenceNo = objDetailsVO.ReferenceNo.Trim();
                dbServer.AddInParameter(command, "ReferenceNo", DbType.String, objDetailsVO.ReferenceNo);
                dbServer.AddInParameter(command, "CreditLimit", DbType.Double, objDetailsVO.CreditLimit);
                dbServer.AddInParameter(command, "EffectiveDate", DbType.DateTime, objDetailsVO.EffectiveDate);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, objDetailsVO.ExpiryDate);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, objDetailsVO.TariffID);
                if (objDetailsVO.EmployeeNo != null) objDetailsVO.EmployeeNo = objDetailsVO.EmployeeNo.Trim();
                dbServer.AddInParameter(command, "EmployeeNo", DbType.String, objDetailsVO.EmployeeNo);
                dbServer.AddInParameter(command, "DesignationID", DbType.Int64, objDetailsVO.DesignationID);
                if (objDetailsVO.Remark != null) objDetailsVO.Remark = objDetailsVO.Remark.Trim();
                dbServer.AddInParameter(command, "Remark", DbType.String, objDetailsVO.Remark);

                //dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId); //objDetailsVO.UnitId);
                //dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, objDetailsVO.CreatedUnitId);

                //dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                //dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objDetailsVO.AddedBy);
                //if (objDetailsVO.AddedOn != null) objDetailsVO.AddedOn = objDetailsVO.AddedOn.Trim();
                //dbServer.AddInParameter(command, "AddedOn", DbType.String, objDetailsVO.AddedOn);
                //dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                //if (objDetailsVO.AddedWindowsLoginName != null) objDetailsVO.AddedWindowsLoginName = objDetailsVO.AddedWindowsLoginName.Trim();
                //dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objDetailsVO.AddedWindowsLoginName);
                dbServer.AddInParameter(command, "MemberRelationID", DbType.Int64, objDetailsVO.MemberRelationID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, pTransaction);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.PatientSponsorDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                throw ex;
            }
            finally
            {

            }

            return BizActionObj;
        }

        // Added By CDS 

        public override IValueObject AddFollowUpPatientNew(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;

            clsAddFollowUpStatusByPatientIdBizActionVO BizActionObj = valueObject as clsAddFollowUpStatusByPatientIdBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientFollowUpPackageDetails");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);

                dbServer.AddInParameter(command, "SponsorID", DbType.Int64, BizActionObj.SponsorID);
                dbServer.AddInParameter(command, "SponsorUnitID", DbType.Int64, BizActionObj.SponsorUnitID);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, BizActionObj.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return valueObject;

        }

        public override IValueObject GetFollowUpPatient(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetFollowUpStatusByPatientIdBizActionVO BizActionObj = valueObject as clsGetFollowUpStatusByPatientIdBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPatientFollowUpDetailsByPatientID");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PatientUnitID);
                dbServer.AddInParameter(command, "TariffID", DbType.Int64, BizActionObj.TariffID);
                dbServer.AddInParameter(command, "SponsorID", DbType.Int64, BizActionObj.SponsorID);
                dbServer.AddInParameter(command, "SponsorUnitID", DbType.Int64, BizActionObj.SponsorUnitID);

                dbServer.AddParameter(command, "IsFollowUpAdded", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.IsFollowUpAdded);
                dbServer.AddParameter(command, "IsPackageDetailsAdded", DbType.Boolean, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.IsPackageDetailsAdded);

                int intStatus = dbServer.ExecuteNonQuery(command);

                BizActionObj.IsFollowUpAdded = (bool)dbServer.GetParameterValue(command, "IsFollowUpAdded");
                BizActionObj.IsPackageDetailsAdded = (bool)dbServer.GetParameterValue(command, "IsPackageDetailsAdded");

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {

            }
            return valueObject;

        }

        public override IValueObject AddPatientSponsorForPathology(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientSponsorForPathologyBizActionVO BizActionObj = valueObject as clsAddPatientSponsorForPathologyBizActionVO;

            try
            {
                clsPatientSponsorVO objDetailsVO = BizActionObj.PatientSponsorDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddPatientSponsorForPathology");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientId);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitId);
                dbServer.AddInParameter(command, "PatientSourceID", DbType.Int64, objDetailsVO.PatientSourceID);
                dbServer.AddInParameter(command, "PatientCategoryID", DbType.Int64, objDetailsVO.PatientCategoryID);
                dbServer.AddInParameter(command, "CompanyID", DbType.Int64, objDetailsVO.CompanyID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
               
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.PatientSponsorDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
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






    }
}
