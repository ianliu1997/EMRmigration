namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseMaterialConsumptionDAL
    {
        private static clsBaseMaterialConsumptionDAL _instance;

        protected clsBaseMaterialConsumptionDAL()
        {
        }

        public abstract IValueObject Add(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseMaterialConsumptionDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "clsMaterialConsumptionDAL";
                    _instance = (clsBaseMaterialConsumptionDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetMaterialConsumptionItemList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetMaterialConsumptionList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetPatientIndentReceiveStock(IValueObject valueObject, clsUserVO UserVo);
    }
}

