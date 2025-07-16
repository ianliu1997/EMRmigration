using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    public abstract class clsBaseGRNReturnDAL
    {
        static private clsBaseGRNReturnDAL _instance = null;

        public static clsBaseGRNReturnDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsGRNReturnDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseGRNReturnDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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

        public abstract IValueObject GRNReturnApprove(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GRNReturnReject(IValueObject valueObject, clsUserVO UserVo);
        
    }
}
