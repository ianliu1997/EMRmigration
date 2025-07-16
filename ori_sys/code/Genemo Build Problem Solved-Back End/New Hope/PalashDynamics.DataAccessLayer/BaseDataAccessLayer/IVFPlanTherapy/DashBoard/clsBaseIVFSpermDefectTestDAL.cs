namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseIVFSpermDefectTestDAL
    {
        private static clsBaseIVFSpermDefectTestDAL _instance;

        protected clsBaseIVFSpermDefectTestDAL()
        {
        }

        public abstract IValueObject AddUpdateSpermDefectTest(IValueObject valueObject, clsUserVO UserVo);
        public static clsBaseIVFSpermDefectTestDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string str = "IVFPlanTherapy.DashBoard.clsIVFSpermDefectTestDAL";
                    _instance = (clsBaseIVFSpermDefectTestDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetSpermDefectTestList(IValueObject valueObject, clsUserVO UserVo);
    }
}

