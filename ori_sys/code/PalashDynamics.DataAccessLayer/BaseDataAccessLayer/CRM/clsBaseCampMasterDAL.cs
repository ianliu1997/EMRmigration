using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM
{
  public abstract class clsBaseCampMasterDAL
    {
      static private clsBaseCampMasterDAL _instance = null;

      public static clsBaseCampMasterDAL GetInstance()
      {
          try
          {
              if (_instance == null)
              {
                  //Get the full name of data access layer class from xml file which stores the list of classess.
                  string _DerivedClassName = "clsCampMasterDAL";
                  string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                  //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                  _instance = (clsBaseCampMasterDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

              }
          }


          catch (Exception ex)
          {
              throw;
          }

          return _instance;
      }

   
      public abstract IValueObject AddCampMaster(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject GetCampMasterList(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject GetCampMasterByID(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject UpdateCampMaster(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject AddCampDetails(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject GetCampDetailsList(IValueObject valueObject, clsUserVO UserVo);
     
      public abstract IValueObject GetCampDetailsByID(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject UpdateCampDetails(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject GetCampFreeAndConssService(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject AddCampService(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject AddEmailSMSSentDetails(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject DeleteCampService(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject GetPROPatientList(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject AddPROPatient(IValueObject valueObject, clsUserVO UserVo);

    }
}
