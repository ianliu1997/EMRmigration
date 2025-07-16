namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.DashBoardVO;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;

    internal class cls_IVFDashboard_SurrogateDAL : cls_BaseIVFDashboard_SurrogateDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private cls_IVFDashboard_SurrogateDAL()
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

        public override IValueObject GetAgencyListOfSurrogate(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_IVFDashboard_GetAgencyListOfSurrogateBizActionVO nvo = valueObject as cls_IVFDashboard_GetAgencyListOfSurrogateBizActionVO;
            nvo.AgencyList = new List<cls_AgencyInfoVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAgencyListOfSurrogate");
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    for (long i = 1L; reader.Read(); i += 1L)
                    {
                        cls_AgencyInfoVO item = new cls_AgencyInfoVO {
                            ID = i,
                            Agencyname = Convert.ToString(DALHelper.HandleDBNull(reader["Agencyname"])),
                            AgencyContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyContactNo"])),
                            AgencyEmail = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyEmail"])),
                            AgencyAddress = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyAddress"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyInfo"]))
                        };
                        nvo.AgencyList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

