using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Radiology
{
    public class clsGetOrderBookingListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetOrderBookingListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        private List<clsRadOrderBookingVO> objList = null;
        public List<clsRadOrderBookingVO> BookingList
        {
            get { return objList; }
            set { objList = value; }
        }


        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? DeliveryStatus { get; set; }
        public bool? CheckDeliveryStatus { get; set; }


        public bool ResultEntryStatus { get; set; }
        public bool CheckResultEntryStatus { get; set; }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public string SortExpression { get; set; }

        #region For Radiology Additions

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MRNO { get; set; }
        public long GenderID { get; set; }
        public long UnitID { get; set; }
        public string Radiologist { get; set; }//Added By Yogesh
        public string Modality { get; set; }
        public long? CategoryID { get; set; }
        public bool IsFinalizedByDr { get; set; }
        public bool IsTechnicianEntry { get; set; }
        public bool NotDone { get; set; }
        public string OPDNO { get; set; }
        public long PatientType { get; set; }  // Set for Patient Type - 1 : OPD 2 : IPD

        #endregion

    }


    public class clsGetOrderBookingDetailsListBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetOrderBookingDetailsListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        private List<clsRadOrderBookingDetailsVO> objList = null;
        public List<clsRadOrderBookingDetailsVO> BookingDetails
        {
            get { return objList; }
            set { objList = value; }
        }


        public long ID { get; set; }
        public bool ReportDelivery { get; set; }
        public bool WorkOrder { get; set; }


        public bool DeliveryStatus { get; set; }
        public bool CheckDeliveryStatus { get; set; }
        public long ModalityID { get; set; }
        public long RadiologistID { get; set; } 

        public bool ResultEntryStatus { get; set; }
        public bool CheckResultEntryStatus { get; set; }

        public bool IsFinalizedByDr { get; set; }
        public bool IsTechnicianEntry { get; set; }
        public bool NotDone { get; set; }

        #region For Radiology Additions

        //Added By Nilesh Raut For View Record in Ho
        public long UnitID { get; set; }

        #endregion

    }

    public class clsFillTestComboBoxBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsFillTestComboBoxBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        private List<MasterListItem> objList = null;
        public List<MasterListItem> MasterList
        {
            get { return objList; }
            set { objList = value; }
        }





    }


    public class clsGetOrderBookingDetailsListForWorkOrderBizActionVO : IBizActionValueObject
    {

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetOrderBookingDetailsListForWorkOrderBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        private List<clsRadOrderBookingDetailsVO> objList = null;
        public List<clsRadOrderBookingDetailsVO> BookingDetails
        {
            get { return objList; }
            set { objList = value; }
        }


        public long ID { get; set; }
        public bool ReportDelivery { get; set; }
        public bool WorkOrder { get; set; }


        public bool DeliveryStatus { get; set; }
        public bool CheckDeliveryStatus { get; set; }

        public bool IsFinalizedByDr { get; set; }
        public bool IsTechnicianEntry { get; set; }
        public bool NotDone { get; set; }

        public bool ResultEntryStatus { get; set; }
        public bool CheckResultEntryStatus { get; set; }

        public string Radiologist { get; set; }
        public string Modality { get; set; }
        public long CategoryID { get; set; }

        #region For Radiology Additions

        //Added By Nilesh Raut For View Record in Ho
        public long UnitID { get; set; }
        public long ModalityID { get; set; }
        public long RadiologistID { get; set; }
        #endregion
        

    }

    public class clsGetPACSTestListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPACSTestListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        public string MRNO { get; set; }
        public bool _IsForStudyComparision = false;
        public bool IsForStudyComparision
        {
            get { return _IsForStudyComparision; }
            set { _IsForStudyComparision = value; }
        }
        public List<clsPACSTestPropertiesVO> PACSTestList { get; set; }
    }
    public class clsGetPACSTestSeriesListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPACSTestSeriesListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        public string MRNO { get; set; }
        public bool _IsForStudyComparision = false;
        public bool IsForStudyComparision
        {
            get { return _IsForStudyComparision; }
            set { _IsForStudyComparision = value; }
        }

        private bool _IsForShowPACS = false;
        public bool IsForShowPACS
        {
            get { return _IsForShowPACS; }
            set { _IsForShowPACS = value; }
        }
        private bool _IsForShowSeriesPACS = false;
        public bool IsForShowSeriesPACS
        {
            get { return _IsForShowSeriesPACS; }
            set { _IsForShowSeriesPACS = value; }
        }

        public List<clsPACSTestSeriesVO> PACSTestSeriesList { get; set; }
        public clsPACSTestSeriesVO PACSTestDetails { get; set; }
        public List<clsPACSTestSeriesVO> PACSTestSeriesImageList { get; set; }

    }
    public class clsPACSTestPropertiesVO
    {
        public string MRNO { get; set; }
        public string MODALITY { get; set; }
        public string IMAGECOUNT { get; set; }
        public string STUDYDESC { get; set; }
        public string STUDYDATE { get; set; }
        public string STUDYTIME { get; set; }
        public string PHYSICIANNAME { get; set; }
        public string PATIENTNAME { get; set; }
        public string PATIENTID { get; set; }
    }
    public class clsPACSTestSeriesVO
    {
        public string MRNO { get; set; }
        public string MODALITY { get; set; }
        public string IMAGECOUNT { get; set; }
        public string IMAGEPATH { get; set; }
        public string SERIESNUMBER { get; set; }
        public string STUDYDESC1 { get; set; }
        public string SERIESDESC { get; set; }
        public string STUDYDESC { get; set; }
        public string STUDYDATE { get; set; }
        public string STUDYTIME { get; set; }
        public string PHYSICIANNAME { get; set; }
        public string PATIENTNAME { get; set; }
        public string PATIENTID { get; set; }
        public bool IsSelected { get; set; }

        public List<clsPACSTestSeriesVO> PacsTestSeriesList { get; set; }
    }

    public class clsGetRadTechnicianEntryListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetRadTechnicianEntryListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        private List<clsRadOrderBookingVO> objList = null;
        public List<clsRadOrderBookingVO> BookingList
        {
            get { return objList; }
            set { objList = value; }
        }


        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public long RadOrderID { get; set; }
        public long OrderDetailID { get; set; }

        public long BillID { get; set; }
        public long ChargeID { get; set; }
        public long ServiceID { get; set; }


    }

   
}
