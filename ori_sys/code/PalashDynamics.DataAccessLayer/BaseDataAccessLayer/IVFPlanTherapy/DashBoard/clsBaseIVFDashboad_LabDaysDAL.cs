using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.IVFPlanTherapy.DashBoard
{
    public abstract class clsBaseIVFDashboad_LabDaysDAL
    {
        static private clsBaseIVFDashboad_LabDaysDAL _instance = null;

        public static clsBaseIVFDashboad_LabDaysDAL GetInstance()
        {
            try
            {
                if (_instance == null)
                {
                    //Get the full name of data access layer class from xml file which stores the list of classess.
                    string _DerivedClassName = "IVFPlanTherapy.DashBoard.clsIVFDashboad_LabDaysDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    //Create instance of Database dependent dataaccesslayer class and type cast it base class.
                    _instance = (clsBaseIVFDashboad_LabDaysDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName), true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return _instance;
        }

        public abstract IValueObject AddUpdateDay0Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay0Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject AddDay0OocList(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay0OocList(IValueObject valueObject, clsUserVO UserVo);
        
        

        public abstract IValueObject AddUpdateDay1Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay1Details(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateDay2Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay2Details(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateDay3Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay3Details(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateDay4Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay4Details(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateDay5Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay5Details(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateDay6Details(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetDay6Details(IValueObject valueObject, clsUserVO UserVo);

        public abstract IValueObject AddUpdateMediaDetails(IValueObject valueObject, clsUserVO UserVo);
        public abstract IValueObject GetMediaDetails(IValueObject valueObject, clsUserVO UserVo);


        public abstract IValueObject AddUpdateGraphicalRepList(IValueObject valueObject, clsUserVO UserVo);
         public abstract IValueObject GetGraphicalRepOocList(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject GetETDetails(IValueObject valueObject, clsUserVO UserVo);
         public abstract IValueObject AddUpdateETDetails(IValueObject valueObject, clsUserVO UserVo);

        //by neena
         public abstract IValueObject GetDay1OocyteDetails(IValueObject valueObject, clsUserVO UserVo);
         public abstract IValueObject GetDay0OocyteDetails(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject GetDay2OocyteDetails(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject GetDay3OocyteDetails(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject GetDay4OocyteDetails(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject GetDay5OocyteDetails(IValueObject valueObject, clsUserVO UserVo);
         public abstract IValueObject GetDay6OocyteDetails(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject AddUpdateFertCheckDetails(IValueObject valueObject, clsUserVO UserVo);
         public abstract IValueObject GetFertCheckDetails(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject UpdateAndGetImageListDetails(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject AddObervationDetails(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject AddUpdateDecision(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject AddUpdatePlanDecision(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject GetSemenSampleList(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject AddUpdateDay7Details(IValueObject valueObject, clsUserVO UserVo);
         public abstract IValueObject GetDay7Details(IValueObject valueObject, clsUserVO UserVo);
         public abstract IValueObject GetDay7OocyteDetails(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject AddLabDayDocuments(IValueObject valueObject, clsUserVO UserVo);
         public abstract IValueObject GetIVFICSIPlannedOocyteDetails(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject GetDate(IValueObject valueObject, clsUserVO UserVo);
         public abstract IValueObject GetFertCheckDate(IValueObject valueObject, clsUserVO UserVo);
         public abstract IValueObject GetObservationDate(IValueObject valueObject, clsUserVO UserVo);
         public abstract IValueObject GetDay1OocyteObservations(IValueObject valueObject, clsUserVO UserVo);

         public abstract IValueObject GetDay0OocyteDetailsOocyteRecipient(IValueObject valueObject, clsUserVO UserVo);
        //
    
    }


}