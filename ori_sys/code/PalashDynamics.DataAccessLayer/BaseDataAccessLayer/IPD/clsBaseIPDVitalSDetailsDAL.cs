using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    
  public abstract class clsBaseIPDVitalSDetailsDAL
    {
      static private clsBaseIPDVitalSDetailsDAL _instance = null;

      public static clsBaseIPDVitalSDetailsDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsIPDVitalSDetailsDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIPDVitalSDetailsDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
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
         //Added By Kiran for Fill Taken ComboBox .

         public abstract IValueObject GetUnitWiseEmpDetails(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject AddVitalSDetails(IValueObject valueObject, clsUserVO UserVo);
       
       
         public abstract IValueObject GetTPRDetailsListByAdmIDAdmUnitID(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject GetVitalsDetailsList(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject GetListofVitalDetails(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject UpdateStatusVitalDetails(IValueObject valueObject, clsUserVO UserVo);


         public abstract IValueObject GetGraphDetails(IValueObject valueObject, clsUserVO UserVo);
        
        #endregion
    }
}
