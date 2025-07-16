using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.ValueObjects.EMR.NewEMR
{
    public class clsGetPatientNewEMRDiaDetailsBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetPatientNewEMRDiaDetailsBizAction";
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

        public long VisitID { get; set; }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public long UnitID { get; set; }

        private List<clsEMRAddDiagnosisVO> objPatientDiagnosis = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsEMRAddDiagnosisVO> PatientDiagnosisDetails
        {
            get { return objPatientDiagnosis; }
            set { objPatientDiagnosis = value; }
        }

    }
}
