using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    class clsIVFEmbryoTransferDAL : clsBaseIVFEmbryoTransferDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsIVFEmbryoTransferDAL()
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

        /*#region FemaleLabDay0 (Done By : Harish)
        public override IValueObject AddUpdateFemaleLabDay0(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;

            clsAddUpdateFemaleLabDay0BizActionVO BizActionObj = valueObject as clsAddUpdateFemaleLabDay0BizActionVO;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command;
                if (BizActionObj.IsUpdate == false)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_IVF_AddFemaleLabDay0");
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.FemaleLabDay0.ID);

                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_IVF_UpdateFemaleLabDay0");
                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.FemaleLabDay0.ID);

                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleId", DbType.Int64, BizActionObj.FemaleLabDay0.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitId", DbType.Int64, BizActionObj.FemaleLabDay0.CoupleUnitID);

                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.FemaleLabDay0.ProcDate);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionObj.FemaleLabDay0.ProcTime);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.FemaleLabDay0.EmbryologistID);
                dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, BizActionObj.FemaleLabDay0.AssEmbryologistID);
                dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, BizActionObj.FemaleLabDay0.AnesthetistID);
                dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, BizActionObj.FemaleLabDay0.AssAnesthetistID);
                dbServer.AddInParameter(command, "IVFCycleCount", DbType.Int64, BizActionObj.FemaleLabDay0.IVFCycleCount);
                dbServer.AddInParameter(command, "SrcNeedleID", DbType.Int64, BizActionObj.FemaleLabDay0.SrcOfNeedleID);
                dbServer.AddInParameter(command, "NeedleCompanyID", DbType.Int64, BizActionObj.FemaleLabDay0.NeedleCompanyID);
                dbServer.AddInParameter(command, "SrcOocyteID", DbType.Int64, BizActionObj.FemaleLabDay0.SrcOfOocyteID);
                dbServer.AddInParameter(command, "OocyteDonorID", DbType.Int64, BizActionObj.FemaleLabDay0.OocyteDonorID);
                dbServer.AddInParameter(command, "SrcOfSemen", DbType.Int64, BizActionObj.FemaleLabDay0.SrcOfSemenID);
                dbServer.AddInParameter(command, "SemenDonorID", DbType.Int64, BizActionObj.FemaleLabDay0.SemenDonorID);
                dbServer.AddInParameter(command, "TreatmentPlanID", DbType.Int64, BizActionObj.FemaleLabDay0.TreatmentTypeID);

                dbServer.AddInParameter(command, "ICSICompleteionTime", DbType.DateTime, BizActionObj.FemaleLabDay0.ICSICompletionTime);
                dbServer.AddInParameter(command, "SrcDenudingNeedleID", DbType.Int64, BizActionObj.FemaleLabDay0.SourceOfDenudingNeedle);
                dbServer.AddInParameter(command, "FertilizationCheckTime", DbType.DateTime, BizActionObj.FemaleLabDay0.FertilizationCheckTime);

                dbServer.AddInParameter(command, "OocytePreparationMedia", DbType.String, BizActionObj.FemaleLabDay0.OocytePreparationMedia);
                dbServer.AddInParameter(command, "SpermPreparationMedia", DbType.String, BizActionObj.FemaleLabDay0.SpermPreparationMedia);
                dbServer.AddInParameter(command, "FinalLayering", DbType.String, BizActionObj.FemaleLabDay0.FinalLayering);
                dbServer.AddInParameter(command, "Matured", DbType.Int32, BizActionObj.FemaleLabDay0.Matured);
                dbServer.AddInParameter(command, "Immatured", DbType.Int32, BizActionObj.FemaleLabDay0.Immatured);
                dbServer.AddInParameter(command, "PostMatured", DbType.Int32, BizActionObj.FemaleLabDay0.PostMatured);
                dbServer.AddInParameter(command, "Total", DbType.Int32, BizActionObj.FemaleLabDay0.Total);
                dbServer.AddInParameter(command, "DoneBy", DbType.String, BizActionObj.FemaleLabDay0.DoneBy);
                dbServer.AddInParameter(command, "FollicularFluid", DbType.String, BizActionObj.FemaleLabDay0.FollicularFluid);
                dbServer.AddInParameter(command, "OPSTypeID", DbType.Int64, BizActionObj.FemaleLabDay0.OPSTypeID);
                dbServer.AddInParameter(command, "IVFCompleteionTime", DbType.DateTime, BizActionObj.FemaleLabDay0.IVFCompletionTime);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.FemaleLabDay0.IsFreezed);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                if (BizActionObj.SuccessStatus == -1) throw new Exception();

                if (BizActionObj.IsUpdate == false)
                {
                    BizActionObj.FemaleLabDay0.ID = (long)dbServer.GetParameterValue(command, "ID");
                }
                else
                {
                    #region Delete Previous Records (IVF+ICSI+Media+FU)
                    DbCommand Sqlcommand = dbServer.GetSqlStringCommand("Delete from T_IVF_FemaleLabDay0Details where UnitID=" + BizActionObj.FemaleLabDay0.UnitID + " AND OocyteID =" + BizActionObj.FemaleLabDay0.ID);
                    int sqlStatus = dbServer.ExecuteNonQuery(Sqlcommand, trans);

                    Sqlcommand = dbServer.GetSqlStringCommand("Delete from T_IVF_LabDayUploadedFiles where UnitID=" + BizActionObj.FemaleLabDay0.UnitID + " AND OocyteID =" + BizActionObj.FemaleLabDay0.ID + " AND LabDay=" + IVFLabDay.Day0);
                    sqlStatus = dbServer.ExecuteNonQuery(Sqlcommand, trans);

                    Sqlcommand = dbServer.GetSqlStringCommand("Delete from T_IVF_LabDayMediaDetails where UnitID=" + BizActionObj.FemaleLabDay0.UnitID + " AND OocyteID =" + BizActionObj.FemaleLabDay0.ID + " AND LabDay=" + IVFLabDay.Day0);
                    sqlStatus = dbServer.ExecuteNonQuery(Sqlcommand, trans);
                    #endregion
                }

                #region FU
                if (BizActionObj.FemaleLabDay0.FUSetting != null && BizActionObj.FemaleLabDay0.FUSetting.Count > 0)
                {
                    for (int i = 0; i < BizActionObj.FemaleLabDay0.FUSetting.Count; i++)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "OocyteID", DbType.Int64, BizActionObj.FemaleLabDay0.ID);
                        dbServer.AddInParameter(command1, "LabDay", DbType.Int16, IVFLabDay.Day0);
                        dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.FemaleLabDay0.FUSetting[i].FileName);
                        dbServer.AddInParameter(command1, "FileIndex", DbType.Int32, BizActionObj.FemaleLabDay0.FUSetting[i].Index);
                        dbServer.AddInParameter(command1, "Value", DbType.Binary, BizActionObj.FemaleLabDay0.FUSetting[i].Data);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.FemaleLabDay0.FUSetting[i].ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        BizActionObj.FemaleLabDay0.FUSetting[i].ID = (long)dbServer.GetParameterValue(command1, "ID");
                        if (BizActionObj.SuccessStatus == -1) throw new Exception();
                    }
                }
                #endregion

                #region IVF+Media
                if (BizActionObj.FemaleLabDay0.IVFSetting != null && BizActionObj.FemaleLabDay0.IVFSetting.Count > 0)
                {
                    for (int i = 0; i < BizActionObj.FemaleLabDay0.IVFSetting.Count; i++)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_IVF_AddFemaleLabDay0Details");

                        //dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                        //if (objDetailsVO.LinkServer != null)
                        //    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));                        

                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "OocyteID", DbType.Int64, BizActionObj.FemaleLabDay0.ID);

                        dbServer.AddInParameter(command1, "OocyteNO", DbType.Int64, BizActionObj.FemaleLabDay0.IVFSetting[i].OocyteNO);
                        dbServer.AddInParameter(command1, "CumulusID", DbType.Int64, BizActionObj.FemaleLabDay0.IVFSetting[i].Cumulus.ID);
                        dbServer.AddInParameter(command1, "GradeID", DbType.Int64, BizActionObj.FemaleLabDay0.IVFSetting[i].Grade.ID);
                        dbServer.AddInParameter(command1, "MOIID", DbType.Int64, BizActionObj.FemaleLabDay0.IVFSetting[i].MOI.ID);
                        dbServer.AddInParameter(command1, "Score", DbType.Int32, BizActionObj.FemaleLabDay0.IVFSetting[i].Score);
                        dbServer.AddInParameter(command1, "ProceedToDay1", DbType.Int64, BizActionObj.FemaleLabDay0.IVFSetting[i].ProceedToDay);
                        dbServer.AddInParameter(command1, "PlanID", DbType.Int64, BizActionObj.FemaleLabDay0.IVFSetting[i].Plan.ID);
                        dbServer.AddInParameter(command1, "MBD", DbType.String, null);
                        dbServer.AddInParameter(command1, "DOSID", DbType.Int64, null);
                        dbServer.AddInParameter(command1, "Comment", DbType.String, null);
                        dbServer.AddInParameter(command1, "PICID", DbType.Int64, null);
                        dbServer.AddInParameter(command1, "IC", DbType.String, null);

                        dbServer.AddInParameter(command1, "PlanTreatmentID", DbType.Int32, 1);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.FemaleLabDay0.IVFSetting[i].ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        BizActionObj.FemaleLabDay0.IVFSetting[i].ID = (long)dbServer.GetParameterValue(command1, "ID");
                        if (BizActionObj.SuccessStatus == -1) throw new Exception();

                        if (BizActionObj.FemaleLabDay0.IVFSetting[i].MediaDetails != null && BizActionObj.FemaleLabDay0.IVFSetting[i].MediaDetails.Count > 0)
                        {
                            foreach (var item3 in BizActionObj.FemaleLabDay0.IVFSetting[i].MediaDetails)
                            {
                                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");

                                dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, BizActionObj.FemaleLabDay0.ID);
                                dbServer.AddInParameter(command4, "DetailID", DbType.Int64, BizActionObj.FemaleLabDay0.IVFSetting[i].ID);
                                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command4, "Date", DbType.DateTime, DateTime.Now);

                                dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day0);
                                dbServer.AddInParameter(command4, "MediaName", DbType.String, item3.ItemName);
                                dbServer.AddInParameter(command4, "Company", DbType.String, item3.Company);
                                dbServer.AddInParameter(command4, "LotNo", DbType.String, item3.BatchCode);
                                dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, item3.ExpiryDate);
                                dbServer.AddInParameter(command4, "PH", DbType.Boolean, item3.PH);
                                dbServer.AddInParameter(command4, "OSM", DbType.Boolean, item3.OSM);
                                dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, item3.VolumeUsed);
                                dbServer.AddInParameter(command4, "Status", DbType.Int64, item3.SelectedStatus.ID);

                                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatus2 = dbServer.ExecuteNonQuery(command4, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                item3.ID = (long)dbServer.GetParameterValue(command4, "ID");
                                if (BizActionObj.SuccessStatus == -1) throw new Exception();
                            }
                        }
                    }
                }
                #endregion

                #region ICSI+Media
                if (BizActionObj.FemaleLabDay0.ICSISetting != null && BizActionObj.FemaleLabDay0.ICSISetting.Count > 0)
                {
                    for (int i = 0; i < BizActionObj.FemaleLabDay0.ICSISetting.Count; i++)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_IVF_AddFemaleLabDay0Details");

                        //dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                        //if (objDetailsVO.LinkServer != null)
                        //    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));                        

                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "OocyteID", DbType.Int64, BizActionObj.FemaleLabDay0.ID);

                        dbServer.AddInParameter(command1, "OocyteNO", DbType.Int64, BizActionObj.FemaleLabDay0.ICSISetting[i].OocyteNO);
                        dbServer.AddInParameter(command1, "CumulusID", DbType.Int64, null);
                        dbServer.AddInParameter(command1, "GradeID", DbType.Int64, null);
                        dbServer.AddInParameter(command1, "MOIID", DbType.Int64, null);
                        dbServer.AddInParameter(command1, "Score", DbType.Int32, null);
                        dbServer.AddInParameter(command1, "ProceedToDay1", DbType.Int64, BizActionObj.FemaleLabDay0.ICSISetting[i].ProceedToDay);
                        dbServer.AddInParameter(command1, "PlanID", DbType.Int64, BizActionObj.FemaleLabDay0.ICSISetting[i].Plan.ID);
                        dbServer.AddInParameter(command1, "MBD", DbType.String, BizActionObj.FemaleLabDay0.ICSISetting[i].MBD);
                        dbServer.AddInParameter(command1, "DOSID", DbType.Int64, BizActionObj.FemaleLabDay0.ICSISetting[i].DOS.ID);
                        dbServer.AddInParameter(command1, "Comment", DbType.String, BizActionObj.FemaleLabDay0.ICSISetting[i].Comment);
                        dbServer.AddInParameter(command1, "PICID", DbType.Int64, BizActionObj.FemaleLabDay0.ICSISetting[i].PIC.ID);
                        dbServer.AddInParameter(command1, "IC", DbType.String, BizActionObj.FemaleLabDay0.ICSISetting[i].IC);

                        dbServer.AddInParameter(command1, "PlanTreatmentID", DbType.Int32, 2);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.FemaleLabDay0.ICSISetting[i].ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        BizActionObj.FemaleLabDay0.ICSISetting[i].ID = (long)dbServer.GetParameterValue(command1, "ID");
                        if (BizActionObj.SuccessStatus == -1) throw new Exception();

                        if (BizActionObj.FemaleLabDay0.ICSISetting[i].MediaDetails != null && BizActionObj.FemaleLabDay0.ICSISetting[i].MediaDetails.Count > 0)
                        {
                            foreach (var item3 in BizActionObj.FemaleLabDay0.ICSISetting[i].MediaDetails)
                            {
                                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");

                                dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, BizActionObj.FemaleLabDay0.ID);
                                dbServer.AddInParameter(command4, "DetailID", DbType.Int64, BizActionObj.FemaleLabDay0.ICSISetting[i].ID);
                                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command4, "Date", DbType.DateTime, DateTime.Now);

                                dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day0);
                                dbServer.AddInParameter(command4, "MediaName", DbType.String, item3.ItemName);
                                dbServer.AddInParameter(command4, "Company", DbType.String, item3.Company);
                                dbServer.AddInParameter(command4, "LotNo", DbType.String, item3.BatchCode);
                                dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, item3.ExpiryDate);
                                dbServer.AddInParameter(command4, "PH", DbType.Boolean, item3.PH);
                                dbServer.AddInParameter(command4, "OSM", DbType.Boolean, item3.OSM);
                                dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, item3.VolumeUsed);
                                dbServer.AddInParameter(command4, "Status", DbType.Int64, item3.SelectedStatus.ID);

                                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatus2 = dbServer.ExecuteNonQuery(command4, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                item3.ID = (long)dbServer.GetParameterValue(command4, "ID");
                                if (BizActionObj.SuccessStatus == -1) throw new Exception();
                            }
                        }
                    }
                }
                #endregion

                #region LabDaySummary
                if (BizActionObj.FemaleLabDay0.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                    obj.LabDaysSummary = BizActionObj.FemaleLabDay0.LabDaySummary;
                    obj.LabDaysSummary.OocyteID = BizActionObj.FemaleLabDay0.ID;
                    obj.LabDaysSummary.CoupleID = BizActionObj.FemaleLabDay0.CoupleID;
                    obj.LabDaysSummary.CoupleUnitID = BizActionObj.FemaleLabDay0.CoupleUnitID;
                    obj.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay0;
                    //obj.LabDaysSummary.IsFreezed = BizActionObj.FemaleLabDay0.IsFreezed;
                    obj.LabDaysSummary.Priority = 1;
                    obj.LabDaysSummary.ProcDate = BizActionObj.FemaleLabDay0.ProcDate;
                    obj.LabDaysSummary.ProcTime = BizActionObj.FemaleLabDay0.ProcTime;
                    obj.LabDaysSummary.UnitID = BizActionObj.FemaleLabDay0.UnitID;

                    if (BizActionObj.IsUpdate == true)
                        obj.IsUpdate = true;
                    else
                        obj.IsUpdate = false;

                    obj = (clsAddUpdateLabDaysSummaryBizActionVO)objBaseDAL.AddUpdateLabDaysSummary(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.FemaleLabDay0.LabDaySummary.ID = obj.LabDaysSummary.ID;
                }
                #endregion

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.FemaleLabDay0 = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetFemaleLabDay0(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetFemaleLabDay0BizActionVO BizAction = (valueObject) as clsGetFemaleLabDay0BizActionVO;

            try
            {
                if (BizAction.FemaleLabDay0 == null)
                    BizAction.FemaleLabDay0 = new clsFemaleLabDay0VO();
                if (BizAction.FemaleLabDay0.IVFSetting == null)
                    BizAction.FemaleLabDay0.IVFSetting = new List<IVFTreatment>();
                if (BizAction.FemaleLabDay0.ICSISetting == null)
                    BizAction.FemaleLabDay0.ICSISetting = new List<ICSITreatment>();
                if (BizAction.FemaleLabDay0.FUSetting == null)
                    BizAction.FemaleLabDay0.FUSetting = new List<FileUpload>();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleLabDay0");

                dbServer.AddInParameter(command, "OocyteID", DbType.Int64, BizAction.OocyteID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizAction.CoupleUnitID);
                dbServer.AddInParameter(command, "LabDay", DbType.Int16, IVFLabDay.Day0);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Common Properties
                        BizAction.FemaleLabDay0.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.FemaleLabDay0.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.FemaleLabDay0.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        BizAction.FemaleLabDay0.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));

                        BizAction.FemaleLabDay0.ProcDate = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                        BizAction.FemaleLabDay0.ProcTime = (DateTime?)(DALHelper.HandleDate(reader["Time"]));
                        BizAction.FemaleLabDay0.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.FemaleLabDay0.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        BizAction.FemaleLabDay0.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        BizAction.FemaleLabDay0.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        BizAction.FemaleLabDay0.IVFCycleCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["IVFCycleCount"]));
                        BizAction.FemaleLabDay0.SrcOfNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcNeedleID"]));
                        BizAction.FemaleLabDay0.NeedleCompanyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["NeedleCompanyID"]));
                        BizAction.FemaleLabDay0.SrcOfOocyteID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcOocyteID"]));
                        BizAction.FemaleLabDay0.OocyteDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OocyteDonorID"]));

                        BizAction.FemaleLabDay0.SrcOfSemenID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcOfSemen"]));
                        BizAction.FemaleLabDay0.SemenDonorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SemenDonorID"]));
                        BizAction.FemaleLabDay0.TreatmentTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TreatmentPlanID"]));
                        BizAction.FemaleLabDay0.OocytePreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["OocytePreparationMedia"]));
                        BizAction.FemaleLabDay0.SpermPreparationMedia = Convert.ToString(DALHelper.HandleDBNull(reader["SpermPreparationMedia"]));
                        BizAction.FemaleLabDay0.FinalLayering = Convert.ToString(DALHelper.HandleDBNull(reader["FinalLayering"]));

                        BizAction.FemaleLabDay0.Matured = Convert.ToInt32(DALHelper.HandleDBNull(reader["Matured"]));
                        BizAction.FemaleLabDay0.Immatured = Convert.ToInt32(DALHelper.HandleDBNull(reader["Immatured"]));
                        BizAction.FemaleLabDay0.PostMatured = Convert.ToInt32(DALHelper.HandleDBNull(reader["PostMatured"]));
                        BizAction.FemaleLabDay0.Total = Convert.ToInt32(DALHelper.HandleDBNull(reader["Total"]));

                        BizAction.FemaleLabDay0.DoneBy = Convert.ToString(DALHelper.HandleDBNull(reader["DoneBy"]));
                        BizAction.FemaleLabDay0.FollicularFluid = Convert.ToString(DALHelper.HandleDBNull(reader["FollicularFluid"]));
                        BizAction.FemaleLabDay0.OPSTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OPSTypeID"]));

                        BizAction.FemaleLabDay0.IVFCompletionTime = (DateTime?)(DALHelper.HandleDate(reader["IVFCompleteionTime"]));
                        BizAction.FemaleLabDay0.ICSICompletionTime = (DateTime?)(DALHelper.HandleDate(reader["ICSICompleteionTime"]));
                        BizAction.FemaleLabDay0.SourceOfDenudingNeedle = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcDenudingNeedleID"]));
                        BizAction.FemaleLabDay0.FertilizationCheckTime = (DateTime?)(DALHelper.HandleDate(reader["FertilizationCheckTime"]));
                        BizAction.FemaleLabDay0.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizAction.FemaleLabDay0.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                    }
                }
                reader.NextResult();
                int CountIVF = 0;
                int CountICSI = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Common Properties

                        IVFTreatment objIVF = new IVFTreatment();
                        ICSITreatment objICSI = new ICSITreatment();

                        int i = Convert.ToInt32(DALHelper.HandleDBNull(reader["PlanTreatmentID"]));
                        if (i == 1)
                        {
                            objIVF.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            objIVF.Index = CountIVF;
                            objIVF.OocyteNO = Convert.ToInt32(DALHelper.HandleDBNull(reader["OocyteNO"]));
                            objIVF.Cumulus = new MasterListItem() { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"])) };
                            objIVF.Grade = new MasterListItem() { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"])) };
                            objIVF.MOI = new MasterListItem() { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"])) };
                            objIVF.Score = Convert.ToInt32(DALHelper.HandleDBNull(reader["Score"]));
                            objIVF.ProceedToDay = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ProceedToDay1"]));
                            objIVF.Plan = new MasterListItem() { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanID"])) };

                            //GetMediaDetails
                            clsBaseIVFLabDayDAL objBaseDAL = clsBaseIVFLabDayDAL.GetInstance();
                            clsGetAllDayMediaDetailsBizActionVO BizActionMedia = new clsGetAllDayMediaDetailsBizActionVO();
                            BizActionMedia.ID = BizAction.FemaleLabDay0.ID;
                            BizActionMedia.DetailID = objIVF.ID;
                            BizActionMedia.LabDay = 0;
                            BizActionMedia = (clsGetAllDayMediaDetailsBizActionVO)objBaseDAL.GetAllDayMediaDetails(BizActionMedia, UserVo);
                            objIVF.MediaDetails = BizActionMedia.MediaList;

                            BizAction.FemaleLabDay0.IVFSetting.Add(objIVF);
                            CountIVF++;
                        }
                        else if (i == 2)
                        {
                            objICSI.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            objICSI.Index = CountICSI;
                            objICSI.OocyteNO = Convert.ToInt32(DALHelper.HandleDBNull(reader["OocyteNO"]));
                            objICSI.DOS = new MasterListItem() { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DOSID"])) };
                            objICSI.PIC = new MasterListItem() { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PICID"])) };
                            objICSI.MBD = Convert.ToString(DALHelper.HandleDBNull(reader["MBD"]));
                            objICSI.Comment = Convert.ToString(DALHelper.HandleDBNull(reader["Comment"]));
                            objICSI.IC = Convert.ToString(DALHelper.HandleDBNull(reader["IC"]));
                            objICSI.ProceedToDay = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ProceedToDay1"]));
                            objICSI.Plan = new MasterListItem() { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanID"])) };

                            //GetMediaDetails
                            clsBaseIVFLabDayDAL objBaseDAL = clsBaseIVFLabDayDAL.GetInstance();
                            clsGetAllDayMediaDetailsBizActionVO BizActionMedia = new clsGetAllDayMediaDetailsBizActionVO();
                            BizActionMedia.ID = BizAction.FemaleLabDay0.ID;
                            BizActionMedia.DetailID = objICSI.ID;
                            BizActionMedia.LabDay = 0;
                            BizActionMedia = (clsGetAllDayMediaDetailsBizActionVO)objBaseDAL.GetAllDayMediaDetails(BizActionMedia, UserVo);
                            objICSI.MediaDetails = BizActionMedia.MediaList;

                            BizAction.FemaleLabDay0.ICSISetting.Add(objICSI);
                            CountICSI++;
                        }
                    }
                }
                reader.NextResult();
                int CountFU = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Common Properties
                        FileUpload objFU = new FileUpload();
                        objFU.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objFU.Index = CountFU;
                        objFU.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        objFU.Data = (byte[])(DALHelper.HandleDBNull(reader["Value"]));
                        BizAction.FemaleLabDay0.FUSetting.Add(objFU);
                        CountFU++;
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                BizAction.FemaleLabDay0 = null;
                throw;
            }
            finally
            {
            }
            return BizAction;
        }
        #endregion*/

        public override IValueObject AddUpdateEmbryoTransfer(IValueObject valueObject, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;

            clsAddUpdateEmbryoTransferBizActionVO BizActionObj = valueObject as clsAddUpdateEmbryoTransferBizActionVO;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                DbCommand command;
                if (BizActionObj.IsUpdate == false)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_IVF_AddEmbryoTransfer");
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.EmbryoTransfer.ID);

                    dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_IVF_UpdateEmbryoTransfer");
                    dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.EmbryoTransfer.ID);

                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, System.DateTime.Now);
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                }

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleId", DbType.Int64, BizActionObj.EmbryoTransfer.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitId", DbType.Int64, BizActionObj.EmbryoTransfer.CoupleUnitID);

                dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.EmbryoTransfer.ProcDate);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, BizActionObj.EmbryoTransfer.ProcTime);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, BizActionObj.EmbryoTransfer.EmbryologistID);
                dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, BizActionObj.EmbryoTransfer.AssEmbryologistID);
                dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, BizActionObj.EmbryoTransfer.AnesthetistID);
                dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, BizActionObj.EmbryoTransfer.AssAnesthetistID);

                dbServer.AddInParameter(command, "IsTreatmentUnderGA", DbType.Boolean, BizActionObj.EmbryoTransfer.IsTreatmentUnderGA);
                dbServer.AddInParameter(command, "CatheterTypeID", DbType.Int64, BizActionObj.EmbryoTransfer.CatheterTypeID);
                dbServer.AddInParameter(command, "Difficulty", DbType.Boolean, BizActionObj.EmbryoTransfer.IsDifficult);
                dbServer.AddInParameter(command, "DifficultyTypeID", DbType.Int64, BizActionObj.EmbryoTransfer.DifficultyTypeID);

                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.EmbryoTransfer.IsFreezed);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                if (BizActionObj.SuccessStatus == -1) throw new Exception();

                if (BizActionObj.IsUpdate == false)
                {
                    BizActionObj.EmbryoTransfer.ID = (long)dbServer.GetParameterValue(command, "ID");
                }
                else
                {
                    #region Delete Previous Records (Details+FU)
                    DbCommand Sqlcommand = dbServer.GetSqlStringCommand("Delete from T_IVF_EmbryoTransferDetails where UnitID=" + BizActionObj.EmbryoTransfer.UnitID + " AND ETID =" + BizActionObj.EmbryoTransfer.ID);
                    int sqlStatus = dbServer.ExecuteNonQuery(Sqlcommand, trans);

                    Sqlcommand = dbServer.GetSqlStringCommand("Delete from T_IVF_LabDayUploadedFiles where UnitID=" + BizActionObj.EmbryoTransfer.UnitID + " AND OocyteID =" + BizActionObj.EmbryoTransfer.ID + " AND LabDay=" + IVFLabDay.EmbryoTransfer);
                    sqlStatus = dbServer.ExecuteNonQuery(Sqlcommand, trans);

                    #endregion
                }

                #region FU
                if (BizActionObj.EmbryoTransfer.FUSetting != null && BizActionObj.EmbryoTransfer.FUSetting.Count > 0)
                {
                    for (int i = 0; i < BizActionObj.EmbryoTransfer.FUSetting.Count; i++)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "OocyteID", DbType.Int64, BizActionObj.EmbryoTransfer.ID);
                        dbServer.AddInParameter(command1, "LabDay", DbType.Int16, IVFLabDay.EmbryoTransfer);
                        dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.EmbryoTransfer.FUSetting[i].FileName);
                        dbServer.AddInParameter(command1, "FileIndex", DbType.Int32, BizActionObj.EmbryoTransfer.FUSetting[i].Index);
                        dbServer.AddInParameter(command1, "Value", DbType.Binary, BizActionObj.EmbryoTransfer.FUSetting[i].Data);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.EmbryoTransfer.FUSetting[i].ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        BizActionObj.EmbryoTransfer.FUSetting[i].ID = (long)dbServer.GetParameterValue(command1, "ID");
                        if (BizActionObj.SuccessStatus == -1) throw new Exception();
                    }
                }
                #endregion

                #region ETDetails
                if (BizActionObj.EmbryoTransfer.Details != null && BizActionObj.EmbryoTransfer.Details.Count > 0)
                {
                    for (int i = 0; i < BizActionObj.EmbryoTransfer.Details.Count; i++)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_IVF_AddEmbryoTransferDetails");

                        //dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                        //if (objDetailsVO.LinkServer != null)
                        //    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));                        

                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "TransferDate", DbType.DateTime, BizActionObj.EmbryoTransfer.Details[i].TransferDate);
                        dbServer.AddInParameter(command, "TransferDay", DbType.String, BizActionObj.EmbryoTransfer.Details[i].TransferDay);

                        dbServer.AddInParameter(command1, "ETID", DbType.Int64, BizActionObj.EmbryoTransfer.ID);
                        dbServer.AddInParameter(command1, "EmbNO", DbType.Int64, BizActionObj.EmbryoTransfer.Details[i].EmbryoNumber);                        
                        dbServer.AddInParameter(command1, "Grade", DbType.String, BizActionObj.EmbryoTransfer.Details[i].Grade);
                        dbServer.AddInParameter(command1, "Score", DbType.Int32, BizActionObj.EmbryoTransfer.Details[i].Score);
                        dbServer.AddInParameter(command1, "FertilizationStage", DbType.String, BizActionObj.EmbryoTransfer.Details[i].FertilizationStage);
                        dbServer.AddInParameter(command1, "EmbStatus", DbType.String, BizActionObj.EmbryoTransfer.Details[i].EmbryoStatus);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.EmbryoTransfer.Details[i].ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        BizActionObj.EmbryoTransfer.Details[i].ID = (long)dbServer.GetParameterValue(command1, "ID");
                        if (BizActionObj.SuccessStatus == -1) throw new Exception();
                        
                    }
                }
                #endregion

                

                #region LabDaySummary
                if (BizActionObj.EmbryoTransfer.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                    obj.LabDaysSummary = BizActionObj.EmbryoTransfer.LabDaySummary;
                    obj.LabDaysSummary.OocyteID = BizActionObj.EmbryoTransfer.ID;
                    obj.LabDaysSummary.CoupleID = BizActionObj.EmbryoTransfer.CoupleID;
                    obj.LabDaysSummary.CoupleUnitID = BizActionObj.EmbryoTransfer.CoupleUnitID;
                    obj.LabDaysSummary.FormID = IVFLabWorkForm.EmbryoTransfer;
                    //obj.LabDaysSummary.IsFreezed = BizActionObj.FemaleLabDay0.IsFreezed;
                    obj.LabDaysSummary.Priority = 1;
                    obj.LabDaysSummary.ProcDate = BizActionObj.EmbryoTransfer.ProcDate;
                    obj.LabDaysSummary.ProcTime = BizActionObj.EmbryoTransfer.ProcTime;
                    obj.LabDaysSummary.UnitID = BizActionObj.EmbryoTransfer.UnitID;

                    if (BizActionObj.IsUpdate == true)
                        obj.IsUpdate = true;
                    else
                        obj.IsUpdate = false;

                    obj = (clsAddUpdateLabDaysSummaryBizActionVO)objBaseDAL.AddUpdateLabDaysSummary(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.EmbryoTransfer.LabDaySummary.ID = obj.LabDaysSummary.ID;
                }
                #endregion

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.EmbryoTransfer = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return BizActionObj;
        }

        public override IValueObject GetEmbryoTransfer(IValueObject valueObject, clsUserVO UserVo)
        {
            bool CurrentMethodExecutionStatus = true;
            clsGetEmbryoTransferBizActionVO BizAction = (valueObject) as clsGetEmbryoTransferBizActionVO;

            try
            {
                if (BizAction.EmbryoTransfer == null)
                    BizAction.EmbryoTransfer = new clsEmbryoTransferVO();
                if (BizAction.EmbryoTransfer.Details == null)
                    BizAction.EmbryoTransfer.Details = new List<clsEmbryoTransferDetailsVO>();
                if (BizAction.EmbryoTransfer.FUSetting == null)
                    BizAction.EmbryoTransfer.FUSetting = new List<FileUpload>();

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetEmbryoTransfer");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizAction.CoupleUnitID);
                dbServer.AddInParameter(command, "LabDay", DbType.Int16, IVFLabDay.EmbryoTransfer);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Common Properties
                        BizAction.EmbryoTransfer.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.EmbryoTransfer.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.EmbryoTransfer.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        BizAction.EmbryoTransfer.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));

                        BizAction.EmbryoTransfer.ProcDate = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                        BizAction.EmbryoTransfer.ProcTime = (DateTime?)(DALHelper.HandleDate(reader["Time"]));
                        BizAction.EmbryoTransfer.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.EmbryoTransfer.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        BizAction.EmbryoTransfer.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        BizAction.EmbryoTransfer.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        //BizAction.EmbryoTransfer.IVFCycleCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["IVFCycleCount"]));
                        
                        BizAction.EmbryoTransfer.IsTreatmentUnderGA = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsTreatmentUnderGA"]));
                        BizAction.EmbryoTransfer.CatheterTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CatheterTypeID"]));
                        BizAction.EmbryoTransfer.IsDifficult = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Difficulty"]));
                        BizAction.EmbryoTransfer.DifficultyTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DifficultyTypeID"]));
                        BizAction.EmbryoTransfer.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizAction.EmbryoTransfer.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                    }
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Common Properties
                        clsEmbryoTransferDetailsVO objETD = new clsEmbryoTransferDetailsVO();

                        objETD.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objETD.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objETD.EmbryoNumber = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbNO"]));
                        objETD.TransferDate = (DateTime?)(DALHelper.HandleDBNull(reader["TransferDate"]));
                        objETD.Day = Convert.ToString(DALHelper.HandleDBNull(reader["TransferDay"]));

                        objETD.Grade = Convert.ToString(DALHelper.HandleDBNull(reader["Grade"]));
                        objETD.Score = (int)DALHelper.HandleDBNull(reader["Score"]);
                        objETD.FertilizationStage = (string)DALHelper.HandleDBNull(reader["FertilizationStage"]);
                        objETD.EmbryoStatus = (string)DALHelper.HandleDBNull(reader["EmbStatus"]);

                        BizAction.EmbryoTransfer.Details.Add(objETD);
                    }
                }
                reader.NextResult();
                int CountFU = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Common Properties
                        FileUpload objFU = new FileUpload();
                        objFU.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objFU.Index = CountFU;
                        objFU.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        objFU.Data = (byte[])(DALHelper.HandleDBNull(reader["Value"]));
                        BizAction.EmbryoTransfer.FUSetting.Add(objFU);
                        CountFU++;
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                BizAction.EmbryoTransfer = null;
                throw;
            }
            finally
            {
            }
            return BizAction;
        }

        public override IValueObject AddForwardedEmbryos(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsAddForwardedEmbryoTransferBizActionVO BizActionObj = valueObject as clsAddForwardedEmbryoTransferBizActionVO;
            BizActionObj = AddUpdateDetails(BizActionObj, UserVo, pConnection, pTransaction);
            return valueObject;
        }

        private clsAddForwardedEmbryoTransferBizActionVO AddUpdateDetails(clsAddForwardedEmbryoTransferBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            DbConnection con = null;
            DbTransaction trans = null;

            try
            {
                if (pConnection != null) con = pConnection;
                else con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed) con.Open();

                if (pTransaction != null) trans = pTransaction;
                else trans = con.BeginTransaction();

                if (BizActionObj.ForwardedEmbryos != null && BizActionObj.ForwardedEmbryos.Count > 0)
                {
                    for (int i = 0; i < BizActionObj.ForwardedEmbryos.Count; i++)
                    {
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_IVF_AddEmbryoTransferDetails");

                        //dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                        //if (objDetailsVO.LinkServer != null)
                        //    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));                        

                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, BizActionObj.UnitID);
                        dbServer.AddInParameter(command1, "CoupleId", DbType.Int64, BizActionObj.CoupleID);
                        dbServer.AddInParameter(command1, "CoupleUnitId", DbType.Int64, BizActionObj.CoupleUnitID);

                        dbServer.AddInParameter(command1, "Date", DbType.DateTime, BizActionObj.ForwardedEmbryos[i].TransferDate);
                        dbServer.AddInParameter(command1, "FormID", DbType.Int64, BizActionObj.ForwardedEmbryos[i].TransferDay);

                        dbServer.AddInParameter(command1, "FormRecID", DbType.Int64, BizActionObj.ForwardedEmbryos[i].RecID);
                        dbServer.AddInParameter(command1, "EmbNO", DbType.Int64, BizActionObj.ForwardedEmbryos[i].EmbryoNumber);
                        dbServer.AddInParameter(command1, "GradeID", DbType.Int64, BizActionObj.ForwardedEmbryos[i].GradeID);
                        dbServer.AddInParameter(command1, "Score", DbType.Int32, BizActionObj.ForwardedEmbryos[i].Score);
                        dbServer.AddInParameter(command1, "FertilizationStageID", DbType.Int64, BizActionObj.ForwardedEmbryos[i].FertilizationStageID);
                        dbServer.AddInParameter(command1, "EmbStatus", DbType.String, BizActionObj.ForwardedEmbryos[i].EmbryoStatus);
                        dbServer.AddInParameter(command1, "IsUsed", DbType.Boolean, false);

                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.ForwardedEmbryos[i].ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        BizActionObj.ForwardedEmbryos[i].ID = (long)dbServer.GetParameterValue(command1, "ID");
                        if (BizActionObj.SuccessStatus == -1) throw new Exception();

                        BizActionObj.SuccessStatus = 0;
                        if (pConnection == null) trans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null) trans.Rollback();
                //throw;
            }
            finally
            {
                if (pConnection == null)
                {
                    con.Close();
                    con = null;
                    trans = null;
                }
            }
            return BizActionObj;
        }

        public override IValueObject GetForwardedEmbryos(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetForwardedEmbryoTransferBizActionVO BizActionObj = valueObject as clsGetForwardedEmbryoTransferBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetForwardedEmbryos");
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.CoupleUnitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                List<long> Embs = new List<long>();

                if (reader.HasRows)
                {
                    if (BizActionObj.EmbryoTransfer == null)
                        BizActionObj.EmbryoTransfer = new List<clsEmbryoTransferDetailsVO>();
                    while (reader.Read())
                    {
                        if (Embs.Contains((long)DALHelper.HandleDBNull(reader["EmbNO"])))
                        {
                            continue;
                        }
                        else
                        {
                            clsEmbryoTransferDetailsVO Obj = new clsEmbryoTransferDetailsVO();

                            Obj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            Obj.UnitID = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                            Obj.TransferDate = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                            Obj.TransferDay = GetIVFenumValue(Convert.ToInt16(DALHelper.HandleDBNull(reader["FormID"])));
                            Obj.Day = Obj.TransferDay.ToString();
                            Obj.EmbryoNumber = (long)DALHelper.HandleDBNull(reader["EmbNO"]);

                            Obj.GradeID = (long)DALHelper.HandleDBNull(reader["GradeID"]);
                            Obj.FertilizationStageID = (long)DALHelper.HandleDBNull(reader["FertilizationStageID"]);

                            Obj.Grade = (string)DALHelper.HandleDBNull(reader["Grade"]);
                            Obj.Score = (int)DALHelper.HandleDBNull(reader["Score"]);
                            Obj.FertilizationStage = (string)DALHelper.HandleDBNull(reader["FertilizationStage"]);

                            BizActionObj.EmbryoTransfer.Add(Obj);

                            Embs.Add(Obj.EmbryoNumber);
                        }
                    }

                    if (BizActionObj.EmbryoTransfer.Count > 1)
                    {
                        BizActionObj.EmbryoTransfer.Reverse();
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

        public IVFLabDay GetIVFenumValue(Int16 ob)
        {
            IVFLabDay ob1 = new IVFLabDay();
            switch (ob)
            {
                case 0:
                    ob1 = IVFLabDay.Day0;
                    break;
                case 1:
                    ob1 = IVFLabDay.Day1;
                    break;
                case 2:
                    ob1 = IVFLabDay.Day2;
                    break;
                case 3:
                    ob1 = IVFLabDay.Day3;
                    break;
                case 4:
                    ob1 = IVFLabDay.Day4;
                    break;
                case 5:
                    ob1 = IVFLabDay.Day5;
                    break;
                case 6:
                    ob1 = IVFLabDay.Day6;
                    break;
            }
            return ob1;
        }
    }
}