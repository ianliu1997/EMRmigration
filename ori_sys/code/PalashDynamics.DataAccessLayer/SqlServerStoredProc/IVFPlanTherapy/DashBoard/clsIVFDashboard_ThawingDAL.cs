using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.DashBoardVO;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    class clsIVFDashboard_ThawingDAL : clsBaseIVFDashboard_ThawingDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsIVFDashboard_ThawingDAL()
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

        public override IValueObject AddUpdateIVFDashBoard_Thawing(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_AddUpdateThawingBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateThawingBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawing");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, BizActionObj.Thawing.DateTime);
                dbServer.AddInParameter(command, "LabPersonId", DbType.Int64, BizActionObj.Thawing.LabPersonId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //added by neena
                // dbServer.AddInParameter(command, "Plan", DbType.Int64, BizActionObj.PostThawingPlan);
                //



                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Thawing.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Thawing.ID = (long)dbServer.GetParameterValue(command, "ID");

                //if (BizActionObj.ThawingDetailsList != null && BizActionObj.ThawingDetailsList.Count > 0)
                //{
                //    foreach (var ObjDetails in BizActionObj.ThawingDetailsList)
                //    {
                if (BizActionObj.ThawingObj != null)
                {
                    clsIVFDashBoard_ThawingDetailsVO ObjDetails = BizActionObj.ThawingObj;
                    DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingDetails");

                    //added by neena
                    dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
                    dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
                    dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
                    dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);
                    //

                    dbServer.AddInParameter(command2, "ID", DbType.Int64, ObjDetails.ID);
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "ThawingID", DbType.Int64, BizActionObj.Thawing.ID);
                    dbServer.AddInParameter(command2, "ThawingUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                    dbServer.AddInParameter(command2, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                    dbServer.AddInParameter(command2, "Date", DbType.DateTime, ObjDetails.DateTime);
                    dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, ObjDetails.CellStageID);
                    dbServer.AddInParameter(command2, "GradeID", DbType.Int64, ObjDetails.GradeID);
                    dbServer.AddInParameter(command2, "NextPlan", DbType.Int64, ObjDetails.PostThawingPlanID);
                    dbServer.AddInParameter(command2, "Comments", DbType.String, ObjDetails.Comments);
                    dbServer.AddInParameter(command2, "Status", DbType.String, ObjDetails.EmbStatus);
                    dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                    dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);
                    //added by neena
                    dbServer.AddInParameter(command2, "TransferDay", DbType.Int64, ObjDetails.TransferDayNo);
                    dbServer.AddInParameter(command2, "StageOfDevelopmentGradeID", DbType.Int64, ObjDetails.StageOfDevelopmentGradeID);
                    dbServer.AddInParameter(command2, "InnerCellMassGradeID", DbType.Int64, ObjDetails.InnerCellMassGradeID);
                    dbServer.AddInParameter(command2, "TrophoectodermGradeID", DbType.Int64, ObjDetails.TrophoectodermGradeID);
                    dbServer.AddInParameter(command2, "LabPersonId", DbType.Int64, BizActionObj.Thawing.LabPersonId);
                    dbServer.AddInParameter(command2, "CellStage", DbType.String, ObjDetails.CellStage);

                    dbServer.AddInParameter(command2, "DateTime", DbType.DateTime, BizActionObj.Thawing.DateTime);
                    dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command2, "IsFreshEmbryoPGDPGS", DbType.Boolean, ObjDetails.IsFreshEmbryoPGDPGS);
                    dbServer.AddInParameter(command2, "IsFrozenEmbryoPGDPGS", DbType.Boolean, ObjDetails.IsFrozenEmbryoPGDPGS);

                    int iStatus = dbServer.ExecuteNonQuery(command2, trans);

                    //if (ObjDetails.Plan == true)
                    if (ObjDetails.PostThawingPlanID == 2)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
                        dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
                        dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
                        dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);
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
                        long ETID = (long)dbServer.GetParameterValue(command3, "ID");


                        DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                        dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                        dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command6, "ETID", DbType.Int64, ETID);
                        dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, ObjDetails.EmbNumber);
                        dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                        dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, ObjDetails.DateTime);
                        if (ObjDetails.TransferDayNo == 1)
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day 1");
                        else if (ObjDetails.TransferDayNo == 2)
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day 2");
                        else if (ObjDetails.TransferDayNo == 3)
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day 3");
                        else if (ObjDetails.TransferDayNo == 4)
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day 4");
                        else if (ObjDetails.TransferDayNo == 5)
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day 5");
                        else if (ObjDetails.TransferDayNo == 6)
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day 6");
                        else if (ObjDetails.TransferDayNo == 7)
                            dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Day 7");
                        dbServer.AddInParameter(command6, "GradeID", DbType.Int64, ObjDetails.GradeID);
                        dbServer.AddInParameter(command6, "Score", DbType.String, null);
                        dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, ObjDetails.CellStageID);
                        dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                        dbServer.AddInParameter(command6, "FileName", DbType.String, null);
                        dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                        dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                        dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                        dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);

                        //added by neena
                        dbServer.AddInParameter(command6, "EmbTransferDay", DbType.Int64, ObjDetails.TransferDayNo);
                        dbServer.AddInParameter(command6, "Remark", DbType.String, null);
                        dbServer.AddInParameter(command6, "GradeID_New", DbType.Int64, ObjDetails.GradeID);
                        dbServer.AddInParameter(command6, "StageofDevelopmentGrade", DbType.Int64, ObjDetails.StageOfDevelopmentGradeID);
                        dbServer.AddInParameter(command6, "InnerCellMassGrade", DbType.Int64, ObjDetails.InnerCellMassGradeID);
                        dbServer.AddInParameter(command6, "TrophoectodermGrade", DbType.Int64, ObjDetails.TrophoectodermGradeID);
                        dbServer.AddInParameter(command6, "CellStage", DbType.String, ObjDetails.CellStage);
                        dbServer.AddInParameter(command6, "IsFreshEmbryoPGDPGS", DbType.Boolean, ObjDetails.IsFreshEmbryoPGDPGS);
                        dbServer.AddInParameter(command6, "IsFrozenEmbryo", DbType.Boolean, ObjDetails.IsFrozenEmbryo);
                        dbServer.AddInParameter(command6, "IsFromThawing", DbType.Boolean, true);
                        //

                        int iStatus6 = dbServer.ExecuteNonQuery(command6, trans);
                        DbCommand command7 = dbServer.GetStoredProcCommand("IVFDashboard_UpdateVitrificationDetailsForThawing");

                        dbServer.AddInParameter(command7, "UsedByOtherCycle", DbType.Boolean, BizActionObj.Thawing.UsedByOtherCycle);
                        dbServer.AddInParameter(command7, "UsedTherapyID", DbType.Int64, BizActionObj.Thawing.UsedTherapyID);
                        dbServer.AddInParameter(command7, "UsedTherapyUnitID", DbType.Int64, BizActionObj.Thawing.UsedTherapyUnitID);
                        dbServer.AddInParameter(command7, "ReceivingDate", DbType.DateTime, BizActionObj.Thawing.DateTime);
                        dbServer.AddInParameter(command7, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                        int iStatus7 = dbServer.ExecuteNonQuery(command7, trans);
                    }

                    #region Refreeze i.e TO Vitrification
                    if (ObjDetails.PostThawingPlanID == 3)
                    {
                        DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddVitrificationForEmbryoRefeezeNew");

                        dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
                        dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
                        dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
                        dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);
                        dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                        dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                        dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                        dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                        dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, false);
                        dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, null);
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
                        dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 0);
                        dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                        dbServer.AddInParameter(command4, "Refreeze", DbType.Boolean, true);
                        dbServer.AddInParameter(command4, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                        dbServer.AddInParameter(command4, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                        dbServer.AddInParameter(command4, "TransferDay", DbType.Int64, ObjDetails.TransferDayNo);

                        int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                        BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command4, "ResultStatus"));
                        BizActionObj.Thawing.ID = Convert.ToInt64(dbServer.GetParameterValue(command4, "ID"));


                    }
                    #endregion


                    #region Refreeze i.e TO Vitrification commented
                    //if (ObjDetails.PostThawingPlanID == 3)
                    //{
                    //    DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddVitrificationForEmbryo");

                    //    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //    dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
                    //    dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
                    //    dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
                    //    dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);
                    //    dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, System.DateTime.Now);
                    //    dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                    //    dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                    //    dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                    //    dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, false);
                    //    dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, null);
                    //    dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //    dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    //    dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    //    dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    //    dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    //    dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                    //    dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                    //    dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                    //    dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                    //    dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);
                    //    //String TanDay = ObjDetails.TransferDay;
                    //    //char Day = TanDay[TanDay.Length - 1];
                    //    dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 0);
                    //    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    //    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                    //    dbServer.AddInParameter(command4, "Refreeze", DbType.Boolean, true);

                    //    int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    //    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    //    BizActionObj.Thawing.ID = Convert.ToInt64(dbServer.GetParameterValue(command4, "ID"));


                    //    DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddVitrificationDetailsForEmbyo");
                    //    dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                    //    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //    dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Thawing.ID);
                    //    dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //    dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                    //    dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                    //    dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                    //    dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                    //    dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                    //    dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                    //    dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                    //    dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                    //    dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                    //    dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                    //    dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                    //    dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                    //    dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, ObjDetails.DateTime);
                    //    dbServer.AddInParameter(command5, "TransferDay", DbType.String, ObjDetails.TransferDay);
                    //    dbServer.AddInParameter(command5, "CellStageID", DbType.String, ObjDetails.CellStageID);
                    //    dbServer.AddInParameter(command5, "GradeID", DbType.Int64, ObjDetails.GradeID);
                    //    dbServer.AddInParameter(command5, "EmbStatus", DbType.String, ObjDetails.EmbStatus);
                    //    dbServer.AddInParameter(command5, "Comments", DbType.String, ObjDetails.Comments);
                    //    dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                    //    //if (BizActionObj.VitrificationMain.IsFreezed == true && BizActionObj.VitrificationMain.IsOnlyVitrification == false)
                    //    //    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, true);
                    //    //else
                    //    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                    //    dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                    //    dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);

                    //    dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                    //    dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                    //    dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                    //    dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                    //    //added by neena
                    //    dbServer.AddInParameter(command5, "TransferDayNo", DbType.Int64, ObjDetails.TransferDayNo);
                    //    dbServer.AddInParameter(command5, "CleavageGrade", DbType.Int64, ObjDetails.GradeID);
                    //    dbServer.AddInParameter(command5, "StageofDevelopmentGrade", DbType.Int64, ObjDetails.StageOfDevelopmentGradeID);
                    //    dbServer.AddInParameter(command5, "InnerCellMassGrade", DbType.Int64, ObjDetails.InnerCellMassGradeID);
                    //    dbServer.AddInParameter(command5, "TrophoectodermGrade", DbType.Int64, ObjDetails.TrophoectodermGradeID);
                    //    dbServer.AddInParameter(command5, "CellStage", DbType.String, ObjDetails.CellStage);
                    //    dbServer.AddInParameter(command5, "VitrificationDate", DbType.DateTime, null);
                    //    dbServer.AddInParameter(command5, "VitrificationTime", DbType.DateTime, null);
                    //    dbServer.AddInParameter(command5, "VitrificationNo", DbType.String, null);
                    //    dbServer.AddInParameter(command5, "IsSaved", DbType.Boolean, false);
                    //    dbServer.AddInParameter(command5, "Refreeze", DbType.Boolean, true);
                    //    //

                    //    int iStatus5 = dbServer.ExecuteNonQuery(command5, trans);



                    //    ////Add in Vitrification table
                    //    //DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddVitrificationForEmbryo");   //IVFDashboard_AddUpdateVitrification

                    //    //dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //    //dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
                    //    //dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
                    //    //dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
                    //    //dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);

                    //    ////dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                    //    //dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, );
                    //    //dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                    //    //dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                    //    //dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                    //    //dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
                    //    //dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
                    //    //dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //    //dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                    //    //dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    //    //dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    //    //dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    //    //dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
                    //    //dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
                    //    //dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
                    //    //dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
                    //    //dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);

                    //    ////dbServer.AddInParameter(command4, "EmbryologistID", DbType.Int64, null);
                    //    ////dbServer.AddInParameter(command4, "AssitantEmbryologistID", DbType.Int64, null);

                    //    //String TanDay = ObjDetails.TransferDay;
                    //    //char Day = TanDay[TanDay.Length - 1];



                    //    //dbServer.AddInParameter(command4, "FromForm", DbType.Int64, Day);
                    //    ////dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
                    //    //dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    //    //dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                    //    //dbServer.AddInParameter(command4, "Refreeze", DbType.Boolean, true);
                    //    //int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    //    //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    //    //BizActionObj.Thawing.ID = (long)dbServer.GetParameterValue(command4, "ID");


                    //    //DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddVitrificationDetailsForEmbyo");  //IVFDashboard_AddUpdateVitrificationDetails

                    //    //dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                    //    //dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //    //dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Thawing.ID);
                    //    //dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //    //dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                    //    //dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                    //    //dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                    //    //dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                    //    //dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                    //    //dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                    //    //dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                    //    //dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                    //    //dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                    //    //dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                    //    //dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                    //    //dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                    //    //dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, ObjDetails.DateTime);
                    //    //dbServer.AddInParameter(command5, "TransferDay", DbType.String, ObjDetails.TransferDay);
                    //    //dbServer.AddInParameter(command5, "CellStageID", DbType.String, ObjDetails.CellStageID);
                    //    //dbServer.AddInParameter(command5, "GradeID", DbType.Int64, ObjDetails.GradeID);
                    //    //dbServer.AddInParameter(command5, "EmbStatus", DbType.String, null);
                    //    //dbServer.AddInParameter(command5, "Comments", DbType.String, null);
                    //    //dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                    //    //dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                    //    //dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                    //    //dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);

                    //    //dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                    //    //dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                    //    //dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                    //    //dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                    //    ////added by neena
                    //    //dbServer.AddInParameter(command5, "TransferDayNo", DbType.Int64, ObjDetails.TransferDayNo);
                    //    //dbServer.AddInParameter(command5, "CleavageGrade", DbType.Int64, ObjDetails.GradeID);
                    //    //dbServer.AddInParameter(command5, "StageofDevelopmentGrade", DbType.Int64, ObjDetails.StageOfDevelopmentGradeID);
                    //    //dbServer.AddInParameter(command5, "InnerCellMassGrade", DbType.Int64, ObjDetails.InnerCellMassGradeID);
                    //    //dbServer.AddInParameter(command5, "TrophoectodermGrade", DbType.Int64, ObjDetails.TrophoectodermGradeID);
                    //    //dbServer.AddInParameter(command5, "CellStage", DbType.String, ObjDetails.CellStage);
                    //    //dbServer.AddInParameter(command5, "VitrificationDate", DbType.DateTime, ObjDetails.VitrificationDate);
                    //    //dbServer.AddInParameter(command5, "VitrificationTime", DbType.DateTime, ObjDetails.VitrificationTime);
                    //    //dbServer.AddInParameter(command5, "VitrificationNo", DbType.String, ObjDetails.VitrificationNo);
                    //    //dbServer.AddInParameter(command5, "Refreeze", DbType.Boolean, true);
                    //    ////

                    //    //int iStatus7 = dbServer.ExecuteNonQuery(command5, trans);
                    //}
                    #endregion

                    #region Extended culture
                    if (ObjDetails.PostThawingPlanID == 4)
                    {
                        DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddExtendedCultureForEmbryo");

                        dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
                        dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
                        dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
                        dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);
                        dbServer.AddInParameter(command4, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                        dbServer.AddInParameter(command4, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                        dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    }
                    #endregion

                    #region PGD/PGS
                    if (ObjDetails.PostThawingPlanID == 7)
                    {
                        DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddPGDPGSForEmbryo");

                        dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
                        dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
                        dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
                        dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);
                        dbServer.AddInParameter(command4, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                        dbServer.AddInParameter(command4, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                        dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    }
                    #endregion
                }
                //}

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Thawing = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;



        }

        //for Oocyte Thawing
        public override IValueObject AddUpdateIVFDashBoard_ThawingOocyte(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_AddUpdateThawingBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateThawingBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawing");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, BizActionObj.Thawing.DateTime);
                dbServer.AddInParameter(command, "LabPersonId", DbType.Int64, BizActionObj.Thawing.LabPersonId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //added by neena
                //dbServer.AddInParameter(command, "Plan", DbType.Int64, BizActionObj.PostThawingPlan);
                //



                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Thawing.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Thawing.ID = (long)dbServer.GetParameterValue(command, "ID");

                //if (BizActionObj.ThawingDetailsList != null && BizActionObj.ThawingDetailsList.Count > 0)
                //{
                //    foreach (var ObjDetails in BizActionObj.ThawingDetailsList)
                //    {
                if (BizActionObj.ThawingObj != null)
                {
                    clsIVFDashBoard_ThawingDetailsVO ObjDetails = BizActionObj.ThawingObj;
                    DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingDetails");

                    //added by neena
                    dbServer.AddInParameter(command2, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
                    dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
                    dbServer.AddInParameter(command2, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
                    dbServer.AddInParameter(command2, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);
                    //

                    dbServer.AddInParameter(command2, "ID", DbType.Int64, ObjDetails.ID);
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "ThawingID", DbType.Int64, BizActionObj.Thawing.ID);
                    dbServer.AddInParameter(command2, "ThawingUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                    dbServer.AddInParameter(command2, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                    dbServer.AddInParameter(command2, "Date", DbType.DateTime, ObjDetails.DateTime);
                    dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, ObjDetails.CellStageID);
                    dbServer.AddInParameter(command2, "GradeID", DbType.Int64, ObjDetails.GradeID);
                    dbServer.AddInParameter(command2, "NextPlan", DbType.Int64, ObjDetails.PostThawingPlanID);
                    dbServer.AddInParameter(command2, "Comments", DbType.String, ObjDetails.Comments);
                    dbServer.AddInParameter(command2, "Status", DbType.String, ObjDetails.EmbStatus);
                    dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                    dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);
                    //added by neena
                    dbServer.AddInParameter(command2, "TransferDay", DbType.Int64, ObjDetails.TransferDayNo);
                    dbServer.AddInParameter(command2, "StageOfDevelopmentGradeID", DbType.Int64, ObjDetails.StageOfDevelopmentGradeID);
                    dbServer.AddInParameter(command2, "InnerCellMassGradeID", DbType.Int64, ObjDetails.InnerCellMassGradeID);
                    dbServer.AddInParameter(command2, "TrophoectodermGradeID", DbType.Int64, ObjDetails.TrophoectodermGradeID);
                    dbServer.AddInParameter(command2, "LabPersonId", DbType.Int64, BizActionObj.Thawing.LabPersonId);
                    dbServer.AddInParameter(command2, "CellStage", DbType.String, ObjDetails.CellStage);
                    dbServer.AddInParameter(command2, "IsFreezeOocytes", DbType.Int64, ObjDetails.IsFreezeOocytes);

                    dbServer.AddInParameter(command2, "DateTime", DbType.DateTime, BizActionObj.Thawing.DateTime);
                    dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    int iStatus = dbServer.ExecuteNonQuery(command2, trans);


                    #region Refreeze i.e TO Vitrification
                    if (ObjDetails.PostThawingPlanID == 3)
                    {
                        //DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddVitrificationForEmbryo");
                        // DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddVitrificationForOcyteRefeeze");
                        DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddVitrificationForOocyteRefeezeNew");

                        dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
                        dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
                        dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
                        dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);
                        dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
                        dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
                        dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
                        dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
                        dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, false);
                        dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, null);
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
                        //String TanDay = ObjDetails.TransferDay;
                        //char Day = TanDay[TanDay.Length - 1];
                        dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 0);
                        dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                        dbServer.AddInParameter(command4, "Refreeze", DbType.Boolean, true);
                        dbServer.AddInParameter(command4, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                        dbServer.AddInParameter(command4, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);

                        int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                        BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command4, "ResultStatus"));
                        BizActionObj.Thawing.ID = Convert.ToInt64(dbServer.GetParameterValue(command4, "ID"));


                        //// DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddVitrificationDetailsOocyte");
                        // DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddVitrificationDetailsOocyteRefeeze");
                        // dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
                        // dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        // dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.Thawing.ID);
                        // dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        // dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                        // dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                        // dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
                        // dbServer.AddInParameter(command5, "EmbDays", DbType.String, null);
                        // dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
                        // dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
                        // dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
                        // dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
                        // dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
                        // dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
                        // dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
                        // dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
                        // dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, ObjDetails.DateTime);
                        // dbServer.AddInParameter(command5, "TransferDay", DbType.String, ObjDetails.TransferDay);
                        // dbServer.AddInParameter(command5, "CellStageID", DbType.String, ObjDetails.CellStageID);
                        // dbServer.AddInParameter(command5, "GradeID", DbType.Int64, ObjDetails.GradeID);
                        // dbServer.AddInParameter(command5, "EmbStatus", DbType.String, ObjDetails.EmbStatus);
                        // dbServer.AddInParameter(command5, "Comments", DbType.String, ObjDetails.Comments);
                        // dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
                        // //if (BizActionObj.VitrificationMain.IsFreezed == true && BizActionObj.VitrificationMain.IsOnlyVitrification == false)
                        // //    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, true);
                        // //else
                        // dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                        // dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                        // dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);

                        // dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
                        // dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
                        // dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
                        // dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

                        // //added by neena
                        // dbServer.AddInParameter(command5, "TransferDayNo", DbType.Int64, ObjDetails.TransferDayNo);
                        // dbServer.AddInParameter(command5, "CleavageGrade", DbType.Int64, ObjDetails.GradeID);
                        // dbServer.AddInParameter(command5, "StageofDevelopmentGrade", DbType.Int64, ObjDetails.StageOfDevelopmentGradeID);
                        // dbServer.AddInParameter(command5, "InnerCellMassGrade", DbType.Int64, ObjDetails.InnerCellMassGradeID);
                        // dbServer.AddInParameter(command5, "TrophoectodermGrade", DbType.Int64, ObjDetails.TrophoectodermGradeID);
                        // dbServer.AddInParameter(command5, "CellStage", DbType.String, ObjDetails.CellStage);
                        // dbServer.AddInParameter(command5, "VitrificationDate", DbType.DateTime, null);
                        // dbServer.AddInParameter(command5, "VitrificationTime", DbType.DateTime, null);
                        // dbServer.AddInParameter(command5, "VitrificationNo", DbType.String, null);
                        // dbServer.AddInParameter(command5, "IsSaved", DbType.Boolean, false);
                        // dbServer.AddInParameter(command5, "Refreeze", DbType.Boolean, true);
                        // //

                        // dbServer.AddInParameter(command5, "IsFreezeOocytes", DbType.Boolean, true);

                        // int iStatus5 = dbServer.ExecuteNonQuery(command5, trans);

                    }
                    #endregion


                    #region Extended culture
                    if (ObjDetails.PostThawingPlanID == 6)
                    {
                        DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddExtendedCultureForOocyte");

                        dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
                        dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
                        dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
                        dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);
                        dbServer.AddInParameter(command4, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                        dbServer.AddInParameter(command4, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                        dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    }
                    #endregion
                }
                //}

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Thawing = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;



        }

        //added by neena

        public override IValueObject AddUpdateIVFDashBoard_ThawingWOCryo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_AddUpdateThawingWOCryoBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateThawingWOCryoBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawing");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, BizActionObj.Thawing.DateTime);
                dbServer.AddInParameter(command, "LabPersonId", DbType.Int64, BizActionObj.Thawing.LabPersonId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);



                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.Thawing.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Thawing.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.ThawingDetailsList != null && BizActionObj.ThawingDetailsList.Count > 0)
                {
                    foreach (var ObjDetails in BizActionObj.ThawingDetailsList)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingDetails");


                        dbServer.AddInParameter(command2, "ID", DbType.Int64, ObjDetails.ID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "ThawingID", DbType.Int64, BizActionObj.Thawing.ID);
                        dbServer.AddInParameter(command2, "ThawingUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                        dbServer.AddInParameter(command2, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, ObjDetails.DateTime);
                        dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, ObjDetails.CellStageID);
                        dbServer.AddInParameter(command2, "GradeID", DbType.Int64, ObjDetails.GradeID);
                        dbServer.AddInParameter(command2, "NextPlan", DbType.Boolean, ObjDetails.Plan);
                        dbServer.AddInParameter(command2, "Comments", DbType.String, ObjDetails.Comments);
                        dbServer.AddInParameter(command2, "Status", DbType.String, ObjDetails.EmbStatus);
                        dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                        dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);
                        //added by neena
                        dbServer.AddInParameter(command2, "TransferDay", DbType.Int64, ObjDetails.TransferDayNo);
                        dbServer.AddInParameter(command2, "IsFreezeOocytes", DbType.Int64, BizActionObj.IsFreezeOocytes);
                        dbServer.AddInParameter(command2, "StageOfDevelopmentGradeID", DbType.Int64, ObjDetails.StageOfDevelopmentGradeID);
                        dbServer.AddInParameter(command2, "InnerCellMassGradeID", DbType.Int64, ObjDetails.InnerCellMassGradeID);
                        dbServer.AddInParameter(command2, "TrophoectodermGradeID", DbType.Int64, ObjDetails.TrophoectodermGradeID);
                        dbServer.AddInParameter(command2, "CellStage", DbType.String, ObjDetails.CellStage);
                        dbServer.AddInParameter(command2, "CycleCode", DbType.String, ObjDetails.CycleCode);
                        dbServer.AddInParameter(command2, "IsFreshEmbryoPGDPGS", DbType.Boolean, ObjDetails.IsFreshEmbryoPGDPGS);

                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);

                        DbCommand command7 = dbServer.GetStoredProcCommand("IVFDashboard_UpdateVitrificationDetailsForThawing");

                        dbServer.AddInParameter(command7, "UsedByOtherCycle", DbType.Boolean, BizActionObj.Thawing.UsedByOtherCycle);
                        dbServer.AddInParameter(command7, "UsedTherapyID", DbType.Int64, BizActionObj.Thawing.UsedTherapyID);
                        dbServer.AddInParameter(command7, "UsedTherapyUnitID", DbType.Int64, BizActionObj.Thawing.UsedTherapyUnitID);
                        dbServer.AddInParameter(command7, "ReceivingDate", DbType.DateTime, BizActionObj.Thawing.DateTime);
                        dbServer.AddInParameter(command7, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                        int iStatus7 = dbServer.ExecuteNonQuery(command7, trans);

                        #region Commented Embryo Transfer

                        //if (ObjDetails.Plan == true)
                        //{
                        //    DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

                        //    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        //    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
                        //    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
                        //    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
                        //    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);
                        //    dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
                        //    dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
                        //    //by rohini dated 14/12/2016
                        //    dbServer.AddInParameter(command3, "PhysicianID", DbType.Int64, null);
                        //    //
                        //    dbServer.AddInParameter(command3, "EmbryologistID", DbType.Boolean, null);
                        //    dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
                        //    dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
                        //    dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
                        //    dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
                        //    dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
                        //    dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
                        //    dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
                        //    dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
                        //    dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
                        //    dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
                        //    dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
                        //    dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
                        //    dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
                        //    dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
                        //    dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
                        //    dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
                        //    dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
                        //    dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        //    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        //    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        //    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        //    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        //    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        //    dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
                        //    dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
                        //    dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
                        //    dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
                        //    dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
                        //    dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
                        //    dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
                        //    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                        //    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                        //    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                        //    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                        //    long ETID = (long)dbServer.GetParameterValue(command3, "ID");


                        //    DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

                        //    dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
                        //    dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        //    dbServer.AddInParameter(command6, "ETID", DbType.Int64, ETID);
                        //    dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        //    dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, ObjDetails.EmbNumber);
                        //    dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                        //    dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, ObjDetails.DateTime);
                        //    dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Thawing");
                        //    dbServer.AddInParameter(command6, "GradeID", DbType.Int64, ObjDetails.GradeID);
                        //    dbServer.AddInParameter(command6, "Score", DbType.String, null);
                        //    dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, ObjDetails.CellStageID);
                        //    dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
                        //    dbServer.AddInParameter(command6, "FileName", DbType.String, null);

                        //    //added by rohini dated 18/12/2015
                        //    dbServer.AddInParameter(command6, "Remark", DbType.String, null);
                        //    dbServer.AddInParameter(command6, "SurrogateID", DbType.Int64, 0);
                        //    dbServer.AddInParameter(command6, "SurrogateUnitID", DbType.Int64, 0);
                        //    dbServer.AddInParameter(command6, "SurrogateMRNo", DbType.String, null);
                        //    //
                        //    dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
                        //    dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                        //    dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        //    dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
                        //    dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        //    dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        //    dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        //    dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                        //    dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);

                        //    int iStatus6 = dbServer.ExecuteNonQuery(command6, trans);

                        //    DbCommand command7 = dbServer.GetStoredProcCommand("IVFDashboard_UpdateVitrificationDetailsForThawing");

                        //    dbServer.AddInParameter(command7, "UsedByOtherCycle", DbType.Boolean, BizActionObj.Thawing.UsedByOtherCycle);
                        //    dbServer.AddInParameter(command7, "UsedTherapyID", DbType.Int64, BizActionObj.Thawing.UsedTherapyID);
                        //    dbServer.AddInParameter(command7, "UsedTherapyUnitID", DbType.Int64, BizActionObj.Thawing.UsedTherapyUnitID);
                        //    dbServer.AddInParameter(command7, "ReceivingDate", DbType.DateTime, BizActionObj.Thawing.DateTime);
                        //    dbServer.AddInParameter(command7, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                        //    int iStatus7 = dbServer.ExecuteNonQuery(command7, trans);

                        //}

                        #endregion

                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Thawing = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }

            return BizActionObj;
        }

        //

        public override IValueObject GetIVFDashBoard_Thawing(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsIVFDashboard_GetThawingBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetThawingBizActionVO;
            try
            {

                DbCommand command;
                if (BizAction.IsOnlyForEmbryoThawing == true)
                    command = dbServer.GetStoredProcCommand("IVFDashboard_GetThawingDetails");
                else
                    command = dbServer.GetStoredProcCommand("IVFDashboard_GetThawingDetailsOocyte");

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.Thawing.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.Thawing.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.Thawing.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.Thawing.PlanTherapyUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.Thawing.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.Thawing.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.Thawing.DateTime = (DateTime?)(DALHelper.HandleDate(reader["DateTime"]));
                        BizAction.Thawing.LabPersonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"]));
                        BizAction.Thawing.IsETFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsETFreezed"]));

                    }
                }

                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashBoard_ThawingDetailsVO ETdetails = new clsIVFDashBoard_ThawingDetailsVO();
                        ETdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ETdetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        ETdetails.ThawingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThawingID"]));
                        ETdetails.ThawingUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThawingUnitID"]));
                        ETdetails.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        ETdetails.DateTime = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                        ETdetails.Plan = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Plan"]));
                        ETdetails.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        ETdetails.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                        ETdetails.EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["Status"]));
                        ETdetails.EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"]));
                        ETdetails.EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"]));

                        ETdetails.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        ETdetails.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));
                        //added by neena
                        ETdetails.StageOfDevelopmentGradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageOfDevelopmentGradeID"]));
                        ETdetails.InnerCellMassGradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGradeID"]));
                        ETdetails.TrophoectodermGradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGradeID"]));
                        ETdetails.TransferDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransferDay"]));
                        ETdetails.PostThawingPlanID = Convert.ToInt32(DALHelper.HandleDBNull(reader["Plan"]));
                        ETdetails.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));

                        ETdetails.IsFreezeOocytes = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezeOocytes"]));
                        ETdetails.IsFreshEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreshEmbryoPGDPGS"]));
                        ETdetails.IsFrozenEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFrozenEmbryoPGDPGS"]));

                        BizAction.ThawingDetailsList.Add(ETdetails);
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

        // Added By CDS For Oocyte
        #region
        //public override IValueObject GetIVFDashBoard_ThawingForOocyte(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    // throw new NotImplementedException();
        //    clsIVFDashboard_GetThawingBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetThawingBizActionVO;
        //    try
        //    {

        //        DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_GetThawingDetailsForOocyte");
        //        dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.ThawingForOocyte.PatientID);
        //        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.ThawingForOocyte.PatientUnitID);
        //        dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.ThawingForOocyte.PlanTherapyID);
        //        dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.ThawingForOocyte.PlanTherapyUnitID);
        //        DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {

        //                BizAction.ThawingForOocyte.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                BizAction.ThawingForOocyte.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
        //                BizAction.ThawingForOocyte.DateTime = (DateTime?)(DALHelper.HandleDate(reader["DateTime"]));
        //                BizAction.ThawingForOocyte.LabPersonId = Convert.ToInt64(DALHelper.HandleDBNull(reader["LabPersonId"]));
        //                BizAction.ThawingForOocyte.IsETFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsETFreezed"]));

        //            }
        //        }

        //        reader.NextResult();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                clsIVFDashBoard_ThawingDetailsVO ETdetails = new clsIVFDashBoard_ThawingDetailsVO();
        //                ETdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
        //                ETdetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
        //                ETdetails.ThawingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThawingID"]));
        //                ETdetails.ThawingUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ThawingUnitID"]));
        //                ETdetails.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
        //                ETdetails.DateTime = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
        //                ETdetails.Plan = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Plan"]));
        //                //ETdetails.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
        //                ETdetails.OocyteGradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
        //                ETdetails.PostThawingPlanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PostThawingPlanID"]));

        //                ETdetails.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
        //                ETdetails.EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["Status"]));
        //                ETdetails.EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
        //                ETdetails.EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteSerialNumber"]));

        //                ETdetails.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
        //                ETdetails.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));
        //                BizAction.ThawingDetailsForOocyteList.Add(ETdetails);
        //            }
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //    }
        //    return BizAction;
        //}
        #endregion
        //

        //Added By CDS Only For Oocyte Thawing
        #region
        //public override IValueObject AddUpdateIVFDashBoard_ThawingForOocyte(IValueObject valueObject, clsUserVO UserVo)
        //{
        //    // throw new NotImplementedException();
        //    clsIVFDashboard_AddUpdateThawingBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateThawingBizActionVO;

        //    DbTransaction trans = null;
        //    DbConnection con = null;
        //    try
        //    {
        //        con = dbServer.CreateConnection();
        //        con.Open();
        //        trans = con.BeginTransaction();
        //        DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingForOocyte");

        //        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.ThawingForOocyte.PatientID);
        //        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.ThawingForOocyte.PatientUnitID);
        //        dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.ThawingForOocyte.PlanTherapyID);
        //        dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.ThawingForOocyte.PlanTherapyUnitID);
        //        dbServer.AddInParameter(command, "DateTime", DbType.DateTime, BizActionObj.ThawingForOocyte.DateTime);
        //        dbServer.AddInParameter(command, "LabPersonId", DbType.Int64, BizActionObj.ThawingForOocyte.LabPersonId);
        //        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


        //        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ThawingForOocyte.ID);
        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

        //        int intStatus = dbServer.ExecuteNonQuery(command, trans);
        //        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
        //        BizActionObj.ThawingForOocyte.ID = (long)dbServer.GetParameterValue(command, "ID");

        //        //if (BizActionObj.ThawingDetailsForOocyteList != null && BizActionObj.ThawingDetailsForOocyteList.Count > 0)
        //        //{
        //        //    foreach (var ObjDetails in BizActionObj.ThawingDetailsForOocyteList)
        //        //    {
        //        if (BizActionObj.ThawingForOocyteObj != null)
        //        {
        //            clsIVFDashBoard_ThawingDetailsVO ObjDetails = BizActionObj.ThawingForOocyteObj;
        //            DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingDetailsForOocyte");

        //            dbServer.AddInParameter(command2, "ID", DbType.Int64, ObjDetails.ID);
        //            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command2, "ThawingID", DbType.Int64, BizActionObj.ThawingForOocyte.ID);
        //            dbServer.AddInParameter(command2, "ThawingUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, ObjDetails.EmbNumber);
        //            dbServer.AddInParameter(command2, "OocyteSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
        //            dbServer.AddInParameter(command2, "Date", DbType.DateTime, ObjDetails.DateTime);
        //            dbServer.AddInParameter(command2, "CellStageID", DbType.Int64, ObjDetails.CellStageID);
        //            //dbServer.AddInParameter(command2, "GradeID", DbType.Int64, ObjDetails.GradeID);
        //            dbServer.AddInParameter(command2, "GradeID", DbType.Int64, ObjDetails.OocyteGradeID);

        //            dbServer.AddInParameter(command2, "PostThawingPlanID", DbType.Int64, ObjDetails.PostThawingPlanID);

        //            dbServer.AddInParameter(command2, "NextPlan", DbType.Boolean, ObjDetails.Plan);
        //            dbServer.AddInParameter(command2, "Comments", DbType.String, ObjDetails.Comments);
        //            dbServer.AddInParameter(command2, "Status", DbType.String, ObjDetails.EmbStatus);
        //            dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
        //            dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);
        //            dbServer.AddInParameter(command2, "CellStage", DbType.String, ObjDetails.CellStage); //added by neena

        //            int iStatus = dbServer.ExecuteNonQuery(command2, trans);

        //            //COmmented BY CDS Fro New Flow  OLD 
        //            //if (ObjDetails.Plan == true)

        //            #region For ET i.e Oocyte Transfer
        //            if (ObjDetails.PostThawingPlanID == 2)
        //            {
        //                DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransfer");

        //                dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.ThawingForOocyte.PatientID);
        //                dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.ThawingForOocyte.PatientUnitID);
        //                dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.ThawingForOocyte.PlanTherapyID);
        //                dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.ThawingForOocyte.PlanTherapyUnitID);
        //                dbServer.AddInParameter(command3, "Date", DbType.DateTime, null);
        //                dbServer.AddInParameter(command3, "Time", DbType.DateTime, null);
        //                dbServer.AddInParameter(command3, "EmbryologistID", DbType.Boolean, null);

        //                dbServer.AddInParameter(command3, "PhysicianID", DbType.Int64, null);

        //                dbServer.AddInParameter(command3, "AssitantEmbryologistID", DbType.Int64, null);
        //                dbServer.AddInParameter(command3, "PatternID", DbType.Int64, null);
        //                dbServer.AddInParameter(command3, "UterineArtery_PI", DbType.Boolean, null);
        //                dbServer.AddInParameter(command3, "UterineArtery_RI", DbType.Boolean, null);
        //                dbServer.AddInParameter(command3, "UterineArtery_SD", DbType.Boolean, null);
        //                dbServer.AddInParameter(command3, "Endometerial_PI", DbType.Boolean, null);
        //                dbServer.AddInParameter(command3, "Endometerial_RI", DbType.Boolean, null);
        //                dbServer.AddInParameter(command3, "Endometerial_SD", DbType.Boolean, null);
        //                dbServer.AddInParameter(command3, "CatheterTypeID", DbType.Int64, null);
        //                dbServer.AddInParameter(command3, "DistanceFundus", DbType.Decimal, null);
        //                dbServer.AddInParameter(command3, "EndometriumThickness", DbType.Decimal, null);
        //                dbServer.AddInParameter(command3, "TeatmentUnderGA", DbType.Boolean, null);
        //                dbServer.AddInParameter(command3, "Difficulty", DbType.Boolean, null);
        //                dbServer.AddInParameter(command3, "DifficultyID", DbType.Int64, null);
        //                dbServer.AddInParameter(command3, "TenaculumUsed", DbType.Boolean, null);
        //                dbServer.AddInParameter(command3, "IsFreezed", DbType.Boolean, false);
        //                dbServer.AddInParameter(command3, "IsOnlyET", DbType.Boolean, false);
        //                dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
        //                dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
        //                dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //                dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
        //                dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //                dbServer.AddInParameter(command3, "AnethetistID", DbType.Int64, null);
        //                dbServer.AddInParameter(command3, "AssistantAnethetistID", DbType.Int64, null);
        //                dbServer.AddInParameter(command3, "SrcOoctyID", DbType.Int64, null);
        //                dbServer.AddInParameter(command3, "SrcSemenID", DbType.Int64, null);
        //                dbServer.AddInParameter(command3, "SrcOoctyCode", DbType.String, null);
        //                dbServer.AddInParameter(command3, "SrcSemenCode", DbType.String, null);
        //                dbServer.AddInParameter(command3, "FromForm", DbType.Int64, 1);
        //                dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);

        //                int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
        //                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
        //                long ETID = (long)dbServer.GetParameterValue(command3, "ID");


        //                DbCommand command6 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateEmbryoTransferDetails");

        //                dbServer.AddInParameter(command6, "ID", DbType.Int64, 0);
        //                dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command6, "ETID", DbType.Int64, ETID);
        //                dbServer.AddInParameter(command6, "ETUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command6, "OocyteNumber", DbType.Int64, ObjDetails.EmbNumber);
        //                dbServer.AddInParameter(command6, "SerialOocyteNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
        //                dbServer.AddInParameter(command6, "TransferDate", DbType.DateTime, ObjDetails.DateTime);
        //                dbServer.AddInParameter(command6, "TransferDay", DbType.String, "Thawing");
        //                dbServer.AddInParameter(command6, "GradeID", DbType.Int64, ObjDetails.GradeID);
        //                dbServer.AddInParameter(command6, "Score", DbType.String, null);
        //                dbServer.AddInParameter(command6, "FertStageID", DbType.Int64, ObjDetails.CellStageID);
        //                dbServer.AddInParameter(command6, "EmbStatus", DbType.Boolean, null);
        //                dbServer.AddInParameter(command6, "Remark", DbType.String, null);
        //                dbServer.AddInParameter(command6, "SurrogateID", DbType.Int64, null);
        //                dbServer.AddInParameter(command6, "SurrogateUnitID", DbType.Int64, null);
        //                dbServer.AddInParameter(command6, "SurrogateMRNo", DbType.Binary, null);
        //                dbServer.AddInParameter(command6, "FileName", DbType.String, null);
        //                dbServer.AddInParameter(command6, "FileContents", DbType.Binary, null);
        //                dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
        //                dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.ID);
        //                dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //                dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
        //                dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

        //                dbServer.AddInParameter(command6, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
        //                dbServer.AddInParameter(command6, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);

        //                int iStatus6 = dbServer.ExecuteNonQuery(command6, trans);
        //                DbCommand command7 = dbServer.GetStoredProcCommand("IVFDashboard_UpdateVitrificationDetailsForThawingForOocyte");

        //                dbServer.AddInParameter(command7, "UsedByOtherCycle", DbType.Boolean, BizActionObj.ThawingForOocyte.UsedByOtherCycle);
        //                dbServer.AddInParameter(command7, "UsedTherapyID", DbType.Int64, BizActionObj.ThawingForOocyte.UsedTherapyID);
        //                dbServer.AddInParameter(command7, "UsedTherapyUnitID", DbType.Int64, BizActionObj.ThawingForOocyte.UsedTherapyUnitID);
        //                dbServer.AddInParameter(command7, "ReceivingDate", DbType.DateTime, BizActionObj.ThawingForOocyte.DateTime);
        //                dbServer.AddInParameter(command7, "OocyteSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
        //                int iStatus7 = dbServer.ExecuteNonQuery(command7, trans);
        //            }
        //            #endregion


        //            #region Refreeze i.e TO Oocyte Vitrification
        //            if (ObjDetails.PostThawingPlanID == 3)
        //            {
        //                //Add in Vitrification table
        //                DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddVitrificationForOocyte");  //  IVFDashboard_AddUpdateForOocyteVitrification

        //                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command4, "PatientID", DbType.Int64, BizActionObj.ThawingForOocyte.PatientID);
        //                dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, BizActionObj.ThawingForOocyte.PatientUnitID);
        //                dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.ThawingForOocyte.PlanTherapyID);
        //                dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.ThawingForOocyte.PlanTherapyUnitID);

        //                //dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, null);
        //                dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, System.DateTime.Now);
        //                dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, null);
        //                dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, null);
        //                dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, null);
        //                dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, null);
        //                dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, false);
        //                dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
        //                dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //                dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
        //                dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //                dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, null);
        //                dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, null);
        //                dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, null);
        //                dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, null);
        //                dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, null);

        //                dbServer.AddInParameter(command4, "EmbryologistID", DbType.Int64, null);
        //                dbServer.AddInParameter(command4, "AssitantEmbryologistID", DbType.Int64, null);

        //                //String TanDay = ObjDetails.TransferDay;
        //                //char Day = TanDay[TanDay.Length - 1];

        //                dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 0);
        //                //dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 1);
        //                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
        //                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
        //                dbServer.AddInParameter(command4, "Refreeze", DbType.Boolean, true);

        //                int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
        //                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
        //                BizActionObj.ThawingForOocyte.ID = (long)dbServer.GetParameterValue(command4, "ID");


        //                DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddVitrificationDetailsForOocyte");  // IVFDashboard_AddUpdateForOocyteVitrificationDetails

        //                dbServer.AddInParameter(command5, "ID", DbType.Int64, 0);
        //                dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.ThawingForOocyte.ID);
        //                dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command5, "OocyteNumber", DbType.Int64, ObjDetails.EmbNumber);
        //                dbServer.AddInParameter(command5, "OocyteSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
        //                dbServer.AddInParameter(command5, "LeafNo", DbType.String, null);
        //                dbServer.AddInParameter(command5, "OocyteDays", DbType.String, null);
        //                dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, null);
        //                dbServer.AddInParameter(command5, "CanId", DbType.Int64, null);
        //                dbServer.AddInParameter(command5, "StrawId", DbType.Int64, null);
        //                dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, null);
        //                dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, null);
        //                dbServer.AddInParameter(command5, "TankId", DbType.Int64, null);
        //                dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, null);
        //                dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, null);
        //                dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, ObjDetails.DateTime);
        //                dbServer.AddInParameter(command5, "TransferDay", DbType.String, ObjDetails.TransferDay);
        //                dbServer.AddInParameter(command5, "CellStageID", DbType.String, ObjDetails.CellStageID);
        //                //dbServer.AddInParameter(command5, "GradeID", DbType.Int64, ObjDetails.GradeID);
        //                dbServer.AddInParameter(command5, "GradeID", DbType.Int64, ObjDetails.OocyteGradeID);
        //                dbServer.AddInParameter(command5, "OocyteStatus", DbType.String, null);
        //                dbServer.AddInParameter(command5, "Comments", DbType.String, null);
        //                dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, null);
        //                dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

        //                dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
        //                dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);

        //                dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, false);
        //                dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, null);
        //                dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, null);
        //                dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, null);

        //                //added by neena
        //                dbServer.AddInParameter(command5, "TransferDayNo", DbType.Int64, 0);
        //                dbServer.AddInParameter(command5, "VitrificationDate", DbType.DateTime, null);
        //                dbServer.AddInParameter(command5, "VitrificationTime", DbType.DateTime, null);
        //                dbServer.AddInParameter(command5, "VitrificationNo", DbType.String, null);
        //                dbServer.AddInParameter(command5, "IsSaved", DbType.Boolean, false);
        //                dbServer.AddInParameter(command5, "Refreeze", DbType.Boolean, true);
        //                dbServer.AddInParameter(command5, "CellStage", DbType.String, ObjDetails.CellStage); //added by neena


        //                int iStatus7 = dbServer.ExecuteNonQuery(command5, trans);
        //            }
        //            #endregion


        //            #region  Culture For Oocyte i.e On The Same Day As It Was Cryo
        //            if (ObjDetails.PostThawingPlanID == 6)         // 	 Culture
        //            {
        //                String TanDay = ObjDetails.TransferDay;
        //                char Day = TanDay[TanDay.Length - 1];

        //                DbCommand command00 = dbServer.GetStoredProcCommand("IVFDashBoard_AddUpdateDaysHistoryDetails");
        //                dbServer.AddInParameter(command00, "PatientID", DbType.Int64, BizActionObj.Thawing.PatientID);
        //                dbServer.AddInParameter(command00, "PatientUnitID", DbType.Int64, BizActionObj.Thawing.PatientUnitID);
        //                dbServer.AddInParameter(command00, "PlanTherapyID", DbType.Int64, BizActionObj.Thawing.PlanTherapyID);
        //                dbServer.AddInParameter(command00, "PlanTherapyUnitID", DbType.Int64, BizActionObj.Thawing.PlanTherapyUnitID);
        //                dbServer.AddInParameter(command00, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
        //                dbServer.AddInParameter(command00, "Day", DbType.String, Day);

        //                int intStatus00 = dbServer.ExecuteNonQuery(command00, trans);
        //            }
        //            #endregion
        //        }



        //        //}


        //        trans.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //        BizActionObj.SuccessStatus = -1;
        //        BizActionObj.Thawing = null;
        //    }
        //    finally
        //    {
        //        con.Close();
        //        trans = null;
        //        con = null;
        //    }

        //    return BizActionObj;



        //}
        #endregion
        // END


    }
}
