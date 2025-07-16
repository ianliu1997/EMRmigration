using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Patient
{
  public abstract class clsBaseTESEDAL 
    {
      static private clsBaseTESEDAL _instance = null;

      public static clsBaseTESEDAL GetInstance()
      {
          try
          {
              if (_instance == null)
              {
                  //Get the full name of data access layer class from xml file which stores the list of classess.
                  string _DerivedClassName = "clsTESE_DAL";
                  string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                  //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                  _instance = (clsBaseTESEDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
              }
          }
          catch (Exception ex)
          {
              throw;
          }
          return _instance;
      }

      public abstract IValueObject AddUpdateTESE(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject GetTESEDetails (IValueObject valueObject, clsUserVO UserVo);

   
    }
}
