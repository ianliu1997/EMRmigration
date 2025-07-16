using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.EMR
{
    public class clsGetPatientEMRDataBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientEMRDataBizAction";
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

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {

                _PatientID = value;
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {

                _PatientUnitID = value;
            }
        }

        private long _TemplateID;
        public long TemplateID
        {
            get { return _TemplateID; }
            set
            {

                _TemplateID = value;


            }
        }

        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {

                _VisitID = value;


            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {

                _UnitID = value;


            }
        }

        public bool IsPrevious { get; set; }
        public bool IsHistory { get; set; }
        //public bool IsIVF { get; set; }
        private clsPatientEMRDataVO PatientEMRData;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsPatientEMRDataVO objPatientEMRData
        {
            get { return PatientEMRData; }
            set { PatientEMRData = value; }
        }
    }

    public class clsGetPatientFeedbackBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientFeedbackBizAction";
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

        private long _PatientID;
        public long PatientID
        {
            get { return _PatientID; }
            set
            {

                _PatientID = value;
            }
        }

        private long _PatientUnitID;
        public long PatientUnitID
        {
            get { return _PatientUnitID; }
            set
            {

                _PatientUnitID = value;
            }
        }

        
        private long _VisitID;
        public long VisitID
        {
            get { return _VisitID; }
            set
            {

                _VisitID = value;


            }
        }

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {

                _UnitID = value;


            }
        }        

        private clsPatientFeedbackVO PatientFeedback;
        /// <summary>
        /// Output Property.
        /// This Property Contains Patient Feedback Details Which is Added.
        /// </summary>
        public clsPatientFeedbackVO objPatientFeedback
        {
            get { return PatientFeedback; }
            set { PatientFeedback = value; }
        }
    }

    public class clsGetPatientEMRSummaryDataBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPatientEMRSummaryDataBizAction";
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
        public long ID { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public long UnitID { get; set; }
        public long PatientEMR { get; set; }//EMR = 1 for IVF Related EMR
        public long TemplateID { get; set; }//Send TemplateId for EMR Related to IVF

        public bool IsIVF { get; set; }
        private List<clsPatientEMRDataVO> objVisit = new List<clsPatientEMRDataVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public List<clsPatientEMRDataVO> SummaryList
        {
            get { return objVisit; }
            set { objVisit = value; }
        }
    }


}
