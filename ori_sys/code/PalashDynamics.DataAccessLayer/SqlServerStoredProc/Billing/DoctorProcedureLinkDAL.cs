using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
using PalashDynamics.ValueObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.Billing;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class DoctorProcedureLinkDAL : DoctorProcedureLinkBaseDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private DoctorProcedureLinkDAL()
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
                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override IValueObject AddDoctorProcedureLink(IValueObject valueObject, clsUserVO UserVo)
        {
     clsAddDoctorProcedureLinkBizActionVO BizActionObj = valueObject as clsAddDoctorProcedureLinkBizActionVO;
            try
            {
                clsDoctorProcedureLinkVO objDetails = BizActionObj.LinkDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDoctorProcedureLinkHistory");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetails.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetails.PatientUnitID);
                dbServer.AddInParameter(command, "ProcedureID", DbType.Int64, objDetails.ProcedureID);
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, objDetails.DoctorID);
                dbServer.AddInParameter(command, "NurseID", DbType.Int64, objDetails.NurseID);
                dbServer.AddInParameter(command, "SpecilazationID", DbType.Int64, objDetails.SpecilazationID);
                dbServer.AddInParameter(command, "BillID", DbType.Int64, objDetails.BillID);
                dbServer.AddInParameter(command, "BillUnitID", DbType.Int64, objDetails.BillUnitID);

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetails.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objDetails.Time);
                dbServer.AddInParameter(command, "BillDate", DbType.DateTime, objDetails.BillDate);
                dbServer.AddInParameter(command, "BillNo", DbType.String, objDetails.BillNo);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetails.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                //dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return valueObject;
        }
        public override IValueObject GetDoctorProcedureLink(IValueObject valueObject, clsUserVO UserVo) 
        {
           // throw new NotImplementedException();
     

                 clsGetDoctorProcedureLinkBizActionVO BizActionObj = valueObject as clsGetDoctorProcedureLinkBizActionVO;
            
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDoctorProcedureLinkHistoryList");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.LinkDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.LinkDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.LinkDetailsList == null)
                        BizActionObj.LinkDetailsList = new List<clsDoctorProcedureLinkVO>();
                    while (reader.Read())
                    {
                        clsDoctorProcedureLinkVO Details = new clsDoctorProcedureLinkVO();
                        Details.ID = (long)reader["ID"];
                        Details.UnitID = (long)reader["UnitID"];
                        Details.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        Details.Time = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                        Details.BillDate = Convert.ToDateTime(DALHelper.HandleDate(reader["BillDate"]));
                        Details.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));
                        Details.ProcedureID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ProcedureID"]));
                        Details.Procedure = Convert.ToString(DALHelper.HandleDBNull(reader["Proceduredone"]));
                        Details.DoctorID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["DoctorID"]));
                        Details.Doctor = Convert.ToString(DALHelper.HandleDBNull(reader["Doctor"]));
                        Details.NurseID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["NurseID"]));
                        Details.Nurse = Convert.ToString(DALHelper.HandleDBNull(reader["Nurse"]));
                        Details.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        Details.SpecilazationID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["SpecilazationID"]));
                        

                        BizActionObj.LinkDetailsList.Add(Details);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return BizActionObj;
        }
        public override IValueObject DeleteDoctorProcedureLink(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteDoctorProcedureLinkBizActionVO BizActionObj = valueObject as clsDeleteDoctorProcedureLinkBizActionVO;
            try
            {
                clsDoctorProcedureLinkVO objDetails = BizActionObj.LinkDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteDoctorProcedureLinkFile");
                dbServer.AddInParameter(command, "ID", DbType.Int64,  objDetails.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetails.UnitID);
                int intStatus = dbServer.ExecuteNonQuery(command);
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            return valueObject;
        } 
    }
    
}
