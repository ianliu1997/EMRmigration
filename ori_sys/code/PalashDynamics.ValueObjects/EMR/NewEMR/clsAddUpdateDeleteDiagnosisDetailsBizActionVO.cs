//Credited by Rajshree on 22nd july 2013
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.EMR ;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.EMR.NewEMR
{
    public class clsAddUpdateDeleteDiagnosisDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddUpdateDeleteDiagnosisDetailsBizAction";
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
        public bool IsUpdate { get; set; }
        public long VisitID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long PatientDiagnosisID { get; set; }
        public Boolean IsOPDIPD { get; set; }
        public string DoctorCode { get; set; }
        public long DoctorID { get; set; }
        
        private List<clsEMRAddDiagnosisVO> _objDiagnosisDetails = null;
        public List<clsEMRAddDiagnosisVO> DiagnosisDetails
        {
            get { return _objDiagnosisDetails; }
            set { _objDiagnosisDetails = value; }
        }
        public Boolean IsICD10 { get; set; }


        private List<clsPatientGeneralVO> _objSurrogatedPatient = null;
        public List<clsPatientGeneralVO> objSurrogatedPatient
        {
            get { return _objSurrogatedPatient; }
            set { _objSurrogatedPatient = value; }
        }
    }
}
