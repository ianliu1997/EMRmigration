using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsPathoTestResultEntryBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Pathology.clsAddPathoResultEntryBizAction";
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

        private clsPathoResultEntryVO objPathoResultEntry = new clsPathoResultEntryVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsPathoResultEntryVO PathoResultEntry
        {
            get { return objPathoResultEntry; }
            set { objPathoResultEntry = value; }
        }
    }

    public class clsGetPathoTestResultEntryBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Pathology.clsGetPathoTestResultEntry";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        
        public long ID { get; set; }
        public long PatientID { get; set; }
        public long UnitID { get; set; }
        public DateTime? Date { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }

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

        private List<clsPathoResultEntryVO> objPathoResultEntry = new List<clsPathoResultEntryVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public List<clsPathoResultEntryVO> PathoResultEntry
        {
            get { return objPathoResultEntry; }
            set { objPathoResultEntry = value; }
        }

    }

    public class clsDeletePathoTestResultEntryBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Pathology.clsDeletePathoTestResultEntry";
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

        private clsPathoResultEntryVO objPathoResultEntry = new clsPathoResultEntryVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsPathoResultEntryVO PathoResultEntry
        {
            get { return objPathoResultEntry; }
            set { objPathoResultEntry = value; }
        }
    }

    // BY BHUSHAN....

    public class clsGetResultOnParameterSelectionBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Pathology.clsGetResultOnParameterSelection";
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

        public long ParamID;
        public string Gender;
        public DateTime? DOB;

        public double resultValue;

        private List<clsPathoResultEntryVO> objPathoResultEntry = new List<clsPathoResultEntryVO>();
       
        public List<clsPathoResultEntryVO> PathoResultEntry
        {
            get { return objPathoResultEntry; }
            set { objPathoResultEntry = value; }
        }
    }


    //By Anjali...........
    public class clsGetPathoTestResultEntryDateWiseBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Pathology.clsGetPathoTestResultEntryDateWise";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        public bool fromform { get; set; }   // 1 for lab test form and 0 for IVFDashboard
        public long ID { get; set; }
        public long PatientID { get; set; }
        public long UnitID { get; set; }
        public DateTime? Date { get; set; }
        public long StartRowIndex { get; set; }
        public long MaximumRows { get; set; }
        public long TotalRows { get; set; }
        public bool PagingEnabled { get; set; }
        public string SearchExpression { get; set; }

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

        private List<clsPathoResultEntryVO> objPathoResultEntry = new List<clsPathoResultEntryVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public List<clsPathoResultEntryVO> PathoResultEntry
        {
            get { return objPathoResultEntry; }
            set { objPathoResultEntry = value; }
        }

    }
}
