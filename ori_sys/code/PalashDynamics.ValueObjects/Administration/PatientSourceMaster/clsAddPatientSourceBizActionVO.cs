using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Administration.PatientSourceMaster
{
   public  class clsAddPatientSourceBizActionVO:IBizActionValueObject
    {
       private clsPatientSourceVO _PatientDetails;
        public clsPatientSourceVO PatientDetails
        {
            get { return _PatientDetails; }
            set { _PatientDetails = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private long _ResultSuccessStatus;
        public long ResultSuccessStatus
        {
            get { return _ResultSuccessStatus; }
            set { _ResultSuccessStatus = value; }
        }

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Administration.PatientSourceMaster.clsAddPatientSourceBizAction";
        }

        public string ToXml()
        {
            return this.ToString();
        }

        public bool IsFromItemGroupMaster { get; set; }  // set true from PalashDynamics.Administration.ItemGroupMaster
    }


   public class clsAddRegistrationChargesBizActionVO : IBizActionValueObject
   {
       private clsRegistrationChargesVO _PatientDetails;
       public clsRegistrationChargesVO PatientDetails
       {
           get { return _PatientDetails; }
           set { _PatientDetails = value; }
       }

       private long _SuccessStatus;
       public long SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }

       private long _ResultSuccessStatus;
       public long ResultSuccessStatus
       {
           get { return _ResultSuccessStatus; }
           set { _ResultSuccessStatus = value; }
       }

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Administration.PatientSourceMaster.clsAddRegistrationChargesBizAction";
       }

       public string ToXml()
       {
           return this.ToString();
       }
   }
}
