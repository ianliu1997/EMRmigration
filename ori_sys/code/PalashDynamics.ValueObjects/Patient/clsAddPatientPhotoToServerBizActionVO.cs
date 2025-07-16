using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Patient  //added by neena
{
    
    public class clsAddPatientPhotoToServerBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddPatientPhotoToServerBizAction";
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

        private clsPatientVO objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientVO PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }

        private List<clsPatientVO> _PatientDetailsList = null;

        public List<clsPatientVO> PatientDetailsList
        {
            get { return _PatientDetailsList; }
            set { _PatientDetailsList = value; }
        }

    }

    public class clsMovePatientPhotoToServerBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsMovePatientPhotoToServerBizAction";
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

        private clsPatientVO objPatient = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientVO PatientDetails
        {
            get { return objPatient; }
            set { objPatient = value; }
        }

        private List<clsPatientVO> _PatientDetailsList = null;

        public List<clsPatientVO> PatientDetailsList
        {
            get { return _PatientDetailsList; }
            set { _PatientDetailsList = value; }
        }

    }
}
