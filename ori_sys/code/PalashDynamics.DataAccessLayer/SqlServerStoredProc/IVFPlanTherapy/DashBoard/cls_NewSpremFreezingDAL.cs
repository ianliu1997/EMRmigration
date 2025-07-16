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
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using PalashDynamics.ValueObjects.DashBoardVO;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    class cls_NewSpremFreezingDAL  : cls_NewBaseSpremFreezingDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private cls_NewSpremFreezingDAL()
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

        public override IValueObject AddUpdateSpremFrezing(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_NewAddUpdateSpremFreezingBizActionVO bizaction = valueObject as cls_NewAddUpdateSpremFreezingBizActionVO;
            DbTransaction trans = null;

            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                trans = con.BeginTransaction();




                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_AddDonorBatchDetails");

                dbServer.AddParameter(command4, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, bizaction.SpremFreezingMainVO.BatchID);
                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command4, "PatientID", DbType.Int64, bizaction.MalePatientID);
                dbServer.AddInParameter(command4, "PatientUnitID", DbType.Int64, bizaction.MalePatientUnitID);
                dbServer.AddInParameter(command4, "BatchCode", DbType.String, bizaction.SpremFreezingMainVO.BatchCode);
                dbServer.AddInParameter(command4, "InvoiceNo", DbType.String, null);
                dbServer.AddInParameter(command4, "ReceivedByID", DbType.Int64, 0);
                dbServer.AddInParameter(command4, "ReceivedDate", DbType.DateTime, null);
                dbServer.AddInParameter(command4, "LabID", DbType.Int64, bizaction.SpremFreezingMainVO.LabID);
                dbServer.AddInParameter(command4, "NoOfVails", DbType.Int32, bizaction.SpremFreezingMainVO.NoOfVails);
                dbServer.AddInParameter(command4, "Volume", DbType.Single, bizaction.SpremFreezingMainVO.Volume);
                dbServer.AddInParameter(command4, "AvailableVolume", DbType.Single, 0);
                dbServer.AddInParameter(command4, "Remark", DbType.String, null);
                dbServer.AddInParameter(command4, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command4, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command4, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command4, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command4, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                bizaction.SuccessStatus = (int)dbServer.GetParameterValue(command4, "ResultStatus");
                bizaction.SpremFreezingMainVO.BatchID = (long)dbServer.GetParameterValue(command4, "ID");
                bizaction.SpremFreezingMainVO.BatchUnitID = UserVo.UserLoginInfo.UnitId;



                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateSpremFreezing_New");

                if (bizaction.ID > 0)
                {
                    dbServer.AddInParameter(command, "ID", DbType.Int64, bizaction.ID);
                }
                else
                {
                    dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                }


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, bizaction.MalePatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, bizaction.MalePatientUnitID);
                //........................................................................
                dbServer.AddInParameter(command, "TherapyID", DbType.Int64, bizaction.SpremFreezingMainVO.TherapyID);
                dbServer.AddInParameter(command, "TherapyUnitID", DbType.Int64, bizaction.SpremFreezingMainVO.TherapyUnitID);
                dbServer.AddInParameter(command, "CycleCode", DbType.String, bizaction.SpremFreezingMainVO.CycleCode);
                dbServer.AddInParameter(command, "BatchID", DbType.Int64, bizaction.SpremFreezingMainVO.BatchID);
                dbServer.AddInParameter(command, "BatchUnitID", DbType.Int64, bizaction.SpremFreezingMainVO.BatchUnitID);
                //.............................................................................................................
                dbServer.AddInParameter(command, "DoctorID", DbType.Int64, bizaction.SpremFreezingMainVO.DoctorID);
                dbServer.AddInParameter(command, "EmbryologistID", DbType.Int64, bizaction.SpremFreezingMainVO.EmbryologistID);
                dbServer.AddInParameter(command, "SpremFreezingTime", DbType.DateTime, bizaction.SpremFreezingMainVO.SpremFreezingDate);
                dbServer.AddInParameter(command, "SpremFreezingDate", DbType.DateTime, bizaction.SpremFreezingMainVO.SpremFreezingDate);
                dbServer.AddInParameter(command, "CollectionMethodID", DbType.Int64, bizaction.SpremFreezingMainVO.CollectionMethodID);
                dbServer.AddInParameter(command, "ViscosityID", DbType.Int64, bizaction.SpremFreezingMainVO.ViscosityID);

                dbServer.AddInParameter(command, "CollectionProblem", DbType.String, bizaction.SpremFreezingMainVO.CollectionProblem);
                dbServer.AddInParameter(command, "Other", DbType.String, bizaction.SpremFreezingMainVO.Other);
                dbServer.AddInParameter(command, "Comments", DbType.String, bizaction.SpremFreezingMainVO.Comments);
                dbServer.AddInParameter(command, "Abstience", DbType.String, bizaction.SpremFreezingMainVO.Abstience);
                dbServer.AddInParameter(command, "Volume", DbType.Single, bizaction.SpremFreezingMainVO.Volume);
                dbServer.AddInParameter(command, "Motility", DbType.Decimal, bizaction.SpremFreezingMainVO.Motility);
                dbServer.AddInParameter(command, "GradeA", DbType.Decimal, bizaction.SpremFreezingMainVO.GradeA);
                dbServer.AddInParameter(command, "GradeB", DbType.Decimal, bizaction.SpremFreezingMainVO.GradeB);
                dbServer.AddInParameter(command, "GradeC", DbType.Decimal, bizaction.SpremFreezingMainVO.GradeC);
                dbServer.AddInParameter(command, "TotalSpremCount", DbType.Int64, bizaction.SpremFreezingMainVO.TotalSpremCount);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, bizaction.SpremFreezingMainVO.Status);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "SpremExpiryDate", DbType.DateTime, bizaction.SpremFreezingMainVO.SpremExpiryDate);
				//added by neena
                dbServer.AddInParameter(command, "SpermTypeID", DbType.Int64, bizaction.SpremFreezingMainVO.SpermTypeID);
                dbServer.AddInParameter(command, "SampleCode", DbType.String, bizaction.SpremFreezingMainVO.SampleCode);
                dbServer.AddInParameter(command, "SampleLinkID", DbType.Int64, bizaction.SpremFreezingVO.SampleLinkID);
                //

                #region For Andrology Flow added on 15052017

                dbServer.AddInParameter(command, "Spillage", DbType.String, bizaction.SpremFreezingMainVO.Spillage);

                dbServer.AddInParameter(command, "SpermCount", DbType.Decimal, bizaction.SpremFreezingMainVO.SpermCount);

                dbServer.AddInParameter(command, "DFI", DbType.Decimal, bizaction.SpremFreezingMainVO.DFI);
                dbServer.AddInParameter(command, "ROS", DbType.Decimal, bizaction.SpremFreezingMainVO.ROS);

                dbServer.AddInParameter(command, "HIV", DbType.Int64, bizaction.SpremFreezingMainVO.HIV);
                dbServer.AddInParameter(command, "HBSAG", DbType.Int64, bizaction.SpremFreezingMainVO.HBSAG);
                dbServer.AddInParameter(command, "VDRL", DbType.Int64, bizaction.SpremFreezingMainVO.VDRL);
                dbServer.AddInParameter(command, "HCV", DbType.Int64, bizaction.SpremFreezingMainVO.HCV);

                dbServer.AddInParameter(command, "CheckedByDoctorID", DbType.Int64, bizaction.SpremFreezingMainVO.CheckedByDoctorID);

                dbServer.AddInParameter(command, "IsConsentCheck", DbType.Boolean, bizaction.SpremFreezingMainVO.IsConsentCheck);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, bizaction.SpremFreezingMainVO.IsFreezed);

                dbServer.AddInParameter(command, "AbstienceID", DbType.Int64, bizaction.SpremFreezingMainVO.AbstienceID);

                dbServer.AddInParameter(command, "PusCells", DbType.String, bizaction.SpremFreezingMainVO.PusCells);
                dbServer.AddInParameter(command, "RoundCells", DbType.String, bizaction.SpremFreezingMainVO.RoundCells);
                dbServer.AddInParameter(command, "EpithelialCells", DbType.String, bizaction.SpremFreezingMainVO.EpithelialCells);
                dbServer.AddInParameter(command, "OtherCells", DbType.String, bizaction.SpremFreezingMainVO.OtherCells);

                dbServer.AddInParameter(command, "VisitID", DbType.Int64, bizaction.SpremFreezingMainVO.VisitID);
                dbServer.AddInParameter(command, "VisitUnitID", DbType.Int64, bizaction.SpremFreezingMainVO.VisitUnitID);

                #endregion

                //added by neena date 12 sep 2017               
                dbServer.AddInParameter(command, "Sperm5thPercentile", DbType.Single, bizaction.SpremFreezingMainVO.Sperm5thPercentile);
                dbServer.AddInParameter(command, "Sperm75thPercentile", DbType.Single, bizaction.SpremFreezingMainVO.Sperm75thPercentile);
                dbServer.AddInParameter(command, "Ejaculate5thPercentile", DbType.Single, bizaction.SpremFreezingMainVO.Ejaculate5thPercentile);
                dbServer.AddInParameter(command, "Ejaculate75thPercentile", DbType.Single, bizaction.SpremFreezingMainVO.Ejaculate75thPercentile);
                dbServer.AddInParameter(command, "TotalMotility5thPercentile", DbType.Single, bizaction.SpremFreezingMainVO.TotalMotility5thPercentile);
                dbServer.AddInParameter(command, "TotalMotility75thPercentile", DbType.Single, bizaction.SpremFreezingMainVO.TotalMotility75thPercentile);
             
                //

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                bizaction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                bizaction.ID = (long)dbServer.GetParameterValue(command, "ID");
                bizaction.UintID = (long)dbServer.GetParameterValue(command, "UnitID");
                // Add Details From Grid . . .

                //DbCommand Sqlcommand = dbServer.GetSqlStringCommand("Delete from T_IVF_SpremFreezingDetails where SpremFreezingID  = " + bizaction.ID + " And SpremFreezingUnitID = " + bizaction.UintID);
                // int sqlStatus = dbServer.ExecuteNonQuery(Sqlcommand, trans);

                foreach (var item in bizaction.SpremFreezingDetails)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddUpdateSpremFreezingDetails_New");

                    if (item.ID > 0)
                    {
                        dbServer.AddInParameter(command1, "ID", DbType.Int64, item.ID);
                    }
                    else
                    {
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                    }

                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "SpremFreezingID", DbType.Int64, bizaction.ID);
                    dbServer.AddInParameter(command1, "SpremFreezingUnitID", DbType.Int64, bizaction.UintID);
                    dbServer.AddInParameter(command1, "ColorCodeID", DbType.Int64, item.GobletColorID);
                    dbServer.AddInParameter(command1, "StrawID", DbType.Int64, item.StrawId);
                    dbServer.AddInParameter(command1, "GlobletShapeID", DbType.Int64, item.GobletShapeId);
                    dbServer.AddInParameter(command1, "GlobletSizeID", DbType.Int64, item.GobletSizeId);
                    dbServer.AddInParameter(command1, "PlanTherapy", DbType.Int64, item.PlanTherapy);
                    dbServer.AddInParameter(command1, "PlanTherapyUnitID", DbType.Int64, item.PlanTherapyUnitID);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, item.Status);
                    dbServer.AddInParameter(command1, "IsModify", DbType.String, item.IsModify);
                    dbServer.AddInParameter(command1, "IsThaw", DbType.String, item.IsThaw);
                    dbServer.AddInParameter(command1, "CaneID", DbType.Int64, item.CanID);
                    dbServer.AddInParameter(command1, "CanisterID", DbType.Int64, item.CanisterId);
                    dbServer.AddInParameter(command1, "TankID", DbType.Int64, item.TankId);
                    dbServer.AddInParameter(command1, "Comments", DbType.String, item.Comments);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = 0;
                    intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                    long SpermNo = Convert.ToInt64(dbServer.GetParameterValue(command1, "ID"));
                    //bizaction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                    if (item.IsThaw == true)
                    {
                        if (item.IsModify == true)
                        {
                            DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_AddUpdateSpremFreezing_New1");
                            if (bizaction.ID > 0)
                            {
                                dbServer.AddInParameter(command3, "ID", DbType.Int64, bizaction.ID);
                            }
                            else
                            {
                                dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            }
                            dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "PatientID", DbType.Int64, bizaction.MalePatientID);
                            dbServer.AddInParameter(command3, "PatientUnitID", DbType.Int64, bizaction.MalePatientUnitID);
                            dbServer.AddInParameter(command3, "TherapyID", DbType.Int64, item.PlanTherapy);
                            dbServer.AddInParameter(command3, "TherapyUnitID", DbType.Int64, item.PlanTherapyUnitID);
                            dbServer.AddInParameter(command3, "SpermFreezingDetailsID", DbType.Int64, SpermNo);
                            dbServer.AddInParameter(command3, "SpermFreezingDetailsUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "BatchID", DbType.Int64, bizaction.SpremFreezingMainVO.BatchID);
                            dbServer.AddInParameter(command3, "BatchUnitID", DbType.Int64, bizaction.SpremFreezingMainVO.BatchUnitID);
                            dbServer.AddInParameter(command3, "UsedVolume", DbType.Single, 0);
                            dbServer.AddInParameter(command3, "CycleCode", DbType.String, null);
                            dbServer.AddInParameter(command3, "FreezingID", DbType.Int64, bizaction.ID);
                            dbServer.AddInParameter(command3, "FreezingUnitID", DbType.Int64, bizaction.UintID);
                            dbServer.AddInParameter(command3, "SpermNo", DbType.Int64, SpermNo);
                            dbServer.AddInParameter(command3, "LabPersonID", DbType.Int64, bizaction.SpremFreezingMainVO.DoctorID);
                            dbServer.AddInParameter(command3, "VitrificationDate", DbType.DateTime, bizaction.SpremFreezingMainVO.SpremFreezingDate);
                            dbServer.AddInParameter(command3, "VitrificationTime", DbType.DateTime, bizaction.SpremFreezingMainVO.SpremFreezingTime);
                            dbServer.AddInParameter(command3, "Volume", DbType.Decimal, bizaction.SpremFreezingMainVO.Volume);
                            dbServer.AddInParameter(command3, "Motility", DbType.Decimal, bizaction.SpremFreezingMainVO.Motility);
                            dbServer.AddInParameter(command3, "TotalSpremCount", DbType.Int64, bizaction.SpremFreezingMainVO.TotalSpremCount);
                            dbServer.AddInParameter(command3, "Status", DbType.Boolean, bizaction.SpremFreezingMainVO.Status);
                            dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);
                            int intStatus2 = dbServer.ExecuteNonQuery(command3, trans);
                            bizaction.SuccessStatus = (int)dbServer.GetParameterValue(command3, "ResultStatus");
                        }
                        else
                        {

                            DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateSpremFreezing_New1");
                            if (bizaction.ID > 0)
                            {
                                dbServer.AddInParameter(command2, "ID", DbType.Int64, bizaction.ID);
                            }
                            else
                            {
                                dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
                            }
                            dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "PatientID", DbType.Int64, bizaction.MalePatientID);
                            dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, bizaction.MalePatientUnitID);
                            dbServer.AddInParameter(command2, "LabPersonID", DbType.Int64, bizaction.SpremFreezingMainVO.DoctorID);
                            dbServer.AddInParameter(command2, "TherapyID", DbType.Int64, item.PlanTherapy);
                            dbServer.AddInParameter(command2, "TherapyUnitID", DbType.Int64, item.PlanTherapyUnitID);
                            dbServer.AddInParameter(command2, "SpermFreezingDetailsID", DbType.Int64, SpermNo);
                            dbServer.AddInParameter(command2, "SpermFreezingDetailsUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "BatchID", DbType.Int64, bizaction.SpremFreezingMainVO.BatchID);
                            dbServer.AddInParameter(command2, "BatchUnitID", DbType.Int64, bizaction.SpremFreezingMainVO.BatchUnitID);
                            dbServer.AddInParameter(command2, "UsedVolume", DbType.Single, 0);
                            dbServer.AddInParameter(command2, "CycleCode", DbType.String, null);
                            dbServer.AddInParameter(command2, "FreezingID", DbType.Int64, bizaction.ID);
                            dbServer.AddInParameter(command2, "FreezingUnitID", DbType.Int64, bizaction.UintID);
                            dbServer.AddInParameter(command2, "SpermNo", DbType.Int64, SpermNo);
                            dbServer.AddInParameter(command2, "VitrificationDate", DbType.DateTime, bizaction.SpremFreezingMainVO.SpremFreezingDate);
                            dbServer.AddInParameter(command2, "VitrificationTime", DbType.DateTime, bizaction.SpremFreezingMainVO.SpremFreezingTime);
                            dbServer.AddInParameter(command2, "Volume", DbType.Decimal, bizaction.SpremFreezingMainVO.Volume);
                            dbServer.AddInParameter(command2, "Motility", DbType.Decimal, bizaction.SpremFreezingMainVO.Motility);
                            dbServer.AddInParameter(command2, "TotalSpremCount", DbType.Int64, bizaction.SpremFreezingMainVO.TotalSpremCount);
                            dbServer.AddInParameter(command2, "Status", DbType.Boolean, bizaction.SpremFreezingMainVO.Status);
                            dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, System.DateTime.Now);
                            dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                            int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                            bizaction.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                        }

                    }
                }
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                trans.Commit();
                con.Close();
                con = null;
                trans = null;
            }
            return bizaction;
        }

        public override IValueObject GetSpremFreezingNew(IValueObject valuObject, clsUserVO UserVO)
        {
            cls_NewGetSpremFreezingBizActionVO Bizaction =  (valuObject) as cls_NewGetSpremFreezingBizActionVO;
            Bizaction.SpremFreezingMainVO = new cls_NewSpremFreezingMainVO();
            //Bizaction.SpremFreezingVO = new clsNew_SpremFreezingVO();

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSpremFreezing_New");
                dbServer.AddInParameter(command, "ID", DbType.Int64, Bizaction.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.String, Bizaction.UintID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Bizaction.SpremFreezingMainVO.SpermTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TypeOfSpermID"]));
                        Bizaction.SpremFreezingMainVO.SampleCode = Convert.ToString(DALHelper.HandleDBNull(reader["SampleCode"]));

                        Bizaction.SpremFreezingMainVO.SpremFreezingDate = (DateTime?)(DALHelper.HandleDate(reader["SpremFreezingDate"]));
                        Bizaction.SpremFreezingMainVO.SpremFreezingTime = (DateTime?)(DALHelper.HandleDate(reader["SpremFreezingDate"]));
                        Bizaction.SpremFreezingMainVO.Abstience = Convert.ToString(DALHelper.HandleDBNull(reader["Abstience"]));
                        Bizaction.SpremFreezingMainVO.CollectionMethodID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionMethodID"]));
                        Bizaction.SpremFreezingMainVO.CollectionProblem = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionProblem"]));
                        Bizaction.SpremFreezingMainVO.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                        Bizaction.SpremFreezingMainVO.DoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        Bizaction.SpremFreezingMainVO.EmbryologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["EmbryologistID"]));
                        Bizaction.SpremFreezingMainVO.GradeA = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeA"]));
                        Bizaction.SpremFreezingMainVO.GradeB = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeB"]));
                        Bizaction.SpremFreezingMainVO.GradeC = Convert.ToDecimal(DALHelper.HandleDBNull(reader["GradeC"]));
                        Bizaction.SpremFreezingMainVO.Motility = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Motility"]));
                        Bizaction.SpremFreezingMainVO.Other = Convert.ToString(DALHelper.HandleDBNull(reader["Other"]));
                        Bizaction.SpremFreezingMainVO.TotalSpremCount = Convert.ToInt64(DALHelper.HandleDBNull(reader["TotalSpremCount"]));
                        Bizaction.SpremFreezingMainVO.ViscosityID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ViscosityID"]));
                        Bizaction.SpremFreezingMainVO.Volume = Convert.ToSingle(DALHelper.HandleDBNull(reader["Volume"]));
                        Bizaction.SpremFreezingMainVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainID"]));
                        Bizaction.SpremFreezingMainVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainUnitID"]));
                        Bizaction.SpremFreezingMainVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        //..................................................................................................................
                        Bizaction.SpremFreezingMainVO.TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"]));
                        Bizaction.SpremFreezingMainVO.TherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitID"]));
                        Bizaction.SpremFreezingMainVO.CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"]));
                        Bizaction.SpremFreezingMainVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        Bizaction.SpremFreezingMainVO.BatchUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchUnitID"]));
                        Bizaction.SpremFreezingMainVO.NoOfVails = Convert.ToSingle(DALHelper.HandleDBNull(reader["NoOfVails"]));
                        Bizaction.SpremFreezingMainVO.SpremExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["SpremExpiryDate"]));
                
                        //..................................................................................................................

                        clsNew_SpremFreezingVO VO = new clsNew_SpremFreezingVO();
                        VO.CanID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CaneID"]));
                        VO.CanisterId = Convert.ToInt64(DALHelper.HandleDBNull(reader["CanisterID"]));
                        VO.GobletColorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ColorCodeID"]));
                        VO.GobletShapeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GlobletShapeID"]));
                        VO.GobletSizeId = Convert.ToInt64(DALHelper.HandleDBNull(reader["GlobletSizeID"]));
                        VO.SpremNostr = Convert.ToString(DALHelper.HandleDBNull(reader["SpremNo"]));
                        VO.StrawId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StrawID"]));
                        VO.TankId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TankID"]));
                        VO.Comments = Convert.ToString(DALHelper.HandleDBNull(reader["Comments"]));
                        VO.IsThaw = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsThaw"]));
                        VO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));

                        #region  For Andrology Flow added on 15052017

                        Bizaction.SpremFreezingMainVO.Spillage = Convert.ToString(DALHelper.HandleDBNull(reader["Spillage"]));

                        Bizaction.SpremFreezingMainVO.SpermCount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SpermCount"]));
                        Bizaction.SpremFreezingMainVO.DFI = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DFI"]));
                        Bizaction.SpremFreezingMainVO.ROS = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ROS"]));

                        Bizaction.SpremFreezingMainVO.HIV = Convert.ToInt64(DALHelper.HandleDBNull(reader["HIV"]));
                        Bizaction.SpremFreezingMainVO.HBSAG = Convert.ToInt64(DALHelper.HandleDBNull(reader["HBSAG"]));
                        Bizaction.SpremFreezingMainVO.VDRL = Convert.ToInt64(DALHelper.HandleDBNull(reader["VDRL"]));
                        Bizaction.SpremFreezingMainVO.HCV = Convert.ToInt64(DALHelper.HandleDBNull(reader["HCV"]));

                        Bizaction.SpremFreezingMainVO.CheckedByDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CheckedByDoctorID"]));

                        Bizaction.SpremFreezingMainVO.IsConsentCheck = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConsentCheck"]));
                        Bizaction.SpremFreezingMainVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));

                        Bizaction.SpremFreezingMainVO.AbstienceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AbstienceID"]));

                        Bizaction.SpremFreezingMainVO.PusCells = Convert.ToString(DALHelper.HandleDBNull(reader["PusCells"]));
                        Bizaction.SpremFreezingMainVO.RoundCells = Convert.ToString(DALHelper.HandleDBNull(reader["RoundCells"]));
                        Bizaction.SpremFreezingMainVO.EpithelialCells = Convert.ToString(DALHelper.HandleDBNull(reader["EpithelialCells"]));
                        Bizaction.SpremFreezingMainVO.OtherCells = Convert.ToString(DALHelper.HandleDBNull(reader["OtherCells"]));

                        Bizaction.SpremFreezingMainVO.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        Bizaction.SpremFreezingMainVO.VisitUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitUnitID"]));

                        #endregion


                        //added by neena date 12 sep 2017                      
                        Bizaction.SpremFreezingMainVO.Sperm5thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["Sperm5thPercentile"]);
                        Bizaction.SpremFreezingMainVO.Sperm75thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["Sperm75thPercentile"]);
                        Bizaction.SpremFreezingMainVO.Ejaculate5thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["Ejaculate5thPercentile"]);
                        Bizaction.SpremFreezingMainVO.Ejaculate75thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["Ejaculate75thPercentile"]);
                        Bizaction.SpremFreezingMainVO.TotalMotility5thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["TotalMotility5thPercentile"]);
                        Bizaction.SpremFreezingMainVO.TotalMotility75thPercentile = (float)(Double)DALHelper.HandleDBNull(reader["TotalMotility75thPercentile"]);                       
                        //
                        Bizaction.SpremFreezingDetails.Add(VO);
                    }
                }
            }
            catch (Exception ex)
            {                
                throw;
            }

            return Bizaction;
        }

        public override IValueObject DeleteSpremFreezingNew(IValueObject valuObject, clsUserVO UserVO)
        {
            cls_NewDeleteSpremFreezingBizActionVO bizaction = (valuObject) as cls_NewDeleteSpremFreezingBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteSpremfreezingDetails_New");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, bizaction.MalePatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, bizaction.MalePatientUnitID);
                dbServer.AddInParameter(command, "SpremID", DbType.Int64, bizaction.SpremID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, bizaction.Status);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                bizaction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception ex)
            {
                con.Close();
                throw ex;

            }
            finally
            {
                con.Close();
            }
            return bizaction;
        }


        //By Anjali.......................................
        public override IValueObject GetSpremFreezingList(IValueObject valuObject, clsUserVO UserVO)
        {
            cls_NewGetListSpremFreezingBizActionVO Bizaction = (valuObject) as cls_NewGetListSpremFreezingBizActionVO;
            Bizaction.SpremFreezingMainList = new List<cls_NewSpremFreezingMainVO>();
           
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetSpremFreezingList");
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, Bizaction.MalePatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.String, Bizaction.MalePatientUnitID);
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cls_NewSpremFreezingMainVO objVO = new cls_NewSpremFreezingMainVO();
                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.SpremFreezingDate = (DateTime?)(DALHelper.HandleDate(reader["SpremFreezingDate"]));
                        objVO.SpremFreezingTime = (DateTime?)(DALHelper.HandleDate(reader["SpremFreezingTime"]));
                        objVO.CollectionMethod = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionMethod"]));
                        objVO.Doctor = Convert.ToString(DALHelper.HandleDBNull(reader["Doctor"]));
                        objVO.Embryologist = Convert.ToString(DALHelper.HandleDBNull(reader["Embryologist"]));
                        objVO.SpermType = Convert.ToString(DALHelper.HandleDBNull(reader["SpermType"]));
                        //..................................................................................................................
                        objVO.TherapyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyID"]));
                        objVO.TherapyUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TherapyUnitID"]));
                        objVO.CycleCode = Convert.ToString(DALHelper.HandleDBNull(reader["CycleCode"]));
                        objVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objVO.BatchUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchUnitID"]));
                        objVO.NoOfVails = Convert.ToSingle(DALHelper.HandleDBNull(reader["NoOfVails"]));
                   
                        //..................................................................................................................

                        objVO.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));   // For Andrology Flow added on 15052017
                        objVO.Motility = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Motility"]));

                        Bizaction.SpremFreezingMainList.Add(objVO);
                       
                    }
                }
            }
            catch (Exception ex)
            {                
                throw;
            }

            return Bizaction;
        }
        
        //....................................................
    }
}
