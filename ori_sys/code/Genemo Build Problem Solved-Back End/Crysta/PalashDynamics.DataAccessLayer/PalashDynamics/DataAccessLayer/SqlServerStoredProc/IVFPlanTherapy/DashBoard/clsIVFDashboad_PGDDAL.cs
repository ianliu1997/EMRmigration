namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.DashBoardVO;
    using PalashDynamics.ValueObjects.IVFPlanTherapy;
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.IO;

    internal class clsIVFDashboad_PGDDAL : clsBaseIVFDashboad_PGDDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private string ImgIP = string.Empty;
        private string ImgVirtualDir = string.Empty;
        private string ImgSaveLocation = string.Empty;

        private clsIVFDashboad_PGDDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                if (this.logManager == null)
                {
                    this.logManager = LogManager.GetInstance();
                }
                this.ImgIP = ConfigurationManager.AppSettings["IVFImgIP"];
                this.ImgVirtualDir = ConfigurationManager.AppSettings["IVFImgVirtualDir"];
                this.ImgSaveLocation = ConfigurationManager.AppSettings["IVFImgSavingLocation"];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddUpdatePGDGeneralDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePGDGeneralDetailsBizActionVO nvo = valueObject as clsAddUpdatePGDGeneralDetailsBizActionVO;
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdatePGDGeneralDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "LabDayNo", DbType.Int64, nvo.PGDGeneralDetails.LabDayNo);
                this.dbServer.AddInParameter(storedProcCommand, "LabDayID", DbType.Int64, nvo.PGDGeneralDetails.LabDayID);
                this.dbServer.AddInParameter(storedProcCommand, "LabDayUnitID", DbType.Int64, nvo.PGDGeneralDetails.LabDayUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PGDGeneralDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PGDGeneralDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "OocyteNumber", DbType.Int64, nvo.PGDGeneralDetails.OocyteNumber);
                this.dbServer.AddInParameter(storedProcCommand, "SerialEmbNumber", DbType.Int64, nvo.PGDGeneralDetails.SerialEmbNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, nvo.PGDGeneralDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, nvo.PGDGeneralDetails.SourceURL);
                this.dbServer.AddInParameter(storedProcCommand, "FileName", DbType.String, nvo.PGDGeneralDetails.FileName);
                this.dbServer.AddInParameter(storedProcCommand, "Physician", DbType.Int64, nvo.PGDGeneralDetails.Physician);
                this.dbServer.AddInParameter(storedProcCommand, "BiospyID", DbType.Int64, nvo.PGDGeneralDetails.BiospyID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferringFacility", DbType.String, nvo.PGDGeneralDetails.ReferringFacility);
                this.dbServer.AddInParameter(storedProcCommand, "ResonOfReferal", DbType.String, nvo.PGDGeneralDetails.ResonOfReferal);
                this.dbServer.AddInParameter(storedProcCommand, "MainFISHRemark", DbType.String, nvo.PGDGeneralDetails.MainFISHRemark);
                this.dbServer.AddInParameter(storedProcCommand, "MainKaryotypingRemark", DbType.String, nvo.PGDGeneralDetails.MainKaryotypingRemark);
                this.dbServer.AddInParameter(storedProcCommand, "SpecimanUsedID", DbType.Int64, nvo.PGDGeneralDetails.SpecimanUsedID);
                this.dbServer.AddInParameter(storedProcCommand, "TechniqueID", DbType.Int64, nvo.PGDGeneralDetails.TechniqueID);
                this.dbServer.AddInParameter(storedProcCommand, "TestOrderedID", DbType.Int64, nvo.PGDGeneralDetails.TestOrderedID);
                this.dbServer.AddInParameter(storedProcCommand, "ResultID", DbType.Int64, nvo.PGDGeneralDetails.ResultID);
                this.dbServer.AddInParameter(storedProcCommand, "ReferringID", DbType.Int64, nvo.PGDGeneralDetails.ReferringID);
                this.dbServer.AddInParameter(storedProcCommand, "SampleReceiveDate", DbType.DateTime, nvo.PGDGeneralDetails.SampleReceiveDate);
                this.dbServer.AddInParameter(storedProcCommand, "ResultDate", DbType.DateTime, nvo.PGDGeneralDetails.ResultDate);
                this.dbServer.AddInParameter(storedProcCommand, "MainFISHInterpretation", DbType.String, nvo.PGDGeneralDetails.MainFISHInterpretation);
                this.dbServer.AddInParameter(storedProcCommand, "SupervisedById", DbType.Int64, nvo.PGDGeneralDetails.SupervisedById);
                this.dbServer.AddInParameter(storedProcCommand, "PGDIndicationID", DbType.Int64, nvo.PGDGeneralDetails.PGDIndicationID);
                this.dbServer.AddInParameter(storedProcCommand, "PGDIndicationDetails", DbType.String, nvo.PGDGeneralDetails.PGDIndicationDetails);
                this.dbServer.AddInParameter(storedProcCommand, "PGDResult", DbType.String, nvo.PGDGeneralDetails.PGDResult);
                this.dbServer.AddInParameter(storedProcCommand, "ReferringFacilityID", DbType.Int64, nvo.PGDGeneralDetails.ReferringFacilityID);
                this.dbServer.AddInParameter(storedProcCommand, "PGDPGSProcedureID", DbType.Int64, nvo.PGDGeneralDetails.PGDPGSProcedureID);
                this.dbServer.AddInParameter(storedProcCommand, "FrozenPGDPGS", DbType.Boolean, nvo.PGDGeneralDetails.FrozenPGDPGS);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.PGDGeneralDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.PGDGeneralDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if ((nvo.PGDFISHList != null) && (nvo.PGDFISHList.Count > 0))
                {
                    foreach (clsPGDFISHVO spgdfishvo in nvo.PGDFISHList)
                    {
                        try
                        {
                            DbCommand command2 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdatePGDFISHDetails");
                            if (spgdfishvo.ID > 0L)
                            {
                                this.dbServer.AddInParameter(command2, "ID", DbType.Int64, spgdfishvo.ID);
                            }
                            else
                            {
                                this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            }
                            this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command2, "PGDGeneralDetailsID", DbType.Int64, nvo.PGDGeneralDetails.ID);
                            this.dbServer.AddInParameter(command2, "PGDGeneralDetailsUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command2, "LabDayNo", DbType.Int64, nvo.PGDGeneralDetails.LabDayNo);
                            this.dbServer.AddInParameter(command2, "LabDayID", DbType.Int64, nvo.PGDGeneralDetails.LabDayID);
                            this.dbServer.AddInParameter(command2, "LabDayUnitID", DbType.Int64, nvo.PGDGeneralDetails.LabDayUnitID);
                            this.dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, nvo.PGDGeneralDetails.OocyteNumber);
                            this.dbServer.AddInParameter(command2, "SerialEmbNumber", DbType.Int64, nvo.PGDGeneralDetails.SerialEmbNumber);
                            this.dbServer.AddInParameter(command2, "ChromosomeStudiedID", DbType.Int64, spgdfishvo.ChromosomeStudiedID);
                            this.dbServer.AddInParameter(command2, "TestOrderedID", DbType.Int64, spgdfishvo.TestOrderedID);
                            this.dbServer.AddInParameter(command2, "NoOfCellCounted", DbType.String, spgdfishvo.NoOfCellCounted);
                            this.dbServer.AddInParameter(command2, "Result", DbType.String, spgdfishvo.Result);
                            this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, spgdfishvo.Status);
                            this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command2, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                if ((nvo.PGDKaryotypingList != null) && (nvo.PGDKaryotypingList.Count > 0))
                {
                    foreach (clsPGDKaryotypingVO gvo in nvo.PGDKaryotypingList)
                    {
                        try
                        {
                            DbCommand command3 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdatePGDKaryotypingDetails");
                            if (gvo.ID > 0L)
                            {
                                this.dbServer.AddInParameter(command3, "ID", DbType.Int64, gvo.ID);
                            }
                            else
                            {
                                this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            }
                            this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command3, "PGDGeneralDetailsID", DbType.Int64, nvo.PGDGeneralDetails.ID);
                            this.dbServer.AddInParameter(command3, "PGDGeneralDetailsUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command3, "LabDayNo", DbType.Int64, nvo.PGDGeneralDetails.LabDayNo);
                            this.dbServer.AddInParameter(command3, "LabDayID", DbType.Int64, nvo.PGDGeneralDetails.LabDayID);
                            this.dbServer.AddInParameter(command3, "LabDayUnitID", DbType.Int64, nvo.PGDGeneralDetails.LabDayUnitID);
                            this.dbServer.AddInParameter(command3, "OocyteNumber", DbType.Int64, nvo.PGDGeneralDetails.OocyteNumber);
                            this.dbServer.AddInParameter(command3, "SerialEmbNumber", DbType.Int64, nvo.PGDGeneralDetails.SerialEmbNumber);
                            this.dbServer.AddInParameter(command3, "ChromosomeStudiedID", DbType.Int64, gvo.ChromosomeStudiedID);
                            this.dbServer.AddInParameter(command3, "CultureTypeID", DbType.Int64, gvo.CultureTypeID);
                            this.dbServer.AddInParameter(command3, "BindingTechnique", DbType.Int64, gvo.BindingTechnique);
                            this.dbServer.AddInParameter(command3, "MetaphaseCounted", DbType.String, gvo.MetaphaseCounted);
                            this.dbServer.AddInParameter(command3, "MetaphaseAnalysed", DbType.String, gvo.MetaphaseAnalysed);
                            this.dbServer.AddInParameter(command3, "MetaphaseKaryotype", DbType.String, gvo.MetaphaseKaryotype);
                            this.dbServer.AddInParameter(command3, "Result", DbType.String, gvo.Result);
                            this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, gvo.Status);
                            this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                            this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                            this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command3, transaction);
                            nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
                if ((nvo.PGDGeneralDetails.ImgList != null) && (nvo.PGDGeneralDetails.ImgList.Count > 0))
                {
                    foreach (clsAddImageVO evo in nvo.PGDGeneralDetails.ImgList)
                    {
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdatePGDPGSImage");
                        this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "PatientID", DbType.Int64, evo.PatientID);
                        this.dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, evo.PatientUnitID);
                        this.dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, nvo.PGDGeneralDetails.PlanTherapyID);
                        this.dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, nvo.PGDGeneralDetails.PlanTherapyUnitID);
                        this.dbServer.AddInParameter(command4, "SerialOocyteNumber", DbType.Int64, nvo.PGDGeneralDetails.SerialEmbNumber);
                        this.dbServer.AddInParameter(command4, "OocyteNumber", DbType.Int64, nvo.PGDGeneralDetails.OocyteNumber);
                        this.dbServer.AddInParameter(command4, "Day", DbType.Int64, evo.Day);
                        this.dbServer.AddInParameter(command4, "DayID", DbType.Int64, evo.ID);
                        this.dbServer.AddInParameter(command4, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "CellStageID", DbType.Int64, 0);
                        this.dbServer.AddInParameter(command4, "FileName", DbType.String, evo.ImagePath);
                        this.dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        if (evo.SeqNo == null)
                        {
                            this.dbServer.AddParameter(command4, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command4, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.SeqNo);
                        }
                        this.dbServer.AddInParameter(command4, "IsApplyTo", DbType.Int32, 0);
                        if (string.IsNullOrEmpty(evo.ServerImageName))
                        {
                            this.dbServer.AddParameter(command4, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        }
                        else
                        {
                            this.dbServer.AddParameter(command4, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ServerImageName);
                        }
                        this.dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ID);
                        this.dbServer.ExecuteNonQuery(command4, transaction);
                        evo.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command4, "ID"));
                        evo.ServerImageName = Convert.ToString(this.dbServer.GetParameterValue(command4, "ServerImageName"));
                        evo.SeqNo = Convert.ToString(this.dbServer.GetParameterValue(command4, "SeqNo"));
                        if (evo.Photo != null)
                        {
                            File.WriteAllBytes(this.ImgSaveLocation + evo.ServerImageName, evo.Photo);
                        }
                    }
                }
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.PGDGeneralDetails = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdatePGDHistoryDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePGDHistoryBizActionVO nvo = valueObject as clsAddUpdatePGDHistoryBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            try
            {
                connection.Open();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdatePGDHistory");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PGDHistoryDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PGDHistoryDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PGDHistoryDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PGDHistoryDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ChromosomalDisease", DbType.String, nvo.PGDHistoryDetails.ChromosomalDisease);
                this.dbServer.AddInParameter(storedProcCommand, "XLinkedDominant", DbType.String, nvo.PGDHistoryDetails.XLinkedDominant);
                this.dbServer.AddInParameter(storedProcCommand, "XLinkedRecessive", DbType.String, nvo.PGDHistoryDetails.XLinkedRecessive);
                this.dbServer.AddInParameter(storedProcCommand, "AutosomalDominant", DbType.String, nvo.PGDHistoryDetails.AutosomalDominant);
                this.dbServer.AddInParameter(storedProcCommand, "AutosomalRecessive", DbType.String, nvo.PGDHistoryDetails.AutosomalRecessive);
                this.dbServer.AddInParameter(storedProcCommand, "Ylinked", DbType.String, nvo.PGDHistoryDetails.Ylinked);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "FamilyHistory", DbType.Int64, nvo.PGDHistoryDetails.FamilyHistory);
                this.dbServer.AddInParameter(storedProcCommand, "AffectedPartner", DbType.Int64, nvo.PGDHistoryDetails.AffectedPartner);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.PGDHistoryDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.PGDHistoryDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.PGDHistoryDetails = null;
            }
            finally
            {
                connection.Close();
            }
            return nvo;
        }

        public override IValueObject GetPGDGeneralDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPGDGeneralDetailsBizActionVO nvo = valueObject as clsGetPGDGeneralDetailsBizActionVO;
            this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_PGDGeneralDetails");
                this.dbServer.AddInParameter(storedProcCommand, "LabDayID", DbType.Int64, nvo.PGDGeneralDetails.LabDayID);
                this.dbServer.AddInParameter(storedProcCommand, "LabDayUnitID", DbType.Int64, nvo.PGDGeneralDetails.LabDayUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "LabDayNo", DbType.Int64, nvo.PGDGeneralDetails.LabDayNo);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PGDGeneralDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PGDGeneralDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SerialEmbNumber", DbType.Int64, nvo.PGDGeneralDetails.SerialEmbNumber);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.PGDGeneralDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.PGDGeneralDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.PGDGeneralDetails.LabDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayNo"]));
                        nvo.PGDGeneralDetails.LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"]));
                        nvo.PGDGeneralDetails.LabDayUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayUnitID"]));
                        nvo.PGDGeneralDetails.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.PGDGeneralDetails.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.PGDGeneralDetails.SerialEmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialEmbNumber"]));
                        nvo.PGDGeneralDetails.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        nvo.PGDGeneralDetails.Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])));
                        nvo.PGDGeneralDetails.SourceURL = Convert.ToString(DALHelper.HandleDBNull(reader["SourceURL"]));
                        nvo.PGDGeneralDetails.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        nvo.PGDGeneralDetails.Physician = Convert.ToInt64(DALHelper.HandleDBNull(reader["Physician"]));
                        nvo.PGDGeneralDetails.BiospyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BiospyID"]));
                        nvo.PGDGeneralDetails.ReferringFacility = Convert.ToString(DALHelper.HandleDBNull(reader["ReferringFacility"]));
                        nvo.PGDGeneralDetails.ResonOfReferal = Convert.ToString(DALHelper.HandleDBNull(reader["ResonOfReferal"]));
                        nvo.PGDGeneralDetails.SpecimanUsedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpecimanUsedID"]));
                        nvo.PGDGeneralDetails.TechniqueID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TechniqueID"]));
                        nvo.PGDGeneralDetails.ResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ResultID"]));
                        nvo.PGDGeneralDetails.MainFISHRemark = Convert.ToString(DALHelper.HandleDBNull(reader["MainFISHRemark"]));
                        nvo.PGDGeneralDetails.MainKaryotypingRemark = Convert.ToString(DALHelper.HandleDBNull(reader["MainKaryotypingRemark"]));
                        nvo.PGDGeneralDetails.SampleReceiveDate = DALHelper.HandleDate(reader["SampleReceiveDate"]);
                        nvo.PGDGeneralDetails.ResultDate = DALHelper.HandleDate(reader["ResultDate"]);
                        nvo.PGDGeneralDetails.ReferringID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferringId"]));
                        nvo.PGDGeneralDetails.SupervisedById = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupervisedById"]));
                        nvo.PGDGeneralDetails.TestOrderedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestOrderedID"]));
                        nvo.PGDGeneralDetails.MainFISHInterpretation = Convert.ToString(DALHelper.HandleDBNull(reader["MainFISHInterpretation"]));
                        nvo.PGDGeneralDetails.PGDIndicationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PGDIndicationID"]));
                        nvo.PGDGeneralDetails.PGDIndicationDetails = Convert.ToString(DALHelper.HandleDBNull(reader["PGDIndicationDetails"]));
                        nvo.PGDGeneralDetails.PGDResult = Convert.ToString(DALHelper.HandleDBNull(reader["PGDResult"]));
                        nvo.PGDGeneralDetails.ReferringFacilityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferringFacilityID"]));
                        nvo.PGDGeneralDetails.PGDPGSProcedureID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PGDPGSProcedureID"]));
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPGDFISHVO item = new clsPGDFISHVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            LabDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayNo"])),
                            LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"])),
                            LabDayUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayUnitID"])),
                            SerialEmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialEmbNumber"])),
                            OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"])),
                            ChromosomeStudiedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ChromosomeStudiedID"])),
                            TestOrderedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestOrderedID"])),
                            NoOfCellCounted = Convert.ToString(DALHelper.HandleDBNull(reader["NoOfCellCounted"])),
                            Result = Convert.ToString(DALHelper.HandleDBNull(reader["Result"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.PGDFISHList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPGDKaryotypingVO item = new clsPGDKaryotypingVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            LabDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayNo"])),
                            LabDayID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayID"])),
                            LabDayUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabDayUnitID"])),
                            SerialEmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialEmbNumber"])),
                            OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"])),
                            ChromosomeStudiedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ChromosomeStudiedID"])),
                            BindingTechnique = Convert.ToInt64(DALHelper.HandleDBNull(reader["BindingTechnique"])),
                            CultureTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CultureTypeID"])),
                            MetaphaseCounted = Convert.ToString(DALHelper.HandleDBNull(reader["MetaphaseCounted"])),
                            MetaphaseAnalysed = Convert.ToString(DALHelper.HandleDBNull(reader["MetaphaseAnalysed"])),
                            MetaphaseKaryotype = Convert.ToString(DALHelper.HandleDBNull(reader["MetaphaseKaryotype"])),
                            Result = Convert.ToString(DALHelper.HandleDBNull(reader["Result"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.PGDKaryotypingList.Add(item);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAddImageVO item = new clsAddImageVO {
                            ImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            OriginalImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["ServerImageName"])),
                            SeqNo = Convert.ToString(DALHelper.HandleDBNull(reader["SeqNo"])),
                            Photo = (byte[]) DALHelper.HandleDBNull(reader["Image"]),
                            Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"])),
                            PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]))
                        };
                        if (!string.IsNullOrEmpty(item.OriginalImagePath))
                        {
                            item.ServerImageName = "..//" + this.ImgVirtualDir + "/" + item.OriginalImagePath;
                        }
                        nvo.PGDGeneralDetails.ImgList.Add(item);
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetPGDHistoryDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPGDHistoryBizActionVO nvo = valueObject as clsGetPGDHistoryBizActionVO;
            this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("IVFDashBoard_GetPGDHistoryDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyID", DbType.Int64, nvo.PGDDetails.PlanTherapyID);
                this.dbServer.AddInParameter(storedProcCommand, "PlanTherapyUnitID", DbType.Int64, nvo.PGDDetails.PlanTherapyUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PGDDetails.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PGDDetails.PatientUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        nvo.PGDDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.PGDDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.PGDDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        nvo.PGDDetails.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        nvo.PGDDetails.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        nvo.PGDDetails.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        nvo.PGDDetails.ChromosomalDisease = Convert.ToString(DALHelper.HandleDBNull(reader["ChromosomalDisease"]));
                        nvo.PGDDetails.XLinkedDominant = Convert.ToString(DALHelper.HandleDBNull(reader["XLinkedDominant"]));
                        nvo.PGDDetails.XLinkedRecessive = Convert.ToString(DALHelper.HandleDBNull(reader["XLinkedRecessive"]));
                        nvo.PGDDetails.AutosomalDominant = Convert.ToString(DALHelper.HandleDBNull(reader["AutosomalDominant"]));
                        nvo.PGDDetails.AutosomalRecessive = Convert.ToString(DALHelper.HandleDBNull(reader["AutosomalRecessive"]));
                        nvo.PGDDetails.Ylinked = Convert.ToString(DALHelper.HandleDBNull(reader["Ylinked"]));
                        nvo.PGDDetails.FamilyHistory = Convert.ToInt64(DALHelper.HandleDBNull(reader["FamilyHistory"]));
                        nvo.PGDDetails.AffectedPartner = Convert.ToInt64(DALHelper.HandleDBNull(reader["AffectedPartner"]));
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }
    }
}

