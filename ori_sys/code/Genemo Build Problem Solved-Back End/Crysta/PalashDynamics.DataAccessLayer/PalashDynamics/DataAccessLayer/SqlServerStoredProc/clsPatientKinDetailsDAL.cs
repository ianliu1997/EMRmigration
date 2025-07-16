namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.OutPatientDepartment;
    using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
    using System;
    using System.Data;
    using System.Data.Common;

    public class clsPatientKinDetailsDAL : clsBasePatientKinDetailsDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsPatientKinDetailsDAL()
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

        public override IValueObject AddPatientKinDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddPatientKinDetailsBizActionVO nvo = valueObject as clsAddPatientKinDetailsBizActionVO;
            try
            {
                clsPatientKinDetailsVO kinDetails = nvo.KinDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPatientKinDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, kinDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "Name", DbType.String, kinDetails.Name);
                this.dbServer.AddInParameter(storedProcCommand, "RelationId", DbType.Int64, kinDetails.RelationId);
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo1", DbType.String, kinDetails.ContactNo1);
                this.dbServer.AddInParameter(storedProcCommand, "ContactNo2", DbType.String, kinDetails.ContactNo2);
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, kinDetails.AddressLine1);
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, kinDetails.AddressLine2);
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, kinDetails.AddressLine3);
                this.dbServer.AddInParameter(storedProcCommand, "CountryId", DbType.Int64, kinDetails.CountryId);
                this.dbServer.AddInParameter(storedProcCommand, "StateId", DbType.Int64, kinDetails.StateId);
                this.dbServer.AddInParameter(storedProcCommand, "DistrictID", DbType.Int64, kinDetails.DistrictID);
                this.dbServer.AddInParameter(storedProcCommand, "TalukaID", DbType.Int64, kinDetails.TalukaID);
                this.dbServer.AddInParameter(storedProcCommand, "CityId", DbType.Int64, kinDetails.CityId);
                this.dbServer.AddInParameter(storedProcCommand, "AreaId", DbType.Int64, kinDetails.AreaId);
                this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, kinDetails.Pincode);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, kinDetails.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, kinDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, kinDetails.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, kinDetails.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, kinDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, kinDetails.AddedWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "KinId", DbType.Int64, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.KinDetails.KinId = (long) this.dbServer.GetParameterValue(storedProcCommand, "KinId");
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }
    }
}

