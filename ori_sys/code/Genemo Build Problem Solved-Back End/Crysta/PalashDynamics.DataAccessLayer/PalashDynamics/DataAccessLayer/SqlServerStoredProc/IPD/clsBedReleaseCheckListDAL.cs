namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration.IPD;
    using System;
    using System.Data;
    using System.Data.Common;

    public class clsBedReleaseCheckListDAL : clsBaseBedReleaseCheckListDAL
    {
        private Database dbServer;

        private clsBedReleaseCheckListDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddUpdateBedReleaseCheckListDetails(IValueObject valueObject, clsUserVO userVO)
        {
            clsIPDBedReleaseCheckListVO tvo = new clsIPDBedReleaseCheckListVO();
            clsAddUpdateBedReleaseCheckListDetailsBizActionVO nvo = valueObject as clsAddUpdateBedReleaseCheckListDetailsBizActionVO;
            try
            {
                tvo = nvo.ItemMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateBedReleaseCheckList");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, tvo.Id);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, tvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, tvo.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, tvo.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "IsMandantory", DbType.Boolean, tvo.IsMandantory);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, tvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddUnitID", DbType.Int64, tvo.AddUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "By", DbType.Int64, tvo.By);
                this.dbServer.AddInParameter(storedProcCommand, "On", DbType.String, tvo.On);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, tvo.DateTime);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("duplicate"))
                {
                    tvo.PrimaryKeyViolationError = true;
                }
                else
                {
                    tvo.GeneralError = true;
                }
            }
            return nvo;
        }

        public override IValueObject GetBedReleaseCheckList(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetBedReleaseCheckListBizActionVO nvo = valueObject as clsGetBedReleaseCheckListBizActionVO;
            clsIPDBedReleaseCheckListVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBedReleseCheckList");
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
                        item = new clsIPDBedReleaseCheckListVO {
                            Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            IsMandantory = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsMandantory"]))
                        };
                        item.MandantoryStatus = !item.IsMandantory ? "No" : "Yes";
                        nvo.ItemMatserDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
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

        public override IValueObject GetBedReleaseList(IValueObject valueObject, clsUserVO objUserVO)
        {
            DbDataReader reader = null;
            clsGetBedReleseListBizActionVO nvo = valueObject as clsGetBedReleseListBizActionVO;
            clsIPDBedReleaseCheckListVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBedReleseList");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsIPDBedReleaseCheckListVO {
                            Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                        };
                        nvo.ItemMatserDetails.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
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
    }
}

