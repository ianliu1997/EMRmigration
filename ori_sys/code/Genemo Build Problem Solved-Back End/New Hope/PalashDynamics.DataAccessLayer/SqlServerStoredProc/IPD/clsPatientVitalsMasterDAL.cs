namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IPD
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration.IPD;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsPatientVitalsMasterDAL : clsBasePatientVitalsMasterDAL
    {
        private Database dbServer;

        private clsPatientVitalsMasterDAL()
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

        public override IValueObject AddUpdatePatientVitalMaster(IValueObject valueObject, clsUserVO UserVO)
        {
            clsIPDPatientVitalsMasterVO rvo = new clsIPDPatientVitalsMasterVO();
            clsAddUpdateIPDPatientVitalsMasterBIzActionVO nvo = valueObject as clsAddUpdateIPDPatientVitalsMasterBIzActionVO;
            try
            {
                rvo = nvo.PatientVitalDetailList[0];
                nvo.PatientVitalDetails = new clsIPDPatientVitalsMasterVO();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdatePatientVitalsMaster");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, rvo.Code.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, rvo.Description.Trim());
                this.dbServer.AddInParameter(storedProcCommand, "IsModify", DbType.Boolean, rvo.IsModify);
                this.dbServer.AddInParameter(storedProcCommand, "DefaultValue", DbType.Double, rvo.DefaultValue);
                this.dbServer.AddInParameter(storedProcCommand, "MinValue", DbType.Double, rvo.MinValue);
                this.dbServer.AddInParameter(storedProcCommand, "MaxValue", DbType.Double, rvo.MaxValue);
                this.dbServer.AddInParameter(storedProcCommand, "Unit", DbType.String, rvo.Unit);
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
                nvo.PatientVitalDetails.SuccessStatus = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetPatientVitalMasterList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetIPDPatientVitalsMasterBIzActionVO nvo = (clsGetIPDPatientVitalsMasterBIzActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("[CIMS_GetPatientVitalMasterList]");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "TotalRows", DbType.Int64, nvo.TotalRows);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.VitalDetailsList == null)
                    {
                        nvo.VitalDetailsList = new List<clsIPDPatientVitalsMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsIPDPatientVitalsMasterVO item = new clsIPDPatientVitalsMasterVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Code = (string) DALHelper.HandleDBNull(reader["Code"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"]),
                            DefaultValue = Convert.ToDouble(reader["DefaultValue"]),
                            MinValue = Convert.ToDouble(reader["MinValue"]),
                            MaxValue = Convert.ToDouble(reader["MaxValue"]),
                            Unit = (string) DALHelper.HandleDBNull(reader["Unit"]),
                            Status = (bool) reader["Status"]
                        };
                        nvo.VitalDetailsList.Add(item);
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

        public override IValueObject UpdateStatusPatientVitalMaster(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsUpdateStatusIPDPatientVitalsMasterBIzActionVO nvo = (clsUpdateStatusIPDPatientVitalsMasterBIzActionVO) valueObject;
            try
            {
                clsIPDPatientVitalsMasterVO vitalDetails = nvo.VitalDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("[CIMS_UpdateStatusPatientVitalMaster]");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, vitalDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, vitalDetails.Status);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

