using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.CompoundDrug
{
    public class clsAddPatientCompoundPrescriptionBizActionVO : IBizActionValueObject
    {
        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.CompoundDrug.clsAddPatientCompoundDrugBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        #endregion

        public clsPatientCompoundPrescriptionVO PatientCompoundPrescription { get; set; }

        public List<clsPatientCompoundPrescriptionVO> PatientCompoundPrescriptionList { get; set; }

        /// <summary>
        ///  Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get
            {
                return _SuccessStatus;
            }
            set
            {
                _SuccessStatus = value;
            }
        }
    }
}
