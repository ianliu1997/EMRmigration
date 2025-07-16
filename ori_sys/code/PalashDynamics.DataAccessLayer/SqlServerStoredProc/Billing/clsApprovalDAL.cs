using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Billing
{
    public class clsApprovalDAL : clsBaseApprovalDAL
    {
         #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsApprovalDAL()
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


        public override IValueObject GetCancellationAndConcessionApprovalRequest(IValueObject valueObject, clsUserVO UserVo)
        {
            throw new NotImplementedException();
        }

    }
}
