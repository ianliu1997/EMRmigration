
//Created Date:31/July/2013
//Created By: Nilesh Raut
//Specification: BizAction VO For Geting the Patient EMR CPOE Prescription Detail

//Review By:
//Review Date:

//Modified By:
//Modified Date: 
using System;
using System.Collections.Generic;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsGetPatientCPOEPrescriptionDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientCPOEPrescriptionDetailsBizAction";
        }
        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public long PrescriptionID { get; set; }
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public long DoctorID { get; set; }
        public string LinkServer { get; set; }
        public bool Status { get; set; }

        public string Advice { get; set; }
        public DateTime? FollowupDate { get; set; }
        public string FollowUpRemark { get; set; }
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
        private List<clsPatientPrescriptionDetailVO> objPatientPrescriptionDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientPrescriptionDetailVO> PatientPrescriptionDetailList
        {
            get { return objPatientPrescriptionDetail; }
            set { objPatientPrescriptionDetail = value; }
        }
    }
}
