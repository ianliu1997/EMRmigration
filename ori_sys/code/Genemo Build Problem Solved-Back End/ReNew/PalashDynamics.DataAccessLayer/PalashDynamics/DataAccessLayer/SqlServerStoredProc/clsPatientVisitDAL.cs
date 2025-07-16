namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Patient;
    using System;
    using System.Data;
    using System.Data.Common;

    public class clsPatientVisitDAL : clsBasePatientVisitDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private string ImgIP = string.Empty;
        private string ImgVirtualDir = string.Empty;
        private string ImgSaveLocation = string.Empty;

        private clsPatientVisitDAL()
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
            catch (Exception exception1)
            {
                throw exception1;
            }
        }

        public override IValueObject GetPatientVisitDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPatientVisitDetailsBizActionVO nvo = (clsGetPatientVisitDetailsBizActionVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientVisitDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.String, nvo.PatientID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "VisitUnitID", DbType.Int64, nvo.VisitUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromCS", DbType.Boolean, nvo.IsFromCS);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.PatientGeneralDetails == null)
                    {
                        nvo.PatientGeneralDetails = new clsPatientGeneralVO();
                    }
                    while (reader.Read())
                    {
                        nvo.PatientGeneralDetails.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        nvo.PatientGeneralDetails.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        nvo.PatientGeneralDetails.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        nvo.PatientGeneralDetails.VisitUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitUnitID"]));
                        nvo.PatientGeneralDetails.OPDNO = Convert.ToString(DALHelper.HandleDBNull(reader["OPDNO"]));
                    }
                }
                reader.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }
    }
}

