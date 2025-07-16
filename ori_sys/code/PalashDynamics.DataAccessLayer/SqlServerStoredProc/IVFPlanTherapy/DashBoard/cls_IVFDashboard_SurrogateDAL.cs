using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.IVFPlanTherapy.DashBoard
{
    class cls_IVFDashboard_SurrogateDAL : cls_BaseIVFDashboard_SurrogateDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private cls_IVFDashboard_SurrogateDAL()
        {
            try
            {
                #region Create Instance of database,LogManager object and BaseSql object
                //Create Instance of the database object and BaseSql object
                if (dbServer == null)
                {
                    dbServer = HMSConfigurationManager.GetDatabaseReference();
                }

                //Create Instance of the LogManager object 
                if (logManager == null)
                {
                    logManager = LogManager.GetInstance();
                }
                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override IValueObject GetAgencyListOfSurrogate(IValueObject valueObject, clsUserVO UserVo)
        {
            cls_IVFDashboard_GetAgencyListOfSurrogateBizActionVO Bizaction = (valueObject) as cls_IVFDashboard_GetAgencyListOfSurrogateBizActionVO;
            Bizaction.AgencyList = new List<cls_AgencyInfoVO>();
            //Bizaction.SpremFreezingVO = new clsNew_SpremFreezingVO();

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetAgencyListOfSurrogate");
                DbDataReader reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    long i = 1;
                    while (reader.Read())
                    {
                        cls_AgencyInfoVO VO = new cls_AgencyInfoVO();
                        VO.ID = i;
                        VO.Agencyname = Convert.ToString(DALHelper.HandleDBNull(reader["Agencyname"]));
                        VO.AgencyContactNo = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyContactNo"]));
                        VO.AgencyEmail = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyEmail"]));
                        VO.AgencyAddress = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyAddress"]));
                        VO.Description = Convert.ToString(DALHelper.HandleDBNull(reader["AgencyInfo"]));
                        Bizaction.AgencyList.Add(VO);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Bizaction;
        }

    }
}
