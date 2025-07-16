using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Patient;

namespace PalashDynamics.ValueObjects.IVFPlanTherapy
{
    public class clsGetAllCoupleofPatientBizActionVO : IBizActionValueObject
    {

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.IVFPlanTherapy.clsGetAllCoupleofPatientBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }



        private List<clsPatientGeneralVO> _MalePatient = new List<clsPatientGeneralVO>();
        public List<clsPatientGeneralVO> MalePatient
        {
            get
            {
                return _MalePatient;
            }
            set
            {
                _MalePatient = value;
            }
        }
        private List<clsPatientGeneralVO> _FemalePatient = new List<clsPatientGeneralVO>();
        public List<clsPatientGeneralVO> FemalePatient
        {
            get
            {
                return _FemalePatient;
            }
            set
            {
                _FemalePatient = value;
            }
        }

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
