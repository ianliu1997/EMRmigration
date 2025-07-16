using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    public abstract class clsBaseBedReleaseCheckListDAL
    {
        static private clsBaseBedReleaseCheckListDAL _instance = null;

        public static clsBaseBedReleaseCheckListDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "IPD.clsBedReleaseCheckListDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _instance = (clsBaseBedReleaseCheckListDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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

        #region

        public abstract IValueObject AddUpdateBedReleaseCheckListDetails(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetBedReleaseCheckList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetBedReleaseList(IValueObject valueObject, clsUserVO UserVo);


        #endregion




    }
}
