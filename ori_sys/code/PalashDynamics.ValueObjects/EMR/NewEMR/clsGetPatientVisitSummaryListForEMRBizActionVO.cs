//Created Date:26/July/2013
//Created By: Nilesh Raut
//Specification: BizAction For Show the Patient Visit Fillup EMR Details

//Review By:
//Review Date:

//Modified By:
//Modified Date: 

using System;
using System.Collections.Generic;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsGetPatientVisitSummaryListForEMRBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.EMR.clsGetPatientVisitSummaryListForEMRBizAction";
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
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }

        private List<clsVisitEMRDetails> objVisit = new List<clsVisitEMRDetails>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsVisitEMRDetails> VisitEMRDetailsList
        {
            get { return objVisit; }
            set { objVisit = value; }
        }
        public List<clsVisitEMRDetails> PatientVisitEMRDetailsIPD
        {
            get { return objVisit; }
            set { objVisit = value; }
        }

    }

    public class clsVisitEMRDetails
    {
        public long PatientId { get; set; }
        public long VisitId { get; set; }
        public long PatientUnitId { get; set; }
        public string OPDNO { get; set; }
        public DateTime VisitDate { get; set; }
        public string VisitCenter { get; set; }
        public string VisitType { get; set; }
        public long VisitTypeID { get; set; }
        public long UnitId { get; set; }
        public string Unit { get; set; }
        public long DepartmentID { get; set; }
        public string DoctorCode { get; set; }
        public long DoctorID { get; set; }
        public string Department { get; set; }
        public string Doctor { get; set; }
        public string Cabin { get; set; }
        public long PrescriptionID { get; set; }
        public string Specialization { get; set; }
        //public string DisplayDate { get; set; }

    }
}
