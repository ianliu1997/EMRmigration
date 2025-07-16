using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.Inventory;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects;
using System.Reflection;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsItemConversionsDAL : clsBaseItemConversionsDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion


        private clsItemConversionsDAL()
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

        public override ValueObjects.IValueObject GetItemConversionsList(ValueObjects.IValueObject valueObject, ValueObjects.clsUserVO UserVo)
        {
            clsGetItemConversionFactorListBizActionVO BizActionObj = valueObject as clsGetItemConversionFactorListBizActionVO;

            DbCommand command;
            DbDataReader reader = null;

            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetItemConversionsListByItemID");

                dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionObj.ItemID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.UOMConversionList == null)
                        BizActionObj.UOMConversionList = new List<clsConversionsVO>();
                    while (reader.Read())
                    {
                        clsConversionsVO objConversionsVO = new clsConversionsVO();
                        objConversionsVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objConversionsVO.FromUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromUOMID"]));
                        objConversionsVO.ToUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToUOMID"]));
                        objConversionsVO.FromUOM = Convert.ToString(DALHelper.HandleDBNull(reader["FromUOM"]));
                        objConversionsVO.ToUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ToUOM"]));
                        objConversionsVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        //objConversionsVO.ConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        objConversionsVO.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        objConversionsVO.MainConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        objConversionsVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        BizActionObj.UOMConversionList.Add(objConversionsVO);
                    }

                }

                reader.NextResult();

                if (reader.HasRows)
                {
                    if (BizActionObj.UOMConvertList == null)
                        BizActionObj.UOMConvertList = new List<MasterListItem>();

                    while (reader.Read())
                    {
                        MasterListItem objConvertVO = new MasterListItem();

                        objConvertVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objConvertVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objConvertVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        BizActionObj.UOMConvertList.Add(objConvertVO);

                    }

                }

                reader.Close();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return BizActionObj;
        }


        public override IValueObject AddUpdateConversionFactorMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            clsAddUpdateItemConversionFactorListBizActionVO BizActionobj = null;
            try
            {
                con = dbServer.CreateConnection();
                trans = null;
                con.Open();
                trans = con.BeginTransaction();

                BizActionobj = valueObject as clsAddUpdateItemConversionFactorListBizActionVO;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddUpdateConversionFactorMaster");
                command.Connection = con;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionobj.UOMConversionVO.ID);
                dbServer.AddInParameter(command, "IsModify", DbType.Boolean, BizActionobj.IsModify);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionobj.UOMConversionVO.ItemID);
                dbServer.AddInParameter(command, "FromUOMID", DbType.Int64, BizActionobj.UOMConversionVO.FromUOMID);
                dbServer.AddInParameter(command, "ToUOMID", DbType.Int64, BizActionobj.UOMConversionVO.ToUOMID);
                dbServer.AddInParameter(command, "ConversionFactor", DbType.Decimal, BizActionobj.UOMConversionVO.ConversionFactor);

                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                trans.Commit();

            }
            catch (Exception ex)
            {

                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionobj.SuccessStatus = -1;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;
            }

            return BizActionobj;
        }

        public override IValueObject DeleteConversionFactorMaster(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = null;
            clsAddUpdateItemConversionFactorListBizActionVO BizActionobj = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                BizActionobj = valueObject as clsAddUpdateItemConversionFactorListBizActionVO;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteConversionFactorMaster");
                command.Connection = con;

                dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionobj.UOMConversionVO.ItemID);
                dbServer.AddInParameter(command, "FromUOMID", DbType.Int64, BizActionobj.UOMConversionVO.FromUOMID);
                dbServer.AddInParameter(command, "ToUOMID", DbType.Int64, BizActionobj.UOMConversionVO.ToUOMID);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdateWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionobj.SuccessStatus = -1;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
            }

            return BizActionobj;
        }


       

    }
}
