using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    public abstract class clsBaseBedDAL
    {
        static private clsBaseBedDAL _instance = null;

        public static clsBaseBedDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "IPD.clsBedDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseBedDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject GetIPDWardByClassID(IValueObject valueObject, clsUserVO UserVo);
        #region Bed Under maintenance
        public abstract IValueObject AddBedUnderMaintenance(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetReleaseBedUnderMaintenanceList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateReleaseBedUnderMaintenance(IValueObject valueObject, clsUserVO UserVo);
        #endregion

        #region Bed Transfer
        public abstract IValueObject GetIPDBedTransferList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject CheckFinalBillbyPatientID(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetIPDBedTransferDetailsForSelectedPatient(IValueObject valueObject, clsUserVO UserVO);
        #endregion

        #region For Nursing Station 20022017

        public abstract IValueObject GetBillAndBedByAdmIDAndAdmUnitID(IValueObject valueObject, clsUserVO UserVo);

        #endregion

    }
}
