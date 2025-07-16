using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing
{
    public abstract class clsBaseChargeDAL
    {

        static private clsBaseChargeDAL _instance = null;

        public static clsBaseChargeDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsChargeDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseChargeDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }


        /// <summary>
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns></returns>
        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo);

        //public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction);
        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction, long iParentID, long iCDParentID);  // Change For IPD Module

        public abstract IValueObject GetList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddRefundServices(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject GetChargeListForApprovalRequestWindow(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetChargeListAgainstBills(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetChargeTaxDetails(IValueObject valueObject, clsUserVO UserVo);
    }
}
