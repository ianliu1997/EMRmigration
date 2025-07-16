using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using System.Data.Common;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    public abstract class cls_NewBaseSpremFreezingDAL
    {
        static private cls_NewBaseSpremFreezingDAL _instance = null;
        public static cls_NewBaseSpremFreezingDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IVFPlanTherapy.DashBoard.cls_NewSpremFreezingDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (cls_NewBaseSpremFreezingDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddUpdateSpremFrezing(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetSpremFreezingNew(IValueObject valuObject, clsUserVO UserVO);

        public abstract IValueObject DeleteSpremFreezingNew(IValueObject valuObject, clsUserVO UserVO);

        //By Anjali
        public abstract IValueObject GetSpremFreezingList(IValueObject valuObject, clsUserVO UserVO);
        
        //..................................
    }
}
