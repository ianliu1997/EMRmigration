using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsPathoUploadReportBizActionVO:IBizActionValueObject
    { 
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsPathoUploadReportBizAction";
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
        private clsPathPatientReportVO objPathPatientReportDetail = new clsPathPatientReportVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsPathPatientReportVO UploadReportDetails
        {
            get { return objPathPatientReportDetail; }
            set { objPathPatientReportDetail = value; }
        }

        public bool IsResultEntry { get; set; }
    }
}
