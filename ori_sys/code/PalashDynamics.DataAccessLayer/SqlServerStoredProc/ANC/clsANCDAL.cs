using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.ANC;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.ANC;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.ANC
{
    class clsANCDAL : clsBaseANCDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsANCDAL()
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

        #region General Details
        public override IValueObject AddUpdateANCGeneralDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddANCBizActionVO BizActionObj = valueObject as clsAddANCBizActionVO;
            try
            {
                clsANCVO objANCGeneralDetails = BizActionObj.ANCGeneralDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateANCExaminationGeneralDetails");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, objANCGeneralDetails.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, objANCGeneralDetails.CoupleUnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objANCGeneralDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objANCGeneralDetails.PatientUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objANCGeneralDetails.Date);
                dbServer.AddInParameter(command, "M", DbType.String, objANCGeneralDetails.M);
                dbServer.AddInParameter(command, "G", DbType.String, objANCGeneralDetails.G);
                dbServer.AddInParameter(command, "A", DbType.String, objANCGeneralDetails.A);
                dbServer.AddInParameter(command, "P", DbType.String, objANCGeneralDetails.P);
                dbServer.AddInParameter(command, "L", DbType.String, objANCGeneralDetails.L);
                dbServer.AddInParameter(command, "LMPDate", DbType.DateTime, objANCGeneralDetails.LMPDate);
                dbServer.AddInParameter(command, "EDDDate", DbType.DateTime, objANCGeneralDetails.EDDDate);
                //dbServer.AddInParameter(command, "Gender", DbType.Int64, objANCGeneralDetails.Gender);
                dbServer.AddInParameter(command, "SpecialRemark", DbType.String, objANCGeneralDetails.SpecialRemark);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objANCGeneralDetails.Isfreezed);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objANCGeneralDetails.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                dbServer.AddInParameter(command, "DateofMarriage", DbType.DateTime, objANCGeneralDetails.DateofMarriage);
                dbServer.AddInParameter(command, "AgeOfMenarche", DbType.String, objANCGeneralDetails.AgeOfMenarche);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objANCGeneralDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                //BizActionObj.ANCGeneralDetails.ID = (long)dbServer.GetParameterValue(command, "ID");


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
        public override IValueObject GetANCGeneralDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetANCGeneralDetailsBizActionVO BizActionObj = valueObject as clsGetANCGeneralDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetANCGeneralDetails");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.ANCGeneralDetails.CoupleID);
                //dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.ANCGeneralDetails.CoupleUnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.ANCGeneralDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.ANCGeneralDetails.PatientUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ANCGeneralDetails == null)
                        BizActionObj.ANCGeneralDetails = new clsANCVO();
                    while (reader.Read())
                    {
                        BizActionObj.ANCGeneralDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.ANCGeneralDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizActionObj.ANCGeneralDetails.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        BizActionObj.ANCGeneralDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BizActionObj.ANCGeneralDetails.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        BizActionObj.ANCGeneralDetails.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));
                        BizActionObj.ANCGeneralDetails.ANC_Code = Convert.ToString(DALHelper.HandleDBNull(reader["ANC_Code"]));
                        BizActionObj.ANCGeneralDetails.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ANCDate"]));
                        BizActionObj.ANCGeneralDetails.LMPDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["LMPDate"]));
                        BizActionObj.ANCGeneralDetails.EDDDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["EDDDate"]));
                        BizActionObj.ANCGeneralDetails.M = Convert.ToString(DALHelper.HandleDBNull(reader["M"]));
                        BizActionObj.ANCGeneralDetails.G = Convert.ToString(DALHelper.HandleDBNull(reader["G"]));
                        BizActionObj.ANCGeneralDetails.L = Convert.ToString(DALHelper.HandleDBNull(reader["L"]));
                        BizActionObj.ANCGeneralDetails.P = Convert.ToString(DALHelper.HandleDBNull(reader["P"]));
                        BizActionObj.ANCGeneralDetails.A = Convert.ToString(DALHelper.HandleDBNull(reader["A"]));
                        BizActionObj.ANCGeneralDetails.SpecialRemark = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialRemark"]));
                        //BizActionObj.ANCGeneralDetails.Gender = (long)DALHelper.HandleDBNull(reader["Gender"]);
                        BizActionObj.ANCGeneralDetails.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        BizActionObj.ANCGeneralDetails.DateofMarriage = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateofMarriage"]));
                        BizActionObj.ANCGeneralDetails.AgeOfMenarche = Convert.ToString(DALHelper.HandleDBNull(reader["AgeOfMenarche"]));
                    }
                }
                reader.NextResult();
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
        public override IValueObject GetANCGeneralDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsANCGetGeneralDetailsListBizActionVO BizActionObj = valueObject as clsANCGetGeneralDetailsListBizActionVO;
            BizActionObj.ANCGeneralDetailsList = new List<clsANCVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetANCGeneralDetailsList_New");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.ANCGeneralDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.ANCGeneralDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        clsANCVO Details = new clsANCVO();
                        Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        Details.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        Details.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));
                        Details.ANC_Code = Convert.ToString(DALHelper.HandleDBNull(reader["ANC_Code"]));
                        Details.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        Details.LMPDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["LMPDate"]));
                        Details.EDDDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["EDDDate"]));
                        Details.M = Convert.ToString(DALHelper.HandleDBNull(reader["M"]));
                        Details.G = Convert.ToString(DALHelper.HandleDBNull(reader["G"]));
                        Details.L = Convert.ToString(DALHelper.HandleDBNull(reader["L"]));
                        Details.P = Convert.ToString(DALHelper.HandleDBNull(reader["P"]));
                        Details.A = Convert.ToString(DALHelper.HandleDBNull(reader["A"]));
                        Details.SpecialRemark = Convert.ToString(DALHelper.HandleDBNull(reader["SpecialRemark"]));
                        //Details.Gender = (long)DALHelper.HandleDBNull(reader["Gender"]);
                        Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        Details.IsClosed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClosed"]));
                        Details.DateofMarriage = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateofMarriage"]));
                        Details.AgeOfMenarche = Convert.ToString(DALHelper.HandleDBNull(reader["AgeOfMenarche"]));

                        BizActionObj.ANCGeneralDetailsList.Add(Details);
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
        #endregion

        #region History
        public override IValueObject AddUpdateANCHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateANCHistoryBizActionVO BizActionObj = valueObject as clsAddUpdateANCHistoryBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;

            try
            {


                clsANCHistoryVO objANCHistoryDetails = BizActionObj.ANCHistory;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Delete from T_ANC_ObstetricHistory where ANCID = " + objANCHistoryDetails.ANCID + " And PatientID=" + objANCHistoryDetails.PatientID + " And PatientUnitID=" + objANCHistoryDetails.PatientUnitID);
                int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateANCExaminationHistory");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objANCHistoryDetails.UnitID);
                dbServer.AddInParameter(command, "ANCID", DbType.Int64, objANCHistoryDetails.ANCID);
                //By Anjali.....
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objANCHistoryDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objANCHistoryDetails.PatientUnitID);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, objANCHistoryDetails.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, objANCHistoryDetails.CoupleUnitID);
                dbServer.AddInParameter(command, "Hypertension", DbType.Boolean, objANCHistoryDetails.Hypertension);
                dbServer.AddInParameter(command, "TB", DbType.Boolean, objANCHistoryDetails.TB);
                dbServer.AddInParameter(command, "Diabeties", DbType.Boolean, objANCHistoryDetails.Diabeties);
                dbServer.AddInParameter(command, "Twins", DbType.Boolean, objANCHistoryDetails.Twins);
                dbServer.AddInParameter(command, "PersonalHistory", DbType.Boolean, objANCHistoryDetails.PersonalHistory);
                dbServer.AddInParameter(command, "LLMPDate", DbType.DateTime, objANCHistoryDetails.LLMPDate);
                //dbServer.AddInParameter(command, "EDDDate", DbType.DateTime, objANCHistoryDetails.EDDDate);
                dbServer.AddInParameter(command, "Menstrualcycle", DbType.String, objANCHistoryDetails.Menstrualcycle);
                dbServer.AddInParameter(command, "Duration", DbType.String, objANCHistoryDetails.Duration);
                dbServer.AddInParameter(command, "Disorder", DbType.String, objANCHistoryDetails.Disorder);
                dbServer.AddInParameter(command, "txtDiabeties", DbType.String, objANCHistoryDetails.txtDiabeties);
                dbServer.AddInParameter(command, "txtHypertension", DbType.String, objANCHistoryDetails.txtHypertension);
                dbServer.AddInParameter(command, "txtPersonalHistory", DbType.String, objANCHistoryDetails.txtPersonalHistory);
                dbServer.AddInParameter(command, "txtTB", DbType.String, objANCHistoryDetails.txtTB);
                dbServer.AddInParameter(command, "txtTwins", DbType.String, objANCHistoryDetails.txtTwins);
                dbServer.AddInParameter(command, "Weight", DbType.Single, objANCHistoryDetails.Weight);
                dbServer.AddInParameter(command, "Surgery", DbType.String, objANCHistoryDetails.Surgery);
                dbServer.AddInParameter(command, "RS", DbType.String, objANCHistoryDetails.RS);
                dbServer.AddInParameter(command, "Pulse", DbType.Single, objANCHistoryDetails.Pulse);
                dbServer.AddInParameter(command, "Otherimportantreleventfactor", DbType.String, objANCHistoryDetails.Otherimportantreleventfactor);
                dbServer.AddInParameter(command, "Lymphadenopathy", DbType.String, objANCHistoryDetails.Lymphadenopathy);
                dbServer.AddInParameter(command, "Hirsutism", DbType.Boolean, objANCHistoryDetails.Hirsutism);
                dbServer.AddInParameter(command, "HirsutismID", DbType.Int64, objANCHistoryDetails.HirsutismID);
                dbServer.AddInParameter(command, "Height", DbType.Single, objANCHistoryDetails.Height);
                dbServer.AddInParameter(command, "Goitre", DbType.String, objANCHistoryDetails.Goitre);
                dbServer.AddInParameter(command, "frequencyandtimingofintercourse", DbType.String, objANCHistoryDetails.frequencyandtimingofintercourse);
                dbServer.AddInParameter(command, "Flurseminis", DbType.String, objANCHistoryDetails.Flurseminis);
                dbServer.AddInParameter(command, "Edema", DbType.String, objANCHistoryDetails.Edema);
                dbServer.AddInParameter(command, "DrugsPresent", DbType.String, objANCHistoryDetails.DrugsPresent);
                dbServer.AddInParameter(command, "DrugsPast", DbType.String, objANCHistoryDetails.DrugsPast);
                dbServer.AddInParameter(command, "Drugs", DbType.String, objANCHistoryDetails.Drugs);
                dbServer.AddInParameter(command, "CVS", DbType.String, objANCHistoryDetails.CVS);
                dbServer.AddInParameter(command, "CNS", DbType.String, objANCHistoryDetails.CNS);
                dbServer.AddInParameter(command, "Breasts", DbType.Single, objANCHistoryDetails.Breasts);
                dbServer.AddInParameter(command, "BpInMm", DbType.Single, objANCHistoryDetails.BpInMm);
                dbServer.AddInParameter(command, "BpInHg", DbType.Single, objANCHistoryDetails.BpInHg);
                dbServer.AddInParameter(command, "AnyOtherfactor", DbType.String, objANCHistoryDetails.AnyOtherfactor);
                dbServer.AddInParameter(command, "Anaemia", DbType.String, objANCHistoryDetails.Anaemia);
                dbServer.AddInParameter(command, "TTIstDose", DbType.Boolean, objANCHistoryDetails.TTIstDose);
                dbServer.AddInParameter(command, "DateIstDose", DbType.DateTime, objANCHistoryDetails.DateIstDose);
                dbServer.AddInParameter(command, "TTIIndDose", DbType.Boolean, objANCHistoryDetails.TTIIstDose);
                dbServer.AddInParameter(command, "DateIIndDose", DbType.DateTime, objANCHistoryDetails.DateIIstDose);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objANCHistoryDetails.Isfreezed);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objANCHistoryDetails.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objANCHistoryDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ANCHistory.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.ANCObsetricHistoryList != null)
                {
                    if (BizActionObj.ANCObsetricHistoryList.Count > 0)
                    {
                        foreach (var item in BizActionObj.ANCObsetricHistoryList)
                        {

                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateANCObstetricHistory");
                            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objANCHistoryDetails.UnitID);
                            dbServer.AddInParameter(command1, "PatientID", DbType.Int64, objANCHistoryDetails.PatientID);
                            dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, objANCHistoryDetails.PatientUnitID);
                            dbServer.AddInParameter(command1, "Baby", DbType.Int64, item.Baby);
                            dbServer.AddInParameter(command1, "DateYear", DbType.DateTime, item.DateYear);
                            dbServer.AddInParameter(command1, "IsFreezed", DbType.Boolean, objANCHistoryDetails.Isfreezed);
                            dbServer.AddInParameter(command1, "Status", DbType.Boolean, item.Status);
                            dbServer.AddInParameter(command1, "Gestation", DbType.String, item.Gestation);
                            dbServer.AddInParameter(command1, "TypeofDelivery", DbType.String, item.TypeofDelivery);
                            dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                            dbServer.AddInParameter(command1, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                            dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                            dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command1, "UpdatedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command1, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command1, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command1, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command1, "ANCID", DbType.Int64, objANCHistoryDetails.ANCID);
                            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                            dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        }
                    }
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                // trans.Commit();
                con.Close();
                trans = null;
                con = null;
            }
            return valueObject;
        }
        public override IValueObject GetANCHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetANCHistoryBizActionVO BizActionObj = valueObject as clsGetANCHistoryBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetANCHistory");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ANCID", DbType.Int64, BizActionObj.ANCHistory.ANCID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ANCHistory == null)
                        BizActionObj.ANCHistory = new clsANCHistoryVO();
                    while (reader.Read())
                    {


                        BizActionObj.ANCHistory.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["HisID"]));
                        BizActionObj.ANCHistory.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["HisUnitID"]));
                        BizActionObj.ANCHistory.ANCID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ANCID"]));
                        BizActionObj.ANCHistory.Hypertension = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Hypertension"]));
                        BizActionObj.ANCHistory.TB = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TB"]));
                        BizActionObj.ANCHistory.Diabeties = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Diabeties"]));
                        BizActionObj.ANCHistory.Twins = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Twins"]));
                        BizActionObj.ANCHistory.PersonalHistory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PersonalHistory"]));
                        BizActionObj.ANCHistory.Hirsutism = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Hirsutism"]));
                        BizActionObj.ANCHistory.HirsutismID = Convert.ToInt64(DALHelper.HandleDBNull(reader["HirsutismID"]));
                        BizActionObj.ANCHistory.LLMPDate = Convert.ToDateTime(DALHelper.HandleDate(reader["LLMPDate"]));
                        BizActionObj.ANCHistory.Menstrualcycle = Convert.ToString(DALHelper.HandleDBNull(reader["Menstrualcycle"]));
                        BizActionObj.ANCHistory.Duration = Convert.ToString(DALHelper.HandleDBNull(reader["Duration"]));
                        BizActionObj.ANCHistory.Disorder = Convert.ToString(DALHelper.HandleDBNull(reader["Disorder"]));
                        BizActionObj.ANCHistory.txtDiabeties = Convert.ToString(DALHelper.HandleDBNull(reader["txtDiabeties"]));
                        BizActionObj.ANCHistory.txtTwins = Convert.ToString(DALHelper.HandleDBNull(reader["txtTwins"]));
                        BizActionObj.ANCHistory.txtHypertension = Convert.ToString(DALHelper.HandleDBNull(reader["txtHypertension"]));
                        BizActionObj.ANCHistory.txtTB = Convert.ToString(DALHelper.HandleDBNull(reader["txtTB"]));
                        BizActionObj.ANCHistory.txtPersonalHistory = Convert.ToString(DALHelper.HandleDBNull(reader["txtPersonalHistory"]));
                        BizActionObj.ANCHistory.DrugsPresent = Convert.ToString(DALHelper.HandleDBNull(reader["DrugsPresent"]));
                        BizActionObj.ANCHistory.DrugsPast = Convert.ToString(DALHelper.HandleDBNull(reader["DrugsPast"]));
                        BizActionObj.ANCHistory.Surgery = Convert.ToString(DALHelper.HandleDBNull(reader["Surgery"]));
                        BizActionObj.ANCHistory.Drugs = Convert.ToString(DALHelper.HandleDBNull(reader["Drugs"]));
                        BizActionObj.ANCHistory.Anaemia = Convert.ToString(DALHelper.HandleDBNull(reader["Anaemia"]));
                        BizActionObj.ANCHistory.Lymphadenopathy = Convert.ToString(DALHelper.HandleDBNull(reader["Lymphadenopathy"]));
                        BizActionObj.ANCHistory.Edema = Convert.ToString(DALHelper.HandleDBNull(reader["Edema"]));
                        BizActionObj.ANCHistory.Goitre = Convert.ToString(DALHelper.HandleDBNull(reader["Goitre"]));
                        BizActionObj.ANCHistory.CVS = Convert.ToString(DALHelper.HandleDBNull(reader["CVS"]));
                        BizActionObj.ANCHistory.CNS = Convert.ToString(DALHelper.HandleDBNull(reader["CNS"]));
                        BizActionObj.ANCHistory.RS = Convert.ToString(DALHelper.HandleDBNull(reader["RS"]));
                        BizActionObj.ANCHistory.Flurseminis = Convert.ToString(DALHelper.HandleDBNull(reader["Flurseminis"]));
                        BizActionObj.ANCHistory.AnyOtherfactor = Convert.ToString(DALHelper.HandleDBNull(reader["AnyOtherfactor"]));
                        BizActionObj.ANCHistory.Otherimportantreleventfactor = Convert.ToString(DALHelper.HandleDBNull(reader["Otherimportantreleventfactor"]));
                        BizActionObj.ANCHistory.frequencyandtimingofintercourse = Convert.ToString(DALHelper.HandleDBNull(reader["frequencyandtimingofintercourse"]));
                        BizActionObj.ANCHistory.TTIstDose = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TTIstDose"]));
                        BizActionObj.ANCHistory.DateIstDose = Convert.ToDateTime(DALHelper.HandleDate(reader["DateIstDose"]));
                        BizActionObj.ANCHistory.TTIIstDose = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TTIIndDose"]));
                        BizActionObj.ANCHistory.DateIIstDose = Convert.ToDateTime(DALHelper.HandleDate(reader["DateIIndDose"]));
                        BizActionObj.ANCHistory.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        BizActionObj.ANCHistory.Pulse = Convert.ToSingle(DALHelper.HandleDBNull(reader["Pulse"]));
                        BizActionObj.ANCHistory.BpInMm = Convert.ToSingle(DALHelper.HandleDBNull(reader["BpInMm"]));
                        BizActionObj.ANCHistory.BpInHg = Convert.ToSingle(DALHelper.HandleDBNull(reader["BpInHg"]));
                        BizActionObj.ANCHistory.Weight = Convert.ToSingle(DALHelper.HandleDBNull(reader["Weight"]));
                        BizActionObj.ANCHistory.Height = Convert.ToSingle(DALHelper.HandleDBNull(reader["Height"]));
                        BizActionObj.ANCHistory.Breasts = Convert.ToString(DALHelper.HandleDBNull(reader["Breasts"]));
                        BizActionObj.ANCHistory.Edema = Convert.ToString(DALHelper.HandleDBNull(reader["Edema"]));

                        clsANCObstetricHistoryVO Details = new clsANCObstetricHistoryVO();
                        Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        Details.TypeofDelivery = Convert.ToString(DALHelper.HandleDBNull(reader["TypeofDelivery"]));
                        Details.DateYear = Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateYear"]));
                        Details.Baby = Convert.ToInt64(DALHelper.HandleDBNull(reader["Baby"]));
                        Details.Gestation = Convert.ToString(DALHelper.HandleDBNull(reader["Gestation"]));
                        Details.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));


                        BizActionObj.ANCObsetricHistoryList.Add(Details);
                    }
                }
                reader.NextResult();
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
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
        public override IValueObject AddUpdateObestricHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateObestricHistoryBizActionVO BizActionObj = valueObject as clsAddUpdateObestricHistoryBizActionVO;
            try
            {
                clsANCObstetricHistoryVO objANCObstetricHistoryDetails = BizActionObj.ANCObstetricHistory;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateANCObstetricHistory");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objANCObstetricHistoryDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objANCObstetricHistoryDetails.PatientUnitID);
                dbServer.AddInParameter(command, "MaturityComplication", DbType.String, objANCObstetricHistoryDetails.MaturityComplication);
                dbServer.AddInParameter(command, "ObstetricRemark", DbType.String, objANCObstetricHistoryDetails.ObstetricRemark);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objANCObstetricHistoryDetails.Isfreezed);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objANCObstetricHistoryDetails.Status);
                dbServer.AddInParameter(command, "DeliveryMode", DbType.String, objANCObstetricHistoryDetails.ModeOfDelivary);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "HistoryID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objANCObstetricHistoryDetails.HistoryID);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objANCObstetricHistoryDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ANCObstetricHistory.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.ANCObstetricHistory.HistoryID = (long)dbServer.GetParameterValue(command, "HistoryID");

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
        public override IValueObject GetObestricHistoryList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetObstricHistoryListBizActionVO BizActionObj = valueObject as clsGetObstricHistoryListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetANCObestrecHistoryList");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "HistoryID", DbType.Int64, BizActionObj.ANCObstetricHistory.HistoryID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ANCObsetricHistoryList == null)
                        BizActionObj.ANCObsetricHistoryList = new List<clsANCObstetricHistoryVO>();
                    while (reader.Read())
                    {
                        clsANCObstetricHistoryVO Details = new clsANCObstetricHistoryVO();
                        Details.ID = (long)reader["ID"];
                        Details.UnitID = (long)reader["UnitID"];
                        Details.MaturityComplication = (string)DALHelper.HandleDBNull(reader["MaturityComplication"]);
                        Details.ObstetricRemark = (string)DALHelper.HandleDBNull(reader["ObstetricRemark"]);
                        Details.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        Details.Isfreezed = (Boolean)DALHelper.HandleDBNull(reader["IsFreezed"]);
                        Details.ModeOfDelivary = (string)DALHelper.HandleDBNull(reader["DeliveryMode"]);

                        BizActionObj.ANCObsetricHistoryList.Add(Details);
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
        public override IValueObject DeleteObestericHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteObstericHistoryBizActionVO BizActionObj = valueObject as clsDeleteObstericHistoryBizActionVO;
            try
            {
                clsANCObstetricHistoryVO objInvestigationDetails = BizActionObj.ANCObstetricHistory;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteANCObstericHistory");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objInvestigationDetails.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objInvestigationDetails.UnitID);
                dbServer.AddInParameter(command, "ANCID", DbType.Int64, objInvestigationDetails.HistoryID);

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

        #endregion

        #region Investigation Details
        public override IValueObject AddUpdateInvestigationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateANCInvestigationDetailsBizActionVO BizActionObj = valueObject as clsAddUpdateANCInvestigationDetailsBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                clsANCInvestigationDetailsVO objInvestigationDetails = BizActionObj.ANCInvestigationDetails;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Delete from T_ANC_InvestigationDetails where ANCID = " + BizActionObj.ANCInvestigationDetails.ANCID + " And PatientID=" + BizActionObj.ANCInvestigationDetails.PatientID + " And PatientUnitID=" + BizActionObj.ANCInvestigationDetails.PatientUnitID);
                int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);


                foreach (var item in BizActionObj.ANCInvestigationDetailsList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateANCInvestigationDetails");

                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, objInvestigationDetails.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objInvestigationDetails.PatientUnitID);
                    dbServer.AddInParameter(command, "CoupleID", DbType.Int64, objInvestigationDetails.CoupleID);
                    dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, objInvestigationDetails.CoupleUnitID);
                    dbServer.AddInParameter(command, "ANCID", DbType.Int64, objInvestigationDetails.ANCID);
                    dbServer.AddInParameter(command, "InvestigationID", DbType.Int64, item.InvestigationID);
                    dbServer.AddInParameter(command, "InvDate", DbType.DateTime, item.InvDate);
                    dbServer.AddInParameter(command, "Report", DbType.String, item.Report);
                    dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objInvestigationDetails.Isfreezed);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, item.Status);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {

                con.Close();
                trans = null;
                con = null;
            }
            return valueObject;
        }
        public override IValueObject GetANCInvestigationList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetANCInvestigationListBizActionVO BizActionObj = valueObject as clsGetANCInvestigationListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetANCInvestigationList");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ANCInvestigationDetails.UnitID);
                dbServer.AddInParameter(command, "ANCID", DbType.Int64, BizActionObj.ANCInvestigationDetails.ANCID);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ANCInvestigationDetailsList == null)
                        BizActionObj.ANCInvestigationDetailsList = new List<clsANCInvestigationDetailsVO>();
                    while (reader.Read())
                    {
                        clsANCInvestigationDetailsVO Details = new clsANCInvestigationDetailsVO();
                        Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        Details.InvDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["InvDate"]));
                        Details.InvestigationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InvestigationID"]));
                        Details.Investigation = Convert.ToString(DALHelper.HandleDBNull(reader["Investigation"]));
                        Details.Report = Convert.ToString(DALHelper.HandleDBNull(reader["Report"]));
                        Details.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        BizActionObj.ANCInvestigationDetailsList.Add(Details);
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
        public override IValueObject AddUpdateUSGDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateUSGdetailsBizActionVO BizActionObj = valueObject as clsAddUpdateUSGdetailsBizActionVO;
            try
            {
                clsANCUSGDetailsVO objUSGDetails = BizActionObj.ANCUSGDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateANCUSGDetails");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objUSGDetails.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objUSGDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objUSGDetails.PatientUnitID);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, objUSGDetails.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, objUSGDetails.CoupleUnitID);
                dbServer.AddInParameter(command, "ANCID", DbType.Int64, objUSGDetails.ANCID);


                dbServer.AddInParameter(command, "SIFT", DbType.DateTime, objUSGDetails.SIFT);
                dbServer.AddInParameter(command, "txtSIFT", DbType.String, objUSGDetails.txtSIFT);
                dbServer.AddInParameter(command, "GIFT", DbType.DateTime, objUSGDetails.GIFT);
                dbServer.AddInParameter(command, "txtGIFT", DbType.String, objUSGDetails.txtGIFT);
                dbServer.AddInParameter(command, "IVF", DbType.DateTime, objUSGDetails.IVF);
                dbServer.AddInParameter(command, "txtIVF", DbType.String, objUSGDetails.txtIVF);
                dbServer.AddInParameter(command, "Sonography", DbType.String, objUSGDetails.Sonography);
                dbServer.AddInParameter(command, "OvulationMonitors", DbType.String, objUSGDetails.OvulationMonitors);
                dbServer.AddInParameter(command, "Mysteroscopy", DbType.String, objUSGDetails.Mysteroscopy);
                dbServer.AddInParameter(command, "Laparoscopy", DbType.String, objUSGDetails.Laparoscopy);
                dbServer.AddInParameter(command, "INVTreatment", DbType.String, objUSGDetails.INVTreatment);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objUSGDetails.Isfreezed);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objUSGDetails.ID);
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
        public override IValueObject GetANCUSGList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetANCUSGListBizActionVO BizActionObj = valueObject as clsGetANCUSGListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetANCUSGList");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ANCUSGDetails.UnitID);
                dbServer.AddInParameter(command, "ANCID", DbType.Int64, BizActionObj.ANCUSGDetails.ANCID);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ANCUSGDetails == null)
                        BizActionObj.ANCUSGDetails = new clsANCUSGDetailsVO();
                    while (reader.Read())
                    {

                        BizActionObj.ANCUSGDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.ANCUSGDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizActionObj.ANCUSGDetails.SIFT = Convert.ToDateTime(DALHelper.HandleDate(reader["SIFT"]));
                        BizActionObj.ANCUSGDetails.txtSIFT = Convert.ToString(DALHelper.HandleDBNull(reader["txtSIFT"]));
                        BizActionObj.ANCUSGDetails.GIFT = Convert.ToDateTime(DALHelper.HandleDate(reader["GIFT"]));
                        BizActionObj.ANCUSGDetails.txtGIFT = Convert.ToString(DALHelper.HandleDBNull(reader["txtGIFT"]));
                        BizActionObj.ANCUSGDetails.IVF = Convert.ToDateTime(DALHelper.HandleDate(reader["IVF"]));
                        BizActionObj.ANCUSGDetails.txtIVF = Convert.ToString(DALHelper.HandleDBNull(reader["txtIVF"]));
                        BizActionObj.ANCUSGDetails.Sonography = Convert.ToString(DALHelper.HandleDBNull(reader["Sonography"]));
                        BizActionObj.ANCUSGDetails.OvulationMonitors = Convert.ToString(DALHelper.HandleDBNull(reader["OvulationMonitors"]));
                        BizActionObj.ANCUSGDetails.Laparoscopy = Convert.ToString(DALHelper.HandleDBNull(reader["Laparoscopy"]));
                        BizActionObj.ANCUSGDetails.INVTreatment = Convert.ToString(DALHelper.HandleDBNull(reader["INVTreatment"]));
                        BizActionObj.ANCUSGDetails.Mysteroscopy = Convert.ToString(DALHelper.HandleDBNull(reader["Mysteroscopy"]));
                        BizActionObj.ANCUSGDetails.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.ANCUSGDetails.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));

                    }
                }
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
        public override IValueObject DeleteUSG(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteANCUSGBizActionVO BizActionObj = valueObject as clsDeleteANCUSGBizActionVO;
            try
            {
                clsANCUSGDetailsVO objInvestigationDetails = BizActionObj.ANCUSGDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteANCUSG");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objInvestigationDetails.ID);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objInvestigationDetails.UnitID);
                dbServer.AddInParameter(command, "ANCID", DbType.Int64, objInvestigationDetails.ANCID);
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
        public override IValueObject DeleteInvestigation(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteANCInvestigationBizActionVO BizActionObj = valueObject as clsDeleteANCInvestigationBizActionVO;
            try
            {
                clsANCInvestigationDetailsVO objInvestigationDetails = BizActionObj.ANCInvestigationDetails;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteANCInvestigation");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objInvestigationDetails.UnitID);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objInvestigationDetails.ID);
                dbServer.AddInParameter(command, "ANCID", DbType.Int64, objInvestigationDetails.ANCID);
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
        #endregion

        #region Examination
        public override IValueObject AddUpdateExaminationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            AddUpdateANCExaminationBizActionVO BizActionObj = valueObject as AddUpdateANCExaminationBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                clsANCExaminationDetailsVO objInvestigationDetails = BizActionObj.ANCExaminationDetails;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Delete from T_ANC_ExaminationDetails where ANCID = " + BizActionObj.ANCExaminationDetails.ANCID + " And PatientID=" + BizActionObj.ANCExaminationDetails.PatientID + " And PatientUnitID=" + BizActionObj.ANCExaminationDetails.PatientUnitID);
                int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);

                foreach (var item in BizActionObj.ANCExaminationDetailsList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateANCExamination");

                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objInvestigationDetails.UnitID);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, objInvestigationDetails.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objInvestigationDetails.PatientUnitID);
                    dbServer.AddInParameter(command, "CoupleID", DbType.Int64, objInvestigationDetails.CoupleID);
                    dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, objInvestigationDetails.CoupleUnitID);
                    dbServer.AddInParameter(command, "ANCID", DbType.Int64, objInvestigationDetails.ANCID);
                    dbServer.AddInParameter(command, "ExaDate", DbType.DateTime, item.ExaDate);
                    dbServer.AddInParameter(command, "ExaTime", DbType.DateTime, item.ExaTime);
                    dbServer.AddInParameter(command, "Doctor", DbType.Int64, item.Doctor);
                    dbServer.AddInParameter(command, "PeriodOfPreg", DbType.String, item.PeriodOfPreg);
                    dbServer.AddInParameter(command, "Bp", DbType.Single, item.BP);
                    dbServer.AddInParameter(command, "Weight", DbType.Single, item.Weight);
                    dbServer.AddInParameter(command, "OademaID", DbType.Int64, item.OademaID);
                    dbServer.AddInParameter(command, "FundalHeight", DbType.Single, item.FundalHeight);
                    dbServer.AddInParameter(command, "PresenAndPos", DbType.Int64, item.PresenAndPos);
                    dbServer.AddInParameter(command, "FHS", DbType.Int64, item.FHS);
                    dbServer.AddInParameter(command, "Remark", DbType.String, item.Remark);
                    dbServer.AddInParameter(command, "RelationtoBrim", DbType.String, item.RelationtoBrim);
                    dbServer.AddInParameter(command, "Treatment", DbType.String, item.Treatment);
                    dbServer.AddInParameter(command, "HTofUterus", DbType.String, item.HTofUterus);
                    dbServer.AddInParameter(command, "AbdGirth", DbType.String, item.AbdGirth);
                    dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objInvestigationDetails.Isfreezed);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, item.Status);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "ID", DbType.Int64, item.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {

                con.Close();
                trans = null;
                con = null;
            }
            return valueObject;
        }
        public override IValueObject GetANCExaminationList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetANCExaminationBizActionVO BizActionObj = valueObject as clsGetANCExaminationBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetANCExaminationList");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ANCExaminationDetails.UnitID);
                dbServer.AddInParameter(command, "ANCID", DbType.Int64, BizActionObj.ANCExaminationDetails.ANCID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ANCExaminationList == null)
                        BizActionObj.ANCExaminationList = new List<clsANCExaminationDetailsVO>();
                    while (reader.Read())
                    {
                        clsANCExaminationDetailsVO Details = new clsANCExaminationDetailsVO();
                        Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        Details.ExaDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ExaDate"]));
                        Details.ExaTime = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ExaTime"]));
                        Details.Doctor = Convert.ToInt64(DALHelper.HandleDBNull(reader["Doctor"]));
                        Details.PeriodOfPreg = Convert.ToString(DALHelper.HandleDBNull(reader["PeriodOfPreg"]));
                        Details.BP = Convert.ToSingle(DALHelper.HandleDBNull(reader["Bp"]));
                        Details.Weight = Convert.ToSingle(DALHelper.HandleDBNull(reader["Weight"]));
                        Details.Oadema = Convert.ToString(DALHelper.HandleDBNull(reader["Oadema"]));
                        Details.FundalHeight = Convert.ToSingle(DALHelper.HandleDBNull(reader["FundalHeight"]));
                        Details.PresenAndPos = Convert.ToInt64(DALHelper.HandleDBNull(reader["PresenAndPos"]));
                        Details.FHS = Convert.ToInt64(DALHelper.HandleDBNull(reader["FHS"]));
                        Details.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        Details.Status = (Boolean)DALHelper.HandleDBNull(reader["Status"]);
                        Details.Isfreezed = (Boolean)DALHelper.HandleDBNull(reader["IsFreezed"]);
                        Details.Treatment = Convert.ToString(DALHelper.HandleDBNull(reader["Treatment"]));
                        Details.HTofUterus = Convert.ToString(DALHelper.HandleDBNull(reader["HTofUterus"]));
                        Details.RelationtoBrim = Convert.ToString(DALHelper.HandleDBNull(reader["RelationtoBrim"]));
                        Details.AbdGirth = Convert.ToString(DALHelper.HandleDBNull(reader["AbdGirth"]));
                        Details.PresenAndPosDescription = Convert.ToString(DALHelper.HandleDBNull(reader["PresenAndPosDescription"]));
                        Details.FHSDescription = Convert.ToString(DALHelper.HandleDBNull(reader["FHSDescription"]));
                        Details.OademaID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OademaID"]));
                        Details.DoctorName = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorName"]));




                        BizActionObj.ANCExaminationList.Add(Details);
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
        public override IValueObject DeleteExamination(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteANCExaminationBizActionVO BizActionObj = valueObject as clsDeleteANCExaminationBizActionVO;
            try
            {
                clsANCExaminationDetailsVO objInvestigationDetails = BizActionObj.ANCExaminationDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteANCExamination");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objInvestigationDetails.ID);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objInvestigationDetails.UnitID);
                dbServer.AddInParameter(command, "ANCID", DbType.Int64, objInvestigationDetails.ANCID);
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
        #endregion

        #region Document
        public override IValueObject AddUpdateANCDocuments(IValueObject valueObject, clsUserVO UserVo)
        {
            AddUpdateANCDocumentBizActionVO BizActionObj = valueObject as AddUpdateANCDocumentBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                clsANCDocumentsVO objInvestigationDetails = BizActionObj.ANCDocuments;
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Delete from T_ANC_Document where ANCID = " + BizActionObj.ANCDocuments.ANCID + " And PatientID=" + BizActionObj.ANCDocuments.PatientID + " And PatientUnitID=" + BizActionObj.ANCDocuments.PatientUnitID);
                int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);


                foreach (var item in BizActionObj.ANCDocumentsList)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateANCDocument");

                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ANCDocuments.UnitID);
                    dbServer.AddInParameter(command, "DocumentDate", DbType.DateTime, item.Date);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.ANCDocuments.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.ANCDocuments.PatientUnitID);
                    dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.ANCDocuments.CoupleID);
                    dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.ANCDocuments.CoupleUnitID);
                    dbServer.AddInParameter(command, "ANCID", DbType.Int64, BizActionObj.ANCDocuments.ANCID);
                    dbServer.AddInParameter(command, "Description", DbType.String, item.Description);
                    dbServer.AddInParameter(command, "Title", DbType.String, item.Title);
                    dbServer.AddInParameter(command, "AttachedFileName", DbType.String, item.AttachedFileName);
                    dbServer.AddInParameter(command, "AttachedFileContent", DbType.Binary, item.AttachedFileContent);
                    dbServer.AddInParameter(command, "IsDeleted", DbType.Boolean, item.IsDeleted);
                    dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.ANCDocuments.Isfreezed);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.ANCDocuments.Status);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "DocumentID", DbType.Int64, item.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {

                con.Close();
                trans = null;
                con = null;
            }
            return valueObject;

        }
        public override IValueObject GetANCDocumentList(IValueObject valueObject, clsUserVO UserVo)
        {
            GetANCDocumentBizActionVO BizActionObj = valueObject as GetANCDocumentBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetANCDocumentList");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ANCDocument.UnitID);
                dbServer.AddInParameter(command, "ANCID", DbType.Int64, BizActionObj.ANCDocument.ANCID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ANCDocumentList == null)
                        BizActionObj.ANCDocumentList = new List<clsANCDocumentsVO>();
                    while (reader.Read())
                    {
                        clsANCDocumentsVO Details = new clsANCDocumentsVO();
                        Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        Details.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        Details.Title = Convert.ToString((DALHelper.HandleDBNull(reader["Title"])));
                        Details.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Details.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        Details.AttachedFileContent = (Byte[])(DALHelper.HandleDBNull(reader["AttachedFileContent"]));
                        Details.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        BizActionObj.ANCDocumentList.Add(Details);
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
        public override IValueObject DeleteDocument(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteANCDocumentBizActionVO BizActionObj = valueObject as clsDeleteANCDocumentBizActionVO;
            try
            {
                clsANCDocumentsVO objInvestigationDetails = BizActionObj.ANCDocuments;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteANCDocument");

                dbServer.AddInParameter(command, "ID", DbType.Int64, objInvestigationDetails.ID);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objInvestigationDetails.CoupleUnitID);
                dbServer.AddInParameter(command, "ANCID", DbType.Int64, objInvestigationDetails.ANCID);
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

        #endregion

        #region Suggestion
        public override IValueObject AddUpdateSuggestion(IValueObject valueObject, clsUserVO UserVo)
        {
            AddUpdateANCSuggestionBizActionVO BizActionObj = valueObject as AddUpdateANCSuggestionBizActionVO;
            try
            {
                clsANCSuggestionVO objInvestigationDetails = BizActionObj.ANCSuggestion;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateANCSuggestion");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ANCSuggestion.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ANCSuggestion.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.ANCSuggestion.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.ANCSuggestion.PatientUnitID);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.ANCSuggestion.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.ANCSuggestion.CoupleUnitID);
                dbServer.AddInParameter(command, "ANCID", DbType.Int64, BizActionObj.ANCSuggestion.ANCID);
                dbServer.AddInParameter(command, "Smoking", DbType.Boolean, BizActionObj.ANCSuggestion.Smoking);
                dbServer.AddInParameter(command, "Alcoholic", DbType.Boolean, BizActionObj.ANCSuggestion.Alcoholic);
                dbServer.AddInParameter(command, "Medication", DbType.Boolean, BizActionObj.ANCSuggestion.Medication);
                dbServer.AddInParameter(command, "Exercise", DbType.Boolean, BizActionObj.ANCSuggestion.Exercise);
                dbServer.AddInParameter(command, "Xray", DbType.Boolean, BizActionObj.ANCSuggestion.Xray);
                dbServer.AddInParameter(command, "IrregularDiet", DbType.Boolean, BizActionObj.ANCSuggestion.IrregularDiet);
                dbServer.AddInParameter(command, "Exertion", DbType.Boolean, BizActionObj.ANCSuggestion.Exertion);
                dbServer.AddInParameter(command, "Cplace", DbType.Boolean, BizActionObj.ANCSuggestion.Cplace);
                dbServer.AddInParameter(command, "HeavyObject", DbType.Boolean, BizActionObj.ANCSuggestion.HeavyObject);
                dbServer.AddInParameter(command, "Tea", DbType.Boolean, BizActionObj.ANCSuggestion.Tea);
                dbServer.AddInParameter(command, "Bag", DbType.Boolean, BizActionObj.ANCSuggestion.Bag);
                dbServer.AddInParameter(command, "Sheets", DbType.Boolean, BizActionObj.ANCSuggestion.Sheets);
                dbServer.AddInParameter(command, "Remark", DbType.String, BizActionObj.ANCSuggestion.Remark);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.ANCSuggestion.Isfreezed);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.ANCSuggestion.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ANCSuggestion.UnitID = (long)dbServer.GetParameterValue(command, "UnitID");

                if (BizActionObj.ANCSuggestion.IsClosed == true)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdateANCTherapyStatus");
                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.ANCSuggestion.PatientID);
                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.ANCSuggestion.PatientUnitID);
                    dbServer.AddInParameter(command1, "CoupleID", DbType.Int64, BizActionObj.ANCSuggestion.CoupleID);
                    dbServer.AddInParameter(command1, "CoupleUnitID", DbType.Int64, BizActionObj.ANCSuggestion.CoupleUnitID);
                    dbServer.AddInParameter(command1, "ANCID", DbType.Int64, BizActionObj.ANCSuggestion.ANCID);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "IsClosed", DbType.Boolean, BizActionObj.ANCSuggestion.IsClosed);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1);
                    //  BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                }
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
        public override IValueObject GetANCSuggestion(IValueObject valueObject, clsUserVO UserVo)
        {
            GetANCSuggestionBizActionVO BizActionObj = valueObject as GetANCSuggestionBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetANCSuggestion");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.ANCSuggestion.UnitID);
                dbServer.AddInParameter(command, "ANCID", DbType.Int64, BizActionObj.ANCSuggestion.ANCID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.ANCSuggestion == null)
                        BizActionObj.ANCSuggestion = new clsANCSuggestionVO();
                    while (reader.Read())
                    {
                        BizActionObj.ANCSuggestion.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizActionObj.ANCSuggestion.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizActionObj.ANCSuggestion.Smoking = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Smoking"]));
                        BizActionObj.ANCSuggestion.Alcoholic = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Alcoholic"]));
                        BizActionObj.ANCSuggestion.Medication = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Medication"]));
                        BizActionObj.ANCSuggestion.Exercise = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Exercise"]));
                        BizActionObj.ANCSuggestion.Xray = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Xray"]));
                        BizActionObj.ANCSuggestion.IrregularDiet = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IrregularDiet"]));
                        BizActionObj.ANCSuggestion.Exertion = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Exertion"]));
                        BizActionObj.ANCSuggestion.Cplace = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Cplace"]));
                        BizActionObj.ANCSuggestion.Tea = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Tea"]));
                        BizActionObj.ANCSuggestion.Bag = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Bag"]));
                        BizActionObj.ANCSuggestion.Sheets = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Sheets"]));
                        BizActionObj.ANCSuggestion.HeavyObject = Convert.ToBoolean(DALHelper.HandleDBNull(reader["HeavyObject"]));
                        BizActionObj.ANCSuggestion.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        BizActionObj.ANCSuggestion.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                    }
                }
                reader.NextResult();
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
        #endregion
    }
}

