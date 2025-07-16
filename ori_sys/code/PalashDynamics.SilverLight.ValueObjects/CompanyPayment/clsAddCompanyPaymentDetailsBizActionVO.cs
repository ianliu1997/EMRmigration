using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Master.CompanyPayment
{
  public  class clsAddCompanyPaymentDetailsBizActionVO:IBizActionValueObject,INotifyPropertyChanged
    {

      private clsCompanyPaymentDetailsVO _CompanyPaymentDetails;
        public clsCompanyPaymentDetailsVO CompanyPaymentDetails
        {
            get { return _CompanyPaymentDetails; }
            set { _CompanyPaymentDetails = value; }
        }

        private List<clsCompanyPaymentDetailsVO> _CompanyPaymentInfoList;
        public List<clsCompanyPaymentDetailsVO> CompanyPaymentInfoList
        {
            get
            {
                if (_CompanyPaymentInfoList == null)
                    _CompanyPaymentInfoList = new List<clsCompanyPaymentDetailsVO>();

                return _CompanyPaymentInfoList;
            }

            set
            {

                _CompanyPaymentInfoList = value;

            }
        }

        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private long lngUnitId;
        public long UnitID
        {
            get { return lngUnitId; }
            set
            {
                if (value != lngUnitId)
                {
                    lngUnitId = value;
                    OnPropertyChanged("UnitID");
                }
            }
        }

        private string _ServiceId;
        public string ServiceId
        {
            get { return _ServiceId; }
            set
            {
                if (value != _ServiceId)
                {
                    _ServiceId = value;
                    OnPropertyChanged("ServiceId");
                }
            }
        }

        private long _DoctorId;
        public long DoctorId
        {
            get { return _DoctorId; }
            set
            {
                if (value != _DoctorId)
                {
                    _DoctorId = value;
                    OnPropertyChanged("DoctorId");
                }
            }
        }

        private long _DepartmentId;
        public long DepartmentId
        {
            get { return _DepartmentId; }
            set
            {
                if (value != _DepartmentId)
                {
                    _DepartmentId = value;
                    OnPropertyChanged("DepartmentId");
                }
            }
        }

        private long _TariffID;
        public long TariffID
        {
            get { return _TariffID; }
            set
            {
                if (value != _TariffID)
                {
                    _TariffID = value;
                    OnPropertyChanged("TariffID");
                }
            }
        }

        private long _SpecializationID;
        public long SpecializationID
        {
            get { return _SpecializationID; }
            set
            {
                if (value != _SpecializationID)
                {
                    _SpecializationID = value;
                    OnPropertyChanged("SpecializationID");
                }
            }
        }
        public bool IsCompanyForm { get; set; }

        #region IBizActionValueObject Members

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Master.clsAddCompanyPaymentDetailsBizAction";
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
