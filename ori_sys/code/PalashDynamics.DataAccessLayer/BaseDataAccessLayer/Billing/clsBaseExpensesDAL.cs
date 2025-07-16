using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.DataAccessLayer.SqlServerStoredProc;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing
{
    public abstract class clsBaseExpensesDAL
    {
        static private clsBaseExpensesDAL _instance = null;

        public static clsBaseExpensesDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsExpensesDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseExpensesDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddExpenses(IValueObject valueObject, clsUserVO UserVo);
        //public abstract IValueObject AddExpenses(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject GetExpenses(IValueObject valueObject, clsUserVO Uservo);
    }
}
