using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsAddPathOrderBookingBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsAddPathOrderBookingBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion


        public bool myTransaction { get; set; }

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

        private clsPathOrderBookingVO objPathOrderBooking = new clsPathOrderBookingVO();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public clsPathOrderBookingVO PathOrderBooking
        {
            get { return objPathOrderBooking; }
            set { objPathOrderBooking = value; }
        }


        private List<clsPathOrderBookingDetailVO> objPathOrderBookingDetailList = new List<clsPathOrderBookingDetailVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBookingDetail List Which is Added.
        /// </summary>
        public List<clsPathOrderBookingDetailVO> PathOrderBookingDetailList
        {
            get { return objPathOrderBookingDetailList; }
            set { objPathOrderBookingDetailList = value; }
        }

    }

    public class FillTemplateComboBoxInPathoResultEntryBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            // throw new NotImplementedException();
            return "PalashDynamics.BusinessLayer.FillTemplateComboBoxInPathoResultEntryBizAction";
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
        public long Pathologist { get; set; }
        public long GenderID { get; set; }


        private List<MasterListItem> objList = null;
        public List<MasterListItem> MasterList
        {
            get { return objList; }
            set { objList = value; }
        }

        private int _IsFormTemplate;
        public int IsFormTemplate
        {
            get;
            set;
        }

    }

    public class clsGetPathoViewTemplateBizActionVO : IBizActionValueObject
    {
        private clsPathoTestTemplateDetailsVO objTemplateList = null;
        public clsPathoTestTemplateDetailsVO Template
        {
            get { return objTemplateList; }
            set { objTemplateList = value; }
        }


        public long TemplateID { get; set; }

        public int Flag { get; set; }

        public long GenderID { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsGetPathoViewTemplateBizAction";
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
