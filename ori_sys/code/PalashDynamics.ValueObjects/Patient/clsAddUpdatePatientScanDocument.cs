using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Patient
{
   public class clsAddUpdatePatientScanDocument : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Patient.clsAddUpdatePatientScanDocumentBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private int _SuccessStatus;

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private  List<clsPatientScanDocumentVO> objPatientScanDocList = null;

        public List<clsPatientScanDocumentVO> PatientScanDocList
        {
            get { return objPatientScanDocList; }
            set { objPatientScanDocList = value; }
        }
    }

   public class clsGetPatientScanDocument : IBizActionValueObject
   {
       #region  IBizActionValueObject
       public string GetBizAction()
       {
           return "PalashDynamics.BusinessLayer.Patient.clsGetPatientScanDocumentBizAction";
       }

       public string ToXml()
       {
           return this.ToXml();
       }


       #endregion

       private int _SuccessStatus;

       public int SuccessStatus
       {
           get { return _SuccessStatus; }
           set { _SuccessStatus = value; }
       }

       private List<clsPatientScanDocumentVO> objPatientScanDocList = null;

       public List<clsPatientScanDocumentVO> PatientScanDocList
       {
           get { return objPatientScanDocList; }
           set { objPatientScanDocList = value; }
       }

       private clsPatientScanDocumentVO objPatientScanDoc = null;

       public clsPatientScanDocumentVO PatientScanDoc
       {
           get { return objPatientScanDoc; }
           set { objPatientScanDoc = value; }
       }
   }
}
