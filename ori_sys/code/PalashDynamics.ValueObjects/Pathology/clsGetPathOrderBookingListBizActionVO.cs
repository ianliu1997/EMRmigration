using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsGetPathOrderBookingListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathOrderBookingListBizAction";
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
        public long UnitID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool CheckSampleType { get; set; }
        public bool SampleType { get; set; }
        public bool CheckUploadStatus { get; set; }
        public bool IsUploaded { get; set; }
        public bool CheckDeliveryStatus { get; set; }
        public bool IsDelivered { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MRNO { get; set; }
        public string BillNo { get; set; }
        public int Billtype { get; set; }
        public bool IsDispatchedClinic { get; set; }
       
        //added by rohini dated 25.2.16 for cross clilnic handle dispatch to clinic

        public bool IsDispatchToClinic { get; set; }
        #region Newly added Properties
        public long CatagoryID { get; set; }     
        public string SampleNo { get; set; }
        public long AuthenticationLevel { get; set; }
        public bool IsForNewDispatch { get; set; }
        public long UserID { get; set; }

    
        #endregion

        public long PatientType { get; set; }  // Set for Patient Type - 1 : OPD 2 : IPD

        
        //check from which form its called
        public string IsFrom { get; set; }
        public long StatusID { get; set; }      //for pending 1 patial 2 completed 3 authorized 4
        public long AgencyID { get; set; }  // Set for Patient Type - 1 : OPD 2 : IPD
        public long TypeID { get; set; }  //by rohini for delivery type
        public bool IsSubOptimal { get; set; }   

        #region Paging
        public int TotalRows { get; set; }
     
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        #endregion

        private List<clsPathOrderBookingVO> objOrderBooking = new List<clsPathOrderBookingVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public List<clsPathOrderBookingVO> OrderBookingList
        {
            get { return objOrderBooking; }
            set { objOrderBooking = value; }
        }

        //added by rohini
        private List<clsPathOrderBookingDetailVO> OrderBookingListDetail = new List<clsPathOrderBookingDetailVO>();
        public List<clsPathOrderBookingDetailVO> OrderBookingDetailList
        {
            get { return OrderBookingListDetail; }
            set { OrderBookingListDetail = value; }
        }
        //
    }


    public class clsGetServerDateTimeBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetServerDateTimeBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;
        
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        public DateTime ServerDateTime { get; set; }
        private List<clsPathOrderBookingVO> objOrderBooking = new List<clsPathOrderBookingVO>();
        public List<clsPathOrderBookingVO> OrderBookingList
        {
            get { return objOrderBooking; }
            set { objOrderBooking = value; }
        }
        //
    }

    public class clsGetBatchCodeBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetBatchCodeBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private int _SuccessStatus;

        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long ID { get; set; }
        public string SampleNo { get; set;}
        public long UnitID { get; set; }
        public string BatchCode { get; set; }
        public long OrderID { get; set; }
        public long DispatchTo { get; set; }
        public bool IsFromAccept { get; set; }
        #region Paging
        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        #endregion
        private List<clsPathOrderBookingDetailVO> objOrderBooking = new List<clsPathOrderBookingDetailVO>();
        public List<clsPathOrderBookingDetailVO> OrderBookingList
        {
            get { return objOrderBooking; }
            set { objOrderBooking = value; }
        }
        //
    }

}
