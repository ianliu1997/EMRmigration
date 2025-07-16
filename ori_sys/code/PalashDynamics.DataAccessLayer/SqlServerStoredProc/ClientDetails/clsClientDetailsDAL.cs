using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.ClientDetails;
using PalashDynamics.ValueObjects;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.ValueObjects.ClientDetails;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsClientDetailsDAL : clsBaseClientDetailsDAL
    {
        #region variable Declaration

        private Database dbServer = null;
        private LogManager logManager = null;
        #endregion


        private clsClientDetailsDAL()
        {
            try
            {
                if (dbServer == null) dbServer = HMSConfigurationManager.GetDatabaseReference();

                if (logManager == null) logManager = LogManager.GetInstance();
            }
            catch (Exception ex)
            {
                
                //throw;
            }

        }


        public override IValueObject GetClient(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            clsGetClientBizActionVO BizAction = valueObject as clsGetClientBizActionVO;


            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetClientDetails");
                DbDataReader reader;

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                     clsClientDetailsVO detailsVO = new clsClientDetailsVO();
                    while (reader.Read())
                    {
                        detailsVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        detailsVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        detailsVO.Name = (string)DALHelper.HandleDBNull(reader["Name"]);
                        detailsVO.CustomerCode = (string)DALHelper.HandleDBNull(reader["CustomerCode"]);
                        detailsVO.ContactNo1 = (string)DALHelper.HandleDBNull(reader["ContactNo1"]);

                    }

                    BizAction.Details = detailsVO;
                }

            }
            catch (Exception)
            {
                
                throw;
            }



            return BizAction;

        }
    }
}
