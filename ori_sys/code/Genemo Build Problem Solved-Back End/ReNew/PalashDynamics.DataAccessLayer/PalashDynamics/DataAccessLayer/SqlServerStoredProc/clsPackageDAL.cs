namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Master;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;

    public class clsPackageDAL : clsBasePackageDAL
    {
        private Database dbServer;

        private clsPackageDAL()
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

        public override IValueObject GetPackageList(IValueObject valueObject)
        {
            clsGetPackageMasterVO rvo = (clsGetPackageMasterVO) valueObject;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackageList");
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

