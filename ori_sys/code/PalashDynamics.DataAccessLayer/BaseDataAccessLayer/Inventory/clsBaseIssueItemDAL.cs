using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    public abstract class clsBaseIssueItemDAL
    {
        static private clsBaseIssueItemDAL _instance = null;

        public static clsBaseIssueItemDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsIssueItemDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIssueItemDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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
        public abstract IValueObject GetItemListByIndentIdForIsueItem(IValueObject valueObject, clsUserVO UserVo);

      /// <summary>
        /// </summary>
        /// <param name="valueObject"></param>
        /// <returns></returns>
        public abstract IValueObject AddIssueItemToStore(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetItemListByIndentIdSrch(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetIssueList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetItemListByIssueId(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetIndentListBySupplier(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetIssueListQS(IValueObject valueObject, clsUserVO UserVo);  // Use to get already saved Issued Items to Quarantine Stores Only

        public abstract IValueObject GetGRNToQSIssueList(IValueObject valueObject, clsUserVO UserVo); //  set on ReceiveGRNToQS form while getting Records for Issue.


    }
}
