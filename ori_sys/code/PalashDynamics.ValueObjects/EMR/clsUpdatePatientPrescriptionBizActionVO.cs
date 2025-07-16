using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsUpdatePatientPrescriptionBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsUpdatePatientPrescriptionBizAction";
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

        public long PrescriptionID;
        public long UnitID;

        private List<clsPatientPrescriptionDetailVO> objPatientPrescriptionDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsPatientPrescriptionDetailVO> PatientPrescriptionDetail
        {
            get { return objPatientPrescriptionDetail; }
            set { objPatientPrescriptionDetail = value; }
        }


    }

    public class clsUpdateDoctorSuggestedServiceBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsUpdateDoctorSuggestedServiceBizAction";
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

        public long PrescriptionID;
        public long UnitID;

        private List<clsDoctorSuggestedServiceDetailVO> objDoctorSuggestedServiceDetail = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains PatientPrescription Summary Which is Added.
        /// </summary>
        public List<clsDoctorSuggestedServiceDetailVO> DoctorSuggestedServiceDetail
        {
            get { return objDoctorSuggestedServiceDetail; }
            set { objDoctorSuggestedServiceDetail = value; }
        }


    }
}
