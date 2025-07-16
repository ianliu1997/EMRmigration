using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PalashDynamics.ValueObjects.Billing
{
    public class clsVisitConsultationChargesVO : IValueObject, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

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

        private clsChargeVO _ChargesDetails = new clsChargeVO();
        public clsChargeVO ChargesDetails
        {
            get { return _ChargesDetails; }
            set
            {
                if (_ChargesDetails != value)
                {
                    _ChargesDetails = value;
                    OnPropertyChanged("ChargesDetails");
                }
            }
        }

        private List<clsChargeVO> _SubChargesList = null;
        public List<clsChargeVO> SubChargesList
        {
            get { return _SubChargesList; }
            set
            {
                _SubChargesList = value;

            }
        }
        private clsBillVO _BillDetails = new clsBillVO();
        public clsBillVO BillDetails
        {
            get { return _BillDetails; }
            set
            {
                if (_BillDetails != value)
                {
                    _BillDetails = value;
                    OnPropertyChanged("BillDetails");
                }
            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                }
            }
        }

        private bool _IsPackageSelected;
        public bool IsPackageSelected
        {
            get { return _IsPackageSelected; }
            set
            {
                if (_IsPackageSelected != value)
                {
                    _IsPackageSelected = value;
                }
            }
        }

        private long _RowID;
        public long RowID
        {
            get { return _RowID; }
            set
            {
                if (_RowID != value)
                {
                    _RowID = value;
                }
            }
        }

        //Added By Arati
        private string _PackageName;
        public string PackageName
        {
            get { return _PackageName; }
            set
            {
                if (value != _PackageName)
                {
                    _PackageName = value;
                    OnPropertyChanged("PackageName");
                }
            }
        }

        private bool _IsEnabled = true;
        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                if (_IsEnabled != value)
                {
                    _IsEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }

        private bool _IsTaxAmountTextBoxEnabled;
        public bool IsTaxAmountTextBoxEnabled
        {
            get { return _IsTaxAmountTextBoxEnabled; }
            set
            {
                if (_IsTaxAmountTextBoxEnabled != value)
                {
                    _IsTaxAmountTextBoxEnabled = value;
                    OnPropertyChanged("IsTaxAmountTextBoxEnabled");
                }
            }
        }




    }
}
