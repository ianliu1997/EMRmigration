namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.CustomExceptions;
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Administration;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Administration.IPD;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class clsIPDConfigDAL : clsBaseIPDConfigDAL
    {
        private Database dbServer;
        private LogManager logManager;

        public clsIPDConfigDAL()
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

        public override IValueObject AddAdmissionTypeServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIPDAddAdmissionTypeServiceListBizActionVO nvo = (clsIPDAddAdmissionTypeServiceListBizActionVO) valueObject;
            nvo.DoctorServiceDetails = new clsIPDAdmissionTypeServiceLinkVO();
            try
            {
                foreach (clsServiceMasterVO rvo in nvo.DoctorServiceList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddAdmissionTypeServiceDetail");
                    this.dbServer.AddInParameter(storedProcCommand, "AdmissionTypeID", DbType.Int64, rvo.AdmissionTypeID);
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, rvo.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, rvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objUserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objUserVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objUserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, rvo.AddedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objUserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.DoctorServiceDetails.ID);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        private clsIPDAddUpdateDietPlanBizactionVO AddDietPlan(clsIPDAddUpdateDietPlanBizactionVO objItem, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsIPDDietPlanMasterVO objDietMatserDetails = new clsIPDDietPlanMasterVO();
                objDietMatserDetails = objItem.objDietMatserDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddDietPlanMaster");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objDietMatserDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, objDietMatserDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, objDietMatserDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "PlanInfo", DbType.String, objDietMatserDetails.PlanInst);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objDietMatserDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objDietMatserDetails.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objDietMatserDetails.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objDietMatserDetails.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, objDietMatserDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objDietMatserDetails.UpdateWindowsLoginName);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDietMatserDetails.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                objItem.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                objItem.objDietMatserDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                if (objItem.SuccessStatus == 1)
                {
                    if ((objDietMatserDetails.ItemList != null) && (objDietMatserDetails.ItemList.Count > 0))
                    {
                        foreach (clsIPDDietPlanItemMasterVO rvo2 in objDietMatserDetails.ItemList)
                        {
                            DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddDietPlanDetails");
                            this.dbServer.AddInParameter(command2, "PlanID", DbType.Int64, objDietMatserDetails.ID);
                            this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objDietMatserDetails.UnitID);
                            this.dbServer.AddInParameter(command2, "ItemId", DbType.Int64, rvo2.FoodItem.ID);
                            this.dbServer.AddInParameter(command2, "Timing", DbType.String, rvo2.Timing);
                            this.dbServer.AddInParameter(command2, "ItemQty", DbType.String, rvo2.ItemQty);
                            this.dbServer.AddInParameter(command2, "ItemUnit", DbType.String, rvo2.ItemUnit);
                            this.dbServer.AddInParameter(command2, "ItemCal", DbType.String, rvo2.ItemCalories);
                            this.dbServer.AddInParameter(command2, "ItemCH", DbType.String, rvo2.ItemCh);
                            this.dbServer.AddInParameter(command2, "ItemFat", DbType.String, rvo2.ItemFat);
                            this.dbServer.AddInParameter(command2, "ItemExpectedCal", DbType.String, rvo2.ExpectedCal);
                            this.dbServer.AddInParameter(command2, "ItemInst", DbType.String, rvo2.ItemInst);
                            this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                            this.dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo2.ID);
                            this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command2, transaction);
                            rvo2.ID = (long) this.dbServer.GetParameterValue(command2, "ID");
                        }
                    }
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                objItem.objDietMatserDetails = null;
            }
            finally
            {
                transaction = null;
                connection.Close();
                connection = null;
            }
            return objItem;
        }

        public override IValueObject AddUpdateAdmissionTypeMaster(IValueObject valueObject, clsUserVO UserVO)
        {
            clsIPDAdmissionTypeVO admissionTypeMasterDetails = new clsIPDAdmissionTypeVO();
            clsIPDAddUpdateAdmissionTypeMasterBizActionVO nvo = valueObject as clsIPDAddUpdateAdmissionTypeMasterBizActionVO;
            try
            {
                admissionTypeMasterDetails = nvo.AdmissionTypeMasterDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateAdmissionTypeMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, admissionTypeMasterDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, admissionTypeMasterDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, admissionTypeMasterDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, admissionTypeMasterDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, admissionTypeMasterDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateAdmissionTypeServiceList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsIPDAddUpdateAdmissionTypeServiceListBizActionVO nvo = valueObject as clsIPDAddUpdateAdmissionTypeServiceListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpadateAdmissionTypeServiceStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.DoctorServiceDetails.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionTypeID", DbType.Int64, nvo.DoctorServiceDetails.AdmissionTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.DoctorServiceDetails.Status);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateBedMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDBedMasterVO objBedMatserDetails = new clsIPDBedMasterVO();
            clsIPDAddUpdateBedMasterBizActionVO nvo = valueObject as clsIPDAddUpdateBedMasterBizActionVO;
            try
            {
                objBedMatserDetails = nvo.objBedMatserDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBedMaster");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objBedMatserDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BedUnitID", DbType.Int64, objBedMatserDetails.BedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, objBedMatserDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, objBedMatserDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "BedCategoryId", DbType.Int64, objBedMatserDetails.BedCategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "WardId", DbType.Int64, objBedMatserDetails.WardID);
                this.dbServer.AddInParameter(storedProcCommand, "RoomId", DbType.Int64, objBedMatserDetails.RoomID);
                this.dbServer.AddInParameter(storedProcCommand, "IsAmmenity", DbType.Boolean, objBedMatserDetails.IsAmmenity);
                this.dbServer.AddInParameter(storedProcCommand, "IsNonCensus", DbType.Boolean, objBedMatserDetails.IsNonCensus);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objBedMatserDetails.Status);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objBedMatserDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, objBedMatserDetails.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objBedMatserDetails.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, objBedMatserDetails.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, objBedMatserDetails.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, objBedMatserDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, objBedMatserDetails.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objBedMatserDetails.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, objBedMatserDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, objBedMatserDetails.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, objBedMatserDetails.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                objBedMatserDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteBedAmmenities");
                this.dbServer.AddInParameter(command2, "BedID", DbType.Int64, objBedMatserDetails.ID);
                this.dbServer.ExecuteNonQuery(command2);
                DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddBedAmmenitiesDetails");
                foreach (MasterListItem item in objBedMatserDetails.AmmenityDetails)
                {
                    command3.Parameters.Clear();
                    this.dbServer.AddInParameter(command3, "BedID", DbType.Int64, objBedMatserDetails.ID);
                    this.dbServer.AddInParameter(command3, "SelUnitID", DbType.Int64, objBedMatserDetails.UnitID);
                    this.dbServer.AddInParameter(command3, "SelAmmID", DbType.Int64, item.ID);
                    this.dbServer.AddInParameter(command3, "SelAmmStaus", DbType.Boolean, objBedMatserDetails.Status);
                    this.dbServer.ExecuteNonQuery(command3);
                }
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateBlockMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDBlockMasterVO rvo = new clsIPDBlockMasterVO();
            clsAddUpdateIPDBlockMasterBizActionVO nvo = valueObject as clsAddUpdateIPDBlockMasterBizActionVO;
            try
            {
                rvo = nvo.objBlockMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBlockMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, rvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, rvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, rvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, rvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, rvo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, rvo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, rvo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, rvo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, rvo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, rvo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, rvo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, rvo.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, rvo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, rvo.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateClassMasterDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDClassMasterVO rvo = new clsIPDClassMasterVO();
            clsIPDAddUpdateClassMasterBizActionVO nvo = valueObject as clsIPDAddUpdateClassMasterBizActionVO;
            try
            {
                rvo = nvo.objClassMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateClassMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, rvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, rvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, rvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, rvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "DepositAdmission", DbType.Decimal, rvo.DepositIPD);
                this.dbServer.AddInParameter(storedProcCommand, "DepositOT", DbType.Decimal, rvo.DepositOT);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, rvo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, rvo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, rvo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, rvo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, rvo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, rvo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, rvo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, rvo.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, rvo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, rvo.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateDietPlanMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDAddUpdateDietPlanBizactionVO objItem = valueObject as clsIPDAddUpdateDietPlanBizactionVO;
            return ((objItem.objDietMatserDetails.ID != 0L) ? this.UpdateDietPlan(objItem, UserVo) : this.AddDietPlan(objItem, UserVo));
        }

        public override IValueObject AddUpdateFloorMaster(IValueObject valueObject, clsUserVO UserVO)
        {
            clsIPDFloorMasterVO flooMasterDetails = new clsIPDFloorMasterVO();
            clsIPDAddUpdateFloorMasterBizActionVO nvo = valueObject as clsIPDAddUpdateFloorMasterBizActionVO;
            try
            {
                flooMasterDetails = nvo.FlooMasterDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateFloorMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, flooMasterDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, flooMasterDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BlockID", DbType.Int64, flooMasterDetails.BlockID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, flooMasterDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, flooMasterDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, flooMasterDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateFoodItemMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDDietNutritionMasterVO rvo = new clsIPDDietNutritionMasterVO();
            clsIPDAddUpdateDietNutritionBizActionVO nvo = valueObject as clsIPDAddUpdateDietNutritionBizActionVO;
            try
            {
                rvo = nvo.objDietMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateFoodItemMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, rvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, rvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, rvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "ItemId", DbType.Int64, rvo.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, rvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, rvo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, rvo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, rvo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, rvo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, rvo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, rvo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, rvo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, rvo.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, rvo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, rvo.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateRoomMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDRoomMasterVO rvo = new clsIPDRoomMasterVO();
            clsIPDAddUpdateRoomMasterBizActionVO nvo = valueObject as clsIPDAddUpdateRoomMasterBizActionVO;
            try
            {
                rvo = nvo.objRoomMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateRoomMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, rvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, rvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, rvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "IsRoomAmmenities", DbType.Boolean, rvo.IsAmmenity);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, rvo.Status);
                if (rvo.IsAmmenity)
                {
                    StringBuilder builder = new StringBuilder();
                    StringBuilder builder2 = new StringBuilder();
                    StringBuilder builder3 = new StringBuilder();
                    StringBuilder builder4 = new StringBuilder();
                    int num = 0;
                    while (true)
                    {
                        if (num >= rvo.AmmenityDetails.Count)
                        {
                            this.dbServer.AddInParameter(storedProcCommand, "AmmenityIdList", DbType.String, builder.ToString());
                            this.dbServer.AddInParameter(storedProcCommand, "AmmenityStatusList", DbType.String, builder2.ToString());
                            this.dbServer.AddInParameter(storedProcCommand, "ARoomIdList", DbType.String, builder3.ToString());
                            this.dbServer.AddInParameter(storedProcCommand, "UnitIdList", DbType.String, builder4.ToString());
                            break;
                        }
                        builder.Append(rvo.AmmenityDetails[num].ID);
                        builder2.Append(rvo.AmmenityDetails[num].Status);
                        builder3.Append(rvo.ID);
                        builder4.Append(rvo.UnitID);
                        if (num < (rvo.AmmenityDetails.Count - 1))
                        {
                            builder.Append(",");
                            builder2.Append(",");
                            builder3.Append(",");
                            builder4.Append(",");
                        }
                        num++;
                    }
                }
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, rvo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, rvo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, rvo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, rvo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, rvo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, rvo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, rvo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, rvo.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, rvo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, rvo.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateWardMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDWardMasterVO rvo = new clsIPDWardMasterVO();
            clsIPDAddUpdateWardMasterBizActionVO nvo = valueObject as clsIPDAddUpdateWardMasterBizActionVO;
            try
            {
                rvo = nvo.objWardMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateWardMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, rvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, rvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, rvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "FloorId", DbType.Int64, rvo.FloorID);
                this.dbServer.AddInParameter(storedProcCommand, "Sex", DbType.Int64, rvo.CategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, rvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "BlockID", DbType.Int64, rvo.BlockID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, rvo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, rvo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, rvo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, rvo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, rvo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, rvo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, rvo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, rvo.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, rvo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, rvo.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAdmisionTypeServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIPDGetAdmissionTypeServiceListBizActionVO nvo = (clsIPDGetAdmissionTypeServiceListBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAdmissionTypeServiceList");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                if ((nvo.ServiceName != null) && (nvo.ServiceName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ServiceName", DbType.String, nvo.ServiceName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "SpecializationID", DbType.Int64, nvo.SpecializationID);
                this.dbServer.AddInParameter(storedProcCommand, "SubSpecializationID", DbType.Int64, nvo.SubSpecializationID);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, nvo.ServiceID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
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
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = (long) reader["ID"],
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            Specialization = (long) DALHelper.HandleDBNull(reader["SpecializationId"]),
                            SubSpecialization = (long) DALHelper.HandleDBNull(reader["SubSpecializationId"]),
                            SpecializationString = (string) DALHelper.HandleDBNull(reader["Specialization"]),
                            SubSpecializationString = (string) DALHelper.HandleDBNull(reader["SubSpecialization"]),
                            Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinRate"])),
                            ConcessionAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionAmount"])),
                            ConcessionPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ConcessionPercent"]))
                        };
                        nvo.ServiceList.Add(item);
                    }
                }
                if (nvo.UnitID > 0L)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetAdmissionTypeServiceLinkedList");
                    this.dbServer.AddInParameter(command2, "AdmissionTypeID", DbType.Int64, nvo.AdmissionTypeID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(command2, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(command2, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(command2, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(command2, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(command2);
                    if (reader2.HasRows)
                    {
                        if (nvo.SelectedServiceList == null)
                        {
                            nvo.SelectedServiceList = new List<clsServiceMasterVO>();
                        }
                        while (true)
                        {
                            if (!reader2.Read())
                            {
                                reader2.NextResult();
                                nvo.TotalRows = (int) this.dbServer.GetParameterValue(command2, "TotalRows");
                                reader2.Close();
                                break;
                            }
                            clsServiceMasterVO item = new clsServiceMasterVO {
                                ID = (long) reader2["ID"],
                                AdmissionTypeName = (string) DALHelper.HandleDBNull(reader2["AdmissionTypeName"]),
                                ServiceName = (string) DALHelper.HandleDBNull(reader2["ServiceName"]),
                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["UnitID"])),
                                UnitName = Convert.ToString(DALHelper.HandleDBNull(reader2["UnitName"])),
                                SpecializationString = (string) DALHelper.HandleDBNull(reader2["Specialization"]),
                                SubSpecializationString = (string) DALHelper.HandleDBNull(reader2["SubSpecialization"])
                            };
                            nvo.SelectedServiceList.Add(item);
                        }
                    }
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAdmissionTypeDetailListForAdmissionTypeMasterByAdmissionTypeID(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizActionVO nvo = (clsIPDGetAdmissionTypeDetailListForAdmissionTypeMasterByIDBizActionVO) valueObject;
            try
            {
                clsIPDAdmissionTypeVO doctorDetails = nvo.DoctorDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAdmissionTypeMasterDetailsListByAdmissionTypeID");
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionTypeID", DbType.Int64, nvo.AdmissionTypeID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (nvo.DoctorDetails == null)
                        {
                            nvo.DoctorDetails = new clsIPDAdmissionTypeVO();
                        }
                        nvo.DoctorDetails.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.DoctorDetails.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.DoctorDetails.Description = (string) DALHelper.HandleDBNull(reader["AdmissionType"]);
                        nvo.DoctorDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
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

        public override IValueObject GetAdmissionTypeMasterList(IValueObject valueObject, clsUserVO UserVO)
        {
            DbDataReader reader = null;
            clsIPDGetAdmissionTypeMasterBizActionVO nvo = valueObject as clsIPDGetAdmissionTypeMasterBizActionVO;
            clsIPDAdmissionTypeVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAdmissionTypeMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsIPDAdmissionTypeVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]))
                        };
                        nvo.objAdmissionTypeMasterDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAdmissionTypeServiceLinkedList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsIPDGetAdmissionTypeServiceLinkedListDetailsBizActionVO nvo = (clsIPDGetAdmissionTypeServiceLinkedListDetailsBizActionVO) valueObject;
            nvo.ServiceList = new List<clsServiceMasterVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAdmissionTypeServiceLinkedList");
                this.dbServer.AddInParameter(storedProcCommand, "AdmissionTypeID", DbType.Int64, nvo.AdmissionTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, 0);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ServiceList == null)
                    {
                        nvo.ServiceList = new List<clsServiceMasterVO>();
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
                        clsServiceMasterVO item = new clsServiceMasterVO {
                            ID = (long) reader["ID"],
                            AdmissionTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AdmissionTypeID"])),
                            AdmissionTypeName = (string) DALHelper.HandleDBNull(reader["AdmissionTypeName"]),
                            ServiceName = (string) DALHelper.HandleDBNull(reader["ServiceName"]),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"])),
                            SpecializationString = (string) DALHelper.HandleDBNull(reader["Specialization"]),
                            SubSpecializationString = (string) DALHelper.HandleDBNull(reader["SubSpecialization"]),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.ServiceList.Add(item);
                    }
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetBedCensusAndNonCensusList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            DbDataReader reader2 = null;
            clsIPDGetBedCensusAndNonCensusListBizActionVO nvo = valueObject as clsIPDGetBedCensusAndNonCensusListBizActionVO;
            clsIPDBedMasterVO rvo = null;
            try
            {
                DbCommand storedProcCommand;
                StringBuilder builder = new StringBuilder();
                if (nvo.IsBedDetails)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBedDetailsByIDandBedUnitID");
                    this.dbServer.AddInParameter(storedProcCommand, "BedID", DbType.Int64, nvo.BedID);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBedCensusAndNonCensusList");
                    this.dbServer.AddInParameter(storedProcCommand, "IsNonCensus", DbType.String, nvo.IsNonCensus);
                    this.dbServer.AddInParameter(storedProcCommand, "Occupied", DbType.Boolean, false);
                    this.dbServer.AddInParameter(storedProcCommand, "IsUnderMaintanence", DbType.Boolean, false);
                    this.dbServer.AddInParameter(storedProcCommand, "IsForReservation", DbType.Boolean, nvo.BedDetails.IsForReservation);
                    this.dbServer.AddInParameter(storedProcCommand, "ClassID", DbType.Int64, nvo.ClassID);
                    this.dbServer.AddInParameter(storedProcCommand, "WardID", DbType.Int64, nvo.WardID);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rvo = new clsIPDBedMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            BedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedID"])),
                            WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardId"])),
                            BedCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryId"])),
                            RoomID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RoomId"])),
                            WardName = Convert.ToString(DALHelper.HandleDBNull(reader["WardName"])),
                            BedCategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["BedCategoryName"])),
                            RoomName = Convert.ToString(DALHelper.HandleDBNull(reader["RoomName"])),
                            IsAmmenity = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBedAmme"])),
                            IsNonCensus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsNonCensus"])),
                            GenderID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GenderID"]))
                        };
                        if (nvo.IsBedDetails)
                        {
                            rvo.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                            rvo.MrNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                            rvo.AdmissionDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])));
                            rvo.PatientIPDNo = Convert.ToString(DALHelper.HandleDBNull(reader["IPDNO"]));
                        }
                       storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBedMasterAmmenitiesDetails");
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rvo.ID);
                        reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                        if (reader2.HasRows)
                        {
                            rvo.AmmenityDetails = new List<MasterListItem>();
                            builder = new StringBuilder();
                            while (true)
                            {
                                if (!reader2.Read())
                                {
                                    rvo.Facilities = builder.ToString();
                                    break;
                                }
                                MasterListItem item = new MasterListItem {
                                    ID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ID"])),
                                    Description = Convert.ToString(DALHelper.HandleDBNull(reader2["Ammenity"])),
                                    Status = (reader2["AmmenityStatus"].HandleDBNull() != null) && Convert.ToBoolean(reader2["AmmenityStatus"])
                                };
                                rvo.AmmenityDetails.Add(item);
                                builder.Append(item.Description);
                                builder.Append(",");
                            }
                        }
                        reader2.Close();
                        nvo.objBedMasterDetails.Add(rvo);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetBedListByDifferentSearch(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIPDBedListBizActionVO nvo = valueObject as clsGetIPDBedListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetIPDBedList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, nvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, nvo.Description);
                this.dbServer.AddInParameter(storedProcCommand, "BedCategoryId", DbType.Int64, nvo.BedCategoryID);
                this.dbServer.AddInParameter(storedProcCommand, "WardID", DbType.Int64, nvo.WardID);
                this.dbServer.AddInParameter(storedProcCommand, "RoomID", DbType.Int64, nvo.RoomID);
                this.dbServer.AddInParameter(storedProcCommand, "Occupied", DbType.Boolean, nvo.Occupied);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.BedDetails == null)
                    {
                        nvo.BedDetails = new List<clsIPDBedMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDBedMasterVO item = new clsIPDBedMasterVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            WardID = (long) DALHelper.HandleDBNull(reader["WardID"]),
                            WardName = (string) DALHelper.HandleDBNull(reader["Ward"]),
                            BedCategoryID = (long) DALHelper.HandleDBNull(reader["BedCategoryId"]),
                            BedCategoryName = (string) DALHelper.HandleDBNull(reader["BedCategory"]),
                            RoomID = (long) DALHelper.HandleDBNull(reader["RoomID"]),
                            RoomName = (string) DALHelper.HandleDBNull(reader["Room"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["status"]),
                            Occupied = (bool) DALHelper.HandleDBNull(reader["Occupied"])
                        };
                        nvo.BedDetails.Add(item);
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

        public override IValueObject GetBedMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            DbDataReader reader2 = null;
            clsIPDGetBedMasterBizActionVO nvo = valueObject as clsIPDGetBedMasterBizActionVO;
            clsIPDBedMasterVO rvo = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBedMasterDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        rvo = new clsIPDBedMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            WardID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardId"])),
                            BedCategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedCategoryId"])),
                            RoomID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RoomId"])),
                            WardName = Convert.ToString(DALHelper.HandleDBNull(reader["WardName"])),
                            BedCategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["BedCategoryName"])),
                            RoomName = Convert.ToString(DALHelper.HandleDBNull(reader["RoomName"])),
                            IsAmmenity = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBedAmme"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"])),
                            IsNonCensus = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsNonCensus"])),
                            BedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BedUnitID"]))
                        };
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetBedMasterAmmenitiesDetails");
                        this.dbServer.AddInParameter(command2, "ID", DbType.Int64, rvo.ID);
                        reader2 = (DbDataReader) this.dbServer.ExecuteReader(command2);
                        if (reader2.HasRows)
                        {
                            rvo.AmmenityDetails = new List<MasterListItem>();
                            while (reader2.Read())
                            {
                                MasterListItem item = new MasterListItem {
                                    ID = (long) DALHelper.HandleDBNull(reader2["BedAmmenityID"]),
                                    Description = Convert.ToString(DALHelper.HandleDBNull(reader2["Ammenity"])),
                                    Status = Convert.ToBoolean(reader2["AmmenityStatus"])
                                };
                                rvo.AmmenityDetails.Add(item);
                            }
                        }
                        reader2.Close();
                        nvo.objBedMasterDetails.Add(rvo);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetBlockMasterDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            throw new NotImplementedException();
        }

        public override IValueObject GetBlockMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsIPDGetBlockMasterBizActionVO nvo = valueObject as clsIPDGetBlockMasterBizActionVO;
            clsIPDBlockMasterVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBlockMasterDetails");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsIPDBlockMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"])),
                            UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]))
                        };
                        nvo.objBlockMasterDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetClassMasterDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsIPDGetClassMasterBizActionVO nvo = valueObject as clsIPDGetClassMasterBizActionVO;
            clsIPDClassMasterVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetClassMasterDetails");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsIPDClassMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"])),
                            DepositIPD = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DepositAdmission"])),
                            DepositOT = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DepositOT"]))
                        };
                        nvo.objClassMasterDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDietPlanMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            DbDataReader reader2 = null;
            clsIPDGetDietPlanBizActionVO nvo = valueObject as clsIPDGetDietPlanBizActionVO;
            clsIPDDietPlanMasterVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetDietPlan");
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsIPDDietPlanMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            PlanInst = Convert.ToString(DALHelper.HandleDBNull(reader["PlanInformation"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetDietPlanDetails");
                        this.dbServer.AddInParameter(command2, "ID", DbType.Int64, item.ID);
                        reader2 = (DbDataReader) this.dbServer.ExecuteReader(command2);
                        if (reader2.HasRows)
                        {
                            item.ItemList = new List<clsIPDDietPlanItemMasterVO>();
                            while (reader2.Read())
                            {
                                clsIPDDietPlanItemMasterVO rvo2 = new clsIPDDietPlanItemMasterVO {
                                    ID = (long) DALHelper.HandleDBNull(reader2["ID"]),
                                    ExpectedCal = (string) DALHelper.HandleDBNull(reader2["ItemExpectedCal"]),
                                    FoodItem = { 
                                        ID = (long) DALHelper.HandleDBNull(reader2["ItemID"]),
                                        Description = (string) DALHelper.HandleDBNull(reader2["FoodItemName"]),
                                        ItemName = (string) DALHelper.HandleDBNull(reader2["FoodItemCatName"]),
                                        ItemID = (long) DALHelper.HandleDBNull(reader2["ItemCategoryID"])
                                    },
                                    ItemCalories = (string) DALHelper.HandleDBNull(reader2["ItemCal"]),
                                    ItemCh = (string) DALHelper.HandleDBNull(reader2["ItemCH"]),
                                    ItemFat = (string) DALHelper.HandleDBNull(reader2["ItemFat"]),
                                    ItemInst = (string) DALHelper.HandleDBNull(reader2["ItemInstruction"]),
                                    ItemQty = (string) DALHelper.HandleDBNull(reader2["ItemQty"]),
                                    ItemUnit = (string) DALHelper.HandleDBNull(reader2["ItemUnit"]),
                                    Timing = (string) DALHelper.HandleDBNull(reader2["Timing"])
                                };
                                item.ItemList.Add(rvo2);
                            }
                        }
                        reader2.Close();
                        nvo.objDietPlanMasterDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetFloorMasterList(IValueObject valueObject, clsUserVO UserVO)
        {
            DbDataReader reader = null;
            clsIPDGetFloorMasterBizActionVO nvo = valueObject as clsIPDGetFloorMasterBizActionVO;
            clsIPDFloorMasterVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFloorMasterList");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsIPDFloorMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            BlockID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlockID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"])),
                            BlockName = Convert.ToString(DALHelper.HandleDBNull(reader["BlockName"]))
                        };
                        nvo.objFloorMasterDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetFoodItemMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsIPDGetDietNutritionBizActionVO nvo = valueObject as clsIPDGetDietNutritionBizActionVO;
            clsIPDDietNutritionMasterVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetFoodItemMasterDetails");
                string str = '%' + nvo.SearchExpression + '%';
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, str);
                this.dbServer.AddInParameter(storedProcCommand, "SearchCategory", DbType.Int64, nvo.SearchCategory);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsIPDDietNutritionMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryName"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]))
                        };
                        nvo.objDietMasterDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetRoomMasterDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsIPDGetRoomMasterBizActionVO nvo = valueObject as clsIPDGetRoomMasterBizActionVO;
            clsIPDRoomMasterVO rvo = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRoomMasterAmmenitiesDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rvo = new clsIPDRoomMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]))
                        };
                        nvo.objRoomMasterDetails.Add(rvo);
                    }
                }
                reader.NextResult();
                if (reader.HasRows)
                {
                    nvo.objRoomMasterAmmenities.AmmenityDetails = new List<MasterListItem>();
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Ammenity"]),
                            Status = (reader["AmmenityStatus"].HandleDBNull() != null) && Convert.ToBoolean(reader["AmmenityStatus"])
                        };
                        nvo.objRoomMasterAmmenities.AmmenityDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetRoomMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            DbDataReader reader2 = null;
            clsIPDGetRoomMasterBizActionVO nvo = valueObject as clsIPDGetRoomMasterBizActionVO;
            clsIPDRoomMasterVO rvo = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRoomMasterDetails");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rvo = new clsIPDRoomMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            IsAmmenity = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRoomAmme"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]))
                        };
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetRoomMasterAmmenitiesDetails");
                        this.dbServer.AddInParameter(command2, "ID", DbType.Int64, rvo.ID);
                        reader2 = (DbDataReader) this.dbServer.ExecuteReader(command2);
                        if (reader2.HasRows)
                        {
                            rvo.AmmenityDetails = new List<MasterListItem>();
                            while (reader2.Read())
                            {
                                MasterListItem item = new MasterListItem {
                                    ID = (long) DALHelper.HandleDBNull(reader2["ID"]),
                                    Description = (string) DALHelper.HandleDBNull(reader2["Ammenity"]),
                                    Status = (reader2["AmmenityStatus"].HandleDBNull() != null) && Convert.ToBoolean(reader2["AmmenityStatus"])
                                };
                                rvo.AmmenityDetails.Add(item);
                            }
                        }
                        reader2.Close();
                        nvo.objRoomMasterDetails.Add(rvo);
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
            return nvo;
        }

        public override IValueObject GetWardMasterList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsIPDGetWardMasterBizActionVO nvo = valueObject as clsIPDGetWardMasterBizActionVO;
            clsIPDWardMasterVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetWardMasterDetails");
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                            break;
                        }
                        item = new clsIPDWardMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            FloorID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FloorId"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"])),
                            UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"])),
                            FloorName = Convert.ToString(DALHelper.HandleDBNull(reader["FloorName"])),
                            CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WardType"])),
                            CategoryName = Convert.ToString(DALHelper.HandleDBNull(reader["WardTypeName"])),
                            BlockID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BlockId"])),
                            BlockName = Convert.ToString(DALHelper.HandleDBNull(reader["BlockName"]))
                        };
                        nvo.objWardMasterDetails.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateAdmissionTypeMasterStatus(IValueObject valueObject, clsUserVO UserVO)
        {
            clsIPDAddUpdateAdmissionTypeMasterBizActionVO nvo = valueObject as clsIPDAddUpdateAdmissionTypeMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpadateAdmissionTypeMasterStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.AdmissionTypeMasterDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.AdmissionTypeMasterDetails.Status);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateBedMasterStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDUpdateBedMasterStatusBizActionVO nvo = valueObject as clsIPDUpdateBedMasterStatusBizActionVO;
            try
            {
                clsIPDBedMasterVO bedStatus = nvo.BedStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateBedMasterStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, bedStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, bedStatus.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.BedStatus.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject UpdateBlockMasterStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDUpdateBlockMasterStatusBizActionVO nvo = valueObject as clsIPDUpdateBlockMasterStatusBizActionVO;
            try
            {
                clsIPDBlockMasterVO blockStatus = nvo.BlockStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateBlockMasterStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, blockStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, blockStatus.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.BlockStatus.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject UpdateClassMasterStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDUpdateClassMasterStatusBizActionVO nvo = valueObject as clsIPDUpdateClassMasterStatusBizActionVO;
            try
            {
                clsIPDClassMasterVO classStatus = nvo.ClassStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateClassMasterStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, classStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, classStatus.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ClassStatus.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        private clsIPDAddUpdateDietPlanBizactionVO UpdateDietPlan(clsIPDAddUpdateDietPlanBizactionVO objItem, clsUserVO UserVo)
        {
            DbTransaction transaction = null;
            DbConnection connection = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                clsIPDDietPlanMasterVO objDietMatserDetails = new clsIPDDietPlanMasterVO();
                objDietMatserDetails = objItem.objDietMatserDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDietPlanMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, objDietMatserDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, objDietMatserDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, objDietMatserDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, objDietMatserDetails.Description);
                this.dbServer.AddInParameter(storedProcCommand, "PlanInfo", DbType.String, objDietMatserDetails.PlanInst);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, objDietMatserDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, objDietMatserDetails.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, objDietMatserDetails.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, objDietMatserDetails.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, objDietMatserDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, objDietMatserDetails.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                objItem.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                if (objItem.SuccessStatus == 1)
                {
                    if ((objDietMatserDetails.ItemList != null) && (objDietMatserDetails.ItemList.Count != 0))
                    {
                        DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteDietPlanDetails");
                        this.dbServer.AddInParameter(command2, "PlanId", DbType.Int64, objDietMatserDetails.ID);
                        this.dbServer.ExecuteNonQuery(command2, transaction);
                    }
                    if ((objDietMatserDetails.ItemList != null) && (objDietMatserDetails.ItemList.Count > 0))
                    {
                        foreach (clsIPDDietPlanItemMasterVO rvo2 in objDietMatserDetails.ItemList)
                        {
                            DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddDietPlanDetails");
                            this.dbServer.AddInParameter(command3, "PlanID", DbType.Int64, objDietMatserDetails.ID);
                            this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, objDietMatserDetails.UnitID);
                            this.dbServer.AddInParameter(command3, "ItemId", DbType.Int64, rvo2.FoodItem.ID);
                            this.dbServer.AddInParameter(command3, "Timing", DbType.String, rvo2.Timing);
                            this.dbServer.AddInParameter(command3, "ItemQty", DbType.String, rvo2.ItemQty);
                            this.dbServer.AddInParameter(command3, "ItemUnit", DbType.String, rvo2.ItemUnit);
                            this.dbServer.AddInParameter(command3, "ItemCal", DbType.String, rvo2.ItemCalories);
                            this.dbServer.AddInParameter(command3, "ItemCH", DbType.String, rvo2.ItemCh);
                            this.dbServer.AddInParameter(command3, "ItemFat", DbType.String, rvo2.ItemFat);
                            this.dbServer.AddInParameter(command3, "ItemExpectedCal", DbType.String, rvo2.ExpectedCal);
                            this.dbServer.AddInParameter(command3, "ItemInst", DbType.String, rvo2.ItemInst);
                            this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, true);
                            this.dbServer.AddParameter(command3, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, rvo2.ID);
                            this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command3, transaction);
                            rvo2.ID = (long) this.dbServer.GetParameterValue(command3, "ID");
                        }
                    }
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                objItem.objDietMatserDetails = null;
            }
            finally
            {
                transaction = null;
                connection.Close();
                connection = null;
            }
            return objItem;
        }

        public override IValueObject UpdateDietPlanMasterStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDUpdateDietNutritionStatusBizActionVO nvo = valueObject as clsIPDUpdateDietNutritionStatusBizActionVO;
            try
            {
                clsIPDDietNutritionMasterVO dietStatus = nvo.DietStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateDietPlanMasterStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dietStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, dietStatus.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.DietStatus.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject UpdateFloorMasterStatus(IValueObject valueObject, clsUserVO UserVO)
        {
            clsIPDAddUpdateFloorMasterBizActionVO nvo = valueObject as clsIPDAddUpdateFloorMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpadateFloorMasterStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.FlooMasterDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.FlooMasterDetails.Status);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateRoomMasterStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDUpdateRoomStatusBizActionVO nvo = valueObject as clsIPDUpdateRoomStatusBizActionVO;
            try
            {
                clsIPDRoomMasterVO roomStatus = nvo.RoomStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateRoomMasterStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, roomStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, roomStatus.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.RoomStatus.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject UpdateStatusFoodItemMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDUpdateDietNutritionStatusBizActionVO nvo = valueObject as clsIPDUpdateDietNutritionStatusBizActionVO;
            try
            {
                clsIPDDietNutritionMasterVO dietStatus = nvo.DietStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateFoodItemMasterStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, dietStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, dietStatus.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.DietStatus.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }

        public override IValueObject UpdateWardMasterStatus(IValueObject valueObject, clsUserVO UserVo)
        {
            clsIPDUpdateWardMasterStatusBizActionVO nvo = valueObject as clsIPDUpdateWardMasterStatusBizActionVO;
            try
            {
                clsIPDWardMasterVO wardStatus = nvo.WardStatus;
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateWardMasterStatus");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, wardStatus.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, wardStatus.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.WardStatus.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
            }
            catch (Exception)
            {
                throw new HmsApplicationException(HMSConfigurationManager.GetValueFromApplicationConfig("ErrorMessage"));
            }
            return nvo;
        }
    }
}

