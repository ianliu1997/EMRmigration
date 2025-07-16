using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Patient
{
    public class clsUpdatePatientForIssueBizActionVO : IBizActionValueObject
    {
        private clsPatientVO _PatientDetails;
        public clsPatientVO PatientDetails
        {
            get { return _PatientDetails; }
            set { _PatientDetails = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsUpdatePatientForIssueBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsDeActiveIssuePatientBizActionVO : IBizActionValueObject
    {
        private clsPatientVO _PatientDetails;
        public clsPatientVO PatientDetails
        {
            get { return _PatientDetails; }
            set { _PatientDetails = value; }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsDeActiveIssuePatientBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }
}

