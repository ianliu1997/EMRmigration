using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Pathology
{
    public class clsUpdatePathOrderBookingDetailListBizActionVO : IBizActionValueObject
    {
        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.clsUpdatePathOrderBookingDetailListBizAction";
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region Newly Added

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool IsForMachinLinking = false;

        private long _MachineID;
        public long MachineID
        {
            get { return _MachineID; }
            set
            {
                if (_MachineID != value)
                {
                    _MachineID = value;
                    OnPropertyChanged("MachineID");
                }
            }
        }

        private long _MachineUnitID;
        public long MachineUnitID
        {
            get { return _MachineUnitID; }
            set
            {
                if (_MachineUnitID != value)
                {
                    _MachineUnitID = value;
                    OnPropertyChanged("MachineUnitID");
                }
            }
        }

        private string _MachineName;
        public string MachineName
        {
            get { return _MachineName; }
            set
            {
                if (_MachineName != value)
                {
                    _MachineName = value;
                    OnPropertyChanged("MachineName");
                }
            }

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
        public bool IsExternalPatient { get; set; }
        public DateTime? SampleCollectionDate { get; set; }
        public bool IsGenerateBatch { get; set; }
        public long UID { get; set; }
        public long DID { get; set; }
        public bool IsForSampleNo { get; set; }
        public bool IsSampleGenerated { get; set; }

        private List<clsPathOrderBookingDetailVO> objOrderBookingDetail = new List<clsPathOrderBookingDetailVO>();
        /// <summary>
        /// Output Property.
        /// This Property Contains OrderBooking List Which is Added.
        /// </summary>
        public List<clsPathOrderBookingDetailVO> OrderBookingDetailList
        {
            get { return objOrderBookingDetail; }
            set { objOrderBookingDetail = value; }
        }


        private clsPathOrderBookingDetailVO _Details;       
        public clsPathOrderBookingDetailVO OrderBookingDetaildetails
        {
            get { return _Details; }
            set { _Details = value; }
        }

        //to return batch no
        private string _returnBatch;
        public string returnBatch
        {
            get { return _returnBatch; }
            set { _returnBatch = value; }
        }
    }
}
