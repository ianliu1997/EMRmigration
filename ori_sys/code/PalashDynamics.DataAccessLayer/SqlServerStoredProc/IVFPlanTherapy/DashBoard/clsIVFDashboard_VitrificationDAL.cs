using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.ValueObjects.IVFPlanTherapy;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    class clsIVFDashboard_VitrificationDAL : clsBaseIVFDashboard_VitrificationDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsIVFDashboard_VitrificationDAL()
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

        //for embryo cryo
        public override IValueObject AddUpdateIVFDashBoard_VitrificationSingle(IValueObject valueObject, clsUserVO UserVo)
        {
            //  throw new NotImplementedException();

            clsIVFDashboard_AddUpdateVitrificationBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateVitrificationBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                //if (BizActionObj.VitrificationMain != null && BizActionObj.VitrificationMain.IsOnlyForEmbryoVitrification == true)
                //{
                DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.VitrificationMain.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.VitrificationMain.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, BizActionObj.VitrificationMain.DateTime);
                dbServer.AddInParameter(command, "VitrificationNo", DbType.String, BizActionObj.VitrificationMain.VitrificationNo);
                dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, BizActionObj.VitrificationMain.PickUpDate);
                dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, BizActionObj.VitrificationMain.ConsentForm);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.VitrificationMain.IsFreezed);
                dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, BizActionObj.VitrificationMain.IsOnlyVitrification);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, BizActionObj.VitrificationMain.SrcOoctyID);
                dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, BizActionObj.VitrificationMain.SrcSemenID);
                dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, BizActionObj.VitrificationMain.SrcOoctyCode);
                dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, BizActionObj.VitrificationMain.SrcSemenCode);
                dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, BizActionObj.VitrificationMain.UsedOwnOocyte);
                dbServer.AddInParameter(command, "FromForm", DbType.Int64, 2);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.VitrificationMain.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddInParameter(command, "IsFreezeOocytes", DbType.Boolean, BizActionObj.IsFreezeOocytes);
                if (BizActionObj.VitrificationDetailsObj.IsRefreeze == true || BizActionObj.VitrificationDetailsObj.IsRefreezeFromOtherCycle == true)
                    BizActionObj.IsRefreeze = true;
                dbServer.AddInParameter(command, "IsRefreeze", DbType.Boolean, BizActionObj.IsRefreeze);
             
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.VitrificationMain.ID = (long)dbServer.GetParameterValue(command, "ID");

                //if (BizActionObj.VitrificationDetailsList != null && BizActionObj.VitrificationDetailsList.Count > 0)
                //{
                //foreach (var ObjDetails in BizActionObj.VitrificationDetailsList)
                //{
                if (BizActionObj.VitrificationDetailsObj != null)
                {
                    clsIVFDashBoard_VitrificationDetailsVO ObjDetails = BizActionObj.VitrificationDetailsObj;
                    DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                    dbServer.AddInParameter(command2, "ID", DbType.Int64, ObjDetails.ID);
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "VitrivicationID", DbType.Int64, BizActionObj.VitrificationMain.ID);
                    dbServer.AddInParameter(command2, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                    dbServer.AddInParameter(command2, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                    dbServer.AddInParameter(command2, "LeafNo", DbType.String, ObjDetails.LeafNo);
                    dbServer.AddInParameter(command2, "EmbDays", DbType.String, ObjDetails.EmbDays);
                    dbServer.AddInParameter(command2, "ColorCodeID", DbType.Int64, ObjDetails.ColorCodeID);
                    dbServer.AddInParameter(command2, "CanId", DbType.Int64, ObjDetails.CanId);
                    dbServer.AddInParameter(command2, "StrawId", DbType.Int64, ObjDetails.StrawId);
                    dbServer.AddInParameter(command2, "GobletShapeId", DbType.Int64, ObjDetails.GobletShapeId);
                    dbServer.AddInParameter(command2, "GobletSizeId", DbType.Int64, ObjDetails.GobletSizeId);
                    dbServer.AddInParameter(command2, "TankId", DbType.Int64, ObjDetails.TankId);
                    dbServer.AddInParameter(command2, "ConistorNo", DbType.Int64, ObjDetails.ConistorNo);
                    dbServer.AddInParameter(command2, "ProtocolTypeID", DbType.Int64, ObjDetails.ProtocolTypeID);
                    dbServer.AddInParameter(command2, "TransferDate", DbType.DateTime, ObjDetails.TransferDate);
                    dbServer.AddInParameter(command2, "TransferDay", DbType.String, ObjDetails.TransferDay);
                    dbServer.AddInParameter(command2, "CellStageID", DbType.String, ObjDetails.CellStageID);
                    dbServer.AddInParameter(command2, "GradeID", DbType.Int64, ObjDetails.GradeID);
                    dbServer.AddInParameter(command2, "EmbStatus", DbType.String, ObjDetails.EmbStatus);
                    dbServer.AddInParameter(command2, "Comments", DbType.String, ObjDetails.Comments);
                    dbServer.AddInParameter(command2, "UsedOwnOocyte", DbType.Boolean, BizActionObj.VitrificationMain.UsedOwnOocyte);
                    if (BizActionObj.VitrificationMain.IsFreezed == true && BizActionObj.VitrificationMain.IsOnlyVitrification == false)
                        dbServer.AddInParameter(command2, "IsThawingDone", DbType.Boolean, true);
                    else
                        dbServer.AddInParameter(command2, "IsThawingDone", DbType.Boolean, false);

                    dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                    dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);

                    dbServer.AddInParameter(command2, "UsedByOtherCycle", DbType.Boolean, BizActionObj.VitrificationMain.UsedByOtherCycle);
                    dbServer.AddInParameter(command2, "UsedTherapyID", DbType.Int64, BizActionObj.VitrificationMain.UsedTherapyID);
                    dbServer.AddInParameter(command2, "UsedTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMain.UsedTherapyUnitID);
                    dbServer.AddInParameter(command2, "ReceivingDate", DbType.DateTime, BizActionObj.VitrificationMain.DateTime);

                    //added by neena
                    dbServer.AddInParameter(command2, "TransferDayNo", DbType.Int64, ObjDetails.TransferDayNo);
                    dbServer.AddInParameter(command2, "CleavageGrade", DbType.Int64, ObjDetails.CleavageGrade);
                    dbServer.AddInParameter(command2, "StageofDevelopmentGrade", DbType.Int64, ObjDetails.StageofDevelopmentGrade);
                    dbServer.AddInParameter(command2, "InnerCellMassGrade", DbType.Int64, ObjDetails.InnerCellMassGrade);
                    dbServer.AddInParameter(command2, "TrophoectodermGrade", DbType.Int64, ObjDetails.TrophoectodermGrade);
                    dbServer.AddInParameter(command2, "CellStage", DbType.String, ObjDetails.CellStage);
                    dbServer.AddInParameter(command2, "VitrificationDate", DbType.DateTime, ObjDetails.VitrificationDate);
                    dbServer.AddInParameter(command2, "VitrificationTime", DbType.DateTime, ObjDetails.VitrificationTime);
                    dbServer.AddInParameter(command2, "VitrificationNo", DbType.String, ObjDetails.VitrificationNo);
                    dbServer.AddInParameter(command2, "IsSaved", DbType.Boolean, true);

                    dbServer.AddInParameter(command2, "ExpiryDate", DbType.DateTime, ObjDetails.ExpiryDate);
                    dbServer.AddInParameter(command2, "ExpiryTime", DbType.DateTime, ObjDetails.ExpiryTime);
                    //Flag set while saving Freeze Oocytes under Freeze All Oocytes Cycle 
                    dbServer.AddInParameter(command2, "IsFreezeOocytes", DbType.Boolean, ObjDetails.IsFreezeOocytes);        // For IVF ADM Changes

                    dbServer.AddInParameter(command2, "CryoCode", DbType.String, ObjDetails.CryoCode);
                    //

                    //added for donate cryo plan
                    dbServer.AddInParameter(command2, "IsDonateCryo", DbType.Boolean, ObjDetails.IsDonateCryo);
                    dbServer.AddInParameter(command2, "RecepientPatientID", DbType.Int64, ObjDetails.RecepientPatientID);
                    dbServer.AddInParameter(command2, "RecepientPatientUnitID", DbType.Int64, ObjDetails.RecepientPatientUnitID);

                    dbServer.AddInParameter(command2, "IsDonorCycleDonateCryo", DbType.Boolean, ObjDetails.IsDonorCycleDonateCryo);
                    dbServer.AddInParameter(command2, "IsFreshEmbryoPGDPGS", DbType.Boolean, ObjDetails.IsFreshEmbryoPGDPGS); 

                    int iStatus = dbServer.ExecuteNonQuery(command2, trans);



                    if (ObjDetails.IsDonateCryo || ObjDetails.IsDonorCycleDonateCryo)
                    {
                        DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                        dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command4, "PatientID", DbType.Int64, ObjDetails.RecepientPatientID);
                        dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, ObjDetails.RecepientPatientUnitID);
                        dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyID);
                        dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyUnitID);
                        dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, BizActionObj.VitrificationMain.DateTime);
                        dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, BizActionObj.VitrificationMain.VitrificationNo);
                        dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, BizActionObj.VitrificationMain.PickUpDate);
                        dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, BizActionObj.VitrificationMain.ConsentForm);
                        dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, BizActionObj.VitrificationMain.IsFreezed);
                        dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, BizActionObj.VitrificationMain.IsOnlyVitrification);
                        dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, BizActionObj.VitrificationMain.SrcOoctyID);
                        dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, BizActionObj.VitrificationMain.SrcSemenID);
                        dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, BizActionObj.VitrificationMain.SrcOoctyCode);
                        dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, BizActionObj.VitrificationMain.SrcSemenCode);
                        dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, BizActionObj.VitrificationMain.UsedOwnOocyte);
                        dbServer.AddInParameter(command4, "IsRefreeze", DbType.Boolean, BizActionObj.VitrificationMain.IsRefeeze);
                        dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 2);
                        dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.VitrificationMain.DonateCryoID);
                        dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                        dbServer.AddInParameter(command4, "IsFreezeOocytes", DbType.Boolean, BizActionObj.IsFreezeOocytes);

                        int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                        BizActionObj.VitrificationMain.DonateCryoID = (long)dbServer.GetParameterValue(command4, "ID");


                        DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                        dbServer.AddInParameter(command5, "ID", DbType.Int64, ObjDetails.ID);
                        dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.VitrificationMain.DonateCryoID);
                        dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                        dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                        dbServer.AddInParameter(command5, "LeafNo", DbType.String, ObjDetails.LeafNo);
                        dbServer.AddInParameter(command5, "EmbDays", DbType.String, ObjDetails.EmbDays);
                        dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, ObjDetails.ColorCodeID);
                        dbServer.AddInParameter(command5, "CanId", DbType.Int64, ObjDetails.CanId);
                        dbServer.AddInParameter(command5, "StrawId", DbType.Int64, ObjDetails.StrawId);
                        dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, ObjDetails.GobletShapeId);
                        dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, ObjDetails.GobletSizeId);
                        dbServer.AddInParameter(command5, "TankId", DbType.Int64, ObjDetails.TankId);
                        dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, ObjDetails.ConistorNo);
                        dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, ObjDetails.ProtocolTypeID);
                        dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, ObjDetails.TransferDate);
                        dbServer.AddInParameter(command5, "TransferDay", DbType.String, ObjDetails.TransferDay);
                        dbServer.AddInParameter(command5, "CellStageID", DbType.String, ObjDetails.CellStageID);
                        dbServer.AddInParameter(command5, "GradeID", DbType.Int64, ObjDetails.GradeID);
                        dbServer.AddInParameter(command5, "EmbStatus", DbType.String, ObjDetails.EmbStatus);
                        dbServer.AddInParameter(command5, "Comments", DbType.String, ObjDetails.Comments);
                        dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, BizActionObj.VitrificationMain.UsedOwnOocyte);
                        if (BizActionObj.VitrificationMain.IsFreezed == true && BizActionObj.VitrificationMain.IsOnlyVitrification == false)
                            dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, true);
                        else
                            dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                        dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                        dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);

                        dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, BizActionObj.VitrificationMain.UsedByOtherCycle);
                        dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, BizActionObj.VitrificationMain.UsedTherapyID);
                        dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMain.UsedTherapyUnitID);
                        dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, BizActionObj.VitrificationMain.DateTime);

                        //added by neena
                        dbServer.AddInParameter(command5, "TransferDayNo", DbType.Int64, ObjDetails.TransferDayNo);
                        dbServer.AddInParameter(command5, "CleavageGrade", DbType.Int64, ObjDetails.CleavageGrade);
                        dbServer.AddInParameter(command5, "StageofDevelopmentGrade", DbType.Int64, ObjDetails.StageofDevelopmentGrade);
                        dbServer.AddInParameter(command5, "InnerCellMassGrade", DbType.Int64, ObjDetails.InnerCellMassGrade);
                        dbServer.AddInParameter(command5, "TrophoectodermGrade", DbType.Int64, ObjDetails.TrophoectodermGrade);
                        dbServer.AddInParameter(command5, "CellStage", DbType.String, ObjDetails.CellStage);
                        dbServer.AddInParameter(command5, "VitrificationDate", DbType.DateTime, ObjDetails.VitrificationDate);
                        dbServer.AddInParameter(command5, "VitrificationTime", DbType.DateTime, ObjDetails.VitrificationTime);
                        dbServer.AddInParameter(command5, "VitrificationNo", DbType.String, ObjDetails.VitrificationNo);
                        dbServer.AddInParameter(command5, "IsSaved", DbType.Boolean, true);

                        dbServer.AddInParameter(command5, "ExpiryDate", DbType.DateTime, ObjDetails.ExpiryDate);
                        dbServer.AddInParameter(command5, "ExpiryTime", DbType.DateTime, ObjDetails.ExpiryTime);
                        //
                        //Flag set while saving Freeze Oocytes under Freeze All Oocytes Cycle 
                        dbServer.AddInParameter(command5, "IsFreezeOocytes", DbType.Boolean, ObjDetails.IsFreezeOocytes);

                        dbServer.AddInParameter(command5, "CryoCode", DbType.String, ObjDetails.CryoCode);
                        //

                        //added for donate cryo plan
                        dbServer.AddInParameter(command5, "IsDonateCryo", DbType.Boolean, null);
                        dbServer.AddInParameter(command5, "RecepientPatientID", DbType.Int64, null);
                        dbServer.AddInParameter(command5, "RecepientPatientUnitID", DbType.Int64, null);
                        //

                        //added to save patient details who donated the oocytes
                        dbServer.AddInParameter(command5, "IsDonatedCryoReceived", DbType.Boolean, true);
                        dbServer.AddInParameter(command5, "DonorPatientID", DbType.Int64, BizActionObj.VitrificationMain.PatientID);
                        dbServer.AddInParameter(command5, "DonorPatientUnitID", DbType.Int64, BizActionObj.VitrificationMain.PatientUnitID);
                        //

                        dbServer.AddInParameter(command5, "IsFreshEmbryoPGDPGS", DbType.Boolean, ObjDetails.IsFreshEmbryoPGDPGS); 
                        int iStatus5 = dbServer.ExecuteNonQuery(command5, trans);
                    }
                }


                //}
                //}
                //}

                // IsCryoWOThaw : Flag use to save only Vitrification\Cryo details from IVF/ICSI/IVF-ICSI/FE ICSI cycles which will be thaw under Freeze All Oocyte/Freeze-Thaw Transfer cycles


                if (BizActionObj.VitrificationMain.IsFreezed == true && BizActionObj.VitrificationMain.IsOnlyVitrification == false && BizActionObj.VitrificationMain.IsCryoWOThaw == false)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawing");

                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.VitrificationMain.PatientID);
                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.VitrificationMain.PatientUnitID);
                    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyID);
                    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyUnitID);
                    dbServer.AddInParameter(command3, "DateTime", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "LabPersonId", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);



                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                    BizActionObj.VitrificationMain.ID = (long)dbServer.GetParameterValue(command3, "ID");

                    if (BizActionObj.VitrificationDetailsList != null && BizActionObj.VitrificationDetailsList.Count > 0)
                    {
                        foreach (var ObjDetails in BizActionObj.VitrificationDetailsList)
                        {
                            DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingDetails");

                            dbServer.AddInParameter(command4, "ID", DbType.Int64, ObjDetails.ID);
                            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "ThawingID", DbType.Int64, BizActionObj.VitrificationMain.ID);
                            dbServer.AddInParameter(command4, "ThawingUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                            dbServer.AddInParameter(command4, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                            dbServer.AddInParameter(command4, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "CellStageID", DbType.Int64, ObjDetails.CellStageID);
                            dbServer.AddInParameter(command4, "GradeID", DbType.Int64, ObjDetails.GradeID);
                            dbServer.AddInParameter(command4, "NextPlan", DbType.Boolean, false);
                            dbServer.AddInParameter(command4, "Comments", DbType.String, null);
                            dbServer.AddInParameter(command4, "Status", DbType.String, null);

                            dbServer.AddInParameter(command4, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                            dbServer.AddInParameter(command4, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);
                            //added by neena
                            dbServer.AddInParameter(command4, "TransferDay", DbType.Int64, ObjDetails.TransferDayNo);

                            int iStatus = dbServer.ExecuteNonQuery(command4, trans);
                        }
                    }
                }

                //added by 
                #region Only For Oocyte Cryopreservation
                //if (BizActionObj.VitrificationMainForOocyte != null && BizActionObj.VitrificationMain.IsOnlyForEmbryoVitrification == false)
                //{
                //    DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateForOocyteVitrification");

                //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PatientID);
                //    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PatientUnitID);
                //    dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PlanTherapyID);
                //    dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PlanTherapyUnitID);
                //    dbServer.AddInParameter(command, "DateTime", DbType.DateTime, BizActionObj.VitrificationMainForOocyte.DateTime);
                //    dbServer.AddInParameter(command, "VitrificationNo", DbType.String, BizActionObj.VitrificationMainForOocyte.VitrificationNo);
                //    dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, BizActionObj.VitrificationMainForOocyte.PickUpDate);
                //    dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, BizActionObj.VitrificationMainForOocyte.ConsentForm);
                //    dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.VitrificationMainForOocyte.IsFreezed);
                //    dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, BizActionObj.VitrificationMainForOocyte.IsOnlyVitrification);
                //    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                //    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.SrcOoctyID);
                //    dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.SrcSemenID);
                //    dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, BizActionObj.VitrificationMainForOocyte.SrcOoctyCode);
                //    dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, BizActionObj.VitrificationMainForOocyte.SrcSemenCode);
                //    dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, BizActionObj.VitrificationMainForOocyte.UsedOwnOocyte);

                //    dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.EmbryologistID);
                //    dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.AssitantEmbryologistID);

                //    dbServer.AddInParameter(command, "FromForm", DbType.Int64, 2);
                //    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.VitrificationMainForOocyte.ID);
                //    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                //    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                //    BizActionObj.VitrificationMainForOocyte.ID = (long)dbServer.GetParameterValue(command, "ID");

                //    //if (BizActionObj.VitrificationDetailsForOocyteList != null && BizActionObj.VitrificationDetailsForOocyteList.Count > 0)
                //    //{
                //    //    foreach (var ObjDetails in BizActionObj.VitrificationDetailsForOocyteList)
                //    //    {

                //    if (BizActionObj.VitrificationDetailsForOocyteObj != null)
                //    {
                //        clsIVFDashBoard_VitrificationDetailsVO ObjDetails = BizActionObj.VitrificationDetailsForOocyteObj;
                //        DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateForOocyteVitrificationDetails");   //old IVFDashboard_AddUpdateVitrificationDetails

                //        dbServer.AddInParameter(command2, "ID", DbType.Int64, ObjDetails.ID);
                //        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //        dbServer.AddInParameter(command2, "VitrivicationID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.ID);
                //        dbServer.AddInParameter(command2, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //        dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, ObjDetails.EmbNumber);
                //        dbServer.AddInParameter(command2, "OocyteSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                //        dbServer.AddInParameter(command2, "LeafNo", DbType.String, ObjDetails.LeafNo);
                //        dbServer.AddInParameter(command2, "OocyteDays", DbType.String, ObjDetails.EmbDays);
                //        dbServer.AddInParameter(command2, "ColorCodeID", DbType.Int64, ObjDetails.ColorCodeID);
                //        dbServer.AddInParameter(command2, "CanId", DbType.Int64, ObjDetails.CanId);
                //        dbServer.AddInParameter(command2, "StrawId", DbType.Int64, ObjDetails.StrawId);
                //        dbServer.AddInParameter(command2, "GobletShapeId", DbType.Int64, ObjDetails.GobletShapeId);
                //        dbServer.AddInParameter(command2, "GobletSizeId", DbType.Int64, ObjDetails.GobletSizeId);
                //        dbServer.AddInParameter(command2, "TankId", DbType.Int64, ObjDetails.TankId);
                //        dbServer.AddInParameter(command2, "ConistorNo", DbType.Int64, ObjDetails.ConistorNo);
                //        dbServer.AddInParameter(command2, "ProtocolTypeID", DbType.Int64, ObjDetails.ProtocolTypeID);
                //        dbServer.AddInParameter(command2, "TransferDate", DbType.DateTime, ObjDetails.TransferDate);
                //        dbServer.AddInParameter(command2, "TransferDay", DbType.String, ObjDetails.TransferDay);
                //        dbServer.AddInParameter(command2, "CellStageID", DbType.String, ObjDetails.CellStageID);
                //        dbServer.AddInParameter(command2, "GradeID", DbType.Int64, ObjDetails.GradeID);
                //        dbServer.AddInParameter(command2, "OocyteStatus", DbType.String, ObjDetails.EmbStatus);
                //        dbServer.AddInParameter(command2, "Comments", DbType.String, ObjDetails.Comments);
                //        dbServer.AddInParameter(command2, "UsedOwnOocyte", DbType.Boolean, BizActionObj.VitrificationMainForOocyte.UsedOwnOocyte);
                //        if (BizActionObj.VitrificationMainForOocyte.IsFreezed == true && BizActionObj.VitrificationMainForOocyte.IsOnlyVitrification == false)
                //            dbServer.AddInParameter(command2, "IsThawingDone", DbType.Boolean, true);
                //        else
                //            dbServer.AddInParameter(command2, "IsThawingDone", DbType.Boolean, false);

                //        dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                //        dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);

                //        dbServer.AddInParameter(command2, "UsedByOtherCycle", DbType.Boolean, BizActionObj.VitrificationMainForOocyte.UsedByOtherCycle);
                //        dbServer.AddInParameter(command2, "UsedTherapyID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.UsedTherapyID);
                //        dbServer.AddInParameter(command2, "UsedTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.UsedTherapyUnitID);
                //        dbServer.AddInParameter(command2, "ReceivingDate", DbType.DateTime, BizActionObj.VitrificationMainForOocyte.DateTime);

                //        //added by neena
                //        dbServer.AddInParameter(command2, "TransferDayNo", DbType.Int64, 0);
                //        dbServer.AddInParameter(command2, "VitrificationDate", DbType.DateTime, ObjDetails.VitrificationDate);
                //        dbServer.AddInParameter(command2, "VitrificationTime", DbType.DateTime, ObjDetails.VitrificationTime);
                //        dbServer.AddInParameter(command2, "VitrificationNo", DbType.String, ObjDetails.VitrificationNo);
                //        dbServer.AddInParameter(command2, "IsSaved", DbType.Boolean, true);

                //        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                //    }
                //}
                ////    }
                ////}

                //if (BizActionObj.VitrificationMainForOocyte.IsFreezed == true && BizActionObj.VitrificationMainForOocyte.IsOnlyVitrification == false)
                //{
                //    DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingForOocyte");   //old IVFDashboard_AddUpdateThawing

                //    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PatientID);
                //    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PatientUnitID);
                //    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PlanTherapyID);
                //    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PlanTherapyUnitID);
                //    dbServer.AddInParameter(command3, "DateTime", DbType.DateTime, null);
                //    dbServer.AddInParameter(command3, "LabPersonId", DbType.Int64, null);
                //    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                //    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);



                //    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                //    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                //    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                //    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                //    BizActionObj.VitrificationMainForOocyte.ID = (long)dbServer.GetParameterValue(command3, "ID");

                //    if (BizActionObj.VitrificationDetailsForOocyteList != null && BizActionObj.VitrificationDetailsForOocyteList.Count > 0)
                //    {
                //        foreach (var ObjDetails in BizActionObj.VitrificationDetailsForOocyteList)
                //        {
                //            DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingDetailsForOocyte");     //old IVFDashboard_AddUpdateThawingDetails

                //            dbServer.AddInParameter(command4, "ID", DbType.Int64, ObjDetails.ID);
                //            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //            dbServer.AddInParameter(command4, "ThawingID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.ID);
                //            dbServer.AddInParameter(command4, "ThawingUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //            dbServer.AddInParameter(command4, "OocyteNumber", DbType.Int64, ObjDetails.EmbNumber);
                //            dbServer.AddInParameter(command4, "OocyteSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                //            dbServer.AddInParameter(command4, "Date", DbType.DateTime, null);
                //            dbServer.AddInParameter(command4, "CellStageID", DbType.Int64, ObjDetails.CellStageID);
                //            dbServer.AddInParameter(command4, "GradeID", DbType.Int64, ObjDetails.GradeID);

                //            dbServer.AddInParameter(command4, "PostThawingPlanID", DbType.Int64, ObjDetails.PostThawingPlanID);

                //            dbServer.AddInParameter(command4, "NextPlan", DbType.Boolean, false);
                //            dbServer.AddInParameter(command4, "Comments", DbType.String, null);
                //            dbServer.AddInParameter(command4, "Status", DbType.String, null);

                //            dbServer.AddInParameter(command4, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                //            dbServer.AddInParameter(command4, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);
                //            dbServer.AddInParameter(command4, "CellStage", DbType.String, ObjDetails.CellStage); //added by neena

                //            int iStatus = dbServer.ExecuteNonQuery(command4, trans);
                //        }
                //    }
                //}
                #endregion

                //END


                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.VitrificationMain = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }

        //for oocyte cryo
        public override IValueObject AddUpdateIVFDashBoard_Vitrification(IValueObject valueObject, clsUserVO UserVo)
        {
            //  throw new NotImplementedException();

            clsIVFDashboard_AddUpdateVitrificationBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateVitrificationBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.VitrificationMain != null)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.VitrificationMain.PatientID);
                    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.VitrificationMain.PatientUnitID);
                    dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyID);
                    dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyUnitID);
                    dbServer.AddInParameter(command, "DateTime", DbType.DateTime, BizActionObj.VitrificationMain.DateTime);
                    dbServer.AddInParameter(command, "VitrificationNo", DbType.String, BizActionObj.VitrificationMain.VitrificationNo);
                    dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, BizActionObj.VitrificationMain.PickUpDate);
                    dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, BizActionObj.VitrificationMain.ConsentForm);
                    dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.VitrificationMain.IsFreezed);
                    dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, BizActionObj.VitrificationMain.IsOnlyVitrification);
                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, BizActionObj.VitrificationMain.SrcOoctyID);
                    dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, BizActionObj.VitrificationMain.SrcSemenID);
                    dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, BizActionObj.VitrificationMain.SrcOoctyCode);
                    dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, BizActionObj.VitrificationMain.SrcSemenCode);
                    dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, BizActionObj.VitrificationMain.UsedOwnOocyte);
                    dbServer.AddInParameter(command, "IsRefreeze", DbType.Boolean, BizActionObj.VitrificationMain.IsRefeeze);
                    dbServer.AddInParameter(command, "FromForm", DbType.Int64, 2);
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.VitrificationMain.ID);
                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddInParameter(command, "IsFreezeOocytes", DbType.Boolean, BizActionObj.IsFreezeOocytes);
                    dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, BizActionObj.VitrificationMain.ExpiryDate);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                    BizActionObj.VitrificationMain.ID = (long)dbServer.GetParameterValue(command, "ID");

                    if (BizActionObj.VitrificationDetailsList != null && BizActionObj.VitrificationDetailsList.Count > 0)
                    {
                        foreach (var ObjDetails in BizActionObj.VitrificationDetailsList)
                        {
                            //if (BizActionObj.VitrificationDetailsObj != null)
                            //{
                            //    clsIVFDashBoard_VitrificationDetailsVO ObjDetails = BizActionObj.VitrificationDetailsObj;
                            DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                            dbServer.AddInParameter(command2, "ID", DbType.Int64, ObjDetails.ID);
                            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "VitrivicationID", DbType.Int64, BizActionObj.VitrificationMain.ID);
                            dbServer.AddInParameter(command2, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                            dbServer.AddInParameter(command2, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                            dbServer.AddInParameter(command2, "LeafNo", DbType.String, ObjDetails.LeafNo);
                            dbServer.AddInParameter(command2, "EmbDays", DbType.String, ObjDetails.EmbDays);
                            dbServer.AddInParameter(command2, "ColorCodeID", DbType.Int64, ObjDetails.ColorCodeID);
                            dbServer.AddInParameter(command2, "CanId", DbType.Int64, ObjDetails.CanId);
                            dbServer.AddInParameter(command2, "StrawId", DbType.Int64, ObjDetails.StrawId);
                            dbServer.AddInParameter(command2, "GobletShapeId", DbType.Int64, ObjDetails.GobletShapeId);
                            dbServer.AddInParameter(command2, "GobletSizeId", DbType.Int64, ObjDetails.GobletSizeId);
                            dbServer.AddInParameter(command2, "TankId", DbType.Int64, ObjDetails.TankId);
                            dbServer.AddInParameter(command2, "ConistorNo", DbType.Int64, ObjDetails.ConistorNo);
                            dbServer.AddInParameter(command2, "ProtocolTypeID", DbType.Int64, ObjDetails.ProtocolTypeID);
                            dbServer.AddInParameter(command2, "TransferDate", DbType.DateTime, ObjDetails.TransferDate);
                            dbServer.AddInParameter(command2, "TransferDay", DbType.String, ObjDetails.TransferDay);
                            dbServer.AddInParameter(command2, "CellStageID", DbType.String, ObjDetails.CellStageID);
                            dbServer.AddInParameter(command2, "GradeID", DbType.Int64, ObjDetails.GradeID);
                            dbServer.AddInParameter(command2, "EmbStatus", DbType.String, ObjDetails.EmbStatus);
                            dbServer.AddInParameter(command2, "Comments", DbType.String, ObjDetails.Comments);
                            dbServer.AddInParameter(command2, "UsedOwnOocyte", DbType.Boolean, BizActionObj.VitrificationMain.UsedOwnOocyte);
                            if (BizActionObj.VitrificationMain.IsFreezed == true && BizActionObj.VitrificationMain.IsOnlyVitrification == false)
                                dbServer.AddInParameter(command2, "IsThawingDone", DbType.Boolean, true);
                            else
                                dbServer.AddInParameter(command2, "IsThawingDone", DbType.Boolean, false);

                            dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                            dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);

                            dbServer.AddInParameter(command2, "UsedByOtherCycle", DbType.Boolean, BizActionObj.VitrificationMain.UsedByOtherCycle);
                            dbServer.AddInParameter(command2, "UsedTherapyID", DbType.Int64, BizActionObj.VitrificationMain.UsedTherapyID);
                            dbServer.AddInParameter(command2, "UsedTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMain.UsedTherapyUnitID);
                            dbServer.AddInParameter(command2, "ReceivingDate", DbType.DateTime, BizActionObj.VitrificationMain.DateTime);

                            //added by neena
                            dbServer.AddInParameter(command2, "TransferDayNo", DbType.Int64, ObjDetails.TransferDayNo);
                            dbServer.AddInParameter(command2, "CleavageGrade", DbType.Int64, ObjDetails.CleavageGrade);
                            dbServer.AddInParameter(command2, "StageofDevelopmentGrade", DbType.Int64, ObjDetails.StageofDevelopmentGrade);
                            dbServer.AddInParameter(command2, "InnerCellMassGrade", DbType.Int64, ObjDetails.InnerCellMassGrade);
                            dbServer.AddInParameter(command2, "TrophoectodermGrade", DbType.Int64, ObjDetails.TrophoectodermGrade);
                            dbServer.AddInParameter(command2, "CellStage", DbType.String, ObjDetails.CellStage);
                            dbServer.AddInParameter(command2, "VitrificationDate", DbType.DateTime, ObjDetails.VitrificationDate);
                            dbServer.AddInParameter(command2, "VitrificationTime", DbType.DateTime, ObjDetails.VitrificationTime);
                            dbServer.AddInParameter(command2, "VitrificationNo", DbType.String, ObjDetails.VitrificationNo);
                            dbServer.AddInParameter(command2, "IsSaved", DbType.Boolean, true);
                            //
                            //Flag set while saving Freeze Oocytes under Freeze All Oocytes Cycle 
                            dbServer.AddInParameter(command2, "IsFreezeOocytes", DbType.Boolean, ObjDetails.IsFreezeOocytes);

                            dbServer.AddInParameter(command2, "CryoCode", DbType.String, ObjDetails.CryoCode);
                            //

                            //added for donate cryo plan
                            dbServer.AddInParameter(command2, "IsDonateCryo", DbType.Boolean, ObjDetails.IsDonateCryo);
                            dbServer.AddInParameter(command2, "RecepientPatientID", DbType.Int64, ObjDetails.RecepientPatientID);
                            dbServer.AddInParameter(command2, "RecepientPatientUnitID", DbType.Int64, ObjDetails.RecepientPatientUnitID);

                            dbServer.AddInParameter(command2, "IsDonorCycleDonateCryo", DbType.Boolean, ObjDetails.IsDonorCycleDonateCryo);

                            int iStatus = dbServer.ExecuteNonQuery(command2, trans);

                            if (ObjDetails.IsDonateCryo || ObjDetails.IsDonorCycleDonateCryo)
                            {
                                DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrification");

                                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command4, "PatientID", DbType.Int64, ObjDetails.RecepientPatientID);
                                dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, ObjDetails.RecepientPatientUnitID);
                                dbServer.AddInParameter(command4, "PlanTherapyID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyID);
                                dbServer.AddInParameter(command4, "PlanTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyUnitID);
                                dbServer.AddInParameter(command4, "DateTime", DbType.DateTime, BizActionObj.VitrificationMain.DateTime);
                                dbServer.AddInParameter(command4, "VitrificationNo", DbType.String, BizActionObj.VitrificationMain.VitrificationNo);
                                dbServer.AddInParameter(command4, "PickUpDate", DbType.DateTime, BizActionObj.VitrificationMain.PickUpDate);
                                dbServer.AddInParameter(command4, "ConsentForm", DbType.Boolean, BizActionObj.VitrificationMain.ConsentForm);
                                dbServer.AddInParameter(command4, "IsFreezed", DbType.Boolean, BizActionObj.VitrificationMain.IsFreezed);
                                dbServer.AddInParameter(command4, "IsOnlyVitrification", DbType.Boolean, BizActionObj.VitrificationMain.IsOnlyVitrification);
                                dbServer.AddInParameter(command4, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                                dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                                dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                dbServer.AddInParameter(command4, "SrcOoctyID", DbType.Int64, BizActionObj.VitrificationMain.SrcOoctyID);
                                dbServer.AddInParameter(command4, "SrcSemenID", DbType.Int64, BizActionObj.VitrificationMain.SrcSemenID);
                                dbServer.AddInParameter(command4, "SrcOoctyCode", DbType.String, BizActionObj.VitrificationMain.SrcOoctyCode);
                                dbServer.AddInParameter(command4, "SrcSemenCode", DbType.String, BizActionObj.VitrificationMain.SrcSemenCode);
                                dbServer.AddInParameter(command4, "UsedOwnOocyte", DbType.Boolean, BizActionObj.VitrificationMain.UsedOwnOocyte);
                                dbServer.AddInParameter(command4, "IsRefreeze", DbType.Boolean, BizActionObj.VitrificationMain.IsRefeeze);
                                dbServer.AddInParameter(command4, "FromForm", DbType.Int64, 2);
                                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.VitrificationMain.DonateCryoID);
                                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                                dbServer.AddInParameter(command4, "IsFreezeOocytes", DbType.Boolean, BizActionObj.IsFreezeOocytes);
                                dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, BizActionObj.VitrificationMain.ExpiryDate);

                                int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                BizActionObj.VitrificationMain.DonateCryoID = (long)dbServer.GetParameterValue(command4, "ID");


                                DbCommand command5 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateVitrificationDetails");

                                dbServer.AddInParameter(command5, "ID", DbType.Int64, ObjDetails.ID);
                                dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command5, "VitrivicationID", DbType.Int64, BizActionObj.VitrificationMain.DonateCryoID);
                                dbServer.AddInParameter(command5, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command5, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                                dbServer.AddInParameter(command5, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                                dbServer.AddInParameter(command5, "LeafNo", DbType.String, ObjDetails.LeafNo);
                                dbServer.AddInParameter(command5, "EmbDays", DbType.String, ObjDetails.EmbDays);
                                dbServer.AddInParameter(command5, "ColorCodeID", DbType.Int64, ObjDetails.ColorCodeID);
                                dbServer.AddInParameter(command5, "CanId", DbType.Int64, ObjDetails.CanId);
                                dbServer.AddInParameter(command5, "StrawId", DbType.Int64, ObjDetails.StrawId);
                                dbServer.AddInParameter(command5, "GobletShapeId", DbType.Int64, ObjDetails.GobletShapeId);
                                dbServer.AddInParameter(command5, "GobletSizeId", DbType.Int64, ObjDetails.GobletSizeId);
                                dbServer.AddInParameter(command5, "TankId", DbType.Int64, ObjDetails.TankId);
                                dbServer.AddInParameter(command5, "ConistorNo", DbType.Int64, ObjDetails.ConistorNo);
                                dbServer.AddInParameter(command5, "ProtocolTypeID", DbType.Int64, ObjDetails.ProtocolTypeID);
                                dbServer.AddInParameter(command5, "TransferDate", DbType.DateTime, ObjDetails.TransferDate);
                                dbServer.AddInParameter(command5, "TransferDay", DbType.String, ObjDetails.TransferDay);
                                dbServer.AddInParameter(command5, "CellStageID", DbType.String, ObjDetails.CellStageID);
                                dbServer.AddInParameter(command5, "GradeID", DbType.Int64, ObjDetails.GradeID);
                                dbServer.AddInParameter(command5, "EmbStatus", DbType.String, ObjDetails.EmbStatus);
                                dbServer.AddInParameter(command5, "Comments", DbType.String, ObjDetails.Comments);
                                dbServer.AddInParameter(command5, "UsedOwnOocyte", DbType.Boolean, BizActionObj.VitrificationMain.UsedOwnOocyte);
                                if (BizActionObj.VitrificationMain.IsFreezed == true && BizActionObj.VitrificationMain.IsOnlyVitrification == false)
                                    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, true);
                                else
                                    dbServer.AddInParameter(command5, "IsThawingDone", DbType.Boolean, false);

                                dbServer.AddInParameter(command5, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                                dbServer.AddInParameter(command5, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);

                                dbServer.AddInParameter(command5, "UsedByOtherCycle", DbType.Boolean, BizActionObj.VitrificationMain.UsedByOtherCycle);
                                dbServer.AddInParameter(command5, "UsedTherapyID", DbType.Int64, BizActionObj.VitrificationMain.UsedTherapyID);
                                dbServer.AddInParameter(command5, "UsedTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMain.UsedTherapyUnitID);
                                dbServer.AddInParameter(command5, "ReceivingDate", DbType.DateTime, BizActionObj.VitrificationMain.DateTime);

                                //added by neena
                                dbServer.AddInParameter(command5, "TransferDayNo", DbType.Int64, ObjDetails.TransferDayNo);
                                dbServer.AddInParameter(command5, "CleavageGrade", DbType.Int64, ObjDetails.CleavageGrade);
                                dbServer.AddInParameter(command5, "StageofDevelopmentGrade", DbType.Int64, ObjDetails.StageofDevelopmentGrade);
                                dbServer.AddInParameter(command5, "InnerCellMassGrade", DbType.Int64, ObjDetails.InnerCellMassGrade);
                                dbServer.AddInParameter(command5, "TrophoectodermGrade", DbType.Int64, ObjDetails.TrophoectodermGrade);
                                dbServer.AddInParameter(command5, "CellStage", DbType.String, ObjDetails.CellStage);
                                dbServer.AddInParameter(command5, "VitrificationDate", DbType.DateTime, ObjDetails.VitrificationDate);
                                dbServer.AddInParameter(command5, "VitrificationTime", DbType.DateTime, ObjDetails.VitrificationTime);
                                dbServer.AddInParameter(command5, "VitrificationNo", DbType.String, ObjDetails.VitrificationNo);
                                dbServer.AddInParameter(command5, "IsSaved", DbType.Boolean, true);
                                //
                                //Flag set while saving Freeze Oocytes under Freeze All Oocytes Cycle 
                                dbServer.AddInParameter(command5, "IsFreezeOocytes", DbType.Boolean, ObjDetails.IsFreezeOocytes);

                                dbServer.AddInParameter(command5, "CryoCode", DbType.String, ObjDetails.CryoCode);
                                //

                                //added for donate cryo plan
                                dbServer.AddInParameter(command5, "IsDonateCryo", DbType.Boolean, null);
                                dbServer.AddInParameter(command5, "RecepientPatientID", DbType.Int64, null);
                                dbServer.AddInParameter(command5, "RecepientPatientUnitID", DbType.Int64, null);
                                //

                                //added to save patient details who donated the oocytes
                                dbServer.AddInParameter(command5, "IsDonatedCryoReceived", DbType.Boolean, true);
                                dbServer.AddInParameter(command5, "DonorPatientID", DbType.Int64, BizActionObj.VitrificationMain.PatientID);
                                dbServer.AddInParameter(command5, "DonorPatientUnitID", DbType.Int64, BizActionObj.VitrificationMain.PatientUnitID);
                                //

                                int iStatus5 = dbServer.ExecuteNonQuery(command5, trans);
                            }
                        }
                        //}
                    }
                }

                if (BizActionObj.VitrificationMain.IsFreezed == true && BizActionObj.VitrificationMain.IsOnlyVitrification == false && BizActionObj.VitrificationMain.IsCryoWOThaw == false)
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawing");

                    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.VitrificationMain.PatientID);
                    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.VitrificationMain.PatientUnitID);
                    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyID);
                    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyUnitID);
                    dbServer.AddInParameter(command3, "DateTime", DbType.DateTime, null);
                    dbServer.AddInParameter(command3, "LabPersonId", DbType.Int64, null);
                    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);



                    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                    BizActionObj.VitrificationMain.ID = (long)dbServer.GetParameterValue(command3, "ID");

                    if (BizActionObj.VitrificationDetailsList != null && BizActionObj.VitrificationDetailsList.Count > 0)
                    {
                        foreach (var ObjDetails in BizActionObj.VitrificationDetailsList)
                        {
                            DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingDetails");

                            dbServer.AddInParameter(command4, "ID", DbType.Int64, ObjDetails.ID);
                            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "ThawingID", DbType.Int64, BizActionObj.VitrificationMain.ID);
                            dbServer.AddInParameter(command4, "ThawingUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "EmbNumber", DbType.Int64, ObjDetails.EmbNumber);
                            dbServer.AddInParameter(command4, "EmbSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                            dbServer.AddInParameter(command4, "Date", DbType.DateTime, null);
                            dbServer.AddInParameter(command4, "CellStageID", DbType.Int64, ObjDetails.CellStageID);
                            dbServer.AddInParameter(command4, "GradeID", DbType.Int64, ObjDetails.GradeID);
                            dbServer.AddInParameter(command4, "NextPlan", DbType.Boolean, false);
                            dbServer.AddInParameter(command4, "Comments", DbType.String, null);
                            dbServer.AddInParameter(command4, "Status", DbType.String, null);

                            dbServer.AddInParameter(command4, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                            dbServer.AddInParameter(command4, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);
                            //added by neena
                            dbServer.AddInParameter(command4, "TransferDay", DbType.Int64, ObjDetails.TransferDayNo);

                            int iStatus = dbServer.ExecuteNonQuery(command4, trans);
                        }
                    }
                }

                //added by 
                #region Only For Oocyte Cryopreservation
                //if (BizActionObj.VitrificationMainForOocyte != null && BizActionObj.VitrificationMain.IsOnlyForEmbryoVitrification == false)
                //{
                //    DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateForOocyteVitrification");

                //    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PatientID);
                //    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PatientUnitID);
                //    dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PlanTherapyID);
                //    dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PlanTherapyUnitID);
                //    dbServer.AddInParameter(command, "DateTime", DbType.DateTime, BizActionObj.VitrificationMainForOocyte.DateTime);
                //    dbServer.AddInParameter(command, "VitrificationNo", DbType.String, BizActionObj.VitrificationMainForOocyte.VitrificationNo);
                //    dbServer.AddInParameter(command, "PickUpDate", DbType.DateTime, BizActionObj.VitrificationMainForOocyte.PickUpDate);
                //    dbServer.AddInParameter(command, "ConsentForm", DbType.Boolean, BizActionObj.VitrificationMainForOocyte.ConsentForm);
                //    dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.VitrificationMainForOocyte.IsFreezed);
                //    dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, BizActionObj.VitrificationMainForOocyte.IsOnlyVitrification);
                //    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                //    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    dbServer.AddInParameter(command, "SrcOoctyID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.SrcOoctyID);
                //    dbServer.AddInParameter(command, "SrcSemenID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.SrcSemenID);
                //    dbServer.AddInParameter(command, "SrcOoctyCode", DbType.String, BizActionObj.VitrificationMainForOocyte.SrcOoctyCode);
                //    dbServer.AddInParameter(command, "SrcSemenCode", DbType.String, BizActionObj.VitrificationMainForOocyte.SrcSemenCode);
                //    dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, BizActionObj.VitrificationMainForOocyte.UsedOwnOocyte);

                //    dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.EmbryologistID);
                //    dbServer.AddInParameter(command, "AssitantEmbryologistID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.AssitantEmbryologistID);

                //    dbServer.AddInParameter(command, "FromForm", DbType.Int64, 2);
                //    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.VitrificationMainForOocyte.ID);
                //    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //    int intStatus = dbServer.ExecuteNonQuery(command, trans);
                //    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                //    BizActionObj.VitrificationMainForOocyte.ID = (long)dbServer.GetParameterValue(command, "ID");

                //    if (BizActionObj.VitrificationDetailsForOocyteList != null && BizActionObj.VitrificationDetailsForOocyteList.Count > 0)
                //    {
                //        foreach (var ObjDetails in BizActionObj.VitrificationDetailsForOocyteList)
                //        {

                //            //if (BizActionObj.VitrificationDetailsForOocyteObj != null)
                //            //{
                //            //    clsIVFDashBoard_VitrificationDetailsVO ObjDetails = BizActionObj.VitrificationDetailsForOocyteObj;
                //            DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateForOocyteVitrificationDetails");   //old IVFDashboard_AddUpdateVitrificationDetails

                //            dbServer.AddInParameter(command2, "ID", DbType.Int64, ObjDetails.ID);
                //            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //            dbServer.AddInParameter(command2, "VitrivicationID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.ID);
                //            dbServer.AddInParameter(command2, "VitrificationUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //            dbServer.AddInParameter(command2, "OocyteNumber", DbType.Int64, ObjDetails.EmbNumber);
                //            dbServer.AddInParameter(command2, "OocyteSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                //            dbServer.AddInParameter(command2, "LeafNo", DbType.String, ObjDetails.LeafNo);
                //            dbServer.AddInParameter(command2, "OocyteDays", DbType.String, ObjDetails.EmbDays);
                //            dbServer.AddInParameter(command2, "ColorCodeID", DbType.Int64, ObjDetails.ColorCodeID);
                //            dbServer.AddInParameter(command2, "CanId", DbType.Int64, ObjDetails.CanId);
                //            dbServer.AddInParameter(command2, "StrawId", DbType.Int64, ObjDetails.StrawId);
                //            dbServer.AddInParameter(command2, "GobletShapeId", DbType.Int64, ObjDetails.GobletShapeId);
                //            dbServer.AddInParameter(command2, "GobletSizeId", DbType.Int64, ObjDetails.GobletSizeId);
                //            dbServer.AddInParameter(command2, "TankId", DbType.Int64, ObjDetails.TankId);
                //            dbServer.AddInParameter(command2, "ConistorNo", DbType.Int64, ObjDetails.ConistorNo);
                //            dbServer.AddInParameter(command2, "ProtocolTypeID", DbType.Int64, ObjDetails.ProtocolTypeID);
                //            dbServer.AddInParameter(command2, "TransferDate", DbType.DateTime, ObjDetails.TransferDate);
                //            dbServer.AddInParameter(command2, "TransferDay", DbType.String, ObjDetails.TransferDay);
                //            dbServer.AddInParameter(command2, "CellStageID", DbType.String, ObjDetails.CellStageID);
                //            dbServer.AddInParameter(command2, "GradeID", DbType.Int64, ObjDetails.GradeID);
                //            dbServer.AddInParameter(command2, "OocyteStatus", DbType.String, ObjDetails.EmbStatus);
                //            dbServer.AddInParameter(command2, "Comments", DbType.String, ObjDetails.Comments);
                //            dbServer.AddInParameter(command2, "UsedOwnOocyte", DbType.Boolean, BizActionObj.VitrificationMainForOocyte.UsedOwnOocyte);
                //            if (BizActionObj.VitrificationMainForOocyte.IsFreezed == true && BizActionObj.VitrificationMainForOocyte.IsOnlyVitrification == false)
                //                dbServer.AddInParameter(command2, "IsThawingDone", DbType.Boolean, true);
                //            else
                //                dbServer.AddInParameter(command2, "IsThawingDone", DbType.Boolean, false);

                //            dbServer.AddInParameter(command2, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                //            dbServer.AddInParameter(command2, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);

                //            dbServer.AddInParameter(command2, "UsedByOtherCycle", DbType.Boolean, BizActionObj.VitrificationMainForOocyte.UsedByOtherCycle);
                //            dbServer.AddInParameter(command2, "UsedTherapyID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.UsedTherapyID);
                //            dbServer.AddInParameter(command2, "UsedTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.UsedTherapyUnitID);
                //            dbServer.AddInParameter(command2, "ReceivingDate", DbType.DateTime, BizActionObj.VitrificationMainForOocyte.DateTime);

                //            //added by neena
                //            dbServer.AddInParameter(command2, "TransferDayNo", DbType.Int64, 0);
                //            dbServer.AddInParameter(command2, "VitrificationDate", DbType.DateTime, ObjDetails.VitrificationDate);
                //            dbServer.AddInParameter(command2, "VitrificationTime", DbType.DateTime, ObjDetails.VitrificationTime);
                //            dbServer.AddInParameter(command2, "VitrificationNo", DbType.String, ObjDetails.VitrificationNo);
                //            dbServer.AddInParameter(command2, "IsSaved", DbType.Boolean, ObjDetails.IsSaved);
                //            int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                //        }
                //    }
                //    //    }
                //}

                //if (BizActionObj.VitrificationMainForOocyte.IsFreezed == true && BizActionObj.VitrificationMainForOocyte.IsOnlyVitrification == false)
                //{
                //    DbCommand command3 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingForOocyte");   //old IVFDashboard_AddUpdateThawing

                //    dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command3, "PatientID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PatientID);
                //    dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PatientUnitID);
                //    dbServer.AddInParameter(command3, "PlanTherapyID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PlanTherapyID);
                //    dbServer.AddInParameter(command3, "PlanTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.PlanTherapyUnitID);
                //    dbServer.AddInParameter(command3, "DateTime", DbType.DateTime, null);
                //    dbServer.AddInParameter(command3, "LabPersonId", DbType.Int64, null);
                //    dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                //    dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);



                //    dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                //    dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                //    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                //    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                //    BizActionObj.VitrificationMainForOocyte.ID = (long)dbServer.GetParameterValue(command3, "ID");

                //    if (BizActionObj.VitrificationDetailsForOocyteList != null && BizActionObj.VitrificationDetailsForOocyteList.Count > 0)
                //    {
                //        foreach (var ObjDetails in BizActionObj.VitrificationDetailsForOocyteList)
                //        {
                //            DbCommand command4 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateThawingDetailsForOocyte");     //old IVFDashboard_AddUpdateThawingDetails

                //            dbServer.AddInParameter(command4, "ID", DbType.Int64, ObjDetails.ID);
                //            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //            dbServer.AddInParameter(command4, "ThawingID", DbType.Int64, BizActionObj.VitrificationMainForOocyte.ID);
                //            dbServer.AddInParameter(command4, "ThawingUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //            dbServer.AddInParameter(command4, "OocyteNumber", DbType.Int64, ObjDetails.EmbNumber);
                //            dbServer.AddInParameter(command4, "OocyteSerialNumber", DbType.Int64, ObjDetails.EmbSerialNumber);
                //            dbServer.AddInParameter(command4, "Date", DbType.DateTime, null);
                //            dbServer.AddInParameter(command4, "CellStageID", DbType.Int64, ObjDetails.CellStageID);
                //            dbServer.AddInParameter(command4, "GradeID", DbType.Int64, ObjDetails.GradeID);

                //            dbServer.AddInParameter(command4, "PostThawingPlanID", DbType.Int64, ObjDetails.PostThawingPlanID);

                //            dbServer.AddInParameter(command4, "NextPlan", DbType.Boolean, false);
                //            dbServer.AddInParameter(command4, "Comments", DbType.String, null);
                //            dbServer.AddInParameter(command4, "Status", DbType.String, null);

                //            dbServer.AddInParameter(command4, "OocyteDonorID", DbType.Int64, ObjDetails.OocyteDonorID);
                //            dbServer.AddInParameter(command4, "OocyteDonorUnitID", DbType.Int64, ObjDetails.OocyteDonorUnitID);
                //            dbServer.AddInParameter(command4, "CellStage", DbType.String, ObjDetails.CellStage); //added by neena

                //            int iStatus = dbServer.ExecuteNonQuery(command4, trans);
                //        }
                //    }
                //}
                #endregion

                //END


                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.VitrificationMain = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetIVFDashBoard_Vitrification(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetVitrificationBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetVitrificationBizActionVO;
            try
            {
                DbCommand command;
                if (BizAction.IsFreezeOocytes == false)
                {
                    command = dbServer.GetStoredProcCommand("IVFDashboard_GetVitrificationDetails");
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("IVFDashboard_GetVitrificationDetailsForOocytes");      // For IVF ADM Changes
                }

                //DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_GetVitrificationDetails");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.VitrificationMain.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.VitrificationMain.PatientUnitID);
                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.VitrificationMain.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.VitrificationMain.PlanTherapyUnitID);
                dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, BizAction.VitrificationMain.IsOnlyVitrification);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "IsForThawTab", DbType.Boolean, BizAction.IsForThawTab);   // For IVF ADM Changes

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.VitrificationMain.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.VitrificationMain.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.VitrificationMain.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                        BizAction.VitrificationMain.PickUpDate = (DateTime?)(DALHelper.HandleDate(reader["PickUpDate"]));
                        BizAction.VitrificationMain.ConsentForm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ConsentForm"]));
                        BizAction.VitrificationMain.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        BizAction.VitrificationMain.IsOnlyVitrification = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOnlyVitrification"]));
                        BizAction.VitrificationMain.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BizAction.VitrificationMain.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        BizAction.VitrificationMain.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        BizAction.VitrificationMain.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        BizAction.VitrificationMain.DateTime = (DateTime?)(DALHelper.HandleDate(reader["DateTime"]));
                        BizAction.VitrificationMain.SrcOoctyCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcOoctyCode"]));
                        BizAction.VitrificationMain.SrcSemenCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcSemenCode"]));
                        BizAction.VitrificationMain.SrcOoctyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcOoctyID"]));
                        BizAction.VitrificationMain.SrcSemenID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcSemenID"]));
                        BizAction.VitrificationMain.UsedOwnOocyte = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UsedOwnOocyte"]));
                        BizAction.VitrificationRefeezeMain.IsRefeeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreeze"])); //added by neena
                        BizAction.VitrificationMain.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                    }
                }

                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashBoard_VitrificationDetailsVO ETdetails = new clsIVFDashBoard_VitrificationDetailsVO();
                        ETdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ETdetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        ETdetails.ColorCodeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ColorCode"]));
                        ETdetails.CanId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CanId"]));
                        ETdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                        ETdetails.TransferDate = (DateTime?)(DALHelper.HandleDate(reader["TransferDate"]));
                        ETdetails.StrawId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StrawId"]));
                        ETdetails.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        ETdetails.GobletShapeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletShapeId"]));
                        ETdetails.GobletSizeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletSizeId"]));
                        ETdetails.TankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TankId"]));
                        ETdetails.EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                        ETdetails.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                        ETdetails.ConistorNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConistorNo"]));
                        ETdetails.EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"]));
                        ETdetails.EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"]));
                        ETdetails.ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"]));
                        ETdetails.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        ETdetails.IsThawingDone = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsThawingDone"]));
                        ETdetails.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        ETdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                        ETdetails.EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["EmbDays"]));
                        ETdetails.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        ETdetails.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));
                        //added by neena
                        ETdetails.IsFreezeOocytes = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezeOocytes"]));
                        ETdetails.TransferDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransferDayNo"]));
                        ETdetails.CleavageGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["CleavageGrade"]));
                        ETdetails.StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"]));
                        ETdetails.InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"]));
                        ETdetails.TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"]));
                        ETdetails.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        ETdetails.VitrificationDate = (DateTime?)(DALHelper.HandleDate(reader["VitrificationDate"]));
                        ETdetails.VitrificationTime = (DateTime?)(DALHelper.HandleDate(reader["VitrificationTime"]));
                        ETdetails.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                        ETdetails.StageofDevelopmentGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeStr"]));
                        ETdetails.InnerCellMassGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["InnerCellMassGradeStr"]));
                        ETdetails.TrophoectodermGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["TrophoectodermGradeStr"]));
                        ETdetails.IsSaved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSaved"]));
                        ETdetails.IsRefreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreeze"]));
                        ETdetails.IsRefreezeFromOtherCycle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreezeFromOtherCycle"]));
                        ETdetails.CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"]));
                        ETdetails.IsDonateCryo = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonateCryo"]));
                        ETdetails.RecepientPatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecepientPatientID"]));
                        ETdetails.RecepientPatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecepientPatientUnitID"]));
                        ETdetails.IsDonatedCryoReceived = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonatedCryoReceived"]));
                        ETdetails.DonorPatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorPatientID"]));
                        ETdetails.DonorPatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorPatientUnitID"]));
                        ETdetails.IsDonorCycleDonateCryo = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonorCycleDonateCryo"]));
                        ETdetails.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                        ETdetails.ExpiryTime = (DateTime?)(DALHelper.HandleDate(reader["ExpiryTime"]));
                        ETdetails.IsFreshEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreshEmbryoPGDPGS"]));
                        ETdetails.IsFrozenEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFrozenEmbryoPGDPGS"]));
                      
                        //
                        BizAction.VitrificationDetailsList.Add(ETdetails);
                    }
                }

                //added by neena
                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        BizAction.VitrificationRefeezeMain.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.VitrificationRefeezeMain.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.VitrificationRefeezeMain.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                        BizAction.VitrificationRefeezeMain.PickUpDate = (DateTime?)(DALHelper.HandleDate(reader["PickUpDate"]));
                        BizAction.VitrificationRefeezeMain.ConsentForm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ConsentForm"]));
                        BizAction.VitrificationRefeezeMain.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        BizAction.VitrificationRefeezeMain.IsOnlyVitrification = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOnlyVitrification"]));
                        BizAction.VitrificationRefeezeMain.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        BizAction.VitrificationRefeezeMain.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        BizAction.VitrificationRefeezeMain.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        BizAction.VitrificationRefeezeMain.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        BizAction.VitrificationRefeezeMain.DateTime = (DateTime?)(DALHelper.HandleDate(reader["DateTime"]));
                        BizAction.VitrificationRefeezeMain.SrcOoctyCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcOoctyCode"]));
                        BizAction.VitrificationRefeezeMain.SrcSemenCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcSemenCode"]));
                        BizAction.VitrificationRefeezeMain.SrcOoctyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcOoctyID"]));
                        BizAction.VitrificationRefeezeMain.SrcSemenID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcSemenID"]));
                        BizAction.VitrificationRefeezeMain.UsedOwnOocyte = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UsedOwnOocyte"]));
                        BizAction.VitrificationRefeezeMain.IsRefeeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreeze"]));
                        BizAction.VitrificationRefeezeMain.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                    }
                }

                reader.NextResult();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashBoard_VitrificationDetailsVO ETdetails = new clsIVFDashBoard_VitrificationDetailsVO();
                        ETdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ETdetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        ETdetails.ColorCodeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ColorCode"]));
                        ETdetails.CanId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CanId"]));
                        ETdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                        ETdetails.TransferDate = (DateTime?)(DALHelper.HandleDate(reader["TransferDate"]));
                        ETdetails.StrawId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StrawId"]));
                        ETdetails.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        ETdetails.GobletShapeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletShapeId"]));
                        ETdetails.GobletSizeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletSizeId"]));
                        ETdetails.TankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TankId"]));
                        ETdetails.EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                        ETdetails.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                        ETdetails.ConistorNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConistorNo"]));
                        ETdetails.EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"]));
                        ETdetails.EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"]));
                        ETdetails.ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"]));
                        ETdetails.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        ETdetails.IsThawingDone = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsThawingDone"]));
                        ETdetails.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        ETdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                        ETdetails.EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["EmbDays"]));
                        ETdetails.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        ETdetails.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));
                        //added by neena
                        ETdetails.IsFreezeOocytes = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezeOocytes"]));
                        ETdetails.TransferDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransferDayNo"]));
                        ETdetails.CleavageGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["CleavageGrade"]));
                        ETdetails.StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"]));
                        ETdetails.InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"]));
                        ETdetails.TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"]));
                        ETdetails.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        ETdetails.VitrificationDate = (DateTime?)(DALHelper.HandleDate(reader["VitrificationDate"]));
                        ETdetails.VitrificationTime = (DateTime?)(DALHelper.HandleDate(reader["VitrificationTime"]));
                        ETdetails.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                        ETdetails.StageofDevelopmentGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeStr"]));
                        ETdetails.InnerCellMassGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["InnerCellMassGradeStr"]));
                        ETdetails.TrophoectodermGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["TrophoectodermGradeStr"]));
                        ETdetails.IsSaved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSaved"]));
                        ETdetails.IsRefreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreeze"]));
                        ETdetails.IsRefreezeFromOtherCycle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreezeFromOtherCycle"]));
                        ETdetails.CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"]));
                        ETdetails.IsDonateCryo = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonateCryo"]));
                        ETdetails.RecepientPatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecepientPatientID"]));
                        ETdetails.RecepientPatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RecepientPatientUnitID"]));
                        ETdetails.IsDonatedCryoReceived = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonatedCryoReceived"]));
                        ETdetails.DonorPatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorPatientID"]));
                        ETdetails.DonorPatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DonorPatientUnitID"]));
                        ETdetails.IsDonorCycleDonateCryo = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDonorCycleDonateCryo"]));
                        ETdetails.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                        ETdetails.ExpiryTime = (DateTime?)(DALHelper.HandleDate(reader["ExpiryTime"]));
                        ETdetails.IsFreshEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDate(reader["IsFreshEmbryoPGDPGS"]));
                    
                        //
                        BizAction.VitrificationRefeezeDetailsList.Add(ETdetails);
                    }
                }

                ////Added By CDS For Oocyte
                //reader.NextResult();
                //if (reader.HasRows)
                //{
                //    while (reader.Read())
                //    {

                //        BizAction.VitrificationMainForOocyte.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                //        BizAction.VitrificationMainForOocyte.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                //        BizAction.VitrificationMainForOocyte.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                //        BizAction.VitrificationMainForOocyte.PickUpDate = (DateTime?)(DALHelper.HandleDate(reader["PickUpDate"]));
                //        BizAction.VitrificationMainForOocyte.ConsentForm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ConsentForm"]));
                //        BizAction.VitrificationMainForOocyte.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                //        BizAction.VitrificationMainForOocyte.IsOnlyVitrification = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOnlyVitrification"]));
                //        BizAction.VitrificationMainForOocyte.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                //        BizAction.VitrificationMainForOocyte.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                //        BizAction.VitrificationMainForOocyte.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                //        BizAction.VitrificationMainForOocyte.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                //        BizAction.VitrificationMainForOocyte.DateTime = (DateTime?)(DALHelper.HandleDate(reader["DateTime"]));
                //        BizAction.VitrificationMainForOocyte.SrcOoctyCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcOoctyCode"]));
                //        BizAction.VitrificationMainForOocyte.SrcSemenCode = Convert.ToString(DALHelper.HandleDBNull(reader["SrcSemenCode"]));
                //        BizAction.VitrificationMainForOocyte.SrcOoctyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcOoctyID"]));
                //        BizAction.VitrificationMainForOocyte.SrcSemenID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcSemenID"]));
                //        BizAction.VitrificationMainForOocyte.UsedOwnOocyte = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UsedOwnOocyte"]));

                //        BizAction.VitrificationMainForOocyte.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                //        BizAction.VitrificationMainForOocyte.AssitantEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssistantEmbryologistID"]));
                //    }
                //}

                //reader.NextResult();
                //if (reader.HasRows)
                //{
                //    while (reader.Read())
                //    {
                //        clsIVFDashBoard_VitrificationDetailsVO ETdetailsForOocyte = new clsIVFDashBoard_VitrificationDetailsVO();
                //        ETdetailsForOocyte.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                //        ETdetailsForOocyte.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                //        ETdetailsForOocyte.ColorCodeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ColorCode"]));
                //        ETdetailsForOocyte.CanId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CanId"]));
                //        ETdetailsForOocyte.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                //        ETdetailsForOocyte.TransferDate = (DateTime?)(DALHelper.HandleDate(reader["TransferDate"]));
                //        ETdetailsForOocyte.StrawId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StrawId"]));
                //        // ETdetailsForOocyte.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                //        ETdetailsForOocyte.OocyteGradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                //        ETdetailsForOocyte.GobletShapeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletShapeId"]));
                //        ETdetailsForOocyte.GobletSizeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletSizeId"]));
                //        ETdetailsForOocyte.TankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TankId"]));
                //        ETdetailsForOocyte.EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["OocyteStatus"]));
                //        ETdetailsForOocyte.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                //        ETdetailsForOocyte.ConistorNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConistorNo"]));
                //        ETdetailsForOocyte.EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                //        ETdetailsForOocyte.EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteSerialNumber"]));
                //        ETdetailsForOocyte.ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"]));
                //        ETdetailsForOocyte.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                //        ETdetailsForOocyte.IsThawingDone = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsThawingDone"]));
                //        ETdetailsForOocyte.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                //        ETdetailsForOocyte.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                //        ETdetailsForOocyte.EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["OocyteDays"]));
                //        ETdetailsForOocyte.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                //        ETdetailsForOocyte.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));
                //        BizAction.VitrificationDetailsForOocyteList.Add(ETdetailsForOocyte);

                //        //added by neena
                //        ETdetailsForOocyte.TransferDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransferDayNo"]));
                //        ETdetailsForOocyte.VitrificationDate = (DateTime?)(DALHelper.HandleDate(reader["VitrificationDate"]));
                //        ETdetailsForOocyte.VitrificationTime = (DateTime?)(DALHelper.HandleDate(reader["VitrificationTime"]));
                //        ETdetailsForOocyte.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                //        ETdetailsForOocyte.IsSaved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSaved"]));
                //        //
                //    }
                //}

                ////EMD 

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
        public override IValueObject GetIVFDashBoard_PreviousEmbFromVitrification(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIVFDashboard_GetPreviousVitrificationBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetPreviousVitrificationBizActionVO;
            try
            {
                DbCommand command;
                if (BizAction.IsFreezeOocytes == true)      // For IVF ADM Changes
                {
                    command = dbServer.GetStoredProcCommand("IVFDashboard_PreviousEmbFromVitrificationForOocytes");     //IsFreezeOocytes Flag set to retrive Freeze Oocytes under FE ICSI Cycle from Freeze All Oocytes Cycle
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("IVFDashboard_PreviousEmbFromVitrification");
                }

                //DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_PreviousEmbFromVitrification");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.VitrificationMain.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.VitrificationMain.PatientUnitID);
                dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, BizAction.VitrificationMain.UsedOwnOocyte);
                dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, BizAction.VitrificationMain.IsOnlyVitrification);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashBoard_VitrificationDetailsVO ETdetails = new clsIVFDashBoard_VitrificationDetailsVO();
                        ETdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ETdetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        ETdetails.ColorCodeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ColorCode"]));
                        ETdetails.CanId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CanId"]));
                        ETdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                        ETdetails.TransferDate = (DateTime?)(DALHelper.HandleDate(reader["TransferDate"]));
                        ETdetails.StrawId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StrawId"]));
                        ETdetails.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        ETdetails.GobletShapeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletShapeId"]));
                        ETdetails.GobletSizeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletSizeId"]));
                        ETdetails.TankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TankId"]));
                        ETdetails.EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                        ETdetails.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                        ETdetails.ConistorNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConistorNo"]));
                        ETdetails.EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"]));
                        ETdetails.EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"]));
                        ETdetails.ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"]));
                        ETdetails.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                        ETdetails.IsThawingDone = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsThawingDone"]));
                        ETdetails.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        ETdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                        ETdetails.EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["EmbDays"]));
                        ETdetails.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                        ETdetails.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));

                        //added by neena
                        ETdetails.TransferDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransferDayNo"]));
                        ETdetails.CleavageGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["CleavageGrade"]));
                        ETdetails.StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"]));
                        ETdetails.InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"]));
                        ETdetails.TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"]));
                        ETdetails.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        ETdetails.VitrificationDate = (DateTime?)(DALHelper.HandleDate(reader["VitrificationDate"]));
                        ETdetails.VitrificationTime = (DateTime?)(DALHelper.HandleDate(reader["VitrificationTime"]));
                        ETdetails.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                        ETdetails.StageofDevelopmentGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeStr"]));
                        ETdetails.InnerCellMassGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["InnerCellMassGradeStr"]));
                        ETdetails.TrophoectodermGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["TrophoectodermGradeStr"]));
                        //

                        BizAction.VitrificationDetailsList.Add(ETdetails);
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

        //Added By CDS 
        public override IValueObject GetIVFDashBoard_PreviousOocyteFromVitrification(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();
            clsIVFDashboard_GetPreviousVitrificationBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetPreviousVitrificationBizActionVO;
            try
            {

                //    DbCommand command = dbServer.GetStoredProcCommand("IVFDashboard_PreviousEmbFromVitrification");
                //    dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.VitrificationMainForOocyte.PatientID);
                //    dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizAction.VitrificationMainForOocyte.PatientUnitID);
                //    dbServer.AddInParameter(command, "UsedOwnOocyte", DbType.Boolean, BizAction.VitrificationMainForOocyte.UsedOwnOocyte);
                //    dbServer.AddInParameter(command, "IsOnlyVitrification", DbType.Boolean, BizAction.VitrificationMainForOocyte.IsOnlyVitrification);
                //    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                //    if (reader.HasRows)
                //    {
                //        while (reader.Read())
                //        {
                //            clsIVFDashBoard_VitrificationDetailsVO ETdetails = new clsIVFDashBoard_VitrificationDetailsVO();
                //            ETdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                //            ETdetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                //            ETdetails.ColorCodeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ColorCode"]));
                //            ETdetails.CanId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CanId"]));
                //            ETdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                //            ETdetails.TransferDate = (DateTime?)(DALHelper.HandleDate(reader["TransferDate"]));
                //            ETdetails.StrawId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StrawId"]));
                //            ETdetails.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                //            ETdetails.GobletShapeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletShapeId"]));
                //            ETdetails.GobletSizeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GobletSizeId"]));
                //            ETdetails.TankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TankId"]));
                //            ETdetails.EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                //            ETdetails.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                //            ETdetails.ConistorNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConistorNo"]));
                //            ETdetails.EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"]));
                //            ETdetails.EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"]));
                //            ETdetails.ProtocolTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ProtocolTypeID"]));
                //            ETdetails.CellStageID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CellStageID"]));
                //            ETdetails.IsThawingDone = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsThawingDone"]));
                //            ETdetails.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                //            ETdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                //            ETdetails.EmbDays = Convert.ToString(DALHelper.HandleDBNull(reader["EmbDays"]));
                //            ETdetails.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));
                //            ETdetails.OocyteDonorUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorUnitID"]));
                //            BizAction.VitrificationDetailsForOocyteList.Add(ETdetails);
                //        }
                //    }
                //    reader.Close();
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

        public override IValueObject GetOocyteVitrificationForCryoBank(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBank BizAction = (valueObject) as cls_IVFDashboard_GetOocyteVitrificationDetailsForCryoBank;
            BizAction.Vitrification = new clsIVFDashboard_GetVitrificationBizActionVO();
            //BizAction.Vitrification. = new List<clsPatientGeneralVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVFDashboard_GetOocyteVirtificationDetailsForEmbryoCryoBank");  //CIMS_IVFDashboard_GetVirtificationDetailsForEmbryoCryoBank

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizAction.SearchExpression);
                if (BizAction.FName != null)
                    dbServer.AddInParameter(command, "FName", DbType.String, BizAction.FName + "%");
                else
                    dbServer.AddInParameter(command, "FName", DbType.String, null);
                if (BizAction.MName != null)// Security.base64Encode(BizAction.FName) + "%");
                    dbServer.AddInParameter(command, "MName", DbType.String, BizAction.MName + "%"); //Security.base64Encode(BizAction.MName) + "%");
                else
                    dbServer.AddInParameter(command, "MName", DbType.String, null);
                if (BizAction.LName != null)
                    dbServer.AddInParameter(command, "LName", DbType.String, BizAction.LName + "%"); //Security.base64Encode(BizAction.LName) + "%");
                else
                    dbServer.AddInParameter(command, "LName", DbType.String, null);
                if (BizAction.FamilyName != null)
                    dbServer.AddInParameter(command, "FamilyName", DbType.String, BizAction.FamilyName + "%"); //Security.base64Encode(BizAction.FamilyName) + "%");
                else
                    dbServer.AddInParameter(command, "FamilyName", DbType.String, null);
                if (BizAction.MRNo != null)
                    dbServer.AddInParameter(command, "MRNo", DbType.String, BizAction.MRNo + "%");
                else
                    dbServer.AddInParameter(command, "MRNo", DbType.String, null);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizAction.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizAction.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashBoard_VitrificationDetailsVO vitdetails = new clsIVFDashBoard_VitrificationDetailsVO();
                        vitdetails.VitrivicationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        vitdetails.VitrificationUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        vitdetails.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                        vitdetails.VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["VitrificationDate"]));
                        vitdetails.EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteSerialNumber"]));
                        vitdetails.EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteNumber"]));
                        vitdetails.ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]));
                        vitdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                        vitdetails.TransferDate = Convert.ToDateTime(DALHelper.HandleDate(reader["TransferDate"]));
                        vitdetails.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        vitdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                        vitdetails.EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["OocyteStatus"]));
                        vitdetails.Tank = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"]));
                        vitdetails.Conistor = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"]));
                        vitdetails.GobletSize = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"]));
                        vitdetails.GobletShape = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"]));
                        vitdetails.Can = Convert.ToString(DALHelper.HandleDBNull(reader["Can"]));
                        vitdetails.Straw = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"]));
                        vitdetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        vitdetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        vitdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitificationDetailsID"]));
                        vitdetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitificationDetailsUnitID"]));
                        vitdetails.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        vitdetails.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));
                        //vitdetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));


                        BizAction.Vitrification.VitrificationDetailsForOocyteList.Add(vitdetails);
                    }
                }
                reader.NextResult();
                BizAction.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");


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
        //END

        public override IValueObject GetVitrificationForCryoBank(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            cls_IVFDashboard_GetVitrificationDetailsForCryoBank BizAction = (valueObject) as cls_IVFDashboard_GetVitrificationDetailsForCryoBank;
            BizAction.Vitrification = new clsIVFDashboard_GetVitrificationBizActionVO();
            //BizAction.Vitrification. = new List<clsPatientGeneralVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVFDashboard_GetVirtificationDetailsForEmbryoCryoBank");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizAction.PatientID);    //added by neena  
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, BizAction.SearchExpression);
                if (BizAction.FName != null)
                    dbServer.AddInParameter(command, "FName", DbType.String, BizAction.FName + "%");
                else
                    dbServer.AddInParameter(command, "FName", DbType.String, null);
                if (BizAction.MName != null)// Security.base64Encode(BizAction.FName) + "%");
                    dbServer.AddInParameter(command, "MName", DbType.String, BizAction.MName + "%"); //Security.base64Encode(BizAction.MName) + "%");
                else
                    dbServer.AddInParameter(command, "MName", DbType.String, null);
                if (BizAction.LName != null)
                    dbServer.AddInParameter(command, "LName", DbType.String, BizAction.LName + "%"); //Security.base64Encode(BizAction.LName) + "%");
                else
                    dbServer.AddInParameter(command, "LName", DbType.String, null);
                if (BizAction.FamilyName != null)
                    dbServer.AddInParameter(command, "FamilyName", DbType.String, BizAction.FamilyName + "%"); //Security.base64Encode(BizAction.FamilyName) + "%");
                else
                    dbServer.AddInParameter(command, "FamilyName", DbType.String, null);
                if (BizAction.MRNo != null)
                    dbServer.AddInParameter(command, "MRNo", DbType.String, BizAction.MRNo + "%");
                else
                    dbServer.AddInParameter(command, "MRNo", DbType.String, null);
                //rohini
                dbServer.AddInParameter(command, "Cane", DbType.Int64, BizAction.Cane);
                //
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizAction.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizAction.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                dbServer.AddInParameter(command, "IsFreezeOocytes", DbType.Boolean, BizAction.IsFreezeOocytes);     // For IVF ADM Changes

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsIVFDashBoard_VitrificationDetailsVO vitdetails = new clsIVFDashBoard_VitrificationDetailsVO();
                        vitdetails.VitrivicationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        vitdetails.VitrificationUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        //vitdetails.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                        //vitdetails.VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["VitrificationDate"]));
                        vitdetails.EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"]));
                        vitdetails.EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"]));
                        vitdetails.ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]));
                        vitdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                        vitdetails.TransferDate = Convert.ToDateTime(DALHelper.HandleDate(reader["TransferDate"]));
                        vitdetails.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        vitdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                        vitdetails.EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                        vitdetails.Tank = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"]));
                        vitdetails.Conistor = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"]));
                        vitdetails.GobletSize = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"]));
                        vitdetails.GobletShape = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"]));
                        vitdetails.Can = Convert.ToString(DALHelper.HandleDBNull(reader["Can"]));
                        vitdetails.Straw = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"]));
                        vitdetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        vitdetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        vitdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitificationDetailsID"]));
                        vitdetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitificationDetailsUnitID"]));
                        vitdetails.PlanTherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyID"]));
                        vitdetails.PlanTherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanTherapyUnitID"]));

                        //added by neena
                        vitdetails.IsFreezeOocytes = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezeOocytes"]));
                        vitdetails.TransferDayNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransferDayNo"]));
                        vitdetails.CleavageGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["CleavageGrade"]));
                        vitdetails.StageofDevelopmentGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["StageofDevelopmentGrade"]));
                        vitdetails.InnerCellMassGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["InnerCellMassGrade"]));
                        vitdetails.TrophoectodermGrade = Convert.ToInt64(DALHelper.HandleDBNull(reader["TrophoectodermGrade"]));
                        vitdetails.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        vitdetails.VitrificationDate = (DateTime?)(DALHelper.HandleDate(reader["VitrificationDate"]));
                        vitdetails.VitrificationTime = (DateTime?)(DALHelper.HandleDate(reader["VitrificationTime"]));
                        vitdetails.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                        vitdetails.StageofDevelopmentGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["StageofDevelopmentGradeStr"]));
                        vitdetails.InnerCellMassGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["InnerCellMassGradeStr"]));
                        vitdetails.TrophoectodermGradeStr = Convert.ToString(DALHelper.HandleDBNull(reader["TrophoectodermGradeStr"]));
                        vitdetails.IsSaved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSaved"]));
                        vitdetails.GradeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"]));
                        vitdetails.IsRefreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreeze"]));
                        vitdetails.IsRefreezeFromOtherCycle = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRefreezeFromOtherCycle"]));
                        vitdetails.CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"]));
                        vitdetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        vitdetails.DonorMRNo = Convert.ToString(DALHelper.HandleDBNull(reader["DonorMRNO"]));
                        vitdetails.DonorPatientName = Convert.ToString(DALHelper.HandleDBNull(reader["DonorPatientName"]));

                        vitdetails.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                        vitdetails.ExpiryTime = (DateTime?)(DALHelper.HandleDate(reader["ExpiryTime"]));
                        vitdetails.LongTerm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["LongTerm"]));
                        vitdetails.ShortTerm = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ShortTerm"]));
                        vitdetails.IsFreshEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreshEmbryoPGDPGS"]));
                        vitdetails.IsFrozenEmbryoPGDPGS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFrozenEmbryoPGDPGS"]));
                        //



                        BizAction.Vitrification.VitrificationDetailsList.Add(vitdetails);
                    }
                }
                reader.NextResult();
                BizAction.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");


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

        public override IValueObject UpdateVitrificationDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            //  throw new NotImplementedException();

            clsIVFDashboard_UpdateVitrificationDetailsBizActionVO BizActionObj = valueObject as clsIVFDashboard_UpdateVitrificationDetailsBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.VitrificationDetailsList != null && BizActionObj.VitrificationDetailsList.Count > 0)
                {
                    foreach (var ObjDetails in BizActionObj.VitrificationDetailsList)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_UpdateVitrificationDetails");

                        dbServer.AddInParameter(command2, "ID", DbType.Int64, ObjDetails.ID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, ObjDetails.UnitID);
                        dbServer.AddInParameter(command2, "VitrificationID", DbType.Int64, ObjDetails.VitrivicationID);
                        dbServer.AddInParameter(command2, "VitrificationUnitID", DbType.Int64, ObjDetails.VitrificationUnitID);
                        dbServer.AddInParameter(command2, "UsedByOtherCycle", DbType.Boolean, true);
                        dbServer.AddInParameter(command2, "UsedTherapyID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyID);
                        dbServer.AddInParameter(command2, "UsedTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyUnitID);
                        dbServer.AddInParameter(command2, "ReceivingDate", DbType.DateTime, BizActionObj.VitrificationMain.ReceivingDate);
                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                    }

                    if (BizActionObj.VitrificationMain.SerialOocyteNumberString != string.Empty)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("IVFDashBoard_AddDay0OocListInGhaphicalRepresentationTableForEmbryoRecipient");
                        dbServer.AddInParameter(command, "ID", DbType.Int64, 0);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "PatientID", DbType.Int64, BizActionObj.VitrificationMain.PatientID);
                        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, BizActionObj.VitrificationMain.PatientUnitID);
                        dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyID);
                        dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMain.PlanTherapyUnitID);
                        dbServer.AddInParameter(command, "DonorPatientID", DbType.Int64, BizActionObj.VitrificationMain.DonorPatientID);
                        dbServer.AddInParameter(command, "DonorPatientUnitID", DbType.Int64, BizActionObj.VitrificationMain.DonorPatientUnitID);
                        //dbServer.AddInParameter(command, "DonorPlanTherapyID", DbType.Int64, BizActionObj.VitrificationMain.DonorPlanTherapyID);
                        //dbServer.AddInParameter(command, "DonorPlanTherapyUnitID", DbType.Int64, BizActionObj.VitrificationMain.DonorPlanTherapyUnitID);
                        dbServer.AddInParameter(command, "SerialOocyteNumberString", DbType.String, BizActionObj.VitrificationMain.SerialOocyteNumberString);
                        dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                        int iStatus1 = dbServer.ExecuteNonQuery(command, trans);
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.VitrificationMain = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetUsedEmbryoDetails(IValueObject valueObject, clsUserVO UserVo)
        {

            clsIVFDashboard_GetUsedEmbryoDetailsBizActionVO BizAction = (valueObject) as clsIVFDashboard_GetUsedEmbryoDetailsBizActionVO;


            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVFDashboard_GetUsedEmbryoDetails");

                dbServer.AddInParameter(command, "PlanTherapyID", DbType.Int64, BizAction.VitrificationMain.PlanTherapyID);
                dbServer.AddInParameter(command, "PlanTherapyUnitID", DbType.Int64, BizAction.VitrificationMain.PlanTherapyUnitID);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.VitrificationDetailsList == null)
                        BizAction.VitrificationDetailsList = new List<clsIVFDashBoard_VitrificationDetailsVO>();
                    while (reader.Read())
                    {
                        clsIVFDashBoard_VitrificationDetailsVO vitdetails = new clsIVFDashBoard_VitrificationDetailsVO();
                        vitdetails.VitrivicationID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        vitdetails.VitrificationUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        vitdetails.VitrificationNo = Convert.ToString(DALHelper.HandleDBNull(reader["VitrificationNo"]));
                        vitdetails.VitrificationDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["VitrificationDate"]));
                        vitdetails.EmbSerialNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbSerialNumber"]));
                        vitdetails.EmbNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNumber"]));
                        vitdetails.ColorCode = Convert.ToString(DALHelper.HandleDBNull(reader["ColorCode"]));
                        vitdetails.TransferDay = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));
                        vitdetails.TransferDate = Convert.ToDateTime(DALHelper.HandleDate(reader["TransferDate"]));
                        vitdetails.CellStage = Convert.ToString(DALHelper.HandleDBNull(reader["CellStage"]));
                        vitdetails.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                        vitdetails.EmbStatus = Convert.ToString(DALHelper.HandleDBNull(reader["EmbStatus"]));
                        vitdetails.Tank = Convert.ToString(DALHelper.HandleDBNull(reader["Tank"]));
                        vitdetails.Conistor = Convert.ToString(DALHelper.HandleDBNull(reader["Canister"]));
                        vitdetails.GobletSize = Convert.ToString(DALHelper.HandleDBNull(reader["GobletSize"]));
                        vitdetails.GobletShape = Convert.ToString(DALHelper.HandleDBNull(reader["GobletShape"]));
                        vitdetails.Can = Convert.ToString(DALHelper.HandleDBNull(reader["Can"]));
                        vitdetails.Straw = Convert.ToString(DALHelper.HandleDBNull(reader["Straw"]));
                        vitdetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        vitdetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        vitdetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitificationDetailsID"]));
                        vitdetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VitificationDetailsUnitID"]));
                        vitdetails.ReceivingDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivingDate"]));
                        BizAction.VitrificationDetailsList.Add(vitdetails);
                    }
                }
                reader.NextResult();


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


        //added by neena
        public override IValueObject AddUpdateDonateDiscardOocyteForCryoBank(IValueObject valueObject, clsUserVO UserVo)
        {
            //  throw new NotImplementedException();

            cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank BizActionObj = valueObject as cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank;

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.Vitrification.VitrificationDetailsList != null && BizActionObj.Vitrification.VitrificationDetailsList.Count > 0)
                {
                    foreach (var item in BizActionObj.Vitrification.VitrificationDetailsList)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank");
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "VitrivicationID", DbType.Int64, item.VitrivicationID);
                        dbServer.AddInParameter(command2, "VitrificationUnitID", DbType.Int64, item.VitrificationUnitID);
                        dbServer.AddInParameter(command2, "EmbSerialNumber", DbType.Int64, item.EmbSerialNumber);
                        dbServer.AddInParameter(command2, "EmbNumber", DbType.Int64, item.EmbNumber);
                        dbServer.AddInParameter(command2, "PlanID", DbType.Int64, item.PlanId);
                        dbServer.AddInParameter(command2, "DonorPatientID", DbType.Int64, item.DonorPatientID);
                        dbServer.AddInParameter(command2, "DonorPatientUnitID", DbType.Int64, item.DonorPatientUnitID);
                        dbServer.AddInParameter(command2, "RecepientPatientID", DbType.Int64, item.RecepientPatientID);
                        dbServer.AddInParameter(command2, "RecepientPatientUnitID", DbType.Int64, item.RecepientPatientUnitID);
                        dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                    }
                }


                //if (BizActionObj.VitrificationDetails != null)
                //{
                //    DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank");
                //    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command2, "VitrivicationID", DbType.Int64, BizActionObj.VitrificationDetails.VitrivicationID);
                //    dbServer.AddInParameter(command2, "VitrificationUnitID", DbType.Int64, BizActionObj.VitrificationDetails.VitrificationUnitID);
                //    dbServer.AddInParameter(command2, "EmbSerialNumber", DbType.Int64, BizActionObj.VitrificationDetails.EmbSerialNumber);
                //    dbServer.AddInParameter(command2, "EmbNumber", DbType.Int64, BizActionObj.VitrificationDetails.EmbNumber);
                //    dbServer.AddInParameter(command2, "PlanID", DbType.Int64, BizActionObj.VitrificationDetails.PlanId);
                //    dbServer.AddInParameter(command2, "DonorPatientID", DbType.Int64, BizActionObj.VitrificationDetails.DonorPatientID);
                //    dbServer.AddInParameter(command2, "DonorPatientUnitID", DbType.Int64, BizActionObj.VitrificationDetails.DonorPatientUnitID);
                //    dbServer.AddInParameter(command2, "RecepientPatientID", DbType.Int64, BizActionObj.VitrificationDetails.RecepientPatientID);
                //    dbServer.AddInParameter(command2, "RecepientPatientUnitID", DbType.Int64, BizActionObj.VitrificationDetails.RecepientPatientUnitID);
                //    dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                //    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    int iStatus = dbServer.ExecuteNonQuery(command2, trans);

                //}               

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Vitrification = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }


        public override IValueObject AddUpdateDonateDiscardEmbryoForCryoBank(IValueObject valueObject, clsUserVO UserVo)
        {
            //  throw new NotImplementedException();

            cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank BizActionObj = valueObject as cls_IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank;

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                if (BizActionObj.Vitrification.VitrificationDetailsList != null && BizActionObj.Vitrification.VitrificationDetailsList.Count > 0)
                {
                    foreach (var item in BizActionObj.Vitrification.VitrificationDetailsList)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateDonateDiscardEmbryoForCryoBank");
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "VitrivicationID", DbType.Int64, item.VitrivicationID);
                        dbServer.AddInParameter(command2, "VitrificationUnitID", DbType.Int64, item.VitrificationUnitID);
                        dbServer.AddInParameter(command2, "EmbSerialNumber", DbType.Int64, item.EmbSerialNumber);
                        dbServer.AddInParameter(command2, "EmbNumber", DbType.Int64, item.EmbNumber);
                        dbServer.AddInParameter(command2, "PlanID", DbType.Int64, item.PlanId);
                        dbServer.AddInParameter(command2, "DonorPatientID", DbType.Int64, item.DonorPatientID);
                        dbServer.AddInParameter(command2, "DonorPatientUnitID", DbType.Int64, item.DonorPatientUnitID);
                        dbServer.AddInParameter(command2, "RecepientPatientID", DbType.Int64, item.RecepientPatientID);
                        dbServer.AddInParameter(command2, "RecepientPatientUnitID", DbType.Int64, item.RecepientPatientUnitID);
                        dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                    }
                }


                //if (BizActionObj.VitrificationDetails != null)
                //{
                //    DbCommand command2 = dbServer.GetStoredProcCommand("IVFDashboard_AddUpdateDonateDiscardOocyteForCryoBank");
                //    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command2, "VitrivicationID", DbType.Int64, BizActionObj.VitrificationDetails.VitrivicationID);
                //    dbServer.AddInParameter(command2, "VitrificationUnitID", DbType.Int64, BizActionObj.VitrificationDetails.VitrificationUnitID);
                //    dbServer.AddInParameter(command2, "EmbSerialNumber", DbType.Int64, BizActionObj.VitrificationDetails.EmbSerialNumber);
                //    dbServer.AddInParameter(command2, "EmbNumber", DbType.Int64, BizActionObj.VitrificationDetails.EmbNumber);
                //    dbServer.AddInParameter(command2, "PlanID", DbType.Int64, BizActionObj.VitrificationDetails.PlanId);
                //    dbServer.AddInParameter(command2, "DonorPatientID", DbType.Int64, BizActionObj.VitrificationDetails.DonorPatientID);
                //    dbServer.AddInParameter(command2, "DonorPatientUnitID", DbType.Int64, BizActionObj.VitrificationDetails.DonorPatientUnitID);
                //    dbServer.AddInParameter(command2, "RecepientPatientID", DbType.Int64, BizActionObj.VitrificationDetails.RecepientPatientID);
                //    dbServer.AddInParameter(command2, "RecepientPatientUnitID", DbType.Int64, BizActionObj.VitrificationDetails.RecepientPatientUnitID);
                //    dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                //    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                //    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                //    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                //    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //    int iStatus = dbServer.ExecuteNonQuery(command2, trans);

                //}               

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Vitrification = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }


        public override IValueObject AddUpdateIVFDashBoard_RenewalDate(IValueObject valueObject, clsUserVO UserVo)
        {
            //  throw new NotImplementedException();

            clsIVFDashboard_AddUpdateVitrificationBizActionVO BizActionObj = valueObject as clsIVFDashboard_AddUpdateVitrificationBizActionVO;

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVFDashBoard_AddUpdateRenewalDate");

                dbServer.AddInParameter(command, "VitificationID", DbType.Int64, BizActionObj.VitificationID);
                dbServer.AddInParameter(command, "VitificationUnitID", DbType.Int64, BizActionObj.VitificationUnitID);
                dbServer.AddInParameter(command, "VitificationDetailsID", DbType.Int64, BizActionObj.VitificationDetailsID);
                dbServer.AddInParameter(command, "VitificationDetailsUnitID", DbType.Int64, BizActionObj.VitificationDetailsUnitID);
                dbServer.AddInParameter(command, "SpremFreezingID", DbType.Int64, BizActionObj.SpremFreezingID);
                dbServer.AddInParameter(command, "SpremFreezingUnitID", DbType.Int64, BizActionObj.SpremFreezingUnitID);
                dbServer.AddInParameter(command, "IsOocyteFreezed", DbType.Boolean, BizActionObj.IsOocyteFreezed);
                dbServer.AddInParameter(command, "IsSprem", DbType.Boolean, BizActionObj.IsSprem);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.StartDate);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, BizActionObj.StartTime);
                dbServer.AddInParameter(command, "ExpiryDate", DbType.DateTime, BizActionObj.ExpiryDate);
                dbServer.AddInParameter(command, "ExpiryTime", DbType.DateTime, BizActionObj.ExpiryTime);
                dbServer.AddInParameter(command, "LongTerm", DbType.Boolean, BizActionObj.LongTerm);
                dbServer.AddInParameter(command, "ShortTerm", DbType.Boolean, BizActionObj.ShortTerm);
              
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                //BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                //BizActionObj.VitrificationMain.ID = (long)dbServer.GetParameterValue(command, "ID");
            

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
                BizActionObj.VitrificationMain = null;
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

    }
}


