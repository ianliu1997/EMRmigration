using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master.Location;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsLocationDAL : clsBaseLocationDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        private clsLocationDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override IValueObject AddCountryInfo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddCountryBizActionVO BizActionObj = (clsAddCountryBizActionVO)valueObject;

            try
            {
                clsCountryVO ObjCountryVO = BizActionObj.objCountryDetail;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddCountryMaster");
                dbServer.AddInParameter(command, "CountryCode", DbType.String, ObjCountryVO.CountryCode.Trim());
                dbServer.AddInParameter(command, "CountryName", DbType.String, ObjCountryVO.CountryName.Trim());


                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjCountryVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjCountryVO.CountryID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);
                BizActionObj.objCountryDetail.CountryID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (BizActionObj.SuccessStatus == 1)
                {
                    if (BizActionObj.CountryStateList.Count > 0)
                    {
                        foreach (var ObjCountryStateInfo in BizActionObj.CountryStateList)
                        {

                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddCountryStateMaster");

                            dbServer.AddInParameter(command1, "CountryID", DbType.Int64, BizActionObj.objCountryDetail.CountryID);
                            dbServer.AddInParameter(command1, "StateID", DbType.Int64, ObjCountryStateInfo.StateID);

                            int iStatus = dbServer.ExecuteNonQuery(command1);
                        }
                    }
                }
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

        public override IValueObject AddStateInfo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddStateBizActionVO BizActionObj = (clsAddStateBizActionVO)valueObject;

            try
            {
                clsStateVO ObjCountryVO = BizActionObj.objStateDetail;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddStateMaster");
                dbServer.AddInParameter(command, "MasterZoneCode", DbType.String, ObjCountryVO.MasterZoneCode.Trim());
                dbServer.AddInParameter(command, "StateName", DbType.String, ObjCountryVO.StateName.Trim());


                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjCountryVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjCountryVO.StateID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);
                BizActionObj.objStateDetail.StateID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (BizActionObj.SuccessStatus == 1)
                {
                    if (BizActionObj.StateDistList.Count > 0)
                    {
                        foreach (var ObjStateDistInfo in BizActionObj.StateDistList)
                        {

                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddStateDistMaster");

                            dbServer.AddInParameter(command1, "StateID", DbType.Int64, BizActionObj.objStateDetail.StateID);
                            dbServer.AddInParameter(command1, "DistID", DbType.Int64, ObjStateDistInfo.DistID);

                            int iStatus = dbServer.ExecuteNonQuery(command1);
                        }
                    }
                }
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

        public override IValueObject AddDistInfo(IValueObject valueObject, clsUserVO UserVo)
        {

            clsAddDistBizActionVO BizActionObj = (clsAddDistBizActionVO)valueObject;

            try
            {
                clsDistVO ObjDistVO = BizActionObj.objDistDetail;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddDistMaster");

                dbServer.AddInParameter(command, "DistName", DbType.String, ObjDistVO.DistName.Trim());

                dbServer.AddInParameter(command, "Code", DbType.String, ObjDistVO.Code.Trim());
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjDistVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjDistVO.DistID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);
                BizActionObj.objDistDetail.DistID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (BizActionObj.SuccessStatus == 1)
                {
                    if (BizActionObj.DistCityList.Count > 0)
                    {
                        foreach (var ObjDistCityInfo in BizActionObj.DistCityList)
                        {

                            DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDistCityMaster");

                            dbServer.AddInParameter(command1, "DistID", DbType.Int64, BizActionObj.objDistDetail.DistID);
                            dbServer.AddInParameter(command1, "CityID", DbType.Int64, ObjDistCityInfo.CityID);

                            int iStatus = dbServer.ExecuteNonQuery(command1);
                        }
                    }
                }
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

        public override IValueObject AddCityInfo(IValueObject valueObject, clsUserVO UserVo)
        {

            clsAddCityBizActionVO BizActionObj = (clsAddCityBizActionVO)valueObject;

            try
            {
                clsCityVO ObjCityVO = BizActionObj.objCityDetail;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddCityMaster");
                dbServer.AddInParameter(command, "CityCode", DbType.String, ObjCityVO.CityCode.Trim());

                dbServer.AddInParameter(command, "CityName", DbType.String, ObjCityVO.CityName.Trim());


                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjCityVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjCityVO.CityID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);
                BizActionObj.objCityDetail.CityID = (long)dbServer.GetParameterValue(command, "ID");
                 BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                 if (BizActionObj.SuccessStatus == 1)
                 {
                     if (BizActionObj.CityAreaList.Count > 0)
                     {
                         foreach (var ObjCityAreaInfo in BizActionObj.CityAreaList)
                         {

                             DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddCityAreaMaster");

                             dbServer.AddInParameter(command1, "CityID", DbType.Int64, BizActionObj.objCityDetail.CityID);
                             dbServer.AddInParameter(command1, "AreaID", DbType.Int64, ObjCityAreaInfo.AreaID);

                             int iStatus = dbServer.ExecuteNonQuery(command1);
                         }
                     }
                 }
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

        public override IValueObject AddAreaInfo(IValueObject valueObject, clsUserVO UserVo)
        {

            clsAddAreaBizActionVO BizActionObj = (clsAddAreaBizActionVO)valueObject;

            try
            {
                clsAreaVO ObjAreaVO = BizActionObj.objAreaDetail;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddAreaMaster");

                dbServer.AddInParameter(command, "AreaName", DbType.String, ObjAreaVO.AreaName.Trim());
                dbServer.AddInParameter(command, "CityID", DbType.Int64, ObjAreaVO.CityID);
                dbServer.AddInParameter(command, "PinCode", DbType.String, ObjAreaVO.PinCode.Trim());

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjAreaVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjAreaVO.AreaID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);
                BizActionObj.objAreaDetail.AreaID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
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




        public override IValueObject UpdateCountryInfo(IValueObject valueObject, clsUserVO UserVo)
        {

            clsUpdateCountryMasterDetails BizActionObj = (clsUpdateCountryMasterDetails)valueObject;

            try
            {
                clsCountryVO ObjCountryVO = BizActionObj.objCountryDetail;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateCountryMaster");
                dbServer.AddInParameter(command, "CountryID", DbType.String, ObjCountryVO.CountryID);
                dbServer.AddInParameter(command, "CountryCode", DbType.String, ObjCountryVO.CountryCode.Trim());
                dbServer.AddInParameter(command, "CountryName", DbType.String, ObjCountryVO.CountryName.Trim());


                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjCountryVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus2 = dbServer.ExecuteNonQuery(command);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (BizActionObj.SuccessStatus == 1)
                {

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteCountryStateMaster");
                    dbServer.AddInParameter(command2, "CountryID", DbType.Int64, BizActionObj.objCountryDetail.CountryID);
                    int iStatus1 = dbServer.ExecuteNonQuery(command2);


                    foreach (var ObjCountryStateInfo in BizActionObj.CountryStateList)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddCountryStateMaster");

                        dbServer.AddInParameter(command1, "CountryID", DbType.Int64, BizActionObj.objCountryDetail.CountryID);
                        dbServer.AddInParameter(command1, "StateID", DbType.Int64, ObjCountryStateInfo.StateID);

                        int iStatus = dbServer.ExecuteNonQuery(command1);
                    }
                }
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

        public override IValueObject UpdateStateInfo(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateStateMasterDetailsBizActionVO BizActionObj = (clsUpdateStateMasterDetailsBizActionVO)valueObject;
            try
            {
                clsStateVO ObjCountryVO = BizActionObj.objStateDetail;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateStateMaster");
                dbServer.AddInParameter(command, "StateID", DbType.String, ObjCountryVO.StateID);
                dbServer.AddInParameter(command, "MasterZoneCode", DbType.String, ObjCountryVO.MasterZoneCode.Trim());
                dbServer.AddInParameter(command, "StateName", DbType.String, ObjCountryVO.StateName.Trim());

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjCountryVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);

                 BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                 if (BizActionObj.SuccessStatus == 1)
                 {


                     DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteStateDistMaster");
                     dbServer.AddInParameter(command2, "StateID", DbType.Int64, BizActionObj.objStateDetail.StateID);
                     int iStatus1 = dbServer.ExecuteNonQuery(command2);

                     foreach (var ObjStateDistInfo in BizActionObj.StateDistList)
                     {

                         DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddStateDistMaster");

                         dbServer.AddInParameter(command1, "StateID", DbType.Int64, BizActionObj.objStateDetail.StateID);
                         dbServer.AddInParameter(command1, "DistID", DbType.Int64, ObjStateDistInfo.DistID);

                         int iStatus = dbServer.ExecuteNonQuery(command1);
                     }
                 }
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

        public override IValueObject UpdateDistInfo(IValueObject valueObject, clsUserVO UserVo)
        {

            clsUpdateDistMasterDetailsBizActionVO BizActionObj = (clsUpdateDistMasterDetailsBizActionVO)valueObject;

            try
            {
                clsDistVO ObjDistVO = BizActionObj.objDistDetail;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateDistMaster");
                dbServer.AddInParameter(command, "DistID", DbType.String, ObjDistVO.DistID);
                dbServer.AddInParameter(command, "Code", DbType.String, ObjDistVO.Code.Trim());
                dbServer.AddInParameter(command, "DistName", DbType.String, ObjDistVO.DistName.Trim());


                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjDistVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);

                 BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                 if (BizActionObj.SuccessStatus == 1)
                 {

                     DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteDistCityMaster");
                     dbServer.AddInParameter(command2, "DistID", DbType.Int64, BizActionObj.objDistDetail.DistID);
                     int iStatus1 = dbServer.ExecuteNonQuery(command2);

                     foreach (var ObjDistCityInfo in BizActionObj.DistCityList)
                     {

                         DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddDistCityMaster");

                         dbServer.AddInParameter(command1, "DistID", DbType.Int64, BizActionObj.objDistDetail.DistID);
                         dbServer.AddInParameter(command1, "CityID", DbType.Int64, ObjDistCityInfo.CityID);

                         int iStatus = dbServer.ExecuteNonQuery(command1);
                     }
                 }
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

        public override IValueObject UpdateCityInfo(IValueObject valueObject, clsUserVO UserVo)
        {

            clsUpdateCityMasterDetailsBizActionVO BizActionObj = (clsUpdateCityMasterDetailsBizActionVO)valueObject;

            try
            {
                clsCityVO ObjDistVO = BizActionObj.objCityDetail;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateCityMaster");
                dbServer.AddInParameter(command, "CityID", DbType.String, ObjDistVO.CityID);
                dbServer.AddInParameter(command, "CityCode", DbType.String, ObjDistVO.CityCode.Trim());
                dbServer.AddInParameter(command, "CityName", DbType.String, ObjDistVO.CityName.Trim());
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);
                // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjDistVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);
                 BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                 if (BizActionObj.SuccessStatus == 1)
                 {


                     DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteCityAreaMaster");
                     dbServer.AddInParameter(command2, "CityID", DbType.Int64, BizActionObj.objCityDetail.CityID);
                     int iStatus1 = dbServer.ExecuteNonQuery(command2);

                     foreach (var ObjCityAreaInfo in BizActionObj.CityAreaList)
                     {

                         DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddCityAreaMaster");

                         dbServer.AddInParameter(command1, "CityID", DbType.Int64, BizActionObj.objCityDetail.CityID);
                         dbServer.AddInParameter(command1, "AreaID", DbType.Int64, ObjCityAreaInfo.AreaID);

                         int iStatus = dbServer.ExecuteNonQuery(command1);
                     }
                 }
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

        public override IValueObject UpdateAreaInfo(IValueObject valueObject, clsUserVO UserVo)
        {

            clsUpdateAreaMasterDetailsBizActionVO BizActionObj = (clsUpdateAreaMasterDetailsBizActionVO)valueObject;

            try
            {
                clsAreaVO ObjAreaVO = BizActionObj.objAreaDetail;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateAreaMaster");
                dbServer.AddInParameter(command, "AreaID", DbType.String, ObjAreaVO.AreaID);

                dbServer.AddInParameter(command, "AreaName", DbType.String, ObjAreaVO.AreaName.Trim());
                dbServer.AddInParameter(command, "PinCode", DbType.String, ObjAreaVO.PinCode.Trim());
                dbServer.AddInParameter(command, "CityID", DbType.Int64, ObjAreaVO.CityID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjAreaVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);

                 BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

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

       
        
        
        
        public override IValueObject GetCountryList(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetCountryListBizActionVO BizActionObj = (clsGetCountryListBizActionVO)valueObject;

            try
            {

                if (BizActionObj.CountryID == 0)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCountryList");
                    if (BizActionObj.CountryName != null && BizActionObj.CountryName.Length != 0)
                        dbServer.AddInParameter(command, "CountryName", DbType.String, BizActionObj.CountryName + "%");
                    else
                        dbServer.AddInParameter(command, "CountryName", DbType.String, null);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                    dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);


                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.CountryList == null)
                            BizActionObj.CountryList = new List<clsCountryVO>();
                        while (reader.Read())
                        {
                            clsCountryVO StateVO = new clsCountryVO();
                            StateVO.CountryID = (long)reader["ID"];
                            StateVO.CountryCode = Convert.ToString(DALHelper.HandleDBNull(reader["CountryCode"]));
                            StateVO.CountryName = reader["CountryName"].ToString();
                            StateVO.Status = Convert.ToBoolean(reader["Status"]);
                            BizActionObj.CountryList.Add(StateVO);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }

                else
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCountryDetailsByID");
                    dbServer.AddInParameter(command, "CountryID", DbType.Int64, BizActionObj.CountryID);
                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.StateList == null)
                            BizActionObj.StateList = new List<clsStateVO>();
                        while (reader.Read())
                        {
                            clsStateVO StateVO = new clsStateVO();
                           
                            StateVO.StateID = (long)reader["StateID"];
                            StateVO.StateName = reader["State"].ToString();
                            BizActionObj.StateList.Add(StateVO);
                        }
                    }
                    reader.NextResult();
                   // BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
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

        public override IValueObject GetStateList(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetStateListBizActionVO BizActionObj = (clsGetStateListBizActionVO)valueObject;

            try
            {
                if (BizActionObj.StateID == 0)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStateList");

                    if (BizActionObj.StateName != null && BizActionObj.StateName.Length != 0)
                        dbServer.AddInParameter(command, "StateName", DbType.String, BizActionObj.StateName + "%");
                    else
                        dbServer.AddInParameter(command, "StateName", DbType.String, null);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                    dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);


                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.StateList == null)
                            BizActionObj.StateList = new List<clsStateVO>();
                        while (reader.Read())
                        {
                            clsStateVO StateVO = new clsStateVO();
                            StateVO.StateID = (long)reader["ID"];
                            StateVO.MasterZoneCode = reader["MasterZoneCode"].ToString();
                            StateVO.StateName = reader["StateName"].ToString();
                            StateVO.Status = Convert.ToBoolean(reader["Status"]);
                            BizActionObj.StateList.Add(StateVO);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
                else
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDistDetailsByID");
                    dbServer.AddInParameter(command, "StateID", DbType.Int64, BizActionObj.StateID);
                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.DistrictList == null)
                            BizActionObj.DistrictList = new List<clsDistVO>();
                        while (reader.Read())
                        {
                            clsDistVO DistVO = new clsDistVO();

                            DistVO.DistID = (long)reader["DistID"];
                            DistVO.DistName = reader["DistName"].ToString();
                            BizActionObj.DistrictList.Add(DistVO);
                        }
                    }
                    reader.NextResult();
                    // BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
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

        public override IValueObject GetDistList(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetDistListBizActionVO BizActionObj = (clsGetDistListBizActionVO)valueObject;

            try
            {
                if (BizActionObj.DistID == 0)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDistList");

                    if (BizActionObj.DistName != null && BizActionObj.DistName.Length != 0)
                        dbServer.AddInParameter(command, "DistName", DbType.String, BizActionObj.DistName + "%");
                    else
                        dbServer.AddInParameter(command, "DistName", DbType.String, null);
                    
                    //if (BizActionObj.DistName != null && BizActionObj.DistName.Length != 0)
                    //    dbServer.AddInParameter(command, "Code", DbType.String,  + "%");
                    //else
                    //    dbServer.AddInParameter(command, "DistName", DbType.String, null);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                    dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);


                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    { 
                        if (BizActionObj.DistList == null)
                            BizActionObj.DistList = new List<clsDistVO>();
                        while (reader.Read())
                        {
                            clsDistVO DistVO = new clsDistVO();
                            DistVO.DistID = (long)reader["ID"];                         
                            
                            DistVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                            DistVO.DistName = reader["DistName"].ToString();
                            DistVO.Status = Convert.ToBoolean(reader["Status"]);
                            BizActionObj.DistList.Add(DistVO);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
                else
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetDistCityDetailsByID");
                    dbServer.AddInParameter(command, "DistID", DbType.Int64, BizActionObj.DistID);
                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.CityList == null)
                            BizActionObj.CityList = new List<clsCityVO>();
                        while (reader.Read())
                        {
                            clsCityVO CityVO = new clsCityVO();

                            CityVO.CityID = (long)reader["CityID"];
                            CityVO.CityName = reader["CityName"].ToString();
                            BizActionObj.CityList.Add(CityVO);
                        }
                    }
                    reader.NextResult();
                    // BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
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

        public override IValueObject GetCityList(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetCityListBizActionVO BizActionObj = (clsGetCityListBizActionVO)valueObject;

            try
            {
                if (BizActionObj.CityID == 0)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCityList");

                    if (BizActionObj.CityName != null && BizActionObj.CityName.Length != 0)
                        dbServer.AddInParameter(command, "CityName", DbType.String, BizActionObj.CityName + "%");
                    else
                        dbServer.AddInParameter(command, "CityName", DbType.String, null);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                    dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);


                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.CityList == null)
                            BizActionObj.CityList = new List<clsCityVO>();
                        while (reader.Read())
                        {
                            clsCityVO CityVO = new clsCityVO();
                            CityVO.CityID = (long)reader["ID"];
                            CityVO.CityName = reader["CityName"].ToString();
                            CityVO.CityCode = reader["CityCode"].ToString();
                            CityVO.Status = Convert.ToBoolean(reader["Status"]);
                            BizActionObj.CityList.Add(CityVO);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
                else
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCityAreaDetailsByID");
                    dbServer.AddInParameter(command, "CityID", DbType.Int64, BizActionObj.CityID);
                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.AreaList == null)
                            BizActionObj.AreaList = new List<clsAreaVO>();
                        while (reader.Read())
                        {
                            clsAreaVO AreaVO = new clsAreaVO();

                            AreaVO.AreaID = (long)reader["AreaID"];
                            AreaVO.AreaName = reader["Area"].ToString();
                            AreaVO.PinCode = reader["PinCode"].ToString();

                            BizActionObj.AreaList.Add(AreaVO);
                        }
                    }
                    reader.NextResult();
                    // BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
                    reader.Close();
                }
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

        public override IValueObject GetAreaList(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetAreaListBizActionVO BizActionObj = (clsGetAreaListBizActionVO)valueObject;

            try
            {
               
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAreaList");

                    if (BizActionObj.AreaName != null && BizActionObj.AreaName.Length != 0)
                        dbServer.AddInParameter(command, "AreaName", DbType.String, BizActionObj.AreaName + "%");
                    else
                        dbServer.AddInParameter(command, "AreaName", DbType.String, null);
                    dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                    dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                    dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                    dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                    dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);


                    DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                    if (reader.HasRows)
                    {
                        if (BizActionObj.AreaList == null)
                            BizActionObj.AreaList = new List<clsAreaVO>();
                        while (reader.Read())
                        {
                            clsAreaVO DistVO = new clsAreaVO();
                            DistVO.AreaID = (long)reader["ID"];
                            DistVO.AreaName = reader["AreaName"].ToString();
                            DistVO.PinCode = reader["PinCode"].ToString();
                            DistVO.CityID = (long)reader["CityID"];
                            DistVO.Status = Convert.ToBoolean(reader["Status"]);
                            BizActionObj.AreaList.Add(DistVO);
                        }
                    }
                    reader.NextResult();
                    BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
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




        public override IValueObject AddAddressLocation6BizActionInfo(IValueObject valueObject, clsUserVO UserVo)
        {

            clsAddAddressLocation6BizActionVO BizActionObj = (clsAddAddressLocation6BizActionVO)valueObject;

            try
            {
                clsAddressLocation6VO ObjAreaVO = BizActionObj.objAddressLocation6Detail;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddAddressLocationMaster");

                dbServer.AddInParameter(command, "AddressLocation6Name", DbType.String, ObjAreaVO.AddressLocation6Name.Trim());
                dbServer.AddInParameter(command, "ZoneID", DbType.Int64, ObjAreaVO.ZoneID);
                dbServer.AddInParameter(command, "PinCode", DbType.String, ObjAreaVO.PinCode.Trim());

                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjAreaVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ObjAreaVO.ID);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus2 = dbServer.ExecuteNonQuery(command);
                BizActionObj.objAddressLocation6Detail.ID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
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


        public override IValueObject GetAddressLocation6List(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetAddressLocation6BizActionVO BizActionObj = (clsGetAddressLocation6BizActionVO)valueObject;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAddressLocation6List");

                if (BizActionObj.AddressLocation6Name != null && BizActionObj.AddressLocation6Name.Length != 0)
                    dbServer.AddInParameter(command, "AddressLocation6Name", DbType.String, BizActionObj.AddressLocation6Name + "%");
                else
                    dbServer.AddInParameter(command, "AddressLocation6Name", DbType.String, null);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.objAddressLocation6List == null)
                        BizActionObj.objAddressLocation6List = new List<clsAddressLocation6VO>();
                    while (reader.Read())
                    {
                        clsAddressLocation6VO objVO = new clsAddressLocation6VO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));

                        objVO.AddressLocation6Name = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));

                        objVO.PinCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));

                        objVO.ZoneID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AreaID"]));

                        objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        BizActionObj.objAddressLocation6List.Add(objVO);
                    }
                }
                reader.NextResult();
                BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
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




        public override IValueObject UpdateAddressLocation6Info(IValueObject valueObject, clsUserVO UserVo)
        {

            clsUpdateAddressLocation6BizActionVO BizActionObj = (clsUpdateAddressLocation6BizActionVO)valueObject;

            try
            {
                clsAddressLocation6VO ObjAreaVO = BizActionObj.objAddressLocation6Detail;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateAddressLocation6Master");
                dbServer.AddInParameter(command, "ID", DbType.String, ObjAreaVO.ID);

                dbServer.AddInParameter(command, "AddressLocation6Name", DbType.String, ObjAreaVO.AddressLocation6Name.Trim());
                dbServer.AddInParameter(command, "PinCode", DbType.String, ObjAreaVO.PinCode.Trim());
                dbServer.AddInParameter(command, "ZoneID", DbType.Int64, ObjAreaVO.ZoneID);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, true);

                // dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, ObjAreaVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus2 = dbServer.ExecuteNonQuery(command);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

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



        //GetAddressLocation6ListByZoneId
        public override IValueObject GetAddressLocation6ListByZoneId(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetAddressLocation6ListByZoneIdBizActionVO BizActionObj = (clsGetAddressLocation6ListByZoneIdBizActionVO)valueObject;

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAddressLocation6ListByZoneID");

                //if (BizActionObj. != null && BizActionObj.AddressLocation6Name.Length != 0)
                //    dbServer.AddInParameter(command, "AddressLocation6Name", DbType.String, BizActionObj.AddressLocation6Name + "%");
                //else
                //    dbServer.AddInParameter(command, "AddressLocation6Name", DbType.String, null);

                dbServer.AddInParameter(command, "ZoneID", DbType.Int64, BizActionObj.ipZoneID);

                //dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                //dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartRowIndex);
                //dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                //dbServer.AddInParameter(command, "sortExpression", DbType.String, "ID");
                //dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.TotalRows);


                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.objAddressLocation6List == null)
                        BizActionObj.objAddressLocation6List = new List<clsAddressLocation6VO>();
                    while (reader.Read())
                    {
                        clsAddressLocation6VO objVO = new clsAddressLocation6VO();

                        objVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddressLocation6ID"]));

                        objVO.AddressLocation6Name = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));

                        objVO.PinCode = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));

                        objVO.ZoneID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AreaID"]));

                       // objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        BizActionObj.objAddressLocation6List.Add(objVO);
                    }
                }
                //reader.NextResult();
                //BizActionObj.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));
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






    }
}
