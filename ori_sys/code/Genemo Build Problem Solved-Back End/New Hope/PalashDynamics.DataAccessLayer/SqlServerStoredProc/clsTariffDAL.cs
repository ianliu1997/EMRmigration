namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Master;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class clsTariffDAL : clsBaseTariffDAL
    {
        private Database dbServer;

        private clsTariffDAL()
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

        public override IValueObject GetCompanyList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetCompanyMasterVO rvo = (clsGetCompanyMasterVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetCompanyList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "IsForPathology", DbType.Int64, rvo.IsForPathology);
                this.dbServer.AddInParameter(storedProcCommand, "PathologyCompanyType", DbType.Int64, rvo.PathologyCompanyType);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryID", DbType.Int64, rvo.PatientCategoryID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (rvo.List == null)
                    {
                        rvo.List = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        rvo.List.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return rvo;
        }

        public override IValueObject GetPatientCategoryList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetPatientCategoryMasterVO rvo = (clsGetPatientCategoryMasterVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientCategoryList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientCategoryId", DbType.Int64, rvo.PatientSourceID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (rvo.List == null)
                    {
                        rvo.List = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        rvo.List.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return rvo;
        }

        public override IValueObject GetTariffList(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetTariffMasterVO rvo = (clsGetTariffMasterVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetTariffList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, rvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "CompanyID", DbType.Int64, rvo.CompanyID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (rvo.List == null)
                    {
                        rvo.List = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        rvo.List.Add(new MasterListItem((long) reader["Id"], reader["Description"].ToString()));
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return rvo;
        }
    }
}

