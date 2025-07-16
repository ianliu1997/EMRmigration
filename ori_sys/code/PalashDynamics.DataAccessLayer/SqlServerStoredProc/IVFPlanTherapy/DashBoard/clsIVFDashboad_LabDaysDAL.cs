using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Data.Common;
using System.Data;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using System.IO;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    class clsIVFDashboad_LabDaysDAL : clsBaseIVFDashboad_LabDaysDAL
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

        private clsIVFDashboad_LabDaysDAL()
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

        #region Day0

        public override IValueObject AddDay0OocList(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_AddDay0OocyteListBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddDay0OocyteListBizActionVO;
            try
            {
                for (int i = 0; i < BizActionObj.Details.PlannedNoOfEmb; i++)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddDay0OocList");

                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Details.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Details.PatientUnitID);
                    dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Details.PlanTherapyID);
                    dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, i + 1);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, BizActionObj.Details.OocyteDonorID);
                    dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Details.OocyteDonorUnitID);

                    dbServer.AddInParameter(command, "ID", DbType.Int64, 0);
                    int Status = dbServer.ExecuteNonQuery(command);

                    DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashBoard_AddDay0OocListInGhaphicalRepresentationTable");

                    dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Details.PatientID);
                    dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Details.PatientUnitID);
                    dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Details.PlanTherapyID);
                    dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command2, "Day0", DbType.Boolean, true);
                    dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, i + 1);
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                    int Status1 = dbServer.ExecuteNonQuery(command2);


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return BizActionObj;
        }
        public override IValueObject GetDay0OocList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetDay0OocyteListBizActionVO BizActionObj = valueObject as clsIVFDashboard_GetDay0OocyteListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay0OocList");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Details.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Details.PlanTherapyUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Day0OocList == null)
                        BizActionObj.Day0OocList = new List<clsIVFDashboard_LabDaysVO>();
                    while (reader.Read())
                    {
                        clsIVFDashboard_LabDaysVO Obj = new clsIVFDashboard_LabDaysVO();
                        Obj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Obj.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        Obj.PatientID = (long)DALHelper.HandleDBNull(reader["FemalePatientID"]);
                        Obj.PatientUnitID = (long)DALHelper.HandleDBNull(reader["FemalePatientUnitID"]);
                        Obj.PlanTherapyID = (long)DALHelper.HandleDBNull(reader["PlanTherapyID"]);
                        Obj.PlanTherapyUnitID = (long)DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]);
                        Obj.OocyteNumber = (long)DALHelper.HandleDBNull(reader["OocyteNumber"]);
                        Obj.SerialOocyteNumber = (long)DALHelper.HandleDBNull(reader["SerialOocuteNumber"]);
                        BizActionObj.Day0OocList.Add(Obj);
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

        public override IValueObject AddUpdateDay0Details(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_AddUpdateDay0BizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateDay0BizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay0");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber);
                dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, BizActionObj.Day0Details.OocyteNumber);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.Day0Details.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionObj.Day0Details.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.Day0Details.EmbryologistID);
                dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day0Details.AssitantEmbryologistID);
                dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, BizActionObj.Day0Details.AnesthetistID);
                dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day0Details.AssitantAnesthetistID);
                dbServer.AddInParameter(command, "CumulusID", DbType.Int64, BizActionObj.Day0Details.CumulusID);
                dbServer.AddInParameter(command, "MOIID", DbType.Int64, BizActionObj.Day0Details.MOIID);
                dbServer.AddInParameter(command, "GradeID", DbType.Int64, BizActionObj.Day0Details.GradeID);
                dbServer.AddInParameter(command, "CellStageID", DbType.Int64, BizActionObj.Day0Details.CellStageID);
                dbServer.AddInParameter(command, "OccDiamension", DbType.String, BizActionObj.Day0Details.OccDiamension);
                dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, BizActionObj.Day0Details.SpermPreperationMedia);
                dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, BizActionObj.Day0Details.OocytePreparationMedia);
                dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, BizActionObj.Day0Details.IncubatorID);
                dbServer.AddInParameter(command, "FinalLayering", DbType.String, BizActionObj.Day0Details.FinalLayering);
                dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, BizActionObj.Day0Details.NextPlanID);
                dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, BizActionObj.Day0Details.Isfreezed);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "Impression", DbType.String, BizActionObj.Day0Details.Impression); // by bHUSHAn
                dbServer.AddInParameter(command, "Comment", DbType.String, BizActionObj.Day0Details.Comment);
                dbServer.AddInParameter(command, "IC", DbType.String, BizActionObj.Day0Details.IC);
                dbServer.AddInParameter(command, "MBD", DbType.String, BizActionObj.Day0Details.MBD);
                dbServer.AddInParameter(command, "DOSID", DbType.Int64, BizActionObj.Day0Details.DOSID);
                dbServer.AddInParameter(command, "PICID", DbType.Int64, BizActionObj.Day0Details.PICID);
                dbServer.AddInParameter(command, "TreatmentID", DbType.Int64, BizActionObj.Day0Details.TreatmentID);

                //added by neena
                dbServer.AddInParameter(command, "TreatmentStartDate", DbType.DateTime, BizActionObj.Day0Details.TreatmentStartDate);
                dbServer.AddInParameter(command, "TreatmentEndDate", DbType.DateTime, BizActionObj.Day0Details.TreatmentEndDate);
                dbServer.AddInParameter(command, "ObservationDate", DbType.DateTime, BizActionObj.Day0Details.ObservationDate);
                dbServer.AddInParameter(command, "ObservationTime", DbType.DateTime, BizActionObj.Day0Details.ObservationTime);

                dbServer.AddInParameter(command, "OocyteMaturity", DbType.Int64, BizActionObj.Day0Details.OocyteMaturityID);
                dbServer.AddInParameter(command, "OocyteCytoplasmDysmorphisim", DbType.Int64, BizActionObj.Day0Details.OocyteCytoplasmDysmorphisim);
                dbServer.AddInParameter(command, "ExtracytoplasmicDysmorphisim", DbType.Int64, BizActionObj.Day0Details.ExtracytoplasmicDysmorphisim);
                dbServer.AddInParameter(command, "OocyteCoronaCumulusComplex", DbType.Int64, BizActionObj.Day0Details.OocyteCoronaCumulusComplex);
                dbServer.AddInParameter(command, "ProcedureDate", DbType.DateTime, BizActionObj.Day0Details.ProcedureDate);
                dbServer.AddInParameter(command, "ProcedureTime", DbType.DateTime, BizActionObj.Day0Details.ProcedureTime);
                dbServer.AddInParameter(command, "SourceOfSperm", DbType.Int64, BizActionObj.Day0Details.SourceOfSperm);
                dbServer.AddInParameter(command, "SpermCollectionMethod", DbType.Int64, BizActionObj.Day0Details.SpermCollectionMethod);
                dbServer.AddInParameter(command, "IMSI", DbType.Boolean, BizActionObj.Day0Details.IMSI);
                dbServer.AddInParameter(command, "Embryoscope", DbType.Boolean, BizActionObj.Day0Details.Embryoscope);
                dbServer.AddInParameter(command, "DiscardReason", DbType.String, BizActionObj.Day0Details.DiscardReason);
                dbServer.AddInParameter(command, "DonorCode", DbType.String, BizActionObj.Day0Details.DonorCode);
                dbServer.AddInParameter(command, "SampleNo", DbType.String, BizActionObj.Day0Details.SampleNo);

                dbServer.AddInParameter(command, "IsDonate", DbType.Boolean, BizActionObj.Day0Details.IsDonate);
                dbServer.AddInParameter(command, "IsDonateCryo", DbType.Boolean, BizActionObj.Day0Details.IsDonateCryo);
                dbServer.AddInParameter(command, "RecepientPatientID", DbType.Int64, BizActionObj.Day0Details.RecepientPatientID);
                dbServer.AddInParameter(command, "RecepientPatientUnitID", DbType.Int64, BizActionObj.Day0Details.RecepientPatientUnitID);
                dbServer.AddInParameter(command, "SemenSample", DbType.String, BizActionObj.Day0Details.SemenSample);

                //added by neena for donate cycle
                dbServer.AddInParameter(command, "IsDonorCycleDonate", DbType.Boolean, BizActionObj.Day0Details.IsDonorCycleDonate);
                dbServer.AddInParameter(command, "IsDonorCycleDonateCryo", DbType.Boolean, BizActionObj.Day0Details.IsDonorCycleDonateCryo);


                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day0Details.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);


                dbServer.AddInParameter(command, "OocyteZonaID", DbType.Int64, BizActionObj.Day0Details.OocyteZonaID);
                dbServer.AddInParameter(command, "OocyteZona", DbType.String, BizActionObj.Day0Details.OocyteZona);
                dbServer.AddInParameter(command, "PVSID", DbType.Int64, BizActionObj.Day0Details.PVSID);
                dbServer.AddInParameter(command, "PVS", DbType.String, BizActionObj.Day0Details.PVS);
                dbServer.AddInParameter(command, "IstPBID", DbType.Int64, BizActionObj.Day0Details.IstPBID);
                dbServer.AddInParameter(command, "IstPB", DbType.String, BizActionObj.Day0Details.IstPB);
                dbServer.AddInParameter(command, "CytoplasmID", DbType.Int64, BizActionObj.Day0Details.CytoplasmID);
                dbServer.AddInParameter(command, "Cytoplasm", DbType.String, BizActionObj.Day0Details.Cytoplasm);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Day0Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                //if (BizActionObj.Day0Details.Photo != null)
                if (BizActionObj.Day0Details.ImgList != null && BizActionObj.Day0Details.ImgList.Count > 0)
                {
                    int cnt = 0;
                    foreach (var item in BizActionObj.Day0Details.ImgList)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                        dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                        dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                        dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                        dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber);
                        dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day0Details.OocyteNumber);
                        dbServer.AddInParameter(command1, "Day", DbType.Int64, 0);
                        dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day0Details.ID);
                        dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day0Details.CellStageID);
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

                if (BizActionObj.Day0Details.NextPlanID == 3 && BizActionObj.Day0Details.Isfreezed == true)
                {
                    DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day1=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day0Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day0Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day0Details.SerialOocyteNumber);
                    int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);

                    DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay1");
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                    dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                    dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                    dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, BizActionObj.Day0Details.OocyteNumber);
                    dbServer.AddInParameter(command2, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command2, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AnesthetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AssitantAnesthetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "CumulusID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "MOIID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "GradeID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "OccDiamension", DbType.String, null);
                    dbServer.AddInParameter(command2, "SpermPreperationMedia", DbType.String, null);
                    dbServer.AddInParameter(command2, "OocytePreparationMedia", DbType.String, null);
                    dbServer.AddInParameter(command2, "IncubatorID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "FinalLayering", DbType.String, null);
                    dbServer.AddInParameter(command2, "NextPlanID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "Isfreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                    dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorID);
                    dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorUnitID);

                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                }

                if (BizActionObj.Day0Details.NextPlanID == 4 && BizActionObj.Day0Details.Isfreezed == true)
                {
                    //Add in ET table
                    DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                    BizActionObj.Day0Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                    DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                    dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day0Details.ID);
                    dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, BizActionObj.Day0Details.OocyteNumber);
                    dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day0Details.Date);
                    dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day0");
                    dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day0Details.GradeID);
                    dbServer.AddInParameter(command6, "Score", DbType.String, null);
                    dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day0Details.CellStageID);
                    dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                    dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                    dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                    dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorID);
                    dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorUnitID);

                    int iStatus = dbServer.ExecuteNonQuery(command6, trans);

                }
                if (BizActionObj.Day0Details.NextPlanID == 2 && BizActionObj.Day0Details.Isfreezed == true)
                {
                    ////added by neena for oocyte freezing
                    //DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateForOocyteVitrification");
                    //dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //dbServer.AddInParameter(command8, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                    //dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                    //dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                    //dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                    //dbServer.AddInParameter(command8, "DateTime", DbType.DateTime, null);
                    //dbServer.AddInParameter(command8, "VitrificationNo", DbType.String, null);
                    //dbServer.AddInParameter(command8, "PickUpDate", DbType.DateTime, null);
                    //dbServer.AddInParameter(command8, "ConsentForm", DbType.Boolean, null);
                    //dbServer.AddInParameter(command8, "IsFreezed", DbType.Boolean, null);
                    //dbServer.AddInParameter(command8, "IsOnlyVitrification", DbType.Boolean, false);
                    //dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                    //dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    //dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    //dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    //dbServer.AddInParameter(command8, "SrcOoctyID", DbType.Int64, null);
                    //dbServer.AddInParameter(command8, "SrcSemenID", DbType.Int64, null);
                    //dbServer.AddInParameter(command8, "SrcOoctyCode", DbType.String, null);
                    //dbServer.AddInParameter(command8, "SrcSemenCode", DbType.String, null);
                    //dbServer.AddInParameter(command8, "UsedOwnOocyte", DbType.Boolean, null);

                    //dbServer.AddInParameter(command8, "EmbryologistID", DbType.Int64, null);
                    //dbServer.AddInParameter(command8, "AssitantEmbryologistID", DbType.Int64, null);

                    //dbServer.AddInParameter(command8, "FromForm", DbType.Int64, 1);
                    //dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                    //int intStatus4 = dbServer.ExecuteNonQuery(command8, trans);
                    //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                    //BizActionObj.Day0Details.ID = (long)dbServer.GetParameterValue(command8, "ID");

                    ////Add in T_IVFDashboard_VitrificationDetailsForOocyte  table

                    //DbCommand command9 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateForOocyteVitrificationDetails");

                    //dbServer.AddInParameter(command9, "ID", DbType.Int64, 0);
                    //dbServer.AddInParameter(command9, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //dbServer.AddInParameter(command9, "VitrivicationID", DbType.Int64, BizActionObj.Day0Details.ID);
                    //dbServer.AddInParameter(command9, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //dbServer.AddInParameter(command9, "OocyteNumber", DbType.Int64, BizActionObj.Day0Details.OocyteNumber);
                    //dbServer.AddInParameter(command9, "OocyteSerialNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber);
                    //dbServer.AddInParameter(command9, "LeafNo", DbType.String, null);
                    //dbServer.AddInParameter(command9, "OocyteDays", DbType.String, null);
                    //dbServer.AddInParameter(command9, "ColorCodeID", DbType.Int64, null);
                    //dbServer.AddInParameter(command9, "CanId", DbType.Int64, null);
                    //dbServer.AddInParameter(command9, "StrawId", DbType.Int64, null);
                    //dbServer.AddInParameter(command9, "GobletShapeId", DbType.Int64, null);
                    //dbServer.AddInParameter(command9, "GobletSizeId", DbType.Int64, null);
                    //dbServer.AddInParameter(command9, "TankId", DbType.Int64, null);
                    //dbServer.AddInParameter(command9, "ConistorNo", DbType.Int64, null);
                    //dbServer.AddInParameter(command9, "ProtocolTypeID", DbType.Int64, null);
                    //dbServer.AddInParameter(command9, "TransferDate", DbType.DateTime, BizActionObj.Day0Details.Date);
                    //dbServer.AddInParameter(command9, "TransferDay", DbType.String, "Day0");
                    //dbServer.AddInParameter(command9, "CellStageID", DbType.String, BizActionObj.Day0Details.CellStageID);
                    //dbServer.AddInParameter(command9, "GradeID", DbType.Int64, BizActionObj.Day0Details.OocyteMaturityID);  //Oocyte Maturity
                    //dbServer.AddInParameter(command9, "OocyteStatus", DbType.String, null);
                    //dbServer.AddInParameter(command9, "Comments", DbType.String, null);
                    //dbServer.AddInParameter(command9, "UsedOwnOocyte", DbType.Boolean, null);
                    //dbServer.AddInParameter(command9, "IsThawingDone", DbType.Boolean, false);

                    //dbServer.AddInParameter(command9, "OocyteDonorID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorID);
                    //dbServer.AddInParameter(command9, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorUnitID);

                    //dbServer.AddInParameter(command9, "UsedByOtherCycle", DbType.Boolean, false);
                    //dbServer.AddInParameter(command9, "UsedTherapyID", DbType.Int64, null);
                    //dbServer.AddInParameter(command9, "UsedTherapyUnitID", DbType.Int64, null);
                    //dbServer.AddInParameter(command9, "ReceivingDate", DbType.DateTime, null);

                    //dbServer.AddInParameter(command9, "TransferDayNo", DbType.Int64, 0); //added by neena

                    //int iStatus = dbServer.ExecuteNonQuery(command9, trans);
                    ////

                    //Add in Vitrification table
                    DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                    dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                    dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                    dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                    dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                    dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    dbServer.AddInParameter(command4, "IsFreezeOocytes", DbType.Boolean, BizActionObj.Day0Details.IsFreezeOocytes); // added for oocyte 

                    int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    BizActionObj.Day0Details.ID = (long)dbServer.GetParameterValue(command4, "ID");

                    DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                    dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day0Details.ID);
                    dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, BizActionObj.Day0Details.OocyteNumber);
                    dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                    dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                    dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day0Details.Date);
                    dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day0");
                    dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day0Details.CellStageID);
                    //dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day0Details.GradeID);
                    dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day0Details.OocyteMaturityID);
                    dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                    dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                    dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                    dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorID);
                    dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorUnitID);

                    dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                    dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                    dbServer.AddInParameter(command5, "TransferDayNo", DbType.Int64, 0); //added by neena
                    dbServer.AddInParameter(command5, "IsFreezeOocytes", DbType.Boolean, BizActionObj.Day0Details.IsFreezeOocytes);  //added by neena

                    int iStatus = dbServer.ExecuteNonQuery(command5, trans);


                }

                //added by neena for donate cryo plan
                if (BizActionObj.Day0Details.NextPlanID == 9 && BizActionObj.Day0Details.Isfreezed == true)
                {
                    //Add in Vitrification table
                    DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                    dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                    dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                    dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                    dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                    dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddInParameter(command4, "IsFreezeOocytes", DbType.Boolean, BizActionObj.Day0Details.IsFreezeOocytes); // added for oocyte 

                    int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    BizActionObj.Day0Details.ID = (long)dbServer.GetParameterValue(command4, "ID");

                    DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                    dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day0Details.ID);
                    dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, BizActionObj.Day0Details.OocyteNumber);
                    dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                    dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                    dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day0Details.Date);
                    dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day0");
                    dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day0Details.CellStageID);
                    //dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day0Details.GradeID);
                    dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day0Details.OocyteMaturityID);
                    dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                    dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                    dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                    dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorID);
                    dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorUnitID);

                    dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                    dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                    dbServer.AddInParameter(command5, "TransferDayNo", DbType.Int64, 0); //added by neena
                    dbServer.AddInParameter(command5, "IsFreezeOocytes", DbType.Boolean, BizActionObj.Day0Details.IsFreezeOocytes);  //added by neena

                    //added for donate cryo plan
                    dbServer.AddInParameter(command5, "IsDonateCryo", DbType.Boolean, BizActionObj.Day0Details.IsDonateCryo);
                    dbServer.AddInParameter(command5, "RecepientPatientID", DbType.Int64, BizActionObj.Day0Details.RecepientPatientID);
                    dbServer.AddInParameter(command5, "RecepientPatientUnitID", DbType.Int64, BizActionObj.Day0Details.RecepientPatientUnitID);
                    //for donar cycle donate cryo
                    dbServer.AddInParameter(command5, "IsDonorCycleDonateCryo", DbType.Boolean, BizActionObj.Day0Details.IsDonorCycleDonateCryo);
                    dbServer.AddInParameter(command5, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                    dbServer.AddInParameter(command5, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);

                    int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                }
                //


                //added by neena dated 18/5/16
                if (BizActionObj.Day0Details.OcyteListList != null)
                {
                    foreach (var item in BizActionObj.Day0Details.OcyteListList)
                    {
                        DbCommand command7 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay0");

                        dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command7, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                        dbServer.AddInParameter(command7, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                        dbServer.AddInParameter(command7, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                        dbServer.AddInParameter(command7, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                        dbServer.AddInParameter(command7, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber + item.FilterID);
                        dbServer.AddInParameter(command7, "OocyteNumber", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command7, "Date", DbType.DateTime, BizActionObj.Day0Details.Date);
                        dbServer.AddInParameter(command7, "Time", DbType.DateTime, BizActionObj.Day0Details.Time);
                        dbServer.AddInParameter(command7, "EmbryologistID", DbType.Int64, BizActionObj.Day0Details.EmbryologistID);
                        dbServer.AddInParameter(command7, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day0Details.AssitantEmbryologistID);
                        dbServer.AddInParameter(command7, "AnesthetistID", DbType.Int64, BizActionObj.Day0Details.AnesthetistID);
                        dbServer.AddInParameter(command7, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day0Details.AssitantAnesthetistID);
                        dbServer.AddInParameter(command7, "CumulusID", DbType.Int64, BizActionObj.Day0Details.CumulusID);
                        dbServer.AddInParameter(command7, "MOIID", DbType.Int64, BizActionObj.Day0Details.MOIID);
                        dbServer.AddInParameter(command7, "GradeID", DbType.Int64, BizActionObj.Day0Details.GradeID);
                        dbServer.AddInParameter(command7, "CellStageID", DbType.Int64, BizActionObj.Day0Details.CellStageID);
                        dbServer.AddInParameter(command7, "OccDiamension", DbType.String, BizActionObj.Day0Details.OccDiamension);
                        dbServer.AddInParameter(command7, "SpermPreperationMedia", DbType.String, BizActionObj.Day0Details.SpermPreperationMedia);
                        dbServer.AddInParameter(command7, "OocytePreparationMedia", DbType.String, BizActionObj.Day0Details.OocytePreparationMedia);
                        dbServer.AddInParameter(command7, "IncubatorID", DbType.Int64, BizActionObj.Day0Details.IncubatorID);
                        dbServer.AddInParameter(command7, "FinalLayering", DbType.String, BizActionObj.Day0Details.FinalLayering);
                        dbServer.AddInParameter(command7, "NextPlanID", DbType.Int64, BizActionObj.Day0Details.NextPlanID);
                        dbServer.AddInParameter(command7, "Isfreezed", DbType.Boolean, BizActionObj.Day0Details.Isfreezed);
                        dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command7, "Impression", DbType.String, BizActionObj.Day0Details.Impression); // by bHUSHAn
                        dbServer.AddInParameter(command7, "Comment", DbType.String, BizActionObj.Day0Details.Comment);
                        dbServer.AddInParameter(command7, "IC", DbType.String, BizActionObj.Day0Details.IC);
                        dbServer.AddInParameter(command7, "MBD", DbType.String, BizActionObj.Day0Details.MBD);
                        dbServer.AddInParameter(command7, "DOSID", DbType.Int64, BizActionObj.Day0Details.DOSID);
                        dbServer.AddInParameter(command7, "PICID", DbType.Int64, BizActionObj.Day0Details.PICID);
                        dbServer.AddInParameter(command7, "TreatmentID", DbType.Int64, BizActionObj.Day0Details.TreatmentID);

                        //added by neena
                        dbServer.AddInParameter(command7, "TreatmentStartDate", DbType.DateTime, BizActionObj.Day0Details.TreatmentStartDate);
                        dbServer.AddInParameter(command7, "TreatmentEndDate", DbType.DateTime, BizActionObj.Day0Details.TreatmentEndDate);
                        dbServer.AddInParameter(command7, "ObservationDate", DbType.DateTime, BizActionObj.Day0Details.ObservationDate);
                        dbServer.AddInParameter(command7, "ObservationTime", DbType.DateTime, BizActionObj.Day0Details.ObservationTime);

                        dbServer.AddInParameter(command7, "OocyteMaturity", DbType.Int64, BizActionObj.Day0Details.OocyteMaturityID);
                        dbServer.AddInParameter(command7, "OocyteCytoplasmDysmorphisim", DbType.Int64, BizActionObj.Day0Details.OocyteCytoplasmDysmorphisim);
                        dbServer.AddInParameter(command7, "ExtracytoplasmicDysmorphisim", DbType.Int64, BizActionObj.Day0Details.ExtracytoplasmicDysmorphisim);
                        dbServer.AddInParameter(command7, "OocyteCoronaCumulusComplex", DbType.Int64, BizActionObj.Day0Details.OocyteCoronaCumulusComplex);
                        dbServer.AddInParameter(command7, "ProcedureDate", DbType.DateTime, BizActionObj.Day0Details.ProcedureDate);
                        dbServer.AddInParameter(command7, "ProcedureTime", DbType.DateTime, BizActionObj.Day0Details.ProcedureTime);
                        dbServer.AddInParameter(command7, "SourceOfSperm", DbType.Int64, BizActionObj.Day0Details.SourceOfSperm);
                        dbServer.AddInParameter(command7, "SpermCollectionMethod", DbType.Int64, BizActionObj.Day0Details.SpermCollectionMethod);
                        dbServer.AddInParameter(command7, "IMSI", DbType.Boolean, BizActionObj.Day0Details.IMSI);
                        dbServer.AddInParameter(command7, "Embryoscope", DbType.Boolean, BizActionObj.Day0Details.Embryoscope);
                        dbServer.AddInParameter(command7, "DiscardReason", DbType.String, BizActionObj.Day0Details.DiscardReason);
                        dbServer.AddInParameter(command7, "DonorCode", DbType.String, BizActionObj.Day0Details.DonorCode);
                        dbServer.AddInParameter(command7, "SampleNo", DbType.String, BizActionObj.Day0Details.SampleNo);

                        dbServer.AddInParameter(command7, "IsDonate", DbType.Boolean, BizActionObj.Day0Details.IsDonate);
                        dbServer.AddInParameter(command7, "IsDonateCryo", DbType.Boolean, BizActionObj.Day0Details.IsDonateCryo);
                        dbServer.AddInParameter(command7, "RecepientPatientID", DbType.Int64, BizActionObj.Day0Details.RecepientPatientID);
                        dbServer.AddInParameter(command7, "RecepientPatientUnitID", DbType.Int64, BizActionObj.Day0Details.RecepientPatientUnitID);
                        dbServer.AddInParameter(command7, "SemenSample", DbType.String, BizActionObj.Day0Details.SemenSample);

                        //added by neena for donate cycle
                        dbServer.AddInParameter(command7, "IsDonorCycleDonate", DbType.Boolean, BizActionObj.Day0Details.IsDonorCycleDonate);
                        dbServer.AddInParameter(command7, "IsDonorCycleDonateCryo", DbType.Boolean, BizActionObj.Day0Details.IsDonorCycleDonateCryo);

                        dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day0Details.ID);
                        dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, int.MaxValue);

                        dbServer.AddInParameter(command7, "OocyteZonaID", DbType.Int64, BizActionObj.Day0Details.OocyteZonaID);
                        dbServer.AddInParameter(command7, "OocyteZona", DbType.String, BizActionObj.Day0Details.OocyteZona);
                        dbServer.AddInParameter(command7, "PVSID", DbType.Int64, BizActionObj.Day0Details.PVSID);
                        dbServer.AddInParameter(command7, "PVS", DbType.String, BizActionObj.Day0Details.PVS);
                        dbServer.AddInParameter(command7, "IstPBID", DbType.Int64, BizActionObj.Day0Details.IstPBID);
                        dbServer.AddInParameter(command7, "IstPB", DbType.String, BizActionObj.Day0Details.IstPB);
                        dbServer.AddInParameter(command7, "CytoplasmID", DbType.Int64, BizActionObj.Day0Details.CytoplasmID);
                        dbServer.AddInParameter(command7, "Cytoplasm", DbType.String, BizActionObj.Day0Details.Cytoplasm);

                        int intStatus7 = dbServer.ExecuteNonQuery(command7, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command7, "ResultStatus");
                        BizActionObj.Day0Details.ID = (long)dbServer.GetParameterValue(command7, "ID");

                        //if (BizActionObj.Day0Details.Photo != null)
                        if (BizActionObj.Day0Details.ImgList != null && BizActionObj.Day0Details.ImgList.Count > 0)
                        {
                            foreach (var item1 in BizActionObj.Day0Details.ImgList)
                            {
                                DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                                dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                                dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                                dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                                dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber + item.FilterID);
                                dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                                dbServer.AddInParameter(command1, "Day", DbType.Int64, 0);
                                dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day0Details.ID);
                                dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day0Details.CellStageID);
                                //dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day0Details.Photo);
                                //dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day0Details.FileName);
                                //dbServer.AddInParameter(command1, "Photo", DbType.Binary, item.Photo);
                                dbServer.AddInParameter(command1, "FileName", DbType.String, item1.ImagePath);
                                //dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                                dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                                //added by neena
                                //dbServer.AddInParameter(command1, "SeqNo", DbType.String, item1.SeqNo);
                                dbServer.AddParameter(command1, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.SeqNo);
                                dbServer.AddInParameter(command1, "IsApplyTo", DbType.Int32, 1);
                                dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ServerImageName);

                                //dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);
                                //

                                int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                                item1.ID = Convert.ToInt64(dbServer.GetParameterValue(command1, "ID"));
                                item1.ServerImageName = Convert.ToString(dbServer.GetParameterValue(command1, "ServerImageName"));
                                item1.SeqNo = Convert.ToString(dbServer.GetParameterValue(command1, "SeqNo"));

                                //if (item1.Photo != null)
                                //    File.WriteAllBytes(ImgSaveLocation + item1.ServerImageName, item1.Photo);
                            }
                        }

                        //if (BizActionObj.Day0Details.Photo != null)
                        //{
                        //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                        //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                        //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                        //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                        //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber + item.FilterID);
                        //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                        //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 0);
                        //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day0Details.ID);
                        //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day0Details.CellStageID);
                        //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day0Details.Photo);
                        //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day0Details.FileName);
                        //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                        //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                        //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        //}


                        if (BizActionObj.Day0Details.NextPlanID == 3 && BizActionObj.Day0Details.Isfreezed == true)
                        {
                            DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day1=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day0Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day0Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + (BizActionObj.Day0Details.SerialOocyteNumber + item.FilterID));
                            int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);

                            DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay1");
                            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                            dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                            dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                            dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command2, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command2, "Time", DbType.DateTime, null);
                            dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AssitantEmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AnesthetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AssitantAnesthetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "CumulusID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "MOIID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "GradeID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "OccDiamension", DbType.String, null);
                            dbServer.AddInParameter(command2, "SpermPreperationMedia", DbType.String, null);
                            dbServer.AddInParameter(command2, "OocytePreparationMedia", DbType.String, null);
                            dbServer.AddInParameter(command2, "IncubatorID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "FinalLayering", DbType.String, null);
                            dbServer.AddInParameter(command2, "NextPlanID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "Isfreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                            dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorID);
                            dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorUnitID);

                            int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                        }

                        if (BizActionObj.Day0Details.NextPlanID == 4 && BizActionObj.Day0Details.Isfreezed == true)
                        {
                            //Add in ET table
                            DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                            dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                            dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                            dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                            BizActionObj.Day0Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                            DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                            dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day0Details.ID);
                            dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day0Details.Date);
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day0");
                            dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day0Details.GradeID);
                            dbServer.AddInParameter(command6, "Score", DbType.String, null);
                            dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day0Details.CellStageID);
                            dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                            dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                            dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                            dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                            dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorID);
                            dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorUnitID);

                            int iStatus = dbServer.ExecuteNonQuery(command6, trans);

                        }
                        if (BizActionObj.Day0Details.NextPlanID == 2 && BizActionObj.Day0Details.Isfreezed == true)
                        {
                            ////Add in T_IVFDashboard_VitrificationForOocyte table
                            //DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateForOocyteVitrification");  //OLD  IVFDashboard_AddUpdateVitrification
                            //dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //dbServer.AddInParameter(command8, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                            //dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                            //dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                            //dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                            //dbServer.AddInParameter(command8, "DateTime", DbType.DateTime, null);
                            //dbServer.AddInParameter(command8, "VitrificationNo", DbType.String, null);
                            //dbServer.AddInParameter(command8, "PickUpDate", DbType.DateTime, null);
                            //dbServer.AddInParameter(command8, "ConsentForm", DbType.Boolean, null);
                            //dbServer.AddInParameter(command8, "IsFreezed", DbType.Boolean, null);
                            //dbServer.AddInParameter(command8, "IsOnlyVitrification", DbType.Boolean, false);
                            //dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                            //dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            //dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            //dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            //dbServer.AddInParameter(command8, "SrcOoctyID", DbType.Int64, null);
                            //dbServer.AddInParameter(command8, "SrcSemenID", DbType.Int64, null);
                            //dbServer.AddInParameter(command8, "SrcOoctyCode", DbType.String, null);
                            //dbServer.AddInParameter(command8, "SrcSemenCode", DbType.String, null);
                            //dbServer.AddInParameter(command8, "UsedOwnOocyte", DbType.Boolean, null);

                            //dbServer.AddInParameter(command8, "EmbryologistID", DbType.Int64, null);
                            //dbServer.AddInParameter(command8, "AssitantEmbryologistID", DbType.Int64, null);

                            //dbServer.AddInParameter(command8, "FromForm", DbType.Int64, 1);
                            //dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                            //int intStatus4 = dbServer.ExecuteNonQuery(command8, trans);
                            //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                            //BizActionObj.Day0Details.ID = (long)dbServer.GetParameterValue(command8, "ID");

                            ////Add in T_IVFDashboard_VitrificationDetailsForOocyte table
                            //DbCommand command9 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateForOocyteVitrificationDetails");   //OLD  IVFDashboard_AddUpdateVitrificationDetails

                            //dbServer.AddInParameter(command9, "ID", DbType.Int64, 0);
                            //dbServer.AddInParameter(command9, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //dbServer.AddInParameter(command9, "VitrivicationID", DbType.Int64, BizActionObj.Day0Details.ID);
                            //dbServer.AddInParameter(command9, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //dbServer.AddInParameter(command9, "OocyteNumber", DbType.Int64, item.ID);
                            //dbServer.AddInParameter(command9, "OocyteSerialNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber + item.FilterID);
                            //dbServer.AddInParameter(command9, "LeafNo", DbType.String, null);
                            //dbServer.AddInParameter(command9, "OocyteDays", DbType.String, null);
                            //dbServer.AddInParameter(command9, "ColorCodeID", DbType.Int64, null);
                            //dbServer.AddInParameter(command9, "CanId", DbType.Int64, null);
                            //dbServer.AddInParameter(command9, "StrawId", DbType.Int64, null);
                            //dbServer.AddInParameter(command9, "GobletShapeId", DbType.Int64, null);
                            //dbServer.AddInParameter(command9, "GobletSizeId", DbType.Int64, null);
                            //dbServer.AddInParameter(command9, "TankId", DbType.Int64, null);
                            //dbServer.AddInParameter(command9, "ConistorNo", DbType.Int64, null);
                            //dbServer.AddInParameter(command9, "ProtocolTypeID", DbType.Int64, null);
                            //dbServer.AddInParameter(command9, "TransferDate", DbType.DateTime, BizActionObj.Day0Details.Date);
                            //dbServer.AddInParameter(command9, "TransferDay", DbType.String, "Day0");
                            //dbServer.AddInParameter(command9, "CellStageID", DbType.String, BizActionObj.Day0Details.CellStageID);
                            //dbServer.AddInParameter(command9, "GradeID", DbType.Int64, BizActionObj.Day0Details.OocyteMaturityID);
                            //dbServer.AddInParameter(command9, "OocyteStatus", DbType.String, null);
                            //dbServer.AddInParameter(command9, "Comments", DbType.String, null);
                            //dbServer.AddInParameter(command9, "UsedOwnOocyte", DbType.Boolean, null);
                            //dbServer.AddInParameter(command9, "IsThawingDone", DbType.Boolean, false);

                            //dbServer.AddInParameter(command9, "OocyteDonorID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorID);
                            //dbServer.AddInParameter(command9, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorUnitID);

                            //dbServer.AddInParameter(command9, "UsedByOtherCycle", DbType.Boolean, false);
                            //dbServer.AddInParameter(command9, "UsedTherapyID", DbType.Int64, null);
                            //dbServer.AddInParameter(command9, "UsedTherapyUnitID", DbType.Int64, null);
                            //dbServer.AddInParameter(command9, "ReceivingDate", DbType.DateTime, null);

                            //dbServer.AddInParameter(command9, "TransferDayNo", DbType.Int64, 0); //added by neena

                            //int iStatus = dbServer.ExecuteNonQuery(command9, trans);
                            ////


                            //Add in Vitrification table
                            DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                            dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                            dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                            dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                            dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                            dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                            dbServer.AddInParameter(command4, "IsFreezeOocytes", DbType.Boolean, BizActionObj.Day0Details.IsFreezeOocytes); // added for oocyte 

                            int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                            BizActionObj.Day0Details.ID = (long)dbServer.GetParameterValue(command4, "ID");

                            DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                            dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day0Details.ID);
                            dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                            dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                            dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day0Details.Date);
                            dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day0");
                            dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day0Details.CellStageID);
                            //dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day0Details.GradeID);
                            dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day0Details.OocyteMaturityID);
                            dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                            dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                            dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                            dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorID);
                            dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorUnitID);

                            dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                            dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                            dbServer.AddInParameter(command5, "TransferDayNo", DbType.Int64, 0); //added by neena
                            dbServer.AddInParameter(command5, "IsFreezeOocytes", DbType.Boolean, BizActionObj.Day0Details.IsFreezeOocytes);  //added by neena

                            int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                        }

                        //added by neena for donate cryo plan
                        if (BizActionObj.Day0Details.NextPlanID == 9 && BizActionObj.Day0Details.Isfreezed == true)
                        {
                            //Add in Vitrification table
                            DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");
                            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                            dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                            dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyID);
                            dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day0Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                            dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                            dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                            dbServer.AddInParameter(command4, "IsFreezeOocytes", DbType.Boolean, BizActionObj.Day0Details.IsFreezeOocytes); // added for oocyte 

                            int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                            BizActionObj.Day0Details.ID = (long)dbServer.GetParameterValue(command4, "ID");

                            DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                            dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day0Details.ID);
                            dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day0Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                            dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                            dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day0Details.Date);
                            dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day0");
                            dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day0Details.CellStageID);
                            //dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day0Details.GradeID);
                            dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day0Details.OocyteMaturityID);
                            dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                            dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                            dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                            dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorID);
                            dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day0Details.OocyteDonorUnitID);

                            dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                            dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                            dbServer.AddInParameter(command5, "TransferDayNo", DbType.Int64, 0); //added by neena
                            dbServer.AddInParameter(command5, "IsFreezeOocytes", DbType.Boolean, BizActionObj.Day0Details.IsFreezeOocytes);  //added by neena

                            //added for donate cryo plan
                            dbServer.AddInParameter(command5, "IsDonateCryo", DbType.Boolean, BizActionObj.Day0Details.IsDonateCryo);
                            dbServer.AddInParameter(command5, "RecepientPatientID", DbType.Int64, BizActionObj.Day0Details.RecepientPatientID);
                            dbServer.AddInParameter(command5, "RecepientPatientUnitID", DbType.Int64, BizActionObj.Day0Details.RecepientPatientUnitID);

                            //for donar cycle donate cryo
                            dbServer.AddInParameter(command5, "IsDonorCycleDonateCryo", DbType.Boolean, BizActionObj.Day0Details.IsDonorCycleDonateCryo);
                            dbServer.AddInParameter(command5, "PatientID", DbType.Int64, BizActionObj.Day0Details.PatientID);
                            dbServer.AddInParameter(command5, "PatientUnitID", DbType.Int64, BizActionObj.Day0Details.PatientUnitID);
                            int iStatus = dbServer.ExecuteNonQuery(command5, trans);

                        }
                        //

                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Day0Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }
        public override IValueObject GetDay0Details(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay0DetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay0DetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay0Details");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        BizAction.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        BizAction.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        BizAction.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        BizAction.Details.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                        BizAction.Details.Time = (DateTime?)(DALHelper.HandleDate(reader["Time"]));
                        BizAction.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        BizAction.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        BizAction.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        BizAction.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        BizAction.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        BizAction.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        BizAction.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        BizAction.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        BizAction.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        BizAction.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        BizAction.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        BizAction.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        BizAction.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OoctyePreparationMedia"]));
                        BizAction.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        BizAction.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        //BizAction.Details.Photo = (byte[])(DALHelper.HandleDBNull(reader["Image"]));
                        //BizAction.Details.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        BizAction.Details.DOSID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DOSID"]));
                        BizAction.Details.PICID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PICID"]));
                        BizAction.Details.MBD = Convert.ToString(DALHelper.HandleDBNull(reader["MBD"]));
                        BizAction.Details.IC = Convert.ToString(DALHelper.HandleDBNull(reader["IC"]));
                        BizAction.Details.Comment = Convert.ToString(DALHelper.HandleDBNull(reader["Comment"]));
                        BizAction.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));// BY BHUSHAN
                        BizAction.Details.TreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TreatmentID"]));

                        BizAction.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        BizAction.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));

                        //by neena
                        BizAction.Details.TreatmentStartDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentStartDate"]));
                        BizAction.Details.TreatmentEndDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentEndDate"]));
                        BizAction.Details.ObservationDate = (DateTime?)(DALHelper.HandleDate(reader["ObservationDate"]));
                        BizAction.Details.ObservationTime = (DateTime?)(DALHelper.HandleDate(reader["ObservationTime"]));

                        BizAction.Details.OocyteMaturityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteMaturity"]));
                        BizAction.Details.OocyteCytoplasmDysmorphisim = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCytoplasmDysmorphisim"]));
                        BizAction.Details.ExtracytoplasmicDysmorphisim = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtracytoplasmicDysmorphisim"]));
                        BizAction.Details.OocyteCoronaCumulusComplex = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCoronaCumulusComplex"]));
                        BizAction.Details.OocyteCoronaCumulusComplex = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteCoronaCumulusComplex"]));
                        BizAction.Details.ProcedureDate = (DateTime?)(DALHelper.HandleDate(reader["ProcedureDate"]));
                        BizAction.Details.ProcedureTime = (DateTime?)(DALHelper.HandleDate(reader["ProcedureTime"]));
                        BizAction.Details.SourceOfSperm = Convert.ToInt64(DALHelper.HandleDBNull(reader["SourceOfSperm"]));
                        BizAction.Details.SpermCollectionMethod = Convert.ToInt64(DALHelper.HandleDBNull(reader["SpermCollectionMethod"]));
                        BizAction.Details.IMSI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IMSI"]));
                        BizAction.Details.Embryoscope = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Embryoscope"]));
                        BizAction.Details.DiscardReason = Convert.ToString(DALHelper.HandleDBNull(reader["DiscardReason"]));
                        BizAction.Details.DonorCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonorCode"]));
                        BizAction.Details.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));
                        BizAction.Details.RecepientPatientName = Convert.ToString(DALHelper.HandleDBNull(reader["RecepientPatientName"]));
                        BizAction.Details.RecepientMrNO = Convert.ToString(DALHelper.HandleDBNull(reader["RecepientMrNO"]));
                        BizAction.Details.SemenSample = Convert.ToString(DALHelper.HandleDBNull(reader["SemenSample"]));
                        BizAction.Details.OocyteZonaID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteZonaID"]));
                        BizAction.Details.OocyteZona = Convert.ToString(DALHelper.HandleDBNull(reader["OocyteZona"]));
                        BizAction.Details.PVSID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PVSID"]));
                        BizAction.Details.PVS = Convert.ToString(DALHelper.HandleDBNull(reader["PVS"]));
                        BizAction.Details.IstPBID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IstPBID"]));
                        BizAction.Details.IstPB = Convert.ToString(DALHelper.HandleDBNull(reader["IstPB"]));
                        BizAction.Details.CytoplasmID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CytoplasmID"]));
                        BizAction.Details.Cytoplasm = Convert.ToString(DALHelper.HandleDBNull(reader["Cytoplasm"]));
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

                        if (!string.IsNullOrEmpty(ObjImg.OriginalImagePath))
                            ObjImg.ServerImageName = "..//" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;
                        //ObjImg.ServerImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;


                        BizAction.Details.ImgList.Add(ObjImg);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        //added by neena

        public override IValueObject GetDate(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay0DetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay0DetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay0Date");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.Details.ProcedureDate = (DateTime?)(DALHelper.HandleDate(reader["ProcedureDate"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        public override IValueObject GetSemenSampleList(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay0DetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay0DetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSemenSampleListForDay0");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MasterListItem obj = new MasterListItem();
                        obj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        obj.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        obj.Description = Convert.ToString(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.SemenSampleList.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        public override IValueObject GetDay0OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay0OocyteDetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay0OocyteDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay0DetailsForOocyte");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                // dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, 0);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay0OocyteDetailsBizActionVO Oocytedetails = new clsIVFDashboard_GetDay0OocyteDetailsBizActionVO();
                        Oocytedetails.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Oocytedetails.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"]));
                        Oocytedetails.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        Oocytedetails.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        BizAction.Oocytelist.Add(Oocytedetails.Details);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        public override IValueObject GetDay0OocyteDetailsOocyteRecipient(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay0OocyteDetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay0OocyteDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay0DetailsForOocyteRecipient");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                // dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, 0);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay0OocyteDetailsBizActionVO Oocytedetails = new clsIVFDashboard_GetDay0OocyteDetailsBizActionVO();
                        Oocytedetails.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Oocytedetails.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"]));
                        Oocytedetails.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Oocytelist.Add(Oocytedetails.Details);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        #endregion

        #region Day1
        public override IValueObject AddUpdateDay1Details(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_AddUpdateDay1BizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateDay1BizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {

                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.Day1Details.DecisionID == 0)
                {
                    DbCommand Sqlcommand11 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day1=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day1Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day1Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day1Details.SerialOocyteNumber);
                    int sqlStatus11 = dbServer.ExecuteNonQuery(Sqlcommand11, trans);
                }

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay1");


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Day1Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Day1Details.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day1Details.SerialOocyteNumber);
                dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, BizActionObj.Day1Details.OocyteNumber);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.Day1Details.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionObj.Day1Details.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.Day1Details.EmbryologistID);
                dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day1Details.AssitantEmbryologistID);
                dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, BizActionObj.Day1Details.AnesthetistID);
                dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day1Details.AssitantAnesthetistID);
                dbServer.AddInParameter(command, "CumulusID", DbType.Int64, BizActionObj.Day1Details.CumulusID);
                dbServer.AddInParameter(command, "MOIID", DbType.Int64, BizActionObj.Day1Details.MOIID);
                dbServer.AddInParameter(command, "GradeID", DbType.Int64, BizActionObj.Day1Details.GradeID);
                dbServer.AddInParameter(command, "CellStageID", DbType.Int64, BizActionObj.Day1Details.CellStageID);
                dbServer.AddInParameter(command, "OccDiamension", DbType.String, BizActionObj.Day1Details.OccDiamension);
                dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, BizActionObj.Day1Details.SpermPreperationMedia);
                dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, BizActionObj.Day1Details.OocytePreparationMedia);
                dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, BizActionObj.Day1Details.IncubatorID);
                dbServer.AddInParameter(command, "FinalLayering", DbType.String, BizActionObj.Day1Details.FinalLayering);
                dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, BizActionObj.Day1Details.NextPlanID);
                dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, BizActionObj.Day1Details.Isfreezed);
                dbServer.AddInParameter(command, "Impression", DbType.String, BizActionObj.Day1Details.Impression); // by bHUSHAn
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day1Details.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //by neena
                dbServer.AddInParameter(command, "CellObservationDate", DbType.DateTime, BizActionObj.Day1Details.CellObservationDate);
                dbServer.AddInParameter(command, "CellObservationTime", DbType.DateTime, BizActionObj.Day1Details.CellObservationTime);
                //

                dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorID);
                dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorUnitID);

                dbServer.AddInParameter(command, "CellStage", DbType.String, BizActionObj.Day1Details.CellStage);
                dbServer.AddInParameter(command, "IsBiopsy", DbType.Boolean, BizActionObj.Day1Details.IsBiopsy);
                dbServer.AddInParameter(command, "BiopsyDate", DbType.DateTime, BizActionObj.Day1Details.BiopsyDate);
                dbServer.AddInParameter(command, "BiopsyTime", DbType.DateTime, BizActionObj.Day1Details.BiopsyTime);
                dbServer.AddInParameter(command, "NoOfCell", DbType.Int64, BizActionObj.Day1Details.NoOfCell);
                dbServer.AddInParameter(command, "CellNo", DbType.Int64, BizActionObj.Day1Details.CellNo);


                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.Day1Details.ImgList != null && BizActionObj.Day1Details.ImgList.Count > 0)
                {
                    int cnt = 0;
                    foreach (var item in BizActionObj.Day1Details.ImgList)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day1Details.PatientID);
                        dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day1Details.PatientUnitID);
                        dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyID);
                        dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyUnitID);
                        dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day1Details.SerialOocyteNumber);
                        dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day1Details.OocyteNumber);
                        dbServer.AddInParameter(command1, "Day", DbType.Int64, 1);
                        dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day1Details.ID);
                        dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day1Details.CellStageID);
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

                        //dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
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

                if (BizActionObj.Day1Details.DetailList != null && BizActionObj.Day1Details.DetailList.Count > 0)
                {
                    foreach (var item in BizActionObj.Day1Details.DetailList)
                    {
                        DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                        dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item.Date);
                        dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item.PatientID);
                        dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                        dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item.PlanTherapyID);
                        dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item.PlanTherapyUnitID);
                        dbServer.AddInParameter(command8, "Description", DbType.String, item.Description);
                        dbServer.AddInParameter(command8, "Title", DbType.String, item.Title);
                        dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item.AttachedFileName);
                        dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item.AttachedFileContent);
                        dbServer.AddInParameter(command8, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.OocyteNumber);
                        dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, item.SerialOocyteNumber);
                        dbServer.AddInParameter(command8, "Day", DbType.Int64, item.Day);
                        //dbServer.AddInParameter(command8, "DocNo", DbType.String, item.DocNo);
                        dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 0);
                        if (item.DocNo == null)
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        else
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.DocNo);
                        //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus8 = dbServer.ExecuteNonQuery(command8);
                        item.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                        //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                    }
                }

                //if (BizActionObj.Day1Details.Photo != null)
                //{
                //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day1Details.PatientID);
                //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day1Details.PatientUnitID);
                //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyID);
                //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyUnitID);
                //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day1Details.SerialOocyteNumber);
                //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day1Details.OocyteNumber);
                //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 1);
                //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day1Details.ID);
                //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day1Details.CellStageID);
                //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day1Details.Photo);
                //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day1Details.FileName);
                //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                //}


                if (BizActionObj.Day1Details.NextPlanID == 3 && BizActionObj.Day1Details.Isfreezed == true)
                {
                    DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day2=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day1Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day1Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day1Details.SerialOocyteNumber);
                    int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);

                    DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay2");

                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Day1Details.PatientID);
                    dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Day1Details.PatientUnitID);
                    dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyID);
                    dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day1Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, BizActionObj.Day1Details.OocyteNumber);
                    dbServer.AddInParameter(command2, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command2, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AnesthetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AssitantAnesthetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "CumulusID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "MOIID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "GradeID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "OccDiamension", DbType.String, null);
                    dbServer.AddInParameter(command2, "SpermPreperationMedia", DbType.String, null);
                    dbServer.AddInParameter(command2, "OocytePreparationMedia", DbType.String, null);
                    dbServer.AddInParameter(command2, "IncubatorID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "FinalLayering", DbType.String, null);
                    dbServer.AddInParameter(command2, "NextPlanID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "Isfreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command2, "FrgmentationID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "BlastmereSymmetryID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "OtherDetails", DbType.String, null);
                    dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                    dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorID);
                    dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorUnitID);

                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                    //  BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                }
                if (BizActionObj.Day1Details.NextPlanID == 4 && BizActionObj.Day1Details.Isfreezed == true)
                {
                    //Add in ET table
                    DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day1Details.PatientID);
                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day1Details.PatientUnitID);
                    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyID);
                    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);


                    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                    BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                    DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                    dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day1Details.ID);
                    dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, BizActionObj.Day1Details.OocyteNumber);
                    dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day1Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day1Details.Date);
                    dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day1");
                    dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day1Details.GradeID);
                    dbServer.AddInParameter(command6, "Score", DbType.String, null);
                    dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day1Details.CellStageID);
                    dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                    dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                    dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                    dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                    dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorID);
                    dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorUnitID);

                    int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                }
                if (BizActionObj.Day1Details.NextPlanID == 2 && BizActionObj.Day1Details.Isfreezed == true)
                {
                    //Add in Vitrification table
                    DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day1Details.PatientID);
                    dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day1Details.PatientUnitID);
                    dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyID);
                    dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                    dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                    dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command4, "ID");


                    DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                    dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day1Details.ID);
                    dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, BizActionObj.Day1Details.OocyteNumber);
                    dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day1Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                    dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                    dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day1Details.Date);
                    dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day1");
                    dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day1Details.CellStageID);
                    dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day1Details.GradeID);
                    dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                    dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                    dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                    dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorID);
                    dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorUnitID);

                    dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                    dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                    int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                }

                //added by rohini dated 21/12/2015
                if (BizActionObj.Day1Details.OcyteListList != null)
                {
                    foreach (var item in BizActionObj.Day1Details.OcyteListList)
                    {
                        try
                        {

                            DbCommand Sqlcommand12 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day1=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day1Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day1Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + (BizActionObj.Day1Details.SerialOocyteNumber + item.FilterID));
                            int sqlStatus12 = dbServer.ExecuteNonQuery(Sqlcommand12, trans);

                            DbCommand command7 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay1");


                            dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command7, "PatientID", DbType.Int64, BizActionObj.Day1Details.PatientID);
                            dbServer.AddInParameter(command7, "PatientUnitID", DbType.Int64, BizActionObj.Day1Details.PatientUnitID);
                            dbServer.AddInParameter(command7, "PlanTherapyID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyID);
                            dbServer.AddInParameter(command7, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command7, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day1Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command7, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command7, "Date", DbType.DateTime, BizActionObj.Day1Details.Date);
                            dbServer.AddInParameter(command7, "Time", DbType.DateTime, BizActionObj.Day1Details.Time);
                            dbServer.AddInParameter(command7, "EmbryologistID", DbType.Int64, BizActionObj.Day1Details.EmbryologistID);
                            dbServer.AddInParameter(command7, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day1Details.AssitantEmbryologistID);
                            dbServer.AddInParameter(command7, "AnesthetistID", DbType.Int64, BizActionObj.Day1Details.AnesthetistID);
                            dbServer.AddInParameter(command7, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day1Details.AssitantAnesthetistID);
                            dbServer.AddInParameter(command7, "CumulusID", DbType.Int64, BizActionObj.Day1Details.CumulusID);
                            dbServer.AddInParameter(command7, "MOIID", DbType.Int64, BizActionObj.Day1Details.MOIID);
                            dbServer.AddInParameter(command7, "GradeID", DbType.Int64, BizActionObj.Day1Details.GradeID);
                            dbServer.AddInParameter(command7, "CellStageID", DbType.Int64, BizActionObj.Day1Details.CellStageID);
                            dbServer.AddInParameter(command7, "OccDiamension", DbType.String, BizActionObj.Day1Details.OccDiamension);
                            dbServer.AddInParameter(command7, "SpermPreperationMedia", DbType.String, BizActionObj.Day1Details.SpermPreperationMedia);
                            dbServer.AddInParameter(command7, "OocytePreparationMedia", DbType.String, BizActionObj.Day1Details.OocytePreparationMedia);
                            dbServer.AddInParameter(command7, "IncubatorID", DbType.Int64, BizActionObj.Day1Details.IncubatorID);
                            dbServer.AddInParameter(command7, "FinalLayering", DbType.String, BizActionObj.Day1Details.FinalLayering);
                            dbServer.AddInParameter(command7, "NextPlanID", DbType.Int64, BizActionObj.Day1Details.NextPlanID);
                            dbServer.AddInParameter(command7, "Isfreezed", DbType.Boolean, BizActionObj.Day1Details.Isfreezed);
                            dbServer.AddInParameter(command7, "Impression", DbType.String, BizActionObj.Day1Details.Impression); // by bHUSHAn
                            dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day1Details.ID);
                            dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, int.MaxValue);

                            //by neena
                            dbServer.AddInParameter(command7, "CellObservationDate", DbType.DateTime, BizActionObj.Day1Details.CellObservationDate);
                            dbServer.AddInParameter(command7, "CellObservationTime", DbType.DateTime, BizActionObj.Day1Details.CellObservationTime);

                            dbServer.AddInParameter(command7, "OocyteDonorID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorID);
                            dbServer.AddInParameter(command7, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorUnitID);

                            dbServer.AddInParameter(command7, "CellStage", DbType.String, BizActionObj.Day1Details.CellStage);
                            dbServer.AddInParameter(command7, "IsBiopsy", DbType.Boolean, BizActionObj.Day1Details.IsBiopsy);
                            dbServer.AddInParameter(command7, "BiopsyDate", DbType.DateTime, BizActionObj.Day1Details.BiopsyDate);
                            dbServer.AddInParameter(command7, "BiopsyTime", DbType.DateTime, BizActionObj.Day1Details.BiopsyTime);
                            dbServer.AddInParameter(command7, "NoOfCell", DbType.Int64, BizActionObj.Day1Details.NoOfCell);
                            dbServer.AddInParameter(command7, "CellNo", DbType.Int64, BizActionObj.Day1Details.CellNo);

                            int intStatus7 = dbServer.ExecuteNonQuery(command7, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command7, "ResultStatus");
                            BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command7, "ID");

                            if (BizActionObj.Day1Details.ImgList != null && BizActionObj.Day1Details.ImgList.Count > 0)
                            {
                                foreach (var item1 in BizActionObj.Day1Details.ImgList)
                                {

                                    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day1Details.PatientID);
                                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day1Details.PatientUnitID);
                                    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyID);
                                    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day1Details.SerialOocyteNumber + item.FilterID);
                                    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command1, "Day", DbType.Int64, 1);
                                    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day1Details.ID);
                                    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day1Details.CellStageID);
                                    //dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day0Details.Photo);
                                    //dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day0Details.FileName);
                                    //dbServer.AddInParameter(command1, "Photo", DbType.Binary, item.Photo);
                                    dbServer.AddInParameter(command1, "FileName", DbType.String, item1.ImagePath);
                                    //dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                                    //added by neena
                                    //dbServer.AddInParameter(command1, "SeqNo", DbType.String, item1.SeqNo);
                                    dbServer.AddParameter(command1, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.SeqNo);
                                    dbServer.AddInParameter(command1, "IsApplyTo", DbType.Int32, 1);
                                    dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ServerImageName);

                                    //dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);
                                    //

                                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                                    item1.ID = Convert.ToInt64(dbServer.GetParameterValue(command1, "ID"));
                                    item1.ServerImageName = Convert.ToString(dbServer.GetParameterValue(command1, "ServerImageName"));
                                    item1.SeqNo = Convert.ToString(dbServer.GetParameterValue(command1, "SeqNo"));

                                    //if (item1.Photo != null)
                                    //    File.WriteAllBytes(ImgSaveLocation + item1.ServerImageName, item1.Photo);

                                    //cnt++;
                                }
                            }

                            //if (BizActionObj.Day1Details.Photo != null)
                            //{
                            //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                            //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day1Details.PatientID);
                            //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day1Details.PatientUnitID);
                            //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyID);
                            //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyUnitID);
                            //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day1Details.SerialOocyteNumber + item.FilterID);
                            //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                            //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 1);
                            //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day1Details.ID);
                            //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day1Details.CellStageID);
                            //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day1Details.Photo);
                            //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day1Details.FileName);
                            //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                            //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                            //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                            //}

                            if (BizActionObj.Day1Details.DetailList != null && BizActionObj.Day1Details.DetailList.Count > 0)
                            {
                                foreach (var item2 in BizActionObj.Day1Details.DetailList)
                                {
                                    DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                                    dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item2.Date);
                                    dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item2.PatientID);
                                    dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item2.PatientUnitID);
                                    dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item2.PlanTherapyID);
                                    dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item2.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command8, "Description", DbType.String, item2.Description);
                                    dbServer.AddInParameter(command8, "Title", DbType.String, item2.Title);
                                    dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item2.AttachedFileName);
                                    dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item2.AttachedFileContent);
                                    dbServer.AddInParameter(command8, "Status", DbType.Boolean, item2.Status);
                                    dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                    dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                    dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                    dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                                    dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day1Details.SerialOocyteNumber + item.FilterID);
                                    dbServer.AddInParameter(command8, "Day", DbType.Int64, item2.Day);
                                    //dbServer.AddInParameter(command8, "DocNo", DbType.String, item2.DocNo);
                                    dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.DocNo);
                                    dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                                    dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 1);
                                    //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                                    int intStatus8 = dbServer.ExecuteNonQuery(command8);
                                    item2.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                                    //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                                }
                            }

                        }

                        catch (Exception ex)
                        {

                        }

                        if (BizActionObj.Day1Details.NextPlanID == 3 && BizActionObj.Day1Details.Isfreezed == true)
                        {
                            DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day2=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day1Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day1Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day1Details.SerialOocyteNumber + item.FilterID);
                            int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);

                            DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay2");

                            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Day1Details.PatientID);
                            dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Day1Details.PatientUnitID);
                            dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyID);
                            dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day1Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command2, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command2, "Time", DbType.DateTime, null);
                            dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AssitantEmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AnesthetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AssitantAnesthetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "CumulusID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "MOIID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "GradeID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "OccDiamension", DbType.String, null);
                            dbServer.AddInParameter(command2, "SpermPreperationMedia", DbType.String, null);
                            dbServer.AddInParameter(command2, "OocytePreparationMedia", DbType.String, null);
                            dbServer.AddInParameter(command2, "IncubatorID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "FinalLayering", DbType.String, null);
                            dbServer.AddInParameter(command2, "NextPlanID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "Isfreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command2, "FrgmentationID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "BlastmereSymmetryID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "OtherDetails", DbType.String, null);
                            dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                            dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorID);
                            dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorUnitID);

                            int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                            //  BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                        }
                        if (BizActionObj.Day1Details.NextPlanID == 4 && BizActionObj.Day1Details.Isfreezed == true)
                        {
                            //Add in ET table
                            DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day1Details.PatientID);
                            dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day1Details.PatientUnitID);
                            dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyID);
                            dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);


                            int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                            BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                            DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                            dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day1Details.ID);
                            dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day1Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day1Details.Date);
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day1");
                            dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day1Details.GradeID);
                            dbServer.AddInParameter(command6, "Score", DbType.String, null);
                            dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day1Details.CellStageID);
                            dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                            dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                            dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                            dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                            dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorID);
                            dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorUnitID);

                            int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                        }
                        if (BizActionObj.Day1Details.NextPlanID == 2 && BizActionObj.Day1Details.Isfreezed == true)
                        {
                            //Add in Vitrification table
                            DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day1Details.PatientID);
                            dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day1Details.PatientUnitID);
                            dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyID);
                            dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day1Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                            dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                            dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                            BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command4, "ID");


                            DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                            dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day1Details.ID);
                            dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day1Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                            dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                            dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day1Details.Date);
                            dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day1");
                            dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day1Details.CellStageID);
                            dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day1Details.GradeID);
                            dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                            dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                            dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                            dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorID);
                            dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day1Details.OocyteDonorUnitID);

                            dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                            dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                            int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                        }

                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Day1Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;


        }
        public override IValueObject GetDay1Details(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay1DetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay1DetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay1Details");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        BizAction.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        BizAction.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        BizAction.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        BizAction.Details.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                        BizAction.Details.Time = (DateTime?)(DALHelper.HandleDate(reader["Time"]));
                        BizAction.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        BizAction.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        BizAction.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        BizAction.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        BizAction.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        BizAction.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        BizAction.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        BizAction.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        BizAction.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        BizAction.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        BizAction.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        BizAction.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        BizAction.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        BizAction.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        BizAction.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        //BizAction.Details.Photo = (byte[])(DALHelper.HandleDBNull(reader["Image"]));
                        //BizAction.Details.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        BizAction.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));// BY BHUSHAN
                        //BizAction.Details.TreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TreatmentID"]));

                        BizAction.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        BizAction.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));

                        //by neena
                        //BizAction.Details.TreatmentStartDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentStartDate"]));
                        //BizAction.Details.TreatmentEndDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentEndDate"]));
                        //BizAction.Details.ObservationDate = (DateTime?)(DALHelper.HandleDate(reader["ObservationDate"]));
                        //BizAction.Details.ObservationTime = (DateTime?)(DALHelper.HandleDate(reader["ObservationTime"]));
                        //BizAction.Details.FertilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertilizationID"]));
                        BizAction.Details.CellObservationDate = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate"]));
                        BizAction.Details.CellObservationTime = (DateTime?)(DALHelper.HandleDate(reader["CellObservationTime"]));
                        BizAction.Details.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        BizAction.Details.IsBiopsy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBiopsy"]));
                        BizAction.Details.BiopsyDate = (DateTime?)(DALHelper.HandleDate(reader["BiopsyDate"]));
                        BizAction.Details.BiopsyTime = (DateTime?)(DALHelper.HandleDate(reader["BiopsyTime"]));
                        BizAction.Details.NoOfCell = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfCell"]));
                        BizAction.Details.CellNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellNo"]));

                        if (BizAction.Details.IsBiopsy == false)
                        {
                            BizAction.Details.BiopsyDate = null;
                            BizAction.Details.BiopsyTime = null;
                        }


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

                        if (!string.IsNullOrEmpty(ObjImg.OriginalImagePath))
                            ObjImg.ServerImageName = "..//" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;
                        // ObjImg.ServerImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;


                        BizAction.Details.ImgList.Add(ObjImg);
                    }
                }

                reader.NextResult();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_TherapyDocumentVO Details = new clsIVFDashboard_TherapyDocumentVO();
                        Details.ID = (long)reader["ID"];
                        Details.UnitID = (long)reader["UnitID"];
                        Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        Details.Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]));
                        Details.Date = (DateTime)(DALHelper.HandleDate(reader["Date"]));
                        Details.Title = Convert.ToString((DALHelper.HandleDBNull(reader["Title"])));
                        Details.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Details.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        Details.AttachedFileContent = (Byte[])(DALHelper.HandleDBNull(reader["AttachedFileContent"]));
                        Details.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));

                        BizAction.Details.DetailList.Add(Details);
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        public override IValueObject GetDay1OocyteObservations(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay1OocyteDetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay1OocyteDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay1DetailsForOocyte");
                //DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetFertCheckDetailsForOocyte");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                //dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64,0);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay1OocyteDetailsBizActionVO Oocytedetails = new clsIVFDashboard_GetDay1OocyteDetailsBizActionVO();
                        Oocytedetails.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Oocytedetails.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"]));
                        Oocytedetails.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Oocytelist.Add(Oocytedetails.Details);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        public override IValueObject GetDay1OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay1OocyteDetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay1OocyteDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                //DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay1DetailsForOocyte");
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetFertCheckDetailsForOocyte");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                //dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64,0);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay1OocyteDetailsBizActionVO Oocytedetails = new clsIVFDashboard_GetDay1OocyteDetailsBizActionVO();
                        Oocytedetails.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Oocytedetails.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"]));
                        Oocytedetails.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Oocytelist.Add(Oocytedetails.Details);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        public override IValueObject GetObservationDate(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay1DetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay1DetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetObservationDate");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                //dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.Details.CellObservationDate = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate"]));

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        public override IValueObject GetIVFICSIPlannedOocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetFertCheckBizActionVO BizAction = valueObject as clsIVFDashboard_GetFertCheckBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay0IVFICSIPlanForOocyte");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.FertCheckDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.FertCheckDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.FertCheckDetails.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetFertCheckBizActionVO Oocytedetails = new clsIVFDashboard_GetFertCheckBizActionVO();
                        Oocytedetails.FertCheckDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Oocytedetails.FertCheckDetails.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"]));
                        Oocytedetails.FertCheckDetails.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Oocytelist.Add(Oocytedetails.FertCheckDetails);
                    }
                }

                reader.NextResult();

                List<clsIVFDashboard_FertCheck> Oocytelist = new List<clsIVFDashboard_FertCheck>();
                while (reader.Read())
                {

                    clsIVFDashboard_GetFertCheckBizActionVO Oocytedetails = new clsIVFDashboard_GetFertCheckBizActionVO();
                    Oocytedetails.FertCheckDetails.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                    Oocytelist.Add(Oocytedetails.FertCheckDetails);
                }

                var list = BizAction.Oocytelist.Where(a => !Oocytelist.Any(b => b.OocyteNumber == a.OocyteNumber)).ToList();
                BizAction.Oocytelist = list;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }


        #endregion

        #region Day2
        public override IValueObject AddUpdateDay2Details(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_AddUpdateDay2BizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateDay2BizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {

                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.Day2Details.DecisionID == 0)
                {
                    DbCommand Sqlcommand11 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day2=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day2Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day2Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day2Details.SerialOocyteNumber);
                    int sqlStatus11 = dbServer.ExecuteNonQuery(Sqlcommand11, trans);
                }

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay2");


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber);
                dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, BizActionObj.Day2Details.OocyteNumber);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.Day2Details.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionObj.Day2Details.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.Day2Details.EmbryologistID);
                dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day2Details.AssitantEmbryologistID);
                dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, BizActionObj.Day2Details.AnesthetistID);
                dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day2Details.AssitantAnesthetistID);
                dbServer.AddInParameter(command, "CumulusID", DbType.Int64, BizActionObj.Day2Details.CumulusID);
                dbServer.AddInParameter(command, "MOIID", DbType.Int64, BizActionObj.Day2Details.MOIID);
                dbServer.AddInParameter(command, "GradeID", DbType.Int64, BizActionObj.Day2Details.GradeID);
                dbServer.AddInParameter(command, "CellStageID", DbType.Int64, BizActionObj.Day2Details.CellStageID);
                dbServer.AddInParameter(command, "OccDiamension", DbType.String, BizActionObj.Day2Details.OccDiamension);
                dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, BizActionObj.Day2Details.SpermPreperationMedia);
                dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, BizActionObj.Day2Details.OocytePreparationMedia);
                dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, BizActionObj.Day2Details.IncubatorID);
                dbServer.AddInParameter(command, "FinalLayering", DbType.String, BizActionObj.Day2Details.FinalLayering);
                dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, BizActionObj.Day2Details.NextPlanID);
                dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, BizActionObj.Day2Details.Isfreezed);
                dbServer.AddInParameter(command, "Impression", DbType.String, BizActionObj.Day2Details.Impression); // by bHUSHAn
                dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, BizActionObj.Day2Details.FrgmentationID);
                dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, BizActionObj.Day2Details.BlastmereSymmetryID);
                dbServer.AddInParameter(command, "OtherDetails", DbType.String, BizActionObj.Day2Details.OtherDetails);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day2Details.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //by neena
                dbServer.AddInParameter(command, "CellObservationDate", DbType.DateTime, BizActionObj.Day2Details.CellObservationDate);
                dbServer.AddInParameter(command, "CellObservationTime", DbType.DateTime, BizActionObj.Day2Details.CellObservationTime);
                //

                dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorID);
                dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorUnitID);

                dbServer.AddInParameter(command, "CellStage", DbType.String, BizActionObj.Day2Details.CellStage);
                dbServer.AddInParameter(command, "IsBiopsy", DbType.Boolean, BizActionObj.Day2Details.IsBiopsy);
                dbServer.AddInParameter(command, "BiopsyDate", DbType.DateTime, BizActionObj.Day2Details.BiopsyDate);
                dbServer.AddInParameter(command, "BiopsyTime", DbType.DateTime, BizActionObj.Day2Details.BiopsyTime);
                dbServer.AddInParameter(command, "NoOfCell", DbType.Int64, BizActionObj.Day2Details.NoOfCell);
                dbServer.AddInParameter(command, "CellNo", DbType.Int64, BizActionObj.Day2Details.CellNo);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Day2Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.Day2Details.ImgList != null && BizActionObj.Day2Details.ImgList.Count > 0)
                {
                    foreach (var item in BizActionObj.Day2Details.ImgList)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                        dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                        dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                        dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                        dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber);
                        dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day2Details.OocyteNumber);
                        dbServer.AddInParameter(command1, "Day", DbType.Int64, 2);
                        dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day2Details.ID);
                        dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day2Details.CellStageID);
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
                        //dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
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

                if (BizActionObj.Day2Details.DetailList != null && BizActionObj.Day2Details.DetailList.Count > 0)
                {
                    foreach (var item in BizActionObj.Day2Details.DetailList)
                    {
                        DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                        dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item.Date);
                        dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item.PatientID);
                        dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                        dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item.PlanTherapyID);
                        dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item.PlanTherapyUnitID);
                        dbServer.AddInParameter(command8, "Description", DbType.String, item.Description);
                        dbServer.AddInParameter(command8, "Title", DbType.String, item.Title);
                        dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item.AttachedFileName);
                        dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item.AttachedFileContent);
                        dbServer.AddInParameter(command8, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.OocyteNumber);
                        dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, item.SerialOocyteNumber);
                        dbServer.AddInParameter(command8, "Day", DbType.Int64, item.Day);
                        //dbServer.AddInParameter(command8, "DocNo", DbType.String, item.DocNo);
                        dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 0);
                        if (item.DocNo == null)
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        else
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.DocNo);

                        //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus8 = dbServer.ExecuteNonQuery(command8);
                        item.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                        //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                    }
                }

                //if (BizActionObj.Day2Details.Photo != null)
                //{
                //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber);
                //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day2Details.OocyteNumber);
                //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 2);
                //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day2Details.ID);
                //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day2Details.CellStageID);
                //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day2Details.Photo);
                //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day2Details.FileName);
                //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                //}

                if (BizActionObj.Day2Details.NextPlanID == 3 && BizActionObj.Day2Details.Isfreezed == true)
                {
                    DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day3=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day2Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day2Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day2Details.SerialOocyteNumber);
                    int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);

                    DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay3");

                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                    dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                    dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                    dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, BizActionObj.Day2Details.OocyteNumber);
                    dbServer.AddInParameter(command2, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command2, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AnesthetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AssitantAnesthetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "CumulusID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "MOIID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "GradeID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "OccDiamension", DbType.String, null);
                    dbServer.AddInParameter(command2, "SpermPreperationMedia", DbType.String, null);
                    dbServer.AddInParameter(command2, "OocytePreparationMedia", DbType.String, null);
                    dbServer.AddInParameter(command2, "IncubatorID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "FinalLayering", DbType.String, null);
                    dbServer.AddInParameter(command2, "NextPlanID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "Isfreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command2, "FrgmentationID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "BlastmereSymmetryID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "OtherDetails", DbType.String, null);
                    dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                    dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorID);
                    dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorUnitID);

                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                    //  BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                }
                if (BizActionObj.Day2Details.NextPlanID == 4 && BizActionObj.Day2Details.Isfreezed == true)
                {
                    //Add in ET table
                    DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                    BizActionObj.Day2Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                    DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                    dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day2Details.ID);
                    dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, BizActionObj.Day2Details.OocyteNumber);
                    dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day2Details.Date);
                    dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day2");
                    dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day2Details.GradeID);
                    dbServer.AddInParameter(command6, "Score", DbType.String, null);
                    dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day2Details.CellStageID);
                    dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                    dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                    dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                    dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                    dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorID);
                    dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorUnitID);

                    int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                }
                if (BizActionObj.Day2Details.NextPlanID == 2 && BizActionObj.Day2Details.Isfreezed == true)
                {
                    //Add in Vitrification table
                    DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                    dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                    dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                    dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                    dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                    dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    BizActionObj.Day2Details.ID = (long)dbServer.GetParameterValue(command4, "ID");


                    DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                    dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day2Details.ID);
                    dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, BizActionObj.Day2Details.OocyteNumber);
                    dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                    dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                    dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day2Details.Date);
                    dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day2");
                    dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day2Details.CellStageID);
                    dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day2Details.GradeID);
                    dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                    dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                    dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                    dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorID);
                    dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorUnitID);

                    dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                    dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                    int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                }

                //added by neena
                if (BizActionObj.Day2Details.OcyteListList != null)
                {
                    foreach (var item in BizActionObj.Day2Details.OcyteListList)
                    {
                        try
                        {
                            DbCommand Sqlcommand12 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day2=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day2Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day2Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + (BizActionObj.Day2Details.SerialOocyteNumber + item.FilterID));
                            int sqlStatus12 = dbServer.ExecuteNonQuery(Sqlcommand12, trans);

                            DbCommand command7 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay2");

                            dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command7, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                            dbServer.AddInParameter(command7, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                            dbServer.AddInParameter(command7, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                            dbServer.AddInParameter(command7, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command7, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command7, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command7, "Date", DbType.DateTime, BizActionObj.Day2Details.Date);
                            dbServer.AddInParameter(command7, "Time", DbType.DateTime, BizActionObj.Day2Details.Time);
                            dbServer.AddInParameter(command7, "EmbryologistID", DbType.Int64, BizActionObj.Day2Details.EmbryologistID);
                            dbServer.AddInParameter(command7, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day2Details.AssitantEmbryologistID);
                            dbServer.AddInParameter(command7, "AnesthetistID", DbType.Int64, BizActionObj.Day2Details.AnesthetistID);
                            dbServer.AddInParameter(command7, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day2Details.AssitantAnesthetistID);
                            dbServer.AddInParameter(command7, "CumulusID", DbType.Int64, BizActionObj.Day2Details.CumulusID);
                            dbServer.AddInParameter(command7, "MOIID", DbType.Int64, BizActionObj.Day2Details.MOIID);
                            dbServer.AddInParameter(command7, "GradeID", DbType.Int64, BizActionObj.Day2Details.GradeID);
                            dbServer.AddInParameter(command7, "CellStageID", DbType.Int64, BizActionObj.Day2Details.CellStageID);
                            dbServer.AddInParameter(command7, "OccDiamension", DbType.String, BizActionObj.Day2Details.OccDiamension);
                            dbServer.AddInParameter(command7, "SpermPreperationMedia", DbType.String, BizActionObj.Day2Details.SpermPreperationMedia);
                            dbServer.AddInParameter(command7, "OocytePreparationMedia", DbType.String, BizActionObj.Day2Details.OocytePreparationMedia);
                            dbServer.AddInParameter(command7, "IncubatorID", DbType.Int64, BizActionObj.Day2Details.IncubatorID);
                            dbServer.AddInParameter(command7, "FinalLayering", DbType.String, BizActionObj.Day2Details.FinalLayering);
                            dbServer.AddInParameter(command7, "NextPlanID", DbType.Int64, BizActionObj.Day2Details.NextPlanID);
                            dbServer.AddInParameter(command7, "Isfreezed", DbType.Boolean, BizActionObj.Day2Details.Isfreezed);
                            dbServer.AddInParameter(command7, "Impression", DbType.String, BizActionObj.Day2Details.Impression); // by bHUSHAn
                            dbServer.AddInParameter(command7, "FrgmentationID", DbType.Int64, BizActionObj.Day2Details.FrgmentationID);
                            dbServer.AddInParameter(command7, "BlastmereSymmetryID", DbType.Int64, BizActionObj.Day2Details.BlastmereSymmetryID);
                            dbServer.AddInParameter(command7, "OtherDetails", DbType.String, BizActionObj.Day2Details.OtherDetails);
                            dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day2Details.ID);
                            dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, int.MaxValue);

                            //by neena
                            dbServer.AddInParameter(command7, "CellObservationDate", DbType.DateTime, BizActionObj.Day2Details.CellObservationDate);
                            dbServer.AddInParameter(command7, "CellObservationTime", DbType.DateTime, BizActionObj.Day2Details.CellObservationTime);
                            //

                            dbServer.AddInParameter(command7, "OocyteDonorID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorID);
                            dbServer.AddInParameter(command7, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorUnitID);

                            dbServer.AddInParameter(command7, "CellStage", DbType.String, BizActionObj.Day2Details.CellStage);
                            dbServer.AddInParameter(command7, "IsBiopsy", DbType.Boolean, BizActionObj.Day2Details.IsBiopsy);
                            dbServer.AddInParameter(command7, "BiopsyDate", DbType.DateTime, BizActionObj.Day2Details.BiopsyDate);
                            dbServer.AddInParameter(command7, "BiopsyTime", DbType.DateTime, BizActionObj.Day2Details.BiopsyTime);
                            dbServer.AddInParameter(command7, "NoOfCell", DbType.Int64, BizActionObj.Day2Details.NoOfCell);
                            dbServer.AddInParameter(command7, "CellNo", DbType.Int64, BizActionObj.Day2Details.CellNo);

                            int intStatus7 = dbServer.ExecuteNonQuery(command7, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command7, "ResultStatus");
                            BizActionObj.Day2Details.ID = (long)dbServer.GetParameterValue(command7, "ID");

                            if (BizActionObj.Day2Details.ImgList != null && BizActionObj.Day2Details.ImgList.Count > 0)
                            {
                                foreach (var item1 in BizActionObj.Day2Details.ImgList)
                                {
                                    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                                    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                                    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber + item.FilterID);
                                    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command1, "Day", DbType.Int64, 2);
                                    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day2Details.ID);
                                    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day2Details.CellStageID);
                                    //dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day0Details.Photo);
                                    //dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day0Details.FileName);
                                    //dbServer.AddInParameter(command1, "Photo", DbType.Binary, item.Photo);
                                    dbServer.AddInParameter(command1, "FileName", DbType.String, item1.ImagePath);
                                    //dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                                    //added by neena
                                    //dbServer.AddInParameter(command1, "SeqNo", DbType.String, item1.SeqNo);
                                    dbServer.AddParameter(command1, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.SeqNo);
                                    dbServer.AddInParameter(command1, "IsApplyTo", DbType.Int32, 1);
                                    dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ServerImageName);

                                    //dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);
                                    //

                                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                                    item1.ID = Convert.ToInt64(dbServer.GetParameterValue(command1, "ID"));
                                    item1.ServerImageName = Convert.ToString(dbServer.GetParameterValue(command1, "ServerImageName"));
                                    item1.SeqNo = Convert.ToString(dbServer.GetParameterValue(command1, "SeqNo"));

                                    //if (item1.Photo != null)
                                    //    File.WriteAllBytes(ImgSaveLocation + item1.ServerImageName, item1.Photo);

                                    //cnt++;
                                }
                            }


                            //if (BizActionObj.Day2Details.Photo != null)
                            //{
                            //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                            //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                            //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                            //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                            //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                            //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber + item.FilterID);
                            //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                            //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 2);
                            //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day2Details.ID);
                            //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day2Details.CellStageID);
                            //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day2Details.Photo);
                            //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day2Details.FileName);
                            //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                            //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                            //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                            //}

                            if (BizActionObj.Day2Details.DetailList != null && BizActionObj.Day2Details.DetailList.Count > 0)
                            {
                                foreach (var item2 in BizActionObj.Day2Details.DetailList)
                                {
                                    DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                                    dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item2.Date);
                                    dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item2.PatientID);
                                    dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item2.PatientUnitID);
                                    dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item2.PlanTherapyID);
                                    dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item2.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command8, "Description", DbType.String, item2.Description);
                                    dbServer.AddInParameter(command8, "Title", DbType.String, item2.Title);
                                    dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item2.AttachedFileName);
                                    dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item2.AttachedFileContent);
                                    dbServer.AddInParameter(command8, "Status", DbType.Boolean, item2.Status);
                                    dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                    dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                    dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                    dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                                    dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber + item.FilterID);
                                    dbServer.AddInParameter(command8, "Day", DbType.Int64, item2.Day);
                                    //dbServer.AddInParameter(command8, "DocNo", DbType.String, item2.DocNo);
                                    dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.DocNo);
                                    dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                                    dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 1);
                                    //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                                    int intStatus8 = dbServer.ExecuteNonQuery(command8);
                                    item2.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                                    //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                                }
                            }

                        }

                        catch (Exception ex)
                        {

                        }

                        if (BizActionObj.Day2Details.NextPlanID == 3 && BizActionObj.Day2Details.Isfreezed == true)
                        {
                            DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day3=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day2Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day2Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day2Details.SerialOocyteNumber + item.FilterID);
                            int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);

                            DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay3");

                            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                            dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                            dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                            dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command2, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command2, "Time", DbType.DateTime, null);
                            dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AssitantEmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AnesthetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AssitantAnesthetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "CumulusID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "MOIID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "GradeID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "OccDiamension", DbType.String, null);
                            dbServer.AddInParameter(command2, "SpermPreperationMedia", DbType.String, null);
                            dbServer.AddInParameter(command2, "OocytePreparationMedia", DbType.String, null);
                            dbServer.AddInParameter(command2, "IncubatorID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "FinalLayering", DbType.String, null);
                            dbServer.AddInParameter(command2, "NextPlanID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "Isfreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command2, "FrgmentationID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "BlastmereSymmetryID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "OtherDetails", DbType.String, null);
                            dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                            dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorID);
                            dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorUnitID);

                            int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                            //  BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                        }
                        if (BizActionObj.Day2Details.NextPlanID == 4 && BizActionObj.Day2Details.Isfreezed == true)
                        {
                            //Add in ET table
                            DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                            dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                            dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                            dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                            BizActionObj.Day2Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                            DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                            dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day2Details.ID);
                            dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day2Details.Date);
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day2");
                            dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day2Details.GradeID);
                            dbServer.AddInParameter(command6, "Score", DbType.String, null);
                            dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day2Details.CellStageID);
                            dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                            dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                            dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                            dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                            dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorID);
                            dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorUnitID);

                            int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                        }
                        if (BizActionObj.Day2Details.NextPlanID == 2 && BizActionObj.Day2Details.Isfreezed == true)
                        {
                            //Add in Vitrification table
                            DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                            dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                            dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                            dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                            dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                            dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                            BizActionObj.Day2Details.ID = (long)dbServer.GetParameterValue(command4, "ID");


                            DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                            dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day2Details.ID);
                            dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                            dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                            dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day2Details.Date);
                            dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day2");
                            dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day2Details.CellStageID);
                            dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day2Details.GradeID);
                            dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                            dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                            dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                            dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorID);
                            dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day2Details.OocyteDonorUnitID);

                            dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                            dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                            int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                        }

                    }
                }


                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Day2Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;
        }
        public override IValueObject GetDay2Details(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay2DetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay2DetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay2Details");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        BizAction.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        BizAction.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        BizAction.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        BizAction.Details.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                        BizAction.Details.Time = (DateTime?)(DALHelper.HandleDate(reader["Time"]));
                        BizAction.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        BizAction.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        BizAction.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        BizAction.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        BizAction.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        BizAction.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        BizAction.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        BizAction.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        BizAction.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        BizAction.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        BizAction.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        BizAction.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        BizAction.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        BizAction.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        BizAction.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        //BizAction.Details.Photo = (byte[])(DALHelper.HandleDBNull(reader["Image"]));
                        BizAction.Details.FrgmentationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrgmentationID"]));
                        BizAction.Details.BlastmereSymmetryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlastmereSymmetryID"]));
                        BizAction.Details.OtherDetails = Convert.ToString(DALHelper.HandleDBNull(reader["OtherDetails"]));
                        //BizAction.Details.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        BizAction.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));// BY BHUSHAN
                        //BizAction.Details.TreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TreatmentID"]));
                        BizAction.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        BizAction.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));

                        //by neena
                        //BizAction.Details.TreatmentStartDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentStartDate"]));
                        //BizAction.Details.TreatmentEndDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentEndDate"]));
                        //BizAction.Details.ObservationDate = (DateTime?)(DALHelper.HandleDate(reader["ObservationDate"]));
                        //BizAction.Details.ObservationTime = (DateTime?)(DALHelper.HandleDate(reader["ObservationTime"]));
                        //BizAction.Details.FertilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertilizationID"]));
                        BizAction.Details.CellObservationDate = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate"]));
                        BizAction.Details.CellObservationTime = (DateTime?)(DALHelper.HandleDate(reader["CellObservationTime"]));
                        BizAction.Details.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        BizAction.Details.IsBiopsy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBiopsy"]));
                        BizAction.Details.BiopsyDate = (DateTime?)(DALHelper.HandleDate(reader["BiopsyDate"]));
                        BizAction.Details.BiopsyTime = (DateTime?)(DALHelper.HandleDate(reader["BiopsyTime"]));
                        BizAction.Details.NoOfCell = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfCell"]));
                        BizAction.Details.CellNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellNo"]));

                        if (BizAction.Details.IsBiopsy == false)
                        {
                            BizAction.Details.BiopsyDate = null;
                            BizAction.Details.BiopsyTime = null;
                        }
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

                        if (!string.IsNullOrEmpty(ObjImg.OriginalImagePath))
                            ObjImg.ServerImageName = "..//" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;
                        //ObjImg.ServerImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;


                        BizAction.Details.ImgList.Add(ObjImg);
                    }
                }

                reader.NextResult();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_TherapyDocumentVO Details = new clsIVFDashboard_TherapyDocumentVO();
                        Details.ID = (long)reader["ID"];
                        Details.UnitID = (long)reader["UnitID"];
                        Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        Details.Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]));
                        Details.Date = (DateTime)(DALHelper.HandleDate(reader["Date"]));
                        Details.Title = Convert.ToString((DALHelper.HandleDBNull(reader["Title"])));
                        Details.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Details.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        Details.AttachedFileContent = (Byte[])(DALHelper.HandleDBNull(reader["AttachedFileContent"]));
                        Details.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));

                        BizAction.Details.DetailList.Add(Details);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        //added by neena
        public override IValueObject GetDay2OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay2OocyteDetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay2OocyteDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay2DetailsForOocyte");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                //dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, 0);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay2OocyteDetailsBizActionVO Oocytedetails = new clsIVFDashboard_GetDay2OocyteDetailsBizActionVO();
                        Oocytedetails.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Oocytedetails.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"]));
                        Oocytedetails.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Oocytelist.Add(Oocytedetails.Details);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }
        #endregion

        #region Day3
        public override IValueObject AddUpdateDay3Details(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_AddUpdateDay3BizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateDay3BizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {

                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.Day3Details.DecisionID == 0)
                {
                    DbCommand Sqlcommand11 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day3=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day3Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day3Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day3Details.SerialOocyteNumber);
                    int sqlStatus11 = dbServer.ExecuteNonQuery(Sqlcommand11, trans);
                }

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay3");


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Day3Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Day3Details.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day3Details.SerialOocyteNumber);
                dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, BizActionObj.Day3Details.OocyteNumber);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.Day3Details.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionObj.Day3Details.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.Day3Details.EmbryologistID);
                dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day3Details.AssitantEmbryologistID);
                dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, BizActionObj.Day3Details.AnesthetistID);
                dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day3Details.AssitantAnesthetistID);
                dbServer.AddInParameter(command, "CumulusID", DbType.Int64, BizActionObj.Day3Details.CumulusID);
                dbServer.AddInParameter(command, "MOIID", DbType.Int64, BizActionObj.Day3Details.MOIID);
                dbServer.AddInParameter(command, "GradeID", DbType.Int64, BizActionObj.Day3Details.GradeID);
                dbServer.AddInParameter(command, "CellStageID", DbType.Int64, BizActionObj.Day3Details.CellStageID);
                dbServer.AddInParameter(command, "OccDiamension", DbType.String, BizActionObj.Day3Details.OccDiamension);
                dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, BizActionObj.Day3Details.SpermPreperationMedia);
                dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, BizActionObj.Day3Details.OocytePreparationMedia);
                dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, BizActionObj.Day3Details.IncubatorID);
                dbServer.AddInParameter(command, "FinalLayering", DbType.String, BizActionObj.Day3Details.FinalLayering);
                dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, BizActionObj.Day3Details.NextPlanID);
                dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, BizActionObj.Day3Details.Isfreezed);
                dbServer.AddInParameter(command, "Impression", DbType.String, BizActionObj.Day3Details.Impression); // by bHUSHAn
                dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, BizActionObj.Day3Details.FrgmentationID);
                dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, BizActionObj.Day3Details.BlastmereSymmetryID);
                dbServer.AddInParameter(command, "OtherDetails", DbType.String, BizActionObj.Day3Details.OtherDetails);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day3Details.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //by neena
                dbServer.AddInParameter(command, "CellObservationDate", DbType.DateTime, BizActionObj.Day3Details.CellObservationDate);
                dbServer.AddInParameter(command, "CellObservationTime", DbType.DateTime, BizActionObj.Day3Details.CellObservationTime);
                //

                dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorID);
                dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorUnitID);

                dbServer.AddInParameter(command, "CellStage", DbType.String, BizActionObj.Day3Details.CellStage);
                dbServer.AddInParameter(command, "IsBiopsy", DbType.Boolean, BizActionObj.Day3Details.IsBiopsy);
                dbServer.AddInParameter(command, "BiopsyDate", DbType.DateTime, BizActionObj.Day3Details.BiopsyDate);
                dbServer.AddInParameter(command, "BiopsyTime", DbType.DateTime, BizActionObj.Day3Details.BiopsyTime);
                dbServer.AddInParameter(command, "NoOfCell", DbType.Int64, BizActionObj.Day3Details.NoOfCell);
                dbServer.AddInParameter(command, "CellNo", DbType.Int64, BizActionObj.Day3Details.CellNo);

                dbServer.AddInParameter(command, "IsEmbryoCompacted", DbType.Boolean, BizActionObj.Day3Details.IsEmbryoCompacted);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Day3Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.Day3Details.ImgList != null && BizActionObj.Day3Details.ImgList.Count > 0)
                {
                    foreach (var item in BizActionObj.Day3Details.ImgList)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day3Details.PatientID);
                        dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day3Details.PatientUnitID);
                        dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyID);
                        dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyUnitID);
                        dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day3Details.SerialOocyteNumber);
                        dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day3Details.OocyteNumber);
                        dbServer.AddInParameter(command1, "Day", DbType.Int64, 3);
                        dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day3Details.ID);
                        dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day3Details.CellStageID);
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
                        // dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
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

                if (BizActionObj.Day3Details.DetailList != null && BizActionObj.Day3Details.DetailList.Count > 0)
                {
                    foreach (var item in BizActionObj.Day3Details.DetailList)
                    {
                        DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                        dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item.Date);
                        dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item.PatientID);
                        dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                        dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item.PlanTherapyID);
                        dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item.PlanTherapyUnitID);
                        dbServer.AddInParameter(command8, "Description", DbType.String, item.Description);
                        dbServer.AddInParameter(command8, "Title", DbType.String, item.Title);
                        dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item.AttachedFileName);
                        dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item.AttachedFileContent);
                        dbServer.AddInParameter(command8, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.OocyteNumber);
                        dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, item.SerialOocyteNumber);
                        dbServer.AddInParameter(command8, "Day", DbType.Int64, item.Day);
                        //dbServer.AddInParameter(command8, "DocNo", DbType.String, item.DocNo);
                        dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 0);
                        if (item.DocNo == null)
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        else
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.DocNo);

                        //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus8 = dbServer.ExecuteNonQuery(command8);
                        item.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                        //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                    }
                }

                //if (BizActionObj.Day3Details.Photo != null)
                //{
                //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day3Details.PatientID);
                //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day3Details.PatientUnitID);
                //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyID);
                //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyUnitID);
                //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day3Details.SerialOocyteNumber);
                //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day3Details.OocyteNumber);
                //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 3);
                //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day3Details.ID);
                //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day3Details.CellStageID);
                //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day3Details.Photo);
                //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day3Details.FileName);
                //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                //}
                if (BizActionObj.Day3Details.NextPlanID == 3 && BizActionObj.Day3Details.Isfreezed == true)
                {
                    DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day4=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day3Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day3Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day3Details.SerialOocyteNumber);
                    int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);

                    DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay4");

                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Day3Details.PatientID);
                    dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Day3Details.PatientUnitID);
                    dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyID);
                    dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day3Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, BizActionObj.Day3Details.OocyteNumber);
                    dbServer.AddInParameter(command2, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command2, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AnesthetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AssitantAnesthetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "CumulusID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "MOIID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "GradeID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "OccDiamension", DbType.String, null);
                    dbServer.AddInParameter(command2, "SpermPreperationMedia", DbType.String, null);
                    dbServer.AddInParameter(command2, "OocytePreparationMedia", DbType.String, null);
                    dbServer.AddInParameter(command2, "IncubatorID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "FinalLayering", DbType.String, null);
                    dbServer.AddInParameter(command2, "NextPlanID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "Isfreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command2, "FrgmentationID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "BlastmereSymmetryID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "OtherDetails", DbType.String, null);
                    dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                    dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorID);
                    dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorUnitID);

                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                    //  BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                }
                if (BizActionObj.Day3Details.NextPlanID == 4 && BizActionObj.Day3Details.Isfreezed == true)
                {
                    //Add in ET table
                    DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day3Details.PatientID);
                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day3Details.PatientUnitID);
                    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyID);
                    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                    BizActionObj.Day3Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                    DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                    dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day3Details.ID);
                    dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, BizActionObj.Day3Details.OocyteNumber);
                    dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day3Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day3Details.Date);
                    dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day3");
                    dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day3Details.GradeID);
                    dbServer.AddInParameter(command6, "Score", DbType.String, null);
                    dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day3Details.CellStageID);
                    dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                    dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                    dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                    dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                    dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorID);
                    dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorUnitID);
                    int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                }
                if (BizActionObj.Day3Details.NextPlanID == 2 && BizActionObj.Day3Details.Isfreezed == true)
                {
                    //Add in Vitrification table
                    DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day3Details.PatientID);
                    dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day3Details.PatientUnitID);
                    dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyID);
                    dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                    dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                    dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    BizActionObj.Day3Details.ID = (long)dbServer.GetParameterValue(command4, "ID");


                    DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                    dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day3Details.ID);
                    dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, BizActionObj.Day3Details.OocyteNumber);
                    dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day3Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                    dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                    dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day3Details.Date);
                    dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day3");
                    dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day3Details.CellStageID);
                    dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day3Details.GradeID);
                    dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                    dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                    dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                    dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorID);
                    dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorUnitID);

                    dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                    dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                    int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                }

                //added by neena
                if (BizActionObj.Day3Details.OcyteListList != null)
                {
                    foreach (var item in BizActionObj.Day3Details.OcyteListList)
                    {
                        try
                        {
                            DbCommand Sqlcommand12 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day3=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day3Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day3Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + (BizActionObj.Day3Details.SerialOocyteNumber + item.FilterID));
                            int sqlStatus12 = dbServer.ExecuteNonQuery(Sqlcommand12, trans);

                            DbCommand command7 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay3");


                            dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command7, "PatientID", DbType.Int64, BizActionObj.Day3Details.PatientID);
                            dbServer.AddInParameter(command7, "PatientUnitID", DbType.Int64, BizActionObj.Day3Details.PatientUnitID);
                            dbServer.AddInParameter(command7, "PlanTherapyID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyID);
                            dbServer.AddInParameter(command7, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command7, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day3Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command7, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command7, "Date", DbType.DateTime, BizActionObj.Day3Details.Date);
                            dbServer.AddInParameter(command7, "Time", DbType.DateTime, BizActionObj.Day3Details.Time);
                            dbServer.AddInParameter(command7, "EmbryologistID", DbType.Int64, BizActionObj.Day3Details.EmbryologistID);
                            dbServer.AddInParameter(command7, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day3Details.AssitantEmbryologistID);
                            dbServer.AddInParameter(command7, "AnesthetistID", DbType.Int64, BizActionObj.Day3Details.AnesthetistID);
                            dbServer.AddInParameter(command7, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day3Details.AssitantAnesthetistID);
                            dbServer.AddInParameter(command7, "CumulusID", DbType.Int64, BizActionObj.Day3Details.CumulusID);
                            dbServer.AddInParameter(command7, "MOIID", DbType.Int64, BizActionObj.Day3Details.MOIID);
                            dbServer.AddInParameter(command7, "GradeID", DbType.Int64, BizActionObj.Day3Details.GradeID);
                            dbServer.AddInParameter(command7, "CellStageID", DbType.Int64, BizActionObj.Day3Details.CellStageID);
                            dbServer.AddInParameter(command7, "OccDiamension", DbType.String, BizActionObj.Day3Details.OccDiamension);
                            dbServer.AddInParameter(command7, "SpermPreperationMedia", DbType.String, BizActionObj.Day3Details.SpermPreperationMedia);
                            dbServer.AddInParameter(command7, "OocytePreparationMedia", DbType.String, BizActionObj.Day3Details.OocytePreparationMedia);
                            dbServer.AddInParameter(command7, "IncubatorID", DbType.Int64, BizActionObj.Day3Details.IncubatorID);
                            dbServer.AddInParameter(command7, "FinalLayering", DbType.String, BizActionObj.Day3Details.FinalLayering);
                            dbServer.AddInParameter(command7, "NextPlanID", DbType.Int64, BizActionObj.Day3Details.NextPlanID);
                            dbServer.AddInParameter(command7, "Isfreezed", DbType.Boolean, BizActionObj.Day3Details.Isfreezed);
                            dbServer.AddInParameter(command7, "Impression", DbType.String, BizActionObj.Day3Details.Impression); // by bHUSHAn
                            dbServer.AddInParameter(command7, "FrgmentationID", DbType.Int64, BizActionObj.Day3Details.FrgmentationID);
                            dbServer.AddInParameter(command7, "BlastmereSymmetryID", DbType.Int64, BizActionObj.Day3Details.BlastmereSymmetryID);
                            dbServer.AddInParameter(command7, "OtherDetails", DbType.String, BizActionObj.Day3Details.OtherDetails);
                            dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day3Details.ID);
                            dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, int.MaxValue);

                            //by neena
                            dbServer.AddInParameter(command7, "CellObservationDate", DbType.DateTime, BizActionObj.Day3Details.CellObservationDate);
                            dbServer.AddInParameter(command7, "CellObservationTime", DbType.DateTime, BizActionObj.Day3Details.CellObservationTime);
                            //

                            dbServer.AddInParameter(command7, "OocyteDonorID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorID);
                            dbServer.AddInParameter(command7, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorUnitID);

                            dbServer.AddInParameter(command7, "CellStage", DbType.String, BizActionObj.Day3Details.CellStage);
                            dbServer.AddInParameter(command7, "IsBiopsy", DbType.Boolean, BizActionObj.Day3Details.IsBiopsy);
                            dbServer.AddInParameter(command7, "BiopsyDate", DbType.DateTime, BizActionObj.Day3Details.BiopsyDate);
                            dbServer.AddInParameter(command7, "BiopsyTime", DbType.DateTime, BizActionObj.Day3Details.BiopsyTime);
                            dbServer.AddInParameter(command7, "NoOfCell", DbType.Int64, BizActionObj.Day3Details.NoOfCell);
                            dbServer.AddInParameter(command7, "CellNo", DbType.Int64, BizActionObj.Day3Details.CellNo);
                            dbServer.AddInParameter(command7, "IsEmbryoCompacted", DbType.Boolean, BizActionObj.Day3Details.IsEmbryoCompacted);

                            int intStatus7 = dbServer.ExecuteNonQuery(command7, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command7, "ResultStatus");
                            BizActionObj.Day3Details.ID = (long)dbServer.GetParameterValue(command7, "ID");

                            if (BizActionObj.Day3Details.ImgList != null && BizActionObj.Day3Details.ImgList.Count > 0)
                            {
                                foreach (var item1 in BizActionObj.Day3Details.ImgList)
                                {
                                    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day3Details.PatientID);
                                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day3Details.PatientUnitID);
                                    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyID);
                                    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day3Details.SerialOocyteNumber + item.FilterID);
                                    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command1, "Day", DbType.Int64, 3);
                                    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day3Details.ID);
                                    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day3Details.CellStageID);
                                    //dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day0Details.Photo);
                                    //dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day0Details.FileName);
                                    //dbServer.AddInParameter(command1, "Photo", DbType.Binary, item.Photo);
                                    dbServer.AddInParameter(command1, "FileName", DbType.String, item1.ImagePath);
                                    //dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                                    //added by neena
                                    //dbServer.AddInParameter(command1, "SeqNo", DbType.String, item1.SeqNo);
                                    dbServer.AddParameter(command1, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.SeqNo);
                                    dbServer.AddInParameter(command1, "IsApplyTo", DbType.Int32, 1);
                                    dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ServerImageName);

                                    //dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);
                                    //

                                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                                    item1.ID = Convert.ToInt64(dbServer.GetParameterValue(command1, "ID"));
                                    item1.ServerImageName = Convert.ToString(dbServer.GetParameterValue(command1, "ServerImageName"));
                                    item1.SeqNo = Convert.ToString(dbServer.GetParameterValue(command1, "SeqNo"));

                                    //if (item1.Photo != null)
                                    //    File.WriteAllBytes(ImgSaveLocation + item1.ServerImageName, item1.Photo);

                                    //cnt++;
                                }
                            }


                            //if (BizActionObj.Day2Details.Photo != null)
                            //{
                            //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                            //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                            //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                            //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                            //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                            //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber + item.FilterID);
                            //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                            //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 2);
                            //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day2Details.ID);
                            //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day2Details.CellStageID);
                            //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day2Details.Photo);
                            //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day2Details.FileName);
                            //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                            //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                            //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                            //}

                            if (BizActionObj.Day3Details.DetailList != null && BizActionObj.Day3Details.DetailList.Count > 0)
                            {
                                foreach (var item2 in BizActionObj.Day3Details.DetailList)
                                {
                                    DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                                    dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item2.Date);
                                    dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item2.PatientID);
                                    dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item2.PatientUnitID);
                                    dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item2.PlanTherapyID);
                                    dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item2.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command8, "Description", DbType.String, item2.Description);
                                    dbServer.AddInParameter(command8, "Title", DbType.String, item2.Title);
                                    dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item2.AttachedFileName);
                                    dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item2.AttachedFileContent);
                                    dbServer.AddInParameter(command8, "Status", DbType.Boolean, item2.Status);
                                    dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                    dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                    dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                    dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                                    dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day3Details.SerialOocyteNumber + item.FilterID);
                                    dbServer.AddInParameter(command8, "Day", DbType.Int64, item2.Day);
                                    //dbServer.AddInParameter(command8, "DocNo", DbType.String, item2.DocNo);
                                    dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.DocNo);
                                    dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                                    dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 1);
                                    //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                                    int intStatus8 = dbServer.ExecuteNonQuery(command8);
                                    item2.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                                    //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                                }
                            }

                        }

                        catch (Exception ex)
                        {

                        }

                        if (BizActionObj.Day3Details.NextPlanID == 3 && BizActionObj.Day3Details.Isfreezed == true)
                        {
                            DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day4=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day3Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day3Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day3Details.SerialOocyteNumber + item.FilterID);
                            int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);

                            DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay4");

                            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Day3Details.PatientID);
                            dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Day3Details.PatientUnitID);
                            dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyID);
                            dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day3Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command2, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command2, "Time", DbType.DateTime, null);
                            dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AssitantEmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AnesthetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AssitantAnesthetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "CumulusID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "MOIID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "GradeID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "OccDiamension", DbType.String, null);
                            dbServer.AddInParameter(command2, "SpermPreperationMedia", DbType.String, null);
                            dbServer.AddInParameter(command2, "OocytePreparationMedia", DbType.String, null);
                            dbServer.AddInParameter(command2, "IncubatorID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "FinalLayering", DbType.String, null);
                            dbServer.AddInParameter(command2, "NextPlanID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "Isfreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command2, "FrgmentationID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "BlastmereSymmetryID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "OtherDetails", DbType.String, null);
                            dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                            dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorID);
                            dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorUnitID);

                            int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                            //  BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                        }
                        if (BizActionObj.Day3Details.NextPlanID == 4 && BizActionObj.Day3Details.Isfreezed == true)
                        {
                            //Add in ET table
                            DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day3Details.PatientID);
                            dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day3Details.PatientUnitID);
                            dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyID);
                            dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                            BizActionObj.Day3Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                            DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                            dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day3Details.ID);
                            dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day3Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day3Details.Date);
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day3");
                            dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day3Details.GradeID);
                            dbServer.AddInParameter(command6, "Score", DbType.String, null);
                            dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day3Details.CellStageID);
                            dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                            dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                            dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                            dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                            dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorID);
                            dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorUnitID);
                            int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                        }
                        if (BizActionObj.Day3Details.NextPlanID == 2 && BizActionObj.Day3Details.Isfreezed == true)
                        {
                            //Add in Vitrification table
                            DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day3Details.PatientID);
                            dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day3Details.PatientUnitID);
                            dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyID);
                            dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day3Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                            dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                            dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                            BizActionObj.Day3Details.ID = (long)dbServer.GetParameterValue(command4, "ID");


                            DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                            dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day3Details.ID);
                            dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day3Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                            dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                            dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day3Details.Date);
                            dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day3");
                            dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day3Details.CellStageID);
                            dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day3Details.GradeID);
                            dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                            dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                            dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                            dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorID);
                            dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day3Details.OocyteDonorUnitID);

                            dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                            dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                            int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                        }
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Day3Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;
        }
        public override IValueObject GetDay3Details(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay3DetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay3DetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay3Details");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        BizAction.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        BizAction.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        BizAction.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        BizAction.Details.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                        BizAction.Details.Time = (DateTime?)(DALHelper.HandleDate(reader["Time"]));
                        BizAction.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        BizAction.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        BizAction.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        BizAction.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        BizAction.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        BizAction.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        BizAction.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        BizAction.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        BizAction.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        BizAction.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        BizAction.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        BizAction.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        BizAction.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        BizAction.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        BizAction.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        //BizAction.Details.Photo = (byte[])(DALHelper.HandleDBNull(reader["Image"]));
                        BizAction.Details.FrgmentationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrgmentationID"]));
                        BizAction.Details.BlastmereSymmetryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlastmereSymmetryID"]));
                        BizAction.Details.OtherDetails = Convert.ToString(DALHelper.HandleDBNull(reader["OtherDetails"]));
                        //BizAction.Details.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        BizAction.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));// BY BHUSHAN
                        //BizAction.Details.TreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TreatmentID"]));
                        BizAction.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        BizAction.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));

                        //by neena
                        //BizAction.Details.TreatmentStartDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentStartDate"]));
                        //BizAction.Details.TreatmentEndDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentEndDate"]));
                        //BizAction.Details.ObservationDate = (DateTime?)(DALHelper.HandleDate(reader["ObservationDate"]));
                        //BizAction.Details.ObservationTime = (DateTime?)(DALHelper.HandleDate(reader["ObservationTime"]));
                        //BizAction.Details.FertilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertilizationID"]));
                        BizAction.Details.CellObservationDate = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate"]));
                        BizAction.Details.CellObservationTime = (DateTime?)(DALHelper.HandleDate(reader["CellObservationTime"]));
                        BizAction.Details.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        BizAction.Details.IsBiopsy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBiopsy"]));
                        BizAction.Details.BiopsyDate = (DateTime?)(DALHelper.HandleDate(reader["BiopsyDate"]));
                        BizAction.Details.BiopsyTime = (DateTime?)(DALHelper.HandleDate(reader["BiopsyTime"]));
                        BizAction.Details.NoOfCell = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfCell"]));
                        BizAction.Details.CellNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellNo"]));
                        BizAction.Details.IsEmbryoCompacted = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEmbryoCompacted"]));

                        if (BizAction.Details.IsBiopsy == false)
                        {
                            BizAction.Details.BiopsyDate = null;
                            BizAction.Details.BiopsyTime = null;
                        }
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

                        if (!string.IsNullOrEmpty(ObjImg.OriginalImagePath))
                            ObjImg.ServerImageName = "..//" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;
                        //ObjImg.ServerImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;


                        BizAction.Details.ImgList.Add(ObjImg);
                    }
                }

                reader.NextResult();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_TherapyDocumentVO Details = new clsIVFDashboard_TherapyDocumentVO();
                        Details.ID = (long)reader["ID"];
                        Details.UnitID = (long)reader["UnitID"];
                        Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        Details.Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]));
                        Details.Date = (DateTime)(DALHelper.HandleDate(reader["Date"]));
                        Details.Title = Convert.ToString((DALHelper.HandleDBNull(reader["Title"])));
                        Details.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Details.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        Details.AttachedFileContent = (Byte[])(DALHelper.HandleDBNull(reader["AttachedFileContent"]));
                        Details.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));

                        BizAction.Details.DetailList.Add(Details);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        //added by neena
        public override IValueObject GetDay3OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay3OocyteDetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay3OocyteDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay3DetailsForOocyte");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                //dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, 0);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay3OocyteDetailsBizActionVO Oocytedetails = new clsIVFDashboard_GetDay3OocyteDetailsBizActionVO();
                        Oocytedetails.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Oocytedetails.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"]));
                        Oocytedetails.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Oocytelist.Add(Oocytedetails.Details);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }
        #endregion

        #region Day4
        public override IValueObject AddUpdateDay4Details(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_AddUpdateDay4BizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateDay4BizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {

                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.Day4Details.DecisionID == 0)
                {
                    DbCommand Sqlcommand11 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day4=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day4Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day4Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day4Details.SerialOocyteNumber);
                    int sqlStatus11 = dbServer.ExecuteNonQuery(Sqlcommand11, trans);
                }

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay4");


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Day4Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Day4Details.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day4Details.SerialOocyteNumber);
                dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, BizActionObj.Day4Details.OocyteNumber);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.Day4Details.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionObj.Day4Details.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.Day4Details.EmbryologistID);
                dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day4Details.AssitantEmbryologistID);
                dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, BizActionObj.Day4Details.AnesthetistID);
                dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day4Details.AssitantAnesthetistID);
                dbServer.AddInParameter(command, "CumulusID", DbType.Int64, BizActionObj.Day4Details.CumulusID);
                dbServer.AddInParameter(command, "MOIID", DbType.Int64, BizActionObj.Day4Details.MOIID);
                dbServer.AddInParameter(command, "GradeID", DbType.Int64, BizActionObj.Day4Details.GradeID);
                dbServer.AddInParameter(command, "CellStageID", DbType.Int64, BizActionObj.Day4Details.CellStageID);
                dbServer.AddInParameter(command, "OccDiamension", DbType.String, BizActionObj.Day4Details.OccDiamension);
                dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, BizActionObj.Day4Details.SpermPreperationMedia);
                dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, BizActionObj.Day4Details.OocytePreparationMedia);
                dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, BizActionObj.Day4Details.IncubatorID);
                dbServer.AddInParameter(command, "FinalLayering", DbType.String, BizActionObj.Day4Details.FinalLayering);
                dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, BizActionObj.Day4Details.NextPlanID);
                dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, BizActionObj.Day4Details.Isfreezed);
                dbServer.AddInParameter(command, "Impression", DbType.String, BizActionObj.Day4Details.Impression); // by bHUSHAn
                dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, BizActionObj.Day4Details.FrgmentationID);
                dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, BizActionObj.Day4Details.BlastmereSymmetryID);
                dbServer.AddInParameter(command, "OtherDetails", DbType.String, BizActionObj.Day4Details.OtherDetails);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day4Details.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //by neena
                dbServer.AddInParameter(command, "CellObservationDate", DbType.DateTime, BizActionObj.Day4Details.CellObservationDate);
                dbServer.AddInParameter(command, "CellObservationTime", DbType.DateTime, BizActionObj.Day4Details.CellObservationTime);
                //

                dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorID);
                dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorUnitID);

                dbServer.AddInParameter(command, "CellStage", DbType.String, BizActionObj.Day4Details.CellStage);
                dbServer.AddInParameter(command, "IsBiopsy", DbType.Boolean, BizActionObj.Day4Details.IsBiopsy);
                dbServer.AddInParameter(command, "BiopsyDate", DbType.DateTime, BizActionObj.Day4Details.BiopsyDate);
                dbServer.AddInParameter(command, "BiopsyTime", DbType.DateTime, BizActionObj.Day4Details.BiopsyTime);
                dbServer.AddInParameter(command, "NoOfCell", DbType.Int64, BizActionObj.Day4Details.NoOfCell);
                dbServer.AddInParameter(command, "CellNo", DbType.Int64, BizActionObj.Day4Details.CellNo);
                dbServer.AddInParameter(command, "IsAssistedHatching", DbType.Boolean, BizActionObj.Day4Details.AssistedHatching);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Day4Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.Day4Details.ImgList != null && BizActionObj.Day4Details.ImgList.Count > 0)
                {
                    foreach (var item in BizActionObj.Day4Details.ImgList)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day4Details.PatientID);
                        dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day4Details.PatientUnitID);
                        dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyID);
                        dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyUnitID);
                        dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day4Details.SerialOocyteNumber);
                        dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day4Details.OocyteNumber);
                        dbServer.AddInParameter(command1, "Day", DbType.Int64, 4);
                        dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day4Details.ID);
                        dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day4Details.CellStageID);
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
                        //dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
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

                if (BizActionObj.Day4Details.DetailList != null && BizActionObj.Day4Details.DetailList.Count > 0)
                {
                    foreach (var item in BizActionObj.Day4Details.DetailList)
                    {
                        DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                        dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item.Date);
                        dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item.PatientID);
                        dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                        dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item.PlanTherapyID);
                        dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item.PlanTherapyUnitID);
                        dbServer.AddInParameter(command8, "Description", DbType.String, item.Description);
                        dbServer.AddInParameter(command8, "Title", DbType.String, item.Title);
                        dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item.AttachedFileName);
                        dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item.AttachedFileContent);
                        dbServer.AddInParameter(command8, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.OocyteNumber);
                        dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, item.SerialOocyteNumber);
                        dbServer.AddInParameter(command8, "Day", DbType.Int64, item.Day);
                        //dbServer.AddInParameter(command8, "DocNo", DbType.String, item.DocNo);
                        dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 0);
                        if (item.DocNo == null)
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        else
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.DocNo);

                        //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus8 = dbServer.ExecuteNonQuery(command8);
                        item.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                        //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                    }
                }

                //if (BizActionObj.Day4Details.Photo != null)
                //{
                //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day4Details.PatientID);
                //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day4Details.PatientUnitID);
                //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyID);
                //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyUnitID);
                //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day4Details.SerialOocyteNumber);
                //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day4Details.OocyteNumber);
                //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 4);
                //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day4Details.ID);
                //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day4Details.CellStageID);
                //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day4Details.Photo);
                //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day4Details.FileName);
                //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                //}


                if (BizActionObj.Day4Details.NextPlanID == 3 && BizActionObj.Day4Details.Isfreezed == true)
                {
                    DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day5=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day4Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day4Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day4Details.SerialOocyteNumber);
                    int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);

                    DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay5");

                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Day4Details.PatientID);
                    dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Day4Details.PatientUnitID);
                    dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyID);
                    dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day4Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, BizActionObj.Day4Details.OocyteNumber);
                    dbServer.AddInParameter(command2, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command2, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AnesthetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AssitantAnesthetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "CumulusID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "MOIID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "GradeID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "OccDiamension", DbType.String, null);
                    dbServer.AddInParameter(command2, "SpermPreperationMedia", DbType.String, null);
                    dbServer.AddInParameter(command2, "OocytePreparationMedia", DbType.String, null);
                    dbServer.AddInParameter(command2, "IncubatorID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "FinalLayering", DbType.String, null);
                    dbServer.AddInParameter(command2, "NextPlanID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "Isfreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command2, "FrgmentationID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "BlastmereSymmetryID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "OtherDetails", DbType.String, null);
                    dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                    dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorID);
                    dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorUnitID);

                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                    //  BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                }
                if (BizActionObj.Day4Details.NextPlanID == 4 && BizActionObj.Day4Details.Isfreezed == true)
                {
                    //Add in ET table
                    DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day4Details.PatientID);
                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day4Details.PatientUnitID);
                    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyID);
                    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                    BizActionObj.Day4Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                    DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                    dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day4Details.ID);
                    dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, BizActionObj.Day4Details.OocyteNumber);
                    dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day4Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day4Details.Date);
                    dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day4");
                    dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day4Details.GradeID);
                    dbServer.AddInParameter(command6, "Score", DbType.String, null);
                    dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day4Details.CellStageID);
                    dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                    dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                    dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                    dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                    dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorID);
                    dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorUnitID);
                    int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                }
                if (BizActionObj.Day4Details.NextPlanID == 2 && BizActionObj.Day4Details.Isfreezed == true)
                {
                    //Add in Vitrification table
                    DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day4Details.PatientID);
                    dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day4Details.PatientUnitID);
                    dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyID);
                    dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                    dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                    dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    BizActionObj.Day4Details.ID = (long)dbServer.GetParameterValue(command4, "ID");


                    DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                    dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day4Details.ID);
                    dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, BizActionObj.Day4Details.OocyteNumber);
                    dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day4Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                    dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                    dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day4Details.Date);
                    dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day4");
                    dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day4Details.CellStageID);
                    dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day4Details.GradeID);
                    dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                    dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                    dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                    dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorID);
                    dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorUnitID);

                    dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                    dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                    int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                }

                //added by neena
                if (BizActionObj.Day4Details.OcyteListList != null)
                {
                    foreach (var item in BizActionObj.Day4Details.OcyteListList)
                    {
                        try
                        {
                            DbCommand Sqlcommand12 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day4=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day4Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day4Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + (BizActionObj.Day4Details.SerialOocyteNumber + item.FilterID));
                            int sqlStatus12 = dbServer.ExecuteNonQuery(Sqlcommand12, trans);

                            DbCommand command7 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay4");


                            dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command7, "PatientID", DbType.Int64, BizActionObj.Day4Details.PatientID);
                            dbServer.AddInParameter(command7, "PatientUnitID", DbType.Int64, BizActionObj.Day4Details.PatientUnitID);
                            dbServer.AddInParameter(command7, "PlanTherapyID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyID);
                            dbServer.AddInParameter(command7, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command7, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day4Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command7, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command7, "Date", DbType.DateTime, BizActionObj.Day4Details.Date);
                            dbServer.AddInParameter(command7, "Time", DbType.DateTime, BizActionObj.Day4Details.Time);
                            dbServer.AddInParameter(command7, "EmbryologistID", DbType.Int64, BizActionObj.Day4Details.EmbryologistID);
                            dbServer.AddInParameter(command7, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day4Details.AssitantEmbryologistID);
                            dbServer.AddInParameter(command7, "AnesthetistID", DbType.Int64, BizActionObj.Day4Details.AnesthetistID);
                            dbServer.AddInParameter(command7, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day4Details.AssitantAnesthetistID);
                            dbServer.AddInParameter(command7, "CumulusID", DbType.Int64, BizActionObj.Day4Details.CumulusID);
                            dbServer.AddInParameter(command7, "MOIID", DbType.Int64, BizActionObj.Day4Details.MOIID);
                            dbServer.AddInParameter(command7, "GradeID", DbType.Int64, BizActionObj.Day4Details.GradeID);
                            dbServer.AddInParameter(command7, "CellStageID", DbType.Int64, BizActionObj.Day4Details.CellStageID);
                            dbServer.AddInParameter(command7, "OccDiamension", DbType.String, BizActionObj.Day4Details.OccDiamension);
                            dbServer.AddInParameter(command7, "SpermPreperationMedia", DbType.String, BizActionObj.Day4Details.SpermPreperationMedia);
                            dbServer.AddInParameter(command7, "OocytePreparationMedia", DbType.String, BizActionObj.Day4Details.OocytePreparationMedia);
                            dbServer.AddInParameter(command7, "IncubatorID", DbType.Int64, BizActionObj.Day4Details.IncubatorID);
                            dbServer.AddInParameter(command7, "FinalLayering", DbType.String, BizActionObj.Day4Details.FinalLayering);
                            dbServer.AddInParameter(command7, "NextPlanID", DbType.Int64, BizActionObj.Day4Details.NextPlanID);
                            dbServer.AddInParameter(command7, "Isfreezed", DbType.Boolean, BizActionObj.Day4Details.Isfreezed);
                            dbServer.AddInParameter(command7, "Impression", DbType.String, BizActionObj.Day4Details.Impression); // by bHUSHAn
                            dbServer.AddInParameter(command7, "FrgmentationID", DbType.Int64, BizActionObj.Day4Details.FrgmentationID);
                            dbServer.AddInParameter(command7, "BlastmereSymmetryID", DbType.Int64, BizActionObj.Day4Details.BlastmereSymmetryID);
                            dbServer.AddInParameter(command7, "OtherDetails", DbType.String, BizActionObj.Day4Details.OtherDetails);
                            dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day4Details.ID);
                            dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, int.MaxValue);

                            //by neena
                            dbServer.AddInParameter(command7, "CellObservationDate", DbType.DateTime, BizActionObj.Day4Details.CellObservationDate);
                            dbServer.AddInParameter(command7, "CellObservationTime", DbType.DateTime, BizActionObj.Day4Details.CellObservationTime);
                            //

                            dbServer.AddInParameter(command7, "OocyteDonorID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorID);
                            dbServer.AddInParameter(command7, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorUnitID);

                            dbServer.AddInParameter(command7, "CellStage", DbType.String, BizActionObj.Day4Details.CellStage);
                            dbServer.AddInParameter(command7, "IsBiopsy", DbType.Boolean, BizActionObj.Day4Details.IsBiopsy);
                            dbServer.AddInParameter(command7, "BiopsyDate", DbType.DateTime, BizActionObj.Day4Details.BiopsyDate);
                            dbServer.AddInParameter(command7, "BiopsyTime", DbType.DateTime, BizActionObj.Day4Details.BiopsyTime);
                            dbServer.AddInParameter(command7, "NoOfCell", DbType.Int64, BizActionObj.Day4Details.NoOfCell);
                            dbServer.AddInParameter(command7, "CellNo", DbType.Int64, BizActionObj.Day4Details.CellNo);
                            dbServer.AddInParameter(command7, "IsAssistedHatching", DbType.Boolean, BizActionObj.Day4Details.AssistedHatching);

                            int intStatus7 = dbServer.ExecuteNonQuery(command7, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command7, "ResultStatus");
                            BizActionObj.Day4Details.ID = (long)dbServer.GetParameterValue(command7, "ID");

                            if (BizActionObj.Day4Details.ImgList != null && BizActionObj.Day4Details.ImgList.Count > 0)
                            {
                                foreach (var item1 in BizActionObj.Day4Details.ImgList)
                                {
                                    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day4Details.PatientID);
                                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day4Details.PatientUnitID);
                                    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyID);
                                    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day4Details.SerialOocyteNumber + item.FilterID);
                                    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command1, "Day", DbType.Int64, 4);
                                    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day4Details.ID);
                                    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day4Details.CellStageID);
                                    //dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day0Details.Photo);
                                    //dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day0Details.FileName);
                                    //dbServer.AddInParameter(command1, "Photo", DbType.Binary, item.Photo);
                                    dbServer.AddInParameter(command1, "FileName", DbType.String, item1.ImagePath);
                                    //dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                                    //added by neena
                                    //dbServer.AddInParameter(command1, "SeqNo", DbType.String, item1.SeqNo);
                                    dbServer.AddParameter(command1, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.SeqNo);
                                    dbServer.AddInParameter(command1, "IsApplyTo", DbType.Int32, 1);
                                    dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ServerImageName);

                                    //dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);
                                    //

                                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                                    item1.ID = Convert.ToInt64(dbServer.GetParameterValue(command1, "ID"));
                                    item1.ServerImageName = Convert.ToString(dbServer.GetParameterValue(command1, "ServerImageName"));
                                    item1.SeqNo = Convert.ToString(dbServer.GetParameterValue(command1, "SeqNo"));

                                    //if (item1.Photo != null)
                                    //    File.WriteAllBytes(ImgSaveLocation + item1.ServerImageName, item1.Photo);

                                    //cnt++;
                                }
                            }


                            //if (BizActionObj.Day2Details.Photo != null)
                            //{
                            //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                            //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                            //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                            //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                            //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                            //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber + item.FilterID);
                            //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                            //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 2);
                            //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day2Details.ID);
                            //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day2Details.CellStageID);
                            //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day2Details.Photo);
                            //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day2Details.FileName);
                            //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                            //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                            //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                            //}

                            if (BizActionObj.Day4Details.DetailList != null && BizActionObj.Day4Details.DetailList.Count > 0)
                            {
                                foreach (var item2 in BizActionObj.Day4Details.DetailList)
                                {
                                    DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                                    dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item2.Date);
                                    dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item2.PatientID);
                                    dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item2.PatientUnitID);
                                    dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item2.PlanTherapyID);
                                    dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item2.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command8, "Description", DbType.String, item2.Description);
                                    dbServer.AddInParameter(command8, "Title", DbType.String, item2.Title);
                                    dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item2.AttachedFileName);
                                    dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item2.AttachedFileContent);
                                    dbServer.AddInParameter(command8, "Status", DbType.Boolean, item2.Status);
                                    dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                    dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                    dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                    dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                                    dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day4Details.SerialOocyteNumber + item.FilterID);
                                    dbServer.AddInParameter(command8, "Day", DbType.Int64, item2.Day);
                                    //dbServer.AddInParameter(command8, "DocNo", DbType.String, item2.DocNo);
                                    dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.DocNo);
                                    dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                                    dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 1);
                                    //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                                    int intStatus8 = dbServer.ExecuteNonQuery(command8);
                                    item2.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                                    //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        if (BizActionObj.Day4Details.NextPlanID == 3 && BizActionObj.Day4Details.Isfreezed == true)
                        {
                            DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day5=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day4Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day4Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day4Details.SerialOocyteNumber + item.FilterID);
                            int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);

                            DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay5");

                            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Day4Details.PatientID);
                            dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Day4Details.PatientUnitID);
                            dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyID);
                            dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day4Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command2, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command2, "Time", DbType.DateTime, null);
                            dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AssitantEmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AnesthetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AssitantAnesthetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "CumulusID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "MOIID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "GradeID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "OccDiamension", DbType.String, null);
                            dbServer.AddInParameter(command2, "SpermPreperationMedia", DbType.String, null);
                            dbServer.AddInParameter(command2, "OocytePreparationMedia", DbType.String, null);
                            dbServer.AddInParameter(command2, "IncubatorID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "FinalLayering", DbType.String, null);
                            dbServer.AddInParameter(command2, "NextPlanID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "Isfreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command2, "FrgmentationID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "BlastmereSymmetryID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "OtherDetails", DbType.String, null);
                            dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                            dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorID);
                            dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorUnitID);

                            int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                            //  BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                        }
                        if (BizActionObj.Day4Details.NextPlanID == 4 && BizActionObj.Day4Details.Isfreezed == true)
                        {
                            //Add in ET table
                            DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day4Details.PatientID);
                            dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day4Details.PatientUnitID);
                            dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyID);
                            dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                            BizActionObj.Day4Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                            DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                            dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day4Details.ID);
                            dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day4Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day4Details.Date);
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day4");
                            dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day4Details.GradeID);
                            dbServer.AddInParameter(command6, "Score", DbType.String, null);
                            dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day4Details.CellStageID);
                            dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                            dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                            dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                            dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                            dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorID);
                            dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorUnitID);
                            int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                        }
                        if (BizActionObj.Day4Details.NextPlanID == 2 && BizActionObj.Day4Details.Isfreezed == true)
                        {
                            //Add in Vitrification table
                            DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day4Details.PatientID);
                            dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day4Details.PatientUnitID);
                            dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyID);
                            dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day4Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                            dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                            dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                            BizActionObj.Day4Details.ID = (long)dbServer.GetParameterValue(command4, "ID");


                            DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                            dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day4Details.ID);
                            dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day4Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                            dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                            dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day4Details.Date);
                            dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day4");
                            dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day4Details.CellStageID);
                            dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day4Details.GradeID);
                            dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                            dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                            dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                            dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorID);
                            dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day4Details.OocyteDonorUnitID);

                            dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                            dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                            int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                        }
                    }
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Day4Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;
        }
        public override IValueObject GetDay4Details(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay4DetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay4DetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay4Details");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        BizAction.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        BizAction.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        BizAction.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        BizAction.Details.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                        BizAction.Details.Time = (DateTime?)(DALHelper.HandleDate(reader["Time"]));
                        BizAction.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        BizAction.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        BizAction.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        BizAction.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        BizAction.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        BizAction.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        BizAction.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        BizAction.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        BizAction.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        BizAction.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        BizAction.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        BizAction.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        BizAction.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        BizAction.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        BizAction.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        //BizAction.Details.Photo = (byte[])(DALHelper.HandleDBNull(reader["Image"]));
                        BizAction.Details.FrgmentationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrgmentationID"]));
                        BizAction.Details.BlastmereSymmetryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlastmereSymmetryID"]));
                        BizAction.Details.OtherDetails = Convert.ToString(DALHelper.HandleDBNull(reader["OtherDetails"]));
                        //BizAction.Details.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        BizAction.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));// BY BHUSHAN
                        //BizAction.Details.TreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TreatmentID"]));
                        BizAction.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        BizAction.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));



                        //by neena
                        //BizAction.Details.TreatmentStartDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentStartDate"]));
                        //BizAction.Details.TreatmentEndDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentEndDate"]));
                        //BizAction.Details.ObservationDate = (DateTime?)(DALHelper.HandleDate(reader["ObservationDate"]));
                        //BizAction.Details.ObservationTime = (DateTime?)(DALHelper.HandleDate(reader["ObservationTime"]));
                        //BizAction.Details.FertilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertilizationID"]));
                        BizAction.Details.CellObservationDate = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate"]));
                        BizAction.Details.CellObservationTime = (DateTime?)(DALHelper.HandleDate(reader["CellObservationTime"]));
                        BizAction.Details.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        BizAction.Details.IsBiopsy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBiopsy"]));
                        BizAction.Details.BiopsyDate = (DateTime?)(DALHelper.HandleDate(reader["BiopsyDate"]));
                        BizAction.Details.BiopsyTime = (DateTime?)(DALHelper.HandleDate(reader["BiopsyTime"]));
                        BizAction.Details.NoOfCell = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfCell"]));
                        BizAction.Details.CellNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellNo"]));
                        BizAction.Details.AssistedHatching = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAssistedHatching"]));
                        if (BizAction.Details.IsBiopsy == false)
                        {
                            BizAction.Details.BiopsyDate = null;
                            BizAction.Details.BiopsyTime = null;
                        }
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

                        if (!string.IsNullOrEmpty(ObjImg.OriginalImagePath))
                            ObjImg.ServerImageName = "..//" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;
                        //ObjImg.ServerImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;


                        BizAction.Details.ImgList.Add(ObjImg);
                    }
                }

                reader.NextResult();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_TherapyDocumentVO Details = new clsIVFDashboard_TherapyDocumentVO();
                        Details.ID = (long)reader["ID"];
                        Details.UnitID = (long)reader["UnitID"];
                        Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        Details.Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]));
                        Details.Date = (DateTime)(DALHelper.HandleDate(reader["Date"]));
                        Details.Title = Convert.ToString((DALHelper.HandleDBNull(reader["Title"])));
                        Details.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Details.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        Details.AttachedFileContent = (Byte[])(DALHelper.HandleDBNull(reader["AttachedFileContent"]));
                        Details.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));

                        BizAction.Details.DetailList.Add(Details);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        //added by neena
        public override IValueObject GetDay4OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay4OocyteDetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay4OocyteDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay4DetailsForOocyte");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                //dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, 0);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay4OocyteDetailsBizActionVO Oocytedetails = new clsIVFDashboard_GetDay4OocyteDetailsBizActionVO();
                        Oocytedetails.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Oocytedetails.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"]));
                        Oocytedetails.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Oocytelist.Add(Oocytedetails.Details);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }
        #endregion

        #region Day5
        public override IValueObject AddUpdateDay5Details(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_AddUpdateDay5BizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateDay5BizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {

                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.Day5Details.DecisionID == 0)
                {
                    DbCommand Sqlcommand11 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day5=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day5Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day5Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day5Details.SerialOocyteNumber);
                    int sqlStatus11 = dbServer.ExecuteNonQuery(Sqlcommand11, trans);
                }

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay5");


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Day5Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Day5Details.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day5Details.SerialOocyteNumber);
                dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, BizActionObj.Day5Details.OocyteNumber);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.Day5Details.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionObj.Day5Details.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.Day5Details.EmbryologistID);
                dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day5Details.AssitantEmbryologistID);
                dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, BizActionObj.Day5Details.AnesthetistID);
                dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day5Details.AssitantAnesthetistID);
                dbServer.AddInParameter(command, "CumulusID", DbType.Int64, BizActionObj.Day5Details.CumulusID);
                dbServer.AddInParameter(command, "MOIID", DbType.Int64, BizActionObj.Day5Details.MOIID);
                dbServer.AddInParameter(command, "GradeID", DbType.Int64, BizActionObj.Day5Details.GradeID);
                dbServer.AddInParameter(command, "CellStageID", DbType.Int64, BizActionObj.Day5Details.CellStageID);
                dbServer.AddInParameter(command, "OccDiamension", DbType.String, BizActionObj.Day5Details.OccDiamension);
                dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, BizActionObj.Day5Details.SpermPreperationMedia);
                dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, BizActionObj.Day5Details.OocytePreparationMedia);
                dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, BizActionObj.Day5Details.IncubatorID);
                dbServer.AddInParameter(command, "FinalLayering", DbType.String, BizActionObj.Day5Details.FinalLayering);
                dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, BizActionObj.Day5Details.NextPlanID);
                dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, BizActionObj.Day5Details.Isfreezed);
                dbServer.AddInParameter(command, "Impression", DbType.String, BizActionObj.Day5Details.Impression); // by bHUSHAn
                dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, BizActionObj.Day5Details.FrgmentationID);
                dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, BizActionObj.Day5Details.BlastmereSymmetryID);
                dbServer.AddInParameter(command, "OtherDetails", DbType.String, BizActionObj.Day5Details.OtherDetails);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day5Details.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //by neena
                dbServer.AddInParameter(command, "CellObservationDate", DbType.DateTime, BizActionObj.Day5Details.CellObservationDate);
                dbServer.AddInParameter(command, "CellObservationTime", DbType.DateTime, BizActionObj.Day5Details.CellObservationTime);
                dbServer.AddInParameter(command, "StageofDevelopmentGrade", DbType.Int64, BizActionObj.Day5Details.StageofDevelopmentGrade);
                dbServer.AddInParameter(command, "InnerCellMassGrade", DbType.Int64, BizActionObj.Day5Details.InnerCellMassGrade);
                dbServer.AddInParameter(command, "TrophoectodermGrade", DbType.Int64, BizActionObj.Day5Details.TrophoectodermGrade);

                dbServer.AddInParameter(command, "CellStage", DbType.String, BizActionObj.Day5Details.CellStage);
                dbServer.AddInParameter(command, "IsBiopsy", DbType.Boolean, BizActionObj.Day5Details.IsBiopsy);
                dbServer.AddInParameter(command, "BiopsyDate", DbType.DateTime, BizActionObj.Day5Details.BiopsyDate);
                dbServer.AddInParameter(command, "BiopsyTime", DbType.DateTime, BizActionObj.Day5Details.BiopsyTime);
                dbServer.AddInParameter(command, "NoOfCell", DbType.Int64, BizActionObj.Day5Details.NoOfCell);
                dbServer.AddInParameter(command, "IsAssistedHatching", DbType.Boolean, BizActionObj.Day5Details.AssistedHatching);
                //

                dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorID);
                dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorUnitID);
                dbServer.AddInParameter(command, "CellNo", DbType.Int64, BizActionObj.Day5Details.CellNo);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Day5Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.Day5Details.ImgList != null && BizActionObj.Day5Details.ImgList.Count > 0)
                {
                    foreach (var item in BizActionObj.Day5Details.ImgList)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day5Details.PatientID);
                        dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day5Details.PatientUnitID);
                        dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyID);
                        dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyUnitID);
                        dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day5Details.SerialOocyteNumber);
                        dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day5Details.OocyteNumber);
                        dbServer.AddInParameter(command1, "Day", DbType.Int64, 5);
                        dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day5Details.ID);
                        dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day5Details.CellStageID);
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
                        //dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
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

                if (BizActionObj.Day5Details.DetailList != null && BizActionObj.Day5Details.DetailList.Count > 0)
                {
                    foreach (var item in BizActionObj.Day5Details.DetailList)
                    {
                        DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                        dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item.Date);
                        dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item.PatientID);
                        dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                        dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item.PlanTherapyID);
                        dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item.PlanTherapyUnitID);
                        dbServer.AddInParameter(command8, "Description", DbType.String, item.Description);
                        dbServer.AddInParameter(command8, "Title", DbType.String, item.Title);
                        dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item.AttachedFileName);
                        dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item.AttachedFileContent);
                        dbServer.AddInParameter(command8, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.OocyteNumber);
                        dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, item.SerialOocyteNumber);
                        dbServer.AddInParameter(command8, "Day", DbType.Int64, item.Day);
                        //dbServer.AddInParameter(command8, "DocNo", DbType.String, item.DocNo);
                        dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 0);
                        if (item.DocNo == null)
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        else
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.DocNo);

                        //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus8 = dbServer.ExecuteNonQuery(command8);
                        item.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                        //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                    }
                }

                //if (BizActionObj.Day5Details.Photo != null)
                //{
                //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day5Details.PatientID);
                //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day5Details.PatientUnitID);
                //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyID);
                //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyUnitID);
                //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day5Details.SerialOocyteNumber);
                //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day5Details.OocyteNumber);
                //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 5);
                //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day5Details.ID);
                //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day5Details.CellStageID);
                //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day5Details.Photo);
                //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day5Details.FileName);
                //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                //}
                if (BizActionObj.Day5Details.NextPlanID == 3 && BizActionObj.Day5Details.Isfreezed == true)
                {
                    DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day6=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day5Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day5Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day5Details.SerialOocyteNumber);
                    int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);

                    DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay6");

                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Day5Details.PatientID);
                    dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Day5Details.PatientUnitID);
                    dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyID);
                    dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day5Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, BizActionObj.Day5Details.OocyteNumber);
                    dbServer.AddInParameter(command2, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command2, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AnesthetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "AssitantAnesthetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "CumulusID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "MOIID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "GradeID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "OccDiamension", DbType.String, null);
                    dbServer.AddInParameter(command2, "SpermPreperationMedia", DbType.String, null);
                    dbServer.AddInParameter(command2, "OocytePreparationMedia", DbType.String, null);
                    dbServer.AddInParameter(command2, "IncubatorID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "FinalLayering", DbType.String, null);
                    dbServer.AddInParameter(command2, "NextPlanID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "Isfreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command2, "FrgmentationID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "BlastmereSymmetryID", DbType.Int64, null);
                    dbServer.AddInParameter(command2, "OtherDetails", DbType.String, null);
                    dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorID);
                    dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorUnitID);


                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                    //  BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                }
                if (BizActionObj.Day5Details.NextPlanID == 4 && BizActionObj.Day5Details.Isfreezed == true)
                {
                    //Add in ET table
                    DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day5Details.PatientID);
                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day5Details.PatientUnitID);
                    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyID);
                    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                    BizActionObj.Day5Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                    DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                    dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day5Details.ID);
                    dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, BizActionObj.Day5Details.OocyteNumber);
                    dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day5Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day5Details.Date);
                    dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day5");
                    dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day5Details.GradeID);
                    dbServer.AddInParameter(command6, "Score", DbType.String, null);
                    dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day5Details.CellStageID);
                    dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                    dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                    dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                    dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                    dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorID);
                    dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorUnitID);
                    int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                }
                if (BizActionObj.Day5Details.NextPlanID == 2 && BizActionObj.Day5Details.Isfreezed == true)
                {
                    //Add in Vitrification table
                    DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day5Details.PatientID);
                    dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day5Details.PatientUnitID);
                    dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyID);
                    dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                    dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                    dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    BizActionObj.Day5Details.ID = (long)dbServer.GetParameterValue(command4, "ID");


                    DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                    dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day5Details.ID);
                    dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, BizActionObj.Day5Details.OocyteNumber);
                    dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day5Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                    dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                    dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day5Details.Date);
                    dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day5");
                    dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day5Details.CellStageID);
                    dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day5Details.GradeID);
                    dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                    dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                    dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                    dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorID);
                    dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorUnitID);

                    dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                    dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                    int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                }

                //added by neena
                if (BizActionObj.Day5Details.OcyteListList != null)
                {
                    foreach (var item in BizActionObj.Day5Details.OcyteListList)
                    {
                        try
                        {
                            DbCommand Sqlcommand12 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day5=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day5Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day5Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + (BizActionObj.Day5Details.SerialOocyteNumber + item.FilterID));
                            int sqlStatus12 = dbServer.ExecuteNonQuery(Sqlcommand12, trans);

                            DbCommand command7 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay5");


                            dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command7, "PatientID", DbType.Int64, BizActionObj.Day5Details.PatientID);
                            dbServer.AddInParameter(command7, "PatientUnitID", DbType.Int64, BizActionObj.Day5Details.PatientUnitID);
                            dbServer.AddInParameter(command7, "PlanTherapyID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyID);
                            dbServer.AddInParameter(command7, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command7, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day5Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command7, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command7, "Date", DbType.DateTime, BizActionObj.Day5Details.Date);
                            dbServer.AddInParameter(command7, "Time", DbType.DateTime, BizActionObj.Day5Details.Time);
                            dbServer.AddInParameter(command7, "EmbryologistID", DbType.Int64, BizActionObj.Day5Details.EmbryologistID);
                            dbServer.AddInParameter(command7, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day5Details.AssitantEmbryologistID);
                            dbServer.AddInParameter(command7, "AnesthetistID", DbType.Int64, BizActionObj.Day5Details.AnesthetistID);
                            dbServer.AddInParameter(command7, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day5Details.AssitantAnesthetistID);
                            dbServer.AddInParameter(command7, "CumulusID", DbType.Int64, BizActionObj.Day5Details.CumulusID);
                            dbServer.AddInParameter(command7, "MOIID", DbType.Int64, BizActionObj.Day5Details.MOIID);
                            dbServer.AddInParameter(command7, "GradeID", DbType.Int64, BizActionObj.Day5Details.GradeID);
                            dbServer.AddInParameter(command7, "CellStageID", DbType.Int64, BizActionObj.Day5Details.CellStageID);
                            dbServer.AddInParameter(command7, "OccDiamension", DbType.String, BizActionObj.Day5Details.OccDiamension);
                            dbServer.AddInParameter(command7, "SpermPreperationMedia", DbType.String, BizActionObj.Day5Details.SpermPreperationMedia);
                            dbServer.AddInParameter(command7, "OocytePreparationMedia", DbType.String, BizActionObj.Day5Details.OocytePreparationMedia);
                            dbServer.AddInParameter(command7, "IncubatorID", DbType.Int64, BizActionObj.Day5Details.IncubatorID);
                            dbServer.AddInParameter(command7, "FinalLayering", DbType.String, BizActionObj.Day5Details.FinalLayering);
                            dbServer.AddInParameter(command7, "NextPlanID", DbType.Int64, BizActionObj.Day5Details.NextPlanID);
                            dbServer.AddInParameter(command7, "Isfreezed", DbType.Boolean, BizActionObj.Day5Details.Isfreezed);
                            dbServer.AddInParameter(command7, "Impression", DbType.String, BizActionObj.Day5Details.Impression); // by bHUSHAn
                            dbServer.AddInParameter(command7, "FrgmentationID", DbType.Int64, BizActionObj.Day5Details.FrgmentationID);
                            dbServer.AddInParameter(command7, "BlastmereSymmetryID", DbType.Int64, BizActionObj.Day5Details.BlastmereSymmetryID);
                            dbServer.AddInParameter(command7, "OtherDetails", DbType.String, BizActionObj.Day5Details.OtherDetails);
                            dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day5Details.ID);
                            dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, int.MaxValue);

                            //by neena
                            dbServer.AddInParameter(command7, "CellObservationDate", DbType.DateTime, BizActionObj.Day5Details.CellObservationDate);
                            dbServer.AddInParameter(command7, "CellObservationTime", DbType.DateTime, BizActionObj.Day5Details.CellObservationTime);
                            dbServer.AddInParameter(command7, "StageofDevelopmentGrade", DbType.Int64, BizActionObj.Day5Details.StageofDevelopmentGrade);
                            dbServer.AddInParameter(command7, "InnerCellMassGrade", DbType.Int64, BizActionObj.Day5Details.InnerCellMassGrade);
                            dbServer.AddInParameter(command7, "TrophoectodermGrade", DbType.Int64, BizActionObj.Day5Details.TrophoectodermGrade);

                            dbServer.AddInParameter(command7, "CellStage", DbType.String, BizActionObj.Day5Details.CellStage);
                            dbServer.AddInParameter(command7, "IsBiopsy", DbType.Boolean, BizActionObj.Day5Details.IsBiopsy);
                            dbServer.AddInParameter(command7, "BiopsyDate", DbType.DateTime, BizActionObj.Day5Details.BiopsyDate);
                            dbServer.AddInParameter(command7, "BiopsyTime", DbType.DateTime, BizActionObj.Day5Details.BiopsyTime);
                            dbServer.AddInParameter(command7, "NoOfCell", DbType.Int64, BizActionObj.Day5Details.NoOfCell);
                            dbServer.AddInParameter(command7, "IsAssistedHatching", DbType.Boolean, BizActionObj.Day5Details.AssistedHatching);
                            dbServer.AddInParameter(command7, "CellNo", DbType.Int64, BizActionObj.Day5Details.CellNo);
                            //

                            dbServer.AddInParameter(command7, "OocyteDonorID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorID);
                            dbServer.AddInParameter(command7, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorUnitID);

                            int intStatus7 = dbServer.ExecuteNonQuery(command7, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command7, "ResultStatus");
                            BizActionObj.Day5Details.ID = (long)dbServer.GetParameterValue(command7, "ID");

                            if (BizActionObj.Day5Details.ImgList != null && BizActionObj.Day5Details.ImgList.Count > 0)
                            {
                                foreach (var item1 in BizActionObj.Day5Details.ImgList)
                                {
                                    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day5Details.PatientID);
                                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day5Details.PatientUnitID);
                                    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyID);
                                    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day5Details.SerialOocyteNumber + item.FilterID);
                                    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command1, "Day", DbType.Int64, 5);
                                    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day5Details.ID);
                                    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day5Details.CellStageID);
                                    //dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day0Details.Photo);
                                    //dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day0Details.FileName);
                                    //dbServer.AddInParameter(command1, "Photo", DbType.Binary, item.Photo);
                                    dbServer.AddInParameter(command1, "FileName", DbType.String, item1.ImagePath);
                                    //dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                                    //added by neena
                                    //dbServer.AddInParameter(command1, "SeqNo", DbType.String, item1.SeqNo);
                                    dbServer.AddParameter(command1, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.SeqNo);
                                    dbServer.AddInParameter(command1, "IsApplyTo", DbType.Int32, 1);
                                    dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ServerImageName);

                                    //dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);
                                    //

                                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                                    item1.ID = Convert.ToInt64(dbServer.GetParameterValue(command1, "ID"));
                                    item1.ServerImageName = Convert.ToString(dbServer.GetParameterValue(command1, "ServerImageName"));
                                    item1.SeqNo = Convert.ToString(dbServer.GetParameterValue(command1, "SeqNo"));

                                    //if (item1.Photo != null)
                                    //    File.WriteAllBytes(ImgSaveLocation + item1.ServerImageName, item1.Photo);

                                    //cnt++;
                                }
                            }


                            //if (BizActionObj.Day2Details.Photo != null)
                            //{
                            //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                            //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                            //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                            //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                            //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                            //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber + item.FilterID);
                            //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                            //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 2);
                            //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day2Details.ID);
                            //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day2Details.CellStageID);
                            //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day2Details.Photo);
                            //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day2Details.FileName);
                            //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                            //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                            //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                            //}

                            if (BizActionObj.Day5Details.DetailList != null && BizActionObj.Day5Details.DetailList.Count > 0)
                            {
                                foreach (var item2 in BizActionObj.Day5Details.DetailList)
                                {
                                    DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                                    dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item2.Date);
                                    dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item2.PatientID);
                                    dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item2.PatientUnitID);
                                    dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item2.PlanTherapyID);
                                    dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item2.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command8, "Description", DbType.String, item2.Description);
                                    dbServer.AddInParameter(command8, "Title", DbType.String, item2.Title);
                                    dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item2.AttachedFileName);
                                    dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item2.AttachedFileContent);
                                    dbServer.AddInParameter(command8, "Status", DbType.Boolean, item2.Status);
                                    dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                    dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                    dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                    dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                                    dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day5Details.SerialOocyteNumber + item.FilterID);
                                    dbServer.AddInParameter(command8, "Day", DbType.Int64, item2.Day);
                                    //dbServer.AddInParameter(command8, "DocNo", DbType.String, item2.DocNo);
                                    dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.DocNo);
                                    dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                                    dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 1);
                                    //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                                    int intStatus8 = dbServer.ExecuteNonQuery(command8);
                                    item2.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                                    //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        if (BizActionObj.Day5Details.NextPlanID == 3 && BizActionObj.Day5Details.Isfreezed == true)
                        {
                            DbCommand Sqlcommand1 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day6=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day5Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day5Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day5Details.SerialOocyteNumber + item.FilterID);
                            int sqlStatus1 = dbServer.ExecuteNonQuery(Sqlcommand1, trans);

                            DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay6");

                            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Day5Details.PatientID);
                            dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Day5Details.PatientUnitID);
                            dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyID);
                            dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day5Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command2, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command2, "Time", DbType.DateTime, null);
                            dbServer.AddInParameter(command2, "EmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AssitantEmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AnesthetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "AssitantAnesthetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "CumulusID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "MOIID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "GradeID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "OccDiamension", DbType.String, null);
                            dbServer.AddInParameter(command2, "SpermPreperationMedia", DbType.String, null);
                            dbServer.AddInParameter(command2, "OocytePreparationMedia", DbType.String, null);
                            dbServer.AddInParameter(command2, "IncubatorID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "FinalLayering", DbType.String, null);
                            dbServer.AddInParameter(command2, "NextPlanID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "Isfreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command2, "FrgmentationID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "BlastmereSymmetryID", DbType.Int64, null);
                            dbServer.AddInParameter(command2, "OtherDetails", DbType.String, null);
                            dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                            dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorID);
                            dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorUnitID);


                            int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                            //  BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                        }
                        if (BizActionObj.Day5Details.NextPlanID == 4 && BizActionObj.Day5Details.Isfreezed == true)
                        {
                            //Add in ET table
                            DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day5Details.PatientID);
                            dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day5Details.PatientUnitID);
                            dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyID);
                            dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                            BizActionObj.Day5Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                            DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                            dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day5Details.ID);
                            dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day5Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day5Details.Date);
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day5");
                            dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day5Details.GradeID);
                            dbServer.AddInParameter(command6, "Score", DbType.String, null);
                            dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day5Details.CellStageID);
                            dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                            dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                            dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                            dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                            dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorID);
                            dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorUnitID);
                            int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                        }
                        if (BizActionObj.Day5Details.NextPlanID == 2 && BizActionObj.Day5Details.Isfreezed == true)
                        {
                            //Add in Vitrification table
                            DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day5Details.PatientID);
                            dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day5Details.PatientUnitID);
                            dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyID);
                            dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day5Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                            dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                            dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                            BizActionObj.Day5Details.ID = (long)dbServer.GetParameterValue(command4, "ID");


                            DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                            dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day5Details.ID);
                            dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day5Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                            dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                            dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day5Details.Date);
                            dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day5");
                            dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day5Details.CellStageID);
                            dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day5Details.GradeID);
                            dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                            dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                            dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                            dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorID);
                            dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day5Details.OocyteDonorUnitID);

                            dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                            dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                            int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                        }
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Day5Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;
        }
        public override IValueObject GetDay5Details(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_GetDay5DetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay5DetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay5Details");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        BizAction.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        BizAction.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        BizAction.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        BizAction.Details.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                        BizAction.Details.Time = (DateTime?)(DALHelper.HandleDate(reader["Time"]));
                        BizAction.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        BizAction.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        BizAction.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        BizAction.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        BizAction.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        BizAction.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        BizAction.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        BizAction.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        BizAction.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        BizAction.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        BizAction.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        BizAction.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        BizAction.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        BizAction.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        BizAction.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        //BizAction.Details.Photo = (byte[])(DALHelper.HandleDBNull(reader["Image"]));
                        BizAction.Details.FrgmentationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrgmentationID"]));
                        BizAction.Details.BlastmereSymmetryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlastmereSymmetryID"]));
                        BizAction.Details.OtherDetails = Convert.ToString(DALHelper.HandleDBNull(reader["OtherDetails"]));
                        //BizAction.Details.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        BizAction.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));// BY BHUSHAN
                        //BizAction.Details.TreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TreatmentID"]));
                        BizAction.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        BizAction.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));

                        //by neena                       
                        //BizAction.Details.TreatmentStartDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentStartDate"]));
                        //BizAction.Details.TreatmentEndDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentEndDate"]));
                        //BizAction.Details.ObservationDate = (DateTime?)(DALHelper.HandleDate(reader["ObservationDate"]));
                        //BizAction.Details.ObservationTime = (DateTime?)(DALHelper.HandleDate(reader["ObservationTime"]));
                        //BizAction.Details.FertilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertilizationID"]));
                        BizAction.Details.CellObservationDate = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate"]));
                        BizAction.Details.CellObservationTime = (DateTime?)(DALHelper.HandleDate(reader["CellObservationTime"]));
                        BizAction.Details.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        BizAction.Details.IsBiopsy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBiopsy"]));
                        BizAction.Details.BiopsyDate = (DateTime?)(DALHelper.HandleDate(reader["BiopsyDate"]));
                        BizAction.Details.BiopsyTime = (DateTime?)(DALHelper.HandleDate(reader["BiopsyTime"]));
                        BizAction.Details.NoOfCell = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfCell"]));
                        BizAction.Details.StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"]));
                        BizAction.Details.InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"]));
                        BizAction.Details.TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"]));
                        BizAction.Details.AssistedHatching = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAssistedHatching"]));
                        BizAction.Details.CellNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellNo"]));

                        if (BizAction.Details.IsBiopsy == false)
                        {
                            BizAction.Details.BiopsyDate = null;
                            BizAction.Details.BiopsyTime = null;
                        }
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

                        if (!string.IsNullOrEmpty(ObjImg.OriginalImagePath))
                            ObjImg.ServerImageName = "..//" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;
                        // ObjImg.ServerImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;


                        BizAction.Details.ImgList.Add(ObjImg);
                    }
                }

                reader.NextResult();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_TherapyDocumentVO Details = new clsIVFDashboard_TherapyDocumentVO();
                        Details.ID = (long)reader["ID"];
                        Details.UnitID = (long)reader["UnitID"];
                        Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        Details.Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]));
                        Details.Date = (DateTime)(DALHelper.HandleDate(reader["Date"]));
                        Details.Title = Convert.ToString((DALHelper.HandleDBNull(reader["Title"])));
                        Details.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Details.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        Details.AttachedFileContent = (Byte[])(DALHelper.HandleDBNull(reader["AttachedFileContent"]));
                        Details.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));

                        BizAction.Details.DetailList.Add(Details);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        //added by neena
        public override IValueObject GetDay5OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay5OocyteDetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay5OocyteDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay5DetailsForOocyte");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                //dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, 0);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay5OocyteDetailsBizActionVO Oocytedetails = new clsIVFDashboard_GetDay5OocyteDetailsBizActionVO();
                        Oocytedetails.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Oocytedetails.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"]));
                        Oocytedetails.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Oocytelist.Add(Oocytedetails.Details);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }
        #endregion

        #region Day6
        public override IValueObject AddUpdateDay6Details(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_AddUpdateDay6BizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateDay6BizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {

                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.Day6Details.DecisionID == 0)
                {
                    DbCommand Sqlcommand11 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day6=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day6Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day6Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day6Details.SerialOocyteNumber);
                    int sqlStatus11 = dbServer.ExecuteNonQuery(Sqlcommand11, trans);
                }

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay6");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Day6Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Day6Details.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day6Details.SerialOocyteNumber);
                dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, BizActionObj.Day6Details.OocyteNumber);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.Day6Details.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionObj.Day6Details.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.Day6Details.EmbryologistID);
                dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day6Details.AssitantEmbryologistID);
                dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, BizActionObj.Day6Details.AnesthetistID);
                dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day6Details.AssitantAnesthetistID);
                dbServer.AddInParameter(command, "CumulusID", DbType.Int64, BizActionObj.Day6Details.CumulusID);
                dbServer.AddInParameter(command, "MOIID", DbType.Int64, BizActionObj.Day6Details.MOIID);
                dbServer.AddInParameter(command, "GradeID", DbType.Int64, BizActionObj.Day6Details.GradeID);
                dbServer.AddInParameter(command, "CellStageID", DbType.Int64, BizActionObj.Day6Details.CellStageID);
                dbServer.AddInParameter(command, "OccDiamension", DbType.String, BizActionObj.Day6Details.OccDiamension);
                dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, BizActionObj.Day6Details.SpermPreperationMedia);
                dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, BizActionObj.Day6Details.OocytePreparationMedia);
                dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, BizActionObj.Day6Details.IncubatorID);
                dbServer.AddInParameter(command, "FinalLayering", DbType.String, BizActionObj.Day6Details.FinalLayering);
                dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, BizActionObj.Day6Details.NextPlanID);
                dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, BizActionObj.Day6Details.Isfreezed);
                dbServer.AddInParameter(command, "Impression", DbType.String, BizActionObj.Day6Details.Impression); // by bHUSHAn
                dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, BizActionObj.Day6Details.FrgmentationID);
                dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, BizActionObj.Day6Details.BlastmereSymmetryID);
                dbServer.AddInParameter(command, "OtherDetails", DbType.String, BizActionObj.Day6Details.OtherDetails);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day6Details.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //by neena
                dbServer.AddInParameter(command, "CellObservationDate", DbType.DateTime, BizActionObj.Day6Details.CellObservationDate);
                dbServer.AddInParameter(command, "CellObservationTime", DbType.DateTime, BizActionObj.Day6Details.CellObservationTime);
                dbServer.AddInParameter(command, "StageofDevelopmentGrade", DbType.Int64, BizActionObj.Day6Details.StageofDevelopmentGrade);
                dbServer.AddInParameter(command, "InnerCellMassGrade", DbType.Int64, BizActionObj.Day6Details.InnerCellMassGrade);
                dbServer.AddInParameter(command, "TrophoectodermGrade", DbType.Int64, BizActionObj.Day6Details.TrophoectodermGrade);

                dbServer.AddInParameter(command, "CellStage", DbType.String, BizActionObj.Day6Details.CellStage);
                dbServer.AddInParameter(command, "IsBiopsy", DbType.Boolean, BizActionObj.Day6Details.IsBiopsy);
                dbServer.AddInParameter(command, "BiopsyDate", DbType.DateTime, BizActionObj.Day6Details.BiopsyDate);
                dbServer.AddInParameter(command, "BiopsyTime", DbType.DateTime, BizActionObj.Day6Details.BiopsyTime);
                dbServer.AddInParameter(command, "NoOfCell", DbType.Int64, BizActionObj.Day6Details.NoOfCell);
                dbServer.AddInParameter(command, "IsAssistedHatching", DbType.Boolean, BizActionObj.Day6Details.AssistedHatching);
                dbServer.AddInParameter(command, "CellNo", DbType.Int64, BizActionObj.Day6Details.CellNo);
                //

                dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, BizActionObj.Day6Details.OocyteDonorID);
                dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day6Details.OocyteDonorUnitID);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Day6Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.Day6Details.ImgList != null && BizActionObj.Day6Details.ImgList.Count > 0)
                {
                    foreach (var item in BizActionObj.Day6Details.ImgList)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day6Details.PatientID);
                        dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day6Details.PatientUnitID);
                        dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyID);
                        dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyUnitID);
                        dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day6Details.SerialOocyteNumber);
                        dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day6Details.OocyteNumber);
                        dbServer.AddInParameter(command1, "Day", DbType.Int64, 6);
                        dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day6Details.ID);
                        dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day6Details.CellStageID);
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
                        //dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
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

                if (BizActionObj.Day6Details.DetailList != null && BizActionObj.Day6Details.DetailList.Count > 0)
                {
                    foreach (var item in BizActionObj.Day6Details.DetailList)
                    {
                        DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                        dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item.Date);
                        dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item.PatientID);
                        dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                        dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item.PlanTherapyID);
                        dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item.PlanTherapyUnitID);
                        dbServer.AddInParameter(command8, "Description", DbType.String, item.Description);
                        dbServer.AddInParameter(command8, "Title", DbType.String, item.Title);
                        dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item.AttachedFileName);
                        dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item.AttachedFileContent);
                        dbServer.AddInParameter(command8, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.OocyteNumber);
                        dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, item.SerialOocyteNumber);
                        dbServer.AddInParameter(command8, "Day", DbType.Int64, item.Day);
                        //dbServer.AddInParameter(command8, "DocNo", DbType.String, item.DocNo);
                        dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 0);
                        if (item.DocNo == null)
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        else
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.DocNo);

                        //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus8 = dbServer.ExecuteNonQuery(command8);
                        item.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                        //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                    }
                }

                //if (BizActionObj.Day6Details.Photo != null)
                //{
                //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day6Details.PatientID);
                //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day6Details.PatientUnitID);
                //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyID);
                //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyUnitID);
                //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day6Details.SerialOocyteNumber);
                //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day6Details.OocyteNumber);
                //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 6);
                //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day6Details.ID);
                //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day6Details.CellStageID);
                //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day6Details.Photo);
                //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day6Details.FileName);
                //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                //}
                if (BizActionObj.Day6Details.NextPlanID == 4 && BizActionObj.Day6Details.Isfreezed == true)
                {
                    //Add in ET table
                    DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day6Details.PatientID);
                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day6Details.PatientUnitID);
                    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyID);
                    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                    BizActionObj.Day6Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                    DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                    dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day6Details.ID);
                    dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, BizActionObj.Day6Details.OocyteNumber);
                    dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day6Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day6Details.Date);
                    dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day6");
                    dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day6Details.GradeID);
                    dbServer.AddInParameter(command6, "Score", DbType.String, null);
                    dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day6Details.CellStageID);
                    dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                    dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                    dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                    dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                    dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day6Details.OocyteDonorID);
                    dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day6Details.OocyteDonorUnitID);

                    int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                }
                if (BizActionObj.Day6Details.NextPlanID == 2 && BizActionObj.Day6Details.Isfreezed == true)
                {
                    //Add in Vitrification table
                    DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day6Details.PatientID);
                    dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day6Details.PatientUnitID);
                    dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyID);
                    dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                    dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                    dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    BizActionObj.Day6Details.ID = (long)dbServer.GetParameterValue(command4, "ID");


                    DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                    dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day6Details.ID);
                    dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, BizActionObj.Day6Details.OocyteNumber);
                    dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day6Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                    dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                    dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day6Details.Date);
                    dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day6");
                    dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day6Details.CellStageID);
                    dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day6Details.GradeID);
                    dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                    dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                    dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                    dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day6Details.OocyteDonorID);
                    dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day6Details.OocyteDonorUnitID);

                    dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                    dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                    int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                }

                //added by neena
                if (BizActionObj.Day6Details.OcyteListList != null)
                {
                    foreach (var item in BizActionObj.Day6Details.OcyteListList)
                    {
                        try
                        {
                            DbCommand Sqlcommand12 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day6=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day6Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day6Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + (BizActionObj.Day6Details.SerialOocyteNumber + item.FilterID));
                            int sqlStatus12 = dbServer.ExecuteNonQuery(Sqlcommand12, trans);

                            DbCommand command7 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay6");


                            dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command7, "PatientID", DbType.Int64, BizActionObj.Day6Details.PatientID);
                            dbServer.AddInParameter(command7, "PatientUnitID", DbType.Int64, BizActionObj.Day6Details.PatientUnitID);
                            dbServer.AddInParameter(command7, "PlanTherapyID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyID);
                            dbServer.AddInParameter(command7, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command7, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day6Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command7, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command7, "Date", DbType.DateTime, BizActionObj.Day6Details.Date);
                            dbServer.AddInParameter(command7, "Time", DbType.DateTime, BizActionObj.Day6Details.Time);
                            dbServer.AddInParameter(command7, "EmbryologistID", DbType.Int64, BizActionObj.Day6Details.EmbryologistID);
                            dbServer.AddInParameter(command7, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day6Details.AssitantEmbryologistID);
                            dbServer.AddInParameter(command7, "AnesthetistID", DbType.Int64, BizActionObj.Day6Details.AnesthetistID);
                            dbServer.AddInParameter(command7, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day6Details.AssitantAnesthetistID);
                            dbServer.AddInParameter(command7, "CumulusID", DbType.Int64, BizActionObj.Day6Details.CumulusID);
                            dbServer.AddInParameter(command7, "MOIID", DbType.Int64, BizActionObj.Day6Details.MOIID);
                            dbServer.AddInParameter(command7, "GradeID", DbType.Int64, BizActionObj.Day6Details.GradeID);
                            dbServer.AddInParameter(command7, "CellStageID", DbType.Int64, BizActionObj.Day6Details.CellStageID);
                            dbServer.AddInParameter(command7, "OccDiamension", DbType.String, BizActionObj.Day6Details.OccDiamension);
                            dbServer.AddInParameter(command7, "SpermPreperationMedia", DbType.String, BizActionObj.Day6Details.SpermPreperationMedia);
                            dbServer.AddInParameter(command7, "OocytePreparationMedia", DbType.String, BizActionObj.Day6Details.OocytePreparationMedia);
                            dbServer.AddInParameter(command7, "IncubatorID", DbType.Int64, BizActionObj.Day6Details.IncubatorID);
                            dbServer.AddInParameter(command7, "FinalLayering", DbType.String, BizActionObj.Day6Details.FinalLayering);
                            dbServer.AddInParameter(command7, "NextPlanID", DbType.Int64, BizActionObj.Day6Details.NextPlanID);
                            dbServer.AddInParameter(command7, "Isfreezed", DbType.Boolean, BizActionObj.Day6Details.Isfreezed);
                            dbServer.AddInParameter(command7, "Impression", DbType.String, BizActionObj.Day6Details.Impression); // by bHUSHAn
                            dbServer.AddInParameter(command7, "FrgmentationID", DbType.Int64, BizActionObj.Day6Details.FrgmentationID);
                            dbServer.AddInParameter(command7, "BlastmereSymmetryID", DbType.Int64, BizActionObj.Day6Details.BlastmereSymmetryID);
                            dbServer.AddInParameter(command7, "OtherDetails", DbType.String, BizActionObj.Day6Details.OtherDetails);
                            dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day6Details.ID);
                            dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, int.MaxValue);

                            //by neena
                            dbServer.AddInParameter(command7, "CellObservationDate", DbType.DateTime, BizActionObj.Day6Details.CellObservationDate);
                            dbServer.AddInParameter(command7, "CellObservationTime", DbType.DateTime, BizActionObj.Day6Details.CellObservationTime);
                            dbServer.AddInParameter(command7, "StageofDevelopmentGrade", DbType.Int64, BizActionObj.Day6Details.StageofDevelopmentGrade);
                            dbServer.AddInParameter(command7, "InnerCellMassGrade", DbType.Int64, BizActionObj.Day6Details.InnerCellMassGrade);
                            dbServer.AddInParameter(command7, "TrophoectodermGrade", DbType.Int64, BizActionObj.Day6Details.TrophoectodermGrade);

                            dbServer.AddInParameter(command7, "CellStage", DbType.String, BizActionObj.Day6Details.CellStage);
                            dbServer.AddInParameter(command7, "IsBiopsy", DbType.Boolean, BizActionObj.Day6Details.IsBiopsy);
                            dbServer.AddInParameter(command7, "BiopsyDate", DbType.DateTime, BizActionObj.Day6Details.BiopsyDate);
                            dbServer.AddInParameter(command7, "BiopsyTime", DbType.DateTime, BizActionObj.Day6Details.BiopsyTime);
                            dbServer.AddInParameter(command7, "NoOfCell", DbType.Int64, BizActionObj.Day6Details.NoOfCell);
                            dbServer.AddInParameter(command7, "IsAssistedHatching", DbType.Boolean, BizActionObj.Day6Details.AssistedHatching);
                            dbServer.AddInParameter(command7, "CellNo", DbType.Int64, BizActionObj.Day6Details.CellNo);
                            //

                            dbServer.AddInParameter(command7, "OocyteDonorID", DbType.Int64, BizActionObj.Day6Details.OocyteDonorID);
                            dbServer.AddInParameter(command7, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day6Details.OocyteDonorUnitID);

                            int intStatus7 = dbServer.ExecuteNonQuery(command7, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command7, "ResultStatus");
                            BizActionObj.Day6Details.ID = (long)dbServer.GetParameterValue(command7, "ID");

                            if (BizActionObj.Day6Details.ImgList != null && BizActionObj.Day6Details.ImgList.Count > 0)
                            {
                                foreach (var item1 in BizActionObj.Day6Details.ImgList)
                                {
                                    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day6Details.PatientID);
                                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day6Details.PatientUnitID);
                                    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyID);
                                    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day6Details.SerialOocyteNumber + item.FilterID);
                                    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command1, "Day", DbType.Int64, 6);
                                    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day6Details.ID);
                                    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day6Details.CellStageID);
                                    //dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day0Details.Photo);
                                    //dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day0Details.FileName);
                                    //dbServer.AddInParameter(command1, "Photo", DbType.Binary, item.Photo);
                                    dbServer.AddInParameter(command1, "FileName", DbType.String, item1.ImagePath);
                                    //dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                                    //added by neena
                                    //dbServer.AddInParameter(command1, "SeqNo", DbType.String, item1.SeqNo);
                                    dbServer.AddParameter(command1, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.SeqNo);
                                    dbServer.AddInParameter(command1, "IsApplyTo", DbType.Int32, 1);
                                    dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ServerImageName);

                                    //dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);
                                    //

                                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                                    item1.ID = Convert.ToInt64(dbServer.GetParameterValue(command1, "ID"));
                                    item1.ServerImageName = Convert.ToString(dbServer.GetParameterValue(command1, "ServerImageName"));
                                    item1.SeqNo = Convert.ToString(dbServer.GetParameterValue(command1, "SeqNo"));

                                    //if (item1.Photo != null)
                                    //    File.WriteAllBytes(ImgSaveLocation + item1.ServerImageName, item1.Photo);

                                    //cnt++;
                                }
                            }


                            //if (BizActionObj.Day2Details.Photo != null)
                            //{
                            //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                            //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                            //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                            //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                            //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                            //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber + item.FilterID);
                            //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                            //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 2);
                            //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day2Details.ID);
                            //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day2Details.CellStageID);
                            //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day2Details.Photo);
                            //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day2Details.FileName);
                            //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                            //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                            //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                            //}

                            if (BizActionObj.Day6Details.DetailList != null && BizActionObj.Day6Details.DetailList.Count > 0)
                            {
                                foreach (var item2 in BizActionObj.Day6Details.DetailList)
                                {
                                    DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                                    dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item2.Date);
                                    dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item2.PatientID);
                                    dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item2.PatientUnitID);
                                    dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item2.PlanTherapyID);
                                    dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item2.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command8, "Description", DbType.String, item2.Description);
                                    dbServer.AddInParameter(command8, "Title", DbType.String, item2.Title);
                                    dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item2.AttachedFileName);
                                    dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item2.AttachedFileContent);
                                    dbServer.AddInParameter(command8, "Status", DbType.Boolean, item2.Status);
                                    dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                    dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                    dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                    dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                                    dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day6Details.SerialOocyteNumber + item.FilterID);
                                    dbServer.AddInParameter(command8, "Day", DbType.Int64, item2.Day);
                                    //dbServer.AddInParameter(command8, "DocNo", DbType.String, item2.DocNo);
                                    dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.DocNo);
                                    dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                                    dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 1);
                                    //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                                    int intStatus8 = dbServer.ExecuteNonQuery(command8);
                                    item2.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                                    //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                        if (BizActionObj.Day6Details.NextPlanID == 4 && BizActionObj.Day6Details.Isfreezed == true)
                        {
                            //Add in ET table
                            DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day6Details.PatientID);
                            dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day6Details.PatientUnitID);
                            dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyID);
                            dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                            BizActionObj.Day6Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                            DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                            dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day6Details.ID);
                            dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day6Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day6Details.Date);
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day6");
                            dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day6Details.GradeID);
                            dbServer.AddInParameter(command6, "Score", DbType.String, null);
                            dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day6Details.CellStageID);
                            dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                            dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                            dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                            dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                            dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day6Details.OocyteDonorID);
                            dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day6Details.OocyteDonorUnitID);

                            int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                        }
                        if (BizActionObj.Day6Details.NextPlanID == 2 && BizActionObj.Day6Details.Isfreezed == true)
                        {
                            //Add in Vitrification table
                            DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day6Details.PatientID);
                            dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day6Details.PatientUnitID);
                            dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyID);
                            dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                            dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                            dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                            BizActionObj.Day6Details.ID = (long)dbServer.GetParameterValue(command4, "ID");


                            DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                            dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day6Details.ID);
                            dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day6Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                            dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                            dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day6Details.Date);
                            dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day6");
                            dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day6Details.CellStageID);
                            dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day6Details.GradeID);
                            dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                            dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                            dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                            dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day6Details.OocyteDonorID);
                            dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day6Details.OocyteDonorUnitID);

                            dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                            dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                            int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                        }
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Day6Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;

        }
        public override IValueObject GetDay6Details(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay6DetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay6DetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay6Details");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        BizAction.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        BizAction.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        BizAction.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        BizAction.Details.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                        BizAction.Details.Time = (DateTime?)(DALHelper.HandleDate(reader["Time"]));
                        BizAction.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        BizAction.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        BizAction.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        BizAction.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        BizAction.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        BizAction.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        BizAction.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        BizAction.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        BizAction.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        BizAction.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        BizAction.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        BizAction.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        BizAction.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        BizAction.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        BizAction.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        //BizAction.Details.Photo = (byte[])(DALHelper.HandleDBNull(reader["Image"]));
                        BizAction.Details.FrgmentationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrgmentationID"]));
                        BizAction.Details.BlastmereSymmetryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlastmereSymmetryID"]));
                        BizAction.Details.OtherDetails = Convert.ToString(DALHelper.HandleDBNull(reader["OtherDetails"]));
                        //BizAction.Details.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        BizAction.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));// BY BHUSHAN
                        // BizAction.Details.TreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TreatmentID"]));
                        BizAction.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        BizAction.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));

                        //by neena                       
                        //BizAction.Details.TreatmentStartDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentStartDate"]));
                        //BizAction.Details.TreatmentEndDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentEndDate"]));
                        //BizAction.Details.ObservationDate = (DateTime?)(DALHelper.HandleDate(reader["ObservationDate"]));
                        //BizAction.Details.ObservationTime = (DateTime?)(DALHelper.HandleDate(reader["ObservationTime"]));
                        //BizAction.Details.FertilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertilizationID"]));
                        BizAction.Details.CellObservationDate = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate"]));
                        BizAction.Details.CellObservationTime = (DateTime?)(DALHelper.HandleDate(reader["CellObservationTime"]));
                        BizAction.Details.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        BizAction.Details.IsBiopsy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBiopsy"]));
                        BizAction.Details.BiopsyDate = (DateTime?)(DALHelper.HandleDate(reader["BiopsyDate"]));
                        BizAction.Details.BiopsyTime = (DateTime?)(DALHelper.HandleDate(reader["BiopsyTime"]));
                        BizAction.Details.NoOfCell = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfCell"]));
                        BizAction.Details.StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"]));
                        BizAction.Details.InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"]));
                        BizAction.Details.TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"]));
                        BizAction.Details.AssistedHatching = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAssistedHatching"]));
                        BizAction.Details.CellNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellNo"]));

                        if (BizAction.Details.IsBiopsy == false)
                        {
                            BizAction.Details.BiopsyDate = null;
                            BizAction.Details.BiopsyTime = null;
                        }
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

                        if (!string.IsNullOrEmpty(ObjImg.OriginalImagePath))
                            ObjImg.ServerImageName = "..//" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;
                        // ObjImg.ServerImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;


                        BizAction.Details.ImgList.Add(ObjImg);
                    }
                }

                reader.NextResult();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_TherapyDocumentVO Details = new clsIVFDashboard_TherapyDocumentVO();
                        Details.ID = (long)reader["ID"];
                        Details.UnitID = (long)reader["UnitID"];
                        Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        Details.Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]));
                        Details.Date = (DateTime)(DALHelper.HandleDate(reader["Date"]));
                        Details.Title = Convert.ToString((DALHelper.HandleDBNull(reader["Title"])));
                        Details.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Details.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        Details.AttachedFileContent = (Byte[])(DALHelper.HandleDBNull(reader["AttachedFileContent"]));
                        Details.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));

                        BizAction.Details.DetailList.Add(Details);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        //added by neena      
        public override IValueObject GetDay6OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay6OocyteDetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay6OocyteDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay6DetailsForOocyte");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                //dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, 0);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay6OocyteDetailsBizActionVO Oocytedetails = new clsIVFDashboard_GetDay6OocyteDetailsBizActionVO();
                        Oocytedetails.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Oocytedetails.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"]));
                        Oocytedetails.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Oocytelist.Add(Oocytedetails.Details);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }

        #endregion

        //added by neena      
        #region Day7
        public override IValueObject AddUpdateDay7Details(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_AddUpdateDay7BizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateDay7BizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {

                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.Day7Details.DecisionID == 0)
                {
                    DbCommand Sqlcommand11 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day7=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day7Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day7Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + BizActionObj.Day7Details.SerialOocyteNumber);
                    int sqlStatus11 = dbServer.ExecuteNonQuery(Sqlcommand11, trans);
                }

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay7");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Day7Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Day7Details.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day7Details.SerialOocyteNumber);
                dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, BizActionObj.Day7Details.OocyteNumber);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.Day7Details.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionObj.Day7Details.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.Day7Details.EmbryologistID);
                dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day7Details.AssitantEmbryologistID);
                dbServer.AddInParameter(command, "AnesthetistID", DbType.Int64, BizActionObj.Day7Details.AnesthetistID);
                dbServer.AddInParameter(command, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day7Details.AssitantAnesthetistID);
                dbServer.AddInParameter(command, "CumulusID", DbType.Int64, BizActionObj.Day7Details.CumulusID);
                dbServer.AddInParameter(command, "MOIID", DbType.Int64, BizActionObj.Day7Details.MOIID);
                dbServer.AddInParameter(command, "GradeID", DbType.Int64, BizActionObj.Day7Details.GradeID);
                dbServer.AddInParameter(command, "CellStageID", DbType.Int64, BizActionObj.Day7Details.CellStageID);
                dbServer.AddInParameter(command, "OccDiamension", DbType.String, BizActionObj.Day7Details.OccDiamension);
                dbServer.AddInParameter(command, "SpermPreperationMedia", DbType.String, BizActionObj.Day7Details.SpermPreperationMedia);
                dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, BizActionObj.Day7Details.OocytePreparationMedia);
                dbServer.AddInParameter(command, "IncubatorID", DbType.Int64, BizActionObj.Day7Details.IncubatorID);
                dbServer.AddInParameter(command, "FinalLayering", DbType.String, BizActionObj.Day7Details.FinalLayering);
                dbServer.AddInParameter(command, "NextPlanID", DbType.Int64, BizActionObj.Day7Details.NextPlanID);
                dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, BizActionObj.Day7Details.Isfreezed);
                dbServer.AddInParameter(command, "Impression", DbType.String, BizActionObj.Day7Details.Impression); // by bHUSHAn
                dbServer.AddInParameter(command, "FrgmentationID", DbType.Int64, BizActionObj.Day7Details.FrgmentationID);
                dbServer.AddInParameter(command, "BlastmereSymmetryID", DbType.Int64, BizActionObj.Day7Details.BlastmereSymmetryID);
                dbServer.AddInParameter(command, "OtherDetails", DbType.String, BizActionObj.Day7Details.OtherDetails);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day7Details.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //by neena
                dbServer.AddInParameter(command, "CellObservationDate", DbType.DateTime, BizActionObj.Day7Details.CellObservationDate);
                dbServer.AddInParameter(command, "CellObservationTime", DbType.DateTime, BizActionObj.Day7Details.CellObservationTime);
                dbServer.AddInParameter(command, "StageofDevelopmentGrade", DbType.Int64, BizActionObj.Day7Details.StageofDevelopmentGrade);
                dbServer.AddInParameter(command, "InnerCellMassGrade", DbType.Int64, BizActionObj.Day7Details.InnerCellMassGrade);
                dbServer.AddInParameter(command, "TrophoectodermGrade", DbType.Int64, BizActionObj.Day7Details.TrophoectodermGrade);

                dbServer.AddInParameter(command, "CellStage", DbType.String, BizActionObj.Day7Details.CellStage);
                dbServer.AddInParameter(command, "IsBiopsy", DbType.Boolean, BizActionObj.Day7Details.IsBiopsy);
                dbServer.AddInParameter(command, "BiopsyDate", DbType.DateTime, BizActionObj.Day7Details.BiopsyDate);
                dbServer.AddInParameter(command, "BiopsyTime", DbType.DateTime, BizActionObj.Day7Details.BiopsyTime);
                dbServer.AddInParameter(command, "NoOfCell", DbType.Int64, BizActionObj.Day7Details.NoOfCell);
                dbServer.AddInParameter(command, "IsAssistedHatching", DbType.Boolean, BizActionObj.Day7Details.AssistedHatching);
                dbServer.AddInParameter(command, "CellNo", DbType.Int64, BizActionObj.Day7Details.CellNo);
                //

                dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, BizActionObj.Day7Details.OocyteDonorID);
                dbServer.AddInParameter(command, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day7Details.OocyteDonorUnitID);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Day7Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.Day7Details.ImgList != null && BizActionObj.Day7Details.ImgList.Count > 0)
                {
                    foreach (var item in BizActionObj.Day7Details.ImgList)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day7Details.PatientID);
                        dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day7Details.PatientUnitID);
                        dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyID);
                        dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyUnitID);
                        dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day7Details.SerialOocyteNumber);
                        dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day7Details.OocyteNumber);
                        dbServer.AddInParameter(command1, "Day", DbType.Int64, 7);
                        dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day7Details.ID);
                        dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day7Details.CellStageID);
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
                        //dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
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

                if (BizActionObj.Day7Details.DetailList != null && BizActionObj.Day7Details.DetailList.Count > 0)
                {
                    foreach (var item in BizActionObj.Day7Details.DetailList)
                    {
                        DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                        dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item.Date);
                        dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item.PatientID);
                        dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                        dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item.PlanTherapyID);
                        dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item.PlanTherapyUnitID);
                        dbServer.AddInParameter(command8, "Description", DbType.String, item.Description);
                        dbServer.AddInParameter(command8, "Title", DbType.String, item.Title);
                        dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item.AttachedFileName);
                        dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item.AttachedFileContent);
                        dbServer.AddInParameter(command8, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.OocyteNumber);
                        dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, item.SerialOocyteNumber);
                        dbServer.AddInParameter(command8, "Day", DbType.Int64, item.Day);
                        //dbServer.AddInParameter(command8, "DocNo", DbType.String, item.DocNo);
                        dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 0);
                        if (item.DocNo == null)
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        else
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.DocNo);

                        //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus8 = dbServer.ExecuteNonQuery(command8);
                        item.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                        //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                    }
                }

                //if (BizActionObj.Day6Details.Photo != null)
                //{
                //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day6Details.PatientID);
                //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day6Details.PatientUnitID);
                //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyID);
                //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day6Details.PlanTherapyUnitID);
                //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day6Details.SerialOocyteNumber);
                //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizActionObj.Day6Details.OocyteNumber);
                //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 6);
                //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day6Details.ID);
                //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day6Details.CellStageID);
                //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day6Details.Photo);
                //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day6Details.FileName);
                //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                //}
                if (BizActionObj.Day7Details.NextPlanID == 4 && BizActionObj.Day7Details.Isfreezed == true)
                {
                    //Add in ET table
                    DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day7Details.PatientID);
                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day7Details.PatientUnitID);
                    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyID);
                    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                    BizActionObj.Day7Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                    DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                    dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day7Details.ID);
                    dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, BizActionObj.Day7Details.OocyteNumber);
                    dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day7Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day7Details.Date);
                    dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day7");
                    dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day7Details.GradeID);
                    dbServer.AddInParameter(command6, "Score", DbType.String, null);
                    dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day7Details.CellStageID);
                    dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                    dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                    dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                    dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                    dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day7Details.OocyteDonorID);
                    dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day7Details.OocyteDonorUnitID);

                    int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                }
                if (BizActionObj.Day7Details.NextPlanID == 2 && BizActionObj.Day7Details.Isfreezed == true)
                {
                    //Add in Vitrification table
                    DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day7Details.PatientID);
                    dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day7Details.PatientUnitID);
                    dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyID);
                    dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyUnitID);
                    dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                    dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                    dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command4,   "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    BizActionObj.Day7Details.ID = (long)dbServer.GetParameterValue(command4, "ID");


                    DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                    dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day7Details.ID);
                    dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, BizActionObj.Day7Details.OocyteNumber);
                    dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day7Details.SerialOocyteNumber);
                    dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                    dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                    dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day7Details.Date);
                    dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day7");
                    dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day7Details.CellStageID);
                    dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day7Details.GradeID);
                    dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                    dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                    dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                    dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day7Details.OocyteDonorID);
                    dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day7Details.OocyteDonorUnitID);

                    dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                    dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                    int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                }

                //added by neena
                if (BizActionObj.Day7Details.OcyteListList != null)
                {
                    foreach (var item in BizActionObj.Day7Details.OcyteListList)
                    {
                        try
                        {
                            DbCommand Sqlcommand12 = dbServer.GetSqlStringCommand("Update T_IVFDashboard_GraphicalRepresentation set Day7=" + 1 + " where  PlanTherapyID=" + BizActionObj.Day7Details.PlanTherapyID + " and PlanTherapyUnitID=" + BizActionObj.Day7Details.PlanTherapyUnitID + " and SerialOocyteNumber=" + (BizActionObj.Day7Details.SerialOocyteNumber + item.FilterID));
                            int sqlStatus12 = dbServer.ExecuteNonQuery(Sqlcommand12, trans);

                            DbCommand command7 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDay7");


                            dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command7, "PatientID", DbType.Int64, BizActionObj.Day7Details.PatientID);
                            dbServer.AddInParameter(command7, "PatientUnitID", DbType.Int64, BizActionObj.Day7Details.PatientUnitID);
                            dbServer.AddInParameter(command7, "PlanTherapyID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyID);
                            dbServer.AddInParameter(command7, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command7, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day7Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command7, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command7, "Date", DbType.DateTime, BizActionObj.Day7Details.Date);
                            dbServer.AddInParameter(command7, "Time", DbType.DateTime, BizActionObj.Day7Details.Time);
                            dbServer.AddInParameter(command7, "EmbryologistID", DbType.Int64, BizActionObj.Day7Details.EmbryologistID);
                            dbServer.AddInParameter(command7, "AssitantEmbryologistID", DbType.Int64, BizActionObj.Day7Details.AssitantEmbryologistID);
                            dbServer.AddInParameter(command7, "AnesthetistID", DbType.Int64, BizActionObj.Day7Details.AnesthetistID);
                            dbServer.AddInParameter(command7, "AssitantAnesthetistID", DbType.Int64, BizActionObj.Day7Details.AssitantAnesthetistID);
                            dbServer.AddInParameter(command7, "CumulusID", DbType.Int64, BizActionObj.Day7Details.CumulusID);
                            dbServer.AddInParameter(command7, "MOIID", DbType.Int64, BizActionObj.Day7Details.MOIID);
                            dbServer.AddInParameter(command7, "GradeID", DbType.Int64, BizActionObj.Day7Details.GradeID);
                            dbServer.AddInParameter(command7, "CellStageID", DbType.Int64, BizActionObj.Day7Details.CellStageID);
                            dbServer.AddInParameter(command7, "OccDiamension", DbType.String, BizActionObj.Day7Details.OccDiamension);
                            dbServer.AddInParameter(command7, "SpermPreperationMedia", DbType.String, BizActionObj.Day7Details.SpermPreperationMedia);
                            dbServer.AddInParameter(command7, "OocytePreparationMedia", DbType.String, BizActionObj.Day7Details.OocytePreparationMedia);
                            dbServer.AddInParameter(command7, "IncubatorID", DbType.Int64, BizActionObj.Day7Details.IncubatorID);
                            dbServer.AddInParameter(command7, "FinalLayering", DbType.String, BizActionObj.Day7Details.FinalLayering);
                            dbServer.AddInParameter(command7, "NextPlanID", DbType.Int64, BizActionObj.Day7Details.NextPlanID);
                            dbServer.AddInParameter(command7, "Isfreezed", DbType.Boolean, BizActionObj.Day7Details.Isfreezed);
                            dbServer.AddInParameter(command7, "Impression", DbType.String, BizActionObj.Day7Details.Impression); // by bHUSHAn
                            dbServer.AddInParameter(command7, "FrgmentationID", DbType.Int64, BizActionObj.Day7Details.FrgmentationID);
                            dbServer.AddInParameter(command7, "BlastmereSymmetryID", DbType.Int64, BizActionObj.Day7Details.BlastmereSymmetryID);
                            dbServer.AddInParameter(command7, "OtherDetails", DbType.String, BizActionObj.Day7Details.OtherDetails);
                            dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Day7Details.ID);
                            dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, int.MaxValue);

                            //by neena
                            dbServer.AddInParameter(command7, "CellObservationDate", DbType.DateTime, BizActionObj.Day7Details.CellObservationDate);
                            dbServer.AddInParameter(command7, "CellObservationTime", DbType.DateTime, BizActionObj.Day7Details.CellObservationTime);
                            dbServer.AddInParameter(command7, "StageofDevelopmentGrade", DbType.Int64, BizActionObj.Day7Details.StageofDevelopmentGrade);
                            dbServer.AddInParameter(command7, "InnerCellMassGrade", DbType.Int64, BizActionObj.Day7Details.InnerCellMassGrade);
                            dbServer.AddInParameter(command7, "TrophoectodermGrade", DbType.Int64, BizActionObj.Day7Details.TrophoectodermGrade);

                            dbServer.AddInParameter(command7, "CellStage", DbType.String, BizActionObj.Day7Details.CellStage);
                            dbServer.AddInParameter(command7, "IsBiopsy", DbType.Boolean, BizActionObj.Day7Details.IsBiopsy);
                            dbServer.AddInParameter(command7, "BiopsyDate", DbType.DateTime, BizActionObj.Day7Details.BiopsyDate);
                            dbServer.AddInParameter(command7, "BiopsyTime", DbType.DateTime, BizActionObj.Day7Details.BiopsyTime);
                            dbServer.AddInParameter(command7, "NoOfCell", DbType.Int64, BizActionObj.Day7Details.NoOfCell);
                            dbServer.AddInParameter(command7, "IsAssistedHatching", DbType.Boolean, BizActionObj.Day7Details.AssistedHatching);
                            dbServer.AddInParameter(command7, "CellNo", DbType.Int64, BizActionObj.Day7Details.CellNo);
                            //

                            dbServer.AddInParameter(command7, "OocyteDonorID", DbType.Int64, BizActionObj.Day7Details.OocyteDonorID);
                            dbServer.AddInParameter(command7, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day7Details.OocyteDonorUnitID);

                            int intStatus7 = dbServer.ExecuteNonQuery(command7, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command7, "ResultStatus");
                            BizActionObj.Day7Details.ID = (long)dbServer.GetParameterValue(command7, "ID");

                            if (BizActionObj.Day7Details.ImgList != null && BizActionObj.Day7Details.ImgList.Count > 0)
                            {
                                foreach (var item1 in BizActionObj.Day7Details.ImgList)
                                {
                                    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day7Details.PatientID);
                                    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day7Details.PatientUnitID);
                                    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyID);
                                    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day7Details.SerialOocyteNumber + item.FilterID);
                                    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command1, "Day", DbType.Int64, 7);
                                    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day7Details.ID);
                                    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day7Details.CellStageID);
                                    //dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day0Details.Photo);
                                    //dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day0Details.FileName);
                                    //dbServer.AddInParameter(command1, "Photo", DbType.Binary, item.Photo);
                                    dbServer.AddInParameter(command1, "FileName", DbType.String, item1.ImagePath);
                                    //dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                                    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                                    //added by neena
                                    //dbServer.AddInParameter(command1, "SeqNo", DbType.String, item1.SeqNo);
                                    dbServer.AddParameter(command1, "SeqNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.SeqNo);
                                    dbServer.AddInParameter(command1, "IsApplyTo", DbType.Int32, 1);
                                    dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ServerImageName);

                                    //dbServer.AddParameter(command1, "ServerImageName", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);
                                    //

                                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                                    item1.ID = Convert.ToInt64(dbServer.GetParameterValue(command1, "ID"));
                                    item1.ServerImageName = Convert.ToString(dbServer.GetParameterValue(command1, "ServerImageName"));
                                    item1.SeqNo = Convert.ToString(dbServer.GetParameterValue(command1, "SeqNo"));

                                    //if (item1.Photo != null)
                                    //    File.WriteAllBytes(ImgSaveLocation + item1.ServerImageName, item1.Photo);

                                    //cnt++;
                                }
                            }


                            //if (BizActionObj.Day2Details.Photo != null)
                            //{
                            //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateEmbryoImage");
                            //    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizActionObj.Day2Details.PatientID);
                            //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizActionObj.Day2Details.PatientUnitID);
                            //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyID);
                            //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day2Details.PlanTherapyUnitID);
                            //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day2Details.SerialOocyteNumber + item.FilterID);
                            //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, item.ID);
                            //    dbServer.AddInParameter(command1, "Day", DbType.Int64, 2);
                            //    dbServer.AddInParameter(command1, "DayID", DbType.Int64, BizActionObj.Day2Details.ID);
                            //    dbServer.AddInParameter(command1, "DayUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "CellStageID", DbType.Int64, BizActionObj.Day2Details.CellStageID);
                            //    dbServer.AddInParameter(command1, "Photo", DbType.Binary, BizActionObj.Day2Details.Photo);
                            //    dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.Day2Details.FileName);
                            //    dbServer.AddInParameter(command1, "ID", DbType.Int64, 0);
                            //    dbServer.AddInParameter(command1, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                            //    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            //    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            //    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            //    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                            //}

                            if (BizActionObj.Day7Details.DetailList != null && BizActionObj.Day7Details.DetailList.Count > 0)
                            {
                                foreach (var item2 in BizActionObj.Day7Details.DetailList)
                                {
                                    DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                                    dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item2.Date);
                                    dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item2.PatientID);
                                    dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item2.PatientUnitID);
                                    dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item2.PlanTherapyID);
                                    dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item2.PlanTherapyUnitID);
                                    dbServer.AddInParameter(command8, "Description", DbType.String, item2.Description);
                                    dbServer.AddInParameter(command8, "Title", DbType.String, item2.Title);
                                    dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item2.AttachedFileName);
                                    dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item2.AttachedFileContent);
                                    dbServer.AddInParameter(command8, "Status", DbType.Boolean, item2.Status);
                                    dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                    dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                                    dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                    dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                                    dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day7Details.SerialOocyteNumber + item.FilterID);
                                    dbServer.AddInParameter(command8, "Day", DbType.Int64, item2.Day);
                                    //dbServer.AddInParameter(command8, "DocNo", DbType.String, item2.DocNo);
                                    dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.DocNo);
                                    dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                                    dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 1);
                                    //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                                    int intStatus8 = dbServer.ExecuteNonQuery(command8);
                                    item2.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                                    //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                        if (BizActionObj.Day7Details.NextPlanID == 4 && BizActionObj.Day7Details.Isfreezed == true)
                        {
                            //Add in ET table
                            DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Day7Details.PatientID);
                            dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Day7Details.PatientUnitID);
                            dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyID);
                            dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                            dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                            dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                            dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                            dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                            BizActionObj.Day7Details.ID = (long)dbServer.GetParameterValue(command3, "ID");


                            DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                            dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.Day7Details.ID);
                            dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.Day7Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, BizActionObj.Day7Details.Date);
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day7");
                            dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.Day7Details.GradeID);
                            dbServer.AddInParameter(command6, "Score", DbType.String, null);
                            dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.Day7Details.CellStageID);
                            dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                            dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                            dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                            dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                            dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                            dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.Day7Details.OocyteDonorID);
                            dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day7Details.OocyteDonorUnitID);

                            int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                        }
                        if (BizActionObj.Day7Details.NextPlanID == 2 && BizActionObj.Day7Details.Isfreezed == true)
                        {
                            //Add in Vitrification table
                            DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Day7Details.PatientID);
                            dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Day7Details.PatientUnitID);
                            dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyID);
                            dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Day7Details.PlanTherapyUnitID);
                            dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                            dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                            dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                            dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                            dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                            dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                            BizActionObj.Day7Details.ID = (long)dbServer.GetParameterValue(command4, "ID");


                            DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                            dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Day7Details.ID);
                            dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, item.ID);
                            dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.Day7Details.SerialOocyteNumber + item.FilterID);
                            dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                            dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                            dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.Day7Details.Date);
                            dbServer.AddInParameter(command5, "TransferDay", DbType.String, "Day7");
                            dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.Day7Details.CellStageID);
                            dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.Day7Details.GradeID);
                            dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                            dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                            dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                            dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                            dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.Day7Details.OocyteDonorID);
                            dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.Day7Details.OocyteDonorUnitID);

                            dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                            dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                            dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                            int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                        }
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Day7Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;

        }
        public override IValueObject GetDay7Details(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay7DetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay7DetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay7Details");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.Details.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        BizAction.Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        BizAction.Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        BizAction.Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        BizAction.Details.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                        BizAction.Details.Time = (DateTime?)(DALHelper.HandleDate(reader["Time"]));
                        BizAction.Details.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.Details.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                        BizAction.Details.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnesthetistID"]));
                        BizAction.Details.AssitantAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnesthetistID"]));
                        BizAction.Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        BizAction.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        BizAction.Details.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        BizAction.Details.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        BizAction.Details.MOIID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"]));
                        BizAction.Details.CumulusID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"]));
                        BizAction.Details.IncubatorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IncubatorID"]));
                        BizAction.Details.OccDiamension = Convert.ToString(DALHelper.HandleDBNull(reader["OccDiamension"]));
                        BizAction.Details.SpermPreperationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        BizAction.Details.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        BizAction.Details.NextPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NextPlanID"]));
                        BizAction.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Details.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));
                        //BizAction.Details.Photo = (byte[])(DALHelper.HandleDBNull(reader["Image"]));
                        BizAction.Details.FrgmentationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrgmentationID"]));
                        BizAction.Details.BlastmereSymmetryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlastmereSymmetryID"]));
                        BizAction.Details.OtherDetails = Convert.ToString(DALHelper.HandleDBNull(reader["OtherDetails"]));
                        //BizAction.Details.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        BizAction.Details.Impression = Convert.ToString(DALHelper.HandleDBNull(reader["Impression"]));// BY BHUSHAN
                        // BizAction.Details.TreatmentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TreatmentID"]));
                        BizAction.Details.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        BizAction.Details.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));

                        //by neena                       
                        //BizAction.Details.TreatmentStartDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentStartDate"]));
                        //BizAction.Details.TreatmentEndDate = (DateTime?)(DALHelper.HandleDate(reader["TreatmentEndDate"]));
                        //BizAction.Details.ObservationDate = (DateTime?)(DALHelper.HandleDate(reader["ObservationDate"]));
                        //BizAction.Details.ObservationTime = (DateTime?)(DALHelper.HandleDate(reader["ObservationTime"]));
                        //BizAction.Details.FertilizationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertilizationID"]));
                        BizAction.Details.CellObservationDate = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate"]));
                        BizAction.Details.CellObservationTime = (DateTime?)(DALHelper.HandleDate(reader["CellObservationTime"]));
                        BizAction.Details.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        BizAction.Details.IsBiopsy = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBiopsy"]));
                        BizAction.Details.BiopsyDate = (DateTime?)(DALHelper.HandleDate(reader["BiopsyDate"]));
                        BizAction.Details.BiopsyTime = (DateTime?)(DALHelper.HandleDate(reader["BiopsyTime"]));
                        BizAction.Details.NoOfCell = Convert.ToInt64(DALHelper.HandleDBNull(reader["NoOfCell"]));
                        BizAction.Details.StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"]));
                        BizAction.Details.InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"]));
                        BizAction.Details.TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"]));
                        BizAction.Details.AssistedHatching = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAssistedHatching"]));
                        BizAction.Details.CellNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellNo"]));

                        if (BizAction.Details.IsBiopsy == false)
                        {
                            BizAction.Details.BiopsyDate = null;
                            BizAction.Details.BiopsyTime = null;
                        }
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

                        if (!string.IsNullOrEmpty(ObjImg.OriginalImagePath))
                            ObjImg.ServerImageName = "..//" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;
                        //ObjImg.ServerImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;


                        BizAction.Details.ImgList.Add(ObjImg);
                    }
                }

                reader.NextResult();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_TherapyDocumentVO Details = new clsIVFDashboard_TherapyDocumentVO();
                        Details.ID = (long)reader["ID"];
                        Details.UnitID = (long)reader["UnitID"];
                        Details.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        Details.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        Details.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocuteNumber"]));
                        Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        Details.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        Details.Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]));
                        Details.Date = (DateTime)(DALHelper.HandleDate(reader["Date"]));
                        Details.Title = Convert.ToString((DALHelper.HandleDBNull(reader["Title"])));
                        Details.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        Details.AttachedFileName = Convert.ToString(DALHelper.HandleDBNull(reader["AttachedFileName"]));
                        Details.AttachedFileContent = (Byte[])(DALHelper.HandleDBNull(reader["AttachedFileContent"]));
                        Details.DocNo = Convert.ToString(DALHelper.HandleDBNull(reader["DocNo"]));

                        BizAction.Details.DetailList.Add(Details);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }
        public override IValueObject GetDay7OocyteDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetDay7OocyteDetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetDay7OocyteDetailsBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetDay7DetailsForOocyte");
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Details.PlanTherapyUnitID);
                //dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, 0);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.Details.SerialOocyteNumber);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_GetDay7OocyteDetailsBizActionVO Oocytedetails = new clsIVFDashboard_GetDay7OocyteDetailsBizActionVO();
                        Oocytedetails.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        Oocytedetails.Details.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["applayOocyte"]));
                        Oocytedetails.Details.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.Oocytelist.Add(Oocytedetails.Details);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                con = null;
            }
            return BizAction;
        }
        #endregion
        //


        #region Media Details
        public override IValueObject AddUpdateMediaDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateMediaDetailsBizActionVO BizAction = valueObject as clsIVFDashboard_AddUpdateMediaDetailsBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                foreach (var item in BizAction.ObserMediaList)
                {
                    con = dbServer.CreateConnection();
                    con.Open();
                    trans = con.BeginTransaction();
                    DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateMediaDetails");
                    command.Connection = con;

                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.MediaDetails.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.MediaDetails.PatientUnitID);
                    dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.MediaDetails.PlanTherapyID);
                    dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.MediaDetails.PlanTherapyUnitID);
                    dbServer.AddInParameter(command, "ProcedureName", DbType.String, BizAction.MediaDetails.ProcedureName);
                    dbServer.AddInParameter(command, "Date", DbType.DateTime, item.Date);
                    dbServer.AddInParameter(command, "MediaName", DbType.String, item.ItemName);
                    dbServer.AddInParameter(command, "LotNo", DbType.String, item.BatchCode);
                    dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, item.ExpiryDate);
                    dbServer.AddInParameter(command, "PH", DbType.Boolean, item.PH);
                    dbServer.AddInParameter(command, "OSM", DbType.Boolean, item.OSM);
                    dbServer.AddInParameter(command, "VolumeUsed", DbType.Int64, item.VolumeUsed);
                    dbServer.AddInParameter(command, "Status", DbType.String, item.Status);
                    dbServer.AddInParameter(command, "BatchID", DbType.Int64, item.BatchID);
                    dbServer.AddInParameter(command, "ItemID", DbType.Int64, item.ItemID);
                    dbServer.AddInParameter(command, "StoreID", DbType.Int64, item.StoreID);
                    dbServer.AddInParameter(command, "Finalized", DbType.Boolean, item.Finalized);
                    dbServer.AddInParameter(command, "StatusIS", DbType.Boolean, item.StatusIS);
                    dbServer.AddInParameter(command, "Company", DbType.String, item.Company);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus4 = dbServer.ExecuteNonQuery(command);
                    BizAction.ResultStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    BizAction.MediaDetails.ID = (Int64)dbServer.GetParameterValue(command, "ID");


                    if (BizAction.ResultStatus == 3)
                    {
                        item.StockDetails.BatchID = item.BatchID;
                        item.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                        item.StockDetails.ItemID = item.ItemID;
                        item.StockDetails.TransactionTypeID = InventoryTransactionType.OTDetails;
                        item.StockDetails.TransactionID = BizAction.MediaDetails.ID;
                        item.StockDetails.TransactionQuantity = (double)(item.VolumeUsed);
                        if (DALHelper.HandleDBNull(item.Date) == null)
                            item.StockDetails.Date = DateTime.Now;
                        else
                            item.StockDetails.Date = Convert.ToDateTime(item.Date);
                        if (DALHelper.HandleDBNull(item.Date) == null)
                            item.StockDetails.Time = DateTime.Now;
                        else
                            item.StockDetails.Time = Convert.ToDateTime(item.Date);
                        item.StockDetails.StoreID = item.StoreID;

                        clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                        clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

                        obj.Details = item.StockDetails;
                        obj.Details.ID = 0;
                        obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

                        if (obj.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }

                        item.StockDetails.ID = obj.Details.ID;
                    }
                    trans.Commit();
                }

            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizAction.SuccessStatus = -1;
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;
            }
            return BizAction;
        }

        public override IValueObject GetMediaDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetMediaDetailsBizActionVO BizAction = valueObject as clsIVFDashboard_GetMediaDetailsBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetMediaDetails");

                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.PatientUnitID);
                dbServer.AddInParameter(command, "ProcedureName", DbType.String, BizAction.ProcedureName);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_MediaDetailsVO MediaDetails = new clsIVFDashboard_MediaDetailsVO();
                        MediaDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        MediaDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        MediaDetails.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["LotNo"]));
                        MediaDetails.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        MediaDetails.Company = Convert.ToString(DALHelper.HandleDBNull(reader["Company"]));
                        MediaDetails.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                        MediaDetails.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                        MediaDetails.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        MediaDetails.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["MediaName"]));
                        MediaDetails.OSM = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OSM"]));
                        MediaDetails.PH = Convert.ToBoolean(DALHelper.HandleDBNull(reader["PH"]));
                        MediaDetails.Status = Convert.ToString(DALHelper.HandleDBNull(reader["Status"]));
                        MediaDetails.StatusIS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["StatusIS"]));
                        MediaDetails.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        MediaDetails.VolumeUsed = Convert.ToInt64(DALHelper.HandleDBNull(reader["VolumeUsed"]));
                        MediaDetails.Finalized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Finalized"]));
                        MediaDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        MediaDetails.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        MediaDetails.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        MediaDetails.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        MediaDetails.AvailableQuantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["AvailableQuantity"]));
                        BizAction.MediaDetailsList.Add(MediaDetails);
                    }
                }
                //trans.Commit();
            }
            catch (Exception)
            {
                con.Close();
                BizAction.SuccessStatus = -1;
                throw;
            }
            finally
            {
                con.Close();
            }
            return BizAction;
        }
        #endregion

        #region Graphical Representation
        public override IValueObject AddUpdateGraphicalRepList(IValueObject valueObject, clsUserVO UserVo)
        {
            throw new NotImplementedException();
        }
        public override IValueObject GetGraphicalRepOocList(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            cls_IVFDashboar_GetGraphicalRepBizActionVO BizActionObj = valueObject as cls_IVFDashboar_GetGraphicalRepBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetGraphicalRepOocList");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Details.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Details.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Details.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Details.PlanTherapyUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.GraphicalOocList == null)
                        BizActionObj.GraphicalOocList = new List<cls_IVFDashboard_GraphicalRepresentationVO>();
                    while (reader.Read())
                    {
                        cls_IVFDashboard_GraphicalRepresentationVO Obj = new cls_IVFDashboard_GraphicalRepresentationVO();
                        Obj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        Obj.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        Obj.PatientID = (long)DALHelper.HandleDBNull(reader["PatientID"]);
                        Obj.PatientUnitID = (long)DALHelper.HandleDBNull(reader["PatientUnitID"]);
                        Obj.PlanTherapyID = (long)DALHelper.HandleDBNull(reader["PlanTherapyID"]);
                        Obj.PlanTherapyUnitID = (long)DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]);
                        Obj.OocyteNumber = (long)DALHelper.HandleDBNull(reader["OocyteNumber"]);
                        Obj.SerialOocyteNumber = (long)DALHelper.HandleDBNull(reader["SerialOocyteNumber"]);
                        Obj.Day0 = (bool?)DALHelper.HandleDBNull(reader["Day0"]);
                        Obj.Day1 = (bool?)DALHelper.HandleDBNull(reader["Day1"]);
                        Obj.Day2 = (bool?)DALHelper.HandleDBNull(reader["Day2"]);
                        Obj.Day3 = (bool?)DALHelper.HandleDBNull(reader["Day3"]);
                        Obj.Day4 = (bool?)DALHelper.HandleDBNull(reader["Day4"]);
                        Obj.Day5 = (bool?)DALHelper.HandleDBNull(reader["Day5"]);
                        Obj.Day6 = (bool?)DALHelper.HandleDBNull(reader["Day6"]);
                        Obj.Day7 = (bool?)DALHelper.HandleDBNull(reader["Day7"]);
                        Obj.Day0CellStage = (long)DALHelper.HandleIntegerNull(reader["Day0CellStage"]);
                        Obj.Day1CellStage = (long)DALHelper.HandleIntegerNull(reader["Day1CellStage"]);
                        Obj.Day2CellStage = (long)DALHelper.HandleIntegerNull(reader["Day2CellStage"]);
                        Obj.Day3CellStage = (long)DALHelper.HandleIntegerNull(reader["Day3CellStage"]);
                        Obj.Day4CellStage = (long)DALHelper.HandleIntegerNull(reader["Day4CellStage"]);
                        Obj.Day5CellStage = (long)DALHelper.HandleIntegerNull(reader["Day5CellStage"]);
                        Obj.Day6CellStage = (long)DALHelper.HandleIntegerNull(reader["Day6CellStage"]);
                        Obj.Day7CellStage = (long)DALHelper.HandleIntegerNull(reader["Day7CellStage"]);
                        // BY BHUSHAN..
                        Obj.ImpressionDay0 = (string)DALHelper.HandleDBNull(reader["ImpressionDay0"]);
                        Obj.ImpressionDay1 = (string)DALHelper.HandleDBNull(reader["ImpressionDay1"]);
                        Obj.ImpressionDay2 = (string)DALHelper.HandleDBNull(reader["ImpressionDay2"]);
                        Obj.ImpressionDay3 = (string)DALHelper.HandleDBNull(reader["ImpressionDay3"]);
                        Obj.ImpressionDay4 = (string)DALHelper.HandleDBNull(reader["ImpressionDay4"]);
                        Obj.ImpressionDay5 = (string)DALHelper.HandleDBNull(reader["ImpressionDay5"]);
                        Obj.ImpressionDay6 = (string)DALHelper.HandleDBNull(reader["ImpressionDay6"]);
                        Obj.ImpressionDay7 = (string)DALHelper.HandleDBNull(reader["ImpressionDay7"]);
                        //By Anjali..............................

                        Obj.Plan0 = (string)DALHelper.HandleDBNull(reader["Plan0"]);
                        Obj.Plan1 = (string)DALHelper.HandleDBNull(reader["Plan1"]);
                        Obj.Plan2 = (string)DALHelper.HandleDBNull(reader["Plan2"]);
                        Obj.Plan3 = (string)DALHelper.HandleDBNull(reader["Plan3"]);
                        Obj.Plan4 = (string)DALHelper.HandleDBNull(reader["Plan4"]);
                        Obj.Plan5 = (string)DALHelper.HandleDBNull(reader["Plan5"]);
                        Obj.Plan6 = (string)DALHelper.HandleDBNull(reader["Plan6"]);
                        Obj.Plan7 = (string)DALHelper.HandleDBNull(reader["Plan7"]);
                        //by neena
                        Obj.CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"]));
                        Obj.FertCheck = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertCheck"]));
                        Obj.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        Obj.IsLabDay0Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay0Freezed"]));
                        Obj.ProcedureDate = (DateTime?)(DALHelper.HandleDate(reader["ProcedureDate"]));
                        Obj.ProcedureTime = (DateTime?)(DALHelper.HandleDate(reader["ProcedureTime"]));
                        Obj.IsLabDay1Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay1Freezed"]));
                        Obj.IsLabDay2Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay2Freezed"]));
                        Obj.IsLabDay3Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay3Freezed"]));
                        Obj.IsLabDay4Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay4Freezed"]));
                        Obj.IsLabDay5Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay5Freezed"]));
                        Obj.IsLabDay6Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay6Freezed"]));
                        Obj.IsLabDay7Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LabDay7Freezed"]));

                        Obj.GradeIDDay1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeIDDay1"]));
                        Obj.GradeIDDay2 = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeIDDay2"]));
                        Obj.GradeIDDay3 = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeIDDay3"]));
                        Obj.GradeIDDay4 = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeIDDay4"]));
                        Obj.StageofDevelopmentGradeDay5 = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeDay5"]));
                        Obj.InnerCellMassGradeDay5 = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGradeDay5"]));
                        Obj.TrophoectodermGradeDay5 = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGradeDay5"]));
                        Obj.StageofDevelopmentGradeDay6 = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeDay6"]));
                        Obj.InnerCellMassGradeDay6 = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGradeDay6"]));
                        Obj.TrophoectodermGradeDay6 = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGradeDay6"]));
                        Obj.StageofDevelopmentGradeDay7 = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeDay7"]));
                        Obj.InnerCellMassGradeDay7 = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGradeDay7"]));
                        Obj.TrophoectodermGradeDay7 = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGradeDay7"]));

                        Obj.CellStageDay1 = (string)DALHelper.HandleDBNull(reader["CellStageDay1"]);
                        Obj.CellStageDay2 = (string)DALHelper.HandleDBNull(reader["CellStageDay2"]);
                        Obj.CellStageDay3 = (string)DALHelper.HandleDBNull(reader["CellStageDay3"]);
                        Obj.CellStageDay4 = (string)DALHelper.HandleDBNull(reader["CellStageDay4"]);
                        Obj.CellStageDay5 = (string)DALHelper.HandleDBNull(reader["CellStageDay5"]);
                        Obj.CellStageDay6 = (string)DALHelper.HandleDBNull(reader["CellStageDay6"]);
                        Obj.CellStageDay7 = (string)DALHelper.HandleDBNull(reader["CellStageDay7"]);
                        Obj.IsExtendedCulture = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExtendedCulture"]));
                        Obj.IsExtendedCultureFromOtherCycle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExtendedCultureFromOtherCycle"]));

                        Obj.ObservationDate1 = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate1"]));
                        Obj.ObservationTime1 = (DateTime?)(DALHelper.HandleDate(reader["CellObservationTime1"]));

                        Obj.ObservationDate2 = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate2"]));
                        Obj.ObservationTime2 = (DateTime?)(DALHelper.HandleDate(reader["CellObservationTime2"]));


                        Obj.ObservationDate3 = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate3"]));
                        Obj.ObservationTime3 = (DateTime?)(DALHelper.HandleDate(reader["CellObservationTime3"]));


                        Obj.ObservationDate4 = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate4"]));
                        Obj.ObservationTime4 = (DateTime?)(DALHelper.HandleDate(reader["CellObservationTime4"]));


                        Obj.ObservationDate5 = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate5"]));
                        Obj.ObservationTime5 = (DateTime?)(DALHelper.HandleDate(reader["CellObservationTime5"]));


                        Obj.ObservationDate6 = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate6"]));
                        Obj.ObservationTime6 = (DateTime?)(DALHelper.HandleDate(reader["CellObservationTime6"]));

                        Obj.ObservationDate7 = (DateTime?)(DALHelper.HandleDate(reader["CellObservationDate7"]));
                        Obj.ObservationTime7 = (DateTime?)(DALHelper.HandleDate(reader["CellObservationTime7"]));

                        Obj.PatientName = (string)DALHelper.HandleDBNull(reader["RecepientPatientName"]);
                        Obj.MRNO = (string)DALHelper.HandleDBNull(reader["RecepientMRNO"]);

                        Obj.BlDay3PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day3PGDPGS"]));
                        Obj.BlFrozenDay3PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day3PGDPGSFrozen"]));
                        if (Obj.BlDay3PGDPGS || Obj.BlFrozenDay3PGDPGS)
                            Obj.Day3PGDPGS = "PGD/PGS";

                        Obj.BlDay5PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day5PGDPGS"]));
                        Obj.BlFrozenDay5PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day5PGDPGSFrozen"]));
                        if (Obj.BlDay5PGDPGS || Obj.BlFrozenDay5PGDPGS)
                            Obj.Day5PGDPGS = "PGD/PGS";

                        Obj.BlDay6PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day6PGDPGS"]));
                        Obj.BlFrozenDay6PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day6PGDPGSFrozen"]));
                        if (Obj.BlDay6PGDPGS || Obj.BlFrozenDay6PGDPGS)
                            Obj.Day6PGDPGS = "PGD/PGS";

                        Obj.BlDay7PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day7PGDPGS"]));
                        Obj.BlFrozenDay7PGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Day7PGDPGSFrozen"]));
                        if (Obj.BlDay7PGDPGS || Obj.BlFrozenDay7PGDPGS)
                            Obj.Day7PGDPGS = "PGD/PGS";

                        Obj.IsFrozenEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFrozenEmbryoPGDPGS"]));


                        if (Obj.Plan0 != null)
                        {
                            if (Obj.IsLabDay0Freezed)
                                if (Obj.Plan0.Equals("IVF") || Obj.Plan0.Equals("ICSI"))
                                    Obj.IsFertCheck = true;
                        }

                        if (Obj.Day1 == true)
                        {
                            Obj.Day1Visible = true;
                        }

                        if (Obj.Day2 == true)
                        {
                            Obj.Day2Visible = true;
                        }

                        if (Obj.Day3 == true)
                        {
                            Obj.Day3Visible = true;
                        }

                        if (Obj.Day4 == true)
                        {
                            Obj.Day4Visible = true;
                        }

                        if (Obj.Day5 == true)
                        {
                            Obj.Day5Visible = true;
                        }

                        if (Obj.Day6 == true)
                        {
                            Obj.Day6Visible = true;
                        }

                        if (Obj.Day6 == true)
                        {
                            Obj.Day6Visible = true;
                        }

                        if (Obj.Day7 == true)
                        {
                            Obj.Day7Visible = true;
                        }
                        //
                        Obj.DecisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DecisionID"]));
                        List<ClsAddObervationEmbryo> EmbryoDayObservation = new List<ClsAddObervationEmbryo>();
                        //Obj.EmbryoDayObservation = new List<ClsAddObervationEmbryo>();
                        if (Obj.Day1==true && Obj.IsLabDay1Freezed == false)
                        {
                            ClsAddObervationEmbryo objEmb = new ClsAddObervationEmbryo();
                            objEmb.Day = Obj.Day1;
                            objEmb.StrDay = "Day1";
                            objEmb.IsFreezed = Obj.IsLabDay1Freezed;
                            objEmb.ServerDate = (DateTime?)(DALHelper.HandleDate(reader["ServerObservationDate1"]));
                            EmbryoDayObservation.Add(objEmb);
                        }
                        if (Obj.Day2 == true && Obj.IsLabDay2Freezed == false)
                        {
                            ClsAddObervationEmbryo objEmb = new ClsAddObervationEmbryo();
                            objEmb.Day = Obj.Day2;
                            objEmb.StrDay = "Day2";
                            objEmb.IsFreezed = Obj.IsLabDay2Freezed;
                            objEmb.ServerDate = (DateTime?)(DALHelper.HandleDate(reader["ServerObservationDate2"]));
                            EmbryoDayObservation.Add(objEmb);
                        }

                        if (Obj.Day3 == true && Obj.IsLabDay3Freezed == false)
                        {
                            ClsAddObervationEmbryo objEmb = new ClsAddObervationEmbryo();
                            objEmb.Day = Obj.Day3;
                            objEmb.StrDay = "Day3";
                            objEmb.IsFreezed = Obj.IsLabDay3Freezed;
                            objEmb.ServerDate = (DateTime?)(DALHelper.HandleDate(reader["ServerObservationDate3"]));
                            EmbryoDayObservation.Add(objEmb);
                        }

                        if (Obj.Day4 == true && Obj.IsLabDay4Freezed == false)
                        {
                            ClsAddObervationEmbryo objEmb = new ClsAddObervationEmbryo();
                            objEmb.Day = Obj.Day4;
                            objEmb.StrDay = "Day4";
                            objEmb.IsFreezed = Obj.IsLabDay4Freezed;
                            objEmb.ServerDate = (DateTime?)(DALHelper.HandleDate(reader["ServerObservationDate4"]));
                            EmbryoDayObservation.Add(objEmb);
                        }
                        if (Obj.Day5 == true && Obj.IsLabDay5Freezed == false)
                        {
                            ClsAddObervationEmbryo objEmb = new ClsAddObervationEmbryo();
                            objEmb.Day = Obj.Day5;
                            objEmb.StrDay = "Day5";
                            objEmb.IsFreezed = Obj.IsLabDay5Freezed;
                            objEmb.ServerDate = (DateTime?)(DALHelper.HandleDate(reader["ServerObservationDate5"]));
                            EmbryoDayObservation.Add(objEmb);
                        }
                        if (Obj.Day6 == true && Obj.IsLabDay6Freezed == false)
                        {
                            ClsAddObervationEmbryo objEmb = new ClsAddObervationEmbryo();
                            objEmb.Day = Obj.Day6;
                            objEmb.StrDay = "Day6";
                            objEmb.IsFreezed = Obj.IsLabDay6Freezed;
                            objEmb.ServerDate = (DateTime?)(DALHelper.HandleDate(reader["ServerObservationDate6"]));
                            EmbryoDayObservation.Add(objEmb);
                        }
                        if (Obj.Day7 == true && Obj.IsLabDay7Freezed == false)
                        {
                            ClsAddObervationEmbryo objEmb = new ClsAddObervationEmbryo();
                            objEmb.Day = Obj.Day7;
                            objEmb.StrDay = "Day7";
                            objEmb.IsFreezed = Obj.IsLabDay7Freezed;
                            objEmb.ServerDate = (DateTime?)(DALHelper.HandleDate(reader["ServerObservationDate7"]));
                            EmbryoDayObservation.Add(objEmb);
                        }
                        Obj.EmbryoDayObservation = EmbryoDayObservation;
                        BizActionObj.GraphicalOocList.Add(Obj);
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

       

        #endregion

        public override IValueObject AddLabDayDocuments(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateDay3BizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateDay3BizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;

            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.Day3Details.DetailList != null && BizActionObj.Day3Details.DetailList.Count > 0)
                {
                    foreach (var item in BizActionObj.Day3Details.DetailList)
                    {
                        DbCommand command8 = dbServer.GetStoredProcCommand("IVFDashboard_AddLabDaysDocument");

                        dbServer.AddInParameter(command8, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command8, "DocumentDate", DbType.DateTime, item.Date);
                        dbServer.AddInParameter(command8, "PatientID", DbType.Int64, item.PatientID);
                        dbServer.AddInParameter(command8, "PatientUnitID", DbType.Int64, item.PatientUnitID);
                        dbServer.AddInParameter(command8, "PlanTherapyID", DbType.Int64, item.PlanTherapyID);
                        dbServer.AddInParameter(command8, "PlanTherapyUnitID", DbType.Int64, item.PlanTherapyUnitID);
                        dbServer.AddInParameter(command8, "Description", DbType.String, item.Description);
                        dbServer.AddInParameter(command8, "Title", DbType.String, item.Title);
                        dbServer.AddInParameter(command8, "AttachedFileName", DbType.String, item.AttachedFileName);
                        dbServer.AddInParameter(command8, "AttachedFileContent", DbType.Binary, item.AttachedFileContent);
                        dbServer.AddInParameter(command8, "Status", DbType.Boolean, item.Status);
                        dbServer.AddInParameter(command8, "CreatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                        dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "UpdatedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command8, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command8, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        dbServer.AddInParameter(command8, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command8, "OocyteNumber", DbType.Int64, item.OocyteNumber);
                        dbServer.AddInParameter(command8, "SerialOocyteNumber", DbType.Int64, item.SerialOocyteNumber);
                        dbServer.AddInParameter(command8, "Day", DbType.Int64, item.Day);
                        //dbServer.AddInParameter(command8, "DocNo", DbType.String, item.DocNo);
                        dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddInParameter(command8, "IsApplyTo", DbType.Int32, 0);
                        if (item.DocNo == null)
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
                        else
                            dbServer.AddParameter(command8, "DocNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.DocNo);

                        //dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus8 = dbServer.ExecuteNonQuery(command8);
                        item.DocNo = Convert.ToString(dbServer.GetParameterValue(command8, "DocNo"));
                        //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Day3Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;
        }


        #region ET

        public override IValueObject AddUpdateETDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_AddUpdateEmbryoTansferBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateEmbryoTansferBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.ETDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.ETDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.ETDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.ETDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.ETDetails.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionObj.ETDetails.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.ETDetails.EmbryologistID);
                dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, BizActionObj.ETDetails.AssitantEmbryologistID);
                dbServer.AddInParameter(command, "PatternID", DbType.Int64, BizActionObj.ETDetails.PatternID);
                dbServer.AddInParameter(command, "UterineArtery_PI", DbType.Boolean, BizActionObj.ETDetails.UterineArtery_PI);
                dbServer.AddInParameter(command, "UterineArtery_RI", DbType.Boolean, BizActionObj.ETDetails.UterineArtery_RI);
                dbServer.AddInParameter(command, "UterineArtery_SD", DbType.Boolean, BizActionObj.ETDetails.UterineArtery_SD);
                dbServer.AddInParameter(command, "Endometerial_PI", DbType.Boolean, BizActionObj.ETDetails.Endometerial_PI);
                dbServer.AddInParameter(command, "Endometerial_RI", DbType.Boolean, BizActionObj.ETDetails.Endometerial_RI);
                dbServer.AddInParameter(command, "Endometerial_SD", DbType.Boolean, BizActionObj.ETDetails.Endometerial_SD);
                dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, BizActionObj.ETDetails.CatheterTypeID);
                dbServer.AddInParameter(command, "DistanceFundus", DbType.Decimal, BizActionObj.ETDetails.DistanceFundus);
                dbServer.AddInParameter(command, "EndometriumThickness", DbType.Decimal, BizActionObj.ETDetails.EndometriumThickness);
                dbServer.AddInParameter(command, "TeatmentUnderGA", DbType.Boolean, BizActionObj.ETDetails.TeatmentUnderGA);
                dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, BizActionObj.ETDetails.Difficulty);
                dbServer.AddInParameter(command, "DifficultyID", DbType.Int64, BizActionObj.ETDetails.DifficultyID);
                dbServer.AddInParameter(command, "TenaculumUsed", DbType.Boolean, BizActionObj.ETDetails.TenaculumUsed);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.ETDetails.IsFreezed);
                dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, BizActionObj.ETDetails.IsOnlyET);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, BizActionObj.ETDetails.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "AnethetistID", DbType.Int64, BizActionObj.ETDetails.AnethetistID);
                dbServer.AddInParameter(command, "AssistantAnethetistID", DbType.Int64, BizActionObj.ETDetails.AssistantAnethetistID);
                dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, BizActionObj.ETDetails.SrcOoctyID);
                dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, BizActionObj.ETDetails.SrcSemenID);
                dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, BizActionObj.ETDetails.SrcOoctyCode);
                dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, BizActionObj.ETDetails.SrcSemenCode);
                dbServer.AddInParameter(command, "FromForm", DbType.Int64, 2);

                //added by neena
                dbServer.AddInParameter(command, "IsAnesthesia", DbType.Boolean, BizActionObj.ETDetails.IsAnesthesia);
                dbServer.AddInParameter(command, "FreshEmb", DbType.Int64, BizActionObj.ETDetails.FreshEmb);
                dbServer.AddInParameter(command, "FrozenEmb", DbType.Int64, BizActionObj.ETDetails.FrozenEmb);
                //

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ETDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ETDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.ETDetailsList != null && BizActionObj.ETDetailsList.Count > 0)
                {
                    foreach (var ObjDetails in BizActionObj.ETDetailsList)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                        dbServer.AddInParameter(command2, "ID", DbType.Int64, ObjDetails.ID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "ETID", DbType.Int64, BizActionObj.ETDetails.ID);
                        dbServer.AddInParameter(command2, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, ObjDetails.OocyteNumber);
                        dbServer.AddInParameter(command2, "SerialOocyteNumber", DbType.Int64, ObjDetails.SerialOocyteNumber);
                        dbServer.AddInParameter(command2, "TransferDate", DbType.DateTime, ObjDetails.TransferDate);
                        dbServer.AddInParameter(command2, "TransferDay", DbType.String, ObjDetails.TransferDay);
                        dbServer.AddInParameter(command2, "GradeID", DbType.Int64, ObjDetails.GradeID);
                        dbServer.AddInParameter(command2, "Score", DbType.String, ObjDetails.Score);
                        dbServer.AddInParameter(command2, "FertStageID", DbType.Int64, ObjDetails.FertStageID);
                        dbServer.AddInParameter(command2, "EmbStatus", DbType.String, ObjDetails.EmbStatus);
                        dbServer.AddInParameter(command2, "FileName", DbType.String, ObjDetails.FileName);
                        dbServer.AddInParameter(command2, "FileContents", DbType.Binary, ObjDetails.FileContents);
                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, ObjDetails.Status);
                        dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                        dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);

                        //added by neena
                        dbServer.AddInParameter(command2, "EmbTransferDay", DbType.Int64, ObjDetails.EmbTransferDay);
                        dbServer.AddInParameter(command2, "Remark", DbType.String, ObjDetails.Remark);
                        dbServer.AddInParameter(command2, "GradeID_New", DbType.Int64, ObjDetails.CleavageGrade);
                        dbServer.AddInParameter(command2, "StageofDevelopmentGrade", DbType.Int64, ObjDetails.StageofDevelopmentGrade);
                        dbServer.AddInParameter(command2, "InnerCellMassGrade", DbType.Int64, ObjDetails.InnerCellMassGrade);
                        dbServer.AddInParameter(command2, "TrophoectodermGrade", DbType.Int64, ObjDetails.TrophoectodermGrade);
                        dbServer.AddInParameter(command2, "CellStage", DbType.String, ObjDetails.CellStage);

                        dbServer.AddInParameter(command2, "SurrogateID", DbType.Int64, ObjDetails.SurrogateID);
                        dbServer.AddInParameter(command2, "SurrogateUnitID", DbType.Int64, ObjDetails.SurrogateUnitID);
                        dbServer.AddInParameter(command2, "SurrogatePatientMrNo", DbType.String, ObjDetails.SurrogatePatientMrNo);
                        //

                        int iStatus = dbServer.ExecuteNonQuery(command2);
                    }
                }
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                BizActionObj.ETDetails = null;
            }
            finally
            {
                con.Close();
            }

            return BizActionObj;

        }
        public override IValueObject GetETDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_GetEmbryoTansferBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetEmbryoTansferBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_GetETDetails");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.ETDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.ETDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.ETDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.ETDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "IsOnlyET", DbType.Boolean, BizAction.ETDetails.IsOnlyET);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.ETDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.ETDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.ETDetails.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                        BizAction.ETDetails.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.ETDetails.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssisEmbryologistID"]));
                        BizAction.ETDetails.Time = (DateTime?)(DALHelper.HandleDate(reader["Time"]));
                        BizAction.ETDetails.EndometriumThickness = Convert.ToDecimal(DALHelper.HandleDBNull(reader["EndometriumThickness"]));
                        BizAction.ETDetails.PatternID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatternID"]));
                        BizAction.ETDetails.Endometerial_PI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Endometerial_PI"]));
                        BizAction.ETDetails.Endometerial_RI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Endometerial_RI"]));
                        BizAction.ETDetails.Endometerial_SD = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Endometerial_SD"]));
                        BizAction.ETDetails.UterineArtery_PI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UterineArtery_PI"]));
                        BizAction.ETDetails.UterineArtery_RI = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UterineArtery_RI"]));
                        BizAction.ETDetails.UterineArtery_SD = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UterineArtery_SD"]));
                        BizAction.ETDetails.DistanceFundus = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DistanceFundus"]));
                        BizAction.ETDetails.CatheterTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CatheterTypeID"]));
                        BizAction.ETDetails.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        BizAction.ETDetails.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizAction.ETDetails.TenaculumUsed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TenaculumUsed"]));
                        BizAction.ETDetails.TeatmentUnderGA = Convert.ToBoolean(DALHelper.HandleDBNull(reader["TeatmentUnderGA"]));
                        BizAction.ETDetails.Difficulty = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Difficulty"]));
                        BizAction.ETDetails.DifficultyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DifficultyID"]));
                        BizAction.ETDetails.AnethetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnethetistID"]));
                        BizAction.ETDetails.AssistantAnethetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantAnethetistID"]));
                        BizAction.ETDetails.SrcOoctyCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcOoctyCode"]));
                        BizAction.ETDetails.SrcSemenCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcSemenCode"]));
                        BizAction.ETDetails.SrcOoctyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcOoctyID"]));
                        BizAction.ETDetails.SrcSemenID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcSemenID"]));

                        //added by neena
                        BizAction.ETDetails.IsAnesthesia = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAnesthesia"]));
                        BizAction.ETDetails.FreshEmb = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreshEmb"]));
                        BizAction.ETDetails.FrozenEmb = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrozenEmb"]));
                        //


                    }
                }

                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashboard_EmbryoTransferDetailsVO ETdetails = new clsIVFDashboard_EmbryoTransferDetailsVO();
                        ETdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ETdetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        ETdetails.ET_ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ET_ID"]));
                        ETdetails.ET_UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ET_UnitID"]));
                        ETdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                        ETdetails.TransferDate = (DateTime?)(DALHelper.HandleDate(reader["TransferDate"]));
                        ETdetails.FertStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertStageID"]));
                        ETdetails.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        //ETdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                        ETdetails.FertStage = Convert.ToString(DALHelper.HandleDBNull(reader["FertStage"]));
                        ETdetails.Score = Convert.ToString(DALHelper.HandleDBNull(reader["Score"]));
                        ETdetails.EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                        ETdetails.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        ETdetails.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"]));
                        ETdetails.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"]));
                        ETdetails.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ETdetails.FileContents = (byte[])(DALHelper.HandleDBNull(reader["FileContents"]));

                        ETdetails.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        ETdetails.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));

                        //added by neena
                        ETdetails.EmbTransferDay = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbTransferDay"]));
                        ETdetails.ServerTransferDate = (DateTime?)(DALHelper.HandleDate(reader["ServerTransferDate"]));
                        ETdetails.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        ETdetails.CleavageGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID_New"]));
                        ETdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                        ETdetails.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        ETdetails.StageofDevelopmentGradeDesc = Convert.ToString(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeDesc"]));
                        ETdetails.InnerCellMassGradeDesc = Convert.ToString(DALHelper.HandleDBNull(reader["InnerCellMassGradeDesc"]));
                        ETdetails.TrophoectodermGradeDesc = Convert.ToString(DALHelper.HandleDBNull(reader["TrophoectodermGradeDesc"]));
                        ETdetails.StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"]));
                        ETdetails.InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"]));
                        ETdetails.TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"]));
                        ETdetails.SurrogateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateID"]));
                        ETdetails.SurrogateUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SurrogateUnitID"]));
                        ETdetails.SurrogatePatientMrNo = Convert.ToString(DALHelper.HandleDBNull(reader["SurrogatePatientMrNo"]));
                        ETdetails.IsFreshEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreshEmbryoPGDPGS"]));
                        ETdetails.IsFrozenEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFrozenEmbryoPGDPGS"]));
                        //

                        BizAction.ETDetailsList.Add(ETdetails);
                    }
                }


                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.ETDetails.FreshEmb = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreshEmb"]));
                        BizAction.ETDetails.FrozenEmb = Convert.ToInt64(DALHelper.HandleDBNull(reader["FrozenEmb"]));
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

        public override IValueObject AddUpdateFertCheckDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateFertCheckBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateFertCheckBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateFertCheck");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.FertCheckDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.FertCheckDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.FertCheckDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.FertCheckDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizActionObj.FertCheckDetails.SerialOocyteNumber);
                dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, BizActionObj.FertCheckDetails.OocyteNumber);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.FertCheckDetails.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionObj.FertCheckDetails.Time);

                dbServer.AddInParameter(command, "Isfreezed", DbType.Boolean, BizActionObj.FertCheckDetails.Isfreezed);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "FertCheck", DbType.Int64, BizActionObj.FertCheckDetails.FertCheck);
                dbServer.AddInParameter(command, "FertCheckResult", DbType.Int64, BizActionObj.FertCheckDetails.FertCheckResult);
                dbServer.AddInParameter(command, "Remarks", DbType.String, BizActionObj.FertCheckDetails.Remarks);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.FertCheckDetails.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.FertCheckDetails.ID = (long)dbServer.GetParameterValue(command, "ID");

                //added by neena dated 18/5/16
                if (BizActionObj.FertCheckDetails.OcyteListList != null)
                {
                    foreach (var item in BizActionObj.FertCheckDetails.OcyteListList)
                    {
                        DbCommand command7 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateFertCheck");
                        dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command7, "PatientID", DbType.Int64, BizActionObj.FertCheckDetails.PatientID);
                        dbServer.AddInParameter(command7, "PatientUnitID", DbType.Int64, BizActionObj.FertCheckDetails.PatientUnitID);
                        dbServer.AddInParameter(command7, "PlanTherapyID", DbType.Int64, BizActionObj.FertCheckDetails.PlanTherapyID);
                        dbServer.AddInParameter(command7, "PlanTherapyUnitID", DbType.Int64, BizActionObj.FertCheckDetails.PlanTherapyUnitID);
                        dbServer.AddInParameter(command7, "SerialOocyteNumber", DbType.Int64, BizActionObj.FertCheckDetails.SerialOocyteNumber + item.FilterID);
                        dbServer.AddInParameter(command7, "OocyteNumber", DbType.Int64, item.ID);
                        dbServer.AddInParameter(command7, "Date", DbType.DateTime, BizActionObj.FertCheckDetails.Date);
                        dbServer.AddInParameter(command7, "Time", DbType.DateTime, BizActionObj.FertCheckDetails.Time);

                        dbServer.AddInParameter(command7, "Isfreezed", DbType.Boolean, BizActionObj.FertCheckDetails.Isfreezed);
                        dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        dbServer.AddInParameter(command7, "FertCheck", DbType.Int64, BizActionObj.FertCheckDetails.FertCheck);
                        dbServer.AddInParameter(command7, "FertCheckResult", DbType.Int64, BizActionObj.FertCheckDetails.FertCheckResult);
                        dbServer.AddInParameter(command7, "Remarks", DbType.String, BizActionObj.FertCheckDetails.Remarks);

                        dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.FertCheckDetails.ID);
                        dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus7 = dbServer.ExecuteNonQuery(command7, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command7, "ResultStatus");
                        BizActionObj.FertCheckDetails.ID = (long)dbServer.GetParameterValue(command7, "ID");

                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.FertCheckDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetFertCheckDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetFertCheckBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetFertCheckBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetFertCheck");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.FertCheckDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.FertCheckDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.FertCheckDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.FertCheckDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, BizAction.FertCheckDetails.OocyteNumber);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.FertCheckDetails.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                        BizAction.FertCheckDetails.Time = (DateTime?)(DALHelper.HandleDate(reader["Time"]));
                        BizAction.FertCheckDetails.Isfreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Isfreezed"]));
                        BizAction.FertCheckDetails.FertCheck = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertCheck"]));
                        BizAction.FertCheckDetails.FertCheckResult = Convert.ToInt64(DALHelper.HandleDBNull(reader["FertCheckResult"]));
                        BizAction.FertCheckDetails.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
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

        public override IValueObject GetFertCheckDate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetFertCheckBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetFertCheckBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_GetFertCheckDate");
                //dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.FertCheckDetails.PatientID);
                //dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.FertCheckDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.FertCheckDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.FertCheckDetails.PlanTherapyUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BizAction.FertCheckDetails.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
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

        public override IValueObject UpdateAndGetImageListDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO BizAction = (valueObject) as clsIVFDashboard_DeleteAndGetLabDayImagesBizActionVO;
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_UpdateLabDaysImagesListStatus");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.ImageObj.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.ImageObj.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.ImageObj.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.ImageObj.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, BizAction.ImageObj.OocyteNumber);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizAction.ImageObj.SerialOocyteNumber);
                dbServer.AddInParameter(command, "Day", DbType.Int64, BizAction.ImageObj.Day);
                //dbServer.AddInParameter(command, "ServerImageName", DbType.String, BizAction.ImageObj.ServerImageName);
                dbServer.AddInParameter(command, "OriginalImagePath", DbType.String, BizAction.ImageObj.OriginalImagePath);
                //dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //int intStatus = dbServer.ExecuteNonQuery(command);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //BizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizAction.ImageList = new List<clsAddImageVO>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAddImageVO ObjImg = new clsAddImageVO();
                        ObjImg.ImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ObjImg.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        //string imageName = Convert.ToString(DALHelper.HandleDBNull(reader["ServerImageName"]));
                        ObjImg.SeqNo = Convert.ToString(DALHelper.HandleDBNull(reader["SeqNo"]));
                        ObjImg.Photo = (byte[])(DALHelper.HandleDBNull(reader["Image"]));
                        ObjImg.Day = Convert.ToInt64(DALHelper.HandleDBNull(reader["Day"]));
                        ObjImg.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        ObjImg.SerialOocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["SerialOocyteNumber"]));
                        ObjImg.OocyteNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        ObjImg.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientID"]));
                        ObjImg.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FemalePatientUnitID"]));
                        ObjImg.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        ObjImg.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        ObjImg.OriginalImagePath = Convert.ToString(DALHelper.HandleDBNull(reader["ServerImageName"]));

                        if (!string.IsNullOrEmpty(ObjImg.OriginalImagePath))
                            ObjImg.ServerImageName = "..//" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;
                        //ObjImg.ServerImageName = "https://" + ImgIP + "/" + ImgVirtualDir + "/" + ObjImg.OriginalImagePath;


                        BizAction.ImageList.Add(ObjImg);



                    }
                }
                reader.Close();


                //if (BizAction.SuccessStatus == 1)
                //{
                //    DbCommand command1 = dbServer.GetStoredProcCommand("IVFDashBoard_GetLabDaysImagesListStatus");
                //    dbServer.AddInParameter(command1, "PatientID", DbType.Int64, BizAction.ImageObj.PatientID);
                //    dbServer.AddInParameter(command1, "PatientUnitID", DbType.Int64, BizAction.ImageObj.PatientUnitID);
                //    dbServer.AddInParameter(command1, "PlanTherapyID", DbType.Int64, BizAction.ImageObj.PlanTherapyID);
                //    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, BizAction.ImageObj.PlanTherapyUnitID);
                //    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command1, "OocyteNumber", DbType.Int64, BizAction.ImageObj.OocyteNumber);
                //    dbServer.AddInParameter(command1, "SerialOocyteNumber", DbType.Int64, BizAction.ImageObj.SerialOocyteNumber);
                //    dbServer.AddInParameter(command1, "Day", DbType.Int64, BizAction.ImageObj.Day);
                //    //dbServer.AddInParameter(command, "ServerImageName", DbType.String, BizAction.ImageObj.ServerImageName);
                //    dbServer.AddInParameter(command1, "OriginalImagePath", DbType.String, BizAction.ImageObj.OriginalImagePath);



                //}
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

        public override IValueObject AddUpdateDecision(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_IVFDashboard_AddUpdateDecisionBizActionVO BizActionObj = valueObject as cls_IVFDashboard_AddUpdateDecisionBizActionVO;
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDecision");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.ETDetails.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.ETDetails.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.ETDetails.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.ETDetails.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "SerialOocyteNumber", DbType.Int64, BizActionObj.ETDetails.SerialOocyteNumber);
                dbServer.AddInParameter(command, "OocyteNumber", DbType.Int64, BizActionObj.ETDetails.OocyteNumber);
                dbServer.AddInParameter(command, "DecisionID", DbType.Int64, BizActionObj.ETDetails.DecisionID);

                //flags added for donate and donae cryo from embryology dashboard --added by neena
                dbServer.AddInParameter(command, "IsDonate", DbType.Boolean, BizActionObj.ETDetails.IsDonate);
                dbServer.AddInParameter(command, "IsDonateCryo", DbType.Boolean, BizActionObj.ETDetails.IsDonateCryo);
                dbServer.AddInParameter(command, "RecepientPatientID", DbType.Int64, BizActionObj.ETDetails.RecepientPatientID);
                dbServer.AddInParameter(command, "RecepientPatientUnitID", DbType.Int64, BizActionObj.ETDetails.RecepientPatientUnitID);
                //added by neena for donate cycle
                dbServer.AddInParameter(command, "IsDonorCycleDonate", DbType.Boolean, BizActionObj.ETDetails.IsDonorCycleDonate);
                dbServer.AddInParameter(command, "IsDonorCycleDonateCryo", DbType.Boolean, BizActionObj.ETDetails.IsDonorCycleDonateCryo);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");


                if (BizActionObj.ETDetails.DecisionID == 3)
                {
                    //Add in ET table
                    DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.ETDetails.PatientID);
                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.ETDetails.PatientUnitID);
                    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.ETDetails.PlanTherapyID);
                    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.ETDetails.PlanTherapyUnitID);
                    dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "EmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                    dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                    dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                    //added by neena
                    dbServer.AddInParameter(command3, "IsAnesthesia", DbType.Boolean, false);
                    dbServer.AddInParameter(command3, "FreshEmb", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "FrozenEmb", DbType.Int64, null);
                    //

                    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                    BizActionObj.ETDetails.ID = (long)dbServer.GetParameterValue(command3, "ID");


                    DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                    dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "ETID", DbType.Int64, BizActionObj.ETDetails.ID);
                    dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, BizActionObj.ETDetails.OocyteNumber);
                    dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, BizActionObj.ETDetails.SerialOocyteNumber);
                    dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command6, "TransferDay", DbType.String, BizActionObj.ETDetails.Day);
                    dbServer.AddInParameter(command6, "GradeID", DbType.Int64, BizActionObj.ETDetails.GradeID);
                    dbServer.AddInParameter(command6, "Score", DbType.String, null);
                    dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, BizActionObj.ETDetails.CellStageID);
                    dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                    dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                    dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                    dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                    dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                    dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, BizActionObj.ETDetails.OocyteDonorID);
                    dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, BizActionObj.ETDetails.OocyteDonorUnitID);

                    //added by neena
                    dbServer.AddInParameter(command6, "EmbTransferDay", DbType.Int64, BizActionObj.ETDetails.DayNo);
                    dbServer.AddInParameter(command6, "Remark", DbType.String, BizActionObj.ETDetails.Remark);
                    dbServer.AddInParameter(command6, "GradeID_New", DbType.Int64, BizActionObj.ETDetails.GradeID);
                    dbServer.AddInParameter(command6, "StageofDevelopmentGrade", DbType.Int64, BizActionObj.ETDetails.StageofDevelopmentGrade);
                    dbServer.AddInParameter(command6, "InnerCellMassGrade", DbType.Int64, BizActionObj.ETDetails.InnerCellMassGrade);
                    dbServer.AddInParameter(command6, "TrophoectodermGrade", DbType.Int64, BizActionObj.ETDetails.TrophoectodermGrade);
                    dbServer.AddInParameter(command6, "CellStage", DbType.String, BizActionObj.ETDetails.CellStage);
                    dbServer.AddInParameter(command6, "IsFreshEmbryo", DbType.Boolean, BizActionObj.ETDetails.IsFreshEmbryo);
                    dbServer.AddInParameter(command6, "IsFrozenEmbryo", DbType.Boolean, BizActionObj.ETDetails.IsFrozenEmbryo);
                    dbServer.AddInParameter(command6, "IsFreshEmbryoPGDPGS", DbType.Boolean, BizActionObj.ETDetails.IsFreshEmbryoPGDPGS);
                    dbServer.AddInParameter(command6, "IsFrozenEmbryoPGDPGS", DbType.Boolean, BizActionObj.ETDetails.IsFrozenEmbryoPGDPGS);
                    //

                    int iStatus = dbServer.ExecuteNonQuery(command6, trans);
                }
                if (BizActionObj.ETDetails.DecisionID == 2)
                {
                    //Add in Vitrification table
                    DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.ETDetails.PatientID);
                    dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.ETDetails.PatientUnitID);
                    dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.ETDetails.PlanTherapyID);
                    dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.ETDetails.PlanTherapyUnitID);
                    dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                    dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                    dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddInParameter(command4, "IsFreezeOocytes", DbType.Boolean, false);

                    int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    BizActionObj.ETDetails.ID = (long)dbServer.GetParameterValue(command4, "ID");


                    DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                    dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.ETDetails.ID);
                    dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, BizActionObj.ETDetails.OocyteNumber);
                    dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.ETDetails.SerialOocyteNumber);
                    dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                    dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                    dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.ETDetails.PlanDate);
                    dbServer.AddInParameter(command5, "TransferDay", DbType.String, BizActionObj.ETDetails.Day);
                    dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.ETDetails.CellStageID);
                    dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.ETDetails.GradeID);
                    dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                    dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                    dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                    dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.ETDetails.OocyteDonorID);
                    dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.ETDetails.OocyteDonorUnitID);

                    dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                    dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                    //added by neena
                    dbServer.AddInParameter(command5, "TransferDayNo", DbType.Int64, BizActionObj.ETDetails.DayNo);
                    dbServer.AddInParameter(command5, "CleavageGrade", DbType.Int64, BizActionObj.ETDetails.GradeID);
                    dbServer.AddInParameter(command5, "StageofDevelopmentGrade", DbType.Int64, BizActionObj.ETDetails.StageofDevelopmentGrade);
                    dbServer.AddInParameter(command5, "InnerCellMassGrade", DbType.Int64, BizActionObj.ETDetails.InnerCellMassGrade);
                    dbServer.AddInParameter(command5, "TrophoectodermGrade", DbType.Int64, BizActionObj.ETDetails.TrophoectodermGrade);
                    dbServer.AddInParameter(command5, "CellStage", DbType.String, BizActionObj.ETDetails.CellStage);
                    dbServer.AddInParameter(command5, "VitrificationDate", DbType.DateTime, BizActionObj.ETDetails.VitrificationDate);
                    dbServer.AddInParameter(command5, "VitrificationTime", DbType.DateTime, BizActionObj.ETDetails.VitrificationTime);
                    dbServer.AddInParameter(command5, "VitrificationNo", DbType.String, BizActionObj.ETDetails.VitrificationNo);
                    dbServer.AddInParameter(command5, "IsFreezeOocytes", DbType.Boolean, false);
                    dbServer.AddInParameter(command5, "IsFreshEmbryoPGDPGS", DbType.Boolean, BizActionObj.ETDetails.IsFreshEmbryoPGDPGS);
                    dbServer.AddInParameter(command5, "IsFrozenEmbryoPGDPGS", DbType.Boolean, BizActionObj.ETDetails.IsFrozenEmbryoPGDPGS);

                    //

                    int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                }

                //added by neena for donate cryo plan
                if (BizActionObj.ETDetails.DecisionID == 5)
                {
                    //Add in Vitrification table
                    DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.ETDetails.PatientID);
                    dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.ETDetails.PatientUnitID);
                    dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.ETDetails.PlanTherapyID);
                    dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.ETDetails.PlanTherapyUnitID);
                    dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                    dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                    dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                    dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                    dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                    dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddInParameter(command4, "IsFreezeOocytes", DbType.Boolean, false);

                    int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    BizActionObj.ETDetails.ID = (long)dbServer.GetParameterValue(command4, "ID");


                    DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                    dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.ETDetails.ID);
                    dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, BizActionObj.ETDetails.OocyteNumber);
                    dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, BizActionObj.ETDetails.SerialOocyteNumber);
                    dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                    dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                    dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, BizActionObj.ETDetails.PlanDate);
                    dbServer.AddInParameter(command5, "TransferDay", DbType.String, BizActionObj.ETDetails.Day);
                    dbServer.AddInParameter(command5, "CellStageID", DbType.String, BizActionObj.ETDetails.CellStageID);
                    dbServer.AddInParameter(command5, "GradeID", DbType.Int64, BizActionObj.ETDetails.GradeID);
                    dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                    dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                    dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                    dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, BizActionObj.ETDetails.OocyteDonorID);
                    dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, BizActionObj.ETDetails.OocyteDonorUnitID);

                    dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                    dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                    dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                    //added by neena
                    dbServer.AddInParameter(command5, "TransferDayNo", DbType.Int64, BizActionObj.ETDetails.DayNo);
                    dbServer.AddInParameter(command5, "CleavageGrade", DbType.Int64, BizActionObj.ETDetails.GradeID);
                    dbServer.AddInParameter(command5, "StageofDevelopmentGrade", DbType.Int64, BizActionObj.ETDetails.StageofDevelopmentGrade);
                    dbServer.AddInParameter(command5, "InnerCellMassGrade", DbType.Int64, BizActionObj.ETDetails.InnerCellMassGrade);
                    dbServer.AddInParameter(command5, "TrophoectodermGrade", DbType.Int64, BizActionObj.ETDetails.TrophoectodermGrade);
                    dbServer.AddInParameter(command5, "CellStage", DbType.String, BizActionObj.ETDetails.CellStage);
                    dbServer.AddInParameter(command5, "VitrificationDate", DbType.DateTime, BizActionObj.ETDetails.VitrificationDate);
                    dbServer.AddInParameter(command5, "VitrificationTime", DbType.DateTime, BizActionObj.ETDetails.VitrificationTime);
                    dbServer.AddInParameter(command5, "VitrificationNo", DbType.String, BizActionObj.ETDetails.VitrificationNo);
                    dbServer.AddInParameter(command5, "IsFreezeOocytes", DbType.Boolean, false);
                    //
                    //added for donate cryo plan
                    dbServer.AddInParameter(command5, "IsDonateCryo", DbType.Boolean, BizActionObj.ETDetails.IsDonateCryo);
                    dbServer.AddInParameter(command5, "RecepientPatientID", DbType.Int64, BizActionObj.ETDetails.RecepientPatientID);
                    dbServer.AddInParameter(command5, "RecepientPatientUnitID", DbType.Int64, BizActionObj.ETDetails.RecepientPatientUnitID);

                    //for donar cycle donate cryo
                    dbServer.AddInParameter(command5, "IsDonorCycleDonateCryo", DbType.Boolean, BizActionObj.ETDetails.IsDonorCycleDonateCryo);
                    dbServer.AddInParameter(command5, "PatientID", DbType.Int64, BizActionObj.ETDetails.PatientID);
                    dbServer.AddInParameter(command5, "PatientUnitID", DbType.Int64, BizActionObj.ETDetails.PatientUnitID);
                    dbServer.AddInParameter(command5, "IsFreshEmbryoPGDPGS", DbType.Boolean, BizActionObj.ETDetails.IsFreshEmbryoPGDPGS);

                    int iStatus = dbServer.ExecuteNonQuery(command5, trans);
                }
                //

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }

        public override IValueObject AddUpdatePlanDecision(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_IVFDashboard_AddUpdatePlanDecisionBizActionVO BizActionObj = valueObject as cls_IVFDashboard_AddUpdatePlanDecisionBizActionVO;

            return BizActionObj;
        }

        public override IValueObject AddObervationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddObservationBizActionVO BizAction = (valueObject) as clsIVFDashboard_AddObservationBizActionVO;
            return BizAction;
        }


        #endregion
    }

}
