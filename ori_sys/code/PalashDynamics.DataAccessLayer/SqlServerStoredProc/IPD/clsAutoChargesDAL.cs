using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.IPD;
using System.Data.Common;
using System.Data;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    public class clsAutoChargesDAL : clsBaseAutoChargesDAL
    {
        #region Variables Declaration
        private Database dbServer = null;
        #endregion

        private clsAutoChargesDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
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

        public override IValueObject AddAutoChargesServiceList(IValueObject valueObject, clsUserVO userVO)
        {
            clsIPDAutoChargesVO objItemVO = new clsIPDAutoChargesVO();
            clsAddIPDAutoChargesServiceListBizActionVO objItem = valueObject as clsAddIPDAutoChargesServiceListBizActionVO;
            try
            {
                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_DeleteAutoCharges");

                dbServer.AddInParameter(command1, "UnitId", DbType.Int64, objItem.UnitId);

                int intStatus1 = dbServer.ExecuteNonQuery(command1);


                DbCommand command;

                foreach (clsIPDAutoChargesVO item in objItem.ChargesMasterList)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_AddAutoChargesServiceList");

                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItem.UnitId);
                    dbServer.AddInParameter(command, "ServiceId", DbType.Int64, item.ServiceId);

                    int intStatus = dbServer.ExecuteNonQuery(command);

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    objItemVO.PrimaryKeyViolationError = true;
                }
                else
                {
                    objItemVO.GeneralError = true;
                }
            }
            return objItem;
        }

        public override IValueObject GetAutoChargesServiceList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetIPDAutoChargesServiceListBizActionVO BizActionObj = (clsGetIPDAutoChargesServiceListBizActionVO)valueObject;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("GetAutoChargesServiceList");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, 0);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, BizActionObj.SearchExpression);

                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.GetChargesMasterList == null)
                        BizActionObj.GetChargesMasterList = new List<clsIPDAutoChargesVO>();
                    while (reader.Read())
                    {
                        clsIPDAutoChargesVO ServiceListVO = new clsIPDAutoChargesVO();
                        ServiceListVO.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"]));
                        ServiceListVO.ServiceId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceId"]));

                        ServiceListVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));

                        BizActionObj.GetChargesMasterList.Add(ServiceListVO);
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

        public override IValueObject DeleteService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsDeleteServiceBizActionVO BizActionObj = valueObject as clsDeleteServiceBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_DeleteService");

                dbServer.AddInParameter(command, "Id", DbType.Int64, BizActionObj.Id);

                int intStatus = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }


    }
}

