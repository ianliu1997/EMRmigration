using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsUpdatePathOrderBookingDetailBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsUpdatePathOrderBookingDetailBizAction";
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

        private clsPathOrderBookingDetailVO objPathOrderBookingDetail = new clsPathOrderBookingDetailVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsPathOrderBookingDetailVO PathOrderBookingDetail
        {
            get { return objPathOrderBookingDetail; }
            set { objPathOrderBookingDetail = value; }
        }


        #region For Pathology Additions

        private List<clsPathOrderBookingDetailVO> objPathOrder = new List<clsPathOrderBookingDetailVO>();
        public List<clsPathOrderBookingDetailVO> PathOrderBookList
        {
            get { return objPathOrder; }
            set { objPathOrder = value; }
        }

        #endregion

    }

    #region For Pathology Additions

    public class clsUpdatePathOrderBookingDetailDeliveryStatusBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsUpdatePathOrderBookingDetailDeliveryStatusBizAction";
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

        private clsPathOrderBookingDetailVO objPathOrderBookingDetail = new clsPathOrderBookingDetailVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsPathOrderBookingDetailVO PathOrderBookingDetail
        {
            get { return objPathOrderBookingDetail; }
            set { objPathOrderBookingDetail = value; }
        }

        private List<clsPathOrderBookingDetailVO> objPathOrder = new List<clsPathOrderBookingDetailVO>();
        public List<clsPathOrderBookingDetailVO> PathOrderBookList
        {
            get { return objPathOrder; }
            set { objPathOrder = value; }
        }
    }


    #endregion

}
