using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy
{
    public abstract class clsBaseIVFLabDayDAL
    {
        static private clsBaseIVFLabDayDAL _instance = null;

        public static clsBaseIVFLabDayDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "clsIVFLabDayDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIVFLabDayDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }
        
        public abstract IValueObject AddLabDay1(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetLabDay0ForDay1(IValueObject valueObject, clsUserVO UserVo);
  
        public abstract IValueObject GetFemaleLabDay1(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFemaleLabDay1MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject AddUpdateFemaleLabDay0(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFemaleLabDay0(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject AddLabDay2(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetLabDay1ForDay2(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFemaleLabDay2(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFemaleLabDay2MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFemaleLabDay3MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetLabDay1Score(IValueObject valueObject, clsUserVO UserVo);
        
        public abstract IValueObject AddLabDay3(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFemaleLabDay3(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetLabDay2Score(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetLabDay2ForDay3(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetAllDayMediaDetails(IValueObject valueObject, clsUserVO UserVo);      
        public abstract IValueObject AddLabDay4(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetLabDay3ForDay4(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFemaleLabDay4(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFemaleLabDay4MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetLabDay3Score(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddLabDay5(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetLabDay4ForDay5(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFemaleLabDay5(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFemaleLabDay5MediaAndCalDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetLabDay4Score(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject AddLabDay6(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetLabDay5ForDay6(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetFemaleLabDay6(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetLabDay5Score(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject GetCleavageGradeMasterList(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject GetLab5And6MasterList(IValueObject valueObject, clsUserVO UserVo);
        


    }
}
