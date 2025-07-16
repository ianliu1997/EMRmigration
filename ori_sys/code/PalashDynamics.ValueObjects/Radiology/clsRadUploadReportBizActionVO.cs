
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Radiology
{
    public class clsRadUploadReportBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsRadUploadReportBizAction";
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

        public long UnitID { get; set; }

        private clsRadPatientReportVO objRadPatientReportDetail = new clsRadPatientReportVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsRadPatientReportVO UploadReportDetails
        {
            get { return objRadPatientReportDetail; }
            set { objRadPatientReportDetail = value; }
        }

        public bool IsResultEntry { get; set; }
    }

    public class clsGetRadUploadReportBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetRadUploadReportBizAction";
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

        public long UnitID { get; set; }

        private clsRadPatientReportVO objRadPatientReportDetail = new clsRadPatientReportVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsRadPatientReportVO UploadReportDetails
        {
            get { return objRadPatientReportDetail; }
            set { objRadPatientReportDetail = value; }
        }

        public bool IsResultEntry { get; set; }
    }
}
