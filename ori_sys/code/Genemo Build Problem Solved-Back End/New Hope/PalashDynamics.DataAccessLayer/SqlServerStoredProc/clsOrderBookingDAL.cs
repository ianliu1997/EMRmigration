namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.MachineParameter;
    using PalashDynamics.ValueObjects.Pathology;
    using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    public class clsOrderBookingDAL : clsBaseOrderBookingDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsOrderBookingDAL()
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
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddHistory(IValueObject valueObject, clsUserVO UserVo)
        {
            clsRemarkHistoryBizActionVO nvo = valueObject as clsRemarkHistoryBizActionVO;
            this.dbServer.GetStoredProcCommand("CIMS_AddRemarkHistory");
            try
            {
                foreach (clsPathOrderBookingDetailVO lvo in nvo.RemarkHistory)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPathoRemarkHistory");
                    this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, nvo.OrderID);
                    this.dbServer.AddInParameter(storedProcCommand, "OrderUnitID", DbType.Int64, nvo.OrderUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, nvo.Remark);
                    this.dbServer.AddInParameter(storedProcCommand, "UserName", DbType.String, nvo.UserName);
                    this.dbServer.AddInParameter(storedProcCommand, "OrderDetailID", DbType.Int64, lvo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "OrderDetailUnitId", DbType.Int64, lvo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, lvo.ID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddMachineToSubTest(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddMachineToSubTestbizActionVO nvo = valueObject as clsAddMachineToSubTestbizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                clsPathoTestMasterVO itemSupplier = nvo.ItemSupplier;
                int num = 0;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddMachineListForSubTest");
                if (nvo.ItemSupplierList.Count > 0)
                {
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 > (nvo.ItemSupplierList.Count - 1))
                        {
                            nvo.SuccessStatus = num;
                            break;
                        }
                        storedProcCommand.Parameters.Clear();
                        this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, 0);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, itemSupplier.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "SubTestID", DbType.Int64, itemSupplier.SubTestID);
                        this.dbServer.AddInParameter(storedProcCommand, "MchineID", DbType.Int64, nvo.ItemSupplierList[num2].ID);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ItemSupplierList[num2].status);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, itemSupplier.CreatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, itemSupplier.UpdatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, itemSupplier.AddedBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, itemSupplier.AddedOn);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, itemSupplier.AddedDateTime);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, itemSupplier.AddedWindowsLoginName);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                        int num3 = this.dbServer.ExecuteNonQuery(storedProcCommand);
                        if (num3 > 0)
                        {
                            num = 1;
                        }
                        num2++;
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject AddMachineToTest(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddMachineToTestbizActionVO nvo = valueObject as clsAddMachineToTestbizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                clsPathoTestMasterVO itemSupplier = nvo.ItemSupplier;
                int num = 0;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddMachineListForTest");
                if (nvo.ItemSupplierList.Count > 0)
                {
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 > (nvo.ItemSupplierList.Count - 1))
                        {
                            nvo.SuccessStatus = num;
                            break;
                        }
                        storedProcCommand.Parameters.Clear();
                        this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, 0);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, itemSupplier.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, itemSupplier.TestID);
                        this.dbServer.AddInParameter(storedProcCommand, "MchineID", DbType.Int64, nvo.ItemSupplierList[num2].ID);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ItemSupplierList[num2].status);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, itemSupplier.CreatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, itemSupplier.UpdatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, itemSupplier.AddedBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, itemSupplier.AddedOn);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, itemSupplier.AddedDateTime);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, itemSupplier.AddedWindowsLoginName);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                        int num3 = this.dbServer.ExecuteNonQuery(storedProcCommand);
                        if (num3 > 0)
                        {
                            num = 1;
                        }
                        num2++;
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject AddPathologistToTemp(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddPathologistToTempbizActionVO nvo = valueObject as clsAddPathologistToTempbizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                clsPathoTestTemplateDetailsVO itemSupplier = nvo.ItemSupplier;
                int num = 0;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPathologistListForTest");
                if (nvo.ItemSupplierList.Count > 0)
                {
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 > (nvo.ItemSupplierList.Count - 1))
                        {
                            nvo.SuccessStatus = num;
                            break;
                        }
                        storedProcCommand.Parameters.Clear();
                        this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, 0);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, itemSupplier.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, itemSupplier.TemplateID);
                        this.dbServer.AddInParameter(storedProcCommand, "PathologistID", DbType.Int64, nvo.ItemSupplierList[num2].ID);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ItemSupplierList[num2].Status);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                        int num3 = this.dbServer.ExecuteNonQuery(storedProcCommand);
                        if (num3 > 0)
                        {
                            num = 1;
                        }
                        num2++;
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject AddPathoPathoProfileMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPathoProfileMasterBizActionVO nvo = valueObject as clsAddPathoProfileMasterBizActionVO;
            try
            {
                clsPathoProfileMasterVO profileDetails = nvo.ProfileDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoProfileMaster");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, profileDetails.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, profileDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, profileDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (nvo.SuccessStatus == 1)
                {
                    nvo.ProfileDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    foreach (clsPathoProfileTestDetailsVO svo in profileDetails.PathoTestList)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoProfileTestDetails");
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "ProfileID", DbType.Int64, nvo.ProfileDetails.ID);
                        this.dbServer.AddInParameter(command2, "TestID", DbType.Int64, svo.TestID);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command2, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command2, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command2, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddPathOrderBooking(IValueObject valueObject, clsUserVO UserVo, DbTransaction myTrans, DbConnection myCon)
        {
            clsAddPathOrderBookingBizActionVO nvo = valueObject as clsAddPathOrderBookingBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = (myCon == null) ? this.dbServer.CreateConnection() : myCon;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                if (myTrans != null)
                {
                    transaction = myTrans;
                }
                else
                {
                    connection.BeginTransaction();
                }
                clsPathOrderBookingVO pathOrderBooking = nvo.PathOrderBooking;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPathOrderBooking");
                this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, pathOrderBooking.LinkServer);
                if (pathOrderBooking.LinkServer != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, pathOrderBooking.LinkServer.Replace(@"\", "_"));
                }
                this.dbServer.AddInParameter(storedProcCommand, "OrderNo", DbType.String, pathOrderBooking.OrderNo);
                this.dbServer.AddInParameter(storedProcCommand, "BillID", DbType.String, pathOrderBooking.BillID);
                this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, pathOrderBooking.BillNo);
                this.dbServer.AddInParameter(storedProcCommand, "ChargeID", DbType.Int64, pathOrderBooking.ChargeID);
                this.dbServer.AddInParameter(storedProcCommand, "TariffServiceID", DbType.Int64, pathOrderBooking.TariffServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "TestCharges", DbType.Single, pathOrderBooking.TestCharges);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorID", DbType.Int64, pathOrderBooking.DoctorID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, pathOrderBooking.OrderDate);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, pathOrderBooking.Time);
                this.dbServer.AddInParameter(storedProcCommand, "SampleType", DbType.Boolean, pathOrderBooking.SampleType);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_ID", DbType.Int64, pathOrderBooking.Opd_Ipd_External_ID);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_UnitID", DbType.Int64, pathOrderBooking.Opd_Ipd_External_UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External", DbType.Int64, pathOrderBooking.Opd_Ipd_External);
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Boolean, pathOrderBooking.IsApproved);
                this.dbServer.AddInParameter(storedProcCommand, "IsDelivered", DbType.Boolean, pathOrderBooking.IsDelivered);
                this.dbServer.AddInParameter(storedProcCommand, "IsCompleted", DbType.Boolean, pathOrderBooking.IsCompleted);
                this.dbServer.AddInParameter(storedProcCommand, "IsPrinted", DbType.Boolean, pathOrderBooking.IsPrinted);
                this.dbServer.AddInParameter(storedProcCommand, "IsOrderGenerated", DbType.Boolean, pathOrderBooking.IsOrderGenerated);
                this.dbServer.AddInParameter(storedProcCommand, "IsExternalPatient", DbType.Boolean, pathOrderBooking.IsExternalPatient);
                this.dbServer.AddInParameter(storedProcCommand, "TestType", DbType.Int64, pathOrderBooking.TestType);
                this.dbServer.AddInParameter(storedProcCommand, "IsCancelled", DbType.Boolean, pathOrderBooking.IsCancelled);
                this.dbServer.AddInParameter(storedProcCommand, "IsResultEntry", DbType.Boolean, pathOrderBooking.IsResultEntry);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, pathOrderBooking.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, ParameterDirection.InputOutput, null, DataRowVersion.Default, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, pathOrderBooking.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.PathOrderBooking.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.PathOrderBooking.Date = Convert.ToDateTime(this.dbServer.GetParameterValue(storedProcCommand, "AddedDateTime"));
                if ((nvo.PathOrderBookingDetailList != null) && (nvo.PathOrderBooking.ID != 0L))
                {
                    List<clsPathOrderBookingDetailVO> pathOrderBookingDetailList = nvo.PathOrderBookingDetailList;
                    int count = nvo.PathOrderBookingDetailList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddPathOrderBookingDetail");
                        this.dbServer.AddInParameter(command2, "LinkServer", DbType.String, pathOrderBookingDetailList[i].LinkServer);
                        if (pathOrderBookingDetailList[i].LinkServer != null)
                        {
                            this.dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, pathOrderBookingDetailList[i].LinkServer.Replace(@"\", "_"));
                        }
                        this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, nvo.PathOrderBooking.Date);
                        this.dbServer.AddInParameter(command2, "OrderID", DbType.Int64, nvo.PathOrderBooking.ID);
                        this.dbServer.AddInParameter(command2, "TestID", DbType.Int64, pathOrderBookingDetailList[i].TestID);
                        this.dbServer.AddInParameter(command2, "ChargeID", DbType.Int64, pathOrderBookingDetailList[i].ChargeID);
                        this.dbServer.AddInParameter(command2, "TariffServiceID", DbType.Int64, pathOrderBookingDetailList[i].TariffServiceID);
                        this.dbServer.AddInParameter(command2, "IsEmergency", DbType.Boolean, pathOrderBookingDetailList[i].IsEmergency);
                        this.dbServer.AddInParameter(command2, "TestCharges", DbType.Single, pathOrderBookingDetailList[i].TestCharges);
                        this.dbServer.AddInParameter(command2, "PathologistID", DbType.Int64, pathOrderBookingDetailList[i].PathologistID);
                        this.dbServer.AddInParameter(command2, "Specimen", DbType.String, pathOrderBookingDetailList[i].Specimen);
                        this.dbServer.AddInParameter(command2, "ClinicalNote", DbType.String, pathOrderBookingDetailList[i].ClinicalNote);
                        this.dbServer.AddInParameter(command2, "SampleNo", DbType.String, pathOrderBookingDetailList[i].SampleNo);
                        this.dbServer.AddInParameter(command2, "FirstLevel", DbType.Boolean, pathOrderBookingDetailList[i].FirstLevel);
                        this.dbServer.AddInParameter(command2, "SecondLevel", DbType.Boolean, pathOrderBookingDetailList[i].SecondLevel);
                        this.dbServer.AddInParameter(command2, "ThirdLevel", DbType.Boolean, pathOrderBookingDetailList[i].ThirdLevel);
                        this.dbServer.AddInParameter(command2, "FirstLevelID", DbType.Int64, pathOrderBookingDetailList[i].FirstLevelID);
                        this.dbServer.AddInParameter(command2, "SecondLevelID", DbType.Int64, pathOrderBookingDetailList[i].SecondLevelID);
                        this.dbServer.AddInParameter(command2, "ThirdLevelID", DbType.Int64, pathOrderBookingDetailList[i].ThirdLevelID);
                        this.dbServer.AddInParameter(command2, "IsApproved", DbType.Boolean, pathOrderBookingDetailList[i].IsApproved);
                        this.dbServer.AddInParameter(command2, "IsCompleted", DbType.Boolean, pathOrderBookingDetailList[i].IsCompleted);
                        this.dbServer.AddInParameter(command2, "IsDelivered", DbType.Boolean, pathOrderBookingDetailList[i].IsDelivered);
                        this.dbServer.AddInParameter(command2, "IsPrinted", DbType.Boolean, pathOrderBookingDetailList[i].IsPrinted);
                        this.dbServer.AddInParameter(command2, "MicrobiologistID", DbType.Int64, pathOrderBookingDetailList[i].MicrobiologistID);
                        this.dbServer.AddInParameter(command2, "Pathologist_1_ID", DbType.Int64, pathOrderBookingDetailList[i].Pathologist_1_ID);
                        this.dbServer.AddInParameter(command2, "Pathologist_2_ID", DbType.Int64, pathOrderBookingDetailList[i].Pathologist_2_ID);
                        this.dbServer.AddInParameter(command2, "RefDoctor", DbType.String, pathOrderBookingDetailList[i].RefDoctor);
                        this.dbServer.AddInParameter(command2, "SampleCollectionNO", DbType.String, pathOrderBookingDetailList[i].SampleCollectionNO);
                        this.dbServer.AddInParameter(command2, "IsOutSourced", DbType.Boolean, pathOrderBookingDetailList[i].IsOutSourced);
                        this.dbServer.AddInParameter(command2, "ExtAgencyID", DbType.Int64, pathOrderBookingDetailList[i].AgencyID);
                        this.dbServer.AddInParameter(command2, "Quantity", DbType.Double, pathOrderBookingDetailList[i].Quantity);
                        this.dbServer.AddInParameter(command2, "IsSampleCollected", DbType.Boolean, pathOrderBookingDetailList[i].IsSampleCollected);
                        this.dbServer.AddInParameter(command2, "SampleCollected", DbType.DateTime, pathOrderBookingDetailList[i].SampleCollected);
                        this.dbServer.AddInParameter(command2, "ItemConsID", DbType.Int64, pathOrderBookingDetailList[i].ItemConsID);
                        this.dbServer.AddInParameter(command2, "IsResultEntry", DbType.Boolean, pathOrderBookingDetailList[i].IsResultEntry);
                        this.dbServer.AddInParameter(command2, "IsFinalized", DbType.Boolean, pathOrderBookingDetailList[i].IsFinalized);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, pathOrderBooking.Status);
                        this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, pathOrderBookingDetailList[i].ID);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                        nvo.PathOrderBookingDetailList[i].ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                    }
                }
                nvo.SuccessStatus = 0;
                if (myCon == null)
                {
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                if (myCon == null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                if (myCon == null)
                {
                    connection.Close();
                    connection = null;
                    transaction = null;
                }
            }
            return valueObject;
        }

        public override IValueObject AddPathoTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPathoTemplateMasterBizActionVO bizActionObj = valueObject as clsAddPathoTemplateMasterBizActionVO;
            bizActionObj = (bizActionObj.TemplateDetails.ID != 0L) ? this.UpdateTemplateDetails(bizActionObj, UserVo) : this.AddTemplateDetails(bizActionObj, UserVo);
            return valueObject;
        }

        public override IValueObject AddPathoTestMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPathoTestMasterBizActionVO nvo = valueObject as clsAddPathoTestMasterBizActionVO;
            try
            {
                clsPathoTestMasterVO testDetails = nvo.TestDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoTestMasterNew1");
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, testDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, testDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "TestPrintName", DbType.String, testDetails.TestPrintName);
                this.dbServer.AddInParameter(storedProcCommand, "IsSubTest", DbType.Boolean, testDetails.IsSubTest);
                this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, testDetails.CategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, testDetails.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "IsParameter", DbType.Boolean, false);
                this.dbServer.AddInParameter(storedProcCommand, "TurnAroundTime", DbType.Double, testDetails.TurnAroundTime);
                this.dbServer.AddInParameter(storedProcCommand, "TubeID", DbType.Int64, testDetails.TubeID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFormTemplate", DbType.Int64, testDetails.IsFormTemplate);
                this.dbServer.AddInParameter(storedProcCommand, "IsAbnormal", DbType.Boolean, testDetails.IsAbnormal);
                this.dbServer.AddInParameter(storedProcCommand, "Note", DbType.String, testDetails.Note);
                this.dbServer.AddInParameter(storedProcCommand, "HasNormalRange", DbType.Boolean, testDetails.HasNormalRange);
                this.dbServer.AddInParameter(storedProcCommand, "HasObserved", DbType.Boolean, testDetails.HasObserved);
                this.dbServer.AddInParameter(storedProcCommand, "PrintTestName", DbType.Boolean, testDetails.PrintTestName);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, testDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "FreeFormat", DbType.Boolean, testDetails.FreeFormat);
                this.dbServer.AddInParameter(storedProcCommand, "NeedAuthorization", DbType.Boolean, testDetails.NeedAuthorization);
                this.dbServer.AddInParameter(storedProcCommand, "IsCultureSenTest", DbType.Boolean, testDetails.IsCultureSenTest);
                this.dbServer.AddInParameter(storedProcCommand, "MachineID", DbType.Int64, testDetails.MachineID);
                this.dbServer.AddInParameter(storedProcCommand, "Technique", DbType.String, testDetails.Technique);
                if (testDetails.FootNote != string.Empty)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FootNote", DbType.String, testDetails.FootNote);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FootNote", DbType.String, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "AppTo", DbType.Int32, testDetails.Applicableto);
                this.dbServer.AddInParameter(storedProcCommand, "ReportTemplate", DbType.Int32, testDetails.ReportTemplate);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, testDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                if (nvo.SuccessStatus == 1)
                {
                    nvo.TestDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    if (nvo.TestDetails.IsFromParameter)
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeletePathologyTemplate");
                        this.dbServer.AddInParameter(command2, "TestID", DbType.Int64, nvo.TestDetails.ID);
                        this.dbServer.ExecuteNonQuery(command2);
                        if (testDetails.IsSubTest)
                        {
                            DbCommand command8 = this.dbServer.GetStoredProcCommand("CIMS_DeletePathologyParaSubTest");
                            this.dbServer.AddInParameter(command8, "TestID", DbType.Int64, nvo.TestDetails.ID);
                            this.dbServer.AddInParameter(command8, "IsfromSubTest", DbType.Boolean, true);
                            this.dbServer.ExecuteNonQuery(command8);
                            DbCommand command9 = this.dbServer.GetStoredProcCommand("CIMS_DeletePathologySampleSubTest");
                            this.dbServer.AddInParameter(command9, "TestID", DbType.Int64, nvo.TestDetails.ID);
                            this.dbServer.AddInParameter(command9, "IsfromSubTest", DbType.Boolean, true);
                            this.dbServer.ExecuteNonQuery(command9);
                            foreach (clsPathoTestParameterVO rvo3 in testDetails.TestParameterList)
                            {
                                DbCommand command10 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoSubTestParameter");
                                this.dbServer.AddInParameter(command10, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command10, "PathoTestID", DbType.Int64, nvo.TestDetails.ID);
                                this.dbServer.AddInParameter(command10, "ParamSTID", DbType.Int64, rvo3.ParamSTID);
                                this.dbServer.AddInParameter(command10, "IsParameter", DbType.Boolean, true);
                                this.dbServer.AddInParameter(command10, "PrintNameType", DbType.Int64, rvo3.SelectedPrintName.ID);
                                this.dbServer.AddInParameter(command10, "PrintName", DbType.String, rvo3.Print);
                                this.dbServer.AddInParameter(command10, "PrintPosition", DbType.Int64, rvo3.PrintPosition);
                                this.dbServer.AddInParameter(command10, "Status", DbType.Boolean, true);
                                this.dbServer.AddInParameter(command10, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command10, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command10, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command10, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command10, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                this.dbServer.AddInParameter(command10, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command10, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command10, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                this.dbServer.AddInParameter(command10, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddInParameter(command10, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddParameter(command10, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo3.ID);
                                this.dbServer.AddOutParameter(command10, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command10);
                            }
                            foreach (clsPathoTestSampleVO evo2 in testDetails.TestSampleList)
                            {
                                DbCommand command11 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoSubTestSample");
                                this.dbServer.AddInParameter(command11, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command11, "PathoTestID", DbType.Int64, nvo.TestDetails.ID);
                                this.dbServer.AddInParameter(command11, "SampleID", DbType.Int64, evo2.SampleID);
                                this.dbServer.AddInParameter(command11, "Frequency", DbType.String, evo2.Frequency);
                                this.dbServer.AddInParameter(command11, "Quantity", DbType.Double, evo2.Quantity);
                                this.dbServer.AddInParameter(command11, "Status", DbType.Double, true);
                                this.dbServer.AddInParameter(command11, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command11, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command11, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command11, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command11, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                this.dbServer.AddInParameter(command11, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command11, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command11, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                this.dbServer.AddInParameter(command11, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddInParameter(command11, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddParameter(command11, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo2.ID);
                                this.dbServer.AddOutParameter(command11, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command11);
                            }
                        }
                        else
                        {
                            DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_DeletePathologyParaSubTest");
                            this.dbServer.AddInParameter(command3, "TestID", DbType.Int64, nvo.TestDetails.ID);
                            this.dbServer.AddInParameter(command3, "IsfromSubTest", DbType.Boolean, false);
                            this.dbServer.ExecuteNonQuery(command3);
                            DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_DeletePathologySampleSubTest");
                            this.dbServer.AddInParameter(command4, "TestID", DbType.Int64, nvo.TestDetails.ID);
                            this.dbServer.AddInParameter(command4, "IsfromSubTest", DbType.Boolean, false);
                            this.dbServer.ExecuteNonQuery(command4);
                            foreach (clsPathoTestParameterVO rvo2 in testDetails.TestParameterList)
                            {
                                DbCommand command5 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoTestParameter");
                                this.dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command5, "PathoTestID", DbType.Int64, nvo.TestDetails.ID);
                                this.dbServer.AddInParameter(command5, "ParamSTID", DbType.Int64, rvo2.ParamSTID);
                                this.dbServer.AddInParameter(command5, "IsParameter", DbType.Boolean, true);
                                this.dbServer.AddInParameter(command5, "PrintNameType", DbType.Int64, 0);
                                this.dbServer.AddInParameter(command5, "PrintName", DbType.String, null);
                                this.dbServer.AddInParameter(command5, "PrintPosition", DbType.Int64, rvo2.PrintPosition);
                                this.dbServer.AddInParameter(command5, "Status", DbType.Boolean, true);
                                this.dbServer.AddInParameter(command5, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command5, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command5, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command5, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command5, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                this.dbServer.AddInParameter(command5, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command5, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command5, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                this.dbServer.AddInParameter(command5, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddInParameter(command5, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddParameter(command5, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo2.ID);
                                this.dbServer.AddOutParameter(command5, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command5);
                            }
                            foreach (clsPathoSubTestVO tvo in testDetails.SubTestList)
                            {
                                DbCommand command6 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoTestParameter");
                                this.dbServer.AddInParameter(command6, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command6, "PathoTestID", DbType.Int64, nvo.TestDetails.ID);
                                this.dbServer.AddInParameter(command6, "ParamSTID", DbType.Int64, tvo.ParamSTID);
                                this.dbServer.AddInParameter(command6, "IsParameter", DbType.Boolean, false);
                                this.dbServer.AddInParameter(command6, "PrintNameType", DbType.Int64, 0);
                                this.dbServer.AddInParameter(command6, "PrintName", DbType.String, null);
                                this.dbServer.AddInParameter(command6, "PrintPosition", DbType.Int64, tvo.PrintPosition);
                                this.dbServer.AddInParameter(command6, "Status", DbType.Boolean, true);
                                this.dbServer.AddInParameter(command6, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command6, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command6, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command6, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command6, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                this.dbServer.AddInParameter(command6, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command6, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command6, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                this.dbServer.AddInParameter(command6, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddInParameter(command6, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddParameter(command6, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                                this.dbServer.AddOutParameter(command6, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command6);
                            }
                            foreach (clsPathoTestSampleVO evo in testDetails.TestSampleList)
                            {
                                DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoTestSample");
                                this.dbServer.AddInParameter(command7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command7, "PathoTestID", DbType.Int64, nvo.TestDetails.ID);
                                this.dbServer.AddInParameter(command7, "SampleID", DbType.Int64, evo.SampleID);
                                this.dbServer.AddInParameter(command7, "Frequency", DbType.String, evo.Frequency);
                                this.dbServer.AddInParameter(command7, "Quantity", DbType.Double, evo.Quantity);
                                this.dbServer.AddInParameter(command7, "Status", DbType.Double, true);
                                this.dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command7, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                this.dbServer.AddInParameter(command7, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command7, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command7, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                this.dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddInParameter(command7, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.ID);
                                this.dbServer.AddOutParameter(command7, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.ExecuteNonQuery(command7);
                            }
                        }
                    }
                    foreach (clsPathoTestItemDetailsVO svo in testDetails.TestItemList)
                    {
                        DbCommand command12 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePathoTestItemDetails");
                        this.dbServer.AddInParameter(command12, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command12, "TestID", DbType.Int64, nvo.TestDetails.ID);
                        this.dbServer.AddInParameter(command12, "ItemID", DbType.Int64, svo.ItemID);
                        this.dbServer.AddInParameter(command12, "Quantity", DbType.Double, svo.Quantity);
                        this.dbServer.AddInParameter(command12, "Status", DbType.Int64, true);
                        this.dbServer.AddInParameter(command12, "UID", DbType.Int64, svo.SelectedUID.ID);
                        this.dbServer.AddInParameter(command12, "UName", DbType.String, svo.SelectedUID.Description);
                        this.dbServer.AddInParameter(command12, "DID", DbType.Int64, svo.SelectedDID.ID);
                        this.dbServer.AddInParameter(command12, "DName", DbType.String, svo.SelectedDID.Description);
                        this.dbServer.AddInParameter(command12, "UOMid", DbType.Int64, svo.SelectedUOM.ID);
                        this.dbServer.AddInParameter(command12, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command12, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command12, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command12, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command12, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command12, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command12, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command12, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        this.dbServer.AddInParameter(command12, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command12, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddParameter(command12, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ID);
                        this.dbServer.AddOutParameter(command12, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command12);
                    }
                    if (!nvo.TestDetails.IsFromParameter)
                    {
                        DbCommand command13 = this.dbServer.GetStoredProcCommand("CIMS_DeletePathologyParaSubTest");
                        this.dbServer.AddInParameter(command13, "TestID", DbType.Int64, nvo.TestDetails.ID);
                        this.dbServer.AddInParameter(command13, "IsfromSubTest", DbType.Boolean, false);
                        this.dbServer.ExecuteNonQuery(command13);
                        if (!nvo.IsUpdate)
                        {
                            foreach (clsPathoTemplateVO evo4 in testDetails.TestTemplateList)
                            {
                                DbCommand command16 = this.dbServer.GetStoredProcCommand("CIMS_AddPathoTestTemplateDetailMaster");
                                this.dbServer.AddInParameter(command16, "TestID", DbType.Int64, nvo.TestDetails.ID);
                                this.dbServer.AddInParameter(command16, "TemplateID", DbType.Int64, evo4.TemplateID);
                                this.dbServer.AddInParameter(command16, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command16, "Status", DbType.Boolean, evo4.Status);
                                this.dbServer.AddInParameter(command16, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command16, "AddedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(command16, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command16, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command16, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddParameter(command16, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo4.ID);
                                this.dbServer.ExecuteNonQuery(command16);
                                evo4.ID = (long) this.dbServer.GetParameterValue(command16, "ID");
                            }
                        }
                        else
                        {
                            if ((testDetails.TestTemplateList != null) && (testDetails.TestTemplateList.Count > 0))
                            {
                                DbCommand command14 = this.dbServer.GetStoredProcCommand("CIMS_DeletePathologyTemplate");
                                this.dbServer.AddInParameter(command14, "TestID", DbType.Int64, nvo.TestDetails.ID);
                                this.dbServer.ExecuteNonQuery(command14);
                            }
                            foreach (clsPathoTemplateVO evo3 in testDetails.TestTemplateList)
                            {
                                DbCommand command15 = this.dbServer.GetStoredProcCommand("CIMS_AddPathoTestTemplateDetailMaster");
                                this.dbServer.AddInParameter(command15, "TestID", DbType.Int64, nvo.TestDetails.ID);
                                this.dbServer.AddInParameter(command15, "TemplateID", DbType.Int64, evo3.TemplateID);
                                this.dbServer.AddInParameter(command15, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command15, "Status", DbType.Boolean, evo3.Status);
                                this.dbServer.AddInParameter(command15, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command15, "AddedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(command15, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command15, "AddedDateTime", DbType.DateTime, DateTime.Now);
                                this.dbServer.AddInParameter(command15, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddParameter(command15, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo3.ID);
                                this.dbServer.ExecuteNonQuery(command15);
                                evo3.ID = (long) this.dbServer.GetParameterValue(command15, "ID");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddPathoTestResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsPathoTestResultEntryBizActionVO nvo = valueObject as clsPathoTestResultEntryBizActionVO;
            try
            {
                clsPathoResultEntryVO pathoResultEntry = nvo.PathoResultEntry;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPathoTestResutEntry");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, pathoResultEntry.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, pathoResultEntry.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, pathoResultEntry.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, pathoResultEntry.CategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, pathoResultEntry.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "ParameterID", DbType.Int64, pathoResultEntry.ParameterID);
                this.dbServer.AddInParameter(storedProcCommand, "LabID", DbType.Int64, pathoResultEntry.LabID);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, pathoResultEntry.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, pathoResultEntry.Time);
                this.dbServer.AddInParameter(storedProcCommand, "ResultValue", DbType.String, pathoResultEntry.ResultValue);
                this.dbServer.AddInParameter(storedProcCommand, "ResultType", DbType.Int64, pathoResultEntry.ResultType);
                this.dbServer.AddInParameter(storedProcCommand, "Note", DbType.String, pathoResultEntry.Note);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, pathoResultEntry.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AttachmentName", DbType.String, pathoResultEntry.AttachmentFileName);
                this.dbServer.AddInParameter(storedProcCommand, "Attachment", DbType.Binary, pathoResultEntry.Attachment);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, pathoResultEntry.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, pathoResultEntry.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, pathoResultEntry.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, pathoResultEntry.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, pathoResultEntry.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, pathoResultEntry.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddInParameter(storedProcCommand, "ParameterUnitId", DbType.Int64, pathoResultEntry.ParameterUnitID);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, pathoResultEntry.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddPathPatientReport(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPathPatientReportBizActionVO bizAction = valueObject as clsAddPathPatientReportBizActionVO;
            bizAction = (bizAction.OrderPathPatientReportList.ID != 0L) ? this.UpdateResult(bizAction, UserVo) : this.AddResult(bizAction, UserVo);
            return valueObject;
        }

        public override IValueObject AddPathPatientReportToGetEmail(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection connection = null;
            DbTransaction transaction = null;
            clsAddPathPatientReportDetailsForEmailSendingBizActionVO nvo = valueObject as clsAddPathPatientReportDetailsForEmailSendingBizActionVO;
            clsPathPatientReportVO orderPathPatientReportList = nvo.OrderPathPatientReportList;
            if (nvo.PathOrderBookList.Count > 0)
            {
                try
                {
                    connection = this.dbServer.CreateConnection();
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    List<clsPathOrderBookingDetailVO> pathOrderBookList = nvo.PathOrderBookList;
                    int count = pathOrderBookList.Count;
                    DbCommand storedProcCommand = null;
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 >= count)
                        {
                            transaction.Commit();
                            connection.Close();
                            break;
                        }
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPathEmailReportDeliveryDetails");
                        storedProcCommand.Connection = connection;
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, pathOrderBookList[num2].UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, pathOrderBookList[num2].OrderBookingID);
                        this.dbServer.AddInParameter(storedProcCommand, "PathOrderBookingDetailID", DbType.Int64, pathOrderBookList[num2].ID);
                        this.dbServer.AddInParameter(storedProcCommand, "PathPatientReportID", DbType.Int64, pathOrderBookList[num2].PathPatientReportID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientEmailID", DbType.String, nvo.PatientEmailID);
                        this.dbServer.AddInParameter(storedProcCommand, "DoctorEmailID", DbType.String, nvo.DoctorEmailID);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Int64, false);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                        this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0);
                        this.dbServer.AddParameter(storedProcCommand, "AttachmentID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.AttachmentID);
                        this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                        nvo.AttachmentID = (long) this.dbServer.GetParameterValue(storedProcCommand, "AttachmentID");
                        num2++;
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    nvo.PathOrderBookingDetailList = null;
                    nvo.OrderPathPatientReportList = null;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection = null;
                    transaction = null;
                }
            }
            return nvo;
        }

        private clsAddPathPatientReportBizActionVO AddResult(clsAddPathPatientReportBizActionVO BizAction, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsPathOrderBookingDetailVO lvo1 = new clsPathOrderBookingDetailVO();
                BizAction.MasterList = new List<clsPathOrderBookingDetailVO>();
                foreach (clsPathPatientReportVO tvo in BizAction.OrderList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPathPatientReport");
                    this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, tvo.OrderID);
                    this.dbServer.AddInParameter(storedProcCommand, "PathOrderBookingDetailID", DbType.Int64, tvo.PathOrderBookingDetailID);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, tvo.SampleNo);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleCollectionTime", DbType.DateTime, tvo.SampleCollectionTime);
                    this.dbServer.AddInParameter(storedProcCommand, "PathologistID1", DbType.Int64, tvo.PathologistID1);
                    this.dbServer.AddInParameter(storedProcCommand, "PathologistID2", DbType.Int64, tvo.PathologistID2);
                    this.dbServer.AddInParameter(storedProcCommand, "PathologistID3", DbType.Int64, tvo.PathologistID3);
                    this.dbServer.AddInParameter(storedProcCommand, "RefDoctorID", DbType.Int64, tvo.RefDoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, tvo.ReferredBy);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFirstLevel", DbType.Boolean, tvo.IsFirstLevel);
                    this.dbServer.AddInParameter(storedProcCommand, "IsSecondLevel", DbType.Boolean, tvo.IsSecondLevel);
                    this.dbServer.AddInParameter(storedProcCommand, "IsThirdLevel", DbType.Boolean, tvo.IsThirdLevel);
                    this.dbServer.AddInParameter(storedProcCommand, "IsAutoApproved", DbType.Int64, tvo.IsAutoApproved);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFinalized ", DbType.Boolean, tvo.IsFinalized);
                    this.dbServer.AddInParameter(storedProcCommand, "ResultAddedDate ", DbType.DateTime, tvo.ResultAddedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, tvo.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.Name);
                    this.dbServer.AddInParameter(storedProcCommand, "IsDoctorAuthorization ", DbType.Boolean, tvo.IsDoctorAuthorization);
                    this.dbServer.AddInParameter(storedProcCommand, "DocAuthorizationID ", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "ApproveBy", DbType.String, tvo.ApproveBy);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    tvo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    if ((BizAction.TestList != null) && (BizAction.TestList.Count > 0))
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_DeletePathPatientParameterDetails");
                        this.dbServer.AddInParameter(command, "OrderID", DbType.Int64, tvo.OrderID);
                        this.dbServer.AddInParameter(command, "PathPatientReportID", DbType.Int64, tvo.ID);
                        this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        foreach (clsPathoTestParameterVO rvo in BizAction.TestList)
                        {
                            if ((tvo.TestID == rvo.PathoTestID) && (tvo.SampleNo == rvo.SampleNo))
                            {
                                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddPathPatientParameterDetails");
                                this.dbServer.AddInParameter(command3, "OrderID", DbType.Int64, tvo.OrderID);
                                this.dbServer.AddInParameter(command3, "PathPatientReportID", DbType.Int64, tvo.ID);
                                this.dbServer.AddInParameter(command3, "IsNumeric", DbType.Int64, rvo.IsNumeric);
                                this.dbServer.AddInParameter(command3, "TestID", DbType.Int64, rvo.PathoTestID);
                                this.dbServer.AddInParameter(command3, "ParameterID", DbType.Int64, rvo.ParameterID);
                                this.dbServer.AddInParameter(command3, "CategoryID", DbType.Int64, rvo.CategoryID);
                                this.dbServer.AddInParameter(command3, "Category", DbType.String, rvo.Category);
                                this.dbServer.AddInParameter(command3, "SubTestID", DbType.Int64, rvo.PathoSubTestID);
                                this.dbServer.AddInParameter(command3, "ParameterName", DbType.String, rvo.ParameterName);
                                this.dbServer.AddInParameter(command3, "ParameterUnit", DbType.String, rvo.ParameterUnit);
                                this.dbServer.AddInParameter(command3, "ParameterPrintName", DbType.String, rvo.Print);
                                this.dbServer.AddInParameter(command3, "ResultValue", DbType.String, rvo.ResultValue);
                                this.dbServer.AddInParameter(command3, "DefaultValue", DbType.String, rvo.DefaultValue);
                                this.dbServer.AddInParameter(command3, "NormalRange", DbType.String, rvo.NormalRange);
                                this.dbServer.AddInParameter(command3, "HelpValue", DbType.String, rvo.HelpValue);
                                if (rvo.Note != string.Empty)
                                {
                                    this.dbServer.AddInParameter(command3, "SuggetionNote", DbType.String, rvo.Note);
                                }
                                else
                                {
                                    this.dbServer.AddInParameter(command3, "SuggetionNote", DbType.String, null);
                                }
                                if (rvo.FootNote != string.Empty)
                                {
                                    this.dbServer.AddInParameter(command3, "FootNote", DbType.String, rvo.FootNote);
                                }
                                else
                                {
                                    this.dbServer.AddInParameter(command3, "FootNote", DbType.String, null);
                                }
                                this.dbServer.AddInParameter(command3, "IsFirstLevel", DbType.String, rvo.IsFirstLevel);
                                this.dbServer.AddInParameter(command3, "IsSecondLevel", DbType.String, rvo.IsSecondLevel);
                                this.dbServer.AddInParameter(command3, "SubTest", DbType.String, rvo.PathoSubTestName);
                                this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                                this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddInParameter(command3, "ReferenceRange", DbType.String, rvo.ReferenceRange);
                                this.dbServer.AddInParameter(command3, "DeltaCheck", DbType.Double, rvo.DeltaCheckValue);
                                this.dbServer.AddInParameter(command3, "ParameterDefaultValueId", DbType.String, rvo.ParameterDefaultValueId);
                                this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo.ID);
                                this.dbServer.ExecuteNonQuery(command3, transaction);
                                rvo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                            }
                        }
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_CheckPathOrderBookingStatus");
                        this.dbServer.AddInParameter(command4, "OrderID", DbType.Int64, tvo.OrderID);
                        this.dbServer.AddInParameter(command4, "TestID", DbType.Int64, tvo.TestID);
                        this.dbServer.AddInParameter(command4, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddParameter(command4, "IsBalence", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.IsBalence);
                        this.dbServer.AddParameter(command4, "IsAbnormal", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.IsAbnormal);
                        this.dbServer.AddParameter(command4, "SampleNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????");
                        this.dbServer.ExecuteNonQuery(command4, transaction);
                        tvo.IsBalence = Convert.ToInt16(this.dbServer.GetParameterValue(command4, "IsBalence"));
                        tvo.IsAbnormal = Convert.ToInt16(this.dbServer.GetParameterValue(command4, "IsAbnormal"));
                        this.dbServer.GetParameterValue(command4, "SampleNo").ToString();
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            TestID = tvo.TestID,
                            SampleNo = Convert.ToString(this.dbServer.GetParameterValue(command4, "SampleNo")),
                            Status = tvo.ResultStatus
                        };
                        BizAction.MasterList.Add(item);
                    }
                }
                if ((BizAction.OrderPathPatientReportList.TestID > 0L) && BizAction.OrderPathPatientReportList.ISTEMplate)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPathPatientReport");
                    this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, BizAction.OrderPathPatientReportList.OrderID);
                    this.dbServer.AddInParameter(storedProcCommand, "PathOrderBookingDetailID", DbType.Int64, BizAction.OrderPathPatientReportList.PathOrderBookingDetailID);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, BizAction.OrderPathPatientReportList.SampleNo);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleCollectionTime", DbType.DateTime, BizAction.OrderPathPatientReportList.SampleCollectionTime);
                    this.dbServer.AddInParameter(storedProcCommand, "PathologistID1", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID1);
                    this.dbServer.AddInParameter(storedProcCommand, "PathologistID2", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID2);
                    this.dbServer.AddInParameter(storedProcCommand, "PathologistID3", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID3);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, BizAction.OrderPathPatientReportList.ReferredBy);
                    this.dbServer.AddInParameter(storedProcCommand, "RefDoctorID", DbType.Int64, BizAction.OrderPathPatientReportList.RefDoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFinalized ", DbType.Boolean, BizAction.OrderPathPatientReportList.IsFinalized);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFirstLevel", DbType.Boolean, BizAction.OrderPathPatientReportList.IsFirstLevel);
                    this.dbServer.AddInParameter(storedProcCommand, "IsSecondLevel", DbType.Boolean, BizAction.OrderPathPatientReportList.IsSecondLevel);
                    this.dbServer.AddInParameter(storedProcCommand, "IsThirdLevel", DbType.Boolean, BizAction.OrderPathPatientReportList.IsThirdLevel);
                    this.dbServer.AddInParameter(storedProcCommand, "ResultAddedDate ", DbType.DateTime, BizAction.OrderPathPatientReportList.ResultAddedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, BizAction.OrderPathPatientReportList.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.Name);
                    this.dbServer.AddInParameter(storedProcCommand, "DocAuthorizationID ", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsDoctorAuthorization ", DbType.Boolean, BizAction.OrderPathPatientReportList.IsDoctorAuthorization);
                    this.dbServer.AddInParameter(storedProcCommand, "ApproveBy", DbType.String, BizAction.OrderPathPatientReportList.ApproveByTemplate);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.OrderPathPatientReportList.ID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    BizAction.OrderPathPatientReportList.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_DeletePathPatientTemplateDetails");
                    this.dbServer.AddInParameter(command, "OrderID", DbType.Int64, BizAction.OrderPathPatientReportList.OrderID);
                    this.dbServer.AddInParameter(command, "PathPatientReportID", DbType.Int64, BizAction.OrderPathPatientReportList.ID);
                    this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.ExecuteNonQuery(command);
                    DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_AddPathoResultEntryTemplate");
                    this.dbServer.AddInParameter(command7, "OrderID", DbType.Int64, BizAction.OrderPathPatientReportList.OrderID);
                    this.dbServer.AddInParameter(command7, "OrderDetailID", DbType.Int64, BizAction.OrderPathPatientReportList.PathOrderBookingDetailID);
                    this.dbServer.AddInParameter(command7, "PathPatientReportID", DbType.Int64, BizAction.OrderPathPatientReportList.ID);
                    this.dbServer.AddInParameter(command7, "TestID", DbType.Int64, BizAction.OrderPathPatientReportList.TestID);
                    this.dbServer.AddInParameter(command7, "Pathologist", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID1);
                    this.dbServer.AddInParameter(command7, "Template", DbType.String, BizAction.OrderPathPatientReportList.TemplateDetails.Template);
                    this.dbServer.AddInParameter(command7, "TemplateID", DbType.Int64, BizAction.OrderPathPatientReportList.TemplateDetails.TemplateID);
                    this.dbServer.AddInParameter(command7, "Status", DbType.Boolean, true);
                    if (BizAction.UnitID > 0L)
                    {
                        this.dbServer.AddInParameter(command7, "UnitId", DbType.Int64, BizAction.UnitID);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(command7, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }
                    this.dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.OrderPathPatientReportList.TemplateDetails.ID);
                    this.dbServer.ExecuteNonQuery(command7, transaction);
                    BizAction.OrderPathPatientReportList.TemplateDetails.ID = (long) this.dbServer.GetParameterValue(command7, "ID");
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                BizAction.OrderList = null;
                BizAction.OrderPathPatientReportList = null;
                BizAction.TestList = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return BizAction;
        }

        private clsAddPathoTemplateMasterBizActionVO AddTemplateDetails(clsAddPathoTemplateMasterBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsPathoTestTemplateDetailsVO templateDetails = BizActionObj.TemplateDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPathoTemplateMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, templateDetails.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, templateDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "MultiplePathoDoctor", DbType.Boolean, templateDetails.MultiplePathoDoctor);
                this.dbServer.AddInParameter(storedProcCommand, "Template", DbType.String, templateDetails.Template);
                this.dbServer.AddInParameter(storedProcCommand, "Pathologist", DbType.Int64, templateDetails.Pathologist);
                this.dbServer.AddInParameter(storedProcCommand, "PathologistName", DbType.String, templateDetails.PathologistName);
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, templateDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, templateDetails.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.TemplateDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                BizActionObj.SuccessStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                List<MasterListItem> genderList = BizActionObj.GenderList;
                if ((genderList != null) || (genderList.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteGenderToTemplate");
                    this.dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, BizActionObj.TemplateDetails.ID);
                    this.dbServer.ExecuteNonQuery(command2);
                    foreach (MasterListItem item in genderList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddPathoGenderToTemplate");
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "GenderID", DbType.Int64, item.ID);
                        this.dbServer.AddInParameter(command3, "TemplateID", DbType.Int64, BizActionObj.TemplateDetails.ID);
                        this.dbServer.AddInParameter(command3, "TemplateUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, item.Status);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, 0x7fffffff);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        this.dbServer.ExecuteNonQuery(command3);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddUpdateParameterLinking(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdateParameterLinkingBizActionVO nvo = valueObject as clsAddUpdateParameterLinkingBizActionVO;
            try
            {
                clsParameterLinkingVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateParameterLinking");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, details.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "MachineParameterID", DbType.String, details.MachineParameterID);
                this.dbServer.AddInParameter(storedProcCommand, "MachineID", DbType.String, details.MachineID);
                this.dbServer.AddInParameter(storedProcCommand, "ParameterID", DbType.Int64, details.ParameterID);
                this.dbServer.AddInParameter(storedProcCommand, "Freezed", DbType.Boolean, details.Freezed);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, details.CreatedUnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, details.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, details.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, details.AddedWindowsLoginName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdatePathoMachineParameter(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddUpdatePathoMachineParameterBizActionVO nvo = valueObject as clsAddUpdatePathoMachineParameterBizActionVO;
            try
            {
                clsMachineParameterMasterVO details = nvo.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateMachineParameter");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, details.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, details.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, details.ParameterDesc);
                this.dbServer.AddInParameter(storedProcCommand, "MachineID", DbType.Int64, details.MachineId);
                this.dbServer.AddInParameter(storedProcCommand, "Freezed", DbType.Boolean, details.Freezed);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject ApprovePathPatientReport(IValueObject valueObject, clsUserVO UserVo)
        {
            clsApprovePathPatientReortBizActionVO nvo = valueObject as clsApprovePathPatientReortBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateApprovalStatusForTests");
                this.dbServer.AddInParameter(storedProcCommand, "OrderDetailsID", DbType.String, nvo.OrderDetailsID);
                this.dbServer.AddInParameter(storedProcCommand, "DoctorUserID", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "IsSecondLevelApproval", DbType.Boolean, nvo.IsSecondLevelApproval);
                this.dbServer.AddInParameter(storedProcCommand, "IsThirdLevelApproval", DbType.Boolean, nvo.IsThirdLevelApproval);
                this.dbServer.AddInParameter(storedProcCommand, "IsCheckedResults", DbType.Boolean, nvo.IsForCheckResults);
                this.dbServer.AddInParameter(storedProcCommand, "ThirdLevelCheckResult", DbType.Boolean, nvo.IsThirdLevelCheckResult);
                this.dbServer.AddInParameter(storedProcCommand, "CheckResultValueMessage", DbType.String, nvo.checkResultValueMessage);
                this.dbServer.AddInParameter(storedProcCommand, "IsApprove", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "IsDigitalSignatureRequired", DbType.Boolean, nvo.IsDigitalSignatureRequired);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.UserName);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                if ((nvo.TestList != null) && (nvo.TestList.Count > 0))
                {
                    foreach (clsPathoTestParameterVO rvo in nvo.TestList.ToList<clsPathoTestParameterVO>())
                    {
                        DbCommand command2 = null;
                        command2 = this.dbServer.GetStoredProcCommand("CIMS_UpdateNotes");
                        this.dbServer.AddInParameter(command2, "OrderID", DbType.Int64, rvo.OrderID);
                        this.dbServer.AddInParameter(command2, "TestID", DbType.Int64, rvo.PathoTestID);
                        this.dbServer.AddInParameter(command2, "FootNote", DbType.String, rvo.FootNote);
                        this.dbServer.AddInParameter(command2, "SuggestionNote", DbType.String, rvo.Note);
                        this.dbServer.ExecuteNonQuery(command2);
                    }
                }
            }
            catch
            {
            }
            return nvo;
        }

        public override IValueObject ChangePathoTemplateStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsAddPathoTemplateMasterBizActionVO nvo = valueObject as clsAddPathoTemplateMasterBizActionVO;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateStatusPathoTemplateMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.TemplateDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.IsStatusChanged);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject ChangePathoTestAgency(IValueObject valueObject, clsUserVO UserVO)
        {
            clsChangePathoTestAgencyBizActionVO nvo = valueObject as clsChangePathoTestAgencyBizActionVO;
            if (nvo.IsOutsource)
            {
                DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_PathoTestOutSourced");
                try
                {
                    this.dbServer.AddInParameter(command, "OrderDetailID", DbType.Int64, nvo.OutSourceID);
                    this.dbServer.AddInParameter(command, "TestId", DbType.Int64, nvo.PathoOutSourceTestDetails.TestID);
                    this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVO.ID);
                    this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.ExecuteNonQuery(command);
                }
                catch (Exception)
                {
                    if (command != null)
                    {
                        command.Dispose();
                        command.Connection.Close();
                    }
                }
                finally
                {
                    if (command != null)
                    {
                        command.Dispose();
                        command.Connection.Close();
                    }
                }
            }
            if (nvo.PathoOutSourceTestDetails.IsForUnassignedAgencyTest)
            {
                foreach (clsPathoTestOutSourceDetailsVO svo in nvo.AssignedAgnecyTestList)
                {
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_AssignPathoTestAgency");
                    try
                    {
                        this.dbServer.AddInParameter(command, "OrderDetailID", DbType.Int64, svo.OrderDetailID);
                        this.dbServer.AddInParameter(command, "AgencyID", DbType.Int64, svo.ChangedAgencyID);
                        this.dbServer.AddInParameter(command, "ChangeReason", DbType.String, svo.ReasonToChangeAgency);
                        this.dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVO.ID);
                        this.dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                        this.dbServer.ExecuteNonQuery(command);
                    }
                    catch (Exception)
                    {
                        if (command != null)
                        {
                            command.Dispose();
                            command.Connection.Close();
                        }
                    }
                    finally
                    {
                        if (command != null)
                        {
                            command.Dispose();
                            command.Connection.Close();
                        }
                    }
                }
                return valueObject;
            }
            DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ModifyPathoTestAgency");
            try
            {
                clsPathoTestOutSourceDetailsVO pathoOutSourceTestDetails = nvo.PathoOutSourceTestDetails;
                this.dbServer.AddInParameter(storedProcCommand, "OrderDetailID", DbType.Int64, pathoOutSourceTestDetails.OrderDetailID);
                this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, pathoOutSourceTestDetails.ChangedAgencyID);
                this.dbServer.AddInParameter(storedProcCommand, "ChangeReason", DbType.String, pathoOutSourceTestDetails.ReasonToChangeAgency);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (storedProcCommand != null)
                {
                    storedProcCommand.Dispose();
                    storedProcCommand.Connection.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject DeletePathoTestResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeletePathoTestResultEntryBizActionVO nvo = valueObject as clsDeletePathoTestResultEntryBizActionVO;
            try
            {
                clsPathoResultEntryVO pathoResultEntry = nvo.PathoResultEntry;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeletePathoTestResutEntry");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, pathoResultEntry.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject FillPathoProfileService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoProfileServicesBizActionVO nvo = valueObject as clsGetPathoProfileServicesBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoProfileServices");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Status = true
                        };
                        nvo.ServiceList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject FillPathoProfileTestByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoProfileTestByIDBizActionVO nvo = valueObject as clsGetPathoProfileTestByIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoProfileTestByID");
                this.dbServer.AddInParameter(storedProcCommand, "ProfileID", DbType.Int64, nvo.ProfileID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.TestList == null)
                    {
                        nvo.TestList = new List<clsPathoProfileTestDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoProfileTestDetailsVO item = new clsPathoProfileTestDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            ProfileID = (long) DALHelper.HandleDBNull(reader["ProfileID"]),
                            TestID = (long) DALHelper.HandleDBNull(reader["TestID"]),
                            Status = false,
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            IsEntryFromTestMaster = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsEntryFromTestMaster"]))
                        };
                        nvo.TestList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject FillTemplateComboBoxInPathoResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            FillTemplateComboBoxInPathoResultEntryBizActionVO nvo = (FillTemplateComboBoxInPathoResultEntryBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_FillTemplateComboBoxInPathoResultEntry");
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, nvo.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "Pathologist", DbType.Int64, nvo.Pathologist);
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, nvo.GenderID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        nvo.MasterList.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString(), Convert.ToDouble((int) reader["IsFormTemplate"])));
                        nvo.IsFormTemplate = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsFormTemplate"]));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAgencyApplicableUnitList(IValueObject valueObject, clsUserVO UserVo)
        {
            IValueObject obj2;
            try
            {
                clsGetPathoAgencyApplicableUnitListBizActionVO nvo = valueObject as clsGetPathoAgencyApplicableUnitListBizActionVO;
                nvo.ServiceAgencyMasterDetails = new List<clsServiceAgencyMasterVO>();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServiceApplicableUnitDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceId);
                this.dbServer.AddInParameter(storedProcCommand, "ApplicableUnitID", DbType.Int64, nvo.ApplicableUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.Close();
                        obj2 = nvo;
                        break;
                    }
                    clsServiceAgencyMasterVO item = new clsServiceAgencyMasterVO {
                        ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                        UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                        ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                        AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"])),
                        ApplicableUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableUnitID"])),
                        AgencyTestName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyTestName"])),
                        Rate = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                        AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"])),
                        AgencyCode = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyCode"])),
                        UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"])),
                        IsDefaultAgency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsDefaultAgency"])),
                        Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                    };
                    nvo.ServiceAgencyMasterDetails.Add(item);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return obj2;
        }

        public override IValueObject GetAssignedAgency(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAssignedAgencyBizActionVO nvo = valueObject as clsGetAssignedAgencyBizActionVO;
            nvo.AgencyList = new List<clsGetAssignedAgencyBizActionVO>();
            try
            {
                this.dbServer.CreateConnection().Open();
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAgencyIForService");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                clsGetAssignedAgencyBizActionVO nvo1 = new clsGetAssignedAgencyBizActionVO();
                while (true)
                {
                    if (!reader.Read())
                    {
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDefaultAgencyIForService");
                        this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                        DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                        while (reader2.Read())
                        {
                            nvo.DefaultAgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["DefaultAgencyId"]));
                            nvo.DefaultAgencyName = Convert.ToString(DALHelper.HandleDBNull(reader2["AgencyName"]));
                        }
                        break;
                    }
                    clsGetAssignedAgencyBizActionVO item = new clsGetAssignedAgencyBizActionVO {
                        AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]))
                    };
                    nvo.AgencyList.Add(item);
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetBatchCode(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetBatchCodeBizActionVO nvo = valueObject as clsGetBatchCodeBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBatchCodeDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, nvo.OrderID);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                if ((nvo.BatchCode != null) && (nvo.BatchCode.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BatchNo", DbType.String, nvo.BatchCode + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BatchNo", DbType.String, null);
                }
                long dispatchTo = nvo.DispatchTo;
                if (nvo.DispatchTo != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DispatchTo", DbType.Int64, nvo.DispatchTo);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "DispatchTo", DbType.Int64, 0);
                }
                if (nvo.IsFromAccept)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsFromAccept", DbType.Boolean, true);
                }
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]))
                        };
                        nvo.OrderBookingList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetBillingStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsPathoCheckBillingStatusVO svo = valueObject as clsPathoCheckBillingStatusVO;
            try
            {
                this.dbServer.CreateConnection().Open();
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBillingStatusForWalkin");
                this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, svo.OrderId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, svo.UnitId);
                this.dbServer.AddParameter(storedProcCommand, "ResultStatus", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, svo.ResultStatus);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                svo.ResultStatus = Convert.ToBoolean(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
            }
            return svo;
        }

        public override IValueObject GetDeltaCheckDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDeltaCheckValueBizActionVO nvo = valueObject as clsGetDeltaCheckValueBizActionVO;
            nvo.List = new List<clsGetDeltaCheckValueBizActionVO>();
            try
            {
                this.dbServer.CreateConnection().Open();
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDeltaCheckForParameter");
                this.dbServer.AddInParameter(storedProcCommand, "ParameterId", DbType.Int64, nvo.PathoTestParameter.ParameterID);
                this.dbServer.AddInParameter(storedProcCommand, "TestId", DbType.String, nvo.PathTestId.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PathPatientDetail.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PathPatientDetail.PatientUnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "mainId", DbType.Int64, nvo.PathPatientDetail.ID);
                this.dbServer.AddInParameter(storedProcCommand, "OrderDate", DbType.DateTime, nvo.PathPatientDetail.OrderDate);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsGetDeltaCheckValueBizActionVO item = new clsGetDeltaCheckValueBizActionVO {
                            ParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParameterID"])),
                            ResultValue = Convert.ToString(DALHelper.HandleDBNull(reader["ResultValue"]))
                        };
                        nvo.List.Add(item);
                    }
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetDispachReceiveDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetDispachReciveDetailListBizActionVO nvo = valueObject as clsGetDispachReciveDetailListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDispatchReceiveDetails");
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                if (nvo.IsSampleDispatched)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsSampleDispatched", DbType.String, nvo.IsSampleDispatched);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "IsSampleDispatched", DbType.String, 0);
                }
                if ((nvo.OrderDetail.BillNo != null) && (nvo.OrderDetail.BillNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, nvo.OrderDetail.BillNo + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, null);
                }
                if ((nvo.BatchNo != null) && (nvo.BatchNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BatchNo", DbType.String, nvo.BatchNo + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BatchNo", DbType.String, null);
                }
                if ((nvo.SampleNo != null) && (nvo.SampleNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.SampleNo + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, null);
                }
                if ((nvo.MRNo != null) && (nvo.MRNo.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, "%" + nvo.MRNo + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, null);
                }
                if ((nvo.OrderDetail.FirstName != null) && (nvo.OrderDetail.FirstName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, "%" + nvo.OrderDetail.FirstName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, null);
                }
                if ((nvo.OrderDetail.LastName != null) && (nvo.OrderDetail.LastName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, "%" + nvo.OrderDetail.LastName + "%");
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "IsPending", DbType.Int64, nvo.IsPending);
                this.dbServer.AddInParameter(storedProcCommand, "IsCollected", DbType.Int64, nvo.IsCollected);
                this.dbServer.AddInParameter(storedProcCommand, "DispatchUserID", DbType.Int64, nvo.IsDispatchByID);
                this.dbServer.AddInParameter(storedProcCommand, "SampleStatus", DbType.Int64, nvo.SampleStatus);
                this.dbServer.AddInParameter(storedProcCommand, "BillType", DbType.Int32, nvo.BillType);
                this.dbServer.AddInParameter(storedProcCommand, "SampleAcceptRejectBy", DbType.Int64, nvo.SampleAcceptRejectBy);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "DispatchTo", DbType.Int64, nvo.ClinicID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.OrderBookingDetailList == null)
                    {
                        nvo.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            SampleNumber = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            SampleDispatchDateTime = (DateTime?) DALHelper.HandleDBNull(reader["dispatchDate"]),
                            TestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"])),
                            TestCode = Convert.ToString(DALHelper.HandleDBNull(reader["TestCode"])),
                            ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                            TubeName = Convert.ToString(DALHelper.HandleDBNull(reader["TubeName"])),
                            ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"])),
                            AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"])),
                            Prefix = Convert.ToString(DALHelper.HandleDBNull(reader["Pre"])),
                            FirstName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"]))),
                            MiddleName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"]))),
                            LastName = Security.base64Decode(Convert.ToString(DALHelper.HandleDBNull(reader["LastName"]))),
                            IsSampleDispatched = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleDispatched"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            IsOutSourced = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOutsourced"])),
                            IsSampleCollected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleCollected"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"])),
                            IsAccepted = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAccepted"])),
                            IsRejected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRejected"])),
                            IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"])),
                            IsSampleReceive = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleReceived"])),
                            DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"])),
                            SampleReceivedDateTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleReceivedDateTime"]),
                            AgeInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInDays"])),
                            OrderDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["OrderDate"]))),
                            TestDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["TestDate"])))
                        };
                        nvo.OrderBookingList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetFinalizedParameter(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoFinalizedEntryBizActionVO nvo = valueObject as clsGetPathoFinalizedEntryBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPAthoResultEntryRushabh");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "DetailID", DbType.String, nvo.DetailID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ResultList == null)
                    {
                        nvo.ResultList = new List<clsPathPatientReportVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (nvo.ResultEntryDetails.TestList == null)
                            {
                                nvo.ResultEntryDetails.TestList = new List<clsPathoTestParameterVO>();
                            }
                            while (true)
                            {
                                if (!reader.Read())
                                {
                                    reader.NextResult();
                                    if (nvo.ResultEntryDetails.ItemList == null)
                                    {
                                        nvo.ResultEntryDetails.ItemList = new List<clsPathoTestItemDetailsVO>();
                                    }
                                    while (true)
                                    {
                                        if (!reader.Read())
                                        {
                                            reader.NextResult();
                                            if (nvo.ResultEntryDetails.TemplateDetails == null)
                                            {
                                                nvo.ResultEntryDetails.TemplateDetails = new clsPathoResultEntryTemplateVO();
                                            }
                                            while (true)
                                            {
                                                if (!reader.Read())
                                                {
                                                    reader.Close();
                                                    break;
                                                }
                                                nvo.ResultEntryDetails.TemplateDetails = new clsPathoResultEntryTemplateVO();
                                                nvo.ResultEntryDetails.TemplateDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                                                nvo.ResultEntryDetails.TemplateDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                                                nvo.ResultEntryDetails.TemplateDetails.OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                                                nvo.ResultEntryDetails.TemplateDetails.OrderDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderDetailID"]));
                                                nvo.ResultEntryDetails.TemplateDetails.PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"]));
                                                nvo.ResultEntryDetails.TemplateDetails.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                                                nvo.ResultEntryDetails.TemplateDetails.PathologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Pathologist"]));
                                                nvo.ResultEntryDetails.TemplateDetails.Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
                                                nvo.ResultEntryDetails.TemplateDetails.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                                                nvo.ResultEntryDetails.TemplateDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                                            }
                                            break;
                                        }
                                        clsPathoTestItemDetailsVO svo = new clsPathoTestItemDetailsVO {
                                            TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"])),
                                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                                            Quantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["IdealQuantity"])),
                                            ActualQantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["ActualQantity"])),
                                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                                            ExpiryDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ExpiryDate"]))),
                                            BalanceQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BalQuantity"]))
                                        };
                                        nvo.ResultEntryDetails.ItemList.Add(svo);
                                    }
                                    break;
                                }
                                clsPathoTestParameterVO rvo = new clsPathoTestParameterVO {
                                    PathoTestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"])),
                                    PathoSubTestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubTestID"])),
                                    ParameterName = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterName"])),
                                    ParameterCode = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterCode"])),
                                    ParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParameterID"])),
                                    Category = Convert.ToString(DALHelper.HandleDBNull(reader["Category"])),
                                    CategoryID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]))),
                                    ParameterUnit = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterUnit"])),
                                    Print = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterPrintName"])),
                                    ResultValue = Convert.ToString(DALHelper.HandleDBNull(reader["ResultValue"])),
                                    DefaultValue = Convert.ToString(DALHelper.HandleDBNull(reader["DefaultValue"])),
                                    NormalRange = Convert.ToString(DALHelper.HandleDBNull(reader["NormalRange"])),
                                    Note = Convert.ToString(DALHelper.HandleDBNull(reader["SuggetionNote"])),
                                    FootNote = Convert.ToString(DALHelper.HandleDBNull(reader["FootNote"])),
                                    PathoSubTestName = Convert.ToString(DALHelper.HandleDBNull(reader["SubTest"])),
                                    IsNumeric = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsNumeric"])),
                                    PathoTestName = Convert.ToString(DALHelper.HandleDBNull(reader["Test"])),
                                    TestCategory = Convert.ToString(DALHelper.HandleDBNull(reader["TestCategory"])),
                                    TestCategoryID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["TestCategoryID"]))),
                                    IsReflexTesting = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReflexTesting"])),
                                    MinImprobable = (double) DALHelper.HandleDBNull(reader["MinImprobable"]),
                                    MaxImprobable = (double) DALHelper.HandleDBNull(reader["MaxImprobable"]),
                                    DeltaCheckDefaultValue = (double) DALHelper.HandleDBNull(reader["DeltaCheck"]),
                                    HighReffValue = (double) DALHelper.HandleDBNull(reader["HighReffValue"]),
                                    LowReffValue = (double) DALHelper.HandleDBNull(reader["LowReffValue"]),
                                    UpperPanicValue = (double) DALHelper.HandleDBNull(reader["UpperPanicValue"]),
                                    LowerPanicValue = (double) DALHelper.HandleDBNull(reader["LowerPanicValue"]),
                                    LowReflex = (double) DALHelper.HandleDBNull(reader["LowReflex"]),
                                    HighReflex = (double) DALHelper.HandleDBNull(reader["HighReflex"]),
                                    ReferenceRange = (((double) DALHelper.HandleDBNull(reader["LowReffValue"])) + " - " + ((double) DALHelper.HandleDBNull(reader["HighReffValue"]))).ToString(),
                                    IsAbnormal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAbnormal"])),
                                    DeltaCheckValue = (double) DALHelper.HandleDBNull(reader["DeltaCheckValue"]),
                                    ParameterDefaultValueId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParameterDefaultValueId"])),
                                    VaryingReferences = Convert.ToString(DALHelper.HandleDBNull(reader["VaryingReferences"])),
                                    Formula = Convert.ToString(DALHelper.HandleDBNull(reader["Formula"])),
                                    FormulaID = Convert.ToString(DALHelper.HandleDBNull(reader["FormulaID"])),
                                    IsMachine = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsMachine"])),
                                    SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"])),
                                    TestAndSampleNO = Convert.ToString(DALHelper.HandleDBNull(reader["TestAndSampleNO"])),
                                    IsSecondLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSecondLevel"]))
                                };
                                nvo.ResultEntryDetails.TestList.Add(rvo);
                            }
                            break;
                        }
                        clsPathPatientReportVO item = new clsPathPatientReportVO {
                            OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"])),
                            PathOrderBookingDetailID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["PathOrderBookingDetailID"]))),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"])),
                            SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"])),
                            SampleCollectionTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleCollectionTime"]),
                            PathologistID1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologistID1"])),
                            ReferredBy = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredBy"])),
                            ResultAddedDateTime = DALHelper.HandleDate(reader["ResultAddedTime"]),
                            SampleReceiveDateTime = DALHelper.HandleDate(reader["SampleReceivedDateTime"]),
                            IsFinalized = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsFinalized"])),
                            BillID = Convert.ToInt64(DALHelper.HandleBoolDBNull(reader["BillId"])),
                            BillNo = Convert.ToString(DALHelper.HandleBoolDBNull(reader["BillNo"]))
                        };
                        nvo.ResultList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetHelpValuesFroResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetHelpValuesFroResultEntryBizActionVO nvo = valueObject as clsGetHelpValuesFroResultEntryBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetHelpValuesFroResultEntry");
                this.dbServer.AddInParameter(storedProcCommand, "ParameterID", DbType.Int64, nvo.ParameterID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.HelpValueList == null)
                    {
                        nvo.HelpValueList = new List<clsPathoTestParameterVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoTestParameterVO item = new clsPathoTestParameterVO {
                            ParameterID = (long) DALHelper.HandleDBNull(reader["ParameterID"]),
                            HelpValueID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            HelpValue = (string) DALHelper.HandleDBNull(reader["HelpValue"]),
                            HelpValueDefault = (bool) DALHelper.HandleDBNull(reader["IsDefault"]),
                            IsAbnormal = (bool) DALHelper.HandleDBNull(reader["IsAbnoramal"])
                        };
                        nvo.HelpValueList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetMachineParameterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoMachineParameterBizActionVO nvo = valueObject as clsGetPathoMachineParameterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMachineParameterList");
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "MachineID", DbType.Int64, nvo.MachineID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DetailsList == null)
                    {
                        nvo.DetailsList = new List<clsMachineParameterMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsMachineParameterMasterVO item = new clsMachineParameterMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            ParameterDesc = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            MachineId = Convert.ToInt64(DALHelper.HandleDBNull(reader["MachineID"])),
                            MachineName = Convert.ToString(DALHelper.HandleDBNull(reader["MachineMaster"])),
                            Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Freeze"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.DetailsList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetMachineToSubTestList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetMachineToSubTestBizActionVO nvo = valueObject as clsGetMachineToSubTestBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMachijneListForSubTest");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ItemSupplier.SubTestID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemSupplierList == null)
                    {
                        nvo.ItemSupplierList = new List<clsPathoTestMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoTestMasterVO item = new clsPathoTestMasterVO {
                            status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            MachineID = (long) DALHelper.HandleDBNull(reader["MachineID"]),
                            SubTestID = (long) DALHelper.HandleDBNull(reader["SubTestID"])
                        };
                        nvo.ItemSupplierList.Add(item);
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetMachineToTestList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetMachineToTestBizActionVO nvo = valueObject as clsGetMachineToTestBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMachijneListForTest");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ItemSupplier.TestID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemSupplierList == null)
                    {
                        nvo.ItemSupplierList = new List<clsPathoTestMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoTestMasterVO item = new clsPathoTestMasterVO {
                            status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            MachineID = (long) DALHelper.HandleDBNull(reader["MachineID"]),
                            TestID = (long) DALHelper.HandleDBNull(reader["TestID"])
                        };
                        nvo.ItemSupplierList.Add(item);
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetParameterLinkingList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetParameterLinkingBizActionVO nvo = valueObject as clsGetParameterLinkingBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetParameterLinkingList");
                this.dbServer.AddInParameter(storedProcCommand, "ParameterID", DbType.Int64, nvo.ParameterID);
                this.dbServer.AddInParameter(storedProcCommand, "MachineParameterID", DbType.Int64, nvo.MachineParaID);
                this.dbServer.AddInParameter(storedProcCommand, "MachineID", DbType.Int64, nvo.MachineID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DetailsList == null)
                    {
                        nvo.DetailsList = new List<clsParameterLinkingVO>();
                    }
                    while (reader.Read())
                    {
                        clsParameterLinkingVO item = new clsParameterLinkingVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            MachineParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MachineParameterId"])),
                            ParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParameterID"])),
                            MachineParameter = Convert.ToString(DALHelper.HandleDBNull(reader["MachineParaDescription"])),
                            ParameterName = Convert.ToString(DALHelper.HandleDBNull(reader["ParaDescription"])),
                            Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Freeze"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            MachineName = Convert.ToString(DALHelper.HandleDBNull(reader["MachineName"])),
                            MachineID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MachineID"]))
                        };
                        nvo.DetailsList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetParameterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetParaByParaAndMachineBizActionVO nvo = valueObject as clsGetParaByParaAndMachineBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetParameterListbyMachineId");
                this.dbServer.AddInParameter(storedProcCommand, "MachineParameterID", DbType.Int64, nvo.MachineParaID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.DetailsList == null)
                    {
                        nvo.DetailsList = new List<clsParameterLinkingVO>();
                    }
                    while (reader.Read())
                    {
                        clsParameterLinkingVO item = new clsParameterLinkingVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ParameterName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                        };
                        nvo.DetailsList.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetParameterListForTest(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetParameterOrSubTestSearchBizActionVO nvo = valueObject as clsGetParameterOrSubTestSearchBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetParameterByName");
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Flag", DbType.Int16, nvo.Flag);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ParameterList == null)
                    {
                        nvo.ParameterList = new List<clsPathoTestMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoTestMasterVO item = new clsPathoTestMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            FormulaID = Convert.ToString(DALHelper.HandleDBNull(reader["FormulaID"]))
                        };
                        nvo.ParameterList.Add(item);
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetPathoGender(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTemplateGenderBizActionVO nvo = valueObject as clsGetPathoTemplateGenderBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoGenderTemplateList");
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.String, nvo.TemplateID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.GenderDetails == null)
                    {
                        nvo.GenderDetails = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]))
                        };
                        nvo.GenderDetails.Add(item);
                    }
                }
                reader.Close();
            }
            catch
            {
            }
            return nvo;
        }

        public override IValueObject GetPathologistToTempList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetPathologistToTempBizActionVO nvo = valueObject as clsGetPathologistToTempBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathologistListForTest");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ItemSupplier.TemplateID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemSupplierList == null)
                    {
                        nvo.ItemSupplierList = new List<clsPathoTestTemplateDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoTestTemplateDetailsVO item = new clsPathoTestTemplateDetailsVO {
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            Pathologist = (long) DALHelper.HandleDBNull(reader["PathologistID"]),
                            TemplateID = (long) DALHelper.HandleDBNull(reader["TemplateID"])
                        };
                        nvo.ItemSupplierList.Add(item);
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetPathoOutSourcedTestList(IValueObject valueObject, clsUserVO UserVO)
        {
            DbDataReader reader = null;
            clsGetPathoOutSourceTestListBizActionVO nvo = valueObject as clsGetPathoOutSourceTestListBizActionVO;
            clsPathoTestOutSourceDetailsVO pathoOutSourceTestDetails = nvo.PathoOutSourceTestDetails;
            clsPathoTestOutSourceDetailsVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOrderBookingOutsourcingTestList");
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, pathoOutSourceTestDetails.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, pathoOutSourceTestDetails.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, pathoOutSourceTestDetails.AgencyID);
                this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, pathoOutSourceTestDetails.FirstName);
                this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, pathoOutSourceTestDetails.LastName);
                this.dbServer.AddInParameter(storedProcCommand, "TestName", DbType.String, pathoOutSourceTestDetails.TestName);
                this.dbServer.AddInParameter(storedProcCommand, "OutSourceType", DbType.Boolean, pathoOutSourceTestDetails.OutSourceType);
                this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, pathoOutSourceTestDetails.MRNo);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsPathoTestOutSourceDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            OrderDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderDetailID"])),
                            FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])),
                            LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])),
                            MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MrNo"])),
                            OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"])),
                            OrderNo = Convert.ToString(DALHelper.HandleDBNull(reader["OrderNo"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            OrderDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]))),
                            TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"])),
                            TestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"])),
                            IsOutSourced = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOutSourced"])),
                            IsOutSourced1 = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOutSourced1"])),
                            AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtAgencyID"])),
                            AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"])),
                            IsChangedAgency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsChangedAgency"])),
                            ReasonToChangeAgency = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyChangeReason"])),
                            AgencyAssignReason = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyAssignReason"])),
                            IsSampleCollected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleCollected"])),
                            SampleDispatchDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["SampleDispatchDateTime"]))),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            Prefix = Convert.ToString(DALHelper.HandleDBNull(reader["Pre"]))
                        };
                        nvo.PathoOutSourceTestList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetPathoParameterSampleAndItemDetailsByTestID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO nvo = valueObject as clsGetPathoParameterSampleAndItemDetailsByTestIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoParameterSampleAndItemDetails");
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, nvo.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFormSubTest", DbType.Int64, nvo.IsFormSubTest);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ParameterList == null)
                    {
                        nvo.ParameterList = new List<clsPathoTestParameterVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoTestParameterVO ObjTemp = new clsPathoTestParameterVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            PathoTestID = (long) DALHelper.HandleDBNull(reader["PathoTestID"]),
                            ParamSTID = (long) DALHelper.HandleDBNull(reader["ParamSTID"]),
                            Status = false,
                            IsParameter = true,
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            SelectedPrintName = { ID = (long) DALHelper.HandleDBNull(reader["PrintNameType"]) }
                        };
                        ObjTemp.SelectedPrintName = ObjTemp.PrintName.FirstOrDefault<MasterListItem>(q => q.ID == ObjTemp.SelectedPrintName.ID);
                        ObjTemp.Print = (string) DALHelper.HandleDBNull(reader["PrintName"]);
                        ObjTemp.PrintPosition = (long) DALHelper.HandleDBNull(reader["PrintPosition"]);
                        ObjTemp.IsNumeric = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsNumeric"]));
                        nvo.ParameterList.Add(ObjTemp);
                    }
                }
                reader.NextResult();
                if (nvo.SubTestList == null)
                {
                    nvo.SubTestList = new List<clsPathoSubTestVO>();
                }
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.NextResult();
                        if (nvo.SampleList == null)
                        {
                            nvo.SampleList = new List<clsPathoTestSampleVO>();
                        }
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                reader.NextResult();
                                if (nvo.ItemList == null)
                                {
                                    nvo.ItemList = new List<clsPathoTestItemDetailsVO>();
                                }
                                while (true)
                                {
                                    if (!reader.Read())
                                    {
                                        reader.NextResult();
                                        if (nvo.TemplateList == null)
                                        {
                                            nvo.TemplateList = new List<clsPathoTemplateVO>();
                                        }
                                        while (true)
                                        {
                                            if (!reader.Read())
                                            {
                                                reader.Close();
                                                break;
                                            }
                                            clsPathoTemplateVO evo2 = new clsPathoTemplateVO {
                                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                                TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"])),
                                                TemplateName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                                                TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"])),
                                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                                            };
                                            nvo.TemplateList.Add(evo2);
                                        }
                                        break;
                                    }
                                    clsPathoTestItemDetailsVO svo = new clsPathoTestItemDetailsVO {
                                        ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                        TestID = (long) DALHelper.HandleDBNull(reader["TestID"]),
                                        ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                                        Quantity = (float) ((double) DALHelper.HandleDBNull(reader["Quantity"])),
                                        ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                                        Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                                        UID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UID"])),
                                        UName = Convert.ToString(DALHelper.HandleDBNull(reader["UName"])),
                                        DID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DID"])),
                                        DName = Convert.ToString(DALHelper.HandleDBNull(reader["DName"])),
                                        UOMid = Convert.ToInt64(DALHelper.HandleDBNull(reader["UOMid"])),
                                        UOMName = Convert.ToString(DALHelper.HandleDBNull(reader["UOMName"]))
                                    };
                                    nvo.ItemList.Add(svo);
                                }
                                break;
                            }
                            clsPathoTestSampleVO item = new clsPathoTestSampleVO {
                                ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                PathoTestID = (long) DALHelper.HandleDBNull(reader["PathoTestID"]),
                                SampleID = (long) DALHelper.HandleDBNull(reader["SampleID"]),
                                Quantity = (float) ((double) DALHelper.HandleDBNull(reader["Quantity"])),
                                SampleName = (string) DALHelper.HandleDBNull(reader["Description"]),
                                Frequency = (string) DALHelper.HandleDBNull(reader["Frequency"]),
                                Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                            };
                            nvo.SampleList.Add(item);
                        }
                        break;
                    }
                    clsPathoSubTestVO tvo1 = new clsPathoSubTestVO {
                        ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                        PathoTestID = (long) DALHelper.HandleDBNull(reader["PathoTestID"]),
                        ParamSTID = (long) DALHelper.HandleDBNull(reader["ParamSTID"]),
                        Status = false,
                        IsParameter = false,
                        Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                        SelectedPrintName = { ID = (long) DALHelper.HandleDBNull(reader["PrintNameType"]) }
                    };
                    tvo1.SelectedPrintName = tvo1.PrintName.FirstOrDefault<MasterListItem>(q => q.ID == tvo1.SelectedPrintName.ID);
                    tvo1.Print = (string) DALHelper.HandleDBNull(reader["PrintName"]);
                    tvo1.PrintPosition = (long) DALHelper.HandleDBNull(reader["PrintPosition"]);
                    nvo.SubTestList.Add(tvo1);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPathoParameterUnitsByParamID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoParameterUnitsByParamIDBizActionVO nvo = valueObject as clsGetPathoParameterUnitsByParamIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoParameterUnitFromParameterID");
                this.dbServer.AddInParameter(storedProcCommand, "ParamID", DbType.Int64, nvo.ParamID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.SampleList == null)
                    {
                        nvo.SampleList = new List<clsPathoParameterUnitMaterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        clsPathoParameterUnitMaterVO item = new clsPathoParameterUnitMaterVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.SampleList.Add(item);
                    }
                }
            }
            catch
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPathoProfileDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoProfileDetailsBizActionVO nvo = valueObject as clsGetPathoProfileDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoProfileList");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ProfileList == null)
                    {
                        nvo.ProfileList = new List<clsPathoProfileMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoProfileMasterVO item = new clsPathoProfileMasterVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceID"]),
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.ProfileList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetPathoProfileServiceIDForPathoTestMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoProfileServiceIDForPathoTestMasterBizActionVO nvo = valueObject as clsGetPathoProfileServiceIDForPathoTestMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoProfileServiceIDForPathoTestMaster");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceDetails == null)
                    {
                        nvo.ServiceDetails = new List<clsPathoTestMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsPathoTestMasterVO item = new clsPathoTestMasterVO {
                            ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceID"])
                        };
                        nvo.ServiceDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPathOrderBookingDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathOrderBookingDetailListBizActionVO nvo = valueObject as clsGetPathOrderBookingDetailListBizActionVO;
            if (nvo.IsFrom == "ResultEntry")
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathDetailListForResultEntry");
                    this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, nvo.OrderID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TestCategoryID", DbType.Int64, nvo.TestCategoryID);
                    this.dbServer.AddInParameter(storedProcCommand, "AuthenticationLevel", DbType.Int64, nvo.AuthenticationLevel);
                    if (nvo.CheckExtraCriteria)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromCollection", DbType.Boolean, nvo.IsFromCollection);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromReceive", DbType.Boolean, nvo.IsFromReceive);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromDispatch", DbType.Boolean, nvo.IsFromDispatch);
                        this.dbServer.AddInParameter(storedProcCommand, "SeacrhFromAcceptReject", DbType.Boolean, nvo.IsFromAcceptRejct);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromResult", DbType.Boolean, nvo.IsFromResult);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromAutoriation", DbType.Boolean, nvo.IsFromAuthorization);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromUpload", DbType.Boolean, nvo.IsFromUpload);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckExtraCriteria", DbType.Boolean, nvo.CheckExtraCriteria);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckSampleType", DbType.Boolean, nvo.CheckSampleType);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleType", DbType.Boolean, nvo.SampleType);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckUploadStatus", DbType.Boolean, nvo.CheckUploadStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "IsUploaded", DbType.Boolean, nvo.IsUploaded);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckDeliveryStatus", DbType.Boolean, nvo.CheckDeliveryStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDelivered", DbType.Boolean, nvo.OrderDetail.IsDelivered);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDelivered1", DbType.Boolean, nvo.OrderDetail.IsDelivered1);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDeliverdthroughEmail", DbType.Boolean, nvo.OrderDetail.IsDeliverdthroughEmail);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDirectDelivered", DbType.Boolean, nvo.OrderDetail.IsDirectDelivered);
                        this.dbServer.AddInParameter(storedProcCommand, "IsResultEntry", DbType.Boolean, nvo.OrderDetail.IsResultEntry);
                        this.dbServer.AddInParameter(storedProcCommand, "IsResultEntry1", DbType.Boolean, nvo.OrderDetail.IsResultEntry1);
                        this.dbServer.AddInParameter(storedProcCommand, "IsBilled", DbType.Boolean, nvo.OrderDetail.IsBilled);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleCollected", DbType.Boolean, nvo.OrderDetail.IsSampleCollected);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleDispatched", DbType.Boolean, nvo.OrderDetail.IsSampleDispatch);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleReceived", DbType.Boolean, nvo.OrderDetail.IsSampleReceive);
                        this.dbServer.AddInParameter(storedProcCommand, "IsAccepted", DbType.Boolean, true);
                        this.dbServer.AddInParameter(storedProcCommand, "IsRejected", DbType.Boolean, nvo.OrderDetail.IsRejected);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSubOptimal", DbType.Boolean, nvo.OrderDetail.IsSubOptimal);
                        this.dbServer.AddInParameter(storedProcCommand, "IsOutSourced", DbType.Boolean, nvo.OrderDetail.IsOutSourced);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleCollectedBy", DbType.String, nvo.OrderDetail.SampleCollectedBy);
                        this.dbServer.AddInParameter(storedProcCommand, "DispatchBy", DbType.String, nvo.OrderDetail.DispatchBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AcceptedOrRejectedByName", DbType.String, nvo.OrderDetail.AcceptedOrRejectedByName);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleReceiveBy", DbType.String, nvo.OrderDetail.SampleReceiveBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.OrderDetail.AgencyID);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.OrderDetail.SampleNo);
                        this.dbServer.AddInParameter(storedProcCommand, "IsExternalPatient", DbType.Boolean, nvo.OrderDetail.IsExternalPatient);
                        this.dbServer.AddInParameter(storedProcCommand, "CatagoryID", DbType.Int64, nvo.OrderDetail.CategoryID);
                        this.dbServer.AddInParameter(storedProcCommand, "ResultEntryUserID", DbType.Int64, nvo.OrderDetail.ResultEntryUserID);
                        this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                        this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    }
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.OrderBookingDetailList == null)
                        {
                            nvo.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                        }
                        while (reader.Read())
                        {
                            clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                                ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                OrderBookingID = (long) DALHelper.HandleDBNull(reader["OrderID"]),
                                PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"])),
                                UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]),
                                DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"])),
                                SampleCollected = (DateTime?) DALHelper.HandleDBNull(reader["SampleCollected"]),
                                IsSampleCollected = (bool) DALHelper.HandleDBNull(reader["IsSampleCollected"]),
                                IsCompleted = (bool) DALHelper.HandleDBNull(reader["IsCompleted"]),
                                IsDelivered = (bool) DALHelper.HandleDBNull(reader["IsDelivered"]),
                                TestID = (long) DALHelper.HandleDBNull(reader["TestID"]),
                                TestCode = (string) DALHelper.HandleDBNull(reader["TestCode"]),
                                TestName = (string) DALHelper.HandleDBNull(reader["TestName"]),
                                ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                                ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                                SourceURL = (string) DALHelper.HandleDBNull(reader["SourceURL"]),
                                Report = (byte[]) DALHelper.HandleDBNull(reader["Report"]),
                                SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"])),
                                IsOutSourced = (bool) DALHelper.HandleDBNull(reader["IsOutSourced"]),
                                Quantity = (double?) DALHelper.HandleDBNull(reader["Quantity"]),
                                Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                                UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"]),
                                IsResultEntry = (bool) DALHelper.HandleDBNull(reader["IsResultEntry"]),
                                IsFinalized = (bool) DALHelper.HandleDBNull(reader["IsFinalized"]),
                                HandDeliverdDateTime = (DateTime?) DALHelper.HandleDBNull(reader["HandDeliverdDateTime"]),
                                IsDirectDelivered = (bool) DALHelper.HandleBoolDBNull(reader["IsDirectDelivered"]),
                                EmailDeliverdDateTime = (DateTime?) DALHelper.HandleDBNull(reader["EmailDeliverdDateTime"]),
                                IsDeliverdthroughEmail = (bool) DALHelper.HandleBoolDBNull(reader["IsDeliverdthroughEmail"]),
                                SampleCollectedDateTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleCollectedDateTime"]),
                                IsSampleDispatch = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleDispatched"])),
                                SampleDispatchDateTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleDispatchDateTime"]),
                                IsSampleReceive = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleReceived"])),
                                SampleReceivedDateTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleReceivedDateTime"]),
                                SampleAcceptRejectStatus = Convert.ToByte(DALHelper.HandleDBNull(reader["SampleAcceptRejectStatus"])),
                                SampleAcceptanceDateTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleAcceptDateTime"]),
                                SampleRejectionDateTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleRejectDateTime"]),
                                SampleRejectionRemark = Convert.ToString(DALHelper.HandleDBNull(reader["RejectionRemark"])),
                                FirstLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["FirstLevel"])),
                                SecondLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["SecondLevel"])),
                                ThirdLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ThirdLevel"])),
                                IsCheckedResults = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCheckedResults"])),
                                ReportTemplate = (bool) DALHelper.HandleDBNull(reader["ReportTemplate"]),
                                RefDoctorID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["RefDoctorID"])))
                            };
                            item.ReportType = item.ReportTemplate ? "Template" : "Parameter";
                            item.AppendWith = Convert.ToInt16(DALHelper.HandleDBNull(reader["AppendSampleNo"]));
                            item.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RefundID"]));
                            item.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryID"]));
                            item.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategoryTypeID"]));
                            item.ThirdLevelCheckResult = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ThirdLevelCheckResult"]));
                            item.CheckResultValueMessage = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultValueMessage"]));
                            item.MsgCheckResultValForSecLevel = Convert.ToString(DALHelper.HandleDBNull(reader["CheckResultValForSecLevel"]));
                            item.PathologistID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["PathologistID1"])));
                            item.TemplateResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateResultID"]));
                            item.TubeName = Convert.ToString(DALHelper.HandleDBNull(reader["TubeName"]));
                            item.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]));
                            item.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"]));
                            item.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExternalPatient"]));
                            item.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                            item.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                            item.IsSampleGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSampleGenerated"]));
                            item.IsServiceRefunded = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsServiceRefunded"]));
                            item.ADateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])));
                            nvo.OrderBookingDetailList.Add(item);
                        }
                    }
                    reader.NextResult();
                    if (nvo.objOutsourceOrderBookingDetail == null)
                    {
                        nvo.objOutsourceOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            OrderBookingID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"])),
                            TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            AgencyID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["AgencyID"]))),
                            AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]))
                        };
                        nvo.objOutsourceOrderBookingDetail.Add(item);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else if (nvo.IsFrom == "Authorization")
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathDetailListForAuthorization");
                    this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, nvo.OrderID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TestCategoryID", DbType.Int64, nvo.TestCategoryID);
                    this.dbServer.AddInParameter(storedProcCommand, "AuthenticationLevel", DbType.Int64, nvo.AuthenticationLevel);
                    if (nvo.CheckExtraCriteria)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromCollection", DbType.Boolean, nvo.IsFromCollection);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromReceive", DbType.Boolean, nvo.IsFromReceive);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromDispatch", DbType.Boolean, nvo.IsFromDispatch);
                        this.dbServer.AddInParameter(storedProcCommand, "SeacrhFromAcceptReject", DbType.Boolean, nvo.IsFromAcceptRejct);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromResult", DbType.Boolean, nvo.IsFromResult);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromAutoriation", DbType.Boolean, nvo.IsFromAuthorization);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromUpload", DbType.Boolean, nvo.IsFromUpload);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckExtraCriteria", DbType.Boolean, nvo.CheckExtraCriteria);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckSampleType", DbType.Boolean, nvo.CheckSampleType);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleType", DbType.Boolean, nvo.SampleType);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckUploadStatus", DbType.Boolean, nvo.CheckUploadStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "IsUploaded", DbType.Boolean, nvo.IsUploaded);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckDeliveryStatus", DbType.Boolean, nvo.CheckDeliveryStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDelivered", DbType.Boolean, nvo.OrderDetail.IsDelivered);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDelivered1", DbType.Boolean, nvo.OrderDetail.IsDelivered1);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDeliverdthroughEmail", DbType.Boolean, nvo.OrderDetail.IsDeliverdthroughEmail);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDirectDelivered", DbType.Boolean, nvo.OrderDetail.IsDirectDelivered);
                        this.dbServer.AddInParameter(storedProcCommand, "IsResultEntry", DbType.Boolean, nvo.OrderDetail.IsResultEntry);
                        this.dbServer.AddInParameter(storedProcCommand, "IsResultEntry1", DbType.Boolean, nvo.OrderDetail.IsResultEntry1);
                        this.dbServer.AddInParameter(storedProcCommand, "IsBilled", DbType.Boolean, nvo.OrderDetail.IsBilled);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleCollected", DbType.Boolean, nvo.OrderDetail.IsSampleCollected);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleDispatched", DbType.Boolean, nvo.OrderDetail.IsSampleDispatch);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleReceived", DbType.Boolean, nvo.OrderDetail.IsSampleReceive);
                        this.dbServer.AddInParameter(storedProcCommand, "IsAccepted", DbType.Boolean, true);
                        this.dbServer.AddInParameter(storedProcCommand, "IsRejected", DbType.Boolean, nvo.OrderDetail.IsRejected);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSubOptimal", DbType.Boolean, nvo.OrderDetail.IsSubOptimal);
                        this.dbServer.AddInParameter(storedProcCommand, "IsOutSourced", DbType.Boolean, nvo.OrderDetail.IsOutSourced);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleCollectedBy", DbType.String, nvo.OrderDetail.SampleCollectedBy);
                        this.dbServer.AddInParameter(storedProcCommand, "DispatchBy", DbType.String, nvo.OrderDetail.DispatchBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AcceptedOrRejectedByName", DbType.String, nvo.OrderDetail.AcceptedOrRejectedByName);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleReceiveBy", DbType.String, nvo.OrderDetail.SampleReceiveBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.OrderDetail.AgencyID);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.OrderDetail.SampleNo);
                        this.dbServer.AddInParameter(storedProcCommand, "IsExternalPatient", DbType.Boolean, nvo.OrderDetail.IsExternalPatient);
                        this.dbServer.AddInParameter(storedProcCommand, "AuthUserID", DbType.Int64, nvo.OrderDetail.ResultEntryUserID);
                        this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                        this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    }
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.OrderBookingDetailList == null)
                        {
                            nvo.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                        }
                        while (reader2.Read())
                        {
                            clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                                ID = (long) DALHelper.HandleDBNull(reader2["ID"]),
                                OrderBookingID = (long) DALHelper.HandleDBNull(reader2["OrderID"]),
                                PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["PathPatientReportID"])),
                                UnitId = (long) DALHelper.HandleDBNull(reader2["UnitId"]),
                                DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["DispatchToID"])),
                                SampleCollected = (DateTime?) DALHelper.HandleDBNull(reader2["SampleCollected"]),
                                IsSampleCollected = (bool) DALHelper.HandleDBNull(reader2["IsSampleCollected"]),
                                IsCompleted = (bool) DALHelper.HandleDBNull(reader2["IsCompleted"]),
                                IsDelivered = (bool) DALHelper.HandleDBNull(reader2["IsDelivered"]),
                                TestID = (long) DALHelper.HandleDBNull(reader2["TestID"]),
                                TestCode = (string) DALHelper.HandleDBNull(reader2["TestCode"]),
                                TestName = (string) DALHelper.HandleDBNull(reader2["TestName"]),
                                ServiceName = (string) DALHelper.HandleDBNull(reader2["ServiceName"]),
                                ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ServiceID"])),
                                SourceURL = (string) DALHelper.HandleDBNull(reader2["SourceURL"]),
                                Report = (byte[]) DALHelper.HandleDBNull(reader2["Report"]),
                                SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader2["SampleNo"])),
                                IsOutSourced = (bool) DALHelper.HandleDBNull(reader2["IsOutSourced"]),
                                Quantity = (double?) DALHelper.HandleDBNull(reader2["Quantity"]),
                                Status = (bool) DALHelper.HandleDBNull(reader2["Status"]),
                                UnitName = (string) DALHelper.HandleDBNull(reader2["UnitName"]),
                                IsResultEntry = (bool) DALHelper.HandleDBNull(reader2["IsResultEntry"]),
                                IsFinalized = (bool) DALHelper.HandleDBNull(reader2["IsFinalized"]),
                                HandDeliverdDateTime = (DateTime?) DALHelper.HandleDBNull(reader2["HandDeliverdDateTime"]),
                                IsDirectDelivered = (bool) DALHelper.HandleBoolDBNull(reader2["IsDirectDelivered"]),
                                EmailDeliverdDateTime = (DateTime?) DALHelper.HandleDBNull(reader2["EmailDeliverdDateTime"]),
                                IsDeliverdthroughEmail = (bool) DALHelper.HandleBoolDBNull(reader2["IsDeliverdthroughEmail"]),
                                SampleCollectedDateTime = (DateTime?) DALHelper.HandleDBNull(reader2["SampleCollectedDateTime"]),
                                IsSampleDispatch = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsSampleDispatched"])),
                                SampleDispatchDateTime = (DateTime?) DALHelper.HandleDBNull(reader2["SampleDispatchDateTime"]),
                                IsSampleReceive = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsSampleReceived"])),
                                SampleReceivedDateTime = (DateTime?) DALHelper.HandleDBNull(reader2["SampleReceivedDateTime"]),
                                SampleAcceptRejectStatus = Convert.ToByte(DALHelper.HandleDBNull(reader2["SampleAcceptRejectStatus"])),
                                SampleAcceptanceDateTime = (DateTime?) DALHelper.HandleDBNull(reader2["SampleAcceptDateTime"]),
                                SampleRejectionDateTime = (DateTime?) DALHelper.HandleDBNull(reader2["SampleRejectDateTime"]),
                                SampleRejectionRemark = Convert.ToString(DALHelper.HandleDBNull(reader2["RejectionRemark"])),
                                FirstLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["FirstLevel"])),
                                SecondLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["SecondLevel"])),
                                ThirdLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["ThirdLevel"])),
                                IsCheckedResults = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsCheckedResults"])),
                                ReportTemplate = (bool) DALHelper.HandleDBNull(reader2["ReportTemplate"]),
                                RefDoctorID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader2["RefDoctorID"])))
                            };
                            item.ReportType = item.ReportTemplate ? "Template" : "Parameter";
                            item.AppendWith = Convert.ToInt16(DALHelper.HandleDBNull(reader2["AppendSampleNo"]));
                            item.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["RefundID"]));
                            item.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["CategoryID"]));
                            item.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["CategoryTypeID"]));
                            item.ThirdLevelCheckResult = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["ThirdLevelCheckResult"]));
                            item.CheckResultValueMessage = Convert.ToString(DALHelper.HandleDBNull(reader2["CheckResultValueMessage"]));
                            item.MsgCheckResultValForSecLevel = Convert.ToString(DALHelper.HandleDBNull(reader2["CheckResultValForSecLevel"]));
                            item.PathologistID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader2["PathologistID1"])));
                            item.TemplateResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["TemplateResultID"]));
                            item.TubeName = Convert.ToString(DALHelper.HandleDBNull(reader2["TubeName"]));
                            item.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader2["AgencyName"]));
                            item.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["DispatchToID"]));
                            item.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsExternalPatient"]));
                            item.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["BillID"]));
                            item.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsResendForNewSample"]));
                            item.IsSampleGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsSampleGenerated"]));
                            item.IsServiceRefunded = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsServiceRefunded"]));
                            item.ADateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader2["Date"])));
                            nvo.OrderBookingDetailList.Add(item);
                        }
                    }
                    reader2.NextResult();
                    if (nvo.objOutsourceOrderBookingDetail == null)
                    {
                        nvo.objOutsourceOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
                    }
                    while (true)
                    {
                        if (!reader2.Read())
                        {
                            reader2.Close();
                            break;
                        }
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            OrderBookingID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["OrderID"])),
                            TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["TestID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ServiceID"])),
                            AgencyID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader2["AgencyID"]))),
                            AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader2["AgencyName"]))
                        };
                        nvo.objOutsourceOrderBookingDetail.Add(item);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else if (nvo.IsFrom == "Delivery")
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathDetailListForDelivery");
                    this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, nvo.OrderID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TestCategoryID", DbType.Int64, nvo.TestCategoryID);
                    if (nvo.CheckExtraCriteria)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CheckExtraCriteria", DbType.Boolean, nvo.CheckExtraCriteria);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckSampleType", DbType.Boolean, nvo.CheckSampleType);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleType", DbType.Boolean, nvo.SampleType);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckUploadStatus", DbType.Boolean, nvo.CheckUploadStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "IsUploaded", DbType.Boolean, nvo.IsUploaded);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckDeliveryStatus", DbType.Boolean, nvo.CheckDeliveryStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDeliverdthroughEmail", DbType.Boolean, nvo.OrderDetail.IsDeliverdthroughEmail);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDirectDelivered", DbType.Boolean, nvo.OrderDetail.IsDirectDelivered);
                        this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.OrderDetail.AgencyID);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.OrderDetail.SampleNo);
                        this.dbServer.AddInParameter(storedProcCommand, "IsExternalPatient", DbType.Boolean, nvo.OrderDetail.IsExternalPatient);
                        this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.OrderDetail.UserID);
                        this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                        this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    }
                    DbDataReader reader3 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader3.HasRows)
                    {
                        if (nvo.OrderBookingDetailList == null)
                        {
                            nvo.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                        }
                        while (reader3.Read())
                        {
                            clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                                ID = (long) DALHelper.HandleDBNull(reader3["ID"]),
                                OrderBookingID = (long) DALHelper.HandleDBNull(reader3["OrderID"]),
                                PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["PathPatientReportID"])),
                                UnitId = (long) DALHelper.HandleDBNull(reader3["UnitId"]),
                                DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["DispatchToID"])),
                                SampleCollected = (DateTime?) DALHelper.HandleDBNull(reader3["SampleCollected"]),
                                IsSampleCollected = (bool) DALHelper.HandleDBNull(reader3["IsSampleCollected"]),
                                IsCompleted = (bool) DALHelper.HandleDBNull(reader3["IsCompleted"]),
                                IsDelivered = (bool) DALHelper.HandleDBNull(reader3["IsDelivered"]),
                                TestID = (long) DALHelper.HandleDBNull(reader3["TestID"]),
                                TestCode = (string) DALHelper.HandleDBNull(reader3["TestCode"]),
                                TestName = (string) DALHelper.HandleDBNull(reader3["TestName"]),
                                ServiceName = (string) DALHelper.HandleDBNull(reader3["ServiceName"]),
                                ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["ServiceID"])),
                                SourceURL = (string) DALHelper.HandleDBNull(reader3["SourceURL"]),
                                Report = (byte[]) DALHelper.HandleDBNull(reader3["Report"]),
                                SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader3["SampleNo"])),
                                IsOutSourced = (bool) DALHelper.HandleDBNull(reader3["IsOutSourced"]),
                                Quantity = (double?) DALHelper.HandleDBNull(reader3["Quantity"]),
                                Status = (bool) DALHelper.HandleDBNull(reader3["Status"]),
                                UnitName = (string) DALHelper.HandleDBNull(reader3["UnitName"]),
                                IsResultEntry = (bool) DALHelper.HandleDBNull(reader3["IsResultEntry"]),
                                IsFinalized = (bool) DALHelper.HandleDBNull(reader3["IsFinalized"]),
                                HandDeliverdDateTime = (DateTime?) DALHelper.HandleDBNull(reader3["HandDeliverdDateTime"]),
                                IsDirectDelivered = (bool) DALHelper.HandleBoolDBNull(reader3["IsDirectDelivered"]),
                                EmailDeliverdDateTime = (DateTime?) DALHelper.HandleDBNull(reader3["EmailDeliverdDateTime"]),
                                IsDeliverdthroughEmail = (bool) DALHelper.HandleBoolDBNull(reader3["IsDeliverdthroughEmail"]),
                                SampleCollectedDateTime = (DateTime?) DALHelper.HandleDBNull(reader3["SampleCollectedDateTime"]),
                                IsSampleDispatch = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsSampleDispatched"])),
                                SampleDispatchDateTime = (DateTime?) DALHelper.HandleDBNull(reader3["SampleDispatchDateTime"]),
                                IsSampleReceive = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsSampleReceived"])),
                                SampleReceivedDateTime = (DateTime?) DALHelper.HandleDBNull(reader3["SampleReceivedDateTime"]),
                                SampleAcceptRejectStatus = Convert.ToByte(DALHelper.HandleDBNull(reader3["SampleAcceptRejectStatus"])),
                                SampleAcceptanceDateTime = (DateTime?) DALHelper.HandleDBNull(reader3["SampleAcceptDateTime"]),
                                SampleRejectionDateTime = (DateTime?) DALHelper.HandleDBNull(reader3["SampleRejectDateTime"]),
                                SampleRejectionRemark = Convert.ToString(DALHelper.HandleDBNull(reader3["RejectionRemark"])),
                                FirstLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["FirstLevel"])),
                                SecondLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["SecondLevel"])),
                                ThirdLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["ThirdLevel"])),
                                IsCheckedResults = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsCheckedResults"])),
                                ReportTemplate = (bool) DALHelper.HandleDBNull(reader3["ReportTemplate"]),
                                RefDoctorID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader3["RefDoctorID"])))
                            };
                            item.ReportType = item.ReportTemplate ? "Template" : "Parameter";
                            item.AppendWith = Convert.ToInt16(DALHelper.HandleDBNull(reader3["AppendSampleNo"]));
                            item.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["RefundID"]));
                            item.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["CategoryID"]));
                            item.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["CategoryTypeID"]));
                            item.ThirdLevelCheckResult = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["ThirdLevelCheckResult"]));
                            item.CheckResultValueMessage = Convert.ToString(DALHelper.HandleDBNull(reader3["CheckResultValueMessage"]));
                            item.MsgCheckResultValForSecLevel = Convert.ToString(DALHelper.HandleDBNull(reader3["CheckResultValForSecLevel"]));
                            item.PathologistID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader3["PathologistID1"])));
                            item.TemplateResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["TemplateResultID"]));
                            item.TubeName = Convert.ToString(DALHelper.HandleDBNull(reader3["TubeName"]));
                            item.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader3["AgencyName"]));
                            item.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["DispatchToID"]));
                            item.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsExternalPatient"]));
                            item.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["BillID"]));
                            item.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsResendForNewSample"]));
                            item.IsSampleGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsSampleGenerated"]));
                            item.IsServiceRefunded = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsServiceRefunded"]));
                            item.ADateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader3["Date"])));
                            nvo.OrderBookingDetailList.Add(item);
                        }
                    }
                    reader3.NextResult();
                    if (nvo.objOutsourceOrderBookingDetail == null)
                    {
                        nvo.objOutsourceOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
                    }
                    while (true)
                    {
                        if (!reader3.Read())
                        {
                            reader3.Close();
                            break;
                        }
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            OrderBookingID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["OrderID"])),
                            TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["TestID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["ServiceID"])),
                            AgencyID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader3["AgencyID"]))),
                            AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader3["AgencyName"]))
                        };
                        nvo.objOutsourceOrderBookingDetail.Add(item);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else if (nvo.IsFrom == "UploadReport")
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathDetailListForUpload");
                    this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, nvo.OrderID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TestCategoryID", DbType.Int64, nvo.TestCategoryID);
                    if (nvo.CheckExtraCriteria)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CheckExtraCriteria", DbType.Boolean, nvo.CheckExtraCriteria);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckSampleType", DbType.Boolean, nvo.CheckSampleType);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleType", DbType.Boolean, nvo.SampleType);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckUploadStatus", DbType.Boolean, nvo.CheckUploadStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckDeliveryStatus", DbType.Boolean, nvo.CheckDeliveryStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDeliverdthroughEmail", DbType.Boolean, nvo.OrderDetail.IsDeliverdthroughEmail);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDirectDelivered", DbType.Boolean, nvo.OrderDetail.IsDirectDelivered);
                        this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.AgencyID);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.OrderDetail.SampleNo);
                        this.dbServer.AddInParameter(storedProcCommand, "IsExternalPatient", DbType.Boolean, nvo.OrderDetail.IsExternalPatient);
                        this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.OrderDetail.UserID);
                        this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                        this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    }
                    DbDataReader reader4 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader4.HasRows)
                    {
                        if (nvo.OrderBookingDetailList == null)
                        {
                            nvo.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                        }
                        while (reader4.Read())
                        {
                            clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                                ID = (long) DALHelper.HandleDBNull(reader4["ID"]),
                                OrderBookingID = (long) DALHelper.HandleDBNull(reader4["OrderID"]),
                                PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["PathPatientReportID"])),
                                UnitId = (long) DALHelper.HandleDBNull(reader4["UnitId"]),
                                DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["DispatchToID"])),
                                SampleCollected = (DateTime?) DALHelper.HandleDBNull(reader4["SampleCollected"]),
                                IsSampleCollected = (bool) DALHelper.HandleDBNull(reader4["IsSampleCollected"]),
                                IsCompleted = (bool) DALHelper.HandleDBNull(reader4["IsCompleted"]),
                                IsDelivered = (bool) DALHelper.HandleDBNull(reader4["IsDelivered"]),
                                TestID = (long) DALHelper.HandleDBNull(reader4["TestID"]),
                                TestCode = (string) DALHelper.HandleDBNull(reader4["TestCode"]),
                                TestName = (string) DALHelper.HandleDBNull(reader4["TestName"]),
                                ServiceName = (string) DALHelper.HandleDBNull(reader4["ServiceName"]),
                                ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["ServiceID"])),
                                SourceURL = (string) DALHelper.HandleDBNull(reader4["SourceURL"]),
                                Report = (byte[]) DALHelper.HandleDBNull(reader4["Report"]),
                                SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader4["SampleNo"])),
                                IsOutSourced = (bool) DALHelper.HandleDBNull(reader4["IsOutSourced"]),
                                Quantity = (double?) DALHelper.HandleDBNull(reader4["Quantity"]),
                                Status = (bool) DALHelper.HandleDBNull(reader4["Status"]),
                                UnitName = (string) DALHelper.HandleDBNull(reader4["UnitName"]),
                                IsResultEntry = (bool) DALHelper.HandleDBNull(reader4["IsResultEntry"]),
                                IsFinalized = (bool) DALHelper.HandleDBNull(reader4["IsFinalized"]),
                                HandDeliverdDateTime = (DateTime?) DALHelper.HandleDBNull(reader4["HandDeliverdDateTime"]),
                                IsDirectDelivered = (bool) DALHelper.HandleBoolDBNull(reader4["IsDirectDelivered"]),
                                EmailDeliverdDateTime = (DateTime?) DALHelper.HandleDBNull(reader4["EmailDeliverdDateTime"]),
                                IsDeliverdthroughEmail = (bool) DALHelper.HandleBoolDBNull(reader4["IsDeliverdthroughEmail"]),
                                SampleCollectedDateTime = (DateTime?) DALHelper.HandleDBNull(reader4["SampleCollectedDateTime"]),
                                IsSampleDispatch = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["IsSampleDispatched"])),
                                SampleDispatchDateTime = (DateTime?) DALHelper.HandleDBNull(reader4["SampleDispatchDateTime"]),
                                IsSampleReceive = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["IsSampleReceived"])),
                                SampleReceivedDateTime = (DateTime?) DALHelper.HandleDBNull(reader4["SampleReceivedDateTime"]),
                                SampleAcceptRejectStatus = Convert.ToByte(DALHelper.HandleDBNull(reader4["SampleAcceptRejectStatus"])),
                                SampleAcceptanceDateTime = (DateTime?) DALHelper.HandleDBNull(reader4["SampleAcceptDateTime"]),
                                SampleRejectionDateTime = (DateTime?) DALHelper.HandleDBNull(reader4["SampleRejectDateTime"]),
                                SampleRejectionRemark = Convert.ToString(DALHelper.HandleDBNull(reader4["RejectionRemark"])),
                                FirstLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["FirstLevel"])),
                                SecondLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["SecondLevel"])),
                                ThirdLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["ThirdLevel"])),
                                IsCheckedResults = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["IsCheckedResults"])),
                                ReportTemplate = (bool) DALHelper.HandleDBNull(reader4["ReportTemplate"]),
                                RefDoctorID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader4["RefDoctorID"])))
                            };
                            item.ReportType = item.ReportTemplate ? "Template" : "Parameter";
                            item.AppendWith = Convert.ToInt16(DALHelper.HandleDBNull(reader4["AppendSampleNo"]));
                            item.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["RefundID"]));
                            item.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["CategoryID"]));
                            item.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["CategoryTypeID"]));
                            item.ThirdLevelCheckResult = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["ThirdLevelCheckResult"]));
                            item.CheckResultValueMessage = Convert.ToString(DALHelper.HandleDBNull(reader4["CheckResultValueMessage"]));
                            item.MsgCheckResultValForSecLevel = Convert.ToString(DALHelper.HandleDBNull(reader4["CheckResultValForSecLevel"]));
                            item.PathologistID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader4["PathologistID1"])));
                            item.TemplateResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["TemplateResultID"]));
                            item.TubeName = Convert.ToString(DALHelper.HandleDBNull(reader4["TubeName"]));
                            item.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader4["AgencyName"]));
                            item.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["DispatchToID"]));
                            item.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["IsExternalPatient"]));
                            item.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["BillID"]));
                            item.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["IsResendForNewSample"]));
                            item.IsSampleGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["IsSampleGenerated"]));
                            item.IsServiceRefunded = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["IsServiceRefunded"]));
                            item.ADateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader4["Date"])));
                            item.AgencyID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader4["ExtAgencyID"])));
                            nvo.OrderBookingDetailList.Add(item);
                        }
                    }
                    reader4.NextResult();
                    if (nvo.objOutsourceOrderBookingDetail == null)
                    {
                        nvo.objOutsourceOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
                    }
                    while (true)
                    {
                        if (!reader4.Read())
                        {
                            reader4.Close();
                            break;
                        }
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            OrderBookingID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["OrderID"])),
                            TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["TestID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["ServiceID"])),
                            AgencyID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader4["AgencyID"]))),
                            AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader4["AgencyName"]))
                        };
                        nvo.objOutsourceOrderBookingDetail.Add(item);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else if (nvo.IsFrom == "SampleCollection")
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathDetailListForCollection");
                    this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, nvo.OrderID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    if (nvo.CheckExtraCriteria)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "CheckExtraCriteria", DbType.Boolean, nvo.CheckExtraCriteria);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckSampleType", DbType.Boolean, nvo.CheckSampleType);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleType", DbType.Boolean, nvo.SampleType);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckUploadStatus", DbType.Boolean, nvo.CheckUploadStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "IsUploaded", DbType.Boolean, nvo.IsUploaded);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckDeliveryStatus", DbType.Boolean, nvo.CheckDeliveryStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.AgencyID);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.OrderDetail.SampleNo);
                        this.dbServer.AddInParameter(storedProcCommand, "IsExternalPatient", DbType.Boolean, nvo.OrderDetail.IsExternalPatient);
                        this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.OrderDetail.UserID);
                        this.dbServer.AddInParameter(storedProcCommand, "StatusID", DbType.Int64, nvo.StatusID);
                        this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                        this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    }
                    DbDataReader reader5 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader5.HasRows)
                    {
                        if (nvo.OrderBookingDetailList == null)
                        {
                            nvo.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                        }
                        while (reader5.Read())
                        {
                            clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                                ID = (long) DALHelper.HandleDBNull(reader5["ID"]),
                                OrderBookingID = (long) DALHelper.HandleDBNull(reader5["OrderID"]),
                                PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["PathPatientReportID"])),
                                UnitId = (long) DALHelper.HandleDBNull(reader5["UnitId"]),
                                DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["DispatchToID"])),
                                SampleCollected = (DateTime?) DALHelper.HandleDBNull(reader5["SampleCollected"]),
                                IsSampleCollected = (bool) DALHelper.HandleDBNull(reader5["IsSampleCollected"]),
                                IsCompleted = (bool) DALHelper.HandleDBNull(reader5["IsCompleted"]),
                                IsDelivered = (bool) DALHelper.HandleDBNull(reader5["IsDelivered"]),
                                TestID = (long) DALHelper.HandleDBNull(reader5["TestID"]),
                                TestCode = (string) DALHelper.HandleDBNull(reader5["TestCode"]),
                                TestName = (string) DALHelper.HandleDBNull(reader5["TestName"]),
                                ServiceName = (string) DALHelper.HandleDBNull(reader5["ServiceName"]),
                                ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["ServiceID"])),
                                SourceURL = (string) DALHelper.HandleDBNull(reader5["SourceURL"]),
                                Report = (byte[]) DALHelper.HandleDBNull(reader5["Report"]),
                                SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader5["SampleNo"])),
                                IsOutSourced = (bool) DALHelper.HandleDBNull(reader5["IsOutSourced"]),
                                Quantity = (double?) DALHelper.HandleDBNull(reader5["Quantity"]),
                                Status = (bool) DALHelper.HandleDBNull(reader5["Status"]),
                                UnitName = (string) DALHelper.HandleDBNull(reader5["UnitName"]),
                                IsResultEntry = (bool) DALHelper.HandleDBNull(reader5["IsResultEntry"]),
                                IsFinalized = (bool) DALHelper.HandleDBNull(reader5["IsFinalized"]),
                                HandDeliverdDateTime = (DateTime?) DALHelper.HandleDBNull(reader5["HandDeliverdDateTime"]),
                                IsDirectDelivered = (bool) DALHelper.HandleBoolDBNull(reader5["IsDirectDelivered"]),
                                EmailDeliverdDateTime = (DateTime?) DALHelper.HandleDBNull(reader5["EmailDeliverdDateTime"]),
                                IsDeliverdthroughEmail = (bool) DALHelper.HandleBoolDBNull(reader5["IsDeliverdthroughEmail"]),
                                SampleCollectedDateTime = (DateTime?) DALHelper.HandleDBNull(reader5["SampleCollectedDateTime"]),
                                IsSampleDispatch = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["IsSampleDispatched"])),
                                SampleDispatchDateTime = (DateTime?) DALHelper.HandleDBNull(reader5["SampleDispatchDateTime"]),
                                IsSampleReceive = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["IsSampleReceived"])),
                                SampleReceivedDateTime = (DateTime?) DALHelper.HandleDBNull(reader5["SampleReceivedDateTime"]),
                                SampleAcceptRejectStatus = Convert.ToByte(DALHelper.HandleDBNull(reader5["SampleAcceptRejectStatus"])),
                                SampleAcceptanceDateTime = (DateTime?) DALHelper.HandleDBNull(reader5["SampleAcceptDateTime"]),
                                SampleRejectionDateTime = (DateTime?) DALHelper.HandleDBNull(reader5["SampleRejectDateTime"]),
                                SampleRejectionRemark = Convert.ToString(DALHelper.HandleDBNull(reader5["RejectionRemark"])),
                                FirstLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["FirstLevel"])),
                                SecondLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["SecondLevel"])),
                                ThirdLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["ThirdLevel"])),
                                IsCheckedResults = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["IsCheckedResults"])),
                                ReportTemplate = (bool) DALHelper.HandleDBNull(reader5["ReportTemplate"]),
                                RefDoctorID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader5["RefDoctorID"])))
                            };
                            item.ReportType = item.ReportTemplate ? "Template" : "Parameter";
                            item.AppendWith = Convert.ToInt16(DALHelper.HandleDBNull(reader5["AppendSampleNo"]));
                            item.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["RefundID"]));
                            item.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["CategoryID"]));
                            item.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["CategoryTypeID"]));
                            item.ThirdLevelCheckResult = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["ThirdLevelCheckResult"]));
                            item.CheckResultValueMessage = Convert.ToString(DALHelper.HandleDBNull(reader5["CheckResultValueMessage"]));
                            item.MsgCheckResultValForSecLevel = Convert.ToString(DALHelper.HandleDBNull(reader5["CheckResultValForSecLevel"]));
                            item.PathologistID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader5["PathologistID1"])));
                            item.TemplateResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["TemplateResultID"]));
                            item.TubeName = Convert.ToString(DALHelper.HandleDBNull(reader5["TubeName"]));
                            item.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader5["AgencyName"]));
                            item.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["DispatchToID"]));
                            item.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["IsExternalPatient"]));
                            item.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["BillID"]));
                            item.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["IsResendForNewSample"]));
                            item.IsSampleGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["IsSampleGenerated"]));
                            item.IsServiceRefunded = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["IsServiceRefunded"]));
                            item.ADateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader5["Date"])));
                            nvo.OrderBookingDetailList.Add(item);
                        }
                    }
                    reader5.NextResult();
                    if (nvo.objOutsourceOrderBookingDetail == null)
                    {
                        nvo.objOutsourceOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
                    }
                    while (true)
                    {
                        if (!reader5.Read())
                        {
                            reader5.Close();
                            break;
                        }
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            OrderBookingID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["OrderID"])),
                            TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["TestID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["ServiceID"])),
                            AgencyID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader5["AgencyID"]))),
                            AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader5["AgencyName"]))
                        };
                        nvo.objOutsourceOrderBookingDetail.Add(item);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathOrderBookingDetailList");
                    this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, nvo.OrderID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "TestCategoryID", DbType.Int64, nvo.TestCategoryID);
                    this.dbServer.AddInParameter(storedProcCommand, "AuthenticationLevel", DbType.Int64, nvo.AuthenticationLevel);
                    if (nvo.CheckExtraCriteria)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromCollection", DbType.Boolean, nvo.IsFromCollection);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromReceive", DbType.Boolean, nvo.IsFromReceive);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromDispatch", DbType.Boolean, nvo.IsFromDispatch);
                        this.dbServer.AddInParameter(storedProcCommand, "SeacrhFromAcceptReject", DbType.Boolean, nvo.IsFromAcceptRejct);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromResult", DbType.Boolean, nvo.IsFromResult);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromAutoriation", DbType.Boolean, nvo.IsFromAuthorization);
                        this.dbServer.AddInParameter(storedProcCommand, "SearchFromUpload", DbType.Boolean, nvo.IsFromUpload);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckExtraCriteria", DbType.Boolean, nvo.CheckExtraCriteria);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckSampleType", DbType.Boolean, nvo.CheckSampleType);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleType", DbType.Boolean, nvo.SampleType);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckUploadStatus", DbType.Boolean, nvo.CheckUploadStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "IsUploaded", DbType.Boolean, nvo.IsUploaded);
                        this.dbServer.AddInParameter(storedProcCommand, "CheckDeliveryStatus", DbType.Boolean, nvo.CheckDeliveryStatus);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDelivered", DbType.Boolean, nvo.OrderDetail.IsDelivered);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDelivered1", DbType.Boolean, nvo.OrderDetail.IsDelivered1);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDeliverdthroughEmail", DbType.Boolean, nvo.OrderDetail.IsDeliverdthroughEmail);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDirectDelivered", DbType.Boolean, nvo.OrderDetail.IsDirectDelivered);
                        this.dbServer.AddInParameter(storedProcCommand, "IsResultEntry", DbType.Boolean, nvo.OrderDetail.IsResultEntry);
                        this.dbServer.AddInParameter(storedProcCommand, "IsResultEntry1", DbType.Boolean, nvo.OrderDetail.IsResultEntry1);
                        this.dbServer.AddInParameter(storedProcCommand, "IsBilled", DbType.Boolean, nvo.OrderDetail.IsBilled);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleCollected", DbType.Boolean, nvo.OrderDetail.IsSampleCollected);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleDispatched", DbType.Boolean, nvo.OrderDetail.IsSampleDispatch);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleReceived", DbType.Boolean, nvo.OrderDetail.IsSampleReceive);
                        this.dbServer.AddInParameter(storedProcCommand, "IsAccepted", DbType.Boolean, nvo.OrderDetail.IsAccepted);
                        this.dbServer.AddInParameter(storedProcCommand, "IsRejected", DbType.Boolean, nvo.OrderDetail.IsRejected);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSubOptimal", DbType.Boolean, nvo.OrderDetail.IsSubOptimal);
                        this.dbServer.AddInParameter(storedProcCommand, "IsOutSourced", DbType.Boolean, nvo.OrderDetail.IsOutSourced);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleCollectedBy", DbType.String, nvo.OrderDetail.SampleCollectedBy);
                        this.dbServer.AddInParameter(storedProcCommand, "DispatchBy", DbType.String, nvo.OrderDetail.DispatchBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AcceptedOrRejectedByName", DbType.String, nvo.OrderDetail.AcceptedOrRejectedByName);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleReceiveBy", DbType.String, nvo.OrderDetail.SampleReceiveBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.OrderDetail.AgencyID);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.OrderDetail.SampleNo);
                        this.dbServer.AddInParameter(storedProcCommand, "IsExternalPatient", DbType.Boolean, nvo.OrderDetail.IsExternalPatient);
                        this.dbServer.AddInParameter(storedProcCommand, "CatagoryID", DbType.Int64, nvo.OrderDetail.CategoryID);
                        this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                        this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    }
                    DbDataReader reader6 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader6.HasRows)
                    {
                        if (nvo.OrderBookingDetailList == null)
                        {
                            nvo.OrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
                        }
                        while (reader6.Read())
                        {
                            clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                                ID = (long) DALHelper.HandleDBNull(reader6["ID"]),
                                OrderBookingID = (long) DALHelper.HandleDBNull(reader6["OrderID"]),
                                PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["PathPatientReportID"])),
                                UnitId = (long) DALHelper.HandleDBNull(reader6["UnitId"]),
                                DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["DispatchToID"])),
                                SampleCollected = (DateTime?) DALHelper.HandleDBNull(reader6["SampleCollected"]),
                                IsSampleCollected = (bool) DALHelper.HandleDBNull(reader6["IsSampleCollected"]),
                                IsCompleted = (bool) DALHelper.HandleDBNull(reader6["IsCompleted"]),
                                IsDelivered = (bool) DALHelper.HandleDBNull(reader6["IsDelivered"]),
                                TestID = (long) DALHelper.HandleDBNull(reader6["TestID"]),
                                TestCode = (string) DALHelper.HandleDBNull(reader6["TestCode"]),
                                TestName = (string) DALHelper.HandleDBNull(reader6["TestName"]),
                                ServiceName = (string) DALHelper.HandleDBNull(reader6["ServiceName"]),
                                ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["ServiceID"])),
                                SourceURL = (string) DALHelper.HandleDBNull(reader6["SourceURL"]),
                                Report = (byte[]) DALHelper.HandleDBNull(reader6["Report"]),
                                SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader6["SampleNo"])),
                                IsOutSourced = (bool) DALHelper.HandleDBNull(reader6["IsOutSourced"]),
                                Quantity = (double?) DALHelper.HandleDBNull(reader6["Quantity"]),
                                Status = (bool) DALHelper.HandleDBNull(reader6["Status"]),
                                UnitName = (string) DALHelper.HandleDBNull(reader6["UnitName"]),
                                IsResultEntry = (bool) DALHelper.HandleDBNull(reader6["IsResultEntry"]),
                                IsFinalized = (bool) DALHelper.HandleDBNull(reader6["IsFinalized"]),
                                HandDeliverdDateTime = (DateTime?) DALHelper.HandleDBNull(reader6["HandDeliverdDateTime"]),
                                IsDirectDelivered = (bool) DALHelper.HandleBoolDBNull(reader6["IsDirectDelivered"]),
                                EmailDeliverdDateTime = (DateTime?) DALHelper.HandleDBNull(reader6["EmailDeliverdDateTime"]),
                                IsDeliverdthroughEmail = (bool) DALHelper.HandleBoolDBNull(reader6["IsDeliverdthroughEmail"]),
                                SampleCollectedDateTime = (DateTime?) DALHelper.HandleDBNull(reader6["SampleCollectedDateTime"]),
                                IsSampleDispatch = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["IsSampleDispatched"])),
                                SampleDispatchDateTime = (DateTime?) DALHelper.HandleDBNull(reader6["SampleDispatchDateTime"]),
                                IsSampleReceive = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["IsSampleReceived"])),
                                SampleReceivedDateTime = (DateTime?) DALHelper.HandleDBNull(reader6["SampleReceivedDateTime"]),
                                SampleAcceptRejectStatus = Convert.ToByte(DALHelper.HandleDBNull(reader6["SampleAcceptRejectStatus"])),
                                SampleAcceptanceDateTime = (DateTime?) DALHelper.HandleDBNull(reader6["SampleAcceptDateTime"]),
                                SampleRejectionDateTime = (DateTime?) DALHelper.HandleDBNull(reader6["SampleRejectDateTime"]),
                                SampleRejectionRemark = Convert.ToString(DALHelper.HandleDBNull(reader6["RejectionRemark"])),
                                FirstLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["FirstLevel"])),
                                SecondLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["SecondLevel"])),
                                ThirdLevel = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["ThirdLevel"])),
                                IsCheckedResults = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["IsCheckedResults"])),
                                ReportTemplate = (bool) DALHelper.HandleDBNull(reader6["ReportTemplate"]),
                                RefDoctorID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader6["RefDoctorID"])))
                            };
                            item.ReportType = item.ReportTemplate ? "Template" : "Parameter";
                            item.AppendWith = Convert.ToInt16(DALHelper.HandleDBNull(reader6["AppendSampleNo"]));
                            item.RefundID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["RefundID"]));
                            item.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["CategoryID"]));
                            item.CategoryTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["CategoryTypeID"]));
                            item.ThirdLevelCheckResult = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["ThirdLevelCheckResult"]));
                            item.CheckResultValueMessage = Convert.ToString(DALHelper.HandleDBNull(reader6["CheckResultValueMessage"]));
                            item.MsgCheckResultValForSecLevel = Convert.ToString(DALHelper.HandleDBNull(reader6["CheckResultValForSecLevel"]));
                            item.PathologistID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader6["PathologistID1"])));
                            item.TemplateResultID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["TemplateResultID"]));
                            item.TubeName = Convert.ToString(DALHelper.HandleDBNull(reader6["TubeName"]));
                            item.AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader6["AgencyName"]));
                            item.DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["DispatchToID"]));
                            item.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["IsExternalPatient"]));
                            item.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["BillID"]));
                            item.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["IsResendForNewSample"]));
                            item.IsSampleGenerated = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["IsSampleGenerated"]));
                            item.IsServiceRefunded = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["IsServiceRefunded"]));
                            item.ADateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader6["Date"])));
                            nvo.OrderBookingDetailList.Add(item);
                        }
                    }
                    reader6.NextResult();
                    if (nvo.objOutsourceOrderBookingDetail == null)
                    {
                        nvo.objOutsourceOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
                    }
                    while (true)
                    {
                        if (!reader6.Read())
                        {
                            reader6.Close();
                            break;
                        }
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            OrderBookingID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["OrderID"])),
                            TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["TestID"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["ServiceID"])),
                            AgencyID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader6["AgencyID"]))),
                            AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader6["AgencyName"]))
                        };
                        nvo.objOutsourceOrderBookingDetail.Add(item);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return valueObject;
        }

        public override IValueObject GetPathOrderBookingList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathOrderBookingListBizActionVO nvo = valueObject as clsGetPathOrderBookingListBizActionVO;
            if (nvo.IsFrom == "ResultEntry")
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathOrderListForResultEntry");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "CatagoryID", DbType.Int64, nvo.CatagoryID);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.SampleNo);
                    this.dbServer.AddInParameter(storedProcCommand, "AuthenticationLevel", DbType.Int64, nvo.AuthenticationLevel);
                    this.dbServer.AddInParameter(storedProcCommand, "CheckSampleType", DbType.Boolean, nvo.CheckSampleType);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleType", DbType.Boolean, nvo.SampleType);
                    this.dbServer.AddInParameter(storedProcCommand, "CheckUploadStatus", DbType.Boolean, nvo.CheckUploadStatus);
                    this.dbServer.AddInParameter(storedProcCommand, "IsUploaded", DbType.Boolean, nvo.IsUploaded);
                    this.dbServer.AddInParameter(storedProcCommand, "IsDelivered", DbType.Boolean, nvo.IsDelivered);
                    if (nvo.IsDispatchedClinic)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "IsDispatchedClinic", DbType.Int64, nvo.IsDispatchedClinic);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "IsDispatchedClinic", DbType.Int64, 0);
                    }
                    if ((nvo.MRNO != null) && (nvo.MRNO.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, "%" + nvo.MRNO + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, null);
                    }
                    if ((nvo.BillNo != null) && (nvo.BillNo.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, "%" + nvo.BillNo + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, null);
                    }
                    if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, "%" + nvo.FirstName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, null);
                    }
                    if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, "%" + nvo.LastName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, null);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "PatientType", DbType.Int64, nvo.PatientType);
                    this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                    this.dbServer.AddInParameter(storedProcCommand, "StatusID", DbType.Int64, nvo.StatusID);
                    this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.AgencyID);
                    this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.OrderBookingList == null)
                        {
                            nvo.OrderBookingList = new List<clsPathOrderBookingVO>();
                        }
                        while (reader.Read())
                        {
                            clsPathOrderBookingVO item = new clsPathOrderBookingVO {
                                ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                                OrderDate = new DateTime?((DateTime) DALHelper.HandleDBNull(reader["Date"])),
                                TestProfile = (long?) DALHelper.HandleDBNull(reader["TestType"]),
                                OrderNo = (string) DALHelper.HandleDBNull(reader["OrderNo"]),
                                BillNo = (string) DALHelper.HandleDBNull(reader["BillNo"]),
                                SampleType = (bool) DALHelper.HandleDBNull(reader["SampleType"]),
                                MRNo = (string) DALHelper.HandleDBNull(reader["MRNo"]),
                                PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                                PatientUnitID = DALHelper.HandleIntegerNull(reader["PatientUnitID"]),
                                FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["FirstName"])),
                                MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["MiddleName"])),
                                LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader["LastName"]))
                            };
                            item.UnitId = (long) DALHelper.HandleDBNull(reader["UnitId"]);
                            item.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                            item.UnitName = (string) DALHelper.HandleDBNull(reader["UnitName"]);
                            item.TotalAmount = (double) DALHelper.HandleDBNull(reader["TotalAmount"]);
                            item.CompanyAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyAmount"]));
                            item.PatientAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientAmount"]));
                            item.PaidAmount = (double) DALHelper.HandleDBNull(reader["PaidAmount"]);
                            item.Balance = (double) DALHelper.HandleDBNull(reader["BalanceAmount"]);
                            item.IsResultEntry = (bool) DALHelper.HandleDBNull(reader["IsResultEntry"]);
                            item.ReferredBy = (string) DALHelper.HandleDBNull(reader["DoctorName"]);
                            item.GenderID = DALHelper.HandleIntegerNull(reader["GenderID"]);
                            item.DateOfBirth = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["DateOfBirth"])));
                            item.ReferredByEmailID = Convert.ToString(DALHelper.HandleDBNull(reader["DoctorEmailID"]));
                            item.ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReferredDoctorID"]));
                            item.RegistrationTime = (DateTime?) DALHelper.HandleDBNull(reader["RegistrationTime"]);
                            item.PatientEmailId = Convert.ToString(DALHelper.HandleDBNull(reader["PatientEmailId"]));
                            item.DoctorID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorId"])));
                            item.IsIPDPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsIPDPatient"]));
                            item.PrefixID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PrefixID"]));
                            item.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader["Prefix"]));
                            item.AgeInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["AgeInDays"]));
                            item.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsExternalPatient"]));
                            item.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"]));
                            item.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillId"]));
                            item.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CID"]));
                            item.Opd_Ipd_External = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["IsIPD"])));
                            item.ResultColor = Convert.ToInt64(DALHelper.HandleDBNull(reader["RS"]));
                            nvo.OrderBookingList.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                    reader.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else if (nvo.IsFrom == "Authorization")
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathOrderListForAuthorization");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.SampleNo);
                    this.dbServer.AddInParameter(storedProcCommand, "CheckSampleType", DbType.Boolean, nvo.CheckSampleType);
                    this.dbServer.AddInParameter(storedProcCommand, "CheckUploadStatus", DbType.Boolean, nvo.CheckUploadStatus);
                    if ((nvo.MRNO != null) && (nvo.MRNO.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, "%" + nvo.MRNO + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, null);
                    }
                    if ((nvo.BillNo != null) && (nvo.BillNo.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, "%" + nvo.BillNo + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, null);
                    }
                    if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, "%" + nvo.FirstName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, null);
                    }
                    if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, "%" + nvo.LastName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, null);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "PatientType", DbType.Int64, nvo.PatientType);
                    this.dbServer.AddInParameter(storedProcCommand, "StatusID", DbType.Int64, nvo.StatusID);
                    this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.AgencyID);
                    this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.OrderBookingList == null)
                        {
                            nvo.OrderBookingList = new List<clsPathOrderBookingVO>();
                        }
                        while (reader2.Read())
                        {
                            clsPathOrderBookingVO item = new clsPathOrderBookingVO {
                                ID = (long) DALHelper.HandleDBNull(reader2["ID"]),
                                UnitId = (long) DALHelper.HandleDBNull(reader2["UnitID"]),
                                OrderDate = new DateTime?((DateTime) DALHelper.HandleDBNull(reader2["Date"])),
                                TestProfile = (long?) DALHelper.HandleDBNull(reader2["TestType"]),
                                OrderNo = (string) DALHelper.HandleDBNull(reader2["OrderNo"]),
                                BillNo = (string) DALHelper.HandleDBNull(reader2["BillNo"]),
                                SampleType = (bool) DALHelper.HandleDBNull(reader2["SampleType"]),
                                MRNo = (string) DALHelper.HandleDBNull(reader2["MRNo"]),
                                PatientID = (long) DALHelper.HandleDBNull(reader2["PatientID"]),
                                PatientUnitID = DALHelper.HandleIntegerNull(reader2["PatientUnitID"]),
                                FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader2["FirstName"])),
                                MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader2["MiddleName"])),
                                LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader2["LastName"]))
                            };
                            item.UnitId = (long) DALHelper.HandleDBNull(reader2["UnitId"]);
                            item.Status = (bool) DALHelper.HandleDBNull(reader2["Status"]);
                            item.UnitName = (string) DALHelper.HandleDBNull(reader2["UnitName"]);
                            item.TotalAmount = (double) DALHelper.HandleDBNull(reader2["TotalAmount"]);
                            item.CompanyAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader2["CompanyAmount"]));
                            item.PatientAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader2["PatientAmount"]));
                            item.PaidAmount = (double) DALHelper.HandleDBNull(reader2["PaidAmount"]);
                            item.Balance = (double) DALHelper.HandleDBNull(reader2["BalanceAmount"]);
                            item.IsResultEntry = (bool) DALHelper.HandleDBNull(reader2["IsResultEntry"]);
                            item.ReferredBy = (string) DALHelper.HandleDBNull(reader2["DoctorName"]);
                            item.GenderID = DALHelper.HandleIntegerNull(reader2["GenderID"]);
                            item.DateOfBirth = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader2["DateOfBirth"])));
                            item.ReferredByEmailID = Convert.ToString(DALHelper.HandleDBNull(reader2["DoctorEmailID"]));
                            item.ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ReferredDoctorID"]));
                            item.RegistrationTime = (DateTime?) DALHelper.HandleDBNull(reader2["RegistrationTime"]);
                            item.PatientEmailId = Convert.ToString(DALHelper.HandleDBNull(reader2["PatientEmailId"]));
                            item.DoctorID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader2["DoctorId"])));
                            item.IsIPDPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsIPDPatient"]));
                            item.PrefixID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["PrefixID"]));
                            item.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader2["Prefix"]));
                            item.AgeInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader2["AgeInDays"]));
                            item.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsExternalPatient"]));
                            item.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsResendForNewSample"]));
                            item.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["BillId"]));
                            item.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["CID"]));
                            item.Opd_Ipd_External = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader2["IsIPD"])));
                            item.ResultColor = Convert.ToInt64(DALHelper.HandleDBNull(reader2["AS1"]));
                            nvo.OrderBookingList.Add(item);
                        }
                    }
                    reader2.NextResult();
                    nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                    reader2.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else if (nvo.IsFrom == "Delivery")
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathOrderListForDelivery");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.SampleNo);
                    this.dbServer.AddInParameter(storedProcCommand, "CheckSampleType", DbType.Boolean, nvo.CheckSampleType);
                    this.dbServer.AddInParameter(storedProcCommand, "CheckUploadStatus", DbType.Boolean, nvo.CheckUploadStatus);
                    if ((nvo.MRNO != null) && (nvo.MRNO.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, "%" + nvo.MRNO + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, null);
                    }
                    if ((nvo.BillNo != null) && (nvo.BillNo.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, "%" + nvo.BillNo + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, null);
                    }
                    if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, "%" + nvo.FirstName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, null);
                    }
                    if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, "%" + nvo.LastName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, null);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "PatientType", DbType.Int64, nvo.PatientType);
                    this.dbServer.AddInParameter(storedProcCommand, "StatusID", DbType.Int64, nvo.StatusID);
                    this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                    this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.AgencyID);
                    this.dbServer.AddInParameter(storedProcCommand, "TypeID", DbType.Int64, nvo.TypeID);
                    this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader3 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader3.HasRows)
                    {
                        if (nvo.OrderBookingList == null)
                        {
                            nvo.OrderBookingList = new List<clsPathOrderBookingVO>();
                        }
                        while (reader3.Read())
                        {
                            clsPathOrderBookingVO item = new clsPathOrderBookingVO {
                                ID = (long) DALHelper.HandleDBNull(reader3["ID"]),
                                UnitId = (long) DALHelper.HandleDBNull(reader3["UnitID"]),
                                OrderDate = new DateTime?((DateTime) DALHelper.HandleDBNull(reader3["Date"])),
                                TestProfile = (long?) DALHelper.HandleDBNull(reader3["TestType"]),
                                OrderNo = (string) DALHelper.HandleDBNull(reader3["OrderNo"]),
                                BillNo = (string) DALHelper.HandleDBNull(reader3["BillNo"]),
                                SampleType = (bool) DALHelper.HandleDBNull(reader3["SampleType"]),
                                MRNo = (string) DALHelper.HandleDBNull(reader3["MRNo"]),
                                PatientID = (long) DALHelper.HandleDBNull(reader3["PatientID"]),
                                PatientUnitID = DALHelper.HandleIntegerNull(reader3["PatientUnitID"]),
                                FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader3["FirstName"])),
                                MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader3["MiddleName"])),
                                LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader3["LastName"]))
                            };
                            item.UnitId = (long) DALHelper.HandleDBNull(reader3["UnitId"]);
                            item.Status = (bool) DALHelper.HandleDBNull(reader3["Status"]);
                            item.UnitName = (string) DALHelper.HandleDBNull(reader3["UnitName"]);
                            item.TotalAmount = (double) DALHelper.HandleDBNull(reader3["TotalAmount"]);
                            item.CompanyAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader3["CompanyAmount"]));
                            item.PatientAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader3["PatientAmount"]));
                            item.PaidAmount = (double) DALHelper.HandleDBNull(reader3["PaidAmount"]);
                            item.Balance = (double) DALHelper.HandleDBNull(reader3["BalanceAmount"]);
                            item.IsResultEntry = (bool) DALHelper.HandleDBNull(reader3["IsResultEntry"]);
                            item.ReferredBy = (string) DALHelper.HandleDBNull(reader3["DoctorName"]);
                            item.GenderID = DALHelper.HandleIntegerNull(reader3["GenderID"]);
                            item.DateOfBirth = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader3["DateOfBirth"])));
                            item.ReferredByEmailID = Convert.ToString(DALHelper.HandleDBNull(reader3["DoctorEmailID"]));
                            item.ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["ReferredDoctorID"]));
                            item.RegistrationTime = (DateTime?) DALHelper.HandleDBNull(reader3["RegistrationTime"]);
                            item.PatientEmailId = Convert.ToString(DALHelper.HandleDBNull(reader3["PatientEmailId"]));
                            item.DoctorID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader3["DoctorId"])));
                            item.IsIPDPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsIPDPatient"]));
                            item.PrefixID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["PrefixID"]));
                            item.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader3["Prefix"]));
                            item.AgeInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader3["AgeInDays"]));
                            item.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsExternalPatient"]));
                            item.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader3["IsResendForNewSample"]));
                            item.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["BillId"]));
                            item.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader3["CID"]));
                            item.Opd_Ipd_External = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader3["IsIPD"])));
                            item.ResultColor = Convert.ToInt64(DALHelper.HandleDBNull(reader3["DS"]));
                            nvo.OrderBookingList.Add(item);
                        }
                    }
                    reader3.NextResult();
                    nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                    reader3.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else if (nvo.IsFrom == "UploadReport")
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathOrderListForUploadReport");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.SampleNo);
                    this.dbServer.AddInParameter(storedProcCommand, "CheckSampleType", DbType.Boolean, nvo.CheckSampleType);
                    this.dbServer.AddInParameter(storedProcCommand, "CheckUploadStatus", DbType.Boolean, nvo.CheckUploadStatus);
                    if ((nvo.MRNO != null) && (nvo.MRNO.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, "%" + nvo.MRNO + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, null);
                    }
                    if ((nvo.BillNo != null) && (nvo.BillNo.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, "%" + nvo.BillNo + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, null);
                    }
                    if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, "%" + nvo.FirstName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, null);
                    }
                    if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, "%" + nvo.LastName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, null);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "PatientType", DbType.Int64, nvo.PatientType);
                    this.dbServer.AddInParameter(storedProcCommand, "StatusID", DbType.Int64, nvo.StatusID);
                    this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.AgencyID);
                    this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader4 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader4.HasRows)
                    {
                        if (nvo.OrderBookingList == null)
                        {
                            nvo.OrderBookingList = new List<clsPathOrderBookingVO>();
                        }
                        while (reader4.Read())
                        {
                            clsPathOrderBookingVO item = new clsPathOrderBookingVO {
                                ID = (long) DALHelper.HandleDBNull(reader4["ID"]),
                                UnitId = (long) DALHelper.HandleDBNull(reader4["UnitID"]),
                                OrderDate = new DateTime?((DateTime) DALHelper.HandleDBNull(reader4["Date"])),
                                TestProfile = (long?) DALHelper.HandleDBNull(reader4["TestType"]),
                                OrderNo = (string) DALHelper.HandleDBNull(reader4["OrderNo"]),
                                BillNo = (string) DALHelper.HandleDBNull(reader4["BillNo"]),
                                SampleType = (bool) DALHelper.HandleDBNull(reader4["SampleType"]),
                                MRNo = (string) DALHelper.HandleDBNull(reader4["MRNo"]),
                                PatientID = (long) DALHelper.HandleDBNull(reader4["PatientID"]),
                                PatientUnitID = DALHelper.HandleIntegerNull(reader4["PatientUnitID"]),
                                FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader4["FirstName"])),
                                MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader4["MiddleName"])),
                                LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader4["LastName"]))
                            };
                            item.UnitId = (long) DALHelper.HandleDBNull(reader4["UnitId"]);
                            item.Status = (bool) DALHelper.HandleDBNull(reader4["Status"]);
                            item.UnitName = (string) DALHelper.HandleDBNull(reader4["UnitName"]);
                            item.TotalAmount = (double) DALHelper.HandleDBNull(reader4["TotalAmount"]);
                            item.CompanyAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader4["CompanyAmount"]));
                            item.PatientAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader4["PatientAmount"]));
                            item.PaidAmount = (double) DALHelper.HandleDBNull(reader4["PaidAmount"]);
                            item.Balance = (double) DALHelper.HandleDBNull(reader4["BalanceAmount"]);
                            item.IsResultEntry = (bool) DALHelper.HandleDBNull(reader4["IsResultEntry"]);
                            item.ReferredBy = (string) DALHelper.HandleDBNull(reader4["DoctorName"]);
                            item.GenderID = DALHelper.HandleIntegerNull(reader4["GenderID"]);
                            item.DateOfBirth = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader4["DateOfBirth"])));
                            item.ReferredByEmailID = Convert.ToString(DALHelper.HandleDBNull(reader4["DoctorEmailID"]));
                            item.ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["ReferredDoctorID"]));
                            item.RegistrationTime = (DateTime?) DALHelper.HandleDBNull(reader4["RegistrationTime"]);
                            item.PatientEmailId = Convert.ToString(DALHelper.HandleDBNull(reader4["PatientEmailId"]));
                            item.DoctorID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader4["DoctorId"])));
                            item.IsIPDPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["IsIPDPatient"]));
                            item.PrefixID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["PrefixID"]));
                            item.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader4["Prefix"]));
                            item.AgeInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader4["AgeInDays"]));
                            item.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["IsExternalPatient"]));
                            item.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader4["IsResendForNewSample"]));
                            item.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["BillId"]));
                            item.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader4["CID"]));
                            item.Opd_Ipd_External = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader4["IsIPD"])));
                            item.ResultColor = Convert.ToInt64(DALHelper.HandleDBNull(reader4["DS"]));
                            nvo.OrderBookingList.Add(item);
                        }
                    }
                    reader4.NextResult();
                    nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                    reader4.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else if (nvo.IsFrom == "SampleCollection")
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathOrderListForSampleCollection");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.SampleNo);
                    this.dbServer.AddInParameter(storedProcCommand, "CheckSampleType", DbType.Boolean, nvo.CheckSampleType);
                    this.dbServer.AddInParameter(storedProcCommand, "CheckUploadStatus", DbType.Boolean, nvo.CheckUploadStatus);
                    if ((nvo.MRNO != null) && (nvo.MRNO.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, "%" + nvo.MRNO + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, null);
                    }
                    if ((nvo.BillNo != null) && (nvo.BillNo.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, "%" + nvo.BillNo + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, null);
                    }
                    if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, "%" + nvo.FirstName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, null);
                    }
                    if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, "%" + nvo.LastName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, null);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "PatientType", DbType.Int64, nvo.PatientType);
                    this.dbServer.AddInParameter(storedProcCommand, "StatusID", DbType.Int64, nvo.StatusID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsSubOptimal", DbType.Boolean, nvo.IsSubOptimal);
                    this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                    this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.AgencyID);
                    this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader5 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader5.HasRows)
                    {
                        if (nvo.OrderBookingList == null)
                        {
                            nvo.OrderBookingList = new List<clsPathOrderBookingVO>();
                        }
                        while (reader5.Read())
                        {
                            clsPathOrderBookingVO item = new clsPathOrderBookingVO {
                                ID = (long) DALHelper.HandleDBNull(reader5["ID"]),
                                UnitId = (long) DALHelper.HandleDBNull(reader5["UnitID"]),
                                OrderDate = new DateTime?((DateTime) DALHelper.HandleDBNull(reader5["Date"])),
                                TestProfile = (long?) DALHelper.HandleDBNull(reader5["TestType"]),
                                OrderNo = (string) DALHelper.HandleDBNull(reader5["OrderNo"]),
                                BillNo = (string) DALHelper.HandleDBNull(reader5["BillNo"]),
                                SampleType = (bool) DALHelper.HandleDBNull(reader5["SampleType"]),
                                MRNo = (string) DALHelper.HandleDBNull(reader5["MRNo"]),
                                PatientID = (long) DALHelper.HandleDBNull(reader5["PatientID"]),
                                PatientUnitID = DALHelper.HandleIntegerNull(reader5["PatientUnitID"]),
                                FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader5["FirstName"])),
                                MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader5["MiddleName"])),
                                LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader5["LastName"]))
                            };
                            item.UnitId = (long) DALHelper.HandleDBNull(reader5["UnitId"]);
                            item.Status = (bool) DALHelper.HandleDBNull(reader5["Status"]);
                            item.UnitName = (string) DALHelper.HandleDBNull(reader5["UnitName"]);
                            item.TotalAmount = (double) DALHelper.HandleDBNull(reader5["TotalAmount"]);
                            item.CompanyAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader5["CompanyAmount"]));
                            item.PatientAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader5["PatientAmount"]));
                            item.PaidAmount = (double) DALHelper.HandleDBNull(reader5["PaidAmount"]);
                            item.Balance = (double) DALHelper.HandleDBNull(reader5["BalanceAmount"]);
                            item.IsResultEntry = (bool) DALHelper.HandleDBNull(reader5["IsResultEntry"]);
                            item.ReferredBy = (string) DALHelper.HandleDBNull(reader5["DoctorName"]);
                            item.GenderID = DALHelper.HandleIntegerNull(reader5["GenderID"]);
                            item.DateOfBirth = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader5["DateOfBirth"])));
                            item.ReferredByEmailID = Convert.ToString(DALHelper.HandleDBNull(reader5["DoctorEmailID"]));
                            item.ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["ReferredDoctorID"]));
                            item.RegistrationTime = (DateTime?) DALHelper.HandleDBNull(reader5["RegistrationTime"]);
                            item.PatientEmailId = Convert.ToString(DALHelper.HandleDBNull(reader5["PatientEmailId"]));
                            item.DoctorID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader5["DoctorId"])));
                            item.IsIPDPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["IsIPDPatient"]));
                            item.PrefixID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["PrefixID"]));
                            item.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader5["Prefix"]));
                            item.AgeInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader5["AgeInDays"]));
                            item.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["IsExternalPatient"]));
                            item.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader5["IsResendForNewSample"]));
                            item.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["BillId"]));
                            item.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader5["CID"]));
                            item.Opd_Ipd_External = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader5["IsIPD"])));
                            item.ResultColor = Convert.ToInt64(DALHelper.HandleDBNull(reader5["CS"]));
                            nvo.OrderBookingList.Add(item);
                        }
                    }
                    reader5.NextResult();
                    nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                    reader5.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathOrderBookingList2");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    if (nvo.IsDispatchedClinic)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "IsDispatchedClinic", DbType.Int64, nvo.IsDispatchedClinic);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "IsDispatchedClinic", DbType.Int64, 0);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "CatagoryID", DbType.Int64, nvo.CatagoryID);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.SampleNo);
                    this.dbServer.AddInParameter(storedProcCommand, "AuthenticationLevel", DbType.Int64, nvo.AuthenticationLevel);
                    this.dbServer.AddInParameter(storedProcCommand, "CheckSampleType", DbType.Boolean, nvo.CheckSampleType);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleType", DbType.Boolean, nvo.SampleType);
                    this.dbServer.AddInParameter(storedProcCommand, "CheckUploadStatus", DbType.Boolean, nvo.CheckUploadStatus);
                    this.dbServer.AddInParameter(storedProcCommand, "IsUploaded", DbType.Boolean, nvo.IsUploaded);
                    this.dbServer.AddInParameter(storedProcCommand, "IsDelivered", DbType.Boolean, nvo.IsDelivered);
                    if ((nvo.MRNO != null) && (nvo.MRNO.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, "%" + nvo.MRNO + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "MRNo", DbType.String, null);
                    }
                    if ((nvo.BillNo != null) && (nvo.BillNo.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, "%" + nvo.BillNo + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "BillNo", DbType.String, null);
                    }
                    if ((nvo.FirstName != null) && (nvo.FirstName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, "%" + nvo.FirstName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, null);
                    }
                    if ((nvo.LastName != null) && (nvo.LastName.Length != 0))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, "%" + nvo.LastName + "%");
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, null);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "PatientType", DbType.Int64, nvo.PatientType);
                    this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader6 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader6.HasRows)
                    {
                        if (nvo.OrderBookingList == null)
                        {
                            nvo.OrderBookingList = new List<clsPathOrderBookingVO>();
                        }
                        while (reader6.Read())
                        {
                            clsPathOrderBookingVO item = new clsPathOrderBookingVO {
                                ID = (long) DALHelper.HandleDBNull(reader6["ID"]),
                                OrderDate = new DateTime?((DateTime) DALHelper.HandleDBNull(reader6["Date"])),
                                TestProfile = (long?) DALHelper.HandleDBNull(reader6["TestType"]),
                                OrderNo = (string) DALHelper.HandleDBNull(reader6["OrderNo"]),
                                BillNo = (string) DALHelper.HandleDBNull(reader6["BillNo"]),
                                SampleType = (bool) DALHelper.HandleDBNull(reader6["SampleType"]),
                                MRNo = (string) DALHelper.HandleDBNull(reader6["MRNo"]),
                                PatientID = (long) DALHelper.HandleDBNull(reader6["PatientID"]),
                                PatientUnitID = DALHelper.HandleIntegerNull(reader6["PatientUnitID"]),
                                Prefix = (string) DALHelper.HandleDBNull(reader6["Pre"]),
                                FirstName = Security.base64Decode((string) DALHelper.HandleDBNull(reader6["FirstName"])),
                                MiddleName = Security.base64Decode((string) DALHelper.HandleDBNull(reader6["MiddleName"])),
                                LastName = Security.base64Decode((string) DALHelper.HandleDBNull(reader6["LastName"])),
                                UnitId = (long) DALHelper.HandleDBNull(reader6["UnitId"]),
                                Status = (bool) DALHelper.HandleDBNull(reader6["Status"]),
                                UnitName = (string) DALHelper.HandleDBNull(reader6["UnitName"]),
                                TotalAmount = (double) DALHelper.HandleDBNull(reader6["TotalAmount"]),
                                CompanyAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader6["CompanyAmount"])),
                                PatientAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader6["PatientAmount"])),
                                PaidAmount = (double) DALHelper.HandleDBNull(reader6["PaidAmount"]),
                                Balance = (double) DALHelper.HandleDBNull(reader6["BalanceAmount"]),
                                IsResultEntry = (bool) DALHelper.HandleDBNull(reader6["IsResultEntry"]),
                                ReferredBy = (string) DALHelper.HandleDBNull(reader6["DoctorName"]),
                                GenderID = DALHelper.HandleIntegerNull(reader6["GenderID"]),
                                DateOfBirth = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader6["DateOfBirth"]))),
                                ReferredByEmailID = Convert.ToString(DALHelper.HandleDBNull(reader6["DoctorEmailID"])),
                                ReferredDoctorID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["ReferredDoctorID"])),
                                RegistrationTime = (DateTime?) DALHelper.HandleDBNull(reader6["RegistrationTime"]),
                                PatientEmailId = Convert.ToString(DALHelper.HandleDBNull(reader6["PatientEmailId"])),
                                DoctorID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader6["DoctorId"]))),
                                IsIPDPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["IsIPDPatient"])),
                                PrefixID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["PrefixID"]))
                            };
                            item.Prefix = Convert.ToString(DALHelper.HandleDBNull(reader6["Prefix"]));
                            item.AgeInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader6["AgeInDays"]));
                            item.IsExternalPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["IsExternalPatient"]));
                            item.IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader6["IsResendForNewSample"]));
                            item.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["BillId"]));
                            item.PatientCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader6["CID"]));
                            item.Opd_Ipd_External = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader6["IsIPD"])));
                            nvo.OrderBookingList.Add(item);
                        }
                    }
                    reader6.NextResult();
                    nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                    reader6.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return valueObject;
        }

        public override IValueObject GetPathOrderBookingReportDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathOrderBookingDetailReportDetailsBizActionVO nvo = valueObject as clsGetPathOrderBookingDetailReportDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathOrderBookingReportDetail");
                this.dbServer.AddInParameter(storedProcCommand, "PathOrderBookingDetailsID", DbType.Int64, nvo.OrderBookingDetail.ID);
                this.dbServer.AddInParameter(storedProcCommand, "PathOrderBookingDetailsUnitID", DbType.Int64, nvo.OrderBookingDetail.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.OrderBookingDetailList == null)
                    {
                        nvo.OrderBookingDetail = new clsPathOrderBookingDetailVO();
                    }
                    while (reader.Read())
                    {
                        nvo.OrderBookingDetail.SourceURL = Convert.ToString(DALHelper.HandleDBNull(reader["SourceURL"]));
                        nvo.OrderBookingDetail.Report = (byte[]) DALHelper.HandleDBNull(reader["Report"]);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetPathoResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoResultEntryBizActionVO nvo = valueObject as clsGetPathoResultEntryBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoResultEntry");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "DetailID", DbType.Int64, nvo.DetailID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ResultEntryDetails == null)
                    {
                        nvo.ResultEntryDetails = new clsPathPatientReportVO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (nvo.ResultEntryDetails.TestList == null)
                            {
                                nvo.ResultEntryDetails.TestList = new List<clsPathoTestParameterVO>();
                            }
                            while (true)
                            {
                                if (!reader.Read())
                                {
                                    reader.NextResult();
                                    if (nvo.ResultEntryDetails.ItemList == null)
                                    {
                                        nvo.ResultEntryDetails.ItemList = new List<clsPathoTestItemDetailsVO>();
                                    }
                                    while (true)
                                    {
                                        if (!reader.Read())
                                        {
                                            reader.NextResult();
                                            if (nvo.ResultEntryDetails.TemplateDetails == null)
                                            {
                                                nvo.ResultEntryDetails.TemplateDetails = new clsPathoResultEntryTemplateVO();
                                            }
                                            while (true)
                                            {
                                                if (!reader.Read())
                                                {
                                                    reader.Close();
                                                    break;
                                                }
                                                nvo.ResultEntryDetails.TemplateDetails = new clsPathoResultEntryTemplateVO();
                                                nvo.ResultEntryDetails.TemplateDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                                                nvo.ResultEntryDetails.TemplateDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                                                nvo.ResultEntryDetails.TemplateDetails.OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                                                nvo.ResultEntryDetails.TemplateDetails.OrderDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderDetailID"]));
                                                nvo.ResultEntryDetails.TemplateDetails.PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"]));
                                                nvo.ResultEntryDetails.TemplateDetails.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                                                nvo.ResultEntryDetails.TemplateDetails.PathologistID = Convert.ToInt64(DALHelper.HandleDBNull(reader["Pathologist"]));
                                                nvo.ResultEntryDetails.TemplateDetails.Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
                                                nvo.ResultEntryDetails.TemplateDetails.TemplateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateID"]));
                                                nvo.ResultEntryDetails.TemplateDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                                            }
                                            break;
                                        }
                                        clsPathoTestItemDetailsVO svo = new clsPathoTestItemDetailsVO {
                                            TestID = (long) DALHelper.HandleDBNull(reader["TestID"]),
                                            ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                                            BatchID = (long) DALHelper.HandleDBNull(reader["BatchID"]),
                                            Quantity = (float) ((double) DALHelper.HandleDBNull(reader["IdealQuantity"])),
                                            ActualQantity = (float) ((double) DALHelper.HandleDBNull(reader["ActualQantity"])),
                                            ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                                            BatchCode = (string) DALHelper.HandleDBNull(reader["BatchCode"]),
                                            ExpiryDate = (DateTime?) DALHelper.HandleDBNull(reader["ExpiryDate"]),
                                            BalanceQuantity = (float) ((double) DALHelper.HandleDBNull(reader["BalQuantity"]))
                                        };
                                        nvo.ResultEntryDetails.ItemList.Add(svo);
                                    }
                                    break;
                                }
                                clsPathoTestParameterVO item = new clsPathoTestParameterVO {
                                    PathoTestID = (long) DALHelper.HandleDBNull(reader["TestID"]),
                                    PathoSubTestID = (long) DALHelper.HandleDBNull(reader["SubTestID"]),
                                    ParameterName = (string) DALHelper.HandleDBNull(reader["ParameterName"]),
                                    ParameterID = (long) DALHelper.HandleDBNull(reader["ParameterID"]),
                                    Category = (string) DALHelper.HandleDBNull(reader["Category"]),
                                    CategoryID = (long?) DALHelper.HandleDBNull(reader["CategoryID"]),
                                    ParameterUnit = (string) DALHelper.HandleDBNull(reader["ParameterUnit"]),
                                    Print = (string) DALHelper.HandleDBNull(reader["ParameterPrintName"]),
                                    ResultValue = (string) DALHelper.HandleDBNull(reader["ResultValue"]),
                                    DefaultValue = (string) DALHelper.HandleDBNull(reader["DefaultValue"]),
                                    NormalRange = (string) DALHelper.HandleDBNull(reader["NormalRange"]),
                                    Note = (string) DALHelper.HandleDBNull(reader["SuggetionNote"]),
                                    FootNote = (string) DALHelper.HandleDBNull(reader["FootNote"]),
                                    PathoSubTestName = (string) DALHelper.HandleDBNull(reader["SubTest"]),
                                    IsNumeric = (bool) DALHelper.HandleDBNull(reader["IsNumeric"]),
                                    ReferenceRange = (string) DALHelper.HandleDBNull(reader["ReferenceRange"])
                                };
                                nvo.ResultEntryDetails.TestList.Add(item);
                            }
                            break;
                        }
                        nvo.ResultEntryDetails.ID = (long) DALHelper.HandleDBNull(reader["PathPatientReportID"]);
                        nvo.ResultEntryDetails.UnitId = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.ResultEntryDetails.SampleNo = (string) DALHelper.HandleDBNull(reader["SampleNo"]);
                        nvo.ResultEntryDetails.SampleCollectionTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleCollectionTime"]);
                        nvo.ResultEntryDetails.PathologistID1 = (long) DALHelper.HandleDBNull(reader["PathologistID1"]);
                        nvo.ResultEntryDetails.ReferredBy = (string) DALHelper.HandleDBNull(reader["ReferredBy"]);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPathoResultEntryPrintDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsPathoResultEntryPrintDetailsBizActionVO nvo = valueObject as clsPathoResultEntryPrintDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_rpt_PathoTemplateResultEntry1");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "IsDelivered", DbType.Int64, nvo.IsDelivered);
                this.dbServer.AddInParameter(storedProcCommand, "IsOpdIpd", DbType.Int64, nvo.IsOpdIpd);
                this.dbServer.AddInParameter(storedProcCommand, "OrderUnitID", DbType.Int64, nvo.OrderUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ResultDetails == null)
                    {
                        nvo.ResultDetails = new clsPathoResultEntryPrintDetailsVO();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            while (true)
                            {
                                if (!reader.Read())
                                {
                                    int num = 0;
                                    while (true)
                                    {
                                        if (!reader.Read())
                                        {
                                            reader.Close();
                                            break;
                                        }
                                        num++;
                                        if (num == 1)
                                        {
                                            nvo.ResultDetails.Pathologist1 = Convert.ToString(DALHelper.HandleDBNull(reader["Pathologist"]));
                                            nvo.ResultDetails.Education1 = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                                            nvo.ResultDetails.PathoDoctorid1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathoDoctorid"]));
                                        }
                                        if (num == 2)
                                        {
                                            nvo.ResultDetails.Pathologist2 = Convert.ToString(DALHelper.HandleDBNull(reader["Pathologist"]));
                                            nvo.ResultDetails.Education2 = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                                            nvo.ResultDetails.PathoDoctorid2 = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathoDoctorid"]));
                                        }
                                        if (num == 3)
                                        {
                                            nvo.ResultDetails.Pathologist3 = Convert.ToString(DALHelper.HandleDBNull(reader["Pathologist"]));
                                            nvo.ResultDetails.Education3 = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                                            nvo.ResultDetails.PathoDoctorid3 = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathoDoctorid"]));
                                        }
                                        if (num == 4)
                                        {
                                            nvo.ResultDetails.Pathologist4 = Convert.ToString(DALHelper.HandleDBNull(reader["Pathologist"]));
                                            nvo.ResultDetails.Education4 = Convert.ToString(DALHelper.HandleDBNull(reader["Education"]));
                                            nvo.ResultDetails.PathoDoctorid4 = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathoDoctorid"]));
                                        }
                                    }
                                    break;
                                }
                                nvo.ResultDetails.UnitLogo = (byte[]) DALHelper.HandleDBNull(reader["Logo"]);
                                nvo.ResultDetails.DisclaimerImg = (byte[]) DALHelper.HandleDBNull(reader["DisImg"]);
                            }
                            break;
                        }
                        nvo.ResultDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.ResultDetails.Salutation = Convert.ToString(DALHelper.HandleDBNull(reader["Salutation"]));
                        nvo.ResultDetails.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        nvo.ResultDetails.ResultAddedDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ResultAddedTime"])));
                        nvo.ResultDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.ResultDetails.OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"]));
                        nvo.ResultDetails.OrderDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderDetailID"]));
                        nvo.ResultDetails.PathPatientReportID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PathPatientReportID"]));
                        nvo.ResultDetails.TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"]));
                        nvo.ResultDetails.Pathologist = Convert.ToInt64(DALHelper.HandleDBNull(reader["Pathologist"]));
                        nvo.ResultDetails.TestTemplate = Convert.ToString(DALHelper.HandleDBNull(reader["UpdatedTemplate"]));
                        nvo.ResultDetails.Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
                        nvo.ResultDetails.TemplateId = Convert.ToInt64(DALHelper.HandleDBNull(reader["TemplateId"]));
                        nvo.ResultDetails.Test = Convert.ToString(DALHelper.HandleDBNull(reader["Test"]));
                        nvo.ResultDetails.ReferredBy = Convert.ToString(DALHelper.HandleDBNull(reader["ReferredBy"]));
                        nvo.ResultDetails.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        nvo.ResultDetails.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"]));
                        nvo.ResultDetails.SampleCollectionTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["SampleCollectionTime"])));
                        nvo.ResultDetails.Gender = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]));
                        nvo.ResultDetails.AgeYear = Convert.ToInt32(DALHelper.HandleDBNull(reader["AgeYear"]));
                        nvo.ResultDetails.AgeMonth = Convert.ToInt32(DALHelper.HandleDBNull(reader["AgeMonth"]));
                        nvo.ResultDetails.AgeDate = Convert.ToInt32(DALHelper.HandleDBNull(reader["AgeDate"]));
                        nvo.ResultDetails.PathoCategory = Convert.ToString(DALHelper.HandleDBNull(reader["PathoCategory"]));
                        nvo.ResultDetails.Pathologist1 = Convert.ToString(DALHelper.HandleDBNull(reader["Pathologist1"]));
                        nvo.ResultDetails.Signature1 = (byte[]) DALHelper.HandleDBNull(reader["DigitalSignature"]);
                        nvo.ResultDetails.PathoDoctorid1 = Convert.ToInt64(DALHelper.HandleDBNull(reader["DoctorID"]));
                        nvo.ResultDetails.PatientCategory = Convert.ToString(DALHelper.HandleDBNull(reader["PatientCategory"]));
                        nvo.ResultDetails.PatientSource = Convert.ToString(DALHelper.HandleDBNull(reader["PatientSource"]));
                        nvo.ResultDetails.Company = Convert.ToString(DALHelper.HandleDBNull(reader["Company"]));
                        nvo.ResultDetails.ContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactNo"]));
                        nvo.ResultDetails.DonarCode = Convert.ToString(DALHelper.HandleDBNull(reader["DonarCode"]));
                        nvo.ResultDetails.ReferenceNo = Convert.ToString(DALHelper.HandleDBNull(reader["ReferenceNo"]));
                        nvo.ResultDetails.ApprovedDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["ApprovedDateTime"])));
                        nvo.ResultDetails.GeneratedDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["GeneratedDateTime"])));
                        nvo.ResultDetails.IsSubOptimal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSubOptimal"]));
                        nvo.ResultDetails.SubOptimalRemark = Convert.ToString(DALHelper.HandleDBNull(reader["SubOptimalRemark"]));
                        nvo.ResultDetails.Authorizedby = Convert.ToString(DALHelper.HandleDBNull(reader["Authorizedby"]));
                        nvo.ResultDetails.Disclaimer = Convert.ToString(DALHelper.HandleDBNull(reader["Disclaimer"]));
                        nvo.ResultDetails.UnitName = Convert.ToString(reader["ClinicName"]);
                        nvo.ResultDetails.AdressLine1 = Convert.ToString(reader["address"]);
                        nvo.ResultDetails.AddressLine2 = Convert.ToString(reader["AddressLine2"]);
                        nvo.ResultDetails.AddressLine3 = Convert.ToString(reader["AddressLine3"]);
                        nvo.ResultDetails.Email = Convert.ToString(reader["Email"]);
                        nvo.ResultDetails.PinCode = Convert.ToString(reader["PinCode"]);
                        nvo.ResultDetails.TinNo = Convert.ToString(reader["TinNo"]);
                        nvo.ResultDetails.RegNo = Convert.ToString(reader["RegNo"]);
                        nvo.ResultDetails.City = Convert.ToString(reader["City"]);
                        nvo.ResultDetails.UnitContactNo = Convert.ToString(reader["MobileNO"]);
                        nvo.ResultDetails.MobileNo = Convert.ToString(reader["MobNo"]);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPathoSubTestMasterDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoSubTestMasterBizActionVO nvo = valueObject as clsGetPathoSubTestMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoSubTestTestList");
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.TestList == null)
                    {
                        nvo.TestList = new List<clsPathoTestMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoTestMasterVO item = new clsPathoTestMasterVO {
                            ID = (long) reader["ID"],
                            UnitID = (long) reader["UnitID"],
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            TurnAroundTime = (double) DALHelper.HandleDBNull(reader["TurnAroundTime"]),
                            TubeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TubeID"])),
                            IsFormTemplate = Convert.ToInt16(DALHelper.HandleDBNull(reader["IsFormTemplate"]))
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable.Value;
                        item.CategoryID = (long) reader["CategoryID"];
                        item.ServiceID = (long) reader["ServiceID"];
                        item.TestPrintName = (string) DALHelper.HandleDBNull(reader["TestPrintName"]);
                        item.IsSubTest = (bool) DALHelper.HandleDBNull(reader["IsSubTest"]);
                        item.IsParameter = (bool) DALHelper.HandleDBNull(reader["IsParameter"]);
                        item.Note = (string) DALHelper.HandleDBNull(reader["Note"]);
                        item.HasNormalRange = (bool) DALHelper.HandleDBNull(reader["HasNormalRange"]);
                        item.HasObserved = (bool) DALHelper.HandleDBNull(reader["HasObserved"]);
                        item.PrintTestName = (bool) DALHelper.HandleDBNull(reader["PrintTestName"]);
                        item.Time = (DateTime) DALHelper.HandleDBNull(reader["Time"]);
                        item.NeedAuthorization = (bool) DALHelper.HandleDBNull(reader["NeedAuthorization"]);
                        item.FreeFormat = (bool) DALHelper.HandleDBNull(reader["FreeFormat"]);
                        item.MachineID = (long) DALHelper.HandleDBNull(reader["MachineID"]);
                        item.Technique = (string) DALHelper.HandleDBNull(reader["Technique"]);
                        item.FootNote = (string) DALHelper.HandleDBNull(reader["FootNote"]);
                        item.IsCultureSenTest = (bool) DALHelper.HandleDBNull(reader["IsCultureSenTest"]);
                        item.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        nvo.TestList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPathoTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTemplateMasterBizActionVO nvo = valueObject as clsGetPathoTemplateMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoTemplateList");
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Pathologist", DbType.Int64, nvo.Pathologist);
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, nvo.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.TemplateDetails == null)
                    {
                        nvo.TemplateDetails = new List<clsPathoTestTemplateDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoTestTemplateDetailsVO item = new clsPathoTestTemplateDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"])),
                            Pathologist = Convert.ToInt64(DALHelper.HandleDBNull(reader["Pathologist"])),
                            GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"])),
                            PathologistName = Convert.ToString(DALHelper.HandleDBNull(reader["PathologistName"])),
                            GenderName = Convert.ToString(DALHelper.HandleDBNull(reader["Gender"]))
                        };
                        nvo.TemplateDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch
            {
            }
            return nvo;
        }

        public override IValueObject GetPathoTestForresultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTestListForResultEntryBizActionVO nvo = valueObject as clsGetPathoTestListForResultEntryBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoTestListForResultEntry");
                this.dbServer.AddInParameter(storedProcCommand, "ApplicableTo", DbType.String, nvo.ApplicaleTo);
                this.dbServer.AddInParameter(storedProcCommand, "CategoryId", DbType.Int64, nvo.Category);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.TestList == null)
                    {
                        nvo.TestList = new List<clsPathoTestMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoTestMasterVO item = new clsPathoTestMasterVO {
                            ID = (long) reader["ID"],
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            TestPrintName = (string) DALHelper.HandleDBNull(reader["TestPrintName"])
                        };
                        nvo.TestList.Add(item);
                    }
                }
                reader.NextResult();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPathoTestForResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTestDetailsForResultEntryBizActionVO nvo = (clsGetPathoTestDetailsForResultEntryBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoTestDetailsForResultEntry");
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, nvo.TestID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    Func<clsPathoTestParameterVO, bool> predicate = null;
                    Func<clsPathoTestParameterVO, bool> func2 = null;
                    Func<clsPathoTestParameterVO, bool> func3 = null;
                    if (nvo.TestList == null)
                    {
                        nvo.TestList = new List<clsPathoTestParameterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            List<clsPathoTestParameterVO> list = new List<clsPathoTestParameterVO>();
                            DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetPathoTestParameterORPathoTestSubTestForResultEntry");
                            DbDataReader reader1 = null;
                            this.dbServer.AddInParameter(command2, "TestID", DbType.Int64, nvo.TestID);
                            this.dbServer.AddInParameter(command2, "IsParameter", DbType.Boolean, nvo.TestList[0].IsParameter);
                            this.dbServer.AddInParameter(command2, "IsNumeric", DbType.Boolean, nvo.TestList[0].IsNumeric);
                            reader1 = (DbDataReader) this.dbServer.ExecuteReader(command2);
                            while (true)
                            {
                                if (!reader1.Read())
                                {
                                    reader1.NextResult();
                                    if (nvo.TestList[0].IsParameter)
                                    {
                                        long paramSTID = 0L;
                                        while (true)
                                        {
                                            if (!reader1.Read())
                                            {
                                                reader1.NextResult();
                                                break;
                                            }
                                            if (nvo.TestList[0].IsNumeric)
                                            {
                                                clsPathoTestParameterVO item = new clsPathoTestParameterVO();
                                                foreach (clsPathoTestParameterVO rvo5 in nvo.TestList)
                                                {
                                                    if ((rvo5.PathoTestID == nvo.TestID) && (rvo5.ParamSTID == ((long) DALHelper.HandleDBNull(reader1["ID"]))))
                                                    {
                                                        item.PathoTestID = rvo5.PathoTestID;
                                                        item.PathoTestName = rvo5.PathoTestName;
                                                        item.ParamSTID = rvo5.ParamSTID;
                                                        item.Print = rvo5.Print;
                                                        item.Note = rvo5.Note;
                                                        item.FootNote = rvo5.FootNote;
                                                        item.IsParameter = rvo5.IsParameter;
                                                        item.IsNumeric = rvo5.IsNumeric;
                                                        item.ParameterUnit = rvo5.ParameterUnit;
                                                        if (!item.IsParameter)
                                                        {
                                                            item.PathoSubTestID = rvo5.ParamSTID;
                                                        }
                                                        item.ParameterName = rvo5.ParameterName;
                                                        item.PathoSubTestName = rvo5.PathoSubTestName;
                                                        item.MinValue = (float) ((double) DALHelper.HandleDBNull(reader1["MinValue"]));
                                                        item.MaxValue = (float) ((double) DALHelper.HandleDBNull(reader1["MaxValue"]));
                                                        item.NormalRange = (((double) DALHelper.HandleDBNull(reader1["MinValue"])) + " - " + ((double) DALHelper.HandleDBNull(reader1["MaxValue"]))).ToString();
                                                        item.DefaultValue = ((double) DALHelper.HandleDBNull(reader1["DefaultValue"])).ToString();
                                                        item.Category = (string) DALHelper.HandleDBNull(reader1["Category"]);
                                                        item.ReferenceRange = (((double) DALHelper.HandleDBNull(reader1["LowReffValue"])) + " - " + ((double) DALHelper.HandleDBNull(reader1["HighReffValue"]))).ToString();
                                                        list.Add(item);
                                                    }
                                                }
                                                continue;
                                            }
                                            clsPathoTestParameterVO rvo6 = new clsPathoTestParameterVO();
                                            foreach (clsPathoTestParameterVO rvo7 in nvo.TestList)
                                            {
                                                if ((rvo7.PathoTestID == nvo.TestID) && (rvo7.ParamSTID == ((long) DALHelper.HandleDBNull(reader1["ID"]))))
                                                {
                                                    if (rvo7.ParamSTID == paramSTID)
                                                    {
                                                        list.Clear();
                                                    }
                                                    rvo6.PathoTestID = rvo7.PathoTestID;
                                                    rvo6.PathoTestName = rvo7.PathoTestName;
                                                    rvo6.ParamSTID = rvo7.ParamSTID;
                                                    rvo6.Print = rvo7.Print;
                                                    rvo6.Note = rvo7.Note;
                                                    rvo6.FootNote = rvo7.FootNote;
                                                    rvo6.IsParameter = rvo7.IsParameter;
                                                    rvo6.IsNumeric = rvo7.IsNumeric;
                                                    rvo6.ParameterUnit = rvo7.ParameterUnit;
                                                    if (!rvo6.IsParameter)
                                                    {
                                                        rvo6.PathoSubTestID = rvo7.ParamSTID;
                                                    }
                                                    rvo6.ParameterName = rvo7.ParameterName;
                                                    rvo6.PathoSubTestName = rvo7.PathoSubTestName;
                                                    paramSTID = rvo7.ParamSTID;
                                                    list.Add(rvo6);
                                                    MasterListItem item = new MasterListItem {
                                                        Description = (string) DALHelper.HandleDBNull(reader1["HelpValue"]),
                                                        ID = (long) DALHelper.HandleDBNull(reader1["HelpValueID"]),
                                                        Status = (bool) DALHelper.HandleDBNull(reader1["IsDefault"])
                                                    };
                                                    nvo.HelpValueList.Add(item);
                                                }
                                            }
                                        }
                                    }
                                    if (!nvo.TestList[0].IsParameter)
                                    {
                                        while (true)
                                        {
                                            if (!reader1.Read())
                                            {
                                                reader1.NextResult();
                                                break;
                                            }
                                            if (func3 == null)
                                            {
                                                func3 = x => x.ParamSTID == ((long) DALHelper.HandleDBNull(reader1["ID"]));
                                            }
                                            foreach (clsPathoTestParameterVO rvo8 in nvo.TestList.Where<clsPathoTestParameterVO>(func3))
                                            {
                                                if (rvo8.PathoTestID == nvo.TestID)
                                                {
                                                    rvo8.ParameterName = (string) DALHelper.HandleDBNull(reader1["Description"]);
                                                    rvo8.IsNumeric = (bool) DALHelper.HandleDBNull(reader1["IsNumeric"]);
                                                }
                                            }
                                        }
                                    }
                                    if (!nvo.TestList[0].IsParameter)
                                    {
                                        while (reader1.Read())
                                        {
                                            clsPathoTestParameterVO item = new clsPathoTestParameterVO();
                                            foreach (clsPathoTestParameterVO rvo10 in nvo.TestList)
                                            {
                                                if ((rvo10.PathoTestID == nvo.TestID) && (rvo10.ParamSTID == ((long) DALHelper.HandleDBNull(reader1["ID"]))))
                                                {
                                                    item.PathoTestID = rvo10.PathoTestID;
                                                    item.PathoTestName = rvo10.PathoTestName;
                                                    item.ParamSTID = rvo10.ParamSTID;
                                                    item.Print = rvo10.Print;
                                                    item.Note = rvo10.Note;
                                                    item.FootNote = rvo10.FootNote;
                                                    item.IsParameter = rvo10.IsParameter;
                                                    item.IsNumeric = rvo10.IsNumeric;
                                                    item.ParameterUnit = rvo10.ParameterUnit;
                                                    if (!item.IsParameter)
                                                    {
                                                        item.PathoSubTestID = rvo10.ParamSTID;
                                                    }
                                                    item.ParameterName = rvo10.ParameterName;
                                                    item.PathoSubTestName = rvo10.PathoSubTestName;
                                                    item.MinValue = (float) ((double) DALHelper.HandleDBNull(reader1["MinValue"]));
                                                    item.MaxValue = (float) ((double) DALHelper.HandleDBNull(reader1["MaxValue"]));
                                                    item.NormalRange = (((double) DALHelper.HandleDBNull(reader1["MinValue"])) + " - " + ((double) DALHelper.HandleDBNull(reader1["MaxValue"]))).ToString();
                                                    item.DefaultValue = ((double) DALHelper.HandleDBNull(reader1["DefaultValue"])).ToString();
                                                    item.Category = (string) DALHelper.HandleDBNull(reader1["Category"]);
                                                    item.ReferenceRange = (((double) DALHelper.HandleDBNull(reader1["LowReffValue"])) + " - " + ((double) DALHelper.HandleDBNull(reader1["HighReffValue"]))).ToString();
                                                    list.Add(item);
                                                }
                                            }
                                        }
                                    }
                                    reader1.Close();
                                    nvo.TestList.Clear();
                                    foreach (clsPathoTestParameterVO rvo11 in list)
                                    {
                                        nvo.TestList.Add(rvo11);
                                    }
                                    break;
                                }
                                if (nvo.TestList[0].IsParameter)
                                {
                                    if (predicate == null)
                                    {
                                        predicate = x => x.ParamSTID == ((long) DALHelper.HandleDBNull(reader1["ID"]));
                                    }
                                    foreach (clsPathoTestParameterVO rvo2 in nvo.TestList.Where<clsPathoTestParameterVO>(predicate))
                                    {
                                        if ((rvo2.PathoTestID == nvo.TestID) && rvo2.IsParameter)
                                        {
                                            rvo2.ParameterName = (string) DALHelper.HandleDBNull(reader1["Description"]);
                                        }
                                    }
                                    continue;
                                }
                                if (func2 == null)
                                {
                                    func2 = x => x.ParamSTID == ((long) DALHelper.HandleDBNull(reader1["ID"]));
                                }
                                foreach (clsPathoTestParameterVO rvo3 in nvo.TestList.Where<clsPathoTestParameterVO>(func2))
                                {
                                    if ((rvo3.PathoTestID == nvo.TestID) && !rvo3.IsParameter)
                                    {
                                        rvo3.PathoSubTestName = (string) DALHelper.HandleDBNull(reader1["Description"]);
                                    }
                                }
                            }
                            break;
                        }
                        clsPathoTestParameterVO rvo = new clsPathoTestParameterVO {
                            PathoTestID = (long) DALHelper.HandleDBNull(reader["TestID"]),
                            PathoTestName = (string) DALHelper.HandleDBNull(reader["TestName"]),
                            ParamSTID = (long) DALHelper.HandleDBNull(reader["ParamSTID"]),
                            Print = (string) DALHelper.HandleDBNull(reader["PrintName"]),
                            Note = (string) DALHelper.HandleDBNull(reader["Note"]),
                            FootNote = (string) DALHelper.HandleDBNull(reader["FootNote"]),
                            IsParameter = (bool) DALHelper.HandleDBNull(reader["IsParameter"]),
                            IsNumeric = (bool) DALHelper.HandleDBNull(reader["IsNumeric"]),
                            ParameterUnit = (string) DALHelper.HandleDBNull(reader["ParameterUnit"])
                        };
                        if (!rvo.IsParameter)
                        {
                            rvo.PathoSubTestID = (long) DALHelper.HandleDBNull(reader["ParamSTID"]);
                        }
                        nvo.TestList.Add(rvo);
                    }
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetPathoTestItemList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTestItemDetailsBizActionVO nvo = (clsGetPathoTestItemDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoTestItemDetails");
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, nvo.TestID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsPathoTestItemDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoTestItemDetailsVO item = new clsPathoTestItemDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            TestID = (long) DALHelper.HandleDBNull(reader["TestID"]),
                            ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                            Quantity = (float) ((double) DALHelper.HandleDBNull(reader["Quantity"])),
                            ActualQantity = (float) ((double) DALHelper.HandleDBNull(reader["Quantity"])),
                            ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.ItemList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetPathoTestMasterDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTestDetailsBizActionVO nvo = valueObject as clsGetPathoTestDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoTestList");
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SortExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.TestList == null)
                    {
                        nvo.TestList = new List<clsPathoTestMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoTestMasterVO item = new clsPathoTestMasterVO {
                            ID = (long) reader["ID"],
                            UnitID = (long) reader["UnitID"],
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            TurnAroundTime = Convert.ToDouble(DALHelper.HandleDBNull(reader["TurnAroundTime"])),
                            TubeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TubeID"])),
                            IsFormTemplate = Convert.ToInt16(DALHelper.HandleDBNull(reader["IsFormTemplate"])),
                            IsAbnormal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAbnormal"]))
                        };
                        DateTime? nullable = DALHelper.HandleDate(reader["Date"]);
                        item.Date = nullable.Value;
                        item.CategoryID = (long) reader["CategoryID"];
                        item.ServiceID = (long) reader["ServiceID"];
                        item.TestPrintName = (string) DALHelper.HandleDBNull(reader["TestPrintName"]);
                        item.IsSubTest = (bool) DALHelper.HandleDBNull(reader["IsSubTest"]);
                        item.Applicableto = Convert.ToInt16(DALHelper.HandleDBNull(reader["Applicableto"]));
                        item.IsParameter = (bool) DALHelper.HandleDBNull(reader["IsParameter"]);
                        item.Note = (string) DALHelper.HandleDBNull(reader["Note"]);
                        item.HasNormalRange = (bool) DALHelper.HandleDBNull(reader["HasNormalRange"]);
                        item.HasObserved = (bool) DALHelper.HandleDBNull(reader["HasObserved"]);
                        item.PrintTestName = (bool) DALHelper.HandleDBNull(reader["PrintTestName"]);
                        item.Time = (DateTime) DALHelper.HandleDBNull(reader["Time"]);
                        item.NeedAuthorization = (bool) DALHelper.HandleDBNull(reader["NeedAuthorization"]);
                        item.FreeFormat = (bool) DALHelper.HandleDBNull(reader["FreeFormat"]);
                        item.MachineID = (long) DALHelper.HandleDBNull(reader["MachineID"]);
                        item.Technique = (string) DALHelper.HandleDBNull(reader["Technique"]);
                        item.FootNote = (string) DALHelper.HandleDBNull(reader["FootNote"]);
                        item.IsCultureSenTest = (bool) DALHelper.HandleDBNull(reader["IsCultureSenTest"]);
                        item.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                        item.ReportTemplate = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ReportTemplate"]));
                        nvo.TestList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPathoTestParameterAndSubTesrForResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTestParameterAndSubTestDetailsBizActionVO nvo = valueObject as clsGetPathoTestParameterAndSubTestDetailsBizActionVO;
            try
            {
                if (nvo.IsForResultEntryLog)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ResultEntryModificationLog");
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.ParameterDetails.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ParameterDetails.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "FirstName", DbType.String, nvo.ParameterDetails.FirstName);
                    this.dbServer.AddInParameter(storedProcCommand, "LastName", DbType.String, nvo.ParameterDetails.LastName);
                    this.dbServer.AddInParameter(storedProcCommand, "TestName", DbType.String, nvo.ParameterDetails.PathoTestName);
                    this.dbServer.AddInParameter(storedProcCommand, "MrNo", DbType.String, nvo.ParameterDetails.MrNo);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.TestList == null)
                        {
                            nvo.TestList = new List<clsPathoTestParameterVO>();
                        }
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                reader.NextResult();
                                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                                reader.Close();
                                break;
                            }
                            clsPathoTestParameterVO item = new clsPathoTestParameterVO {
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"])),
                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])),
                                LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])),
                                PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                                MrNo = Convert.ToString(DALHelper.HandleDBNull(reader["MrNo"])),
                                PathoTestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"])),
                                PathoTestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"])),
                                PathoSubTestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SubTestID"])),
                                ParameterName = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterName"])),
                                ParameterUnit = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterUnit"])),
                                ResultValue = Convert.ToString(DALHelper.HandleDBNull(reader["ResultValue"])),
                                DefaultValue = Convert.ToString(DALHelper.HandleDBNull(reader["DefaultValue"])),
                                IsNumeric = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsNumeric"])),
                                OrderDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["OrderDate"]))),
                                NormalRange = Convert.ToString(DALHelper.HandleDBNull(reader["NormalRange"])),
                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                                CreatedUnitID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["CreatedUnitID"]))),
                                UpdatedUnitID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["UpdatedUnitID"]))),
                                AddedBy = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["AddedBy"]))),
                                AddedOn = Convert.ToString(DALHelper.HandleDBNull(reader["AddedOn"])),
                                AddedDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["AddedDateTime"]))),
                                UpdatedDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["UpdatedDateTime"]))),
                                UpdatedBy = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["UpdatedBy"]))),
                                UpdatedOn = Convert.ToString(DALHelper.HandleDBNull(reader["UpdatedOn"])),
                                AddedWindowsLoginName = Convert.ToString(DALHelper.HandleDBNull(reader["AddedWindowsLoginName"])),
                                UpdateWindowsLoginName = Convert.ToString(DALHelper.HandleDBNull(reader["UpdateWindowsLoginName"])),
                                LoginName = Convert.ToString(DALHelper.HandleDBNull(reader["LoginName"])),
                                UserName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"])),
                                TestCategory = Convert.ToString(DALHelper.HandleDBNull(reader["CategoryName"])),
                                ReferenceRange = (string) DALHelper.HandleDBNull(reader["ReferenceRange"])
                            };
                            nvo.TestList.Add(item);
                        }
                    }
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoTestParameterAndSubTestDetailsListBySomnath");
                    this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.String, nvo.TestID);
                    this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, nvo.CategoryID);
                    this.dbServer.AddInParameter(storedProcCommand, "AgeInDays", DbType.Int64, nvo.AgeInDays);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.MultipleSampleNo);
                    this.dbServer.AddInParameter(storedProcCommand, "DetailID", DbType.String, nvo.DetailID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PatientId);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitId", DbType.Int64, nvo.PatientUnitId);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.TestList == null)
                        {
                            nvo.TestList = new List<clsPathoTestParameterVO>();
                        }
                        while (true)
                        {
                            if (!reader2.Read())
                            {
                                reader2.NextResult();
                                while (reader2.Read())
                                {
                                    nvo.Note = Convert.ToString(DALHelper.HandleDBNull(reader2["Note"]));
                                    nvo.FootNote = Convert.ToString(DALHelper.HandleDBNull(reader2["FootNote"]));
                                }
                                break;
                            }
                            clsPathoTestParameterVO item = new clsPathoTestParameterVO {
                                PathoTestID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader2["TestID"])),
                                PathoTestName = Convert.ToString(DALHelper.HandleDBNull(reader2["TestName"])),
                                PathoSubTestID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader2["SubTestID"])),
                                PathoSubTestName = Convert.ToString(DALHelper.HandleDBNull(reader2["SubTestDescription"])),
                                ParameterName = Convert.ToString(DALHelper.HandleDBNull(reader2["Parameter"])),
                                ParameterCode = Convert.ToString(DALHelper.HandleDBNull(reader2["ParameterCode"])),
                                ParameterID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader2["ParameterID"])),
                                ParameterUnit = Convert.ToString(DALHelper.HandleDBNull(reader2["Unit"])),
                                MinValue = (float) ((double) DALHelper.HandleDBNull(reader2["MinValue"])),
                                MaxValue = (float) ((double) DALHelper.HandleDBNull(reader2["MaxValue"])),
                                NormalRange = (((double) DALHelper.HandleDBNull(reader2["MinValue"])) + " - " + ((double) DALHelper.HandleDBNull(reader2["MaxValue"]))).ToString()
                            };
                            item.DefaultValue = ((double) DALHelper.HandleDBNull(reader2["DefaultValue"])).ToString();
                            item.ResultValue = Convert.ToString(DALHelper.HandleDBNull(reader2["ParameterValue"]));
                            item.Category = Convert.ToString(DALHelper.HandleDBNull(reader2["Category"]));
                            item.CategoryID = new long?(DALHelper.HandleIntegerNull(reader2["CategoryID"]));
                            item.IsNumeric = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsNumeric"]));
                            item.TestCategory = Convert.ToString(DALHelper.HandleDBNull(reader2["TestCategory"]));
                            item.TestCategoryID = (long?) DALHelper.HandleDBNull(reader2["TestCategoryID"]);
                            item.PreviousResultValue = Convert.ToString(DALHelper.HandleDBNull(reader2["PreviousResultValue"]));
                            item.IsReflexTesting = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsReflexTesting"]));
                            item.IsMachine = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsMachine"]));
                            item.DeltaCheckDefaultValue = (double) DALHelper.HandleDBNull(reader2["DeltaCheck"]);
                            item.MinImprobable = (double) DALHelper.HandleDBNull(reader2["MinImprobable"]);
                            item.MaxImprobable = (double) DALHelper.HandleDBNull(reader2["MaxImprobable"]);
                            item.HighReffValue = (double) DALHelper.HandleDBNull(reader2["HighReffValue"]);
                            item.LowReffValue = (double) DALHelper.HandleDBNull(reader2["LowReffValue"]);
                            item.UpperPanicValue = (double) DALHelper.HandleDBNull(reader2["UpperPanicValue"]);
                            item.LowerPanicValue = (double) DALHelper.HandleDBNull(reader2["LowerpanicValue"]);
                            item.LowReflex = (double) DALHelper.HandleDBNull(reader2["LowReflex"]);
                            item.HighReflex = (double) DALHelper.HandleDBNull(reader2["HighReflex"]);
                            item.ReferenceRange = (((double) DALHelper.HandleDBNull(reader2["LowReffValue"])) + " - " + ((double) DALHelper.HandleDBNull(reader2["HighReffValue"]))).ToString();
                            item.ParameterDefaultValueId = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ParameterDefaultValueId"]));
                            item.VaryingReferences = Convert.ToString(DALHelper.HandleDBNull(reader2["VaryingReferences"]));
                            item.FormulaID = Convert.ToString(DALHelper.HandleDBNull(reader2["FormulaID"]));
                            item.Formula = Convert.ToString(DALHelper.HandleDBNull(reader2["Formula"]));
                            item.HelpValue1 = Convert.ToString(DALHelper.HandleDBNull(reader2["HelpValue"]));
                            item.IsAbnoramal = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsAbnoramal"]));
                            item.SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader2["SampleNo"]));
                            item.TestAndSampleNO = Convert.ToString(DALHelper.HandleDBNull(reader2["TestAndSampleNO"]));
                            nvo.TestList.Add(item);
                        }
                    }
                    reader2.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetPathoTestResultEntry(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTestResultEntryBizActionVO nvo = valueObject as clsGetPathoTestResultEntryBizActionVO;
            try
            {
                DateTime date = DateTime.Now.Date;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoTestResultEntry");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.PatientID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PathoResultEntry == null)
                    {
                        nvo.PathoResultEntry = new List<clsPathoResultEntryVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoResultEntryVO item = new clsPathoResultEntryVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            CategoryID = (long) DALHelper.HandleDBNull(reader["CategoryId"]),
                            TestID = (long) DALHelper.HandleDBNull(reader["TestId"]),
                            ParameterID = (long) DALHelper.HandleDBNull(reader["ParameterId"]),
                            ParameterUnitID = (long) DALHelper.HandleDBNull(reader["ParameterUnitId"]),
                            LabID = (long) DALHelper.HandleDBNull(reader["LabId"])
                        };
                        DateTime time2 = (DateTime) DALHelper.HandleDBNull(reader["Date"]);
                        item.Date = time2.Date;
                        item.sDate = time2.Date.ToShortDateString();
                        item.Time = (DateTime) DALHelper.HandleDBNull(reader["Time"]);
                        item.ResultType = (long) DALHelper.HandleDBNull(reader["ResultType"]);
                        item.ResultValue = (string) DALHelper.HandleDBNull(reader["ResultValue"]);
                        item.Note = (string) DALHelper.HandleDBNull(reader["Notes"]);
                        item.ParameterName = (string) DALHelper.HandleDBNull(reader["ParameterName"]);
                        item.ParameterUnitName = (string) DALHelper.HandleDBNull(reader["ParameterUnitName"]);
                        item.LabName = (string) DALHelper.HandleDBNull(reader["LabName"]);
                        item.ResultTypeName = (string) DALHelper.HandleDBNull(reader["ResultTypeName"]);
                        item.Category = (string) DALHelper.HandleDBNull(reader["CategoryName"]);
                        item.Attachment = (byte[]) DALHelper.HandleDBNull(reader["Attachment"]);
                        item.AttachmentFileName = (string) DALHelper.HandleDBNull(reader["AttachmentFileName"]);
                        TimeSpan span = (TimeSpan) (date - time2);
                        long days = span.Days;
                        item.EllapsedTime = days + " Days";
                        nvo.PathoResultEntry.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPathoTestResultEntryDateWise(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoTestResultEntryDateWiseBizActionVO nvo = valueObject as clsGetPathoTestResultEntryDateWiseBizActionVO;
            try
            {
                DateTime date = DateTime.Now.Date;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoTestResultEntryDateWise");
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.Date, nvo.Date);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "fromform", DbType.Int64, nvo.fromform);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PathoResultEntry == null)
                    {
                        nvo.PathoResultEntry = new List<clsPathoResultEntryVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoResultEntryVO item = new clsPathoResultEntryVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            PatientID = (long) DALHelper.HandleDBNull(reader["PatientID"]),
                            CategoryID = (long) DALHelper.HandleDBNull(reader["CategoryId"]),
                            TestID = (long) DALHelper.HandleDBNull(reader["TestId"]),
                            ParameterID = (long) DALHelper.HandleDBNull(reader["ParameterId"]),
                            ParameterUnitID = (long) DALHelper.HandleDBNull(reader["ParameterUnitId"]),
                            LabID = (long) DALHelper.HandleDBNull(reader["LabId"])
                        };
                        DateTime time2 = (DateTime) DALHelper.HandleDBNull(reader["Date"]);
                        item.Date = time2.Date;
                        item.sDate = time2.Date.ToShortDateString();
                        item.Time = (DateTime) DALHelper.HandleDBNull(reader["Time"]);
                        item.ResultType = (long) DALHelper.HandleDBNull(reader["ResultType"]);
                        item.ResultValue = (string) DALHelper.HandleDBNull(reader["ResultValue"]);
                        item.Note = (string) DALHelper.HandleDBNull(reader["Notes"]);
                        item.ParameterName = (string) DALHelper.HandleDBNull(reader["ParameterName"]);
                        item.ParameterUnitName = (string) DALHelper.HandleDBNull(reader["ParameterUnitName"]);
                        item.LabName = (string) DALHelper.HandleDBNull(reader["LabName"]);
                        item.ResultTypeName = (string) DALHelper.HandleDBNull(reader["ResultTypeName"]);
                        item.Category = (string) DALHelper.HandleDBNull(reader["CategoryName"]);
                        item.Attachment = (byte[]) DALHelper.HandleDBNull(reader["Attachment"]);
                        item.AttachmentFileName = (string) DALHelper.HandleDBNull(reader["AttachmentFileName"]);
                        item.IsNumeric = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsNumeric"]));
                        TimeSpan span = (TimeSpan) (date - time2);
                        long days = span.Days;
                        item.EllapsedTime = days + " Days";
                        nvo.PathoResultEntry.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetPathoUnAssignedAgencyTestList(IValueObject valueObject, clsUserVO UserVO)
        {
            DbDataReader reader = null;
            clsGetPathoOutSourceTestListBizActionVO nvo = valueObject as clsGetPathoOutSourceTestListBizActionVO;
            clsPathoTestOutSourceDetailsVO pathoOutSourceTestDetails = nvo.PathoOutSourceTestDetails;
            clsPathoTestOutSourceDetailsVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetOrderBookingOutsourcingTestListForUnAssignedAgency");
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsPathoTestOutSourceDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"])),
                            OrderDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderDetailID"])),
                            FirstName = Convert.ToString(DALHelper.HandleDBNull(reader["FirstName"])),
                            LastName = Convert.ToString(DALHelper.HandleDBNull(reader["LastName"])),
                            MiddleName = Convert.ToString(DALHelper.HandleDBNull(reader["MiddleName"])),
                            MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MrNo"])),
                            OrderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["OrderID"])),
                            OrderNo = Convert.ToString(DALHelper.HandleDBNull(reader["OrderNo"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            OrderDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]))),
                            TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"])),
                            TestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"])),
                            IsOutSourced = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOutSourced"])),
                            IsOutSourced1 = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsOutSourced1"])),
                            AgencyID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtAgencyID"])),
                            AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"])),
                            BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                            BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"])),
                            IsChangedAgency = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsChangedAgency"])),
                            ReasonToChangeAgency = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyChangeReason"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]))
                        };
                        nvo.UnAssignedAgnecyTestList.Add(item);
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetPreviousRecordDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPreviousParameterValueBizActionVO nvo = valueObject as clsGetPreviousParameterValueBizActionVO;
            nvo.ParameterList = new List<clsGetPreviousParameterValueBizActionVO>();
            try
            {
                this.dbServer.CreateConnection().Open();
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPreviousResultForParameterID");
                this.dbServer.AddInParameter(storedProcCommand, "ParameterId", DbType.Int64, nvo.PathoTestParameter.ParameterID);
                this.dbServer.AddInParameter(storedProcCommand, "TestId", DbType.Int64, nvo.PathTestId.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.PathPatientDetail.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PathPatientDetail.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "mainId", DbType.Int64, nvo.PathPatientDetail.ID);
                this.dbServer.AddInParameter(storedProcCommand, "OrderDate", DbType.DateTime, nvo.PathPatientDetail.OrderDate);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                while (reader.Read())
                {
                    clsGetPreviousParameterValueBizActionVO item = new clsGetPreviousParameterValueBizActionVO {
                        ParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParameterID"])),
                        ParameterValue = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterName"])),
                        ResultValue = Convert.ToString(DALHelper.HandleDBNull(reader["ResultValue"])),
                        Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ADate"]))
                    };
                    nvo.ParameterList.Add(item);
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetReflexTestingDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetReflexTestingServiceParameterBizActionVO nvo = valueObject as clsGetReflexTestingServiceParameterBizActionVO;
            nvo.ServiceList = new List<clsGetReflexTestingServiceParameterBizActionVO>();
            try
            {
                this.dbServer.CreateConnection().Open();
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetReflexTextingParameters");
                this.dbServer.AddInParameter(storedProcCommand, "ParameterId", DbType.Int64, nvo.ParameterID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                while (reader.Read())
                {
                    clsGetReflexTestingServiceParameterBizActionVO item = new clsGetReflexTestingServiceParameterBizActionVO {
                        ParameterID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ParameterID"])),
                        ParameterName = Convert.ToString(DALHelper.HandleDBNull(reader["ParameterName"])),
                        ServiceName = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceName"])),
                        ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"]))
                    };
                    nvo.ServiceList.Add(item);
                }
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetResultOnParameterSelection(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetResultOnParameterSelectionBizActionVO nvo = valueObject as clsGetResultOnParameterSelectionBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetResultOnParameterSelection");
                this.dbServer.AddInParameter(storedProcCommand, "ParamID", DbType.Int64, nvo.ParamID);
                this.dbServer.AddInParameter(storedProcCommand, "Gender", DbType.String, nvo.Gender);
                this.dbServer.AddInParameter(storedProcCommand, "DOB", DbType.DateTime, nvo.DOB);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PathoResultEntry == null)
                    {
                        nvo.PathoResultEntry = new List<clsPathoResultEntryVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            reader.Close();
                            break;
                        }
                        clsPathoResultEntryVO item = new clsPathoResultEntryVO {
                            MinValue = (double) DALHelper.HandleDBNull(reader["MinValue"]),
                            MaxValue = (double) DALHelper.HandleDBNull(reader["MaxValue"]),
                            DefaultValue = (double) DALHelper.HandleDBNull(reader["DefaultValue"])
                        };
                        nvo.PathoResultEntry.Add(item);
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject GetServerDateTime(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetServerDateTimeBizActionVO nvo = valueObject as clsGetServerDateTimeBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetServerDateTime");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPathOrderBookingVO item = new clsPathOrderBookingVO();
                        nvo.ServerDateTime = (DateTime) reader["ServerDateTime"];
                        nvo.OrderBookingList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetTemplateListForTest(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetWordOrReportTemplateBizActionVO nvo = valueObject as clsGetWordOrReportTemplateBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTemplateByName");
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Flag", DbType.Int16, nvo.Flag);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.TemplateDetails == null)
                    {
                        nvo.TemplateDetails = new List<clsPathoTestTemplateDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathoTestTemplateDetailsVO item = new clsPathoTestTemplateDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.TemplateDetails.Add(item);
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetTestDetailsByTestID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathOrderTestDetailListBizActionVO nvo = valueObject as clsGetPathOrderTestDetailListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathTestDetailByTestID");
                this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, nvo.OrderID);
                this.dbServer.AddInParameter(storedProcCommand, "OrderDetailID", DbType.Int64, nvo.OrderDetailID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, nvo.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.SampleNo);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"])),
                            TestID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TestID"])),
                            TestName = Convert.ToString(DALHelper.HandleDBNull(reader["TestName"])),
                            SampleNo = Convert.ToString(DALHelper.HandleDBNull(reader["SampleNo"])),
                            UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"])),
                            SampleCollectedDateTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleCollectedDateTime"]),
                            SampleCollectedTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleCollectedDateTime"]),
                            SampleCollectionCenter = Convert.ToString(DALHelper.HandleDBNull(reader["SampleCollectionCenter"])),
                            CollectionCenter = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionCenter"])),
                            SampleCollectedBy = Convert.ToString(DALHelper.HandleDBNull(reader["SampleCollectedBy"])),
                            FastingStatusHrs = Convert.ToInt16(DALHelper.HandleDBNull(reader["FastingStatusHrs"])),
                            Gestation = Convert.ToString(DALHelper.HandleDBNull(reader["Gestation"])),
                            SampleReceivedDateTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleReceivedDateTime"]),
                            SampleReceivedTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleReceivedDateTime"]),
                            SampleReceiveBy = Convert.ToString(DALHelper.HandleDBNull(reader["SampleReceiveBy"])),
                            SampleDispatchDateTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleDispatchDateTime"]),
                            SampleDispatchTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleDispatchDateTime"]),
                            DispatchBy = Convert.ToString(DALHelper.HandleDBNull(reader["SampleDispatchBy"])),
                            DispatchToID = Convert.ToInt64(DALHelper.HandleDBNull(reader["DispatchToID"])),
                            DispatchToName = Convert.ToString(DALHelper.HandleDBNull(reader["DispatchToName"])),
                            SampleRejectionDateTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleRejectDateTime"]),
                            SampleRejectionTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleRejectDateTime"]),
                            SampleRejectionRemark = Convert.ToString(DALHelper.HandleDBNull(reader["RejectionRemark"])),
                            EmailDeliverdDateTime = (DateTime?) DALHelper.HandleDBNull(reader["EmailDeliverdDateTime"]),
                            HandDeliverdDateTime = (DateTime?) DALHelper.HandleDBNull(reader["HandDeliverdDateTime"]),
                            SampleAcceptanceDateTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleAcceptDateTime"]),
                            SampleAcceptanceTime = (DateTime?) DALHelper.HandleDBNull(reader["SampleAcceptDateTime"]),
                            AcceptedOrRejectedByName = Convert.ToString(DALHelper.HandleDBNull(reader["AcceptedOrRejectedByName"])),
                            AcceptedOrRejectedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AcceptedOrRejectedByID"])),
                            CollectionName = Convert.ToString(DALHelper.HandleDBNull(reader["CollectionName"])),
                            CollectionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CollectionID"])),
                            IsSubOptimal = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsSubOptimal"])),
                            IsResendForNewSample = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsResendForNewSample"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            AgencyID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["ExtAgencyID"]))),
                            AgencyName = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyName"]))
                        };
                        nvo.CollectionOrderBookingDetailList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathTestDetailOfResultEntryEditByTestID");
                this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, nvo.OrderID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, nvo.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.SampleNo);
                DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                    {
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            Reason = Convert.ToString(DALHelper.HandleDBNull(reader2["Reason"])),
                            DateTimeNow = (DateTime?) DALHelper.HandleDBNull(reader2["DateTimeNow"]),
                            UserName = Convert.ToString(DALHelper.HandleDBNull(reader2["UserName"]))
                        };
                        nvo.ReslutEntryEditList.Add(item);
                    }
                }
                reader2.Close();
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathTestDetailOfResultByTestID");
                this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, nvo.OrderID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, nvo.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.SampleNo);
                DbDataReader reader3 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader3.HasRows)
                {
                    while (reader3.Read())
                    {
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            ResultEntryBy = Convert.ToString(DALHelper.HandleDBNull(reader3["ResultEntryBy"])),
                            ResultDateTime = (DateTime?) DALHelper.HandleDBNull(reader3["ResultDateTime"])
                        };
                        nvo.ResultOrderBookingDetailList.Add(item);
                    }
                }
                reader3.Close();
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathTestDetailOfAuthorizationByTestID");
                this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, nvo.OrderID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TestID", DbType.Int64, nvo.TestID);
                this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, nvo.SampleNo);
                DbDataReader reader4 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader4.HasRows)
                {
                    while (reader4.Read())
                    {
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            ApproveBy = Convert.ToString(DALHelper.HandleDBNull(reader4["ApproveBy"])),
                            ADateTime = (DateTime?) DALHelper.HandleDBNull(reader4["ADateTime"])
                        };
                        nvo.AuthorizedOrderBookingDetailList.Add(item);
                    }
                }
                reader4.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetTestList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathTestDetailsBizActionVO nvo = valueObject as clsGetPathTestDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoTestListForBill");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.TestList == null)
                    {
                        nvo.TestList = new List<clsPathOrderBookingVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathOrderBookingVO item = new clsPathOrderBookingVO {
                            ID = (long) reader["ID"],
                            UnitId = (long) reader["UnitID"],
                            TestID = (long) reader["TestID"],
                            ServiceID = (long) reader["ServiceID"],
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.TestList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetTestListWithDetailsID(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            clsGetPathTestDetailsBizActionVO nvo = valueObject as clsGetPathTestDetailsBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = (pConnection == null) ? this.dbServer.CreateConnection() : pConnection;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                transaction = (pTransaction == null) ? connection.BeginTransaction() : pTransaction;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPathoTestListForBillWithPOBDID");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "POBID", DbType.Int64, nvo.pobID);
                this.dbServer.AddInParameter(storedProcCommand, "POBUnitID", DbType.Int64, nvo.pobUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ChargeID", DbType.Int64, nvo.pChargeID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand, transaction);
                if (reader.HasRows)
                {
                    if (nvo.TestList == null)
                    {
                        nvo.TestList = new List<clsPathOrderBookingVO>();
                    }
                    while (reader.Read())
                    {
                        clsPathOrderBookingVO item = new clsPathOrderBookingVO {
                            ID = Convert.ToInt64(reader["ID"]),
                            UnitId = Convert.ToInt64(reader["UnitID"]),
                            TestID = Convert.ToInt64(reader["TestID"]),
                            ServiceID = Convert.ToInt64(reader["ServiceID"]),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            POBDID = Convert.ToInt64(reader["POBDID"])
                        };
                        nvo.TestList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
            finally
            {
                if (pConnection == null)
                {
                    connection.Close();
                    connection = null;
                    transaction = null;
                }
            }
            return nvo;
        }

        public override IValueObject UpdateEmailDeliveryStatusinPathDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdatePathOrderBookingDetailDeliveryStatusBizActionVO nvo = valueObject as clsUpdatePathOrderBookingDetailDeliveryStatusBizActionVO;
            if (nvo.PathOrderBookList.Count > 0)
            {
                try
                {
                    List<clsPathOrderBookingDetailVO> pathOrderBookList = nvo.PathOrderBookList;
                    int count = pathOrderBookList.Count;
                    DbCommand storedProcCommand = null;
                    for (int i = 0; i < count; i++)
                    {
                        storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateEmailDeliveryStatusinPathDetails");
                        this.dbServer.AddInParameter(storedProcCommand, "PathOrderBookingDetailID", DbType.Int64, pathOrderBookList[i].ID);
                        this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, pathOrderBookList[i].OrderBookingID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, pathOrderBookList[i].UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "IsDeliverdthroughEmail", DbType.Boolean, pathOrderBookList[i].IsDeliverdthroughEmail);
                        this.dbServer.AddInParameter(storedProcCommand, "EmailDeliverdDateTime", DbType.DateTime, null);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    }
                }
                catch (Exception exception1)
                {
                    throw exception1;
                }
            }
            return nvo;
        }

        public override IValueObject UpdatePathOrderBookindDetail(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdatePathOrderBookingDetailBizActionVO nvo = valueObject as clsUpdatePathOrderBookingDetailBizActionVO;
            try
            {
                List<clsPathOrderBookingDetailVO> pathOrderBookList = nvo.PathOrderBookList;
                int count = pathOrderBookList.Count;
                DbCommand storedProcCommand = null;
                for (int i = 0; i < count; i++)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailForDirectDelievery");
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, pathOrderBookList[i].LinkServer);
                    if (pathOrderBookList[i].LinkServer != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, pathOrderBookList[i].LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, pathOrderBookList[i].ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, pathOrderBookList[i].UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitId", DbType.Int64, pathOrderBookList[i].UpdatedUnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, pathOrderBookList[i].UpdatedOn);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, pathOrderBookList[i].UpdatedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, pathOrderBookList[i].UpdatedWindowsLoginName);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
            }
            catch (Exception)
            {
            }
            return valueObject;
        }

        public override IValueObject UpdatePathOrderBookingDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            string str = "";
            clsUpdatePathOrderBookingDetailListBizActionVO nvo = valueObject as clsUpdatePathOrderBookingDetailListBizActionVO;
            bool flag = false;
            int count = nvo.OrderBookingDetailList.Count;
            try
            {
                DbCommand storedProcCommand = null;
                if (nvo.IsGenerateBatch)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBatchCode");
                    try
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, null);
                        this.dbServer.AddInParameter(storedProcCommand, "BatchNo", DbType.String, null);
                        this.dbServer.AddInParameter(storedProcCommand, "Dispatch", DbType.String, null);
                        this.dbServer.AddInParameter(storedProcCommand, "Unit", DbType.String, null);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UID);
                        this.dbServer.AddInParameter(storedProcCommand, "DispatchUnitID", DbType.Int64, nvo.DID);
                        str = Convert.ToString(this.dbServer.ExecuteScalar(storedProcCommand));
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        if (storedProcCommand != null)
                        {
                            storedProcCommand.Dispose();
                            storedProcCommand.Connection.Close();
                        }
                    }
                }
                goto TR_005D;
            TR_0009:
                str = string.Empty;
                goto TR_0003;
            TR_005D:
                using (List<clsPathOrderBookingDetailVO>.Enumerator enumerator = nvo.OrderBookingDetailList.GetEnumerator())
                {
                    clsPathOrderBookingDetailVO current;
                    goto TR_005B;
                TR_003D:
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailsForSampleAcceptReject");
                    try
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, current.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleAccepted", DbType.Boolean, current.IsSampleAccepted);
                        this.dbServer.AddInParameter(storedProcCommand, "SampleAcceptRejectStatus", DbType.Byte, current.SampleAcceptRejectStatus);
                        if (current.IsSampleAccepted)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "SampleAcceptDateTime", DbType.DateTime, current.SampleAcceptanceDateTime);
                        }
                        else
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "SampleRejectDateTime", DbType.DateTime, current.SampleRejectionDateTime);
                        }
                        this.dbServer.AddInParameter(storedProcCommand, "RejectionRemark", DbType.String, current.SampleRejectionRemark);
                        this.dbServer.AddInParameter(storedProcCommand, "AcceptedOrRejectedByID", DbType.String, current.AcceptedOrRejectedByID);
                        this.dbServer.AddInParameter(storedProcCommand, "AcceptedOrRejectedByName", DbType.String, current.AcceptedOrRejectedByName);
                        this.dbServer.AddInParameter(storedProcCommand, "IsAccepted", DbType.Boolean, current.IsAccepted);
                        this.dbServer.AddInParameter(storedProcCommand, "IsRejected", DbType.Boolean, current.IsRejected);
                        this.dbServer.AddInParameter(storedProcCommand, "IsSubOptimal", DbType.Boolean, current.IsSubOptimal);
                        this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, current.Remark);
                        this.dbServer.AddInParameter(storedProcCommand, "IsResendForNewSample", DbType.Boolean, current.IsResendForNewSample);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        if (storedProcCommand != null)
                        {
                            storedProcCommand.Dispose();
                            storedProcCommand.Connection.Close();
                        }
                    }
                TR_005B:
                    while (true)
                    {
                        if (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            if (current.IsFromSampleColletion || nvo.IsSampleGenerated)
                            {
                                flag = true;
                                try
                                {
                                    clsPathOrderBookingDetailVO orderBookingDetaildetails = nvo.OrderBookingDetaildetails;
                                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetail");
                                    this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, orderBookingDetaildetails.LinkServer);
                                    if (orderBookingDetaildetails.LinkServer != null)
                                    {
                                        this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, orderBookingDetaildetails.LinkServer.Replace(@"\", "_"));
                                    }
                                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, current.ID);
                                    if (nvo.IsSampleGenerated)
                                    {
                                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleGenerated", DbType.Boolean, nvo.IsSampleGenerated);
                                        this.dbServer.AddInParameter(storedProcCommand, "GenerateSampleFalseAtCollection", DbType.Boolean, true);
                                    }
                                    else
                                    {
                                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleGenerated", DbType.Boolean, true);
                                        this.dbServer.AddInParameter(storedProcCommand, "GenerateSampleFalseAtCollection", DbType.Boolean, false);
                                    }
                                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, current.SampleNo);
                                    this.dbServer.AddInParameter(storedProcCommand, "IsOutSourced", DbType.Boolean, current.IsOutSourced);
                                    this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, current.AgencyID);
                                    this.dbServer.AddInParameter(storedProcCommand, "Quantity", DbType.Double, current.Quantity);
                                    this.dbServer.AddInParameter(storedProcCommand, "SampleCollected", DbType.DateTime, nvo.SampleCollectionDate);
                                    if (nvo.IsSampleGenerated)
                                    {
                                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleCollected", DbType.Boolean, 0);
                                    }
                                    else
                                    {
                                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleCollected", DbType.Boolean, current.IsSampleCollected);
                                    }
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    this.dbServer.AddInParameter(storedProcCommand, "SampleCollectedDateTime", DbType.DateTime, current.SampleCollectedDateTime);
                                    this.dbServer.AddInParameter(storedProcCommand, "SampleCollectionCenter", DbType.String, current.SampleCollectionCenter);
                                    this.dbServer.AddInParameter(storedProcCommand, "FastingStatusID", DbType.Int64, current.FastingStatusID);
                                    this.dbServer.AddInParameter(storedProcCommand, "FastingStatusHrs", DbType.Int16, current.FastingStatusHrs);
                                    this.dbServer.AddInParameter(storedProcCommand, "CollectionID", DbType.Int64, current.CollectionID);
                                    this.dbServer.AddInParameter(storedProcCommand, "Gestation", DbType.String, current.Gestation);
                                    this.dbServer.AddInParameter(storedProcCommand, "FastingStatusName", DbType.String, current.FastingStatusName);
                                    this.dbServer.AddInParameter(storedProcCommand, "CollectionName", DbType.String, current.CollectionName);
                                    this.dbServer.AddInParameter(storedProcCommand, "CollectionCenter", DbType.String, current.CollectionCenter);
                                    this.dbServer.AddInParameter(storedProcCommand, "SampleCollectedBy", DbType.String, current.SampleCollectedBy);
                                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                                finally
                                {
                                    if (storedProcCommand != null)
                                    {
                                        storedProcCommand.Dispose();
                                        storedProcCommand.Connection.Close();
                                    }
                                }
                                continue;
                            }
                            if (current.IsFromSampleDispatch)
                            {
                                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailsForSampleDispatch");
                                try
                                {
                                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, current.ID);
                                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                                    this.dbServer.AddInParameter(storedProcCommand, "IsSampleDispatched", DbType.Boolean, current.IsSampleDispatch);
                                    this.dbServer.AddInParameter(storedProcCommand, "DispatchToID", DbType.Int64, current.DispatchToID);
                                    this.dbServer.AddInParameter(storedProcCommand, "DispatchToName", DbType.String, current.DispatchToName);
                                    this.dbServer.AddInParameter(storedProcCommand, "DispatchBy", DbType.String, current.DispatchBy);
                                    this.dbServer.AddInParameter(storedProcCommand, "BatchNo", DbType.String, str);
                                    this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, current.OrderID);
                                    this.dbServer.AddInParameter(storedProcCommand, "SampleDispatchDateTime", DbType.DateTime, current.SampleDispatchDateTime);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                                    nvo.returnBatch = str;
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                                finally
                                {
                                    if (storedProcCommand != null)
                                    {
                                        storedProcCommand.Dispose();
                                        storedProcCommand.Connection.Close();
                                    }
                                }
                                continue;
                            }
                            if (current.IsFromSampleReceive)
                            {
                                if (!nvo.IsExternalPatient)
                                {
                                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailsForSampleReceive");
                                    try
                                    {
                                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, current.ID);
                                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                                        this.dbServer.AddInParameter(storedProcCommand, "IsSampleReceived", DbType.Boolean, current.IsSampleReceive);
                                        this.dbServer.AddInParameter(storedProcCommand, "SampleReceivedDateTime", DbType.DateTime, current.SampleReceivedDateTime);
                                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                                        this.dbServer.AddInParameter(storedProcCommand, "SampleReceiveBy", DbType.String, current.SampleReceiveBy);
                                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                                    }
                                    catch (Exception)
                                    {
                                        throw;
                                    }
                                    finally
                                    {
                                        if (storedProcCommand != null)
                                        {
                                            storedProcCommand.Dispose();
                                            storedProcCommand.Connection.Close();
                                        }
                                    }
                                    continue;
                                }
                                flag = true;
                                try
                                {
                                    clsPathOrderBookingDetailVO orderBookingDetaildetails = nvo.OrderBookingDetaildetails;
                                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetail");
                                    this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, orderBookingDetaildetails.LinkServer);
                                    if (orderBookingDetaildetails.LinkServer != null)
                                    {
                                        this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, orderBookingDetaildetails.LinkServer.Replace(@"\", "_"));
                                    }
                                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, current.ID);
                                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, current.SampleNo);
                                    this.dbServer.AddInParameter(storedProcCommand, "IsOutSourced", DbType.Boolean, current.IsOutSourced);
                                    this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, current.AgencyID);
                                    this.dbServer.AddInParameter(storedProcCommand, "Quantity", DbType.Double, current.Quantity);
                                    this.dbServer.AddInParameter(storedProcCommand, "SampleCollected", DbType.DateTime, DateTime.Now);
                                    this.dbServer.AddInParameter(storedProcCommand, "IsSampleCollected", DbType.Boolean, true);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    this.dbServer.AddInParameter(storedProcCommand, "SampleCollectedDateTime", DbType.DateTime, DateTime.Now);
                                    this.dbServer.AddInParameter(storedProcCommand, "SampleCollectionCenter", DbType.String, current.SampleCollectionCenter);
                                    this.dbServer.AddInParameter(storedProcCommand, "FastingStatusID", DbType.Int64, current.FastingStatusID);
                                    this.dbServer.AddInParameter(storedProcCommand, "FastingStatusHrs", DbType.Int16, current.FastingStatusHrs);
                                    this.dbServer.AddInParameter(storedProcCommand, "CollectionID", DbType.Int64, current.CollectionID);
                                    this.dbServer.AddInParameter(storedProcCommand, "Gestation", DbType.String, current.Gestation);
                                    this.dbServer.AddInParameter(storedProcCommand, "FastingStatusName", DbType.String, current.FastingStatusName);
                                    this.dbServer.AddInParameter(storedProcCommand, "CollectionName", DbType.String, current.CollectionName);
                                    this.dbServer.AddInParameter(storedProcCommand, "CollectionCenter", DbType.String, current.CollectionCenter);
                                    this.dbServer.AddInParameter(storedProcCommand, "SampleCollectedBy", DbType.String, current.SampleCollectedBy);
                                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailsForSampleDispatch");
                                try
                                {
                                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, current.ID);
                                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                                    this.dbServer.AddInParameter(storedProcCommand, "IsSampleDispatched", DbType.Boolean, true);
                                    this.dbServer.AddInParameter(storedProcCommand, "DispatchToID", DbType.Int64, nvo.UnitID);
                                    this.dbServer.AddInParameter(storedProcCommand, "DispatchToName", DbType.String, current.DispatchToName);
                                    this.dbServer.AddInParameter(storedProcCommand, "DispatchBy", DbType.String, current.DispatchBy);
                                    this.dbServer.AddInParameter(storedProcCommand, "SampleDispatchDateTime", DbType.DateTime, DateTime.Now);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailsForSampleReceive");
                                try
                                {
                                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, current.ID);
                                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                                    this.dbServer.AddInParameter(storedProcCommand, "IsSampleReceived", DbType.Boolean, current.IsSampleReceive);
                                    this.dbServer.AddInParameter(storedProcCommand, "SampleReceivedDateTime", DbType.DateTime, current.SampleReceivedDateTime);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                                    this.dbServer.AddInParameter(storedProcCommand, "SampleReceiveBy", DbType.String, current.SampleReceiveBy);
                                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                                finally
                                {
                                    if (storedProcCommand != null)
                                    {
                                        storedProcCommand.Dispose();
                                        storedProcCommand.Connection.Close();
                                    }
                                }
                                continue;
                            }
                            if (!current.IsFromSampleAcceptReject)
                            {
                                continue;
                            }
                            storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBookingDetailsForSampleReceive");
                            try
                            {
                                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, current.ID);
                                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                                this.dbServer.AddInParameter(storedProcCommand, "IsSampleReceived", DbType.Boolean, current.IsSampleReceive);
                                this.dbServer.AddInParameter(storedProcCommand, "SampleReceivedDateTime", DbType.DateTime, current.SampleReceivedDateTime);
                                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                                this.dbServer.AddInParameter(storedProcCommand, "SampleReceiveBy", DbType.String, current.AcceptedOrRejectedByName);
                                this.dbServer.ExecuteNonQuery(storedProcCommand);
                                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                            finally
                            {
                                if (storedProcCommand != null)
                                {
                                    storedProcCommand.Dispose();
                                    storedProcCommand.Connection.Close();
                                }
                            }
                        }
                        else
                        {
                            goto TR_0009;
                        }
                        break;
                    }
                    goto TR_003D;
                }
            }
            catch (Exception)
            {
            }
        TR_0003:
            if (flag)
            {
                try
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePathOrderBooking");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.OrderID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
                catch (Exception)
                {
                }
            }
            return valueObject;
        }

        private clsAddPathPatientReportBizActionVO UpdateResult(clsAddPathPatientReportBizActionVO BizAction, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsPathOrderBookingDetailVO lvo1 = new clsPathOrderBookingDetailVO();
                BizAction.MasterList = new List<clsPathOrderBookingDetailVO>();
                foreach (clsPathPatientReportVO tvo in BizAction.OrderList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePathPatientReport");
                    this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.String, tvo.OrderID);
                    this.dbServer.AddInParameter(storedProcCommand, "PathOrderBookingDetailID", DbType.Int64, tvo.PathOrderBookingDetailID);
                    this.dbServer.AddInParameter(storedProcCommand, "PathologistID1", DbType.Int64, tvo.PathologistID1);
                    this.dbServer.AddInParameter(storedProcCommand, "RefDoctorID", DbType.Int64, tvo.RefDoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, tvo.ReferredBy);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFinalized ", DbType.Boolean, true);
                    this.dbServer.AddInParameter(storedProcCommand, "IsSecondLevel ", DbType.Boolean, tvo.IsSecondLevel);
                    this.dbServer.AddInParameter(storedProcCommand, "IsAutoApproved", DbType.Int64, tvo.IsAutoApproved);
                    this.dbServer.AddInParameter(storedProcCommand, "ApproveBy", DbType.String, tvo.ApproveBy);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, tvo.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.ID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    tvo.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    if ((BizAction.TestList != null) && (BizAction.TestList.Count > 0))
                    {
                        DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_DeletePathPatientParameterDetails");
                        this.dbServer.AddInParameter(command, "OrderID", DbType.Int64, tvo.OrderID);
                        this.dbServer.AddInParameter(command, "PathPatientReportID", DbType.Int64, tvo.ID);
                        this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.ExecuteNonQuery(command, transaction);
                        foreach (clsPathoTestParameterVO rvo in BizAction.TestList)
                        {
                            if ((tvo.TestID == rvo.PathoTestID) && (tvo.SampleNo == rvo.SampleNo))
                            {
                                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddPathPatientParameterDetails");
                                this.dbServer.AddInParameter(command3, "OrderID", DbType.Int64, tvo.OrderID);
                                this.dbServer.AddInParameter(command3, "PathPatientReportID", DbType.Int64, tvo.ID);
                                this.dbServer.AddInParameter(command3, "IsNumeric", DbType.Int64, rvo.IsNumeric);
                                this.dbServer.AddInParameter(command3, "TestID", DbType.Int64, rvo.PathoTestID);
                                this.dbServer.AddInParameter(command3, "ParameterID", DbType.Int64, rvo.ParameterID);
                                this.dbServer.AddInParameter(command3, "CategoryID", DbType.Int64, rvo.CategoryID);
                                this.dbServer.AddInParameter(command3, "Category", DbType.String, rvo.Category);
                                this.dbServer.AddInParameter(command3, "SubTestID", DbType.Int64, rvo.PathoSubTestID);
                                this.dbServer.AddInParameter(command3, "ParameterName", DbType.String, rvo.ParameterName);
                                this.dbServer.AddInParameter(command3, "ParameterUnit", DbType.String, rvo.ParameterUnit);
                                this.dbServer.AddInParameter(command3, "ParameterPrintName", DbType.String, rvo.Print);
                                this.dbServer.AddInParameter(command3, "ResultValue", DbType.String, rvo.ResultValue);
                                this.dbServer.AddInParameter(command3, "DefaultValue", DbType.String, rvo.DefaultValue);
                                this.dbServer.AddInParameter(command3, "NormalRange", DbType.String, rvo.NormalRange);
                                this.dbServer.AddInParameter(command3, "HelpValue", DbType.String, rvo.HelpValue);
                                if (rvo.Note != string.Empty)
                                {
                                    this.dbServer.AddInParameter(command3, "SuggetionNote", DbType.String, rvo.Note);
                                }
                                else
                                {
                                    this.dbServer.AddInParameter(command3, "SuggetionNote", DbType.String, null);
                                }
                                if (rvo.FootNote != string.Empty)
                                {
                                    this.dbServer.AddInParameter(command3, "FootNote", DbType.String, rvo.FootNote);
                                }
                                else
                                {
                                    this.dbServer.AddInParameter(command3, "FootNote", DbType.String, null);
                                }
                                this.dbServer.AddInParameter(command3, "IsFirstLevel", DbType.String, rvo.IsFirstLevel);
                                this.dbServer.AddInParameter(command3, "IsSecondLevel", DbType.String, rvo.IsSecondLevel);
                                this.dbServer.AddInParameter(command3, "SubTest", DbType.String, rvo.PathoSubTestName);
                                this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                                this.dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                                this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                                this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                this.dbServer.AddInParameter(command3, "ReferenceRange", DbType.String, rvo.ReferenceRange);
                                this.dbServer.AddInParameter(command3, "DeltaCheck", DbType.Double, rvo.DeltaCheckValue);
                                this.dbServer.AddInParameter(command3, "ParameterDefaultValueId", DbType.String, rvo.ParameterDefaultValueId);
                                this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo.ID);
                                this.dbServer.ExecuteNonQuery(command3, transaction);
                                rvo.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                            }
                        }
                        DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_CheckPathOrderBookingStatus");
                        this.dbServer.AddInParameter(command4, "OrderID", DbType.Int64, tvo.OrderID);
                        this.dbServer.AddInParameter(command4, "TestID", DbType.Int64, tvo.TestID);
                        this.dbServer.AddInParameter(command4, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddParameter(command4, "IsBalence", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.IsBalence);
                        this.dbServer.AddParameter(command4, "IsAbnormal", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, tvo.IsAbnormal);
                        this.dbServer.AddParameter(command4, "SampleNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "????????????????");
                        this.dbServer.ExecuteNonQuery(command4, transaction);
                        tvo.IsBalence = Convert.ToInt16(this.dbServer.GetParameterValue(command4, "IsBalence"));
                        tvo.IsAbnormal = Convert.ToInt16(this.dbServer.GetParameterValue(command4, "IsAbnormal"));
                        this.dbServer.GetParameterValue(command4, "SampleNo").ToString();
                        clsPathOrderBookingDetailVO item = new clsPathOrderBookingDetailVO {
                            TestID = tvo.TestID,
                            SampleNo = Convert.ToString(this.dbServer.GetParameterValue(command4, "SampleNo")),
                            Status = tvo.ResultStatus
                        };
                        BizAction.MasterList.Add(item);
                    }
                }
                if ((BizAction.OrderPathPatientReportList.TestID > 0L) && BizAction.OrderPathPatientReportList.ISTEMplate)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPathPatientReport");
                    this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, BizAction.OrderPathPatientReportList.OrderID);
                    this.dbServer.AddInParameter(storedProcCommand, "PathOrderBookingDetailID", DbType.Int64, BizAction.OrderPathPatientReportList.PathOrderBookingDetailID);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, BizAction.OrderPathPatientReportList.SampleNo);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleCollectionTime", DbType.DateTime, BizAction.OrderPathPatientReportList.SampleCollectionTime);
                    this.dbServer.AddInParameter(storedProcCommand, "PathologistID1", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID1);
                    this.dbServer.AddInParameter(storedProcCommand, "PathologistID2", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID2);
                    this.dbServer.AddInParameter(storedProcCommand, "PathologistID3", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID3);
                    this.dbServer.AddInParameter(storedProcCommand, "ReferredBy", DbType.String, BizAction.OrderPathPatientReportList.ReferredBy);
                    this.dbServer.AddInParameter(storedProcCommand, "RefDoctorID", DbType.Int64, BizAction.OrderPathPatientReportList.RefDoctorID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFinalized ", DbType.Boolean, BizAction.OrderPathPatientReportList.IsFinalized);
                    this.dbServer.AddInParameter(storedProcCommand, "IsFirstLevel", DbType.Boolean, BizAction.OrderPathPatientReportList.IsFirstLevel);
                    this.dbServer.AddInParameter(storedProcCommand, "IsSecondLevel", DbType.Boolean, BizAction.OrderPathPatientReportList.IsSecondLevel);
                    this.dbServer.AddInParameter(storedProcCommand, "IsThirdLevel", DbType.Boolean, BizAction.OrderPathPatientReportList.IsThirdLevel);
                    this.dbServer.AddInParameter(storedProcCommand, "ResultAddedDate ", DbType.DateTime, BizAction.OrderPathPatientReportList.ResultAddedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, BizAction.OrderPathPatientReportList.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.Name);
                    this.dbServer.AddInParameter(storedProcCommand, "DocAuthorizationID ", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "IsDoctorAuthorization ", DbType.Boolean, BizAction.OrderPathPatientReportList.IsDoctorAuthorization);
                    this.dbServer.AddInParameter(storedProcCommand, "ApproveBy", DbType.String, BizAction.OrderPathPatientReportList.ApproveByTemplate);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.OrderPathPatientReportList.ID);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    BizAction.OrderPathPatientReportList.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                    DbCommand command = this.dbServer.GetStoredProcCommand("CIMS_DeletePathPatientTemplateDetails");
                    this.dbServer.AddInParameter(command, "OrderID", DbType.Int64, BizAction.OrderPathPatientReportList.OrderID);
                    this.dbServer.AddInParameter(command, "PathPatientReportID", DbType.Int64, BizAction.OrderPathPatientReportList.ID);
                    this.dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.ExecuteNonQuery(command);
                    DbCommand command7 = this.dbServer.GetStoredProcCommand("CIMS_AddPathoResultEntryTemplate");
                    this.dbServer.AddInParameter(command7, "OrderID", DbType.Int64, BizAction.OrderPathPatientReportList.OrderID);
                    this.dbServer.AddInParameter(command7, "OrderDetailID", DbType.Int64, BizAction.OrderPathPatientReportList.PathOrderBookingDetailID);
                    this.dbServer.AddInParameter(command7, "PathPatientReportID", DbType.Int64, BizAction.OrderPathPatientReportList.ID);
                    this.dbServer.AddInParameter(command7, "TestID", DbType.Int64, BizAction.OrderPathPatientReportList.TestID);
                    this.dbServer.AddInParameter(command7, "Pathologist", DbType.Int64, BizAction.OrderPathPatientReportList.PathologistID1);
                    this.dbServer.AddInParameter(command7, "Template", DbType.String, BizAction.OrderPathPatientReportList.TemplateDetails.Template);
                    this.dbServer.AddInParameter(command7, "TemplateID", DbType.Int64, BizAction.OrderPathPatientReportList.TemplateDetails.TemplateID);
                    this.dbServer.AddInParameter(command7, "Status", DbType.Boolean, true);
                    if (BizAction.UnitID > 0L)
                    {
                        this.dbServer.AddInParameter(command7, "UnitId", DbType.Int64, BizAction.UnitID);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(command7, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }
                    this.dbServer.AddInParameter(command7, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command7, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command7, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(command7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(command7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizAction.OrderPathPatientReportList.TemplateDetails.ID);
                    this.dbServer.ExecuteNonQuery(command7, transaction);
                    BizAction.OrderPathPatientReportList.TemplateDetails.ID = (long) this.dbServer.GetParameterValue(command7, "ID");
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                BizAction.OrderList = null;
                BizAction.OrderPathPatientReportList = null;
                BizAction.TestList = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
                connection = null;
            }
            return BizAction;
        }

        public override IValueObject UpdateStatusMachineParameterMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateStatusMachineParameterBizActionVO nvo = valueObject as clsUpdateStatusMachineParameterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("UpdateStatusMachineParameter");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "MachineID", DbType.Int64, nvo.MachineID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateStatusParameterLinking(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateStatusParameterLinkingBizActionVO nvo = valueObject as clsUpdateStatusParameterLinkingBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("UpdateStatusParameterLinkingStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ApplicationParameterID", DbType.Int64, nvo.AppParameterID);
                this.dbServer.AddInParameter(storedProcCommand, "MachineParameterID", DbType.Int64, nvo.MachineParameterID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch
            {
                throw;
            }
            return nvo;
        }

        private clsAddPathoTemplateMasterBizActionVO UpdateTemplateDetails(clsAddPathoTemplateMasterBizActionVO BizActionObj, clsUserVO UserVo)
        {
            try
            {
                clsPathoTestTemplateDetailsVO templateDetails = BizActionObj.TemplateDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdatePathoTemplateMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, templateDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, templateDetails.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, templateDetails.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "PathologistName", DbType.String, templateDetails.PathologistName.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Template", DbType.String, templateDetails.Template.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Pathologist", DbType.Int64, templateDetails.Pathologist);
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, templateDetails.GenderID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = (long) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                List<MasterListItem> genderList = BizActionObj.TemplateDetails.GenderList;
                if ((genderList != null) || (genderList.Count > 0))
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteGenderToTemplate");
                    this.dbServer.AddInParameter(command2, "TemplateID", DbType.Int64, templateDetails.ID);
                    this.dbServer.ExecuteNonQuery(command2);
                    foreach (MasterListItem item in genderList)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddPathoGenderToTemplate");
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "GenderID", DbType.Int64, item.ID);
                        this.dbServer.AddInParameter(command3, "TemplateID", DbType.Int64, templateDetails.ID);
                        this.dbServer.AddInParameter(command3, "TemplateUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, item.Status);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, UserVo.ID);
                        this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int64, 0x7fffffff);
                        this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        this.dbServer.ExecuteNonQuery(command3);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject UploadReport(IValueObject valueObject, clsUserVO UserVo)
        {
            clsPathoUploadReportBizActionVO nvo = valueObject as clsPathoUploadReportBizActionVO;
            try
            {
                clsPathPatientReportVO uploadReportDetails = nvo.UploadReportDetails;
                if (nvo.IsResultEntry)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUploadReport");
                    this.dbServer.AddInParameter(storedProcCommand, "PathOrderBookingDetailID", DbType.Int64, uploadReportDetails.PathOrderBookingDetailID);
                    this.dbServer.AddInParameter(storedProcCommand, "PathPatientReportID", DbType.Int64, uploadReportDetails.PathPatientReportID);
                    this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, uploadReportDetails.SourceURL);
                    this.dbServer.AddInParameter(storedProcCommand, "Report", DbType.Binary, uploadReportDetails.Report);
                    this.dbServer.AddInParameter(storedProcCommand, "Notes", DbType.String, uploadReportDetails.Notes);
                    this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, uploadReportDetails.Remarks);
                    this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, uploadReportDetails.Time);
                    if (nvo.UnitID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, uploadReportDetails.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPathPatientReportWithOutResultEntry");
                    this.dbServer.AddInParameter(storedProcCommand, "SourceURL", DbType.String, uploadReportDetails.SourceURL);
                    this.dbServer.AddInParameter(storedProcCommand, "PathOrderBookingDetailID", DbType.Int64, uploadReportDetails.PathOrderBookingDetailID);
                    this.dbServer.AddInParameter(storedProcCommand, "OrderID", DbType.Int64, uploadReportDetails.OrderID);
                    this.dbServer.AddInParameter(storedProcCommand, "SampleNo", DbType.String, uploadReportDetails.SampleNo);
                    this.dbServer.AddInParameter(storedProcCommand, "Report", DbType.Binary, uploadReportDetails.Report);
                    this.dbServer.AddInParameter(storedProcCommand, "Notes", DbType.String, uploadReportDetails.Notes);
                    this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, uploadReportDetails.Remarks);
                    this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, uploadReportDetails.Time);
                    if (nvo.UnitID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, nvo.UnitID);
                    }
                    else
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, uploadReportDetails.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.UploadReportDetails.ServiceID);
                    this.dbServer.AddInParameter(storedProcCommand, "AgencyID", DbType.Int64, nvo.UploadReportDetails.AgencyID);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, uploadReportDetails.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    nvo.UploadReportDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject ViewPathoTemplate(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPathoViewTemplateBizActionVO nvo = (clsGetPathoViewTemplateBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ViewPathoTemplate");
                this.dbServer.AddInParameter(storedProcCommand, "TemplateID", DbType.Int64, nvo.TemplateID);
                this.dbServer.AddInParameter(storedProcCommand, "Flag", DbType.Int64, nvo.Flag);
                this.dbServer.AddInParameter(storedProcCommand, "GenderID", DbType.Int64, nvo.GenderID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.Template == null)
                    {
                        nvo.Template = new clsPathoTestTemplateDetailsVO();
                    }
                    while (reader.Read())
                    {
                        nvo.Template.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.Template.Template = Convert.ToString(DALHelper.HandleDBNull(reader["Template"]));
                        nvo.Template.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

