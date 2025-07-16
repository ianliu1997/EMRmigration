using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer
{
  public abstract class clsBaseAppointmentDAL
  {
      static private clsBaseAppointmentDAL _instance = null;
      /// <summary>
      /// Returns an instance of the provider type specified in the config file
      /// </summary>

      public static clsBaseAppointmentDAL GetInstance()
      {
          try
          {
              if (_instance == null)
              {
                  //Get the full name of data access layer class from xml file which stores the list of classess.
                  string _DerivedClassName = "clsAppointmentDAL";
                  string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                  //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                  _instance = (clsBaseAppointmentDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

              }
          }


          catch (Exception ex)
          {
              throw;
          }
          
              return _instance;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="valueObject"></param>
      /// <returns></returns>

      public abstract IValueObject AddAppointment(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject GetAppointmentDetails(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject CancelAppointment(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject GetAppointmentBYId(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject GetAppointmentBYMrNo(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject GetDoctorTime(IValueObject valueObject, clsUserVO UserVo);
      //public abstract IValueObject GetPatientPastVisit(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject AddCancelAppReason(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject GetAppointmentByDoctorAndAppointmentDate(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject CheckMRNO(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject CheckAppointmentTime(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject AddMarkVisit(IValueObject valueObject, clsUserVO UserVo);

      public abstract IValueObject CheckMarkVisit(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject GetPastAppointment(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject GetFutureAppointment(IValueObject valueObject, clsUserVO UserVo);
      public abstract IValueObject CheckAppointmentPatientDuplicacy(IValueObject valueObject, clsUserVO UserVo);


  }
}
