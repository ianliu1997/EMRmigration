using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Radiology
{
    public class clsUpdateRadOrderBookingDetailListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Radiology.clsUpdateRadOrderBookingDetailListBizAction";
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

        public long OrderID { get; set; }
        public long UnitID { get; set; }
        public DateTime? ReportCollectionDate { get; set; }

        private List<clsRadOrderBookingDetailsVO> objOrderBookingDetail = new List<clsRadOrderBookingDetailsVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public List<clsRadOrderBookingDetailsVO> OrderBookingDetailList
        {
            get { return objOrderBookingDetail; }
            set { objOrderBookingDetail = value; }
        }


        private clsRadOrderBookingDetailsVO _Details;
        public clsRadOrderBookingDetailsVO OrderBookingDetaildetails
        {
            get { return _Details; }
            set { _Details = value; }
        }

        private List<clsRadOrderBookingDetailsVO> _DetailsList;
        public List<clsRadOrderBookingDetailsVO> OrderBookingDetaildetailsList
        {
            get { return _DetailsList; }
            set { _DetailsList = value; }
        }
    }
}
