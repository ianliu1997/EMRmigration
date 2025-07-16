using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Radiology
{
    class clsGetAddUpdateRadiologyNewBizActionVO
    {
    }

    public class clsUpdateRadOrderBookingDetailDeliveryStatusBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsUpdateRadOrderBookingDetailDeliveryStatusBizAction";
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

        private clsRadOrderBookingDetailsVO objPathOrderBookingDetail = new clsRadOrderBookingDetailsVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsRadOrderBookingDetailsVO PathOrderBookingDetail
        {
            get { return objPathOrderBookingDetail; }
            set { objPathOrderBookingDetail = value; }
        }

        private List<clsRadOrderBookingDetailsVO> objPathOrder = new List<clsRadOrderBookingDetailsVO>();
        public List<clsRadOrderBookingDetailsVO> PathOrderBookList
        {
            get { return objPathOrder; }
            set { objPathOrder = value; }
        }
    }

    public class clsGetRadiologistBySpecializationBizActionVO : IBizActionValueObject
    {


        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public long UnitID { get; set; }
        public long SpecializationID { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetRadiologistBySpecializationBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }

    public class clsGetRadiologistGenderByIDBizActionVO : IBizActionValueObject
    {


        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }

        public long UnitID { get; set; }
        public long DoctorID { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetRadiologistGenderByIDBizAction";
        }

        #endregion


        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
    }


    public class clsRadPathPatientReportDetailsForEmailSendingBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsRadPathPatientReportDetailsForEmailSendingBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        private bool _AutoDeductStock;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public bool AutoDeductStock
        {
            get { return _AutoDeductStock; }
            set { _AutoDeductStock = value; }
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

        private clsRadOrderBookingVO _RadOrderList = new clsRadOrderBookingVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsRadOrderBookingVO RadOrderList
        {
            get { return _RadOrderList; }
            set { _RadOrderList = value; }
        }


        private List<clsRadOrderBookingDetailsVO> _RadOrderBookingDetailList = new List<clsRadOrderBookingDetailsVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public List<clsRadOrderBookingDetailsVO> RadOrderBookingDetailList
        {
            get { return _RadOrderBookingDetailList; }
            set { _RadOrderBookingDetailList = value; }
        }

        public long AttachmentID { get; set; }
        public long UnitID { get; set; }
        public long RadOrderID { get; set; }
        public long RadOrderBookingDetailID { get; set; }
        public long RadPatientReportID { get; set; }
        public string PatientEmailID { get; set; }
        public string DoctorEmailID { get; set; }
        public bool Status { get; set; }
        public byte[] Report { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public string PatientName { get; set; }
        public DateTime? VisitDate { get; set; }
        public string SourceURL { get; set; }

        private List<clsRadOrderBookingDetailsVO> objPathOrder = new List<clsRadOrderBookingDetailsVO>();
        public List<clsRadOrderBookingDetailsVO> PathOrderBookList
        {
            get { return objPathOrder; }
            set { objPathOrder = value; }
        }
    }


    public class clsGetRadiologistResultEntryBizActionVO : IBizActionValueObject
    {


        private List<MasterListItem> _MasterList = null;
        public List<MasterListItem> MasterList
        {
            get
            { return _MasterList; }

            set
            { _MasterList = value; }
        }


        public long UnitId { get; set; }

        public long ID { get; set; }
        public long TestIDNew { get; set; }
        public long DepartmentId { get; set; }

        public clsRadiologyVO ItemSupplier { get; set; }
        public List<clsRadiologyVO> ItemSupplierList { get; set; }
        public List<clsRadiologyVO> ItemList { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetRadiologistResultEntryBizAction";
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
