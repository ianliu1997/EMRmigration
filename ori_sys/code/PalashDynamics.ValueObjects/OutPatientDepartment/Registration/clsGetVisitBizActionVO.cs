using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.IPD;

namespace PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster
{
    public class clsGetVisitBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetVisitBizAction";
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

        public bool GetLatestVisit { get; set; }
        public bool GetVisitCount { get; set; }
        public bool ForHO { get; set; }
        public long UnitID { get; set; }
        public long OPD_IPD_External { get; set; }

        private clsVisitVO objVisit = new clsVisitVO();
        private clsGetIPDAdmissionBizActionVO _ObjAdmission = new clsGetIPDAdmissionBizActionVO();

        public clsGetIPDAdmissionBizActionVO ObjAdmission
        {
            get { return _ObjAdmission; }
            set { _ObjAdmission = value; }
        }
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsVisitVO Details
        { 
            get { return objVisit; }
            set { objVisit = value; }
        }

    }

    public class clsGetCurrentVisitBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetCurrentVisitBizAction";
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

        public bool GetLatestVisit { get; set; }
        public bool ForHO { get; set; }
        public long UnitID { get; set; }

        private clsVisitVO objVisit = new clsVisitVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsVisitVO Details
        {
            get { return objVisit; }
            set { objVisit = value; }
        }

    }

    public class clsGetEMRVisitBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetEMRVisitBizAction";
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

        public bool GetLatestVisit { get; set; }
        public bool ForHO { get; set; }
        public long UnitID { get; set; }

        private clsVisitVO objVisit = new clsVisitVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsVisitVO Details
        {
            get { return objVisit; }
            set { objVisit = value; }
        }

    }

    public class clsGetEMRAdmissionBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetEMRAdmissionBizAction";
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

        public bool GetLatestVisit { get; set; }

        private clsVisitVO objVisit = new clsVisitVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsVisitVO Details
        {
            get { return objVisit; }
            set { objVisit = value; }
        }

    }


    public class clsGetEMRVisitDignosisiValidationVo : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetEMRVisitDignosisiValidation";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private long _VisitID;
        public long VisitID
        {
            get
            {
                return _VisitID;
            }
            set
            {
                _VisitID = value;
            }
        }

        private long _PatientID;
        public long PatientID
        {
            get
            {
                return _PatientID;
            }
            set
            {
                _PatientID = value;
            }
        }


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
    }

   
}
