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
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    class clsIVFLabDayDAL : clsBaseIVFLabDayDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsIVFLabDayDAL()
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

       

        #region FemaleLabDay0 (Done By : Harish)
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
                dbServer.AddInParameter(command, "OocyteDonorID", DbType.String, BizActionObj.FemaleLabDay0.OocyteDonorID);
                //By Anjali
               // dbServer.AddInParameter(command, "OocyteDonorCode", DbType.String, BizActionObj.FemaleLabDay0.OocyteDonorCode);

                dbServer.AddInParameter(command, "SrcOfSemen", DbType.Int64, BizActionObj.FemaleLabDay0.SrcOfSemenID);
                dbServer.AddInParameter(command, "SemenDonorID", DbType.String, BizActionObj.FemaleLabDay0.SemenDonorID);
                //By Anjali
              //  dbServer.AddInParameter(command, "SemenDonorCode", DbType.String, BizActionObj.FemaleLabDay0.SemenDonorCode);

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
                    DbCommand Sqlcommand = dbServer.GetSqlStringCommand("Delete from T_IVF_FemaleLabDay0Details where UnitID=" + BizActionObj.FemaleLabDay0.UnitID + " AND OocyteID =" + BizActionObj.FemaleLabDay0.ID );
                    int sqlStatus = dbServer.ExecuteNonQuery(Sqlcommand, trans);

                    Sqlcommand = dbServer.GetSqlStringCommand("Delete from T_IVF_LabDayUploadedFiles where UnitID=" + BizActionObj.FemaleLabDay0.UnitID + " AND OocyteID =" + BizActionObj.FemaleLabDay0.ID+" AND LabDay=0" );
                    sqlStatus = dbServer.ExecuteNonQuery(Sqlcommand, trans);

                    Sqlcommand = dbServer.GetSqlStringCommand("Delete from T_IVF_LabDayMediaDetails where UnitID=" + BizActionObj.FemaleLabDay0.UnitID + " AND OocyteID =" + BizActionObj.FemaleLabDay0.ID + " AND LabDay=0" );
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
                if (BizActionObj.FemaleLabDay0.SemenDetails != null)
                {
                    DbCommand command12 = dbServer.GetStoredProcCommand("CIMS_DeleteLab1SemenDetails");

                    dbServer.AddInParameter(command12, "Day1ID", DbType.Int64, BizActionObj.FemaleLabDay0.ID);
                    dbServer.AddInParameter(command12, "LabDay", DbType.Int32, IVFLabDay.Day0);
                    int intStatus2 = dbServer.ExecuteNonQuery(command12);
                }

                //if (BizActionObj.Day1Details.SemenDetails != null)
                if (BizActionObj.FemaleLabDay0.SemenDetails != null)
                {
                    //var item4 = BizActionObj.Day1Details.SemenDetails;
                    var item4 = BizActionObj.FemaleLabDay0.SemenDetails;
                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");

                    //dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                    dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, BizActionObj.FemaleLabDay0.ID);
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day0);

                    //dbServer.AddInParameter(command4, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day1Details.SemenDetails.MethodOfSpermPreparation);
                    //dbServer.AddInParameter(command4, "SourceOfSemen", DbType.Int64, BizActionObj.Day1Details.SemenDetails.SourceOfSemen);
                    dbServer.AddInParameter(command4, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.FemaleLabDay0.SemenDetails.MethodOfSpermPreparation);
                    dbServer.AddInParameter(command4, "SourceOfSemen", DbType.Int64, BizActionObj.FemaleLabDay0.SemenDetails.SourceOfSemen);

                    dbServer.AddInParameter(command4, "PreSelfVolume", DbType.String, item4.PreSelfVolume);
                    dbServer.AddInParameter(command4, "PreSelfConcentration", DbType.String, item4.PreSelfConcentration);
                    dbServer.AddInParameter(command4, "PreSelfMotality", DbType.String, item4.PreSelfMotality);
                    dbServer.AddInParameter(command4, "PreSelfWBC", DbType.String, item4.PreSelfWBC);


                    dbServer.AddInParameter(command4, "PreDonorVolume", DbType.String, item4.PreDonorVolume);
                    dbServer.AddInParameter(command4, "PreDonorConcentration", DbType.String, item4.PreDonorConcentration);
                    dbServer.AddInParameter(command4, "PreDonorMotality", DbType.String, item4.PreDonorMotality);
                    dbServer.AddInParameter(command4, "PreDonorWBC", DbType.String, item4.PreDonorWBC);

                    dbServer.AddInParameter(command4, "PostSelfVolume", DbType.String, item4.PostSelfVolume);
                    dbServer.AddInParameter(command4, "PostSelfConcentration", DbType.String, item4.PostSelfConcentration);
                    dbServer.AddInParameter(command4, "PostSelfMotality", DbType.String, item4.PostSelfMotality);
                    dbServer.AddInParameter(command4, "PostSelfWBC", DbType.String, item4.PostSelfWBC);

                    dbServer.AddInParameter(command4, "PostDonorVolume ", DbType.String, item4.PostDonorVolume);
                    dbServer.AddInParameter(command4, "PostDonorConcentration", DbType.String, item4.PostDonorConcentration);
                    dbServer.AddInParameter(command4, "PostDonorMotality", DbType.String, item4.PostDonorMotality);
                    dbServer.AddInParameter(command4, "PostDonorWBC", DbType.String, item4.PostDonorWBC);

                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item4.ID);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    item4.ID = (long)dbServer.GetParameterValue(command4, "ID");


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

                        if(BizActionObj.FemaleLabDay0.IVFSetting[i].Cumulus !=null)
                            dbServer.AddInParameter(command1, "CumulusID", DbType.Int64, BizActionObj.FemaleLabDay0.IVFSetting[i].Cumulus.ID);
                        else
                            dbServer.AddInParameter(command1, "CumulusID", DbType.Int64, 0);
                        
                        if(BizActionObj.FemaleLabDay0.IVFSetting[i].Grade !=null)
                            dbServer.AddInParameter(command1, "GradeID", DbType.Int64, BizActionObj.FemaleLabDay0.IVFSetting[i].Grade.ID);
                        else
                            dbServer.AddInParameter(command1, "GradeID", DbType.Int64, 0);

                        if (BizActionObj.FemaleLabDay0.IVFSetting[i].MOI != null)
                            dbServer.AddInParameter(command1, "MOIID", DbType.Int64, BizActionObj.FemaleLabDay0.IVFSetting[i].MOI.ID);
                        else
                            dbServer.AddInParameter(command1, "MOIID", DbType.Int64, 0);

                        if (BizActionObj.FemaleLabDay0.IVFSetting[i].Score !=null)
                            dbServer.AddInParameter(command1, "Score", DbType.Int32, BizActionObj.FemaleLabDay0.IVFSetting[i].Score);
                        else
                            dbServer.AddInParameter(command1, "Score", DbType.Int32, 0);


                        //dbServer.AddInParameter(command1, "ProceedToDay1", DbType.Int64, BizActionObj.FemaleLabDay0.IVFSetting[i].ProceedToDay);
                         

                        if (BizActionObj.FemaleLabDay0.IVFSetting[i].Plan !=null)
                            dbServer.AddInParameter(command1, "PlanID", DbType.Int64, BizActionObj.FemaleLabDay0.IVFSetting[i].Plan.ID);
                        else
                            dbServer.AddInParameter(command1, "PlanID", DbType.Int64, 0);

                        if (BizActionObj.FemaleLabDay0.IVFSetting[i].Plan.ID == 3)
                        {
                            dbServer.AddInParameter(command1, "ProceedToDay1", DbType.Int64, true);
                        }
                        else
                        {
                            dbServer.AddInParameter(command1, "ProceedToDay1", DbType.Int64, false);
                        }
                        dbServer.AddInParameter(command1, "MBD", DbType.String, null);
                        dbServer.AddInParameter(command1, "DOSID", DbType.Int64, null);
                        dbServer.AddInParameter(command1, "Comment", DbType.String, null);
                        dbServer.AddInParameter(command1, "PICID", DbType.Int64, null);
                        dbServer.AddInParameter(command1, "IC", DbType.String, null);


                        dbServer.AddInParameter(command1, "PlanTreatmentID", DbType.Int32, 1);

                        dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.FemaleLabDay0.IVFSetting[i].FileName);
                        dbServer.AddInParameter(command1, "FileContents", DbType.Binary, BizActionObj.FemaleLabDay0.IVFSetting[i].FileContents);


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
                                dbServer.AddInParameter(command4, "BatchID", DbType.Int64, item3.BatchID);
                                dbServer.AddInParameter(command4, "StoreID", DbType.Int64, item3.StoreID);



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


                        dbServer.AddInParameter(command1, "FileName", DbType.String, BizActionObj.FemaleLabDay0.ICSISetting[i].FileName);
                        dbServer.AddInParameter(command1, "FileContents", DbType.Binary, BizActionObj.FemaleLabDay0.ICSISetting[i].FileContents);

                  
                        //dbServer.AddInParameter(command1, "ProceedToDay1", DbType.Int64, BizActionObj.FemaleLabDay0.ICSISetting[i].ProceedToDay);
                       
                        if (BizActionObj.FemaleLabDay0.ICSISetting[i].Plan != null)
                            dbServer.AddInParameter(command1, "PlanID", DbType.Int64, BizActionObj.FemaleLabDay0.ICSISetting[i].Plan.ID);
                        else
                            dbServer.AddInParameter(command1, "PlanID", DbType.Int64, 0);
                        if (BizActionObj.FemaleLabDay0.ICSISetting[i].Plan.ID == 3)
                        {
                            dbServer.AddInParameter(command1, "ProceedToDay1", DbType.Int64, true);
                        }
                        else
                        {
                            dbServer.AddInParameter(command1, "ProceedToDay1", DbType.Int64, false);
                        }

                        if (BizActionObj.FemaleLabDay0.ICSISetting[i].MBD != null)
                            dbServer.AddInParameter(command1, "MBD", DbType.String, BizActionObj.FemaleLabDay0.ICSISetting[i].MBD);
                        else
                            dbServer.AddInParameter(command1, "MBD", DbType.String, 0);

                        if (BizActionObj.FemaleLabDay0.ICSISetting[i].DOS != null)
                            dbServer.AddInParameter(command1, "DOSID", DbType.Int64, BizActionObj.FemaleLabDay0.ICSISetting[i].DOS.ID);
                        else
                            dbServer.AddInParameter(command1, "DOSID", DbType.Int64, 0);

                       
                        dbServer.AddInParameter(command1, "Comment", DbType.String, BizActionObj.FemaleLabDay0.ICSISetting[i].Comment);

                        if (BizActionObj.FemaleLabDay0.ICSISetting[i].PIC != null)
                            dbServer.AddInParameter(command1, "PICID", DbType.Int64, BizActionObj.FemaleLabDay0.ICSISetting[i].PIC.ID);
                        else
                            dbServer.AddInParameter(command1, "PICID", DbType.Int64, 0);

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
                                dbServer.AddInParameter(command4, "BatchID", DbType.Int64, item3.BatchID);
                                dbServer.AddInParameter(command4, "StoreID", DbType.Int64, item3.StoreID);

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
                if (BizActionObj.FemaleLabDay0.LabDaySummary !=null)
                {
                    clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                    obj.LabDaysSummary = BizActionObj.FemaleLabDay0.LabDaySummary;
                    obj.LabDaysSummary.OocyteID = BizActionObj.FemaleLabDay0.ID;
                    obj.LabDaysSummary.CoupleID = BizActionObj.FemaleLabDay0.CoupleID;
                    obj.LabDaysSummary.CoupleUnitID = BizActionObj.FemaleLabDay0.CoupleUnitID;
                    obj.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay0;
                    obj.LabDaysSummary.IsFreezed = BizActionObj.FemaleLabDay0.IsFreezed;
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
                        BizAction.FemaleLabDay0.OocyteDonorID = Convert.ToString(DALHelper.HandleDBNull(reader["OocyteDonorID"]));

                        BizAction.FemaleLabDay0.SrcOfSemenID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrcOfSemen"]));
                        BizAction.FemaleLabDay0.SemenDonorID = Convert.ToString(DALHelper.HandleDBNull(reader["SemenDonorID"]));
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
                        ICSITreatment objICSI =new ICSITreatment();

                        int i = Convert.ToInt32(DALHelper.HandleDBNull(reader["PlanTreatmentID"]));
                        if (i == 1)
                        {
                            objIVF.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                            objIVF.Index = CountIVF;
                            //By Anjali.............
                            objIVF.SerialOccyteNo = Convert.ToInt32(DALHelper.HandleDBNull(reader["SerialOccyteNo"]));
                           // ......................
                            objIVF.OocyteNO = Convert.ToInt32(DALHelper.HandleDBNull(reader["OocyteNO"]));
                            objIVF.Cumulus = new MasterListItem() { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CumulusID"])) };
                            objIVF.Grade = new MasterListItem() { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GradeID"])) };
                            objIVF.MOI = new MasterListItem() { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MOIID"])) };
                            objIVF.Score = Convert.ToInt32(DALHelper.HandleDBNull(reader["Score"]));
                            objIVF.ProceedToDay = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ProceedToDay1"]));
                            objIVF.Plan = new MasterListItem() { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PlanID"])) };
                            objIVF.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                            objIVF.FileContents = (byte[])(DALHelper.HandleDBNull(reader["FileContents"]));

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
                            objICSI.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                            objICSI.FileContents = (byte[])(DALHelper.HandleDBNull(reader["FileContents"]));


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
                reader.NextResult();
                if (BizAction.FemaleLabDay0.SemenDetails == null)
                    BizAction.FemaleLabDay0.SemenDetails = new clsFemaleSemenDetailsVO();


                while (reader.Read())
                {
                    BizAction.FemaleLabDay0.SemenDetails.MOSP = (string)(DALHelper.HandleDBNull(reader["MOSP"]));
                    BizAction.FemaleLabDay0.SemenDetails.SOS = (string)(DALHelper.HandleDBNull(reader["SOS"]));

                    BizAction.FemaleLabDay0.SemenDetails.MethodOfSpermPreparation = (long)(DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]));
                    BizAction.FemaleLabDay0.SemenDetails.SourceOfSemen = (long)(DALHelper.HandleDBNull(reader["SourceOfSemen"]));

                    BizAction.FemaleLabDay0.SemenDetails.PreSelfVolume = (string)(DALHelper.HandleDBNull(reader["PreSelfVolume"]));
                    BizAction.FemaleLabDay0.SemenDetails.PreSelfConcentration = (string)(DALHelper.HandleDBNull(reader["PreSelfConcentration"]));
                    BizAction.FemaleLabDay0.SemenDetails.PreSelfMotality = (string)(DALHelper.HandleDBNull(reader["PreSelfMotality"]));
                    BizAction.FemaleLabDay0.SemenDetails.PreSelfWBC = (string)(DALHelper.HandleDBNull(reader["PreSelfWBC"]));

                    BizAction.FemaleLabDay0.SemenDetails.PreDonorVolume = (string)(DALHelper.HandleDBNull(reader["PreDonorVolume"]));
                    BizAction.FemaleLabDay0.SemenDetails.PreDonorConcentration = (string)(DALHelper.HandleDBNull(reader["PreDonorConcentration"]));
                    BizAction.FemaleLabDay0.SemenDetails.PreDonorMotality = (string)(DALHelper.HandleDBNull(reader["PreDonorMotality"]));
                    BizAction.FemaleLabDay0.SemenDetails.PreDonorWBC = (string)(DALHelper.HandleDBNull(reader["PreDonorWBC"]));


                    BizAction.FemaleLabDay0.SemenDetails.PostSelfVolume = (string)(DALHelper.HandleDBNull(reader["PostSelfVolume"]));
                    BizAction.FemaleLabDay0.SemenDetails.PostSelfConcentration = (string)(DALHelper.HandleDBNull(reader["PostSelfConcentration"]));
                    BizAction.FemaleLabDay0.SemenDetails.PostSelfMotality = (string)(DALHelper.HandleDBNull(reader["PostSelfMotality"]));
                    BizAction.FemaleLabDay0.SemenDetails.PostSelfWBC = (string)(DALHelper.HandleDBNull(reader["PostSelfWBC"]));

                    BizAction.FemaleLabDay0.SemenDetails.PostDonorVolume = (string)(DALHelper.HandleDBNull(reader["PostDonorVolume"]));
                    BizAction.FemaleLabDay0.SemenDetails.PostDonorConcentration = (string)(DALHelper.HandleDBNull(reader["PostDonorConcentration"]));
                    BizAction.FemaleLabDay0.SemenDetails.PostDonorMotality = (string)(DALHelper.HandleDBNull(reader["PostDonorMotality"]));
                    BizAction.FemaleLabDay0.SemenDetails.PostDonorWBC = (string)(DALHelper.HandleDBNull(reader["PostDonorWBC"]));
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
        #endregion

        #region FemaleLabDay1 :Created by Priyanka

        public override IValueObject AddLabDay1(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddLabDay1BizActionVO BizActionObj = valueObject as clsAddLabDay1BizActionVO;

            if (BizActionObj.Day1Details.ID == 0)
                BizActionObj = AddDay1(BizActionObj, UserVo);
            else
                BizActionObj = UpdateDay1(BizActionObj, UserVo);

            return valueObject;



           



        }

        private clsAddLabDay1BizActionVO AddDay1(clsAddLabDay1BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsFemaleLabDay1VO ObjDay1VO = BizActionObj.Day1Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay1");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, ObjDay1VO.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, ObjDay1VO.CoupleUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjDay1VO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, ObjDay1VO.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, ObjDay1VO.EmbryologistID);
                dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, ObjDay1VO.AssEmbryologistID);
                dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, ObjDay1VO.AnesthetistID);
                dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, ObjDay1VO.AssAnesthetistID);
                dbServer.AddInParameter(command, "IVFCycleCount", DbType.Int64, ObjDay1VO.IVFCycleCount);
                dbServer.AddInParameter(command, "SourceNeedleID", DbType.Int64, ObjDay1VO.SourceNeedleID);
                dbServer.AddInParameter(command, "InfectionObserved", DbType.String, ObjDay1VO.InfectionObserved);
                dbServer.AddInParameter(command, "TotNoOfOocytes", DbType.Int32, ObjDay1VO.TotNoOfOocytes);
                dbServer.AddInParameter(command, "TotNoOf2PN", DbType.Int32, ObjDay1VO.TotNoOf2PN);
                dbServer.AddInParameter(command, "TotNoOf3PN", DbType.Int32, ObjDay1VO.TotNoOf3PN);
                dbServer.AddInParameter(command, "TotNoOf2PB", DbType.Int32, ObjDay1VO.TotNoOf2PB);
                dbServer.AddInParameter(command, "ToNoOfMI", DbType.Int32, ObjDay1VO.ToNoOfMI);
                dbServer.AddInParameter(command, "ToNoOfMII", DbType.Int32, ObjDay1VO.ToNoOfMII);
                dbServer.AddInParameter(command, "ToNoOfGV", DbType.Int32, ObjDay1VO.ToNoOfGV);
                dbServer.AddInParameter(command, "ToNoOfDeGenerated", DbType.Int32, ObjDay1VO.ToNoOfDeGenerated);
                dbServer.AddInParameter(command, "ToNoOfLost ", DbType.Int32, ObjDay1VO.ToNoOfLost);
                dbServer.AddInParameter(command, "IsFreezed ", DbType.Boolean, ObjDay1VO.IsFreezed);

                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                //--- To Link LabDay1 with PlanTherapy.PlanTherapyId fetch from LabDay0
                long PlanTherapyId = 0;

                foreach (var item11 in ObjDay1VO.FertilizationAssesmentDetails)
                {
                    PlanTherapyId = item11.PlanTherapyId;
                    break;
                }
                 
                    dbServer.AddInParameter(command, "PlanTherapyId", DbType.Int64, PlanTherapyId);
                //--- To Link LabDay1 with PlanTherapy.PlanTherapyId fetch from LabDay0 ----------

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDay1VO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Day1Details.ID = (long)dbServer.GetParameterValue(command, "ID");



                if (BizActionObj.Day1Details.ObservationDetails != null && BizActionObj.Day1Details.ObservationDetails.Count > 0)
                {
                    foreach (var item in ObjDay1VO.ObservationDetails)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay1ObservationDetails");

                        dbServer.AddInParameter(command1, "Day1ID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command1, "Time", DbType.DateTime, item.Time);
                        dbServer.AddInParameter(command1, "HrAtIns", DbType.Double, item.HrAtIns);
                        dbServer.AddInParameter(command1, "Observation", DbType.String, item.Observation);
                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);
                        dbServer.AddInParameter(command1, "FertiCheckPeriod", DbType.String, item.FertiCheckPeriod); 
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

                        int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                        item.ID = (long)dbServer.GetParameterValue(command1, "ID");
                    }
                }


                if (BizActionObj.Day1Details.FertilizationAssesmentDetails != null && BizActionObj.Day1Details.FertilizationAssesmentDetails.Count > 0)
                {
                    List<clsEmbryoTransferDetailsVO> ETForwardedDetails = new List<clsEmbryoTransferDetailsVO>();
                    foreach (var item1 in ObjDay1VO.FertilizationAssesmentDetails)
                    {

                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay1Details");

                        dbServer.AddInParameter(command2, "Day1ID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "OoNo", DbType.Int64, item1.OoNo);
                        //By Anjali..........
                        dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, item1.SerialOccyteNo);
                        //...................
                        dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, item1.PlanTreatmentID);
                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, item1.Date);
                        dbServer.AddInParameter(command2, "Time", DbType.DateTime, item1.Time);
                        dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, item1.SrcOocyteID);
                        dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, item1.OocyteDonorID);
                        dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, item1.SrcOfSemen);
                        dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, item1.SemenDonorID);
                        
                        if (item1.SelectedFePlan != null)
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, item1.SelectedFePlan.ID);
                        else
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);

                        if (item1.SelectedGrade != null)
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, item1.SelectedGrade.ID);
                        else
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);

                        dbServer.AddInParameter(command2, "Score", DbType.Int64, item1.Score);
                        dbServer.AddInParameter(command2, "PV", DbType.Boolean, item1.PV);
                        dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, item1.XFactor);
                        dbServer.AddInParameter(command2, "Others", DbType.String, item1.Others);
                        //dbServer.AddInParameter(command2, "ProceedDay2", DbType.Boolean, item1.ProceedDay2);

                        dbServer.AddInParameter(command2, "FileName", DbType.String, item1.FileName);
                        dbServer.AddInParameter(command2, "FileContents", DbType.Binary,item1.FileContents);


                        if (item1.SelectedPlan != null)
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, item1.SelectedPlan.ID);
                        else
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);

                        if (item1.SelectedPlan.ID == 3)
                        {
                            dbServer.AddInParameter(command2, "ProceedDay2", DbType.Boolean, true);
                        }
                        else
                        {
                            dbServer.AddInParameter(command2, "ProceedDay2", DbType.Boolean, false);
                        }
                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);

                        

                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);

                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                        item1.ID = (long)dbServer.GetParameterValue(command2, "ID");

                        


                        //if (ObjDay1VO.IsFreezed == true)
                        //{
                        //    if (item1.SelectedPlan != null && item1.SelectedGrade.ID == 3)
                        //    {                                
                        //        clsEmbryoTransferDetailsVO obj = new clsEmbryoTransferDetailsVO();
                        //        obj.TransferDate = item1.Date;
                        //        obj.TransferDay = IVFLabDay.Day1;
                        //        obj.RecID = item1.ID;
                        //        obj.EmbryoNumber = item1.OoNo;
                        //        if (item1.SelectedGrade!=null)
                        //            obj.GradeID = item1.SelectedGrade.ID;
                        //        else
                        //            obj.GradeID = 0;
                        //        obj.Score = Convert.ToInt32(item1.Score);
                        //        obj.FertilizationStageID = item1.FertilisationStage;                                
                                
                        //        ETForwardedDetails.Add(obj);
                        //    }
                        //}

                        if (item1.MediaDetails != null && item1.MediaDetails.Count > 0)
                        {
                            foreach (var item3 in item1.MediaDetails)
                            {


                                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");

                                dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                                dbServer.AddInParameter(command4, "DetailID", DbType.Int64, item1.ID);
                                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command4, "Date", DbType.DateTime, item3.Date);

                                dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day1);
                                dbServer.AddInParameter(command4, "MediaName", DbType.String, item3.ItemName);
                                dbServer.AddInParameter(command4, "Company", DbType.String, item3.Company);
                                dbServer.AddInParameter(command4, "LotNo", DbType.String, item3.BatchCode);
                                dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, item3.ExpiryDate);
                                dbServer.AddInParameter(command4, "PH", DbType.Boolean, item3.PH);
                                dbServer.AddInParameter(command4, "OSM", DbType.Boolean, item3.OSM);
                                dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, item3.VolumeUsed);
                                dbServer.AddInParameter(command4, "Status", DbType.Int64, item3.SelectedStatus.ID);
                                dbServer.AddInParameter(command4, "BatchID", DbType.Int64, item3.BatchID);
                                dbServer.AddInParameter(command4, "StoreID", DbType.Int64, item3.StoreID);

                                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                item3.ID = (long)dbServer.GetParameterValue(command4, "ID");
                            }
                        }
                        if (item1.CalculateDetails != null )
                        {
                            var item6 = item1.CalculateDetails;

                            DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay1CalculateDetails");

                            dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                            dbServer.AddInParameter(command5, "DetailID", DbType.Int64, item1.ID);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);

                            dbServer.AddInParameter(command5, "TwoPNClosed", DbType.Boolean, item6.TwoPNClosed);
                            dbServer.AddInParameter(command5, "TwoPNSeparated", DbType.Boolean, item6.TwoPNSeparated);
                            dbServer.AddInParameter(command5, "NucleoliAlign", DbType.Boolean, item6.NucleoliAlign);
                            dbServer.AddInParameter(command5, "BeginningAlign", DbType.Boolean, item6.BeginningAlign);
                            dbServer.AddInParameter(command5, "Scattered", DbType.Boolean, item6.Scattered);
                            dbServer.AddInParameter(command5, "CytoplasmHetero", DbType.Boolean, item6.CytoplasmHetero);
                            dbServer.AddInParameter(command5, "CytoplasmHomo", DbType.Boolean, item6.CytoplasmHomo);
                            dbServer.AddInParameter(command5, "NuclearMembrane", DbType.Boolean, item6.NuclearMembrane);


                            dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item6.ID);
                            dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus1 = dbServer.ExecuteNonQuery(command5, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command5, "ResultStatus");
                            item6.ID = (long)dbServer.GetParameterValue(command5, "ID");
                            
                            
                        }


                    }

                    //if (BizActionObj.Day1Details.IsFreezed==true  && ETForwardedDetails.Count > 0)
                    //{
                    //    clsBaseIVFEmbryoTransferDAL objBaseDAL = clsBaseIVFEmbryoTransferDAL.GetInstance();
                    //    clsAddForwardedEmbryoTransferBizActionVO BizActionFrwrd = new clsAddForwardedEmbryoTransferBizActionVO();
                    //    BizActionFrwrd.UnitID = BizActionObj.Day1Details.UnitID;
                    //    BizActionFrwrd.CoupleID = BizActionObj.Day1Details.CoupleID;
                    //    BizActionFrwrd.CoupleUnitID = BizActionObj.Day1Details.CoupleUnitID;
                    //    BizActionFrwrd.ForwardedEmbryos = ETForwardedDetails;

                    //    BizActionFrwrd = (clsAddForwardedEmbryoTransferBizActionVO)objBaseDAL.AddForwardedEmbryos(BizActionFrwrd, UserVo, con, trans);

                    //    if (BizActionFrwrd.SuccessStatus == -1) throw new Exception();
                    //}
                }

                if (BizActionObj.Day1Details.FUSetting != null && BizActionObj.Day1Details.FUSetting.Count > 0)
                {
                    foreach (var item2 in ObjDay1VO.FUSetting)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                        dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day1);
                        dbServer.AddInParameter(command3, "FileName", DbType.String, item2.FileName);
                        dbServer.AddInParameter(command3, "FileIndex", DbType.Int32, item2.Index);
                        dbServer.AddInParameter(command3, "Value", DbType.Binary, item2.Data);
                        dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);

                        dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                        dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                        item2.ID = (long)dbServer.GetParameterValue(command3, "ID");

                    }
                }

                if (BizActionObj.Day1Details.SemenDetails != null)
                {
                    var item4 = BizActionObj.Day1Details.SemenDetails;
                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");

                    dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day1);

                    dbServer.AddInParameter(command4, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day1Details.SemenDetails.MethodOfSpermPreparation);
                    dbServer.AddInParameter(command4, "SourceOfSemen", DbType.Int64, BizActionObj.Day1Details.SemenDetails.SourceOfSemen);

                    dbServer.AddInParameter(command4, "PreSelfVolume", DbType.String, item4.PreSelfVolume);
                    dbServer.AddInParameter(command4, "PreSelfConcentration", DbType.String, item4.PreSelfConcentration);
                    dbServer.AddInParameter(command4, "PreSelfMotality", DbType.String, item4.PreSelfMotality);
                    dbServer.AddInParameter(command4, "PreSelfWBC", DbType.String, item4.PreSelfWBC);


                    dbServer.AddInParameter(command4, "PreDonorVolume", DbType.String, item4.PreDonorVolume);
                    dbServer.AddInParameter(command4, "PreDonorConcentration", DbType.String, item4.PreDonorConcentration);
                    dbServer.AddInParameter(command4, "PreDonorMotality", DbType.String, item4.PreDonorMotality);
                    dbServer.AddInParameter(command4, "PreDonorWBC", DbType.String, item4.PreDonorWBC);

                    dbServer.AddInParameter(command4, "PostSelfVolume", DbType.String, item4.PostSelfVolume);
                    dbServer.AddInParameter(command4, "PostSelfConcentration", DbType.String, item4.PostSelfConcentration);
                    dbServer.AddInParameter(command4, "PostSelfMotality", DbType.String, item4.PostSelfMotality);
                    dbServer.AddInParameter(command4, "PostSelfWBC", DbType.String, item4.PostSelfWBC);

                    dbServer.AddInParameter(command4, "PostDonorVolume ", DbType.String, item4.PostDonorVolume);
                    dbServer.AddInParameter(command4, "PostDonorConcentration", DbType.String, item4.PostDonorConcentration);
                    dbServer.AddInParameter(command4, "PostDonorMotality", DbType.String, item4.PostDonorMotality);
                    dbServer.AddInParameter(command4, "PostDonorWBC", DbType.String, item4.PostDonorWBC);

                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item4.ID);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    item4.ID = (long)dbServer.GetParameterValue(command4, "ID");


                }

                if (BizActionObj.Day1Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                    obj.LabDaysSummary = BizActionObj.Day1Details.LabDaySummary;
                    obj.LabDaysSummary.OocyteID = BizActionObj.Day1Details.ID;
                    obj.LabDaysSummary.CoupleID = BizActionObj.Day1Details.CoupleID;
                    obj.LabDaysSummary.CoupleUnitID = BizActionObj.Day1Details.CoupleUnitID;
                    obj.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay1;
                    obj.LabDaysSummary.Priority = 1;
                    obj.LabDaysSummary.ProcDate = BizActionObj.Day1Details.Date;
                    obj.LabDaysSummary.ProcTime = BizActionObj.Day1Details.Time;
                    obj.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    obj.LabDaysSummary.IsFreezed = BizActionObj.Day1Details.IsFreezed;

                    obj.IsUpdate = false;

                    obj = (clsAddUpdateLabDaysSummaryBizActionVO)objBaseDAL.AddUpdateLabDaysSummary(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Day1Details.LabDaySummary.ID = obj.LabDaysSummary.ID;
                }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
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

        private clsAddLabDay1BizActionVO UpdateDay1(clsAddLabDay1BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsFemaleLabDay1VO ObjDay1VO = BizActionObj.Day1Details;
               
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateFemaleLabDay1");

                    dbServer.AddInParameter(command, "ID", DbType.Int64, ObjDay1VO.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    ObjDay1VO.UnitID = UserVo.UserLoginInfo.UnitId;
                    dbServer.AddInParameter(command, "CoupleID", DbType.Int64, ObjDay1VO.CoupleID);
                    dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, ObjDay1VO.CoupleUnitID);

                    dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjDay1VO.Date);
                    dbServer.AddInParameter(command, "Time", DbType.DateTime, ObjDay1VO.Time);
                    dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, ObjDay1VO.EmbryologistID);
                    dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, ObjDay1VO.AssEmbryologistID);
                    dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, ObjDay1VO.AnesthetistID);
                    dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, ObjDay1VO.AssAnesthetistID);
                    dbServer.AddInParameter(command, "IVFCycleCount", DbType.Int64, ObjDay1VO.IVFCycleCount);
                    dbServer.AddInParameter(command, "SourceNeedleID", DbType.Int64, ObjDay1VO.SourceNeedleID);
                    dbServer.AddInParameter(command, "InfectionObserved", DbType.String, ObjDay1VO.InfectionObserved);
                    dbServer.AddInParameter(command, "TotNoOfOocytes", DbType.Int32, ObjDay1VO.TotNoOfOocytes);
                    dbServer.AddInParameter(command, "TotNoOf2PN", DbType.Int32, ObjDay1VO.TotNoOf2PN);
                    dbServer.AddInParameter(command, "TotNoOf3PN", DbType.Int32, ObjDay1VO.TotNoOf3PN);
                    dbServer.AddInParameter(command, "TotNoOf2PB", DbType.Int32, ObjDay1VO.TotNoOf2PB);
                    dbServer.AddInParameter(command, "ToNoOfMI", DbType.Int32, ObjDay1VO.ToNoOfMI);
                    dbServer.AddInParameter(command, "ToNoOfMII", DbType.Int32, ObjDay1VO.ToNoOfMII);
                    dbServer.AddInParameter(command, "ToNoOfGV", DbType.Int32, ObjDay1VO.ToNoOfGV);
                    dbServer.AddInParameter(command, "ToNoOfDeGenerated", DbType.Int32, ObjDay1VO.ToNoOfDeGenerated);
                    dbServer.AddInParameter(command, "ToNoOfLost ", DbType.Int32, ObjDay1VO.ToNoOfLost);
                    dbServer.AddInParameter(command, "IsFreezed ", DbType.Boolean, ObjDay1VO.IsFreezed);

                    dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);


                    if (BizActionObj.Day1Details.ObservationDetails != null && BizActionObj.Day1Details.ObservationDetails.Count > 0)
                    {
                        DbCommand command10 = dbServer.GetStoredProcCommand("CIMS_DeleteLab1ObservatonDetails");

                        dbServer.AddInParameter(command10, "Day1ID", DbType.Int64, ObjDay1VO.ID);
                        int intStatus2 = dbServer.ExecuteNonQuery(command10);
                    }


                    if (BizActionObj.Day1Details.ObservationDetails != null && BizActionObj.Day1Details.ObservationDetails.Count > 0)
                    {
                        foreach (var item in ObjDay1VO.ObservationDetails)
                        {

                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay1ObservationDetails");

                            dbServer.AddInParameter(command1, "Day1ID", DbType.Int64, ObjDay1VO.ID);
                            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command1, "Time", DbType.DateTime, item.Time);
                            dbServer.AddInParameter(command1, "HrAtIns", DbType.Double, item.HrAtIns);
                            dbServer.AddInParameter(command1, "Observation", DbType.String, item.Observation);
                            dbServer.AddInParameter(command1, "FertiCheckPeriod", DbType.String, item.FertiCheckPeriod);
                            dbServer.AddInParameter(command1, "Status", DbType.Boolean, true);

                            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

                            int iStatus = dbServer.ExecuteNonQuery(command1, trans);
                            item.ID = (long)dbServer.GetParameterValue(command1, "ID");
                        }
                    }

                    if (BizActionObj.Day1Details.FertilizationAssesmentDetails != null && BizActionObj.Day1Details.FertilizationAssesmentDetails.Count > 0)
                    {
                        DbCommand command11 = dbServer.GetStoredProcCommand("CIMS_DeleteLab1Details");

                        dbServer.AddInParameter(command11, "Day1ID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command11, "LabDay", DbType.Int32, IVFLabDay.Day1);
                        int intStatus2 = dbServer.ExecuteNonQuery(command11);
                    }

                    if (BizActionObj.Day1Details.FertilizationAssesmentDetails != null && BizActionObj.Day1Details.FertilizationAssesmentDetails.Count > 0)
                    {
                        List<clsEmbryoTransferDetailsVO> ETForwardedDetails = new List<clsEmbryoTransferDetailsVO>();
                        foreach (var item1 in ObjDay1VO.FertilizationAssesmentDetails)
                        {

                            DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay1Details");

                            dbServer.AddInParameter(command2, "Day1ID", DbType.Int64, ObjDay1VO.ID);
                            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "OoNo", DbType.Int64, item1.OoNo);
                            //By Anjali..........
                            dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, item1.SerialOccyteNo);
                            //...................

                            dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, item1.PlanTreatmentID);
                            dbServer.AddInParameter(command2, "Date", DbType.DateTime, item1.Date);
                            dbServer.AddInParameter(command2, "Time", DbType.DateTime, item1.Time);
                            dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, item1.SrcOocyteID);
                            dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, item1.OocyteDonorID);
                            dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, item1.SrcOfSemen);
                            dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, item1.SemenDonorID);
                            if (item1.SelectedFePlan !=null)
                                dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, item1.SelectedFePlan.ID);
                            else
                                dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);
                            if (item1.SelectedGrade !=null)
                                dbServer.AddInParameter(command2, "Grade", DbType.Int64, item1.SelectedGrade.ID);
                            else
                                dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);
                            dbServer.AddInParameter(command2, "Score", DbType.Int64, item1.Score);
                            dbServer.AddInParameter(command2, "PV", DbType.Boolean, item1.PV);
                            dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, item1.XFactor);
                            dbServer.AddInParameter(command2, "Others", DbType.String, item1.Others);
                            //dbServer.AddInParameter(command2, "ProceedDay2", DbType.Boolean, item1.ProceedDay2);
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, item1.SelectedPlan.ID);
                            if (item1.SelectedPlan.ID == 3)
                            {
                                dbServer.AddInParameter(command2, "ProceedDay2", DbType.Boolean, true);
                            }
                            else
                            {
                                dbServer.AddInParameter(command2, "ProceedDay2", DbType.Boolean, false);
                            }
                            dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);

                            dbServer.AddInParameter(command2, "FileName", DbType.String, item1.FileName);
                            dbServer.AddInParameter(command2, "FileContents", DbType.Binary, item1.FileContents);


                            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);

                            int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                            item1.ID = (long)dbServer.GetParameterValue(command2, "ID");


                            //if (ObjDay1VO.IsFreezed == true)
                            //{
                            //    if (item1.SelectedPlan != null && item1.SelectedGrade.ID == 4)
                            //    {
                            //        clsEmbryoTransferDetailsVO obj = new clsEmbryoTransferDetailsVO();
                            //        obj.TransferDate = item1.Date;
                            //        obj.TransferDay = IVFLabDay.Day1;
                            //        obj.RecID = item1.ID;
                            //        obj.EmbryoNumber = item1.OoNo;
                            //        if (item1.SelectedGrade != null)
                            //            obj.GradeID = item1.SelectedGrade.ID;
                            //        else
                            //            obj.GradeID = 0;
                            //        obj.Score = Convert.ToInt32(item1.Score);
                            //        obj.FertilizationStageID = item1.FertilisationStage;

                            //        ETForwardedDetails.Add(obj);
                            //    }
                            //}

                            if (item1.MediaDetails != null && item1.MediaDetails.Count > 0)
                            {
                                foreach (var item3 in item1.MediaDetails)
                                {


                                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");

                                    dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                                    dbServer.AddInParameter(command4, "DetailID", DbType.Int64, item1.ID);
                                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command4, "Date", DbType.DateTime, item3.Date);

                                    dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day1);
                                    dbServer.AddInParameter(command4, "MediaName", DbType.String, item3.ItemName);
                                    dbServer.AddInParameter(command4, "Company", DbType.String, item3.Company);
                                    dbServer.AddInParameter(command4, "LotNo", DbType.String, item3.BatchCode);
                                    dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, item3.ExpiryDate);
                                    dbServer.AddInParameter(command4, "PH", DbType.Boolean, item3.PH);
                                    dbServer.AddInParameter(command4, "OSM", DbType.Boolean, item3.OSM);
                                    dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, item3.VolumeUsed);
                                    dbServer.AddInParameter(command4, "Status", DbType.Int64, item3.SelectedStatus.ID);
                                    dbServer.AddInParameter(command4, "BatchID", DbType.Int64, item3.BatchID);
                                    dbServer.AddInParameter(command4, "StoreID", DbType.Int64, item3.StoreID);

                                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                    item3.ID = (long)dbServer.GetParameterValue(command4, "ID");
                                }
                            }
                            if (item1.CalculateDetails != null )
                            {
                                var item6 = item1.CalculateDetails;


                                DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay1CalculateDetails");

                                dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                                dbServer.AddInParameter(command5, "DetailID", DbType.Int64, item1.ID);
                                dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                                dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);

                                dbServer.AddInParameter(command5, "TwoPNClosed", DbType.Boolean, item6.TwoPNClosed);
                                dbServer.AddInParameter(command5, "TwoPNSeparated", DbType.Boolean, item6.TwoPNSeparated);
                                dbServer.AddInParameter(command5, "NucleoliAlign", DbType.Boolean, item6.NucleoliAlign);
                                dbServer.AddInParameter(command5, "BeginningAlign", DbType.Boolean, item6.BeginningAlign);
                                dbServer.AddInParameter(command5, "Scattered", DbType.Boolean, item6.Scattered);
                                dbServer.AddInParameter(command5, "CytoplasmHetero", DbType.Boolean, item6.CytoplasmHetero);
                                dbServer.AddInParameter(command5, "CytoplasmHomo", DbType.Boolean, item6.CytoplasmHomo);
                                dbServer.AddInParameter(command5, "NuclearMembrane", DbType.Boolean, item6.NuclearMembrane);


                                dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item6.ID);
                                dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatus1 = dbServer.ExecuteNonQuery(command5, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command5, "ResultStatus");
                                item6.ID = (long)dbServer.GetParameterValue(command5, "ID");
                                
                                
                            }


                        }

                        //if (BizActionObj.Day1Details.IsFreezed == true && ETForwardedDetails.Count > 0)
                        //{
                        //    clsBaseIVFEmbryoTransferDAL objBaseDAL = clsBaseIVFEmbryoTransferDAL.GetInstance();
                        //    clsAddForwardedEmbryoTransferBizActionVO BizActionFrwrd = new clsAddForwardedEmbryoTransferBizActionVO();
                        //    BizActionFrwrd.UnitID = BizActionObj.Day1Details.UnitID;
                        //    BizActionFrwrd.CoupleID = BizActionObj.Day1Details.CoupleID;
                        //    BizActionFrwrd.CoupleUnitID = BizActionObj.Day1Details.CoupleUnitID;
                        //    BizActionFrwrd.ForwardedEmbryos = ETForwardedDetails;

                        //    BizActionFrwrd = (clsAddForwardedEmbryoTransferBizActionVO)objBaseDAL.AddForwardedEmbryos(BizActionFrwrd, UserVo, con, trans);

                        //    if (BizActionFrwrd.SuccessStatus == -1) throw new Exception();
                        //}

                    }
                    if (BizActionObj.Day1Details.FUSetting != null && BizActionObj.Day1Details.FUSetting.Count > 0)
                    {
                        DbCommand command13 = dbServer.GetStoredProcCommand("CIMS_DeleteLab1UploadFileDetails");

                        dbServer.AddInParameter(command13, "Day1ID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command13, "LabDay", DbType.Int32, IVFLabDay.Day1);
                        dbServer.AddInParameter(command13, "UnitID", DbType.Int64, ObjDay1VO.UnitID);
                        int intStatus2 = dbServer.ExecuteNonQuery(command13);
                    }
                    if (BizActionObj.Day1Details.FUSetting != null && BizActionObj.Day1Details.FUSetting.Count > 0)
                    {
                        foreach (var item2 in ObjDay1VO.FUSetting)
                        {
                            DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                            dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day1);
                            dbServer.AddInParameter(command3, "FileName", DbType.String, item2.FileName);
                            dbServer.AddInParameter(command3, "FileIndex", DbType.Int32, item2.Index);
                            dbServer.AddInParameter(command3, "Value", DbType.Binary, item2.Data);
                            dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);

                            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                            item2.ID = (long)dbServer.GetParameterValue(command3, "ID");

                        }
                    }
                    if (BizActionObj.Day1Details.SemenDetails != null)
                    {
                        DbCommand command12 = dbServer.GetStoredProcCommand("CIMS_DeleteLab1SemenDetails");

                        dbServer.AddInParameter(command12, "Day1ID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command12, "LabDay", DbType.Int32, IVFLabDay.Day1);
                        int intStatus2 = dbServer.ExecuteNonQuery(command12);
                    }
                    if (BizActionObj.Day1Details.SemenDetails != null)
                    {
                        var item4 = BizActionObj.Day1Details.SemenDetails;
                        DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");

                        dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day1);

                        dbServer.AddInParameter(command4, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day1Details.SemenDetails.MethodOfSpermPreparation);
                        dbServer.AddInParameter(command4, "SourceOfSemen", DbType.Int64, BizActionObj.Day1Details.SemenDetails.SourceOfSemen);

                        dbServer.AddInParameter(command4, "PreSelfVolume", DbType.String, item4.PreSelfVolume);
                        dbServer.AddInParameter(command4, "PreSelfConcentration", DbType.String, item4.PreSelfConcentration);
                        dbServer.AddInParameter(command4, "PreSelfMotality", DbType.String, item4.PreSelfMotality);
                        dbServer.AddInParameter(command4, "PreSelfWBC", DbType.String, item4.PreSelfWBC);


                        dbServer.AddInParameter(command4, "PreDonorVolume", DbType.String, item4.PreDonorVolume);
                        dbServer.AddInParameter(command4, "PreDonorConcentration", DbType.String, item4.PreDonorConcentration);
                        dbServer.AddInParameter(command4, "PreDonorMotality", DbType.String, item4.PreDonorMotality);
                        dbServer.AddInParameter(command4, "PreDonorWBC", DbType.String, item4.PreDonorWBC);

                        dbServer.AddInParameter(command4, "PostSelfVolume", DbType.String, item4.PostSelfVolume);
                        dbServer.AddInParameter(command4, "PostSelfConcentration", DbType.String, item4.PostSelfConcentration);
                        dbServer.AddInParameter(command4, "PostSelfMotality", DbType.String, item4.PostSelfMotality);
                        dbServer.AddInParameter(command4, "PostSelfWBC", DbType.String, item4.PostSelfWBC);

                        dbServer.AddInParameter(command4, "PostDonorVolume ", DbType.String, item4.PostDonorVolume);
                        dbServer.AddInParameter(command4, "PostDonorConcentration", DbType.String, item4.PostDonorConcentration);
                        dbServer.AddInParameter(command4, "PostDonorMotality", DbType.String, item4.PostDonorMotality);
                        dbServer.AddInParameter(command4, "PostDonorWBC", DbType.String, item4.PostDonorWBC);

                        dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item4.ID);
                        dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                        item4.ID = (long)dbServer.GetParameterValue(command4, "ID");


                    }

                    if (BizActionObj.Day1Details.LabDaySummary != null)
                    {
                        clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                        clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                        obj.LabDaysSummary = BizActionObj.Day1Details.LabDaySummary;
                        obj.LabDaysSummary.OocyteID = BizActionObj.Day1Details.ID;
                        obj.LabDaysSummary.CoupleID = BizActionObj.Day1Details.CoupleID;
                        obj.LabDaysSummary.CoupleUnitID = BizActionObj.Day1Details.CoupleUnitID;
                        obj.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay1;
                        obj.LabDaysSummary.Priority = 1;
                        obj.LabDaysSummary.ProcDate = BizActionObj.Day1Details.Date;
                        obj.LabDaysSummary.ProcTime = BizActionObj.Day1Details.Time;
                        obj.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                        obj.LabDaysSummary.IsFreezed = BizActionObj.Day1Details.IsFreezed;


                        obj.IsUpdate = true;

                        obj = (clsAddUpdateLabDaysSummaryBizActionVO)objBaseDAL.AddUpdateLabDaysSummary(obj, UserVo, con, trans);
                        if (obj.SuccessStatus == -1) throw new Exception();
                        BizActionObj.Day1Details.LabDaySummary.ID = obj.LabDaysSummary.ID;
                    }
                

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
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

        public override IValueObject GetLabDay0ForDay1(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLabDay0ForLabDay1BizActionVO BizActionObj = valueObject as clsGetLabDay0ForLabDay1BizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFemaleLabDay0ForLabDay1");
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.CoupleUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Day1Details == null)
                        BizActionObj.Day1Details = new List<clsFemaleLabDay1FertilizationAssesmentVO>();
                    while (reader.Read())
                    {
                        clsFemaleLabDay1FertilizationAssesmentVO Obj = new clsFemaleLabDay1FertilizationAssesmentVO();
                        Obj.ID = (long)DALHelper.HandleDBNull(reader["DetailID"]);
                        Obj.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        Obj.Time = (DateTime?)DALHelper.HandleDate(reader["Time"]);

                        Obj.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);
                        //By Anjali................
                        Obj.SerialOccyteNo = (long)DALHelper.HandleDBNull(reader["SerialOccyteNo"]);
                        //............................

                        Obj.PlanTreatmentID = (int)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);
                        Obj.PlanTreatment = (string)DALHelper.HandleDBNull(reader["Protocol"]);

                        Obj.SrcOocyteID = (long)DALHelper.HandleDBNull(reader["SrcOocyteID"]);
                        Obj.SrcOocyteDescription = (string)DALHelper.HandleDBNull(reader["SOOocyte"]);

                        Obj.OocyteDonorID = (string)DALHelper.HandleDBNull(reader["OSCode"]);
                        

                        Obj.SrcOfSemen = (long)DALHelper.HandleDBNull(reader["SrcOfSemen"]);
                        Obj.SrcOfSemenDescription = (string)DALHelper.HandleDBNull(reader["SOSemen"]);

                        Obj.SemenDonorID = (string)DALHelper.HandleDBNull(reader["SSCode"]);

                        Obj.PlanTherapyId = (long)DALHelper.HandleDBNull(reader["PlanTherapyId"]);

                        BizActionObj.Day1Details.Add(Obj);

                        BizActionObj.AnaesthetistID = (long)DALHelper.HandleDBNull(reader["AnasthesistID"]);
                        BizActionObj.AssAnaesthetistID = (long)DALHelper.HandleDBNull(reader["AssAnasthesistID"]);
                        BizActionObj.AssEmbryologistID = (long)DALHelper.HandleDBNull(reader["AssEmbryologistID"]);
                        BizActionObj.EmbryologistID = (long)DALHelper.HandleDBNull(reader["EmbryologistID"]);
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

     

        public override IValueObject GetFemaleLabDay1(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay1DetailsBizActionVO BizAction = (valueObject) as clsGetDay1DetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleLabDay1");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizAction.CoupleUnitID);
                dbServer.AddInParameter(command, "LabDay", DbType.Int16, IVFLabDay.Day1);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.LabDay1 == null)
                        BizAction.LabDay1 = new clsFemaleLabDay1VO();
                    while (reader.Read())
                    {


                        BizAction.LabDay1.ID = (long)(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.LabDay1.UnitID = (long)(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.LabDay1.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        BizAction.LabDay1.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));

                        BizAction.LabDay1.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        BizAction.LabDay1.Time = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                        BizAction.LabDay1.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.LabDay1.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        BizAction.LabDay1.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        BizAction.LabDay1.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        BizAction.LabDay1.SourceNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SourceNeedleID"]));
                        BizAction.LabDay1.InfectionObserved = (string)(DALHelper.HandleDBNull(reader["InfectionObserved"]));

                        BizAction.LabDay1.TotNoOfOocytes = (int)(DALHelper.HandleDBNull(reader["TotNoOfOocytes"]));
                        BizAction.LabDay1.TotNoOf2PN = (int)(DALHelper.HandleDBNull(reader["TotNoOf2PN"]));
                        BizAction.LabDay1.TotNoOf3PN = (int)(DALHelper.HandleDBNull(reader["TotNoOf3PN"]));
                        BizAction.LabDay1.TotNoOf2PB = (int)(DALHelper.HandleDBNull(reader["TotNoOf2PB"]));
                        BizAction.LabDay1.ToNoOfMI = (int)(DALHelper.HandleDBNull(reader["ToNoOfMI"]));
                        BizAction.LabDay1.ToNoOfMII = (int)(DALHelper.HandleDBNull(reader["ToNoOfMII"]));
                        BizAction.LabDay1.ToNoOfGV = (int)(DALHelper.HandleDBNull(reader["ToNoOfGV"]));
                        BizAction.LabDay1.ToNoOfDeGenerated = (int)(DALHelper.HandleDBNull(reader["ToNoOfDeGenerated"]));
                        BizAction.LabDay1.ToNoOfLost = (int)(DALHelper.HandleDBNull(reader["ToNoOfLost"]));
                        BizAction.LabDay1.Status = (bool)(DALHelper.HandleDBNull(reader["Status"]));
                        BizAction.LabDay1.IsFreezed = (bool)(DALHelper.HandleDBNull(reader["IsFreezed"]));

                    }

                    reader.NextResult();
                    if (BizAction.LabDay1.ObservationDetails == null)
                        BizAction.LabDay1.ObservationDetails = new List<clsFemaleLabDay1InseminationPlatesVO>();


                    while (reader.Read())
                    {
                        clsFemaleLabDay1InseminationPlatesVO ObjItem = new clsFemaleLabDay1InseminationPlatesVO();
                        ObjItem.Time = (DateTime?)(DALHelper.HandleDBNull(reader["Time"]));
                        ObjItem.HrAtIns = (double)(DALHelper.HandleDBNull(reader["HrAtIns"]));
                        ObjItem.Observation = (string)(DALHelper.HandleDBNull(reader["Observation"]));
                        ObjItem.FertiCheckPeriod = (string)(DALHelper.HandleDBNull(reader["FertiCheckPeriod"]));
                        BizAction.LabDay1.ObservationDetails.Add(ObjItem);

                    }
                    reader.NextResult();
                    if (BizAction.LabDay1.FertilizationAssesmentDetails == null)
                        BizAction.LabDay1.FertilizationAssesmentDetails = new List<clsFemaleLabDay1FertilizationAssesmentVO>();


                    while (reader.Read())
                    {
                        clsFemaleLabDay1FertilizationAssesmentVO ObjFer = new clsFemaleLabDay1FertilizationAssesmentVO();

                        ObjFer.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);

                        //By Anjali.............
                          ObjFer.SerialOccyteNo = (long)DALHelper.HandleDBNull(reader["SerialOccyteNo"]);
                    
                        //................
                        ObjFer.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ObjFer.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        ObjFer.Time = (DateTime?)DALHelper.HandleDate(reader["Time"]);

                        ObjFer.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);
                        ObjFer.PlanTreatment = (string)DALHelper.HandleDBNull(reader["Protocol"]);

                        ObjFer.SrcOocyteID = (long)DALHelper.HandleDBNull(reader["SrcOocyteID"]);
                        ObjFer.SrcOocyteDescription = (string)DALHelper.HandleDBNull(reader["SOOocyte"]);

                        ObjFer.OocyteDonorID = (string)DALHelper.HandleDBNull(reader["OSCode"]);
                      

                        ObjFer.SrcOfSemen = (long)DALHelper.HandleDBNull(reader["SrcOfSemen"]);
                        ObjFer.SrcOfSemenDescription = (string)DALHelper.HandleDBNull(reader["SOSemen"]);

                        ObjFer.SemenDonorID = (string)DALHelper.HandleDBNull(reader["SSCode"]);
                      
                        ObjFer.SelectedFePlan.ID = (long)(DALHelper.HandleDBNull(reader["FertilisationStage"]));
                        //if ((string)(DALHelper.HandleDBNull(reader["Fertilization"]))!=null)
                        ObjFer.SelectedFePlan.Description = (string)(DALHelper.HandleDBNull(reader["Fertilization"]));
                        //else
                        //    ObjFer.SelectedFePlan.Description = "--Select--";

                        ObjFer.SelectedGrade.ID = (long)(DALHelper.HandleDBNull(reader["GradeID"]));
                        ObjFer.SelectedGrade.Description = (string)(DALHelper.HandleDBNull(reader["Grade"]));

                        ObjFer.Score = (long)(DALHelper.HandleDBNull(reader["Score"]));
                        ObjFer.PV = (bool)(DALHelper.HandleDBNull(reader["PV"]));
                        ObjFer.XFactor = (bool)(DALHelper.HandleDBNull(reader["XFactor"]));
                        ObjFer.Others = (string)(DALHelper.HandleDBNull(reader["Others"]));
                        ObjFer.ProceedDay2 = (bool)(DALHelper.HandleDBNull(reader["ProceedDay2"]));
                        ObjFer.SelectedPlan.ID = (long)(DALHelper.HandleDBNull(reader["PlanID"]));
                        ObjFer.SelectedPlan.Description = (string)(DALHelper.HandleDBNull(reader["PlanName"]));

                        ObjFer.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ObjFer.FileContents = (byte[])(DALHelper.HandleDBNull(reader["FileContents"]));


                           

                        //For getting Details
                        clsBaseIVFLabDayDAL objBaseDAL = clsBaseIVFLabDayDAL.GetInstance();
                        clsGetDay1MediaAndCalcDetailsBizActionVO BizActionDetails = new clsGetDay1MediaAndCalcDetailsBizActionVO();
                        BizActionDetails.ID = BizAction.ID;
                        BizActionDetails.DetailID = ObjFer.ID;
                        BizActionDetails = (clsGetDay1MediaAndCalcDetailsBizActionVO)objBaseDAL.GetFemaleLabDay1MediaAndCalDetails(BizActionDetails, UserVo);
                        ObjFer.CalculateDetails = BizActionDetails.LabDayCalDetails;

                        //For getting Media details
                        clsBaseIVFLabDayDAL objBaseDAL2 = clsBaseIVFLabDayDAL.GetInstance();
                        clsGetAllDayMediaDetailsBizActionVO BizActionMedia = new clsGetAllDayMediaDetailsBizActionVO();
                        BizActionMedia.ID = BizAction.ID;
                        BizActionMedia.DetailID = ObjFer.ID;
                        BizActionMedia.LabDay = 1;
                        BizActionMedia = (clsGetAllDayMediaDetailsBizActionVO)objBaseDAL2.GetAllDayMediaDetails(BizActionMedia, UserVo);
                        ObjFer.MediaDetails = BizActionMedia.MediaList;

                        BizAction.LabDay1.FertilizationAssesmentDetails.Add(ObjFer);

                    }
                    reader.NextResult();
                    if (BizAction.LabDay1.SemenDetails == null)
                        BizAction.LabDay1.SemenDetails = new clsFemaleSemenDetailsVO();


                    while (reader.Read())
                    {
                        BizAction.LabDay1.SemenDetails.MOSP = (string)(DALHelper.HandleDBNull(reader["MOSP"]));
                        BizAction.LabDay1.SemenDetails.SOS = (string)(DALHelper.HandleDBNull(reader["SOS"]));

                        BizAction.LabDay1.SemenDetails.MethodOfSpermPreparation = (long)(DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]));
                        BizAction.LabDay1.SemenDetails.SourceOfSemen = (long)(DALHelper.HandleDBNull(reader["SourceOfSemen"]));

                        BizAction.LabDay1.SemenDetails.PreSelfVolume = (string)(DALHelper.HandleDBNull(reader["PreSelfVolume"]));
                        BizAction.LabDay1.SemenDetails.PreSelfConcentration = (string)(DALHelper.HandleDBNull(reader["PreSelfConcentration"]));
                        BizAction.LabDay1.SemenDetails.PreSelfMotality = (string)(DALHelper.HandleDBNull(reader["PreSelfMotality"]));
                        BizAction.LabDay1.SemenDetails.PreSelfWBC = (string)(DALHelper.HandleDBNull(reader["PreSelfWBC"]));

                        BizAction.LabDay1.SemenDetails.PreDonorVolume = (string)(DALHelper.HandleDBNull(reader["PreDonorVolume"]));
                        BizAction.LabDay1.SemenDetails.PreDonorConcentration = (string)(DALHelper.HandleDBNull(reader["PreDonorConcentration"]));
                        BizAction.LabDay1.SemenDetails.PreDonorMotality = (string)(DALHelper.HandleDBNull(reader["PreDonorMotality"]));
                        BizAction.LabDay1.SemenDetails.PreDonorWBC = (string)(DALHelper.HandleDBNull(reader["PreDonorWBC"]));


                        BizAction.LabDay1.SemenDetails.PostSelfVolume = (string)(DALHelper.HandleDBNull(reader["PostSelfVolume"]));
                        BizAction.LabDay1.SemenDetails.PostSelfConcentration = (string)(DALHelper.HandleDBNull(reader["PostSelfConcentration"]));
                        BizAction.LabDay1.SemenDetails.PostSelfMotality = (string)(DALHelper.HandleDBNull(reader["PostSelfMotality"]));
                        BizAction.LabDay1.SemenDetails.PostSelfWBC = (string)(DALHelper.HandleDBNull(reader["PostSelfWBC"]));

                        BizAction.LabDay1.SemenDetails.PostDonorVolume = (string)(DALHelper.HandleDBNull(reader["PostDonorVolume"]));
                        BizAction.LabDay1.SemenDetails.PostDonorConcentration = (string)(DALHelper.HandleDBNull(reader["PostDonorConcentration"]));
                        BizAction.LabDay1.SemenDetails.PostDonorMotality = (string)(DALHelper.HandleDBNull(reader["PostDonorMotality"]));
                        BizAction.LabDay1.SemenDetails.PostDonorWBC = (string)(DALHelper.HandleDBNull(reader["PostDonorWBC"]));
                    }

                    reader.NextResult();
                    if (BizAction.LabDay1.FUSetting == null)
                        BizAction.LabDay1.FUSetting = new List<FileUpload>();
                    while (reader.Read())
                    {
                        FileUpload ObjFile = new FileUpload();
                        ObjFile.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ObjFile.Index = (Int32)(DALHelper.HandleDBNull(reader["FileIndex"]));
                        ObjFile.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ObjFile.Data = (byte[])(DALHelper.HandleDBNull(reader["Value"]));
                        BizAction.LabDay1.FUSetting.Add(ObjFile);
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

        public override IValueObject GetFemaleLabDay1MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay1MediaAndCalcDetailsBizActionVO BizAction = (valueObject) as clsGetDay1MediaAndCalcDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMediaDetailsAndCalculateDetail");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "DetailID", DbType.Int64, BizAction.DetailID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.LabDayCalDetails == null)
                        BizAction.LabDayCalDetails = new clsFemaleLabDay1CalculateDetailsVO();

                    while (reader.Read())
                    {

                        BizAction.LabDayCalDetails.FertilizationID = (long)(DALHelper.HandleDBNull(reader["DetailID"]));
                        BizAction.LabDayCalDetails.TwoPNClosed = (bool)(DALHelper.HandleDBNull(reader["TwoPNClosed"]));
                        BizAction.LabDayCalDetails.TwoPNSeparated = (bool)(DALHelper.HandleDBNull(reader["TwoPNSeparated"]));
                        BizAction.LabDayCalDetails.NucleoliAlign = (bool)(DALHelper.HandleDBNull(reader["NucleoliAlign"]));
                        BizAction.LabDayCalDetails.BeginningAlign = (bool)(DALHelper.HandleDBNull(reader["BeginningAlign"]));
                        BizAction.LabDayCalDetails.Scattered = (bool)(DALHelper.HandleDBNull(reader["Scattered"]));
                        BizAction.LabDayCalDetails.CytoplasmHetero = (bool)(DALHelper.HandleDBNull(reader["CytoplasmHetero"]));
                        BizAction.LabDayCalDetails.CytoplasmHomo = (bool)(DALHelper.HandleDBNull(reader["CytoplasmHomo"]));
                        BizAction.LabDayCalDetails.NuclearMembrane = (bool)(DALHelper.HandleDBNull(reader["NuclearMembrane"]));
                        
                    }

                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizAction;

        }

        #endregion


        #region Day 2
        public override IValueObject AddLabDay2(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddLabDay2BizActionVO BizActionObj = valueObject as clsAddLabDay2BizActionVO;

            if (BizActionObj.Day2Details.ID == 0)
                BizActionObj = AddDay2(BizActionObj, UserVo);
            else
                BizActionObj = UpdateDay2(BizActionObj, UserVo);

            return valueObject;          

        }

        private clsAddLabDay2BizActionVO AddDay2(clsAddLabDay2BizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsFemaleLabDay2VO ObjDay1VO = BizActionObj.Day2Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay2");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, ObjDay1VO.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, ObjDay1VO.CoupleUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjDay1VO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, ObjDay1VO.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, ObjDay1VO.EmbryologistID);
                dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, ObjDay1VO.AssEmbryologistID);
                dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, ObjDay1VO.AnesthetistID);
                dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, ObjDay1VO.AssAnesthetistID);
                dbServer.AddInParameter(command, "IVFCycleCount", DbType.Int64, ObjDay1VO.IVFCycleCount);
                dbServer.AddInParameter(command, "SourceNeedleID", DbType.Int64, ObjDay1VO.SourceNeedleID);
                dbServer.AddInParameter(command, "InfectionObserved", DbType.String, ObjDay1VO.InfectionObserved);
                dbServer.AddInParameter(command, "TotNoOfOocytes", DbType.Int32, ObjDay1VO.TotNoOfOocytes);
                dbServer.AddInParameter(command, "TotNoOf2PN", DbType.Int32, ObjDay1VO.TotNoOf2PN);
                dbServer.AddInParameter(command, "TotNoOf3PN", DbType.Int32, ObjDay1VO.TotNoOf3PN);
                dbServer.AddInParameter(command, "TotNoOf2PB", DbType.Int32, ObjDay1VO.TotNoOf2PB);
                dbServer.AddInParameter(command, "ToNoOfMI", DbType.Int32, ObjDay1VO.ToNoOfMI);
                dbServer.AddInParameter(command, "ToNoOfMII", DbType.Int32, ObjDay1VO.ToNoOfMII);
                dbServer.AddInParameter(command, "ToNoOfGV", DbType.Int32, ObjDay1VO.ToNoOfGV);
                dbServer.AddInParameter(command, "ToNoOfDeGenerated", DbType.Int32, ObjDay1VO.ToNoOfDeGenerated);
                dbServer.AddInParameter(command, "ToNoOfLost ", DbType.Int32, ObjDay1VO.ToNoOfLost);

                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, ObjDay1VO.IsFreezed);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);



                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDay1VO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Day2Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.Day2Details.FertilizationAssesmentDetails != null && BizActionObj.Day2Details.FertilizationAssesmentDetails.Count > 0)
                {
                    List<clsEmbryoTransferDetailsVO> ETForwardedDetails = new List<clsEmbryoTransferDetailsVO>();

                    foreach (var item1 in ObjDay1VO.FertilizationAssesmentDetails)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay2Details");

                        dbServer.AddInParameter(command2, "Day2ID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "OoNo", DbType.Int64, item1.OoNo);
                        //By Anjali..........
                        dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, item1.SerialOccyteNo);
                        //...................
                        dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, item1.PlanTreatmentID);
                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, item1.Date);
                        dbServer.AddInParameter(command2, "Time", DbType.DateTime, item1.Time);
                        dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, item1.SrcOocyteID);
                        dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, item1.OocyteDonorID);
                        dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, item1.SrcOfSemen);
                        dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, item1.SemenDonorID);

                        dbServer.AddInParameter(command2, "FileName", DbType.String, item1.FileName);
                        dbServer.AddInParameter(command2, "FileContents", DbType.Binary, item1.FileContents);

                        if (item1.SelectedFePlan != null)
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, item1.SelectedFePlan.ID);
                        else
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);

                        if (item1.SelectedGrade != null)
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, item1.SelectedGrade.ID);
                        else
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);
                        if (item1.SelectedFragmentation != null)
                            dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, item1.SelectedFragmentation.ID);
                        else
                            dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, 0);

                        if (item1.SelectedBlastomereSymmetry != null)
                            dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, item1.SelectedBlastomereSymmetry.ID);
                        else
                            dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, 0);

                        dbServer.AddInParameter(command2, "Score", DbType.Int64, item1.Score);
                        dbServer.AddInParameter(command2, "PV", DbType.Boolean, item1.PV);
                        dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, item1.XFactor);
                        dbServer.AddInParameter(command2, "Others", DbType.String, item1.Others);
                        //dbServer.AddInParameter(command2, "ProceedDay3", DbType.Boolean, item1.ProceedDay3);
                        if(item1.SelectedPlan !=null)
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, item1.SelectedPlan.ID);
                        else
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);
                        if (item1.SelectedPlan.ID == 3)
                        {
                            dbServer.AddInParameter(command2, "ProceedDay3", DbType.Boolean, true);
                        }
                        else
                        {
                            dbServer.AddInParameter(command2, "ProceedDay3", DbType.Boolean, false);
                        }
                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);

                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                        item1.ID = (long)dbServer.GetParameterValue(command2, "ID");


                        // Forward Embryos
                        //if (ObjDay1VO.IsFreezed == true)
                        //{
                        //    if (item1.SelectedPlan != null && item1.SelectedGrade.ID == 4)
                        //    {
                        //        clsEmbryoTransferDetailsVO obj = new clsEmbryoTransferDetailsVO();
                        //        obj.TransferDate = item1.Date;
                        //        obj.TransferDay = IVFLabDay.Day2;
                        //        obj.RecID = item1.ID;
                        //        obj.EmbryoNumber = item1.OoNo;
                        //        if (item1.SelectedGrade != null)
                        //            obj.GradeID = item1.SelectedGrade.ID;
                        //        else
                        //            obj.GradeID = 0;
                        //        obj.Score = Convert.ToInt32(item1.Score);
                        //        obj.FertilizationStageID = item1.FertilisationStage;

                        //        ETForwardedDetails.Add(obj);
                        //    }
                        //}


                        if (item1.MediaDetails != null && item1.MediaDetails.Count > 0)
                        {
                            foreach (var item3 in item1.MediaDetails)
                            {
                                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");

                                dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                                dbServer.AddInParameter(command4, "DetailID", DbType.Int64, item1.ID);
                                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command4, "Date", DbType.DateTime, item3.Date);
                                dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day2);
                                dbServer.AddInParameter(command4, "MediaName", DbType.String, item3.ItemName);
                                dbServer.AddInParameter(command4, "Company", DbType.String, item3.Company);
                                dbServer.AddInParameter(command4, "LotNo", DbType.String, item3.BatchCode);
                                dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, item3.ExpiryDate);
                                dbServer.AddInParameter(command4, "PH", DbType.Boolean, item3.PH);
                                dbServer.AddInParameter(command4, "OSM", DbType.Boolean, item3.OSM);
                                dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, item3.VolumeUsed);
                                dbServer.AddInParameter(command4, "Status", DbType.Int64, item3.SelectedStatus.ID);
                                dbServer.AddInParameter(command4, "BatchID", DbType.Int64, item3.BatchID);
                                dbServer.AddInParameter(command4, "StoreID", DbType.Int64, item3.StoreID);
                                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                item3.ID = (long)dbServer.GetParameterValue(command4, "ID");
                            }
                        }
                        if (item1.Day2CalculateDetails != null)
                        {
                                var item6 = item1.Day2CalculateDetails;
                                DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay2CalculateDetails");

                                dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                                dbServer.AddInParameter(command5, "DetailID", DbType.Int64, item1.ID);
                                dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                                dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);
                                dbServer.AddInParameter(command5, "ZonaThickness", DbType.Int32, item6.ZonaThickness);
                                dbServer.AddInParameter(command5, "ZonaTexture", DbType.Int32, item6.ZonaTexture);
                                dbServer.AddInParameter(command5, "BlastomereSize", DbType.Int32, item6.BlastomereSize);
                                dbServer.AddInParameter(command5, "BlastomereShape", DbType.Int32, item6.BlastomereShape);
                                dbServer.AddInParameter(command5, "BlastomeresVolume", DbType.Int32, item6.BlastomeresVolume);
                                dbServer.AddInParameter(command5, "Membrane", DbType.Int32, item6.Membrane);
                                dbServer.AddInParameter(command5, "Cytoplasm", DbType.Int32, item6.Cytoplasm);
                                dbServer.AddInParameter(command5, "Fragmentation", DbType.Int32, item6.Fragmentation);
                                dbServer.AddInParameter(command5, "DevelopmentRate", DbType.Int32, item6.DevelopmentRate);
                                dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item6.ID);
                                dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatus1 = dbServer.ExecuteNonQuery(command5, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command5, "ResultStatus");
                                item6.ID = (long)dbServer.GetParameterValue(command5, "ID");
                            
                        }
                    }

                    //if (BizActionObj.Day2Details.IsFreezed == true && ETForwardedDetails.Count > 0)
                    //{
                    //    clsBaseIVFEmbryoTransferDAL objBaseDAL = clsBaseIVFEmbryoTransferDAL.GetInstance();
                    //    clsAddForwardedEmbryoTransferBizActionVO BizActionFrwrd = new clsAddForwardedEmbryoTransferBizActionVO();
                    //    BizActionFrwrd.UnitID = BizActionObj.Day2Details.UnitID;
                    //    BizActionFrwrd.CoupleID = BizActionObj.Day2Details.CoupleID;
                    //    BizActionFrwrd.CoupleUnitID = BizActionObj.Day2Details.CoupleUnitID;
                    //    BizActionFrwrd.ForwardedEmbryos = ETForwardedDetails;

                    //    BizActionFrwrd = (clsAddForwardedEmbryoTransferBizActionVO)objBaseDAL.AddForwardedEmbryos(BizActionFrwrd, UserVo, con, trans);

                    //    if (BizActionFrwrd.SuccessStatus == -1) throw new Exception();
                    //}
                }

                if (BizActionObj.Day2Details.FUSetting != null && BizActionObj.Day2Details.FUSetting.Count > 0)
                {
                    foreach (var item2 in ObjDay1VO.FUSetting)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                        dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day2);
                        dbServer.AddInParameter(command3, "FileName", DbType.String, item2.FileName);
                        dbServer.AddInParameter(command3, "FileIndex", DbType.Int32, item2.Index);
                        dbServer.AddInParameter(command3, "Value", DbType.Binary, item2.Data);
                        dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                        dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                        item2.ID = (long)dbServer.GetParameterValue(command3, "ID");
                    }
                }

                if (BizActionObj.Day2Details.SemenDetails != null)
                {
                    var item4 = BizActionObj.Day2Details.SemenDetails;
                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");

                    dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day2);
                    dbServer.AddInParameter(command4, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day2Details.SemenDetails.MethodOfSpermPreparation);
                    dbServer.AddInParameter(command4, "SourceOfSemen", DbType.Int64, BizActionObj.Day2Details.SemenDetails.SourceOfSemen);
                    dbServer.AddInParameter(command4, "PreSelfVolume", DbType.String, item4.PreSelfVolume);
                    dbServer.AddInParameter(command4, "PreSelfConcentration", DbType.String, item4.PreSelfConcentration);
                    dbServer.AddInParameter(command4, "PreSelfMotality", DbType.String, item4.PreSelfMotality);
                    dbServer.AddInParameter(command4, "PreSelfWBC", DbType.String, item4.PreSelfWBC);
                    dbServer.AddInParameter(command4, "PreDonorVolume", DbType.String, item4.PreDonorVolume);
                    dbServer.AddInParameter(command4, "PreDonorConcentration", DbType.String, item4.PreDonorConcentration);
                    dbServer.AddInParameter(command4, "PreDonorMotality", DbType.String, item4.PreDonorMotality);
                    dbServer.AddInParameter(command4, "PreDonorWBC", DbType.String, item4.PreDonorWBC);
                    dbServer.AddInParameter(command4, "PostSelfVolume", DbType.String, item4.PostSelfVolume);
                    dbServer.AddInParameter(command4, "PostSelfConcentration", DbType.String, item4.PostSelfConcentration);
                    dbServer.AddInParameter(command4, "PostSelfMotality", DbType.String, item4.PostSelfMotality);
                    dbServer.AddInParameter(command4, "PostSelfWBC", DbType.String, item4.PostSelfWBC);
                    dbServer.AddInParameter(command4, "PostDonorVolume ", DbType.String, item4.PostDonorVolume);
                    dbServer.AddInParameter(command4, "PostDonorConcentration", DbType.String, item4.PostDonorConcentration);
                    dbServer.AddInParameter(command4, "PostDonorMotality", DbType.String, item4.PostDonorMotality);
                    dbServer.AddInParameter(command4, "PostDonorWBC", DbType.String, item4.PostDonorWBC);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item4.ID);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    item4.ID = (long)dbServer.GetParameterValue(command4, "ID");
                }
                if (BizActionObj.Day2Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                    obj.LabDaysSummary = BizActionObj.Day2Details.LabDaySummary;
                    obj.LabDaysSummary.OocyteID = BizActionObj.Day2Details.ID;
                    obj.LabDaysSummary.CoupleID = BizActionObj.Day2Details.CoupleID;
                    obj.LabDaysSummary.CoupleUnitID = BizActionObj.Day2Details.CoupleUnitID;
                    obj.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay2;
                    obj.LabDaysSummary.Priority = 1;
                    obj.LabDaysSummary.ProcDate = BizActionObj.Day2Details.Date;
                    obj.LabDaysSummary.ProcTime = BizActionObj.Day2Details.Time;
                    obj.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    obj.LabDaysSummary.IsFreezed = BizActionObj.Day2Details.IsFreezed;
                    obj.IsUpdate = false;
                    obj = (clsAddUpdateLabDaysSummaryBizActionVO)objBaseDAL.AddUpdateLabDaysSummary(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Day2Details.LabDaySummary.ID = obj.LabDaysSummary.ID;
                }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
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
        
        private clsAddLabDay2BizActionVO UpdateDay2(clsAddLabDay2BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsFemaleLabDay2VO ObjDay1VO = BizActionObj.Day2Details;
                
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateFemaleLabDay2");

                    dbServer.AddInParameter(command, "ID", DbType.Int64, ObjDay1VO.ID);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    ObjDay1VO.UnitID = UserVo.UserLoginInfo.UnitId;
                    dbServer.AddInParameter(command, "CoupleID", DbType.Int64, ObjDay1VO.CoupleID);
                    dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, ObjDay1VO.CoupleUnitID);

                    dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjDay1VO.Date);
                    dbServer.AddInParameter(command, "Time", DbType.DateTime, ObjDay1VO.Time);
                    dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, ObjDay1VO.EmbryologistID);
                    dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, ObjDay1VO.AssEmbryologistID);
                    dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, ObjDay1VO.AnesthetistID);
                    dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, ObjDay1VO.AssAnesthetistID);
                    dbServer.AddInParameter(command, "IVFCycleCount", DbType.Int64, ObjDay1VO.IVFCycleCount);
                    dbServer.AddInParameter(command, "SourceNeedleID", DbType.Int64, ObjDay1VO.SourceNeedleID);
                    dbServer.AddInParameter(command, "InfectionObserved", DbType.String, ObjDay1VO.InfectionObserved);
                    dbServer.AddInParameter(command, "TotNoOfOocytes", DbType.Int32, ObjDay1VO.TotNoOfOocytes);
                    dbServer.AddInParameter(command, "TotNoOf2PN", DbType.Int32, ObjDay1VO.TotNoOf2PN);
                    dbServer.AddInParameter(command, "TotNoOf3PN", DbType.Int32, ObjDay1VO.TotNoOf3PN);
                    dbServer.AddInParameter(command, "TotNoOf2PB", DbType.Int32, ObjDay1VO.TotNoOf2PB);
                    dbServer.AddInParameter(command, "ToNoOfMI", DbType.Int32, ObjDay1VO.ToNoOfMI);
                    dbServer.AddInParameter(command, "ToNoOfMII", DbType.Int32, ObjDay1VO.ToNoOfMII);
                    dbServer.AddInParameter(command, "ToNoOfGV", DbType.Int32, ObjDay1VO.ToNoOfGV);
                    dbServer.AddInParameter(command, "ToNoOfDeGenerated", DbType.Int32, ObjDay1VO.ToNoOfDeGenerated);
                    dbServer.AddInParameter(command, "ToNoOfLost ", DbType.Int32, ObjDay1VO.ToNoOfLost);

                    dbServer.AddInParameter(command, "IsFreezed  ", DbType.Boolean, ObjDay1VO.IsFreezed);
                    dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    int intStatus = dbServer.ExecuteNonQuery(command, trans);


                    if (BizActionObj.Day2Details.FertilizationAssesmentDetails != null && BizActionObj.Day2Details.FertilizationAssesmentDetails.Count > 0)
                    {
                        DbCommand command11 = dbServer.GetStoredProcCommand("CIMS_DeleteLab2Details");

                        dbServer.AddInParameter(command11, "Day2ID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command11, "LabDay", DbType.Int32, IVFLabDay.Day2);
                        int intStatus2 = dbServer.ExecuteNonQuery(command11, trans);
                    }

                    if (BizActionObj.Day2Details.FertilizationAssesmentDetails != null && BizActionObj.Day2Details.FertilizationAssesmentDetails.Count > 0)
                    {
                        List<clsEmbryoTransferDetailsVO> ETForwardedDetails = new List<clsEmbryoTransferDetailsVO>();

                        foreach (var item1 in ObjDay1VO.FertilizationAssesmentDetails)
                        {
                            DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay2Details");

                            dbServer.AddInParameter(command2, "Day2ID", DbType.Int64, ObjDay1VO.ID);
                            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "OoNo", DbType.Int64, item1.OoNo);
                            //By Anjali..........
                            dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, item1.SerialOccyteNo);
                            //...................
                            dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, item1.PlanTreatmentID);
                            dbServer.AddInParameter(command2, "Date", DbType.DateTime, item1.Date);
                            dbServer.AddInParameter(command2, "Time", DbType.DateTime, item1.Time);
                            dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, item1.SrcOocyteID);
                            dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, item1.OocyteDonorID);
                            dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, item1.SrcOfSemen);
                            dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, item1.SemenDonorID);
                            dbServer.AddInParameter(command2, "FileName", DbType.String, item1.FileName);
                            dbServer.AddInParameter(command2, "FileContents", DbType.Binary, item1.FileContents);


                            if (item1.SelectedFePlan != null)
                                dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, item1.SelectedFePlan.ID);
                            else
                                dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);

                            if (item1.SelectedGrade != null)
                                dbServer.AddInParameter(command2, "Grade", DbType.Int64, item1.SelectedGrade.ID);
                            else
                                dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);
                            if (item1.SelectedFragmentation != null)
                                dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, item1.SelectedFragmentation.ID);
                            else
                                dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, 0);

                            if (item1.SelectedBlastomereSymmetry != null)
                                dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, item1.SelectedBlastomereSymmetry.ID);
                            else
                                dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, 0);

                            dbServer.AddInParameter(command2, "Score", DbType.Int64, item1.Score);
                            dbServer.AddInParameter(command2, "PV", DbType.Boolean, item1.PV);
                            dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, item1.XFactor);
                            dbServer.AddInParameter(command2, "Others", DbType.String, item1.Others);
                            //dbServer.AddInParameter(command2, "ProceedDay3", DbType.Boolean, item1.ProceedDay3);
                            if (item1.SelectedPlan != null)
                                dbServer.AddInParameter(command2, "PlanID", DbType.Int64, item1.SelectedPlan.ID);
                            else
                                dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);
                            if (item1.SelectedPlan.ID == 3)
                            {
                                dbServer.AddInParameter(command2, "ProceedDay3", DbType.Boolean, true);
                            }
                            else
                            {
                                dbServer.AddInParameter(command2, "ProceedDay3", DbType.Boolean, false);
                            }
                            
                            dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);

                            int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                            item1.ID = (long)dbServer.GetParameterValue(command2, "ID");

                            //if (ObjDay1VO.IsFreezed == true)
                            //{
                            //    if (item1.SelectedPlan != null && item1.SelectedGrade.ID == 4)
                            //    {
                            //        clsEmbryoTransferDetailsVO obj = new clsEmbryoTransferDetailsVO();
                            //        obj.TransferDate = item1.Date;
                            //        obj.TransferDay = IVFLabDay.Day2;
                            //        obj.RecID = item1.ID;
                            //        obj.EmbryoNumber = item1.OoNo;
                            //        if (item1.SelectedGrade != null)
                            //            obj.GradeID = item1.SelectedGrade.ID;
                            //        else
                            //            obj.GradeID = 0;
                            //        obj.Score = Convert.ToInt32(item1.Score);
                            //        obj.FertilizationStageID = item1.FertilisationStage;

                            //        ETForwardedDetails.Add(obj);
                            //    }
                            //}

                            if (item1.MediaDetails != null && item1.MediaDetails.Count > 0)
                            {
                                foreach (var item3 in item1.MediaDetails)
                                {
                                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");

                                    dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                                    dbServer.AddInParameter(command4, "DetailID", DbType.Int64, item1.ID);
                                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command4, "Date", DbType.DateTime, item3.Date);
                                    dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day2);
                                    dbServer.AddInParameter(command4, "MediaName", DbType.String, item3.ItemName);
                                    dbServer.AddInParameter(command4, "Company", DbType.String, item3.Company);
                                    dbServer.AddInParameter(command4, "LotNo", DbType.String, item3.BatchCode);
                                    dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, item3.ExpiryDate);
                                    dbServer.AddInParameter(command4, "PH", DbType.Boolean, item3.PH);
                                    dbServer.AddInParameter(command4, "OSM", DbType.Boolean, item3.OSM);
                                    dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, item3.VolumeUsed);
                                    dbServer.AddInParameter(command4, "Status", DbType.Int64, item3.SelectedStatus.ID);
                                    dbServer.AddInParameter(command4, "BatchID", DbType.Int64, item3.BatchID);
                                    dbServer.AddInParameter(command4, "StoreID", DbType.Int64, item3.StoreID);  

                                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                    item3.ID = (long)dbServer.GetParameterValue(command4, "ID");
                                }
                            }
                            if (item1.Day2CalculateDetails != null)
                            {
                                var item6 = item1.Day2CalculateDetails;
                                
                                    DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay2CalculateDetails");

                                    dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                                    dbServer.AddInParameter(command5, "DetailID", DbType.Int64, item1.ID);
                                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                                    dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);
                                    dbServer.AddInParameter(command5, "ZonaThickness", DbType.Int32, item6.ZonaThickness);
                                    dbServer.AddInParameter(command5, "ZonaTexture", DbType.Int32, item6.ZonaTexture);
                                    dbServer.AddInParameter(command5, "BlastomereSize", DbType.Int32, item6.BlastomereSize);
                                    dbServer.AddInParameter(command5, "BlastomereShape", DbType.Int32, item6.BlastomereShape);
                                    dbServer.AddInParameter(command5, "BlastomeresVolume", DbType.Int32, item6.BlastomeresVolume);
                                    dbServer.AddInParameter(command5, "Membrane", DbType.Int32, item6.Membrane);
                                    dbServer.AddInParameter(command5, "Cytoplasm", DbType.Int32, item6.Cytoplasm);
                                    dbServer.AddInParameter(command5, "Fragmentation", DbType.Int32, item6.Fragmentation);
                                    dbServer.AddInParameter(command5, "DevelopmentRate", DbType.Int32, item6.DevelopmentRate);
                                    dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item6.ID);
                                    dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);

                                    int intStatus1 = dbServer.ExecuteNonQuery(command5, trans);
                                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command5, "ResultStatus");
                                    item6.ID = (long)dbServer.GetParameterValue(command5, "ID");
                                
                            }
                        }

                        //if (BizActionObj.Day2Details.IsFreezed == true && ETForwardedDetails.Count > 0)
                        //{
                        //    clsBaseIVFEmbryoTransferDAL objBaseDAL = clsBaseIVFEmbryoTransferDAL.GetInstance();
                        //    clsAddForwardedEmbryoTransferBizActionVO BizActionFrwrd = new clsAddForwardedEmbryoTransferBizActionVO();
                        //    BizActionFrwrd.UnitID = BizActionObj.Day2Details.UnitID;
                        //    BizActionFrwrd.CoupleID = BizActionObj.Day2Details.CoupleID;
                        //    BizActionFrwrd.CoupleUnitID = BizActionObj.Day2Details.CoupleUnitID;
                        //    BizActionFrwrd.ForwardedEmbryos = ETForwardedDetails;

                        //    BizActionFrwrd = (clsAddForwardedEmbryoTransferBizActionVO)objBaseDAL.AddForwardedEmbryos(BizActionFrwrd, UserVo, con, trans);

                        //    if (BizActionFrwrd.SuccessStatus == -1) throw new Exception();
                        //}
                    }

                    if (BizActionObj.Day2Details.FUSetting != null && BizActionObj.Day2Details.FUSetting.Count > 0)
                    {
                        DbCommand command13 = dbServer.GetStoredProcCommand("CIMS_DeleteLab2UploadFileDetails");

                        dbServer.AddInParameter(command13, "Day2ID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command13, "LabDay", DbType.Int32, IVFLabDay.Day2);
                        dbServer.AddInParameter(command13, "UnitID", DbType.Int64, ObjDay1VO.UnitID);
                        int intStatus2 = dbServer.ExecuteNonQuery(command13, trans);
                    }
                    if (BizActionObj.Day2Details.FUSetting != null && BizActionObj.Day2Details.FUSetting.Count > 0)
                    {
                        foreach (var item2 in ObjDay1VO.FUSetting)
                        {
                            DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                            dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day2);
                            dbServer.AddInParameter(command3, "FileName", DbType.String, item2.FileName);
                            dbServer.AddInParameter(command3, "FileIndex", DbType.Int32, item2.Index);
                            dbServer.AddInParameter(command3, "Value", DbType.Binary, item2.Data);
                            dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                            dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                            item2.ID = (long)dbServer.GetParameterValue(command3, "ID");
                        }
                    }


                    if (BizActionObj.Day2Details.SemenDetails != null)
                    {
                        DbCommand command12 = dbServer.GetStoredProcCommand("CIMS_DeleteLab2SemenDetails");

                        dbServer.AddInParameter(command12, "Day2ID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command12, "LabDay", DbType.Int32, IVFLabDay.Day2);
                        int intStatus2 = dbServer.ExecuteNonQuery(command12, trans);
                    }
                    if (BizActionObj.Day2Details.SemenDetails != null)
                    {
                        var item4 = BizActionObj.Day2Details.SemenDetails;
                        DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");

                        dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day2);
                        dbServer.AddInParameter(command4, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day2Details.SemenDetails.MethodOfSpermPreparation);
                        dbServer.AddInParameter(command4, "SourceOfSemen", DbType.Int64, BizActionObj.Day2Details.SemenDetails.SourceOfSemen);
                        dbServer.AddInParameter(command4, "PreSelfVolume", DbType.String, item4.PreSelfVolume);
                        dbServer.AddInParameter(command4, "PreSelfConcentration", DbType.String, item4.PreSelfConcentration);
                        dbServer.AddInParameter(command4, "PreSelfMotality", DbType.String, item4.PreSelfMotality);
                        dbServer.AddInParameter(command4, "PreSelfWBC", DbType.String, item4.PreSelfWBC);
                        dbServer.AddInParameter(command4, "PreDonorVolume", DbType.String, item4.PreDonorVolume);
                        dbServer.AddInParameter(command4, "PreDonorConcentration", DbType.String, item4.PreDonorConcentration);
                        dbServer.AddInParameter(command4, "PreDonorMotality", DbType.String, item4.PreDonorMotality);
                        dbServer.AddInParameter(command4, "PreDonorWBC", DbType.String, item4.PreDonorWBC);
                        dbServer.AddInParameter(command4, "PostSelfVolume", DbType.String, item4.PostSelfVolume);
                        dbServer.AddInParameter(command4, "PostSelfConcentration", DbType.String, item4.PostSelfConcentration);
                        dbServer.AddInParameter(command4, "PostSelfMotality", DbType.String, item4.PostSelfMotality);
                        dbServer.AddInParameter(command4, "PostSelfWBC", DbType.String, item4.PostSelfWBC);
                        dbServer.AddInParameter(command4, "PostDonorVolume ", DbType.String, item4.PostDonorVolume);
                        dbServer.AddInParameter(command4, "PostDonorConcentration", DbType.String, item4.PostDonorConcentration);
                        dbServer.AddInParameter(command4, "PostDonorMotality", DbType.String, item4.PostDonorMotality);
                        dbServer.AddInParameter(command4, "PostDonorWBC", DbType.String, item4.PostDonorWBC);
                        dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item4.ID);
                        dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                        item4.ID = (long)dbServer.GetParameterValue(command4, "ID");
                    }





                    if (BizActionObj.Day2Details.LabDaySummary != null)
                    {
                        clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                        clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                        obj.LabDaysSummary = BizActionObj.Day2Details.LabDaySummary;
                        obj.LabDaysSummary.OocyteID = BizActionObj.Day2Details.ID;
                        obj.LabDaysSummary.CoupleID = BizActionObj.Day2Details.CoupleID;
                        obj.LabDaysSummary.CoupleUnitID = BizActionObj.Day2Details.CoupleUnitID;
                        obj.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay2;
                        obj.LabDaysSummary.Priority = 1;
                        obj.LabDaysSummary.ProcDate = BizActionObj.Day2Details.Date;
                        obj.LabDaysSummary.ProcTime = BizActionObj.Day2Details.Time;
                        obj.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                        obj.LabDaysSummary.IsFreezed = BizActionObj.Day2Details.IsFreezed;


                        obj.IsUpdate = true;

                        obj = (clsAddUpdateLabDaysSummaryBizActionVO)objBaseDAL.AddUpdateLabDaysSummary(obj, UserVo, con, trans);
                        if (obj.SuccessStatus == -1) throw new Exception();
                        BizActionObj.Day2Details.LabDaySummary.ID = obj.LabDaysSummary.ID;
                    }
                

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
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

        public override IValueObject GetLabDay1ForDay2(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLabDay1ForLabDay2BizActionVO BizActionObj = valueObject as clsGetLabDay1ForLabDay2BizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFemaleLabDay1ForLabDay2");
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.CoupleUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Day2Details == null)
                        BizActionObj.Day2Details = new List<clsFemaleLabDay2FertilizationAssesmentVO>();
                    while (reader.Read())
                    {
                        clsFemaleLabDay2FertilizationAssesmentVO Obj = new clsFemaleLabDay2FertilizationAssesmentVO();

                        Obj.ID = (long)DALHelper.HandleDBNull(reader["DetailID"]);
                        Obj.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);

                        //By Anjali.............
                        Obj.SerialOccyteNo = (long)DALHelper.HandleDBNull(reader["SerialOccyteNo"]);

                        //................
                        Obj.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        Obj.Time = (DateTime?)DALHelper.HandleDate(reader["Time"]);
                        Obj.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);
                        Obj.PlanTreatment = (string)DALHelper.HandleDBNull(reader["Protocol"]);
                        Obj.SrcOocyteID = (long)DALHelper.HandleDBNull(reader["SrcOocyteID"]);
                        Obj.SrcOocyteDescription = (string)DALHelper.HandleDBNull(reader["SOOocyte"]);
                        Obj.OocyteDonorID = (string)DALHelper.HandleDBNull(reader["OSCode"]);                        
                        Obj.SrcOfSemen = (long)DALHelper.HandleDBNull(reader["SrcOfSemen"]);
                        Obj.SrcOfSemenDescription = (string)DALHelper.HandleDBNull(reader["SOSemen"]);
                        Obj.SemenDonorID = (string)DALHelper.HandleDBNull(reader["SSCode"]);                    

                        BizActionObj.Day2Details.Add(Obj);

                        BizActionObj.AnaesthetistID = (long)DALHelper.HandleDBNull(reader["AnasthesistID"]);
                        BizActionObj.AssAnaesthetistID = (long)DALHelper.HandleDBNull(reader["AssAnasthesistID"]);
                        BizActionObj.AssEmbryologistID = (long)DALHelper.HandleDBNull(reader["AssEmbryologistID"]);
                        BizActionObj.EmbryologistID = (long)DALHelper.HandleDBNull(reader["EmbryologistID"]);
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
        
        public override IValueObject GetFemaleLabDay2(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay2DetailsBizActionVO BizAction = (valueObject) as clsGetDay2DetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleLabDay2");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizAction.CoupleUnitID);
                dbServer.AddInParameter(command, "LabDay", DbType.Int16, IVFLabDay.Day2);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.LabDay2 == null)
                        BizAction.LabDay2 = new clsFemaleLabDay2VO();
                    while (reader.Read())
                    {


                        BizAction.LabDay2.ID = (long)(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.LabDay2.UnitID = (long)(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.LabDay2.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        BizAction.LabDay2.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));

                        BizAction.LabDay2.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        BizAction.LabDay2.Time = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                        BizAction.LabDay2.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.LabDay2.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        BizAction.LabDay2.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        BizAction.LabDay2.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        BizAction.LabDay2.SourceNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SourceNeedleID"]));
                        BizAction.LabDay2.InfectionObserved = (string)(DALHelper.HandleDBNull(reader["InfectionObserved"]));

                        BizAction.LabDay2.TotNoOfOocytes = (int)(DALHelper.HandleDBNull(reader["TotNoOfOocytes"]));
                        BizAction.LabDay2.TotNoOf2PN = (int)(DALHelper.HandleDBNull(reader["TotNoOf2PN"]));
                        BizAction.LabDay2.TotNoOf3PN = (int)(DALHelper.HandleDBNull(reader["TotNoOf3PN"]));
                        BizAction.LabDay2.TotNoOf2PB = (int)(DALHelper.HandleDBNull(reader["TotNoOf2PB"]));
                        BizAction.LabDay2.ToNoOfMI = (int)(DALHelper.HandleDBNull(reader["ToNoOfMI"]));
                        BizAction.LabDay2.ToNoOfMII = (int)(DALHelper.HandleDBNull(reader["ToNoOfMII"]));
                        BizAction.LabDay2.ToNoOfGV = (int)(DALHelper.HandleDBNull(reader["ToNoOfGV"]));
                        BizAction.LabDay2.ToNoOfDeGenerated = (int)(DALHelper.HandleDBNull(reader["ToNoOfDeGenerated"]));
                        BizAction.LabDay2.ToNoOfLost = (int)(DALHelper.HandleDBNull(reader["ToNoOfLost"]));
                        BizAction.LabDay2.IsFreezed = (bool)(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        BizAction.LabDay2.Status = (bool)(DALHelper.HandleDBNull(reader["Status"]));

                    }
                   
                    reader.NextResult();
                    if (BizAction.LabDay2.FertilizationAssesmentDetails == null)
                        BizAction.LabDay2.FertilizationAssesmentDetails = new List<clsFemaleLabDay2FertilizationAssesmentVO>();


                    while (reader.Read())
                    {
                        clsFemaleLabDay2FertilizationAssesmentVO ObjFer = new clsFemaleLabDay2FertilizationAssesmentVO();
                        ObjFer.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ObjFer.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);
                        //By Anjali.............
                        ObjFer.SerialOccyteNo = (long)DALHelper.HandleDBNull(reader["SerialOccyteNo"]);

                        //................
                        ObjFer.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        ObjFer.Time = (DateTime?)DALHelper.HandleDate(reader["Time"]);

                        ObjFer.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);
                        ObjFer.PlanTreatment = (string)DALHelper.HandleDBNull(reader["Protocol"]);

                        ObjFer.SrcOocyteID = (long)DALHelper.HandleDBNull(reader["SrcOocyteID"]);
                        ObjFer.SrcOocyteDescription = (string)DALHelper.HandleDBNull(reader["SOOocyte"]);

                        ObjFer.OocyteDonorID = (string)DALHelper.HandleDBNull(reader["OSCode"]);
                      

                        ObjFer.SrcOfSemen = (long)DALHelper.HandleDBNull(reader["SrcOfSemen"]);
                        ObjFer.SrcOfSemenDescription = (string)DALHelper.HandleDBNull(reader["SOSemen"]);

                        ObjFer.SemenDonorID = (string)DALHelper.HandleDBNull(reader["SSCode"]);
                      

                        ObjFer.SelectedFePlan.ID = (long)(DALHelper.HandleDBNull(reader["FertilisationStage"]));
                        ObjFer.SelectedFePlan.Description = (string)(DALHelper.HandleDBNull(reader["Fertilization"]));

                        ObjFer.SelectedFragmentation.ID = (long)(DALHelper.HandleDBNull(reader["FragmentationID"]));
                        ObjFer.SelectedFragmentation.Description = (string)(DALHelper.HandleDBNull(reader["Fragmentation"]));

                        ObjFer.SelectedBlastomereSymmetry.ID = (long)(DALHelper.HandleDBNull(reader["BlastomereSymmetryID"]));
                        ObjFer.SelectedBlastomereSymmetry.Description = (string)(DALHelper.HandleDBNull(reader["BlastomereSymmetry"]));
            
                        ObjFer.Score = (long)(DALHelper.HandleDBNull(reader["Score"]));
                        ObjFer.PV = (bool)(DALHelper.HandleDBNull(reader["PV"]));
                        ObjFer.XFactor = (bool)(DALHelper.HandleDBNull(reader["XFactor"]));
                        ObjFer.Others = (string)(DALHelper.HandleDBNull(reader["Others"]));
                        ObjFer.ProceedDay3 = (bool)(DALHelper.HandleDBNull(reader["ProceedDay3"]));
                        
                        ObjFer.SelectedGrade.ID = (long)(DALHelper.HandleDBNull(reader["GradeID"]));
                        ObjFer.SelectedGrade.Description = (string)(DALHelper.HandleDBNull(reader["Grade"]));

                        ObjFer.SelectedPlan.ID = (long)(DALHelper.HandleDBNull(reader["PlanID"]));
                        ObjFer.SelectedPlan.Description = (string)(DALHelper.HandleDBNull(reader["PlanName"]));
                        ObjFer.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ObjFer.FileContents = (byte[])(DALHelper.HandleDBNull(reader["FileContents"]));


                        //For Getting Details
                        clsBaseIVFLabDayDAL objBaseDAL = clsBaseIVFLabDayDAL.GetInstance();
                        clsGetDay2MediaAndCalcDetailsBizActionVO BizActionDetails = new clsGetDay2MediaAndCalcDetailsBizActionVO();

                        BizActionDetails.ID = BizAction.ID;
                        BizActionDetails.DetailID = ObjFer.ID;
                        BizActionDetails = (clsGetDay2MediaAndCalcDetailsBizActionVO)objBaseDAL.GetFemaleLabDay2MediaAndCalDetails(BizActionDetails, UserVo);
                        ObjFer.Day2CalculateDetails = BizActionDetails.LabDayCalDetails;


                        //For getting Media details
                        clsBaseIVFLabDayDAL objBaseDAL2 = clsBaseIVFLabDayDAL.GetInstance();
                        clsGetAllDayMediaDetailsBizActionVO BizActionMedia = new clsGetAllDayMediaDetailsBizActionVO();
                        BizActionMedia.ID = BizAction.ID;
                        BizActionMedia.DetailID = ObjFer.ID;
                        BizActionMedia.LabDay = 2;
                        BizActionMedia = (clsGetAllDayMediaDetailsBizActionVO)objBaseDAL2.GetAllDayMediaDetails(BizActionMedia, UserVo);
                        ObjFer.MediaDetails = BizActionMedia.MediaList;


                        BizAction.LabDay2.FertilizationAssesmentDetails.Add(ObjFer);

                    }
                    reader.NextResult();
                    if (BizAction.LabDay2.SemenDetails == null)
                        BizAction.LabDay2.SemenDetails = new clsFemaleSemenDetailsVO();


                    while (reader.Read())
                    {
                        BizAction.LabDay2.SemenDetails.MOSP = (string)(DALHelper.HandleDBNull(reader["MOSP"]));
                        BizAction.LabDay2.SemenDetails.SOS = (string)(DALHelper.HandleDBNull(reader["SOS"]));

                        BizAction.LabDay2.SemenDetails.MethodOfSpermPreparation = (long)(DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]));
                        BizAction.LabDay2.SemenDetails.SourceOfSemen = (long)(DALHelper.HandleDBNull(reader["SourceOfSemen"]));

                        BizAction.LabDay2.SemenDetails.PreSelfVolume = (string)(DALHelper.HandleDBNull(reader["PreSelfVolume"]));
                        BizAction.LabDay2.SemenDetails.PreSelfConcentration = (string)(DALHelper.HandleDBNull(reader["PreSelfConcentration"]));
                        BizAction.LabDay2.SemenDetails.PreSelfMotality = (string)(DALHelper.HandleDBNull(reader["PreSelfMotality"]));
                        BizAction.LabDay2.SemenDetails.PreSelfWBC = (string)(DALHelper.HandleDBNull(reader["PreSelfWBC"]));

                        BizAction.LabDay2.SemenDetails.PreDonorVolume = (string)(DALHelper.HandleDBNull(reader["PreDonorVolume"]));
                        BizAction.LabDay2.SemenDetails.PreDonorConcentration = (string)(DALHelper.HandleDBNull(reader["PreDonorConcentration"]));
                        BizAction.LabDay2.SemenDetails.PreDonorMotality = (string)(DALHelper.HandleDBNull(reader["PreDonorMotality"]));
                        BizAction.LabDay2.SemenDetails.PreDonorWBC = (string)(DALHelper.HandleDBNull(reader["PreDonorWBC"]));


                        BizAction.LabDay2.SemenDetails.PostSelfVolume = (string)(DALHelper.HandleDBNull(reader["PostSelfVolume"]));
                        BizAction.LabDay2.SemenDetails.PostSelfConcentration = (string)(DALHelper.HandleDBNull(reader["PostSelfConcentration"]));
                        BizAction.LabDay2.SemenDetails.PostSelfMotality = (string)(DALHelper.HandleDBNull(reader["PostSelfMotality"]));
                        BizAction.LabDay2.SemenDetails.PostSelfWBC = (string)(DALHelper.HandleDBNull(reader["PostSelfWBC"]));

                        BizAction.LabDay2.SemenDetails.PostDonorVolume = (string)(DALHelper.HandleDBNull(reader["PostDonorVolume"]));
                        BizAction.LabDay2.SemenDetails.PostDonorConcentration = (string)(DALHelper.HandleDBNull(reader["PostDonorConcentration"]));
                        BizAction.LabDay2.SemenDetails.PostDonorMotality = (string)(DALHelper.HandleDBNull(reader["PostDonorMotality"]));
                        BizAction.LabDay2.SemenDetails.PostDonorWBC = (string)(DALHelper.HandleDBNull(reader["PostDonorWBC"]));
                    }

                    reader.NextResult();
                    if (BizAction.LabDay2.FUSetting == null)
                        BizAction.LabDay2.FUSetting = new List<FileUpload>();
                    while (reader.Read())
                    {
                        FileUpload ObjFile = new FileUpload();
                        ObjFile.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ObjFile.Index = (Int32)(DALHelper.HandleDBNull(reader["FileIndex"]));
                        ObjFile.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ObjFile.Data = (byte[])(DALHelper.HandleDBNull(reader["Value"]));
                        BizAction.LabDay2.FUSetting.Add(ObjFile);
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
        
        public override IValueObject GetFemaleLabDay2MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay2MediaAndCalcDetailsBizActionVO BizAction = (valueObject) as clsGetDay2MediaAndCalcDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMediaDetailsAndCalculateDetailForDay2");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "DetailID", DbType.Int64, BizAction.DetailID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.LabDayCalDetails == null)
                        BizAction.LabDayCalDetails = new clsFemaleLabDay2CalculateDetailsVO();

                    while (reader.Read())
                    {

                        BizAction.LabDayCalDetails.FertilizationID = (long)(DALHelper.HandleDBNull(reader["DetailID"]));
                        BizAction.LabDayCalDetails.ZonaThickness = (Int16)(DALHelper.HandleDBNull(reader["ZonaThickness"]));
                        BizAction.LabDayCalDetails.ZonaTexture = (Int16)(DALHelper.HandleDBNull(reader["ZonaTexture"]));
                        BizAction.LabDayCalDetails.BlastomereSize = (Int16)(DALHelper.HandleDBNull(reader["BlastomereSize"]));
                        BizAction.LabDayCalDetails.BlastomereShape = (Int16)(DALHelper.HandleDBNull(reader["BlastomereShape"]));
                        BizAction.LabDayCalDetails.BlastomeresVolume = (Int16)(DALHelper.HandleDBNull(reader["BlastomeresVolume"]));
                        BizAction.LabDayCalDetails.Membrane = (Int16)(DALHelper.HandleDBNull(reader["Membrane"]));
                        BizAction.LabDayCalDetails.Cytoplasm = (Int16)(DALHelper.HandleDBNull(reader["Cytoplasm"]));
                        BizAction.LabDayCalDetails.Fragmentation = (Int16)(DALHelper.HandleDBNull(reader["Fragmentation"]));
                        BizAction.LabDayCalDetails.DevelopmentRate = (Int16)(DALHelper.HandleDBNull(reader["DevelopmentRate"]));
                    
                        
                    }

                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizAction;

        }


        public override IValueObject GetLabDay1Score(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay1ScoreBizActionVO BizActionObj = valueObject as clsGetDay1ScoreBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFemaleDay1Score");

                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.CoupleUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Day0Score == null)
                        BizActionObj.Day0Score = new List<clsFemaleLabDay1FertilizationAssesmentVO>();
                    while (reader.Read())
                    {
                        clsFemaleLabDay1FertilizationAssesmentVO Obj = new clsFemaleLabDay1FertilizationAssesmentVO();

                        Obj.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);
                        Obj.Score = (long)DALHelper.HandleDBNull(reader["Score"]);
                        Obj.SelectedGrade.Description = (string)DALHelper.HandleDBNull(reader["Grade"]);
                        Obj.SelectedFePlan.Description = (string)DALHelper.HandleDBNull(reader["FertilisationStage"]);
                        Obj.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);

                        BizActionObj.Day0Score.Add(Obj);
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

        #region Day 3

        
        public override IValueObject AddLabDay3(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddLabDay2BizActionVO BizActionObj = valueObject as clsAddLabDay2BizActionVO;

            if (BizActionObj.Day2Details.ID == 0)
                BizActionObj = AddDay3(BizActionObj, UserVo);
            else
                BizActionObj = UpdateDay3(BizActionObj, UserVo);

            return valueObject; 

        }

        private clsAddLabDay2BizActionVO AddDay3(clsAddLabDay2BizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsFemaleLabDay2VO ObjDay1VO = BizActionObj.Day2Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay3");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, ObjDay1VO.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, ObjDay1VO.CoupleUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjDay1VO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, ObjDay1VO.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, ObjDay1VO.EmbryologistID);
                dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, ObjDay1VO.AssEmbryologistID);
                dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, ObjDay1VO.AnesthetistID);
                dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, ObjDay1VO.AssAnesthetistID);
                dbServer.AddInParameter(command, "IVFCycleCount", DbType.Int64, ObjDay1VO.IVFCycleCount);
                dbServer.AddInParameter(command, "SourceNeedleID", DbType.Int64, ObjDay1VO.SourceNeedleID);
                dbServer.AddInParameter(command, "InfectionObserved", DbType.String, ObjDay1VO.InfectionObserved);
                dbServer.AddInParameter(command, "TotNoOfOocytes", DbType.Int32, ObjDay1VO.TotNoOfOocytes);
                dbServer.AddInParameter(command, "TotNoOf2PN", DbType.Int32, ObjDay1VO.TotNoOf2PN);
                dbServer.AddInParameter(command, "TotNoOf3PN", DbType.Int32, ObjDay1VO.TotNoOf3PN);
                dbServer.AddInParameter(command, "TotNoOf2PB", DbType.Int32, ObjDay1VO.TotNoOf2PB);
                dbServer.AddInParameter(command, "ToNoOfMI", DbType.Int32, ObjDay1VO.ToNoOfMI);
                dbServer.AddInParameter(command, "ToNoOfMII", DbType.Int32, ObjDay1VO.ToNoOfMII);
                dbServer.AddInParameter(command, "ToNoOfGV", DbType.Int32, ObjDay1VO.ToNoOfGV);
                dbServer.AddInParameter(command, "ToNoOfDeGenerated", DbType.Int32, ObjDay1VO.ToNoOfDeGenerated);
                dbServer.AddInParameter(command, "ToNoOfLost ", DbType.Int32, ObjDay1VO.ToNoOfLost);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, ObjDay1VO.IsFreezed);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDay1VO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Day2Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.Day2Details.FertilizationAssesmentDetails != null && BizActionObj.Day2Details.FertilizationAssesmentDetails.Count > 0)
                {
                    List<clsEmbryoTransferDetailsVO> ETForwardedDetails = new List<clsEmbryoTransferDetailsVO>();

                    foreach (var item1 in ObjDay1VO.FertilizationAssesmentDetails)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay3Details");
                        
                        dbServer.AddInParameter(command2, "Day3ID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "OoNo", DbType.Int64, item1.OoNo);
                        //By Anjali..........
                        dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, item1.SerialOccyteNo);
                        //...................
                        dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, item1.PlanTreatmentID);
                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, item1.Date);
                        dbServer.AddInParameter(command2, "Time", DbType.DateTime, item1.Time);
                        dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, item1.SrcOocyteID);
                        dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, item1.OocyteDonorID);
                        dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, item1.SrcOfSemen);
                        dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, item1.SemenDonorID);
                        dbServer.AddInParameter(command2, "FileName", DbType.String, item1.FileName);
                        dbServer.AddInParameter(command2, "FileContents", DbType.Binary, item1.FileContents);

                        if (item1.SelectedFePlan != null)
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, item1.SelectedFePlan.ID);
                        else
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);
                        if (item1.SelectedGrade != null)
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, item1.SelectedGrade.ID);
                        else
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);

                        if (item1.SelectedFragmentation != null)
                            dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, item1.SelectedFragmentation.ID);
                        else
                            dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, 0);
                        if (item1.SelectedBlastomereSymmetry != null)
                            dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, item1.SelectedBlastomereSymmetry.ID);
                        else
                            dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, 0);
                        dbServer.AddInParameter(command2, "Score", DbType.Int64, item1.Score);
                        dbServer.AddInParameter(command2, "PV", DbType.Boolean, item1.PV);
                        dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, item1.XFactor);
                        dbServer.AddInParameter(command2, "Others", DbType.String, item1.Others);
                        //dbServer.AddInParameter(command2, "ProceedDay4", DbType.Boolean, item1.ProceedDay3);
                        if (item1.SelectedPlan != null)
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, item1.SelectedPlan.ID);
                        else
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);

                        if (item1.SelectedPlan.ID == 3)
                        {
                            dbServer.AddInParameter(command2, "ProceedDay4", DbType.Boolean, true);
                        }
                        else
                        {
                            dbServer.AddInParameter(command2, "ProceedDay4", DbType.Boolean, false);
                        }
                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);
                        
                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                        item1.ID = (long)dbServer.GetParameterValue(command2, "ID");
                        // Forward Embryos
                        //if (ObjDay1VO.IsFreezed == true)
                        //{
                        //    if (item1.SelectedPlan != null && item1.SelectedGrade.ID == 4)
                        //    {
                        //        clsEmbryoTransferDetailsVO obj = new clsEmbryoTransferDetailsVO();
                        //        obj.TransferDate = item1.Date;
                        //        obj.TransferDay = IVFLabDay.Day2;
                        //        obj.RecID = item1.ID;
                        //        obj.EmbryoNumber = item1.OoNo;
                        //        if (item1.SelectedGrade != null)
                        //            obj.GradeID = item1.SelectedGrade.ID;
                        //        else
                        //            obj.GradeID = 0;
                        //        obj.Score = Convert.ToInt32(item1.Score);
                        //        obj.FertilizationStageID = item1.FertilisationStage;
                        //        ETForwardedDetails.Add(obj);
                        //    }
                        //}

                        if (item1.MediaDetails != null && item1.MediaDetails.Count > 0)
                        {
                            foreach (var item3 in item1.MediaDetails)
                            {
                                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                                dbServer.AddInParameter(command4, "DetailID", DbType.Int64, item1.ID);
                                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command4, "Date", DbType.DateTime, DateTime.Now);
                                dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day3);
                                dbServer.AddInParameter(command4, "MediaName", DbType.String, item3.ItemName);
                                dbServer.AddInParameter(command4, "Company", DbType.String, item3.Company);
                                dbServer.AddInParameter(command4, "LotNo", DbType.String, item3.BatchCode);
                                dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, item3.ExpiryDate);
                                dbServer.AddInParameter(command4, "PH", DbType.Boolean, item3.PH);
                                dbServer.AddInParameter(command4, "OSM", DbType.Boolean, item3.OSM);
                                dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, item3.VolumeUsed);
                                dbServer.AddInParameter(command4, "Status", DbType.Int64, item3.SelectedStatus.ID);
                                dbServer.AddInParameter(command4, "BatchID", DbType.Int64, item3.BatchID);
                                dbServer.AddInParameter(command4, "StoreID", DbType.Int64, item3.StoreID);
                                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                                int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                item3.ID = (long)dbServer.GetParameterValue(command4, "ID");
                            }
                        }
                        
                        if (item1.Day2CalculateDetails != null)
                        {
                            var item6 = item1.Day2CalculateDetails;
                            DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay3CalculateDetails");
                            dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                            dbServer.AddInParameter(command5, "DetailID", DbType.Int64, item1.ID);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command5, "ZonaThickness", DbType.Int32, item6.ZonaThickness);
                            dbServer.AddInParameter(command5, "ZonaTexture", DbType.Int32, item6.ZonaTexture);
                            dbServer.AddInParameter(command5, "BlastomereSize", DbType.Int32, item6.BlastomereSize);
                            dbServer.AddInParameter(command5, "BlastomereShape", DbType.Int32, item6.BlastomereShape);
                            dbServer.AddInParameter(command5, "BlastomeresVolume", DbType.Int32, item6.BlastomeresVolume);
                            dbServer.AddInParameter(command5, "Membrane", DbType.Int32, item6.Membrane);
                            dbServer.AddInParameter(command5, "Cytoplasm", DbType.Int32, item6.Cytoplasm);
                            dbServer.AddInParameter(command5, "Fragmentation", DbType.Int32, item6.Fragmentation);
                            dbServer.AddInParameter(command5, "DevelopmentRate", DbType.Int32, item6.DevelopmentRate);
                            dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item6.ID);
                            dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);
                            int intStatus1 = dbServer.ExecuteNonQuery(command5, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command5, "ResultStatus");
                            item6.ID = (long)dbServer.GetParameterValue(command5, "ID");
                        }
                    }

                    //if (BizActionObj.Day2Details.IsFreezed == true && ETForwardedDetails.Count > 0)
                    //{
                    //    clsBaseIVFEmbryoTransferDAL objBaseDAL = clsBaseIVFEmbryoTransferDAL.GetInstance();
                    //    clsAddForwardedEmbryoTransferBizActionVO BizActionFrwrd = new clsAddForwardedEmbryoTransferBizActionVO();
                    //    BizActionFrwrd.UnitID = BizActionObj.Day2Details.UnitID;
                    //    BizActionFrwrd.CoupleID = BizActionObj.Day2Details.CoupleID;
                    //    BizActionFrwrd.CoupleUnitID = BizActionObj.Day2Details.CoupleUnitID;
                    //    BizActionFrwrd.ForwardedEmbryos = ETForwardedDetails;

                    //    BizActionFrwrd = (clsAddForwardedEmbryoTransferBizActionVO)objBaseDAL.AddForwardedEmbryos(BizActionFrwrd, UserVo, con, trans);

                    //    if (BizActionFrwrd.SuccessStatus == -1) throw new Exception();
                    //}
                }
                
                if (BizActionObj.Day2Details.FUSetting != null && BizActionObj.Day2Details.FUSetting.Count > 0)
                {
                    foreach (var item2 in ObjDay1VO.FUSetting)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                        dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day3);
                        dbServer.AddInParameter(command3, "FileName", DbType.String, item2.FileName);
                        dbServer.AddInParameter(command3, "FileIndex", DbType.Int32, item2.Index);
                        dbServer.AddInParameter(command3, "Value", DbType.Binary, item2.Data);
                        dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                        dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                        item2.ID = (long)dbServer.GetParameterValue(command3, "ID");
                    }
                }
                               
                if (BizActionObj.Day2Details.SemenDetails != null)
                {
                    var item4 = BizActionObj.Day2Details.SemenDetails;
                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");
                    dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day3);
                    dbServer.AddInParameter(command4, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day2Details.SemenDetails.MethodOfSpermPreparation);
                    dbServer.AddInParameter(command4, "SourceOfSemen", DbType.Int64, BizActionObj.Day2Details.SemenDetails.SourceOfSemen);
                    dbServer.AddInParameter(command4, "PreSelfVolume", DbType.String, item4.PreSelfVolume);
                    dbServer.AddInParameter(command4, "PreSelfConcentration", DbType.String, item4.PreSelfConcentration);
                    dbServer.AddInParameter(command4, "PreSelfMotality", DbType.String, item4.PreSelfMotality);
                    dbServer.AddInParameter(command4, "PreSelfWBC", DbType.String, item4.PreSelfWBC);
                    dbServer.AddInParameter(command4, "PreDonorVolume", DbType.String, item4.PreDonorVolume);
                    dbServer.AddInParameter(command4, "PreDonorConcentration", DbType.String, item4.PreDonorConcentration);
                    dbServer.AddInParameter(command4, "PreDonorMotality", DbType.String, item4.PreDonorMotality);
                    dbServer.AddInParameter(command4, "PreDonorWBC", DbType.String, item4.PreDonorWBC);
                    dbServer.AddInParameter(command4, "PostSelfVolume", DbType.String, item4.PostSelfVolume);
                    dbServer.AddInParameter(command4, "PostSelfConcentration", DbType.String, item4.PostSelfConcentration);
                    dbServer.AddInParameter(command4, "PostSelfMotality", DbType.String, item4.PostSelfMotality);
                    dbServer.AddInParameter(command4, "PostSelfWBC", DbType.String, item4.PostSelfWBC);
                    dbServer.AddInParameter(command4, "PostDonorVolume ", DbType.String, item4.PostDonorVolume);
                    dbServer.AddInParameter(command4, "PostDonorConcentration", DbType.String, item4.PostDonorConcentration);
                    dbServer.AddInParameter(command4, "PostDonorMotality", DbType.String, item4.PostDonorMotality);
                    dbServer.AddInParameter(command4, "PostDonorWBC", DbType.String, item4.PostDonorWBC);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item4.ID);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    item4.ID = (long)dbServer.GetParameterValue(command4, "ID");
                }

          
                if (BizActionObj.Day2Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                    obj.LabDaysSummary = BizActionObj.Day2Details.LabDaySummary;
                    obj.LabDaysSummary.OocyteID = BizActionObj.Day2Details.ID;
                    obj.LabDaysSummary.CoupleID = BizActionObj.Day2Details.CoupleID;
                    obj.LabDaysSummary.CoupleUnitID = BizActionObj.Day2Details.CoupleUnitID;
                    obj.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay3;
                    obj.LabDaysSummary.Priority = 1;
                    obj.LabDaysSummary.ProcDate = BizActionObj.Day2Details.Date;
                    obj.LabDaysSummary.ProcTime = BizActionObj.Day2Details.Time;
                    obj.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    obj.LabDaysSummary.IsFreezed = BizActionObj.Day2Details.IsFreezed;
                    obj.IsUpdate = false;
                    obj = (clsAddUpdateLabDaysSummaryBizActionVO)objBaseDAL.AddUpdateLabDaysSummary(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Day2Details.LabDaySummary.ID = obj.LabDaysSummary.ID;
                }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
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

        private clsAddLabDay2BizActionVO UpdateDay3(clsAddLabDay2BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsFemaleLabDay2VO ObjDay1VO = BizActionObj.Day2Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateFemaleLabDay3");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjDay1VO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                ObjDay1VO.UnitID = UserVo.UserLoginInfo.UnitId;
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, ObjDay1VO.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, ObjDay1VO.CoupleUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjDay1VO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, ObjDay1VO.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, ObjDay1VO.EmbryologistID);
                dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, ObjDay1VO.AssEmbryologistID);
                dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, ObjDay1VO.AnesthetistID);
                dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, ObjDay1VO.AssAnesthetistID);
                dbServer.AddInParameter(command, "IVFCycleCount", DbType.Int64, ObjDay1VO.IVFCycleCount);
                dbServer.AddInParameter(command, "SourceNeedleID", DbType.Int64, ObjDay1VO.SourceNeedleID);
                dbServer.AddInParameter(command, "InfectionObserved", DbType.String, ObjDay1VO.InfectionObserved);
                dbServer.AddInParameter(command, "TotNoOfOocytes", DbType.Int32, ObjDay1VO.TotNoOfOocytes);
                dbServer.AddInParameter(command, "TotNoOf2PN", DbType.Int32, ObjDay1VO.TotNoOf2PN);
                dbServer.AddInParameter(command, "TotNoOf3PN", DbType.Int32, ObjDay1VO.TotNoOf3PN);
                dbServer.AddInParameter(command, "TotNoOf2PB", DbType.Int32, ObjDay1VO.TotNoOf2PB);
                dbServer.AddInParameter(command, "ToNoOfMI", DbType.Int32, ObjDay1VO.ToNoOfMI);
                dbServer.AddInParameter(command, "ToNoOfMII", DbType.Int32, ObjDay1VO.ToNoOfMII);
                dbServer.AddInParameter(command, "ToNoOfGV", DbType.Int32, ObjDay1VO.ToNoOfGV);
                dbServer.AddInParameter(command, "ToNoOfDeGenerated", DbType.Int32, ObjDay1VO.ToNoOfDeGenerated);
                dbServer.AddInParameter(command, "ToNoOfLost ", DbType.Int32, ObjDay1VO.ToNoOfLost);
                dbServer.AddInParameter(command, "IsFreezed  ", DbType.Boolean, ObjDay1VO.IsFreezed);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                if (BizActionObj.Day2Details.FertilizationAssesmentDetails != null && BizActionObj.Day2Details.FertilizationAssesmentDetails.Count > 0)
                {
                    DbCommand command11 = dbServer.GetStoredProcCommand("CIMS_DeleteLab3Details");
                    dbServer.AddInParameter(command11, "Day3ID", DbType.Int64, ObjDay1VO.ID);
                    dbServer.AddInParameter(command11, "LabDay", DbType.Int32, IVFLabDay.Day3);
                    int intStatus2 = dbServer.ExecuteNonQuery(command11, trans);
                }
                if (BizActionObj.Day2Details.FertilizationAssesmentDetails != null && BizActionObj.Day2Details.FertilizationAssesmentDetails.Count > 0)
                {
                    List<clsEmbryoTransferDetailsVO> ETForwardedDetails = new List<clsEmbryoTransferDetailsVO>();

                    foreach (var item1 in ObjDay1VO.FertilizationAssesmentDetails)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay3Details");

                        dbServer.AddInParameter(command2, "Day3ID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "OoNo", DbType.Int64, item1.OoNo);
                        //By Anjali..........
                        dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, item1.SerialOccyteNo);
                        //...................
                        dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, item1.PlanTreatmentID);
                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, item1.Date);
                        dbServer.AddInParameter(command2, "Time", DbType.DateTime, item1.Time);
                        dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, item1.SrcOocyteID);
                        dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, item1.OocyteDonorID);
                        dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, item1.SrcOfSemen);
                        dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, item1.SemenDonorID);
                        dbServer.AddInParameter(command2, "FileName", DbType.String, item1.FileName);
                        dbServer.AddInParameter(command2, "FileContents", DbType.Binary, item1.FileContents);

                        if (item1.SelectedFePlan != null)
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, item1.SelectedFePlan.ID);
                        else
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);
                        if (item1.SelectedGrade != null)
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, item1.SelectedGrade.ID);
                        else
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);
                        if (item1.SelectedFragmentation != null)
                            dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, item1.SelectedFragmentation.ID);
                        else
                            dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, 0);
                        if (item1.SelectedBlastomereSymmetry != null)
                            dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, item1.SelectedBlastomereSymmetry.ID);
                        else
                            dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, 0);
                        dbServer.AddInParameter(command2, "Score", DbType.Int64, item1.Score);
                        dbServer.AddInParameter(command2, "PV", DbType.Boolean, item1.PV);
                        dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, item1.XFactor);
                        dbServer.AddInParameter(command2, "Others", DbType.String, item1.Others);
                        //dbServer.AddInParameter(command2, "ProceedDay4", DbType.Boolean, item1.ProceedDay3);
                        if (item1.SelectedPlan != null)
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, item1.SelectedPlan.ID);
                        else
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);
                        if (item1.SelectedPlan.ID == 3)
                        {
                            dbServer.AddInParameter(command2, "ProceedDay4", DbType.Boolean, true);
                        }
                        else
                        {
                            dbServer.AddInParameter(command2, "ProceedDay4", DbType.Boolean, false);
                        }
                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);

                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                        item1.ID = (long)dbServer.GetParameterValue(command2, "ID");
                        // Forward Embryos
                        //if (ObjDay1VO.IsFreezed == true)
                        //{
                        //    if (item1.SelectedPlan != null && item1.SelectedGrade.ID == 4)
                        //    {
                        //        clsEmbryoTransferDetailsVO obj = new clsEmbryoTransferDetailsVO();
                        //        obj.TransferDate = item1.Date;
                        //        obj.TransferDay = IVFLabDay.Day2;
                        //        obj.RecID = item1.ID;
                        //        obj.EmbryoNumber = item1.OoNo;
                        //        if (item1.SelectedGrade != null)
                        //            obj.GradeID = item1.SelectedGrade.ID;
                        //        else
                        //            obj.GradeID = 0;
                        //        obj.Score = Convert.ToInt32(item1.Score);
                        //        obj.FertilizationStageID = item1.FertilisationStage;
                        //        ETForwardedDetails.Add(obj);
                        //    }
                        //}

                        if (item1.MediaDetails != null && item1.MediaDetails.Count > 0)
                        {
                            foreach (var item3 in item1.MediaDetails)
                            {
                                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");
                                dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                                dbServer.AddInParameter(command4, "DetailID", DbType.Int64, item1.ID);
                                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command4, "Date", DbType.DateTime, DateTime.Now);
                                dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day3);
                                dbServer.AddInParameter(command4, "MediaName", DbType.String, item3.ItemName);
                                dbServer.AddInParameter(command4, "Company", DbType.String, item3.Company);
                                dbServer.AddInParameter(command4, "LotNo", DbType.String, item3.BatchCode);
                                dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, item3.ExpiryDate);
                                dbServer.AddInParameter(command4, "PH", DbType.Boolean, item3.PH);
                                dbServer.AddInParameter(command4, "OSM", DbType.Boolean, item3.OSM);
                                dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, item3.VolumeUsed);
                                dbServer.AddInParameter(command4, "Status", DbType.Int64, item3.SelectedStatus.ID);
                                dbServer.AddInParameter(command4, "BatchID", DbType.Int64, item3.BatchID);
                                dbServer.AddInParameter(command4, "StoreID", DbType.Int64, item3.StoreID);
                                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);
                                int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                item3.ID = (long)dbServer.GetParameterValue(command4, "ID");
                            }
                        }

                        if (item1.Day2CalculateDetails != null)
                        {
                            var item6 = item1.Day2CalculateDetails;
                            DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay3CalculateDetails");
                            dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                            dbServer.AddInParameter(command5, "DetailID", DbType.Int64, item1.ID);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command5, "ZonaThickness", DbType.Int32, item6.ZonaThickness);
                            dbServer.AddInParameter(command5, "ZonaTexture", DbType.Int32, item6.ZonaTexture);
                            dbServer.AddInParameter(command5, "BlastomereSize", DbType.Int32, item6.BlastomereSize);
                            dbServer.AddInParameter(command5, "BlastomereShape", DbType.Int32, item6.BlastomereShape);
                            dbServer.AddInParameter(command5, "BlastomeresVolume", DbType.Int32, item6.BlastomeresVolume);
                            dbServer.AddInParameter(command5, "Membrane", DbType.Int32, item6.Membrane);
                            dbServer.AddInParameter(command5, "Cytoplasm", DbType.Int32, item6.Cytoplasm);
                            dbServer.AddInParameter(command5, "Fragmentation", DbType.Int32, item6.Fragmentation);
                            dbServer.AddInParameter(command5, "DevelopmentRate", DbType.Int32, item6.DevelopmentRate);
                            dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item6.ID);
                            dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);
                            int intStatus1 = dbServer.ExecuteNonQuery(command5, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command5, "ResultStatus");
                            item6.ID = (long)dbServer.GetParameterValue(command5, "ID");
                        }
                    }

                    //if (BizActionObj.Day2Details.IsFreezed == true && ETForwardedDetails.Count > 0)
                    //{
                    //    clsBaseIVFEmbryoTransferDAL objBaseDAL = clsBaseIVFEmbryoTransferDAL.GetInstance();
                    //    clsAddForwardedEmbryoTransferBizActionVO BizActionFrwrd = new clsAddForwardedEmbryoTransferBizActionVO();
                    //    BizActionFrwrd.UnitID = BizActionObj.Day2Details.UnitID;
                    //    BizActionFrwrd.CoupleID = BizActionObj.Day2Details.CoupleID;
                    //    BizActionFrwrd.CoupleUnitID = BizActionObj.Day2Details.CoupleUnitID;
                    //    BizActionFrwrd.ForwardedEmbryos = ETForwardedDetails;

                    //    BizActionFrwrd = (clsAddForwardedEmbryoTransferBizActionVO)objBaseDAL.AddForwardedEmbryos(BizActionFrwrd, UserVo, con, trans);

                    //    if (BizActionFrwrd.SuccessStatus == -1) throw new Exception();
                    //}
                }

                if (BizActionObj.Day2Details.FUSetting != null && BizActionObj.Day2Details.FUSetting.Count > 0)
                {
                    DbCommand command13 = dbServer.GetStoredProcCommand("CIMS_DeleteLab3UploadFileDetails");

                    dbServer.AddInParameter(command13, "Day3ID", DbType.Int64, ObjDay1VO.ID);
                    dbServer.AddInParameter(command13, "LabDay", DbType.Int32, IVFLabDay.Day3);
                    dbServer.AddInParameter(command13, "UnitID", DbType.Int64, ObjDay1VO.UnitID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command13, trans);
                }
                if (BizActionObj.Day2Details.FUSetting != null && BizActionObj.Day2Details.FUSetting.Count > 0)
                {
                    foreach (var item2 in ObjDay1VO.FUSetting)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                        dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day3);
                        dbServer.AddInParameter(command3, "FileName", DbType.String, item2.FileName);
                        dbServer.AddInParameter(command3, "FileIndex", DbType.Int32, item2.Index);
                        dbServer.AddInParameter(command3, "Value", DbType.Binary, item2.Data);
                        dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                        dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);
                        int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                        item2.ID = (long)dbServer.GetParameterValue(command3, "ID");
                    }
                }


                if (BizActionObj.Day2Details.SemenDetails != null)
                {
                    DbCommand command12 = dbServer.GetStoredProcCommand("CIMS_DeleteLab3SemenDetails");

                    dbServer.AddInParameter(command12, "Day3ID", DbType.Int64, ObjDay1VO.ID);
                    dbServer.AddInParameter(command12, "LabDay", DbType.Int32, IVFLabDay.Day3);
                    int intStatus2 = dbServer.ExecuteNonQuery(command12, trans);
                }
                if (BizActionObj.Day2Details.SemenDetails != null)
                {
                    var item4 = BizActionObj.Day2Details.SemenDetails;
                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");

                    dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay1VO.ID);
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day3);
                    dbServer.AddInParameter(command4, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day2Details.SemenDetails.MethodOfSpermPreparation);
                    dbServer.AddInParameter(command4, "SourceOfSemen", DbType.Int64, BizActionObj.Day2Details.SemenDetails.SourceOfSemen);
                    dbServer.AddInParameter(command4, "PreSelfVolume", DbType.String, item4.PreSelfVolume);
                    dbServer.AddInParameter(command4, "PreSelfConcentration", DbType.String, item4.PreSelfConcentration);
                    dbServer.AddInParameter(command4, "PreSelfMotality", DbType.String, item4.PreSelfMotality);
                    dbServer.AddInParameter(command4, "PreSelfWBC", DbType.String, item4.PreSelfWBC);
                    dbServer.AddInParameter(command4, "PreDonorVolume", DbType.String, item4.PreDonorVolume);
                    dbServer.AddInParameter(command4, "PreDonorConcentration", DbType.String, item4.PreDonorConcentration);
                    dbServer.AddInParameter(command4, "PreDonorMotality", DbType.String, item4.PreDonorMotality);
                    dbServer.AddInParameter(command4, "PreDonorWBC", DbType.String, item4.PreDonorWBC);
                    dbServer.AddInParameter(command4, "PostSelfVolume", DbType.String, item4.PostSelfVolume);
                    dbServer.AddInParameter(command4, "PostSelfConcentration", DbType.String, item4.PostSelfConcentration);
                    dbServer.AddInParameter(command4, "PostSelfMotality", DbType.String, item4.PostSelfMotality);
                    dbServer.AddInParameter(command4, "PostSelfWBC", DbType.String, item4.PostSelfWBC);
                    dbServer.AddInParameter(command4, "PostDonorVolume ", DbType.String, item4.PostDonorVolume);
                    dbServer.AddInParameter(command4, "PostDonorConcentration", DbType.String, item4.PostDonorConcentration);
                    dbServer.AddInParameter(command4, "PostDonorMotality", DbType.String, item4.PostDonorMotality);
                    dbServer.AddInParameter(command4, "PostDonorWBC", DbType.String, item4.PostDonorWBC);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item4.ID);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    item4.ID = (long)dbServer.GetParameterValue(command4, "ID");
                }
                
                if (BizActionObj.Day2Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                    obj.LabDaysSummary = BizActionObj.Day2Details.LabDaySummary;
                    obj.LabDaysSummary.OocyteID = BizActionObj.Day2Details.ID;
                    obj.LabDaysSummary.CoupleID = BizActionObj.Day2Details.CoupleID;
                    obj.LabDaysSummary.CoupleUnitID = BizActionObj.Day2Details.CoupleUnitID;
                    obj.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay3;
                    obj.LabDaysSummary.Priority = 1;
                    obj.LabDaysSummary.ProcDate = BizActionObj.Day2Details.Date;
                    obj.LabDaysSummary.ProcTime = BizActionObj.Day2Details.Time;
                    obj.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    obj.LabDaysSummary.IsFreezed = BizActionObj.Day2Details.IsFreezed;

                    obj.IsUpdate = true;

                    obj = (clsAddUpdateLabDaysSummaryBizActionVO)objBaseDAL.AddUpdateLabDaysSummary(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Day2Details.LabDaySummary.ID = obj.LabDaysSummary.ID;
                }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
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

        public override IValueObject GetFemaleLabDay3MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay2MediaAndCalcDetailsBizActionVO BizAction = (valueObject) as clsGetDay2MediaAndCalcDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMediaDetailsAndCalculateDetailForDay3");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "DetailID", DbType.Int64, BizAction.DetailID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.LabDayCalDetails == null)
                        BizAction.LabDayCalDetails = new clsFemaleLabDay2CalculateDetailsVO();

                    while (reader.Read())
                    {
                        BizAction.LabDayCalDetails.FertilizationID = (long)(DALHelper.HandleDBNull(reader["DetailID"]));
                        BizAction.LabDayCalDetails.ZonaThickness = (Int16)(DALHelper.HandleDBNull(reader["ZonaThickness"]));
                        BizAction.LabDayCalDetails.ZonaTexture = (Int16)(DALHelper.HandleDBNull(reader["ZonaTexture"]));
                        BizAction.LabDayCalDetails.BlastomereSize = (Int16)(DALHelper.HandleDBNull(reader["BlastomereSize"]));
                        BizAction.LabDayCalDetails.BlastomereShape = (Int16)(DALHelper.HandleDBNull(reader["BlastomereShape"]));
                        BizAction.LabDayCalDetails.BlastomeresVolume = (Int16)(DALHelper.HandleDBNull(reader["BlastomeresVolume"]));
                        BizAction.LabDayCalDetails.Membrane = (Int16)(DALHelper.HandleDBNull(reader["Membrane"]));
                        BizAction.LabDayCalDetails.Cytoplasm = (Int16)(DALHelper.HandleDBNull(reader["Cytoplasm"]));
                        BizAction.LabDayCalDetails.Fragmentation = (Int16)(DALHelper.HandleDBNull(reader["Fragmentation"]));
                        BizAction.LabDayCalDetails.DevelopmentRate = (Int16)(DALHelper.HandleDBNull(reader["DevelopmentRate"]));
                    }

                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizAction;

        }

        public override IValueObject GetFemaleLabDay3(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay2DetailsBizActionVO BizAction = (valueObject) as clsGetDay2DetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleLabDay3");
                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizAction.CoupleUnitID);
                dbServer.AddInParameter(command, "LabDay", DbType.Int16, IVFLabDay.Day3);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.LabDay2 == null)
                        BizAction.LabDay2 = new clsFemaleLabDay2VO();
                    while (reader.Read())
                    {
                        BizAction.LabDay2.ID = (long)(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.LabDay2.UnitID = (long)(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.LabDay2.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        BizAction.LabDay2.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));

                        BizAction.LabDay2.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        BizAction.LabDay2.Time = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                        BizAction.LabDay2.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.LabDay2.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        BizAction.LabDay2.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        BizAction.LabDay2.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        BizAction.LabDay2.SourceNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SourceNeedleID"]));
                        BizAction.LabDay2.InfectionObserved = (string)(DALHelper.HandleDBNull(reader["InfectionObserved"]));

                        BizAction.LabDay2.TotNoOfOocytes = (int)(DALHelper.HandleDBNull(reader["TotNoOfOocytes"]));
                        BizAction.LabDay2.TotNoOf2PN = (int)(DALHelper.HandleDBNull(reader["TotNoOf2PN"]));
                        BizAction.LabDay2.TotNoOf3PN = (int)(DALHelper.HandleDBNull(reader["TotNoOf3PN"]));
                        BizAction.LabDay2.TotNoOf2PB = (int)(DALHelper.HandleDBNull(reader["TotNoOf2PB"]));
                        BizAction.LabDay2.ToNoOfMI = (int)(DALHelper.HandleDBNull(reader["ToNoOfMI"]));
                        BizAction.LabDay2.ToNoOfMII = (int)(DALHelper.HandleDBNull(reader["ToNoOfMII"]));
                        BizAction.LabDay2.ToNoOfGV = (int)(DALHelper.HandleDBNull(reader["ToNoOfGV"]));
                        BizAction.LabDay2.ToNoOfDeGenerated = (int)(DALHelper.HandleDBNull(reader["ToNoOfDeGenerated"]));
                        BizAction.LabDay2.ToNoOfLost = (int)(DALHelper.HandleDBNull(reader["ToNoOfLost"]));
                        BizAction.LabDay2.IsFreezed = (bool)(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        BizAction.LabDay2.Status = (bool)(DALHelper.HandleDBNull(reader["Status"]));

                    }

                    reader.NextResult();
                    if (BizAction.LabDay2.FertilizationAssesmentDetails == null)
                        BizAction.LabDay2.FertilizationAssesmentDetails = new List<clsFemaleLabDay2FertilizationAssesmentVO>();


                    while (reader.Read())
                    {
                        clsFemaleLabDay2FertilizationAssesmentVO ObjFer = new clsFemaleLabDay2FertilizationAssesmentVO();
                        ObjFer.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ObjFer.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);

                        //By Anjali.............
                        ObjFer.SerialOccyteNo = (long)DALHelper.HandleDBNull(reader["SerialOccyteNo"]);

                        //................
                        ObjFer.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        ObjFer.Time = (DateTime?)DALHelper.HandleDate(reader["Time"]);

                        ObjFer.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);
                        ObjFer.PlanTreatment = (string)DALHelper.HandleDBNull(reader["Protocol"]);

                        ObjFer.SrcOocyteID = (long)DALHelper.HandleDBNull(reader["SrcOocyteID"]);
                        ObjFer.SrcOocyteDescription = (string)DALHelper.HandleDBNull(reader["SOOocyte"]);

                        ObjFer.OocyteDonorID = (string)DALHelper.HandleDBNull(reader["OSCode"]);


                        ObjFer.SrcOfSemen = (long)DALHelper.HandleDBNull(reader["SrcOfSemen"]);
                        ObjFer.SrcOfSemenDescription = (string)DALHelper.HandleDBNull(reader["SOSemen"]);

                        ObjFer.SemenDonorID = (string)DALHelper.HandleDBNull(reader["SSCode"]);


                        ObjFer.SelectedFePlan.ID = (long)(DALHelper.HandleDBNull(reader["FertilisationStage"]));
                        ObjFer.SelectedFePlan.Description = (string)(DALHelper.HandleDBNull(reader["Fertilization"]));

                        ObjFer.Score = (long)(DALHelper.HandleDBNull(reader["Score"]));
                        ObjFer.PV = (bool)(DALHelper.HandleDBNull(reader["PV"]));
                        ObjFer.XFactor = (bool)(DALHelper.HandleDBNull(reader["XFactor"]));
                        ObjFer.Others = (string)(DALHelper.HandleDBNull(reader["Others"]));
                        ObjFer.ProceedDay3 = (bool)(DALHelper.HandleDBNull(reader["ProceedDay4"]));

                        ObjFer.SelectedGrade.ID = (long)(DALHelper.HandleDBNull(reader["GradeID"]));
                        ObjFer.SelectedGrade.Description = (string)(DALHelper.HandleDBNull(reader["Grade"]));

                        ObjFer.SelectedFragmentation.ID = (long)(DALHelper.HandleDBNull(reader["FragmentationID"]));
                        ObjFer.SelectedFragmentation.Description = (string)(DALHelper.HandleDBNull(reader["Fragmentation"]));

                        ObjFer.SelectedBlastomereSymmetry.ID = (long)(DALHelper.HandleDBNull(reader["BlastomereSymmetryID"]));
                        ObjFer.SelectedBlastomereSymmetry.Description = (string)(DALHelper.HandleDBNull(reader["BlastomereSymmetry"]));

                        ObjFer.SelectedPlan.ID = (long)(DALHelper.HandleDBNull(reader["PlanID"]));
                        ObjFer.SelectedPlan.Description = (string)(DALHelper.HandleDBNull(reader["PlanName"]));
                        ObjFer.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ObjFer.FileContents = (byte[])(DALHelper.HandleDBNull(reader["FileContents"]));

                        //For Getting Details
                        clsBaseIVFLabDayDAL objBaseDAL = clsBaseIVFLabDayDAL.GetInstance();
                        clsGetDay2MediaAndCalcDetailsBizActionVO BizActionDetails = new clsGetDay2MediaAndCalcDetailsBizActionVO();

                        BizActionDetails.ID = BizAction.ID;
                        BizActionDetails.DetailID = ObjFer.ID;
                        BizActionDetails = (clsGetDay2MediaAndCalcDetailsBizActionVO)objBaseDAL.GetFemaleLabDay3MediaAndCalDetails(BizActionDetails, UserVo);
                        ObjFer.Day2CalculateDetails = BizActionDetails.LabDayCalDetails;


                        //For getting Media details
                        clsBaseIVFLabDayDAL objBaseDAL2 = clsBaseIVFLabDayDAL.GetInstance();
                        clsGetAllDayMediaDetailsBizActionVO BizActionMedia = new clsGetAllDayMediaDetailsBizActionVO();
                        BizActionMedia.ID = BizAction.ID;
                        BizActionMedia.DetailID = ObjFer.ID;
                        BizActionMedia.LabDay = 3;
                        BizActionMedia = (clsGetAllDayMediaDetailsBizActionVO)objBaseDAL2.GetAllDayMediaDetails(BizActionMedia, UserVo);
                        ObjFer.MediaDetails = BizActionMedia.MediaList;


                        BizAction.LabDay2.FertilizationAssesmentDetails.Add(ObjFer);

                    }
                    reader.NextResult();
                    if (BizAction.LabDay2.SemenDetails == null)
                        BizAction.LabDay2.SemenDetails = new clsFemaleSemenDetailsVO();


                    while (reader.Read())
                    {
                        BizAction.LabDay2.SemenDetails.MOSP = (string)(DALHelper.HandleDBNull(reader["MOSP"]));
                        BizAction.LabDay2.SemenDetails.SOS = (string)(DALHelper.HandleDBNull(reader["SOS"]));

                        BizAction.LabDay2.SemenDetails.MethodOfSpermPreparation = (long)(DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]));
                        BizAction.LabDay2.SemenDetails.SourceOfSemen = (long)(DALHelper.HandleDBNull(reader["SourceOfSemen"]));

                        BizAction.LabDay2.SemenDetails.PreSelfVolume = (string)(DALHelper.HandleDBNull(reader["PreSelfVolume"]));
                        BizAction.LabDay2.SemenDetails.PreSelfConcentration = (string)(DALHelper.HandleDBNull(reader["PreSelfConcentration"]));
                        BizAction.LabDay2.SemenDetails.PreSelfMotality = (string)(DALHelper.HandleDBNull(reader["PreSelfMotality"]));
                        BizAction.LabDay2.SemenDetails.PreSelfWBC = (string)(DALHelper.HandleDBNull(reader["PreSelfWBC"]));

                        BizAction.LabDay2.SemenDetails.PreDonorVolume = (string)(DALHelper.HandleDBNull(reader["PreDonorVolume"]));
                        BizAction.LabDay2.SemenDetails.PreDonorConcentration = (string)(DALHelper.HandleDBNull(reader["PreDonorConcentration"]));
                        BizAction.LabDay2.SemenDetails.PreDonorMotality = (string)(DALHelper.HandleDBNull(reader["PreDonorMotality"]));
                        BizAction.LabDay2.SemenDetails.PreDonorWBC = (string)(DALHelper.HandleDBNull(reader["PreDonorWBC"]));


                        BizAction.LabDay2.SemenDetails.PostSelfVolume = (string)(DALHelper.HandleDBNull(reader["PostSelfVolume"]));
                        BizAction.LabDay2.SemenDetails.PostSelfConcentration = (string)(DALHelper.HandleDBNull(reader["PostSelfConcentration"]));
                        BizAction.LabDay2.SemenDetails.PostSelfMotality = (string)(DALHelper.HandleDBNull(reader["PostSelfMotality"]));
                        BizAction.LabDay2.SemenDetails.PostSelfWBC = (string)(DALHelper.HandleDBNull(reader["PostSelfWBC"]));

                        BizAction.LabDay2.SemenDetails.PostDonorVolume = (string)(DALHelper.HandleDBNull(reader["PostDonorVolume"]));
                        BizAction.LabDay2.SemenDetails.PostDonorConcentration = (string)(DALHelper.HandleDBNull(reader["PostDonorConcentration"]));
                        BizAction.LabDay2.SemenDetails.PostDonorMotality = (string)(DALHelper.HandleDBNull(reader["PostDonorMotality"]));
                        BizAction.LabDay2.SemenDetails.PostDonorWBC = (string)(DALHelper.HandleDBNull(reader["PostDonorWBC"]));
                    }

                    reader.NextResult();
                    if (BizAction.LabDay2.FUSetting == null)
                        BizAction.LabDay2.FUSetting = new List<FileUpload>();
                    while (reader.Read())
                    {
                        FileUpload ObjFile = new FileUpload();
                        ObjFile.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ObjFile.Index = (Int32)(DALHelper.HandleDBNull(reader["FileIndex"]));
                        ObjFile.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ObjFile.Data = (byte[])(DALHelper.HandleDBNull(reader["Value"]));
                        BizAction.LabDay2.FUSetting.Add(ObjFile);
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
        public override IValueObject GetLabDay2ForDay3(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLabDay1ForLabDay2BizActionVO BizActionObj = valueObject as clsGetLabDay1ForLabDay2BizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFemaleLabDay1ForLabDay3");
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.CoupleUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Day2Details == null)
                        BizActionObj.Day2Details = new List<clsFemaleLabDay2FertilizationAssesmentVO>();
                    while (reader.Read())
                    {
                        clsFemaleLabDay2FertilizationAssesmentVO Obj = new clsFemaleLabDay2FertilizationAssesmentVO();
                        Obj.ID = (long)DALHelper.HandleDBNull(reader["DetailID"]);
                        Obj.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);

                        //By Anjali.............
                        Obj.SerialOccyteNo = (long)DALHelper.HandleDBNull(reader["SerialOccyteNo"]);

                        //................
                        Obj.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        Obj.Time = (DateTime?)DALHelper.HandleDate(reader["Time"]);
                        Obj.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);
                        Obj.PlanTreatment = (string)DALHelper.HandleDBNull(reader["Protocol"]);
                        Obj.SrcOocyteID = (long)DALHelper.HandleDBNull(reader["SrcOocyteID"]);
                        Obj.SrcOocyteDescription = (string)DALHelper.HandleDBNull(reader["SOOocyte"]);
                        Obj.OocyteDonorID = (string)DALHelper.HandleDBNull(reader["OSCode"]);
                        Obj.SrcOfSemen = (long)DALHelper.HandleDBNull(reader["SrcOfSemen"]);
                        Obj.SrcOfSemenDescription = (string)DALHelper.HandleDBNull(reader["SOSemen"]);
                        Obj.SemenDonorID = (string)DALHelper.HandleDBNull(reader["SSCode"]);
                        BizActionObj.Day2Details.Add(Obj);

                        BizActionObj.AnaesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        BizActionObj.AssAnaesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        BizActionObj.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizActionObj.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
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

        #region Day 4
        public override IValueObject AddLabDay4(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddLabDay4BizActionVO BizActionObj = valueObject as clsAddLabDay4BizActionVO;

            if (BizActionObj.Day4Details.ID == 0)
                BizActionObj = AddDay4(BizActionObj, UserVo);
            else
                BizActionObj = UpdateDay4(BizActionObj, UserVo);

            return valueObject;   
        }

        private clsAddLabDay4BizActionVO AddDay4(clsAddLabDay4BizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsFemaleLabDay4VO ObjDay4VO = BizActionObj.Day4Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay4");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, ObjDay4VO.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, ObjDay4VO.CoupleUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjDay4VO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, ObjDay4VO.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, ObjDay4VO.EmbryologistID);
                dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, ObjDay4VO.AssEmbryologistID);
                dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, ObjDay4VO.AnesthetistID);
                dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, ObjDay4VO.AssAnesthetistID);
                dbServer.AddInParameter(command, "IVFCycleCount", DbType.Int64, ObjDay4VO.IVFCycleCount);
                dbServer.AddInParameter(command, "SourceNeedleID", DbType.Int64, ObjDay4VO.SourceNeedleID);
                dbServer.AddInParameter(command, "InfectionObserved", DbType.String, ObjDay4VO.InfectionObserved);
                dbServer.AddInParameter(command, "TotNoOfOocytes", DbType.Int32, ObjDay4VO.TotNoOfOocytes);
                dbServer.AddInParameter(command, "TotNoOf2PN", DbType.Int32, ObjDay4VO.TotNoOf2PN);
                dbServer.AddInParameter(command, "TotNoOf3PN", DbType.Int32, ObjDay4VO.TotNoOf3PN);
                dbServer.AddInParameter(command, "TotNoOf2PB", DbType.Int32, ObjDay4VO.TotNoOf2PB);
                dbServer.AddInParameter(command, "ToNoOfMI", DbType.Int32, ObjDay4VO.ToNoOfMI);
                dbServer.AddInParameter(command, "ToNoOfMII", DbType.Int32, ObjDay4VO.ToNoOfMII);
                dbServer.AddInParameter(command, "ToNoOfGV", DbType.Int32, ObjDay4VO.ToNoOfGV);
                dbServer.AddInParameter(command, "ToNoOfDeGenerated", DbType.Int32, ObjDay4VO.ToNoOfDeGenerated);
                dbServer.AddInParameter(command, "ToNoOfLost ", DbType.Int32, ObjDay4VO.ToNoOfLost);

                dbServer.AddInParameter(command, "IsFreezed  ", DbType.Boolean, ObjDay4VO.IsFreezed);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDay4VO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Day4Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.Day4Details.FertilizationAssesmentDetails != null && BizActionObj.Day4Details.FertilizationAssesmentDetails.Count > 0)
                {
                    foreach (var item1 in ObjDay4VO.FertilizationAssesmentDetails)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay4Details");

                        dbServer.AddInParameter(command2, "Day4ID", DbType.Int64, ObjDay4VO.ID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "OoNo", DbType.Int64, item1.OoNo);
                        //By Anjali..........
                        dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, item1.SerialOccyteNo);
                        //...................
                        dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, item1.PlanTreatmentID);
                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, item1.Date);
                        dbServer.AddInParameter(command2, "Time", DbType.DateTime, item1.Time);
                        dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, item1.SrcOocyteID);
                        dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, item1.OocyteDonorID);
                        dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, item1.SrcOfSemen);
                        dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, item1.SemenDonorID);
                        dbServer.AddInParameter(command2, "FileName", DbType.String, item1.FileName);
                        dbServer.AddInParameter(command2, "FileContents", DbType.Binary, item1.FileContents);

                        if (item1.SelectedFePlan!=null)
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, item1.SelectedFePlan.ID);
                        else
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);
                        
                        if (item1.SelectedGrade !=null)
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, item1.SelectedGrade.ID);
                        else
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);

                        if (item1.SelectedFragmentation != null)
                            dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, item1.SelectedFragmentation.ID);
                        else
                            dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, 0);
                        if (item1.SelectedBlastomereSymmetry != null)
                            dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, item1.SelectedBlastomereSymmetry.ID);
                        else
                            dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, 0);
                        dbServer.AddInParameter(command2, "Score", DbType.Int64, item1.Score);
                        dbServer.AddInParameter(command2, "PV", DbType.Boolean, item1.PV);
                        dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, item1.XFactor);
                        dbServer.AddInParameter(command2, "Others", DbType.String, item1.Others);
                        //dbServer.AddInParameter(command2, "ProceedDay5", DbType.Boolean, item1.ProceedDay5);
                        if (item1.SelectedPlan!=null)
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, item1.SelectedPlan.ID);
                        else
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);
                        if (item1.SelectedPlan.ID == 3)
                        {
                            dbServer.AddInParameter(command2, "ProceedDay5", DbType.Boolean, true);
                        }
                        else
                        {
                            dbServer.AddInParameter(command2, "ProceedDay5", DbType.Boolean, false);
                        }
                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);

                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                        item1.ID = (long)dbServer.GetParameterValue(command2, "ID");

                        if (item1.MediaDetails != null && item1.MediaDetails.Count > 0)
                        {
                            foreach (var item3 in item1.MediaDetails)
                            {
                                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");

                                dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                                dbServer.AddInParameter(command4, "DetailID", DbType.Int64, item1.ID);
                                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command4, "Date", DbType.DateTime, item3.Date);
                                dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day4);
                                dbServer.AddInParameter(command4, "MediaName", DbType.String, item3.ItemName);
                                dbServer.AddInParameter(command4, "Company", DbType.String, item3.Company);
                                dbServer.AddInParameter(command4, "LotNo", DbType.String, item3.BatchCode);
                                dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, item3.ExpiryDate);
                                dbServer.AddInParameter(command4, "PH", DbType.Boolean, item3.PH);
                                dbServer.AddInParameter(command4, "OSM", DbType.Boolean, item3.OSM);
                                dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, item3.VolumeUsed);
                                dbServer.AddInParameter(command4, "Status", DbType.Int64, item3.SelectedStatus.ID);
                                dbServer.AddInParameter(command4, "BatchID", DbType.Int64, item3.BatchID);
                                dbServer.AddInParameter(command4, "StoreID", DbType.Int64, item3.StoreID);  

                                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                item3.ID = (long)dbServer.GetParameterValue(command4, "ID");
                            }
                        }
                        if (item1.Day4CalculateDetails != null)
                        {
                            var item6 = item1.Day4CalculateDetails;

                            DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay4CalculateDetails");

                            dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                            dbServer.AddInParameter(command5, "DetailID", DbType.Int64, item1.ID);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command5, "Compaction", DbType.Boolean, item6.Compaction);
                            dbServer.AddInParameter(command5, "SignsOfBlastocoel", DbType.Boolean, item6.SignsOfBlastocoel);
                           
                            dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item6.ID);
                            dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus1 = dbServer.ExecuteNonQuery(command5, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command5, "ResultStatus");
                            item6.ID = (long)dbServer.GetParameterValue(command5, "ID");

                        }
                    }
                }

                if (BizActionObj.Day4Details.FUSetting != null && BizActionObj.Day4Details.FUSetting.Count > 0)
                {
                    foreach (var item2 in ObjDay4VO.FUSetting)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                        dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day4);
                        dbServer.AddInParameter(command3, "FileName", DbType.String, item2.FileName);
                        dbServer.AddInParameter(command3, "FileIndex", DbType.Int32, item2.Index);
                        dbServer.AddInParameter(command3, "Value", DbType.Binary, item2.Data);
                        dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                        dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                        item2.ID = (long)dbServer.GetParameterValue(command3, "ID");
                    }
                }

                if (BizActionObj.Day4Details.SemenDetails != null)
                {
                    var item4 = BizActionObj.Day4Details.SemenDetails;
                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");

                    dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day4);
                    dbServer.AddInParameter(command4, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day4Details.SemenDetails.MethodOfSpermPreparation);
                    dbServer.AddInParameter(command4, "SourceOfSemen", DbType.Int64, BizActionObj.Day4Details.SemenDetails.SourceOfSemen);
                    dbServer.AddInParameter(command4, "PreSelfVolume", DbType.String, item4.PreSelfVolume);
                    dbServer.AddInParameter(command4, "PreSelfConcentration", DbType.String, item4.PreSelfConcentration);
                    dbServer.AddInParameter(command4, "PreSelfMotality", DbType.String, item4.PreSelfMotality);
                    dbServer.AddInParameter(command4, "PreSelfWBC", DbType.String, item4.PreSelfWBC);
                    dbServer.AddInParameter(command4, "PreDonorVolume", DbType.String, item4.PreDonorVolume);
                    dbServer.AddInParameter(command4, "PreDonorConcentration", DbType.String, item4.PreDonorConcentration);
                    dbServer.AddInParameter(command4, "PreDonorMotality", DbType.String, item4.PreDonorMotality);
                    dbServer.AddInParameter(command4, "PreDonorWBC", DbType.String, item4.PreDonorWBC);
                    dbServer.AddInParameter(command4, "PostSelfVolume", DbType.String, item4.PostSelfVolume);
                    dbServer.AddInParameter(command4, "PostSelfConcentration", DbType.String, item4.PostSelfConcentration);
                    dbServer.AddInParameter(command4, "PostSelfMotality", DbType.String, item4.PostSelfMotality);
                    dbServer.AddInParameter(command4, "PostSelfWBC", DbType.String, item4.PostSelfWBC);
                    dbServer.AddInParameter(command4, "PostDonorVolume ", DbType.String, item4.PostDonorVolume);
                    dbServer.AddInParameter(command4, "PostDonorConcentration", DbType.String, item4.PostDonorConcentration);
                    dbServer.AddInParameter(command4, "PostDonorMotality", DbType.String, item4.PostDonorMotality);
                    dbServer.AddInParameter(command4, "PostDonorWBC", DbType.String, item4.PostDonorWBC);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item4.ID);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    item4.ID = (long)dbServer.GetParameterValue(command4, "ID");
                }
                if (BizActionObj.Day4Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                    obj.LabDaysSummary = BizActionObj.Day4Details.LabDaySummary;
                    obj.LabDaysSummary.OocyteID = BizActionObj.Day4Details.ID;
                    obj.LabDaysSummary.CoupleID = BizActionObj.Day4Details.CoupleID;
                    obj.LabDaysSummary.CoupleUnitID = BizActionObj.Day4Details.CoupleUnitID;
                    obj.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay4;
                    obj.LabDaysSummary.Priority = 1;
                    obj.LabDaysSummary.ProcDate = BizActionObj.Day4Details.Date;
                    obj.LabDaysSummary.ProcTime = BizActionObj.Day4Details.Time;
                    obj.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    obj.LabDaysSummary.IsFreezed = BizActionObj.Day4Details.IsFreezed;
                    obj.IsUpdate = false;
                    obj = (clsAddUpdateLabDaysSummaryBizActionVO)objBaseDAL.AddUpdateLabDaysSummary(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Day4Details.LabDaySummary.ID = obj.LabDaysSummary.ID;
                }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
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

        private clsAddLabDay4BizActionVO UpdateDay4(clsAddLabDay4BizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsFemaleLabDay4VO ObjDay4VO = BizActionObj.Day4Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateFemaleLabDay4");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjDay4VO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                ObjDay4VO.UnitID = UserVo.UserLoginInfo.UnitId;
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, ObjDay4VO.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, ObjDay4VO.CoupleUnitID);

                dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjDay4VO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, ObjDay4VO.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, ObjDay4VO.EmbryologistID);
                dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, ObjDay4VO.AssEmbryologistID);
                dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, ObjDay4VO.AnesthetistID);
                dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, ObjDay4VO.AssAnesthetistID);
                dbServer.AddInParameter(command, "IVFCycleCount", DbType.Int64, ObjDay4VO.IVFCycleCount);
                dbServer.AddInParameter(command, "SourceNeedleID", DbType.Int64, ObjDay4VO.SourceNeedleID);
                dbServer.AddInParameter(command, "InfectionObserved", DbType.String, ObjDay4VO.InfectionObserved);
                dbServer.AddInParameter(command, "TotNoOfOocytes", DbType.Int32, ObjDay4VO.TotNoOfOocytes);
                dbServer.AddInParameter(command, "TotNoOf2PN", DbType.Int32, ObjDay4VO.TotNoOf2PN);
                dbServer.AddInParameter(command, "TotNoOf3PN", DbType.Int32, ObjDay4VO.TotNoOf3PN);
                dbServer.AddInParameter(command, "TotNoOf2PB", DbType.Int32, ObjDay4VO.TotNoOf2PB);
                dbServer.AddInParameter(command, "ToNoOfMI", DbType.Int32, ObjDay4VO.ToNoOfMI);
                dbServer.AddInParameter(command, "ToNoOfMII", DbType.Int32, ObjDay4VO.ToNoOfMII);
                dbServer.AddInParameter(command, "ToNoOfGV", DbType.Int32, ObjDay4VO.ToNoOfGV);
                dbServer.AddInParameter(command, "ToNoOfDeGenerated", DbType.Int32, ObjDay4VO.ToNoOfDeGenerated);
                dbServer.AddInParameter(command, "ToNoOfLost ", DbType.Int32, ObjDay4VO.ToNoOfLost);
                dbServer.AddInParameter(command, "IsFreezed  ", DbType.Boolean, ObjDay4VO.IsFreezed);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                if (BizActionObj.Day4Details.FertilizationAssesmentDetails != null && BizActionObj.Day4Details.FertilizationAssesmentDetails.Count > 0)
                {
                    DbCommand command11 = dbServer.GetStoredProcCommand("CIMS_DeleteLab4Details");

                    dbServer.AddInParameter(command11, "Day4ID", DbType.Int64, ObjDay4VO.ID);
                    dbServer.AddInParameter(command11, "LabDay", DbType.Int32, IVFLabDay.Day4);
                    int intStatus2 = dbServer.ExecuteNonQuery(command11, trans);
                }


                if (BizActionObj.Day4Details.FertilizationAssesmentDetails != null && BizActionObj.Day4Details.FertilizationAssesmentDetails.Count > 0)
                {
                    foreach (var item1 in ObjDay4VO.FertilizationAssesmentDetails)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay4Details");

                        dbServer.AddInParameter(command2, "Day4ID", DbType.Int64, ObjDay4VO.ID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "OoNo", DbType.Int64, item1.OoNo);
                        //By Anjali..........
                        dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, item1.SerialOccyteNo);
                        //...................
                        dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, item1.PlanTreatmentID);
                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, item1.Date);
                        dbServer.AddInParameter(command2, "Time", DbType.DateTime, item1.Time);
                        dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, item1.SrcOocyteID);
                        dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, item1.OocyteDonorID);
                        dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, item1.SrcOfSemen);
                        dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, item1.SemenDonorID);
                        dbServer.AddInParameter(command2, "FileName", DbType.String, item1.FileName);
                        dbServer.AddInParameter(command2, "FileContents", DbType.Binary, item1.FileContents);


                        if (item1.SelectedFePlan !=null)
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, item1.SelectedFePlan.ID);
                        else
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);

                        if (item1.SelectedGrade !=null)
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, item1.SelectedGrade.ID);
                        else
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);

                        if (item1.SelectedFragmentation != null)
                            dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, item1.SelectedFragmentation.ID);
                        else
                            dbServer.AddInParameter(command2, "Fragmentation", DbType.Int64, 0);
                        if (item1.SelectedBlastomereSymmetry != null)
                            dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, item1.SelectedBlastomereSymmetry.ID);
                        else
                            dbServer.AddInParameter(command2, "BlastomereSymmetry", DbType.Int64, 0);

                        dbServer.AddInParameter(command2, "Score", DbType.Int64, item1.Score);
                        dbServer.AddInParameter(command2, "PV", DbType.Boolean, item1.PV);
                        dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, item1.XFactor);
                        dbServer.AddInParameter(command2, "Others", DbType.String, item1.Others);
                        //dbServer.AddInParameter(command2, "ProceedDay5", DbType.Boolean, item1.ProceedDay5);
                        if (item1.SelectedPlan !=null)
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, item1.SelectedPlan.ID);
                        else
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);
                        if (item1.SelectedPlan.ID == 3)
                        {
                            dbServer.AddInParameter(command2, "ProceedDay5", DbType.Boolean, true);
                        }
                        else
                        {
                            dbServer.AddInParameter(command2, "ProceedDay5", DbType.Boolean,false);
                        }

                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);

                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                        item1.ID = (long)dbServer.GetParameterValue(command2, "ID");

                        if (item1.MediaDetails != null && item1.MediaDetails.Count > 0)
                        {
                            foreach (var item3 in item1.MediaDetails)
                            {
                                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");

                                dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                                dbServer.AddInParameter(command4, "DetailID", DbType.Int64, item1.ID);
                                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command4, "Date", DbType.DateTime, item3.Date);
                                dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day4);
                                dbServer.AddInParameter(command4, "MediaName", DbType.String, item3.ItemName);
                                dbServer.AddInParameter(command4, "Company", DbType.String, item3.Company);
                                dbServer.AddInParameter(command4, "LotNo", DbType.String, item3.BatchCode);
                                dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, item3.ExpiryDate);
                                dbServer.AddInParameter(command4, "PH", DbType.Boolean, item3.PH);
                                dbServer.AddInParameter(command4, "OSM", DbType.Boolean, item3.OSM);
                                dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, item3.VolumeUsed);
                                dbServer.AddInParameter(command4, "Status", DbType.Int64, item3.SelectedStatus.ID);
                                dbServer.AddInParameter(command4, "BatchID", DbType.Int64, item3.BatchID);
                                dbServer.AddInParameter(command4, "StoreID", DbType.Int64, item3.StoreID);  

                                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                item3.ID = (long)dbServer.GetParameterValue(command4, "ID");
                            }
                        }
                        if (item1.Day4CalculateDetails != null)
                        {
                            var item6 = item1.Day4CalculateDetails;

                            DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay4CalculateDetails");

                            dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                            dbServer.AddInParameter(command5, "DetailID", DbType.Int64, item1.ID);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command5, "Compaction", DbType.Boolean, item6.Compaction);
                            dbServer.AddInParameter(command5, "SignsOfBlastocoel", DbType.Boolean, item6.SignsOfBlastocoel);

                            dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item6.ID);
                            dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus1 = dbServer.ExecuteNonQuery(command5, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command5, "ResultStatus");
                            item6.ID = (long)dbServer.GetParameterValue(command5, "ID");

                        }
                    }
                }


                if (BizActionObj.Day4Details.FUSetting != null && BizActionObj.Day4Details.FUSetting.Count > 0)
                {
                    DbCommand command13 = dbServer.GetStoredProcCommand("CIMS_DeleteLab4UploadFileDetails");

                    dbServer.AddInParameter(command13, "Day4ID", DbType.Int64, ObjDay4VO.ID);
                    dbServer.AddInParameter(command13, "LabDay", DbType.Int32, IVFLabDay.Day4);
                    dbServer.AddInParameter(command13, "UnitID", DbType.Int64, ObjDay4VO.UnitID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command13, trans);
                }

                if (BizActionObj.Day4Details.FUSetting != null && BizActionObj.Day4Details.FUSetting.Count > 0)
                {
                    foreach (var item2 in ObjDay4VO.FUSetting)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                        dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day4);
                        dbServer.AddInParameter(command3, "FileName", DbType.String, item2.FileName);
                        dbServer.AddInParameter(command3, "FileIndex", DbType.Int32, item2.Index);
                        dbServer.AddInParameter(command3, "Value", DbType.Binary, item2.Data);
                        dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                        dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                        item2.ID = (long)dbServer.GetParameterValue(command3, "ID");
                    }
                }

                if (BizActionObj.Day4Details.SemenDetails != null)
                {
                    DbCommand command12 = dbServer.GetStoredProcCommand("CIMS_DeleteLab4SemenDetails");

                    dbServer.AddInParameter(command12, "Day4ID ", DbType.Int64, ObjDay4VO.ID);
                    dbServer.AddInParameter(command12, "LabDay", DbType.Int32, IVFLabDay.Day4);
                    int intStatus2 = dbServer.ExecuteNonQuery(command12, trans);
                }

                if (BizActionObj.Day4Details.SemenDetails != null)
                {
                    var item4 = BizActionObj.Day4Details.SemenDetails;
                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");

                    dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day4);
                    dbServer.AddInParameter(command4, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day4Details.SemenDetails.MethodOfSpermPreparation);
                    dbServer.AddInParameter(command4, "SourceOfSemen", DbType.Int64, BizActionObj.Day4Details.SemenDetails.SourceOfSemen);
                    dbServer.AddInParameter(command4, "PreSelfVolume", DbType.String, item4.PreSelfVolume);
                    dbServer.AddInParameter(command4, "PreSelfConcentration", DbType.String, item4.PreSelfConcentration);
                    dbServer.AddInParameter(command4, "PreSelfMotality", DbType.String, item4.PreSelfMotality);
                    dbServer.AddInParameter(command4, "PreSelfWBC", DbType.String, item4.PreSelfWBC);
                    dbServer.AddInParameter(command4, "PreDonorVolume", DbType.String, item4.PreDonorVolume);
                    dbServer.AddInParameter(command4, "PreDonorConcentration", DbType.String, item4.PreDonorConcentration);
                    dbServer.AddInParameter(command4, "PreDonorMotality", DbType.String, item4.PreDonorMotality);
                    dbServer.AddInParameter(command4, "PreDonorWBC", DbType.String, item4.PreDonorWBC);
                    dbServer.AddInParameter(command4, "PostSelfVolume", DbType.String, item4.PostSelfVolume);
                    dbServer.AddInParameter(command4, "PostSelfConcentration", DbType.String, item4.PostSelfConcentration);
                    dbServer.AddInParameter(command4, "PostSelfMotality", DbType.String, item4.PostSelfMotality);
                    dbServer.AddInParameter(command4, "PostSelfWBC", DbType.String, item4.PostSelfWBC);
                    dbServer.AddInParameter(command4, "PostDonorVolume ", DbType.String, item4.PostDonorVolume);
                    dbServer.AddInParameter(command4, "PostDonorConcentration", DbType.String, item4.PostDonorConcentration);
                    dbServer.AddInParameter(command4, "PostDonorMotality", DbType.String, item4.PostDonorMotality);
                    dbServer.AddInParameter(command4, "PostDonorWBC", DbType.String, item4.PostDonorWBC);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item4.ID);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    item4.ID = (long)dbServer.GetParameterValue(command4, "ID");
                }
                if (BizActionObj.Day4Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                    obj.LabDaysSummary = BizActionObj.Day4Details.LabDaySummary;
                    obj.LabDaysSummary.OocyteID = BizActionObj.Day4Details.ID;
                    obj.LabDaysSummary.CoupleID = BizActionObj.Day4Details.CoupleID;
                    obj.LabDaysSummary.CoupleUnitID = BizActionObj.Day4Details.CoupleUnitID;
                    obj.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay4;
                    obj.LabDaysSummary.Priority = 1;
                    obj.LabDaysSummary.ProcDate = BizActionObj.Day4Details.Date;
                    obj.LabDaysSummary.ProcTime = BizActionObj.Day4Details.Time;
                    obj.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;


                    obj.IsUpdate = true;

                    obj = (clsAddUpdateLabDaysSummaryBizActionVO)objBaseDAL.AddUpdateLabDaysSummary(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Day4Details.LabDaySummary.ID = obj.LabDaysSummary.ID;
                }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
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
        
        public override IValueObject GetLabDay3ForDay4(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLabDay3ForLabDay4BizActionVO BizActionObj = valueObject as clsGetLabDay3ForLabDay4BizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFemaleLabDay3ForLabDay4");
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.CoupleUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Day4Details == null)
                        BizActionObj.Day4Details = new List<clsFemaleLabDay4FertilizationAssesmentVO>();
                    while (reader.Read())
                    {
                        clsFemaleLabDay4FertilizationAssesmentVO Obj = new clsFemaleLabDay4FertilizationAssesmentVO();
                        Obj.ID = (long)DALHelper.HandleDBNull(reader["DetailID"]);
                       Obj.OoNo=(long)DALHelper.HandleDBNull(reader["OoNo"]);

                       //By Anjali.............
                       Obj.SerialOccyteNo = (long)DALHelper.HandleDBNull(reader["SerialOccyteNo"]);

                       //................
                        Obj.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        Obj.Time = (DateTime?)DALHelper.HandleDate(reader["Time"]);

                        Obj.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);
                        Obj.PlanTreatment = (string)DALHelper.HandleDBNull(reader["Protocol"]);

                        Obj.SrcOocyteID = (long)DALHelper.HandleDBNull(reader["SrcOocyteID"]);
                        Obj.SrcOocyteDescription = (string)DALHelper.HandleDBNull(reader["SOOocyte"]);

                        Obj.OocyteDonorID = (string)DALHelper.HandleDBNull(reader["OSCode"]);
                      

                        Obj.SrcOfSemen = (long)DALHelper.HandleDBNull(reader["SrcOfSemen"]);
                        Obj.SrcOfSemenDescription = (string)DALHelper.HandleDBNull(reader["SOSemen"]);

                        Obj.SemenDonorID = (string)DALHelper.HandleDBNull(reader["SSCode"]);
                        

                        BizActionObj.Day4Details.Add(Obj);

                        BizActionObj.AnaesthetistID = (long)DALHelper.HandleDBNull(reader["AnasthesistID"]);
                        BizActionObj.AssAnaesthetistID = (long)DALHelper.HandleDBNull(reader["AssAnasthesistID"]);
                        BizActionObj.AssEmbryologistID = (long)DALHelper.HandleDBNull(reader["AssEmbryologistID"]);
                        BizActionObj.EmbryologistID = (long)DALHelper.HandleDBNull(reader["EmbryologistID"]);
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

        public override IValueObject GetFemaleLabDay4(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay4DetailsBizActionVO BizAction = (valueObject) as clsGetDay4DetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleLabDay4");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizAction.CoupleUnitID);
                dbServer.AddInParameter(command, "LabDay", DbType.Int16, IVFLabDay.Day4);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.LabDay4 == null)
                        BizAction.LabDay4 = new clsFemaleLabDay4VO();
                    while (reader.Read())
                    {


                        BizAction.LabDay4.ID = (long)(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.LabDay4.UnitID = (long)(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.LabDay4.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        BizAction.LabDay4.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));

                        BizAction.LabDay4.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        BizAction.LabDay4.Time = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                        BizAction.LabDay4.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.LabDay4.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        BizAction.LabDay4.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        BizAction.LabDay4.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        BizAction.LabDay4.SourceNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SourceNeedleID"]));
                        BizAction.LabDay4.InfectionObserved = (string)(DALHelper.HandleDBNull(reader["InfectionObserved"]));

                        BizAction.LabDay4.TotNoOfOocytes = (int)(DALHelper.HandleDBNull(reader["TotNoOfOocytes"]));
                        BizAction.LabDay4.TotNoOf2PN = (int)(DALHelper.HandleDBNull(reader["TotNoOf2PN"]));
                        BizAction.LabDay4.TotNoOf3PN = (int)(DALHelper.HandleDBNull(reader["TotNoOf3PN"]));
                        BizAction.LabDay4.TotNoOf2PB = (int)(DALHelper.HandleDBNull(reader["TotNoOf2PB"]));
                        BizAction.LabDay4.ToNoOfMI = (int)(DALHelper.HandleDBNull(reader["ToNoOfMI"]));
                        BizAction.LabDay4.ToNoOfMII = (int)(DALHelper.HandleDBNull(reader["ToNoOfMII"]));
                        BizAction.LabDay4.ToNoOfGV = (int)(DALHelper.HandleDBNull(reader["ToNoOfGV"]));
                        BizAction.LabDay4.ToNoOfDeGenerated = (int)(DALHelper.HandleDBNull(reader["ToNoOfDeGenerated"]));
                        BizAction.LabDay4.ToNoOfLost = (int)(DALHelper.HandleDBNull(reader["ToNoOfLost"]));
                        BizAction.LabDay4.IsFreezed = (bool)(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        BizAction.LabDay4.Status = (bool)(DALHelper.HandleDBNull(reader["Status"]));

                    }

                    reader.NextResult();
                    if (BizAction.LabDay4.FertilizationAssesmentDetails == null)
                        BizAction.LabDay4.FertilizationAssesmentDetails = new List<clsFemaleLabDay4FertilizationAssesmentVO>();


                    while (reader.Read())
                    {
                        clsFemaleLabDay4FertilizationAssesmentVO ObjFer = new clsFemaleLabDay4FertilizationAssesmentVO();
                        ObjFer.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ObjFer.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);

                        //By Anjali.............
                        ObjFer.SerialOccyteNo = (long)DALHelper.HandleDBNull(reader["SerialOccyteNo"]);

                        //................
                        ObjFer.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        ObjFer.Time = (DateTime?)DALHelper.HandleDate(reader["Time"]);

                        ObjFer.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);
                        ObjFer.PlanTreatment = (string)DALHelper.HandleDBNull(reader["Protocol"]);

                        ObjFer.SrcOocyteID = (long)DALHelper.HandleDBNull(reader["SrcOocyteID"]);
                        ObjFer.SrcOocyteDescription = (string)DALHelper.HandleDBNull(reader["SOOocyte"]);

                        ObjFer.OocyteDonorID = (string)DALHelper.HandleDBNull(reader["OSCode"]);
                       

                        ObjFer.SrcOfSemen = (long)DALHelper.HandleDBNull(reader["SrcOfSemen"]);
                        ObjFer.SrcOfSemenDescription = (string)DALHelper.HandleDBNull(reader["SOSemen"]);

                        ObjFer.SemenDonorID = (string)DALHelper.HandleDBNull(reader["SSCode"]);
       

                        ObjFer.SelectedFePlan.ID = (long)(DALHelper.HandleDBNull(reader["FertilisationStage"]));
                        ObjFer.SelectedFePlan.Description = (string)(DALHelper.HandleDBNull(reader["Fertilization"]));
                      
                        ObjFer.Score = (long)(DALHelper.HandleDBNull(reader["Score"]));
                        ObjFer.PV = (bool)(DALHelper.HandleDBNull(reader["PV"]));
                        ObjFer.XFactor = (bool)(DALHelper.HandleDBNull(reader["XFactor"]));
                        ObjFer.Others = (string)(DALHelper.HandleDBNull(reader["Others"]));
                        ObjFer.ProceedDay5 = (bool)(DALHelper.HandleDBNull(reader["ProceedDay5"]));

                        ObjFer.SelectedGrade.ID = (long)(DALHelper.HandleDBNull(reader["GradeID"]));
                        ObjFer.SelectedGrade.Description = (string)(DALHelper.HandleDBNull(reader["Grade"]));

                        ObjFer.SelectedFragmentation.ID = (long)(DALHelper.HandleDBNull(reader["FragmentationID"]));
                        ObjFer.SelectedFragmentation.Description = (string)(DALHelper.HandleDBNull(reader["Fragmentation"]));
                        ObjFer.SelectedBlastomereSymmetry.ID = (long)(DALHelper.HandleDBNull(reader["BlastomereSymmetryID"]));
                        ObjFer.SelectedBlastomereSymmetry.Description = (string)(DALHelper.HandleDBNull(reader["BlastomereSymmetry"]));

                        ObjFer.SelectedPlan.ID = (long)(DALHelper.HandleDBNull(reader["PlanID"]));
                        ObjFer.SelectedPlan.Description = (string)(DALHelper.HandleDBNull(reader["PlanName"]));
                        ObjFer.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ObjFer.FileContents = (byte[])(DALHelper.HandleDBNull(reader["FileContents"]));


                        //For getting details
                        clsBaseIVFLabDayDAL objBaseDAL = clsBaseIVFLabDayDAL.GetInstance();
                        clsGetDay4MediaAndCalcDetailsBizActionVO BizActionDetails = new clsGetDay4MediaAndCalcDetailsBizActionVO();

                        BizActionDetails.ID = BizAction.ID;
                        BizActionDetails.DetailID = ObjFer.ID;
                        BizActionDetails = (clsGetDay4MediaAndCalcDetailsBizActionVO)objBaseDAL.GetFemaleLabDay4MediaAndCalDetails(BizActionDetails, UserVo);
                        ObjFer.Day4CalculateDetails = BizActionDetails.LabDayCalDetails;

                        //For getting Media details
                        clsBaseIVFLabDayDAL objBaseDAL2 = clsBaseIVFLabDayDAL.GetInstance();
                        clsGetAllDayMediaDetailsBizActionVO BizActionMedia = new clsGetAllDayMediaDetailsBizActionVO();
                        BizActionMedia.ID = BizAction.ID;
                        BizActionMedia.DetailID = ObjFer.ID;
                        BizActionMedia.LabDay = 4;
                        BizActionMedia = (clsGetAllDayMediaDetailsBizActionVO)objBaseDAL2.GetAllDayMediaDetails(BizActionMedia, UserVo);
                        ObjFer.MediaDetails = BizActionMedia.MediaList;

                        BizAction.LabDay4.FertilizationAssesmentDetails.Add(ObjFer);

                    }
                    reader.NextResult();
                    if (BizAction.LabDay4.SemenDetails == null)
                        BizAction.LabDay4.SemenDetails = new clsFemaleSemenDetailsVO();


                    while (reader.Read())
                    {
                        BizAction.LabDay4.SemenDetails.MOSP = (string)(DALHelper.HandleDBNull(reader["MOSP"]));
                        BizAction.LabDay4.SemenDetails.SOS = (string)(DALHelper.HandleDBNull(reader["SOS"]));

                        BizAction.LabDay4.SemenDetails.MethodOfSpermPreparation = (long)(DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]));
                        BizAction.LabDay4.SemenDetails.SourceOfSemen = (long)(DALHelper.HandleDBNull(reader["SourceOfSemen"]));

                        BizAction.LabDay4.SemenDetails.PreSelfVolume = (string)(DALHelper.HandleDBNull(reader["PreSelfVolume"]));
                        BizAction.LabDay4.SemenDetails.PreSelfConcentration = (string)(DALHelper.HandleDBNull(reader["PreSelfConcentration"]));
                        BizAction.LabDay4.SemenDetails.PreSelfMotality = (string)(DALHelper.HandleDBNull(reader["PreSelfMotality"]));
                        BizAction.LabDay4.SemenDetails.PreSelfWBC = (string)(DALHelper.HandleDBNull(reader["PreSelfWBC"]));

                        BizAction.LabDay4.SemenDetails.PreDonorVolume = (string)(DALHelper.HandleDBNull(reader["PreDonorVolume"]));
                        BizAction.LabDay4.SemenDetails.PreDonorConcentration = (string)(DALHelper.HandleDBNull(reader["PreDonorConcentration"]));
                        BizAction.LabDay4.SemenDetails.PreDonorMotality = (string)(DALHelper.HandleDBNull(reader["PreDonorMotality"]));
                        BizAction.LabDay4.SemenDetails.PreDonorWBC = (string)(DALHelper.HandleDBNull(reader["PreDonorWBC"]));


                        BizAction.LabDay4.SemenDetails.PostSelfVolume = (string)(DALHelper.HandleDBNull(reader["PostSelfVolume"]));
                        BizAction.LabDay4.SemenDetails.PostSelfConcentration = (string)(DALHelper.HandleDBNull(reader["PostSelfConcentration"]));
                        BizAction.LabDay4.SemenDetails.PostSelfMotality = (string)(DALHelper.HandleDBNull(reader["PostSelfMotality"]));
                        BizAction.LabDay4.SemenDetails.PostSelfWBC = (string)(DALHelper.HandleDBNull(reader["PostSelfWBC"]));

                        BizAction.LabDay4.SemenDetails.PostDonorVolume = (string)(DALHelper.HandleDBNull(reader["PostDonorVolume"]));
                        BizAction.LabDay4.SemenDetails.PostDonorConcentration = (string)(DALHelper.HandleDBNull(reader["PostDonorConcentration"]));
                        BizAction.LabDay4.SemenDetails.PostDonorMotality = (string)(DALHelper.HandleDBNull(reader["PostDonorMotality"]));
                        BizAction.LabDay4.SemenDetails.PostDonorWBC = (string)(DALHelper.HandleDBNull(reader["PostDonorWBC"]));
                    }

                    reader.NextResult();
                    if (BizAction.LabDay4.FUSetting == null)
                        BizAction.LabDay4.FUSetting = new List<FileUpload>();
                    while (reader.Read())
                    {
                        FileUpload ObjFile = new FileUpload();
                        ObjFile.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ObjFile.Index = (Int32)(DALHelper.HandleDBNull(reader["FileIndex"]));
                        ObjFile.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ObjFile.Data = (byte[])(DALHelper.HandleDBNull(reader["Value"]));
                        BizAction.LabDay4.FUSetting.Add(ObjFile);
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

        public override IValueObject GetFemaleLabDay4MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay4MediaAndCalcDetailsBizActionVO BizAction = (valueObject) as clsGetDay4MediaAndCalcDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMediaDetailsAndCalculateDetailForDay4");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "DetailID", DbType.Int64, BizAction.DetailID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.LabDayCalDetails == null)
                        BizAction.LabDayCalDetails = new clsFemaleLabDay4CalculateDetailsVO();

                    while (reader.Read())
                    {
                        
                        BizAction.LabDayCalDetails.FertilizationID = (long)(DALHelper.HandleDBNull(reader["DetailID"]));
                        BizAction.LabDayCalDetails.Compaction = (bool)(DALHelper.HandleDBNull(reader["Compaction"]));
                        BizAction.LabDayCalDetails.SignsOfBlastocoel = (bool)(DALHelper.HandleDBNull(reader["SignsOfBlastocoel"]));

                       
                    }

                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizAction;

        }

        public override IValueObject GetLabDay2Score(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay3ScoreBizActionVO BizActionObj = valueObject as clsGetDay3ScoreBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFemaleDay2Score");
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.CoupleUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (BizActionObj.Day3Score == null)
                        BizActionObj.Day3Score = new List<clsFemaleLabDay4FertilizationAssesmentVO>();
                    while (reader.Read())
                    {
                        clsFemaleLabDay4FertilizationAssesmentVO Obj = new clsFemaleLabDay4FertilizationAssesmentVO();

                        Obj.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);
                        Obj.Score = (long)DALHelper.HandleDBNull(reader["Score"]);
                        Obj.SelectedGrade.Description = (string)DALHelper.HandleDBNull(reader["Grade"]);
                        Obj.SelectedFePlan.Description = (string)DALHelper.HandleDBNull(reader["FertilisationStage"]);
                        Obj.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);

                        BizActionObj.Day3Score.Add(Obj);
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
        public override IValueObject GetLabDay3Score(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay3ScoreBizActionVO BizActionObj = valueObject as clsGetDay3ScoreBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFemaleDay3Score");

                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.CoupleUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Day3Score == null)
                        BizActionObj.Day3Score = new List<clsFemaleLabDay4FertilizationAssesmentVO>();
                    while (reader.Read())
                    {
                        clsFemaleLabDay4FertilizationAssesmentVO Obj = new clsFemaleLabDay4FertilizationAssesmentVO();

                        Obj.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);
                        Obj.Score = (long)DALHelper.HandleDBNull(reader["Score"]);
                        Obj.SelectedGrade.Description = (string)DALHelper.HandleDBNull(reader["Grade"]);
                        Obj.SelectedFePlan.Description = (string)DALHelper.HandleDBNull(reader["FertilisationStage"]);
                        Obj.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);

                        BizActionObj.Day3Score.Add(Obj);
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
       
       
        public override IValueObject GetAllDayMediaDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAllDayMediaDetailsBizActionVO BizAction = (valueObject) as clsGetAllDayMediaDetailsBizActionVO;
             try
             {
                 DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMediaDetailsForAllDay");

                 dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                 dbServer.AddInParameter(command, "DetailID", DbType.Int64, BizAction.DetailID);
                 dbServer.AddInParameter(command, "Day", DbType.Int32, BizAction.LabDay);

                 DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                 if (reader.HasRows)
                 {
                     if (BizAction.MediaList == null)
                         BizAction.MediaList=new List<clsFemaleMediaDetailsVO>();
                     while (reader.Read())
                     {
                         clsFemaleMediaDetailsVO Media = new clsFemaleMediaDetailsVO();

                         Media.Date = (DateTime?)(DALHelper.HandleDate(reader["Date"]));
                         Media.ItemName = (string)(DALHelper.HandleDBNull(reader["MediaName"]));
                         Media.Company = (string)(DALHelper.HandleDBNull(reader["Company"]));
                         Media.BatchCode = (string)(DALHelper.HandleDBNull(reader["LotNo"]));
                         Media.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                         Media.PH = (bool)(DALHelper.HandleDBNull(reader["PH"]));
                         Media.OSM = (bool)(DALHelper.HandleDBNull(reader["OSM"]));
                         Media.VolumeUsed = (string)(DALHelper.HandleDBNull(reader["VolumeUsed"]));
                         Media.SelectedStatus.ID = (long)(DALHelper.HandleDBNull(reader["Status"]));
                         Media.SelectedStatus = Media.Status.FirstOrDefault(q => q.ID == Media.SelectedStatus.ID);
                         Media.StoreID = (long)(DALHelper.HandleDBNull(reader["StoreID"]));
                         Media.BatchID = (long)(DALHelper.HandleDBNull(reader["BatchID"]));
                         BizAction.MediaList.Add(Media);
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

        #region Day 5
        public override IValueObject AddLabDay5(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddLabDay5BizActionVO BizActionObj = valueObject as clsAddLabDay5BizActionVO;

            if (BizActionObj.Day5Details.ID == 0)
                BizActionObj = AddDay5(BizActionObj, UserVo);
            else
                BizActionObj = UpdateDay5(BizActionObj, UserVo);

            return valueObject;   
        }

        private clsAddLabDay5BizActionVO AddDay5(clsAddLabDay5BizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsFemaleLabDay5VO ObjDay4VO = BizActionObj.Day5Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay5");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, ObjDay4VO.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, ObjDay4VO.CoupleUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjDay4VO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, ObjDay4VO.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, ObjDay4VO.EmbryologistID);
                dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, ObjDay4VO.AssEmbryologistID);
                dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, ObjDay4VO.AnesthetistID);
                dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, ObjDay4VO.AssAnesthetistID);
                dbServer.AddInParameter(command, "IVFCycleCount", DbType.Int64, ObjDay4VO.IVFCycleCount);
                dbServer.AddInParameter(command, "SourceNeedleID", DbType.Int64, ObjDay4VO.SourceNeedleID);
                dbServer.AddInParameter(command, "InfectionObserved", DbType.String, ObjDay4VO.InfectionObserved);
                dbServer.AddInParameter(command, "TotNoOfOocytes", DbType.Int32, ObjDay4VO.TotNoOfOocytes);
                dbServer.AddInParameter(command, "TotNoOf2PN", DbType.Int32, ObjDay4VO.TotNoOf2PN);
                dbServer.AddInParameter(command, "TotNoOf3PN", DbType.Int32, ObjDay4VO.TotNoOf3PN);
                dbServer.AddInParameter(command, "TotNoOf2PB", DbType.Int32, ObjDay4VO.TotNoOf2PB);
                dbServer.AddInParameter(command, "ToNoOfMI", DbType.Int32, ObjDay4VO.ToNoOfMI);
                dbServer.AddInParameter(command, "ToNoOfMII", DbType.Int32, ObjDay4VO.ToNoOfMII);
                dbServer.AddInParameter(command, "ToNoOfGV", DbType.Int32, ObjDay4VO.ToNoOfGV);
                dbServer.AddInParameter(command, "ToNoOfDeGenerated", DbType.Int32, ObjDay4VO.ToNoOfDeGenerated);
                dbServer.AddInParameter(command, "ToNoOfLost ", DbType.Int32, ObjDay4VO.ToNoOfLost);
                dbServer.AddInParameter(command, "IsFreezed ", DbType.Boolean, ObjDay4VO.IsFreezed);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDay4VO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Day5Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.Day5Details.FertilizationAssesmentDetails != null && BizActionObj.Day5Details.FertilizationAssesmentDetails.Count > 0)
                {
                    foreach (var item1 in ObjDay4VO.FertilizationAssesmentDetails)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay5Details");

                        dbServer.AddInParameter(command2, "Day5ID", DbType.Int64, ObjDay4VO.ID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "OoNo", DbType.Int64, item1.OoNo);
                        //By Anjali..........
                        dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, item1.SerialOccyteNo);
                        //...................
                        dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, item1.PlanTreatmentID);
                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, item1.Date);
                        dbServer.AddInParameter(command2, "Time", DbType.DateTime, item1.Time);
                        dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, item1.SrcOocyteID);
                        dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, item1.OocyteDonorID);
                        dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, item1.SrcOfSemen);
                        dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, item1.SemenDonorID);
                        dbServer.AddInParameter(command2, "FileName", DbType.String, item1.FileName);
                        dbServer.AddInParameter(command2, "FileContents", DbType.Binary, item1.FileContents);

                        if (item1.SelectedFePlan != null)
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, item1.SelectedFePlan.ID);
                        else
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);

                        if (item1.SelectedGrade != null)
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, item1.SelectedGrade.ID);
                        else
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);

                        if (item1.SelectedICM != null)
                            dbServer.AddInParameter(command2, "ICM", DbType.Int64, item1.SelectedICM.ID);
                        else
                            dbServer.AddInParameter(command2, "ICM", DbType.Int64, 0);
                        if (item1.SelectedTrophectoderm != null)
                            dbServer.AddInParameter(command2, "Trophectoderm", DbType.Int64, item1.SelectedTrophectoderm.ID);
                        else
                            dbServer.AddInParameter(command2, "Trophectoderm", DbType.Int64, 0);
                        if (item1.SelectedExpansion != null)
                            dbServer.AddInParameter(command2, "Expansion", DbType.Int64, item1.SelectedExpansion.ID);
                        else
                            dbServer.AddInParameter(command2, "Expansion", DbType.Int64, 0);

                        if (item1.SelectedBlastocytsGrade != null)
                            dbServer.AddInParameter(command2, "BlastocytsGrade", DbType.Int64, item1.SelectedBlastocytsGrade.ID);
                        else
                            dbServer.AddInParameter(command2, "BlastocytsGrade", DbType.Int64, 0);

                        dbServer.AddInParameter(command2, "Score", DbType.Int64, item1.Score);
                        dbServer.AddInParameter(command2, "PV", DbType.Boolean, item1.PV);
                        dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, item1.XFactor);
                        dbServer.AddInParameter(command2, "Others", DbType.String, item1.Others);
                        //dbServer.AddInParameter(command2, "ProceedDay6", DbType.Boolean, item1.ProceedDay6);
                        if (item1.SelectedPlan !=null)
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, item1.SelectedPlan.ID);
                        else
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);
                        if (item1.SelectedPlan.ID == 3)
                        {
                            dbServer.AddInParameter(command2, "ProceedDay6", DbType.Boolean, true);
                        }
                        else
                        {
                            dbServer.AddInParameter(command2, "ProceedDay6", DbType.Boolean, false);
                        }

                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);

                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                        item1.ID = (long)dbServer.GetParameterValue(command2, "ID");

                        if (item1.MediaDetails != null && item1.MediaDetails.Count > 0)
                        {
                            foreach (var item3 in item1.MediaDetails)
                            {
                                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");

                                dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                                dbServer.AddInParameter(command4, "DetailID", DbType.Int64, item1.ID);
                                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command4, "Date", DbType.DateTime, item3.Date);
                                dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day5);
                                dbServer.AddInParameter(command4, "MediaName", DbType.String, item3.ItemName);
                                dbServer.AddInParameter(command4, "Company", DbType.String, item3.Company);
                                dbServer.AddInParameter(command4, "LotNo", DbType.String, item3.BatchCode);
                                dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, item3.ExpiryDate);
                                dbServer.AddInParameter(command4, "PH", DbType.Boolean, item3.PH);
                                dbServer.AddInParameter(command4, "OSM", DbType.Boolean, item3.OSM);
                                dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, item3.VolumeUsed);
                                dbServer.AddInParameter(command4, "Status", DbType.Int64, item3.SelectedStatus.ID);
                                dbServer.AddInParameter(command4, "BatchID", DbType.Int64, item3.BatchID);
                                dbServer.AddInParameter(command4, "StoreID", DbType.Int64, item3.StoreID);  

                                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                item3.ID = (long)dbServer.GetParameterValue(command4, "ID");
                            }
                        }
                        if (item1.Day5CalculateDetails != null)
                        {
                            var item6 = item1.Day5CalculateDetails;

                            DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay5CalculateDetails");

                            dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                            dbServer.AddInParameter(command5, "DetailID", DbType.Int64, item1.ID);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command5, "BlastocoelsCavity", DbType.Boolean, item6.BlastocoelsCavity);
                            dbServer.AddInParameter(command5, "TightlyPackedCells", DbType.Boolean, item6.TightlyPackedCells);
                            dbServer.AddInParameter(command5, "FormingEpithelium", DbType.Boolean, item6.FormingEpithelium);

                            dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item6.ID);
                            dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus1 = dbServer.ExecuteNonQuery(command5, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command5, "ResultStatus");
                            item6.ID = (long)dbServer.GetParameterValue(command5, "ID");

                        }
                    }
                }

                if (BizActionObj.Day5Details.FUSetting != null && BizActionObj.Day5Details.FUSetting.Count > 0)
                {
                    foreach (var item2 in ObjDay4VO.FUSetting)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                        dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day5);
                        dbServer.AddInParameter(command3, "FileName", DbType.String, item2.FileName);
                        dbServer.AddInParameter(command3, "FileIndex", DbType.Int32, item2.Index);
                        dbServer.AddInParameter(command3, "Value", DbType.Binary, item2.Data);
                        dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                        dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                        item2.ID = (long)dbServer.GetParameterValue(command3, "ID");
                    }
                }

                if (BizActionObj.Day5Details.SemenDetails != null)
                {
                    var item4 = BizActionObj.Day5Details.SemenDetails;
                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");

                    dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day5);
                    dbServer.AddInParameter(command4, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day5Details.SemenDetails.MethodOfSpermPreparation);
                    dbServer.AddInParameter(command4, "SourceOfSemen", DbType.Int64, BizActionObj.Day5Details.SemenDetails.SourceOfSemen);
                    dbServer.AddInParameter(command4, "PreSelfVolume", DbType.String, item4.PreSelfVolume);
                    dbServer.AddInParameter(command4, "PreSelfConcentration", DbType.String, item4.PreSelfConcentration);
                    dbServer.AddInParameter(command4, "PreSelfMotality", DbType.String, item4.PreSelfMotality);
                    dbServer.AddInParameter(command4, "PreSelfWBC", DbType.String, item4.PreSelfWBC);
                    dbServer.AddInParameter(command4, "PreDonorVolume", DbType.String, item4.PreDonorVolume);
                    dbServer.AddInParameter(command4, "PreDonorConcentration", DbType.String, item4.PreDonorConcentration);
                    dbServer.AddInParameter(command4, "PreDonorMotality", DbType.String, item4.PreDonorMotality);
                    dbServer.AddInParameter(command4, "PreDonorWBC", DbType.String, item4.PreDonorWBC);
                    dbServer.AddInParameter(command4, "PostSelfVolume", DbType.String, item4.PostSelfVolume);
                    dbServer.AddInParameter(command4, "PostSelfConcentration", DbType.String, item4.PostSelfConcentration);
                    dbServer.AddInParameter(command4, "PostSelfMotality", DbType.String, item4.PostSelfMotality);
                    dbServer.AddInParameter(command4, "PostSelfWBC", DbType.String, item4.PostSelfWBC);
                    dbServer.AddInParameter(command4, "PostDonorVolume ", DbType.String, item4.PostDonorVolume);
                    dbServer.AddInParameter(command4, "PostDonorConcentration", DbType.String, item4.PostDonorConcentration);
                    dbServer.AddInParameter(command4, "PostDonorMotality", DbType.String, item4.PostDonorMotality);
                    dbServer.AddInParameter(command4, "PostDonorWBC", DbType.String, item4.PostDonorWBC);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item4.ID);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    item4.ID = (long)dbServer.GetParameterValue(command4, "ID");
                }
                if (BizActionObj.Day5Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                    obj.LabDaysSummary = BizActionObj.Day5Details.LabDaySummary;
                    obj.LabDaysSummary.OocyteID = BizActionObj.Day5Details.ID;
                    obj.LabDaysSummary.CoupleID = BizActionObj.Day5Details.CoupleID;
                    obj.LabDaysSummary.CoupleUnitID = BizActionObj.Day5Details.CoupleUnitID;
                    obj.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay5;
                    obj.LabDaysSummary.Priority = 1;
                    obj.LabDaysSummary.ProcDate = BizActionObj.Day5Details.Date;
                    obj.LabDaysSummary.ProcTime = BizActionObj.Day5Details.Time;
                    obj.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    obj.LabDaysSummary.IsFreezed = BizActionObj.Day5Details.IsFreezed;

                    obj.IsUpdate = false;
                    obj = (clsAddUpdateLabDaysSummaryBizActionVO)objBaseDAL.AddUpdateLabDaysSummary(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Day5Details.LabDaySummary.ID = obj.LabDaysSummary.ID;
                }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
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

        private clsAddLabDay5BizActionVO UpdateDay5(clsAddLabDay5BizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsFemaleLabDay5VO ObjDay4VO = BizActionObj.Day5Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateFemaleLabDay5");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjDay4VO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                ObjDay4VO.UnitID = UserVo.UserLoginInfo.UnitId;
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, ObjDay4VO.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, ObjDay4VO.CoupleUnitID);

                dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjDay4VO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, ObjDay4VO.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, ObjDay4VO.EmbryologistID);
                dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, ObjDay4VO.AssEmbryologistID);
                dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, ObjDay4VO.AnesthetistID);
                dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, ObjDay4VO.AssAnesthetistID);
                dbServer.AddInParameter(command, "IVFCycleCount", DbType.Int64, ObjDay4VO.IVFCycleCount);
                dbServer.AddInParameter(command, "SourceNeedleID", DbType.Int64, ObjDay4VO.SourceNeedleID);
                dbServer.AddInParameter(command, "InfectionObserved", DbType.String, ObjDay4VO.InfectionObserved);
                dbServer.AddInParameter(command, "TotNoOfOocytes", DbType.Int32, ObjDay4VO.TotNoOfOocytes);
                dbServer.AddInParameter(command, "TotNoOf2PN", DbType.Int32, ObjDay4VO.TotNoOf2PN);
                dbServer.AddInParameter(command, "TotNoOf3PN", DbType.Int32, ObjDay4VO.TotNoOf3PN);
                dbServer.AddInParameter(command, "TotNoOf2PB", DbType.Int32, ObjDay4VO.TotNoOf2PB);
                dbServer.AddInParameter(command, "ToNoOfMI", DbType.Int32, ObjDay4VO.ToNoOfMI);
                dbServer.AddInParameter(command, "ToNoOfMII", DbType.Int32, ObjDay4VO.ToNoOfMII);
                dbServer.AddInParameter(command, "ToNoOfGV", DbType.Int32, ObjDay4VO.ToNoOfGV);
                dbServer.AddInParameter(command, "ToNoOfDeGenerated", DbType.Int32, ObjDay4VO.ToNoOfDeGenerated);
                dbServer.AddInParameter(command, "ToNoOfLost ", DbType.Int32, ObjDay4VO.ToNoOfLost);
                dbServer.AddInParameter(command, "IsFreezed ", DbType.Boolean, ObjDay4VO.IsFreezed);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                if (BizActionObj.Day5Details.FertilizationAssesmentDetails != null && BizActionObj.Day5Details.FertilizationAssesmentDetails.Count > 0)
                {
                    DbCommand command11 = dbServer.GetStoredProcCommand("CIMS_DeleteLab5Details");

                    dbServer.AddInParameter(command11, "Day5ID", DbType.Int64, ObjDay4VO.ID);
                    dbServer.AddInParameter(command11, "LabDay", DbType.Int32, IVFLabDay.Day5);
                    int intStatus2 = dbServer.ExecuteNonQuery(command11, trans);
                }

                if (BizActionObj.Day5Details.FertilizationAssesmentDetails != null && BizActionObj.Day5Details.FertilizationAssesmentDetails.Count > 0)
                {
                    foreach (var item1 in ObjDay4VO.FertilizationAssesmentDetails)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay5Details");

                        dbServer.AddInParameter(command2, "Day5ID", DbType.Int64, ObjDay4VO.ID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "OoNo", DbType.Int64, item1.OoNo);
                        //By Anjali..........
                        dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, item1.SerialOccyteNo);
                        //...................
                        dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, item1.PlanTreatmentID);
                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, item1.Date);
                        dbServer.AddInParameter(command2, "Time", DbType.DateTime, item1.Time);
                        dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, item1.SrcOocyteID);
                        dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, item1.OocyteDonorID);
                        dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, item1.SrcOfSemen);
                        dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, item1.SemenDonorID);
                        dbServer.AddInParameter(command2, "FileName", DbType.String, item1.FileName);
                        dbServer.AddInParameter(command2, "FileContents", DbType.Binary, item1.FileContents);

                        if (item1.SelectedFePlan != null)
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, item1.SelectedFePlan.ID);
                        else
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);

                        if (item1.SelectedGrade != null)
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, item1.SelectedGrade.ID);
                        else
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);

                        if (item1.SelectedICM != null)
                            dbServer.AddInParameter(command2, "ICM", DbType.Int64, item1.SelectedICM.ID);
                        else
                            dbServer.AddInParameter(command2, "ICM", DbType.Int64, 0);
                        if (item1.SelectedTrophectoderm != null)
                            dbServer.AddInParameter(command2, "Trophectoderm", DbType.Int64, item1.SelectedTrophectoderm.ID);
                        else
                            dbServer.AddInParameter(command2, "Trophectoderm", DbType.Int64, 0);
                        if (item1.SelectedExpansion != null)
                            dbServer.AddInParameter(command2, "Expansion", DbType.Int64, item1.SelectedExpansion.ID);
                        else
                            dbServer.AddInParameter(command2, "Expansion", DbType.Int64, 0);

                        if (item1.SelectedBlastocytsGrade != null)
                            dbServer.AddInParameter(command2, "BlastocytsGrade", DbType.Int64, item1.SelectedBlastocytsGrade.ID);
                        else
                            dbServer.AddInParameter(command2, "BlastocytsGrade", DbType.Int64, 0);
                        dbServer.AddInParameter(command2, "Score", DbType.Int64, item1.Score);
                        dbServer.AddInParameter(command2, "PV", DbType.Boolean, item1.PV);
                        dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, item1.XFactor);
                        dbServer.AddInParameter(command2, "Others", DbType.String, item1.Others);
                        //dbServer.AddInParameter(command2, "ProceedDay6", DbType.Boolean, item1.ProceedDay6);
                        if (item1.SelectedPlan !=null)
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, item1.SelectedPlan.ID);
                        else
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);

                        if (item1.SelectedPlan.ID == 3)
                        {
                            dbServer.AddInParameter(command2, "ProceedDay6", DbType.Boolean,true);
                        }
                        else
                        {
                            dbServer.AddInParameter(command2, "ProceedDay6", DbType.Boolean, false);
                        }
                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);

                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                        item1.ID = (long)dbServer.GetParameterValue(command2, "ID");

                        if (item1.MediaDetails != null && item1.MediaDetails.Count > 0)
                        {
                            foreach (var item3 in item1.MediaDetails)
                            {
                                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");

                                dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                                dbServer.AddInParameter(command4, "DetailID", DbType.Int64, item1.ID);
                                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command4, "Date", DbType.DateTime, item3.Date);
                                dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day5);
                                dbServer.AddInParameter(command4, "MediaName", DbType.String, item3.ItemName);
                                dbServer.AddInParameter(command4, "Company", DbType.String, item3.Company);
                                dbServer.AddInParameter(command4, "LotNo", DbType.String, item3.BatchCode);
                                dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, item3.ExpiryDate);
                                dbServer.AddInParameter(command4, "PH", DbType.Boolean, item3.PH);
                                dbServer.AddInParameter(command4, "OSM", DbType.Boolean, item3.OSM);
                                dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, item3.VolumeUsed);
                                dbServer.AddInParameter(command4, "Status", DbType.Int64, item3.SelectedStatus.ID);
                                dbServer.AddInParameter(command4, "BatchID", DbType.Int64, item3.BatchID);
                                dbServer.AddInParameter(command4, "StoreID", DbType.Int64, item3.StoreID);  
                                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                item3.ID = (long)dbServer.GetParameterValue(command4, "ID");
                            }
                        }
                        if (item1.Day5CalculateDetails != null)
                        {
                            var item6 = item1.Day5CalculateDetails;

                            DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDay5CalculateDetails");

                            dbServer.AddInParameter(command5, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                            dbServer.AddInParameter(command5, "DetailID", DbType.Int64, item1.ID);
                            dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command5, "ProcedureDate", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command5, "ProcedureTime", DbType.DateTime, DateTime.Now);
                            dbServer.AddInParameter(command5, "BlastocoelsCavity", DbType.Boolean, item6.BlastocoelsCavity);
                            dbServer.AddInParameter(command5, "TightlyPackedCells", DbType.Boolean, item6.TightlyPackedCells);
                            dbServer.AddInParameter(command5, "FormingEpithelium", DbType.Boolean, item6.FormingEpithelium);

                            dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item6.ID);
                            dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus1 = dbServer.ExecuteNonQuery(command5, trans);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command5, "ResultStatus");
                            item6.ID = (long)dbServer.GetParameterValue(command5, "ID");

                        }
                    }
                }


                if (BizActionObj.Day5Details.FUSetting != null && BizActionObj.Day5Details.FUSetting.Count > 0)
                {
                    DbCommand command13 = dbServer.GetStoredProcCommand("CIMS_DeleteLab5UploadFileDetails");

                    dbServer.AddInParameter(command13, "Day5ID", DbType.Int64, ObjDay4VO.ID);
                    dbServer.AddInParameter(command13, "LabDay", DbType.Int32, IVFLabDay.Day5);
                    dbServer.AddInParameter(command13, "UnitID", DbType.Int64, ObjDay4VO.UnitID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command13, trans);
                }

                if (BizActionObj.Day5Details.FUSetting != null && BizActionObj.Day5Details.FUSetting.Count > 0)
                {
                    foreach (var item2 in ObjDay4VO.FUSetting)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                        dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day5);
                        dbServer.AddInParameter(command3, "FileName", DbType.String, item2.FileName);
                        dbServer.AddInParameter(command3, "FileIndex", DbType.Int32, item2.Index);
                        dbServer.AddInParameter(command3, "Value", DbType.Binary, item2.Data);
                        dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                        dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                        item2.ID = (long)dbServer.GetParameterValue(command3, "ID");
                    }
                }

                if (BizActionObj.Day5Details.SemenDetails != null)
                {
                    DbCommand command12 = dbServer.GetStoredProcCommand("CIMS_DeleteLab5SemenDetails");

                    dbServer.AddInParameter(command12, "Day5ID ", DbType.Int64, ObjDay4VO.ID);
                    dbServer.AddInParameter(command12, "LabDay", DbType.Int32, IVFLabDay.Day5);
                    int intStatus2 = dbServer.ExecuteNonQuery(command12, trans);
                }

                if (BizActionObj.Day5Details.SemenDetails != null)
                {
                    var item4 = BizActionObj.Day5Details.SemenDetails;
                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");

                    dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay4VO.ID);
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day5);
                    dbServer.AddInParameter(command4, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day5Details.SemenDetails.MethodOfSpermPreparation);
                    dbServer.AddInParameter(command4, "SourceOfSemen", DbType.Int64, BizActionObj.Day5Details.SemenDetails.SourceOfSemen);
                    dbServer.AddInParameter(command4, "PreSelfVolume", DbType.String, item4.PreSelfVolume);
                    dbServer.AddInParameter(command4, "PreSelfConcentration", DbType.String, item4.PreSelfConcentration);
                    dbServer.AddInParameter(command4, "PreSelfMotality", DbType.String, item4.PreSelfMotality);
                    dbServer.AddInParameter(command4, "PreSelfWBC", DbType.String, item4.PreSelfWBC);
                    dbServer.AddInParameter(command4, "PreDonorVolume", DbType.String, item4.PreDonorVolume);
                    dbServer.AddInParameter(command4, "PreDonorConcentration", DbType.String, item4.PreDonorConcentration);
                    dbServer.AddInParameter(command4, "PreDonorMotality", DbType.String, item4.PreDonorMotality);
                    dbServer.AddInParameter(command4, "PreDonorWBC", DbType.String, item4.PreDonorWBC);
                    dbServer.AddInParameter(command4, "PostSelfVolume", DbType.String, item4.PostSelfVolume);
                    dbServer.AddInParameter(command4, "PostSelfConcentration", DbType.String, item4.PostSelfConcentration);
                    dbServer.AddInParameter(command4, "PostSelfMotality", DbType.String, item4.PostSelfMotality);
                    dbServer.AddInParameter(command4, "PostSelfWBC", DbType.String, item4.PostSelfWBC);
                    dbServer.AddInParameter(command4, "PostDonorVolume ", DbType.String, item4.PostDonorVolume);
                    dbServer.AddInParameter(command4, "PostDonorConcentration", DbType.String, item4.PostDonorConcentration);
                    dbServer.AddInParameter(command4, "PostDonorMotality", DbType.String, item4.PostDonorMotality);
                    dbServer.AddInParameter(command4, "PostDonorWBC", DbType.String, item4.PostDonorWBC);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item4.ID);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    item4.ID = (long)dbServer.GetParameterValue(command4, "ID");
                }
                if (BizActionObj.Day5Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                    obj.LabDaysSummary = BizActionObj.Day5Details.LabDaySummary;
                    obj.LabDaysSummary.OocyteID = BizActionObj.Day5Details.ID;
                    obj.LabDaysSummary.CoupleID = BizActionObj.Day5Details.CoupleID;
                    obj.LabDaysSummary.CoupleUnitID = BizActionObj.Day5Details.CoupleUnitID;
                    obj.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay5;
                    obj.LabDaysSummary.Priority = 1;
                    obj.LabDaysSummary.ProcDate = BizActionObj.Day5Details.Date;
                    obj.LabDaysSummary.ProcTime = BizActionObj.Day5Details.Time;
                    obj.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    obj.LabDaysSummary.IsFreezed = BizActionObj.Day5Details.IsFreezed;

                    
                    obj.IsUpdate = true;

                    obj = (clsAddUpdateLabDaysSummaryBizActionVO)objBaseDAL.AddUpdateLabDaysSummary(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Day5Details.LabDaySummary.ID = obj.LabDaysSummary.ID;
                }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
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

        public override IValueObject GetLabDay4ForDay5(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLabDay4ForLabDay5BizActionVO BizActionObj = valueObject as clsGetLabDay4ForLabDay5BizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFemaleLabDay4ForLabDay5");
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.CoupleUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Day5Details == null)
                        BizActionObj.Day5Details = new List<clsFemaleLabDay5FertilizationAssesmentVO>();
                    while (reader.Read())
                    {
                        clsFemaleLabDay5FertilizationAssesmentVO Obj = new clsFemaleLabDay5FertilizationAssesmentVO();
                       
                        Obj.ID = (long)DALHelper.HandleDBNull(reader["DetailID"]);
                        Obj.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);

                        //By Anjali.............
                        Obj.SerialOccyteNo = (long)DALHelper.HandleDBNull(reader["SerialOccyteNo"]);

                        //................

                        Obj.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        Obj.Time = (DateTime?)DALHelper.HandleDate(reader["Time"]);

                        Obj.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);
                        Obj.PlanTreatment = (string)DALHelper.HandleDBNull(reader["Protocol"]);

                        Obj.SrcOocyteID = (long)DALHelper.HandleDBNull(reader["SrcOocyteID"]);
                        Obj.SrcOocyteDescription = (string)DALHelper.HandleDBNull(reader["SOOocyte"]);

                        Obj.OocyteDonorID = (string)DALHelper.HandleDBNull(reader["OSCode"]);
                       

                        Obj.SrcOfSemen = (long)DALHelper.HandleDBNull(reader["SrcOfSemen"]);
                        Obj.SrcOfSemenDescription = (string)DALHelper.HandleDBNull(reader["SOSemen"]);

                        Obj.SemenDonorID = (string)DALHelper.HandleDBNull(reader["SSCode"]);
                        

                        BizActionObj.Day5Details.Add(Obj);

                        BizActionObj.AnaesthetistID = (long)DALHelper.HandleDBNull(reader["AnasthesistID"]);
                        BizActionObj.AssAnaesthetistID = (long)DALHelper.HandleDBNull(reader["AssAnasthesistID"]);
                        BizActionObj.AssEmbryologistID = (long)DALHelper.HandleDBNull(reader["AssEmbryologistID"]);
                        BizActionObj.EmbryologistID = (long)DALHelper.HandleDBNull(reader["EmbryologistID"]);
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

        public override IValueObject GetFemaleLabDay5(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay5DetailsBizActionVO BizAction = (valueObject) as clsGetDay5DetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleLabDay5");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizAction.CoupleUnitID);
                dbServer.AddInParameter(command, "LabDay", DbType.Int16, IVFLabDay.Day5);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.LabDay5 == null)
                        BizAction.LabDay5 = new clsFemaleLabDay5VO();
                    while (reader.Read())
                    {


                        BizAction.LabDay5.ID = (long)(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.LabDay5.UnitID = (long)(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.LabDay5.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        BizAction.LabDay5.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));

                        BizAction.LabDay5.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        BizAction.LabDay5.Time = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                        BizAction.LabDay5.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.LabDay5.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        BizAction.LabDay5.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        BizAction.LabDay5.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        BizAction.LabDay5.SourceNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SourceNeedleID"]));
                        BizAction.LabDay5.InfectionObserved = (string)(DALHelper.HandleDBNull(reader["InfectionObserved"]));

                        BizAction.LabDay5.TotNoOfOocytes = (int)(DALHelper.HandleDBNull(reader["TotNoOfOocytes"]));
                        BizAction.LabDay5.TotNoOf2PN = (int)(DALHelper.HandleDBNull(reader["TotNoOf2PN"]));
                        BizAction.LabDay5.TotNoOf3PN = (int)(DALHelper.HandleDBNull(reader["TotNoOf3PN"]));
                        BizAction.LabDay5.TotNoOf2PB = (int)(DALHelper.HandleDBNull(reader["TotNoOf2PB"]));
                        BizAction.LabDay5.ToNoOfMI = (int)(DALHelper.HandleDBNull(reader["ToNoOfMI"]));
                        BizAction.LabDay5.ToNoOfMII = (int)(DALHelper.HandleDBNull(reader["ToNoOfMII"]));
                        BizAction.LabDay5.ToNoOfGV = (int)(DALHelper.HandleDBNull(reader["ToNoOfGV"]));
                        BizAction.LabDay5.ToNoOfDeGenerated = (int)(DALHelper.HandleDBNull(reader["ToNoOfDeGenerated"]));
                        BizAction.LabDay5.ToNoOfLost = (int)(DALHelper.HandleDBNull(reader["ToNoOfLost"]));
                        
                        BizAction.LabDay5.IsFreezed = (bool)(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        BizAction.LabDay5.Status = (bool)(DALHelper.HandleDBNull(reader["Status"]));


                    }

                    reader.NextResult();
                    if (BizAction.LabDay5.FertilizationAssesmentDetails == null)
                        BizAction.LabDay5.FertilizationAssesmentDetails = new List<clsFemaleLabDay5FertilizationAssesmentVO>();


                    while (reader.Read())
                    {
                        clsFemaleLabDay5FertilizationAssesmentVO ObjFer = new clsFemaleLabDay5FertilizationAssesmentVO();

                        ObjFer.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);

                        //By Anjali.............
                        ObjFer.SerialOccyteNo = (long)DALHelper.HandleDBNull(reader["SerialOccyteNo"]);

                        //................
                        ObjFer.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ObjFer.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        ObjFer.Time = (DateTime?)DALHelper.HandleDate(reader["Time"]);

                        ObjFer.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);
                        ObjFer.PlanTreatment = (string)DALHelper.HandleDBNull(reader["Protocol"]);

                        ObjFer.SrcOocyteID = (long)DALHelper.HandleDBNull(reader["SrcOocyteID"]);
                        ObjFer.SrcOocyteDescription = (string)DALHelper.HandleDBNull(reader["SOOocyte"]);

                        ObjFer.OocyteDonorID = (string)DALHelper.HandleDBNull(reader["OSCode"]);
                      

                        ObjFer.SrcOfSemen = (long)DALHelper.HandleDBNull(reader["SrcOfSemen"]);
                        ObjFer.SrcOfSemenDescription = (string)DALHelper.HandleDBNull(reader["SOSemen"]);

                        ObjFer.SemenDonorID = (string)DALHelper.HandleDBNull(reader["SSCode"]);
                      

                        ObjFer.SelectedFePlan.ID = (long)(DALHelper.HandleDBNull(reader["FertilisationStage"]));
                        ObjFer.SelectedFePlan.Description = (string)(DALHelper.HandleDBNull(reader["Fertilization"]));
                      
                        ObjFer.Score = (long)(DALHelper.HandleDBNull(reader["Score"]));
                        ObjFer.PV = (bool)(DALHelper.HandleDBNull(reader["PV"]));
                        ObjFer.XFactor = (bool)(DALHelper.HandleDBNull(reader["XFactor"]));
                        ObjFer.Others = (string)(DALHelper.HandleDBNull(reader["Others"]));
                        ObjFer.ProceedDay6 = (bool)(DALHelper.HandleDBNull(reader["ProceedDay6"]));

                        ObjFer.SelectedGrade.ID = (long)(DALHelper.HandleDBNull(reader["GradeID"]));
                        ObjFer.SelectedGrade.Description = (string)(DALHelper.HandleDBNull(reader["Grade"]));
                        
                        ObjFer.SelectedBlastocytsGrade.ID = (long)(DALHelper.HandleDBNull(reader["BlastocytsGradeID"]));
                        ObjFer.SelectedBlastocytsGrade.Description = (string)(DALHelper.HandleDBNull(reader["BlastocytsGrade"]));
                        
                        ObjFer.SelectedICM.ID = (long)(DALHelper.HandleDBNull(reader["ICMID"]));
                        ObjFer.SelectedICM.Description = (string)(DALHelper.HandleDBNull(reader["ICM"]));
                       
                        ObjFer.SelectedExpansion.ID = (long)(DALHelper.HandleDBNull(reader["ExpansionID"]));
                        ObjFer.SelectedExpansion.Description = (string)(DALHelper.HandleDBNull(reader["Expansion"]));

                        ObjFer.SelectedTrophectoderm.ID = (long)(DALHelper.HandleDBNull(reader["TrophectodermID"]));
                        ObjFer.SelectedTrophectoderm.Description = (string)(DALHelper.HandleDBNull(reader["Trophectoderm"]));

                        ObjFer.SelectedPlan.ID = (long)(DALHelper.HandleDBNull(reader["PlanID"]));
                        ObjFer.SelectedPlan.Description = (string)(DALHelper.HandleDBNull(reader["PlanName"]));
                        
                        ObjFer.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ObjFer.FileContents = (byte[])(DALHelper.HandleDBNull(reader["FileContents"]));



                        clsBaseIVFLabDayDAL objBaseDAL = clsBaseIVFLabDayDAL.GetInstance();
                        clsGetDay5MediaAndCalcDetailsBizActionVO BizActionDetails = new clsGetDay5MediaAndCalcDetailsBizActionVO();

                        BizActionDetails.ID = BizAction.ID;
                        BizActionDetails.DetailID = ObjFer.ID;
                        BizActionDetails = (clsGetDay5MediaAndCalcDetailsBizActionVO)objBaseDAL.GetFemaleLabDay5MediaAndCalDetails(BizActionDetails, UserVo);
                        ObjFer.Day5CalculateDetails = BizActionDetails.LabDayCalDetails;


                        //For getting Media details
                        clsBaseIVFLabDayDAL objBaseDAL2 = clsBaseIVFLabDayDAL.GetInstance();
                        clsGetAllDayMediaDetailsBizActionVO BizActionMedia = new clsGetAllDayMediaDetailsBizActionVO();
                        BizActionMedia.ID = BizAction.ID;
                        BizActionMedia.DetailID = ObjFer.ID;
                        BizActionMedia.LabDay = 5;
                        BizActionMedia = (clsGetAllDayMediaDetailsBizActionVO)objBaseDAL2.GetAllDayMediaDetails(BizActionMedia, UserVo);
                        ObjFer.MediaDetails = BizActionMedia.MediaList;

                        BizAction.LabDay5.FertilizationAssesmentDetails.Add(ObjFer);

                    }
                    reader.NextResult();
                    if (BizAction.LabDay5.SemenDetails == null)
                        BizAction.LabDay5.SemenDetails = new clsFemaleSemenDetailsVO();


                    while (reader.Read())
                    {
                        BizAction.LabDay5.SemenDetails.MOSP = (string)(DALHelper.HandleDBNull(reader["MOSP"]));
                        BizAction.LabDay5.SemenDetails.SOS = (string)(DALHelper.HandleDBNull(reader["SOS"]));

                        BizAction.LabDay5.SemenDetails.MethodOfSpermPreparation = (long)(DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]));
                        BizAction.LabDay5.SemenDetails.SourceOfSemen = (long)(DALHelper.HandleDBNull(reader["SourceOfSemen"]));

                        BizAction.LabDay5.SemenDetails.PreSelfVolume = (string)(DALHelper.HandleDBNull(reader["PreSelfVolume"]));
                        BizAction.LabDay5.SemenDetails.PreSelfConcentration = (string)(DALHelper.HandleDBNull(reader["PreSelfConcentration"]));
                        BizAction.LabDay5.SemenDetails.PreSelfMotality = (string)(DALHelper.HandleDBNull(reader["PreSelfMotality"]));
                        BizAction.LabDay5.SemenDetails.PreSelfWBC = (string)(DALHelper.HandleDBNull(reader["PreSelfWBC"]));

                        BizAction.LabDay5.SemenDetails.PreDonorVolume = (string)(DALHelper.HandleDBNull(reader["PreDonorVolume"]));
                        BizAction.LabDay5.SemenDetails.PreDonorConcentration = (string)(DALHelper.HandleDBNull(reader["PreDonorConcentration"]));
                        BizAction.LabDay5.SemenDetails.PreDonorMotality = (string)(DALHelper.HandleDBNull(reader["PreDonorMotality"]));
                        BizAction.LabDay5.SemenDetails.PreDonorWBC = (string)(DALHelper.HandleDBNull(reader["PreDonorWBC"]));


                        BizAction.LabDay5.SemenDetails.PostSelfVolume = (string)(DALHelper.HandleDBNull(reader["PostSelfVolume"]));
                        BizAction.LabDay5.SemenDetails.PostSelfConcentration = (string)(DALHelper.HandleDBNull(reader["PostSelfConcentration"]));
                        BizAction.LabDay5.SemenDetails.PostSelfMotality = (string)(DALHelper.HandleDBNull(reader["PostSelfMotality"]));
                        BizAction.LabDay5.SemenDetails.PostSelfWBC = (string)(DALHelper.HandleDBNull(reader["PostSelfWBC"]));

                        BizAction.LabDay5.SemenDetails.PostDonorVolume = (string)(DALHelper.HandleDBNull(reader["PostDonorVolume"]));
                        BizAction.LabDay5.SemenDetails.PostDonorConcentration = (string)(DALHelper.HandleDBNull(reader["PostDonorConcentration"]));
                        BizAction.LabDay5.SemenDetails.PostDonorMotality = (string)(DALHelper.HandleDBNull(reader["PostDonorMotality"]));
                        BizAction.LabDay5.SemenDetails.PostDonorWBC = (string)(DALHelper.HandleDBNull(reader["PostDonorWBC"]));
                    }

                    reader.NextResult();
                    if (BizAction.LabDay5.FUSetting == null)
                        BizAction.LabDay5.FUSetting = new List<FileUpload>();
                    while (reader.Read())
                    {
                        FileUpload ObjFile = new FileUpload();
                        ObjFile.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ObjFile.Index = (Int32)(DALHelper.HandleDBNull(reader["FileIndex"]));
                        ObjFile.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ObjFile.Data = (byte[])(DALHelper.HandleDBNull(reader["Value"]));
                        BizAction.LabDay5.FUSetting.Add(ObjFile);
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

        public override IValueObject GetLabDay4Score(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay4ScoreBizActionVO BizActionObj = valueObject as clsGetDay4ScoreBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFemaleDay4Score");

                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.CoupleUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Day4Score == null)
                        BizActionObj.Day4Score = new List<clsFemaleLabDay5FertilizationAssesmentVO>();
                    while (reader.Read())
                    {
                        clsFemaleLabDay5FertilizationAssesmentVO Obj = new clsFemaleLabDay5FertilizationAssesmentVO();

                        Obj.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);
                        Obj.Score = (long)DALHelper.HandleDBNull(reader["Score"]);
                        Obj.SelectedGrade.Description = (string)DALHelper.HandleDBNull(reader["Grade"]);
                        Obj.SelectedFePlan.Description = (string)DALHelper.HandleDBNull(reader["FertilisationStage"]);
                        Obj.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);
                        BizActionObj.Day4Score.Add(Obj);
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


        public override IValueObject GetFemaleLabDay5MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay5MediaAndCalcDetailsBizActionVO BizAction = (valueObject) as clsGetDay5MediaAndCalcDetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetMediaDetailsAndCalculateDetailForDay5");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "DetailID", DbType.Int64, BizAction.DetailID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.LabDayCalDetails == null)
                        BizAction.LabDayCalDetails = new clsFemaleLabDay5CalculateDetailsVO();

                    while (reader.Read())
                    {

                        BizAction.LabDayCalDetails.FertilizationID = (long)(DALHelper.HandleDBNull(reader["DetailID"]));
                        BizAction.LabDayCalDetails.BlastocoelsCavity = (bool)(DALHelper.HandleDBNull(reader["BlastocoelsCavity"]));
                        BizAction.LabDayCalDetails.TightlyPackedCells = (bool)(DALHelper.HandleDBNull(reader["TightlyPackedCells"]));
                        BizAction.LabDayCalDetails.FormingEpithelium = (bool)(DALHelper.HandleDBNull(reader["FormingEpithelium"]));

                    }

                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return BizAction;

        }
        #endregion


        #region Day 6
        public override IValueObject AddLabDay6(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddLabDay6BizActionVO BizActionObj = valueObject as clsAddLabDay6BizActionVO;

            if (BizActionObj.Day6Details.ID == 0)
                BizActionObj = AddDay6(BizActionObj, UserVo);
            else
                BizActionObj = UpdateDay6(BizActionObj, UserVo);

            return valueObject;
        }

        private clsAddLabDay6BizActionVO AddDay6(clsAddLabDay6BizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsFemaleLabDay6VO ObjDay6VO = BizActionObj.Day6Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay6");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, ObjDay6VO.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, ObjDay6VO.CoupleUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjDay6VO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, ObjDay6VO.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, ObjDay6VO.EmbryologistID);
                dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, ObjDay6VO.AssEmbryologistID);
                dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, ObjDay6VO.AnesthetistID);
                dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, ObjDay6VO.AssAnesthetistID);
                dbServer.AddInParameter(command, "IVFCycleCount", DbType.Int64, ObjDay6VO.IVFCycleCount);
                dbServer.AddInParameter(command, "SourceNeedleID", DbType.Int64, ObjDay6VO.SourceNeedleID);
                dbServer.AddInParameter(command, "InfectionObserved", DbType.String, ObjDay6VO.InfectionObserved);
                dbServer.AddInParameter(command, "TotNoOfOocytes", DbType.Int32, ObjDay6VO.TotNoOfOocytes);
                dbServer.AddInParameter(command, "TotNoOf2PN", DbType.Int32, ObjDay6VO.TotNoOf2PN);
                dbServer.AddInParameter(command, "TotNoOf3PN", DbType.Int32, ObjDay6VO.TotNoOf3PN);
                dbServer.AddInParameter(command, "TotNoOf2PB", DbType.Int32, ObjDay6VO.TotNoOf2PB);
                dbServer.AddInParameter(command, "ToNoOfMI", DbType.Int32, ObjDay6VO.ToNoOfMI);
                dbServer.AddInParameter(command, "ToNoOfMII", DbType.Int32, ObjDay6VO.ToNoOfMII);
                dbServer.AddInParameter(command, "ToNoOfGV", DbType.Int32, ObjDay6VO.ToNoOfGV);
                dbServer.AddInParameter(command, "ToNoOfDeGenerated", DbType.Int32, ObjDay6VO.ToNoOfDeGenerated);
                dbServer.AddInParameter(command, "ToNoOfLost ", DbType.Int32, ObjDay6VO.ToNoOfLost);
                dbServer.AddInParameter(command, "IsFreezed ", DbType.Boolean, ObjDay6VO.IsFreezed);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDay6VO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Day6Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.Day6Details.FertilizationAssesmentDetails != null && BizActionObj.Day6Details.FertilizationAssesmentDetails.Count > 0)
                {
                    foreach (var item1 in ObjDay6VO.FertilizationAssesmentDetails)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay6Details");

                        dbServer.AddInParameter(command2, "Day6ID", DbType.Int64, ObjDay6VO.ID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "OoNo", DbType.Int64, item1.OoNo);
                        //By Anjali..........
                        dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, item1.SerialOccyteNo);
                        //...................
                        dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, item1.PlanTreatmentID);
                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, item1.Date);
                        dbServer.AddInParameter(command2, "Time", DbType.DateTime, item1.Time);
                        dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, item1.SrcOocyteID);
                        dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, item1.OocyteDonorID);
                        dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, item1.SrcOfSemen);
                        dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, item1.SemenDonorID);
                        dbServer.AddInParameter(command2, "FileName", DbType.String, item1.FileName);
                        dbServer.AddInParameter(command2, "FileContents", DbType.Binary, item1.FileContents);

                        if (item1.SelectedFePlan!=null)
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, item1.SelectedFePlan.ID);
                        else
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);
                        if (item1.SelectedGrade !=null)
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, item1.SelectedGrade.ID);
                        else
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);

                        dbServer.AddInParameter(command2, "Score", DbType.Int64, item1.Score);
                        dbServer.AddInParameter(command2, "PV", DbType.Boolean, item1.PV);
                        dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, item1.XFactor);
                        dbServer.AddInParameter(command2, "Others", DbType.String, item1.Others);
                        //dbServer.AddInParameter(command2, "ProceedDay7", DbType.Boolean, item1.ProceedDay7);
                        if (item1.SelectedPlan !=null)
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, item1.SelectedPlan.ID);
                        else
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);
                        if (item1.SelectedPlan.ID == 3)
                        {
                            dbServer.AddInParameter(command2, "ProceedDay7", DbType.Boolean,true); 
                        }
                        else
                        {
                            dbServer.AddInParameter(command2, "ProceedDay7", DbType.Boolean, false); 
                        }
                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);

                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                        item1.ID = (long)dbServer.GetParameterValue(command2, "ID");

                        if (item1.MediaDetails != null && item1.MediaDetails.Count > 0)
                        {
                            foreach (var item3 in item1.MediaDetails)
                            {
                                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");

                                dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay6VO.ID);
                                dbServer.AddInParameter(command4, "DetailID", DbType.Int64, item1.ID);
                                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command4, "Date", DbType.DateTime, item3.Date);
                                dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day6);
                                dbServer.AddInParameter(command4, "MediaName", DbType.String, item3.ItemName);
                                dbServer.AddInParameter(command4, "Company", DbType.String, item3.Company);
                                dbServer.AddInParameter(command4, "LotNo", DbType.String, item3.BatchCode);
                                dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, item3.ExpiryDate);
                                dbServer.AddInParameter(command4, "PH", DbType.Boolean, item3.PH);
                                dbServer.AddInParameter(command4, "OSM", DbType.Boolean, item3.OSM);
                                dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, item3.VolumeUsed);
                                dbServer.AddInParameter(command4, "Status", DbType.Int64, item3.SelectedStatus.ID);
                                dbServer.AddInParameter(command4, "BatchID", DbType.Int64, item3.BatchID);
                                dbServer.AddInParameter(command4, "StoreID", DbType.Int64, item3.StoreID);  

                                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                item3.ID = (long)dbServer.GetParameterValue(command4, "ID");
                            }
                        }
                        
                    }
                }

                if (BizActionObj.Day6Details.FUSetting != null && BizActionObj.Day6Details.FUSetting.Count > 0)
                {
                    foreach (var item2 in ObjDay6VO.FUSetting)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                        dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, ObjDay6VO.ID);
                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day6);
                        dbServer.AddInParameter(command3, "FileName", DbType.String, item2.FileName);
                        dbServer.AddInParameter(command3, "FileIndex", DbType.Int32, item2.Index);
                        dbServer.AddInParameter(command3, "Value", DbType.Binary, item2.Data);
                        dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                        dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                        item2.ID = (long)dbServer.GetParameterValue(command3, "ID");
                    }
                }

                if (BizActionObj.Day6Details.SemenDetails != null)
                {
                    var item4 = BizActionObj.Day6Details.SemenDetails;
                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");

                    dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay6VO.ID);
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day6);
                    dbServer.AddInParameter(command4, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day6Details.SemenDetails.MethodOfSpermPreparation);
                    dbServer.AddInParameter(command4, "SourceOfSemen", DbType.Int64, BizActionObj.Day6Details.SemenDetails.SourceOfSemen);
                    dbServer.AddInParameter(command4, "PreSelfVolume", DbType.String, item4.PreSelfVolume);
                    dbServer.AddInParameter(command4, "PreSelfConcentration", DbType.String, item4.PreSelfConcentration);
                    dbServer.AddInParameter(command4, "PreSelfMotality", DbType.String, item4.PreSelfMotality);
                    dbServer.AddInParameter(command4, "PreSelfWBC", DbType.String, item4.PreSelfWBC);
                    dbServer.AddInParameter(command4, "PreDonorVolume", DbType.String, item4.PreDonorVolume);
                    dbServer.AddInParameter(command4, "PreDonorConcentration", DbType.String, item4.PreDonorConcentration);
                    dbServer.AddInParameter(command4, "PreDonorMotality", DbType.String, item4.PreDonorMotality);
                    dbServer.AddInParameter(command4, "PreDonorWBC", DbType.String, item4.PreDonorWBC);
                    dbServer.AddInParameter(command4, "PostSelfVolume", DbType.String, item4.PostSelfVolume);
                    dbServer.AddInParameter(command4, "PostSelfConcentration", DbType.String, item4.PostSelfConcentration);
                    dbServer.AddInParameter(command4, "PostSelfMotality", DbType.String, item4.PostSelfMotality);
                    dbServer.AddInParameter(command4, "PostSelfWBC", DbType.String, item4.PostSelfWBC);
                    dbServer.AddInParameter(command4, "PostDonorVolume ", DbType.String, item4.PostDonorVolume);
                    dbServer.AddInParameter(command4, "PostDonorConcentration", DbType.String, item4.PostDonorConcentration);
                    dbServer.AddInParameter(command4, "PostDonorMotality", DbType.String, item4.PostDonorMotality);
                    dbServer.AddInParameter(command4, "PostDonorWBC", DbType.String, item4.PostDonorWBC);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item4.ID);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    item4.ID = (long)dbServer.GetParameterValue(command4, "ID");
                }
                if (BizActionObj.Day6Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                    obj.LabDaysSummary = BizActionObj.Day6Details.LabDaySummary;
                    obj.LabDaysSummary.OocyteID = BizActionObj.Day6Details.ID;
                    obj.LabDaysSummary.CoupleID = BizActionObj.Day6Details.CoupleID;
                    obj.LabDaysSummary.CoupleUnitID = BizActionObj.Day6Details.CoupleUnitID;
                    obj.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay6;
                    obj.LabDaysSummary.Priority = 1;
                    obj.LabDaysSummary.ProcDate = BizActionObj.Day6Details.Date;
                    obj.LabDaysSummary.ProcTime = BizActionObj.Day6Details.Time;
                    obj.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    obj.LabDaysSummary.IsFreezed = BizActionObj.Day6Details.IsFreezed;

                    obj.IsUpdate = false;
                    obj = (clsAddUpdateLabDaysSummaryBizActionVO)objBaseDAL.AddUpdateLabDaysSummary(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Day6Details.LabDaySummary.ID = obj.LabDaysSummary.ID;
                }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
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

        private clsAddLabDay6BizActionVO UpdateDay6(clsAddLabDay6BizActionVO BizActionObj, clsUserVO UserVo)
        {

            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();
                clsFemaleLabDay6VO ObjDay6VO = BizActionObj.Day6Details;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateFemaleLabDay6");

                dbServer.AddInParameter(command, "ID", DbType.Int64, ObjDay6VO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, ObjDay6VO.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, ObjDay6VO.CoupleUnitID);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, ObjDay6VO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, ObjDay6VO.Time);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, ObjDay6VO.EmbryologistID);
                dbServer.AddInParameter(command, "AssEmbryologistID", DbType.Int64, ObjDay6VO.AssEmbryologistID);
                dbServer.AddInParameter(command, "AnasthesistID", DbType.Int64, ObjDay6VO.AnesthetistID);
                dbServer.AddInParameter(command, "AssAnasthesistID", DbType.Int64, ObjDay6VO.AssAnesthetistID);
                dbServer.AddInParameter(command, "IVFCycleCount", DbType.Int64, ObjDay6VO.IVFCycleCount);
                dbServer.AddInParameter(command, "SourceNeedleID", DbType.Int64, ObjDay6VO.SourceNeedleID);
                dbServer.AddInParameter(command, "InfectionObserved", DbType.String, ObjDay6VO.InfectionObserved);
                dbServer.AddInParameter(command, "TotNoOfOocytes", DbType.Int32, ObjDay6VO.TotNoOfOocytes);
                dbServer.AddInParameter(command, "TotNoOf2PN", DbType.Int32, ObjDay6VO.TotNoOf2PN);
                dbServer.AddInParameter(command, "TotNoOf3PN", DbType.Int32, ObjDay6VO.TotNoOf3PN);
                dbServer.AddInParameter(command, "TotNoOf2PB", DbType.Int32, ObjDay6VO.TotNoOf2PB);
                dbServer.AddInParameter(command, "ToNoOfMI", DbType.Int32, ObjDay6VO.ToNoOfMI);
                dbServer.AddInParameter(command, "ToNoOfMII", DbType.Int32, ObjDay6VO.ToNoOfMII);
                dbServer.AddInParameter(command, "ToNoOfGV", DbType.Int32, ObjDay6VO.ToNoOfGV);
                dbServer.AddInParameter(command, "ToNoOfDeGenerated", DbType.Int32, ObjDay6VO.ToNoOfDeGenerated);
                dbServer.AddInParameter(command, "ToNoOfLost ", DbType.Int32, ObjDay6VO.ToNoOfLost);
                dbServer.AddInParameter(command, "IsFreezed ", DbType.Boolean, ObjDay6VO.IsFreezed);
                dbServer.AddInParameter(command, "Status  ", DbType.Boolean, true);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);


                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                if (BizActionObj.Day6Details.FertilizationAssesmentDetails != null && BizActionObj.Day6Details.FertilizationAssesmentDetails.Count > 0)
                {
                    DbCommand command11 = dbServer.GetStoredProcCommand("CIMS_DeleteLab6Details");

                    dbServer.AddInParameter(command11, "Day6ID", DbType.Int64, ObjDay6VO.ID);
                    dbServer.AddInParameter(command11, "LabDay", DbType.Int32, IVFLabDay.Day6);
                    int intStatus2 = dbServer.ExecuteNonQuery(command11, trans);
                }

                if (BizActionObj.Day6Details.FertilizationAssesmentDetails != null && BizActionObj.Day6Details.FertilizationAssesmentDetails.Count > 0)
                {
                    foreach (var item1 in ObjDay6VO.FertilizationAssesmentDetails)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddFemaleLabDay6Details");

                        dbServer.AddInParameter(command2, "Day6ID", DbType.Int64, ObjDay6VO.ID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "OoNo", DbType.Int64, item1.OoNo);
                        //By Anjali..........
                        dbServer.AddInParameter(command2, "SerialOccyteNo", DbType.Int64, item1.SerialOccyteNo);
                        //...................
                        dbServer.AddInParameter(command2, "PlanTreatmentID", DbType.Int64, item1.PlanTreatmentID);
                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, item1.Date);
                        dbServer.AddInParameter(command2, "Time", DbType.DateTime, item1.Time);
                        dbServer.AddInParameter(command2, "SrcOocyteID", DbType.Int64, item1.SrcOocyteID);
                        dbServer.AddInParameter(command2, "OocyteDonorID", DbType.String, item1.OocyteDonorID);
                        dbServer.AddInParameter(command2, "SrcOfSemen", DbType.Int64, item1.SrcOfSemen);
                        dbServer.AddInParameter(command2, "SemenDonorID", DbType.String, item1.SemenDonorID);
                        dbServer.AddInParameter(command2, "FileName", DbType.String, item1.FileName);
                        dbServer.AddInParameter(command2, "FileContents", DbType.Binary, item1.FileContents);


                        if (item1.SelectedFePlan != null)
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, item1.SelectedFePlan.ID);
                        else
                            dbServer.AddInParameter(command2, "FertilisationStage", DbType.Int64, 0);
                        
                        if (item1.SelectedGrade != null)
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, item1.SelectedGrade.ID);
                        else
                            dbServer.AddInParameter(command2, "Grade", DbType.Int64, 0);
                        dbServer.AddInParameter(command2, "Score", DbType.Int64, item1.Score);
                        dbServer.AddInParameter(command2, "PV", DbType.Boolean, item1.PV);
                        dbServer.AddInParameter(command2, "XFactor", DbType.Boolean, item1.XFactor);
                        dbServer.AddInParameter(command2, "Others", DbType.String, item1.Others);
                        dbServer.AddInParameter(command2, "ProceedDay7", DbType.Boolean, item1.ProceedDay7);
                       
                        if (item1.SelectedPlan!=null)
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, item1.SelectedPlan.ID);
                        else
                            dbServer.AddInParameter(command2, "PlanID", DbType.Int64, 0);

                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item1.ID);

                        int iStatus = dbServer.ExecuteNonQuery(command2, trans);
                        item1.ID = (long)dbServer.GetParameterValue(command2, "ID");

                        if (item1.MediaDetails != null && item1.MediaDetails.Count > 0)
                        {
                            foreach (var item3 in item1.MediaDetails)
                            {
                                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayMediaDetails");

                                dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay6VO.ID);
                                dbServer.AddInParameter(command4, "DetailID", DbType.Int64, item1.ID);
                                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(command4, "Date", DbType.DateTime, item3.Date);
                                dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day6);
                                dbServer.AddInParameter(command4, "MediaName", DbType.String, item3.ItemName);
                                dbServer.AddInParameter(command4, "Company", DbType.String, item3.Company);
                                dbServer.AddInParameter(command4, "LotNo", DbType.String, item3.BatchCode);
                                dbServer.AddInParameter(command4, "ExpiryDate", DbType.DateTime, item3.ExpiryDate);
                                dbServer.AddInParameter(command4, "PH", DbType.Boolean, item3.PH);
                                dbServer.AddInParameter(command4, "OSM", DbType.Boolean, item3.OSM);
                                dbServer.AddInParameter(command4, "VolumeUsed", DbType.String, item3.VolumeUsed);
                                dbServer.AddInParameter(command4, "Status", DbType.Int64, item3.SelectedStatus.ID);
                                dbServer.AddInParameter(command4, "BatchID", DbType.Int64, item3.BatchID);
                                dbServer.AddInParameter(command4, "StoreID", DbType.Int64, item3.StoreID);  

                                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item3.ID);
                                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                                int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                                item3.ID = (long)dbServer.GetParameterValue(command4, "ID");
                            }
                        }

                    }
                }

                if (BizActionObj.Day6Details.FUSetting != null && BizActionObj.Day6Details.FUSetting.Count > 0)
                {
                    DbCommand command13 = dbServer.GetStoredProcCommand("CIMS_DeleteLab6UploadFileDetails");

                    dbServer.AddInParameter(command13, "Day6ID", DbType.Int64, ObjDay6VO.ID);
                    dbServer.AddInParameter(command13, "LabDay", DbType.Int32, IVFLabDay.Day6);
                    dbServer.AddInParameter(command13, "UnitID", DbType.Int64, ObjDay6VO.UnitID);
                    int intStatus2 = dbServer.ExecuteNonQuery(command13, trans);
                }

                if (BizActionObj.Day6Details.FUSetting != null && BizActionObj.Day6Details.FUSetting.Count > 0)
                {
                    foreach (var item2 in ObjDay6VO.FUSetting)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDayUploadedFiles");

                        dbServer.AddInParameter(command3, "OocyteID", DbType.Int64, ObjDay6VO.ID);
                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command3, "LabDay", DbType.Int16, IVFLabDay.Day6);
                        dbServer.AddInParameter(command3, "FileName", DbType.String, item2.FileName);
                        dbServer.AddInParameter(command3, "FileIndex", DbType.Int32, item2.Index);
                        dbServer.AddInParameter(command3, "Value", DbType.Binary, item2.Data);
                        dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                        dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item2.ID);
                        dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command3, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                        item2.ID = (long)dbServer.GetParameterValue(command3, "ID");
                    }
                }


                if (BizActionObj.Day6Details.SemenDetails != null)
                {
                    DbCommand command12 = dbServer.GetStoredProcCommand("CIMS_DeleteLab6SemenDetails");

                    dbServer.AddInParameter(command12, "Day6ID ", DbType.Int64, ObjDay6VO.ID);
                    dbServer.AddInParameter(command12, "LabDay", DbType.Int32, IVFLabDay.Day6);
                    int intStatus2 = dbServer.ExecuteNonQuery(command12, trans);
                }
                if (BizActionObj.Day6Details.SemenDetails != null)
                {
                    var item4 = BizActionObj.Day6Details.SemenDetails;
                    DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_IVF_AddLabDaySemenDetails");

                    dbServer.AddInParameter(command4, "OocyteID", DbType.Int64, ObjDay6VO.ID);
                    dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command4, "LabDay", DbType.Int16, IVFLabDay.Day6);
                    dbServer.AddInParameter(command4, "MethodOfSpermPreparation", DbType.Int64, BizActionObj.Day6Details.SemenDetails.MethodOfSpermPreparation);
                    dbServer.AddInParameter(command4, "SourceOfSemen", DbType.Int64, BizActionObj.Day6Details.SemenDetails.SourceOfSemen);
                    dbServer.AddInParameter(command4, "PreSelfVolume", DbType.String, item4.PreSelfVolume);
                    dbServer.AddInParameter(command4, "PreSelfConcentration", DbType.String, item4.PreSelfConcentration);
                    dbServer.AddInParameter(command4, "PreSelfMotality", DbType.String, item4.PreSelfMotality);
                    dbServer.AddInParameter(command4, "PreSelfWBC", DbType.String, item4.PreSelfWBC);
                    dbServer.AddInParameter(command4, "PreDonorVolume", DbType.String, item4.PreDonorVolume);
                    dbServer.AddInParameter(command4, "PreDonorConcentration", DbType.String, item4.PreDonorConcentration);
                    dbServer.AddInParameter(command4, "PreDonorMotality", DbType.String, item4.PreDonorMotality);
                    dbServer.AddInParameter(command4, "PreDonorWBC", DbType.String, item4.PreDonorWBC);
                    dbServer.AddInParameter(command4, "PostSelfVolume", DbType.String, item4.PostSelfVolume);
                    dbServer.AddInParameter(command4, "PostSelfConcentration", DbType.String, item4.PostSelfConcentration);
                    dbServer.AddInParameter(command4, "PostSelfMotality", DbType.String, item4.PostSelfMotality);
                    dbServer.AddInParameter(command4, "PostSelfWBC", DbType.String, item4.PostSelfWBC);
                    dbServer.AddInParameter(command4, "PostDonorVolume ", DbType.String, item4.PostDonorVolume);
                    dbServer.AddInParameter(command4, "PostDonorConcentration", DbType.String, item4.PostDonorConcentration);
                    dbServer.AddInParameter(command4, "PostDonorMotality", DbType.String, item4.PostDonorMotality);
                    dbServer.AddInParameter(command4, "PostDonorWBC", DbType.String, item4.PostDonorWBC);
                    dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item4.ID);
                    dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command4, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                    item4.ID = (long)dbServer.GetParameterValue(command4, "ID");
                }
                if (BizActionObj.Day6Details.LabDaySummary != null)
                {
                    clsBaseIVFLabDaysSummaryDAL objBaseDAL = clsBaseIVFLabDaysSummaryDAL.GetInstance();
                    clsAddUpdateLabDaysSummaryBizActionVO obj = new clsAddUpdateLabDaysSummaryBizActionVO();

                    obj.LabDaysSummary = BizActionObj.Day6Details.LabDaySummary;
                    obj.LabDaysSummary.OocyteID = BizActionObj.Day6Details.ID;
                    obj.LabDaysSummary.CoupleID = BizActionObj.Day6Details.CoupleID;
                    obj.LabDaysSummary.CoupleUnitID = BizActionObj.Day6Details.CoupleUnitID;
                    obj.LabDaysSummary.FormID = IVFLabWorkForm.FemaleLabDay6;
                    obj.LabDaysSummary.Priority = 1;
                    obj.LabDaysSummary.ProcDate = BizActionObj.Day6Details.Date;
                    obj.LabDaysSummary.ProcTime = BizActionObj.Day6Details.Time;
                    obj.LabDaysSummary.UnitID = UserVo.UserLoginInfo.UnitId;
                    obj.LabDaysSummary.IsFreezed = BizActionObj.Day6Details.IsFreezed;

                    obj.IsUpdate = true;

                    obj = (clsAddUpdateLabDaysSummaryBizActionVO)objBaseDAL.AddUpdateLabDaysSummary(obj, UserVo, con, trans);
                    if (obj.SuccessStatus == -1) throw new Exception();
                    BizActionObj.Day6Details.LabDaySummary.ID = obj.LabDaysSummary.ID;
                }

                trans.Commit();
                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
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

        public override IValueObject GetLabDay5ForDay6(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetLabDay5ForLabDay6BizActionVO BizActionObj = valueObject as clsGetLabDay5ForLabDay6BizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFemaleLabDay5ForLabDay6");
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.CoupleUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Day6Details == null)
                        BizActionObj.Day6Details = new List<clsFemaleLabDay6FertilizationAssesmentVO>();
                    while (reader.Read())
                    {
                        clsFemaleLabDay6FertilizationAssesmentVO Obj = new clsFemaleLabDay6FertilizationAssesmentVO();

                        Obj.ID = (long)DALHelper.HandleDBNull(reader["DetailID"]);
                        Obj.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);

                        //By Anjali.............
                        Obj.SerialOccyteNo = (long)DALHelper.HandleDBNull(reader["SerialOccyteNo"]);

                        //................
                        Obj.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        Obj.Time = (DateTime?)DALHelper.HandleDate(reader["Time"]);

                        Obj.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);
                        Obj.PlanTreatment = (string)DALHelper.HandleDBNull(reader["Protocol"]);

                        Obj.SrcOocyteID = (long)DALHelper.HandleDBNull(reader["SrcOocyteID"]);
                        Obj.SrcOocyteDescription = (string)DALHelper.HandleDBNull(reader["SOOocyte"]);

                        Obj.OocyteDonorID = (string)DALHelper.HandleDBNull(reader["OSCode"]);
                        

                        Obj.SrcOfSemen = (long)DALHelper.HandleDBNull(reader["SrcOfSemen"]);
                        Obj.SrcOfSemenDescription = (string)DALHelper.HandleDBNull(reader["SOSemen"]);

                        Obj.SemenDonorID = (string)DALHelper.HandleDBNull(reader["SSCode"]);
                        

                        BizActionObj.Day6Details.Add(Obj);

                        BizActionObj.AnaesthetistID = (long)DALHelper.HandleDBNull(reader["AnasthesistID"]);
                        BizActionObj.AssAnaesthetistID = (long)DALHelper.HandleDBNull(reader["AssAnasthesistID"]);
                        BizActionObj.AssEmbryologistID = (long)DALHelper.HandleDBNull(reader["AssEmbryologistID"]);
                        BizActionObj.EmbryologistID = (long)DALHelper.HandleDBNull(reader["EmbryologistID"]);
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

        public override IValueObject GetFemaleLabDay6(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay6DetailsBizActionVO BizAction = (valueObject) as clsGetDay6DetailsBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_IVF_GetFemaleLabDay6");

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizAction.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizAction.UnitID);
                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizAction.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizAction.CoupleUnitID);
                dbServer.AddInParameter(command, "LabDay", DbType.Int16, IVFLabDay.Day6);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.LabDay6 == null)
                        BizAction.LabDay6 = new clsFemaleLabDay6VO();
                    while (reader.Read())
                    {


                        BizAction.LabDay6.ID = (long)(DALHelper.HandleDBNull(reader["ID"]));
                        BizAction.LabDay6.UnitID = (long)(DALHelper.HandleDBNull(reader["UnitID"]));
                        BizAction.LabDay6.CoupleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleID"]));
                        BizAction.LabDay6.CoupleUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CoupleUnitID"]));

                        BizAction.LabDay6.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        BizAction.LabDay6.Time = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                        BizAction.LabDay6.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        BizAction.LabDay6.AssEmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssEmbryologistID"]));
                        BizAction.LabDay6.AnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AnasthesistID"]));
                        BizAction.LabDay6.AssAnesthetistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AssAnasthesistID"]));
                        BizAction.LabDay6.SourceNeedleID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SourceNeedleID"]));
                        BizAction.LabDay6.InfectionObserved = (string)(DALHelper.HandleDBNull(reader["InfectionObserved"]));

                        BizAction.LabDay6.TotNoOfOocytes = (int)(DALHelper.HandleDBNull(reader["TotNoOfOocytes"]));
                        BizAction.LabDay6.TotNoOf2PN = (int)(DALHelper.HandleDBNull(reader["TotNoOf2PN"]));
                        BizAction.LabDay6.TotNoOf3PN = (int)(DALHelper.HandleDBNull(reader["TotNoOf3PN"]));
                        BizAction.LabDay6.TotNoOf2PB = (int)(DALHelper.HandleDBNull(reader["TotNoOf2PB"]));
                        BizAction.LabDay6.ToNoOfMI = (int)(DALHelper.HandleDBNull(reader["ToNoOfMI"]));
                        BizAction.LabDay6.ToNoOfMII = (int)(DALHelper.HandleDBNull(reader["ToNoOfMII"]));
                        BizAction.LabDay6.ToNoOfGV = (int)(DALHelper.HandleDBNull(reader["ToNoOfGV"]));
                        BizAction.LabDay6.ToNoOfDeGenerated = (int)(DALHelper.HandleDBNull(reader["ToNoOfDeGenerated"]));
                        BizAction.LabDay6.ToNoOfLost = (int)(DALHelper.HandleDBNull(reader["ToNoOfLost"]));

                        BizAction.LabDay6.IsFreezed = (bool)(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        BizAction.LabDay6.Status = (bool)(DALHelper.HandleDBNull(reader["Status"]));


                    }

                    reader.NextResult();
                    if (BizAction.LabDay6.FertilizationAssesmentDetails == null)
                        BizAction.LabDay6.FertilizationAssesmentDetails = new List<clsFemaleLabDay6FertilizationAssesmentVO>();


                    while (reader.Read())
                    {
                        clsFemaleLabDay6FertilizationAssesmentVO ObjFer = new clsFemaleLabDay6FertilizationAssesmentVO();

                        ObjFer.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);

                        //By Anjali.............
                        ObjFer.SerialOccyteNo = (long)DALHelper.HandleDBNull(reader["SerialOccyteNo"]);

                        //................
                        ObjFer.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        ObjFer.Date = (DateTime?)DALHelper.HandleDate(reader["Date"]);
                        ObjFer.Time = (DateTime?)DALHelper.HandleDate(reader["Time"]);

                        ObjFer.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);
                        ObjFer.PlanTreatment = (string)DALHelper.HandleDBNull(reader["Protocol"]);

                        ObjFer.SrcOocyteID = (long)DALHelper.HandleDBNull(reader["SrcOocyteID"]);
                        ObjFer.SrcOocyteDescription = (string)DALHelper.HandleDBNull(reader["SOOocyte"]);

                        ObjFer.OocyteDonorID = (string)DALHelper.HandleDBNull(reader["OSCode"]);
                        

                        ObjFer.SrcOfSemen = (long)DALHelper.HandleDBNull(reader["SrcOfSemen"]);
                        ObjFer.SrcOfSemenDescription = (string)DALHelper.HandleDBNull(reader["SOSemen"]);

                        ObjFer.SemenDonorID = (string)DALHelper.HandleDBNull(reader["SSCode"]);
                      

                        ObjFer.SelectedFePlan.ID = (long)(DALHelper.HandleDBNull(reader["FertilisationStage"]));
                        ObjFer.SelectedFePlan.Description = (string)(DALHelper.HandleDBNull(reader["Fertilization"]));

                        ObjFer.Score = (long)(DALHelper.HandleDBNull(reader["Score"]));
                        ObjFer.PV = (bool)(DALHelper.HandleDBNull(reader["PV"]));
                        ObjFer.XFactor = (bool)(DALHelper.HandleDBNull(reader["XFactor"]));
                        ObjFer.Others = (string)(DALHelper.HandleDBNull(reader["Others"]));
                        ObjFer.ProceedDay7 = (bool)(DALHelper.HandleDBNull(reader["ProceedDay7"]));

                        ObjFer.SelectedGrade.ID = (long)(DALHelper.HandleDBNull(reader["GradeID"]));
                        ObjFer.SelectedGrade.Description = (string)(DALHelper.HandleDBNull(reader["Grade"]));

                        ObjFer.SelectedPlan.ID = (long)(DALHelper.HandleDBNull(reader["PlanID"]));
                        ObjFer.SelectedPlan.Description = (string)(DALHelper.HandleDBNull(reader["PlanName"]));
                        ObjFer.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ObjFer.FileContents = (byte[])(DALHelper.HandleDBNull(reader["FileContents"]));


                        clsBaseIVFLabDayDAL objBaseDAL = clsBaseIVFLabDayDAL.GetInstance();
                        clsGetAllDayMediaDetailsBizActionVO BizActionMedia = new clsGetAllDayMediaDetailsBizActionVO();
                        BizActionMedia.ID = BizAction.ID;
                        BizActionMedia.DetailID = ObjFer.ID;
                        BizActionMedia.LabDay = 6;
                        BizActionMedia = (clsGetAllDayMediaDetailsBizActionVO)objBaseDAL.GetAllDayMediaDetails(BizActionMedia, UserVo);
                        ObjFer.MediaDetails = BizActionMedia.MediaList;

                        BizAction.LabDay6.FertilizationAssesmentDetails.Add(ObjFer);

                    }
                    reader.NextResult();
                    if (BizAction.LabDay6.SemenDetails == null)
                        BizAction.LabDay6.SemenDetails = new clsFemaleSemenDetailsVO();


                    while (reader.Read())
                    {
                        BizAction.LabDay6.SemenDetails.MOSP = (string)(DALHelper.HandleDBNull(reader["MOSP"]));
                        BizAction.LabDay6.SemenDetails.SOS = (string)(DALHelper.HandleDBNull(reader["SOS"]));

                        BizAction.LabDay6.SemenDetails.MethodOfSpermPreparation = (long)(DALHelper.HandleDBNull(reader["MethodOfSpermPreparation"]));
                        BizAction.LabDay6.SemenDetails.SourceOfSemen = (long)(DALHelper.HandleDBNull(reader["SourceOfSemen"]));

                        BizAction.LabDay6.SemenDetails.PreSelfVolume = (string)(DALHelper.HandleDBNull(reader["PreSelfVolume"]));
                        BizAction.LabDay6.SemenDetails.PreSelfConcentration = (string)(DALHelper.HandleDBNull(reader["PreSelfConcentration"]));
                        BizAction.LabDay6.SemenDetails.PreSelfMotality = (string)(DALHelper.HandleDBNull(reader["PreSelfMotality"]));
                        BizAction.LabDay6.SemenDetails.PreSelfWBC = (string)(DALHelper.HandleDBNull(reader["PreSelfWBC"]));

                        BizAction.LabDay6.SemenDetails.PreDonorVolume = (string)(DALHelper.HandleDBNull(reader["PreDonorVolume"]));
                        BizAction.LabDay6.SemenDetails.PreDonorConcentration = (string)(DALHelper.HandleDBNull(reader["PreDonorConcentration"]));
                        BizAction.LabDay6.SemenDetails.PreDonorMotality = (string)(DALHelper.HandleDBNull(reader["PreDonorMotality"]));
                        BizAction.LabDay6.SemenDetails.PreDonorWBC = (string)(DALHelper.HandleDBNull(reader["PreDonorWBC"]));


                        BizAction.LabDay6.SemenDetails.PostSelfVolume = (string)(DALHelper.HandleDBNull(reader["PostSelfVolume"]));
                        BizAction.LabDay6.SemenDetails.PostSelfConcentration = (string)(DALHelper.HandleDBNull(reader["PostSelfConcentration"]));
                        BizAction.LabDay6.SemenDetails.PostSelfMotality = (string)(DALHelper.HandleDBNull(reader["PostSelfMotality"]));
                        BizAction.LabDay6.SemenDetails.PostSelfWBC = (string)(DALHelper.HandleDBNull(reader["PostSelfWBC"]));

                        BizAction.LabDay6.SemenDetails.PostDonorVolume = (string)(DALHelper.HandleDBNull(reader["PostDonorVolume"]));
                        BizAction.LabDay6.SemenDetails.PostDonorConcentration = (string)(DALHelper.HandleDBNull(reader["PostDonorConcentration"]));
                        BizAction.LabDay6.SemenDetails.PostDonorMotality = (string)(DALHelper.HandleDBNull(reader["PostDonorMotality"]));
                        BizAction.LabDay6.SemenDetails.PostDonorWBC = (string)(DALHelper.HandleDBNull(reader["PostDonorWBC"]));
                    }

                    reader.NextResult();
                    if (BizAction.LabDay6.FUSetting == null)
                        BizAction.LabDay6.FUSetting = new List<FileUpload>();
                    while (reader.Read())
                    {
                        FileUpload ObjFile = new FileUpload();
                        ObjFile.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        ObjFile.Index = (Int32)(DALHelper.HandleDBNull(reader["FileIndex"]));
                        ObjFile.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        ObjFile.Data = (byte[])(DALHelper.HandleDBNull(reader["Value"]));
                        BizAction.LabDay6.FUSetting.Add(ObjFile);
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

        public override IValueObject GetLabDay5Score(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDay5ScoreBizActionVO BizActionObj = valueObject as clsGetDay5ScoreBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetFemaleDay5Score");

                dbServer.AddInParameter(command, "CoupleID", DbType.Int64, BizActionObj.CoupleID);
                dbServer.AddInParameter(command, "CoupleUnitID", DbType.Int64, BizActionObj.CoupleUnitID);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.Day5Score == null)
                        BizActionObj.Day5Score = new List<clsFemaleLabDay6FertilizationAssesmentVO>();
                    while (reader.Read())
                    {
                        clsFemaleLabDay6FertilizationAssesmentVO Obj = new clsFemaleLabDay6FertilizationAssesmentVO();

                        Obj.OoNo = (long)DALHelper.HandleDBNull(reader["OoNo"]);
                        Obj.Score = (long)DALHelper.HandleDBNull(reader["Score"]);
                        Obj.SelectedGrade.Description = (string)DALHelper.HandleDBNull(reader["Grade"]);
                        Obj.SelectedFePlan.Description = (string)DALHelper.HandleDBNull(reader["FertilisationStage"]);
                        Obj.PlanTreatmentID = (long)DALHelper.HandleDBNull(reader["PlanTreatmentID"]);
                        BizActionObj.Day5Score.Add(Obj);
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

        //added by neena
        public override IValueObject GetCleavageGradeMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetCleavageGradeMasterListBizActionVO BizAction = (valueObject) as clsGetCleavageGradeMasterListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCleavageGradeMasterList");

                dbServer.AddInParameter(command, "ApplyTo", DbType.Int64, BizAction.CleavageGrade.ApplyTo);
               

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizAction.CleavageGradeList == null)
                        BizAction.CleavageGradeList = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem();
                        item.ID = (long)(DALHelper.HandleDBNull(reader["ID"]));
                        item.Code = (string)(DALHelper.HandleDBNull(reader["Code"]));
                        item.Description = (string)(DALHelper.HandleDBNull(reader["Name"]));
                        item.Name = (string)(DALHelper.HandleDBNull(reader["Description"]));
                        item.Flag = (string)(DALHelper.HandleDBNull(reader["Flag"]));
                        item.Status = (bool)(DALHelper.HandleDBNull(reader["Status"]));
                        item.ApplyTo = (long)(DALHelper.HandleDBNull(reader["ApplyTo"]));
                        item.FragmentationID = (long)(DALHelper.HandleDBNull(reader["FragmentationID"]));
                        BizAction.CleavageGradeList.Add(item);
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


        public override IValueObject GetLab5And6MasterList(IValueObject valueObject, clsUserVO UserVO)
        {
            bool CurrentMethodExecutionStatus = true;

            clsGetLab5_6MasterListBizActionVO BizActionObj = (clsGetLab5_6MasterListBizActionVO)valueObject;

            try
            {
                StringBuilder FilterExpression = new StringBuilder();

                if (BizActionObj.IsActive.HasValue)
                    FilterExpression.Append("Status = '" + BizActionObj.IsActive.Value + "'");

                if (BizActionObj.Parent != null)
                {
                    if (FilterExpression.Length > 0)
                        FilterExpression.Append(" And ");
                    FilterExpression.Append(BizActionObj.Parent.Value.ToString() + "='" + BizActionObj.Parent.Key.ToString() + "'");
                }


                //Take storeprocedure name as input parameter and creates DbCommand Object.
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetIVFMasterList");
                //Adding MasterTableName as Input Parameter to filter record
                dbServer.AddInParameter(command, "MasterTableName", DbType.String, BizActionObj.MasterTable.ToString());
                dbServer.AddInParameter(command, "FilterExpression", DbType.String, FilterExpression.ToString());
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
                        BizActionObj.MasterList.Add(new MasterListItem((long)reader["ID"], reader["Code"].ToString(), reader["Description"].ToString(), reader["Name"].ToString(), reader["Flag"].ToString(), (bool)reader["Status"]));//HandleDBNull(reader["Date"])));

                        ////Added By CDS 22/2/16
                        //BizActionObj.MasterList.Add(new MasterListItem((long)reader["Id"], reader["Code"].ToString(), reader["Description"].ToString(), (DateTime?)DALHelper.HandleDate(reader["Date"])));//HandleDBNull(reader["Date"])));

                    }
                }
                //if (!reader.IsClosed)
                //{
                //    reader.Close();
                //}
                reader.Close();

            }
            catch (Exception ex)
            {
                CurrentMethodExecutionStatus = false;
                BizActionObj.Error = ex.Message;  //"Error Occured";
                //logManager.LogError(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            finally
            {
                //log error  
                //logManager.LogInfo(BizActionObj.GetBizActionGuid(), UserVo.UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), CurrentMethodExecutionStatus ? "Method Executed Successfully." : "There was a problem while Executing method.Check error log for details.");
            }


            return BizActionObj;
        }

    }

}
