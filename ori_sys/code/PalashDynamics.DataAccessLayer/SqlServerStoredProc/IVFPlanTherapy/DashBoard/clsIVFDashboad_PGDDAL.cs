using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Data;
using System.IO;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    class clsIVFDashboad_PGDDAL: clsBaseIVFDashboad_PGDDAL
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

        private clsIVFDashboad_PGDDAL()
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

                //added by neena
                ImgIP = System.Configuration.ConfigurationManager.AppSettings["IVFImgIP"];
                ImgVirtualDir = System.Configuration.ConfigurationManager.AppSettings["IVFImgVirtualDir"];
                ImgSaveLocation = System.Configuration.ConfigurationManager.AppSettings["IVFImgSavingLocation"];
                //

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override IValueObject AddUpdatePGDHistoryDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsAddUpdatePGDHistoryBizActionVO BizActionObj = valueObject as clsAddUpdatePGDHistoryBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdatePGDHistory");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.PGDHistoryDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.PGDHistoryDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.PGDHistoryDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.PGDHistoryDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "ChromosomalDisease", DbType.String, BizActionObj.PGDHistoryDetails.ChromosomalDisease);
                dbServer.AddInParameter(command, "XLinkedDominant", DbType.String, BizActionObj.PGDHistoryDetails.XLinkedDominant);
                dbServer.AddInParameter(command, "XLinkedRecessive", DbType.String, BizActionObj.PGDHistoryDetails.XLinkedRecessive);
                dbServer.AddInParameter(command, "AutosomalDominant", DbType.String, BizActionObj.PGDHistoryDetails.AutosomalDominant);
                dbServer.AddInParameter(command, "AutosomalRecessive", DbType.String, BizActionObj.PGDHistoryDetails.AutosomalRecessive);
                dbServer.AddInParameter(command, "Ylinked", DbType.String, BizActionObj.PGDHistoryDetails.Ylinked);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "FamilyHistory", DbType.Int64, BizActionObj.PGDHistoryDetails.FamilyHistory);
                dbServer.AddInParameter(command, "AffectedPartner", DbType.Int64, BizActionObj.PGDHistoryDetails.AffectedPartner);
              
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.PGDHistoryDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.PGDHistoryDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.PGDHistoryDetails = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;

        }
        public override IValueObject GetPGDHistoryDetails(IValueObject valueObject, clsUserVO UserVo)
        {
           // throw new NotImplementedException();
            clsGetPGDHistoryBizActionVO BizAction = valueObject as clsGetPGDHistoryBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetPGDHistoryDetails");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.PGDDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.PGDDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PGDDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PGDDetails.PatientUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.PGDDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.PGDDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.PGDDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BizAction.PGDDetails.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        BizAction.PGDDetails.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        BizAction.PGDDetails.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        BizAction.PGDDetails.ChromosomalDisease = Convert.ToString(DALHelper.HandleDBNull(reader["ChromosomalDisease"]));
                        BizAction.PGDDetails.XLinkedDominant = Convert.ToString(DALHelper.HandleDBNull(reader["XLinkedDominant"]));
                        BizAction.PGDDetails.XLinkedRecessive = Convert.ToString(DALHelper.HandleDBNull(reader["XLinkedRecessive"]));
                        BizAction.PGDDetails.AutosomalDominant = Convert.ToString(DALHelper.HandleDBNull(reader["AutosomalDominant"]));
                        BizAction.PGDDetails.AutosomalRecessive = Convert.ToString(DALHelper.HandleDBNull(reader["AutosomalRecessive"]));
                        BizAction.PGDDetails.Ylinked = Convert.ToString(DALHelper.HandleDBNull(reader["Ylinked"]));
                        BizAction.PGDDetails.FamilyHistory = Convert.ToInt64(DALHelper.HandleDBNull(reader["FamilyHistory"]));
                        BizAction.PGDDetails.AffectedPartner = Convert.ToInt64(DALHelper.HandleDBNull(reader["AffectedPartner"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return BizAction;
        }
        public override IValueObject AddUpdatePGDGeneralDetails(IValueObject valueObject, clsUserVO UserVo)
        {
           // throw new NotImplementedException();
            clsAddUpdatePGDGeneralDetailsBizActionVO BizActionObj = valueObject as clsAddUpdatePGDGeneralDetailsBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {

                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdatePGDGeneralDetails");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "LabDayNo", DbType.Int64, BizActionObj.PGDGeneralDetails.LabDayNo);
                dbServer.AddInParameter(command, "LabDayID", DbType.Int64, BizActionObj.PGDGeneralDetails.LabDayID);
                dbServer.AddInParameter(command, "LabDayUnitID", DbType.Int64, BizActionObj.PGDGeneralDetails.LabDayUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.PGDGeneralDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.PGDGeneralDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, BizActionObj.PGDGeneralDetails.OocyteNumber);
                dbServer.AddInParameter(command, "SerialEmbNumber", DbType.Int64, BizActionObj.PGDGeneralDetails.SerialEmbNumber);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.PGDGeneralDetails.Date);
                dbServer.AddInParameter(command, "SourceURL", DbType.String, BizActionObj.PGDGeneralDetails.SourceURL);
                dbServer.AddInParameter(command, "FileName", DbType.String, BizActionObj.PGDGeneralDetails.FileName);
                dbServer.AddInParameter(command, "Physician", DbType.Int64, BizActionObj.PGDGeneralDetails.Physician);
                dbServer.AddInParameter(command, "BiospyID", DbType.Int64, BizActionObj.PGDGeneralDetails.BiospyID);
                dbServer.AddInParameter(command, "ReferringFacility", DbType.String, BizActionObj.PGDGeneralDetails.ReferringFacility);
                dbServer.AddInParameter(command, "ResonOfReferal", DbType.String, BizActionObj.PGDGeneralDetails.ResonOfReferal);
                dbServer.AddInParameter(command, "MainFISHRemark", DbType.String, BizActionObj.PGDGeneralDetails.MainFISHRemark);
                dbServer.AddInParameter(command, "MainKaryotypingRemark", DbType.String, BizActionObj.PGDGeneralDetails.MainKaryotypingRemark);
                dbServer.AddInParameter(command, "SpecimanUsedID", DbType.Int64, BizActionObj.PGDGeneralDetails.SpecimanUsedID);
                dbServer.AddInParameter(command, "TechniqueID", DbType.Int64, BizActionObj.PGDGeneralDetails.TechniqueID);
               
                //
                dbServer.AddInParameter(command, "TestOrderedID", DbType.Int64, BizActionObj.PGDGeneralDetails.TestOrderedID);
                dbServer.AddInParameter(command, "ResultID", DbType.Int64, BizActionObj.PGDGeneralDetails.ResultID);
                dbServer.AddInParameter(command, "ReferringID", DbType.Int64, BizActionObj.PGDGeneralDetails.ReferringID);
                dbServer.AddInParameter(command, "SampleReceiveDate", DbType.DateTime, BizActionObj.PGDGeneralDetails.SampleReceiveDate);
                dbServer.AddInParameter(command, "ResultDate", DbType.DateTime, BizActionObj.PGDGeneralDetails.ResultDate);
                dbServer.AddInParameter(command, "MainFISHInterpretation", DbType.String, BizActionObj.PGDGeneralDetails.MainFISHInterpretation);
                dbServer.AddInParameter(command, "SupervisedById", DbType.Int64, BizActionObj.PGDGeneralDetails.SupervisedById);

                dbServer.AddInParameter(command, "PGDIndicationID", DbType.Int64, BizActionObj.PGDGeneralDetails.PGDIndicationID);                  // For IVF ADM Changes
                dbServer.AddInParameter(command, "PGDIndicationDetails", DbType.String, BizActionObj.PGDGeneralDetails.PGDIndicationDetails);       // For IVF ADM Changes

                dbServer.AddInParameter(command, "PGDResult", DbType.String, BizActionObj.PGDGeneralDetails.PGDResult);                             // For IVF ADM Changes
                dbServer.AddInParameter(command, "ReferringFacilityID", DbType.Int64, BizActionObj.PGDGeneralDetails.ReferringFacilityID);          // For IVF ADM Changes
                dbServer.AddInParameter(command, "PGDPGSProcedureID", DbType.Int64, BizActionObj.PGDGeneralDetails.PGDPGSProcedureID);
                dbServer.AddInParameter(command, "FrozenPGDPGS", DbType.Boolean, BizActionObj.PGDGeneralDetails.FrozenPGDPGS);  
                //

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.PGDGeneralDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.PGDGeneralDetails.ID = (long)dbServer.GetParameterValue(command, "ID");
                if (BizActionObj.PGDFISHList != null)
                {
                    if (BizActionObj.PGDFISHList.Count > 0)
                    {
                        foreach (var item in BizActionObj.PGDFISHList)
                        {
                            try
                            {
                                DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdatePGDFISHDetails");
                                if (item.ID > 0)
                                {
                                    dbServer.AddInParameter(command1, "ID", DbType.Int64, item.ID);
                                }
                                else
                                {
                                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                                }
                                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "PGDGeneralDetailsID", DbType.Int64, BizActionObj.PGDGeneralDetails.ID);
                                dbServer.AddInParameter(command1, "PGDGeneralDetailsUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "LabDayNo", DbType.Int64, BizActionObj.PGDGeneralDetails.LabDayNo);
                                dbServer.AddInParameter(command1, "LabDayID", DbType.Int64, BizActionObj.PGDGeneralDetails.LabDayID);
                                dbServer.AddInParameter(command1, "LabDayUnitID", DbType.Int64, BizActionObj.PGDGeneralDetails.LabDayUnitID);
                                dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.PGDGeneralDetails.OocyteNumber);
                                dbServer.AddInParameter(command1, "SerialEmbNumber", DbType.Int64, BizActionObj.PGDGeneralDetails.SerialEmbNumber);
                                dbServer.AddInParameter(command1, "ChromosomeStudiedID", DbType.Int64, item.ChromosomeStudiedID);
                                dbServer.AddInParameter(command1, "TestOrderedID", DbType.Int64, item.TestOrderedID);
                                dbServer.AddInParameter(command1, "NoOfCellCounted", DbType.String, item.NoOfCellCounted);
                                dbServer.AddInParameter(command1, "Result", DbType.String, item.Result);
                                dbServer.AddInParameter(command1, "Status", DbType.Boolean, item.Status);
                                dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                                int intStatus1 = 0;
                                intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                    }
                }
                if (BizActionObj.PGDKaryotypingList != null)
                {
                    if (BizActionObj.PGDKaryotypingList.Count > 0)
                    {
                        foreach (var item in BizActionObj.PGDKaryotypingList)
                        {
                            try
                            {
                                DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdatePGDKaryotypingDetails");
                                if (item.ID > 0)
                                {
                                    dbServer.AddInParameter(command1, "ID", DbType.Int64, item.ID);
                                }
                                else
                                {
                                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                                }
                                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "PGDGeneralDetailsID", DbType.Int64, BizActionObj.PGDGeneralDetails.ID);
                                dbServer.AddInParameter(command1, "PGDGeneralDetailsUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "LabDayNo", DbType.Int64, BizActionObj.PGDGeneralDetails.LabDayNo);
                                dbServer.AddInParameter(command1, "LabDayID", DbType.Int64, BizActionObj.PGDGeneralDetails.LabDayID);
                                dbServer.AddInParameter(command1, "LabDayUnitID", DbType.Int64, BizActionObj.PGDGeneralDetails.LabDayUnitID);
                                dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.PGDGeneralDetails.OocyteNumber);
                                dbServer.AddInParameter(command1, "SerialEmbNumber", DbType.Int64, BizActionObj.PGDGeneralDetails.SerialEmbNumber);
                                dbServer.AddInParameter(command1, "ChromosomeStudiedID", DbType.Int64, item.ChromosomeStudiedID);
                                dbServer.AddInParameter(command1, "CultureTypeID", DbType.Int64, item.CultureTypeID);
                                dbServer.AddInParameter(command1, "BindingTechnique", DbType.Int64, item.BindingTechnique);
                                dbServer.AddInParameter(command1, "MetaphaseCounted", DbType.String, item.MetaphaseCounted);
                                dbServer.AddInParameter(command1, "MetaphaseAnalysed", DbType.String, item.MetaphaseAnalysed);
                                dbServer.AddInParameter(command1, "MetaphaseKaryotype", DbType.String, item.MetaphaseKaryotype);
                                dbServer.AddInParameter(command1, "Result", DbType.String, item.Result);
                                dbServer.AddInParameter(command1, "Status", DbType.Boolean, item.Status);
                                dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                                int intStatus1 = 0;
                                intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                    }
                }

                //if (BizActionObj.Day0Details.Photo != null)
                if (BizActionObj.PGDGeneralDetails.ImgList != null && BizActionObj.PGDGeneralDetails.ImgList.Count > 0)
                {
                    int cnt = 0;
                    foreach (var item in BizActionObj.PGDGeneralDetails.ImgList)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdatePGDPGSImage");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "PatientID", DbType.Int64, item.PatientID);
                        dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                        dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.PGDGeneralDetails.PlanTherapyID);
                        dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.PGDGeneralDetails.PlanTherapyUnitID);
                        dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.PGDGeneralDetails.SerialEmbNumber);
                        dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.PGDGeneralDetails.OocyteNumber);
                        dbServer.AddInParameter(command1, "Day", DbType.Int64, item.Day);
                        dbServer.AddInParameter(command1, "DayID", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, 0);
                        //dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day0Details.Photo);
                        //dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day0Details.FileName);
                        //dbServer.AddInParameter(command1, "Photo", DbType.Binary, item.Photo);
                        dbServer.AddInParameter(command1, "FileName", DbType.String, item.ImagePath);
                        //dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                        dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        //added by neena
                        if (item.SeqNo == null)
                            dbServer.AddParameter(command1, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        else
                            dbServer.AddParameter(command1, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.SeqNo);
                        //dbServer.AddInParameter(command1, "SeqNo", DbType.String, item.SeqNo);
                        dbServer.AddInParameter(command1, "IsApplyTo", DbType.Int32, 0);

                        if (string.IsNullOrEmpty(item.ServerImageName))
                            dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        else
                            dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ServerImageName);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        //

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        item.ID = Convert.ToInt64(dbServer.GetParameterValue(command1, "ID"));
                        item.ServerImageName = Convert.ToString(dbServer.GetParameterValue(command1, "ServerImageName"));
                        item.SeqNo = Convert.ToString(dbServer.GetParameterValue(command1, "SeqNo"));
                        if (item.Photo != null)
                            File.WriteAllBytes(ImgSaveLocation + item.ServerImageName, item.Photo);

                        //cnt++;
                    }
                }



                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.PGDGeneralDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;

        }
        public override IValueObject GetPGDGeneralDetails(IValueObject valueObject, clsUserVO UserVo)
        {
           // throw new NotImplementedException();
            clsGetPGDGeneralDetailsBizActionVO BizAction = valueObject as clsGetPGDGeneralDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_PGDGeneralDetails");
                dbServer.AddInParameter(command, "LabDayID", DbType.Int64, BizAction.PGDGeneralDetails.LabDayID);
                dbServer.AddInParameter(command, "LabDayUnitID", DbType.Int64, BizAction.PGDGeneralDetails.LabDayUnitID);
                dbServer.AddInParameter(command, "LabDayNo", DbType.Int64, BizAction.PGDGeneralDetails.LabDayNo);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.PGDGeneralDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.PGDGeneralDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialEmbNumber", DbType.Int64, BizAction.PGDGeneralDetails.SerialEmbNumber);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.PGDGeneralDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.PGDGeneralDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.PGDGeneralDetails.LabDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayNo"]));
                        BizAction.PGDGeneralDetails.LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"]));
                        BizAction.PGDGeneralDetails.LabDayUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayUnitID"]));
                        BizAction.PGDGeneralDetails.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        BizAction.PGDGeneralDetails.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        BizAction.PGDGeneralDetails.SerialEmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialEmbNumber"]));
                        BizAction.PGDGeneralDetails.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        BizAction.PGDGeneralDetails.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        BizAction.PGDGeneralDetails.SourceURL = Convert.ToString(DALHelper.HandleDBNull(reader["SourceURL"]));
                        BizAction.PGDGeneralDetails.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        BizAction.PGDGeneralDetails.Physician = Convert.ToInt64(DALHelper.HandleDBNull(reader["Physician"]));
                        BizAction.PGDGeneralDetails.BiospyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BiospyID"]));
                        BizAction.PGDGeneralDetails.ReferringFacility = Convert.ToString(DALHelper.HandleDBNull(reader["ReferringFacility"]));
                        BizAction.PGDGeneralDetails.ResonOfReferal = Convert.ToString(DALHelper.HandleDBNull(reader["ResonOfReferal"]));
                        BizAction.PGDGeneralDetails.SpecimanUsedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecimanUsedID"]));
                        BizAction.PGDGeneralDetails.TechniqueID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TechniqueID"]));
                        BizAction.PGDGeneralDetails.ResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResultID"]));

                        #region For ART Flow - PGD

                        BizAction.PGDGeneralDetails.MainFISHRemark = Convert.ToString(DALHelper.HandleDBNull(reader["MainFISHRemark"]));
                        BizAction.PGDGeneralDetails.MainKaryotypingRemark = Convert.ToString(DALHelper.HandleDBNull(reader["MainKaryotypingRemark"]));

                        BizAction.PGDGeneralDetails.SampleReceiveDate = (DateTime?)(DALHelper.HandleDate(reader["SampleReceiveDate"]));
                        BizAction.PGDGeneralDetails.ResultDate = (DateTime?)(DALHelper.HandleDate(reader["ResultDate"]));
                        BizAction.PGDGeneralDetails.ReferringID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferringId"]));
                        BizAction.PGDGeneralDetails.SupervisedById = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupervisedById"]));
                        BizAction.PGDGeneralDetails.TestOrderedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestOrderedID"]));
                        BizAction.PGDGeneralDetails.MainFISHInterpretation = Convert.ToString(DALHelper.HandleDBNull(reader["MainFISHInterpretation"]));

                        BizAction.PGDGeneralDetails.PGDIndicationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PGDIndicationID"]));               // For IVF ADM Changes
                        BizAction.PGDGeneralDetails.PGDIndicationDetails = Convert.ToString(DALHelper.HandleDBNull(reader["PGDIndicationDetails"]));    // For IVF ADM Changes

                        BizAction.PGDGeneralDetails.PGDResult = Convert.ToString(DALHelper.HandleDBNull(reader["PGDResult"]));                          // For IVF ADM Changes
                        BizAction.PGDGeneralDetails.ReferringFacilityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferringFacilityID"]));       // For IVF ADM Changes
                        BizAction.PGDGeneralDetails.PGDPGSProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PGDPGSProcedureID"]));       // For IVF ADM Changes
                      
                        #endregion
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPGDFISHVO FISH = new clsPGDFISHVO();
                        FISH.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        FISH.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        FISH.LabDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayNo"]));
                        FISH.LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"]));
                        FISH.LabDayUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayUnitID"]));
                        FISH.SerialEmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialEmbNumber"]));
                        FISH.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        FISH.ChromosomeStudiedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ChromosomeStudiedID"]));
                        FISH.TestOrderedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestOrderedID"]));
                        FISH.NoOfCellCounted = Convert.ToString(DALHelper.HandleDBNull(reader["NoOfCellCounted"]));
                        FISH.Result = Convert.ToString(DALHelper.HandleDBNull(reader["Result"]));
                        FISH.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizAction.PGDFISHList.Add(FISH);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPGDKaryotypingVO Karyotyping = new clsPGDKaryotypingVO();
                        Karyotyping.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Karyotyping.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        Karyotyping.LabDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayNo"]));
                        Karyotyping.LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"]));
                        Karyotyping.LabDayUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayUnitID"]));
                        Karyotyping.SerialEmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialEmbNumber"]));
                        Karyotyping.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        Karyotyping.ChromosomeStudiedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ChromosomeStudiedID"]));
                        Karyotyping.BindingTechnique = Convert.ToInt64(DALHelper.HandleDBNull(reader["BindingTechnique"]));
                        Karyotyping.CultureTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CultureTypeID"]));
                        Karyotyping.MetaphaseCounted = Convert.ToString(DALHelper.HandleDBNull(reader["MetaphaseCounted"]));
                        Karyotyping.MetaphaseAnalysed = Convert.ToString(DALHelper.HandleDBNull(reader["MetaphaseAnalysed"]));
                        Karyotyping.MetaphaseKaryotype = Convert.ToString(DALHelper.HandleDBNull(reader["MetaphaseKaryotype"]));
                        Karyotyping.Result = Convert.ToString(DALHelper.HandleDBNull(reader["Result"]));
                        Karyotyping.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizAction.PGDKaryotypingList.Add(Karyotyping);
                    }
                }

                reader.NextResult();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAddImageVO ObjImg = new clsAddImageVO();
                        ObjImg.ImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ObjImg.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ObjImg.OriginalImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["ServerImageName"]));
                        //string imageName = Convert.ToString(DALHelper.HandleDBNull(reader["ServerImageName"]));
                        ObjImg.SeqNo = Convert.ToString(DALHelper.HandleDBNull(reader["SeqNo"]));
                        ObjImg.Photo = (byte[])(DALHelper.HandleDBNull(reader["Image"]));
                        ObjImg.Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]));
                        ObjImg.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        ObjImg.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));

                        if (!string.IsNullOrEmpty(ObjImg.OriginalImagePath))
                            ObjImg.ServerImageName = "..//" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;
                        //ObjImg.ServerImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;


                        BizAction.PGDGeneralDetails.ImgList.Add(ObjImg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return BizAction;
        }
       
    }


}
