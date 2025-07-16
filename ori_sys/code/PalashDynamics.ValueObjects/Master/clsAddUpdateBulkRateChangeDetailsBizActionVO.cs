using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Billing;

namespace PalashDynamics.ValueObjects.Master
{
    public class clsAddUpdateBulkRateChangeDetailsBizActionVO : IBizActionValueObject, INotifyPropertyChanged
    {
        private clsTariffMasterBizActionVO _BulkRateChangeDetails;
        public clsTariffMasterBizActionVO BulkRateChangeDetailsVO
        {
            get { return _BulkRateChangeDetails; }
            set { _BulkRateChangeDetails = value; }
        }

        private List<clsTariffMasterBizActionVO> _TariffDetailsList;
        public List<clsTariffMasterBizActionVO> TariffDetailsList
        {
            get
            {
                if (_TariffDetailsList == null)
                    _TariffDetailsList = new List<clsTariffMasterBizActionVO>();

                return _TariffDetailsList;
            }

            set
            {
                _TariffDetailsList = value;

            }
        }

        private List<clsSubSpecializationVO> _SubSpecializationList;
        public List<clsSubSpecializationVO> SubSpecializationList
        {
            get
            {
                if (_SubSpecializationList == null)
                    _SubSpecializationList = new List<clsSubSpecializationVO>();

                return _SubSpecializationList;
            }

            set
            {
                _SubSpecializationList = value;

            }
        }

        public bool IsModify { get; set; }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsAddUpdateBulkRateChangeDetailsBizAction";
        }
        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }

    public class clsGetBulkRateChangeDetailsListBizActionVO : IBizActionValueObject, INotifyPropertyChanged
    {
        private clsTariffMasterBizActionVO _BulkRateChangeDetails;
        public clsTariffMasterBizActionVO BulkRateChangeDetailsVO
        {
            get { return _BulkRateChangeDetails; }
            set { _BulkRateChangeDetails = value; }
        }

        private List<clsTariffMasterBizActionVO> _TariffDetailsList;
        public List<clsTariffMasterBizActionVO> TariffDetailsList
        {
            get
            {
                if (_TariffDetailsList == null)
                    _TariffDetailsList = new List<clsTariffMasterBizActionVO>();

                return _TariffDetailsList;
            }

            set
            {
                _TariffDetailsList = value;

            }
        }

        private List<clsSubSpecializationVO> _SubSpecializationList;
        public List<clsSubSpecializationVO> SubSpecializationList
        {
            get
            {
                if (_SubSpecializationList == null)
                    _SubSpecializationList = new List<clsSubSpecializationVO>();

                return _SubSpecializationList;
            }

            set
            {
                _SubSpecializationList = value;

            }
        }

        private string _TariffList;
        public string TariffList
        {
            get { return _TariffList; }
            set
            {
                _TariffList = value;
                OnPropertyChanged("TariffList");
            }
        }

        public bool IsFromViewClick { get; set; }

        public int TotalRows { get; set; }
        public long StartRowIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool PagingEnabled { get; set; }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        #region IBizActionValueObject Members
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsGetBulkRateChangeDetailsListBizAction";
        }
        #endregion

        #region IValueObject Members

        public string ToXml()
        {
            return this.ToString();
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("PropertyName"));
        }

        #endregion
    }


}
