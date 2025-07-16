using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{

    public abstract class clsBaseGRNDAL
    {
        static private clsBaseGRNDAL _instance = null;

        public static clsBaseGRNDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsGRNDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseGRNDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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


        public abstract IValueObject GetSearchList(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject GetItemDetailsList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetGRNForGRNReturn(IValueObject valueObject, clsUserVO UserVo);
        //public abstract IValueObject CheckItemSupplier(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject DeleteGRNItems(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateGRNForBarcode(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetItemDetailsByID(IValueObject valueObject, clsUserVO UserVo);

        // Added By CDS
        public abstract IValueObject UpdateGRNForApproval(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateGRNForApproval(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);

        public abstract IValueObject UpdateGRNForRejection(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject UpdateGRNForRejection(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction);
        // END 

        public abstract IValueObject GetGRNItemsForQS(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetGRNItemsForGRNReturnQSSearch(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetGRNInvoiceFile(IValueObject valueObject, clsUserVO UserVo);
    }
}
