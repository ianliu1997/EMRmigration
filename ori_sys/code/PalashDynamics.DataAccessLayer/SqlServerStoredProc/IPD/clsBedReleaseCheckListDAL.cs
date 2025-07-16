using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration.IPD;
using System.Data.Common;
using System.Data;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
using PalashDynamics.ValueObjects.IPD;


namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    public class clsBedReleaseCheckListDAL : clsBaseBedReleaseCheckListDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        #endregion

        private clsBedReleaseCheckListDAL()
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


        public override IValueObject AddUpdateBedReleaseCheckListDetails(IValueObject valueObject, clsUserVO userVO)
        {
            clsIPDBedReleaseCheckListVO objItemVO = new clsIPDBedReleaseCheckListVO();
            clsAddUpdateBedReleaseCheckListDetailsBizActionVO objItem = valueObject as clsAddUpdateBedReleaseCheckListDetailsBizActionVO;
            try
            {
                DbCommand command;
                objItemVO = objItem.ItemMatserDetails[0];
                command = dbServer.GetStoredProcCommand("CIMS_AddUpdateBedReleaseCheckList");
                dbServer.AddInParameter(command, "Id", DbType.Int64, objItemVO.Id);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItemVO.UnitId);

                dbServer.AddInParameter(command, "Code", DbType.String, objItemVO.Code.Trim());
                dbServer.AddInParameter(command, "Description", DbType.String, objItemVO.Description.Trim());
                dbServer.AddInParameter(command, "IsMandantory", DbType.Boolean, objItemVO.IsMandantory);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objItemVO.Status);
                dbServer.AddInParameter(command, "AddUnitID", DbType.Int64, objItemVO.AddUnitID);

                dbServer.AddInParameter(command, "By", DbType.Int64, objItemVO.By);
                dbServer.AddInParameter(command, "On", DbType.String, objItemVO.On);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, objItemVO.DateTime);
                //dbServer.AddInParameter(command, "WindowsLoginName", DbType.String, objItemVO.WindowsLoginName);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int64, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command);
                objItem.SuccessStatus = Convert.ToInt16(dbServer.GetParameterValue(command, "ResultStatus"));
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

        public override IValueObject GetBedReleaseCheckList(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetBedReleaseCheckListBizActionVO objItem = valueObject as clsGetBedReleaseCheckListBizActionVO;
            clsIPDBedReleaseCheckListVO objBedRelVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetBedReleseCheckList");
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SearchExpression);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objBedRelVO = new clsIPDBedReleaseCheckListVO();
                        objBedRelVO.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objBedRelVO.Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"]));
                        objBedRelVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));

                        objBedRelVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objBedRelVO.IsMandantory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsMandantory"]));
                        if (objBedRelVO.IsMandantory == true)
                        {
                            objBedRelVO.MandantoryStatus = "Yes";
                        }
                        else
                        {
                            objBedRelVO.MandantoryStatus = "No";
                        }
                        objItem.ItemMatserDetails.Add(objBedRelVO);
                    }
                }
                reader.NextResult();
                objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objItem;
        }

        //Added By kiran for Get Relese list For Bed Relese form. 
        public override IValueObject GetBedReleaseList(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetBedReleseListBizActionVO objItem = valueObject as clsGetBedReleseListBizActionVO;
            clsIPDBedReleaseCheckListVO objBedRelVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetBedReleseList");

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objBedRelVO = new clsIPDBedReleaseCheckListVO();
                        objBedRelVO.Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objBedRelVO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));


                        objItem.ItemMatserDetails.Add(objBedRelVO);
                    }
                }
                reader.NextResult();
                objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                        reader.Close();
                }
            }
            return objItem;
        }


    }


}
