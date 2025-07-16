using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.CRM
{
   public abstract class clsBaseLoyaltyProgramDAL
    {
       static private clsBaseLoyaltyProgramDAL _instance = null;

       public static clsBaseLoyaltyProgramDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsLoyaltyProgramDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseLoyaltyProgramDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);

                }
            }


            catch (Exception ex)
            {
                throw;
            }

            return _instance;
        }
       public abstract IValueObject AddLoyaltyProgram(IValueObject valueObject, clsUserVO UserVo);

       public abstract IValueObject GetLoyaltyProgram(IValueObject valueObject, clsUserVO UserVo);

       public abstract IValueObject UpdateLoyaltyProgram(IValueObject valueObject, clsUserVO UserVo);

       public abstract IValueObject GetLoyaltyProgramByID(IValueObject valueObject, clsUserVO UserVo);

       public abstract IValueObject GetLoyaltyProgramTariffByID(IValueObject valueObject, clsUserVO UserVo);
       
       public abstract IValueObject GetRelationMasterList(IValueObject valueObject, clsUserVO UserVo);

       public abstract IValueObject GetCategoryList(IValueObject valueObject, clsUserVO UserVo);
       
       public abstract IValueObject GetClinicList(IValueObject valueObject, clsUserVO UserVo);
       
       public abstract IValueObject GetFamilyDetailsByID(IValueObject valueObject, clsUserVO UserVo);

       public abstract IValueObject GetAttachmentDetailsByID(IValueObject valueObject, clsUserVO UserVo);

       public abstract IValueObject FillFamilyTariffUsingRelationID(IValueObject valueObject, clsUserVO UserVo);

       public abstract IValueObject FillCardTypeCombo(IValueObject valueObject, clsUserVO UserVo);




    }
}
