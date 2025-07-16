using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{


    public abstract class clsBaseMaterialConsumptionDAL
    {
        static private clsBaseMaterialConsumptionDAL _instance = null;

        public static clsBaseMaterialConsumptionDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsMaterialConsumptionDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseMaterialConsumptionDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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
        public abstract IValueObject GetMaterialConsumptionList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetMaterialConsumptionItemList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetPatientIndentReceiveStock(IValueObject valueObject, clsUserVO UserVo);


      


    }
}
