using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Radiology
{
    public class clsAddRadOrderBookingBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddRadOrderBookingBizAction";
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

        public bool myTransaction { get; set; }

        private clsRadOrderBookingVO objDetails = null;
        public clsRadOrderBookingVO BookingDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }

    public class clsAddRadResultEntryBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsAddRadResultEntryBizAction";
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


        private bool _AutoDeductStock;
        public bool AutoDeductStock
        {
            get { return _AutoDeductStock; }
            set { _AutoDeductStock = value; }
        }

        private clsRadResultEntryVO objDetails = null;
        public clsRadResultEntryVO ResultDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private bool _IsUpload;
        public bool IsUpload
        {
            get { return _IsUpload; }
            set
            {
                _IsUpload = value;

            }
        }

        private bool _IsResultEntry;
        public bool IsResultEntry
        {
            get { return _IsResultEntry; }
            set
            {
                _IsResultEntry = value;

            }
        }

    }

    public class clsGetRadResultEntryBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsGetRadResultEntryBizAction";
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

        public long ID { get; set; }
        public long DetailID { get; set; }
        public long UnitID { get; set; }


        private clsRadResultEntryVO objDetails = null;
        public clsRadResultEntryVO ResultDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        private List<clsRadItemDetailMasterVO> _TestItemList;
        public List<clsRadItemDetailMasterVO> TestItemList
        {
            get
            {
                if (_TestItemList == null)
                    _TestItemList = new List<clsRadItemDetailMasterVO>();

                return _TestItemList;
            }

            set
            {

                _TestItemList = value;

            }
        }
    }

    public class clsUpdateReportDeliveryBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsUpdateReportDeliveryBizAction";
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

        private clsRadOrderBookingDetailsVO objDetails = null;
        public clsRadOrderBookingDetailsVO Details
        {
            get { return objDetails; }
            set { objDetails = value; }
        }

        #region For Radiology Additions

        private List<clsRadOrderBookingDetailsVO> objList = null;
        public List<clsRadOrderBookingDetailsVO> RadioTestList
        {
            get { return objList; }
            set { objList = value; }
        }

        #endregion
    }

    public class clsFillTemplateComboBoxForResultEntryBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsFillTemplateComboBoxForResultEntryBizAction";
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
        public long TestID { get; set; }

        public long Radiologist { get; set; }
        public long GenderID { get; set; }
        public long TemplateResultID { get; set; }

        private List<MasterListItem> objList = null;
        public List<MasterListItem> MasterList
        {
            get { return objList; }
            set { objList = value; }
        }


    }

    public class clsUpdateRadTechnicianEntryBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.clsUpdateRadTechnicianEntryBizAction";
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


        private bool _AutoDeductStock;
        public bool AutoDeductStock
        {
            get { return _AutoDeductStock; }
            set { _AutoDeductStock = value; }
        }
        private long _ConsumptionID;
        public long ConsumptionID
        {
            get { return _ConsumptionID; }
            set { _ConsumptionID = value; }
        }

        private bool _ItemCusmption;
        public bool ItemCusmption
        {
            get { return _ItemCusmption; }
            set { _ItemCusmption = value; }
        }



        private clsRadResultEntryVO objDetails = null;
        public clsRadResultEntryVO ResultDetails
        {
            get { return objDetails; }
            set { objDetails = value; }
        }
    }
   
}

