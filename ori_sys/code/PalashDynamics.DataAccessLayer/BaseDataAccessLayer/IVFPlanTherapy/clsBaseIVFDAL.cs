using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy
{
   public abstract class clsBaseIVFDAL
    {
       static private clsBaseIVFDAL _instance = null;

       public static clsBaseIVFDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsIVFDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIVFDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }


       public abstract IValueObject AddGeneralExaminationForMale(IValueObject valueObject, clsUserVO UserVo);
       public abstract IValueObject AddGeneralExaminationForFemale(IValueObject valueObject, clsUserVO UserVo);
       public abstract IValueObject AddMaleHistory(IValueObject valueObject, clsUserVO UserVo);
       public abstract IValueObject AddFemaleHistory(IValueObject valueObject, clsUserVO UserVo);
       public abstract IValueObject GetFemaleHistory(IValueObject valueObject, clsUserVO UserVo);
       public abstract IValueObject UpdateFemaleHistory(IValueObject valueObject, clsUserVO UserVo);
       public abstract IValueObject GetGeneralExaminationForMale(IValueObject valueObject, clsUserVO UserVo);
       public abstract IValueObject GetMaleHistory(IValueObject valueobject, clsUserVO UserVO);
       public abstract IValueObject GetGeneralExaminationForFemale(IValueObject valueObject, clsUserVO UserVo);


       //By Anjali....
       public abstract IValueObject AddClinicalSummary(IValueObject valueObject, clsUserVO UserVo);
       public abstract IValueObject GetClinicalSummary(IValueObject valueObject, clsUserVO UserVo);
    }
}
