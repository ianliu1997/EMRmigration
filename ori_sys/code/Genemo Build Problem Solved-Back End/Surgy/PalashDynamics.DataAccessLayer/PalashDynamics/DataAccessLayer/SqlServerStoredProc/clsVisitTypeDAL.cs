namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Masters;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Master;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    internal class clsVisitTypeDAL : clsBaseVisitTypeDAL
    {
        private Database dbServer;
        public bool chkFlag = true;

        private clsVisitTypeDAL()
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

        private clsAddVisitTypeBizActionVO AddVisit(clsAddVisitTypeBizActionVO BizActionObj, clsUserVO UserVO)
        {
            try
            {
                clsVisitTypeVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddVisitTypeMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, details.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, details.Description.Trim());
                long serviceID = details.ServiceID;
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, details.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "IsClinical", DbType.Boolean, details.IsClinical);
                this.dbServer.AddInParameter(storedProcCommand, "FreeDaysDuration", DbType.Int64, details.FreeDaysDuration);
                this.dbServer.AddInParameter(storedProcCommand, "IsFree", DbType.Boolean, details.IsFree);
                this.dbServer.AddInParameter(storedProcCommand, "ConsultationVisitType", DbType.Int64, details.ConsultationVisitType);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "Id", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, details.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.Details.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "Id");
                BizActionObj.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }

        public override IValueObject AddVisitType(IValueObject valueObject, clsUserVO UserVO)
        {
            clsAddVisitTypeBizActionVO bizActionObj = (clsAddVisitTypeBizActionVO) valueObject;
            return ((bizActionObj.Details.ID != 0L) ? this.UpdateVisit(bizActionObj, UserVO) : this.AddVisit(bizActionObj, UserVO));
        }

        public override IValueObject CheckVisitTypeMappedWithPackageService(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckVisitTypeMappedWithPackageServiceBizActionVO nvo = (clsCheckVisitTypeMappedWithPackageServiceBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckVisitTypeMappedWithPackageService");
                this.dbServer.AddInParameter(storedProcCommand, "VisitTypeID", DbType.Int64, nvo.VisitTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "IsPackage", DbType.Boolean, nvo.IsPackage);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.VisitTypeDetails == null)
                    {
                        nvo.VisitTypeDetails = new clsVisitTypeVO();
                    }
                    while (reader.Read())
                    {
                        nvo.VisitTypeDetails.ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceID"]);
                        nvo.VisitTypeDetails.IsPackage = (bool) DALHelper.HandleDBNull(reader["IsPackage"]);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetVisitTypeBizActionVO nvo = (clsGetVisitTypeBizActionVO) valueObject;
            try
            {
                StringBuilder builder1 = new StringBuilder();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetVisitType");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsVisitTypeVO>();
                    }
                    while (reader.Read())
                    {
                        clsVisitTypeVO item = new clsVisitTypeVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            ServiceID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ServiceID"])),
                            IsClinical = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsClinical"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]))
                        };
                        nvo.List.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject GetVisitTypeMaster(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetAllVisitTypeMasetrBizActionVO nvo = (clsGetAllVisitTypeMasetrBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetVisitTypeMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "sortExpression", DbType.String, nvo.sortExpression);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.List == null)
                    {
                        nvo.List = new List<clsVisitTypeVO>();
                    }
                    while (reader.Read())
                    {
                        clsVisitTypeVO item = new clsVisitTypeVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            ServiceID = (long) DALHelper.HandleDBNull(reader["ServiceID"]),
                            IsClinical = (bool) DALHelper.HandleDBNull(reader["IsClinical"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            IsFree = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFree"])),
                            FreeDaysDuration = Convert.ToInt64(DALHelper.HandleDBNull(reader["FreeDaysDuration"])),
                            ConsultationVisitType = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConsultationVisitTypeID"]))
                        };
                        nvo.List.Add(item);
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

        private clsAddVisitTypeBizActionVO UpdateVisit(clsAddVisitTypeBizActionVO BizActionObj, clsUserVO UserVO)
        {
            try
            {
                clsVisitTypeVO details = BizActionObj.Details;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateVisitTypeMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, details.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, details.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, details.Description.Trim());
                long serviceID = details.ServiceID;
                this.dbServer.AddInParameter(storedProcCommand, "ServiceID", DbType.Int64, details.ServiceID);
                this.dbServer.AddInParameter(storedProcCommand, "IsClinical", DbType.Boolean, details.IsClinical);
                this.dbServer.AddInParameter(storedProcCommand, "FreeDaysDuration", DbType.Int64, details.FreeDaysDuration);
                this.dbServer.AddInParameter(storedProcCommand, "IsFree", DbType.Boolean, details.IsFree);
                this.dbServer.AddInParameter(storedProcCommand, "ConsultationVisitType", DbType.Int64, details.ConsultationVisitType);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, details.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                BizActionObj.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return BizActionObj;
        }
    }
}

