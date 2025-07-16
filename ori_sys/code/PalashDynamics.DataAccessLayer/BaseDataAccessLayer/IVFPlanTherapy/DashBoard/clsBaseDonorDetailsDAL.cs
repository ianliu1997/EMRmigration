using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    public abstract class clsBaseDonorDetailsDAL
    {
        static private clsBaseDonorDetailsDAL _instance = null;

        public static clsBaseDonorDetailsDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IVFPlanTherapy.DashBoard.clsDonorDetailsDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseDonorDetailsDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddUpdateDonorDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDonorDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDonorRegistrationDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDonorDetailsForIUI(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDonorDetailsAgainstSearch(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDonorDetailsToModify(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDonorBatchDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject CheckDuplicasyDonorCodeAndBLab(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetDetailsOfReceivedOocyte(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateRecievOocytesDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetOPUDate(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetSemenBatchAndSpermiogram(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject GetDonorList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddUpdateDonorBatch(IValueObject valueObject, clsUserVO UserVo);

        //added by neena for getting donated oocyte embryo details
        public abstract IValueObject GetDetailsOfReceivedOocyteEmbryo(IValueObject valueObject, clsUserVO UserVo);       
                
        public abstract IValueObject GetDetailsOfReceivedOocyteEmbryoFromDonorCycle(IValueObject valueObject, clsUserVO UserVo);
        //
    }
}
