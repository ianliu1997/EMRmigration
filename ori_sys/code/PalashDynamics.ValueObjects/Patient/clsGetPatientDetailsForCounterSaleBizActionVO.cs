using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Billing;

namespace PalashDynamics.ValueObjects.Patient
{
   public class clsGetPatientDetailsForCounterSaleBizActionVO:IBizActionValueObject
    {
        private clsPatientVO _PatientDetails;
        public clsPatientVO PatientDetails
        {
            get { return _PatientDetails; }
            set { _PatientDetails = value; }

        }


        public string MRNO { get; set; }
        public long UnitID { get; set; }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientDetailsForCounterSaleBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

   public class clsGetPatientDetailsForPathologyBizActionVO : IBizActionValueObject
   {
       private clsPatientVO _PatientDetails;
       public clsPatientVO PatientDetails
       {
           get { return _PatientDetails; }
           set { _PatientDetails = value; }

       }


       private List<clsChargeVO> objPatientBillDetails = null;
       public List<clsChargeVO> PatientBillDetailList
       {
           get { return objPatientBillDetails; }
           set { objPatientBillDetails = value; }
       }

       private clsBillVO objPatientBillInfoDetails = null;
       public clsBillVO PatientBillInfoDetail
       {
           get { return objPatientBillInfoDetails; }
           set { objPatientBillInfoDetails = value; }
       }

       public string MRNO { get; set; }
       public long UnitID { get; set; }


       #region IBizActionValueObject Members

       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.clsGetPatientDetailsForPathologyBizAction";
       }

       #endregion

       #region IValueObject Members

       public string ToXml()
       {
           return this.ToString();
       }

       #endregion
   }
}
