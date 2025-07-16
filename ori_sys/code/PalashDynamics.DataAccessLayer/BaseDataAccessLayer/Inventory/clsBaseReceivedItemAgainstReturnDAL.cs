using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer
{

    public abstract class clsBaseReceivedItemAgainstReturnDAL
    {
        static private clsBaseReceivedItemAgainstReturnDAL _instance = null;

        public static clsBaseReceivedItemAgainstReturnDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsReceivedItemAgainstReturnDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseReceivedItemAgainstReturnDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetReceivedListAgainstReturn(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetItemListByReturnReceivedId(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddReceivedItemAgainstReturn(IValueObject valueObject, clsUserVO UserVo);
              
    }
}
