using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IPD
{
    public abstract class clsBaseReservationDAL
    {
        static private clsBaseReservationDAL _instance = null;

        public static clsBaseReservationDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IPD.clsBedReservationDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseReservationDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }
        public abstract IValueObject AddIPDBedReservation(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIPDBedReservationList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIPDBedReservationStatus(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddIPDPatientReminderLog(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetIPDPatientReminderLog(IValueObject valueObject, clsUserVO UserVo);
    }
    public abstract class clsBaseUnReservationDAL
    {
        static private clsBaseUnReservationDAL _instance = null;

        public static clsBaseUnReservationDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    string _DerivedClassName = "IPD.clsBedUnReservationDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseUnReservationDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }
        public abstract IValueObject AddIPDBedUnReservation(IValueObject valueObject, clsUserVO UserVo);
    }

    public abstract class clsPatientReminderLogDAL
    {
        static private clsPatientReminderLogDAL _instance = null;

        public static clsPatientReminderLogDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsBedReservationDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsPatientReminderLogDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddIPDPatientReminderLog(IValueObject valueObject, clsUserVO UserVo);
    }
}
