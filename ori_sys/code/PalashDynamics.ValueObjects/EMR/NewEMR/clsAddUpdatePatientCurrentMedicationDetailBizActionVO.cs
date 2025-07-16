
//Created Date:22/July/2013
//Created By: Nilesh Raut
//Specification: BizAction VO For Add and Update the Patient EMR Current Medication Detail

//Review By:
//Review Date:

//Modified By:
//Modified Date: 
using System.Collections.Generic;

namespace PalashDynamics.ValueObjects.EMR
{
   public class clsAddUpdatePatientCurrentMedicationDetailBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.EMR.clsAddUpdatePatientCurrentMedicationDetailBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public long DoctorID { get; set; }
        public string LinkServer { get; set; }
        public bool Status { get; set; }
        public string DoctorCode { get; set; }
        public bool IsOPDIPD { get; set; }
        public string Remark { get; set; }
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

        // Flag to define Add or Update transaction
        // For Add = 0
        // For Update = 1


        private List<clsPatientPrescriptionDetailVO> objPatientCurrentMedicationDetail = null;
        public List<clsPatientPrescriptionDetailVO> PatientCurrentMedicationDetail
        {
            get { return objPatientCurrentMedicationDetail; }
            set { objPatientCurrentMedicationDetail = value; }
        }

    }
}
