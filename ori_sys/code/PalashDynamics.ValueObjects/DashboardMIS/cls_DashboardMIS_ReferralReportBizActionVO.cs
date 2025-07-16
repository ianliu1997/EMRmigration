using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace PalashDynamics.ValueObjects.DashboardMIS
{
    public class cls_DashboardMIS_ReferralReportBizActionVO : IBizActionValueObject
    {
        public cls_DashboardMIS_ReferralReportBizActionVO()
        {}

        #region  IBizActionValueObject
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.DashboardMIS.clsGetReferralReportBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        #endregion

        private cls_DashboardMIS_ReferralReportVO _Details = new cls_DashboardMIS_ReferralReportVO();
        public cls_DashboardMIS_ReferralReportVO Details
        {
            get
            {
                return _Details;
            }
            set
            {
                _Details = value;
            }
        }

        private List<cls_DashboardMIS_ReferralReportListVO> _ReferralReportList = new List<cls_DashboardMIS_ReferralReportListVO>();
        public List<cls_DashboardMIS_ReferralReportListVO> ReferralReportList
        {
            get { return _ReferralReportList; }
            set { _ReferralReportList = value; }
        }
    }

    public class cls_DashboardMIS_ReferralReportVO : IValueObject, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region Properties

        private long _UnitID;
        public long UnitID
        {
            get { return _UnitID; }
            set
            {
                if (_UnitID != value)
                {
                    _UnitID = value;
                }
            }
        }


        private DateTime? _FromUpdated = DateTime.Now;
        public DateTime? FromUpdated
        {
            get { return _FromUpdated; }
            set
            {
                if (_FromUpdated != value)
                {
                    _FromUpdated = value;
                }
            }
        }

        private DateTime? _ToUpdated = DateTime.Now;
        public DateTime? ToUpdated
        {
            get { return _ToUpdated; }
            set
            {
                if (_ToUpdated != value)
                {
                    _ToUpdated = value;
                }
            }
        }

       

        #endregion
    }


    public class cls_DashboardMIS_ReferralReportListVO
    {
         #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion
        private long _SrNo;
        public long SrNo
        {
            get { return _SrNo; }
            set
            {
                if (_SrNo != value)
                {
                    _SrNo = value;
                   OnPropertyChanged("SrNo");
                }
            }
        }

        private string _UnitName;
        public string UnitName
        {
            get { return _UnitName; }
            set
            {
                if (_UnitName != value)
                {
                    _UnitName = value;  
                    OnPropertyChanged("UnitName");
                }
            }
        }
        
        private string _Date;
        public string Date
        {
            get { return _Date; }
            set
            {
                if (_Date != value)
                {
                    _Date = value;
                }
            }
        }

        private string _BDName;
        public string BDName
        {
            get { return _BDName; }
            set
            {
                if (_BDName != value)
                {
                    _BDName = value;
                }
            }
        }

        private string _DocName;
        public string DocName
        {
            get { return _DocName; }
            set
            {
                if (_DocName != value)
                {
                    _DocName = value;
                }
            }
        }

        private long _InternalDoctorRefCount;
        public long InternalDoctorRefCount
        {
            get { return _InternalDoctorRefCount; }
            set
            {
                if (_InternalDoctorRefCount != value)
                {
                    _InternalDoctorRefCount = value;

                }
            }
        }

        private long _ExternalDoctorRefCount;
        public long ExternalDoctorRefCount
        {
            get { return _ExternalDoctorRefCount; }
            set
            {
                if (_ExternalDoctorRefCount != value)
                {
                    _ExternalDoctorRefCount = value;

                }
            }
        }

        private long _RefPaitentCount;
        public long RefPaitentCount
        {
            get { return _RefPaitentCount; }
            set
            {
                if (_RefPaitentCount != value)
                {
                    _RefPaitentCount = value;

                }
            }
        }
        
    }
}
