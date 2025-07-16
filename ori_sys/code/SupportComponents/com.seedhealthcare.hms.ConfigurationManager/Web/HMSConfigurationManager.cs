using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Web;
using System.Reflection;

namespace com.seedhealthcare.hms.Web.ConfigurationManager
{
    /// <summary>
    /// This Static Class Contains Methods Which Provides Configuration Settings.
    /// </summary>

    static public class HMSConfigurationManager
    {
       
        #region Variables Declaration

            private static Database dbServer = null;

        #endregion

        #region Public Medhods

        /// <summary>
        /// This Method Returns The NameSpace Of DataAccessLayer To Be Used
        /// </summary>
        /// <returns></returns>
        public static string GetDataAccesslayerNameSpace()
        {
            try
            {
                return GetValueFromApplicationConfig("DALNameSpace");
            }
            catch (System.Configuration.ConfigurationErrorsException cex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Returns The Values Stored in AppSettings Of Web.config file By Providing Key.
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string GetValueFromApplicationConfig(string strKey)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[strKey];
            }
            catch (System.Configuration.ConfigurationErrorsException cex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method Returns The Database refrence.
        /// </summary>
        /// <returns></returns>
        public static Database GetDatabaseReference()
        {
            try
            {
                if (dbServer == null)
                {
                    //IConfigurationSource source = new FileConfigurationSource(@"Configuration\ApplicationConfiguration\Application.config");
                    //DatabaseProviderFactory factory = new DatabaseProviderFactory(source);
                    dbServer = DatabaseFactory.CreateDatabase("DBConnection");
                }
                return dbServer;
            }
            catch (System.Configuration.ConfigurationException cex)
            {
                throw;
            }
            catch (TargetInvocationException TIEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static Database GetDatabaseReference(string sDBServer)
        {
            try
            {
                if (sDBServer == "DMS")
                {
                    dbServer = DatabaseFactory.CreateDatabase("DMSConnection");
                }
                if (sDBServer == "EMR")
                {
                    dbServer = DatabaseFactory.CreateDatabase("EMRConnection");
                }
                return dbServer;
            }
            catch (System.Configuration.ConfigurationException cex)
            {
                throw;
            }
            catch (TargetInvocationException TIEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

    }
}
