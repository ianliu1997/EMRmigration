using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.ClientDetails
{
   public abstract class clsBaseClientDetailsDAL
    {

       static private clsBaseClientDetailsDAL _instance;

       public static clsBaseClientDetailsDAL GetInstance()
       {
           try
           {

               if (_instance == null)
               {
                   string _DerivedClassName = "clsClientDetailsDAL";
                   string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;

                   _instance = (clsBaseClientDetailsDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
               }
           }
           catch (Exception ex)
           {
               
               throw;
           }

            return _instance;

       }

       public abstract IValueObject GetClient(IValueObject valueObject, clsUserVO UserVo);

    }
}
