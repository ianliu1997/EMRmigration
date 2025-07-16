using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsGetPatientPrescriptionBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientPrescriptionBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        
        private clsPatientPrescriptionVO objPatientPrescription = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public clsPatientPrescriptionVO PatientPrescriptionSummary
        {
            get { return objPatientPrescription; }
            set { objPatientPrescription = value; }
        }
       
    }
}
