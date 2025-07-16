using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using System.Data.Common;
using System.Data;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Patient;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsTESE_DAL : clsBaseTESEDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsTESE_DAL()
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
                throw ex;
            }
        }

        public override IValueObject AddUpdateTESE(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateTESEBizActionVO BizActionObj = valueObject as clsAddUpdateTESEBizActionVO;
            List<clsTESEDetailsVO> TestList = new List<clsTESEDetailsVO>();
            TestList = BizActionObj.TESEDetailsList;
            clsCoupleVO coupleDetails = new clsCoupleVO();
            coupleDetails = BizActionObj.TESE.CoupleDetail;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_AddUpdateTESE");
                //if (BizActionObj.TESEDetailsList[0].TESE_ID > 0)
                //{
                //    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.TESEDetailsList[0].TESE_ID);
                //}
                if (BizActionObj.TESE.ID > 0) 
                {
                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.TESE.ID);
                }
                else
                {
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                }

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, coupleDetails.CoupleId);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, coupleDetails.CoupleUnitId);
                dbServer.AddInParameter(command, "PatientTypeID", DbType.Int64, coupleDetails.MalePatient.PatientTypeID);
                dbServer.AddInParameter(command, "RegistrationDate", DbType.DateTime, coupleDetails.CoupleRegDate);
                dbServer.AddInParameter(command, "CryoDate", DbType.Date, BizActionObj.TESE.CryoDate);
                dbServer.AddInParameter(command, "CryoTime", DbType.Time, BizActionObj.TESE.CryoTime);
                dbServer.AddInParameter(command, "EmbroLogistID", DbType.Int64, BizActionObj.TESE.EmbroLogistID);
                dbServer.AddInParameter(command, "LabInchargeID", DbType.Int64, BizActionObj.TESE.LabInchargeID);
                dbServer.AddInParameter(command, "Tissue", DbType.String, BizActionObj.TESE.Tissue);
                dbServer.AddInParameter(command, "TissueSideID", DbType.Int64, BizActionObj.TESE.TissueSideID);
                //     dbServer.AddInParameter(command, "Status", DbType.Boolean, true);                
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, BizActionObj.TESE.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //   dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Synchronized", DbType.Boolean, false);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = 0;
                intStatus = dbServer.ExecuteNonQuery(command);
                //payDetails.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.TESE.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.TESE.UnitID = (long)dbServer.GetParameterValue(command, "UnitID");

                foreach (var item in BizActionObj.TESEDetailsList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_IVF_AddUpdateTESEDetails");

                    if (item.ID > 0)
                    {
                        dbServer.AddInParameter(command1, "ID", DbType.Int64, item.ID);
                    }
                    else
                    {
                        dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                    }
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "TESE_ID", DbType.Int64, BizActionObj.TESE.ID);
                    dbServer.AddInParameter(command1, "TESE_UnitID", DbType.Int64, BizActionObj.TESE.UnitID);
                    dbServer.AddInParameter(command1, "Tissue", DbType.String, item.Tissue);
                    dbServer.AddInParameter(command1, "NoofVailsFrozen", DbType.Int64, item.NoofVailsFrozen);
                    dbServer.AddInParameter(command1, "ContainerNumber", DbType.Int64, item.ContainerNumber);
                    dbServer.AddInParameter(command1, "HolderNumber", DbType.Int64, item.HolderNumber);
                    dbServer.AddInParameter(command1, "status", DbType.Boolean, true);
                    dbServer.AddInParameter(command1, "No_of_VailsUsed", DbType.Int64, item.No_of_VailsUsed);
                    dbServer.AddInParameter(command1, "No_of_VailsRemain", DbType.Int64, item.No_of_VailsRemain);
                    dbServer.AddInParameter(command1, "RenewalDate", DbType.DateTime, item.RenewalDate);

                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, BizActionObj.TESE.AddedDateTime);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                    dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command1, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = 0;
                    intStatus1 = dbServer.ExecuteNonQuery(command1);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                }


               // BizActionObj.SuccessStatus = 0;
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

        public override IValueObject GetTESEDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetTESEBizActionVO BizActionObj = new clsGetTESEBizActionVO();
            BizActionObj = valueObject as clsGetTESEBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetTESEDetails");
                DbDataReader reader;
                BizActionObj.TESEDeatailsList = new List<clsTESEDetailsVO>();
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.TESE.CoupleUnitID);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.TESE.CoupleID);

                //dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                //dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SortExpression);

                //dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsTESEDetailsVO BizObjGeneral = new clsTESEDetailsVO();
                        BizObjGeneral.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));
                   
                        BizObjGeneral.Tissue = Convert.ToString(DALHelper.HandleDBNull(reader["Tissue"]));
                        //BizObj.CryoDate = Convert.ToDateTime (DALHelper.HandleDate (reader[""]));
                        //BizObj.CryoTime = Convert.ToDateTime (DALHelper.HandleDBNull (reader[""]));
                        BizObjGeneral.EmbroLogistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbroLogistID"]));
                        BizObjGeneral.LabInchargeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabInchargeID"]));

                        BizObjGeneral.EmbroLogist = Convert.ToString(DALHelper.HandleDBNull(reader["Embrologist"]));
                        BizObjGeneral.LabIncharge = Convert.ToString(DALHelper.HandleDBNull(reader["labincharge"]));

                        BizObjGeneral.No_of_VailsRemain = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["No_of_VailsRemain"]));
                        BizObjGeneral.No_of_VailsUsed = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["No_of_VailsUsed"]));
                        BizObjGeneral.NoofVailsFrozen = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["NoofVailsFrozen"]));
                        BizObjGeneral.TESE_ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TESE_ID"]));
                        BizObjGeneral.TESE_UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TESE_UnitID"]));
                        BizObjGeneral.CryoDate = Convert.ToDateTime(DALHelper.HandleDate(reader["CryoDate"]));
                        BizObjGeneral.CryoTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["CryoTime"]));
                        BizObjGeneral.ContainerNumber = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContainerNumber"]));
                        BizObjGeneral.HolderNumber = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["HolderNumber"]));
                        BizObjGeneral.RenewalDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["RenewalDate"]));
                        BizObjGeneral.TissueSideID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TissueSideID"]));

                        BizActionObj.TESEDeatailsList.Add(BizObjGeneral);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valueObject;
        }

    }
}
