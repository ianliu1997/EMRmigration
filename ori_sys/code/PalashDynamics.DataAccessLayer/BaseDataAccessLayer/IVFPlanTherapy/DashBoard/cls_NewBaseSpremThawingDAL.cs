using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    public abstract class cls_NewBaseSpremThawingDAL
    {
        static private cls_NewBaseSpremThawingDAL _instance = null;
        public static cls_NewBaseSpremThawingDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IVFPlanTherapy.DashBoard.cls_NewSpremThawingDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (cls_NewBaseSpremThawingDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }
        
        public abstract IValueObject GetSpremFreezingforThawingNew(IValueObject valuObject, clsUserVO UserVO);

        public abstract IValueObject GetThawingDetailsList(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetSpermFrezingDetailsForThawing(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetThawingDetailsListForIUI(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetTesaPesaForCode(IValueObject valueObject, clsUserVO UserVO);
        public abstract IValueObject GetSpermFrezingDetailsForThawingView(IValueObject valueObject, clsUserVO UserVO);

    }
}
