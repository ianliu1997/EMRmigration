using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    public abstract class clsBaseIVFDashboard_VitrificationDAL
    {
        static private clsBaseIVFDashboard_VitrificationDAL _instance = null;
        public static clsBaseIVFDashboard_VitrificationDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IVFPlanTherapy.DashBoard.clsIVFDashboard_VitrificationDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIVFDashboard_VitrificationDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddUpdateIVFDashBoard_Vitrification(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashBoard_Vitrification(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIVFDashBoard_PreviousEmbFromVitrification(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetVitrificationForCryoBank(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject UpdateVitrificationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetUsedEmbryoDetails(IValueObject valueObject, clsUserVO UserVo);

        //Added By CDS On 12/07/16
        public abstract IValueObject GetIVFDashBoard_PreviousOocyteFromVitrification(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOocyteVitrificationForCryoBank(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateIVFDashBoard_VitrificationSingle(IValueObject valueObject, clsUserVO UserVo);
        //END
        
        //added by neena
        public abstract IValueObject AddUpdateDonateDiscardOocyteForCryoBank(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDonateDiscardEmbryoForCryoBank(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateIVFDashBoard_RenewalDate(IValueObject valueObject, clsUserVO UserVo);
        //
    }
}
