namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Masters
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

    public class clsCounterDAL : clsBaseCounterDAL
    {
        private Database dbServer;

        private clsCounterDAL()
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

        public override IValueObject GetCounterListByUnitId(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsGetCounterDetailsBizActionVO nvo = valueObject as clsGetCounterDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CounterListByUnitId");
                this.dbServer.AddInParameter(storedProcCommand, "ClinicID", DbType.Int64, nvo.ClinicID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.CounterDetails == null)
                    {
                        nvo.CounterDetails = new List<clsCounterVO>();
                    }
                    while (reader.Read())
                    {
                        clsCounterVO item = new clsCounterVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            Description = (string) DALHelper.HandleDBNull(reader["Description"])
                        };
                        nvo.CounterDetails.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
            }
            return nvo;
        }
    }
}

